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
    public partial class PrintStudentsPage : BaseContentPage
    {
        #region Variables

        private StudentBL studentBL;
        private ReportDocument RptDocument = new ReportDocument();
        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (accessDenied)
            {
                return;
            }
            // Get data
            studentBL = new StudentBL(UserSchool);
            DataSet dsHocSinh = new DataSet();
            List<TabularStudent> tabularStudents = new List<TabularStudent>();
            double dTotalRecords;
            Configuration_Year year = null;
            Category_Faculty faculty = null;
            Category_Grade grade = null;
            Class_Class Class = null;
            String strStudentName = null;
            String strStudentCode = null;

            if (!Page.IsPostBack)
            {
                year = (Configuration_Year)GetSession(AppConstant.SESSION_SELECTED_YEAR);
                ViewState[AppConstant.VIEWSTATE_SELECTED_YEAR] = year.YearId;
                RemoveSession(AppConstant.SESSION_SELECTED_YEAR);

                faculty = (Category_Faculty)GetSession(AppConstant.SESSION_SELECTED_FACULTY);
                ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTY] = faculty.FacultyId;
                RemoveSession(AppConstant.SESSION_SELECTED_FACULTY);
                if (faculty.FacultyId == 0)
                {
                    faculty = null;
                }

                grade = (Category_Grade)GetSession(AppConstant.SESSION_SELECTED_GRADE);
                ViewState[AppConstant.VIEWSTATE_SELECTED_GRADE] = grade.GradeId;
                RemoveSession(AppConstant.SESSION_SELECTED_GRADE);
                if (grade.GradeId == 0)
                {
                    grade = null;
                }

                Class = (Class_Class)GetSession(AppConstant.SESSION_SELECTED_CLASS);
                ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS] = Class.ClassId;
                RemoveSession(AppConstant.SESSION_SELECTED_CLASS);
                if (Class.ClassId == 0)
                {
                    Class = null;
                }

                strStudentName = (string)GetSession(AppConstant.SESSION_SELECTED_STUDENTNAME);
                ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTNAME] = strStudentName;
                RemoveSession(AppConstant.SESSION_SELECTED_STUDENTNAME);

                strStudentCode = (string)GetSession(AppConstant.SESSION_SELECTED_STUDENTCODE);
                ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTCODE] = strStudentCode;
                RemoveSession(AppConstant.SESSION_SELECTED_STUDENTCODE);
            }
            else
            {
                year = new Configuration_Year();
                year.YearId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_YEAR];
                year = (new SystemConfigBL(UserSchool)).GetYear(year.YearId);

                if ((int)ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTY] != 0)
                {
                    faculty = (new FacultyBL(UserSchool)).GetFaculty((int)ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTY]);
                }

                if ((int)ViewState[AppConstant.VIEWSTATE_SELECTED_GRADE] != 0)
                {
                    grade = (new GradeBL(UserSchool)).GetGrade((int)ViewState[AppConstant.VIEWSTATE_SELECTED_GRADE]);
                }

                if ((int)ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS] != 0)
                {
                    Class = new ClassBL(UserSchool).GetClass((int)ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS]);
                }

                strStudentName = (string)ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTNAME];
                strStudentCode = (string)ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTCODE];
            }

            
            tabularStudents = studentBL.GetTabularStudents(year, faculty, grade, Class, strStudentCode, strStudentName,
            1, 50, out dTotalRecords);

            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("ClassId", Type.GetType("System.Int32"));
            dtSource.Columns.Add("ClassName", Type.GetType("System.String"));
            dtSource.Columns.Add("FacultyName", Type.GetType("System.String"));
            dtSource.Columns.Add("FullName", Type.GetType("System.String"));
            dtSource.Columns.Add("GradeName", Type.GetType("System.String"));
            dtSource.Columns.Add("StudentCode", Type.GetType("System.String"));
            dtSource.Columns.Add("StudentId", Type.GetType("System.Int32"));
            dtSource.Columns.Add("Year", Type.GetType("System.String"));
            for (int i = 0; i < tabularStudents.Count; i++)
            {
                DataRow dr = dtSource.NewRow();
                dr["ClassId"] = tabularStudents[i].ClassId;
                dr["ClassName"] = tabularStudents[i].ClassName;
                dr["FacultyName"] = tabularStudents[i].FacultyName;
                dr["FullName"] = tabularStudents[i].FullName;
                dr["GradeName"] = tabularStudents[i].GradeName;
                dr["StudentCode"] = tabularStudents[i].StudentCode;
                dr["StudentId"] = tabularStudents[i].StudentId;
                dr["Year"] = year.YearName;
                dtSource.Rows.Add(dr);
            }
            dsHocSinh.Tables.Add(dtSource);
            RptDocument.Load(Server.MapPath("~/modules/report/Rpt_Student.rpt"));
            RptDocument.SetDataSource(dsHocSinh.Tables[0]);
            RptDocument.SetParameterValue("School", UserSchool.SchoolName);
            RptDocument.SetParameterValue("Year", year == null ? "Tất cả" : year.YearName);
            FacultyBL facultyBL = new FacultyBL(UserSchool);
            RptDocument.SetParameterValue("Fal", (faculty == null || faculty.FacultyId == 0) ? "Tất cả" : faculty.FacultyName);
            RptDocument.SetParameterValue("Grade", (grade == null || grade.GradeId == 0)? "Tất cả" : grade.GradeName);
            RptDocument.SetParameterValue("Class", (Class == null || Class.ClassId == 0)? "Tất cả" : Class.ClassName);
           
            Rpt_DanhSachHocSinh.ReportSource = RptDocument;
            Rpt_DanhSachHocSinh.DisplayGroupTree = false;
        }
    }
}