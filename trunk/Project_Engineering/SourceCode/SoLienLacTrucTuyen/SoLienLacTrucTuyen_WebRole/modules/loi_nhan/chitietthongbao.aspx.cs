using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen_WebRole.Modules;
using EContactBook.DataAccess;
using SoLienLacTrucTuyen.BusinessLogic;
using System.Web.Security;
using System.Text;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class DetailMessage : BaseContentPage
    {
        #region Fields
        private MessageBL messageBL;
        private MessageToParents_Message message;
        #endregion

        #region Page event handlers
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

            messageBL = new MessageBL(UserSchool);
            if (!Page.IsPostBack)
            {
                if (CheckSessionKey(AppConstant.SESSION_MESSAGE))
                {
                    message = (MessageToParents_Message)GetSession(AppConstant.SESSION_MESSAGE);
                    RemoveSession(AppConstant.SESSION_MESSAGE);
                    ViewState[AppConstant.VIEWSTATE_MESSAGEID] = message.MessageId;
                    SaveSearchSessions();

                    message = messageBL.GetMessage(message.MessageId);
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("Thông báo đến phụ huynh của học sinh ");
                    stringBuilder.Append(message.Student_StudentInClass.Student_Student.FullName);
                    stringBuilder.Append(", mã học sinh ");
                    stringBuilder.Append(message.Student_StudentInClass.Student_Student.StudentCode);
                    LblStudentInformation.Text = stringBuilder.ToString();
                    LblTitle.Text = message.Title;
                    LblContent.Text = message.MessageContent;
                    LblFeedback.Text = message.Feedback;
                }
                else
                {
                    Response.Redirect(AppConstant.PAGEPATH_MESSAGE_LIST);
                }
            }
            else
            {
                if (ViewState[AppConstant.VIEWSTATE_MESSAGEID] != null)
                {
                    message = new MessageToParents_Message();
                    message.MessageId = (int)ViewState[AppConstant.VIEWSTATE_MESSAGEID];
                }
            }
        }

        private void SaveSearchSessions()
        {
            if (CheckSessionKey(AppConstant.SESSION_SELECTED_YEAR)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_FROMDATE)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_TODATE)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_CONFIRMSTATUS))
            {
                ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID] = (Int32)GetSession(AppConstant.SESSION_SELECTED_YEAR);
                RemoveSession(AppConstant.SESSION_SELECTED_YEAR);

                ViewState[AppConstant.VIEWSTATE_SELECTED_FROMDATE] = (DateTime)GetSession(AppConstant.SESSION_SELECTED_FROMDATE);
                RemoveSession(AppConstant.SESSION_SELECTED_FROMDATE);

                ViewState[AppConstant.VIEWSTATE_SELECTED_TODATE] = (DateTime)GetSession(AppConstant.SESSION_SELECTED_TODATE);
                RemoveSession(AppConstant.SESSION_SELECTED_TODATE);

                ViewState[AppConstant.VIEWSTATE_SELECTED_CONFIRMSTATUS] = (Int32)GetSession(AppConstant.SESSION_SELECTED_CONFIRMSTATUS);
                RemoveSession(AppConstant.SESSION_SELECTED_CONFIRMSTATUS);
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            AddSession(AppConstant.SESSION_MESSAGE, message);
            Response.Redirect(AppConstant.PAGEPATH_MESSAGE_MODIFY);
        }

        protected void BtnBackPrevPage_Click(object sender, ImageClickEventArgs e)
        {
            if (ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID] != null)
            {
                AddSession(AppConstant.SESSION_SELECTED_YEAR, ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID]);
                AddSession(AppConstant.SESSION_SELECTED_FROMDATE, ViewState[AppConstant.VIEWSTATE_SELECTED_FROMDATE]);
                AddSession(AppConstant.SESSION_SELECTED_TODATE, ViewState[AppConstant.VIEWSTATE_SELECTED_TODATE]);
                AddSession(AppConstant.SESSION_SELECTED_CONFIRMSTATUS, ViewState[AppConstant.VIEWSTATE_SELECTED_CONFIRMSTATUS]);
            }

            Response.Redirect(AppConstant.PAGEPATH_MESSAGE_LIST);
        }
        #endregion

        #region Methods
        private void ProcPermissions()
        {
            BtnEdit.Visible = accessibilities.Contains(AccessibilityEnum.Modify);
        }        
        #endregion
    }
}