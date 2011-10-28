using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public abstract class BaseContentPage : System.Web.UI.Page
    {
        #region Fields
        protected List<SoLienLacTrucTuyen.BusinessEntity.AccessibilityEnum> lstAccessibilities;
        protected bool isAccessDenied = false;
        #endregion

        #region Page event handlers
        protected virtual void Page_Load(object sender, EventArgs e)
        {
            SoLienLacTrucTuyen.BusinessLogic.UserBL userBL = new SoLienLacTrucTuyen.BusinessLogic.UserBL();
            SoLienLacTrucTuyen.BusinessLogic.RoleBL roleBL = new SoLienLacTrucTuyen.BusinessLogic.RoleBL();

            string pageUrl = Page.Request.Path;
            Guid role = userBL.GetRoleId(User.Identity.Name);

            if (!roleBL.ValidateAuthorization(role, pageUrl))
            {
                Response.Redirect((string)GetGlobalResourceObject("MainResource", "AccessDeniedPageUrl"));
                isAccessDenied = false;
                return;
            }

            Site masterPage = (Site)Page.Master;
            masterPage.UserRole = role;
            masterPage.PageUrl = pageUrl;

            lstAccessibilities = roleBL.GetAccessibilities(role, pageUrl);
        }
        #endregion
    }
}