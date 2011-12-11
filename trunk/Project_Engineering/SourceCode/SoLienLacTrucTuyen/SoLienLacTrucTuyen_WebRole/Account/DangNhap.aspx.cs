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
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class LoginPage : System.Web.UI.Page
    {
        #region Page event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            Site masterPage = (Site)Page.Master;
            masterPage.PageTitle = "Đăng nhập";

            masterPage.SetPageTitleVisibility(false);

            if (!Page.IsPostBack)
            {
                BindDDLSchools();
            }
        }

        private void BindDDLSchools()
        {
            SchoolBL schoolBL = new SchoolBL();
            List<School_School> schools = schoolBL.GetSchools();

            DropDownList ddlSchools = (DropDownList)LoginCtrl.FindControl("DdlSchools");
            ddlSchools.DataSource = schools;
            ddlSchools.DataTextField = "SchoolName";
            ddlSchools.DataValueField = "SchoolID";
            ddlSchools.DataBind();
        }
        #endregion

        #region Login event handlers
        protected void LoginCtrl_Authenticate(object sender, AuthenticateEventArgs e)
        {
            // Make actual UserName
            DropDownList ddlSchools = (DropDownList)LoginCtrl.FindControl("DdlSchools");
            int iSltSchool = Int32.Parse(ddlSchools.SelectedValue);
            LoginCtrl.UserName = iSltSchool + AppConstant.UNDERSCORE + LoginCtrl.UserName;

            if (ValidateUser(LoginCtrl.UserName, LoginCtrl.Password))
            {
                e.Authenticated = true;
                School_School school = new School_School();
                school.SchoolId = iSltSchool;
                school.SchoolName = ddlSchools.SelectedItem.Text;

                Session[AppConstant.SCHOOL] = school;

                aspnet_Role role = (new UserBL(school)).GetRole(LoginCtrl.UserName);
                AuthorizationBL authorizationBL = new AuthorizationBL(school);
                if (authorizationBL.IsRoleParents(role))
                {
                    StudentBL studentBL = new StudentBL(school);
                    string strStudentCode = LoginCtrl.UserName.Split(AppConstant.UNDERSCORE_CHAR)[1];
                    string strMembershipStudentSessionKey = LoginCtrl.UserName
                        + AppConstant.UNDERSCORE + AppConstant.SESSION_MEMBERSHIP_STUDENT;
                    Session[strMembershipStudentSessionKey] = studentBL.GetStudent(strStudentCode);
                }
            }
            else
            {
                LoginCtrl.UserName = LoginCtrl.UserName.Split(AppConstant.UNDERSCORE_CHAR)[1];
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