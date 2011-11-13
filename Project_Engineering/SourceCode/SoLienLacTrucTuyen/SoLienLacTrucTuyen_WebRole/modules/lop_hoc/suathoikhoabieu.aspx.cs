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
    public partial class SuaThoiKhoaBieuPage : BaseContentPage
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
                    if (ViewState["MaThu"] != null)
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
        private ScheduleBL scheduleBL;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (isAccessDenied)
            {
                return;
            }

            // Init variables            
            scheduleBL = new ScheduleBL();

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
            SystemConfigBL systemConfigBL = new SystemConfigBL();
            ClassBL classBL = new ClassBL();
            LopHoc_Lop Class = new LopHoc_Lop();

            Class.MaLopHoc = MaLopHoc;

            TabularClass lopHoc = classBL.GetTabularClass(Class);
            CauHinh_HocKy hocKy = systemConfigBL.GetTerm(MaHocKy);

            LblTitle.Text = string.Format("THỜI KHÓA BIỂU LỚP {0} ({1} - {2} - NĂM HỌC {3})",
                lopHoc.TenLopHoc, DdlThu.SelectedItem.Text, hocKy.TenHocKy, lopHoc.TenNamHoc);
        }

        public void FillDDLThu()
        {
            SystemConfigBL cauHinhBL = new SystemConfigBL();
            List<CauHinh_Thu> listThu = cauHinhBL.GetDayInWeeks();
            DdlThu.DataSource = listThu;
            DdlThu.DataValueField = "MaThu";
            DdlThu.DataTextField = "TenThu";
            DdlThu.DataBind();

            //DdlThu.SelectedValue = OriginalMaThu.ToString();
        }

        private void BindRptThoiKhoaBieu()
        {
            LopHoc_Lop Class = new LopHoc_Lop();
            Class.MaLopHoc = MaLopHoc;
            CauHinh_HocKy term = new CauHinh_HocKy();
            term.MaHocKy = MaHocKy;
            CauHinh_Thu dayInWeek = new CauHinh_Thu();
            dayInWeek.MaThu = Int32.Parse(DdlThu.SelectedValue);

            List<TeachingPeriodSchedule> lTKBTheoTiets = scheduleBL.GetTeachingPeriodSchedules(Class, term, dayInWeek);
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
            LopHoc_MonHocTKB schedule = new LopHoc_MonHocTKB();
            schedule.MaMonHocTKB = Int32.Parse(this.HdfMaMonHocTKB.Value);
            scheduleBL.DeleteSchedule(schedule);
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
                    TeachingPeriodSchedule tkbTheoTiet = (TeachingPeriodSchedule)e.Item.DataItem;
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
                        SystemConfigBL systemConfigBL = new SystemConfigBL();
                        TeachingPeriodBL tietBL = new TeachingPeriodBL();
                        ClassBL classBL = new ClassBL();
                        LopHoc_Lop Class = new LopHoc_Lop();
                        Class.MaLopHoc = MaLopHoc;

                        int maTiet = Int32.Parse(e.CommandArgument.ToString());
                        ViewState["MaTiet_Add"] = maTiet;

                        TabularClass lopHoc = classBL.GetTabularClass(Class);
                        CauHinh_HocKy hocKy = systemConfigBL.GetTerm(MaHocKy);

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
                        LopHoc_MonHocTKB schedule = scheduleBL.GetSchedule(maMonHocTKB);
                        TeachingPeriodSchedule tKBTheoTiet = scheduleBL.GetTeachingPeriodSchedule(schedule);
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