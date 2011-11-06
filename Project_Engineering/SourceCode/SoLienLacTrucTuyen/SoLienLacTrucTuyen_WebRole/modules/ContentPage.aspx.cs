using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public abstract class BaseContentPage : System.Web.UI.Page
    {
        private const string SESSION_SCHOOL = "_SESSION_SCHOOL";
        public School UserSchool
        {
            get
            {
                School school = null;
                if (Session[SESSION_SCHOOL] != null)
                {
                    school = (School)Session[User.Identity.Name + SESSION_SCHOOL];
                }

                return school;
            }
        }

        #region Fields
        protected List<SoLienLacTrucTuyen.BusinessEntity.AccessibilityEnum> lstAccessibilities;
        protected bool isAccessDenied = false;
        #endregion

        #region Page event handlers
        protected virtual void Page_Load(object sender, EventArgs e)
        {
            UserBL userBL = new UserBL();
            RoleBL roleBL = new RoleBL();

            string pageUrl = Page.Request.Path;
            List<aspnet_Role> roles = userBL.GetRoles(User.Identity.Name);

            Guid role = userBL.GetRoleId(User.Identity.Name);

            if (!roleBL.ValidateAuthorization(roles, pageUrl))
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