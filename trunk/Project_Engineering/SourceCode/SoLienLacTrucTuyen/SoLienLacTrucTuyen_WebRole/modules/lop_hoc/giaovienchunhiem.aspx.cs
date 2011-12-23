using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;
using AjaxControlToolkit;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class FormerTeacherPage : BaseContentPage
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
                    BindRptTeachers();
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
        protected void RptGVCN_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
                    if (e.Item.DataItem != null)
                    {
                        TabularFormerTeacher giaoVienChuNhiem = (TabularFormerTeacher)e.Item.DataItem;
                        if (giaoVienChuNhiem != null)
                        {
                            Guid UserId = giaoVienChuNhiem.UserId;
                            HyperLink hlkTenGVCN = (HyperLink)e.Item.FindControl("HlkTenGVCN");
                            hlkTenGVCN.NavigateUrl = string.Format("/modules/danh_muc/giao_vien/chitietgiaovien.aspx?giaovien={0}", UserId);

                            int ClassId = giaoVienChuNhiem.ClassId;
                            HyperLink hlkClassName = (HyperLink)e.Item.FindControl("HlkClassName");
                            hlkClassName.NavigateUrl = string.Format("/modules/lop_hoc/chitietlophoc.aspx?malop={0}", ClassId);

                            //if (!GiaoVienChuNhiemBL.CheckCanDeleteGVCN(maGVCN))
                            //{
                            //    ImageButton btnDeleteItem = (ImageButton)e.Item.FindControl("BtnDeleteItem");
                            //    btnDeleteItem.ImageUrl = "~/Styles/Images/button_delete_disable.png";
                            //    btnDeleteItem.Enabled = false;
                            //}
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

        protected void RptGVCN_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDeleteItem":
                    {
                        // Set confirm text and show dialog
                        this.LblConfirmDelete.Text = "Bạn có chắc xóa GVCN <b>" + e.CommandArgument + "</b> này không?";
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        // Save current MaGVCN to global
                        HiddenField hdfRptMaGVCN = (HiddenField)e.Item.FindControl("HdfRptMaGVCN");
                        this.HdfMaGVCN.Value = hdfRptMaGVCN.Value;

                        // Save modal popup ClientID
                        this.HdfRptGVCNMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        int maGVCN = Int32.Parse(e.CommandArgument.ToString());
                        Response.Redirect(string.Format("suagiaovienchunhiem.aspx?id={0}", maGVCN));                        
                        break;
                    }
                case "CmdDetailItemLopHoc":
                    {
                        //int ClassId = Int32.Parse(e.CommandArgument.ToString());
                        //Response.Redirect(string.Format("chitietlophoc.aspx?malop={0}", ClassId));
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
            BindRptTeachers();
        }

        protected void BtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("themgiaovienchunhiem.aspx");
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            int maGVCN = Int32.Parse(this.HdfMaGVCN.Value);
            Class_FormerTeacher formerTeacher = new Class_FormerTeacher();
            formerTeacher.FormerTeacherId = maGVCN;
            formerTeacherBL.Delete(formerTeacher);
            isSearch = false;
            BindDDLClasses();
            BindRptTeachers();
        }
        #endregion

        #region Pager event handlers
        public void pager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currentPageIndex;
            BindRptTeachers();
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

        private void BindRptTeachers()
        {
            Configuration_Year year = null;
            Category_Faculty faculty = null;
            Category_Grade grade = null;
            Class_Class Class = null;

            int YearId = Int32.Parse(DdlNamHoc.SelectedValue);
            if (YearId != 0)
            {
                year = new Configuration_Year();
                year.YearId = YearId;
            }
            
            int FacultyId = Int32.Parse(DdlNganh.SelectedValue);
            if (FacultyId != 0)
            {
                faculty = new Category_Faculty();
                faculty.FacultyId = YearId;
            }
            
            int GradeId = Int32.Parse(DdlKhoiLop.SelectedValue);
            if (GradeId != 0)
            {
                grade = new Category_Grade();
                grade.GradeId = GradeId;
            }
            
            int ClassId = Int32.Parse(DdlLopHoc.SelectedValue);
            if (ClassId != 0)
            {
                Class = new Class_Class();
                Class.ClassId = ClassId;
            }
            
            string tenGVCN = TxtTenGVCN.Text.Trim();
            string maGVCN = TxtMaGVCN.Text.Trim();
            double dTotalRecords = 0;

            List<TabularFormerTeacher> lstTbGVCN = formerTeacherBL.GetListFormerTeachers(
                year, faculty, grade, Class, maGVCN, tenGVCN,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            // Decrease page current index when delete
            if (lstTbGVCN.Count == 0 && dTotalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRptTeachers();
                return;
            }

            bool bDisplayData = (lstTbGVCN.Count != 0) ? true : false;
            ProcessDislayInfo(bDisplayData);
            RptGVCN.DataSource = lstTbGVCN;
            RptGVCN.DataBind();
            MainDataPager.ItemCount = dTotalRecords;
        }

        private void ProcessDislayInfo(bool bDisplayData)
        {
            PnlPopupConfirmDelete.Visible = bDisplayData;
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
                BtnSearch.ImageUrl = "~/Styles/Images/button_search_with_text_disable.png";
                BtnSearch.Enabled = false;

                BtnAdd.ImageUrl = "~/Styles/Images/button_add_with_text_disable.png";
                BtnAdd.Enabled = false;

                PnlPopupConfirmDelete.Visible = false;
                RptGVCN.Visible = false;
                LblSearchResult.Visible = true;
                LblSearchResult.Text = "Chưa có thông tin giáo viên chủ nhiệm";

                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;

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

            List<Class_Class> lstLop = classBL.GetListClasses(year, faculty, grade);
            DdlLopHoc.DataSource = lstLop;
            DdlLopHoc.DataValueField = "ClassId";
            DdlLopHoc.DataTextField = "ClassName";
            DdlLopHoc.DataBind();

            if (DdlLopHoc.Items.Count == 0)
            {
                BtnSearch.ImageUrl = "~/Styles/Images/button_search_with_text_disable.png";
                BtnSearch.Enabled = false;
            }
            else
            {
                BtnSearch.ImageUrl = "~/Styles/Images/button_search_with_text.png";
                BtnSearch.Enabled = true;
            }

            if (lstLop.Count > 1)
            {
                DdlLopHoc.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }
        #endregion
    }
}