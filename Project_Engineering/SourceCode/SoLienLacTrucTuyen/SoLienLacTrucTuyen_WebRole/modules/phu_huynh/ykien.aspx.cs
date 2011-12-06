using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessEntity;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;
using AjaxControlToolkit;

namespace SoLienLacTrucTuyen_WebRole.Modules.ModuleParents
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
            if (isAccessDenied)
            {
                // User can not access this page
                return;
            }

            parentsCommentBL = new ParentsCommentBL(UserSchool);
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
            if (DdlXacNhan.SelectedIndex == 0 || DdlXacNhan.SelectedIndex == 1)
            {
                commentStatus = new Configuration_CommentStatus();
                commentStatus.CommentStatusId = Int32.Parse(DdlXacNhan.SelectedValue);
            }
            double dTotalRecords;

            tabularParentsComments = parentsCommentBL.GetTabularParentsComments(year, commentStatus, dtBeginDate, dtEndDate, 
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
            List<Configuration_Year> lstNamHoc = systemConfigBL.GetListYears();
            DdlYears.DataSource = lstNamHoc;
            DdlYears.DataValueField = "YearId";
            DdlYears.DataTextField = "YearName";
            DdlYears.DataBind();
            DdlYears.SelectedValue = (new SystemConfigBL(UserSchool)).GetCurrentYear().ToString();
        }

        private void BindDDLCommentStatus()
        {
            List<Configuration_CommentStatus> commentStatuses = parentsCommentBL.GetCommentStatuses();
            DdlXacNhan.DataSource = commentStatuses;
            DdlXacNhan.DataValueField = "CommentStatusId";
            DdlXacNhan.DataTextField = "CommentStatusName";
            DdlXacNhan.DataBind();
        }

        private void InitDates()
        {
            DateTime today = DateTime.Now;
            DateTime beginDateOfMonth = new DateTime(today.Year, today.Month, 1);
            TxtBeginDate.Text = beginDateOfMonth.ToShortDateString();
            DateTime dateOfNextMonth = today.AddMonths(1);
            DateTime beginDateOfNextMonth = new DateTime(dateOfNextMonth.Year, dateOfNextMonth.Month, 1);
            DateTime endDateOfMonth = beginDateOfNextMonth.AddDays(-1);
            TxtEndDate.Text = endDateOfMonth.ToShortDateString();
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
                        //int ClassId = Int32.Parse(e.CommandArgument.ToString());
                        //Class_Class lophoc = lopHocBL.GetLopHoc(ClassId);

                        //LblClassNameChiTiet.Text = lophoc.ClassName;
                        //LblFacultyNameChiTiet.Text = (new facultyBL(UserSchool)).GetNganhHoc(lophoc.FacultyId).FacultyName;
                        //LblGradeNameChiTiet.Text = (new grades(UserSchool)).GetKhoiLop(lophoc.GradeId).GradeName;
                        //LblSiSoChiTiet.Text = lophoc.SiSo.ToString();
                        //ModalPopupExtender mPEDetail = (ModalPopupExtender)e.Item.FindControl("MPEDetail");
                        //mPEDetail.Show();

                        //this.HdfClassId.Value = ClassId.ToString();
                        //this.HdfRptLopHocMPEDetail.Value = mPEDetail.ClientID;
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