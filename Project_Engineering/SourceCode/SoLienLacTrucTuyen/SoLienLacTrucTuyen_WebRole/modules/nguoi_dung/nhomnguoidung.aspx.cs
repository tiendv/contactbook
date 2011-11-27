using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SoLienLacTrucTuyen.BusinessEntity;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;
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
            if (isAccessDenied)
            {
                return;
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
            if(!lstAccessibilities.Contains(AccessibilityEnum.Add))
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
            PnlPopupConfirmDelete.Visible = bDisplayData;
            PnlPopupEdit.Visible = bDisplayData;
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
                strActualRoleName = UserSchool.SchoolId + "_" + strRoleName;
                Roles.CreateRole(strActualRoleName);
                roleBL.CreateRoleDetail(strRoleName, strDescription);

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
            string roleName = this.HdfRoleName.Value;
            roleBL.DeleteRole(roleName);

            isSearch = false;
            BindRptRoles();
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            string roleName = this.hdfEditingRoleName.Value;
            string newRoleName = this.TxtRoleNameEdit.Text;
            string description = this.TxtMoTaNhomNguoiDungSua.Text;

            ModalPopupExtender modalPopupEdit = new ModalPopupExtender();
            foreach (RepeaterItem rptItem in RptRoles.Items)
            {
                if (rptItem.ItemType == ListItemType.Item
                    || rptItem.ItemType == ListItemType.AlternatingItem)
                {
                    modalPopupEdit = (ModalPopupExtender)rptItem.FindControl("MPEEdit");
                    if (modalPopupEdit.ClientID == HdfRptRolesMPEEdit.Value)
                    {
                        break;
                    }
                }
            }

            if (!Page.IsValid)
            {
                return;
            }

            if (newRoleName == "" || newRoleName == "*")
            {
                RoleNameRequiredEdit.IsValid = false;
                modalPopupEdit.Show();
                return;
            }
            else
            {
                if (roleBL.RoleExists(roleName, newRoleName))
                {
                    RoleNameValidatorEdit.IsValid = false;
                    modalPopupEdit.Show();
                    return;
                }
            }            

            roleBL.UpdateRole(roleName, newRoleName, description);
            BindRptRoles();
        }        
        #endregion

        #region Repeater event handlers
        protected void RptRoles_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (!lstAccessibilities.Contains(AccessibilityEnum.Modify))
            {
                if (e.Item.ItemType == ListItemType.Header)
                {
                    e.Item.FindControl("thSuaNhomNguoiDung").Visible = false;
                }

                if (e.Item.ItemType == ListItemType.Item || 
                    e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    e.Item.FindControl("tdSuaNhomNguoiDung").Visible = false;
                }

                this.PnlPopupEdit.Visible = false;
            }

            if (!lstAccessibilities.Contains(AccessibilityEnum.Delete))
            {
                if (e.Item.ItemType == ListItemType.Header)
                {
                    e.Item.FindControl("thXoaNhomNguoiDung").Visible = false;
                }

                if (e.Item.ItemType == ListItemType.Item ||
                    e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    e.Item.FindControl("tdXoaNhomNguoiDung").Visible = false;
                }

                this.PnlPopupConfirmDelete.Visible = false;
            }
            else
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    TabularRole tbRole = (TabularRole)e.Item.DataItem;
                    if (!roleBL.IsDeletable(tbRole.RoleName))
                    {
                        ImageButton btnDeleteItem = (ImageButton)e.Item.FindControl("BtnDeleteItem");
                        btnDeleteItem.ImageUrl = AppConstant.IMAGESOURCE_DELETE_DISABLE;
                        btnDeleteItem.Enabled = false;
                    }
                }
            }            
        }

        protected void RptRoles_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDeleteItem":
                    {
                        this.LblConfirmDelete.Text = "Bạn có chắc xóa nhóm người dùng <b>\"" + e.CommandArgument + "\"</b> này không?";
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        this.HdfRoleName.Value = ((HiddenField)e.Item.FindControl("HdfRptTenNhomNguoiDung")).Value;
                        this.HdfRptRolesMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        Guid roleId = new Guid(e.CommandArgument.ToString());
                        TabularRole tabularRole = roleBL.GetTabularRole(roleId);
                        if (tabularRole != null)
                        {
                            TxtRoleNameEdit.Text = tabularRole.DisplayedName;
                            TxtMoTaNhomNguoiDungSua.Text = tabularRole.Description;
                            ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                            mPEEdit.Show();

                            this.HdfRptRolesMPEEdit.Value = mPEEdit.ClientID;
                            this.hdfEditingRoleName.Value = tabularRole.RoleName;
                        }

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
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