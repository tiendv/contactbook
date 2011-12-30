using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class AddSchoolPage : BaseContentPage
    {
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
        }

        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            BackToPrevPage();
        }

        protected void BtnSaveCancel_Click(object sender, ImageClickEventArgs e)
        {
            BackToPrevPage();
        }

        private void BackToPrevPage()
        {
            Response.Redirect(AppConstant.PAGEPATH_SCHOOLLIST);
        }

    }
}