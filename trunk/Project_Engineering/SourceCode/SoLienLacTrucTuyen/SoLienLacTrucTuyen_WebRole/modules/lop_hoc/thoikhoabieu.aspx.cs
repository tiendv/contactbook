using System;
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
        List<TeachingPeriodSchedule> teachingPeriodSchedules = new List<TeachingPeriodSchedule>();
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
                    BindRptTeachingPeriod();
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
                    int iYearId = Int32.Parse(DdlNamHoc.SelectedValue);
                    int iTermId = Int32.Parse(DdlHocKy.SelectedValue);
                    int iDayInWeekId = dailySchedule.DayInWeekId;
                    int iClassId = 0;
                    try
                    {
                        iClassId = Int32.Parse(DdlLopHoc.SelectedValue);
                    }
                    catch (Exception) { return; }

                    Label lblNghiSang = (Label)e.Item.FindControl("LblNghiSang");
                    SessionedSchedule morningSchedule = dailySchedule.SessionedSchedules[0];
                    if (morningSchedule.ListThoiKhoaBieuTheoTiet.Count == 0)
                    {
                        lblNghiSang.Visible = true;
                    }
                    else
                    {
                        lblNghiSang.Visible = false;
                        List<TeachingPeriodSchedule> lstThoiKhoaBieuTheoTiet = morningSchedule.ListThoiKhoaBieuTheoTiet;
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

        protected void RptSchedule_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rptDailySchedule = (Repeater)e.Item.FindControl("RptSessionSchedule");
                Label LblSessionName = (Label)e.Item.FindControl("LblSessionName");

                rptDailySchedule.DataSource = teachingPeriodSchedules.Where(schedule => schedule.SessionName == LblSessionName.Text)
                    .GroupBy(schedule => schedule.TeachingPeriodName).Select(
                    schedule => new
                    {
                        TeachingPeriodName = schedule.Key
                    })
                    .ToList();
                rptDailySchedule.DataBind();
            }
        }

        protected void RptSessionSchedule_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rptTeachingPeriodSchedule = (Repeater)e.Item.FindControl("RptTeachingPeriodSchedule");
                Label LblTeachingPeriodName = (Label)e.Item.FindControl("LblTeachingPeriodName");

                rptTeachingPeriodSchedule.DataSource = teachingPeriodSchedules.Where(schedule => schedule.TeachingPeriodName == LblTeachingPeriodName.Text)
                    .Select(
                    schedule => new
                    {
                        SubjectName = schedule.SubjectName
                    })
                    .ToList();
                rptTeachingPeriodSchedule.DataBind();
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
        private void BindRptSchedule()
        {
            if (DdlLopHoc.Items.Count != 0)
            {
                ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID] = Int32.Parse(DdlLopHoc.SelectedValue);
            }
            else
            {
                ProcessDislayInfo(false);
                return;
            }

            ViewState[AppConstant.VIEWSTATE_SELECTED_TERMID] = Int32.Parse(DdlHocKy.SelectedValue);

            LblTitle.Text = string.Format("Thời khóa biểu lớp {0} - {1} năm học {2}",
                DdlLopHoc.SelectedItem.Text, DdlHocKy.SelectedItem.Text, DdlNamHoc.SelectedItem.Text);

            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<Configuration_DayInWeek> dayInWeeks = systemConfigBL.GetDayInWeeks();


            Class_Class Class = new Class_Class();
            Class.ClassId = Int32.Parse(DdlLopHoc.SelectedValue);

            Configuration_Term term = new Configuration_Term();
            term.TermId = Int32.Parse(DdlHocKy.SelectedValue);

            teachingPeriodSchedules = scheduleBL.GetTeachingPeriodSchedules(Class, term);
            RptSchedule.DataSource = teachingPeriodSchedules.GroupBy(schedule => schedule.SessionName).Select(
                schedule => new
                {
                    SessionName = schedule.Key,
                    Count = schedule.GroupBy(s => s.TeachingPeriodId).Count()
                })
                .ToList();
            RptSchedule.DataBind();
            ProcessDislayInfo(true);
        }

        private void ProcPermissions()
        {
            BtnEdit.Visible = accessibilities.Contains(AccessibilityEnum.Modify);
        }

        private void ProcessDislayInfo(bool displayData)
        {
            LblErrorResult.Visible = !displayData;
            RptSchedule.Visible = displayData;
            LblTitle.Visible = displayData;
            tbHeader.Visible = displayData;
            RptTeachingPeriod.Visible = displayData;
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

        private void BindRptTeachingPeriod()
        {
            TeachingPeriodBL teachingPeriodBL = new TeachingPeriodBL(UserSchool);
            double d = 0;
            RptTeachingPeriod.DataSource = teachingPeriodBL.GetTabularTeachingPeriods(AppConstant.STRING_BLANK, null, 1, 100, out d);
            RptTeachingPeriod.DataBind();
        }
        #endregion
    }
}