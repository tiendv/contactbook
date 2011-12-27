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
    public partial class ChiTietNguoiDung : BaseContentPage
    {
        #region Fields
        UserBL userBL;
        #endregion

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
                    Response.Redirect(AppConstant.PAGEPATH_USERS);
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
                    Response.Redirect(AppConstant.PAGEPATH_USERS);
                }
            }

            aspnet_User user = userBL.GetUser(gUserId);
            LblUserName.Text = user.UserName.Split(AppConstant.UNDERSCORE_CHAR)[1];
            TxtFullName.Text = user.aspnet_Membership.FullName;
            List<aspnet_Role> roles = userBL.GetRoles(user.UserName);
            StringBuilder strb = new StringBuilder();
            foreach(aspnet_Role role in roles)
            {
                strb.Append(role.UserManagement_RoleDetail.DisplayedName);
                strb.Append(", ");
            }

            LblRoleName.Text = strb.ToString().Trim().Trim(new char[] { ',' });
            TxtEmail.Text = user.aspnet_Membership.Email;
            LblCreatedDate.Text = user.aspnet_Membership.CreateDate.ToShortDateString();
            LblLastedLoginDate.Text = user.aspnet_Membership.LastLoginDate.ToShortDateString();
        }
    }
}