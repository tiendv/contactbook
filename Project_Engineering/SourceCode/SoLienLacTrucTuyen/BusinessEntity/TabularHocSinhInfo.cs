﻿using System;
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
        public string Gender { get; set; }
        public string DayOfBirth { get; set; }
    }
}
