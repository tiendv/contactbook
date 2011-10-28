using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.BusinessEntity
{
    public class TabularLoiNhanKhan
    {
        public int MaLoiNhanKhan { get; set; }
        public DateTime Ngay { get; set; }
        public int MaHocSinh { get; set; }

        public string TieuDe { get; set; }        
        public string StrNgay { get; set; }
        public string MaHocSinhHienThi { get; set; }
        public string TenHocSinh { get; set; }
        public string XacNhan { get; set; }
    }
}
