using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class AuthenticationPage : BaseContentPage
    {
        #region Fields
        AuthorizationBL authorizationBL;
        private RoleBL roleBL;
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

            authorizationBL = new AuthorizationBL(UserSchool);
            roleBL = new RoleBL(UserSchool);

            if (!Page.IsPostBack)
            {
                BindDDLRoles();
                BindRptAuthorizations();
            }
        }
        #endregion

        #region Repeater event handlers
        protected void RptPhanQuyen_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            FunctionsBL functionBL = null;

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.DataItem != null)
                {
                    // Bind DetailedAuthorization
                    TabularAuthorization tabularAuthorization = (TabularAuthorization)e.Item.DataItem;
                    Repeater rptDetailedAuthorization = (Repeater)e.Item.FindControl("RptChiTietPhanQuyen");
                    rptDetailedAuthorization.DataSource = tabularAuthorization.detailedAuthorizations;
                    rptDetailedAuthorization.DataBind();

                    // Make HomePage' Authorizations are invisible
                    functionBL = new FunctionsBL();        
                    if (tabularAuthorization.FunctionCategoryName == functionBL.GetHomePageFunctionCategory())
                    {
                        foreach (RepeaterItem detailedAuthorization in rptDetailedAuthorization.Items)
                        {
                            if (detailedAuthorization.ItemType == ListItemType.Item || detailedAuthorization.ItemType == ListItemType.AlternatingItem)
                            {
                                CheckBox ckbxView = (CheckBox)detailedAuthorization.FindControl("CkbxView");
                                ckbxView.Enabled = false;

                                detailedAuthorization.FindControl("CkbxAdd").Visible = false;
                                detailedAuthorization.FindControl("CkbxModify").Visible = false;
                                detailedAuthorization.FindControl("CkbxDelete").Visible = false;
                            }
                        }
                    }

                    aspnet_Role role = new aspnet_Role();
                    role.RoleId = new Guid(DdlRoles.SelectedValue.ToString());
                    if (authorizationBL.IsRoleAdmin(role))
                    {
                        if (functionBL.GetAdminOnlyFunctionCategories().Contains(tabularAuthorization.FunctionCategoryName))
                        {
                            foreach (RepeaterItem item in rptDetailedAuthorization.Items)
                            {
                                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                                {
                                    CheckBox ckbxView = (CheckBox)item.FindControl("CkbxView");
                                    ckbxView.Enabled = false;
                                    CheckBox ckbxAdd = (CheckBox)item.FindControl("CkbxAdd");
                                    ckbxAdd.Enabled = false;
                                    CheckBox ckbxModify = (CheckBox)item.FindControl("CkbxModify");
                                    ckbxModify.Enabled = false;
                                    CheckBox ckbxDelete = (CheckBox)item.FindControl("CkbxDelete");
                                    ckbxDelete.Enabled = false;
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            BindRptAuthorizations();
        }
        
        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            aspnet_Role role = null;            
            List<TabularDetailedAuthorization> detailedAuthorizations = new List<TabularDetailedAuthorization>();
            foreach (RepeaterItem rptItemAuthorization in RptPhanQuyen.Items)
            {
                if (rptItemAuthorization.ItemType == ListItemType.Item || rptItemAuthorization.ItemType == ListItemType.AlternatingItem)
                {
                    Repeater RptDetailedAuthorization = (Repeater)rptItemAuthorization.FindControl("RptChiTietPhanQuyen");
                    foreach (RepeaterItem rptItemDetailedAuthorization in RptDetailedAuthorization.Items)
                    {
                        if (rptItemDetailedAuthorization.ItemType == ListItemType.Item || rptItemDetailedAuthorization.ItemType == ListItemType.AlternatingItem)
                        {
                            TabularDetailedAuthorization detailedAuthorization = new TabularDetailedAuthorization();

                            HiddenField hfFunctionId = (HiddenField)rptItemDetailedAuthorization.FindControl("HfFunctionId");
                            detailedAuthorization.FunctionId = Int32.Parse(hfFunctionId.Value);

                            CheckBox CkbxView = (CheckBox)rptItemDetailedAuthorization.FindControl("CkbxView");
                            detailedAuthorization.ViewAccessibility = CkbxView.Checked;

                            CheckBox CkbxAdd = (CheckBox)rptItemDetailedAuthorization.FindControl("CkbxAdd");
                            detailedAuthorization.AddAccessibility = CkbxAdd.Checked;

                            CheckBox CkbxModify = (CheckBox)rptItemDetailedAuthorization.FindControl("CkbxModify");
                            detailedAuthorization.ModifyAccessibility = CkbxModify.Checked;

                            CheckBox CkbxDelete = (CheckBox)rptItemDetailedAuthorization.FindControl("CkbxDelete");
                            detailedAuthorization.DeleteAccessibility = CkbxDelete.Checked;

                            detailedAuthorizations.Add(detailedAuthorization);
                        }
                    }
                }
            }

            role = new aspnet_Role();
            role.RoleId = new Guid(DdlRoles.SelectedValue); 
            authorizationBL.Authorize(role, detailedAuthorizations);
            BindRptAuthorizations();            
        }
        #endregion

        #region Methods
        private void BindDDLRoles()
        {
            List<TabularRole> tabularRoles = authorizationBL.GetAuthorizedRoles();
            DdlRoles.DataSource = tabularRoles;
            DdlRoles.DataValueField = "RoleId";
            DdlRoles.DataTextField = "DisplayedName";
            DdlRoles.DataBind();
        }

        private void BindRptAuthorizations()
        {
            aspnet_Role role = new aspnet_Role();
            role.RoleId = new Guid(DdlRoles.SelectedValue.ToString());

            List<TabularAuthorization> tabularAuthorizations;
            tabularAuthorizations = authorizationBL.GetTabularAuthorizations(role);
            RptPhanQuyen.DataSource = tabularAuthorizations;
            RptPhanQuyen.DataBind();
        }
        #endregion
    }
}