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
    public partial class ApproveStudentMarkPage : BaseContentPage
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
                    BtnApprove.Enabled = false;
                    BtnApprove.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_ADD_DISABLE;
                }
                else
                {
                    RetrieveSessions();
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
            //BtnApprove.Visible = BtnAdd.Visible && accessibilities.Contains(AccessibilityEnum.Add);
            //BtnEdit.Visible = accessibilities.Contains(AccessibilityEnum.Modify);
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
            BindDddStatus();
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

            if (DdlNamHoc.Items.Count != 0 && CurrentYear != null)
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

            if (DdlNganh.Items.Count != 0)
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

        private void BindDddStatus()
        {
            DdlStatus.Items.Add(new ListItem("Tất cả", "0"));
            DdlStatus.Items.Add(new ListItem("Đã phê chuẩn", "1"));
            DdlStatus.Items.Add(new ListItem("Chưa phê chuẩn", "-1"));
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

                if (DdlMonHoc.Items.Count > 1)
                {
                    DdlMonHoc.Items.Insert(0, new ListItem("Tất cả", "0"));
                }
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
            List<Category_Subject> subjects = new List<Category_Subject>();
            Configuration_Term term = new Configuration_Term();
            MarkTypeBL markTypeBL = new MarkTypeBL(UserSchool);
            Category_MarkType markType = null;
            List<ConsideredStudentMark> consideredStudentMarks = new List<ConsideredStudentMark>();
            double dTotalRecords = 0;

            Class.ClassId = Int32.Parse(DdlLopHoc.SelectedValue);
            if ((DdlMonHoc.Items.Count == 1 && DdlMonHoc.SelectedIndex == 0) || (DdlMonHoc.Items.Count > 1 && DdlMonHoc.SelectedIndex > 0))
            {
                subjects.Add(new Category_Subject { SubjectId = Int32.Parse(DdlMonHoc.SelectedValue) });
            }
            else
            {
                foreach (ListItem subject in DdlMonHoc.Items)
                {
                    subjects.Add(new Category_Subject { SubjectId = Int32.Parse(subject.Value) });
                }
            }

            term.TermId = Int32.Parse(DdlHocKy.SelectedValue);

            if (DdlKhoiLop.Items.Count != 0)
            {
                Category_Grade grade = new Category_Grade();
                grade.GradeId = Int32.Parse(DdlKhoiLop.SelectedValue);
                if (DdlLoaiDiem.SelectedIndex > 0)
                {
                    string markTypeName = DdlLoaiDiem.SelectedItem.Text;
                    markType = markTypeBL.GetMarkType(grade, markTypeName);
                }
            }

            bool? bApprovedStatus = null;
            if (DdlStatus.SelectedValue == "1")
            {
                bApprovedStatus = true;
            }
            else if (DdlStatus.SelectedValue == "-1")
            {
                bApprovedStatus = false;
            }

            if (RBtnMonth.Checked)
            {
                if (DddMonths.SelectedIndex > 0)
                {
                    int month = Int32.Parse(DddMonths.SelectedValue);
                    consideredStudentMarks = studyingResultBL.GetConsideredStudentMarks(Class, subjects, term, month, markType, bApprovedStatus,
                        MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);
                }
                else
                {
                    consideredStudentMarks = studyingResultBL.GetConsideredStudentMarks(Class, subjects, term, markType, bApprovedStatus,
                     MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);
                }
            }
            //else if (RBtnWeek.Checked)
            //{
            //    if (DdlWeeks.SelectedIndex > 0)
            //    {
            //        string[] strDates = DdlWeeks.SelectedValue.Split('-');
            //        DateTime dtBeginDate = DateTime.Parse(strDates[0]);
            //        DateTime dtEndDate = DateTime.Parse(strDates[1]);
            //        // get student mark information
            //        consideredStudentMarks = studyingResultBL.GetTabularStudentMarks(Class, subject, term, dtBeginDate, dtEndDate, markTypes,
            //            MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);
            //    }
            //    else
            //    {
            //        consideredStudentMarks = studyingResultBL.GetTabularStudentMarks(Class, subject, term, markTypes, bApprovedStatus,
            //         MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);
            //    }
            //}


            // bind to repeater and datapager
            this.RptDiemMonHoc.DataSource = consideredStudentMarks;
            this.RptDiemMonHoc.DataBind();
            MainDataPager.ItemCount = dTotalRecords;

            // display information
            bool bDisplayData = (consideredStudentMarks.Count != 0) ? true : false;
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
            BindRptStudentMarks();
        }

        protected void BtnApprove_Click(object sender, ImageClickEventArgs e)
        {
            CheckBox ckbxSelect = null;
            HiddenField HdfDetailTermSubjectMarkId = null;
            HiddenField HdfStatus = null;
            Student_DetailedTermSubjectMark detail = null;
            TextBox TxtNote = null;

            foreach (RepeaterItem item in RptDiemMonHoc.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    ckbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (ckbxSelect.Checked)
                    {
                        HdfStatus = (HiddenField)item.FindControl("HdfStatus");
                        if (HdfStatus.Value == "False")
                        {
                            HdfDetailTermSubjectMarkId = (HiddenField)item.FindControl("HdfDetailTermSubjectMarkId");
                            detail = new Student_DetailedTermSubjectMark();
                            TxtNote = (TextBox)item.FindControl("TxtNote");
                            detail.DetailedTermSubjectMark = Int32.Parse(HdfDetailTermSubjectMarkId.Value);
                            studyingResultBL.ApproveMark(detail, TxtNote.Text);
                        }
                    }
                }
            }

            BindRptStudentMarks();
        }

        protected void BtnUnApprove_Click(object sender, ImageClickEventArgs e)
        {
            CheckBox ckbxSelect = null;
            HiddenField HdfDetailTermSubjectMarkId = null;
            HiddenField HdfStatus = null;
            TextBox TxtNote = null;
            Student_DetailedTermSubjectMark detail = null;

            foreach (RepeaterItem item in RptDiemMonHoc.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    ckbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (ckbxSelect.Checked)
                    {
                        HdfStatus = (HiddenField)item.FindControl("HdfStatus");
                        if (HdfStatus.Value == "True")
                        {
                            HdfDetailTermSubjectMarkId = (HiddenField)item.FindControl("HdfDetailTermSubjectMarkId");
                            detail = new Student_DetailedTermSubjectMark();
                            TxtNote = (TextBox)item.FindControl("TxtNote");
                            detail.DetailedTermSubjectMark = Int32.Parse(HdfDetailTermSubjectMarkId.Value);
                            studyingResultBL.UnapproveMark(detail, TxtNote.Text);
                        }
                    }
                }
            }

            BindRptStudentMarks();
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