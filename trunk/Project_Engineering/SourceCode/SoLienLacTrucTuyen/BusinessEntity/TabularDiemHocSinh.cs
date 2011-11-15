using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.BusinessEntity
{
    public class TabularStudentMark
    {
        public int MaHocSinh { get; set; }
        public int MaDiemHK { get; set; }
        public int MaHocSinhLopHoc { get; set; }
        public string MaHocSinhHienThi { get; set; }
        public string TenHocSinh { get; set; }

        public double DiemTrungBinh { get; set; }

        public List<MarkTypedMark> DiemTheoLoaiDiems { get; set; }
    }

    public struct DetailMark
    {
        public int MaLoaiDiem { get; set; }
        public double GiaTri { get; set; }
    }

    public class MarkTypedMark
    {
        public int MaLoaiDiem { get; set; }
        public string StringDiems { get; set; }
        public string TenLoaiDiem { get; set; }
    }
}
