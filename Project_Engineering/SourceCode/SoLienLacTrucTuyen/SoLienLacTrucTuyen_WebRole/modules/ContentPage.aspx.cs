using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public abstract class BaseContentPage : System.Web.UI.Page
    {
        #region Properties
        public School_School UserSchool
        {
            get
            {
                School_School school = null;
                if (Session[AppConstant.SCHOOL] != null)
                {
                    school = (School_School)Session[AppConstant.SCHOOL];
                }

                return school;
            }

            set
            {
                Session[AppConstant.SCHOOL] = value;
            }
        }

        public Configuration_Year CurrentYear
        {
            get
            {
                Configuration_Year currentYear = null;
                if (Session[AppConstant.SESSION_CURRENT_YEAR] != null)
                {
                    currentYear = (Configuration_Year)Session[AppConstant.SESSION_CURRENT_YEAR];
                }

                return currentYear;
            }

            set
            {
                Session[AppConstant.SESSION_CURRENT_YEAR] = value;
            }
        }

        public List<aspnet_Role> LogedInUserRoles
        {
            get
            {
                List<aspnet_Role> roles = new List<aspnet_Role>();
                if (Session[AppConstant.SESSION_LOGEDIN_ROLES] != null)
                {
                    roles = (List<aspnet_Role>)Session[AppConstant.SESSION_LOGEDIN_ROLES];
                }

                return roles;
            }
        }

        public aspnet_User LogedInUser
        {
            get
            {
                aspnet_User logedInUser = null;
                if (Session[AppConstant.SESSION_LOGEDIN_USER] != null)
                {
                    logedInUser = (aspnet_User)Session[AppConstant.SESSION_LOGEDIN_USER];
                }

                return logedInUser;
            }
        }

        public bool IsFormerTeacher
        {
            get
            {
                bool bIsFormerTeacher = false;
                if (Session[AppConstant.SESSION_LOGEDIN_USER_IS_FORMERTEACHER] != null)
                {
                    bIsFormerTeacher = (bool)Session[AppConstant.SESSION_LOGEDIN_USER_IS_FORMERTEACHER];
                }

                return bIsFormerTeacher;
            }
        }

        public bool IsSubjectTeacher
        {
            get
            {
                bool bIsSubjectTeacher = false;
                if (Session[AppConstant.SESSION_LOGEDIN_USER_IS_SUBJECTTEACHER] != null)
                {
                    bIsSubjectTeacher = (bool)Session[AppConstant.SESSION_LOGEDIN_USER_IS_SUBJECTTEACHER];
                }

                return bIsSubjectTeacher;
            }
        }
        #endregion

        #region Fields
        protected List<AccessibilityEnum> accessibilities;
        protected bool accessDenied = false;
        protected bool sessionExpired = false;
        #endregion

        #region Page event handlers
        protected virtual void Page_Load(object sender, EventArgs e)
        {
            if(Session[AppConstant.SCHOOL] == null)
            {
                sessionExpired = true;
                return;
            }

            UserSchool = (School_School)Session[AppConstant.SCHOOL];
            UserBL userBL = new UserBL(UserSchool);
            AuthorizationBL authorizationBL = new AuthorizationBL(UserSchool);

            string pageUrl = Page.Request.Path;

            //List<aspnet_Role> roles = userBL.GetRoles(User.Identity.Name);
            List<aspnet_Role> roles = LogedInUserRoles;

            if (!authorizationBL.ValidateAuthorization(roles, pageUrl))
            {
                Response.Redirect(GetText("AccessDeniedPageUrl"));
                accessDenied = false;
                return;
            }

            Site masterPage = (Site)Page.Master;
            masterPage.UserName = User.Identity.Name;
            masterPage.UserRole = roles[0];
            masterPage.PageUrl = pageUrl;
            masterPage.UserSchool = UserSchool;

            accessibilities = authorizationBL.GetAccessibilities(roles, pageUrl);
        }
        #endregion

        #region Method(s)
        protected void AddSession(string key, object value)
        {
            Session.Add(UserSchool.SchoolId + AppConstant.UNDERSCORE + key, value);
        }

        protected void RemoveSession(string key)
        {
            if (CheckSessionKey(key))
            {
                Session.Remove(UserSchool.SchoolId + AppConstant.UNDERSCORE + key);
            }
        }

        protected object GetSession(string key)
        {
            if (CheckSessionKey(key))
            {
                return Session[UserSchool.SchoolId + AppConstant.UNDERSCORE + key];
            }
            else
            {
                return null;
            }
        }

        protected bool CheckSessionKey(string key)
        {
            if (Session[UserSchool.SchoolId + AppConstant.UNDERSCORE + key] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected string GetText(string key)
        {
            return (string)GetGlobalResourceObject("MainResource", key);
        }
        #endregion
    }
}