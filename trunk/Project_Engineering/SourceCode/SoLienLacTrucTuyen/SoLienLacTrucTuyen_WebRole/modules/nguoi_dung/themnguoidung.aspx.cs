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
        private StudentBL studentBL;

        public string SeletedRole
        {
            get
            {
                if (ViewState["SeletedRole"] != null)
                {
                    return ViewState["SeletedRole"].ToString();
                }
                else
                {
                    return "";
                }
            }

            set
            {
                ViewState["SeletedRole"] = value;
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
            studentBL = new StudentBL(UserSchool);
            roleBL = new RoleBL(UserSchool);
            if (!Page.IsPostBack)
            {
                BindDDLRoles();

                DropDownList DdlRoles = (DropDownList)SeleteRoleStep.FindControl("DdlRoles");
                if (DdlRoles.Items.Count != 0)
                {
                    SeletedRole = DdlRoles.Items[0].Value;
                    ProcessUI();
                }
            }
        }
        #endregion

        #region Methods
        private void BindDDLRoles()
        {
            List<aspnet_Role> roles = roleBL.GetRolesForAddingUser();

            DropDownList ddlRoles = (DropDownList)SeleteRoleStep.FindControl("DdlRoles");
            ddlRoles.DataSource = roles;
            ddlRoles.DataValueField = "RoleName";
            ddlRoles.DataTextField = "RoleName";
            ddlRoles.DataBind();
            ddlRoles.Items.Add(new ListItem("Giáo viên", "Giáo viên"));
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
            string strSltRoleName = ((DropDownList)SeleteRoleStep.FindControl("DdlRoles")).SelectedValue;
            SeletedRole = strSltRoleName;

            ImageButton NextButton = (ImageButton)RegisterUserWizard.FindControl("StartNavigationTemplateContainerID").FindControl("StartNextButton");
            NextButton.Visible = true;
            Label lblStepError = (Label)SeleteRoleStep.FindControl("LblStepError");
            lblStepError.Text = "";

            Control container = CreateUserStep.ContentTemplateContainer;
            ((Label)container.FindControl("LblSelectedRole")).Text = SeletedRole;
            ((Label)container.FindControl("LblTenNguoiDung")).Text = "Tên người dùng:";
            ((RequiredFieldValidator)container.FindControl("RealNameRequired")).Enabled = true;
            ((HtmlTableRow)container.FindControl("HtmlTrThoiHan")).Style.Add(HtmlTextWriterStyle.Display, "none");
            ((HtmlTableRow)container.FindControl("HtmlTrTenThat")).Style.Add(HtmlTextWriterStyle.Display, "block");

            ViewState["SeletedRoleParents"] = false;

            MultiView multiViewCtrl = (MultiView)SeleteRoleStep.FindControl("MultiViewCtrl");
            multiViewCtrl.ActiveViewIndex = 0;

            if (roleBL.IsRolePARENTS(strSltRoleName))
            {
                multiViewCtrl.ActiveViewIndex = 1;
                Repeater rptRoleBasedFunctions = (Repeater)multiViewCtrl.FindControl("RptRoleBasedFunctions");
                rptRoleBasedFunctions.DataSource = roleBL.GetListRoleParentsBasedFunctions();
                rptRoleBasedFunctions.DataBind();

                ((Label)container.FindControl("LblTenNguoiDung")).Text = "Mã học sinh:";
                ((RequiredFieldValidator)container.FindControl("RealNameRequired")).Enabled = false;
                ((HtmlTableRow)container.FindControl("HtmlTrThoiHan")).Style.Add(HtmlTextWriterStyle.Display, "block");
                ((HtmlTableRow)container.FindControl("HtmlTrTenThat")).Style.Add(HtmlTextWriterStyle.Display, "none");
                ViewState["SeletedRoleParents"] = true;
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

            string roleName = roleBL.GetChildRoleParentsByFunctions(functionIds);
            if (roleName != "")
            {
                SeletedRole = roleName;
            }

        }
        #endregion

        #region CreateUserWizard event handlers
        protected void RegisterUserWizard_CreatingUser(object sender, LoginCancelEventArgs e)
        {
            StringBuilder strB = new StringBuilder();
            strB.Append(User.Identity.Name.Split('-')[0]);
            //strB.Append(UserSchool.SchoolId.ToString()); 
            strB.Append("-");
            strB.Append(RegisterUserWizard.UserName);

            RegisterUserWizard.UserName = strB.ToString();
        }

        protected void RegisterUserWizard_CreatedUser(object sender, EventArgs e)
        {
            string userName = RegisterUserWizard.UserName;
            string roleName = SeletedRole;

            roleBL.AddUserToRole(userName, roleName);

            aspnet_User createdUser = new aspnet_User();
            createdUser.UserName = userName;
            //userBL.UpdateMembership(createdUser);
        }

        protected void RegisterUserWizard_ContinueButtonClick(object sender, ImageClickEventArgs e)
        {
            BackPrevPage();
        }
        #endregion
    }
}