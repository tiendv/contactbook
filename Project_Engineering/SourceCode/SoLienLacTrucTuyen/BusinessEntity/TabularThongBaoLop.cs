using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.BusinessEntity
{
    public class TabularThongBaoLop
    {
        public int MaThongBaoLop { get; set; }
        public DateTime Ngay { get; set; }
        public int ClassId { get; set; }

        public string TieuDe { get; set; }
        public string StrNgay { get; set; }
        public string TenLop { get; set; }
    }
}
