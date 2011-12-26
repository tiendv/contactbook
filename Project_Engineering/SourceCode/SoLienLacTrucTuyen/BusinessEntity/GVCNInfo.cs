using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.BusinessEntity
{
    public class GVCNInfo
    {
        public int MaGVCN { get; set; }
        public string MaGVCNHienThi { get; set; }
        public string TenGVCN { get; set; }
        public DateTime NgaySinh { get; set; }
        public bool GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        //public byte[] HinhAnh { get; set; 
    }
}
