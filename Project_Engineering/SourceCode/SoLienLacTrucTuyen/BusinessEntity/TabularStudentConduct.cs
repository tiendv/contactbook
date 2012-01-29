using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.BusinessEntity
{
    public class TabularStudentConduct
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public int? ConductId { get; set; }
        public string ConductName { get; set; }

        public int TotalOfUnaskedAbsentDays { get; set; }
        public int TotalOfAskedAbsentDays { get; set; }
        public int TotalOfAbsentDays { get; set; }
    }
}
