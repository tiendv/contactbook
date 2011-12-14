using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace EContactBook.DataAccess
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

    public class AuthorizationDA : BaseDA
    {
        internal const string FUNCTIONCATEGOTY_HOMEPAGE = "Trang Chủ";
        internal const string FUNCTIONCATEGOTY_USER = "Người Dùng";
        internal const string FUNCTIONCATEGOTY_CATEGORY = "Danh Mục";
        internal const string FUNCTIONCATEGOTY_CLASS = "Lớp Học";
        internal const string FUNCTIONCATEGOTY_STUDENT = "Học Sinh";
        internal const string FUNCTIONCATEGOTY_AGENTNEW = "Lời Nhắn Khẩn";
        internal const string FUNCTIONCATEGOTY_COMMENT = "Góp Ý";

        public const string FUNCTIONFLAG_OTHERS = "OTHERS";

        public AuthorizationDA(School_School school)
            : base(school)
        {
        }

        public void InsertAuthorizations(aspnet_Role role, List<UserManagement_Function> functions)
        {
            UserManagement_Authorization authorization = null;
            foreach (UserManagement_Function function in functions)
            {
                IQueryable<UserManagement_AuthorizedPage> iqAuthorizedPage = from authorizedPage in db.UserManagement_AuthorizedPages
                                                                             where authorizedPage.FunctionId == function.FunctionId
                                                                             select authorizedPage;
                if (iqAuthorizedPage.Count() != 0)
                {
                    foreach (UserManagement_AuthorizedPage authorizedPage in iqAuthorizedPage)
                    {
                        authorization = new UserManagement_Authorization();
                        authorization.AuthorizedPagePathId = authorizedPage.AuthorizedPageId;
                        authorization.RoleId = role.RoleId;
                        if (authorizedPage.AuthorizedPageId == 1)
                        {
                            authorization.IsActivated = true;
                        }
                        else
                        {
                            authorization.IsActivated = false;
                        }
                        db.UserManagement_Authorizations.InsertOnSubmit(authorization);
                    }
                }
            }
            db.SubmitChanges();
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
                    roleParentsAuthorization.UserParentId = userParents.UserId;

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

        public void Authorize(aspnet_Role role, List<UserManagement_AuthorizedPage> lstAuthorizedPages)
        {
            IQueryable<UserManagement_Authorization> deletingAuthorizations;
            deletingAuthorizations = from authorization in db.UserManagement_Authorizations
                                     where authorization.RoleId == role.RoleId
                                     select authorization;
            foreach (UserManagement_Authorization authorization in deletingAuthorizations)
            {
                db.UserManagement_Authorizations.DeleteOnSubmit(authorization);
            }
            db.SubmitChanges();

            foreach (UserManagement_AuthorizedPage authorizedPage in lstAuthorizedPages)
            {
                db.UserManagement_Authorizations.InsertOnSubmit(
                    new UserManagement_Authorization()
                    {
                        AuthorizedPagePathId = authorizedPage.AuthorizedPageId,
                        RoleId = role.RoleId
                    });
            }
            db.SubmitChanges();
        }

        public List<UserManagement_AuthorizedPage> GetListAuthorizedPages(int functionId, int accessibilityId)
        {
            IQueryable<UserManagement_AuthorizedPage> authorizedPages;
            authorizedPages = from authorizedPage in db.UserManagement_AuthorizedPages
                              where authorizedPage.FunctionId == functionId
                                && authorizedPage.AccessibilityId == accessibilityId
                              select authorizedPage;
            if (authorizedPages.Count() != 0)
            {
                return authorizedPages.ToList();
            }
            else
            {
                return new List<UserManagement_AuthorizedPage>();
            }
        }

        public List<TabularAuthorization> GetTabularAuthorizations(aspnet_Role role)
        {
            List<TabularAuthorization> tabularAuthorizations = new List<TabularAuthorization>();
            TabularAuthorization tabularAuthorization = null;
            FunctionsDA functionDA = new FunctionsDA();
            List<string> functionFlags = functionDA.GetFunctionFlags(role);

            // Get list of Function Category
            IQueryable<string> functionCatagories = from function in db.UserManagement_Functions
                                                    where (functionFlags.Contains(function.FunctionFlag) == true)
                                                    group function by function.FunctionCategory into g
                                                    select g.Key;

            // Loop through function categories
            foreach (string functionCatagory in functionCatagories)
            {
                tabularAuthorization = new TabularAuthorization();

                // Get list of function-category-based function
                IQueryable<UserManagement_Function> iqCategoryBasedFunctions;
                iqCategoryBasedFunctions = from function in db.UserManagement_Functions
                                           where function.FunctionCategory == functionCatagory && (functionFlags.Contains(function.FunctionFlag) == true)
                                           select function;

                List<TabularDetailedAuthorization> tabularDetailedAuthorizations = new List<TabularDetailedAuthorization>();

                // Loop through function-category-based functions
                foreach (UserManagement_Function catagoryBasedFunction in iqCategoryBasedFunctions)
                {
                    if (functionDA.ExistsChildFunction(catagoryBasedFunction))
                    {
                        continue;
                    }

                    // Init TabularDetailedAuthorization
                    TabularDetailedAuthorization detailedAuthorization = new TabularDetailedAuthorization()
                    {
                        FunctionId = catagoryBasedFunction.FunctionId,
                        FunctionName = catagoryBasedFunction.FunctionName,
                        ViewAccessibility = false,
                        AddAccessibility = false,
                        ModifyAccessibility = false,
                        DeleteAccessibility = false,
                        ViewDisplay = false,
                        AddDisplay = false,
                        ModifyDisplay = false,
                        DeleteDisplay = false
                    };

                    IQueryable<int> iqFunctionBasedAccessibilityIds;
                    iqFunctionBasedAccessibilityIds = from authorizedPage in db.UserManagement_AuthorizedPages
                                                      where authorizedPage.FunctionId == catagoryBasedFunction.FunctionId
                                                      select authorizedPage.AccessibilityId;
                    if (iqFunctionBasedAccessibilityIds.Count() != 0)
                    {
                        iqFunctionBasedAccessibilityIds = iqFunctionBasedAccessibilityIds.Distinct();
                    }

                    foreach (int functionBasedAccessibilityId in iqFunctionBasedAccessibilityIds)
                    {
                        switch (functionBasedAccessibilityId)
                        {
                            case (int)AccessibilityEnum.View:
                                {
                                    detailedAuthorization.ViewDisplay = true;
                                    break;
                                }
                            case (int)AccessibilityEnum.Add:
                                {
                                    detailedAuthorization.AddDisplay = true;
                                    break;
                                }
                            case (int)AccessibilityEnum.Modify:
                                {
                                    detailedAuthorization.ModifyDisplay = true;
                                    break;
                                }
                            case (int)AccessibilityEnum.Delete:
                                {
                                    detailedAuthorization.DeleteDisplay = true;
                                    break;
                                }
                        }
                    }

                    IQueryable<int> iqRoledFunctionBaseAccessibilyIds;
                    iqRoledFunctionBaseAccessibilyIds = from authorizedPage in db.UserManagement_AuthorizedPages
                                                        join authorization in db.UserManagement_Authorizations
                                                            on authorizedPage.AuthorizedPageId equals authorization.AuthorizedPagePathId
                                                        where authorizedPage.FunctionId == catagoryBasedFunction.FunctionId
                                                            && authorization.IsActivated == true
                                                            && authorization.RoleId == role.RoleId
                                                        select authorizedPage.AccessibilityId;
                    if (iqRoledFunctionBaseAccessibilyIds.Count() != 0)
                    {
                        iqRoledFunctionBaseAccessibilyIds = iqRoledFunctionBaseAccessibilyIds.Distinct();
                        if (iqRoledFunctionBaseAccessibilyIds.Contains((int)AccessibilityEnum.View))
                        {
                            detailedAuthorization.ViewAccessibility = true;
                        }

                        if (iqRoledFunctionBaseAccessibilyIds.Contains((int)AccessibilityEnum.Add))
                        {
                            detailedAuthorization.AddAccessibility = true;
                        }

                        if (iqRoledFunctionBaseAccessibilyIds.Contains((int)AccessibilityEnum.Modify))
                        {
                            detailedAuthorization.ModifyAccessibility = true;
                        }

                        if (iqRoledFunctionBaseAccessibilyIds.Contains((int)AccessibilityEnum.Delete))
                        {
                            detailedAuthorization.DeleteAccessibility = true;
                        }
                    }

                    tabularDetailedAuthorizations.Add(detailedAuthorization);
                }

                if (tabularDetailedAuthorizations.Count != 0)
                {
                    tabularAuthorization.FunctionCategoryName = functionCatagory;
                    tabularAuthorization.detailedAuthorizations = tabularDetailedAuthorizations;
                    tabularAuthorizations.Add(tabularAuthorization);
                }
            }

            return tabularAuthorizations;
        }

        public void UpdateAuthorization(aspnet_Role role, UserManagement_Function function, AccessibilityEnum accessibility, bool IsActivated)
        {
            IQueryable<UserManagement_Authorization> iqAuthorization = from authorization in db.UserManagement_Authorizations
                                                                       where authorization.RoleId == role.RoleId
                                                                        && authorization.UserManagement_AuthorizedPage.FunctionId == function.FunctionId
                                                                        && authorization.UserManagement_AuthorizedPage.AccessibilityId == (int)accessibility
                                                                       select authorization;
            if (iqAuthorization.Count() != 0)
            {
                foreach (UserManagement_Authorization authorization in iqAuthorization)
                {
                    authorization.IsActivated = IsActivated;
                }

                db.SubmitChanges();
            }
        }

        public void UpdateRoleParentsAuthorizations(UserManagement_Function function, AccessibilityEnum accessibility, bool IsActivated)
        {
            IQueryable<UserManagement_RoleParentsAuthorization> iqAuthorization;
            iqAuthorization = from authorization in db.UserManagement_RoleParentsAuthorizations
                              where authorization.IsRegistered == true
                               && authorization.UserManagement_Authorization.UserManagement_AuthorizedPage.FunctionId == function.FunctionId
                               && authorization.UserManagement_Authorization.UserManagement_AuthorizedPage.AccessibilityId == (int)accessibility
                              select authorization;

            if (iqAuthorization.Count() != 0)
            {
                foreach (UserManagement_RoleParentsAuthorization authorization in iqAuthorization)
                {
                    authorization.IsActivated = IsActivated;
                }

                db.SubmitChanges();
            }
        }

        public List<UserManagement_Authorization> GetAuthorizations(aspnet_Role role, bool isActivated)
        {
            List<UserManagement_Authorization> authorizations = new List<UserManagement_Authorization>();
            IQueryable<UserManagement_Authorization> iqAuthorization = from authorization in db.UserManagement_Authorizations
                                                                       where authorization.RoleId == role.RoleId && authorization.IsActivated == isActivated
                                                                       select authorization;
            if (iqAuthorization.Count() != 0)
            {
                authorizations = iqAuthorization.ToList();
            }

            return authorizations;
        }

        public void InsertRoleParentsAuthorization(List<UserManagement_RoleParentsAuthorization> roleParentsAuthorizations)
        {
            foreach (UserManagement_RoleParentsAuthorization roleParentsAuthorization in roleParentsAuthorizations)
            {
                db.UserManagement_RoleParentsAuthorizations.InsertOnSubmit(roleParentsAuthorization);
            }
            db.SubmitChanges();
        }

        public void DeleteAuthorization(string userName)
        {
            IQueryable<UserManagement_RoleParentsAuthorization> iqAuthorization;
            iqAuthorization = from authorization in db.UserManagement_RoleParentsAuthorizations
                              where authorization.aspnet_User.UserName == userName
                              select authorization;
            if (iqAuthorization.Count() != 0)
            {
                foreach (UserManagement_RoleParentsAuthorization authorization in iqAuthorization)
                {
                    db.UserManagement_RoleParentsAuthorizations.DeleteOnSubmit(authorization);
                }
                db.SubmitChanges();
            }
        }
    }
}
