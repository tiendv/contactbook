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
    public partial class UserListPage : BaseContentPage
    {
        #region Fields
        private UserBL userBL;
        private AuthorizationBL authorizationBL;
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

            userBL = new UserBL(UserSchool);
            authorizationBL = new AuthorizationBL(UserSchool);

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

        private void BindRptUsers()
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
            BtnAddUser.Visible = accessibilities.Contains(AccessibilityEnum.Add);
            BtnEdit.Visible = accessibilities.Contains(AccessibilityEnum.Modify);
            BtnDelete.Visible = accessibilities.Contains(AccessibilityEnum.Delete);
            PnlPopupConfirmDelete.Visible = accessibilities.Contains(AccessibilityEnum.Delete);
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
            Response.Redirect(AppConstant.PAGEPATH_USER_ADD);
        }

        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            HiddenField HdfRptMaNhomNguoiDung = null;
            aspnet_User user = null;
            foreach (RepeaterItem item in RptUser.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox CkbxSelect = (CheckBox)item.FindControl("CkbxSelectUser");

                    if (CkbxSelect.Checked)
                    {
                        HdfRptMaNhomNguoiDung = (HiddenField)item.FindControl("HdfRptMaNhomNguoiDung");
                        user = new aspnet_User();
                        user.UserId = new Guid(HdfRptMaNhomNguoiDung.Value);
                        AddSession(AppConstant.SESSION_SELECTED_USER, user);
                        Response.Redirect(AppConstant.PAGEPATH_USER_EDIT);
                        return;
                    }
                }
            }
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            bool bInfoInUse = false;
            HiddenField hdfactualUserName = null;
            HiddenField HdfRptMaNhomNguoiDung = null;
            aspnet_User user = null;
            foreach (RepeaterItem item in RptUser.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox CkbxSelectUser = (CheckBox)item.FindControl("CkbxSelectUser");
                    if (CkbxSelectUser.Checked)
                    {
                        HdfRptMaNhomNguoiDung = (HiddenField)item.FindControl("HdfRptMaNhomNguoiDung");
                        user = new aspnet_User();
                        user.UserId = new Guid(HdfRptMaNhomNguoiDung.Value);
                        if (userBL.IsDeletable(user))
                        {
                            hdfactualUserName = (HiddenField)item.FindControl("HdfRptActualUserName");
                            authorizationBL.DeleteAuthorization(hdfactualUserName.Value);
                            Membership.DeleteUser(hdfactualUserName.Value, true);
                        }
                        else
                        {
                            bInfoInUse = true;
                        }
                    }
                }
            }

            isSearch = false;
            BindRptUsers();

            if (bInfoInUse)
            {
                MPEInfoInUse.Show();
            }
        }

        protected void BtnActivate_Click(object sender, ImageClickEventArgs e)
        {
            bool bHasEmptyEmail = false;
            HiddenField HdfRptMaNhomNguoiDung = null;
            HiddenField HdfRptActualUserName = null;
            Label LblEmail = null;
            List<aspnet_User> users = new List<aspnet_User>();
            aspnet_User user = null;
            string strSendMailError;
            foreach (RepeaterItem item in RptUser.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox CkbxSelectUser = (CheckBox)item.FindControl("CkbxSelectUser");
                    if (CkbxSelectUser.Checked)
                    {
                        LblEmail = (Label)item.FindControl("LblEmail");
                        if (CheckUntils.IsNullOrBlank(LblEmail.Text) == false)
                        {
                            HdfRptMaNhomNguoiDung = (HiddenField)item.FindControl("HdfRptMaNhomNguoiDung");
                            user = new aspnet_User();
                            user.UserId = new Guid(HdfRptMaNhomNguoiDung.Value);
                            HdfRptActualUserName = (HiddenField)item.FindControl("HdfRptActualUserName");
                            user.UserName = HdfRptActualUserName.Value;
                            MembershipUser membershipUser = Membership.GetUser(user.UserName);
                            string strPassword = membershipUser.ResetPassword();

                            strSendMailError = MailBL.SendByGmail(UserSchool.Email, membershipUser.Email,
                                "[eContact.com] Kích hoạt tài khoản thành công",
                                string.Format("Trường {0} xin thông báo đã kích hoạt thành công tài khoản {1} với mật khẩu truy cập là {2}",
                                    UserSchool.SchoolName, user.UserName.Split(AppConstant.UNDERSCORE_CHAR)[1], strPassword),
                                UserSchool.Email.Split('@')[0],
                                UserSchool.Password);
                            if (CheckUntils.IsNullOrBlank(strSendMailError) == false)
                            {
                                LblSendMailError.Text = strSendMailError + "<br/>Vui lòng kích hoạt lại người dùng";
                                MPESendMailReport.Show();
                                return;
                            }

                            userBL.ActivateUsers(user);                               
                            Membership.UpdateUser(membershipUser);
                        }
                        else
                        {                            
                            bHasEmptyEmail = true;
                        }
                    }
                }
            }

            isSearch = false;
            BindRptUsers();

            if (bHasEmptyEmail)
            {
                MPEActivateReport.Show();
            }
        }

        protected void BtnDeactivate_Click(object sender, ImageClickEventArgs e)
        {
            HiddenField HdfRptMaNhomNguoiDung = null;
            List<aspnet_User> users = new List<aspnet_User>();
            aspnet_User user = null;
            foreach (RepeaterItem item in RptUser.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox CkbxSelectUser = (CheckBox)item.FindControl("CkbxSelectUser");
                    if (CkbxSelectUser.Checked)
                    {
                        HdfRptMaNhomNguoiDung = (HiddenField)item.FindControl("HdfRptMaNhomNguoiDung");
                        user = new aspnet_User();
                        user.UserId = new Guid(HdfRptMaNhomNguoiDung.Value);
                        users.Add(user);
                    }
                }
            }

            userBL.DeactivateUsers(users);
            isSearch = false;
            BindRptUsers();
        }
        #endregion

        #region Repeater event handlers
        protected void RptUser_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDetailItem":
                    {
                        aspnet_User user = new aspnet_User();
                        user.UserId = new Guid(e.CommandArgument.ToString());
                        AddSession(AppConstant.SESSION_SELECTED_USER, user);
                        Response.Redirect(AppConstant.PAGEPATH_USER_DETAIL);
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