using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class FormerTeacherListPage : BaseContentPage
    {
        #region Fields
        private FormerTeacherBL formerTeacherBL;
        private ClassBL classBL;
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

            formerTeacherBL = new FormerTeacherBL(UserSchool);
            classBL = new ClassBL(UserSchool);

            if (!Page.IsPostBack)
            {
                BindDropDownLists();
                isSearch = false;

                if (DdlLopHoc.Items.Count != 0)
                {
                    BindRptFormerTeachers();
                }
                else
                {
                    ProcessDislayInfo(false);
                }
            }

            ProcPermissions();
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

        #region Button event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            isSearch = true;
            BindRptFormerTeachers();
        }

        protected void BtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("themgiaovienchunhiem.aspx");
        }

        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            HiddenField hdfRptFormerTeacherId = null;
            Class_FormerTeacher formerTeacher = null;
            foreach (RepeaterItem item in RptGVCN.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox CkbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (CkbxSelect.Checked)
                    {
                        formerTeacher = new Class_FormerTeacher();
                        
                        hdfRptFormerTeacherId = (HiddenField)item.FindControl("HdfRptMaGVCN");
                        formerTeacher.FormerTeacherId = Int32.Parse(hdfRptFormerTeacherId.Value);
                        AddSession(AppConstant.SESSION_SELECTED_FORMERTEACHER, formerTeacher);
                        Response.Redirect(AppConstant.PAGEPATH_FORMERTEACHER_MODIFY);
                    }
                }
            }
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            CheckBox ckbxSelect = null;
            HiddenField HdfRptMaGVCN = null;
            HiddenField HdfUserId = null;
            Class_FormerTeacher formerTeacher = null;
            aspnet_User teacher = null;

            foreach (RepeaterItem item in RptGVCN.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    ckbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (ckbxSelect.Checked)
                    {
                        HdfRptMaGVCN = (HiddenField)item.FindControl("HdfRptMaGVCN");
                        formerTeacher = new Class_FormerTeacher();
                        formerTeacher.FormerTeacherId = Int32.Parse(HdfRptMaGVCN.Value);

                        HdfUserId = (HiddenField)item.FindControl("HdfUserId");
                        teacher = new aspnet_User();
                        teacher.UserId = new Guid(HdfUserId.Value);
                        
                        formerTeacherBL.Delete(formerTeacher, teacher);
                    }
                }
            }

            isSearch = false;
            BindDDLClasses();
            BindRptFormerTeachers();
        }
        #endregion

        #region Pager event handlers
        public void pager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currentPageIndex;
            BindRptFormerTeachers();
        }
        #endregion

        #region Methods
        private void ProcPermissions()
        {
            BtnAdd.Visible = accessibilities.Contains(AccessibilityEnum.Add);
            BtnEdit.Visible = accessibilities.Contains(AccessibilityEnum.Modify);
            BtnDelete.Visible = accessibilities.Contains(AccessibilityEnum.Delete);
            PnlPopupConfirmDelete.Visible = accessibilities.Contains(AccessibilityEnum.Delete);
        }

        private void BindRptFormerTeachers()
        {
            // there is no class
            if (DdlLopHoc.Items.Count == 0)
            {
                ProcessDislayInfo(false);
                return;
            }
            
            Category_Faculty faculty = null;
            Category_Grade grade = null;
            Class_Class Class = null;

            Configuration_Year year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);
            
            if (DdlNganh.SelectedIndex > 0)
            {
                faculty = new Category_Faculty();
                faculty.FacultyId = Int32.Parse(DdlNganh.SelectedValue);
            }            

            if (DdlKhoiLop.SelectedIndex > 0)
            {
                grade = new Category_Grade();
                grade.GradeId = Int32.Parse(DdlKhoiLop.SelectedValue);
            }
            
            if (DdlLopHoc.SelectedIndex > 0)
            {
                Class = new Class_Class();
                Class.ClassId = Int32.Parse(DdlLopHoc.SelectedValue);
            }
            
            string strTeacherName = TxtTenGVCN.Text.Trim();
            string strTeacherCode = TxtMaGVCN.Text.Trim();
            double dTotalRecords = 0;

            List<TabularFormerTeacher> tabularFormerTeachers = formerTeacherBL.GetListFormerTeachers(
                year, faculty, grade, Class, strTeacherCode, strTeacherName,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            // Decrease page current index when delete
            if (tabularFormerTeachers.Count == 0 && dTotalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRptFormerTeachers();
                return;
            }

            bool bDisplayData = (tabularFormerTeachers.Count != 0) ? true : false;
            ProcessDislayInfo(bDisplayData);
            RptGVCN.DataSource = tabularFormerTeachers;
            RptGVCN.DataBind();
            MainDataPager.ItemCount = dTotalRecords;
        }

        private void ProcessDislayInfo(bool bDisplayData)
        {   
            RptGVCN.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin giáo viên chủ nhiệm";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy giáo viên chủ nhiệm";
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

        private void BindDropDownListNamHoc()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<Configuration_Year> years = systemConfigBL.GetListYears();
            DdlNamHoc.DataSource = years;
            DdlNamHoc.DataValueField = "YearId";
            DdlNamHoc.DataTextField = "YearName";
            DdlNamHoc.DataBind();
        }

        private void BindDDLClasses()
        {
            Configuration_Year year = null;
            Category_Faculty faculty = null;
            Category_Grade grade = null;

            if (DdlNamHoc.Items.Count == 0 || DdlNganh.Items.Count == 0 || DdlKhoiLop.Items.Count == 0)
            {
                BtnSearch.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SEARCH_DISABLE;
                BtnSearch.Enabled = false;

                BtnAdd.ImageUrl = "~/Styles/buttons/button_add_disable.png";
                BtnAdd.Enabled = false;

                PnlPopupConfirmDelete.Visible = false;
                RptGVCN.Visible = false;
                LblSearchResult.Visible = true;
                LblSearchResult.Text = "Chưa có thông tin giáo viên chủ nhiệm";

                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;

                return;
            }
            else
            {
                BtnSearch.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SEARCH;
                BtnSearch.Enabled = true;
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

            List<Class_Class> lstLop = classBL.GetClasses(LogedInUser, IsFormerTeacher, IsSubjectTeacher, year, faculty, grade, null);
            DdlLopHoc.DataSource = lstLop;
            DdlLopHoc.DataValueField = "ClassId";
            DdlLopHoc.DataTextField = "ClassName";
            DdlLopHoc.DataBind();

            if (DdlLopHoc.Items.Count == 0)
            {
                BtnSearch.ImageUrl = "~/Styles/buttons/button_search_disable.png";
                BtnSearch.Enabled = false;
            }
            else
            {
                BtnSearch.ImageUrl = "~/Styles/buttons/button_search.png";
                BtnSearch.Enabled = true;
            }

            if (lstLop.Count > 1)
            {
                DdlLopHoc.Items.Insert(0, new ListItem("Tất cả", "0"));
            }

            if (DdlLopHoc.Items.Count != 0)
            {
                BtnAdd.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_ADD;
                BtnAdd.Enabled = true;
            }
            else
            {
                BtnAdd.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_ADD_DISABLE;
                BtnAdd.Enabled = false;
            }
        }
        #endregion

        #region Repeater event handlers
        protected void RptFormerTeachers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                e.Item.FindControl("thSelectAll").Visible = (accessibilities.Contains(AccessibilityEnum.Modify) || accessibilities.Contains(AccessibilityEnum.Delete));
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.FindControl("tdSelect").Visible = (accessibilities.Contains(AccessibilityEnum.Modify) || accessibilities.Contains(AccessibilityEnum.Delete));
            }
        }
        #endregion
    }
}