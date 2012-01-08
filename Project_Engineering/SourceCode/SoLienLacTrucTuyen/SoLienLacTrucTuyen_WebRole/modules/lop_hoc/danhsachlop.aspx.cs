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
    public partial class ClassesPage : BaseContentPage, IPostBackEventHandler
    {
        #region Fields
        ClassBL classBL;
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

            classBL = new ClassBL(UserSchool);
            if (!Page.IsPostBack)
            {
                BindDropDownLists();
                isSearch = false;

                if (DdlLopHoc.Items.Count != 0)
                {
                    BindRepeaterLopHoc();
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

        #region Repeater event handlers
        protected void RptLopHoc_ItemDataBound(object sender, RepeaterItemEventArgs e)
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

                PnlPopupEdit.Visible = false;
            }

            if (accessibilities.Contains(AccessibilityEnum.Delete))
            {
                if (e.Item.ItemType == ListItemType.Item
                    || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (e.Item.DataItem != null)
                    {
                        TabularClass lopHoc = (TabularClass)e.Item.DataItem;
                        if (lopHoc != null)
                        {
                            Class_Class Class = new Class_Class();
                            Class.ClassId = lopHoc.ClassId;
                            if (!classBL.IsDeletable(Class))
                            {
                                ImageButton btnDeleteItem = (ImageButton)e.Item.FindControl("BtnDeleteItem");
                                btnDeleteItem.ImageUrl = "~/Styles/Images/button_delete_disable.png";
                                btnDeleteItem.Enabled = false;
                            }
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

            // Set NavigateUrl for Hyperlink HomeroomTeacher
            string pageUrl = Page.Request.Path;
            List<aspnet_Role> roles = (new UserBL(UserSchool)).GetRoles(User.Identity.Name);
            if ((new AuthorizationBL(UserSchool)).ValidateAuthorization(roles, pageUrl))
            {
                if (e.Item.ItemType == ListItemType.Item
                    || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    TabularClass classInfo = (TabularClass)e.Item.DataItem;
                    if (classInfo != null)
                    {
                        Guid homeroomTecherCode = classInfo.HomeroomTeacherCode;
                        HyperLink hlkHomeRoomTeacher = (HyperLink)e.Item.FindControl("HlkHomeRoomTeacher");
                        hlkHomeRoomTeacher.NavigateUrl = string.Format("~/modules/danh_muc/giao_vien/chitietgiaovien.aspx?giaovien={0}",
                            homeroomTecherCode);
                    }
                }
            }
        }

        protected void RptLopHoc_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDeleteItem":
                    {
                        // Set confirm text and show dialog
                        this.LblConfirmDelete.Text = "Bạn có chắc xóa lớp học <b>" + e.CommandArgument + "</b> này không?";
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        // Save current ClassId to global
                        HiddenField hdfRptClassId = (HiddenField)e.Item.FindControl("HdfRptClassId");
                        this.HdfClassId.Value = hdfRptClassId.Value;

                        // Save modal popup ClientID
                        this.HdfRptLopHocMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        int ClassId = Int32.Parse(e.CommandArgument.ToString());
                        Class_Class lophoc = classBL.GetClass(ClassId);
                        this.HdfSltClassName.Value = lophoc.ClassName;
                        TxtClassNameSua.Text = lophoc.ClassName;
                        LblNganhHocSua.Text = lophoc.Category_Faculty.FacultyName;
                        LblKhoiLopSua.Text = lophoc.Category_Grade.GradeName;
                        LblNamHocSua.Text = lophoc.Configuration_Year.YearName;
                        ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        mPEEdit.Show();

                        this.HdfClassId.Value = ClassId.ToString();
                        this.HdfRptLopHocMPEEdit.Value = mPEEdit.ClientID;

                        break;
                    }
                case "CmdDetailItem":
                    {
                        int ClassId = Int32.Parse(e.CommandArgument.ToString());
                        Response.Redirect("/modules/lop_Hoc/chitietlophoc.aspx?malop=" + ClassId);
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
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            isSearch = true;
            BindRepeaterLopHoc();
        }
        protected void BtnPrint_Click(object sender, ImageClickEventArgs e)
        {
            #region Add Info 2 Session

            Configuration_Year year = null;
            Category_Faculty faculty = null;
            Category_Grade grade = null;
            Class_Class Class = null;

            year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);
            year.YearName = DdlNamHoc.SelectedItem.Text;
            try
            {
                if (DdlNganh.SelectedIndex > 0)
                {
                    faculty = new Category_Faculty();
                    faculty.FacultyId = Int32.Parse(DdlNganh.SelectedValue);
                    faculty.FacultyName = DdlNganh.SelectedItem.Text; ;
                }
            }
            catch (Exception) { }

            try
            {
                if (DdlKhoiLop.SelectedIndex > 0)
                {
                    grade = new Category_Grade();
                    grade.GradeId = Int32.Parse(DdlKhoiLop.SelectedValue);
                    grade.GradeName = DdlKhoiLop.SelectedItem.Text; ;
                }
            }
            catch (Exception) { }

            AddSession(AppConstant.SESSION_PAGEPATH, AppConstant.PAGEPATH_PRINTCLASSES);
            AddSession(AppConstant.SESSION_SELECTED_YEAR, year);
            AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);
            AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);
            Response.Redirect(AppConstant.PAGEPATH_PRINTSTUDENTS);
            #endregion
        }
        protected void PrePrint()
        {
            #region Add Info 2 Session

            Configuration_Year year = null;
            Category_Faculty faculty = null;
            Category_Grade grade = null;
            Class_Class Class = null;

            year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);
            year.YearName = DdlNamHoc.SelectedItem.Text;
            try
            {
                if (DdlNganh.SelectedIndex > 0)
                {
                    faculty = new Category_Faculty();
                    faculty.FacultyId = Int32.Parse(DdlNganh.SelectedValue);
                    faculty.FacultyName = DdlNganh.SelectedItem.Text; ;
                }
            }
            catch (Exception) { }

            try
            {
                if (DdlKhoiLop.SelectedIndex > 0)
                {
                    grade = new Category_Grade();
                    grade.GradeId = Int32.Parse(DdlKhoiLop.SelectedValue);
                    grade.GradeName = DdlKhoiLop.SelectedItem.Text; ;
                }
            }
            catch (Exception) { }

            AddSession(AppConstant.SESSION_PAGEPATH, AppConstant.PAGEPATH_PRINTCLASSES);
            AddSession(AppConstant.SESSION_SELECTED_YEAR, year);
            AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);
            AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);
            //Response.Redirect(AppConstant.PAGEPATH_PRINTSTUDENTS);
            #endregion
        }
        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {
            Category_Faculty faculty = null;
            Category_Grade grade = null;
            Configuration_Year year = null;

            if (!Page.IsValid)
            {
                return;
            }

            year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlNamHocThem.SelectedValue);
            string ClassName = this.TxtClassNameThem.Text.Trim();            

            if (ClassName == "")
            {
                ClassNameRequiredAdd.IsValid = false;
                MPEAdd.Show();
                return;
            }
            else
            {
                if (classBL.ClassNameExists(ClassName, year))
                {
                    ClassNameValidatorAdd.IsValid = false;
                    MPEAdd.Show();
                    return;
                }
            }

            faculty = new Category_Faculty();
            faculty.FacultyId = Int32.Parse(DdlNganhHocThem.SelectedValue);
            grade = new Category_Grade();
            grade.GradeId = Int32.Parse(DdlKhoiLopThem.SelectedValue);

            classBL.InsertClass(ClassName, year, faculty, grade);

            BindDDLClasses();
            MainDataPager.CurrentIndex = 1;
            BindRepeaterLopHoc();

            this.TxtClassNameThem.Text = "";
            this.DdlNganhHocThem.SelectedIndex = 0;
            this.DdlKhoiLopThem.SelectedIndex = 0;
            this.DdlNamHocThem.SelectedIndex = 0;
            if (this.CkbAddAfterSave.Checked)
            {
                this.MPEAdd.Show();
            }
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            Class_Class editedClass = null;
            Configuration_Year year = null;

            if (!Page.IsValid)
            {
                return;
            }

            int ClassId = Int32.Parse(this.HdfClassId.Value);
            string oldClassName = this.HdfSltClassName.Value;
            string ClassName = this.TxtClassNameSua.Text.Trim();

            ModalPopupExtender modalPopupEdit = new ModalPopupExtender();
            foreach (RepeaterItem rptItem in RptLopHoc.Items)
            {
                if (rptItem.ItemType == ListItemType.Item
                    || rptItem.ItemType == ListItemType.AlternatingItem)
                {
                    modalPopupEdit = (ModalPopupExtender)rptItem.FindControl("MPEEdit");
                    if (modalPopupEdit.ClientID == HdfRptLopHocMPEEdit.Value)
                    {
                        break;
                    }
                }
            }

            if (ClassName == "")
            {
                ClassNameRequiredEdit.IsValid = false;
                modalPopupEdit.Show();
                return;
            }
            else
            {
                year = (new SystemConfigBL(UserSchool)).GetLastedYear();
                if (classBL.ClassNameExists(oldClassName, ClassName, year))
                {
                    ClassNameValidatorEdit.IsValid = false;
                    modalPopupEdit.Show();
                    return;
                }
            }

            editedClass = new Class_Class();
            editedClass.ClassId = ClassId;
            classBL.UpdateClass(editedClass, ClassName);
            BindRepeaterLopHoc();
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            Class_Class Class = new Class_Class();
            Class.ClassId = Int32.Parse(this.HdfClassId.Value);
            classBL.DeleteClass(Class);
            isSearch = false;
            BindDDLClasses();
            BindRepeaterLopHoc();
        }
        #endregion

        #region Pager event handlers
        public void pager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currentPageIndex;
            BindRepeaterLopHoc();
        }
        #endregion

        #region Methods
        private void ProcPermissions()
        {
            if (accessibilities.Contains(AccessibilityEnum.Add))
            {
                BtnAdd.Visible = true;
                PnlPopupAdd.Visible = true;
                MPEAdd.Enabled = true;
            }
            else
            {
                BtnAdd.Visible = false;
                PnlPopupAdd.Visible = false;
                MPEAdd.Enabled = false;
            }
        }

        private void BindRepeaterLopHoc()
        {
            List<TabularClass> tabularClasses;
            double dTotalRecords;
            Category_Faculty faculty = null;
            Category_Grade grade = null;
            Configuration_Year year = null;
            Class_Class Class = null;
            
            try
            {
                if (DdlLopHoc.SelectedIndex > 0)
                {
                    Class = new Class_Class();
                    Class.ClassId = Int32.Parse(DdlLopHoc.SelectedValue);                         
                }
            }
            catch (Exception) { }
            
            if (Class == null) // "Tất cả"
            {
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
                
                tabularClasses = classBL.GetTabularClasses(year, faculty, grade,
                    MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);
                MainDataPager.ItemCount = dTotalRecords;
            }
            else
            {
                tabularClasses = new List<TabularClass> { classBL.GetTabularClass(Class) };
                dTotalRecords = 1;
            }

            // Decrease page current index when delete
            if (tabularClasses.Count == 0 && MainDataPager.ItemCount != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRepeaterLopHoc();
                return;
            }

            bool bDisplayData = (tabularClasses.Count != 0) ? true : false;
            ProcessDislayInfo(bDisplayData);
            RptLopHoc.DataSource = tabularClasses;
            RptLopHoc.DataBind();
            MainDataPager.ItemCount = dTotalRecords;
        }

        private void ProcessDislayInfo(bool bDisplayData)
        {
            PnlPopupConfirmDelete.Visible = bDisplayData;
            PnlPopupEdit.Visible = bDisplayData;
            RptLopHoc.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin lớp học";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy lớp học";
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
            BindDDLYears();

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

            DdlKhoiLopThem.DataSource = lstKhoiLop;
            DdlKhoiLopThem.DataValueField = "GradeId";
            DdlKhoiLopThem.DataTextField = "GradeName";
            DdlKhoiLopThem.DataBind();
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

            DdlNganhHocThem.DataSource = faculties;
            DdlNganhHocThem.DataValueField = "FacultyId";
            DdlNganhHocThem.DataTextField = "FacultyName";
            DdlNganhHocThem.DataBind();
        }

        private void BindDDLYears()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<Configuration_Year> years = systemConfigBL.GetListYears();
            DdlNamHoc.DataSource = years;
            DdlNamHoc.DataValueField = "YearId";
            DdlNamHoc.DataTextField = "YearName";
            DdlNamHoc.DataBind();
            
            DdlNamHocThem.DataSource = years;
            DdlNamHocThem.DataValueField = "YearId";
            DdlNamHocThem.DataTextField = "YearName";
            DdlNamHocThem.DataBind();

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

                BtnAdd.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_ADD_DISABLE;
                BtnAdd.Enabled = false;

                BtnPrint.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_EXPORT_DISABLED;
                BtnPrint.Enabled = false;

                PnlPopupConfirmDelete.Visible = false;
                PnlPopupEdit.Visible = false;
                RptLopHoc.Visible = false;
                LblSearchResult.Visible = true;
                LblSearchResult.Text = "Chưa có thông tin lớp học";

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

            List<Class_Class> lstLop = classBL.GetListClasses(year, faculty, grade);
            DdlLopHoc.DataSource = lstLop;
            DdlLopHoc.DataValueField = "ClassId";
            DdlLopHoc.DataTextField = "ClassName";
            DdlLopHoc.DataBind();

            if (lstLop.Count > 1)
            {
                DdlLopHoc.Items.Insert(0, new ListItem("Tất cả", "0"));
            }

            if (DdlLopHoc.Items.Count != 0)
            {
                BtnPrint.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_EXPORT;
                BtnPrint.Enabled = true;
            }
            else
            {
                BtnPrint.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_EXPORT_DISABLED;
                BtnPrint.Enabled = false;
            }
        }

        public void ShowAddPopup()
        {
            MPEAdd.Show();
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            PrePrint();
        }
        #endregion
    }
}