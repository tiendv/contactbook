using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.BusinessEntity;
using EContactBook.DataAccess;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class ModifyStudentMarkPage : BaseContentPage
    {
        #region Fields
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

            studyingResultBL = new StudyingResultBL(UserSchool);

            if (!Page.IsPostBack)
            {
                FillStudentInfo();
                BindRptStudentMarks();
            }
        }
        #endregion

        #region Methods
        private void BindRptStudentMarks()
        {
            List<TabularTermSubjectMark> tabularTermSubjectMarks = new List<TabularTermSubjectMark>();

            // declare variables
            Student_Student student = (Student_Student)GetSession(AppConstant.SESSION_SELECTED_STUDENT);
            RemoveSession(AppConstant.SESSION_SELECTED_STUDENT);

            List<Category_MarkType> markTypes = (List<Category_MarkType>)GetSession(AppConstant.SESSION_SELECTED_MARKTYPES);
            RemoveSession(AppConstant.SESSION_SELECTED_MARKTYPES);

            Category_Subject subject = new Category_Subject();
            subject.SubjectId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_SUBJECT];

            Class_Class Class = new Class_Class();
            Class.ClassId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS];

            Configuration_Term term = new Configuration_Term();
            term.TermId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_TERM];

            tabularTermSubjectMarks = studyingResultBL.GetTabularTermSubjectMarks(student, markTypes, subject, Class, term);
            this.RptDiemMonHoc.DataSource = tabularTermSubjectMarks;
            this.RptDiemMonHoc.DataBind();
            MainDataPager.ItemCount = tabularTermSubjectMarks.Count;

        }

        private void FillStudentInfo()
        {
            if (CheckSessionKey(AppConstant.SESSION_SELECTED_STUDENT)
               && CheckSessionKey(AppConstant.SESSION_SELECTED_CLASS)
               && CheckSessionKey(AppConstant.SESSION_SELECTED_TERM)
               && CheckSessionKey(AppConstant.SESSION_SELECTED_MARKTYPE)
               && CheckSessionKey(AppConstant.SESSION_SELECTED_MARKTYPES)
               && CheckSessionKey(AppConstant.SESSION_SELECTED_SUBJECT)
               && CheckSessionKey(AppConstant.SESSION_SELECTED_FACULTY)
               && CheckSessionKey(AppConstant.SESSION_SELECTED_GRADE))
            {
                Student_Student student = (Student_Student)GetSession(AppConstant.SESSION_SELECTED_STUDENT);
                ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTID] = student.StudentId;

                List<Category_MarkType> markTypes = (List<Category_MarkType>)GetSession(AppConstant.SESSION_SELECTED_MARKTYPES);

                Category_Subject subject = (Category_Subject)GetSession(AppConstant.SESSION_SELECTED_SUBJECT);
                Class_Class Class = (Class_Class)GetSession(AppConstant.SESSION_SELECTED_CLASS);

                Configuration_Term term = (Configuration_Term)GetSession(AppConstant.SESSION_SELECTED_TERM);

                LblStudentName.Text = student.FullName;
                LblStudentCode.Text = student.StudentCode;

                ViewState[AppConstant.VIEWSTATE_SELECTED_YEAR] = ((Configuration_Year)GetSession(AppConstant.SESSION_SELECTED_YEAR)).YearId;
                RemoveSession(AppConstant.SESSION_SELECTED_YEAR);

                ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTY] = ((Category_Faculty)GetSession(AppConstant.SESSION_SELECTED_FACULTY)).FacultyId;
                RemoveSession(AppConstant.SESSION_SELECTED_FACULTY);

                ViewState[AppConstant.VIEWSTATE_SELECTED_GRADE] = ((Category_Grade)GetSession(AppConstant.SESSION_SELECTED_GRADE)).GradeId;
                RemoveSession(AppConstant.SESSION_SELECTED_GRADE);

                ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS] = ((Class_Class)GetSession(AppConstant.SESSION_SELECTED_CLASS)).ClassId;
                RemoveSession(AppConstant.SESSION_SELECTED_CLASS);

                ViewState[AppConstant.VIEWSTATE_SELECTED_SUBJECT] = ((Category_Subject)GetSession(AppConstant.SESSION_SELECTED_SUBJECT)).SubjectId;
                RemoveSession(AppConstant.SESSION_SELECTED_SUBJECT);

                ViewState[AppConstant.VIEWSTATE_SELECTED_MARKTYPE] = ((Category_MarkType)GetSession(AppConstant.SESSION_SELECTED_MARKTYPE)).MarkTypeId;
                RemoveSession(AppConstant.SESSION_SELECTED_MARKTYPE);

                ViewState[AppConstant.VIEWSTATE_SELECTED_TERM] = ((Configuration_Term)GetSession(AppConstant.SESSION_SELECTED_TERM)).TermId;
                RemoveSession(AppConstant.SESSION_SELECTED_TERM);
            }
        }

        private void RedirectToPrevPage()
        {
            Configuration_Year year = new Configuration_Year();
            year.YearId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_YEAR];
            Category_Faculty faculty = new Category_Faculty();
            faculty.FacultyId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTY];
            Category_Grade grade = new Category_Grade();
            grade.GradeId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_GRADE];
            Class_Class Class = new Class_Class();
            Class.ClassId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS];
            Category_Subject subject = new Category_Subject();
            subject.SubjectId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_SUBJECT];
            Configuration_Term term = new Configuration_Term();
            term.TermId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_TERM];
            Category_MarkType markType = new Category_MarkType();
            markType.MarkTypeId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_MARKTYPE];

            AddSession(AppConstant.SESSION_SELECTED_YEAR, year);
            AddSession(AppConstant.SESSION_SELECTED_TERM, term);
            AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);
            AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);
            AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);
            AddSession(AppConstant.SESSION_SELECTED_SUBJECT, subject);
            AddSession(AppConstant.SESSION_SELECTED_MARKTYPE, markType);

            Response.Redirect(AppConstant.PAGEPATH_STUDENT_MARK);
        }
        #endregion

        #region Button event handlers
        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            List<TabularTermSubjectMark> tabularTermSubjectMarks = new List<TabularTermSubjectMark>();
            TabularTermSubjectMark tabularTermSubjectMark = null;
            TabularDetailTermSubjectMark detailTermSubjectMark = null;
            HiddenField hdfMarkTypeId = null;
            Repeater rptDiemTheoLoaiDiem = null;
            HiddenField hdfOldMarkValue = null;
            TextBox txtMarkValue = null;
            Student_Student student = new Student_Student();
            Class_Class Class = new Class_Class();
            Configuration_Term term = new Configuration_Term();
            Category_Subject subject = new Category_Subject();
            student.StudentId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTID];
            Class.ClassId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS];
            term.TermId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_TERM];
            subject.SubjectId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_SUBJECT];

            foreach (RepeaterItem item in RptDiemMonHoc.Items)
            {
                hdfMarkTypeId = (HiddenField)item.FindControl("HdfMarkTypeId");
                tabularTermSubjectMark = new TabularTermSubjectMark();
                tabularTermSubjectMark.MarkTypeId = Int32.Parse(hdfMarkTypeId.Value);

                rptDiemTheoLoaiDiem = (Repeater)item.FindControl("RptDiemTheoLoaiDiem");
                tabularTermSubjectMark.TabularDetailTermSubjectMarks = new List<TabularDetailTermSubjectMark>();
                foreach (RepeaterItem innerItem in rptDiemTheoLoaiDiem.Items)
                {
                    hdfOldMarkValue = (HiddenField)innerItem.FindControl("HdfOldMarkValue");
                    txtMarkValue = (TextBox)innerItem.FindControl("TxtMarkValue");
                    if (hdfOldMarkValue.Value != txtMarkValue.Text)
                    {
                        detailTermSubjectMark = new TabularDetailTermSubjectMark();
                        detailTermSubjectMark.DetailTermSubjectMarkId = Int32.Parse(((HiddenField)innerItem.FindControl("HdfDetailTermSubjectMarkId")).Value);
                        if (!CheckUntils.IsNullOrBlank(txtMarkValue.Text))
                        {
                            detailTermSubjectMark.MarkValue = double.Parse(txtMarkValue.Text);
                        }
                        else
                        {
                            detailTermSubjectMark.MarkValue = -1;
                        }

                        tabularTermSubjectMark.TabularDetailTermSubjectMarks.Add(detailTermSubjectMark);
                    }
                }

                if (tabularTermSubjectMark.TabularDetailTermSubjectMarks.Count != 0)
                {
                    tabularTermSubjectMarks.Add(tabularTermSubjectMark);
                }
            }

            if (tabularTermSubjectMarks.Count != 0)
            {
                studyingResultBL.UpdateDetailedMark(student, Class, term, subject, tabularTermSubjectMarks);
            }

            RedirectToPrevPage();
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            RedirectToPrevPage();
        }
        #endregion

        #region Repeater event handlers
        protected void RptDiemMonHoc_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TabularTermSubjectMark tabularTermSubjectMark = (TabularTermSubjectMark)e.Item.DataItem;
                Repeater rptMarkTypeBasedMarks = (Repeater)e.Item.FindControl("RptDiemTheoLoaiDiem");

                rptMarkTypeBasedMarks.DataSource = tabularTermSubjectMark.TabularDetailTermSubjectMarks;
                rptMarkTypeBasedMarks.DataBind();
            }
        }
        #endregion
    }
}