using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class PhanQuyenBL : BaseBL
    {
        private PhanQuyenDA phanQuyenDA;

        public PhanQuyenBL(School_School school)
            : base(school)
        {
            phanQuyenDA = new PhanQuyenDA(school);
        }

        public List<TabularAuthorization> GetTabularAuthorizations(aspnet_Role role)
        {
            return phanQuyenDA.GetTabularAuthorizations(role);
        }

        public void PhanQuyen(Guid roleId, List<TabularDetailedAuthorization> lstChiTietPhanQuyens)
        {
            FunctionsBL functionsBL = new FunctionsBL();

            List<UserManagement_AuthorizedPage> lstAuthorizedPages;
            lstAuthorizedPages = new List<UserManagement_AuthorizedPage>();

            foreach (TabularDetailedAuthorization tbChiTietPhanQuyen in lstChiTietPhanQuyens)
            {
                int functionId = tbChiTietPhanQuyen.FunctionId;
                bool bIncludeParentsFunction = false;
                if (tbChiTietPhanQuyen.ViewAccessibility)
                {
                    lstAuthorizedPages = lstAuthorizedPages.Concat(
                        phanQuyenDA.GetListAuthorizedPages(functionId,
                            (int)AccessibilityEnum.View)).ToList();
                    bIncludeParentsFunction = true;
                }

                if (tbChiTietPhanQuyen.AddAccessibility)
                {
                    lstAuthorizedPages = lstAuthorizedPages.Concat(
                        phanQuyenDA.GetListAuthorizedPages(functionId,
                            (int)AccessibilityEnum.Add)).ToList();
                    bIncludeParentsFunction = true;
                }

                if (tbChiTietPhanQuyen.ModifyAccessibility)
                {
                    lstAuthorizedPages = lstAuthorizedPages.Concat(
                        phanQuyenDA.GetListAuthorizedPages(functionId,
                            (int)AccessibilityEnum.Modify)).ToList();
                    bIncludeParentsFunction = true;
                }

                if (tbChiTietPhanQuyen.DeleteAccessibility)
                {
                    lstAuthorizedPages = lstAuthorizedPages.Concat(
                        phanQuyenDA.GetListAuthorizedPages(functionId,
                            (int)AccessibilityEnum.Delete)).ToList();
                    bIncludeParentsFunction = true;
                }

                if (bIncludeParentsFunction)
                {
                    int? parentsFunctionId = functionsBL.GetParentFunctionId(functionId);
                    if (parentsFunctionId != null)
                    {
                        lstAuthorizedPages = lstAuthorizedPages.Concat(
                            phanQuyenDA.GetListAuthorizedPages((int)parentsFunctionId,
                                (int)AccessibilityEnum.View)).ToList();
                    }
                }
            }

            lstAuthorizedPages = lstAuthorizedPages.Distinct().ToList();
            phanQuyenDA.PhanQuyen(roleId, lstAuthorizedPages);
        }
    }
}
