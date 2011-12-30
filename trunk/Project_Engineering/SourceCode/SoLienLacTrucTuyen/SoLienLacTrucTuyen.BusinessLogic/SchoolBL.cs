using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class SchoolBL
    {
        SchoolDA schoolDA;

        public SchoolBL()
        {
            schoolDA = new SchoolDA();
        }

        public List<School_School> GetSchools()
        {
            return schoolDA.GetSchools();
        }

        public List<School_School> GetSchools(string schoolName, int pageIndex, int pageSize, out double totalRecords)
        {
            List<School_School> schools = new List<School_School>();

            if (CheckUntils.IsAllOrBlank(schoolName))
            {
                schools = schoolDA.GetSchools(pageIndex, pageSize, out totalRecords);
            }
            else
            {
                schools = schoolDA.GetSchools(schoolName, pageIndex, pageSize, out totalRecords);
            }

            return schools;
        }

        public void DeleteSchool(List<School_School> schools)
        {
            foreach (School_School school in schools)
            {
                schoolDA.DeleteSchool(school);
            }            
        }
    }
}
