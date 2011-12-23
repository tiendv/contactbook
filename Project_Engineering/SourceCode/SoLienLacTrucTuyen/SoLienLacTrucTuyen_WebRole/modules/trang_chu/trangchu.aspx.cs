using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using System.Web.Security;
using EContactBook.DataAccess;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class TrangChu : BaseContentPage
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

            if (!Page.IsPostBack)
            {
                LblSchoolName.Text = UserSchool.SchoolName;
            }

            RoleBL roleBL = new RoleBL(UserSchool);
            aspnet_Role roleParents = roleBL.GetRoleParents();
            aspnet_Role roleAdministrator = roleBL.GetRoleAdministrator();
            LoginView1.RoleGroups[0].Roles = new string[] { roleParents.RoleName };
            LoginView1.RoleGroups[1].Roles = new string[] { roleAdministrator.RoleName };
        }
    }
}