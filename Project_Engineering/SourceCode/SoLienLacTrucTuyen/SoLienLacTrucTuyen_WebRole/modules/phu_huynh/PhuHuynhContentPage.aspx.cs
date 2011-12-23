using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;
using SoLienLacTrucTuyen_WebRole.Modules;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.ModuleParents
{
    public abstract class BaseContentPage : System.Web.UI.Page
    {
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

        public Student_Student LoggedInStudent
        {
            get
            {
                Student_Student loggedInStudent = null;
                string strLoggedInStudentSessionKey = User.Identity.Name
                        + AppConstant.UNDERSCORE + AppConstant.SESSION_MEMBERSHIP_STUDENT;

                if (Session[strLoggedInStudentSessionKey] != null)
                {
                    loggedInStudent = (Student_Student)Session[strLoggedInStudentSessionKey];
                }

                return loggedInStudent;
            }

            set
            {
                string stLoggedInStudentSessionKey = User.Identity.Name
                        + AppConstant.UNDERSCORE + AppConstant.SESSION_MEMBERSHIP_STUDENT;

                Session[stLoggedInStudentSessionKey] = value;
            }
        }

        #region Fields
        protected List<AccessibilityEnum> accessibilities;
        protected bool accessDenied = false;
        protected bool sessionExpired;
        #endregion

        #region Page event handlers
        protected virtual void Page_Load(object sender, EventArgs e)
        {
            if (Session[AppConstant.SCHOOL] == null)
            {
                sessionExpired = true;
                return;
            }

            UserSchool = (School_School)Session[AppConstant.SCHOOL];
            UserBL userBL = new UserBL(UserSchool);
            AuthorizationBL authorizationBL = new AuthorizationBL(UserSchool);

            string pageUrl = Page.Request.Path;
            List<aspnet_Role> roles = userBL.GetRoles(User.Identity.Name);

            if (!authorizationBL.ValidateAuthorization(User.Identity.Name, pageUrl))
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
    }
}