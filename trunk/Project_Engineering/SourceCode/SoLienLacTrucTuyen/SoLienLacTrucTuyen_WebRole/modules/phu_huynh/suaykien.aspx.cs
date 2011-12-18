﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen_WebRole.Modules;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;

namespace SoLienLacTrucTuyen_WebRole.ModuleParents
{
    public partial class EditParentsCommentPage : BaseContentPage
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
                    Response.Redirect(AppConstant.PAGEPATH_COMMENTS);
                }
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            string strContent = TxtContent.Text.Trim();

            if (strContent == "")
            {
                RequiredContent.IsValid = false;
                return;
            }

            ParentComment_Comment comment = new ParentComment_Comment();
            comment.CommentId = (int)ViewState[AppConstant.VIEWSTATE_PARENTSCOMMENTID];
            parentsCommentBL.UpdateParentsComment(comment, strContent);

            Response.Redirect(AppConstant.PAGEPATH_COMMENTS);
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(AppConstant.PAGEPATH_COMMENTS);
        }
        #endregion

        private void FillParentsComment()
        {
            ParentComment_Comment comment = parentsCommentBL.GetParentsComments((int)ViewState[AppConstant.VIEWSTATE_PARENTSCOMMENTID]);
            LblTitle.Text = comment.Title;
            TxtContent.Text = comment.CommentContent;
        }
    }
}