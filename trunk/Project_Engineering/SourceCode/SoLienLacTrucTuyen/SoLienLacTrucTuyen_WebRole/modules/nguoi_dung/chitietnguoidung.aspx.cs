using System;
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
    public partial class DetailUserPage : BaseContentPage
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
                if (CheckSessionKey(AppConstant.SESSION_SELECTED_USER))
                {
                    gUserId = ((aspnet_User)GetSession(AppConstant.SESSION_SELECTED_USER)).UserId;
                    RemoveSession(AppConstant.SESSION_SELECTED_USER);
                    ViewState[AppConstant.VIEWSTATE_USER] = gUserId;
                }
                else
                {
                    Response.Redirect(AppConstant.PAGEPATH_USER_LIST);
                }
            }
            else
            {
                if (ViewState[AppConstant.VIEWSTATE_USER] != null)
                {
                    gUserId = (Guid)ViewState[AppConstant.VIEWSTATE_USER];
                }
                else
                {
                    Response.Redirect(AppConstant.PAGEPATH_USER_LIST);
                }
            }

            aspnet_User user = userBL.GetUser(gUserId);
            LblUserName.Text = user.UserName.Split(AppConstant.UNDERSCORE_CHAR)[1];
            LblFullName.Text = user.aspnet_Membership.FullName;
            List<aspnet_Role> roles = userBL.GetRoles(user.UserName);
            StringBuilder strb = new StringBuilder();
            foreach (aspnet_Role role in roles)
            {
                strb.Append(role.UserManagement_RoleDetail.DisplayedName);
                strb.Append(", ");
            }

            LblRoleName.Text = strb.ToString().Trim().Trim(new char[] { ',' });
            LblEmail.Text = user.aspnet_Membership.Email;
            LblCreatedDate.Text = user.aspnet_Membership.CreateDate.ToShortDateString();
            LblLastedLoginDate.Text = user.aspnet_Membership.LastLoginDate.ToShortDateString();
            if (user.aspnet_Membership.IsActivated == null)
            {
                LblStatus.Text = "Chưa kích hoạt";
            }
            else if ((bool)user.aspnet_Membership.IsActivated)
            {
                LblStatus.Text = "Đã kích hoạt";
            }
            else
            {
                LblStatus.Text = "Chưa kích hoạt";
            }
        }
        #endregion

        #region Button event handler(s)
        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            aspnet_User user = new aspnet_User();
            user.UserId = new Guid(ViewState[AppConstant.VIEWSTATE_USER].ToString());
            AddSession(AppConstant.SESSION_SELECTED_USER, user);
            Response.Redirect(AppConstant.PAGEPATH_USER_EDIT);
        }

        protected void BtnBackPrevPage_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(AppConstant.PAGEPATH_USER_LIST);
        }
        #endregion
    }
}