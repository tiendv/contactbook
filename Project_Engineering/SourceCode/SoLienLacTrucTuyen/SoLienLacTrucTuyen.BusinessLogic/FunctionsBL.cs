using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class FunctionsBL
    {
        private FunctionsDA functionDA;

        public FunctionsBL()
        {
            functionDA = new FunctionsDA();
        }

        public bool ExistsChildFunction(UserManagement_Function function)
        {
            return functionDA.ExistsChildFunction(function);
        }

        public int? GetParentFunctionId(int functionId)
        {
            return functionDA.GetParentFunctionId(functionId);
        }

        public UserManagement_Function GetParentFunction(UserManagement_Function function)
        {
            return functionDA.GetParentFunction(function);
        }

        public List<UserManagement_Function> GetFunctions(string functionFlag)
        {
            return functionDA.GetFlagedFunctions(functionFlag);
        }

        public bool ExistAuthorization(int functionId, int accessibilityId)
        {
            return functionDA.ExistAuthorization(functionId, accessibilityId);
        }

        public List<string> GetFunctionFlags(aspnet_Role role)
        {
            return functionDA.GetFunctionFlags(role);
        }

        public string GetHomePageFunctionCategory()
        {
            return functionDA.GetHomePageFunctionCategory();
        }

        public UserManagement_Function GetHomePageFunction()
        {
            return functionDA.GetHomePageFunction();
        }

        public List<string> GetAdminOnlyFunctionCategories()
        {
            return functionDA.GetAdminOnlyFunctionCategories();
        }
    }
}
