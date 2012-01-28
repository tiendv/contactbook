using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class ConductBL:BaseBL
    {
        private ConductDA conductDA;

        public ConductBL(School_School school)
            : base(school)
        {
            conductDA = new ConductDA(school);
        }

        public void InsertConduct(string conductName)
        {
            Category_Conduct conduct = new Category_Conduct();
            conduct.ConductName = conductName;
            conductDA.InsertConduct(conduct);
        }

        public void UpdateConduct(Category_Conduct conduct, string newConductName)
        {   
            conduct.ConductName = newConductName;
            conductDA.UpdateConduct(conduct);
        }

        public void DeleteConduct(Category_Conduct conduct)
        {
            conductDA.DeleteConduct(conduct);
        }

        public Category_Conduct GetConduct(int conductId)
        {
            return conductDA.GetConduct(conductId);
        }

        public Category_Conduct GetConduct(string conductName)
        {
            return conductDA.GetConduct(conductName);
        }
        
        public List<Category_Conduct> GetListConducts(bool hasUndefinedOption)
        {
            List<Category_Conduct> lConducts = conductDA.GetConducts();

            if (hasUndefinedOption)
            {
                lConducts.Add(new Category_Conduct
                {
                    ConductName = "Chưa xác định",
                    ConductId = -1
                });
            }

            return lConducts;
        }

        public List<Category_Conduct> GetListConducts(int currentIndex, int pageSize, out double totalRecords)
        {
            return conductDA.GetConducts(currentIndex, pageSize, out totalRecords);
        }

        public List<Category_Conduct> GetListConducts(string conductName, int pageIndex, int pageSize, out double totalRecords)
        {
            List<Category_Conduct> lConducts = new List<Category_Conduct>();

            if ((conductName == "") || (string.Compare(conductName, "tất cả", true) == 0))
            {
                lConducts = conductDA.GetConducts(pageIndex, pageSize, out totalRecords);
            }
            else
            {
                Category_Conduct conduct = GetConduct(conductName);
                if (conduct != null)
                {
                    lConducts.Add(conduct);
                }
                totalRecords = lConducts.Count;
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

        public bool IsDeletable(Category_Conduct conduct)
        {
            return conductDA.IsDeletable(conduct);
        }
    }
}
