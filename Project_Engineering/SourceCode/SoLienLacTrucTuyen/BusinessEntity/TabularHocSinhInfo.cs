using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.BusinessEntity
{
    public class TabularStudent
    {
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public string StudentCode { get; set; }
        public string FullName { get; set; }
        public string FacultyName { get; set; }
        public string GradeName { get; set; }
        public string ClassName { get; set; }
        public bool Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string StringDateOfBirth { get; set; }
        public string StringGender { get; set; }
        public int StudentInClassId { get; set; }
    }
}
