using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;
using AjaxControlToolkit;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class DanhSachLop : BaseContentPage
    {
        #region Fields
        ClassBL lopHocBL;
        private bool isSearch;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (isAccessDenied)
            {
                return;
            }

            lopHocBL = new ClassBL(UserSchool);
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
            BindDropDownListLopHoc();
        }

        protected void DdlNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropDownListLopHoc();
        }

        protected void DdlKhoiLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropDownListLopHoc();
        }
        #endregion

        #region Repeater event handlers
        protected void RptLopHoc_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (lstAccessibilities.Contains(AccessibilityEnum.Modify))
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

            if (lstAccessibilities.Contains(AccessibilityEnum.Delete))
            {
                if (e.Item.ItemType == ListItemType.Item
                    || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (e.Item.DataItem != null)
                    {
                        TabularClass lopHoc = (TabularClass)e.Item.DataItem;
                        if (lopHoc != null)
                        {
                            LopHoc_Lop Class = new LopHoc_Lop();
                            Class.MaLopHoc = lopHoc.MaLopHoc;
                            if (!lopHocBL.IsDeletable(Class))
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
            Guid role = (new UserBL(UserSchool)).GetRoleId(User.Identity.Name);
            if ((new AuthorizationBL(UserSchool)).ValidateAuthorization(role, pageUrl))
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

                        // Save current MaLopHoc to global
                        HiddenField hdfRptMaLopHoc = (HiddenField)e.Item.FindControl("HdfRptMaLopHoc");
                        this.HdfMaLopHoc.Value = hdfRptMaLopHoc.Value;

                        // Save modal popup ClientID
                        this.HdfRptLopHocMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        int maLopHoc = Int32.Parse(e.CommandArgument.ToString());
                        LopHoc_Lop lophoc = lopHocBL.GetClass(maLopHoc);
                        this.HdfSltClassName.Value = lophoc.TenLopHoc;
                        TxtTenLopHocSua.Text = lophoc.TenLopHoc;
                        LblNganhHocSua.Text = lophoc.DanhMuc_NganhHoc.TenNganhHoc;
                        LblKhoiLopSua.Text = lophoc.DanhMuc_KhoiLop.TenKhoiLop;
                        LblNamHocSua.Text = lophoc.CauHinh_NamHoc.TenNamHoc;
                        ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        mPEEdit.Show();

                        this.HdfMaLopHoc.Value = maLopHoc.ToString();
                        this.HdfRptLopHocMPEEdit.Value = mPEEdit.ClientID;

                        break;
                    }
                case "CmdDetailItem":
                    {
                        int maLopHoc = Int32.Parse(e.CommandArgument.ToString());
                        Response.Redirect("/modules/lop_Hoc/chitietlophoc.aspx?malop=" + maLopHoc);
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

        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {
            DanhMuc_NganhHoc faculty = null;
            DanhMuc_KhoiLop grade = null;
            CauHinh_NamHoc year = null;

            if (!Page.IsValid)
            {
                return;
            }

            year = new CauHinh_NamHoc();
            year.MaNamHoc = Int32.Parse(DdlNamHocThem.SelectedValue);
            string tenLopHoc = this.TxtTenLopHocThem.Text.Trim();            

            if (tenLopHoc == "")
            {
                TenLopHocRequiredAdd.IsValid = false;
                MPEAdd.Show();
                return;
            }
            else
            {
                if (lopHocBL.ClassNameExists(tenLopHoc, year))
                {
                    TenLopHocValidatorAdd.IsValid = false;
                    MPEAdd.Show();
                    return;
                }
            }

            faculty = new DanhMuc_NganhHoc();
            faculty.MaNganhHoc = Int32.Parse(DdlNganhHocThem.SelectedValue);
            grade = new DanhMuc_KhoiLop();
            grade.MaKhoiLop = Int32.Parse(DdlKhoiLopThem.SelectedValue);

            lopHocBL.InsertClass(tenLopHoc, year, faculty, grade);

            BindDropDownListLopHoc();
            MainDataPager.CurrentIndex = 1;
            BindRepeaterLopHoc();

            this.TxtTenLopHocThem.Text = "";
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
            LopHoc_Lop editedClass = null;
            CauHinh_NamHoc year = null;

            if (!Page.IsValid)
            {
                return;
            }

            int maLopHoc = Int32.Parse(this.HdfMaLopHoc.Value);
            string oldClassName = this.HdfSltClassName.Value;
            string tenLopHoc = this.TxtTenLopHocSua.Text.Trim();

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

            if (tenLopHoc == "")
            {
                TenLopHocRequiredEdit.IsValid = false;
                modalPopupEdit.Show();
                return;
            }
            else
            {
                year = (new SystemConfigBL(UserSchool)).GetCurrentYear();
                if (lopHocBL.ClassNameExists(oldClassName, tenLopHoc, year))
                {
                    TenLopHocValidatorEdit.IsValid = false;
                    modalPopupEdit.Show();
                    return;
                }
            }

            editedClass = new LopHoc_Lop();
            editedClass.MaLopHoc = maLopHoc;
            lopHocBL.UpdateClass(editedClass, tenLopHoc);
            BindRepeaterLopHoc();
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            LopHoc_Lop Class = new LopHoc_Lop();
            Class.MaLopHoc = Int32.Parse(this.HdfMaLopHoc.Value);
            lopHocBL.DeleteClass(Class);
            isSearch = false;
            BindDropDownListLopHoc();
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
            if (lstAccessibilities.Contains(AccessibilityEnum.Add))
            {
                BtnAdd.Enabled = true;
                BtnAdd.ImageUrl = "~/Styles/Images/button_add_with_text.png";
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
            DanhMuc_NganhHoc faculty = null;
            DanhMuc_KhoiLop grade = null;
            CauHinh_NamHoc year = null;
            LopHoc_Lop Class = null;
            
            try
            {
                if (DdlLopHoc.SelectedIndex > 0)
                {
                    Class = new LopHoc_Lop();
                    Class.MaLopHoc = Int32.Parse(DdlLopHoc.SelectedValue);                         
                }
            }
            catch (Exception) { }
            
            if (Class == null) // "Tất cả"
            {
                year = new CauHinh_NamHoc();
                year.MaNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);

                try
                {
                    if (DdlNganh.SelectedIndex > 0)
                    {
                        faculty = new DanhMuc_NganhHoc();
                        faculty.MaNganhHoc = Int32.Parse(DdlNganh.SelectedValue);
                    }
                }
                catch (Exception) { }
                
                try
                {
                    if (DdlKhoiLop.SelectedIndex > 0)
                    {
                        grade = new DanhMuc_KhoiLop();
                        grade.MaKhoiLop = Int32.Parse(DdlKhoiLop.SelectedValue);
                    }
                }
                catch (Exception) { }
                
                tabularClasses = lopHocBL.GetTabularClasses(year, faculty, grade,
                    MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);
                MainDataPager.ItemCount = dTotalRecords;
            }
            else
            {
                tabularClasses = new List<TabularClass> { lopHocBL.GetTabularClass(Class) };
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
            BindDropDownListNamHoc();

            BindDropDownListNganhHoc();

            BindDropDownListKhoiLop();

            BindDropDownListLopHoc();
        }

        private void BindDropDownListKhoiLop()
        {
            GradeBL KhoiLopBL = new GradeBL(UserSchool);
            List<DanhMuc_KhoiLop> lstKhoiLop = KhoiLopBL.GetListGrades();
            DdlKhoiLop.DataSource = lstKhoiLop;
            DdlKhoiLop.DataValueField = "MaKhoiLop";
            DdlKhoiLop.DataTextField = "TenKhoiLop";
            DdlKhoiLop.DataBind();
            if (lstKhoiLop.Count > 1)
            {
                DdlKhoiLop.Items.Insert(0, new ListItem("Tất cả", "0"));
            }

            DdlKhoiLopThem.DataSource = lstKhoiLop;
            DdlKhoiLopThem.DataValueField = "MaKhoiLop";
            DdlKhoiLopThem.DataTextField = "TenKhoiLop";
            DdlKhoiLopThem.DataBind();
        }

        private void BindDropDownListNganhHoc()
        {
            FacultyBL nganhHocBL = new FacultyBL(UserSchool);
            List<DanhMuc_NganhHoc> lstNganhHoc = nganhHocBL.GetFaculties();
            DdlNganh.DataSource = lstNganhHoc;
            DdlNganh.DataValueField = "MaNganhHoc";
            DdlNganh.DataTextField = "TenNganhHoc";
            DdlNganh.DataBind();
            if (lstNganhHoc.Count > 1)
            {
                DdlNganh.Items.Insert(0, new ListItem("Tất cả", "0"));
            }

            DdlNganhHocThem.DataSource = lstNganhHoc;
            DdlNganhHocThem.DataValueField = "MaNganhHoc";
            DdlNganhHocThem.DataTextField = "TenNganhHoc";
            DdlNganhHocThem.DataBind();
        }

        private void BindDropDownListNamHoc()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<CauHinh_NamHoc> lstNamHoc = systemConfigBL.GetListYears();
            DdlNamHoc.DataSource = lstNamHoc;
            DdlNamHoc.DataValueField = "MaNamHoc";
            DdlNamHoc.DataTextField = "TenNamHoc";
            DdlNamHoc.DataBind();
            if (DdlNamHoc.Items.Count != 0)
            {

            }

            DdlNamHocThem.DataSource = lstNamHoc;
            DdlNamHocThem.DataValueField = "MaNamHoc";
            DdlNamHocThem.DataTextField = "TenNamHoc";
            DdlNamHocThem.DataBind();
            if (DdlNamHocThem.Items.Count != 0)
            {

            }
        }

        private void BindDropDownListLopHoc()
        {
            CauHinh_NamHoc year = null;
            DanhMuc_NganhHoc faculty = null;
            DanhMuc_KhoiLop grade = null;

            if (DdlNamHoc.Items.Count == 0 || DdlNganh.Items.Count == 0 || DdlKhoiLop.Items.Count == 0)
            {
                BtnSearch.ImageUrl = "~/Styles/Images/button_search_with_text_disable.png";
                BtnSearch.Enabled = false;

                BtnAdd.ImageUrl = "~/Styles/Images/button_add_with_text_disable.png";
                BtnAdd.Enabled = false;

                PnlPopupConfirmDelete.Visible = false;
                PnlPopupEdit.Visible = false;
                RptLopHoc.Visible = false;
                LblSearchResult.Visible = true;
                LblSearchResult.Text = "Chưa có thông tin lớp học";

                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;

                return;
            }

            year = new CauHinh_NamHoc();
            year.MaNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);

            try
            {
                if (DdlNganh.SelectedIndex > 0)
                {
                    faculty = new DanhMuc_NganhHoc();
                    faculty.MaNganhHoc = Int32.Parse(DdlNganh.SelectedValue);
                }
            }
            catch (Exception) { }
            
            try
            {
                if (DdlKhoiLop.SelectedIndex > 0)
                {
                    grade = new DanhMuc_KhoiLop();
                    grade.MaKhoiLop = Int32.Parse(DdlKhoiLop.SelectedValue);
                }
            }
            catch (Exception) { }

            List<LopHoc_Lop> lstLop = lopHocBL.GetListClasses(year, faculty, grade);
            DdlLopHoc.DataSource = lstLop;
            DdlLopHoc.DataValueField = "MaLopHoc";
            DdlLopHoc.DataTextField = "TenLopHoc";
            DdlLopHoc.DataBind();

            if (lstLop.Count > 1)
            {
                DdlLopHoc.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }

        public void ShowAddPopup()
        {
            MPEAdd.Show();
        }
        #endregion
    }
}