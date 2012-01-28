using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.BusinessEntity
{
    public class TabularLearningResult
    {
        public int LearningResultId { get; set; }
        public string LearningResultName { get; set; }

        public List<TabularDetailLearningResult> DetailLearningResults { get; set; }
    }

    public class TabularDetailLearningResult
    {
        public int LearningResultId { get; set; }

        public int LearningAptitudeId { get; set; }
        public string LearningAptitudeName { get; set; }

        public double BeginAverageMark { get; set; }
        public double EndAverageMark { get; set; }

        public int ConductId { get; set; }
        public string ConductName { get; set; }

    }
}
