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
    public partial class ParentsCommentPage : BaseContentPage
    {
        #region Fields
        private bool isSearch;
        private StudentBL studentBL;
        private ParentsCommentBL parentsCommentBL;
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

            parentsCommentBL = new ParentsCommentBL(UserSchool);
            studentBL = new StudentBL(UserSchool);

            if (!Page.IsPostBack)
            {
                isSearch = false;
                BindDropDownLists();
                InitDates();

                BindRptParentsComments();
            }
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

        private void BindRptParentsComments()
        {
            List<TabularParentsComment> tabularParentsComments = null;

            Configuration_Year year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlYears.SelectedValue);
            DateTime dtBeginDate = DateTime.Parse(TxtBeginDate.Text);
            DateTime dtEndDate = DateTime.Parse(TxtEndDate.Text);
            Configuration_CommentStatus commentStatus = null;
            if (DdlXacNhan.SelectedIndex > 0)
            {
                commentStatus = new Configuration_CommentStatus();
                commentStatus.CommentStatusId = Int32.Parse(DdlXacNhan.SelectedValue);
            }
            double dTotalRecords;

            tabularParentsComments = parentsCommentBL.GetTabularParentsComments(LoggedInStudent, year, commentStatus, dtBeginDate, dtEndDate,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            if (tabularParentsComments.Count == 0 && dTotalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRptParentsComments();
                return;
            }

            bool bDisplayData = (tabularParentsComments.Count != 0) ? true : false;
            ProcessDisplayGUI(bDisplayData);

            RptLoiNhanKhan.DataSource = tabularParentsComments;
            RptLoiNhanKhan.DataBind();
            MainDataPager.ItemCount = dTotalRecords;

            int iUnConfirmedFedbackCommentCount = parentsCommentBL.GetUnConfirmedFedbackComment(LoggedInStudent);
            if (iUnConfirmedFedbackCommentCount != 0)
            {
                PnlCommentStatus.Visible = true;
                LblCommentStatus.Text = iUnConfirmedFedbackCommentCount.ToString();
            }
            else
            {
                PnlCommentStatus.Visible = false;
            }
        }

        private void ProcessDisplayGUI(bool displayData)
        {
            RptLoiNhanKhan.Visible = displayData;
            LblSearchResult.Visible = !displayData;

            if (LblSearchResult.Visible)
            {
                if (isSearch)
                {
                    LblSearchResult.Text = "Không tìm thấy thông tin góp ý của phụ huynh"; 
                }
                else
                {
                    LblSearchResult.Text = "Chưa có thông tin góp ý của phụ huynh";
                }
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
            }
            else
            {
                MainDataPager.Visible = true;
            }

            PnlNote.Visible = displayData;
        }

        private void BindDropDownLists()
        {
            BindDDLYears();
            BindDDLCommentStatus();
        }

        private void BindDDLYears()
        {
            List<Configuration_Year> years = studentBL.GetYears(LoggedInStudent);
            DdlYears.DataSource = years;
            DdlYears.DataValueField = "YearId";
            DdlYears.DataTextField = "YearName";
            DdlYears.DataBind();
        }

        private void BindDDLCommentStatus()
        {
            List<Configuration_CommentStatus> commentStatuses = parentsCommentBL.GetCommentStatuses(false);
            DdlXacNhan.DataSource = commentStatuses;
            DdlXacNhan.DataValueField = "CommentStatusId";
            DdlXacNhan.DataTextField = "CommentStatusName";
            DdlXacNhan.DataBind();

            if (DdlXacNhan.Items.Count > 1)
            {
                DdlXacNhan.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }

        private void InitDates()
        {
            DateTime today = DateTime.Now;
            TxtBeginDate.Text = today.AddMonths(-1).ToShortDateString();
            TxtEndDate.Text = today.AddMonths(1).ToShortDateString();

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
                        ParentComment_Comment comment = null;
                        Configuration_CommentStatus commentStatus = new Configuration_CommentStatus();

                        int iCommentId = Int32.Parse(e.CommandArgument.ToString());
                        comment = parentsCommentBL.GetParentsComments(iCommentId);

                        if (comment.CommentStatusId == 2) // Đã xác nhận (PH chưa xem)
                        {
                            commentStatus.CommentStatusId = 3;
                            parentsCommentBL.UpdateParentsComment(comment, commentStatus);
                        }

                        AddSession(AppConstant.SESSION_PARENTSCOMMENTID, comment);

                        Response.Redirect(AppConstant.PAGEPATH_PARENTS_COMMENT_DETAIL);
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
            BindRptParentsComments();
        }

        protected void BtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(AppConstant.PAGEPATH_PARENTS_COMMENT_ADD);
        }

        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            HiddenField HdfRptMaLoiNhanKhan = null;
            foreach (RepeaterItem item in RptLoiNhanKhan.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox CkbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (CkbxSelect.Checked)
                    {
                        HdfRptMaLoiNhanKhan = (HiddenField)item.FindControl("HdfRptMaLoiNhanKhan");
                        ParentComment_Comment comment = parentsCommentBL.GetParentsComments(Int32.Parse(HdfRptMaLoiNhanKhan.Value));
                        AddSession(AppConstant.SESSION_PARENTSCOMMENTID, comment);

                        Response.Redirect(AppConstant.PAGEPATH_PARENTS_COMMENT_EDIT);
                        return;
                    }
                }
            }
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            bool bInfoInUse = false;
            CheckBox ckbxSelect = null;
            HiddenField HdfRptMaLoiNhanKhan = null;
            ParentComment_Comment comment = null;

            foreach (RepeaterItem item in RptLoiNhanKhan.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    ckbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (ckbxSelect.Checked)
                    {
                        HdfRptMaLoiNhanKhan = (HiddenField)item.FindControl("HdfRptMaLoiNhanKhan");
                        comment = new ParentComment_Comment();
                        comment.CommentId = Int32.Parse(HdfRptMaLoiNhanKhan.Value);

                        if (parentsCommentBL.IsDeletable(comment))
                        {
                            parentsCommentBL.DeleteParentsComment(comment);
                        }
                        else
                        {
                            bInfoInUse = true;
                        }
                    }
                }
            }

            isSearch = false;
            BindRptParentsComments();

            if (bInfoInUse)
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
            BindRptParentsComments();
        }
        #endregion
    }
}