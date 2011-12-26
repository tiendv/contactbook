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

            if (User.IsInRole(roleParents.RoleName))
            {
                Student_Student loggedInStudent = null;
                string strLoggedInStudentSessionKey = User.Identity.Name
                        + AppConstant.UNDERSCORE + AppConstant.SESSION_MEMBERSHIP_STUDENT;
                if (Session[strLoggedInStudentSessionKey] != null)
                {
                    loggedInStudent = (Student_Student)Session[strLoggedInStudentSessionKey];
                }

                MessageBL messageBL = new MessageBL(UserSchool);
                int iNewMessageCount = messageBL.GetNewMessageCount(loggedInStudent);
                int iUnconfirmedMessageCount = messageBL.GetUnconfirmedMessageCount(loggedInStudent);
                ((Label)LoginView1.Controls[0].FindControl("LblNewMessageStatus")).Text = string.Format("Bạn có {0} thông báo mới", iNewMessageCount);
                if (iUnconfirmedMessageCount != 0)
                {
                    ((Label)LoginView1.Controls[0].FindControl("LblUnconfirmedMessageStatus")).Text = string.Format("Bạn còn {0} thông báo chưa phản hồi", iUnconfirmedMessageCount);
                }
            }

            if (accessDenied)
            {
                return;
            }
        }
    }
}