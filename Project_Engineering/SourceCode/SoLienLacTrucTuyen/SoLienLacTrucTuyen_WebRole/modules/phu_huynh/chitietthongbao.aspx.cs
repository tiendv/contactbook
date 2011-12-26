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

namespace SoLienLacTrucTuyen_WebRole.ModuleParents
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
                }
            }
            else
            {
                if (ViewState[AppConstant.VIEWSTATE_MESSAGEID] != null)
                {
                    message = messageBL.GetMessage((int)ViewState[AppConstant.VIEWSTATE_MESSAGEID]);
                }
            }

            if (message != null)
            {
                LblTitle.Text = message.Title;
                LblContent.Text = message.MessageContent;                
                if (message.MessageStatusId == 3)
                {
                    TxtFeedback.Text = message.Feedback;
                    PnlButtons.Width = new Unit(PnlButtons.Width.Value / 2);
                    BtnConfirm.Visible = false;
                    TxtFeedback.ReadOnly = true;
                }
                else
                {
                    TxtFeedback.ReadOnly = false;
                }
            }
            else
            {
                Response.Redirect(AppConstant.PAGEPATH_MESSAGE);
            }
        }

        private void SaveSearchSessions()
        {
            if (CheckSessionKey(AppConstant.SESSION_SELECTED_YEAR)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_FROMDATE)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_TODATE)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_CONFIRMSTATUS))
            {
                ViewState[AppConstant.VIEWSTATE_SELECTED_YEAR] = (Int32)GetSession(AppConstant.SESSION_SELECTED_YEAR);
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
        protected void BtnConfirm_Click(object sender, ImageClickEventArgs e)
        {
            message.Feedback = TxtFeedback.Text;
            messageBL.ConfirmMessage(message);

            RedirectToPrevPage();
        }

        protected void BtnClose_Click(object sender, ImageClickEventArgs e)
        {
            RedirectToPrevPage();
        }
        #endregion

        #region Methods
        private void RedirectToPrevPage()
        {
            AddSearchSessions();

            Response.Redirect(AppConstant.PAGEPATH_MESSAGE);
        }

        private void AddSearchSessions()
        {
            if (ViewState[AppConstant.VIEWSTATE_SELECTED_YEAR] != null)
            {
                AddSession(AppConstant.SESSION_SELECTED_YEAR, ViewState[AppConstant.VIEWSTATE_SELECTED_YEAR]);
                AddSession(AppConstant.SESSION_SELECTED_FROMDATE, ViewState[AppConstant.VIEWSTATE_SELECTED_FROMDATE]);
                AddSession(AppConstant.SESSION_SELECTED_TODATE, ViewState[AppConstant.VIEWSTATE_SELECTED_TODATE]);
                AddSession(AppConstant.SESSION_SELECTED_CONFIRMSTATUS, ViewState[AppConstant.VIEWSTATE_SELECTED_CONFIRMSTATUS]);
            }
        }
        #endregion
    }
}