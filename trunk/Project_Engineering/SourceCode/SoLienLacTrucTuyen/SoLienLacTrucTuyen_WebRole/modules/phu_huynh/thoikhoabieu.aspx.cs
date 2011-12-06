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

namespace SoLienLacTrucTuyen_WebRole.Modules.ModuleParents
{
    public partial class SchedulePage : BaseContentPage
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
                    int YearId = Int32.Parse(DdlNamHoc.SelectedValue);
                    int TermId = Int32.Parse(DdlHocKy.SelectedValue);
                    int DayInWeekId = dailySchedule.DayInWeekId;
                    int ClassId = 0;
                    try
                    {
                        ClassId = Int32.Parse(DdlLopHoc.SelectedValue);
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
            Configuration_Term term = null;
            Class_Class Class = null;
            List<DailySchedule> dailySchedules;

            // Get search criterias
            term = new Configuration_Term();
            term.TermId = Int32.Parse(DdlHocKy.SelectedValue);
            
            if (DdlLopHoc.Items.Count != 0)
            {
                Class = new Class_Class();
                Class.ClassId = Int32.Parse(DdlLopHoc.SelectedValue);
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

            //Session["ThoiKhoaBieu_YearId"] = YearId;
            //Session["ThoiKhoaBieu_TermId"] = TermId;
            //Session["ThoiKhoaBieu_ClassId"] = ClassId;
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
            List<Configuration_Year> lstNamHoc = systemConfigBL.GetListYears();
            DdlNamHoc.DataSource = lstNamHoc;
            DdlNamHoc.DataValueField = "YearId";
            DdlNamHoc.DataTextField = "YearName";
            DdlNamHoc.DataBind();            
            
            if (Session["ThoiKhoaBieu_YearId"] != null)
            {
                DdlNamHoc.SelectedValue = Session["ThoiKhoaBieu_YearId"].ToString();
                Session.Remove("ThoiKhoaBieu_YearId");
            }
            else
            {
                DdlNamHoc.SelectedValue = (new SystemConfigBL(UserSchool)).GetCurrentYear().ToString();
            }
        }

        private void BindDDlFaculties()
        {
            FacultyBL facultyBL = new FacultyBL(UserSchool);
            List<Category_Faculty> faculties = facultyBL.GetFaculties();
            DdlNganh.DataSource = faculties;
            DdlNganh.DataValueField = "FacultyId";
            DdlNganh.DataTextField = "FacultyName";
            DdlNganh.DataBind();
            if (faculties.Count > 1)
            {
                DdlNganh.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }

        private void BindDDlGrades()
        {
            GradeBL grades = new GradeBL(UserSchool);
            List<Category_Grade> lstKhoiLop = grades.GetListGrades();
            DdlKhoiLop.DataSource = lstKhoiLop;
            DdlKhoiLop.DataValueField = "GradeId";
            DdlKhoiLop.DataTextField = "GradeName";
            DdlKhoiLop.DataBind();
            if (lstKhoiLop.Count > 1)
            {
                DdlKhoiLop.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }        

        private void BindDDLTerms()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<Configuration_Term> lstHocKy = systemConfigBL.GetListTerms();
            DdlHocKy.DataSource = lstHocKy;
            DdlHocKy.DataValueField = "TermId";
            DdlHocKy.DataTextField = "TermName";
            DdlHocKy.DataBind();

            if (Session["ThoiKhoaBieu_TermId"] != null)
            {
                DdlHocKy.SelectedValue = Session["ThoiKhoaBieu_TermId"].ToString();
                Session.Remove("ThoiKhoaBieu_TermId");
            }
            else
            {
                DdlHocKy.SelectedValue = systemConfigBL.GetCurrentTerm().ToString();
            }
        }

        private void BindDDLClasses()
        {
            Configuration_Year year = null;
            Category_Faculty faculty = null;
            Category_Grade grade = null;

            if (DdlNamHoc.Items.Count == 0 || DdlNganh.Items.Count == 0 || DdlKhoiLop.Items.Count == 0)
            {
                BtnSearch.ImageUrl = "~/Styles/Images/button_search_with_text_disable.png";
                BtnSearch.Enabled = false;
                return;
            }

            year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);

            try
            {
                if (DdlNganh.SelectedIndex > 0)
                {
                    faculty = new Category_Faculty();
                    faculty.FacultyId = Int32.Parse(DdlNganh.SelectedValue);
                }
            }
            catch (Exception) { }

            try
            {
                if (DdlKhoiLop.SelectedIndex > 0)
                {
                    grade = new Category_Grade();
                    grade.GradeId = Int32.Parse(DdlKhoiLop.SelectedValue);
                }
            }
            catch (Exception) { }

            List<Class_Class> lstLop = classBL.GetListClasses(year, faculty, grade);
            DdlLopHoc.DataSource = lstLop;
            DdlLopHoc.DataValueField = "ClassId";
            DdlLopHoc.DataTextField = "ClassName";
            DdlLopHoc.DataBind();

            if (Session["ThoiKhoaBieu_ClassId"] != null)
            {
                DdlLopHoc.SelectedValue = Session["ThoiKhoaBieu_ClassId"].ToString();
                Session.Remove("ThoiKhoaBieu_ClassId");
            }
        }
        #endregion   
    }
}