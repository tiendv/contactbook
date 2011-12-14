using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.DataAccess
{
    public class AttitudeDA : BaseDA
    {
        public AttitudeDA(School_School school)
            : base(school)
        {

        }

        public void InsertAttitude(Category_Attitude attitude)
        {
            attitude.SchoolId = school.SchoolId;
            db.Category_Attitudes.InsertOnSubmit(attitude);
            db.SubmitChanges();
        }

        public void UpdateAttitude(Category_Attitude editedAttitude)
        {
            Category_Attitude attitude = null;
            IQueryable<Category_Attitude> iqAttitude = from att in db.Category_Attitudes
                                                           where att.AttitudeId == editedAttitude.AttitudeId
                                                           && att.SchoolId == school.SchoolId
                                                           select att;
            if (iqAttitude.Count() != 0)
            {
                attitude = iqAttitude.First();
                attitude.AttitudeName = editedAttitude.AttitudeName;
                db.SubmitChanges();
            }
        }

        public void DeleteAttitude(Category_Attitude deletedAttitude)
        {
            Category_Attitude attitude = null;
            IQueryable<Category_Attitude> iqAttitude = from att in db.Category_Attitudes
                                                           where att.AttitudeId == deletedAttitude.AttitudeId
                                                           && att.SchoolId == school.SchoolId
                                                           select att;
            if (iqAttitude.Count() != 0)
            {
                attitude = iqAttitude.First();
                db.Category_Attitudes.DeleteOnSubmit(attitude);
                db.SubmitChanges();
            }
        }

        public Category_Attitude GetAttitude(int attitudeId)
        {
            Category_Attitude attitude = null;
            IQueryable<Category_Attitude> iqAttitude = from att in db.Category_Attitudes
                                                           where att.AttitudeId == attitudeId
                                                           && att.SchoolId == school.SchoolId
                                                           select att;
            if (iqAttitude.Count() != 0)
            {
                attitude = iqAttitude.First();
            }

            return attitude;
        }

        public List<Category_Attitude> GetAttitudes()
        {
            List<Category_Attitude> lAttitudes = new List<Category_Attitude>();

            IQueryable<Category_Attitude> iqAttitude = from att in db.Category_Attitudes
                                                           where att.SchoolId == school.SchoolId
                                                           select att;
            if (iqAttitude.Count() != 0)
            {
                lAttitudes = iqAttitude.OrderBy(att => att.AttitudeName).ToList();
            }

            return lAttitudes;
        }

        public List<Category_Attitude> GetAttitudes(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Category_Attitude> iqAttitude = from att in db.Category_Attitudes
                                                           where att.SchoolId == school.SchoolId
                                                           select att;
            return GetAttitudes(ref iqAttitude, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Category_Attitude> GetAttitudes(string attitudeName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Category_Attitude> iqAttitude = from att in db.Category_Attitudes
                                                           where att.AttitudeName == attitudeName
                                                           && att.SchoolId == school.SchoolId
                                                           select att;
            return GetAttitudes(ref iqAttitude, pageCurrentIndex, pageSize, out totalRecords);
        }

        public bool IsDeletable(Category_Attitude attitude)
        {
            IQueryable<Student_Activity> iqStudentActivity = from studAct in db.Student_Activities
                                                             where studAct.AttitudeId == attitude.AttitudeId
                                                             select studAct;
            if (iqStudentActivity.Count() != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool AttitudeNameExists(string attitudeName)
        {
            IQueryable<Category_Attitude> iqAttitude = from att in db.Category_Attitudes
                                                           where att.AttitudeName == attitudeName
                                                           && att.SchoolId == school.SchoolId
                                                           select att;
            if (iqAttitude.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private List<Category_Attitude> GetAttitudes(ref IQueryable<Category_Attitude> iqAttitude, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<Category_Attitude> lAttitudes = new List<Category_Attitude>();
            totalRecords = iqAttitude.Count();
            if (iqAttitude.Count() != 0)
            {
                lAttitudes = iqAttitude.OrderBy(att => att.AttitudeName)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return lAttitudes;
        }
    }
}
