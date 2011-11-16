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
    public partial class ThoiKhoaBieu : BaseContentPage
    {
        #region Fields
        private ClassBL classBL;
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

            classBL = new ClassBL(UserSchool);
            scheduleBL = new ScheduleBL(UserSchool);
         
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

                    BindRptSchedule();
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
            BindDDLClasses();
        }

        protected void DdlKhoiLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLClasses();
        }

        protected void DdlNamHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLClasses();
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
                    DailySchedule dailySchedule = (DailySchedule)e.Item.DataItem;
                    int maNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);
                    int maHocKy = Int32.Parse(DdlHocKy.SelectedValue);
                    int maThu = dailySchedule.MaThu;
                    int maLopHoc = 0;
                    try
                    {
                        maLopHoc = Int32.Parse(DdlLopHoc.SelectedValue);
                    }
                    catch (Exception) { return; }
                    
                    Label lblNghiSang = (Label)e.Item.FindControl("LblNghiSang");
                    SessionedSchedule thoiKhoaBieuBuoiSang = dailySchedule.SessionedSchedules[0];
                    if (thoiKhoaBieuBuoiSang.ListThoiKhoaBieuTheoTiet.Count == 0)
                    {
                        lblNghiSang.Visible = true;
                    }
                    else
                    {                        
                        lblNghiSang.Visible = false;
                        List<TeachingPeriodSchedule> lstThoiKhoaBieuTheoTiet = thoiKhoaBieuBuoiSang.ListThoiKhoaBieuTheoTiet;
                        Repeater RptMonHocBuoiSang = (Repeater)e.Item.FindControl("RptMonHocBuoiSang");
                        RptMonHocBuoiSang.DataSource = lstThoiKhoaBieuTheoTiet;
                        RptMonHocBuoiSang.DataBind();
                    }

                    Label lblNghiChieu = (Label)e.Item.FindControl("LblNghiChieu");
                    SessionedSchedule thoiKhoaBieuBuoiChieu = dailySchedule.SessionedSchedules[1];
                    if (thoiKhoaBieuBuoiChieu.ListThoiKhoaBieuTheoTiet.Count == 0)
                    {
                        lblNghiChieu.Visible = true;
                    }
                    else
                    {
                        lblNghiChieu.Visible = false;
                        List<TeachingPeriodSchedule> lstThoiKhoaBieuTheoTiet = thoiKhoaBieuBuoiChieu.ListThoiKhoaBieuTheoTiet;
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
            BindRptSchedule();
        }
        #endregion

        #region Methods
        private void BindRptSchedule()
        {
            CauHinh_HocKy term = null;
            LopHoc_Lop Class = null;
            List<DailySchedule> dailySchedules;

            // Get search criterias
            term = new CauHinh_HocKy();
            term.MaHocKy = Int32.Parse(DdlHocKy.SelectedValue);
            
            if (DdlLopHoc.Items.Count != 0)
            {
                Class = new LopHoc_Lop();
                Class.MaLopHoc = Int32.Parse(DdlLopHoc.SelectedValue);
                dailySchedules = scheduleBL.GetDailySchedules(Class, term);
                this.LblSearchResult.Visible = false;
                this.RptMonHocTKB.Visible = true;
            }
            else // In case there is no Lớp in Khối and Ngành
            {                
                this.LblSearchResult.Visible = true;
                this.RptMonHocTKB.Visible = false;
                return;
            }

            RptMonHocTKB.DataSource = dailySchedules;
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
            BindDDlYears();
            BindDDLTerms();
            BindDDlFaculties();
            BindDDlGrades();
            BindDDLClasses();
        }

        private void BindDDlYears()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<CauHinh_NamHoc> lstNamHoc = systemConfigBL.GetListYears();
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
                DdlNamHoc.SelectedValue = (new SystemConfigBL(UserSchool)).GetCurrentYear().ToString();
            }
        }

        private void BindDDlFaculties()
        {
            FacultyBL nganhHocBL = new FacultyBL(UserSchool);
            List<DanhMuc_NganhHoc> lstNganhHoc = nganhHocBL.GetFaculties();
            DdlNganh.DataSource = lstNganhHoc;
            DdlNganh.DataValueField = "MaNganhHoc";
            DdlNganh.DataTextField = "TenNganhHoc";
            DdlNganh.DataBind();
            if (lstNganhHoc.Count > 1)
            {
                DdlNganh.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }

        private void BindDDlGrades()
        {
            GradeBL KhoiLopBL = new GradeBL(UserSchool);
            List<DanhMuc_KhoiLop> lstKhoiLop = KhoiLopBL.GetListGrades();
            DdlKhoiLop.DataSource = lstKhoiLop;
            DdlKhoiLop.DataValueField = "MaKhoiLop";
            DdlKhoiLop.DataTextField = "TenKhoiLop";
            DdlKhoiLop.DataBind();
            if (lstKhoiLop.Count > 1)
            {
                DdlKhoiLop.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }        

        private void BindDDLTerms()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<CauHinh_HocKy> lstHocKy = systemConfigBL.GetListTerms();
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
                DdlHocKy.SelectedValue = systemConfigBL.GetCurrentTerm().ToString();
            }
        }

        private void BindDDLClasses()
        {
            CauHinh_NamHoc year = null;
            DanhMuc_NganhHoc faculty = null;
            DanhMuc_KhoiLop grade = null;

            if (DdlNamHoc.Items.Count == 0 || DdlNganh.Items.Count == 0 || DdlKhoiLop.Items.Count == 0)
            {
                BtnSearch.ImageUrl = "~/Styles/Images/button_search_with_text_disable.png";
                BtnSearch.Enabled = false;
                return;
            }

            year = new CauHinh_NamHoc();
            year.MaNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);

            try
            {
                if (DdlNganh.SelectedIndex > 0)
                {
                    faculty = new DanhMuc_NganhHoc();
                    faculty.MaNganhHoc = Int32.Parse(DdlNganh.SelectedValue);
                }
            }
            catch (Exception) { }

            try
            {
                if (DdlKhoiLop.SelectedIndex > 0)
                {
                    grade = new DanhMuc_KhoiLop();
                    grade.MaKhoiLop = Int32.Parse(DdlKhoiLop.SelectedValue);
                }
            }
            catch (Exception) { }

            List<LopHoc_Lop> lstLop = classBL.GetListClasses(year, faculty, grade);
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