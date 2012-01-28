﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;
using AjaxControlToolkit;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class SchedulePage : BaseContentPage, IPostBackEventHandler
    {
        #region Fields
        private ClassBL classBL;
        private ScheduleBL scheduleBL;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (accessDenied)
            {
                return;
            }

            if (sessionExpired)
            {
                FormsAuthentication.SignOut();
                Response.Redirect(FormsAuthentication.LoginUrl);
            }

            classBL = new ClassBL(UserSchool);
            scheduleBL = new ScheduleBL(UserSchool);
         
            if (!Page.IsPostBack)
            {
                BindDropDownLists();
                RetrieveSessions();

                if (DdlLopHoc.Items.Count != 0)
                {
                    BindRptSchedule();
                }
                else
                {
                    ProcessDislayInfo(false);
                }
            }

            ProcPermissions();
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
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
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
        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            // get class information from viewstate and set to session
            Class_Class Class = new Class_Class();
            Class.ClassId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID];
            AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);

            // get term information from viewstate and set to session
            Configuration_Term term = new Configuration_Term();
            term.TermId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_TERMID];
            AddSession(AppConstant.SESSION_SELECTED_TERM, term);

            // redirect to "sapxepthoikhoabieu.aspx" page
            Response.Redirect(AppConstant.PAGEPATH_SCHEDULE_AGGRANGE);
        }

        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            BindRptSchedule();
        }
        #endregion

        #region Methods
        private void ProcPermissions()
        {
            BtnEdit.Visible = accessibilities.Contains(AccessibilityEnum.Modify);
        }

        private void BindRptSchedule()
        {
            Class_Class Class = null;
            List<DailySchedule> weeklySchedule;

            // Get search criterias
            Configuration_Term term = new Configuration_Term();
            term.TermId = Int32.Parse(DdlHocKy.SelectedValue);
            
            if (DdlLopHoc.Items.Count != 0)
            {
                Class = new Class_Class();
                Class.ClassId = Int32.Parse(DdlLopHoc.SelectedValue);
                weeklySchedule = scheduleBL.GetDailySchedules(Class, term);
                this.LblSearchResult.Visible = false;
                this.RptMonHocTKB.Visible = true;
            }
            else // In case there is no Lớp in Khối and Ngành
            {                
                this.LblSearchResult.Visible = true;
                this.RptMonHocTKB.Visible = false;
                return;
            }

            RptMonHocTKB.DataSource = weeklySchedule;
            RptMonHocTKB.DataBind();

            ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID] = Class.ClassId;
            ViewState[AppConstant.VIEWSTATE_SELECTED_TERMID] = term.TermId;
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

            if (DdlNamHoc.Items.Count != 0)
            {
                if (Session["ThoiKhoaBieu_YearId"] != null)
                {
                    DdlNamHoc.SelectedValue = Session["ThoiKhoaBieu_YearId"].ToString();
                    Session.Remove("ThoiKhoaBieu_YearId");
                }
                else
                {
                    DdlNamHoc.SelectedValue = (new SystemConfigBL(UserSchool)).GetLastedYear().ToString();
                }
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
                Configuration_Term currentTerm = systemConfigBL.GetCurrentTerm();
                if (currentTerm != null)
                {
                    DdlHocKy.SelectedValue = currentTerm.TermId.ToString();
                }
            }
        }

        private void BindDDLClasses()
        {
            Configuration_Year year = null;
            Category_Faculty faculty = null;
            Category_Grade grade = null;

            if (DdlNamHoc.Items.Count == 0 || DdlNganh.Items.Count == 0 || DdlKhoiLop.Items.Count == 0)
            {
                BtnSearch.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SEARCH_DISABLE;
                BtnSearch.Enabled = false;

                BtnPrint.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_EXPORT_DISABLED;
                BtnPrint.Enabled = false;

                BtnEdit.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_ARRANGE_DISABLE;
                BtnEdit.Enabled = false;
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

            Configuration_Term term = new Configuration_Term();
            term.TermId = Int32.Parse(DdlHocKy.SelectedValue);

            List<Class_Class> Classes = classBL.GetClasses(LogedInUser, IsFormerTeacher, IsSubjectTeacher, year, faculty, grade, term);
            DdlLopHoc.DataSource = Classes;
            DdlLopHoc.DataValueField = "ClassId";
            DdlLopHoc.DataTextField = "ClassName";
            DdlLopHoc.DataBind();

            if (DdlLopHoc.Items.Count > 0)
            {
                BtnSearch.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SEARCH;
                BtnSearch.Enabled = true;

                BtnPrint.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_EXPORT;
                BtnPrint.Enabled = true;

                if (Session["ThoiKhoaBieu_ClassId"] != null)
                {
                    DdlLopHoc.SelectedValue = Session["ThoiKhoaBieu_ClassId"].ToString();
                    Session.Remove("ThoiKhoaBieu_ClassId");
                }
            }
            else
            {
                BtnSearch.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SEARCH_DISABLE;
                BtnSearch.Enabled = false;

                BtnPrint.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_EXPORT_DISABLED;
                BtnPrint.Enabled = false;

                BtnEdit.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_ARRANGE_DISABLE;
                BtnEdit.Enabled = false;
            }
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            PrePrint();
        }

        protected void PrePrint()
        {
            #region Add Info 2 Session

            Configuration_Year year = null;
            Category_Faculty faculty = null;
            Category_Grade grade = null;
            Class_Class Class = null;
            Configuration_Term term = null;

            year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);
            year.YearName = DdlNamHoc.SelectedItem.Text;
            try
            {
                faculty = new Category_Faculty();
                faculty.FacultyId = Int32.Parse(DdlNganh.SelectedValue);
                faculty.FacultyName = DdlNganh.SelectedItem.Text; ;

            }
            catch (Exception) { }

            try
            {

                grade = new Category_Grade();
                grade.GradeId = Int32.Parse(DdlKhoiLop.SelectedValue);
                grade.GradeName = DdlKhoiLop.SelectedItem.Text; ;
            }
            catch (Exception) { }
            try
            {
                Class = new Class_Class();
                Class.ClassId = Int32.Parse(DdlLopHoc.SelectedValue);
                Class.ClassName = DdlLopHoc.SelectedItem.Text; ;
            }
            catch (Exception) { }
            try
            {
                term = new Configuration_Term();
                term.TermId = Int32.Parse(DdlHocKy.SelectedValue);
                term.TermName = DdlHocKy.SelectedItem.Text; ;
            }
            catch (Exception) { }

            AddSession(AppConstant.SESSION_PAGEPATH, AppConstant.PAGEPATH_PRINTTERM);
            AddSession(AppConstant.SESSION_SELECTED_YEAR, year);
            AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);
            AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);
            AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);
            AddSession(AppConstant.SESSION_SELECTED_TERM, term);
            //Response.Redirect(AppConstant.PAGEPATH_PRINTSTUDENTS);
            #endregion
        }

        private bool RetrieveSessions()
        {
            if (CheckSessionKey(AppConstant.SESSION_SELECTED_CLASS) && CheckSessionKey(AppConstant.SESSION_SELECTED_TERM))
            {
                Class_Class Class = (Class_Class)GetSession(AppConstant.SESSION_SELECTED_CLASS);
                RemoveSession(AppConstant.SESSION_SELECTED_CLASS); 
                DdlLopHoc.SelectedValue = Class.ClassId.ToString();
                
                Configuration_Term term = (Configuration_Term)GetSession(AppConstant.SESSION_SELECTED_TERM);
                RemoveSession(AppConstant.SESSION_SELECTED_TERM);
                DdlHocKy.SelectedValue = term.TermId.ToString();

                return true;
            }

            return false;
        }
        #endregion   
    }
}