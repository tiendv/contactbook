using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.BusinessEntity
{
    public class TabularPhanQuyen
    {
        public string FunctionCategoryName { get; set; }
        public List<TabularChiTietPhanQuyen> ListChiTietPhanQuyens { get; set; }
    }

    public class TabularChiTietPhanQuyen
    {
        public int FunctionId { get; set; }
        public string FunctionName { get; set; }
        public bool ViewAccessibility { get; set; }
        public bool AddAccessibility { get; set; }
        public bool ModifyAccessibility { get; set; }
        public bool DeleteAccessibility { get; set; }

        public bool ViewDisplay { get; set; }
        public bool AddDisplay { get; set; }
        public bool ModifyDisplay { get; set; }
        public bool DeleteDisplay { get; set; }
    }
}
