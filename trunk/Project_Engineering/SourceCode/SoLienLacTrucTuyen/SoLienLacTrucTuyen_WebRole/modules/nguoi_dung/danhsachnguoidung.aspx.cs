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
    public partial class UsersPage : BaseContentPage
    {
        #region Fields
        private UserBL userBL;
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

            userBL = new UserBL(UserSchool);

            if (!Page.IsPostBack)
            {
                BindDDLRoles();
                isSearch = false;
                MainDataPager.CurrentIndex = 1;

                BindRptUsers();
            }

            ProcPermissions();
        }
        #endregion

        #region Methods
        private void BindDDLRoles()
        {
            RoleBL roleBL = new RoleBL(UserSchool);
            List<TabularRole> roles = roleBL.GetTabularRoles();
            DdlRoles.DataSource = roles;
            DdlRoles.DataValueField = "RoleId";
            DdlRoles.DataTextField = "DisplayedName";
            DdlRoles.DataBind();

            if (DdlRoles.Items.Count != 0)
            {
                DdlRoles.Items.Insert(0, new ListItem("Tất cả", new Guid().ToString()));
            }
        }

        public void BindRptUsers()
        {
            aspnet_Role role = null;
            List<TabularUser> tabularUsers;
            double dTotalRecords;
            string strUserName = TxtSearchUserName.Text.Trim();

            if (DdlRoles.Items.Count != 0)
            {
                if (DdlRoles.SelectedIndex > 0)
                {
                    role = new aspnet_Role();
                    role.RoleId = new Guid(DdlRoles.SelectedValue);
                }
            }

            tabularUsers = userBL.GetTabularUsers(role, strUserName, MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            // Decrease page current index when delete
            if (tabularUsers.Count == 0 && dTotalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRptUsers();
                return;
            }

            bool bDisplayData = (tabularUsers.Count != 0) ? true : false;
            PnlPopupConfirmDelete.Visible = bDisplayData;
            RptUser.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin người dùng";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy người dùng";
                }

                MainDataPager.CurrentIndex = 1;
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
            }
            else
            {
                MainDataPager.Visible = true;
            }

            RptUser.DataSource = tabularUsers;
            RptUser.DataBind();
            MainDataPager.ItemCount = dTotalRecords;
        }

        private void ProcPermissions()
        {
            if (lstAccessibilities.Contains(AccessibilityEnum.Add))
            {
                if (DdlRoles.Items.Count == 0)
                {
                    BtnAddUser.Enabled = false;
                    BtnAddUser.ImageUrl = "~/Styles/Images/button_add_with_text_disable.png";
                }
                else
                {
                    BtnAddUser.Enabled = true;
                    BtnAddUser.ImageUrl = "~/Styles/Images/button_add_with_text.png";
                }
            }
            else
            {
                this.BtnAddUser.Visible = false;
            }
        }
        #endregion

        #region Button click event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            MainDataPager.ItemCount = 0;
            isSearch = true;
            BindRptUsers();
        }

        protected void BtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/modules/nguoi_dung/themnguoidung.aspx");
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            string userName = this.HdfUserName.Value;

            if (!userBL.UserInRoleParents(userName))
            {
                Membership.DeleteUser(userName, true);
            }

            isSearch = false;
            BindRptUsers();

        }
        #endregion

        #region Repeater event handlers
        protected void RptUser_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (!lstAccessibilities.Contains(AccessibilityEnum.Modify))
            {
                if (e.Item.ItemType == ListItemType.Header)
                {
                    e.Item.FindControl("thEditUser").Visible = false;
                }

                if (e.Item.ItemType == ListItemType.Item ||
                    e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    e.Item.FindControl("tdEditUser").Visible = false;
                }
            }

            if (!lstAccessibilities.Contains(AccessibilityEnum.Delete))
            {
                if (e.Item.ItemType == ListItemType.Header)
                {
                    e.Item.FindControl("thDeleteUser").Visible = false;
                }

                if (e.Item.ItemType == ListItemType.Item ||
                    e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    e.Item.FindControl("tdDeleteUser").Visible = false;
                }

                this.PnlPopupConfirmDelete.Visible = false;
            }
            else
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    aspnet_User user = new aspnet_User();
                    user.UserId = ((TabularUser)e.Item.DataItem).UserId;
                    if (!userBL.IsDeletable(user))
                    {
                        ImageButton btnDeleteItem = (ImageButton)e.Item.FindControl("BtnDeleteItem");
                        btnDeleteItem.ImageUrl = "~/Styles/Images/button_delete_disable.png";
                        btnDeleteItem.Enabled = false;
                    }
                }
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HyperLink hlkTenNguoiDung = (HyperLink)e.Item.FindControl("HlkTenNguoiDung");
                hlkTenNguoiDung.NavigateUrl = "~/modules/nguoi_dung/chitietnguoidung.aspx?UserId="
                    + ((TabularUser)e.Item.DataItem).UserId;
            }
        }

        protected void RptUser_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDeleteItem":
                    {
                        this.LblConfirmDelete.Text = "Bạn có chắc xóa người dùng <b>\""
                            + e.CommandArgument + "\"</b> này không?";
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        this.HdfUserName.Value = ((HiddenField)e.Item.FindControl("HdfRptActualUserName")).Value;
                        this.HdfRptUserMPEDelete.Value = mPEDelete.ClientID;                        

                        break;
                    }
                case "CmdEditItem":
                    {
                        Response.Redirect(string.Format(AppConstant.PAGEPATH_EDITUSER + "?UserId={0}", e.CommandArgument));
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
        public void DataPager_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currnetPageIndx;
            BindRptUsers();
        }
        #endregion
    }
}