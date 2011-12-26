using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using System.Web.Security;
using EContactBook.DataAccess;

namespace SoLienLacTrucTuyen_WebRole.ModuleParents
{
    public partial class HomePage : BaseContentPage
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

            MessageBL messageBL = new MessageBL(UserSchool);
            int iNewMessageCount = messageBL.GetNewMessageCount(LoggedInStudent);
            int iUnconfirmedMessageCount = messageBL.GetUnconfirmedMessageCount(LoggedInStudent);
            LblNewMessageStatus.Text = string.Format("Bạn có {0} thông báo mới", iNewMessageCount);
            LblUnconfirmedMessageStatus.Text = string.Format("Bạn còn {0} thông báo chưa phản hồi", iUnconfirmedMessageCount);            
        }
    }
}