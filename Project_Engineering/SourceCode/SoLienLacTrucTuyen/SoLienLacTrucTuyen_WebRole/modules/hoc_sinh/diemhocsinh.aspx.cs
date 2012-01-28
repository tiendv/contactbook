using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.BusinessEntity;
using EContactBook.DataAccess;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class SearchStudentMarkPage : BaseContentPage
    {
        #region Fields
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

            studyingResultBL = new StudyingResultBL(UserSchool);

            if (!Page.IsPostBack)
            {
                BindDropDownLists();
                if (DdlNamHoc.Items.Count == 0 || DdlNganh.Items.Count == 0 || DdlKhoiLop.Items.Count == 0)
                {
                    ProccessDisplayGUI(false);
                    BtnAdd.Enabled = false;
                    BtnAdd.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_ADD_DISABLE;
                }
                else
                {
                    RetrieveSessions();
                    BindRptMarkTypes();
                    BindRptStudentMarks();
                }
            }

            ProcPermissions();
        }
        #endregion

        #region DropDownList event hanlders
        protected void DdlNamHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLClasses();
            BindDDLScheduledSubjects();
            BindDDLMonths();
            BindDDLWeeks();
        }

        protected void DdlNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLClasses();
            BindDDLScheduledSubjects();
        }

        protected void DdlKhoiLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLClasses();
            BindDDLScheduledSubjects();
            BindDDLMarkTypes();
        }

        protected void DdlHocKy_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLScheduledSubjects();
            BindDDLMonths();
            BindDDLWeeks();
        }

        protected void DdlLopHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLScheduledSubjects();
        }
        #endregion

        #region Methods
        private void ProcPermissions()
        {
            BtnAdd.Visible = BtnAdd.Visible && accessibilities.Contains(AccessibilityEnum.Add);
            BtnEdit.Visible = accessibilities.Contains(AccessibilityEnum.Modify);
        }

        private void BindDropDownLists()
        {
            BindDDLYears();
            BindDDLTerms();
            BindDDLMonths();
            BindDDLWeeks();
            BindDDLFaculties();
            BindDDLGrades();
            BindDDLClasses();
            BindDDLScheduledSubjects();
            BindDDLMarkTypes();
        }

        private void BindDDLYears()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<Configuration_Year> years = systemConfigBL.GetListYears();
            DdlNamHoc.DataSource = years;
            DdlNamHoc.DataValueField = "YearId";
            DdlNamHoc.DataTextField = "YearName";
            DdlNamHoc.DataBind();

            if (DdlNamHoc.Items.Count != 0)
            {
                DdlNamHoc.SelectedValue = systemConfigBL.GetLastedYear().ToString();
            }
        }

        private void BindDDLTerms()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<Configuration_Term> terms = systemConfigBL.GetListTerms();

            DdlHocKy.DataSource = terms;
            DdlHocKy.DataValueField = "TermId";
            DdlHocKy.DataTextField = "TermName";
            DdlHocKy.DataBind();

            if (DdlNamHoc.Items.Count != 0)
            {
                if (CurrentYear.YearId == Int32.Parse(DdlNamHoc.SelectedValue))
                {
                    DdlHocKy.SelectedValue = systemConfigBL.GetCurrentTerm().TermId.ToString();
                }
            }
        }

        private void BindDDLMonths()
        {
            if (DdlNamHoc.Items.Count != 0 && DdlHocKy.Items.Count != 0)
            {
                Configuration_Year year = new Configuration_Year();
                year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);

                Configuration_Term term = new Configuration_Term();
                term.TermId = Int32.Parse(DdlHocKy.SelectedValue);

                SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);

                List<Month> months = systemConfigBL.GetMonths(year, term);
                DddMonths.DataSource = months;
                DddMonths.DataValueField = "MonthId";
                DddMonths.DataTextField = "MonthName";
                DddMonths.DataBind();

                DddMonths.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }

        private void BindDDLWeeks()
        {
            if (DdlNamHoc.Items.Count != 0 && DdlHocKy.Items.Count != 0)
            {
                Configuration_Year year = new Configuration_Year();
                year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);

                Configuration_Term term = new Configuration_Term();
                term.TermId = Int32.Parse(DdlHocKy.SelectedValue);

                SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);

                List<Week> months = systemConfigBL.GetWeeks(year, term);
                DdlWeeks.DataSource = months;
                DdlWeeks.DataValueField = "WeekId";
                DdlWeeks.DataTextField = "WeekName";
                DdlWeeks.DataBind();

                DdlWeeks.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }

        private void BindDDLFaculties()
        {
            FacultyBL facultyBL = new FacultyBL(UserSchool);
            List<Category_Faculty> faculties = facultyBL.GetFaculties();
            DdlNganh.DataSource = faculties;
            DdlNganh.DataValueField = "FacultyId";
            DdlNganh.DataTextField = "FacultyName";
            DdlNganh.DataBind();
            if (faculties.Count > 1)
            {
                DdlNganh.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }

        private void BindDDLGrades()
        {
            GradeBL gradeBL = new GradeBL(UserSchool);
            List<Category_Grade> grades = gradeBL.GetListGrades();
            DdlKhoiLop.DataSource = grades;
            DdlKhoiLop.DataValueField = "GradeId";
            DdlKhoiLop.DataTextField = "GradeName";
            DdlKhoiLop.DataBind();
        }

        private void BindDDLMarkTypes()
        {
            if (DdlKhoiLop.Items.Count != 0)
            {
                Category_Grade grade = new Category_Grade();
                grade.GradeId = Int32.Parse(DdlKhoiLop.SelectedValue);

                MarkTypeBL loaiDiemBL = new MarkTypeBL(UserSchool);
                List<Category_MarkType> lstLoaiDiem = loaiDiemBL.GetListMarkTypes(grade);
                DdlLoaiDiem.DataSource = lstLoaiDiem;
                DdlLoaiDiem.DataValueField = "MarkTypeId";
                DdlLoaiDiem.DataTextField = "MarkTypeName";
                DdlLoaiDiem.DataBind();
                if (lstLoaiDiem.Count > 1)
                {
                    DdlLoaiDiem.Items.Insert(0, new ListItem("Tất cả", "0"));
                }
            }
        }

        private void BindDDLClasses()
        {
            if (DdlNamHoc.Items.Count == 0 || DdlNganh.Items.Count == 0 || DdlKhoiLop.Items.Count == 0)
            {
                return;
            }

            Configuration_Year year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue); 
            
            Category_Faculty faculty = null;
            Category_Grade grade = null;            

            if(DdlNganh.Items.Count != 0)
            {
                if (DdlNganh.SelectedIndex > 0)
                {
                    faculty = new Category_Faculty();
                    faculty.FacultyId = Int32.Parse(DdlNganh.SelectedValue);
                }
            }

            if (DdlKhoiLop.Items.Count != 0)
            {
                if (DdlKhoiLop.SelectedIndex >= 0)
                {
                    grade = new Category_Grade();
                    grade.GradeId = Int32.Parse(DdlKhoiLop.SelectedValue);
                }
            }

            Configuration_Term term = new Configuration_Term();
            term.TermId = Int32.Parse(DdlHocKy.SelectedValue);

            ClassBL classBL = new ClassBL(UserSchool);
            List<Class_Class> Classes = classBL.GetClasses(LogedInUser, IsFormerTeacher, IsSubjectTeacher, year, faculty, grade, term);
            DdlLopHoc.DataSource = Classes;
            DdlLopHoc.DataValueField = "ClassId";
            DdlLopHoc.DataTextField = "ClassName";
            DdlLopHoc.DataBind();
        }

        /// <summary>
        /// Bind dropdownlist subjects
        /// </summary>
        private void BindDDLScheduledSubjects()
        {   
            ScheduleBL scheduleBL = new ScheduleBL(UserSchool);
            Class_Class Class = null;
            Configuration_Term term = null;

            if (DdlLopHoc.Items.Count == 0)
            {
                DdlMonHoc.DataSource = null;
            }
            else
            {
                Class = new Class_Class();
                Class.ClassId = Int32.Parse(DdlLopHoc.SelectedValue);
                term = new Configuration_Term();
                term.TermId = Int32.Parse(DdlHocKy.SelectedValue);

                List<Category_Subject> scheduledSubjects = scheduleBL.GetScheduledSubjects(Class, term);
                DdlMonHoc.DataSource = scheduledSubjects;
                DdlMonHoc.DataValueField = "SubjectId";
                DdlMonHoc.DataTextField = "SubjectName";
                DdlMonHoc.DataBind();
            }
        }

        private void BindRptMarkTypes()
        {
            if (DdlKhoiLop.Items.Count != 0)
            {
                Category_Grade grade = new Category_Grade();
                grade.GradeId = Int32.Parse(DdlKhoiLop.SelectedValue);
                MarkTypeBL markTypeBL = new MarkTypeBL(UserSchool);
                List<Category_MarkType> markTypes = new List<Category_MarkType>();

                if (DdlLoaiDiem.Items.Count != 0)
                {
                    if (DdlLoaiDiem.SelectedIndex == 0)
                    {
                        markTypes = markTypeBL.GetListMarkTypes(grade);
                    }
                    else
                    {
                        string markTypeName = DdlLoaiDiem.SelectedValue;
                        markTypes.Add(markTypeBL.GetMarkType(grade, markTypeName));
                    }
                }

                this.RptLoaiDiem.DataSource = markTypes;
                this.RptLoaiDiem.DataBind();
            }
        }

        private void BindRptStudentMarks()
        {
            // case: there is no Class or schedule subject or marktype
            if (DdlLopHoc.Items.Count == 0 || DdlMonHoc.Items.Count == 0 || DdlLoaiDiem.Items.Count == 0)
            {
                // do not display 
                ProccessDisplayGUI(false);
                return;
            }

            // declare variables
            Class_Class Class = new Class_Class();
            Category_Subject subject = new Category_Subject();
            Configuration_Term term = new Configuration_Term();
            MarkTypeBL markTypeBL = new MarkTypeBL(UserSchool);
            List<Category_MarkType> markTypes = new List<Category_MarkType>();
            List<TabularStudentMark> tabularStudentMarks = new List<TabularStudentMark>();
            double dTotalRecords = 0;

            Class.ClassId = Int32.Parse(DdlLopHoc.SelectedValue);
            subject.SubjectId = Int32.Parse(DdlMonHoc.SelectedValue);
            term.TermId = Int32.Parse(DdlHocKy.SelectedValue);

            if (DdlKhoiLop.Items.Count != 0)
            {
                Category_Grade grade = new Category_Grade();
                grade.GradeId = Int32.Parse(DdlKhoiLop.SelectedValue);
                if (DdlLoaiDiem.SelectedIndex == 0)
                {
                    markTypes = markTypeBL.GetListMarkTypes(grade);
                }
                else
                {
                    string markTypeName = DdlLoaiDiem.SelectedItem.Text;
                    markTypes.Add(markTypeBL.GetMarkType(grade, markTypeName));
                }
            }

            if (RBtnMonth.Checked)
            {
                if (DddMonths.SelectedIndex > 0)
                {
                    int month = Int32.Parse(DddMonths.SelectedValue);
                    tabularStudentMarks = studyingResultBL.GetTabularStudentMarks(Class, subject, term, month, markTypes,
                        MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);
                }
                else
                {
                    tabularStudentMarks = studyingResultBL.GetTabularStudentMarks(Class, subject, term, markTypes,
                     MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);
                }
            }
            else if (RBtnWeek.Checked)
            {                
                if (DdlWeeks.SelectedIndex > 0)
                {
                    string[] strDates = DdlWeeks.SelectedValue.Split('-');                    
                    DateTime dtBeginDate = DateTime.Parse(strDates[0]);
                    DateTime dtEndDate = DateTime.Parse(strDates[1]);
                    // get student mark information
                    tabularStudentMarks = studyingResultBL.GetTabularStudentMarks(Class, subject, term, dtBeginDate, dtEndDate, markTypes, 
                        MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);
                }
                else
                {
                    tabularStudentMarks = studyingResultBL.GetTabularStudentMarks(Class, subject, term, markTypes,
                     MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);
                }
            }


            // bind to repeater and datapager
            this.RptDiemMonHoc.DataSource = tabularStudentMarks;
            this.RptDiemMonHoc.DataBind();
            MainDataPager.ItemCount = dTotalRecords;

            // display information
            bool bDisplayData = (tabularStudentMarks.Count != 0) ? true : false;
            ProccessDisplayGUI(bDisplayData);

            // save selection
            ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID] = Int32.Parse(DdlNamHoc.SelectedValue);
            ViewState[AppConstant.VIEWSTATE_SELECTED_YEARNAME] = DdlNamHoc.SelectedItem.Text;
            ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTYID] = Int32.Parse(DdlNganh.SelectedValue);
            ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTYNAME] = DdlNganh.SelectedItem.Text;
            ViewState[AppConstant.VIEWSTATE_SELECTED_GRADEID] = Int32.Parse(DdlKhoiLop.SelectedValue);
            ViewState[AppConstant.VIEWSTATE_SELECTED_GRADENAME] = DdlKhoiLop.SelectedItem.Text;
            ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID] = Int32.Parse(DdlLopHoc.SelectedValue);
            ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSNAME] = DdlLopHoc.SelectedItem.Text;
            ViewState[AppConstant.VIEWSTATE_SELECTED_SUBJECTID] = Int32.Parse(DdlMonHoc.SelectedValue);
            ViewState[AppConstant.VIEWSTATE_SELECTED_SUBJECTNAME] = DdlMonHoc.SelectedItem.Text;
            ViewState[AppConstant.VIEWSTATE_SELECTED_MARKTYPEID] = Int32.Parse(DdlLoaiDiem.SelectedValue);
            ViewState[AppConstant.VIEWSTATE_SELECTED_MARKTYPENAME] = DdlLoaiDiem.SelectedItem.Text;
            ViewState[AppConstant.VIEWSTATE_SELECTED_TERMID] = Int32.Parse(DdlHocKy.SelectedValue);
            ViewState[AppConstant.VIEWSTATE_SELECTED_TERMNAME] = DdlHocKy.SelectedItem.Text;
        }

        private void ProccessDisplayGUI(bool bDisplayData)
        {
            RptDiemMonHoc.Visible = bDisplayData;
            MainDataPager.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            RptLoaiDiem.Visible = bDisplayData;
            tdSTT.Visible = bDisplayData;
            tdMaHocSinh.Visible = bDisplayData;
            tdHoTenHocSinh.Visible = bDisplayData;
            tdDTB.Visible = bDisplayData;
            thSelectAll.Visible = (accessibilities.Contains(AccessibilityEnum.Modify) && bDisplayData);
        }

        /// <summary>
        /// Get session from previous page
        /// </summary>
        private void RetrieveSessions()
        {
            if (CheckSessionKey(AppConstant.SESSION_SELECTED_YEAR)
               && CheckSessionKey(AppConstant.SESSION_SELECTED_FACULTY)
               && CheckSessionKey(AppConstant.SESSION_SELECTED_GRADE)
               && CheckSessionKey(AppConstant.SESSION_SELECTED_CLASS)
               && CheckSessionKey(AppConstant.SESSION_SELECTED_TERM)
               && CheckSessionKey(AppConstant.SESSION_SELECTED_MARKTYPE)
               && CheckSessionKey(AppConstant.SESSION_SELECTED_SUBJECT))
            {
                // get session and save key value to viewstate, then remove session
                DdlNamHoc.SelectedValue = ((Configuration_Year)GetSession(AppConstant.SESSION_SELECTED_YEAR)).YearId.ToString();
                RemoveSession(AppConstant.SESSION_SELECTED_YEAR);

                DdlNganh.SelectedValue = ((Category_Faculty)GetSession(AppConstant.SESSION_SELECTED_FACULTY)).FacultyId.ToString();
                RemoveSession(AppConstant.SESSION_SELECTED_FACULTY);

                DdlKhoiLop.SelectedValue = ((Category_Grade)GetSession(AppConstant.SESSION_SELECTED_GRADE)).GradeId.ToString();
                RemoveSession(AppConstant.SESSION_SELECTED_GRADE);

                DdlLopHoc.SelectedValue = ((Class_Class)GetSession(AppConstant.SESSION_SELECTED_CLASS)).ClassId.ToString();
                RemoveSession(AppConstant.SESSION_SELECTED_CLASS);

                String str = ((Category_Subject)GetSession(AppConstant.SESSION_SELECTED_SUBJECT)).SubjectId.ToString();
                DdlMonHoc.SelectedValue = str;
                RemoveSession(AppConstant.SESSION_SELECTED_SUBJECT);

                DdlLoaiDiem.SelectedValue = ((Category_MarkType)GetSession(AppConstant.SESSION_SELECTED_MARKTYPE)).MarkTypeId.ToString();
                RemoveSession(AppConstant.SESSION_SELECTED_MARKTYPE);

                DdlHocKy.SelectedValue = ((Configuration_Term)GetSession(AppConstant.SESSION_SELECTED_TERM)).TermId.ToString();
                RemoveSession(AppConstant.SESSION_SELECTED_TERM);
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            BindRptMarkTypes();
            BindRptStudentMarks();
        }

        protected void BtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            //Class_Class Class = new Class_Class();
            //Configuration_Term term = new Configuration_Term(); 
            //Category_Subject subject = new Category_Subject();            
            //Category_MarkType markType = new Category_MarkType();
            //Category_Faculty faculty = new Category_Faculty();
            //Category_Grade grade = new Category_Grade();

            //Class.ClassId = Int32.Parse(DdlLopHoc.SelectedValue);            
            //AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);
            //subject.SubjectId = Int32.Parse(DdlMonHoc.SelectedValue);
            //AddSession(AppConstant.SESSION_SELECTED_SUBJECT, subject);
            //term.TermId = Int32.Parse(DdlHocKy.SelectedValue);
            //AddSession(AppConstant.SESSION_SELECTED_TERM, term);
            //markType.MarkTypeId = Int32.Parse(DdlLoaiDiem.SelectedValue);
            //AddSession(AppConstant.SESSION_SELECTED_MARKTYPE, markType);
            //faculty.FacultyId = Int32.Parse(DdlNganh.SelectedValue);
            //AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);
            //grade.GradeId = Int32.Parse(DdlKhoiLop.SelectedValue);
            //AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);

            Response.Redirect(AppConstant.PAGEPATH_STUDENT_ADDMARK);
        }

        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            Configuration_Year year = new Configuration_Year();
            year.YearId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID];
            year.YearName = (string)ViewState[AppConstant.VIEWSTATE_SELECTED_YEARNAME];

            Category_Faculty faculty = new Category_Faculty();
            faculty.FacultyId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTYID];
            faculty.FacultyName = (string)ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTYNAME];

            Category_Grade grade = new Category_Grade();
            grade.GradeId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_GRADEID];
            grade.GradeName = (string)ViewState[AppConstant.VIEWSTATE_SELECTED_GRADENAME];

            Class_Class Class = new Class_Class();
            Class.ClassId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID];
            Class.ClassName = (string)ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSNAME];

            Category_Subject subject = new Category_Subject();
            subject.SubjectId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_SUBJECTID];
            subject.SubjectName = (string)ViewState[AppConstant.VIEWSTATE_SELECTED_SUBJECTNAME];

            Configuration_Term term = new Configuration_Term();
            term.TermId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_TERMID];
            term.TermName = (string)ViewState[AppConstant.VIEWSTATE_SELECTED_TERMNAME];

            Category_MarkType markType = new Category_MarkType();
            markType.MarkTypeId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_MARKTYPEID];
            markType.MarkTypeName = (string)ViewState[AppConstant.VIEWSTATE_SELECTED_MARKTYPENAME];

            List<Category_MarkType> markTypes = new List<Category_MarkType>();

            Student_Student student = new Student_Student();
            HiddenField hdfStudentId = null;
            HyperLink hlkStudentCode = null;
            HyperLink hlkStudentFullName = null;
            CheckBox ckbxSelect = null;
            Repeater rptMarkTypeBasedMarks = null;
            HiddenField hdfMarkTypeId = null;
            HiddenField hdfMarkTypeName = null;

            foreach (RepeaterItem item in RptDiemMonHoc.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    ckbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (ckbxSelect.Checked)
                    {
                        rptMarkTypeBasedMarks = (Repeater)item.FindControl("RptDiemTheoLoaiDiem");
                        foreach (RepeaterItem innerItem in rptMarkTypeBasedMarks.Items)
                        {
                            if (innerItem.ItemType == ListItemType.Item || innerItem.ItemType == ListItemType.AlternatingItem)
                            {
                                hdfMarkTypeId = (HiddenField)innerItem.FindControl("HdfMarkTypeId");
                                hdfMarkTypeName = (HiddenField)innerItem.FindControl("HdfMarkTypeName");
                                markTypes.Add(new Category_MarkType { MarkTypeId = Int32.Parse(hdfMarkTypeId.Value), MarkTypeName = hdfMarkTypeName.Value });
                            }
                        }

                        hdfStudentId = (HiddenField)item.FindControl("HdfStudentId");
                        hlkStudentCode = (HyperLink)item.FindControl("HlkStudentCode");
                        hlkStudentFullName = (HyperLink)item.FindControl("HlkStudentFullName");
                        student.StudentId = Int32.Parse(hdfStudentId.Value);
                        student.StudentCode = hlkStudentCode.Text;
                        student.FullName = hlkStudentFullName.Text;

                        AddSession(AppConstant.SESSION_SELECTED_YEAR, year);
                        AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);
                        AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);
                        AddSession(AppConstant.SESSION_SELECTED_CLASS, Class); 
                        AddSession(AppConstant.SESSION_SELECTED_TERM, term);
                        AddSession(AppConstant.SESSION_SELECTED_SUBJECT, subject); 
                        AddSession(AppConstant.SESSION_SELECTED_MARKTYPE, markType);
                        
                        AddSession(AppConstant.SESSION_SELECTED_MARKTYPES, markTypes);
                        AddSession(AppConstant.SESSION_SELECTED_STUDENT, student);
                        Response.Redirect(AppConstant.PAGEPATH_STUDENT_EDITMARK);
                    }
                }
            }
        }
        #endregion

        #region Repeater event handlers
        protected void RptDiemMonHoc_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.FindControl("tdSelect").Visible = (accessibilities.Contains(AccessibilityEnum.Modify) || accessibilities.Contains(AccessibilityEnum.Delete));
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TabularStudentMark tabularStudentMark = (TabularStudentMark)e.Item.DataItem;
                Repeater rptMarkTypeBasedMarks = (Repeater)e.Item.FindControl("RptDiemTheoLoaiDiem");

                rptMarkTypeBasedMarks.DataSource = tabularStudentMark.DiemTheoLoaiDiems;
                rptMarkTypeBasedMarks.DataBind();
            }
        }
        #endregion

        #region Pager event handlers
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currnetPageIndx;
            BindRptStudentMarks();
        }
        #endregion
    }
}