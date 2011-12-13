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
        private LoiNhanKhanBL messageBL;
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

            messageBL = new LoiNhanKhanBL(UserSchool);
            studentBL = new StudentBL(UserSchool);

            if (!Page.IsPostBack)
            {
                isSearch = false;
                BindDropDownLists();
                InitDates();

                BindRptLoiNhanKhan();
            }
        }
        #endregion

        #region Methods
        private void BindRptLoiNhanKhan()
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
                BindRptLoiNhanKhan();
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
            PnlPopupConfirmMessage.Visible = bDisplayData;
            PnlPopupCancelConfirmMessage.Visible = bDisplayData;
            PnlPopupDetail.Visible = bDisplayData;
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
                MessageToParents_Message message = (MessageToParents_Message)e.Item.DataItem;
                ImageButton btnConfirmItem = (ImageButton)e.Item.FindControl("BtnConfirmItem");
                btnConfirmItem.Visible = !message.IsConfirmed;
                ImageButton btnCancelConfirmItem = (ImageButton)e.Item.FindControl("BtnCancelConfirmItem");
                btnCancelConfirmItem.Visible = message.IsConfirmed;
            }
        }

        protected void RptLoiNhanKhan_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdConfirmItem":
                    {
                        this.LblConfirmMessage.Text = "Bạn có chắc xác nhận lời nhắn khẩn <b>" + e.CommandArgument + "</b> này không?";
                        ModalPopupExtender mPEConfirm = (ModalPopupExtender)e.Item.FindControl("MPEConfirm");
                        mPEConfirm.Show();

                        HiddenField hdfRptMaLoiNhanKhan = (HiddenField)e.Item.FindControl("HdfRptMaLoiNhanKhan");
                        this.HdfMaLoiNhanKhan.Value = hdfRptMaLoiNhanKhan.Value;

                        this.HdfRptLoiNhanKhanMPEConfirm.Value = mPEConfirm.ClientID;

                        break;
                    }
                case "CmdCancelConfirmItem":
                    {
                        this.LblCancelConfirmMessage.Text = "Bạn có chắc hủy xác nhận lời nhắn khẩn <b>" + e.CommandArgument + "</b> này không?";
                        ModalPopupExtender mPECancelConfirm = (ModalPopupExtender)e.Item.FindControl("MPECancelConfirm");
                        mPECancelConfirm.Show();

                        HiddenField hdfRptMaLoiNhanKhan = (HiddenField)e.Item.FindControl("HdfRptMaLoiNhanKhan");
                        this.HdfMaLoiNhanKhan.Value = hdfRptMaLoiNhanKhan.Value;

                        this.HdfRptLoiNhanKhanMPECancelConfirm.Value = mPECancelConfirm.ClientID;

                        break;
                    }
                case "CmdDetailItem":
                    {
                        int iMessageId = Int32.Parse(e.CommandArgument.ToString());
                        MessageToParents_Message message = messageBL.GetLoiNhanKhan(iMessageId);

                        LblTitle.Text = message.Title;
                        LblContent.Text = message.MessageContent;

                        ModalPopupExtender mPEDetail = (ModalPopupExtender)e.Item.FindControl("MPEDetail");
                        mPEDetail.Show();

                        this.HdfMaLoiNhanKhan.Value = iMessageId.ToString();
                        this.HdfRptLoiNhanKhanMPEDetail.Value = mPEDetail.ClientID;
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
            BindRptLoiNhanKhan();
        }

        protected void BtnOKConfirmItem_Click(object sender, ImageClickEventArgs e)
        {
            MessageToParents_Message message = new MessageToParents_Message();
            message.MessageId = Int32.Parse(this.HdfMaLoiNhanKhan.Value);
            messageBL.ConfirmMessage(message);
            isSearch = false;
            BindRptLoiNhanKhan();
        }

        protected void BtnCancelConfirmItem_Click(object sender, ImageClickEventArgs e)
        {
            MessageToParents_Message message = new MessageToParents_Message();
            message.MessageId = Int32.Parse(this.HdfMaLoiNhanKhan.Value);
            messageBL.UnconfirmMessage(message);
            isSearch = false;
            BindRptLoiNhanKhan();
        }
        #endregion

        #region Pager event handlers
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currentPageIndex;
            BindRptLoiNhanKhan();
        }
        #endregion
    }
}