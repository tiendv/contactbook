using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.BusinessEntity;

namespace EContactBook.DataAccess
{
    public class TeachingPeriodDA : BaseDA
    {
        public TeachingPeriodDA(School_School school)
            : base(school)
        {
        }

        public void DeleteTeachingPeriod(Category_TeachingPeriod deleteTeachingPeriod)
        {
            Category_TeachingPeriod teachingPeriod = null;

            IQueryable<Category_TeachingPeriod> iTeachingPeriod = from tchPeriod in db.Category_TeachingPeriods
                                                                  where tchPeriod.TeachingPeriodId == deleteTeachingPeriod.TeachingPeriodId
                                                                  select tchPeriod;
            if (iTeachingPeriod.Count() != 0)
            {
                teachingPeriod = iTeachingPeriod.First();
                db.Category_TeachingPeriods.DeleteOnSubmit(teachingPeriod);
                db.SubmitChanges();
            }
        }

        public void InsertTeachingPeriod(Category_TeachingPeriod newTeachingPeriod)
        {
            newTeachingPeriod.SchoolId = school.SchoolId;
            db.Category_TeachingPeriods.InsertOnSubmit(newTeachingPeriod);
            db.SubmitChanges();
        }

        public void UpdateTeachingPeriod(Category_TeachingPeriod editedTeachingPeriod)
        {
            Category_TeachingPeriod teachingPeriod = null;

            IQueryable<Category_TeachingPeriod> iTeachingPeriod = from tchPeriod in db.Category_TeachingPeriods
                                                                  where tchPeriod.TeachingPeriodId == editedTeachingPeriod.TeachingPeriodId
                                                                  select tchPeriod;
            if (iTeachingPeriod.Count() != 0)
            {
                teachingPeriod = iTeachingPeriod.First();
                teachingPeriod.TeachingPeriodName = editedTeachingPeriod.TeachingPeriodName;
                teachingPeriod.SessionId = editedTeachingPeriod.SessionId;
                teachingPeriod.TeachingPeriodOrder = editedTeachingPeriod.TeachingPeriodOrder;
                teachingPeriod.BeginTime = editedTeachingPeriod.BeginTime;
                teachingPeriod.EndTime = editedTeachingPeriod.EndTime;

                db.SubmitChanges();
            }
        }

        public Category_TeachingPeriod GetTeachingPeriod(int teachingPeriodId)
        {
            Category_TeachingPeriod teachingPeriod = null;

            IQueryable<Category_TeachingPeriod> iTeachingPeriod = from tchPeriod in db.Category_TeachingPeriods
                                                                  where tchPeriod.TeachingPeriodId == teachingPeriodId
                                                                  select tchPeriod;
            if (iTeachingPeriod.Count() != 0)
            {
                teachingPeriod = iTeachingPeriod.First();
            }

            return teachingPeriod;
        }

        public List<Category_TeachingPeriod> GetTeachingPeriods()
        {
            List<Category_TeachingPeriod> lTeachingPeriods = new List<Category_TeachingPeriod>();

            IQueryable<Category_TeachingPeriod> iTeachingPeriod = from tchPeriod in db.Category_TeachingPeriods
                                                                  where tchPeriod.SchoolId == school.SchoolId
                                                                  select tchPeriod;
            if (iTeachingPeriod.Count() != 0)
            {
                lTeachingPeriods = iTeachingPeriod.OrderBy(tchPeriod => tchPeriod.BeginTime).ThenBy(tchPeriod => tchPeriod.TeachingPeriodOrder).ToList();
            }

            return lTeachingPeriods;
        }

        public List<Category_TeachingPeriod> GetTeachingPeriods(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Category_TeachingPeriod> iqTeachingPeriod = from tchPeriod in db.Category_TeachingPeriods
                                                                   where tchPeriod.SchoolId == school.SchoolId
                                                                   select tchPeriod;
            return GetTeachingPeriods(ref iqTeachingPeriod, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Category_TeachingPeriod> GetTeachingPeriods(string teachingPeriodName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Category_TeachingPeriod> iqTeachingPeriod = from tchPeriod in db.Category_TeachingPeriods
                                                                   where tchPeriod.TeachingPeriodName == teachingPeriodName
                                                                   && tchPeriod.SchoolId == school.SchoolId
                                                                   select tchPeriod;

            return GetTeachingPeriods(ref iqTeachingPeriod, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Category_TeachingPeriod> GetTeachingPeriods(Configuration_Session session, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Category_TeachingPeriod> iqTeachingPeriod = from tchPeriod in db.Category_TeachingPeriods
                                                                   where tchPeriod.SessionId == session.SessionId
                                                                        && tchPeriod.SchoolId == school.SchoolId
                                                                   select tchPeriod;

            return GetTeachingPeriods(ref iqTeachingPeriod, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Category_TeachingPeriod> GetTeachingPeriods(string teachingPeriodName, Configuration_Session session, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Category_TeachingPeriod> iqTeachingPeriod = from tchPeriod in db.Category_TeachingPeriods
                                                                   where tchPeriod.TeachingPeriodName == teachingPeriodName
                                                                       && tchPeriod.SessionId == session.SessionId
                                                                       && tchPeriod.SchoolId == school.SchoolId
                                                                   select tchPeriod;

            return GetTeachingPeriods(ref iqTeachingPeriod, pageCurrentIndex, pageSize, out totalRecords);
        }

        public bool TeachingPeriodNameExists(string teachingPeriodName)
        {
            IQueryable<Category_TeachingPeriod> iqTeachingPeriod = from tchPeriod in db.Category_TeachingPeriods
                                                                   where tchPeriod.TeachingPeriodName == teachingPeriodName
                                                                   && tchPeriod.SchoolId == school.SchoolId
                                                                   select tchPeriod;
            if (iqTeachingPeriod.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private List<Category_TeachingPeriod> GetTeachingPeriods(ref IQueryable<Category_TeachingPeriod> iqTeachingPeriod, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<Category_TeachingPeriod> teachingPeriods = new List<Category_TeachingPeriod>();

            totalRecords = iqTeachingPeriod.Count();
            if (totalRecords != 0)
            {
                teachingPeriods = iqTeachingPeriod.OrderBy(tchPeriod => tchPeriod.BeginTime)
                    .ThenBy(tiet => tiet.TeachingPeriodOrder).Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return teachingPeriods;
        }

        public int GetTeachingPeriodCount()
        {
            IQueryable<Category_TeachingPeriod> iqTeachingPeriod = from t in db.Category_TeachingPeriods
                                                                   where t.SchoolId == school.SchoolId
                                                                   select t;

            return iqTeachingPeriod.Count();
        }
    }
}
