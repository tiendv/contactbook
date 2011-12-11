using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class FunctionsDA : BaseDA
    {
        public FunctionsDA()
            : base()
        {
        }

        public bool ExistsChildFunction(UserManagement_Function function)
        {
            IQueryable<UserManagement_Function> childFunctions;
            childFunctions = from func in db.UserManagement_Functions
                             where func.ParentFunctionId == function.FunctionId
                             select func;
            if (childFunctions.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int? GetParentFunctionId(int functionId)
        {
            IQueryable<int> parentsFunctionId;
            parentsFunctionId = from function in db.UserManagement_Functions
                                where function.FunctionId == functionId
                                    && function.ParentFunctionId != null
                                select (int)function.ParentFunctionId;
            if (parentsFunctionId.Count() != 0)
            {
                return parentsFunctionId.First();
            }
            else
            {
                return null;
            }
        }

        public UserManagement_Function GetParentFunction(UserManagement_Function function)
        {
            UserManagement_Function parentFunction = null;

            IQueryable<int> iqParentsFunctionId;
            iqParentsFunctionId = from func in db.UserManagement_Functions
                                  where func.FunctionId == function.FunctionId && func.ParentFunctionId != null
                                  select (int)func.ParentFunctionId;
            if (iqParentsFunctionId.Count() != 0)
            {
                IQueryable<UserManagement_Function> iqParentsFunction;
                iqParentsFunction = from func in db.UserManagement_Functions
                                    where func.FunctionId == iqParentsFunctionId.First()
                                    select func;
                if (iqParentsFunction.Count() != 0)
                {
                    parentFunction = iqParentsFunction.First();
                }
            }

            return parentFunction;
        }

        public bool ExistAuthorization(int functionId, int accessibilityId)
        {
            IQueryable<UserManagement_Authorization> authorizations;
            authorizations = from authorization in db.UserManagement_Authorizations
                             join authorizedPage in db.UserManagement_AuthorizedPages
                                on authorization.AuthorizedPagePathId equals authorizedPage.AuthorizedPageId
                             where authorizedPage.FunctionId == functionId
                                && authorizedPage.AccessibilityId == accessibilityId
                             select authorization;
            if (authorizations.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<string> GetFunctionFlags(aspnet_Role role)
        {
            IQueryable<string> iqFunctionFlag = from rl in db.aspnet_Roles
                                                where rl.RoleId == role.RoleId
                                                select rl.UserManagement_RoleDetail.UserManagement_RoleCategory.FunctionFlag;

            List<string> functionFlags = iqFunctionFlag.First().Split(',').ToList();
            return functionFlags;
        }

        public UserManagement_Function GetHomePageFunction()
        {
            int homePageFunctionId = (from param in db.System_Parameters
                                      select param.HomePageFunctionId).First();
            UserManagement_Function homePageFunction = (from function in db.UserManagement_Functions
                                                        where function.FunctionId == homePageFunctionId
                                                        select function).First();
            return homePageFunction;
        }

        public string GetHomePageFunctionCategory()
        {
            int homePageFunctionId = (from param in db.System_Parameters
                                      select param.HomePageFunctionId).First();
            string homePageFunctionCategory = (from function in db.UserManagement_Functions
                                               where function.FunctionId == homePageFunctionId
                                               select function.FunctionCategory).First();
            return homePageFunctionCategory;
        }

        public List<string> GetAdminOnlyFunctionCategories()
        {
            string flag_ADMINONLY = FunctionFlag.GetName(typeof(FunctionFlag), FunctionFlag.ADMINONLY);

            IQueryable<string> functionCategories = from function in db.UserManagement_Functions
                                                    where function.FunctionFlag == flag_ADMINONLY
                                                    select function.FunctionCategory;
            if (functionCategories.Count() != 0)
            {
                return functionCategories.Distinct().ToList();
            }
            else
            {
                return new List<string>();
            }
        }

        internal List<UserManagement_Function> GetFunctions(aspnet_Role role)
        {
            List<UserManagement_Function> roleBasedFunctions = new List<UserManagement_Function>();
            IQueryable<UserManagement_Function> iqRoleBasedFunction = from authorization in db.UserManagement_Authorizations
                                                                      where authorization.RoleId == role.RoleId && authorization.IsActivated == true
                                                                      select authorization.UserManagement_AuthorizedPage.UserManagement_Function;
            if (iqRoleBasedFunction.Count() != 0)
            {
                roleBasedFunctions = iqRoleBasedFunction.Distinct().ToList();
            }

            return roleBasedFunctions;
        }

        internal List<UserManagement_Function> GetFunctions(string userName)
        {
            List<UserManagement_Function> roleBasedFunctions = new List<UserManagement_Function>();
            IQueryable<UserManagement_Function> iqRoleBasedFunction = from authorization in db.UserManagement_RoleParentsAuthorizations
                                                                      where authorization.aspnet_User.UserName == userName && authorization.IsActivated == true
                                                                      select authorization.UserManagement_Authorization.UserManagement_AuthorizedPage.UserManagement_Function;
            if (iqRoleBasedFunction.Count() != 0)
            {
                roleBasedFunctions = iqRoleBasedFunction.Distinct().ToList();
            }

            return roleBasedFunctions;
        }

        public List<UserManagement_Function> GetFlagedFunctions(string functionFlag)
        {
            List<UserManagement_Function> flagBasedFunctions = new List<UserManagement_Function>();
            IQueryable<UserManagement_Function> iqFlagBasedFunction = from function in db.UserManagement_Functions
                                                                      where function.FunctionFlag == functionFlag
                                                                      select function;
            if (iqFlagBasedFunction.Count() != 0)
            {
                flagBasedFunctions = iqFlagBasedFunction.Distinct().ToList();
            }

            return flagBasedFunctions;
        }
    }
}
