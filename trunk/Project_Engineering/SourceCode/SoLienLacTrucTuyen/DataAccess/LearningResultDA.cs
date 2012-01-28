using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.DataAccess
{
    public class LearningResultDA : BaseDA
    {
        public LearningResultDA(School_School school)
            : base(school)
        {
        }

        public Category_LearningResult InsertLearningResult(Category_LearningResult learningResult)
        {
            learningResult.SchoolId = school.SchoolId;
            db.Category_LearningResults.InsertOnSubmit(learningResult);
            db.SubmitChanges();

            return GetLastedLearningResult();
        }

        public void UpdateLearningResult(Category_LearningResult learningResult, string learningResultName)
        {
            IQueryable<Category_LearningResult> iqLearningResult = from l in db.Category_LearningResults
                                                                   where l.LearningResultId == learningResult.LearningResultId
                                                                   select l;
            if (iqLearningResult.Count() != 0)
            {
                learningResult = iqLearningResult.First();
                learningResult.LearningResultName = learningResultName;
                db.SubmitChanges();
            }
        }

        public void DeleteLearningResult(Category_LearningResult learningResult)
        {
            // Firstly, delete all details of learning result
            DeleteDetailLearningResult(learningResult);

            // Secondly, delete learning result
            IQueryable<Category_LearningResult> iqLearningResult = from l in db.Category_LearningResults
                                                                   where l.LearningResultId == learningResult.LearningResultId
                                                                   select l;
            if (iqLearningResult.Count() != 0)
            {
                db.Category_LearningResults.DeleteOnSubmit(iqLearningResult.First());
                db.SubmitChanges();
            }
        }
        
        public void InsertDetailLearningResult(Category_DetailedLearningResult detailedLearningResult)
        {
            db.Category_DetailedLearningResults.InsertOnSubmit(detailedLearningResult);
            db.SubmitChanges();
        }        

        public void DeleteDetailLearningResult(Category_LearningResult learningResult)
        {
            IQueryable<Category_DetailedLearningResult> iqDetailedLearningResult;
            iqDetailedLearningResult = from detailedLearningResult in db.Category_DetailedLearningResults
                                       where detailedLearningResult.LearningResultId == learningResult.LearningResultId
                                       select detailedLearningResult;
            if (iqDetailedLearningResult.Count() != 0)
            {
                foreach (Category_DetailedLearningResult detailedLearningResult in iqDetailedLearningResult)
                {
                    db.Category_DetailedLearningResults.DeleteOnSubmit(detailedLearningResult);
                }

                db.SubmitChanges();
            }
        }

        public Category_LearningResult GetLearningResult(int learningResultId)
        {
            IQueryable<Category_LearningResult> iqLearningResult = from l in db.Category_LearningResults
                                                                   where l.LearningResultId == learningResultId
                                                                   select l;
            if (iqLearningResult.Count() != 0)
            {
                return iqLearningResult.First();
            }
            else
            {
                return null;
            }
        }

        public Category_LearningResult GetLastedLearningResult()
        {
            IQueryable<Category_LearningResult> iqLearningResult = from l in db.Category_LearningResults
                                                                   where l.SchoolId == school.SchoolId
                                                                   select l;
            if (iqLearningResult.Count() != 0)
            {
                return iqLearningResult.OrderByDescending(l => l.LearningResultId).First();
            }
            else
            {
                return null;
            }
        }

        public bool LearningResultNameExists(string LearningResultName)
        {
            IQueryable<Category_LearningResult> iqLearningResult = from l in db.Category_LearningResults
                                                                   where l.LearningResultName == LearningResultName
                                                                   && l.SchoolId == school.SchoolId
                                                                   select l;
            if (iqLearningResult.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Category_LearningResult> GetLearningResults(string LearningResultName, int pageCurrentIndex, int pageSize, out double totalRecord)
        {
            IQueryable<Category_LearningResult> iqLearningResult = from l in db.Category_LearningResults
                                                                   where l.LearningResultName == LearningResultName
                                                                   && l.SchoolId == school.SchoolId
                                                                   select l;
            totalRecord = iqLearningResult.Count();
            if (totalRecord != 0)
            {
                return iqLearningResult.OrderBy(l => l.LearningResultName)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<Category_LearningResult>();
            }
        }

        public List<Category_LearningResult> GetLearningResults(int pageCurrentIndex, int pageSize, out double totalRecord)
        {
            IQueryable<Category_LearningResult> iqLearningResult = from l in db.Category_LearningResults
                                                                   where l.SchoolId == school.SchoolId
                                                                   select l;
            totalRecord = iqLearningResult.Count();
            if (totalRecord != 0)
            {
                return iqLearningResult.OrderBy(danhHieu => danhHieu.LearningResultName)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<Category_LearningResult>();
            }
        }

        public bool IsDeletable(Category_LearningResult learningResult)
        {
            return true;
        }

        public Category_LearningResult GetLearningResult(Category_Conduct conduct, Category_LearningAptitude learningAptitude)
        {
            Category_LearningResult learningResult = null;
            IQueryable<Category_LearningResult> iqLearningResult = from detailedLearningResult in db.Category_DetailedLearningResults
                                                                   where detailedLearningResult.ConductId == conduct.ConductId
                                                                   && detailedLearningResult.LearningAptitudeId == learningAptitude.LearningAptitudeId
                                                                   select detailedLearningResult.Category_LearningResult;
            if (iqLearningResult.Count() != 0)
            {
                learningResult = iqLearningResult.First();
            }

            return learningResult;
        }

        public List<Category_DetailedLearningResult> GetDetailedLearningResults(Category_LearningResult learningResult)
        {
            IQueryable<Category_DetailedLearningResult> iqDetailedLearningResult;
            iqDetailedLearningResult = from detailedLearningResult in db.Category_DetailedLearningResults
                                       where detailedLearningResult.LearningResultId == learningResult.LearningResultId
                                       select detailedLearningResult;

            if (iqDetailedLearningResult.Count() != 0)
            {
                return iqDetailedLearningResult.OrderBy(detailedLearningResult => detailedLearningResult.Category_LearningAptitude.BeginAverageMark).ToList();
            }
            else
            {
                return new List<Category_DetailedLearningResult>();
            }
        }
    }
}
