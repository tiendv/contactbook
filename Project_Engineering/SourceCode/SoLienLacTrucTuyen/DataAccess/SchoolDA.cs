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

        public void InsertSchool(School_School school)
        {
            db.School_Schools.InsertOnSubmit(school);
            db.SubmitChanges();
        }

        public School_School GetLastedInsertedSchool()
        {
            School_School lastedInsertedSchool = null;

            IQueryable<School_School> iqSchool = from school in db.School_Schools
                                                 select school;
            if (iqSchool.Count() != 0)
            {
                lastedInsertedSchool = iqSchool.OrderByDescending(school => school.SchoolId).First();
            }

            return lastedInsertedSchool;
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
            IQueryable<School_School> iqSchool = from school in db.School_Schools
                                                 where school.SchoolId != 0
                                                 select school;
            
            return GetSchools(ref iqSchool, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<School_School> GetSchools(string schoolName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<School_School> iqSchool = from school in db.School_Schools
                                                 where school.SchoolId != 0 && school.SchoolName == schoolName
                                                 select school;

            return GetSchools(ref iqSchool, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<School_School> GetSchools(ConfigurationDistrict district, int pageCurrentIndex, int pageSize, out double totalRecords)
        {            
            IQueryable<School_School> iqSchool = from school in db.School_Schools
                                                 where school.SchoolId != 0 && school.DistrictId == district.DistrictId
                                                 select school;

            return GetSchools(ref iqSchool, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<School_School> GetSchools(ConfigurationDistrict district, string schoolName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
           IQueryable<School_School> iqSchool = from school in db.School_Schools
                                                 where school.SchoolId != 0 && school.DistrictId == district.DistrictId
                                                 && school.SchoolName == schoolName
                                                 select school;

           return GetSchools(ref iqSchool, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<School_School> GetSchools(ConfigurationProvince province, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<School_School> iqSchool = from school in db.School_Schools
                                                 join district in db.ConfigurationDistricts on school.DistrictId equals district.DistrictId
                                                 where school.SchoolId != 0 && district.ProvinceId == province.ProvinceId
                                                 select school;

            return GetSchools(ref iqSchool, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<School_School> GetSchools(ConfigurationProvince province, string schoolName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<School_School> iqSchool = from school in db.School_Schools
                                                 join district in db.ConfigurationDistricts on school.DistrictId equals district.DistrictId
                                                 where school.SchoolId != 0 && district.ProvinceId == province.ProvinceId
                                                 && school.SchoolName == schoolName
                                                 select school;

            return GetSchools(ref iqSchool, pageCurrentIndex, pageSize, out totalRecords);
        }

        private List<School_School> GetSchools(ref IQueryable<School_School> iqSchool, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<School_School> schools = new List<School_School>();

            totalRecords = iqSchool.Count();
            if (totalRecords != 0)
            {
                schools = iqSchool.OrderBy(school => school.SchoolName)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return schools;
        }
    }
}
