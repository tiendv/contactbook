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
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class MessageListPage : BaseContentPage
    {
        #region Fields
        private bool isSearch;
        private MessageBL messageBL;
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

            if (sessionExpired)
            {
                FormsAuthentication.SignOut();
                Response.Redirect(FormsAuthentication.LoginUrl);
            }

            messageBL = new MessageBL(UserSchool);
            if (!Page.IsPostBack)
            {
                isSearch = false;
                BindDropDownLists();
                InitDates();
                if (DdlNamHoc.Items.Count != 0)
                {
                    BindRptMessages();
                }
            }

            ProcPermissions();
        }
        #endregion

        #region Methods
        private void ProcPermissions()
        {
            BtnAdd.Visible = accessibilities.Contains(AccessibilityEnum.Add);
            BtnEdit.Visible = accessibilities.Contains(AccessibilityEnum.Modify);
            BtnDelete.Visible = accessibilities.Contains(AccessibilityEnum.Delete);
            PnlPopupConfirmDelete.Visible = accessibilities.Contains(AccessibilityEnum.Delete);
        }

        private void BindRptMessages()
        {
            Configuration_Year year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);
            DateTime? dtBeginDate = DateUtils.StringToDateVN(TxtTuNgay.Text);
            DateTime? dtEndDate = DateUtils.StringToDateVN(TxtDenNgay.Text);
            string strStudentCode = TxtStudentCode.Text;
            Configuration_MessageStatus messageStatus = null;
            if (DdlXacNhan.SelectedIndex > 0)
            {
                messageStatus = new Configuration_MessageStatus();
                messageStatus.MessageStatusId = Int32.Parse(DdlXacNhan.SelectedValue);
            }

            double dTotalRecords;
            List<TabularMessage> tabularMessages = messageBL.GetTabularMessages(LogedInUser, IsFormerTeacher,
                year, (DateTime)dtBeginDate, (DateTime)dtEndDate, strStudentCode, messageStatus, true, 
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
        }

        private void ProcessDislayInfo(bool bDisplayData)
        {
            RptLoiNhanKhan.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin thông báo";
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
            BindDDLNamHoc();
            BindDDLMessageStatuses();
        }

        private void BindDDLNamHoc()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<Configuration_Year> years = systemConfigBL.GetListYears();
            DdlNamHoc.DataSource = years;
            DdlNamHoc.DataValueField = "YearId";
            DdlNamHoc.DataTextField = "YearName";
            DdlNamHoc.DataBind();

            if (DdlNamHoc.Items.Count != 0)
            {
                BtnSearch.Enabled = true;
                BtnSearch.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SEARCH;

                BtnAdd.Enabled = true;
                BtnAdd.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_ADD;

                DdlNamHoc.SelectedValue = (new SystemConfigBL(UserSchool)).GetLastedYear().ToString();
            }
            else
            {
                BtnSearch.Enabled = false;
                BtnSearch.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SEARCH_DISABLE;

                BtnAdd.Enabled = false;
                BtnAdd.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_ADD_DISABLE;

                ProcessDislayInfo(false);
            }
        }

        private void BindDDLMessageStatuses()
        {
            List<Configuration_MessageStatus> messageStatuses = messageBL.GetMessageStatuses(true);
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
        #endregion

        #region Repeater event handlers
        protected void RptLoiNhanKhan_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                e.Item.FindControl("thSelectAll").Visible = (accessibilities.Contains(AccessibilityEnum.Modify) || accessibilities.Contains(AccessibilityEnum.Delete));
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.FindControl("tdSelect").Visible = (accessibilities.Contains(AccessibilityEnum.Modify) || accessibilities.Contains(AccessibilityEnum.Delete));
            }
        }

        protected void RptLoiNhanKhan_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDetailItem":
                    {
                        MessageToParents_Message message = new MessageToParents_Message();
                        message.MessageId = Int32.Parse(e.CommandArgument.ToString());

                        AddSession(AppConstant.SESSION_MESSAGE, message);
                        Response.Redirect(AppConstant.PAGEPATH_MESSAGE_DETAIL);

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

        protected void BtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(AppConstant.PAGEPATH_MESSAGE_ADD);
        }

        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            HiddenField hdfRptMaLoiNhanKhan = null;
            MessageToParents_Message message = null;
            foreach (RepeaterItem item in RptLoiNhanKhan.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox CkbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (CkbxSelect.Checked)
                    {
                        hdfRptMaLoiNhanKhan = (HiddenField)item.FindControl("HdfRptMaLoiNhanKhan");
                        message = new MessageToParents_Message();
                        message.MessageId = Int32.Parse(hdfRptMaLoiNhanKhan.Value);
                        AddSession(AppConstant.SESSION_MESSAGE, message);
                        Response.Redirect(AppConstant.PAGEPATH_MESSAGE_MODIFY);
                    }
                }
            }
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            bool bIsConfirmed = false;
            HiddenField hdfRptMaLoiNhanKhan = null;
            MessageToParents_Message message = null;
            foreach (RepeaterItem item in RptLoiNhanKhan.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox CkbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (CkbxSelect.Checked)
                    {
                        hdfRptMaLoiNhanKhan = (HiddenField)item.FindControl("HdfRptMaLoiNhanKhan");
                        message = new MessageToParents_Message();
                        message.MessageId = Int32.Parse(hdfRptMaLoiNhanKhan.Value);

                        if (messageBL.IsDeletable(message))
                        {
                            messageBL.DeleteMessage(message);
                        }             
                        else
                        {
                            bIsConfirmed = true;
                        }
                    }
                }
            }
            
            isSearch = false;
            BindRptMessages();

            if (bIsConfirmed)
            {
                MPEInfoInUse.Show();
            }
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