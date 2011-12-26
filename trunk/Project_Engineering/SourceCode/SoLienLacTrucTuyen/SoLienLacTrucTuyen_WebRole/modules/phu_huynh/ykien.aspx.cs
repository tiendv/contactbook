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
            ProcessDislayInfo(bDisplayData);

            RptLoiNhanKhan.DataSource = tabularParentsComments;
            RptLoiNhanKhan.DataBind();
            MainDataPager.ItemCount = dTotalRecords;
        }

        private void ProcessDislayInfo(bool bDisplayData)
        {
            RptLoiNhanKhan.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;
            PnlPopupConfirmDelete.Visible = bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin góp ý của phụ huynh";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy thông tin góp ý của phụ huynh";
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
            List<Configuration_CommentStatus> commentStatuses = parentsCommentBL.GetCommentStatuses();
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
            DateTime dtToday = DateTime.Now;
            DateTime dtBeginDateOfMonth = new DateTime(dtToday.Year, dtToday.Month, 1);
            TxtBeginDate.Text = dtBeginDateOfMonth.ToShortDateString();
            DateTime dtDateOfNextMonth = dtToday.AddMonths(1);
            DateTime dtBeginDateOfNextMonth = new DateTime(dtDateOfNextMonth.Year, dtDateOfNextMonth.Month, 1);
            DateTime dtEndDateOfMonth = dtBeginDateOfNextMonth.AddDays(-1);
            TxtEndDate.Text = dtEndDateOfMonth.ToShortDateString();
        }
        #endregion
        
        #region Repeater event handlers
        protected void RptLoiNhanKhan_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TabularParentsComment tabularParentsComment = (TabularParentsComment)e.Item.DataItem;
                if (tabularParentsComment.Feedback != "")
                {
                    ImageButton btnDeleteItem = (ImageButton)e.Item.FindControl("BtnDeleteItem");
                    btnDeleteItem.ImageUrl = "~/Styles/Images/button_delete_disable.png";
                    btnDeleteItem.Enabled = false;

                    ImageButton btnEditItem = (ImageButton)e.Item.FindControl("BtnEditItem");
                    btnEditItem.ImageUrl = "~/Styles/Images/button_edit_disable.png";
                    btnEditItem.Enabled = false;
                }
            }
        }

        protected void RptLoiNhanKhan_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDetailItem":
                    {
                        int iCommentId = Int32.Parse(e.CommandArgument.ToString());
                        
                        ParentComment_Comment comment = parentsCommentBL.GetParentsComments(iCommentId);
                        AddSession(AppConstant.SESSION_PARENTSCOMMENTID, comment);

                        Response.Redirect(AppConstant.PAGEPATH_DETAILEDCOMMENT);
                        break;
                    }
                case "CmdDeleteItem":
                    {
                        this.LblConfirmDelete.Text = "Bạn có chắc xóa góp ý <b>" + e.CommandArgument + "</b> này không?";
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        HiddenField hdfRptCommentId = (HiddenField)e.Item.FindControl("HdfRptMaLoiNhanKhan");
                        this.HdfCommentId.Value = hdfRptCommentId.Value;
                        this.HdfRptCommentMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }                
                case "CmdEditItem":
                    {
                        int iCommentId = Int32.Parse(e.CommandArgument.ToString());

                        ParentComment_Comment comment = parentsCommentBL.GetParentsComments(iCommentId);
                        AddSession(AppConstant.SESSION_PARENTSCOMMENTID, comment);

                        Response.Redirect(AppConstant.PAGEPATH_EDITCOMMENT);
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
            Response.Redirect(AppConstant.PAGEPATH_ADDCOMMENT);
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            ParentComment_Comment comment = new ParentComment_Comment();
            comment.CommentId = Int32.Parse(HdfCommentId.Value);
            parentsCommentBL.DeleteParentsComment(comment);
            isSearch = false;
            BindRptParentsComments();
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