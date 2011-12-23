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
using System.Web.Security;


namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class PrintStudentsPage : BaseContentPage
    {                
        private ReportDocument RptDocument = new ReportDocument();     
        private string strPagePath;

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

            #region Variables

            DataSet ds = new DataSet();            
            
            StudentBL studentBL = new StudentBL(UserSchool);
            TeacherBL teacherBL = new TeacherBL(UserSchool);
            ClassBL classBL = new ClassBL(UserSchool);
            ScheduleBL schiduleBL = new ScheduleBL(UserSchool);            
            double dTotalRecords;            
            
            Configuration_Year year = null;
            Category_Faculty faculty = null;
            Category_Grade grade = null;
            Class_Class classes = null;
            Configuration_Term term = null;
            String strStudentName = null;
            String strStudentCode = null;

            String strTeacherID = null;
            String strTeacherName= null;
            

            List<TabularStudent> tabularStudents = new List<TabularStudent>();
            List<TabularTeacher> tabularTeachers = new List<TabularTeacher>();
            List<TabularClass> tabularClasses = new List<TabularClass>();
            List<DailySchedule> tabularDailySchedule = new List<DailySchedule>();

            #endregion

            if (!Page.IsPostBack)
            {
                #region 
                
                strPagePath = (string)GetSession(AppConstant.SESSION_PAGEPATH);
                ViewState[AppConstant.SESSION_PAGEPATH] = strPagePath;                
                RemoveSession(AppConstant.SESSION_PAGEPATH);

                switch (strPagePath)
                {
                    case AppConstant.PAGEPATH_PRINTSTUDENTS:
                        {
                            #region
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

                            classes = (Class_Class)GetSession(AppConstant.SESSION_SELECTED_CLASS);
                            ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS] = classes.ClassId;
                            RemoveSession(AppConstant.SESSION_SELECTED_CLASS);
                            if (classes.ClassId == 0)
                            {
                                classes = null;
                            }

                            strStudentName = (string)GetSession(AppConstant.SESSION_SELECTED_STUDENTNAME);
                            ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTNAME] = strStudentName;
                            RemoveSession(AppConstant.SESSION_SELECTED_STUDENTNAME);

                            strStudentCode = (string)GetSession(AppConstant.SESSION_SELECTED_STUDENTCODE);
                            ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTCODE] = strStudentCode;
                            RemoveSession(AppConstant.SESSION_SELECTED_STUDENTCODE);
                            #endregion
                            break;
                        }
                    case AppConstant.PAGEPATH_PRINTTEACHERS:
                        {
                            #region
                            strTeacherID = (string)GetSession(AppConstant.SESSION_TEACHERID);
                            ViewState[AppConstant.VIEWSTATE_SELECTED_TEACHERID] = strTeacherID;
                            RemoveSession(AppConstant.SESSION_TEACHERID);

                            strTeacherName = (string)GetSession(AppConstant.SESSION_TEACHERNAME);
                            ViewState[AppConstant.VIEWSTATE_SELECTED_TEACHERNAME] = strTeacherName;
                            RemoveSession(AppConstant.SESSION_TEACHERNAME);
                            #endregion
                            break;
                        }
                    case AppConstant.PAGEPATH_PRINTCLASSES:
                        {
                            #region
                            year = (Configuration_Year)GetSession(AppConstant.SESSION_SELECTED_YEAR);
                            ViewState[AppConstant.VIEWSTATE_SELECTED_YEAR] = (year==null?0:year.YearId);
                            RemoveSession(AppConstant.SESSION_SELECTED_YEAR);

                            faculty = (Category_Faculty)GetSession(AppConstant.SESSION_SELECTED_FACULTY);
                            ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTY] = (faculty==null?0:faculty.FacultyId);
                            RemoveSession(AppConstant.SESSION_SELECTED_FACULTY);

                            grade = (Category_Grade)GetSession(AppConstant.SESSION_SELECTED_GRADE);
                            ViewState[AppConstant.VIEWSTATE_SELECTED_GRADE] = grade==null?0:grade.GradeId;
                            RemoveSession(AppConstant.SESSION_SELECTED_GRADE);
                            #endregion
                            break;
                        }
                    case AppConstant.PAGEPATH_PRINTTERM:
                        {
                            #region
                            year = (Configuration_Year)GetSession(AppConstant.SESSION_SELECTED_YEAR);
                            ViewState[AppConstant.VIEWSTATE_SELECTED_YEAR] = year.YearId;
                            RemoveSession(AppConstant.SESSION_SELECTED_YEAR);

                            faculty = (Category_Faculty)GetSession(AppConstant.SESSION_SELECTED_FACULTY);
                            ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTY] = (faculty == null ? 0 : faculty.FacultyId);
                            RemoveSession(AppConstant.SESSION_SELECTED_FACULTY);

                            grade = (Category_Grade)GetSession(AppConstant.SESSION_SELECTED_GRADE);
                            ViewState[AppConstant.VIEWSTATE_SELECTED_GRADE] = grade == null ? 0 : grade.GradeId;
                            RemoveSession(AppConstant.SESSION_SELECTED_GRADE);        

                            classes = (Class_Class)GetSession(AppConstant.SESSION_SELECTED_CLASS);
                            ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS] = classes==null?0:classes.ClassId;
                            RemoveSession(AppConstant.SESSION_SELECTED_CLASS);

                            term = (Configuration_Term)GetSession(AppConstant.SESSION_SELECTED_TERM);
                            ViewState[AppConstant.SESSION_SELECTED_TERM] = term==null?0:term.TermId;
                            RemoveSession(AppConstant.SESSION_SELECTED_TERM);
                            #endregion                            
                            break;
                        }                        
                    default:
                        break;
                }


                #endregion
            }
            else
            {
                #region

                strPagePath = (string)ViewState[AppConstant.SESSION_PAGEPATH];
                switch (strPagePath)
                {
                    case AppConstant.PAGEPATH_PRINTSTUDENTS:
                        {
                            #region
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
                                classes = new ClassBL(UserSchool).GetClass((int)ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS]);
                            }

                            strStudentName = (string)ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTNAME];
                            strStudentCode = (string)ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTCODE];
                            #endregion
                            break;
                        }
                    case AppConstant.PAGEPATH_PRINTTEACHERS:
                        {
                            #region
                            strTeacherID = (string)ViewState[AppConstant.VIEWSTATE_SELECTED_TEACHERID];
                            strTeacherName = (string)ViewState[AppConstant.VIEWSTATE_SELECTED_TEACHERNAME];
                            #endregion
                            break;
                        }
                    case AppConstant.PAGEPATH_PRINTCLASSES:
                        {
                            #region
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
                            #endregion
                            break;
                        }
                    case AppConstant.PAGEPATH_PRINTTERM:
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
                                classes = new ClassBL(UserSchool).GetClass((int)ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS]);
                            }
                            if ((int)ViewState[AppConstant.SESSION_SELECTED_TERM] != 0)
                            {
                                term =  new  SystemConfigBL(UserSchool).GetTerm((int)ViewState[AppConstant.SESSION_SELECTED_TERM]);
                            }
                            break;
                        }
                    default:
                        break;
                }

                #endregion
            }

            #region
            
            switch (strPagePath)
            {
                case AppConstant.PAGEPATH_PRINTSTUDENTS:
                    {
                        #region
                        tabularStudents = studentBL.GetTabularStudents(year, faculty, grade, classes, strStudentCode, strStudentName,
                        
                        1, 50, out dTotalRecords);
                        ds = new DataSet();
                        DataTable dtSource = new DataTable();
                        dtSource.Columns.Add("ClassId", Type.GetType("System.Int32"));
                        dtSource.Columns.Add("ClassName", Type.GetType("System.String"));
                        dtSource.Columns.Add("FacultyName", Type.GetType("System.String"));
                        dtSource.Columns.Add("FullName", Type.GetType("System.String"));
                        dtSource.Columns.Add("GradeName", Type.GetType("System.String"));
                        dtSource.Columns.Add("StudentCode", Type.GetType("System.String"));
                        dtSource.Columns.Add("StudentId", Type.GetType("System.Int32"));
                        dtSource.Columns.Add("Gender", Type.GetType("System.String"));
                        dtSource.Columns.Add("DayOfBirth", Type.GetType("System.String"));
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
                            dr["Gender"] = tabularStudents[i].Gender;
                            dr["DayOfBirth"] = tabularStudents[i].DayOfBirth;
                            dtSource.Rows.Add(dr);
                        }
                        ds.Tables.Add(dtSource);
                        RptDocument.Load(Server.MapPath("~/modules/report/Rpt_Student.rpt"));
                        RptDocument.SetDataSource(ds.Tables[0]);
                        RptDocument.SetParameterValue("School", UserSchool.SchoolName);
                        RptDocument.SetParameterValue("Year", year == null ? "Tất cả" : year.YearName);
                        FacultyBL facultyBL = new FacultyBL(UserSchool);
                        RptDocument.SetParameterValue("Fal", (faculty == null || faculty.FacultyId == 0) ? "Tất cả" : faculty.FacultyName);
                        RptDocument.SetParameterValue("Grade", (grade == null || grade.GradeId == 0) ? "Tất cả" : grade.GradeName);
                        RptDocument.SetParameterValue("Class", (classes == null || classes.ClassId == 0) ? "Tất cả" : classes.ClassName);

                        #endregion
                        break;
                    }
                case AppConstant.PAGEPATH_PRINTTEACHERS:
                    {
                        #region
                        tabularTeachers = teacherBL.GetTabularTeachers(strTeacherID, strTeacherName,1, 20, out dTotalRecords);
                        ds = new DataSet();
                        DataTable dtSource = new DataTable();
                        dtSource.Columns.Add("MaHienThiGiaoVien", Type.GetType("System.String"));
                        dtSource.Columns.Add("HoTen", Type.GetType("System.String"));
                        dtSource.Columns.Add("GioiTinh", Type.GetType("System.String"));
                        dtSource.Columns.Add("NgaySinh", Type.GetType("System.String"));
                        for (int i = 0; i < tabularTeachers.Count; i++)
                        {
                            DataRow dr = dtSource.NewRow();
                            dr["MaHienThiGiaoVien"] = tabularTeachers[i].MaHienThiGiaoVien;
                            dr["HoTen"] = tabularTeachers[i].HoTen;
                            dr["GioiTinh"] = tabularTeachers[i].StringGioiTinh;
                            dr["NgaySinh"] = tabularTeachers[i].StringNgaySinh;
                            dtSource.Rows.Add(dr);
                        }
                        ds.Tables.Add(dtSource);
                        RptDocument.Load(Server.MapPath("~/modules/report/Rpt_Teachers.rpt"));
                        RptDocument.SetDataSource(ds.Tables[0]);
                        RptDocument.SetParameterValue("School", UserSchool.SchoolName);
                        #endregion
                        break;
                    }
                case AppConstant.PAGEPATH_PRINTCLASSES:
                    {
                        #region
                        tabularClasses = classBL.GetTabularClasses(year, faculty, grade,1, 50, out dTotalRecords);

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
                        ds.Tables.Add(dtSource);
                        RptDocument.Load(Server.MapPath("~/modules/report/Rpt_Classes.rpt"));
                        RptDocument.SetDataSource(ds.Tables[0]);
                        RptDocument.SetParameterValue("School", UserSchool.SchoolName);
                        RptDocument.SetParameterValue("Year", year == null ? "Tất cả" : year.YearName);
                        RptDocument.SetParameterValue("Fal", faculty == null ? "Tất cả" : faculty.FacultyName);
                        RptDocument.SetParameterValue("Grade", grade == null ? "Tất cả" : grade.GradeName);   
                        #endregion
                        break;
                    }
                case AppConstant.PAGEPATH_PRINTTERM:
                    {
                        #region
                        tabularDailySchedule = schiduleBL.GetDailySchedules(classes, term);
                        TabularClass tlbClass = classBL.GetTabularClass(classes);
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
                            if (_count == 0)
                            {
                                DataRow drSource = dtSource.NewRow();
                                drSource["DayInWeekName"] = "Thứ " +(tabularDailySchedule[i].DayInWeekId + 1).ToString();
                                dtSource.Rows.Add(drSource);
                            }
                            else
                            {
                                // Morning
                                for (int j = 0; j < _count; j++)
                                {
                                    DataRow drSource = dtSource.NewRow();
                                    drSource["DayInWeekName"] = "Thứ " + (tabularDailySchedule[i].DayInWeekId + 1).ToString();
                                    // Morning
                                    if (j < tabularDailySchedule[i].SessionedSchedules[0].ListThoiKhoaBieuTheoTiet.Count)
                                    {
                                        drSource["Morning_StringDetailTeachingPeriod"] = tabularDailySchedule[i].SessionedSchedules[0].ListThoiKhoaBieuTheoTiet[j].StringDetailTeachingPeriod.Replace("<b>", "").Replace("</b>", "").Replace("<br/>", "");
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
                                        drSource["Afternoon_StringDetailTeachingPeriod"] = tabularDailySchedule[i].SessionedSchedules[1].ListThoiKhoaBieuTheoTiet[j].StringDetailTeachingPeriod.Replace("<b>", "").Replace("</b>", "").Replace("<br/>", "");
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


                        }
                        ds.Tables.Add(dtSource);                                                
                        RptDocument.Load(Server.MapPath("~/modules/report/Rpt_Term.rpt"));
                        RptDocument.SetDataSource(ds.Tables[0]);
                        RptDocument.SetParameterValue("School", UserSchool.SchoolName);
                        RptDocument.SetParameterValue("Year", year == null ? "Tất cả" : year.YearName);
                        RptDocument.SetParameterValue("Term", term == null ? "Tất cả" : term.TermName);
                        RptDocument.SetParameterValue("Fal", tlbClass.FacultyName);
                        RptDocument.SetParameterValue("Grade", tlbClass.GradeName);
                        RptDocument.SetParameterValue("Classes", classes.ClassName);
                        Rpt_DanhSachHocSinh.Zoom(75);
                        #endregion
                        break;
                    }
                default:
                    break;
            }


            #endregion

            Rpt_DanhSachHocSinh.ReportSource = RptDocument;
            Rpt_DanhSachHocSinh.DisplayGroupTree = false;
        }
    }
}