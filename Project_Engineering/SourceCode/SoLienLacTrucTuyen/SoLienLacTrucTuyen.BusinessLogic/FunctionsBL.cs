using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class FunctionsBL
    {
        private FunctionsDA functionDA;

        public FunctionsBL()
        {
            functionDA = new FunctionsDA();
        }

        public bool ExistsChildFunction(int parentsFunctionId)
        {
            return functionDA.ExistsChildFunction(parentsFunctionId);
        }

        public int? GetParentFunctionId(int functionId)
        {
            return functionDA.GetParentFunctionId(functionId);
        }

        public bool ExistAuthorization(int functionId, int accessibilityId)
        {
            return functionDA.ExistAuthorization(functionId, accessibilityId);
        }

        public List<string> GetFunctionFlags(Guid roleId)
        {
            return functionDA.GetFunctionFlags(roleId);
        }

        public string GetHomePageFunctionCategory()
        {
            return functionDA.GetHomePageFunctionCategory();
        }

        public List<string> GetAdminOnlyFunctionCategories()
        {
            return functionDA.GetAdminOnlyFunctionCategories();
        }
    }
}
