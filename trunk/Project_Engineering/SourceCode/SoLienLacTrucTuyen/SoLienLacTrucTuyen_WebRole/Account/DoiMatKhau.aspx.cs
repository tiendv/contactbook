using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class DoiMatKhau : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string pageUrl = Page.Request.Path;
            //Guid role = (new UserBL(UserSchool)).GetRoleId(User.Identity.Name);

            Site masterPage = (Site)Page.Master;
            masterPage.PageTitle = "Đổi mật khẩu";
            //masterPage.UserRole = role;
            //masterPage.PageUrl = pageUrl;
        }

        protected void ChangeUserPassword_ChangePasswordError(object sender, EventArgs e)
        {
            Control control = ChangeUserPassword.ChangePasswordTemplateContainer;
            RequiredFieldValidator currentPasswordRequired = (RequiredFieldValidator)control.FindControl("CurrentPasswordRequired");
            currentPasswordRequired.ErrorMessage = "Mật khẩu cũ không hợp lệ hoặc mật khẩu mới không đúng định dạng.<br/>"
                + "Vui lòng nhập lại!";
            currentPasswordRequired.IsValid = false;
        }
    }
}