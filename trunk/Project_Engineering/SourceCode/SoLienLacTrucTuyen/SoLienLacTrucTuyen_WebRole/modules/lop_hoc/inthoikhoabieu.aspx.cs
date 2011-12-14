using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;
using AjaxControlToolkit;
using CrystalDecisions;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class PrintTermPage : BaseContentPage
    {
        #region Variables

        private ScheduleBL schiduleBL;
        private ReportDocument RptDocument = new ReportDocument();
        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            DataSet dsClass = new DataSet();
            if (!Page.IsPostBack)
            {
                List<DailySchedule> tabularDailySchedule = new List<DailySchedule>();
                double dTotalRecords;

                Configuration_Year year = (Configuration_Year)GetSession(AppConstant.SESSION_SELECTED_YEAR);
                RemoveSession(AppConstant.SESSION_SELECTED_YEAR);                

                Category_Faculty faculty = (Category_Faculty)GetSession(AppConstant.SESSION_SELECTED_FACULTY);
                RemoveSession(AppConstant.SESSION_SELECTED_FACULTY);

                Category_Grade grade = (Category_Grade)GetSession(AppConstant.SESSION_SELECTED_GRADE);
                RemoveSession(AppConstant.SESSION_SELECTED_GRADE);

                Class_Class classes = (Class_Class)GetSession(AppConstant.SESSION_SELECTED_CLASS);
                RemoveSession(AppConstant.SESSION_SELECTED_CLASS);

                Configuration_Term term = (Configuration_Term)GetSession(AppConstant.SESSION_SELECTED_TERM);
                RemoveSession(AppConstant.SESSION_SELECTED_TERM);

                // Get data
                schiduleBL = new ScheduleBL(UserSchool);
                tabularDailySchedule = schiduleBL.GetDailySchedules(classes, term);

                DataTable dtSource = new DataTable();
                dtSource.Columns.Add("DayInWeekName", Type.GetType("System.String"));
                dtSource.Columns.Add("Morning_StringDetailTeachingPeriod", Type.GetType("System.String"));
                dtSource.Columns.Add("Morning_SubjectName", Type.GetType("System.String"));
                dtSource.Columns.Add("Morning_TeacherName", Type.GetType("System.String"));
                dtSource.Columns.Add("Afternoon_StringDetailTeachingPeriod", Type.GetType("System.String"));
                dtSource.Columns.Add("Afternoon_SubjectName", Type.GetType("System.String"));
                dtSource.Columns.Add("Afternoon_TeacherName", Type.GetType("System.String"));                
                int _count;
                for (int i = 0; i < tabularDailySchedule.Count; i++)
                {
                    if (tabularDailySchedule[i].SessionedSchedules[0].ListThoiKhoaBieuTheoTiet.Count >
                        tabularDailySchedule[i].SessionedSchedules[1].ListThoiKhoaBieuTheoTiet.Count)
                        _count = tabularDailySchedule[i].SessionedSchedules[0].ListThoiKhoaBieuTheoTiet.Count;
                    else
                        _count = tabularDailySchedule[i].SessionedSchedules[1].ListThoiKhoaBieuTheoTiet.Count;
                    // Morning
                    for (int j = 0; j <_count; j++)
                    {
                        DataRow drSource = dtSource.NewRow();
                        drSource["DayInWeekName"] = tabularDailySchedule[i].DayInWeekName;
                        // Morning
                        if (j < tabularDailySchedule[i].SessionedSchedules[0].ListThoiKhoaBieuTheoTiet.Count)
                        {
                            drSource["Morning_StringDetailTeachingPeriod"] = tabularDailySchedule[i].SessionedSchedules[0].ListThoiKhoaBieuTheoTiet[j].StringDetailTeachingPeriod;
                            drSource["Morning_SubjectName"] = tabularDailySchedule[i].SessionedSchedules[0].ListThoiKhoaBieuTheoTiet[j].SubjectName;
                            drSource["Morning_TeacherName"] = tabularDailySchedule[i].SessionedSchedules[0].ListThoiKhoaBieuTheoTiet[j].TeacherName;
                        }
                        else
                        {
                            drSource["Morning_StringDetailTeachingPeriod"] = string.Empty;
                            drSource["Morning_SubjectName"] = string.Empty;
                            drSource["Morning_TeacherName"] = string.Empty;
                        }
                        // Afternoon
                        if (j < tabularDailySchedule[i].SessionedSchedules[1].ListThoiKhoaBieuTheoTiet.Count)
                        {
                            drSource["Afternoon_StringDetailTeachingPeriod"] = tabularDailySchedule[i].SessionedSchedules[1].ListThoiKhoaBieuTheoTiet[j].StringDetailTeachingPeriod;
                            drSource["Afternoon_SubjectName"] = tabularDailySchedule[i].SessionedSchedules[1].ListThoiKhoaBieuTheoTiet[j].SubjectName;
                            drSource["Afternoon_TeacherName"] = tabularDailySchedule[i].SessionedSchedules[1].ListThoiKhoaBieuTheoTiet[j].TeacherName;
                        }
                        else
                        {
                            drSource["Afternoon_StringDetailTeachingPeriod"] = string.Empty;
                            drSource["Afternoon_SubjectName"] = string.Empty;
                            drSource["Afternoon_TeacherName"] = string.Empty;
                        }
                        dtSource.Rows.Add(drSource);
                    }
                }
                dsClass.Tables.Add(dtSource);
                RptDocument.Load(Server.MapPath("~/modules/report/Rpt_Classes.rpt"));
                RptDocument.SetDataSource(dsClass.Tables[0]);
                RptDocument.SetParameterValue("School", UserSchool.SchoolName);
                RptDocument.SetParameterValue("Year", year == null ? "Tất cả" : year.YearName);
                RptDocument.SetParameterValue("Fal", faculty == null ? "Tất cả" : faculty.FacultyName);
                RptDocument.SetParameterValue("Grade", grade == null ? "Tất cả" : grade.GradeName);                
                Session["report1"] = RptDocument;
            }

            if (Session["report1"] != null)
            {
                RptDocument = (ReportDocument)Session["report1"];
            }
            Rpt_ThoiKhoaBieu.ReportSource = RptDocument;
            Rpt_ThoiKhoaBieu.DisplayGroupTree = false;
        }
    }
}