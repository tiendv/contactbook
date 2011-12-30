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
                
            }

            RoleBL roleBL = new RoleBL(UserSchool);
            aspnet_Role roleSupplier = roleBL.GetRoleSupplier();
            aspnet_Role roleParents = roleBL.GetRoleParents();

            if (roleParents != null)
            {                
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

                //LblGreetingHead.Text = string.Format("Chào mừng {0} đến với hệ thống sổ liên lạc trực tuyến eContactBook của {1}",
                //    roleSupplier.UserManagement_RoleDetail.DisplayedName, UserSchool.SchoolName);

                // LblGreetingTail.Text = UserSchool.SchoolName;
            }
            else
            {
                LoginView1.RoleGroups[2].Roles = new string[] { roleSupplier.RoleName };
            }

            if (accessDenied)
            {
                return;
            }
        }
    }
}