using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class PhanQuyenDA : BaseDA
    {
        public PhanQuyenDA(School_School school)
            : base(school)
        {
        }

        public List<TabularPhanQuyen> GetListPhanQuyens(Guid roleId)
        {
            FunctionsDA functionDA = new FunctionsDA();

            // List TabularPhanQuyen for UI
            List<TabularPhanQuyen> lstTbPhanQuyens = new List<TabularPhanQuyen>();

            List<string> lstFunctionFlagStrings = functionDA.GetFunctionFlags(roleId);

            // Get list of Function Category
            IQueryable<string> functionCatagories = from function in db.UserManagement_Functions
                                                    where (lstFunctionFlagStrings.Contains(function.FunctionFlag) == true)
                                                    group function by function.FunctionCategory into g
                                                    select g.Key;

            // Loop through function categories
            foreach (string functionCatagory in functionCatagories)
            {
                TabularPhanQuyen tbPhanQuyen = new TabularPhanQuyen();

                // Get list of function-category-based function
                IQueryable<UserManagement_Function> catagoryBasedFunctions;
                catagoryBasedFunctions = from function in db.UserManagement_Functions
                                         where function.FunctionCategory == functionCatagory
                                            && (lstFunctionFlagStrings.Contains(function.FunctionFlag) == true)                                            
                                         select function;

                List<TabularChiTietPhanQuyen> lstTbChiTietPhanQuyen = new List<TabularChiTietPhanQuyen>();

                

                // Loop through function-category-based functions
                foreach (UserManagement_Function catagoryBasedFunction in catagoryBasedFunctions)
                {
                    if (functionDA.ExistsChildFunction(catagoryBasedFunction.FunctionId))
                    {
                        continue;
                    }

                    // Init TabularChiTietPhanQuyen
                    TabularChiTietPhanQuyen tbChiTietPhanQuyen = new TabularChiTietPhanQuyen()
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

                    IQueryable<int> functionBasedAccessibilities;
                    functionBasedAccessibilities = from authorizedPages in db.UserManagement_AuthorizedPages
                                                   where authorizedPages.FunctionId == catagoryBasedFunction.FunctionId
                                                   select authorizedPages.AccessibilityId;
                    if (functionBasedAccessibilities.Count() != 0)
                    {
                        functionBasedAccessibilities = functionBasedAccessibilities.Distinct();
                    }

                    foreach (int functionBasedAccessibility in functionBasedAccessibilities)
                    {
                        switch (functionBasedAccessibility)
                        {
                            case (int)AccessibilityEnum.View:
                                {
                                    tbChiTietPhanQuyen.ViewDisplay = true;
                                    break;
                                }
                            case (int)AccessibilityEnum.Add:
                                {
                                    tbChiTietPhanQuyen.AddDisplay = true;
                                    break;
                                }
                            case (int)AccessibilityEnum.Modify:
                                {
                                    tbChiTietPhanQuyen.ModifyDisplay = true;
                                    break;
                                }
                            case (int)AccessibilityEnum.Delete:
                                {
                                    tbChiTietPhanQuyen.DeleteDisplay = true;
                                    break;
                                }
                        }
                    }

                    IQueryable<int> roledFunctionBaseAccessibilies;
                    roledFunctionBaseAccessibilies = from authorizedPage in db.UserManagement_AuthorizedPages
                                                    join authorization in db.UserManagement_Authorizations 
                                                        on authorizedPage.AuthorizedPageId equals authorization.AuthorizedPagePathId
                                                    where authorizedPage.FunctionId == catagoryBasedFunction.FunctionId
                                                        && authorization.RoleId == roleId
                                                    select authorizedPage.AccessibilityId;
                    if (roledFunctionBaseAccessibilies.Count() != 0)
                    {
                        roledFunctionBaseAccessibilies = roledFunctionBaseAccessibilies.Distinct();
                        if (roledFunctionBaseAccessibilies.Contains((int)AccessibilityEnum.View))
                        {
                            tbChiTietPhanQuyen.ViewAccessibility = true;                            
                        }

                        if (roledFunctionBaseAccessibilies.Contains((int)AccessibilityEnum.Add))
                        {
                            tbChiTietPhanQuyen.AddAccessibility = true;
                        }

                        if (roledFunctionBaseAccessibilies.Contains((int)AccessibilityEnum.Modify))
                        {
                            tbChiTietPhanQuyen.ModifyAccessibility = true;
                        }

                        if (roledFunctionBaseAccessibilies.Contains((int)AccessibilityEnum.Delete))
                        {
                            tbChiTietPhanQuyen.DeleteAccessibility = true;
                        }
                    }

                    lstTbChiTietPhanQuyen.Add(tbChiTietPhanQuyen);
                }

                if (lstTbChiTietPhanQuyen.Count != 0)
                {
                    tbPhanQuyen.FunctionCategoryName = functionCatagory;
                    tbPhanQuyen.ListChiTietPhanQuyens = lstTbChiTietPhanQuyen;
                    lstTbPhanQuyens.Add(tbPhanQuyen);
                }
            }

            return lstTbPhanQuyens;
        }        

        public void PhanQuyen(Guid roleId, List<UserManagement_AuthorizedPage> lstAuthorizedPages)
        {
            IQueryable<UserManagement_Authorization> deletingAuthorizations;
            deletingAuthorizations = from authorization in db.UserManagement_Authorizations
                                     where authorization.RoleId == roleId
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
                        RoleId = roleId
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
    }
}
