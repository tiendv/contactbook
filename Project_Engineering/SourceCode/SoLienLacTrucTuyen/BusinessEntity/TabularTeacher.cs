using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.BusinessEntity
{
    public class TabularTeacher
    {
        public int MaGiaoVien { get; set; }

        public string MaHienThiGiaoVien { get; set; }
        public string HoTen { get; set; }

        private bool gioiTinh;
        public bool GioiTinh 
        {
            get
            {
                return gioiTinh;
            }
            set
            {
                gioiTinh = value;
                StringGioiTinh = (gioiTinh) ? "Nam" : "Nữ";
            }
        }

        private DateTime ngaySinh;
        public DateTime NgaySinh 
        { 
            get
            {
                return ngaySinh;
            } 
            set 
            {
                ngaySinh = value;
                StringNgaySinh = String.Format("{0}/{1}/{2}", ngaySinh.Day, ngaySinh.Month, ngaySinh.Year);
            } 
        }

        public string StringGioiTinh { get; set; }
        public string StringNgaySinh { get; set; }
    }

    public class TabularFormering
    {
        public int MaNamHoc { get; set; }
        public string TenNamHoc { get; set; }

        public int MaLopHoc { get; set; }
        public string TenLopHoc { get; set; }
    }

    public class TabularTeaching
    {
        public int MaNamHoc { get; set; }
        public string TenNamHoc { get; set; }

        public int MaHocKy { get; set; }
        public string TenHocKy { get; set; }

        public int MaLopHoc { get; set; }
        public string TenLopHoc { get; set; }

        public int MaMonHoc { get; set; }
        public string TenMonHoc { get; set; }
    }
}
