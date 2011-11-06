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
    public partial class DanhSachNguoiDung : BaseContentPage
    {
        #region Fields
        private UserBL userBL;
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

            userBL = new UserBL();
            roleBL = new RoleBL();

            if (!Page.IsPostBack)
            {
                BindDropDownList();
                isSearch = false;
                MainDataPager.CurrentIndex = 1;

                BindDataRepeater();
            }

            ProcPermissions();
        }
        #endregion

        #region Methods
        private void BindDropDownList()
        {
            List<aspnet_Role> lstNhomNguoiDung = roleBL.GetListRoles();
            DdlRoles.DataSource = lstNhomNguoiDung;
            DdlRoles.DataValueField = "RoleId";
            DdlRoles.DataTextField = "RoleName";
            DdlRoles.DataBind();

            if (DdlRoles.Items.Count != 0)
            {
                DdlRoles.Items.Insert(0, new ListItem("Tất cả", new Guid().ToString()));
            }
        }

        public void BindDataRepeater()
        {
            string userName = TxtSearchUserName.Text.Trim();
            Guid searchedRole;
            if (DdlRoles.Items.Count != 0)
            {
                searchedRole = new Guid(DdlRoles.SelectedValue);
            }
            else
            {
                searchedRole = new Guid();
            }

            double totalRecords;
            List<TabularUser> lstTbUsers = userBL.GetListTabularUsers(
                searchedRole, userName,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out totalRecords);

            // Decrease page current index when delete
            if (lstTbUsers.Count == 0 && totalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindDataRepeater();
                return;
            }

            bool bDisplayData = (lstTbUsers.Count != 0) ? true : false;
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

            RptUser.DataSource = lstTbUsers;
            RptUser.DataBind();
            MainDataPager.ItemCount = totalRecords;
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
            BindDataRepeater();
        }

        protected void BtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/Modules/Nguoi_Dung/ThemNguoiDung.aspx");
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            string userName = this.HdfUserName.Value;

            if (!userBL.UserInRoleParents(userName))
            {
                Membership.DeleteUser(userName, true);
            }

            isSearch = false;
            BindDataRepeater();

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
                    TabularUser nguoiDung = (TabularUser)e.Item.DataItem;
                    Guid maNguoiDung = nguoiDung.UserId;
                    if (!userBL.CanDeleteNguoiDung(maNguoiDung))
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

                        this.HdfUserName.Value = e.CommandArgument.ToString();
                        this.HdfRptUserMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        Response.Redirect("~/modules/nguoi_dung/suanguoidung.aspx?UserId="
                            + e.CommandArgument);
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
            BindDataRepeater();
        }
        #endregion
    }
}