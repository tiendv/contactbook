using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.OleDb;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;
using AjaxControlToolkit;
using CrystalDecisions;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using Microsoft.Office.Interop.Excel;
using System.Web.Security;
using System.Text;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class MessageModifyPage : BaseContentPage
    {
        #region Fields
        private StudentBL studentBL;
        MessageBL messageBL;
        MessageToParents_Message message;
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
            studentBL = new StudentBL(UserSchool);

            if (!Page.IsPostBack)
            {
                if (RetrieveSessions())
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("Thông báo đến phụ huynh của học sinh ");
                    stringBuilder.Append(message.Student_StudentInClass.Student_Student.FullName);
                    stringBuilder.Append(", mã học sinh ");
                    stringBuilder.Append(message.Student_StudentInClass.Student_Student.StudentCode);
                    LblTitle.Text = stringBuilder.ToString();
                    TxtTitle.Text = message.Title;
                    TxtContent.Text = message.MessageContent;
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
        #endregion

        #region Button event handlers
        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            messageBL.UpdateMessage(message, TxtTitle.Text, TxtContent.Text);
            BacktoPrevPage();
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            BacktoPrevPage();
        }
        #endregion        

        #region Methods
        private void BacktoPrevPage()
        {
            Response.Redirect(AppConstant.PAGEPATH_MESSAGE_LIST);
        }

        private bool RetrieveSessions()
        {
            if (CheckSessionKey(AppConstant.SESSION_MESSAGE))
            {
                message = (MessageToParents_Message)GetSession(AppConstant.SESSION_MESSAGE);
                RemoveSession(AppConstant.SESSION_MESSAGE);
                ViewState[AppConstant.VIEWSTATE_MESSAGEID] = message.MessageId;

                message = messageBL.GetMessage(message.MessageId);
                if (message != null)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}