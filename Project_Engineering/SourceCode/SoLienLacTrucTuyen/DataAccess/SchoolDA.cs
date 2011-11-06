using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class SchoolDA: BaseDA
    {
        public SchoolDA()
            : base()
        {
        }

        public List<School> GetSchools()
        {
            IQueryable<School> iqSchool = from school in db.Schools
                                          select school;
            if (iqSchool.Count() != 0)
            {
                return iqSchool.OrderBy(school => school.SchoolName).ToList();
            }
            else
            {
                return new List<School>();
            }
        }
    }
}
