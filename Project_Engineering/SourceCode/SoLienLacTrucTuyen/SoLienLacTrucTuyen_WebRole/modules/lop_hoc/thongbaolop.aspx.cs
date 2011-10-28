﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessEntity;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;
using AjaxControlToolkit;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class ThongBao : System.Web.UI.Page
    {
        //private UserBL userBL;
        
        //private ThongBaoLopBL thongBaoBL;
        //private bool isSearch;

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    thongBaoBL = new ThongBaoBL();
        //    userBL = new UserBL();

        //    Site masterPage = (Site)Page.Master;
        //    masterPage.UserRole = userBL.GetRoleId(User.Identity.Name);
        //    masterPage.PageUrl = Page.Request.Path;
        //    masterPage.PageTitle = "thông báo";

        //    if (!Page.IsPostBack)
        //    {
        //        isSearch = false;
        //        BindDropDownListNamHoc();
        //        BindDropDownListXacNhan();
        //        InitDates();
        //        BindRepeater();

        //        BindDropDownListNganhHoc();
        //        BindDropDownListKhoiLop();
        //        BindDropDownListLopHocThem();
        //    }
        //}

        ////#region Methods
        ////private void BindRepeater()
        ////{
        ////    int maNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);
        ////    DateTime tuNgay = DateTime.Parse(TxtTuNgay.Text);
        ////    DateTime denNgay = DateTime.Parse(TxtDenNgay.Text);
        ////    string maHocSinhHienThi = TxtMaHS.Text;
        ////    int xacNhan = Int32.Parse(DdlXacNhan.SelectedValue);

        ////    double totalRecords;
        ////    List<TabularThongBao> lstTabularThongBao = thongBaoBL.GetListTabularThongBao(
        ////        maNamHoc, tuNgay, denNgay,
        ////        maHocSinhHienThi, xacNhan, MainDataPager.CurrentIndex, MainDataPager.PageSize, out totalRecords);
            
        ////    if (lstTabularThongBao.Count == 0 && totalRecords != 0)
        ////    {
        ////        MainDataPager.CurrentIndex--;
        ////        BindRepeater();
        ////        return;
        ////    }

        ////    bool bDisplayData = (lstTabularThongBao.Count != 0) ? true : false;
        ////    ProcessDislayInfo(bDisplayData);
        ////    RptThongBao.DataSource = lstTabularThongBao;
        ////    RptThongBao.DataBind();
        ////    MainDataPager.ItemCount = totalRecords;
        ////}

        ////private void ProcessDislayInfo(bool bDisplayData)
        ////{
        ////    //PnlPopupConfirmDelete.Visible = bDisplayData;
        ////    //PnlPopupEdit.Visible = bDisplayData;
        ////    //PnlPopupDetail.Visible = bDisplayData;
        ////    RptThongBao.Visible = bDisplayData;
        ////    LblSearchResult.Visible = !bDisplayData;

        ////    if (LblSearchResult.Visible)
        ////    {
        ////        if (!isSearch)
        ////        {
        ////            LblSearchResult.Text = "Chưa có thông tin thông báo";
        ////        }
        ////        else
        ////        {
        ////            LblSearchResult.Text = "Không tìm thấy thông báo";
        ////        }
        ////        MainDataPager.ItemCount = 0;
        ////        MainDataPager.Visible = false;
        ////    }
        ////    else
        ////    {
        ////        MainDataPager.Visible = true;
        ////    }
        ////}

        ////private void BindDropDownListNamHoc()
        ////{
        ////    NamHocBL namHocBL = new NamHocBL();
        ////    List<CauHinh_NamHoc> lstNamHoc = namHocBL.GetListNamHoc();
        ////    DdlNamHoc.DataSource = lstNamHoc;
        ////    DdlNamHoc.DataValueField = "MaNamHoc";
        ////    DdlNamHoc.DataTextField = "TenNamHoc";
        ////    DdlNamHoc.DataBind();
        ////    DdlNamHoc.SelectedValue = (new CauHinhHeThongBL()).GetMaNamHocHienHanh().ToString();
        ////}

        ////private void BindDropDownListXacNhan()
        ////{
        ////    DdlXacNhan.Items.Add(new ListItem("Tất cả", "-1"));
        ////    DdlXacNhan.Items.Add(new ListItem("Có", "1"));
        ////    DdlXacNhan.Items.Add(new ListItem("Không", "0"));            
        ////}

        ////private void InitDates()
        ////{
        ////    DateTime today = DateTime.Now;
        ////    DateTime beginDateOfMonth = new DateTime(today.Year, today.Month, 1);
        ////    TxtTuNgay.Text = beginDateOfMonth.ToShortDateString();
        ////    DateTime dateOfNextMonth = today.AddMonths(1);
        ////    DateTime beginDateOfNextMonth = new DateTime(dateOfNextMonth.Year, dateOfNextMonth.Month, 1);
        ////    DateTime endDateOfMonth = beginDateOfNextMonth.AddDays(-1);
        ////    TxtDenNgay.Text = endDateOfMonth.ToShortDateString();

        ////    TxtNgayThem.Text = today.ToShortDateString();
        ////}

        ////private void BindDropDownListKhoiLop()
        ////{
        ////    KhoiLopBL KhoiLopBL = new KhoiLopBL();
        ////    List<DanhMuc_KhoiLop> lstKhoiLop = KhoiLopBL.GetListKhoiLop();
        ////    DdlKhoiLopThem.DataSource = lstKhoiLop;
        ////    DdlKhoiLopThem.DataValueField = "MaKhoiLop";
        ////    DdlKhoiLopThem.DataTextField = "TenKhoiLop";
        ////    DdlKhoiLopThem.DataBind();
        ////    if (lstKhoiLop.Count > 1)
        ////    {
        ////        DdlKhoiLopThem.Items.Insert(0, new ListItem("Tất cả", "0"));
        ////    }
        ////}

        ////private void BindDropDownListNganhHoc()
        ////{
        ////    NganhHocBL nganhHocBL = new NganhHocBL();
        ////    List<DanhMuc_NganhHoc> lstNganhHoc = nganhHocBL.GetListNganhHoc();
        ////    DdlNganhHocThem.DataSource = lstNganhHoc;
        ////    DdlNganhHocThem.DataValueField = "MaNganhHoc";
        ////    DdlNganhHocThem.DataTextField = "TenNganhHoc";
        ////    DdlNganhHocThem.DataBind();
        ////    if (lstNganhHoc.Count > 1)
        ////    {
        ////        DdlNganhHocThem.Items.Insert(0, new ListItem("Tất cả", "0"));
        ////    }
        ////}

        ////private void BindDropDownListLopHocThem()
        ////{
        ////    int maNamHoc = (new CauHinhHeThongBL()).GetMaNamHocHienHanh();

        ////    int maNganhHoc = 0;
        ////    try
        ////    {
        ////        maNganhHoc = Int32.Parse(DdlNganhHocThem.SelectedValue);
        ////    }
        ////    catch (Exception) { }

        ////    int maKhoiLop = 0;
        ////    try
        ////    {
        ////        maKhoiLop = Int32.Parse(DdlKhoiLopThem.SelectedValue);
        ////    }
        ////    catch (Exception) { }

        ////    LopHocBL lopHocBL = new LopHocBL();
        ////    List<LopHoc_Lop> lstLop = lopHocBL.GetListLopHoc(maNganhHoc, maKhoiLop, maNamHoc);
        ////    DdlLopThem.DataSource = lstLop;
        ////    DdlLopThem.DataValueField = "MaLopHoc";
        ////    DdlLopThem.DataTextField = "TenLopHoc";
        ////    DdlLopThem.DataBind();

        ////    if (lstLop.Count > 1)
        ////    {
        ////        DdlLopThem.Items.Insert(0, new ListItem("Tất cả", "0"));
        ////    }

        ////    BindDropDownListHocSinhThem();
        ////}        
        ////#endregion

        ////#region DropDownList event hanlders
        ////protected void DdlNganhThem_SelectedIndexChanged(object sender, EventArgs e)
        ////{
        ////    BindDropDownListLopHocThem();
        ////}

        ////protected void DdlKhoiLopThem_SelectedIndexChanged(object sender, EventArgs e)
        ////{
        ////    BindDropDownListLopHocThem();
        ////}

        ////protected void DdlLopHocThem_SelectedIndexChanged(object sender, EventArgs e)
        ////{
        ////    BindDropDownListHocSinhThem();
        ////}

        ////private void BindDropDownListHocSinhThem()
        ////{
        ////    if (DdlLopThem.Items.Count != 0)
        ////    {
        ////        int maNganh = Int32.Parse(DdlNganhHocThem.SelectedValue);
        ////        int maKhoi = Int32.Parse(DdlKhoiLopThem.SelectedValue);
        ////        int maLopHoc = Int32.Parse(DdlLopThem.SelectedValue);
        ////        List<DDLFormatHocSinh> lstHocSinh = (new HocSinhBL()).GetListHocSinh(maNganh, maKhoi, maLopHoc);
        ////        DdlHocSinhThem.DataSource = lstHocSinh;
        ////        DdlHocSinhThem.DataTextField = "MaHocSinhHienThi";
        ////        DdlHocSinhThem.DataValueField = "MaHocSinhLopHoc";
        ////        DdlHocSinhThem.DataBind();
        ////        if (DdlHocSinhThem.Items.Count > 1)
        ////        {
        ////            DdlHocSinhThem.Items.Insert(0, new ListItem("Tất cả", "0"));
        ////        }
        ////    }
        ////    else
        ////    {
        ////        DdlHocSinhThem.DataSource = new List<HocSinh_ThongTinCaNhan>();
        ////        DdlHocSinhThem.DataTextField = "MaHocSinhHienThi";
        ////        DdlHocSinhThem.DataValueField = "MaHocSinh";
        ////        DdlHocSinhThem.DataBind();
        ////    }                
        ////}
        ////#endregion

        ////#region Repeater event handlers
        ////protected void RptThongBao_ItemDataBound(object sender, RepeaterItemEventArgs e)
        ////{
        ////    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        ////    {
        ////        //LopHocInfo lopHoc = (LopHocInfo)e.Item.DataItem;
        ////        //if (lopHoc != null)
        ////        //{
        ////        //    int maLopHoc = lopHoc.MaLopHoc;
        ////        //    if (!lopHocBL.CheckCanDeleteLopHoc(maLopHoc))
        ////        //    {
        ////        //        ImageButton btnDeleteItem = (ImageButton)e.Item.FindControl("BtnDeleteItem");
        ////        //        btnDeleteItem.ImageUrl = "~/Styles/Images/button_delete_disable.png";
        ////        //        btnDeleteItem.Enabled = false;
        ////        //    }
        ////        //}
        ////    }
        ////}

        ////protected void RptThongBao_ItemCommand(object source, RepeaterCommandEventArgs e)
        ////{
        ////    switch (e.CommandName)
        ////    {
        ////        case "CmdDeleteItem":
        ////            {
        ////                this.LblConfirmDelete.Text = "Bạn có chắc xóa thông báo <b>" + e.CommandArgument + "</b> này không?";
        ////                ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
        ////                mPEDelete.Show();

        ////                HiddenField hdfRptMaThongBao = (HiddenField)e.Item.FindControl("HdfRptMaThongBao");
        ////                this.HdfMaThongBao.Value = hdfRptMaThongBao.Value;

        ////                this.HdfRptThongBaoMPEDelete.Value = mPEDelete.ClientID;

        ////                break;
        ////            }
        ////        case "CmdEditItem":
        ////            {
        ////                int maThongBao = Int32.Parse(e.CommandArgument.ToString());
        ////                ThongBao_ThongBao ThongBao = thongBaoBL.GetThongBao(maThongBao);

        ////                TxtTieuDeSua.Text = ThongBao.TieuDe;
        ////                TxtNoiDungSua.Text = TxtNoiDungSua.Text;
        ////                TxtNgaySua.Text = ThongBao.Ngay.ToShortDateString();

        ////                HocSinh_HocSinhLopHoc hocSinhLopHoc = (new HocSinhBL()).GetHocSinhLopHoc(ThongBao.MaHocSinhLopHoc);
        ////                LblMaHocSinhSua.Text = (new HocSinhBL()).GetThongTinCaNhan(hocSinhLopHoc.MaHocSinh).MaHocSinhHienThi;
        ////                LopHocInfo lopHoc = (new LopHocBL()).GetLopHocInfo(hocSinhLopHoc.MaLopHoc);
        ////                LblNganhHocSua.Text = lopHoc.TenNganhHoc;
        ////                LblKhoiSua.Text = lopHoc.TenKhoiLop;
        ////                LblLopSua.Text = lopHoc.TenLopHoc;

        ////                ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
        ////                mPEEdit.Show();

        ////                this.HdfMaThongBao.Value = maThongBao.ToString();
        ////                this.HdfRptThongBaoMPEEdit.Value = mPEEdit.ClientID;

        ////                break;
        ////            }
        ////        case "CmdDetailItem":
        ////            {
        ////                //int maLopHoc = Int32.Parse(e.CommandArgument.ToString());
        ////                //LopHoc_Lop lophoc = lopHocBL.GetLopHoc(maLopHoc);

        ////                //LblTenLopHocChiTiet.Text = lophoc.TenLopHoc;
        ////                //LblTenNganhHocChiTiet.Text = (new NganhHocBL()).GetNganhHoc(lophoc.MaNganhHoc).TenNganhHoc;
        ////                //LblTenKhoiLopChiTiet.Text = (new KhoiLopBL()).GetKhoiLop(lophoc.MaKhoiLop).TenKhoiLop;
        ////                //LblSiSoChiTiet.Text = lophoc.SiSo.ToString();
        ////                //ModalPopupExtender mPEDetail = (ModalPopupExtender)e.Item.FindControl("MPEDetail");
        ////                //mPEDetail.Show();

        ////                //this.HdfMaLopHoc.Value = maLopHoc.ToString();
        ////                //this.HdfRptLopHocMPEDetail.Value = mPEDetail.ClientID;
        ////                break;
        ////            }
        ////        case "CmdDetailItemGVCN":
        ////            {
        ////                //string tenGVCN = e.CommandArgument.ToString();

        ////                //GVCNBL gvcnBL = new GVCNBL();
        ////                //this.HdfMaLopHocChiTiet.Value = giaoVienChuNhiem.MaLopHoc.ToString();
        ////                //string tenLopHoc = lopHocBL.GetLopHoc(giaoVienChuNhiem.MaLopHoc).TenLopHoc;
        ////                //this.TxtLopHocChiTiet.Text = tenLopHoc;

        ////                //this.LblTenGVCNChiTiet.Text = giaoVienChuNhiem.HoTen;
        ////                //DateTime ngaySinh = giaoVienChuNhiem.NgaySinh;
        ////                //this.LblNgaySinhChiTiet.Text = ngaySinh.Day + "/" + ngaySinh.Month + "/" + ngaySinh.Year;
        ////                //this.LblGioiTinhChiTiet.Text = (giaoVienChuNhiem.GioiTinh) ? "Nam" : "Nữ";
        ////                //this.LblDiaChiChiTiet.Text = giaoVienChuNhiem.DiaChi;
        ////                //this.LblDienThoaiChiTiet.Text = giaoVienChuNhiem.DienThoai;

        ////                //ModalPopupExtender mPEDetailGVCN = (ModalPopupExtender)e.Item.FindControl("MPEDetailGVCN");
        ////                //mPEDetailGVCN.Show();

        ////                //this.HdfMaGVCN.Value = maGVCN.ToString();
        ////                //this.HdfRptGVCNMPEDetail.Value = mPEDetail.ClientID;

        ////                break;
        ////            }
        ////        default:
        ////            {
        ////                break;
        ////            }
        ////    }
        ////}
        ////#endregion

        ////#region Button event handlers
        ////protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        ////{
        ////    MainDataPager.CurrentIndex = 1;
        ////    isSearch = true;
        ////    BindRepeater();
        ////}

        ////protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        ////{
        ////    MainDataPager.CurrentIndex = 1;

        ////    string tieuDe = TxtTieuDeThem.Text;
        ////    string noiDung = TxtNoiDungThem.Text;
        ////    DateTime ngay = DateTime.Parse(TxtNgayThem.Text);
        ////    int maHocSinhLopHoc = Int32.Parse(DdlHocSinhThem.SelectedValue);
        ////    if (maHocSinhLopHoc == 0)
        ////    {
        ////        for(int i = 1; i < DdlHocSinhThem.Items.Count; i++)
        ////        {
        ////            thongBaoBL.InsertThongBao(Int32.Parse(DdlHocSinhThem.Items[i].Value),
        ////                tieuDe, noiDung, ngay);
        ////        }
        ////    }
        ////    else
        ////    {
        ////        thongBaoBL.InsertThongBao(maHocSinhLopHoc,
        ////                tieuDe, noiDung, ngay);
        ////    }

        ////    BindRepeater();

        ////    TxtTieuDeThem.Text = "";
        ////    TxtNoiDungThem.Text = "";
        ////    TxtNgayThem.Text = DateTime.Now.ToShortDateString();

        ////    if (this.CkbAddAfterSave.Checked)
        ////    {
        ////        this.MPEAdd.Show();
        ////    }
        ////}

        ////protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        ////{
        ////    int maThongBao = Int32.Parse(this.HdfMaThongBao.Value);
        ////    thongBaoBL.UpdateThongBao(maThongBao, TxtTieuDeSua.Text, TxtNoiDungSua.Text, DateTime.Parse(TxtNgaySua.Text));
        ////    BindRepeater();
        ////}

        ////protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        ////{
        ////    int maLopNhanKhan = Int32.Parse(this.HdfMaThongBao.Value);
        ////    thongBaoBL.DeleteLopHoc(maLopNhanKhan);
        ////    isSearch = false;
        ////    BindRepeater();
        ////}
        ////#endregion

        ////#region Pager event handlers
        ////public void MainDataPager_Command(object sender, CommandEventArgs e)
        ////{
        ////    int currentPageIndex = Convert.ToInt32(e.CommandArgument);
        ////    this.MainDataPager.CurrentIndex = currentPageIndex;
        ////    //BindRepeaterLopHoc();
        ////}
        ////#endregion
    }
}