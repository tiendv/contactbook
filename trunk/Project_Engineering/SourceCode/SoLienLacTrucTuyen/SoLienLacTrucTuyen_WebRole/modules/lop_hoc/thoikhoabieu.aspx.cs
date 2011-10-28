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
    public partial class ThoiKhoaBieu : System.Web.UI.Page
    {
        #region Fields
        private LopHocBL lopHocBL;
        private ThoiKhoaBieuBL thoiKhoaBieuBL;
        private List<AccessibilityEnum> lstAccessibilities;
        #endregion

        #region Page event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            RoleBL roleBL = new RoleBL();
            UserBL userBL = new UserBL();
            lopHocBL = new LopHocBL();
            thoiKhoaBieuBL = new ThoiKhoaBieuBL();

            string pageUrl = Page.Request.Path;
            Guid role = userBL.GetRoleId(User.Identity.Name);

            if (!roleBL.ValidateAuthorization(role, pageUrl))
            {
                Response.Redirect((string)GetGlobalResourceObject("MainResource", "AccessDeniedPageUrl"));
                return;
            }

            Site masterPage = (Site)Page.Master;
            masterPage.UserRole = role;
            masterPage.PageUrl = pageUrl;

            lstAccessibilities = roleBL.GetAccessibilities(role, pageUrl);

            if (!Page.IsPostBack)
            {
                BindDropDownLists();

                if (DdlLopHoc.Items.Count != 0)
                {
                    if (Request.QueryString["NamHoc"] != null && Request.QueryString["HocKy"] != null
                            && Request.QueryString["LopHoc"] != null)
                    {
                        this.DdlNamHoc.SelectedValue = (Request.QueryString["NamHoc"].ToString());
                        this.DdlHocKy.SelectedValue = (Request.QueryString["HocKy"].ToString());
                        this.DdlLopHoc.SelectedValue = (Request.QueryString["LopHoc"].ToString());
                    }

                    BindRptThoiKhoaBieu();
                }
                else
                {
                    ProcessDislayInfo(false);
                }
            }
        }
        #endregion

        #region DropDownList event hanlders
        protected void DdlNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropDownListLopHoc();
        }

        protected void DdlKhoiLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropDownListLopHoc();
        }

        protected void DdlNamHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropDownListLopHoc();
        }
     
        #endregion

        #region Repeater event handlers
        protected void RptMonHocTKB_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item 
                || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.DataItem != null)
                {
                    ThoiKhoaBieuTheoThu thoiKhoaBieuTheoThu = (ThoiKhoaBieuTheoThu)e.Item.DataItem;
                    int maNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);
                    int maHocKy = Int32.Parse(DdlHocKy.SelectedValue);
                    int maThu = thoiKhoaBieuTheoThu.MaThu;
                    int maLopHoc = 0;
                    try
                    {
                        maLopHoc = Int32.Parse(DdlLopHoc.SelectedValue);
                    }
                    catch (Exception) { return; }
                    
                    Label lblNghiSang = (Label)e.Item.FindControl("LblNghiSang");
                    ThoiKhoaBieuTheoBuoi thoiKhoaBieuBuoiSang = thoiKhoaBieuTheoThu.ListThoiKhoaBieuTheoBuoi[0];
                    if (thoiKhoaBieuBuoiSang.ListThoiKhoaBieuTheoTiet.Count == 0)
                    {
                        lblNghiSang.Visible = true;
                    }
                    else
                    {                        
                        lblNghiSang.Visible = false;
                        List<ThoiKhoaBieuTheoTiet> lstThoiKhoaBieuTheoTiet = thoiKhoaBieuBuoiSang.ListThoiKhoaBieuTheoTiet;
                        Repeater RptMonHocBuoiSang = (Repeater)e.Item.FindControl("RptMonHocBuoiSang");
                        RptMonHocBuoiSang.DataSource = lstThoiKhoaBieuTheoTiet;
                        RptMonHocBuoiSang.DataBind();
                    }

                    Label lblNghiChieu = (Label)e.Item.FindControl("LblNghiChieu");
                    ThoiKhoaBieuTheoBuoi thoiKhoaBieuBuoiChieu = thoiKhoaBieuTheoThu.ListThoiKhoaBieuTheoBuoi[1];
                    if (thoiKhoaBieuBuoiChieu.ListThoiKhoaBieuTheoTiet.Count == 0)
                    {
                        lblNghiChieu.Visible = true;
                    }
                    else
                    {
                        lblNghiChieu.Visible = false;
                        List<ThoiKhoaBieuTheoTiet> lstThoiKhoaBieuTheoTiet = thoiKhoaBieuBuoiChieu.ListThoiKhoaBieuTheoTiet;
                        Repeater RptMonHocBuoiChieu = (Repeater)e.Item.FindControl("RptMonHocBuoiChieu");
                        RptMonHocBuoiChieu.DataSource = lstThoiKhoaBieuTheoTiet;
                        RptMonHocBuoiChieu.DataBind();
                    }
                }
            }
        }

        protected void RptMonHocTKB_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdEditItem":
                    {
                        string maLopHoc = ((HiddenField)e.Item.FindControl("HdfRptMaLopHoc")).Value;
                        string maHocKy = ((HiddenField)e.Item.FindControl("HdfRptMaHocKy")).Value;
                        string maThu = ((HiddenField)e.Item.FindControl("HdfRptMaThu")).Value;

                        Response.Redirect(string.Format("suathoikhoabieu.aspx?lop={0}&hocky={1}&thu={2}", 
                            maLopHoc, maHocKy, maThu));
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            BindRptThoiKhoaBieu();
        }
        #endregion

        #region Methods
        private void BindRptThoiKhoaBieu()
        {
            // Get search criterias
            int maNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);
            int maHocKy = Int32.Parse(DdlHocKy.SelectedValue);
            int maLopHoc = 0;
            if (DdlLopHoc.Items.Count != 0)
            {
                maLopHoc = Int32.Parse(DdlLopHoc.SelectedValue);
                this.LblSearchResult.Visible = false;
                this.RptMonHocTKB.Visible = true;
            }
            else // In case there is no Lớp in Khối and Ngành
            {                
                this.LblSearchResult.Visible = true;
                this.RptMonHocTKB.Visible = false;
            }

            // Get list of ThoiKhoaBieuTheoThu from DB
            // and bind to Repeater
            List<ThoiKhoaBieuTheoThu> lstTKBTheoThu;
            lstTKBTheoThu = thoiKhoaBieuBL.GetThoiKhoaBieu(maNamHoc, maHocKy, maLopHoc);
            RptMonHocTKB.DataSource = lstTKBTheoThu;
            RptMonHocTKB.DataBind();

            //Session["ThoiKhoaBieu_MaNamHoc"] = maNamHoc;
            //Session["ThoiKhoaBieu_MaHocKy"] = maHocKy;
            //Session["ThoiKhoaBieu_MaLopHoc"] = maLopHoc;
        }

        private void ProcessDislayInfo(bool bDisplayData)
        {
            
        }

        private void BindDropDownLists()
        {
            BindDropDownListNamHoc();
            BindDropDownListHocKy();
            BindDropDownListNganhHoc();
            BindDropDownListKhoiLop();
            BindDropDownListLopHoc();
        }

        private void BindDropDownListNamHoc()
        {
            NamHocBL namHocBL = new NamHocBL();
            List<CauHinh_NamHoc> lstNamHoc = namHocBL.GetListNamHoc();
            DdlNamHoc.DataSource = lstNamHoc;
            DdlNamHoc.DataValueField = "MaNamHoc";
            DdlNamHoc.DataTextField = "TenNamHoc";
            DdlNamHoc.DataBind();            
            
            if (Session["ThoiKhoaBieu_MaNamHoc"] != null)
            {
                DdlNamHoc.SelectedValue = Session["ThoiKhoaBieu_MaNamHoc"].ToString();
                Session.Remove("ThoiKhoaBieu_MaNamHoc");
            }
            else
            {
                DdlNamHoc.SelectedValue = (new CauHinhHeThongBL()).GetMaNamHocHienHanh().ToString();
            }
        }

        private void BindDropDownListNganhHoc()
        {
            FacultyBL nganhHocBL = new FacultyBL();
            List<DanhMuc_NganhHoc> lstNganhHoc = nganhHocBL.GetListNganhHoc();
            DdlNganh.DataSource = lstNganhHoc;
            DdlNganh.DataValueField = "MaNganhHoc";
            DdlNganh.DataTextField = "TenNganhHoc";
            DdlNganh.DataBind();
            if (lstNganhHoc.Count > 1)
            {
                DdlNganh.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }

        private void BindDropDownListKhoiLop()
        {
            KhoiLopBL KhoiLopBL = new KhoiLopBL();
            List<DanhMuc_KhoiLop> lstKhoiLop = KhoiLopBL.GetListKhoiLop();
            DdlKhoiLop.DataSource = lstKhoiLop;
            DdlKhoiLop.DataValueField = "MaKhoiLop";
            DdlKhoiLop.DataTextField = "TenKhoiLop";
            DdlKhoiLop.DataBind();
            if (lstKhoiLop.Count > 1)
            {
                DdlKhoiLop.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }        

        private void BindDropDownListHocKy()
        {
            HocKyBL hocKyBL = new HocKyBL();
            List<CauHinh_HocKy> lstHocKy = hocKyBL.GetListHocKy();
            DdlHocKy.DataSource = lstHocKy;
            DdlHocKy.DataValueField = "MaHocKy";
            DdlHocKy.DataTextField = "TenHocKy";
            DdlHocKy.DataBind();

            if (Session["ThoiKhoaBieu_MaHocKy"] != null)
            {
                DdlHocKy.SelectedValue = Session["ThoiKhoaBieu_MaHocKy"].ToString();
                Session.Remove("ThoiKhoaBieu_MaHocKy");
            }
            else
            {
                CauHinhHeThongBL cauHinhHeThongBL = new CauHinhHeThongBL();
                DdlHocKy.SelectedValue = cauHinhHeThongBL.GetMaHocKyHienHanh().ToString();
            }
        }

        private void BindDropDownListLopHoc()
        {
            if (DdlNamHoc.Items.Count == 0 || DdlNganh.Items.Count == 0 || DdlKhoiLop.Items.Count == 0)
            {
                BtnSearch.ImageUrl = "~/Styles/Images/button_search_with_text_disable.png";
                BtnSearch.Enabled = false;
                return;
            }

            int maNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);

            int maNganhHoc = 0;
            try
            {
                maNganhHoc = Int32.Parse(DdlNganh.SelectedValue);
            }
            catch (Exception) { }

            int maKhoiLop = 0;
            try
            {
                maKhoiLop = Int32.Parse(DdlKhoiLop.SelectedValue);
            }
            catch (Exception) { }
            
            List<LopHoc_Lop> lstLop = lopHocBL.GetListLopHoc(maNganhHoc, maKhoiLop, maNamHoc);
            DdlLopHoc.DataSource = lstLop;
            DdlLopHoc.DataValueField = "MaLopHoc";
            DdlLopHoc.DataTextField = "TenLopHoc";
            DdlLopHoc.DataBind();

            if (Session["ThoiKhoaBieu_MaLopHoc"] != null)
            {
                DdlLopHoc.SelectedValue = Session["ThoiKhoaBieu_MaLopHoc"].ToString();
                Session.Remove("ThoiKhoaBieu_MaLopHoc");
            }
        }
        #endregion   
    }
}