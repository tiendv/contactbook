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

namespace SoLienLacTrucTuyen_WebRole.ModuleParents
{
    public partial class DetailedParentsCommentPage : BaseContentPage
    {
        private ParentsCommentBL parentsCommentBL;

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
                    Response.Redirect(AppConstant.PAGEPATH_PARENTS_COMMENT_LIST);
                }
            }
        }

        private void FillParentsComment()
        {
            ParentComment_Comment comment = parentsCommentBL.GetParentsComments((int)ViewState[AppConstant.VIEWSTATE_PARENTSCOMMENTID]);
            LblTitle.Text = comment.Title;
            LblContent.Text = comment.CommentContent;
            LblFeedback.Text = comment.Feedback;

            if (comment.Feedback.Trim() == "")
            {
                BtnEdit.Visible = true;
            }
            else
            {
                BtnEdit.Visible = false;
            }
        }

        #region Button event handlers
        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            int iCommentId = (int)ViewState[AppConstant.VIEWSTATE_PARENTSCOMMENTID];
            ParentComment_Comment comment = parentsCommentBL.GetParentsComments(iCommentId);
            AddSession(AppConstant.SESSION_PARENTSCOMMENTID, comment);

            Response.Redirect(AppConstant.PAGEPATH_PARENTS_COMMENT_EDIT);
        }

        protected void BtnBackPrevPage_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(AppConstant.PAGEPATH_PARENTS_COMMENT_LIST);
        }
        #endregion
    }
}