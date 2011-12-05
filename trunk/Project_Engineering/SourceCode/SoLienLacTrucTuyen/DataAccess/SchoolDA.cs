using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
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
    }
}
