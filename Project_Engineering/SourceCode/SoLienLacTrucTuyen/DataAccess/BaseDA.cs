﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class BaseDA
    {
        protected DbEContactBookDataContext db;
        protected School school;

        protected const string TEACHER = "TEACHER";
        protected const string SUBJECTTEACHER = "SUBJECTTEACHER";
        protected const string FORMERTEACHER = "FORMERTEACHER";
        protected const string ADMIN = "ADMIN";
        protected const string PARENTS = "PARENTS";
        protected const string USERDEFINED = "USERDEFINED";        

        public BaseDA(School school)
        {
            this.school = school;

            db = new DbEContactBookDataContext();
        }

        public BaseDA()
        {
            db = new DbEContactBookDataContext();
        }
    }
}
