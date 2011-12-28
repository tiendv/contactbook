using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;
using System.Web.Security;
using System.Text;
using System.Web.Configuration;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class AddUserPage : BaseContentPage
    {
        #region Fields
        private RoleBL roleBL;
        private UserBL userBL;
        private AuthorizationBL authorizationBL;
        private const string STEP_SELECTROLE_DDLROLE = "DdlRoles";
        private const string STEP_SELECTROLE_LBLSTEPERROR = "LblStepError";
        private const string STEP_SELECTROLE_MULVIEW = "MultiViewCtrl";
        private const string STARTNEXTBUTTON = "StartNextButton";
        private const string STEP_CREATEUSER_LBLROLE = "LblSelectedRole";
        private const string STEP_CREATEUSER_LBLUSERNAME = "LblTenNguoiDung";
        private const string STEP_CREATEUSER_VALIDATORREALNAME = "RealNameRequired";
        private const string STEP_CREATEUSER_TR_PERIOD = "HtmlTrThoiHan";
        private const string STEP_CREATEUSER_TR_REALNAME = "HtmlTrTenThat";
        private const string STEP_CREATEUSER_TXT_REALNAME = "TxtTenThat";

        private const string VIEWSTATE_ISCHOSEROLEPARENTS = "IsChoseRoleParents";
        private const string VIEWSTATE_ISCHOSEROLETEACHERS = "IsChoseRoleTeachers";

        public Guid SeletedRoleId
        {
            get
            {
                Guid roleId = new Guid();
                if (ViewState[AppConstant.VIEWSTATE_ROLE] != null)
                {
                    roleId = new Guid(ViewState[AppConstant.VIEWSTATE_ROLE].ToString());
                }

                return roleId;
            }

            set
            {
                ViewState[AppConstant.VIEWSTATE_ROLE] = value;
            }
        }
        #endregion

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

            userBL = new UserBL(UserSchool);
            roleBL = new RoleBL(UserSchool);
            authorizationBL = new AuthorizationBL(UserSchool);

            if (!Page.IsPostBack)
            {
                BindDDLRoles();

                DropDownList DdlRoles = (DropDownList)SeleteRoleStep.FindControl(STEP_SELECTROLE_DDLROLE);
                if (DdlRoles.Items.Count != 0)
                {
                    Display();
                }
            }
        }
        #endregion

        #region Methods
        private void BindDDLRoles()
        {
            List<TabularRole> roles = roleBL.GetTabularRoles();
            DropDownList ddlRoles = (DropDownList)SeleteRoleStep.FindControl(STEP_SELECTROLE_DDLROLE);
            ddlRoles.DataSource = roles;
            ddlRoles.DataValueField = "RoleId";
            ddlRoles.DataTextField = "DisplayedName";
            ddlRoles.DataBind();

            SeletedRoleId = new Guid(DdlRoles.Items[0].Value);
        }

        protected void ValidateUserName(object source, ServerValidateEventArgs args)
        {
            string password = Membership.GeneratePassword(8, 2);

            StringBuilder strB = new StringBuilder();
            strB.Append(UserSchool.SchoolId);
            strB.Append("_");
            strB.Append(RegisterUserWizard.UserName);
            MembershipUserCollection users = Membership.FindUsersByName(strB.ToString());
            if (users.Count != 0)
            {
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
        }

        protected void ValidateStudentCode(object source, ServerValidateEventArgs args)
        {
            StudentBL studentBL = null;
            if ((bool)ViewState[VIEWSTATE_ISCHOSEROLEPARENTS])
            {
                studentBL = new StudentBL(UserSchool);
                args.IsValid = studentBL.StudentCodeExists(RegisterUserWizard.UserName);
                CustomValidator userNameCustomValidator = ((CustomValidator)CreateUserStep.ContentTemplateContainer.FindControl("UserNameCustomValidator"));
                userNameCustomValidator.IsValid = true;
            }
            else
            {
                args.IsValid = true;
            }
        }

        private void BackPrevPage()
        {
            Response.Redirect(AppConstant.PAGEPATH_USERS);
        }

        private void Display()
        {
            AuthorizationBL authorizationBL = new AuthorizationBL(UserSchool);
            aspnet_Role role = new aspnet_Role();
            role.RoleId = new Guid(((DropDownList)SeleteRoleStep.FindControl(STEP_SELECTROLE_DDLROLE)).SelectedValue);
            role.RoleName = ((DropDownList)SeleteRoleStep.FindControl(STEP_SELECTROLE_DDLROLE)).SelectedItem.Text;
            SeletedRoleId = role.RoleId;

            ImageButton NextButton = (ImageButton)RegisterUserWizard.FindControl(
                "StartNavigationTemplateContainerID").FindControl(STARTNEXTBUTTON);
            NextButton.Visible = true; // default
            Label lblStepError = (Label)SeleteRoleStep.FindControl(STEP_SELECTROLE_LBLSTEPERROR);
            lblStepError.Text = "Chưa chọn dịch vụ"; // default
            lblStepError.Visible = false;

            Control container = CreateUserStep.ContentTemplateContainer;
            ((Label)container.FindControl(STEP_CREATEUSER_LBLROLE)).Text = role.RoleName;
            Label lblUserName = ((Label)container.FindControl(STEP_CREATEUSER_LBLUSERNAME));
            RequiredFieldValidator reqValidatorRealName = ((RequiredFieldValidator)container.FindControl(STEP_CREATEUSER_VALIDATORREALNAME));
            HtmlTableRow htmlTableRowPeriod = ((HtmlTableRow)container.FindControl(STEP_CREATEUSER_TR_PERIOD));
            HtmlTableRow htmlTableRowRealName = ((HtmlTableRow)container.FindControl(STEP_CREATEUSER_TR_REALNAME));
            MultiView multiViewCtrl = (MultiView)SeleteRoleStep.FindControl(STEP_SELECTROLE_MULVIEW);
            multiViewCtrl.ActiveViewIndex = 0;

            if (authorizationBL.IsRoleParents(role))
            {
                List<UserManagement_Authorization> authorizations = authorizationBL.GetSupliedRoleParentsAuthorizations();
                if (authorizations.Count != 0)
                {
                    List<UserManagement_Function> functions = authorizationBL.GetSupliedRoleParentsFunctions(authorizations);
                    Repeater rptRoleBasedFunctions = (Repeater)multiViewCtrl.FindControl("RptRoleBasedFunctions");
                    rptRoleBasedFunctions.DataSource = functions;
                    rptRoleBasedFunctions.DataBind();
                    AddSession(AppConstant.SESSION_SUPPLIEDPARENTSAUTHORIZATIONS, authorizations);
                }

                lblUserName.Text = "Mã học sinh:";
                multiViewCtrl.ActiveViewIndex = 1;


                reqValidatorRealName.Enabled = false;
                htmlTableRowRealName.Style.Add(HtmlTextWriterStyle.Display, AppConstant.CSSSTYLE_DISPLAY_NONE);
                htmlTableRowPeriod.Style.Add(HtmlTextWriterStyle.Display, AppConstant.CSSSTYLE_DISPLAY_BLOCK);
                Label lblLastedYearName = ((Label)container.FindControl("LblLastedYearName"));
                SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
                lblLastedYearName.Text = systemConfigBL.GetLastedYear().YearName;

                ViewState[VIEWSTATE_ISCHOSEROLEPARENTS] = true;
                ViewState[VIEWSTATE_ISCHOSEROLETEACHERS] = false;
            }
            else
            {
                reqValidatorRealName.Enabled = true;
                htmlTableRowRealName.Style.Add(HtmlTextWriterStyle.Display, AppConstant.CSSSTYLE_DISPLAY_BLOCK);
                htmlTableRowPeriod.Style.Add(HtmlTextWriterStyle.Display, AppConstant.CSSSTYLE_DISPLAY_NONE);

                ViewState[VIEWSTATE_ISCHOSEROLEPARENTS] = false;

                if (authorizationBL.IsRoleTeachers(role))
                {
                    lblUserName.Text = "Mã giáo viên:";
                    ViewState[VIEWSTATE_ISCHOSEROLETEACHERS] = true;
                }
                else
                {
                    lblUserName.Text = "Tên người dùng:";
                    ViewState[VIEWSTATE_ISCHOSEROLETEACHERS] = false;
                }
            }
        }
        #endregion

        #region DropDownList event handlers
        protected void DdlRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            Display();
        }

        
        #endregion

        #region Button event handlers
        protected void BtnFinish_Click(object sender, ImageClickEventArgs e)
        {
            BackPrevPage();
        }

        protected void BtnNext_Click(object sender, ImageClickEventArgs e)
        {   
            /*
             * 1. Loop in repeater RptRoleBasedFunctions
             *      1.1. Determine wheather CheckBox is checked
             *          1.1.1. Add checked function to list
             *      1.2. Continue loop
             * 2. Save checked function list to session
             * */

            List<UserManagement_Function> functions = new List<UserManagement_Function>();
            List<ChoseService> choseServices = new List<ChoseService>();
            ChoseService choseService = null;
            bool bChoseService = false;
            Repeater rptRoleParentsFunctions = (Repeater)SeleteRoleStep.FindControl("RptRoleBasedFunctions");
            foreach (RepeaterItem rptItem in rptRoleParentsFunctions.Items)
            {
                if (rptItem.ItemType == ListItemType.Item || rptItem.ItemType == ListItemType.AlternatingItem)
                {
                    choseService = new ChoseService();
                    choseService.FunctionId = Int32.Parse(((HiddenField)rptItem.FindControl("HdfFunctionId")).Value);

                    CheckBox ChkBxSelectedFunction = (CheckBox)rptItem.FindControl("ChkBxSelectedFunction");
                    if (ChkBxSelectedFunction.Checked)
                    {
                        bChoseService = true;
                        choseService.Chose = true;
                        CheckBox ChkBxGetEmail = (CheckBox)rptItem.FindControl("ChkBxGetEmail");
                        CheckBox ChkBxGetSMS = (CheckBox)rptItem.FindControl("ChkBxGetSMS");
                        choseService.GetEmail = ChkBxGetEmail.Checked;
                        choseService.GetSMS = ChkBxGetSMS.Checked;
                    }
                    else
                    {
                        choseService.Chose = false;
                        choseService.GetEmail = false;
                        choseService.GetSMS = false;
                    }

                    choseServices.Add(choseService);
                }
            }

            if (bChoseService)
            {
                HdfHasChoseService.Value = "true"; 
                if (choseServices.Count != 0)
                {
                    AddSession(AppConstant.SESSION_SELECTEDPARENTSFUNCTIONS, choseServices);
                }
            }
            else
            {
                HdfHasChoseService.Value = "false";
            }
        }
        #endregion

        #region CreateUserWizard event handlers
        /// <summary>
        /// Process before create new user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RegisterUserWizard_CreatingUser(object sender, LoginCancelEventArgs e)
        {
            // Make actual RegisterUserWizard.UserName
            StringBuilder strB = new StringBuilder();
            strB.Append(UserSchool.SchoolId);
            strB.Append(AppConstant.UNDERSCORE);
            strB.Append(RegisterUserWizard.UserName);
            RegisterUserWizard.UserName = strB.ToString();

            string strGeneratedPassword = Membership.GeneratePassword(Membership.Provider.MinRequiredPasswordLength,
                Membership.Provider.MinRequiredNonAlphanumericCharacters);
            TextBox txtPassword = (TextBox)CreateUserStep.ContentTemplateContainer.FindControl("Password");
            txtPassword.Text = strGeneratedPassword;
            TextBox txtConfirmPassword = (TextBox)CreateUserStep.ContentTemplateContainer.FindControl("ConfirmPassword");
            txtConfirmPassword.Text = strGeneratedPassword;
        }

        /// <summary>
        /// Process after create user successfully
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RegisterUserWizard_CreatedUser(object sender, EventArgs e)
        {
            // Declare viables
            AuthorizationBL authorizationBL = null;
            aspnet_Role role = null;
            aspnet_User createdUser = null;
            List<ChoseService> choseServices = null;
            List<UserManagement_Authorization> authorizations = null;

            // assign role to new created user            
            role = new aspnet_Role();
            role.RoleId = SeletedRoleId;
            authorizationBL = new AuthorizationBL(UserSchool);
            authorizationBL.AddUserToRole(RegisterUserWizard.UserName, role);

            // update user's membership
            createdUser = new aspnet_User();
            createdUser.UserName = RegisterUserWizard.UserName;
            String strFullName = ((TextBox)CreateUserStep.ContentTemplateContainer.FindControl(STEP_CREATEUSER_TXT_REALNAME)).Text;
            userBL.UpdateMembership(createdUser, (bool)ViewState[VIEWSTATE_ISCHOSEROLETEACHERS], strFullName, RegisterUserWizard.Email);

            // Create User Parents' Authorization
            if ((bool)ViewState[VIEWSTATE_ISCHOSEROLEPARENTS])
            {
                if (CheckSessionKey(AppConstant.SESSION_SELECTEDPARENTSFUNCTIONS))
                {
                    choseServices = (List<ChoseService>)GetSession(AppConstant.SESSION_SELECTEDPARENTSFUNCTIONS);
                    authorizations = (List<UserManagement_Authorization>)GetSession(AppConstant.SESSION_SUPPLIEDPARENTSAUTHORIZATIONS);

                    // Add registerd services
                    authorizationBL.AddParentsUserRegisteredServices(createdUser, authorizations, choseServices);
                }
            }

            // remove unused sessions
            RemoveSession(AppConstant.SESSION_SUPPLIEDPARENTSAUTHORIZATIONS);
            RemoveSession(AppConstant.SESSION_SELECTEDPARENTSFUNCTIONS);

            MailBL.SendByGmail("duyna1989@gmail.com", RegisterUserWizard.Email, 
                "Tạo tài khoản thành công", 
                string.Format("pass:{0}", RegisterUserWizard.Password), 
                "duyna1989", "1qazxsw@");

            SchoolBL schoolBL = new SchoolBL();
            //schoolBL.UpdateTotalOfUser(UserSchool);
        }

        protected void RegisterUserWizard_ContinueButtonClick(object sender, ImageClickEventArgs e)
        {
            BackPrevPage();
        }
        #endregion
    }
}