using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.BusinessEntity
{
    public class TabularClass
    {
        public int MaLopHoc { get; set; }
        public string TenLopHoc { get; set; }

        public int MaNganhHoc { get; set; }
        public string TenNganhHoc { get; set; }

        public int MaKhoiLop { get; set; }
        public string TenKhoiLop { get; set; }

        public Guid HomeroomTeacherCode { get; set; }
        public string TenGVCN { get; set; }
        public int SiSo { get; set; }

        public int MaNamHoc { get; set; }
        public string TenNamHoc { get; set; }
    }
}
