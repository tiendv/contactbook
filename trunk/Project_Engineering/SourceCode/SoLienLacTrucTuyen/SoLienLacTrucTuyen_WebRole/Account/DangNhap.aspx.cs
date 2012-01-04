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
            // Make actual UserName
            DropDownList ddlSchools = (DropDownList)LoginCtrl.FindControl("DdlSchools");
            int iSltSchool = Int32.Parse(ddlSchools.SelectedValue);
            LoginCtrl.UserName = iSltSchool + AppConstant.UNDERSCORE + LoginCtrl.UserName;

            if (ValidateUser(LoginCtrl.UserName, LoginCtrl.Password))
            {
                e.Authenticated = true;

                // add School to session
                School_School school = new School_School();
                school.SchoolId = iSltSchool;
                school.SchoolName = ddlSchools.SelectedItem.Text;
                Session[AppConstant.SCHOOL] = school;

                aspnet_Role role = (new UserBL(school)).GetRole(LoginCtrl.UserName);
                AuthorizationBL authorizationBL = new AuthorizationBL(school);
                if (authorizationBL.IsRoleParents(role))
                {
                    StudentBL studentBL = new StudentBL(school);
                    //string strStudentCode = LoginCtrl.UserName.Split(AppConstant.UNDERSCORE_CHAR)[1];
                    string strStudentCode = LoginCtrl.UserName.Substring(4);
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

        protected void LoginCtrl_OnLoggedIn(object sender, EventArgs e)
        {
            aspnet_Role role = (new UserBL()).GetRole(LoginCtrl.UserName);
            string strDefaultPage = role.UserManagement_RoleDetail.UserManagement_RoleCategory.UserManagementPagePath.PhysicalPath;
            Response.Redirect(strDefaultPage);
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
            List<ConfigurationProvince> provinces = systemConfigBL.GetProvinces();
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
                ConfigurationProvince province = new ConfigurationProvince();
                province.ProvinceId = Int32.Parse(DdlProvinces.SelectedValue);

                if (DdlProvinces.SelectedIndex >= 1)
                {
                    List<ConfigurationDistrict> districts = systemConfigBL.GetDistricts(province);
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
            ConfigurationProvince province = null;
            ConfigurationDistrict district = null;
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
                    district = new ConfigurationDistrict();
                    district.DistrictId = Int32.Parse(DdlDistricts.SelectedValue);
                    schools = schoolBL.GetSchools(district);
                }
                else
                {
                    province = new ConfigurationProvince();
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