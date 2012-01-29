using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EContactBook.BusinessEntity;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class ViewStudentConductPage : BaseContentPage
    {
        #region Fields
        private StudyingResultBL studyingResultBL;
        private ConductBL conductBL;
        private StudentBL studentBL;
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

            studentBL = new StudentBL(UserSchool);
            studyingResultBL = new StudyingResultBL(UserSchool);
            conductBL = new ConductBL(UserSchool);

            if (!Page.IsPostBack)
            {
                BindDropDownLists();
                RetrieveSessions();
                BindRptStudentConducts();
            }

            ProcPermissions();
        }
        #endregion

        #region DropDownList event hanlders
        protected void DdlNamHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLLopHoc();
        }

        protected void DdlNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLLopHoc();
        }

        protected void DdlKhoiLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLLopHoc();
        }
        #endregion

        #region Methods
        private void ProcPermissions()
        {
            BtnEdit.Visible = accessibilities.Contains(AccessibilityEnum.Modify);
        }

        private void BindDropDownLists()
        {
            BindDDLNamHoc();
            BindDDLHocKy();
            BindDDLNganhHoc();
            BindDDLKhoiLop();
            BindDDLLopHoc();
        }

        private void BindDDLNamHoc()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<Configuration_Year> years = systemConfigBL.GetListYears();
            DdlNamHoc.DataSource = years;
            DdlNamHoc.DataValueField = "YearId";
            DdlNamHoc.DataTextField = "YearName";
            DdlNamHoc.DataBind();

            if (DdlNamHoc.Items.Count != 0)
            {
                SystemConfigBL cauHinhBL = new SystemConfigBL(UserSchool);
                DdlNamHoc.SelectedValue = cauHinhBL.GetLastedYear().ToString();
            }
        }

        private void BindDDLHocKy()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<Configuration_Term> lstHocKy = systemConfigBL.GetListTerms();
            DdlHocKy.DataSource = lstHocKy;
            DdlHocKy.DataValueField = "TermId";
            DdlHocKy.DataTextField = "TermName";
            DdlHocKy.DataBind();
        }

        private void BindDDLNganhHoc()
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

        private void BindDDLKhoiLop()
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

        private void BindDDLLopHoc()
        {
            Configuration_Year year = null;
            Category_Faculty faculty = null;
            Category_Grade grade = null;

            if (DdlNamHoc.Items.Count == 0 || DdlNganh.Items.Count == 0 || DdlKhoiLop.Items.Count == 0)
            {
                BtnSearch.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SEARCH_DISABLE;
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

            ClassBL classBL = new ClassBL(UserSchool);
            List<Class_Class> Classes = classBL.GetClasses(LogedInUser, IsFormerTeacher, IsSubjectTeacher, year, faculty, grade, null);
            DdlLopHoc.DataSource = Classes;
            DdlLopHoc.DataValueField = "ClassId";
            DdlLopHoc.DataTextField = "ClassName";
            DdlLopHoc.DataBind();
        }

        private void BindRptStudentConducts()
        {
            if (DdlLopHoc.Items.Count == 0)
            {
                ProccessDisplayGUI(false);
                return;
            }

            Class_Class Class = new Class_Class();
            Class.ClassId = Int32.Parse(DdlLopHoc.SelectedValue);

            Configuration_Term term = new Configuration_Term();
            term.TermId = Int32.Parse(DdlHocKy.SelectedValue);

            double dTotalRecords = 0;
            List<TabularStudentConduct> tabularStudentConducts;
            tabularStudentConducts = studentBL.GetTabularStudentConducts(Class, term, MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            this.RptHanhKiemHocSinh.DataSource = tabularStudentConducts;
            this.RptHanhKiemHocSinh.DataBind();
            MainDataPager.ItemCount = dTotalRecords;

            bool bDisplayData = (tabularStudentConducts.Count != 0) ? true : false;
            ProccessDisplayGUI(bDisplayData);

            ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID] = Int32.Parse(DdlNamHoc.SelectedValue);
            ViewState[AppConstant.VIEWSTATE_SELECTED_YEARNAME] = DdlNamHoc.SelectedItem.Text;

            ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTYID] = Int32.Parse(DdlNganh.SelectedValue);
            ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTYNAME] = DdlNganh.SelectedItem.Text;
            
            ViewState[AppConstant.VIEWSTATE_SELECTED_GRADEID] = Int32.Parse(DdlKhoiLop.SelectedValue);
            ViewState[AppConstant.VIEWSTATE_SELECTED_GRADENAME] = DdlKhoiLop.SelectedItem.Text;

            ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID] = Int32.Parse(DdlLopHoc.SelectedValue);
            ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSNAME] = DdlLopHoc.SelectedItem.Text;

            ViewState[AppConstant.VIEWSTATE_SELECTED_TERMID] = Int32.Parse(DdlHocKy.SelectedValue);
            ViewState[AppConstant.VIEWSTATE_SELECTED_TERMNAME] = DdlHocKy.SelectedItem.Text;
        }

        private void ProccessDisplayGUI(bool display)
        {
            RptHanhKiemHocSinh.Visible = display;
            MainDataPager.Visible = display;            
            BtnEdit.Enabled = display;
            if (display)
            {
                BtnEdit.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_DANHGIA;
            }
            else
            {
                BtnEdit.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_DANHGIA_DISABLE;
            }

            LblSearchResult.Visible = !display;
        }

        private bool RetrieveSessions()
        {
            if (CheckSessionKey(AppConstant.SESSION_SELECTED_YEAR)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_TERM)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_FACULTY)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_GRADE)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_CLASS))
            {
                DdlNamHoc.SelectedValue = ((Configuration_Year)GetSession(AppConstant.SESSION_SELECTED_YEAR)).YearId.ToString();
                RemoveSession(AppConstant.SESSION_SELECTED_YEAR);

                DdlHocKy.SelectedValue = ((Configuration_Term)GetSession(AppConstant.SESSION_SELECTED_TERM)).TermId.ToString();
                RemoveSession(AppConstant.SESSION_SELECTED_TERM);

                DdlNganh.SelectedValue = ((Category_Faculty)GetSession(AppConstant.SESSION_SELECTED_FACULTY)).FacultyId.ToString();
                RemoveSession(AppConstant.SESSION_SELECTED_FACULTY);

                DdlKhoiLop.SelectedValue = ((Category_Grade)GetSession(AppConstant.SESSION_SELECTED_GRADE)).GradeId.ToString();
                RemoveSession(AppConstant.SESSION_SELECTED_GRADE);

                DdlLopHoc.SelectedValue = ((Class_Class)GetSession(AppConstant.SESSION_SELECTED_CLASS)).ClassId.ToString();
                RemoveSession(AppConstant.SESSION_SELECTED_CLASS);

                return true;
            }

            return false;
        }
        #endregion

        #region Button event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            BindRptStudentConducts();
        }

        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            Configuration_Year year = new Configuration_Year();
            year.YearId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID];
            year.YearName = (string)ViewState[AppConstant.VIEWSTATE_SELECTED_YEARNAME];

            Category_Faculty faculty = new Category_Faculty();
            faculty.FacultyId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTYID];
            faculty.FacultyName = (string)ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTYNAME];

            Category_Grade grade = new Category_Grade();
            grade.GradeId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_GRADEID];
            grade.GradeName = (string)ViewState[AppConstant.VIEWSTATE_SELECTED_GRADENAME];

            Class_Class Class = new Class_Class();
            Class.ClassId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID];
            Class.ClassName = (string)ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSNAME];

            Configuration_Term term = new Configuration_Term();
            term.TermId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_TERMID];
            term.TermName = (string)ViewState[AppConstant.VIEWSTATE_SELECTED_TERMNAME];

            AddSession(AppConstant.SESSION_SELECTED_YEAR, year);
            AddSession(AppConstant.SESSION_SELECTED_TERM, term);
            AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);
            AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);
            AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);

            Response.Redirect(AppConstant.PAGEPATH_STUDENT_CONDUCT_MODIFY);
        }
        #endregion

        #region Repeater event handlers
        protected void RptHanhKiemHocSinh_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            
        }
        #endregion

        #region Pager event handlers
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currnetPageIndx;
            BindRptStudentConducts();
        }
        #endregion
    }
}