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
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class hoatdong : System.Web.UI.Page
    {
        #region Fields
        private HocSinhBL hocSinhBL;
        private BuoiBL buoiBL;
        private HoatDongBL hoatDongBL;
        private ThaiDoThamGiaBL thaiDoThamGiaBL;
        private bool isSearch;
        private List<AccessibilityEnum> lstAccessibilities;
        #endregion

        #region Page event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            RoleBL roleBL = new RoleBL();
            UserBL userBL = new UserBL();
            hocSinhBL = new HocSinhBL();
            buoiBL = new BuoiBL();
            hoatDongBL = new HoatDongBL();
            thaiDoThamGiaBL = new ThaiDoThamGiaBL();

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
                if (Request.QueryString["hocsinh"] != null)
                {
                    int maHocSinh = Int32.Parse(Request.QueryString["hocsinh"]);
                    ViewState["MaHocSinh"] = maHocSinh;
                    HdfMaHocSinh.Value = maHocSinh.ToString();
                    BindDropDownLists();
                    InitDates();
                    isSearch = false;
                    BindRepeater();
                    
                    HlkThongTinCaNhan.NavigateUrl = String.Format("thongtincanhan.aspx?hocsinh={0}", maHocSinh);
                    HlkKetQuaHocTap.NavigateUrl = String.Format("ketquahoctap.aspx?hocsinh={0}", maHocSinh);
                    HlkNgayNghiHoc.NavigateUrl = String.Format("ngaynghihoc.aspx?hocsinh={0}", maHocSinh);
                }
            }

            ProcPermissions();
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
            }
            else
            {
                BtnAdd.Visible = false;
                PnlPopupAdd.Visible = false;
            }
        }

        private void BindDropDownLists()
        {
            BindDropDownListNamHoc();
            BindDropDownListHocKy();
            BindDropDownListThaiDoThamGia();
        }

        private void BindDropDownListNamHoc()
        {
            if (ViewState["MaHocSinh"] != null)
            {
                NamHocBL namHocBL = new NamHocBL();
                List<CauHinh_NamHoc> lstNamHoc = hocSinhBL.GetListNamHoc((int)ViewState["MaHocSinh"]);
                DdlNamHoc.DataSource = lstNamHoc;
                DdlNamHoc.DataValueField = "MaNamHoc";
                DdlNamHoc.DataTextField = "TenNamHoc";
                DdlNamHoc.DataBind();
            }
        }

        private void BindDropDownListHocKy()
        {
            HocKyBL hocKyBL = new HocKyBL();
            List<CauHinh_HocKy> lstHocKy = hocKyBL.GetListHocKy();
            DdlHocKy.DataSource = lstHocKy;
            DdlHocKy.DataValueField = "MaHocKy";
            DdlHocKy.DataTextField = "TenHocKy";
            DdlHocKy.DataBind();
            DdlHocKy.SelectedValue = (new CauHinhHeThongBL()).GetMaHocKyHienHanh().ToString();

            DdlHocKyThem.DataSource = lstHocKy;
            DdlHocKyThem.DataValueField = "MaHocKy";
            DdlHocKyThem.DataTextField = "TenHocKy";
            DdlHocKyThem.DataBind();
            DdlHocKyThem.SelectedValue = (new CauHinhHeThongBL()).GetMaHocKyHienHanh().ToString();
        }

        private void BindDropDownListThaiDoThamGia()
        {
            List<DanhMuc_ThaiDoThamGia> lstThaiDoThamGia = thaiDoThamGiaBL.GetListThaiDoThamGia();
            DdlThaiDoThamGiaThem.DataSource = lstThaiDoThamGia;
            DdlThaiDoThamGiaThem.DataValueField = "MaThaiDoThamGia";
            DdlThaiDoThamGiaThem.DataTextField = "TenThaiDoThamGia";
            DdlThaiDoThamGiaThem.DataBind();
            DdlThaiDoThamGiaThem.Items.Insert(0, new ListItem("Chưa xác định", "0"));

            DdlThaiDoThamGiaSua.DataSource = lstThaiDoThamGia;
            DdlThaiDoThamGiaSua.DataValueField = "MaThaiDoThamGia";
            DdlThaiDoThamGiaSua.DataTextField = "TenThaiDoThamGia";
            DdlThaiDoThamGiaSua.DataBind();
            DdlThaiDoThamGiaSua.Items.Insert(0, new ListItem("Chưa xác định", "0"));
        }

        private void InitDates()
        {
            DateTime today = DateTime.Now;
            DateTime beginDateOfMonth = new DateTime(today.Year, today.Month, 1);
            TxtTuNgay.Text = beginDateOfMonth.ToShortDateString();
            DateTime dateOfNextMonth = today.AddMonths(1);
            DateTime beginDateOfNextMonth = new DateTime(dateOfNextMonth.Year, dateOfNextMonth.Month, 1);
            DateTime endDateOfMonth = beginDateOfNextMonth.AddDays(-1);
            TxtDenNgay.Text = endDateOfMonth.ToShortDateString();

            TxtNgayThem.Text = today.ToShortDateString();

            TxtNgaySua.Text = today.ToShortDateString();
        }

        private void BindRepeater()
        {
            int maHocSinh = (int)ViewState["MaHocSinh"];
            int maNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);
            int maHocKy = Int32.Parse(DdlHocKy.SelectedValue);
            DateTime tuNgay = DateTime.Parse(TxtTuNgay.Text);
            DateTime DenNgay = DateTime.Parse(TxtDenNgay.Text);

            double totalRecords;
            List<TabularHoatDong> lstTabularHoatDongs;
            lstTabularHoatDongs = hoatDongBL.GetListTabularHoatDong(maHocSinh, maNamHoc, maHocKy,
                tuNgay, DenNgay, MainDataPager.CurrentIndex, MainDataPager.PageSize,
                out totalRecords);

            if (totalRecords != 0 && lstTabularHoatDongs.Count == 0)
            {
                MainDataPager.CurrentIndex--;
                BindRepeater();
                return;
            }

            bool bDisplayData = (lstTabularHoatDongs.Count != 0) ? true : false;
            ProcessDislayInfo(bDisplayData);
            RptHoatDong.DataSource = lstTabularHoatDongs;
            RptHoatDong.DataBind();
            MainDataPager.ItemCount = totalRecords;
        }

        private void ProcessDislayInfo(bool bDisplayData)
        {
            PnlPopupConfirmDelete.Visible = bDisplayData;
            PnlPopupEdit.Visible = bDisplayData;
            RptHoatDong.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin hoạt động";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy hoạt động";
                }
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
            }
            else
            {
                MainDataPager.Visible = true;
            }
        }
        #endregion

        #region DataPager event handlers
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            MainDataPager.CurrentIndex = currentPageIndex;
            BindRepeater();
        }
        #endregion

        #region Button event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            isSearch = true;
            BindRepeater();
        }

        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {
            string strNgay = this.TxtNgayThem.Text.Trim();
            int maHocSinh = (int)ViewState["MaHocSinh"];
            int maHocKy = Int32.Parse(this.DdlHocKyThem.SelectedValue);
            string tieuDe = this.TxtTieuDeThem.Text.Trim();

            if (tieuDe == "")
            {
                TieuDeRequiredAdd.IsValid = false;
                MPEAdd.Show();
                return;
            }
            else
            {
                if (strNgay == "")
                {
                    NgayRequiredAdd.IsValid = false;
                    MPEAdd.Show();
                    return;
                }
                else
                {
                    if (!Regex.IsMatch(strNgay, NgayExpression.ValidationExpression))
                    {
                        NgayExpression.IsValid = false;
                        MPEAdd.Show();
                        return;
                    }
                    else
                    {
                        try
                        {
                            DateTime.Parse(strNgay);
                        }
                        catch (Exception ex)
                        {
                            DateTimeValidatorAdd.IsValid = false;
                            MPEAdd.Show();
                            return;
                        }

                        if (hoatDongBL.HoatDongExists(null, tieuDe, maHocSinh, maHocKy, DateTime.Parse(strNgay)))
                        {
                            TieuDeValidatorAdd.IsValid = false;
                            MPEAdd.Show();
                            return;
                        }
                    }
                }
            }

            string moTa = this.TxtMoTaThem.Text;
            DateTime ngay = DateTime.Parse(this.TxtNgayThem.Text);
            int maThaiDoThamGia = Int32.Parse(this.DdlThaiDoThamGiaThem.SelectedValue);

            hoatDongBL.InsertHoatDong(maHocSinh, maHocKy, ngay, tieuDe, moTa, maThaiDoThamGia);

            MainDataPager.CurrentIndex = 1;
            BindRepeater();

            this.DdlHocKyThem.SelectedIndex = 0;
            this.TxtNgayThem.Text = DateTime.Now.ToShortDateString();
            this.TxtTieuDeThem.Text = "";
            this.TxtMoTaThem.Text = "";
            this.DdlThaiDoThamGiaThem.SelectedIndex = 0;

            if (this.CkbAddAfterSave.Checked)
            {
                this.MPEAdd.Show();
            }
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender modalPopupEdit = new ModalPopupExtender();
            foreach (RepeaterItem rptItem in RptHoatDong.Items)
            {
                if (rptItem.ItemType == ListItemType.Item || rptItem.ItemType == ListItemType.AlternatingItem)
                {
                    modalPopupEdit = (ModalPopupExtender)rptItem.FindControl("MPEEdit");
                    if (modalPopupEdit.ClientID == HdfRptHoatDongMPEEdit.Value)
                    {
                        break;
                    }
                }
            }

            int maHoatDong = Int32.Parse(this.HdfMaHoatDong.Value);
            string strNgay = TxtNgaySua.Text.Trim();
            if (strNgay == "")
            {
                NgayRequiredEdit.IsValid = false;
                modalPopupEdit.Show();
                return;
            }
            else
            {
                if (!Regex.IsMatch(strNgay, NgayExpressionEdit.ValidationExpression))
                {
                    NgayExpressionEdit.IsValid = false;
                    modalPopupEdit.Show();
                    return;
                }
                else
                {
                    try
                    {
                        DateTime.Parse(strNgay);
                    }
                    catch (Exception ex)
                    {
                        DateTimeValidatorEdit.IsValid = false;
                        modalPopupEdit.Show();
                        return;
                    }

                    if (hoatDongBL.HoatDongExists(maHoatDong, LblTieuDeSua.Text, (int)ViewState["MaHocSinh"],
                        (int)ViewState["MaHocKy"], DateTime.Parse(strNgay)))
                    {
                        NgayValidatorEdit.IsValid = false;
                        modalPopupEdit.Show();
                        return;
                    }
                }
            }

            DateTime ngay = DateTime.Parse(this.TxtNgaySua.Text);
            string moTa = this.TxtMoTaSua.Text;
            int maThaiDoThamGia = Int32.Parse(this.DdlThaiDoThamGiaSua.SelectedValue);

            hoatDongBL.UpdateHoatDong(maHoatDong, ngay, moTa, maThaiDoThamGia);

            MainDataPager.CurrentIndex = 1;
            BindRepeater();
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            int maHoatDong = Int32.Parse(this.HdfMaHoatDong.Value);
            hoatDongBL.DeleteHoatDong(maHoatDong);

            isSearch = false;
            BindRepeater();
        }

        protected void BtnBackPrevPage_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("danhsachhocsinh.aspx");
        }
        #endregion

        #region Repeater event handlers
        protected void RptHoatDong_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
                        Control control = e.Item.FindControl("HdfRptMaHoatDong");
                        if (control != null)
                        {
                            //int maNgayNghiHoc = Int32.Parse(((HiddenField)control).Value);
                            //if (ngayNghiHocBL.IsXacNhan(maNgayNghiHoc))
                            //{
                            //    ImageButton btnDeleteItem = (ImageButton)e.Item.FindControl("BtnDeleteItem");
                            //    btnDeleteItem.ImageUrl = "~/Styles/Images/button_delete_disable.png";
                            //    btnDeleteItem.Enabled = false;

                            //    ImageButton btnEditItem = (ImageButton)e.Item.FindControl("BtnEditItem");
                            //    btnEditItem.ImageUrl = "~/Styles/Images/button_edit_disable.png";
                            //    btnEditItem.Enabled = false;
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

        protected void RptHoatDong_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDeleteItem":
                    {
                        LblConfirmDelete.Text = "Bạn có chắc xóa hoạt động này không?";
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        HiddenField hdfRptMaHoatDong = (HiddenField)e.Item.FindControl("HdfRptMaHoatDong");
                        HdfMaHoatDong.Value = hdfRptMaHoatDong.Value;

                        HdfRptHoatDongMPEDelete.Value = mPEDelete.ClientID;
                        break;
                    }
                case "CmdEditItem":
                    {
                        int maHoatDong = Int32.Parse(e.CommandArgument.ToString());
                        HocSinh_HoatDong hoatDong = hoatDongBL.GetHoatDong(maHoatDong);

                        this.LblTieuDeSua.Text = hoatDong.TieuDe;
                        this.HdfTieuDe.Value = hoatDong.TieuDe;
                        this.TxtMoTaSua.Text = hoatDong.NoiDung;
                        ViewState["MaHocKy"] = hoatDong.MaHocKy;
                        this.HdfMaHocKy.Value = hoatDong.MaHocKy.ToString();
                        this.LblHocKySua.Text = (new HocKyBL()).GetHocKy(hoatDong.MaHocKy).TenHocKy;
                        this.TxtNgaySua.Text = hoatDong.Ngay.ToShortDateString();
                        if (hoatDong.MaThaiDoThamGia == null)
                        {
                            this.DdlThaiDoThamGiaSua.SelectedValue = "0";
                        }
                        else
                        {
                            this.DdlThaiDoThamGiaSua.SelectedValue = hoatDong.MaThaiDoThamGia.ToString();
                        }

                        ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        mPEEdit.Show();

                        this.HdfMaHoatDong.Value = e.CommandArgument.ToString();
                        this.HdfRptHoatDongMPEEdit.Value = mPEEdit.ClientID;

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        #endregion
    }
}