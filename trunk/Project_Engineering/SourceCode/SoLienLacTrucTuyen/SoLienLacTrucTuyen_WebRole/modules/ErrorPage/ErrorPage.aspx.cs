using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class ErrorPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Site masterPage = (Site)Page.Master;
            masterPage.PageTitle = "Lỗi";

            FormsAuthentication.SignOut();
            Response.Redirect(AppConstant.PAGEPATH_SIGNIN);
        }
    }
}