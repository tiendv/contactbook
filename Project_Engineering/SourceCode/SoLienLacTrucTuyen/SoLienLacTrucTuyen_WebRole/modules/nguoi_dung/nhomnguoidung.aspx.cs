using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using EContactBook.BusinessEntity;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using AjaxControlToolkit;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class RolesPage : BaseContentPage
    {
        #region Fields
        private RoleBL roleBL;
        private bool isSearch;
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

            roleBL = new RoleBL(UserSchool);

            if (!Page.IsPostBack)
            {
                ProcPermissions();

                isSearch = false;
                MainDataPager.CurrentIndex = 1;
                BindRptRoles();
            }
        }

        private void ProcPermissions()
        {
            if (!accessibilities.Contains(AccessibilityEnum.Add))
            {
                this.BtnAddRole.Visible = false;
                this.MPEAdd.Enabled = false;
                this.PnlPopupAdd.Visible = false;
            }
        }
        #endregion

        #region Methods
        public void BindRptRoles()
        {
            string strRoleName = TxtSearchNhomNguoiDung.Text.Trim();
            double dTotalRecords;
            List<TabularRole> tabularRoles = roleBL.GetTabularRoles(strRoleName,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            // Decrease page current index when delete
            if (tabularRoles.Count == 0 && dTotalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRptRoles();
                return;
            }

            bool bDisplayData = (tabularRoles.Count != 0) ? true : false;
            RptRoles.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = string.Format(
                        (string)GetGlobalResourceObject("MainResource", "LblSearchResultText"),
                        "nhóm người dùng");
                }
                else
                {
                    LblSearchResult.Text = string.Format(
                        (string)GetGlobalResourceObject("MainResource", "LblSearchResultSearchText"),
                        "nhóm người dùng");
                }

                MainDataPager.CurrentIndex = 1;
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
            }
            else
            {
                MainDataPager.Visible = true;
            }

            RptRoles.DataSource = tabularRoles;
            RptRoles.DataBind();
            MainDataPager.ItemCount = dTotalRecords;
        }
        #endregion

        #region Button click event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            MainDataPager.ItemCount = 0;
            isSearch = true;
            BindRptRoles();
        }

        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {
            string strActualRoleName = "";
            string strRoleName = this.TxtRoleNameAdd.Text.Trim();
            string strDescription = this.TxtRoleDescriptionAdd.Text.Trim();

            if (Page.IsValid)
            {
                if (strRoleName == "" || strRoleName == "*")
                {
                    RoleNameRequiredAdd.IsValid = false;
                    MPEAdd.Show();
                    return;
                }
                else
                {
                    if (roleBL.RoleExists(strRoleName))
                    {
                        RoleNameValidatorAdd.IsValid = false;
                        MPEAdd.Show();
                        return;
                    }
                }

                // Insert                
                strActualRoleName = UserSchool.SchoolId + AppConstant.UNDERSCORE + strRoleName;
                Roles.CreateRole(strActualRoleName);
                roleBL.CreateRoleDetail(strRoleName, strDescription);
                AuthorizationBL authorizationBL = new AuthorizationBL(UserSchool);
                authorizationBL.InsertAuthorizations(roleBL.GetRole(strActualRoleName));
                // Rebind data
                MainDataPager.CurrentIndex = 1;
                BindRptRoles();

                // Reset UI 
                this.TxtRoleNameAdd.Text = "";
                this.TxtRoleDescriptionAdd.Text = "";

                if (this.CkbAddAfterSave.Checked)
                {
                    this.MPEAdd.Show();
                }
            }
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            bool bInfoInUse = false;
            CheckBox ckbxSelect = null;
            HiddenField HdfRptTenNhomNguoiDung = null;
            string roleName;

            foreach (RepeaterItem item in RptRoles.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    ckbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (ckbxSelect.Checked)
                    {
                        HdfRptTenNhomNguoiDung = (HiddenField)item.FindControl("HdfRptTenNhomNguoiDung");
                        roleName = HdfRptTenNhomNguoiDung.Value;
                        if (roleBL.IsDeletable(roleName))
                        {
                            roleBL.DeleteRole(roleName);
                        }
                        else
                        {
                            bInfoInUse = true;
                        }
                    }
                }
            }

            isSearch = false;
            BindRptRoles();

            if (bInfoInUse)
            {
                MPEInfoInUse.Show();
            }
        }

        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            HiddenField HdfRptMaNhomNguoiDung = null;
            foreach (RepeaterItem item in RptRoles.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox CkbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (CkbxSelect.Checked)
                    {
                        HdfRptMaNhomNguoiDung = (HiddenField)item.FindControl("HdfRptMaNhomNguoiDung");
                        TabularRole tabularRole = roleBL.GetTabularRole(new Guid(HdfRptMaNhomNguoiDung.Value));
                        if (tabularRole != null)
                        {
                            TxtRoleNameEdit.Text = tabularRole.DisplayedName;
                            TxtDescriptionNhomNguoiDungSua.Text = tabularRole.Description;
                            MPEEdit.Show();

                            this.hdfEditingRoleName.Value = tabularRole.RoleName;
                            return;
                        }
                    }
                }
            }
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            string roleName = this.hdfEditingRoleName.Value;
            string newRoleName = this.TxtRoleNameEdit.Text;
            string description = this.TxtDescriptionNhomNguoiDungSua.Text;

            if (!Page.IsValid)
            {
                return;
            }

            if (newRoleName == "" || newRoleName == "*")
            {
                RoleNameRequiredEdit.IsValid = false;
                MPEEdit.Show();
                return;
            }
            else
            {
                if (roleBL.RoleExists(roleName, newRoleName))
                {
                    RoleNameValidatorEdit.IsValid = false;
                    MPEEdit.Show();
                    return;
                }
            }

            roleBL.UpdateRole(roleName, newRoleName, description);
            BindRptRoles();
        }
        #endregion

        #region Pager event handlers
        public void pager_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currnetPageIndx;
            BindRptRoles();
        }
        #endregion
    }
}