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

        public bool ExistsChildFunction(int parentsFunctionId)
        {
            IQueryable<UserManagement_Function> childFunctions;
            childFunctions = from function in db.UserManagement_Functions
                             where function.ParentFunctionId == parentsFunctionId
                             select function;
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

        public List<string> GetFunctionFlags(Guid roleId)
        {
            IQueryable<string> iqFunctionFlag = from rl in db.aspnet_Roles
                                                where rl.RoleId == roleId
                                                select rl.UserManagement_RoleDetail.UserManagement_RoleCategory.FunctionFlag;

            List<string> functionFlags = iqFunctionFlag.First().Split(',').ToList();
            return functionFlags;
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
    }
}
