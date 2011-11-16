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

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class thongtincanhan : BaseContentPage
    {
        #region Fields
        private StudentBL hocSinhBL;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (isAccessDenied)
            {
                return;
            }

            hocSinhBL = new StudentBL(UserSchool);

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["hocsinh"] != null)
                {
                    int maHocSinh = Int32.Parse(Request.QueryString["hocsinh"]);
                    ViewState["MaHocSinh"] = maHocSinh;
                    BindDropDownListNamHoc(maHocSinh);
                    FillThongTinCaNhan(maHocSinh);

                    HlkKetQuaHocTap.NavigateUrl = string.Format("ketquahoctap.aspx?hocsinh={0}", maHocSinh);
                    HlkNgayNghiHoc.NavigateUrl = string.Format("ngaynghihoc.aspx?hocsinh={0}", maHocSinh);                    
                    HlkHoatDong.NavigateUrl = String.Format("hoatdong.aspx?hocsinh={0}", maHocSinh);
                }
            }            
        }
        #endregion

        #region Methods
        private void BindDropDownListNamHoc(int maHocSinh)
        {
            HocSinh_ThongTinCaNhan student = new HocSinh_ThongTinCaNhan();
            student.MaHocSinh = maHocSinh;
            List<CauHinh_NamHoc> lstNamHoc = hocSinhBL.GetYears(student);
            DdlNamHoc.DataSource = lstNamHoc;
            DdlNamHoc.DataValueField = "MaNamHoc";
            DdlNamHoc.DataTextField = "TenNamHoc";
            DdlNamHoc.DataBind();
        }

        private void FillLopHoc()
        {
            HocSinh_ThongTinCaNhan student = new HocSinh_ThongTinCaNhan();
            student.MaHocSinh = (int)ViewState["MaHocSinh"];

            CauHinh_NamHoc year = new CauHinh_NamHoc();
            year.MaNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);            

            TabularClass lopHoc = hocSinhBL.GetTabularClass(year, student);
            LblLopHoc.Text = lopHoc.TenLopHoc;
        }

        private void FillThongTinCaNhan(int maHocSinh)
        {
            HocSinh_ThongTinCaNhan thongTinCaNhan = hocSinhBL.GetStudent(maHocSinh);

            this.LblMaHocSinhHienThi.Text = thongTinCaNhan.MaHocSinhHienThi;
            this.LblHoTenHocSinh.Text = thongTinCaNhan.HoTen;
            this.LblGioiTinh.Text = (thongTinCaNhan.GioiTinh == true) ? "Nam" : "Nữ";
            this.LblNgaySinhHocSinh.Text = thongTinCaNhan.NgaySinh.Day.ToString()
                + "/" + thongTinCaNhan.NgaySinh.Month.ToString()
                + "/" + thongTinCaNhan.NgaySinh.Year.ToString();
            this.LblNoiSinh.Text = thongTinCaNhan.NoiSinh;
            this.LblDiaChi.Text = thongTinCaNhan.DiaChi;
            this.LblDienThoai.Text = thongTinCaNhan.DienThoai;
            this.LblHoTenBo.Text = thongTinCaNhan.HoTenBo;
            if (thongTinCaNhan.NgaySinhBo != null)
            {
                DateTime ngaySinhBo = (DateTime)thongTinCaNhan.NgaySinhBo;
                this.LblNgaySinhBo.Text = ngaySinhBo.Day.ToString() + "/" + ngaySinhBo.Month.ToString()
                    + "/" + ngaySinhBo.Year.ToString();
            }
            this.LblNgheNghiepBo.Text = thongTinCaNhan.NgheNghiepBo;
            this.LblHoTenMe.Text = thongTinCaNhan.HoTenMe;
            if (thongTinCaNhan.NgaySinhMe != null)
            {
                DateTime ngaySinhMe = (DateTime)thongTinCaNhan.NgaySinhMe;
                this.LblNgaySinhMe.Text = ngaySinhMe.Day.ToString() + "/" + ngaySinhMe.Month.ToString()
                    + "/" + ngaySinhMe.Year.ToString();
            }
            this.LblNgheNghiepMe.Text = thongTinCaNhan.NgheNghiepMe;
            if (thongTinCaNhan.NgaySinhNguoiDoDau != null)
            {
                DateTime ngaySinhNguoiDoDau = (DateTime)thongTinCaNhan.NgaySinhNguoiDoDau;
                this.LblNgaySinhNguoiDoDau.Text = ngaySinhNguoiDoDau.Day.ToString() + "/" + ngaySinhNguoiDoDau.Month.ToString()
                    + "/" + ngaySinhNguoiDoDau.Year.ToString();
            }
            this.LblNgheNghiepNguoiDoDau.Text = thongTinCaNhan.NgheNghiepNguoiDoDau;

            FillLopHoc();
        }
        #endregion

        #region Button event handlers
        protected void BtnSua_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(string.Format("suahocsinh.aspx?hocsinh={0}", ViewState["MaHocSinh"]));
        }

        protected void BtnBackPrevPage_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("danhsachhocsinh.aspx");
        }
        #endregion

        #region Dropdownlist event handlers
        protected void DdlNamHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillLopHoc();
        }
        #endregion
    }
}