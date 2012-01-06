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

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class ChangeStudentGradePage : BaseContentPage
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
                BindDropDownLists();                
                this.isSearch = false;                

                if (DdlLopHoc.Items.Count != 0)
                {
                    BindRptStudents();
                }
                else
                {
                    ProcessDislayInfo(false);
                }
            }
        }
        #endregion

        #region DropDownList event hanlders
        protected void DdlNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLClasses();
        }

        protected void DdlKhoiLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLClasses();
        }
        #endregion        

        #region Button event handlers
        protected void BtnPrint_Click(object sender, ImageClickEventArgs e)
        {
            
        }

        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            isSearch = true;
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
            year.YearId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_YEAR];

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
            FillYear();
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

        private void FillYear()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            Configuration_Year year = systemConfigBL.GetPreviousYear();
            LblYear.Text = year.YearName;
            ViewState[AppConstant.VIEWSTATE_SELECTED_YEAR] = year.YearId;
        }

        private void BindDDLClasses()
        {
            ClassBL classBL = new ClassBL(UserSchool);
            List<Class_Class> classes = null;
            Configuration_Year year = null;
            Category_Faculty faculty = null;
            Category_Grade grade = null;

            if (ViewState[AppConstant.VIEWSTATE_SELECTED_YEAR] == null || DdlNganh.Items.Count == 0 || DdlKhoiLop.Items.Count == 0)
            {
                BtnSearch.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SEARCH_DISABLE;
                BtnSearch.Enabled = false;
                //BtnAdd.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_ADD_DISABLE;
                //BtnAdd.Enabled = false;

                RptHocSinh.Visible = false;
                LblSearchResult.Visible = true;
                LblSearchResult.Text = "Chưa có thông tin học sinh";

                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
                return;
            }

            year = new Configuration_Year();
            year.YearId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_YEAR];

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
        }       
        #endregion
    }
}