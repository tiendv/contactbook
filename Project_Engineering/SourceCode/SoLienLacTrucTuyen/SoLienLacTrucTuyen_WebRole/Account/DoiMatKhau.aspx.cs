using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Site masterPage = (Site)Page.Master;
            masterPage.PageTitle = "Đổi mật khẩu";
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