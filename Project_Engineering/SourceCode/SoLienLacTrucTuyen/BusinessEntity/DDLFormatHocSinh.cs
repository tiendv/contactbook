using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.BusinessEntity
{
    public class StudentDropdownListItem
    {
        public int StudentId { get; set; }        
        public string StudentCode { get; set; }
        public string StudentName { get; set; }

        public int StudentInClassId { get; set; }

        public const string STUDENT_CODE = "StudentCode";
        public const string STUDENT_IN_CLASS_ID = "StudentInClassId";
    }
}
