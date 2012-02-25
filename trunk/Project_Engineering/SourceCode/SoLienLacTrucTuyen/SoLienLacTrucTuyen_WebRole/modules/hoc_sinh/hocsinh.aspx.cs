using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using SoLienLacTrucTuyen.BusinessLogic;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class StudentManagementPage : BaseContentPage
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
            string strPagePath = HlkStudentList.NavigateUrl.TrimStart('~');
            PnlStudentList.Visible = authorizationBL.ValidateAuthorization(LogedInUserRoles, strPagePath);
            strPagePath = HlkStudentMark.NavigateUrl.TrimStart('~');
            PnlStudentMark.Visible = authorizationBL.ValidateAuthorization(LogedInUserRoles, strPagePath);
            strPagePath = HlkApprovalStudentMark.NavigateUrl.TrimStart('~');
            PnlApprovalStudentMark.Visible = authorizationBL.ValidateAuthorization(LogedInUserRoles, strPagePath);
            strPagePath = HlkStudentConduct.NavigateUrl.TrimStart('~');
            PnlStudentConduct.Visible = authorizationBL.ValidateAuthorization(LogedInUserRoles, strPagePath);
            strPagePath = HlkChangeGrade.NavigateUrl.TrimStart('~');
            PnlChangeGrade.Visible = authorizationBL.ValidateAuthorization(LogedInUserRoles, strPagePath);
        }
    }
}