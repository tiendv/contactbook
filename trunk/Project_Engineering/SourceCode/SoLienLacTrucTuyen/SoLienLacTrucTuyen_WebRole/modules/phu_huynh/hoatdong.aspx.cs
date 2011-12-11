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
using SoLienLacTrucTuyen_WebRole.Modules;

namespace SoLienLacTrucTuyen_WebRole.ModuleParents
{
    public partial class StudentActivityPage : BaseContentPage
    {
        #region Fields
        private StudentBL studentBL;
        private SystemConfigBL systemConfigBL;
        private StudentActivityBL studentActivityBL;
        private AttitudeBL attitudeBL;
        private bool isSearch;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (accessDenied)
            {
                return;
            }

            studentBL = new StudentBL(UserSchool);
            systemConfigBL = new SystemConfigBL(UserSchool);
            studentActivityBL = new StudentActivityBL(UserSchool);
            attitudeBL = new AttitudeBL(UserSchool);

            if (!Page.IsPostBack)
            {
                if (LoggedInStudent != null)
                {
                    BindDropDownLists();
                    InitDates();
                    isSearch = false;
                    BindRptStudentActivities();
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
            BindDDLTerms();
        }

        private void BindDDLYears()
        {            
            List<Configuration_Year> years = studentBL.GetYears(LoggedInStudent);
            DdlNamHoc.DataSource = years;
            DdlNamHoc.DataValueField = "YearId";
            DdlNamHoc.DataTextField = "YearName";
            DdlNamHoc.DataBind();
        }

        private void BindDDLTerms()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<Configuration_Term> terms = systemConfigBL.GetListTerms();
            DdlHocKy.DataSource = terms;
            DdlHocKy.DataValueField = "TermId";
            DdlHocKy.DataTextField = "TermName";
            DdlHocKy.DataBind();
            DdlHocKy.SelectedValue = (new SystemConfigBL(UserSchool)).GetCurrentTerm().ToString();
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

        private void BindRptStudentActivities()
        {
            Configuration_Year year = null;
            Configuration_Term term = null;
            DateTime dtBeginDate;
            DateTime dtEndDate;
            double dTotalRecords;
            List<TabularStudentActivity> tabularStudentActivities;
            
            if (DdlNamHoc.SelectedIndex >= 0)
            {
                year = new Configuration_Year();
                year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);
            }
            if (DdlHocKy.SelectedIndex >= 0)
            {
                term = new Configuration_Term();
                term.TermId = Int32.Parse(DdlHocKy.SelectedValue);
            }
            dtBeginDate = DateTime.Parse(TxtTuNgay.Text);
            dtEndDate = DateTime.Parse(TxtDenNgay.Text);

            tabularStudentActivities = studentActivityBL.GetTabularStudentActivities(
                LoggedInStudent, year, term, dtBeginDate, dtEndDate,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            if (dTotalRecords != 0 && tabularStudentActivities.Count == 0)
            {
                MainDataPager.CurrentIndex--;
                BindRptStudentActivities();
                return;
            }

            bool bDisplayData = (tabularStudentActivities.Count != 0) ? true : false;
            ProcessDislayInfo(bDisplayData);

            RptHoatDong.DataSource = tabularStudentActivities;
            RptHoatDong.DataBind();
            MainDataPager.ItemCount = dTotalRecords;
        }

        private void ProcessDislayInfo(bool bDisplayData)
        {
            RptHoatDong.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin hoạt động";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy hoạt động";
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

        #region DataPager event handlers
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            MainDataPager.CurrentIndex = currentPageIndex;
            BindRptStudentActivities();
        }
        #endregion

        #region Button event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            isSearch = true;
            BindRptStudentActivities();
        }
        #endregion
    }
}