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
        public School UserSchool
        {
            get
            {
                School school = null;
                if (Session[AppConstant.SCHOOL] != null)
                {
                    school = (School)Session[AppConstant.SCHOOL];
                }

                return school;
            }

            set
            {
                Session[AppConstant.SCHOOL] = value;
            }
        }

        #region Fields
        protected List<SoLienLacTrucTuyen.BusinessEntity.AccessibilityEnum> lstAccessibilities;
        protected bool isAccessDenied = false;
        #endregion

        #region Page event handlers
        protected virtual void Page_Load(object sender, EventArgs e)
        {
            UserSchool = (School)Session[AppConstant.SCHOOL];
            UserBL userBL = new UserBL(UserSchool);
            RoleBL roleBL = new RoleBL(UserSchool);
            AuthorizationBL authorizationBL = new AuthorizationBL(UserSchool);

            string pageUrl = Page.Request.Path;
            List<aspnet_Role> roles = userBL.GetRoles(User.Identity.Name);

            Guid role = userBL.GetRoleId(User.Identity.Name);

            if (!authorizationBL.ValidateAuthorization(roles, pageUrl))
            {
                Response.Redirect((string)GetGlobalResourceObject("MainResource", "AccessDeniedPageUrl"));
                isAccessDenied = false;
                return;
            }

            Site masterPage = (Site)Page.Master;
            masterPage.UserRole = role;
            masterPage.PageUrl = pageUrl;

            lstAccessibilities = authorizationBL.GetAccessibilities(role, pageUrl);

            if (Session[AppConstant.SCHOOL] != null)
            {
                UserSchool = (School)Session[AppConstant.SCHOOL];
            }
        }
        #endregion
    }
}