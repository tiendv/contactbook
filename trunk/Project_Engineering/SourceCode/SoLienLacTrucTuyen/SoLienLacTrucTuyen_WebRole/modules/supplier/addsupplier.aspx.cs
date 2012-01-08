using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class AddSchoolPage : BaseContentPage
    {
        SchoolBL schoolBL;

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (accessDenied)
            {
                return;
            }

            if (sessionExpired)
            {
                FormsAuthentication.SignOut();
                Response.Redirect(FormsAuthentication.LoginUrl);
            }

            schoolBL = new SchoolBL();

            if (!Page.IsPostBack)
            {
                BindDDLProvinces();
                BindDDLDistricts();
                GetSearchedSessions();
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            SchoolBL schoolBL = null;
            School_School lastedInsertedSchool = null;
            School_School supplier = null;

            if (ValidateInput())
            {
                ConfigurationDistrict district = new ConfigurationDistrict();
                district.DistrictId = Int32.Parse(DdlDistricts.SelectedValue);
                string strSchoolName = TxtSchoolName.Text.Trim();
                string strAddress = TxtAddress.Text.Trim();
                string strEmail = TxtEmail.Text.Trim();
                string strPhone = TxtPhone.Text.Trim();
                byte[] bLogo = FileUploadLogo.FileBytes;
                string strEmailPassword = Membership.GeneratePassword(Membership.Provider.MinRequiredPasswordLength,
                    Membership.Provider.MinRequiredNonAlphanumericCharacters);                

                // insert new school and then return it generated id
                schoolBL = new SchoolBL();
                lastedInsertedSchool = schoolBL.InsertSchool(district, strSchoolName, strAddress, strPhone, strEmail, strEmailPassword, bLogo);

                // create default information for school
                CreateSchoolDefaultInfomation(lastedInsertedSchool);

                // send confirmation mail to school
                supplier = schoolBL.GetSupplier();
                string strEmailContent = string.Format("Account: {0}, Password: {1}", lastedInsertedSchool.Email, lastedInsertedSchool.Password);
                MailBL.SendByGmail(supplier.Email, lastedInsertedSchool.Email, "Thông báo tạo thông tin trường thành công", 
                    strEmailContent, supplier.Password);

                // redirect back to previous page
                BackToPrevPage();
            }
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            BackToPrevPage();
        }
        #endregion

        #region DropDownList event hanlders
        protected void DdlProvinces_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLDistricts();
        }
        #endregion

        #region Methods
        private bool ValidateInput()
        {
            if (CheckUntils.IsNullOrBlank(DdlDistricts.Text.Trim()))
            {
                DistrictRequired.IsValid = false;
                return false;
            }

            if (CheckUntils.IsNullOrBlank(TxtSchoolName.Text))
            {
                SchoolNameRequired.IsValid = false;
                return false;
            }

            if (CheckUntils.IsNullOrBlank(TxtAddress.Text))
            {
                AddressRequired.IsValid = false;
                return false;
            }

            if (CheckUntils.IsNullOrBlank(TxtPhone.Text))
            {
                PhoneRequired.IsValid = false;
                return false;
            }

            if (CheckUntils.IsNullOrBlank(TxtEmail.Text))
            {
                EmailRequired.IsValid = false;
                return false;
            }

            if (Regex.IsMatch(FileUploadLogo.FileName, FileUpLoadValidator.ValidationExpression))
            {
                FileUpLoadValidator.IsValid = false;
                return false;
            }

            if (DdlDistricts.Items.Count != 0)
            {
                ConfigurationDistrict district = new ConfigurationDistrict();
                district.DistrictId = Int32.Parse(DdlDistricts.SelectedValue);
                if (schoolBL.SchoolNameExists(district, TxtSchoolName.Text.Trim()))
                {
                    SchoolNameCustomValidator.IsValid = false;
                    return false;
                }
                else
                {
                    SchoolNameCustomValidator.IsValid = true;
                }
            }

            if (MailBL.CheckEmailExist(TxtEmail.Text))
            {
                EmailCustomValidator.IsValid = true;
            }
            else
            {
                EmailCustomValidator.IsValid = false;
                return false;
            }

            return true;
        }

        protected void SchoolNameCustomValidator_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            if(DdlDistricts.Items.Count != 0)
            {
                ConfigurationDistrict district = new ConfigurationDistrict();
                district.DistrictId = Int32.Parse(DdlDistricts.SelectedValue);
                if (schoolBL.SchoolNameExists(district, TxtSchoolName.Text.Trim()))
                {
                    e.IsValid = false;
                }
                else
                {
                    e.IsValid = true;
                }
            }            
        }

        protected void EmailCustomValidator_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            if (MailBL.CheckEmailExist(TxtEmail.Text))
            {
                e.IsValid = true;
            }
            else
            {
                e.IsValid = false;
            }
        }

        private void BackToPrevPage()
        {
            SaveSearchedSessions();
            Response.Redirect(AppConstant.PAGEPATH_SCHOOLLIST);
        }

        private void SaveSearchedSessions()
        {
            AddSession(AppConstant.SESSION_SELECTED_PROVINCE, ViewState[AppConstant.VIEWSTATE_SELECTED_PROVINCEID]);
            AddSession(AppConstant.SESSION_SELECTED_DISTRICT, ViewState[AppConstant.VIEWSTATE_SELECTED_DISTRICTID]);
            AddSession(AppConstant.SESSION_SELECTED_SCHOOLNAME, ViewState[AppConstant.VIEWSTATE_SELECTED_SCHOOLNAME]);
        }

        private void GetSearchedSessions()
        {
            if (CheckSessionKey(AppConstant.SESSION_SELECTED_PROVINCE)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_DISTRICT)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_SCHOOLNAME))
            {
                ViewState[AppConstant.VIEWSTATE_SELECTED_PROVINCEID] = (Int32)GetSession(AppConstant.SESSION_SELECTED_PROVINCE);
                RemoveSession(AppConstant.SESSION_SELECTED_PROVINCE);

                ViewState[AppConstant.VIEWSTATE_SELECTED_DISTRICTID] = (Int32)GetSession(AppConstant.SESSION_SELECTED_DISTRICT);
                RemoveSession(AppConstant.SESSION_SELECTED_DISTRICT);

                ViewState[AppConstant.VIEWSTATE_SELECTED_SCHOOLNAME] = (string)GetSession(AppConstant.SESSION_SELECTED_SCHOOLNAME);
                RemoveSession(AppConstant.SESSION_SELECTED_SCHOOLNAME);
            }
        }

        private void BindDDLProvinces()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<ConfigurationProvince> provinces = systemConfigBL.GetProvinces();
            DdlProvinces.DataSource = provinces;
            DdlProvinces.DataValueField = "ProvinceId";
            DdlProvinces.DataTextField = "ProvinceName";
            DdlProvinces.DataBind();
        }

        private void BindDDLDistricts()
        {
            if (DdlProvinces.Items.Count != 0)
            {
                ConfigurationProvince province = new ConfigurationProvince();
                province.ProvinceId = Int32.Parse(DdlProvinces.SelectedValue);

                SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
                List<ConfigurationDistrict> districts = systemConfigBL.GetDistricts(province);
                DdlDistricts.DataSource = districts;
                DdlDistricts.DataValueField = "DistrictId";
                DdlDistricts.DataTextField = "DistrictName";
                DdlDistricts.DataBind();
            }
        }

        private byte[] ImageToByteArray(System.Drawing.Image image)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                return memoryStream.ToArray();
            }
        }

        private void CreateSchoolDefaultInfomation(School_School school)
        {
            RoleBL roleBL = null;
            UserBL userBL = null;
            AuthorizationBL authorizationBL = null;
            aspnet_Role parentRole = null;
            UserManagement_RoleCategory roleCategory = null;
            string strRoleName;
            StringBuilder strBuilder = null;

            // create school's default roles
            roleBL = new RoleBL();
            userBL = new UserBL(school);
            roleCategory = new UserManagement_RoleCategory();
            authorizationBL = new AuthorizationBL(school);
            strBuilder = new StringBuilder();

            strBuilder.Append(school.SchoolId.ToString());
            strBuilder.Append(AppConstant.UNDERSCORE);
            strBuilder.Append("Quản trị");
            strRoleName = strBuilder.ToString();
            Roles.CreateRole(strRoleName); // create role by asp.net's method
            roleCategory.RoleCategoryId = "ADMIN";
            roleBL.CreateRoleDetail("Quản trị", "", false, null, roleCategory, school);
            authorizationBL.InsertAuthorizations(roleBL.GetRole(strRoleName));

            strRoleName = school.SchoolId.ToString() + AppConstant.UNDERSCORE + "Giáo viên";
            Roles.CreateRole(strRoleName);
            roleCategory.RoleCategoryId = "TEACHER";
            roleBL.CreateRoleDetail("Giáo viên", "", false, null, roleCategory, school);
            authorizationBL.InsertAuthorizations(roleBL.GetRole(strRoleName));

            strRoleName = school.SchoolId.ToString() + AppConstant.UNDERSCORE + "Phụ huynh";
            Roles.CreateRole(strRoleName);
            roleCategory.RoleCategoryId = "PARENTS";
            roleBL.CreateRoleDetail("Phụ huynh", "", false, null, roleCategory, school);
            authorizationBL.InsertAuthorizations(roleBL.GetRole(strRoleName));

            strRoleName = school.SchoolId.ToString() + AppConstant.UNDERSCORE + "Giáo viên chủ nhiệm";
            Roles.CreateRole(strRoleName);
            roleCategory.RoleCategoryId = "FORMERTEACHER";
            parentRole = roleBL.GetRole(school.SchoolId.ToString() + AppConstant.UNDERSCORE + "Giáo viên");
            roleBL.CreateRoleDetail("Giáo viên chủ nhiệm", "", false, parentRole, roleCategory, school);
            authorizationBL.InsertAuthorizations(roleBL.GetRole(strRoleName));

            strRoleName = school.SchoolId.ToString() + AppConstant.UNDERSCORE + "Giáo viên bộ môn";
            Roles.CreateRole(strRoleName);
            roleCategory.RoleCategoryId = "SUBJECTTEACHER";
            roleBL.CreateRoleDetail("Giáo viên bộ môn", "", false, parentRole, roleCategory, school);
            authorizationBL.InsertAuthorizations(roleBL.GetRole(strRoleName));

            // create school's default user master administrator
            string strAdminUserPassword = "1qazxsw@";
            string strAdminUserName = school.SchoolId.ToString() + AppConstant.UNDERSCORE + "masteradmin";
            // create user by asp .net method
            Membership.CreateUser(strAdminUserName, strAdminUserPassword, school.Email);
            // update created membership information
            aspnet_User userAdmin = new aspnet_User();
            userAdmin.aspnet_Membership = new aspnet_Membership();
            userAdmin.aspnet_Membership.Email = school.Email;
            userAdmin.UserName = strAdminUserName;
            userBL.CreateUserMasterAdministrator(userAdmin);

            // create school's default user administrator
            strAdminUserPassword = "1qazxsw@";
            strAdminUserName = school.SchoolId.ToString() + AppConstant.UNDERSCORE + "admin";
            // create user by asp .net method
            Membership.CreateUser(strAdminUserName, strAdminUserPassword, school.Email);
            // update created membership information
            userAdmin = new aspnet_User();
            userAdmin.aspnet_Membership = new aspnet_Membership();
            userAdmin.aspnet_Membership.Email = school.Email;
            userAdmin.UserName = strAdminUserName;
            userBL.CreateUserAdministrator(userAdmin);
        }
        #endregion
    }
}