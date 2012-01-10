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
    public partial class ParentsCommentPage : BaseContentPage
    {
        #region Fields
        private bool isSearch;
        private ParentsCommentBL parentsCommentBL;
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

            parentsCommentBL = new ParentsCommentBL(UserSchool);
            if (!Page.IsPostBack)
            {
                isSearch = false;
                BindDropDownLists();
                InitDates();

                if (DdlYears.Items.Count != 0)
                {
                    BindRptParentsComments();
                }
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

            tabularParentsComments = parentsCommentBL.GetTabularParentsComments(year, commentStatus, dtBeginDate, dtEndDate, 
                true, MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

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
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<Configuration_Year> years = systemConfigBL.GetListYears();
            DdlYears.DataSource = years;
            DdlYears.DataValueField = "YearId";
            DdlYears.DataTextField = "YearName";
            DdlYears.DataBind();

            if (DdlYears.Items.Count == 0)
            {
                BtnSearch.Enabled = false;
                BtnSearch.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SEARCH_DISABLE;

                ProcessDislayInfo(false);
            }
            else
            {
                BtnSearch.Enabled = true;
                BtnSearch.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SEARCH;

                DdlYears.SelectedValue = (new SystemConfigBL(UserSchool)).GetLastedYear().ToString();
            }
        }

        private void BindDDLCommentStatus()
        {
            List<Configuration_CommentStatus> commentStatuses = parentsCommentBL.GetCommentStatuses(true);
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
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //LopHocInfo lopHoc = (LopHocInfo)e.Item.DataItem;
                //if (lopHoc != null)
                //{
                //    int ClassId = lopHoc.ClassId;
                //    if (!lopHocBL.CheckCanDeleteLopHoc(ClassId))
                //    {
                //        ImageButton btnDeleteItem = (ImageButton)e.Item.FindControl("BtnDeleteItem");
                //        btnDeleteItem.ImageUrl = "~/Styles/Images/button_delete_disable.png";
                //        btnDeleteItem.Enabled = false;
                //    }
                //}
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

                        Response.Redirect(AppConstant.PAGEPATH_DETAILEDPARENTSCOMMENTS);
                        break;
                    }
                case "CmdEditItem":
                    {
                        int iCommentId = Int32.Parse(e.CommandArgument.ToString());

                        ParentComment_Comment comment = parentsCommentBL.GetParentsComments(iCommentId);
                        AddSession(AppConstant.SESSION_PARENTSCOMMENTID, comment);

                        Response.Redirect(AppConstant.PAGEPATH_FEEDBACKPARENTSCOMMENTS);
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