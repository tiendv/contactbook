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

        public PhanQuyenBL(School school)
            : base(school)
        {
            phanQuyenDA = new PhanQuyenDA(school);
        }

        public List<TabularPhanQuyen> GetListPhanQuyens(Guid roleId)
        {
            return phanQuyenDA.GetListPhanQuyens(roleId);
        }

        public void PhanQuyen(Guid roleId, List<TabularChiTietPhanQuyen> lstChiTietPhanQuyens)
        {
            FunctionsBL functionsBL = new FunctionsBL();

            List<UserManagement_AuthorizedPage> lstAuthorizedPages;
            lstAuthorizedPages = new List<UserManagement_AuthorizedPage>();

            foreach (TabularChiTietPhanQuyen tbChiTietPhanQuyen in lstChiTietPhanQuyens)
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
