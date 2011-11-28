using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class HocLucBL : BaseBL
    {
        HocLucDA hocLucDA;

        public HocLucBL(School school)
            : base(school)
        {
            hocLucDA = new HocLucDA(school);
        }

        public void InsertHocLuc(DanhMuc_HocLuc hocLucEn)
        {
            hocLucDA.InsertHocLuc(hocLucEn);
        }

        public void UpdateHocLuc(int maHocLuc, string tenHocLuc, float dtbDau, float dtbCuoi)
        {
            hocLucDA.UpdateHocLuc(new DanhMuc_HocLuc()
            {
                MaHocLuc = maHocLuc,
                TenHocLuc = tenHocLuc,
                DTBDau = dtbDau,
                DTBCuoi = dtbCuoi
            });
        }

        public void DeleteHocLuc(int maHocLuc)
        {
            hocLucDA.DeleteHocLuc(maHocLuc);
        }

        public DanhMuc_HocLuc GetHocLuc(int maHocLuc)
        {
            return hocLucDA.GetHocLuc(maHocLuc);
        }

        public List<DanhMuc_HocLuc> GetListHocLuc(bool allOptions)
        {
            List<DanhMuc_HocLuc> hocLucs = hocLucDA.GetListHocLuc();
            if (hocLucs.Count != 0 && allOptions)
            {
                hocLucs.Insert(hocLucs.Count, new DanhMuc_HocLuc
                {
                    MaHocLuc = 0,
                    TenHocLuc = "Tất cả"
                });
            }
            return hocLucs;
        }

        public List<DanhMuc_HocLuc> GetListHocLuc(int pageCurrentIndex, int pageSize)
        {
            return hocLucDA.GetListHocLuc(pageCurrentIndex, pageSize);
        }

        public List<DanhMuc_HocLuc> GetListHocLuc(string tenHocLuc, int pageCurrentIndex, int pageSize)
        {
            return hocLucDA.GetListHocLuc(tenHocLuc, pageCurrentIndex, pageSize);
        }

        public double GetHocLucCount()
        {
            return hocLucDA.GetHocLucCount();
        }

        public double GetHocLucCount(string tenHocLuc)
        {
            return hocLucDA.GetHocLucCount(tenHocLuc);
        }

        public bool CheckExistHocLuc(int maHocLuc, string tenHocLuc)
        {
            return hocLucDA.CheckExistTenHocLuc(maHocLuc, tenHocLuc);
        }

        public bool CheckCanDeleteHocLuc(int maHocLuc)
        {
            return hocLucDA.CheckCanDeleteHocLuc(maHocLuc);
        }

        public bool ConductNameExists(string conductName)
        {
            return hocLucDA.ConductNameExists(conductName);
        }
        public void InsertConduct(DanhMuc_HocLuc conduct)
        {
            hocLucDA.InsertConduct(conduct);
        }
        public void DeleteConduct(DanhMuc_HocLuc conduct)
        {
            hocLucDA.DeleteConduct(conduct);
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
                bResult = hocLucDA.ConductNameExists(newConductName);
            }

            return bResult;
        }
        public void UpdateConduct(string editedConductName, string newConductName)
        {
            DanhMuc_HocLuc hocluc = GetConduct(editedConductName);

            hocluc.TenHocLuc = newConductName;

            hocLucDA.UpdateConduct(hocluc);
        }
        public DanhMuc_HocLuc GetConduct(string conductName)
        {
            return hocLucDA.GetConduct(conductName);
        }
        public bool IsDeletable(string conductName)
        {
            return hocLucDA.IsDeletable(conductName);
        }
    }
}
