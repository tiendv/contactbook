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
        LopHocBL lopHocBL;
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

            lopHocBL = new LopHocBL();
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
                        LopHocInfo lopHoc = (LopHocInfo)e.Item.DataItem;
                        if (lopHoc != null)
                        {
                            int maLopHoc = lopHoc.MaLopHoc;
                            if (!lopHocBL.CanDeleteLopHoc(maLopHoc))
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
            Guid role = (new UserBL()).GetRoleId(User.Identity.Name);
            if ((new RoleBL()).ValidateAuthorization(role, pageUrl))
            {
                if (e.Item.ItemType == ListItemType.Item
                    || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    LopHocInfo classInfo = (LopHocInfo)e.Item.DataItem;
                    if (classInfo != null)
                    {
                        int homeroomTecherCode = classInfo.HomeroomTeacherCode;
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
                        LopHoc_Lop lophoc = lopHocBL.GetLopHoc(maLopHoc);
                        TxtTenLopHocSua.Text = lophoc.TenLopHoc;
                        LblNganhHocSua.Text = (new FacultyBL()).GetNganhHoc(lophoc.MaNganhHoc).TenNganhHoc;
                        LblKhoiLopSua.Text = (new KhoiLopBL()).GetKhoiLop(lophoc.MaKhoiLop).TenKhoiLop;
                        LblNamHocSua.Text = (new NamHocBL()).GetNamHoc(lophoc.MaNamHoc).TenNamHoc;
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
            if (!Page.IsValid)
            {
                return;
            }

            int maNamHoc = Int32.Parse(DdlNamHocThem.SelectedValue);
            string tenLopHoc = this.TxtTenLopHocThem.Text.Trim();

            if (tenLopHoc == "")
            {
                TenLopHocRequiredAdd.IsValid = false;
                MPEAdd.Show();
                return;
            }
            else
            {
                if (lopHocBL.LopHocExists(tenLopHoc, maNamHoc))
                {
                    TenLopHocValidatorAdd.IsValid = false;
                    MPEAdd.Show();
                    return;
                }
            }

            int maNganhHoc = Int32.Parse(DdlNganhHocThem.SelectedValue);
            int maKhoiLop = Int32.Parse(DdlKhoiLopThem.SelectedValue);

            lopHocBL.InsertLopHoc(tenLopHoc, maNganhHoc, maKhoiLop, maNamHoc);

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
            if (!Page.IsValid)
            {
                return;
            }

            int maLopHoc = Int32.Parse(this.HdfMaLopHoc.Value);
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
                if (lopHocBL.LopHocExists(maLopHoc, tenLopHoc))
                {
                    TenLopHocValidatorEdit.IsValid = false;
                    modalPopupEdit.Show();
                    return;
                }
            }

            lopHocBL.UpdateLopHoc(maLopHoc, tenLopHoc);
            BindRepeaterLopHoc();
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            int maLopHoc = Int32.Parse(this.HdfMaLopHoc.Value);
            lopHocBL.DeleteLopHoc(maLopHoc);
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
            int maLopHoc = 0;
            try
            {
                maLopHoc = Int32.Parse(DdlLopHoc.SelectedValue);
            }
            catch (Exception) { }

            List<LopHocInfo> lstLopHocInfo;
            if (maLopHoc == 0) // "Tất cả"
            {
                int maNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);

                int maNganhHoc = 0;
                try
                {
                    maNganhHoc = Int32.Parse(DdlNganh.SelectedValue);
                }
                catch (Exception) { }

                int maKhoiLop = 0;
                try
                {
                    maKhoiLop = Int32.Parse(DdlKhoiLop.SelectedValue);
                }
                catch (Exception) { }
                double totalRecords;
                lstLopHocInfo = lopHocBL.GetListLopHocInfo(maNganhHoc, maKhoiLop, maNamHoc,
                    MainDataPager.CurrentIndex, MainDataPager.PageSize, out totalRecords);
                MainDataPager.ItemCount = totalRecords;
            }
            else
            {
                lstLopHocInfo = new List<LopHocInfo> { lopHocBL.GetLopHocInfo(maLopHoc) };
                MainDataPager.ItemCount = 1;
            }

            // Decrease page current index when delete
            if (lstLopHocInfo.Count == 0 && MainDataPager.ItemCount != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRepeaterLopHoc();
                return;
            }

            bool bDisplayData = (lstLopHocInfo.Count != 0) ? true : false;
            ProcessDislayInfo(bDisplayData);
            RptLopHoc.DataSource = lstLopHocInfo;
            RptLopHoc.DataBind();
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
            KhoiLopBL KhoiLopBL = new KhoiLopBL();
            List<DanhMuc_KhoiLop> lstKhoiLop = KhoiLopBL.GetListKhoiLop();
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
            FacultyBL nganhHocBL = new FacultyBL();
            List<DanhMuc_NganhHoc> lstNganhHoc = nganhHocBL.GetListNganhHoc();
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
            NamHocBL namHocBL = new NamHocBL();
            List<CauHinh_NamHoc> lstNamHoc = namHocBL.GetListNamHoc();
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

            int maNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);

            int maNganhHoc = 0;
            try
            {
                maNganhHoc = Int32.Parse(DdlNganh.SelectedValue);
            }
            catch (Exception) { }

            int maKhoiLop = 0;
            try
            {
                maKhoiLop = Int32.Parse(DdlKhoiLop.SelectedValue);
            }
            catch (Exception) { }

            List<LopHoc_Lop> lstLop = lopHocBL.GetListLopHoc(maNganhHoc, maKhoiLop, maNamHoc);
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