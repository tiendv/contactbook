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
                if (FillStudentInfo())
                {
                    BindRptStudentMarks();
                }
                else
                {
                    Response.Redirect(AppConstant.PAGEPATH_STUDENT_MARK);
                }
            }
        }
        #endregion

        #region Methods
        private void BindRptStudentMarks()
        {
            List<TabularTermSubjectMark> tabularTermSubjectMarks = new List<TabularTermSubjectMark>();

            // declare variables
            List<Category_MarkType> markTypes = (List<Category_MarkType>)GetSession(AppConstant.SESSION_SELECTED_MARKTYPES);
            RemoveSession(AppConstant.SESSION_SELECTED_MARKTYPES);

            Category_Subject subject = new Category_Subject();
            subject.SubjectId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_SUBJECTID];

            Class_Class Class = new Class_Class();
            Class.ClassId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID];

            Configuration_Term term = new Configuration_Term();
            term.TermId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_TERMID];

            Student_Student student = new Student_Student();
            student.StudentId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTID];
            
            tabularTermSubjectMarks = studyingResultBL.GetTabularTermSubjectMarks(student, markTypes, subject, Class, term);
            this.RptDiemMonHoc.DataSource = tabularTermSubjectMarks;
            this.RptDiemMonHoc.DataBind();
            MainDataPager.ItemCount = tabularTermSubjectMarks.Count;

        }

        private bool FillStudentInfo()
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
                RemoveSession(AppConstant.SESSION_SELECTED_STUDENT);
                ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTID] = student.StudentId;
                LblStudentName.Text = student.FullName;
                LblStudentCode.Text = student.StudentCode;

                List<Category_MarkType> markTypes = (List<Category_MarkType>)GetSession(AppConstant.SESSION_SELECTED_MARKTYPES);

                Category_Subject subject = (Category_Subject)GetSession(AppConstant.SESSION_SELECTED_SUBJECT);
                RemoveSession(AppConstant.SESSION_SELECTED_SUBJECT);
                ViewState[AppConstant.VIEWSTATE_SELECTED_SUBJECTID] = subject.SubjectId;
                LblSubjectName.Text = subject.SubjectName;

                Class_Class Class = (Class_Class)GetSession(AppConstant.SESSION_SELECTED_CLASS);
                RemoveSession(AppConstant.SESSION_SELECTED_CLASS);
                ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID] = Class.ClassId;
                LblClassName.Text = Class.ClassName;

                Configuration_Term term = (Configuration_Term)GetSession(AppConstant.SESSION_SELECTED_TERM);
                RemoveSession(AppConstant.SESSION_SELECTED_TERM);
                ViewState[AppConstant.VIEWSTATE_SELECTED_TERMID] = term.TermId; 
                LblTermName.Text = term.TermName;

                Configuration_Year year = (Configuration_Year)GetSession(AppConstant.SESSION_SELECTED_YEAR);
                RemoveSession(AppConstant.SESSION_SELECTED_YEAR); 
                ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID] = year.YearId;                
                LblYearName.Text = year.YearName;

                Category_Faculty faculty = (Category_Faculty)GetSession(AppConstant.SESSION_SELECTED_FACULTY);
                RemoveSession(AppConstant.SESSION_SELECTED_FACULTY); 
                ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTYID] = faculty.FacultyId;                

                ViewState[AppConstant.VIEWSTATE_SELECTED_GRADEID] = ((Category_Grade)GetSession(AppConstant.SESSION_SELECTED_GRADE)).GradeId;
                RemoveSession(AppConstant.SESSION_SELECTED_GRADE);

                Category_MarkType markType = (Category_MarkType)GetSession(AppConstant.SESSION_SELECTED_MARKTYPE);
                RemoveSession(AppConstant.SESSION_SELECTED_MARKTYPE);
                ViewState[AppConstant.VIEWSTATE_SELECTED_MARKTYPEID] = markType.MarkTypeId;
                LblMarkTypeName.Text = markType.MarkTypeName;

                return true;
            }

            return false;
        }

        private void RedirectToPrevPage()
        {
            //Configuration_Year year = new Configuration_Year();
            //year.YearId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID];
            //Category_Faculty faculty = new Category_Faculty();
            //faculty.FacultyId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTYID];
            //Category_Grade grade = new Category_Grade();
            //grade.GradeId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_GRADEID];
            //Class_Class Class = new Class_Class();
            //Class.ClassId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID];
            //Category_Subject subject = new Category_Subject();
            //subject.SubjectId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_SUBJECTID];
            //Configuration_Term term = new Configuration_Term();
            //term.TermId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_TERMID];
            //Category_MarkType markType = new Category_MarkType();
            //markType.MarkTypeId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_MARKTYPEID];

            //AddSession(AppConstant.SESSION_SELECTED_YEAR, year);
            //AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);            
            //AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);            
            //AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);
            //AddSession(AppConstant.SESSION_SELECTED_TERM, term);
            //AddSession(AppConstant.SESSION_SELECTED_SUBJECT, subject);
            //AddSession(AppConstant.SESSION_SELECTED_MARKTYPE, markType);

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
            Class.ClassId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID];
            term.TermId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_TERMID];
            subject.SubjectId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_SUBJECTID];

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
                studyingResultBL.UpdateDetailedMarks(student, Class, term, subject, tabularTermSubjectMarks);
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