using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class FormerTeacherAddPage : BaseContentPage
    {
        #region Fields
        private TeacherBL teacherBL;
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

            teacherBL = new TeacherBL(UserSchool);

            if (!Page.IsPostBack)
            {
                BindDropDownLists();
                if (ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID] != null && DdlNganh.Items.Count != 0 && DdlKhoiLop.Items.Count != 0)
                {
                    BindRepeater();
                }
                else
                {
                    ProcDisplayInfo(false);
                }                
            }
        }
        #endregion

        #region Repeater event handlers
        protected void RptGiaoVien_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void RptGiaoVien_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                default:
                    {
                        break;
                    }
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            MainDataPager.ItemCount = 0;
            isSearch = true;
            BindRepeater();
        }

        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            foreach (RepeaterItem item in RptGiaoVien.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    Control control = item.FindControl("RBtnSelect");
                    if (control != null)
                    {
                        RadioButton rBtnSelect = (RadioButton)control;
                        if (rBtnSelect.Checked)
                        {
                            Class_Class Class = new Class_Class();
                            aspnet_User teacher = new aspnet_User();
                            FormerTeacherBL gvcnBL = new FormerTeacherBL(UserSchool);

                            HiddenField hdfRptUserId = (HiddenField)item.FindControl("HdfRptUserId");
                            Guid UserId = new Guid(hdfRptUserId.Value);
                            int ClassId = Int32.Parse(DdlLopHoc.SelectedValue);

                            Class.ClassId = ClassId;
                            teacher.UserId = UserId;
                            gvcnBL.Insert(Class, teacher);
                            Response.Redirect("giaovienchunhiem.aspx");
                        }
                    }
                }
            }
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("giaovienchunhiem.aspx");
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

        #region DataPager event handlers
        public void DataPager_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currnetPageIndx;
            BindRepeater();
        }
        #endregion

        #region Methods
        private void BindDropDownLists()
        {
            FillYear();
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

        private void FillYear()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            Configuration_Year year = systemConfigBL.GetLastedYear();
            if (year != null)
            {
                LblYear.Text = year.YearName;
                ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID] = year.YearId;

                BindRepeater();
                LblTitleTeacherList.Text = string.Format("DANH SÁCH GIÁO VIÊN CHƯA PHÂN CÔNG CHỦ NHIỆM (NĂM HỌC {0})", year.YearName);
            }
            else
            {
                ProcDisplayInfo(false);
            }
        }

        private void BindDDLClasses()
        {
            Configuration_Year year = null;
            Category_Faculty faculty = null;
            Category_Grade grade = null;

            bool bEnabled;
            if (ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID] == null || DdlNganh.Items.Count == 0 || DdlKhoiLop.Items.Count == 0)
            {
                bEnabled = false;
            }
            else
            {
                year = new Configuration_Year();
                year.YearId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID];

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

                List<Class_Class> lstLop = (new ClassBL(UserSchool)).GetUnformeredClasses(year, faculty, grade);
                DdlLopHoc.DataSource = lstLop;
                DdlLopHoc.DataValueField = "ClassId";
                DdlLopHoc.DataTextField = "ClassName";
                DdlLopHoc.DataBind();

                if (DdlLopHoc.Items.Count == 0)
                {
                    bEnabled = false;
                }
                else
                {
                    bEnabled = true;
                }
            }

            BtnSave.Enabled = bEnabled;
            BtnSave.ImageUrl = (bEnabled) ? "~/Styles/buttons/button_save.png" : "~/Styles/buttons/button_save_disable.png";
        }

        private void BindRepeater()
        {
            if (ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID] == null)
            {
                ProcDisplayInfo(false);
                return;
            }
            
            Configuration_Year year = new Configuration_Year();
            year.YearId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID];
            string strTeacherCode = TxtSearchMaHienThiGiaoVien.Text.Trim();
            string strTeacherName = TxtSearchTenGiaoVien.Text.Trim();

            double dTotalRecords;
            List<TabularTeacher> tabularTeachers = teacherBL.GetTabularUnformeredTeachers(year, strTeacherCode, strTeacherName,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            // Decrease page current index when delete
            if (tabularTeachers.Count == 0 && dTotalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRepeater();
                return;
            }

            bool bDisplayData = (tabularTeachers.Count != 0) ? true : false;
            ProcDisplayInfo(bDisplayData);

            RptGiaoVien.DataSource = tabularTeachers;
            RptGiaoVien.DataBind();
            MainDataPager.ItemCount = dTotalRecords;

            if (bDisplayData)
            {
                foreach (RepeaterItem item in RptGiaoVien.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        RadioButton rBtnSelect = (RadioButton)item.FindControl("RBtnSelect");
                        rBtnSelect.Checked = true;
                        return;
                    }
                }
            }
        }

        public void ProcDisplayInfo(bool bDisplay)
        {
            RptGiaoVien.Visible = bDisplay;
            LblSearchResult.Visible = !bDisplay;
            bool b = (DdlLopHoc.Items.Count != 0) && bDisplay;
            BtnSave.Enabled = b;
            BtnSave.ImageUrl = (b) ? "~/Styles/buttons/button_save.png" : "~/Styles/buttons/button_save_disable.png";
            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin giáo viên";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy giáo viên";
                }

                MainDataPager.CurrentIndex = 1;
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
            }
            else
            {
                MainDataPager.Visible = true;
            }
        }
        #endregion
    }
}