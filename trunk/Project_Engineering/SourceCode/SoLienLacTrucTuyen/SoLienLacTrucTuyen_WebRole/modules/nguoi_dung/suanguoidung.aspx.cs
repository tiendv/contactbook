﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.BusinessEntity;
using System.Web.Security;
using EContactBook.DataAccess;
using System.Text;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class ModifyUserPage : BaseContentPage
    {
        #region Fields
        UserBL userBL;
        #endregion

        #region Page event handler(s)
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
            Guid gUserId = new Guid();
            if (!Page.IsPostBack)
            {
                BindDDLStatus();
                if (CheckSessionKey(AppConstant.SESSION_SELECTED_USER))
                {
                    gUserId = ((aspnet_User)GetSession(AppConstant.SESSION_SELECTED_USER)).UserId;
                    RemoveSession(AppConstant.SESSION_SELECTED_USER);
                    ViewState[AppConstant.VIEWSTATE_USER] = gUserId;
                    aspnet_User user = userBL.GetUser(gUserId);
                    aspnet_Membership membership = user.aspnet_Membership;
                    ViewState[AppConstant.VIEWSTATE_USER_ISTEACHER] = membership.IsTeacher;
                    ViewState[AppConstant.VIEWSTATE_USER_ISDELETABLE] = membership.IsDeletable;
                    ViewState[AppConstant.VIEWSTATE_USER_ID] = membership.UserId.ToString();
                    LblUserName.Text = user.UserName.Split(AppConstant.UNDERSCORE_CHAR)[1];
                    TxtFullName.Text = membership.FullName;
                    List<aspnet_Role> roles = userBL.GetRoles(user.UserName);
                    StringBuilder strb = new StringBuilder();
                    foreach (aspnet_Role role in roles)
                    {
                        strb.Append(role.UserManagement_RoleDetail.DisplayedName);
                        strb.Append(", ");
                    }

                    TxtEmail.Text = membership.Email;
                    if (membership.IsActivated == null)
                    {
                        DDLStatus.SelectedIndex = 0;
                    }
                    else if ((bool)membership.IsActivated)
                    {
                        DDLStatus.SelectedIndex = 1;
                    }
                    else
                    {
                        DDLStatus.SelectedIndex = 0;
                    }
                }
                else
                {
                    Response.Redirect(AppConstant.PAGEPATH_USER_LIST);
                }
            }
        }
        #endregion

        #region Button event handler(s)
        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            string strUserName = LblUserName.Text;
            string strFullName = TxtFullName.Text.Trim();
            string strEmail = TxtEmail.Text.Trim();
            bool bStatus;
            if (DDLStatus.SelectedIndex == 0)
            {
                bStatus = false;
            }
            else
            {
                bStatus = true;
            }

            aspnet_User user = new aspnet_User();
            user.UserId = new Guid((string)ViewState[AppConstant.VIEWSTATE_USER_ID]);
            user.UserName = strUserName;
            bool bIsTeacher = (bool)ViewState[AppConstant.VIEWSTATE_USER_ISTEACHER];
            bool bIsDeletable = (bool)ViewState[AppConstant.VIEWSTATE_USER_ISDELETABLE];

            userBL.UpdateMembership(user, bIsTeacher, strFullName, strEmail, bStatus, bIsDeletable);
            BackToPreviousPage();
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            BackToPreviousPage();
        }
        #endregion

        #region Method(s)
        private void BindDDLStatus()
        {
            DDLStatus.Items.Add(new ListItem("Chưa kích hoạt", "false"));
            DDLStatus.Items.Add(new ListItem("Đã kích hoạt", "true"));
        }

        private void BackToPreviousPage()
        {
            Response.Redirect(AppConstant.PAGEPATH_USER_LIST);
        }
        #endregion
    }
}