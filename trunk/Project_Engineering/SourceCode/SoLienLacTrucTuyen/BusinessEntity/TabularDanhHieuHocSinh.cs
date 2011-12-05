using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.BusinessEntity
{
    public class TabularTermStudentResult
    {
        public int LearningResultIdHSHK { get; set; }
        public int? ConductId { get; set; }
        public double DiemTB { get; set; }

        public string TermName { get; set; }
        public string StrDiemTB { get; set; }
        public string LearningAptitudeName { get; set; }
        public string ConductName { get; set; }
        public string LearningResultName { get; set; }
    }
}
