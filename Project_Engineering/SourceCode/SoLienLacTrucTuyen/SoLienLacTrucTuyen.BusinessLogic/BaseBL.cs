using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class BaseBL
    {
        protected School school;

        public BaseBL(School school)
        {
            this.school = school;
        }

        public BaseBL()
        {
        }

        protected string GetActualName(String name)
        {
            string strActualName = school.SchoolId + "_" + name;
            return strActualName;
        }
    }
}
