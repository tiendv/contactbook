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
using SoLienLacTrucTuyen_WebRole.Modules;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.ModuleParents
{
    public partial class StudentRatingPage : BaseContentPage
    {
        #region Fields
        private StudentBL studentBL;
        private MarkTypeBL markTypeBL;
        private StudyingResultBL studyingResultBL;
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
            markTypeBL = new MarkTypeBL(UserSchool);
            studyingResultBL = new StudyingResultBL(UserSchool);

            if (!Page.IsPostBack)
            {
                if (LoggedInStudent != null)
                {
                    BindDropDownLists();
                    BindRptMarkTypes();
                    BindRptKetQuaDiem();
                    BindRepeaterDanhHieu();
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
        }

        private void BindRptMarkTypes()
        {
            if (DdlNamHoc.Items.Count != 0)
            {
                Configuration_Year year = new Configuration_Year();
                Class_Class Class = null;

                year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);
                Class = studentBL.GetClass(LoggedInStudent, year);

                List<Category_MarkType> markTypes = markTypeBL.GetListMarkTypes(Class.Category_Grade);
                this.RptLoaiDiem.DataSource = markTypes;
            }
            this.RptLoaiDiem.DataBind();
        }

        private void BindRptKetQuaDiem()
        {
            Configuration_Year year = new Configuration_Year();
            Configuration_Term term = new Configuration_Term();

            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);
            term.TermId = Int32.Parse(DdlHocKy.SelectedValue);

            double dTotalRecords;
            List<TabularSubjectTermResult> lstTbKetQuaMonHoc;
            lstTbKetQuaMonHoc = studyingResultBL.GetTabularSubjectTermResults(LoggedInStudent, year, term,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            bool bDisplayData = (lstTbKetQuaMonHoc.Count != 0) ? true : false;
            RptKetQuaDiem.Visible = bDisplayData;
            MainDataPager.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            RptLoaiDiem.Visible = bDisplayData;
            tdKQHocTapSTT.Visible = bDisplayData;
            tdKQHocTapMonHoc.Visible = bDisplayData;
            tdKQHocTapDTB.Visible = bDisplayData;

            this.RptKetQuaDiem.DataSource = lstTbKetQuaMonHoc;
            this.RptKetQuaDiem.DataBind();
            MainDataPager.ItemCount = dTotalRecords;
        }

        private void BindRepeaterDanhHieu()
        {
            Configuration_Year year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);

            double dTotalRecords;
            List<TabularTermStudentResult> tabularTermStudentResults;
            tabularTermStudentResults = studyingResultBL.GetTabularTermStudentResults(LoggedInStudent, year,
                DataPagerDanhHieu.CurrentIndex, DataPagerDanhHieu.PageSize, out dTotalRecords);

            RptDanhHieu.DataSource = tabularTermStudentResults;
            RptDanhHieu.DataBind();
            DataPagerDanhHieu.ItemCount = dTotalRecords;

            bool bDisplayed = (tabularTermStudentResults.Count != 0) ? true : false;
            RptDanhHieu.Visible = bDisplayed;
        }
        #endregion

        #region Repeater event handlers
        protected void RptKetQuaDiem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Student_TermSubjectMark termSubjectedMark = null;
            if (e.Item.ItemType == ListItemType.Item
                || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Control control = e.Item.FindControl("HdfMaDiemMonHK");
                if (control != null)
                {
                    termSubjectedMark = new Student_TermSubjectMark();
                    termSubjectedMark.TermSubjectMarkId = Int32.Parse(((HiddenField)control).Value);
                    List<StrDiemMonHocLoaiDiem> lstStrDiemMonHocLoaiDiem;
                    lstStrDiemMonHocLoaiDiem = studyingResultBL.GetSubjectMarks(termSubjectedMark);
                    Repeater rptDiemMonHoc = (Repeater)e.Item.FindControl("RptDiemTheoMonHoc");
                    rptDiemMonHoc.DataSource = lstStrDiemMonHocLoaiDiem;
                    rptDiemMonHoc.DataBind();
                }
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            BindRptKetQuaDiem();
            BindRepeaterDanhHieu();
        }
        #endregion

        #region DataPager event handlers
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currentPageIndex;
            BindRptKetQuaDiem();
        }
        #endregion
    }
}
