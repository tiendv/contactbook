using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.BusinessEntity
{
    [Serializable]
    public class MonHocInfo
    {
        public int MaMonHoc { get; set; }
        public string TenMonHoc { get; set; }
        public string TenNganhHoc { get; set; }
        public string TenKhoiLop { get; set; }
        public double HeSoDiem { get; set; }
    }
}
