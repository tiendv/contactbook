using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.BusinessEntity
{
    public class MyMenuItem
    {
        public int MaMenu { get; set; }
        public string TieuDe { get; set; }
        public string MoTa { get; set; }
        public string Url { get; set; }        
        public int ThuTuHienThi { get; set; }
        public bool HienHanh { get; set; }
        public bool HienThi { get; set; }
        public int CapDo { get; set; }
        public int? ParentMenuId { get; set; }
    }
}
