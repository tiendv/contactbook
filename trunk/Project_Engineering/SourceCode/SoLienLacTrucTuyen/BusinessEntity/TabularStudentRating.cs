using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.BusinessEntity
{
    public class TabularStudentRating
    {
        public int OrderNo { get; set; }
        public string StudentFullName { get; set; }
        public string StudentCode { get; set; }

        // AverageMark
        public double AverageMark { get; set; }
        public string StringAverageMark { get; set; }

        public int ConductId { get; set; }
        public string ConductName { get; set; }
        public int LearningAptitudeId { get; set; }
        public string LearningAptitudeName { get; set; }

        // LearningResultName
        public string LearningResultName { get; set; }

        public string TermName { get; set; }
    }
}
