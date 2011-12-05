using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.BusinessEntity
{
    [Serializable]
    public class TabularSubject
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }

        public string FacultyName { get; set; }
        public string GradeName { get; set; }
        public double MarkRatio { get; set; }
    }
}
