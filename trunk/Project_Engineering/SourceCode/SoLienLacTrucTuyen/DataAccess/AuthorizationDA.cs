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

        public void AddServicesToParentsUser(UserManagement_Function functions)
        {
            throw new NotImplementedException();
        }

        public void AddServicesToParentsUser(aspnet_User userParents, List<UserManagement_Function> functions)
        {
            List<UserManagement_Authorization> parentsAuthorizations = null;
            aspnet_Role roleParents = GetRoleParents();            
            UserManagement_RoleParentsAuthorization roleParentsAuthorization = null;
            IQueryable<UserManagement_Authorization> iqRoleParentsAuthorizations;
            List<int> functionIds;            

            iqRoleParentsAuthorizations = from authorization in db.UserManagement_Authorizations
                                          where authorization.RoleId == roleParents.RoleId
                                          select authorization;

            if (iqRoleParentsAuthorizations.Count() != 0)
            {
                parentsAuthorizations = iqRoleParentsAuthorizations.ToList();
                functionIds = new List<int>();
                foreach (UserManagement_Function function in functions)
                {
                    functionIds.Add(function.FunctionId);
                }

                int i = 0;
                while (i < parentsAuthorizations.Count)
                {
                    if (!functionIds.Contains(parentsAuthorizations[i].UserManagement_AuthorizedPage.FunctionId))
                    {
                        parentsAuthorizations.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }

                userParents = (from user in db.aspnet_Users
                               where user.UserName == userParents.UserName
                               select user).First();

                foreach (UserManagement_Authorization authorization in parentsAuthorizations)
                {
                    roleParentsAuthorization = new UserManagement_RoleParentsAuthorization();
                    roleParentsAuthorization.RoleParentAuthorizationId = authorization.AuthorizationId;
                    roleParentsAuthorization.UserId = userParents.UserId;

                    db.UserManagement_RoleParentsAuthorizations.InsertOnSubmit(roleParentsAuthorization);
                }
                db.SubmitChanges();
            }
        }

        public aspnet_Role GetRoleParents()
        {
            aspnet_Role roleParents = null;
            IQueryable<aspnet_Role> iqRoleParent = from role in db.aspnet_Roles
                                                   where role.UserManagement_RoleDetail.SchoolId == school.SchoolId
                                                   && role.UserManagement_RoleDetail.UserManagement_RoleCategory.RoleCategoryId == PARENTS
                                                   select role;
            if (iqRoleParent.Count() != 0)
            {
                roleParents = iqRoleParent.First();
            }

            return roleParents;
        }
    }
}
