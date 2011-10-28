using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using SoLienLacTrucTuyen.BusinessLogic;
using System.Text;
using System.Security.Cryptography;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class DangNhap : System.Web.UI.Page
    {
        #region Page event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            Site masterPage = (Site)Page.Master;
            masterPage.PageTitle = "Đăng nhập";

            masterPage.SetPageTitleVisibility(false);
        }
        #endregion

        #region Login event handlers
        protected void LoginCtrl_Authenticate(object sender, AuthenticateEventArgs e)
        {
            if (ValidateUser(LoginCtrl.UserName, LoginCtrl.Password))
            {
                e.Authenticated = true;
            }
            else
            {
                e.Authenticated = false;
            }
        }

        protected void LoginCtrl_LoginError(object sender, EventArgs e)
        {
            LoginCtrl.FailureText = (string)GetGlobalResourceObject(
                "AccountResource", "LoginFailText");
        }
        #endregion

        #region Methods
        private bool ValidateUser(string userName, string password)
        {
            if (Membership.ValidateUser(userName, password))
            {
                // Validate custom information
                // ...
                //
                UserBL userBL = new UserBL();
                return userBL.ValidateUser(User.Identity.Name);
            }
            else
            {
                return false;
            }

        }
        #endregion
    }
}