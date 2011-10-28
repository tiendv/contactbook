using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.BusinessEntity
{
    public class ThoiKhoaBieuTheoThu
    {
        public int MaLopHoc { get; set; }

        public int MaNamHoc { get; set; }
        public int MaHocKy { get; set; }
        public int MaThu { get; set; }
        public string TenThu { get; set; }

        public List<ThoiKhoaBieuTheoBuoi> ListThoiKhoaBieuTheoBuoi { get; set; }
    }

    public class ThoiKhoaBieuTheoBuoi
    {
        public int MaBuoi { get; set; }
        public List<ThoiKhoaBieuTheoTiet> ListThoiKhoaBieuTheoTiet { get; set; }
    }

    public class ThoiKhoaBieuTheoTiet
    {
        public int MaMonHocTKB { get; set; }
        public int MaMonHoc { get; set; }        
        public string TenMonHoc { get; set; }
        public int MaGiaoVien { get; set; }
        public string TenGiaoVien { get; set; }
        public int Tiet { get; set; }
        public string ChiTietTiet { get; set; }
                
        public int MaLopHoc { get; set; }
        public int MaThu { get; set; }
        public int MaHocKy { get; set; }
    }
}
