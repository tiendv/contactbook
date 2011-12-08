using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;
using AjaxControlToolkit;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

namespace SoLienLacTrucTuyen_WebRole.Modules.ModuleParents
{
    public partial class StudentAbsentPage : BaseContentPage
    {
        #region Fields
        private StudentBL studentBL;
        private AbsentBL absentBL;
        private SystemConfigBL systemConfigBL;
        private bool isSearch;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (isAccessDenied)
            {
                return;
            }

            studentBL = new StudentBL(UserSchool);
            absentBL = new AbsentBL(UserSchool);

            systemConfigBL = new SystemConfigBL(UserSchool);

            if (!Page.IsPostBack)
            {
                if (MembershipStudent != null)
                {
                    BindDropDownLists();
                    InitDates();
                    isSearch = false;
                    BindRptStudentAbsents();
                }
                else
                {
                    Response.Redirect(AppConstant.PAGEPATH_HOMEPAGE);
                }
            }
        }
        #endregion

        #region Methods
        private void BindDropDownLists()
        {
            BindDDLYears();
            BindDDDLTerms();
        }

        private void BindDDLYears()
        {
            List<Configuration_Year> years = studentBL.GetYears(MembershipStudent);
            DdlNamHoc.DataSource = years;
            DdlNamHoc.DataValueField = "YearId";
            DdlNamHoc.DataTextField = "YearName";
            DdlNamHoc.DataBind();
        }

        private void BindDDDLTerms()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<Configuration_Term> terms = systemConfigBL.GetListTerms();
            DdlHocKy.DataSource = terms;
            DdlHocKy.DataValueField = "TermId";
            DdlHocKy.DataTextField = "TermName";
            DdlHocKy.DataBind();
            DdlHocKy.SelectedValue = systemConfigBL.GetCurrentTerm().ToString();
        }

        private void InitDates()
        {
            DateTime dtToday = DateTime.Now;
            DateTime dtBeginDateOfMonth = new DateTime(dtToday.Year, dtToday.Month, 1);
            DateTime dtDateOfNextMonth = dtToday.AddMonths(1);
            DateTime dtBeginDateOfNextMonth = new DateTime(dtDateOfNextMonth.Year, dtDateOfNextMonth.Month, 1);
            DateTime dtEndDateOfMonth = dtBeginDateOfNextMonth.AddDays(-1);

            TxtTuNgay.Text = dtBeginDateOfMonth.ToShortDateString();
            TxtDenNgay.Text = dtEndDateOfMonth.ToShortDateString();
        }

        private void BindRptStudentAbsents()
        {
            Configuration_Year year = null;
            Configuration_Term term = null;
            double dTotalRecords;
            List<TabularAbsent> tabularAbsents;

            year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);
            term = new Configuration_Term();
            term.TermId = Int32.Parse(DdlHocKy.SelectedValue);
            DateTime dtBeginDate = DateTime.Parse(TxtTuNgay.Text);
            DateTime dtEndDate = DateTime.Parse(TxtDenNgay.Text);

            tabularAbsents = absentBL.GetTabularAbsents(MembershipStudent, year, term, dtBeginDate, dtEndDate,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            if (dTotalRecords != 0 && tabularAbsents.Count == 0)
            {
                MainDataPager.CurrentIndex--;
                BindRptStudentAbsents();
                return;
            }

            bool bDisplayData = (tabularAbsents.Count != 0) ? true : false;
            ProcessDislayInfo(bDisplayData);
            RptNgayNghi.DataSource = tabularAbsents;
            RptNgayNghi.DataBind();
            MainDataPager.ItemCount = dTotalRecords;
        }

        private void ProcessDislayInfo(bool bDisplayData)
        {
            PnlPopupConfirm.Visible = bDisplayData;
            RptNgayNghi.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin ngày nghỉ học";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy ngày nghỉ học";
                }
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
            }
            else
            {
                MainDataPager.Visible = true;
            }
        }
        #endregion

        #region Repeater event handlers
        protected void RptNgayNghi_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Student_Absent absent = null;

            if (lstAccessibilities.Contains(AccessibilityEnum.Delete))
            {
                if (e.Item.ItemType == ListItemType.Item
                    || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (e.Item.DataItem != null)
                    {
                        Control control = e.Item.FindControl("HdfRptMaNgayNghiHoc");
                        if (control != null)
                        {
                            absent = new Student_Absent();
                            absent.AbsentId = Int32.Parse(((HiddenField)control).Value);
                            if (absentBL.Confirmed(absent))
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
                }
            }
        }

        protected void RptNgayNghi_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDeleteItem":
                    {
                        LblConfirmDelete.Text = "Bạn có chắc xác nhận ngày nghỉ học này không?";
                        ModalPopupExtender mPEConfirm = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEConfirm.Show();

                        HiddenField hdfRptMaNgayNghiHoc = (HiddenField)e.Item.FindControl("HdfRptMaNgayNghiHoc");
                        HdfMaNgayNghiHoc.Value = hdfRptMaNgayNghiHoc.Value;

                        HdfRptAbsentMPEConfirm.Value = mPEConfirm.ClientID;
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
            BindRptStudentAbsents();
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            int maNgayNghiHoc = Int32.Parse(this.HdfMaNgayNghiHoc.Value);
            absentBL.DeleteAbsent(maNgayNghiHoc);
            isSearch = false;
            BindRptStudentAbsents();
        }
        #endregion

        #region DataPager event handlers
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            MainDataPager.CurrentIndex = currentPageIndex;
            BindRptStudentAbsents();
        }
        #endregion
    }
}