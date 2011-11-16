using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class ConductBL:BaseBL
    {
        private ConductDA conductDA;

        public ConductBL(School school)
            : base(school)
        {
            conductDA = new ConductDA(school);
        }

        public void InsertConduct(DanhMuc_HanhKiem conduct)
        {
            conductDA.InsertConduct(conduct);
        }

        public void UpdateConduct(string editedConductName, string newConductName)
        {
            DanhMuc_HanhKiem conduct = GetConduct(editedConductName);
            
            conduct.TenHanhKiem = newConductName;

            conductDA.UpdateConduct(conduct);
        }

        public void DeleteConduct(DanhMuc_HanhKiem conduct)
        {
            conductDA.DeleteConduct(conduct);
        }

        public DanhMuc_HanhKiem GetConduct(int conductId)
        {
            return conductDA.GetConduct(conductId);
        }

        public DanhMuc_HanhKiem GetConduct(string conductName)
        {
            return conductDA.GetConduct(conductName);
        }
        
        public List<DanhMuc_HanhKiem> GetListConducts(bool hasUndefinedOption)
        {
            List<DanhMuc_HanhKiem> lConducts = conductDA.GetConducts();

            if (hasUndefinedOption)
            {
                lConducts.Add(new DanhMuc_HanhKiem
                {
                    TenHanhKiem = "Chưa xác định",
                    MaHanhKiem = -1
                });
            }

            return lConducts;
        }

        public List<DanhMuc_HanhKiem> GetListConducts(int currentIndex, int pageSize, out double totalRecords)
        {
            return conductDA.GetConducts(currentIndex, pageSize, out totalRecords);
        }

        public List<DanhMuc_HanhKiem> GetListConducts(string conductName, int pageIndex, int pageSize, out double totalRecords)
        {
            List<DanhMuc_HanhKiem> lConducts = new List<DanhMuc_HanhKiem>();

            if ((conductName == "") || (string.Compare(conductName, "tất cả", true) == 0))
            {
                lConducts = conductDA.GetConducts(pageIndex, pageSize, out totalRecords);
            }
            else
            {
                DanhMuc_HanhKiem conduct = GetConduct(conductName);
                lConducts.Add(conduct);
                totalRecords = 1;
            }

            return lConducts;
        }

        public bool IsDeletable(string conductName)
        {
            return conductDA.IsDeletable(conductName);
        }

        public bool ConductNameExists(string conductName)
        {
            return conductDA.ConductNameExists(conductName);
        }

        public bool ConductNameExists(string oldConductName, string newConductName)
        {
            bool bResult = false;

            if (oldConductName == newConductName)
            {
                bResult = false;
            }
            else
            {
                bResult = conductDA.ConductNameExists(newConductName);
            }

            return bResult;
        }
    }
}
