using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class QuanLiLopHoc : BaseContentPage
    {
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

            AuthorizationBL authorizationBL = new AuthorizationBL(UserSchool);
            string strUserName = User.Identity.Name;
            string strPagePath = HlkClass.NavigateUrl.TrimStart('~');
            PnlClass.Visible = authorizationBL.ValidateAuthorization(LogedInUserRoles, strPagePath);
            strPagePath = HlkFormerTeacher.NavigateUrl.TrimStart('~');
            PnlFormerTeacher.Visible = authorizationBL.ValidateAuthorization(LogedInUserRoles, strPagePath);
            strPagePath = HlkSchedule.NavigateUrl.TrimStart('~');
            PnlSchedule.Visible = authorizationBL.ValidateAuthorization(LogedInUserRoles, strPagePath);
        }
    }
}