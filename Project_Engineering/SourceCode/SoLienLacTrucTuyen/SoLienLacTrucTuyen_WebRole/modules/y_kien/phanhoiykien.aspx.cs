using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen_WebRole.Modules;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class FeedbackParentsCommentPage : BaseContentPage
    {
        #region Fields
        private ParentsCommentBL parentsCommentBL;
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

            parentsCommentBL = new ParentsCommentBL(UserSchool);
            if (!Page.IsPostBack)
            {
                if (CheckSessionKey(AppConstant.SESSION_PARENTSCOMMENTID))
                {
                    ParentComment_Comment comment = (ParentComment_Comment)GetSession(AppConstant.SESSION_PARENTSCOMMENTID);
                    RemoveSession(AppConstant.VIEWSTATE_PARENTSCOMMENTID);
                    ViewState[AppConstant.VIEWSTATE_PARENTSCOMMENTID] = comment.CommentId;

                    FillParentsComment();
                }
                else
                {
                    Response.Redirect(AppConstant.PAGEPATH_PARENTSCOMMENTS);
                }
            }
        }
        #endregion

        #region Methods
        private void FillParentsComment()
        {
            ParentComment_Comment comment = parentsCommentBL.GetParentsComments((int)ViewState[AppConstant.VIEWSTATE_PARENTSCOMMENTID]);
            LblTitle.Text = comment.Title;
            LblContent.Text = comment.CommentContent;
            TxtFeedback.Text = comment.Feedback;

            Student_Student student = comment.Student_StudentInClass.Student_Student;
            LblStudentInfo.Text = string.Format("Góp ý của phụ huynh học sinh {0} (mã học sinh: {1}, lớp: {2})",
                comment.Student_StudentInClass.Student_Student.FullName,
                comment.Student_StudentInClass.Student_Student.StudentCode,
                comment.Student_StudentInClass.Class_Class.ClassName);

            LblDate.Text = comment.Date.ToShortDateString();
        }
        #endregion

        #region Button event handlers
        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            ParentComment_Comment comment = new ParentComment_Comment(); 
            string strFeedback = TxtFeedback.Text.Trim();

            if (CheckUntils.IsNullOrBlank(strFeedback))
            {
                RequiredFeedback.IsValid = false;
                return;
            }
            
            comment.CommentId = (int)ViewState[AppConstant.VIEWSTATE_PARENTSCOMMENTID];
            parentsCommentBL.Feedback(comment, strFeedback);

            // redirect to page [Góp Ý]
            Response.Redirect(AppConstant.PAGEPATH_PARENTSCOMMENTS);
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(AppConstant.PAGEPATH_PARENTSCOMMENTS);
        }
        #endregion
    }
}