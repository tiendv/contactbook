using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.BusinessEntity
{
    public class TabularTeacher
    {
        public Guid MaGiaoVien { get; set; }

        public string MaHienThiGiaoVien { get; set; }
        public string HoTen { get; set; }

        private bool? gioiTinh;
        public bool? GioiTinh 
        {
            get
            {
                return gioiTinh;
            }
            set
            {
                gioiTinh = value;
                if (gioiTinh != null)
                {
                    StringGioiTinh = ((bool)gioiTinh) ? "Nam" : "Nữ";
                }
            }
        }

        private DateTime? ngaySinh;
        public DateTime? NgaySinh 
        { 
            get
            {
                return ngaySinh;
            } 
            set 
            {
                ngaySinh = value;
                if (ngaySinh != null)
                {
                    DateTime birthday = (DateTime)ngaySinh;
                    StringNgaySinh = String.Format("{0}/{1}/{2}", birthday.Day, birthday.Month, birthday.Year);
                }
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
