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
using EContactBook.DataAccess;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class LoginPage : System.Web.UI.Page
    {
        #region Page event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect(AppConstant.PAGEPATH_HOMEPAGE);
            }

            Site masterPage = (Site)Page.Master;
            masterPage.PageTitle = "Đăng nhập";

            masterPage.SetPageTitleVisibility(false);

            if (!Page.IsPostBack)
            {
                BindDDLProvinces();
                BindDDLDistricts();
                BindDDLSchools();
            }
        }
        #endregion

        #region Login event handlers
        protected void LoginCtrl_Authenticate(object sender, AuthenticateEventArgs e)
        {
            SchoolBL schoolBL = new SchoolBL();
            // Make actual UserName
            DropDownList ddlSchools = (DropDownList)LoginCtrl.FindControl("DdlSchools");
            int iSltSchool = Int32.Parse(ddlSchools.SelectedValue);
            LoginCtrl.UserName = iSltSchool + AppConstant.UNDERSCORE + LoginCtrl.UserName;

            if (ValidateUser(LoginCtrl.UserName, LoginCtrl.Password))
            {
                e.Authenticated = true;

                // add School to session
                School_School school = schoolBL.GetSchool(iSltSchool);
                Session[AppConstant.SCHOOL] = school;
                SystemConfigBL systemConfigBL = new SystemConfigBL(school);
                Session[AppConstant.SESSION_CURRENT_YEAR] = systemConfigBL.GetLastedYear();
                aspnet_Role role = (new UserBL(school)).GetRole(LoginCtrl.UserName);
                AuthorizationBL authorizationBL = new AuthorizationBL(school);
                if (authorizationBL.IsRoleParents(role))
                {
                    StudentBL studentBL = new StudentBL(school);
                    string strStudentCode = LoginCtrl.UserName.Split(new char[]{AppConstant.UNDERSCORE_CHAR})[1];
                    strStudentCode = strStudentCode.Substring(2);
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

        /// <summary>
        /// Happen after user log in successfully
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoginCtrl_OnLoggedIn(object sender, EventArgs e)
        {   
            // declare BLs
            School_School school = (School_School)Session[AppConstant.SCHOOL];
            RoleBL roleBL = new RoleBL(school); 
            UserBL userBL = new UserBL(school);

            // save loged in user to session
            Session[AppConstant.SESSION_LOGEDIN_USER] = userBL.GetUser(LoginCtrl.UserName);

            // get and save loged in user's roles to session
            List<aspnet_Role> roles = userBL.GetRoles(LoginCtrl.UserName);
            Session[AppConstant.SESSION_LOGEDIN_ROLES] = roles;
            Session[AppConstant.SESSION_LOGEDIN_USER_IS_FORMERTEACHER] = roleBL.HasRoleFormerTeacher(roles);
            Session[AppConstant.SESSION_LOGEDIN_USER_IS_SUBJECTTEACHER] = roleBL.HasRoleSubjectTeacher(roles);

            // redirect to default page
            if (roles.Count != 0)
            {
                string strDefaultPage = roles[0].UserManagement_RoleDetail.UserManagement_RoleCategory.UserManagement_PagePath.PhysicalPath;
                Response.Redirect(strDefaultPage);
            }
        }
        #endregion

        #region DropDownList event hanlders
        protected void DdlProvinces_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLDistricts();
            BindDDLSchools();
        }

        protected void DdlDistricts_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLSchools();
        }
        #endregion

        #region Methods
        private bool ValidateUser(string userName, string password)
        {
            if (Membership.ValidateUser(userName, password)) // default validation of ASP.NET
            {
                // Customized validate
                UserBL userBL = new UserBL();
                return userBL.ValidateUser(userName);
            }
            else
            {
                return false;
            }
        }

        private void BindDDLProvinces()
        {
            DropDownList DdlProvinces = (DropDownList)LoginCtrl.FindControl("DdlProvinces");

            SystemConfigBL systemConfigBL = new SystemConfigBL();
            List<Configuration_Province> provinces = systemConfigBL.GetProvinces();
            DdlProvinces.DataSource = provinces;
            DdlProvinces.DataValueField = "ProvinceId";
            DdlProvinces.DataTextField = "ProvinceName";
            DdlProvinces.DataBind();

            DdlProvinces.Items.Insert(0, new ListItem("(-- Chọn tỉnh/thành --)", "0"));
        }

        private void BindDDLDistricts()
        {
            DropDownList DdlProvinces = (DropDownList)LoginCtrl.FindControl("DdlProvinces");
            DropDownList DdlDistricts = (DropDownList)LoginCtrl.FindControl("DdlDistricts");
            SystemConfigBL systemConfigBL = new SystemConfigBL();

            if (DdlProvinces.SelectedIndex >= 0)
            {
                Configuration_Province province = new Configuration_Province();
                province.ProvinceId = Int32.Parse(DdlProvinces.SelectedValue);

                if (DdlProvinces.SelectedIndex >= 1)
                {
                    List<Configuration_District> districts = systemConfigBL.GetDistricts(province);
                    DdlDistricts.DataSource = districts;
                    DdlDistricts.DataValueField = "DistrictId";
                    DdlDistricts.DataTextField = "DistrictName";
                    DdlDistricts.DataBind();
                }
                
                DdlDistricts.Items.Insert(0, new ListItem("(-- Chọn huyện/quận --)", "0"));
            }
        }

        private void BindDDLSchools()
        {
            DropDownList DdlProvinces = (DropDownList)LoginCtrl.FindControl("DdlProvinces");
            DropDownList DdlDistricts = (DropDownList)LoginCtrl.FindControl("DdlDistricts");
            DropDownList ddlSchools = (DropDownList)LoginCtrl.FindControl("DdlSchools");
            SchoolBL schoolBL = new SchoolBL();
            Configuration_Province province = null;
            Configuration_District district = null;
            List<School_School> schools = new List<School_School>();

            if (DdlProvinces.SelectedIndex == 0)
            {
                ddlSchools.Items.Clear();
                ddlSchools.Items.Insert(0, new ListItem("(-- Chọn trường --)", "0"));
            }
            else
            {
                if (DdlDistricts.SelectedIndex > 0)
                {
                    district = new Configuration_District();
                    district.DistrictId = Int32.Parse(DdlDistricts.SelectedValue);
                    schools = schoolBL.GetSchools(district);
                }
                else
                {
                    province = new Configuration_Province();
                    province.ProvinceId = Int32.Parse(DdlProvinces.SelectedValue);
                    schools = schoolBL.GetSchools(province);
                }

                
                ddlSchools.DataSource = schools;
                ddlSchools.DataTextField = "SchoolName";
                ddlSchools.DataValueField = "SchoolID";
                ddlSchools.DataBind();
            }
        }
        #endregion
    }
}