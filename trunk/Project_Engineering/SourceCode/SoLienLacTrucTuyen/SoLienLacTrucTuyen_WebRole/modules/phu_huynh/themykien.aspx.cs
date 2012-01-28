using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen_WebRole.Modules;
using SoLienLacTrucTuyen.BusinessLogic;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.ModuleParents
{
    public partial class AddParentsCommentPage : BaseContentPage
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
        }
        #endregion

        #region Button event handlers
        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            string strTitle = TxtTitle.Text.Trim();
            string strContent = TxtContent.Text.Trim();

            if (strTitle == "")
            {
                RequiredTitle.IsValid = false;
                return;
            }

            if (strContent == "")
            {
                RequiredContent.IsValid = false;
                return;
            }

            parentsCommentBL.InsertParentsComment(LoggedInStudent, strTitle, strContent);

            if (this.CkbAddAfterSave.Checked)
            {
                TxtTitle.Text = "";
                TxtContent.Text = "";
            }
            else
            {
                Response.Redirect(AppConstant.PAGEPATH_PARENTS_COMMENT_LIST);
            }
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(AppConstant.PAGEPATH_PARENTS_COMMENT_LIST);
        }
        #endregion
    }
}