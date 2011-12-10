using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;
using AjaxControlToolkit;
using CrystalDecisions;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class PrintStudentsPage : BaseContentPage
    {
        #region Variables 
        private StudentBL studentBL;
        private ReportDocument RptDocument = new ReportDocument();
        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (isAccessDenied)
            {
                return;
            }

            DataTable dtSource = null;
            List<TabularStudent> tabularStudents = new List<TabularStudent>();
            DataSet dsStudents = new DataSet();
            if(!Page.IsPostBack)
            {
                if (CheckSessionKey(AppConstant.SESSION_SELECTED_YEAR)
                    && CheckSessionKey(AppConstant.SESSION_SELECTED_FACULTY)
                    && CheckSessionKey(AppConstant.SESSION_SELECTED_GRADE)
                    && CheckSessionKey(AppConstant.SESSION_SELECTED_CLASS)
                    && CheckSessionKey(AppConstant.SESSION_SELECTED_STUDENTCODE)
                    && CheckSessionKey(AppConstant.SESSION_SELECTED_STUDENTNAME))
                {
                    Configuration_Year year = (Configuration_Year)GetSession(AppConstant.SESSION_SELECTED_YEAR);
                    RemoveSession(AppConstant.SESSION_SELECTED_YEAR);

                    Category_Faculty faculty = (Category_Faculty)GetSession(AppConstant.SESSION_SELECTED_FACULTY);
                    RemoveSession(AppConstant.SESSION_SELECTED_FACULTY);
                    if (faculty.FacultyId == 0)
                    {
                        faculty = null;
                    }

                    Category_Grade grade = (Category_Grade)GetSession(AppConstant.SESSION_SELECTED_GRADE);
                    RemoveSession(AppConstant.SESSION_SELECTED_GRADE);
                    if (grade.GradeId == 0)
                    {
                        grade = null;
                    }

                    Class_Class Class = (Class_Class)GetSession(AppConstant.SESSION_SELECTED_CLASS);
                    RemoveSession(AppConstant.SESSION_SELECTED_CLASS);
                    if (Class.ClassId == 0)
                    {
                        Class = null;
                    }

                    String strStudentName = (string)GetSession(AppConstant.SESSION_SELECTED_STUDENTNAME);
                    RemoveSession(AppConstant.SESSION_SELECTED_STUDENTNAME);

                    String strStudentCode = (string)GetSession(AppConstant.SESSION_SELECTED_STUDENTCODE);
                    RemoveSession(AppConstant.SESSION_SELECTED_STUDENTCODE);

                    // Get data
                    studentBL = new StudentBL(UserSchool);
                    tabularStudents = studentBL.GetTabularStudents(year, faculty, grade, Class, strStudentCode, strStudentName);
                }
                
                dtSource = new DataTable();
                dtSource.Columns.Add("ClassId", Type.GetType("System.Int32"));
                dtSource.Columns.Add("ClassName", Type.GetType("System.String"));
                dtSource.Columns.Add("FacultyName", Type.GetType("System.String"));
                dtSource.Columns.Add("FullName", Type.GetType("System.String"));
                dtSource.Columns.Add("GradeName", Type.GetType("System.String"));
                dtSource.Columns.Add("StudentCode", Type.GetType("System.String"));
                dtSource.Columns.Add("StudentId", Type.GetType("System.Int32"));
                dtSource.Columns.Add("Gender", Type.GetType("System.String"));
                dtSource.Columns.Add("DayOfBirth", Type.GetType("System.String"));

                DataRow dr = null;
                foreach (TabularStudent tabularStudent in tabularStudents)
                {
                    dr = dtSource.NewRow();
                    dr["ClassId"] = tabularStudent.ClassId;
                    dr["ClassName"] = tabularStudent.ClassName;
                    dr["FacultyName"] = tabularStudent.FacultyName;
                    dr["FullName"] = tabularStudent.FullName;
                    dr["GradeName"] = tabularStudent.GradeName;
                    dr["StudentCode"] = tabularStudent.StudentCode;
                    dr["StudentId"] = tabularStudent.StudentId;
                    dr["Gender"] = tabularStudent.Gender;
                    dr["DayOfBirth"] = tabularStudent.DayOfBirth;
                    dtSource.Rows.Add(dr);
                }
                
                dsStudents.Tables.Add(dtSource);
                RptDocument.Load(Server.MapPath("~/modules/report/Rpt_Student.rpt"));
                RptDocument.SetDataSource(dsStudents.Tables[0]);
                Session["report1"] = RptDocument;
            }
                        
            if (Session["report1"] != null)
            {
            //    RptDocument.Load(Server.MapPath("~/modules/report/Rpt_Student.rpt"));                
            //}
            //else
            //{
                RptDocument = (ReportDocument)Session["report1"];
            }
            Rpt_DanhSachHocSinh.ReportSource = RptDocument;
            Rpt_DanhSachHocSinh.DisplayGroupTree = false;
        }
    }
}