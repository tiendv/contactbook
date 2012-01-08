using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.BusinessEntity
{
    public class TabularMarkType
    {
        public int MarkTypeId { get; set; }

        public string MarkTypeName { get; set; }

        public double MarkRatio { get; set; }

        public short MaxQuantity { get; set; }

        public bool IsUsedForCalculatingAvg { get; set; }

        public int GradeId { get; set; }

        public string GradeName { get; set; }
    }
}
