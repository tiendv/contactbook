using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.BusinessEntity
{
    public class TabularGiaoVienChuNhiem
    {
        public int MaGVCN { get; set; }

        public int MaGiaoVien { get; set; }
        public string TenGiaoVien { get; set; }
        public int MaLopHoc { get; set; }
        public string TenLopHoc { get; set; }
    }
}
