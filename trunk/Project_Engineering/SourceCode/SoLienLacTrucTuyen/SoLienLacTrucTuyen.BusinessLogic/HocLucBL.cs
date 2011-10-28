using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class HocLucBL
    {
        HocLucDA hocLucDA;

        public HocLucBL()
        {
            hocLucDA = new HocLucDA();
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
    }
}
