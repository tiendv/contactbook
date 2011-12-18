using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessEntity;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using AjaxControlToolkit;

namespace SoLienLacTrucTuyen_WebRole.Modules.ModuleParents
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
                // User can not access this page
                return;
            }

            messageBL = new MessageBL(UserSchool);
            studentBL = new StudentBL(UserSchool);

            if (!Page.IsPostBack)
            {
                isSearch = false;
                BindDropDownLists();
                InitDates();

                BindRptMessages();
            }
        }
        #endregion

        #region Methods
        private void BindRptMessages()
        {
            Configuration_Year year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);
            DateTime dtBeginDate = DateTime.Parse(TxtTuNgay.Text);
            DateTime dtEndDate = DateTime.Parse(TxtDenNgay.Text);
            bool? bConfirmed = null;
            if (DdlXacNhan.SelectedIndex == 0)
            {
                bConfirmed = false;
            }
            else
            {
                if (DdlXacNhan.SelectedIndex == 1)
                {
                    bConfirmed = true;
                }
            }

            double dTotalRecords;
            List<MessageToParents_Message> messages = messageBL.GetMessages(
                year, dtBeginDate, dtEndDate, MembershipStudent, bConfirmed,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            if (messages.Count == 0 && dTotalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRptMessages();
                return;
            }

            bool bDisplayData = (messages.Count != 0) ? true : false;
            ProcessDislayInfo(bDisplayData);

            RptLoiNhanKhan.DataSource = messages;
            RptLoiNhanKhan.DataBind();
            MainDataPager.ItemCount = dTotalRecords;
        }

        private void ProcessDislayInfo(bool bDisplayData)
        {
            RptLoiNhanKhan.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin lời nhắn khẩn";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy lời nhắn khẩn";
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
            BindDDLNamHoc();
            BindDDLXacNhan();
        }

        private void BindDDLNamHoc()
        {
            List<Configuration_Year> years = studentBL.GetYears(MembershipStudent);
            DdlNamHoc.DataSource = years;
            DdlNamHoc.DataValueField = "YearId";
            DdlNamHoc.DataTextField = "YearName";
            DdlNamHoc.DataBind();
        }

        private void BindDDLXacNhan()
        {
            DdlXacNhan.Items.Add(new ListItem("Không", "0"));
            DdlXacNhan.Items.Add(new ListItem("Có", "1"));
            DdlXacNhan.Items.Add(new ListItem("Tất cả", "-1"));
        }

        private void InitDates()
        {
            DateTime today = DateTime.Now;
            DateTime beginDateOfMonth = new DateTime(today.Year, today.Month, 1);
            TxtTuNgay.Text = beginDateOfMonth.ToShortDateString();
            DateTime dateOfNextMonth = today.AddMonths(1);
            DateTime beginDateOfNextMonth = new DateTime(dateOfNextMonth.Year, dateOfNextMonth.Month, 1);
            DateTime endDateOfMonth = beginDateOfNextMonth.AddDays(-1);
            TxtDenNgay.Text = endDateOfMonth.ToShortDateString();
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

                MessageToParents_Message message = (MessageToParents_Message)e.Item.DataItem;
                imgAlreadyReadMsg.Visible = message.IsRead;
                imgUnreadMsg.Visible = !message.IsRead;
                imgWarning.Visible = !message.IsConfirmed && message.IsRead;
                lkbtnTitle.Font.Bold = !message.IsRead;
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
                        Response.Redirect(AppConstant.PAGEPATH_DETAILEDMESSAGE);
                        
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
        }

        protected void BtnOKConfirmItem_Click(object sender, ImageClickEventArgs e)
        {
            MessageToParents_Message message = new MessageToParents_Message();
            message.MessageId = Int32.Parse(this.HdfMaLoiNhanKhan.Value);
            messageBL.ConfirmMessage(message);
            isSearch = false;
            BindRptMessages();
        }

        protected void BtnCancelConfirmItem_Click(object sender, ImageClickEventArgs e)
        {
            MessageToParents_Message message = new MessageToParents_Message();
            message.MessageId = Int32.Parse(this.HdfMaLoiNhanKhan.Value);
            messageBL.UnconfirmMessage(message);
            isSearch = false;
            BindRptMessages();
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