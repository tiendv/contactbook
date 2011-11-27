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
        private const string STEP_SELECTEDROLE_DDLROLE = "DdlRoles";
        private const string STEP_SELECTEDROLE_LBLSTEPERROR = "LblStepError";
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

                DropDownList DdlRoles = (DropDownList)SeleteRoleStep.FindControl(STEP_SELECTEDROLE_DDLROLE);
                if (DdlRoles.Items.Count != 0)
                {
                    ProcessUI();
                }
            }
        }
        #endregion

        #region Methods
        private void BindDDLRoles()
        {
            List<TabularRole> roles = roleBL.GetTabularRoles();
            DropDownList ddlRoles = (DropDownList)SeleteRoleStep.FindControl(STEP_SELECTEDROLE_DDLROLE);
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
            ProcessUI();
        }

        private void ProcessUI()
        {
            AuthorizationBL authorizationBL = new AuthorizationBL(UserSchool);
            aspnet_Role role = null;
            Guid roleId = new Guid(((DropDownList)SeleteRoleStep.FindControl(STEP_SELECTEDROLE_DDLROLE)).SelectedValue);
            SeletedRoleId = roleId;
            role = new aspnet_Role();
            role.RoleId = roleId;

            string strRoleName = ((DropDownList)SeleteRoleStep.FindControl(STEP_SELECTEDROLE_DDLROLE)).SelectedItem.Text;

            ImageButton NextButton = (ImageButton)RegisterUserWizard.FindControl("StartNavigationTemplateContainerID").FindControl("StartNextButton");
            NextButton.Visible = true; // default
            Label lblStepError = (Label)SeleteRoleStep.FindControl(STEP_SELECTEDROLE_LBLSTEPERROR);
            lblStepError.Text = ""; // default

            Control container = CreateUserStep.ContentTemplateContainer;
            ((Label)container.FindControl("LblSelectedRole")).Text = strRoleName;
            Label lblUserName = ((Label)container.FindControl("LblTenNguoiDung"));
            ((RequiredFieldValidator)container.FindControl("RealNameRequired")).Enabled = true;
            ((HtmlTableRow)container.FindControl("HtmlTrThoiHan")).Style.Add(HtmlTextWriterStyle.Display, "none");
            ((HtmlTableRow)container.FindControl("HtmlTrTenThat")).Style.Add(HtmlTextWriterStyle.Display, "block");

            MultiView multiViewCtrl = (MultiView)SeleteRoleStep.FindControl("MultiViewCtrl");
            multiViewCtrl.ActiveViewIndex = 0;

            if (authorizationBL.IsRolePARENTS(role))
            {
                lblUserName.Text = "Mã học sinh:";
                multiViewCtrl.ActiveViewIndex = 1;
                Repeater rptRoleBasedFunctions = (Repeater)multiViewCtrl.FindControl("RptRoleBasedFunctions");
                rptRoleBasedFunctions.DataSource = authorizationBL.GetListRoleParentsBasedFunctions();
                rptRoleBasedFunctions.DataBind();

                ((RequiredFieldValidator)container.FindControl("RealNameRequired")).Enabled = false;
                ((HtmlTableRow)container.FindControl("HtmlTrThoiHan")).Style.Add(HtmlTextWriterStyle.Display, AppConstant.CSSSTYLE_DISPLAY_BLOCK);
                ((HtmlTableRow)container.FindControl("HtmlTrTenThat")).Style.Add(HtmlTextWriterStyle.Display, AppConstant.CSSSTYLE_DISPLAY_NONE);
                ViewState[VIEWSTATE_ISCHOSEROLEPARENTS] = true;
                ViewState[VIEWSTATE_ISCHOSEROLETEACHERS] = false;
            }
            else
            {
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
            List<int> functionIds = new List<int>();
            Repeater rptRoleBasedFunctions = (Repeater)SeleteRoleStep.FindControl("RptRoleBasedFunctions");
            foreach (RepeaterItem rptItem in rptRoleBasedFunctions.Items)
            {
                if (rptItem.ItemType == ListItemType.Item || rptItem.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox ChkBxSelectedFunction = (CheckBox)rptItem.FindControl("ChkBxSelectedFunction");
                    if (ChkBxSelectedFunction.Checked)
                    {
                        HiddenField HdfFunctionId = (HiddenField)rptItem.FindControl("HdfFunctionId");
                        functionIds.Add(Int32.Parse(HdfFunctionId.Value));
                    }
                }
            }

            //string roleName = roleBL.GetChildRoleParentsByFunctions(functionIds);
            //if (roleName != "")
            //{
            //    SeletedRoleId = roleName;
            //}

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
            userBL.UpdateMembership(createdUser, isTeacher);
        }

        protected void RegisterUserWizard_ContinueButtonClick(object sender, ImageClickEventArgs e)
        {
            BackPrevPage();
        }
        #endregion
    }
}