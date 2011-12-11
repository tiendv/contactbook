using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen_WebRole.Modules;
using SoLienLacTrucTuyen.BusinessLogic;

namespace SoLienLacTrucTuyen_WebRole.ModuleParents
{
    public partial class EditParentsCommentPage : BaseContentPage
    {
        private ParentsCommentBL parentsCommentBL;

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (accessDenied)
            {
                return;
            }

            parentsCommentBL = new ParentsCommentBL(UserSchool);
        }

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
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(AppConstant.PAGEPATH_COMMENTS);
        }
        #endregion
    }
}