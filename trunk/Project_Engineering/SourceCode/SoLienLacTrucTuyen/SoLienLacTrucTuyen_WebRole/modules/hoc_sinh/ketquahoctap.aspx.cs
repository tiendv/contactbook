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
using System.Web.UI.HtmlControls;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class StudentStudyingResultPage : BaseContentPage
    {
        #region Fields
        private StudentBL studentBL;
        private MarkTypeBL markTypeBL;
        private StudyingResultBL studyingResultBL;
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
            markTypeBL = new MarkTypeBL(UserSchool);
            studyingResultBL = new StudyingResultBL(UserSchool);

            if (!Page.IsPostBack)
            {
                if (CheckSessionKey(AppConstant.SESSION_STUDENT))
                {
                    Student_Student student = (Student_Student)GetSession(AppConstant.SESSION_STUDENT);
                    RemoveSession(AppConstant.SESSION_STUDENT);
                    ViewState[AppConstant.VIEWSTATE_STUDENTID] = student.StudentId;

                    Class_Class studentClass = (Class_Class)GetSession(AppConstant.SESSION_STUDENTCLASS);
                    RemoveSession(AppConstant.SESSION_STUDENTCLASS);
                    ViewState[AppConstant.VIEWSTATE_STUDENTCLASS_ID] = studentClass.ClassId;

                    Configuration_Year year = (Configuration_Year)GetSession(AppConstant.SESSION_SELECTED_YEAR);
                    RemoveSession(AppConstant.SESSION_SELECTED_YEAR);
                    ViewState[AppConstant.VIEWSTATE_STUDENTID] = student.StudentId;

                    Category_Faculty faculty = (Category_Faculty)GetSession(AppConstant.SESSION_SELECTED_FACULTY);
                    RemoveSession(AppConstant.SESSION_SELECTED_FACULTY);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTY] = faculty.FacultyId;

                    Category_Grade grade = (Category_Grade)GetSession(AppConstant.SESSION_SELECTED_GRADE);
                    RemoveSession(AppConstant.SESSION_SELECTED_GRADE);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_GRADE] = grade.GradeId;

                    Class_Class Class = (Class_Class)GetSession(AppConstant.SESSION_SELECTED_CLASS);
                    RemoveSession(AppConstant.SESSION_SELECTED_CLASS);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS] = Class.ClassId;

                    String strStudentName = (string)GetSession(AppConstant.SESSION_SELECTED_STUDENTNAME);
                    RemoveSession(AppConstant.SESSION_SELECTED_STUDENTNAME);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTNAME] = strStudentName;

                    String strStudentCode = (string)GetSession(AppConstant.SESSION_SELECTED_STUDENTCODE);
                    RemoveSession(AppConstant.SESSION_SELECTED_STUDENTCODE);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTCODE] = strStudentCode;

                    ViewState[AppConstant.VIEWSTATE_STUDENTID] = student.StudentId;

                    BindDropDownLists();
                    BindRptMarkTypes();
                    BindRptKetQuaDiem();
                    BindRepeaterDanhHieu();

                    AuthorizationBL authorizationBL = new AuthorizationBL(UserSchool);
                    List<UserManagement_PagePath> pagePages = authorizationBL.GetStudentPages(
                        (new UserBL()).GetRoles(User.Identity.Name));
                    RptStudentFunctions.DataSource = pagePages;
                    RptStudentFunctions.DataBind();
                }
                else
                {
                    Response.Redirect(AppConstant.PAGEPATH_STUDENTS);
                }
            }
        }
        #endregion

        #region Methods
        private void BindDropDownLists()
        {
            BindDDLYears();
            BindDDLTerms();
        }

        private void BindDDLYears()
        {
            if (ViewState[AppConstant.VIEWSTATE_STUDENTID] != null)
            {
                Student_Student student = new Student_Student();
                student.StudentId = (int)ViewState[AppConstant.VIEWSTATE_STUDENTID];
                List<Configuration_Year> years = studentBL.GetYears(student);
                DdlNamHoc.DataSource = years;
                DdlNamHoc.DataValueField = "YearId";
                DdlNamHoc.DataTextField = "YearName";
                DdlNamHoc.DataBind();
            }
        }

        private void BindDDLTerms()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<Configuration_Term> terms = systemConfigBL.GetListTerms();
            DdlHocKy.DataSource = terms;
            DdlHocKy.DataValueField = "TermId";
            DdlHocKy.DataTextField = "TermName";
            DdlHocKy.DataBind();
        }

        private void BindRptMarkTypes()
        {
            Student_Student student = new Student_Student();
            student.StudentId = (int)ViewState[AppConstant.VIEWSTATE_STUDENTID];
            Configuration_Year year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);
            Class_Class Class = studentBL.GetClass(student, year);

            List<Category_MarkType> markTypes = markTypeBL.GetListMarkTypes(Class.Category_Grade);
            this.RptLoaiDiem.DataSource = markTypes;
            this.RptLoaiDiem.DataBind();
        }

        private void BindRptKetQuaDiem()
        {
            Configuration_Year year = new Configuration_Year();
            Student_Student student = new Student_Student();
            Configuration_Term term = new Configuration_Term();

            student.StudentId = (int)ViewState[AppConstant.VIEWSTATE_STUDENTID]; ;
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);
            term.TermId = Int32.Parse(DdlHocKy.SelectedValue);

            double dTotalRecords;
            List<TabularSubjectTermResult> tabularSubjectTermResults;
            tabularSubjectTermResults = studyingResultBL.GetTabularSubjectTermResults(student, year, term,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            bool bDisplayData = (tabularSubjectTermResults.Count != 0) ? true : false;
            RptKetQuaDiem.Visible = bDisplayData;
            MainDataPager.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            RptLoaiDiem.Visible = bDisplayData;
            tdKQHocTapSTT.Visible = bDisplayData;
            tdKQHocTapMonHoc.Visible = bDisplayData;
            tdKQHocTapDTB.Visible = bDisplayData;

            this.RptKetQuaDiem.DataSource = tabularSubjectTermResults;
            this.RptKetQuaDiem.DataBind();
            MainDataPager.ItemCount = dTotalRecords;
        }

        private void BindRepeaterDanhHieu()
        {
            Student_Student student = new Student_Student();
            Configuration_Year year = new Configuration_Year();

            student.StudentId = (int)ViewState[AppConstant.VIEWSTATE_STUDENTID];
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);

            double dTotalRecords;
            List<TabularTermStudentResult> lstTbDanhHieu;
            lstTbDanhHieu = studyingResultBL.GetTabularTermStudentResults(student, year,
                DataPagerDanhHieu.CurrentIndex, DataPagerDanhHieu.PageSize, out dTotalRecords);

            RptDanhHieu.DataSource = lstTbDanhHieu;
            RptDanhHieu.DataBind();
            DataPagerDanhHieu.ItemCount = dTotalRecords;

            bool bDisplayed = (lstTbDanhHieu.Count != 0) ? true : false;
            RptDanhHieu.Visible = bDisplayed;
        }
        #endregion

        #region Repeater event handlers
        protected void RptKetQuaDiem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Student_TermSubjectMark termSubjectedMark = null;
            if (e.Item.ItemType == ListItemType.Item
                || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Control control = e.Item.FindControl("HdfMaDiemMonHK");
                if (control != null)
                {
                    termSubjectedMark = new Student_TermSubjectMark();
                    termSubjectedMark.TermSubjectMarkId = Int32.Parse(((HiddenField)control).Value);
                    List<StrDiemMonHocLoaiDiem> lstStrDiemMonHocLoaiDiem;
                    lstStrDiemMonHocLoaiDiem = studyingResultBL.GetSubjectMarks(termSubjectedMark);
                    Repeater rptDiemMonHoc = (Repeater)e.Item.FindControl("RptDiemTheoMonHoc");
                    rptDiemMonHoc.DataSource = lstStrDiemMonHocLoaiDiem;
                    rptDiemMonHoc.DataBind();
                }
            }
        }

        protected void RptStudentFunctions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdfPhysicalPath = (HiddenField)e.Item.FindControl("HdfPhysicalPath");
                if (hdfPhysicalPath.Value == Request.Path)
                {
                    LinkButton lkBtnStudentPage = (LinkButton)e.Item.FindControl("LkBtnStudentPage");
                    lkBtnStudentPage.Style.Add(HtmlTextWriterStyle.TextDecoration, "none");
                }
            }
        }

        protected void RptStudentFunctions_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Redirect":
                    {
                        Student_Student student = new Student_Student();
                        student.StudentId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_STUDENTID].ToString());
                        AddSession(AppConstant.SESSION_STUDENT, student);

                        Configuration_Year year = new Configuration_Year();
                        year.YearId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_STUDENTID].ToString());
                        AddSession(AppConstant.SESSION_SELECTED_YEAR, year);

                        Category_Faculty faculty = new Category_Faculty();
                        faculty.FacultyId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTY].ToString());
                        AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);

                        Category_Grade grade = new Category_Grade();
                        grade.GradeId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_GRADE].ToString());
                        AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);

                        Class_Class Class = new Class_Class();
                        Class.ClassId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS].ToString());
                        AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);

                        String strStudentName = ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTNAME].ToString();
                        AddSession(AppConstant.SESSION_SELECTED_STUDENTNAME, strStudentName);

                        String strStudentCode = ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTCODE].ToString();
                        AddSession(AppConstant.SESSION_SELECTED_STUDENTCODE, strStudentCode);

                        Class_Class studentClass = new Class_Class();
                        studentClass.ClassId = (int)ViewState[AppConstant.VIEWSTATE_STUDENTCLASS_ID];
                        AddSession(AppConstant.SESSION_STUDENTCLASS, studentClass);

                        Response.Redirect((string)e.CommandArgument);
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
            MainDataPager.CurrentIndex = 1;
            BindRptKetQuaDiem();
            BindRepeaterDanhHieu();
        }

        protected void BtnBackPrevPage_Click(object sender, ImageClickEventArgs e)
        {
            Configuration_Year year = new Configuration_Year();
            year.YearId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_STUDENTID].ToString());
            AddSession(AppConstant.SESSION_SELECTED_YEAR, year);

            Category_Faculty faculty = new Category_Faculty();
            faculty.FacultyId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTY].ToString());
            AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);

            Category_Grade grade = new Category_Grade();
            grade.GradeId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_GRADE].ToString());
            AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);

            Class_Class Class = new Class_Class();
            Class.ClassId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS].ToString());
            AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);

            String strStudentName = ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTNAME].ToString();
            AddSession(AppConstant.SESSION_SELECTED_STUDENTNAME, strStudentName);

            String strStudentCode = ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTCODE].ToString();
            AddSession(AppConstant.SESSION_SELECTED_STUDENTCODE, strStudentCode);

            Response.Redirect(AppConstant.PAGEPATH_STUDENTS);
        }
        #endregion

        #region DataPager event handlers
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currentPageIndex;
            BindRptKetQuaDiem();
        }
        #endregion
    }
}