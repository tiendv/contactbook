using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.BusinessEntity
{
    public class TabularTermStudentResult
    {
        public string TermName { get; set; }

        // Conduct
        public int? ConductId { get; set; }
        public string ConductName { get; set; }

        // LearningAptitude
        public int LearningAptitudeId { get; set; }
        public string LearningAptitudeName { get; set; }

        // AverageMark
        public double AverageMark { get; set; }
        public string StringAverageMark { get; set; }

        // LearningResultName
        public string LearningResultName { get; set; }
    }
}
