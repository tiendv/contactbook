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
    public partial class StudentsPage : BaseContentPage
    {
        #region Fields
        private StudentBL studentBL;
        private bool isSearch;
        ReportDocument RptDocument = new ReportDocument();
        #endregion
        
        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            //CrystalReportViewer1.Visible = false;
            base.Page_Load(sender, e);
            if (accessDenied)
            {
                return;
            }

            studentBL = new StudentBL(UserSchool);

            if (!Page.IsPostBack)
            {
                BindDropDownLists();
                this.isSearch = false;

                // Khôi phục lại thông tin khi chuyển sang trang khác rồi trở về trang này
                    if (CheckSessionKey(AppConstant.SESSION_SELECTED_YEAR) 
                        && CheckSessionKey(AppConstant.SESSION_SELECTED_FACULTY) 
                        && CheckSessionKey(AppConstant.SESSION_SELECTED_GRADE)
                        && CheckSessionKey(AppConstant.SESSION_SELECTED_CLASS)
                        && CheckSessionKey(AppConstant.SESSION_SELECTED_STUDENTCODE)
                        && CheckSessionKey(AppConstant.SESSION_SELECTED_STUDENTNAME))
                {
                    Configuration_Year year = (Configuration_Year)GetSession(AppConstant.SESSION_SELECTED_YEAR);
                    RemoveSession(AppConstant.SESSION_SELECTED_YEAR);
                    DdlNamHoc.SelectedValue = year.YearId.ToString();

                    Category_Faculty faculty = (Category_Faculty)GetSession(AppConstant.SESSION_SELECTED_FACULTY);
                    RemoveSession(AppConstant.SESSION_SELECTED_FACULTY);
                    DdlNganh.SelectedValue = faculty.FacultyId.ToString();

                    Category_Grade grade = (Category_Grade)GetSession(AppConstant.SESSION_SELECTED_GRADE);
                    RemoveSession(AppConstant.SESSION_SELECTED_GRADE);
                    DdlKhoiLop.SelectedValue = grade.GradeId.ToString();

                    Class_Class Class = (Class_Class)GetSession(AppConstant.SESSION_SELECTED_CLASS);
                    RemoveSession(AppConstant.SESSION_SELECTED_CLASS);
                    DdlLopHoc.SelectedValue = Class.ClassId.ToString();

                    String strStudentName = (string)GetSession(AppConstant.SESSION_SELECTED_STUDENTNAME);
                    RemoveSession(AppConstant.SESSION_SELECTED_STUDENTNAME);
                    TxtTenHocSinh.Text = strStudentName;

                    String strStudentCode = (string)GetSession(AppConstant.SESSION_SELECTED_STUDENTCODE);
                    RemoveSession(AppConstant.SESSION_SELECTED_STUDENTCODE);
                    TxtMaHocSinh.Text = strStudentCode;

                    isSearch = true;
                }

                if (DdlLopHoc.Items.Count != 0)
                {
                    BindRptStudents();
                }
                else
                {
                    ProcessDislayInfo(false);
                }

                ProcPermissions();
            }
        }
        #endregion

        #region DropDownList event hanlders
        protected void DdlNamHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLClasses();
        }

        protected void DdlNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLClasses();
        }

        protected void DdlKhoiLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLClasses();
        }
        #endregion

        #region Repeater event handlers
        protected void RptHocSinh_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (accessibilities.Contains(AccessibilityEnum.Modify))
            {
                // Do something
            }
            else
            {
                if (e.Item.ItemType == ListItemType.Header)
                {
                    e.Item.FindControl("thEdit").Visible = false;
                }

                if (e.Item.ItemType == ListItemType.Item ||
                    e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    e.Item.FindControl("tdEdit").Visible = false;
                }
            }

            if (accessibilities.Contains(AccessibilityEnum.Delete))
            {
                if (e.Item.ItemType == ListItemType.Item
                    || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    Control control = e.Item.FindControl("LbtnMaHocSinhHienThi");
                    if (control != null)
                    {
                        string maHocSinhHienThi = ((LinkButton)control).Text;
                        if (!studentBL.IsDeletable(maHocSinhHienThi))
                        {
                            ImageButton btnDeleteItem = (ImageButton)e.Item.FindControl("BtnDeleteItem");
                            btnDeleteItem.ImageUrl = "~/Styles/Images/button_delete_disable.png";
                            btnDeleteItem.Enabled = false;
                        }
                    }
                }
            }
            else
            {
                if (e.Item.ItemType == ListItemType.Header)
                {
                    e.Item.FindControl("thDelete").Visible = false;
                }

                if (e.Item.ItemType == ListItemType.Item ||
                    e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    e.Item.FindControl("tdDelete").Visible = false;
                }

                this.PnlPopupConfirmDelete.Visible = false;
            }
        }

        protected void RptHocSinh_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDetailItem":
                    {
                        Configuration_Year year = new Configuration_Year();
                        year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);
                        AddSession(AppConstant.SESSION_SELECTED_YEAR, year);

                        Category_Faculty faculty = new Category_Faculty();
                        faculty.FacultyId = Int32.Parse(DdlNganh.SelectedValue);
                        AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);

                        Category_Grade grade = new Category_Grade();
                        grade.GradeId = Int32.Parse(DdlKhoiLop.SelectedValue);
                        AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);

                        Class_Class Class = new Class_Class();
                        Class.ClassId = Int32.Parse(DdlLopHoc.SelectedValue);
                        AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);

                        String strStudentName = TxtTenHocSinh.Text;
                        AddSession(AppConstant.SESSION_SELECTED_STUDENTNAME, strStudentName);

                        String strStudentCode = TxtMaHocSinh.Text;
                        AddSession(AppConstant.SESSION_SELECTED_STUDENTCODE, strStudentCode);

                        Student_Student student = new Student_Student();
                        student.StudentId = Int32.Parse(e.CommandArgument.ToString());
                        AddSession(AppConstant.SESSION_STUDENT, student);

                        Class_Class studentClass = new Class_Class();
                        studentClass.ClassId = Int32.Parse(((HiddenField)e.Item.FindControl("HdfClassId")).Value);
                        AddSession(AppConstant.SESSION_STUDENTCLASS, Class);

                        Response.Redirect(AppConstant.PAGEPATH_STUDENTINFOR);
                        break;
                    }
                case "CmdEditItem":
                    {
                        Configuration_Year year = new Configuration_Year();
                        year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);
                        AddSession(AppConstant.SESSION_SELECTED_YEAR, year);

                        Category_Faculty faculty = new Category_Faculty();
                        faculty.FacultyId = Int32.Parse(DdlNganh.SelectedValue);
                        AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);

                        Category_Grade grade = new Category_Grade();
                        grade.GradeId = Int32.Parse(DdlKhoiLop.SelectedValue);
                        AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);

                        Class_Class Class = new Class_Class();
                        Class.ClassId = Int32.Parse(DdlLopHoc.SelectedValue);
                        AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);

                        String strStudentName = TxtTenHocSinh.Text;
                        AddSession(AppConstant.SESSION_SELECTED_STUDENTNAME, strStudentName);

                        String strStudentCode = TxtMaHocSinh.Text;
                        AddSession(AppConstant.SESSION_SELECTED_STUDENTCODE, strStudentCode);

                        // Get seleteced student and set to session
                        Student_Student student = new Student_Student();
                        student.StudentId = Int32.Parse(e.CommandArgument.ToString());
                        AddSession(AppConstant.SESSION_STUDENT, student);

                        // Get seleteced class and set to session
                        Class_Class studentClass = new Class_Class();
                        studentClass.ClassId = Int32.Parse(((HiddenField)e.Item.FindControl("HdfClassId")).Value);
                        AddSession(AppConstant.SESSION_STUDENTCLASS, studentClass);

                        AddSession(AppConstant.SESSION_PREV_PAGE, Request.Path);

                        // redirect to "Sửa học sinh"
                        Response.Redirect(AppConstant.PAGEPATH_STUDENTEDIT);
                        break;
                    }
                case "CmdDeleteItem":
                    {
                        // Set confirm text and show dialog
                        this.LblConfirmDelete.Text = "Bạn có chắc xóa học sinh <b>" + e.CommandArgument + "</b> này không?";
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        // Save current MaHocSinh to global
                        HiddenField hdfRptMaHocSinh = (HiddenField)e.Item.FindControl("HdfRptMaHocSinh");
                        this.HdfMaHocSinh.Value = hdfRptMaHocSinh.Value;

                        // Save modal popup ClientID
                        this.HdfRptHocSinhMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnPrint_Click(object sender, ImageClickEventArgs e)
        {
            #region Add Info 2 Session
            Configuration_Year year = null;
            Category_Faculty faculty = null;
            Category_Grade grade = null;
            Class_Class Class = null;
            string studentName = this.TxtTenHocSinh.Text;
            string studentCode = this.TxtMaHocSinh.Text;

            year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);
            year.YearName = DdlNamHoc.SelectedItem.Text;
            try
            {
                faculty = new Category_Faculty();
                if (DdlNganh.SelectedIndex > 0)
                {                    
                    faculty.FacultyId = Int32.Parse(DdlNganh.SelectedValue);
                    faculty.FacultyName = DdlNganh.SelectedItem.Text;
                }
            }
            catch (Exception) { }

            try
            {
                grade = new Category_Grade();
                if (DdlKhoiLop.SelectedIndex > 0)
                {
                    
                    grade.GradeId = Int32.Parse(DdlKhoiLop.SelectedValue);
                    grade.GradeName = DdlKhoiLop.SelectedItem.Text;
                }
            }
            catch (Exception) { }

            try
            {
                Class = new Class_Class();
                if (DdlLopHoc.SelectedIndex > 0)
                {
                    
                    Class.ClassId = Int32.Parse(DdlLopHoc.SelectedValue);
                    Class.ClassName = DdlLopHoc.SelectedItem.Text;
                }

            }
            catch (Exception) { }

            AddSession(AppConstant.SESSION_SELECTED_YEAR, year);
            AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);
            AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);
            AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);
            AddSession(AppConstant.SESSION_SELECTED_STUDENTNAME, studentName);
            AddSession(AppConstant.SESSION_SELECTED_STUDENTCODE, studentCode);
            Response.Redirect(AppConstant.PAGEPATH_PRINTSTUDENTS);
            #endregion
        }

        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            isSearch = true;
            BindRptStudents();

            // Save searched info
            this.HdfSearchNamHoc.Value = this.DdlNamHoc.SelectedValue;
            this.HdfSearchKhoiLop.Value = this.DdlKhoiLop.SelectedValue;
            this.HdfSearchNganhHoc.Value = this.DdlNganh.SelectedValue;
            this.HdfSearchLopHoc.Value = this.DdlLopHoc.SelectedValue;
            this.HdfSearchTenHocSinh.Value = this.TxtTenHocSinh.Text;
            this.HdfSearchMaHocSinh.Value = this.TxtMaHocSinh.Text;
        }

        protected void BtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("themhocsinh.aspx");
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            Student_Student student = new Student_Student();
            student.StudentId = Int32.Parse(this.HdfMaHocSinh.Value);
            studentBL.DeleteStudent(student);

            isSearch = false;
            BindDDLClasses();
            BindRptStudents();
        }
        #endregion

        #region Pager event handlers
        public void pager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currentPageIndex;
            BindRptStudents();
        }
        #endregion

        #region Methods
        private void ProcPermissions()
        {
            if (accessibilities.Contains(AccessibilityEnum.Add))
            {
                BtnAdd.Enabled = true;
                BtnAdd.ImageUrl = "~/Styles/Images/button_add_with_text.png";
            }
            else
            {
                BtnAdd.Visible = false;
            }
        }

        private void BindRptStudents()
        {
            List<TabularStudent> tabularStudents = new List<TabularStudent>();
            double dTotalRecords;
            Configuration_Year year = null;
            Category_Faculty faculty = null;
            Category_Grade grade = null;
            Class_Class Class = null;
            string studentName = this.TxtTenHocSinh.Text;
            string studentCode = this.TxtMaHocSinh.Text;

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

            try
            {
                if (DdlLopHoc.SelectedIndex > 0)
                {
                    Class = new Class_Class();
                    Class.ClassId = Int32.Parse(DdlLopHoc.SelectedValue);
                }
            }
            catch (Exception) { }

            tabularStudents = studentBL.GetTabularStudents(year, faculty, grade, Class, studentCode, studentName,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            // Decrease page current index when delete
            if (tabularStudents.Count == 0 && dTotalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRptStudents();
                return;
            }

            bool bDisplayData = (tabularStudents.Count != 0) ? true : false;
            ProcessDislayInfo(bDisplayData);
            RptHocSinh.DataSource = tabularStudents;
            RptHocSinh.DataBind();
            MainDataPager.ItemCount = dTotalRecords;
        }

        private void ProcessDislayInfo(bool bDisplayData)
        {
            PnlPopupConfirmDelete.Visible = bDisplayData;
            RptHocSinh.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin học sinh";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy học sinh";
                }
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
            }
            else
            {
                MainDataPager.Visible = true;
            }
        }

        private void BindDropDownLists()
        {
            BindDropDownListNamHoc();

            BindDDLFaculties();

            BindDDLGrades();

            BindDDLClasses();
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

        private void BindDropDownListNamHoc()
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
                DdlNamHoc.SelectedValue = cauHinhBL.GetCurrentYear().ToString();
            }
        }

        private void BindDDLClasses()
        {
            ClassBL classBL = new ClassBL(UserSchool);
            List<Class_Class> classes = null;
            Configuration_Year year = null;
            Category_Faculty faculty = null;
            Category_Grade grade = null;

            if (DdlNamHoc.Items.Count == 0 || DdlNganh.Items.Count == 0 || DdlKhoiLop.Items.Count == 0)
            {
                BtnSearch.ImageUrl = AppConstant.IMAGESOURCE_SEARCH_DISABLE;
                BtnSearch.Enabled = false;
                BtnAdd.ImageUrl = AppConstant.IMAGESOURCE_ADD_DISABLE;
                BtnAdd.Enabled = false;

                PnlPopupConfirmDelete.Visible = false;
                RptHocSinh.Visible = false;
                LblSearchResult.Visible = true;
                LblSearchResult.Text = "Chưa có thông tin học sinh";

                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;

                return;
            }

            year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);

            if ((DdlNganh.Items.Count == 1) || (DdlNganh.Items.Count > 1 && DdlNganh.SelectedIndex > 0))
            {
                faculty = new Category_Faculty();
                faculty.FacultyId = Int32.Parse(DdlNganh.SelectedValue);
            }

            if ((DdlKhoiLop.Items.Count == 1) || (DdlKhoiLop.Items.Count > 1 && DdlKhoiLop.SelectedIndex > 0))
            {
                grade = new Category_Grade();
                grade.GradeId = Int32.Parse(DdlKhoiLop.SelectedValue);
            }

            classes = classBL.GetListClasses(year, faculty, grade);
            DdlLopHoc.DataSource = classes;
            DdlLopHoc.DataValueField = "ClassId";
            DdlLopHoc.DataTextField = "ClassName";
            DdlLopHoc.DataBind();
            if (classes.Count > 1)
            {
                DdlLopHoc.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }
        #endregion
    }
}