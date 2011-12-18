using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen_WebRole.Modules;
using EContactBook.DataAccess;
using SoLienLacTrucTuyen.BusinessLogic;

namespace SoLienLacTrucTuyen_WebRole.ModuleParents
{
    public partial class DetailMessage : BaseContentPage
    {
        private MessageBL messageBL;
        private MessageToParents_Message message;
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (accessDenied)
            {
                // User can not access this page
                return;
            }

            messageBL = new MessageBL(UserSchool);
            if (!Page.IsPostBack)
            {
                if (CheckSessionKey(AppConstant.SESSION_MESSAGE))
                {
                    message = (MessageToParents_Message)GetSession(AppConstant.SESSION_MESSAGE);
                    RemoveSession(AppConstant.SESSION_MESSAGE);
                    ViewState[AppConstant.VIEWSTATE_MESSAGEID] = message.MessageId;
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
                if (message.IsConfirmed)
                {
                    LblStatus.Text = "Đã xác nhận";
                }
                else
                {
                    LblStatus.Text = "Đã được xem (chưa xác nhận)";
                }
            }
            else
            {
                Response.Redirect(AppConstant.PAGEPATH_MESSAGE);
            }
        }

        #region Button event handlers
        protected void BtnConfirm_Click(object sender, ImageClickEventArgs e)
        {
            messageBL.ConfirmMessage(message);

            Response.Redirect(AppConstant.PAGEPATH_MESSAGE);
        }

        protected void BtnClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(AppConstant.PAGEPATH_MESSAGE);
        }
        #endregion
    }
}