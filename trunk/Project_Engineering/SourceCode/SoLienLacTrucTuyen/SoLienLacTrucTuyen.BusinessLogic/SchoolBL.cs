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
    }
}
