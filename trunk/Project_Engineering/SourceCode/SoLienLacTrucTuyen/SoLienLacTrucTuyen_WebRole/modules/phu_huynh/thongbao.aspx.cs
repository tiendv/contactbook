using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EContactBook.BusinessEntity;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using AjaxControlToolkit;
using SoLienLacTrucTuyen_WebRole.Modules;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.ModuleParents
{
    public partial class MessageForParentsPage : BaseContentPage
    {
        #region Fields
        private bool isSearch;
        private MessageBL messageBL;
        StudentBL studentBL;
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
                isSearch = false;
                BindDropDownLists();
                InitDates();
                GetSearchSessions();
                BindRptMessages();
            }
        }
        #endregion

        #region Methods
        private void BindRptMessages()
        {
            Configuration_Year year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);
            DateTime? dtBeginDate = DateUtils.StringToDateVN(TxtTuNgay.Text);
            DateTime? dtEndDate = DateUtils.StringToDateVN(TxtDenNgay.Text);
            if (dtBeginDate == null)
            {
                BeginDateValidator.IsValid = false;
                return;
            }

            if (dtEndDate == null)
            {
                EndDateValidator.IsValid = false;
                return;
            }

            Configuration_MessageStatus messageStatus = null;
            if (DdlXacNhan.SelectedIndex > 0)
            {
                messageStatus = new Configuration_MessageStatus();
                messageStatus.MessageStatusId = Int32.Parse(DdlXacNhan.SelectedValue);
            }

            double dTotalRecords;
            List<TabularMessage> tabularMessages = messageBL.GetTabularMessages(year, (DateTime)dtBeginDate, (DateTime)dtEndDate, LoggedInStudent, messageStatus,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            if (tabularMessages.Count == 0 && dTotalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRptMessages();
                return;
            }

            bool bDisplayData = (tabularMessages.Count != 0) ? true : false;
            ProcessDislayInfo(bDisplayData);

            RptLoiNhanKhan.DataSource = tabularMessages;
            RptLoiNhanKhan.DataBind();
            MainDataPager.ItemCount = dTotalRecords;

            int iNewMessageCount = messageBL.GetNewMessageCount(LoggedInStudent);
            int iUnconfirmedMessageStatus = messageBL.GetUnconfirmedMessageCount(LoggedInStudent);
            if (iNewMessageCount != 0)
            {
                PnlMessageStatus.Visible = true;
                LblMessageStatus.Text = iNewMessageCount.ToString();
                LblUnconfirmedMessageStatus.Text = iUnconfirmedMessageStatus.ToString();
            }
            else
            {
                PnlMessageStatus.Visible = false;
            }

            PnlNote.Visible = bDisplayData;
        }

        private void ProcessDislayInfo(bool displayData)
        {
            RptLoiNhanKhan.Visible = displayData;
            LblSearchResult.Visible = !displayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông báo";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy thông báo";
                }
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
            }
            else
            {
                MainDataPager.Visible = true;
            }
        }

        private void BindDropDownLists()
        {
            BindDDLYears();
            BindDDLMessageStatuses();
        }

        private void BindDDLYears()
        {
            List<Configuration_Year> years = studentBL.GetYears(LoggedInStudent);
            DdlNamHoc.DataSource = years;
            DdlNamHoc.DataValueField = "YearId";
            DdlNamHoc.DataTextField = "YearName";
            DdlNamHoc.DataBind();
        }

        private void BindDDLMessageStatuses()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);            
            List<Configuration_MessageStatus> messageStatuses = systemConfigBL.GetMessageStatuses();
            DdlXacNhan.DataSource = messageStatuses;
            DdlXacNhan.DataValueField = "MessageStatusId";
            DdlXacNhan.DataTextField = "MessageStatusName";
            DdlXacNhan.DataBind();

            DdlXacNhan.Items.Insert(0, new ListItem("Tất cả", "0")); 
        }

        private void InitDates()
        {
            DateTime today = DateTime.Now;
            TxtTuNgay.Text = today.AddMonths(-1).ToString(AppConstant.DATEFORMAT_DDMMYYYY);
            TxtDenNgay.Text = today.AddMonths(1).ToString(AppConstant.DATEFORMAT_DDMMYYYY);

            // dont remove this code
            //DateTime today = DateTime.Now;
            //DateTime beginDateOfMonth = new DateTime(today.Year, today.Month, 1);
            //TxtTuNgay.Text = beginDateOfMonth.ToShortDateString();
            //DateTime dateOfNextMonth = today.AddMonths(1);
            //DateTime beginDateOfNextMonth = new DateTime(dateOfNextMonth.Year, dateOfNextMonth.Month, 1);
            //DateTime endDateOfMonth = beginDateOfNextMonth.AddDays(-1);
            //TxtDenNgay.Text = endDateOfMonth.ToShortDateString();
        }
        
        private void GetSearchSessions()
        {
            if (CheckSessionKey(AppConstant.SESSION_SELECTED_YEAR)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_FROMDATE)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_TODATE)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_CONFIRMSTATUS))
            {
                ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID] = (Int32)GetSession(AppConstant.SESSION_SELECTED_YEAR);
                RemoveSession(AppConstant.SESSION_SELECTED_YEAR);
                DdlNamHoc.SelectedValue = ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID].ToString();

                ViewState[AppConstant.VIEWSTATE_SELECTED_FROMDATE] = (DateTime)GetSession(AppConstant.SESSION_SELECTED_FROMDATE);
                RemoveSession(AppConstant.SESSION_SELECTED_FROMDATE);
                TxtTuNgay.Text = ((DateTime)ViewState[AppConstant.VIEWSTATE_SELECTED_FROMDATE]).ToShortDateString();

                ViewState[AppConstant.VIEWSTATE_SELECTED_TODATE] = (DateTime)GetSession(AppConstant.SESSION_SELECTED_TODATE);
                RemoveSession(AppConstant.SESSION_SELECTED_TODATE);
                TxtDenNgay.Text = ((DateTime)ViewState[AppConstant.VIEWSTATE_SELECTED_TODATE]).ToShortDateString();

                ViewState[AppConstant.VIEWSTATE_SELECTED_CONFIRMSTATUS] = (Int32)GetSession(AppConstant.SESSION_SELECTED_CONFIRMSTATUS);
                RemoveSession(AppConstant.SESSION_SELECTED_CONFIRMSTATUS);
                DdlXacNhan.SelectedValue = ViewState[AppConstant.VIEWSTATE_SELECTED_CONFIRMSTATUS].ToString();
            }
        }
        
        private void SaveSearchSessions()
        {   
            AddSession(AppConstant.SESSION_SELECTED_YEAR, ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID]);
            AddSession(AppConstant.SESSION_SELECTED_FROMDATE, ViewState[AppConstant.VIEWSTATE_SELECTED_FROMDATE]);
            AddSession(AppConstant.SESSION_SELECTED_TODATE, ViewState[AppConstant.VIEWSTATE_SELECTED_TODATE]);
            AddSession(AppConstant.SESSION_SELECTED_CONFIRMSTATUS, ViewState[AppConstant.VIEWSTATE_SELECTED_CONFIRMSTATUS]);
        }
        #endregion

        #region Repeater event handlers
        protected void RptLoiNhanKhan_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Image imgAlreadyReadMsg = (Image)e.Item.FindControl("ImgAlreadyReadMsg");
                Image imgWarning = (Image)e.Item.FindControl("ImgWarning");
                Image imgUnreadMsg = (Image)e.Item.FindControl("ImgUnreadMsg");
                LinkButton lkbtnTitle = (LinkButton)e.Item.FindControl("LkbtnTitle");

                TabularMessage message = (TabularMessage)e.Item.DataItem;
                bool bRead = (message.MessageStatusId > 1) ? true : false;
                bool bConfirmed = (message.MessageStatusId == 3) ? true : false;

                imgAlreadyReadMsg.Visible = bRead;
                imgUnreadMsg.Visible = !bRead;
                imgWarning.Visible = !bConfirmed && bRead;
                lkbtnTitle.Font.Bold = !bRead;
            }
        }

        protected void RptLoiNhanKhan_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDetailItem":
                    {
                        int iMessageId = Int32.Parse(e.CommandArgument.ToString());
                        MessageToParents_Message message = messageBL.GetMessage(iMessageId);

                        if (!message.IsRead)
                        {
                            messageBL.MarkMessageAsRead(message);
                        }

                        AddSession(AppConstant.SESSION_MESSAGE, message);
                        SaveSearchSessions();
                        Response.Redirect(AppConstant.PAGEPATH_PARENTS_MESSAGE_DETAIL);

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            isSearch = true;
            BindRptMessages();

            // save selections to viewstate
            ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID] = Int32.Parse(DdlNamHoc.SelectedValue);
            ViewState[AppConstant.VIEWSTATE_SELECTED_FROMDATE] = DateTime.Parse(TxtTuNgay.Text);
            ViewState[AppConstant.VIEWSTATE_SELECTED_TODATE] = DateTime.Parse(TxtDenNgay.Text);
            ViewState[AppConstant.VIEWSTATE_SELECTED_CONFIRMSTATUS] = Int32.Parse(DdlXacNhan.SelectedValue);
        }
        #endregion

        #region Pager event handlers
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currentPageIndex;
            BindRptMessages();
        }
        #endregion
    }
}