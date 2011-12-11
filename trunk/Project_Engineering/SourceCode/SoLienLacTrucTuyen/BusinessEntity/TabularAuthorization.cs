using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.BusinessEntity
{
    public class TabularAuthorization
    {
        public string FunctionCategoryName { get; set; }
        public List<TabularDetailedAuthorization> detailedAuthorizations { get; set; }
    }

    public class TabularDetailedAuthorization
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
