using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;
using System.Web.Security;
using System.Text;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class AddUserPage : BaseContentPage
    {
        #region Fields
        private RoleBL roleBL;
        private UserBL userBL;
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
            if (isAccessDenied)
            {
                return;
            }

            userBL = new UserBL(UserSchool);
            roleBL = new RoleBL(UserSchool);

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

        private void BackPrevPage()
        {
            Response.Redirect(AppConstant.PAGEPATH_USERS);
        }
        #endregion

        #region DropDownList event handlers
        protected void DdlRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            Display();
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
            lblStepError.Text = ""; // default

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
                lblUserName.Text = "Mã học sinh:";
                multiViewCtrl.ActiveViewIndex = 1;
                Repeater rptRoleBasedFunctions = (Repeater)multiViewCtrl.FindControl("RptRoleBasedFunctions");
                rptRoleBasedFunctions.DataSource = authorizationBL.GetListRoleParentsBasedFunctions();
                rptRoleBasedFunctions.DataBind();

                reqValidatorRealName.Enabled = false;
                htmlTableRowRealName.Style.Add(HtmlTextWriterStyle.Display, AppConstant.CSSSTYLE_DISPLAY_NONE);
                htmlTableRowPeriod.Style.Add(HtmlTextWriterStyle.Display, AppConstant.CSSSTYLE_DISPLAY_BLOCK);
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
            UserManagement_Function function = null;

            Repeater rptRoleParentsFunctions = (Repeater)SeleteRoleStep.FindControl("RptRoleBasedFunctions");
            foreach (RepeaterItem rptItem in rptRoleParentsFunctions.Items)
            {
                if (rptItem.ItemType == ListItemType.Item || rptItem.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox ChkBxSelectedFunction = (CheckBox)rptItem.FindControl("ChkBxSelectedFunction");
                    if (ChkBxSelectedFunction.Checked)
                    {
                        function = new UserManagement_Function();
                        function.FunctionId = Int32.Parse(((HiddenField)rptItem.FindControl("HdfFunctionId")).Value);
                        functions.Add(function);
                    }
                }
            }

            if (functions.Count != 0)
            {
                AddSession(AppConstant.SESSION_SELECTEDPARENTSFUNCTION, functions);
            }
        }
        #endregion

        #region CreateUserWizard event handlers
        protected void RegisterUserWizard_CreatingUser(object sender, LoginCancelEventArgs e)
        {
            // Make actual UserName
            StringBuilder strB = new StringBuilder();
            strB.Append(UserSchool.SchoolId);
            strB.Append("_");
            strB.Append(RegisterUserWizard.UserName);
            RegisterUserWizard.UserName = strB.ToString();
        }

        protected void RegisterUserWizard_CreatedUser(object sender, EventArgs e)
        {
            AuthorizationBL authorizationBL = new AuthorizationBL(UserSchool);

            // assign role to new created user
            aspnet_Role role = new aspnet_Role();
            role.RoleId = SeletedRoleId;
            string userName = RegisterUserWizard.UserName;
            authorizationBL.AddUserToRole(userName, role);

            // update user's membership
            aspnet_User createdUser = new aspnet_User();
            createdUser.UserName = userName;
            bool isTeacher = false;
            if ((bool)ViewState[VIEWSTATE_ISCHOSEROLETEACHERS])
            {
                isTeacher = true;
            }
            Control container = CreateUserStep.ContentTemplateContainer;
            String strRealName = ((TextBox)container.FindControl(STEP_CREATEUSER_TXT_REALNAME)).Text;
            userBL.UpdateMembership(createdUser, isTeacher, strRealName, RegisterUserWizard.Email);

            if ((bool)ViewState[VIEWSTATE_ISCHOSEROLEPARENTS])
            {
                if (CheckSessionKey(AppConstant.SESSION_SELECTEDPARENTSFUNCTION))
                {
                    List<UserManagement_Function> functions = (List<UserManagement_Function>)GetSession(
                        AppConstant.SESSION_SELECTEDPARENTSFUNCTION);

                    authorizationBL.AddServicesToParentsUser(createdUser, functions);
                }                
            }

            RemoveSession(AppConstant.SESSION_SELECTEDPARENTSFUNCTION);
        }

        protected void RegisterUserWizard_ContinueButtonClick(object sender, ImageClickEventArgs e)
        {
            BackPrevPage();
        }
        #endregion
    }
}