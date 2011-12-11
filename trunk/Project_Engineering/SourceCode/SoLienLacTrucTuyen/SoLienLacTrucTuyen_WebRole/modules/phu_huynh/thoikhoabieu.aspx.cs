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
using SoLienLacTrucTuyen_WebRole.Modules;

namespace SoLienLacTrucTuyen_WebRole.ModuleParents
{
    public partial class SchedulePage : BaseContentPage
    {
        #region Fields
        private StudentBL studentBL;
        private ClassBL classBL;
        private ScheduleBL scheduleBL;
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
            classBL = new ClassBL(UserSchool);
            scheduleBL = new ScheduleBL(UserSchool);

            if (!Page.IsPostBack)
            {
                BindDropDownLists();
                FillClass();
                BindRptSchedule();
            }
        }
        #endregion

        #region DropDownList event hanlders
        protected void DdlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillClass();
            BindRptSchedule();
        }

        protected void DdlTerm_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindRptSchedule();
        }
        #endregion

        #region Repeater event handlers
        protected void RptMonHocTKB_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item
                || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.DataItem != null)
                {
                    DailySchedule dailySchedule = (DailySchedule)e.Item.DataItem;
                    int YearId = Int32.Parse(DdlYear.SelectedValue);
                    int TermId = Int32.Parse(DdlHocKy.SelectedValue);
                    int DayInWeekId = dailySchedule.DayInWeekId;
                    int ClassId = (int)ViewState[AppConstant.VIEWSTATE_CLASSID];

                    Label lblNghiSang = (Label)e.Item.FindControl("LblNghiSang");
                    SessionedSchedule thoiKhoaBieuBuoiSang = dailySchedule.SessionedSchedules[0];
                    if (thoiKhoaBieuBuoiSang.ListThoiKhoaBieuTheoTiet.Count == 0)
                    {
                        lblNghiSang.Visible = true;
                    }
                    else
                    {
                        lblNghiSang.Visible = false;
                        List<TeachingPeriodSchedule> lstThoiKhoaBieuTheoTiet = thoiKhoaBieuBuoiSang.ListThoiKhoaBieuTheoTiet;
                        Repeater RptMonHocBuoiSang = (Repeater)e.Item.FindControl("RptMonHocBuoiSang");
                        RptMonHocBuoiSang.DataSource = lstThoiKhoaBieuTheoTiet;
                        RptMonHocBuoiSang.DataBind();
                    }

                    Label lblNghiChieu = (Label)e.Item.FindControl("LblNghiChieu");
                    SessionedSchedule thoiKhoaBieuBuoiChieu = dailySchedule.SessionedSchedules[1];
                    if (thoiKhoaBieuBuoiChieu.ListThoiKhoaBieuTheoTiet.Count == 0)
                    {
                        lblNghiChieu.Visible = true;
                    }
                    else
                    {
                        lblNghiChieu.Visible = false;
                        List<TeachingPeriodSchedule> lstThoiKhoaBieuTheoTiet = thoiKhoaBieuBuoiChieu.ListThoiKhoaBieuTheoTiet;
                        Repeater RptMonHocBuoiChieu = (Repeater)e.Item.FindControl("RptMonHocBuoiChieu");
                        RptMonHocBuoiChieu.DataSource = lstThoiKhoaBieuTheoTiet;
                        RptMonHocBuoiChieu.DataBind();
                    }
                }
            }
        }
        #endregion

        #region Methods
        private void BindRptSchedule()
        {
            Configuration_Term term = null;
            Class_Class Class = null;
            List<DailySchedule> dailySchedules;

            // Get search criterias
            term = new Configuration_Term();
            term.TermId = Int32.Parse(DdlHocKy.SelectedValue);
            Class = new Class_Class();
            Class.ClassId = (int)ViewState[AppConstant.VIEWSTATE_CLASSID];
            dailySchedules = scheduleBL.GetDailySchedules(Class, term);

            RptMonHocTKB.DataSource = dailySchedules;
            RptMonHocTKB.DataBind();
        }

        private void BindDropDownLists()
        {
            BindDDlYears();
            BindDDLTerms();
        }

        private void BindDDlYears()
        {
            List<Configuration_Year> years = studentBL.GetYears(LoggedInStudent);
            DdlYear.DataSource = years;
            DdlYear.DataValueField = "YearId";
            DdlYear.DataTextField = "YearName";
            DdlYear.DataBind();
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

        private void FillClass()
        {
            StudentBL studentBL = new StudentBL(UserSchool);
            Configuration_Year year = null;
            if (DdlYear.Items.Count == 0)
            {
                return;
            }

            year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlYear.SelectedValue);
            Class_Class Class = studentBL.GetClass(LoggedInStudent, year);
            ViewState[AppConstant.VIEWSTATE_CLASSID] = Class.ClassId;
            LblClassName.Text = Class.ClassName;
        }
        #endregion
    }
}