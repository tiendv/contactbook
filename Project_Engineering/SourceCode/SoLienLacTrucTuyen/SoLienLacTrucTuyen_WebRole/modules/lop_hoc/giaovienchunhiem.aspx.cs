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
    public partial class GVCN : System.Web.UI.Page
    {
        #region Fields
        private GiaoVienChuNhiemBL giaoVienChuNhiemBL;
        private LopHocBL lopHocBL;
        private bool isSearch;
        private List<AccessibilityEnum> lstAccessibilities;
        #endregion

        #region Page event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            RoleBL roleBL = new RoleBL();
            UserBL userBL = new UserBL();
            giaoVienChuNhiemBL = new GiaoVienChuNhiemBL();
            lopHocBL = new LopHocBL();

            string pageUrl = Page.Request.Path;
            Guid role = userBL.GetRoleId(User.Identity.Name);

            if (!roleBL.ValidateAuthorization(role, pageUrl))
            {
                Response.Redirect((string)GetGlobalResourceObject("MainResource", "AccessDeniedPageUrl"));
                return;
            }

            Site masterPage = (Site)Page.Master;
            masterPage.UserRole = role;
            masterPage.PageUrl = pageUrl;

            lstAccessibilities = roleBL.GetAccessibilities(role, pageUrl);

            if (!Page.IsPostBack)
            {
                BindDropDownLists();
                isSearch = false;

                if (DdlLopHoc.Items.Count != 0)
                {
                    BindRepeater();
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
                        TabularGiaoVienChuNhiem giaoVienChuNhiem = (TabularGiaoVienChuNhiem)e.Item.DataItem;
                        if (giaoVienChuNhiem != null)
                        {
                            int maGiaoVien = giaoVienChuNhiem.MaGiaoVien;
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
            BindRepeater();
        }

        protected void BtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("themgiaovienchunhiem.aspx");
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            int maGVCN = Int32.Parse(this.HdfMaGVCN.Value);
            giaoVienChuNhiemBL.Delete(maGVCN);
            isSearch = false;
            BindDropDownListLopHoc();
            BindRepeater();
        }
        #endregion

        #region Pager event handlers
        public void pager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currentPageIndex;
            BindRepeater();
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

        private void BindRepeater()
        {
            int maNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);
            int maNganhHoc = Int32.Parse(DdlNganh.SelectedValue);
            int maKhoiLop = Int32.Parse(DdlKhoiLop.SelectedValue);
            int maLopHoc = Int32.Parse(DdlLopHoc.SelectedValue);
            string tenGVCN = TxtTenGVCN.Text.Trim();
            string maGVCN = TxtMaGVCN.Text.Trim();
            double totalRecords = 0;

            List<TabularGiaoVienChuNhiem> lstTbGVCN = giaoVienChuNhiemBL.GetListTbGiaoVienChuNhiems(
                maNamHoc, maNganhHoc, maKhoiLop, maLopHoc, maGVCN, tenGVCN,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out totalRecords);

            // Decrease page current index when delete
            if (lstTbGVCN.Count == 0 && totalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRepeater();
                return;
            }

            bool bDisplayData = (lstTbGVCN.Count != 0) ? true : false;
            ProcessDislayInfo(bDisplayData);
            RptGVCN.DataSource = lstTbGVCN;
            RptGVCN.DataBind();
            MainDataPager.ItemCount = totalRecords;
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
            GradeBL KhoiLopBL = new GradeBL();
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
            FacultyBL nganhHocBL = new FacultyBL();
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
            NamHocBL namHocBL = new NamHocBL();
            List<CauHinh_NamHoc> lstNamHoc = namHocBL.GetListNamHoc();
            DdlNamHoc.DataSource = lstNamHoc;
            DdlNamHoc.DataValueField = "MaNamHoc";
            DdlNamHoc.DataTextField = "TenNamHoc";
            DdlNamHoc.DataBind();
        }

        private void BindDropDownListLopHoc()
        {
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

            int maNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);
            int maNganhHoc = Int32.Parse(DdlNganh.SelectedValue);
            int maKhoiLop = Int32.Parse(DdlKhoiLop.SelectedValue);

            List<LopHoc_Lop> lstLop = lopHocBL.GetListLopHoc(maNganhHoc, maKhoiLop, maNamHoc);
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