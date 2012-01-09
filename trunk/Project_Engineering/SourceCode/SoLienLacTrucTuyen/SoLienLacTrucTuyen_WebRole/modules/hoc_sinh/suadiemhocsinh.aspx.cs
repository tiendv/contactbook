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
            if (CheckSessionKey(AppConstant.SESSION_SELECTED_STUDENT)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_CLASS)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_TERM)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_MARKTYPES)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_SUBJECT))
            {
                Student_Student student = (Student_Student)GetSession(AppConstant.SESSION_SELECTED_STUDENT);
                List<Category_MarkType> markTypes = (List<Category_MarkType>)GetSession(AppConstant.SESSION_SELECTED_MARKTYPES);
                Category_Subject subject = (Category_Subject)GetSession(AppConstant.SESSION_SELECTED_SUBJECT);
                Class_Class Class = (Class_Class)GetSession(AppConstant.SESSION_SELECTED_CLASS);
                Configuration_Term term = (Configuration_Term)GetSession(AppConstant.SESSION_SELECTED_TERM);

                tabularTermSubjectMarks = studyingResultBL.GetTabularTermSubjectMarks(student, markTypes, subject, Class, term);
                this.RptDiemMonHoc.DataSource = tabularTermSubjectMarks;
                this.RptDiemMonHoc.DataBind();
                MainDataPager.ItemCount = tabularTermSubjectMarks.Count;
            }
        }

        private void FillStudentInfo()
        {
            if (CheckSessionKey(AppConstant.SESSION_SELECTED_STUDENT)
               && CheckSessionKey(AppConstant.SESSION_SELECTED_CLASS)
               && CheckSessionKey(AppConstant.SESSION_SELECTED_TERM)
               && CheckSessionKey(AppConstant.SESSION_SELECTED_MARKTYPES)
               && CheckSessionKey(AppConstant.SESSION_SELECTED_SUBJECT))
            {
                Student_Student student = (Student_Student)GetSession(AppConstant.SESSION_SELECTED_STUDENT);
                List<Category_MarkType> markTypes = (List<Category_MarkType>)GetSession(AppConstant.SESSION_SELECTED_MARKTYPES);
                Category_Subject subject = (Category_Subject)GetSession(AppConstant.SESSION_SELECTED_SUBJECT);
                Class_Class Class = (Class_Class)GetSession(AppConstant.SESSION_SELECTED_CLASS);
                Configuration_Term term = (Configuration_Term)GetSession(AppConstant.SESSION_SELECTED_TERM);

                LblStudentName.Text = student.FullName;
                LblStudentCode.Text = student.StudentCode;
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(AppConstant.PAGEPATH_STUDENT_MARK);
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(AppConstant.PAGEPATH_STUDENT_MARK);
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