using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EContactBook.DataAccess;

namespace SoLienLacTrucTuyen_WebRole
{
    public class PermanentSession
    {
        public aspnet_User LogedInUser { get; set; }

        public List<aspnet_Role> LogedInRoles { get; set; }

        public bool IsRoleSubjectTeacher { get; set; }

        public bool IsRoleFormerTeacher { get; set; }

        public School_School School { get; set; }

        public Configuration_Year LastedYear { get; set; }

        public aspnet_Role RoleSubjectTeacher { get; set; }

        public aspnet_Role RoleFormerTeacher { get; set; }
    }
}