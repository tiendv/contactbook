using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public enum FunctionFlag
    {
        ADMINONLY,
        PARENTSONLY,
        SUBJECTBASETEACHERS,
        MANAGEBASETEACHERS,
        ALLROLES,
        OTHERS,
    }

    public class AuthorizationDA: BaseDA
    {
        internal const string FUNCTIONCATEGOTY_HOMEPAGE = "Trang Chủ";
        internal const string FUNCTIONCATEGOTY_USER = "Người Dùng";
        internal const string FUNCTIONCATEGOTY_CATEGORY = "Danh Mục";
        internal const string FUNCTIONCATEGOTY_CLASS = "Lớp Học";
        internal const string FUNCTIONCATEGOTY_STUDENT = "Học Sinh";
        internal const string FUNCTIONCATEGOTY_AGENTNEW = "Lời Nhắn Khẩn";
        internal const string FUNCTIONCATEGOTY_COMMENT = "Góp Ý";

        public const string FUNCTIONFLAG_OTHERS = "OTHERS";

        public AuthorizationDA(School school)
            : base(school)
        {
        }

        public List<UserManagement_Function> GetStudentFunctions(string functionFlag)  
        {
            List<UserManagement_Function> studentFunctions = new List<UserManagement_Function>();

            IQueryable<UserManagement_Function> iqFunction = from func in db.UserManagement_Functions
                                                             where func.FunctionFlag == functionFlag
                                                             && func.FunctionCategory == FUNCTIONCATEGOTY_STUDENT
                                                             select func;

            if (iqFunction.Count() != 0)
            {
                studentFunctions = iqFunction.ToList();
            }

            return studentFunctions;
        }

        //public List<UserManagement_PagePath> GetStudentPages(aspnet_Role role)
        //{

        //}
    }
}
