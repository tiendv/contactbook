﻿using System;
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
        
        private int _Year;
        private int _Falculty;
        private int _Grade;
        private int _Class;
        private string _StudentName;
        private string _StudentCode;
        private StudentBL studentBL;
        private ReportDocument RptDocument = new ReportDocument();
        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            DataSet dsHocSinh = new DataSet();
            if (!Page.IsPostBack)
            {
                List<TabularStudent> tabularStudents = new List<TabularStudent>();
                double dTotalRecords;

                if (CheckSessionKey(AppConstant.SESSION_SELECTED_YEAR)
                    && CheckSessionKey(AppConstant.SESSION_SELECTED_FACULTY)
                    && CheckSessionKey(AppConstant.SESSION_SELECTED_GRADE)
                    && CheckSessionKey(AppConstant.SESSION_SELECTED_CLASS)
                    && CheckSessionKey(AppConstant.SESSION_SELECTED_STUDENTCODE)
                    && CheckSessionKey(AppConstant.SESSION_SELECTED_STUDENTNAME))
                {
                    Configuration_Year year = (Configuration_Year)GetSession(AppConstant.SESSION_SELECTED_YEAR);
                    RemoveSession(AppConstant.SESSION_SELECTED_YEAR);
                    _Year = year.YearId;

                    Category_Faculty faculty = (Category_Faculty)GetSession(AppConstant.SESSION_SELECTED_FACULTY);
                    RemoveSession(AppConstant.SESSION_SELECTED_FACULTY);
                    _Falculty = faculty.FacultyId;

                    Category_Grade grade = (Category_Grade)GetSession(AppConstant.SESSION_SELECTED_GRADE);
                    RemoveSession(AppConstant.SESSION_SELECTED_GRADE);
                    _Grade = grade.GradeId;

                    Class_Class Class = (Class_Class)GetSession(AppConstant.SESSION_SELECTED_CLASS);
                    RemoveSession(AppConstant.SESSION_SELECTED_CLASS);
                    _Class = Class.ClassId;

                    String strStudentName = (string)GetSession(AppConstant.SESSION_SELECTED_STUDENTNAME);
                    RemoveSession(AppConstant.SESSION_SELECTED_STUDENTNAME);
                    _StudentName = strStudentName;

                    String strStudentCode = (string)GetSession(AppConstant.SESSION_SELECTED_STUDENTCODE);
                    RemoveSession(AppConstant.SESSION_SELECTED_STUDENTCODE);
                    _StudentCode = strStudentCode;

                    // Get data
                    studentBL = new StudentBL(UserSchool);
                    tabularStudents = studentBL.GetTabularStudents(year, faculty, grade, Class, _StudentCode, _StudentName,
                        1, 20, out dTotalRecords);

                }
                
                DataTable dtSource = new DataTable();
                dtSource.Columns.Add("ClassId", Type.GetType("System.Int32"));
                dtSource.Columns.Add("ClassName", Type.GetType("System.String"));
                dtSource.Columns.Add("FacultyName", Type.GetType("System.String"));
                dtSource.Columns.Add("FullName", Type.GetType("System.String"));
                dtSource.Columns.Add("GradeName", Type.GetType("System.String"));
                dtSource.Columns.Add("StudentCode", Type.GetType("System.String"));
                dtSource.Columns.Add("StudentId", Type.GetType("System.Int32"));                
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
                    dtSource.Rows.Add(dr);
                }
                dsHocSinh.Tables.Add(dtSource);
                RptDocument.Load(Server.MapPath("~/modules/report/Rpt_Student.rpt"));
                RptDocument.SetDataSource(dsHocSinh.Tables[0]);
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