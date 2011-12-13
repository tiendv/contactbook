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
    public partial class PrintClassesPage : BaseContentPage
    {
        #region Variables

        private int _Year;
        private int _Falculty;
        private int _Grade;
        private int _Class;
        private ClassBL classBL;
        private ReportDocument RptDocument = new ReportDocument();
        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            DataSet dsClass = new DataSet();
            if (!Page.IsPostBack)
            {
                List<TabularClass> tabularClasses = new List<TabularClass>();
                double dTotalRecords;

                Configuration_Year year = (Configuration_Year)GetSession(AppConstant.SESSION_SELECTED_YEAR);
                RemoveSession(AppConstant.SESSION_SELECTED_YEAR);
                _Year = year.YearId;

                Category_Faculty faculty = (Category_Faculty)GetSession(AppConstant.SESSION_SELECTED_FACULTY);
                RemoveSession(AppConstant.SESSION_SELECTED_FACULTY);

                Category_Grade grade = (Category_Grade)GetSession(AppConstant.SESSION_SELECTED_GRADE);
                RemoveSession(AppConstant.SESSION_SELECTED_GRADE);

                // Get data
                classBL = new ClassBL(UserSchool);
                tabularClasses = classBL.GetTabularClasses(year, faculty, grade, 
                1, 50, out dTotalRecords);

                DataTable dtSource = new DataTable();
                dtSource.Columns.Add("ClassId", Type.GetType("System.Int32"));
                dtSource.Columns.Add("ClassName", Type.GetType("System.String"));
                dtSource.Columns.Add("FacultyName", Type.GetType("System.String"));
                dtSource.Columns.Add("FullName", Type.GetType("System.String"));
                dtSource.Columns.Add("GradeName", Type.GetType("System.String"));
                dtSource.Columns.Add("TenGVCN", Type.GetType("System.String"));
                dtSource.Columns.Add("SiSo", Type.GetType("System.Int16"));
                dtSource.Columns.Add("YearName", Type.GetType("System.String"));
                for (int i = 0; i < tabularClasses.Count; i++)
                {
                    DataRow dr = dtSource.NewRow();
                    dr["ClassId"] = tabularClasses[i].ClassId;
                    dr["ClassName"] = tabularClasses[i].ClassName;
                    dr["FacultyName"] = tabularClasses[i].FacultyName;                    
                    dr["GradeName"] = tabularClasses[i].GradeName;
                    dr["TenGVCN"] = tabularClasses[i].TenGVCN;
                    dr["SiSo"] = tabularClasses[i].SiSo;
                    dr["YearName"] = tabularClasses[i].YearName;                   
                    dtSource.Rows.Add(dr);
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
            Rpt_DanhSachLopHoc.ReportSource = RptDocument;
            Rpt_DanhSachLopHoc.DisplayGroupTree = false;
        }
    }
}