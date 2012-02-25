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
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class ChangeStudentGradeSeletePage : BaseContentPage
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
                if (BindDropDownLists())
                {
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
                else
                {
                    PnlLinkToCategory.Visible = true;
                    PnlData.Visible = false;
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
        protected void BtnNext_Click(object sender, ImageClickEventArgs e)
        {
            List<TabularStudent> tabularStudents = new List<TabularStudent>();
            TabularStudent tabularStudent = null;
            HiddenField hdfRptStudentId = null;
            HiddenField hdfRptClassName = null;
            LinkButton lbtnStudentCode = null;
            CheckBox ckbxSelect = null;
            Label lblFullName = null;
            foreach(RepeaterItem item in RptHocSinh.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    ckbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (ckbxSelect.Checked)
                    {
                        hdfRptStudentId = (HiddenField)item.FindControl("HdfRptStudentId");
                        lbtnStudentCode = (LinkButton)item.FindControl("LbtnStudentCode");
                        lblFullName = (Label)item.FindControl("LblFullName");
                        hdfRptClassName = (HiddenField)item.FindControl("hdfRptClassName");
                        tabularStudent = new TabularStudent();
                        tabularStudent.StudentId = Int32.Parse(hdfRptStudentId.Value);
                        tabularStudent.StudentCode = lbtnStudentCode.Text;
                        tabularStudent.FullName = lblFullName.Text;
                        tabularStudent.ClassName = ((Label)item.FindControl("LblClassName")).Text;
                        tabularStudents.Add(tabularStudent);
                    }
                }
            }

            AddSession(AppConstant.SESSION_CHANGEGRADE_STUDENTS, tabularStudents);
            Response.Redirect(AppConstant.PAGEPATH_STUDENT_CHANGEGRADE_SAVE);
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
            Configuration_Year year = new Configuration_Year();
            Category_Faculty faculty = null;
            Category_Grade grade = null;
            Class_Class Class = null;
            string studentName = this.TxtTenHocSinh.Text;
            string studentCode = this.TxtMaHocSinh.Text;

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
                if (DdlNganh.Items.Count > 1 && DdlKhoiLop.SelectedIndex > 0)
                {
                    grade = new Category_Grade();
                    grade.GradeId = Int32.Parse(DdlKhoiLop.SelectedValue);
                }

                if (DdlNganh.Items.Count > 1 && DdlKhoiLop.SelectedIndex > 0)
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

            tabularStudents = studentBL.GetUnChangeGradeStudents(year, faculty, grade, Class, studentCode, studentName,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

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

                BtnNext.Enabled = false;
                BtnNext.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_NEXT_DISABLED;
            }
            else
            {
                BtnNext.Enabled = true;
                BtnNext.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_NEXT;

                MainDataPager.Visible = true;
            }

        }

        private bool BindDropDownLists()
        {
            if (FillYear())
            {
                BindDDLFaculties();
                BindDDLGrades();
                BindDDLClasses();

                return true;
            }
            else
            {
                return false;
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

        private bool FillYear()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            Configuration_Year year = systemConfigBL.GetPreviousYear();
            if (year == null)
            {
                return false;
            }
            else
            {
                LblYear.Text = year.YearName;
                ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID] = year.YearId;
                return true;
            }
        }

        private void BindDDLClasses()
        {
            ClassBL classBL = new ClassBL(UserSchool);
            List<Class_Class> classes = null;
            Configuration_Year year = null;
            Category_Faculty faculty = null;
            Category_Grade grade = null;

            if (ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID] == null || DdlNganh.Items.Count == 0 || DdlKhoiLop.Items.Count == 0)
            {
                BtnSearch.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SEARCH_DISABLE;
                BtnSearch.Enabled = false;
                BtnNext.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_NEXT_DISABLED;
                BtnNext.Enabled = false;

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

            if (DdlLopHoc.Items.Count == 0)
            {
                BtnNext.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_NEXT_DISABLED;
                BtnNext.Enabled = false;
            }
            else
            {
                BtnNext.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_NEXT;
                BtnNext.Enabled = true;

                if (DdlLopHoc.Items.Count > 1)
                {
                    DdlLopHoc.Items.Insert(0, new ListItem("Tất cả", "0"));
                }
            }
        }       
        #endregion
    }
}