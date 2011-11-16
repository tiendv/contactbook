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
    public partial class ThongTinCaNhan_User : BaseContentPage
    {
        #region Fields
        private StudentBL hocSinhBL;
        private int maHocSinh;
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
            //maHocSinh = hocSinhBL.GetStudent(masterPage.UserNameSession);

            if (!Page.IsPostBack)
            {
                BindDropDownListNamHoc();
                FillThongTinCaNhan(maHocSinh);
            }
        }       
        #endregion        

        #region Methods
        private void BindDropDownListNamHoc()
        {
            HocSinh_ThongTinCaNhan student = new HocSinh_ThongTinCaNhan();
            student.MaHocSinh = maHocSinh;
            List<CauHinh_NamHoc> lstNamHoc = hocSinhBL.GetYears(student);
            DdlNamHoc.DataSource = lstNamHoc;
            DdlNamHoc.DataValueField = "MaNamHoc";
            DdlNamHoc.DataTextField = "TenNamHoc";
            DdlNamHoc.DataBind();
        }

        private void FillThongTinCaNhan(int maHocSinh)
        {
            CauHinh_NamHoc year = new CauHinh_NamHoc();
            HocSinh_ThongTinCaNhan student = new HocSinh_ThongTinCaNhan();

            year.MaNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);            
            student.MaHocSinh = maHocSinh;

            HocSinh_HocSinhLopHoc hocSinhLopHoc = hocSinhBL.GetStudentInClass(student, year);
            if(hocSinhLopHoc != null)
            {
                TabularClass lopHoc = hocSinhBL.GetTabularClass(year, student);
                this.LblLopHoc.Text = lopHoc.TenLopHoc;
                this.LblNganhHoc.Text = lopHoc.TenNganhHoc;
                this.LblKhoiLop.Text = lopHoc.TenKhoiLop;

                HocSinh_ThongTinCaNhan thongTinCaNhan = hocSinhBL.GetStudent(maHocSinh);
                this.LblMaHocSinhHienThi.Text = thongTinCaNhan.MaHocSinhHienThi;
                this.LblHoTenHocSinh.Text = thongTinCaNhan.HoTen;
                this.LblGioiTinh.Text = (thongTinCaNhan.GioiTinh == true) ? "Nam" : "Nữ";
                this.LblNgaySinhHocSinh.Text = thongTinCaNhan.NgaySinh.Day.ToString() + "/" + thongTinCaNhan.NgaySinh.Month.ToString()
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
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            FillThongTinCaNhan(maHocSinh);
        }
        #endregion
    }
}