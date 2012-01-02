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
                BindRptMarkTypes();
                BindRptStudentMarks();
            }
        }
        #endregion

        #region DropDownList event hanlders
        protected void DdlNamHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLLopHoc();
        }

        protected void DdlNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLLopHoc();
        }

        protected void DdlKhoiLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLLopHoc();
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
            BindDDLKhoiLop();
            BindDDLLopHoc();
            BindDDLMonHoc();
            BindDDLoaiDiem();
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

        private void BindDDLKhoiLop()
        {
            GradeBL grades = new GradeBL(UserSchool);
            List<Category_Grade> lstKhoiLop = grades.GetListGrades();
            DdlKhoiLop.DataSource = lstKhoiLop;
            DdlKhoiLop.DataValueField = "GradeId";
            DdlKhoiLop.DataTextField = "GradeName";
            DdlKhoiLop.DataBind();
            if (lstKhoiLop.Count > 1)
            {
                DdlKhoiLop.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }

        private void BindDDLoaiDiem()
        {
            MarkTypeBL loaiDiemBL = new MarkTypeBL(UserSchool);
            List<Category_MarkType> lstLoaiDiem = loaiDiemBL.GetListMarkTypes();
            DdlLoaiDiem.DataSource = lstLoaiDiem;
            DdlLoaiDiem.DataValueField = "MarkTypeName";
            DdlLoaiDiem.DataTextField = "MarkTypeName";
            DdlLoaiDiem.DataBind();
            if (lstLoaiDiem.Count > 1)
            {
                DdlLoaiDiem.Items.Insert(0, new ListItem("Tất cả", ""));
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

            try
            {
                if (DdlKhoiLop.SelectedIndex > 0)
                {
                    grade = new Category_Grade();
                    grade.GradeId = Int32.Parse(DdlKhoiLop.SelectedValue);
                }
            }
            catch (Exception) { }

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
            MarkTypeBL markTypeBL = new MarkTypeBL(UserSchool);
            List<Category_MarkType> markTypes = new List<Category_MarkType>();

            if (DdlLoaiDiem.Items.Count != 0)
            {
                if (DdlLoaiDiem.SelectedIndex == 0)
                {
                    markTypes = markTypeBL.GetListMarkTypes();
                }
                else
                {
                    string markTypeName = DdlLoaiDiem.SelectedValue;
                    markTypes.Add(markTypeBL.GetMarkType(markTypeName));
                }
            }

            this.RptLoaiDiem.DataSource = markTypes;
            this.RptLoaiDiem.DataBind();
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
            if (DdlLoaiDiem.SelectedIndex == 0)
            {
                markTypes = markTypeBL.GetListMarkTypes();
            }
            else
            {
                string markTypeName = DdlLoaiDiem.SelectedValue;
                markTypes.Add(markTypeBL.GetMarkType(markTypeName));
            }

            if (RBtnTerm.Checked)
            {
                // get student mark information
                tabularStudentMarks = studyingResultBL.GetTabularStudentMarks(Class, subject, term, markTypes,
                    MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);
            } else if(RBtnMonth.Checked)
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
        }
        #endregion

        #region Button event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            //isSearch = true;
            BindRptMarkTypes();
            BindRptStudentMarks();
        }
        #endregion

        #region Repeater event handlers
        protected void RptDiemMonHoc_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item
                || e.Item.ItemType == ListItemType.AlternatingItem)
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