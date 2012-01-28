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
    public partial class AddMessagePage : BaseContentPage
    {
        #region Fields
        private StudentBL studentBL;
        private bool isSearch;
        MessageBL messageBL;
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

            messageBL = new MessageBL(UserSchool);
            studentBL = new StudentBL(UserSchool);

            if (!Page.IsPostBack)
            {
                BindDropDownLists();
                this.isSearch = false;

                if (CheckUntils.IsNullOrBlank(LblYear.Text) || DdlKhoiLop.Items.Count == 0
                    || DdlNganh.Items.Count == 0)
                {
                    Response.Redirect(AppConstant.PAGEPATH_MESSAGE_LIST);
                }
                else
                {
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
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

            }
        }

        protected void RptHocSinh_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }
        #endregion

        #region Button event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            isSearch = true;
            BindRptStudents();

            RbtnSelectAll.Checked = true;
            RbtnSelectSome.Checked = false;
        }

        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (RbtnSelectAll.Checked)
            {
                Configuration_Year year = null;
                Category_Faculty faculty = null;
                Category_Grade grade = null;
                Class_Class Class = null;

                year = new Configuration_Year();
                year.YearId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID];

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

                List<Student_StudentInClass> studentInClasses = studentBL.GetStudentInClasses(year, faculty, grade, Class);
                foreach (Student_StudentInClass studentInClass in studentInClasses)
                {
                    messageBL.InsertMessage(studentInClass.StudentInClassId, TxtTitle.Text, TxtContent.Text, DateTime.Now);
                }
            }
            else
            {
                foreach (RepeaterItem item in RptHocSinh.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        System.Web.UI.WebControls.CheckBox CkbxSelectUser = (System.Web.UI.WebControls.CheckBox)item.FindControl("CkbxSelect");
                        if (CkbxSelectUser.Checked)
                        {
                            HiddenField HdfRptStudentInClassId = (HiddenField)item.FindControl("HdfRptStudentInClassId");
                            messageBL.InsertMessage(Int32.Parse(HdfRptStudentInClassId.Value), TxtTitle.Text, TxtContent.Text, DateTime.Now);
                        }
                    }
                }
            }

            BacktoPrevPage();
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            BacktoPrevPage();
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
            double dTotalRecords = GetStudents(ref tabularStudents);

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
            PnlStudentSelection.Visible = bDisplayData;
            BtnSaveEdit.Enabled = bDisplayData;
            if (bDisplayData)
            {
                BtnSaveEdit.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SAVE;
            }
            else
            {
                BtnSaveEdit.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SAVE_DISABLE;
            }
        }

        private double GetStudents(ref List<TabularStudent> tabularStudents)
        {
            double dTotalRecords;
            Configuration_Year year = null;
            Category_Faculty faculty = null;
            Category_Grade grade = null;
            Class_Class Class = null;
            string studentCode = "";

            year = new Configuration_Year();
            year.YearId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID];

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

            tabularStudents = studentBL.GetTabularStudents(LogedInUser, IsFormerTeacher, year, faculty, grade, Class, studentCode, "",
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);
            return dTotalRecords;
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
            Configuration_Year year = systemConfigBL.GetLastedYear();
            if (year != null)
            {
                LblYear.Text = year.YearName;
                ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID] = year.YearId;
            }
        }

        private void BindDDLClasses()
        {
            ClassBL classBL = new ClassBL(UserSchool);
            List<Class_Class> classes = null;
            Configuration_Year year = null;
            Category_Faculty faculty = null;
            Category_Grade grade = null;

            if (DdlNganh.Items.Count == 0 || DdlKhoiLop.Items.Count == 0)
            {
                BtnSearch.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SEARCH_DISABLE;
                BtnSearch.Enabled = false;

                RptHocSinh.Visible = false;
                LblSearchResult.Visible = true;
                LblSearchResult.Text = "Chưa có thông tin học sinh";

                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;

                return;
            }

            year = new Configuration_Year();
            year.YearId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID];

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
            }
        }

        private void BacktoPrevPage()
        {
            Response.Redirect(AppConstant.PAGEPATH_MESSAGE_LIST);
        }
        #endregion
    }
}