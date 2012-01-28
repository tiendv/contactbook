using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.DataAccess
{
    public class LearningAptitudeDA : BaseDA
    {
        public LearningAptitudeDA(School_School school)
            : base(school)
        {
        }

        public void InsertLearningAptitude(Category_LearningAptitude learningAptitude)
        {
            learningAptitude.SchoolId = school.SchoolId;
            db.Category_LearningAptitudes.InsertOnSubmit(learningAptitude);
            db.SubmitChanges();
        }

        public void UpdateLearningAptitude(Category_LearningAptitude editedLearningAptitude)
        {
            Category_LearningAptitude learningAptitude = null;

            IQueryable<Category_LearningAptitude> iqLearningAptitude = from la in db.Category_LearningAptitudes
                                                                       where la.LearningAptitudeId == editedLearningAptitude.LearningAptitudeId
                                                                       select la;
            if (iqLearningAptitude.Count() != 0)
            {
                learningAptitude = iqLearningAptitude.First();
                learningAptitude.LearningAptitudeName = editedLearningAptitude.LearningAptitudeName;
                learningAptitude.BeginAverageMark = editedLearningAptitude.BeginAverageMark;
                learningAptitude.EndAverageMark = editedLearningAptitude.EndAverageMark;
                db.SubmitChanges();
            }
        }

        public void DeleteLearningAptitude(Category_LearningAptitude deletedLearningAptitude)
        {
            Category_LearningAptitude learningAptitude = null;

            IQueryable<Category_LearningAptitude> iqLearningAptitude = from la in db.Category_LearningAptitudes
                                                                       where la.LearningAptitudeId == deletedLearningAptitude.LearningAptitudeId
                                                                       select la;
            if (iqLearningAptitude.Count() != 0)
            {
                learningAptitude = iqLearningAptitude.First();
                db.Category_LearningAptitudes.DeleteOnSubmit(learningAptitude);
                db.SubmitChanges();
            }
        }

        public Category_LearningAptitude GetLearningAptitude(int learningAptitudeId)
        {
            Category_LearningAptitude learningAptitude = null;

            IQueryable<Category_LearningAptitude> iqLearningAptitude = from la in db.Category_LearningAptitudes
                                                                       where la.LearningAptitudeId == learningAptitudeId
                                                                       select la;
            if (iqLearningAptitude.Count() != 0)
            {
                learningAptitude = iqLearningAptitude.First();
            }

            return learningAptitude;
        }

        public Category_LearningAptitude GetLearningAptitude(string learningAptitudeName)
        {
            Category_LearningAptitude learningAptitude = null;

            IQueryable<Category_LearningAptitude> iqLearningAptitude = from la in db.Category_LearningAptitudes
                                                                       where la.LearningAptitudeName == learningAptitudeName
                                                                       && la.SchoolId == school.SchoolId
                                                                       select la;
            if (iqLearningAptitude.Count() != 0)
            {
                learningAptitude = iqLearningAptitude.First();
            }

            return learningAptitude;
        }

        public Category_LearningAptitude GetLearningAptitude(double averageMark)
        {
            Category_LearningAptitude learningAptitude = null;

            IQueryable<Category_LearningAptitude> iqLearningAptitude = from lA in db.Category_LearningAptitudes
                                                                       where lA.BeginAverageMark <= averageMark
                                                                       && lA.EndAverageMark >= averageMark
                                                                       && lA.SchoolId == school.SchoolId
                                                                       select lA;
            if (iqLearningAptitude.Count() != 0)
            {
                learningAptitude = iqLearningAptitude.First();
            }

            return learningAptitude;
        }

        public List<Category_LearningAptitude> GetLearningAptitudes()
        {
            IQueryable<Category_LearningAptitude> iqLearningAptitude = from learningAptitude in db.Category_LearningAptitudes
                                                                       where learningAptitude.SchoolId == school.SchoolId
                                                                       select learningAptitude;
            if (iqLearningAptitude.Count() != 0)
            {
                return iqLearningAptitude.OrderBy(learningAptitude => learningAptitude.LearningAptitudeName).ToList();
            }
            else
            {
                return new List<Category_LearningAptitude>();
            }
        }

        public List<Category_LearningAptitude> GetLearningAptitudes(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Category_LearningAptitude> iqLearningAptitude = from learningAptitude in db.Category_LearningAptitudes
                                                                       where learningAptitude.SchoolId == school.SchoolId
                                                                       select learningAptitude;
            totalRecords = iqLearningAptitude.Count();
            if (totalRecords != 0)
            {
                return iqLearningAptitude.OrderBy(learningAptitude => learningAptitude.BeginAverageMark)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<Category_LearningAptitude>();
            }
        }

        public List<Category_LearningAptitude> GetLearningAptitudes(string learningAptitudeName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Category_LearningAptitude> iqLearningAptitude = from learningAptitude in db.Category_LearningAptitudes
                                                                       where learningAptitude.LearningAptitudeName == learningAptitudeName
                                                                            && learningAptitude.SchoolId == school.SchoolId
                                                                       select learningAptitude;
            totalRecords = iqLearningAptitude.Count();
            if (totalRecords != 0)
            {
                return iqLearningAptitude.OrderBy(learningAptitude => learningAptitude.BeginAverageMark)
                   .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<Category_LearningAptitude>();
            }
        }

        public bool LearningAptitudeNameExists(string learningAptitudeName)
        {
            IQueryable<Category_LearningAptitude> iqLearningAptitude = from learningAptitude in db.Category_LearningAptitudes
                                                                       where learningAptitude.LearningAptitudeName == learningAptitudeName
                                                                       && learningAptitude.SchoolId == school.SchoolId
                                                                       select learningAptitude;
            if (iqLearningAptitude.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Category_LearningAptitude LearningAptitudeMarkExists(double averageMark)
        {
            Category_LearningAptitude learningAptitude = null;
            IQueryable<Category_LearningAptitude> iqLearningAptitude = from l in db.Category_LearningAptitudes
                                                                       where l.SchoolId == school.SchoolId && l.BeginAverageMark <= averageMark
                                                                        && l.EndAverageMark >= averageMark
                                                                       select l;
            if (iqLearningAptitude.Count() != 0)
            {
                learningAptitude = iqLearningAptitude.First();
            }

            return learningAptitude;
        }
    }
}
