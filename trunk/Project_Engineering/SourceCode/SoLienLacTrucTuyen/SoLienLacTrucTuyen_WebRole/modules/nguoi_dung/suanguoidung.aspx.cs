using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class SuaNguoiDung : System.Web.UI.Page
    {
        #region Fields
        private UserBL userBL;
        private RoleBL roleBL;
        private bool isSearch;
        #endregion 

        protected void Page_Load(object sender, EventArgs e)
        {
            userBL = new UserBL();
            roleBL = new RoleBL();

            string pageUrl = Page.Request.Path;
            Guid role = userBL.GetRoleId(User.Identity.Name);

            if (!roleBL.ValidateAuthorization(role, pageUrl))
            {
                Response.Redirect("/Modules/ErrorPage/AccessDenied.aspx");
                return;
            }

            Site masterPage = (Site)Page.Master;
            masterPage.UserRole = role;
            masterPage.PageUrl = pageUrl;
        }
    }
}