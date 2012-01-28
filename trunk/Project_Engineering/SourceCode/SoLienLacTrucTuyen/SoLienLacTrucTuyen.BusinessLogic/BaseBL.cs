using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class BaseBL
    {
        protected const string STRING_UNDEFINED = "(Chưa xác định)";
        
        protected School_School school;

        public BaseBL(School_School school)
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
