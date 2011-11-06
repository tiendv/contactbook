using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.BusinessEntity
{
    public class TabularTermStudentResult
    {
        public int MaDanhHieuHSHK { get; set; }
        public int? MaHanhKiem { get; set; }
        public double DiemTB { get; set; }

        public string TenHocKy { get; set; }
        public string StrDiemTB { get; set; }
        public string TenHocLuc { get; set; }
        public string TenHanhKiem { get; set; }
        public string TenDanhHieu { get; set; }
    }
}
