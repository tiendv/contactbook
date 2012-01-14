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
                GetSessions();
                BindRptMarkTypes();
                BindRptStudentMarks();
            }
        }
        #endregion

        #region DropDownList event hanlders
        protected void DdlNamHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLLopHoc();
            BindDDLMonHoc();
        }

        protected void DdlNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLLopHoc();
        }

        protected void DdlKhoiLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLLopHoc();
            BindDDLMarkTypes();
        }

        protected void DdlHocKy_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLMonHoc();
        }

        protected void DdlLopHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLMonHoc();
        }
        #endregion

        #region Methods
        private void BindDropDownLists()
        {
            BindDDLNamHoc();
            BindDDLHocKy();
            BindDDLMonths();
            BindDDLNganhHoc();
            BindDDLGrades();
            BindDDLLopHoc();
            BindDDLMonHoc();
            BindDDLMarkTypes();
        }

        private void BindDDLNamHoc()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<Configuration_Year> years = systemConfigBL.GetListYears();
            DdlNamHoc.DataSource = years;
            DdlNamHoc.DataValueField = "YearId";
            DdlNamHoc.DataTextField = "YearName";
            DdlNamHoc.DataBind();

            if (DdlNamHoc.Items.Count != 0)
            {
                SystemConfigBL cauHinhBL = new SystemConfigBL(UserSchool);
                DdlNamHoc.SelectedValue = cauHinhBL.GetLastedYear().ToString();
            }
        }

        private void BindDDLHocKy()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<Configuration_Term> lstHocKy = systemConfigBL.GetListTerms();
            DdlHocKy.DataSource = lstHocKy;
            DdlHocKy.DataValueField = "TermId";
            DdlHocKy.DataTextField = "TermName";
            DdlHocKy.DataBind();
        }

        private void BindDDLMonths()
        {
            DddMonths.Items.Add(new ListItem("Tất cả", "0"));
            DddMonths.Items.Add(new ListItem("Tháng 1", "1"));
            DddMonths.Items.Add(new ListItem("Tháng 2", "2"));
            DddMonths.Items.Add(new ListItem("Tháng 3", "3"));
            DddMonths.Items.Add(new ListItem("Tháng 4", "4"));
            DddMonths.Items.Add(new ListItem("Tháng 5", "5"));
            DddMonths.Items.Add(new ListItem("Tháng 6", "6"));
            DddMonths.Items.Add(new ListItem("Tháng 7", "7"));
            DddMonths.Items.Add(new ListItem("Tháng 8", "8"));
            DddMonths.Items.Add(new ListItem("Tháng 9", "9"));
            DddMonths.Items.Add(new ListItem("Tháng 10", "10"));
            DddMonths.Items.Add(new ListItem("Tháng 11", "11"));
            DddMonths.Items.Add(new ListItem("Tháng 12", "12"));
        }

        private void BindDDLNganhHoc()
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

        private void BindDDLLopHoc()
        {
            Configuration_Year year = null;
            Category_Faculty faculty = null;
            Category_Grade grade = null;

            if (DdlNamHoc.Items.Count == 0 || DdlNganh.Items.Count == 0 || DdlKhoiLop.Items.Count == 0)
            {
                BtnSearch.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SEARCH_DISABLE;
                BtnSearch.Enabled = false;
                return;
            }

            year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);

            try
            {
                if (DdlNganh.SelectedIndex > 0)
                {
                    faculty = new Category_Faculty();
                    faculty.FacultyId = Int32.Parse(DdlNganh.SelectedValue);
                }
            }
            catch (Exception) { }

            if (DdlKhoiLop.Items.Count != 0)
            {
                if (DdlKhoiLop.SelectedIndex >= 0)
                {
                    grade = new Category_Grade();
                    grade.GradeId = Int32.Parse(DdlKhoiLop.SelectedValue);
                }
            }

            ClassBL lopHocBL = new ClassBL(UserSchool);
            List<Class_Class> lstLop = lopHocBL.GetListClasses(year, faculty, grade);
            DdlLopHoc.DataSource = lstLop;
            DdlLopHoc.DataValueField = "ClassId";
            DdlLopHoc.DataTextField = "ClassName";
            DdlLopHoc.DataBind();

            if (DdlLopHoc.Items.Count != 0)
            {
                BtnSearch.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SEARCH;
                BtnSearch.Enabled = true;
            }
            else
            {
                BtnSearch.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SEARCH_DISABLE;
                BtnSearch.Enabled = false;
            }
        }

        private void BindDDLMonHoc()
        {
            Class_Class Class = null;
            Configuration_Term term = null;
            ScheduleBL scheduleBL = new ScheduleBL(UserSchool);

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
            // declare variables
            Class_Class Class = null;
            Category_Subject subject = null;
            Configuration_Term term = null;
            MarkTypeBL markTypeBL = new MarkTypeBL(UserSchool);
            List<Category_MarkType> markTypes = new List<Category_MarkType>();
            List<TabularStudentMark> tabularStudentMarks = new List<TabularStudentMark>();
            double dTotalRecords = 0;

            // case: there is no Class or schedule subject or marktype
            if (DdlLopHoc.Items.Count == 0 || DdlMonHoc.Items.Count == 0 || DdlLoaiDiem.Items.Count == 0)
            {
                // do not display 
                ProcDisplayGUI(false);
                return;
            }

            // init object against user selections
            Class = new Class_Class();
            Class.ClassId = Int32.Parse(DdlLopHoc.SelectedValue);
            subject = new Category_Subject();
            subject.SubjectId = Int32.Parse(DdlMonHoc.SelectedValue);
            term = new Configuration_Term();
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

            if (RBtnTerm.Checked)
            {
                // get student mark information
                tabularStudentMarks = studyingResultBL.GetTabularStudentMarks(Class, subject, term, markTypes,
                    MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);
            }
            else if (RBtnMonth.Checked)
            {
                int month = Int32.Parse(DddMonths.SelectedValue);
                // get student mark information
                tabularStudentMarks = studyingResultBL.GetTabularStudentMarks(Class, subject, month, markTypes,
                    MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);
            }


            // bind to repeater and datapager
            this.RptDiemMonHoc.DataSource = tabularStudentMarks;
            this.RptDiemMonHoc.DataBind();
            MainDataPager.ItemCount = dTotalRecords;

            // display information
            bool bDisplayData = (tabularStudentMarks.Count != 0) ? true : false;
            ProcDisplayGUI(bDisplayData);

            // save selection
            ViewState[AppConstant.VIEWSTATE_SELECTED_YEAR] = Int32.Parse(DdlNamHoc.SelectedValue);
            ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTY] = Int32.Parse(DdlNganh.SelectedValue);
            ViewState[AppConstant.VIEWSTATE_SELECTED_GRADE] = Int32.Parse(DdlKhoiLop.SelectedValue);
            ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS] = Int32.Parse(DdlLopHoc.SelectedValue);
            ViewState[AppConstant.VIEWSTATE_SELECTED_SUBJECT] = Int32.Parse(DdlMonHoc.SelectedValue);
            ViewState[AppConstant.VIEWSTATE_SELECTED_MARKTYPE] = Int32.Parse(DdlLoaiDiem.SelectedValue);
            ViewState[AppConstant.VIEWSTATE_SELECTED_TERM] = Int32.Parse(DdlHocKy.SelectedValue);
        }

        private void ProcDisplayGUI(bool bDisplayData)
        {
            RptDiemMonHoc.Visible = bDisplayData;
            MainDataPager.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            RptLoaiDiem.Visible = bDisplayData;
            tdSTT.Visible = bDisplayData;
            tdMaHocSinh.Visible = bDisplayData;
            tdHoTenHocSinh.Visible = bDisplayData;
            tdDTB.Visible = bDisplayData;
            tdSelectAll.Visible = bDisplayData;
        }

        /// <summary>
        /// Get session from previous page
        /// </summary>
        private void GetSessions()
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
                
                DdlMonHoc.SelectedValue = ((Category_Subject)GetSession(AppConstant.SESSION_SELECTED_SUBJECT)).SubjectId.ToString();
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
            year.YearId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_YEAR];
            Category_Faculty faculty = new Category_Faculty();
            faculty.FacultyId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTY];
            Category_Grade grade = new Category_Grade();
            grade.GradeId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_GRADE];
            Class_Class Class = new Class_Class();
            Class.ClassId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS];
            Category_Subject subject = new Category_Subject();
            subject.SubjectId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_SUBJECT];
            Configuration_Term term = new Configuration_Term();
            term.TermId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_TERM];
            Category_MarkType markType = new Category_MarkType();
            markType.MarkTypeId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_MARKTYPE];
            List<Category_MarkType> markTypes = new List<Category_MarkType>();

            Student_Student student = new Student_Student();

            foreach (RepeaterItem item in RptDiemMonHoc.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox CkbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (CkbxSelect.Checked)
                    {
                        Repeater rptMarkTypeBasedMarks = (Repeater)item.FindControl("RptDiemTheoLoaiDiem");
                        foreach (RepeaterItem innerItem in rptMarkTypeBasedMarks.Items)
                        {
                            if (innerItem.ItemType == ListItemType.Item || innerItem.ItemType == ListItemType.AlternatingItem)
                            {
                                HiddenField hdfMarkTypeId = (HiddenField)innerItem.FindControl("HdfMarkTypeId");
                                HiddenField hdfMarkTypeName = (HiddenField)innerItem.FindControl("HdfMarkTypeName");
                                markTypes.Add(new Category_MarkType { MarkTypeId = Int32.Parse(hdfMarkTypeId.Value), MarkTypeName = hdfMarkTypeName.Value});
                            }
                        }

                        HiddenField hdfStudentId = (HiddenField)item.FindControl("HdfStudentId");
                        HyperLink HlkStudentCode = (HyperLink)item.FindControl("HlkStudentCode");
                        HyperLink HlkStudentFullName = (HyperLink)item.FindControl("HlkStudentFullName");
                        student.StudentId = Int32.Parse(hdfStudentId.Value);
                        student.StudentCode = HlkStudentCode.Text;
                        student.FullName = HlkStudentFullName.Text;

                        AddSession(AppConstant.SESSION_SELECTED_YEAR, year);
                        AddSession(AppConstant.SESSION_SELECTED_TERM, term);
                        AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);
                        AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);
                        AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);
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