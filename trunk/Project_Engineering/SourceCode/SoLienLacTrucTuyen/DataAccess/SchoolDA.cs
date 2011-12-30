using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.DataAccess
{
    public class SchoolDA : BaseDA
    {
        public SchoolDA()
        {
        }

        public List<School_School> GetSchools()
        {
            IQueryable<School_School> iqSchool = from school in db.School_Schools
                                          select school;
            if (iqSchool.Count() != 0)
            {
                return iqSchool.OrderBy(school => school.SchoolName).ToList();
            }
            else
            {
                return new List<School_School>();
            }
        }

        public List<School_School> GetSchools(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<School_School> schools = new List<School_School>();

            IQueryable<School_School> iqSchool = from school in db.School_Schools
                                                 where school.SchoolId != 0
                                                 select school;
            totalRecords = iqSchool.Count();
            if (totalRecords != 0)
            {
                schools = iqSchool.OrderBy(school => school.SchoolName)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return schools;
        }

        public List<School_School> GetSchools(string schoolName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<School_School> schools = new List<School_School>();

            IQueryable<School_School> iqSchool = from school in db.School_Schools
                                                 where school.SchoolId != 0 && school.SchoolName == schoolName
                                                 select school;
            totalRecords = iqSchool.Count();
            if (totalRecords != 0)
            {
                schools = iqSchool.OrderBy(school => school.SchoolName)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return schools;
        }

        public void DeleteSchool(School_School deletedSchool)
        {
            IQueryable<School_School> iqSchool = from school in db.School_Schools
                                                 where school.SchoolId == deletedSchool.SchoolId
                                                 select school;
            
            if (iqSchool.Count() != 0)
            {
                deletedSchool = iqSchool.First();
                db.School_Schools.DeleteOnSubmit(deletedSchool);
                db.SubmitChanges();
            }
        }
    }
}
