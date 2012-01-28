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
    public partial class CategoryManagerPage : BaseContentPage
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
            string strPagePath = HlkYear.NavigateUrl.TrimStart('~');
            PnlYear.Visible = authorizationBL.ValidateAuthorization(LogedInUserRoles, strPagePath);
            strPagePath = HlkFaculty.NavigateUrl.TrimStart('~');
            PnlFaculty.Visible = authorizationBL.ValidateAuthorization(LogedInUserRoles, strPagePath);
            strPagePath = HlkGrade.NavigateUrl.TrimStart('~');
            PnlGrade.Visible = authorizationBL.ValidateAuthorization(LogedInUserRoles, strPagePath);
            strPagePath = HlkSubject.NavigateUrl.TrimStart('~');
            PnlSubject.Visible = authorizationBL.ValidateAuthorization(LogedInUserRoles, strPagePath);
            strPagePath = HlkTeachingPeriod.NavigateUrl.TrimStart('~');
            PnlTeachingPeriod.Visible = authorizationBL.ValidateAuthorization(LogedInUserRoles, strPagePath);
            strPagePath = HlkMarkType.NavigateUrl.TrimStart('~');
            PnlMarkType.Visible = authorizationBL.ValidateAuthorization(LogedInUserRoles, strPagePath);
            strPagePath = HlkLearningAptitude.NavigateUrl.TrimStart('~');
            PnlLearningAptitude.Visible = authorizationBL.ValidateAuthorization(LogedInUserRoles, strPagePath);
            strPagePath = HlkConduct.NavigateUrl.TrimStart('~');
            PnlConduct.Visible = authorizationBL.ValidateAuthorization(LogedInUserRoles, strPagePath);
            strPagePath = HlkLearningResult.NavigateUrl.TrimStart('~');
            PnlLearningResult.Visible = authorizationBL.ValidateAuthorization(LogedInUserRoles, strPagePath);
            strPagePath = HlkTeacher.NavigateUrl.TrimStart('~');
            PnlTeacher.Visible = authorizationBL.ValidateAuthorization(LogedInUserRoles, strPagePath);
        }
    }
}