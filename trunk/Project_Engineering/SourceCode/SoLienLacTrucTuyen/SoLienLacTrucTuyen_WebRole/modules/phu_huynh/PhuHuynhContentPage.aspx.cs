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
                Student_Student membershipStudent = null;

                if (User.Identity.IsAuthenticated)
                {
                    string strMembershipStudentSessionKey = User.Identity.Name
                        + AppConstant.UNDERSCORE + AppConstant.SESSION_MEMBERSHIP_STUDENT;
                    if (Session[strMembershipStudentSessionKey] != null)
                    {
                        membershipStudent = (Student_Student)Session[strMembershipStudentSessionKey];
                    }
                }

                return membershipStudent;
            }

            set
            {
                if (User.Identity.IsAuthenticated)
                {
                    Session[User.Identity.Name + AppConstant.UNDERSCORE + AppConstant.SESSION_MEMBERSHIP_STUDENT] = value;
                }
            }
        }

        #region Fields
        protected List<AccessibilityEnum> accessibilities;
        protected bool accessDenied = false;
        #endregion

        #region Page event handlers
        protected virtual void Page_Load(object sender, EventArgs e)
        {
            UserSchool = (School_School)Session[AppConstant.SCHOOL];
            UserBL userBL = new UserBL(UserSchool);
            RoleBL roleBL = new RoleBL(UserSchool);
            AuthorizationBL authorizationBL = new AuthorizationBL(UserSchool);

            string pageUrl = Page.Request.Path;
            List<aspnet_Role> roles = userBL.GetRoles(User.Identity.Name);
            aspnet_Role role = roles[0];

            if (!authorizationBL.ValidateAuthorization(User.Identity.Name, pageUrl))
            {
                Response.Redirect((string)GetGlobalResourceObject("MainResource", "AccessDeniedPageUrl"));
                accessDenied = false;
                return;
            }

            Site masterPage = (Site)Page.Master;
            masterPage.UserName = User.Identity.Name;
            masterPage.UserRole = role;
            masterPage.PageUrl = pageUrl;
            masterPage.UserSchool = UserSchool;

            accessibilities = authorizationBL.GetAccessibilities(roles, pageUrl);

            if (Session[AppConstant.SCHOOL] != null)
            {
                UserSchool = (School_School)Session[AppConstant.SCHOOL];
            }
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
    }
}