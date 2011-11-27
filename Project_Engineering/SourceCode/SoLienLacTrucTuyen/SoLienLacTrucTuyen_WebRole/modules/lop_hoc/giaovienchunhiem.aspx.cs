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
            if (isAccessDenied)
            {
                return;
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
        protected void RptGVCN_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
            }

            if (lstAccessibilities.Contains(AccessibilityEnum.Delete))
            {
                if (e.Item.ItemType == ListItemType.Item
                    || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (e.Item.DataItem != null)
                    {
                        TabularFormerTeacher giaoVienChuNhiem = (TabularFormerTeacher)e.Item.DataItem;
                        if (giaoVienChuNhiem != null)
                        {
                            Guid maGiaoVien = giaoVienChuNhiem.MaGiaoVien;
                            HyperLink hlkTenGVCN = (HyperLink)e.Item.FindControl("HlkTenGVCN");
                            hlkTenGVCN.NavigateUrl = string.Format("/modules/danh_muc/giao_vien/chitietgiaovien.aspx?giaovien={0}", maGiaoVien);

                            int maLopHoc = giaoVienChuNhiem.MaLopHoc;
                            HyperLink hlkTenLopHoc = (HyperLink)e.Item.FindControl("HlkTenLopHoc");
                            hlkTenLopHoc.NavigateUrl = string.Format("/modules/lop_hoc/chitietlophoc.aspx?malop={0}", maLopHoc);

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
                        //int maLopHoc = Int32.Parse(e.CommandArgument.ToString());
                        //Response.Redirect(string.Format("chitietlophoc.aspx?malop={0}", maLopHoc));
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
            LopHoc_GVCN formerTeacher = new LopHoc_GVCN();
            formerTeacher.MaGVCN = maGVCN;
            formerTeacherBL.Delete(formerTeacher);
            isSearch = false;
            BindDropDownListLopHoc();
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
            if (lstAccessibilities.Contains(AccessibilityEnum.Add))
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
            CauHinh_NamHoc year = null;
            DanhMuc_NganhHoc faculty = null;
            DanhMuc_KhoiLop grade = null;
            LopHoc_Lop Class = null;

            int maNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);
            if (maNamHoc != 0)
            {
                year = new CauHinh_NamHoc();
                year.MaNamHoc = maNamHoc;
            }
            
            int maNganhHoc = Int32.Parse(DdlNganh.SelectedValue);
            if (maNganhHoc != 0)
            {
                faculty = new DanhMuc_NganhHoc();
                faculty.MaNganhHoc = maNamHoc;
            }
            
            int maKhoiLop = Int32.Parse(DdlKhoiLop.SelectedValue);
            if (maKhoiLop != 0)
            {
                grade = new DanhMuc_KhoiLop();
                grade.MaKhoiLop = maKhoiLop;
            }
            
            int maLopHoc = Int32.Parse(DdlLopHoc.SelectedValue);
            if (maLopHoc != 0)
            {
                Class = new LopHoc_Lop();
                Class.MaLopHoc = maLopHoc;
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
        }

        private void BindDropDownListNamHoc()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<CauHinh_NamHoc> lstNamHoc = systemConfigBL.GetListYears();
            DdlNamHoc.DataSource = lstNamHoc;
            DdlNamHoc.DataValueField = "MaNamHoc";
            DdlNamHoc.DataTextField = "TenNamHoc";
            DdlNamHoc.DataBind();
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
                RptGVCN.Visible = false;
                LblSearchResult.Visible = true;
                LblSearchResult.Text = "Chưa có thông tin giáo viên chủ nhiệm";

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

            List<LopHoc_Lop> lstLop = classBL.GetListClasses(year, faculty, grade);
            DdlLopHoc.DataSource = lstLop;
            DdlLopHoc.DataValueField = "MaLopHoc";
            DdlLopHoc.DataTextField = "TenLopHoc";
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