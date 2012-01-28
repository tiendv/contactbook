using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.OleDb;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;
using AjaxControlToolkit;
using CrystalDecisions;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using Microsoft.Office.Interop.Excel;
using System.Web.Security;
using System.Text;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class StudentsPage : BaseContentPage, IPostBackEventHandler
    {
        #region Fields
        private StudentBL studentBL;
        private bool isSearch;
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

            studentBL = new StudentBL(UserSchool);

            if (!Page.IsPostBack)
            {
                BindDDLYears();
                BindDDLFaculties();
                BindDDLGrades();
                BindDDLClasses();
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
                        System.Web.UI.WebControls.Label lblFullName = (System.Web.UI.WebControls.Label)e.Item.FindControl("LblFullName");
                        student.FullName = lblFullName.Text;
                        AddSession(AppConstant.SESSION_STUDENT, student);

                        Class_Class studentClass = new Class_Class();
                        // studentClass.ClassId = Int32.Parse(((HiddenField)e.Item.FindControl("HdfClassId")).Value);
                        AddSession(AppConstant.SESSION_STUDENTCLASS, Class);

                        Response.Redirect(AppConstant.PAGEPATH_STUDENT_INFOR);
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

            AddSession(AppConstant.SESSION_PAGEPATH, AppConstant.PAGEPATH_STUDENT_PRINT);
            AddSession(AppConstant.SESSION_SELECTED_YEAR, year);
            AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);
            AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);
            AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);
            AddSession(AppConstant.SESSION_SELECTED_STUDENTNAME, studentName);
            AddSession(AppConstant.SESSION_SELECTED_STUDENTCODE, studentCode);            
            //Response.Redirect(AppConstant.PAGEPATH_PRINTSTUDENTS);
            //Response.Write("<script language='javascript'>window.open('default.aspx','two','menubar=no,height=520,width=460,titlebar=yes,top=75,left=125 ,scrollbars=no,status=no,resizable=no');</script>");
            if (BtnPrint.Attributes.Count == 0)
            {
                BtnPrint.Attributes.Add("onclick", "window.showModalDialog('indanhsachhocsinh.aspx',null,'status:no;dialogWidth:1000px; dialogHeight:1200px;');");
                BtnPrint_Click(sender, e);
            }
            //Response.Write("<script language='javascript'>window.showModalDialog('indanhsachhocsinh.aspx', '', dialogWidth:300px; dialogHeight:200px; center:yes');</script>");
            #endregion
        }

        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            isSearch = true;
            BindRptStudents();
        }

        protected void BtnImport_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("importhocsinh.aspx");
        }

        protected void BtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("themhocsinh.aspx");
        }

        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            foreach (RepeaterItem item in RptHocSinh.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    System.Web.UI.WebControls.CheckBox CkbxSelect = (System.Web.UI.WebControls.CheckBox)item.FindControl("CkbxSelect");
                    if (CkbxSelect.Checked)
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
                        HiddenField HdfRptMaHocSinh = (HiddenField)item.FindControl("HdfRptMaHocSinh");
                        student.StudentId = Int32.Parse(HdfRptMaHocSinh.Value);
                        AddSession(AppConstant.SESSION_STUDENT, student);

                        // Get seleteced class and set to session
                        Class_Class studentClass = new Class_Class();
                        // studentClass.ClassId = Int32.Parse(((HiddenField)item.FindControl("HdfClassId")).Value);
                        AddSession(AppConstant.SESSION_STUDENTCLASS, Class);

                        AddSession(AppConstant.SESSION_PREV_PAGE, Request.Path);

                        // redirect to "Sửa học sinh"
                        Response.Redirect(AppConstant.PAGEPATH_STUDENT_EDIT);
                    }
                }
            }
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            HiddenField hdfRptMaHocSinh = null;
            AuthorizationBL authorizationBL = new AuthorizationBL(UserSchool);
            Student_Student student = null;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (RepeaterItem item in RptHocSinh.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    hdfRptMaHocSinh = (HiddenField)item.FindControl("HdfRptMaHocSinh");
                    stringBuilder.Clear();
                    stringBuilder.Append(AppConstant.USERPARENT_PREFIX);
                    stringBuilder.Append(AppConstant.UNDERSCORE);
                    stringBuilder.Append(((LinkButton)item.FindControl("LbtnStudentCode")).Text);

                    int iStudentId = Int32.Parse(hdfRptMaHocSinh.Value);
                    if (studentBL.IsDeletable(iStudentId))
                    {   
                        authorizationBL.DeleteAuthorization(stringBuilder.ToString());
                        Membership.DeleteUser(stringBuilder.ToString(), true);

                        student = new Student_Student();
                        student.StudentId = Int32.Parse(hdfRptMaHocSinh.Value);
                        studentBL.DeleteStudent(student);
                    }
                }
            }

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
        protected void PrePrint()
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

            AddSession(AppConstant.SESSION_PAGEPATH, AppConstant.PAGEPATH_STUDENT_PRINT);
            AddSession(AppConstant.SESSION_SELECTED_YEAR, year);
            AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);
            AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);
            AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);
            AddSession(AppConstant.SESSION_SELECTED_STUDENTNAME, studentName);
            AddSession(AppConstant.SESSION_SELECTED_STUDENTCODE, studentCode);
            //Response.Redirect(AppConstant.PAGEPATH_PRINTSTUDENTS);
            //Response.Write("<script language='javascript'>window.open('default.aspx','two','menubar=no,height=520,width=460,titlebar=yes,top=75,left=125 ,scrollbars=no,status=no,resizable=no');</script>");
            //if (BtnPrint.Attributes.Count == 0)
            //{
            //    BtnPrint.Attributes.Add("onclick", "window.showModalDialog('indanhsachhocsinh.aspx',null,'status:no;dialogWidth:1000px; dialogHeight:1200px;');");
            //    BtnPrint_Click(sender, e);
            //}
            //Response.Write("<script language='javascript'>window.showModalDialog('indanhsachhocsinh.aspx', '', dialogWidth:300px; dialogHeight:200px; center:yes');</script>");
            #endregion
        }
        
        private void ProcPermissions()
        {
            BtnAdd.Visible = accessibilities.Contains(AccessibilityEnum.Add);
            BtnImport.Visible = accessibilities.Contains(AccessibilityEnum.Add);
            BtnEdit.Visible = accessibilities.Contains(AccessibilityEnum.Modify);
            BtnDelete.Visible = accessibilities.Contains(AccessibilityEnum.Delete);
            PnlPopupConfirmDelete.Visible = accessibilities.Contains(AccessibilityEnum.Delete);
        }

        /// <summary>
        /// Bind data to repeater
        /// </summary>
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


            if((DdlLopHoc.Items.Count == 1) || (DdlLopHoc.Items.Count > 1 && DdlLopHoc.SelectedIndex > 0))
            {
                Class = new Class_Class();
                Class.ClassId = Int32.Parse(DdlLopHoc.SelectedValue);
            }

            tabularStudents = studentBL.GetTabularStudents(LogedInUser, IsFormerTeacher, year, faculty, grade, Class, studentCode, studentName,
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

                BtnPrint.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_EXPORT_DISABLED;
                BtnPrint.Enabled = false;
            }
            else
            {
                MainDataPager.Visible = true;

                BtnPrint.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_EXPORT;
                BtnPrint.Enabled = true;
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
                SystemConfigBL cauHinhBL = new SystemConfigBL(UserSchool);
                DdlNamHoc.SelectedValue = cauHinhBL.GetLastedYear().ToString();
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
                BtnSearch.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SEARCH_DISABLE;
                BtnSearch.Enabled = false;
                BtnAdd.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_ADD_DISABLE;
                BtnAdd.Enabled = false;
                BtnImport.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_IMPORT_DISABLE;
                BtnImport.Enabled = false;

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

            classes = classBL.GetClasses(LogedInUser, IsFormerTeacher, IsSubjectTeacher, year, faculty, grade, null);
            DdlLopHoc.DataSource = classes;
            DdlLopHoc.DataValueField = "ClassId";
            DdlLopHoc.DataTextField = "ClassName";
            DdlLopHoc.DataBind();

            if (classes.Count > 1)
            {
                DdlLopHoc.Items.Insert(0, new ListItem("Tất cả", "0"));

                BtnAdd.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_ADD;
                BtnAdd.Enabled = true;
                BtnImport.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_IMPORT;
                BtnImport.Enabled = true;
            }
            else
            {
                BtnAdd.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_ADD_DISABLE;
                BtnAdd.Enabled = false;
                BtnImport.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_IMPORT_DISABLE;
                BtnImport.Enabled = false;
            }
        }        

        public void RaisePostBackEvent(string eventArgument)
        {
            //throw new NotImplementedException();
            PrePrint();
        }
        #endregion
    }
}