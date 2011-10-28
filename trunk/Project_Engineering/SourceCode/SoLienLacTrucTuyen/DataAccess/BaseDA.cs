using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class BaseDA
    {
        protected DbEContactBookDataContext db;

        public BaseDA()
        {
            db = new DbEContactBookDataContext();
        }        
    }
}
