using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;
using AjaxControlToolkit;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using SoLienLacTrucTuyen_WebRole.Modules;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.ModuleParents
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
            if (accessDenied)
            {
                return;
            }

            if (sessionExpired)
            {
                FormsAuthentication.SignOut();
                Response.Redirect(FormsAuthentication.LoginUrl);
            }

            studentBL = new StudentBL(UserSchool);
            absentBL = new AbsentBL(UserSchool);
            systemConfigBL = new SystemConfigBL(UserSchool);

            if (!Page.IsPostBack)
            {
                if (LoggedInStudent != null)
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
            List<Configuration_Year> years = studentBL.GetYears(LoggedInStudent);
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

            tabularAbsents = absentBL.GetTabularAbsents(LoggedInStudent, year, term, (DateTime)dtBeginDate, (DateTime)dtEndDate,
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
            PnlPopupUnConfirm.Visible = bDisplayData;
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
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.DataItem != null)
                {
                    Student_Absent absent = new Student_Absent();
                    absent.AbsentId = Int32.Parse(((HiddenField)e.Item.FindControl("HdfRptMaNgayNghiHoc")).Value);
                    if (absentBL.Confirmed(absent))
                    {
                        ImageButton btnConfirm = (ImageButton)e.Item.FindControl("BtnConfirm");
                        btnConfirm.Visible = false;
                    }
                    else
                    {
                        ImageButton btnUnConfirm = (ImageButton)e.Item.FindControl("BtnUnConfirm");
                        btnUnConfirm.Visible = false;
                    }
                }

            }
        }

        protected void RptNgayNghi_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdEditItem":
                    {
                        LblConfirmDelete.Text = "Bạn có chắc xác nhận ngày nghỉ học này không?";
                        ModalPopupExtender mPEConfirm = (ModalPopupExtender)e.Item.FindControl("MPEConfirm");
                        mPEConfirm.Show();

                        HiddenField hdfRptMaNgayNghiHoc = (HiddenField)e.Item.FindControl("HdfRptMaNgayNghiHoc");
                        HdfMaNgayNghiHoc.Value = hdfRptMaNgayNghiHoc.Value;

                        HdfRptAbsentMPEConfirm.Value = mPEConfirm.ClientID;
                        break;
                    }
                case "CmdUnConfirm":
                    {
                        LblUnConfirm.Text = "Bạn có chắc hủy xác nhận ngày nghỉ học này không?";
                        ModalPopupExtender mPEUnConfirm = (ModalPopupExtender)e.Item.FindControl("MPEUnConfirm");
                        mPEUnConfirm.Show();

                        HiddenField hdfRptAbsentId = (HiddenField)e.Item.FindControl("HdfRptMaNgayNghiHoc");
                        HdfMaNgayNghiHoc.Value = hdfRptAbsentId.Value;

                        HdfRptAbsentMPEUnConfirm.Value = mPEUnConfirm.ClientID;
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
            Student_Absent absent = new Student_Absent();
            absent.AbsentId = Int32.Parse(this.HdfMaNgayNghiHoc.Value);
            absentBL.ConfirmAbsent(absent);
            isSearch = false;
            BindRptStudentAbsents();
        }

        protected void BtnOKUnConfirm_Click(object sender, ImageClickEventArgs e)
        {
            Student_Absent absent = new Student_Absent();
            absent.AbsentId = Int32.Parse(this.HdfMaNgayNghiHoc.Value);
            absentBL.UnconfirmAbsent(absent);
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