using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;
using AjaxControlToolkit;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class SuaThoiKhoaBieuPage : Page
    {
        #region Fields, Properties
        public int MaLopHoc
        {
            get
            {
                int maLopHoc;
                if (Request.QueryString["lop"] != null)
                {
                    maLopHoc = Int32.Parse(Request.QueryString["lop"]);
                    ViewState["MaLopHoc"] = maLopHoc;
                }
                else
                {
                    if (ViewState["MaLopHoc"] != null)
                    {
                        maLopHoc = (int)ViewState["MaLopHoc"];
                    }
                    else
                    {                        
                        maLopHoc = (int)Session[User.Identity.Name + "_Fr_ThemTiet_To_SuaTKB_MaLop"];
                        Session.Remove(User.Identity.Name + "_Fr_ThemTiet_To_SuaTKB_MaLop");                        
                    }
                }
                return maLopHoc;
            }
        }
        public int MaHocKy
        {
            get
            {
                if (Request.QueryString["hocky"] != null)
                {
                    int maHocKy = Int32.Parse(Request.QueryString["hocky"]);
                    ViewState["MaHocKy"] = maHocKy;
                    return maHocKy;
                }
                else
                {
                    if (ViewState["MaHocKy"] != null)
                    {
                        return (int)ViewState["MaHocKy"];
                    }
                    else
                    {
                        int maHocKy = (int)Session[User.Identity.Name + "_Fr_ThemTiet_To_SuaTKB_MaHocKy"];
                        Session.Remove(User.Identity.Name + "_Fr_ThemTiet_To_SuaTKB_HocKy");
                        return maHocKy;
                    }
                }
            }
        }
        public int OriginalMaThu
        {
            get
            {
                if (Request.QueryString["thu"] != null)
                {
                    int maThu = Int32.Parse(Request.QueryString["thu"]);
                    ViewState["MaThu"] = maThu;
                    return maThu;
                }
                else
                {
                    if(ViewState["MaThu"] != null)
                    {                        
                        return (int)ViewState["MaThu"];
                    }
                    else
                    {
                        int maThu = (int)Session[User.Identity.Name + "_Fr_ThemTiet_To_SuaTKB_MaThu"];
                        Session.Remove(User.Identity.Name + "_Fr_ThemTiet_To_SuaTKB_MaThu");
                        return maThu;
                        
                    }
                }
            }
        }
        private ThoiKhoaBieuBL thoiKhoaBieuBL;
        #endregion

        #region Page event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            // Init variables
            RoleBL roleBL = new RoleBL();
            UserBL userBL = new UserBL();
            thoiKhoaBieuBL = new ThoiKhoaBieuBL();

            string pageUrl = Page.Request.Path;
            Guid role = userBL.GetRoleId(User.Identity.Name);

            // Check role's accesibility
            if (!roleBL.ValidateAuthorization(role, pageUrl))
            {
                Response.Redirect((string)GetGlobalResourceObject("MainResource", "AccessDeniedPageUrl"));
                return;
            }

            // Set Master page's properties
            Site masterPage = (Site)Page.Master;
            masterPage.UserRole = role;
            masterPage.PageUrl = pageUrl;

            if (!Page.IsPostBack)
            {
                FillDDLThu();
                FillTitlePage();

                BindRptThoiKhoaBieu();
            }
        }
        #endregion

        #region Methods
        private void FillTitlePage()
        {
            LopHocInfo lopHoc = (new LopHocBL()).GetLopHocInfo(MaLopHoc);
            CauHinh_HocKy hocKy = (new HocKyBL()).GetHocKy(MaHocKy);

            LblTitle.Text = string.Format("THỜI KHÓA BIỂU LỚP {0} ({1} - {2} - NĂM HỌC {3})",
                lopHoc.TenLopHoc, DdlThu.SelectedItem.Text, hocKy.TenHocKy, lopHoc.TenNamHoc);
        }

        public void FillDDLThu()
        {
            SystemConfigBL cauHinhBL = new SystemConfigBL();
            List<CauHinh_Thu> listThu = cauHinhBL.GetListThu();
            DdlThu.DataSource = listThu;
            DdlThu.DataValueField = "MaThu";
            DdlThu.DataTextField = "TenThu";
            DdlThu.DataBind();

            //DdlThu.SelectedValue = OriginalMaThu.ToString();
        }

        private void BindRptThoiKhoaBieu()
        {
            int maThu = Int32.Parse(DdlThu.SelectedValue);

            List<ThoiKhoaBieuTheoTiet> lTKBTheoTiets;
            lTKBTheoTiets = thoiKhoaBieuBL.GetThoiKhoaBieuTheoThu(MaLopHoc, MaHocKy, maThu);
            MainDataPager.ItemCount = lTKBTheoTiets.Count;

            RptThoiKhoaBieu.DataSource = lTKBTheoTiets;
            RptThoiKhoaBieu.DataBind();
        }
        #endregion

        #region Button event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            FillTitlePage();
            MainDataPager.CurrentIndex = 1;
            BindRptThoiKhoaBieu();
        }

        protected void BtnBackPrevPage_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/modules/lop_hoc/thoikhoabieu.aspx");
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            int maMonHocTKB = Int32.Parse(this.HdfMaMonHocTKB.Value);
            thoiKhoaBieuBL.Delete(maMonHocTKB);
            BindRptThoiKhoaBieu();
        }
        #endregion

        #region Repeater event handlers
        protected void RptThoiKhoaBieu_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item
                || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.DataItem != null)
                {
                    ThoiKhoaBieuTheoTiet tkbTheoTiet = (ThoiKhoaBieuTheoTiet)e.Item.DataItem;
                    if (tkbTheoTiet.MaMonHoc == 0)
                    {
                        ImageButton btnDeleteItem = (ImageButton)e.Item.FindControl("BtnDeleteItem");
                        btnDeleteItem.ImageUrl = "~/Styles/Icons/icon_delete_disabled.png";
                        btnDeleteItem.Enabled = false;

                        ImageButton btnEditItem = (ImageButton)e.Item.FindControl("BtnEditItem");
                        btnEditItem.ImageUrl = "~/Styles/Icons/icon_Edit_disabled.png";
                        btnEditItem.Enabled = false;
                    }
                    else
                    {
                        ImageButton btnAddItem = (ImageButton)e.Item.FindControl("BtnAddItem");
                        btnAddItem.ImageUrl = "~/Styles/Icons/icon_Add_disabled.png";
                        btnAddItem.Enabled = false;
                    }
                }
            }
        }

        protected void RptThoiKhoaBieu_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdAddItem":
                    {
                        int maTiet = Int32.Parse(e.CommandArgument.ToString());
                        ViewState["MaTiet_Add"] = maTiet;
                        TietBL tietBL = new TietBL();
                        LopHocInfo lopHoc = (new LopHocBL()).GetLopHocInfo(MaLopHoc);
                        CauHinh_HocKy hocKy = (new HocKyBL()).GetHocKy(MaHocKy);

                        Response.Redirect(string.Format("themtietthoikhoabieu.aspx?lop={0}&hocky={1}&thu={2}&tiet={3}",
                            MaLopHoc, MaHocKy, DdlThu.SelectedValue, maTiet));
                        break;
                    }
                case "CmdDeleteItem":
                    {
                        // Set confirm text and show dialog
                        this.LblConfirmDelete.Text = "Bạn có chắc xóa thông tin này không?";

                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        // Save current MaLopHoc to global
                        HiddenField hdfMaMonHocTKB = (HiddenField)e.Item.FindControl("HdfMaMonHocTKB");

                        this.HdfMaMonHocTKB.Value = hdfMaMonHocTKB.Value;

                        // Save modal popup ClientID
                        this.HdfRptThoiKhoaBieuMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        int maMonHocTKB = Int32.Parse(e.CommandArgument.ToString());
                        ThoiKhoaBieuTheoTiet tKBTheoTiet = thoiKhoaBieuBL.GetThoiKhoaBieuTheoTiet(
                            maMonHocTKB);
                        int maTiet = tKBTheoTiet.Tiet;
                        int maMonHoc = tKBTheoTiet.MaMonHoc;
                        ViewState["MaMonHocTKB_Edit"] = maMonHocTKB;
                        ViewState["MaMonHoc_Edit"] = maMonHoc;                     

                        Response.Redirect(string.Format("suatietthoikhoabieu.aspx?id={0}&lop={1}&hocky={2}&thu={3}&tiet={4}",
                            maMonHocTKB, MaLopHoc, MaHocKy, DdlThu.SelectedValue, maTiet));
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        #endregion        
    }
}