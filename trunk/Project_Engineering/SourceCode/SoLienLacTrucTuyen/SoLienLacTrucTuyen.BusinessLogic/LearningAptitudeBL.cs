using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class LearningAptitudeBL : BaseBL
    {
        LearningAptitudeDA learningAptitudeDA;

        public LearningAptitudeBL(School_School school)
            : base(school)
        {
            learningAptitudeDA = new LearningAptitudeDA(school);
        }

        public void InsertLearningAptitude(Category_LearningAptitude learningAptitude)
        {
            learningAptitudeDA.InsertLearningAptitude(learningAptitude);
        }

        public void UpdateLearningAptitude(Category_LearningAptitude learningAptitude, string learningAptitudeName, double beginAverageMark, double endAverageMark)
        {
            learningAptitude.LearningAptitudeName = learningAptitudeName;
            learningAptitude.BeginAverageMark = beginAverageMark;
            learningAptitude.EndAverageMark = endAverageMark;

            learningAptitudeDA.UpdateLearningAptitude(learningAptitude);
        }

        public void DeleteLearningAptitude(Category_LearningAptitude learningAptitude)
        {
            learningAptitudeDA.DeleteLearningAptitude(learningAptitude);
        }

        public Category_LearningAptitude GetLearningAptitude(int learningAptitudeId)
        {
            return learningAptitudeDA.GetLearningAptitude(learningAptitudeId);
        }

        public Category_LearningAptitude GetLearningAptitude(double averageMark)
        {
            return learningAptitudeDA.GetLearningAptitude(averageMark);
        }

        public List<Category_LearningAptitude> GetLearningAptitudes()
        {
            return learningAptitudeDA.GetLearningAptitudes();
        }

        public List<Category_LearningAptitude> GetLearningAptitudes(string learningAptitudeName, int pageCurrentIndex, int pageSize, out double totalRecord)
        {
            if ((learningAptitudeName == "") || (string.Compare(learningAptitudeName, "tất cả", true) == 0))
            {
                return learningAptitudeDA.GetLearningAptitudes(pageCurrentIndex, pageSize, out totalRecord);
            }
            else
            {
                return learningAptitudeDA.GetLearningAptitudes(learningAptitudeName, pageCurrentIndex, pageSize, out totalRecord);
            }            
        }

        public bool LearningAptitudeNameExists(string learningAptitudeName)
        {
            return learningAptitudeDA.LearningAptitudeNameExists(learningAptitudeName);
        }

        public bool LearningAptitudeNameExists(string oldLearningAptitudeName, string newLearningAptitudeName)
        {
            if (oldLearningAptitudeName == newLearningAptitudeName)
            {
                return false;
            }
            else
            {
                return LearningAptitudeNameExists(newLearningAptitudeName);
            }
        }

        public bool IsDeletable(Category_LearningAptitude learningAptitude)
        {
            StudyingResultBL studyingResultBL = new StudyingResultBL(school);
            return studyingResultBL.TermLearningResultExists(learningAptitude);
        }
    }
}
