using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.BusinessEntity
{
    public class TabularClass
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }

        public int FacultyId { get; set; }
        public string FacultyName { get; set; }

        public int GradeId { get; set; }
        public string GradeName { get; set; }

        public Guid HomeroomTeacherCode { get; set; }
        public string TenGVCN { get; set; }
        public int SiSo { get; set; }

        public int YearId { get; set; }
        public string YearName { get; set; }
    }
}
