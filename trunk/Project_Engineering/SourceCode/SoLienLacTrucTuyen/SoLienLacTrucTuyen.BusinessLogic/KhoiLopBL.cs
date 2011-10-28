using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class KhoiLopBL
    {
        private KhoiLopDA khoiLopDA;

        public KhoiLopBL()
        {
            khoiLopDA = new KhoiLopDA();
        }

        public void InsertKhoiLop(string tenKhoiLop, short thuTuHienThi)
        {
            DanhMuc_KhoiLop khoiLop = new DanhMuc_KhoiLop
            {
                TenKhoiLop = tenKhoiLop,
                ThuTuHienThi = thuTuHienThi
            };

            khoiLopDA.InsertKhoiLop(khoiLop);
        }

        public void UpdateKhoiLop(DanhMuc_KhoiLop KhoiLop)
        {
            khoiLopDA.UpdateKhoiLop(KhoiLop);
        }

        public void DeleteKhoiLop(int maKhoiLop)
        {
            khoiLopDA.DeleteKhoiLop(maKhoiLop);
        }

        public DanhMuc_KhoiLop GetKhoiLop(int maKhoiLop)
        {
            return khoiLopDA.GetKhoiLop(maKhoiLop);
        }

        public List<DanhMuc_KhoiLop> GetListKhoiLop()
        {
            return khoiLopDA.GetListKhoiLop();
        }

        public List<DanhMuc_KhoiLop> GetListKhoiLop(string tenKhoiLop, int pageIndex, int pageSize, out double totalRecords)
        {
            if (String.Compare(tenKhoiLop, "tất cả", true) == 0 || tenKhoiLop == "")
            {
                return khoiLopDA.GetListKhoiLop(pageIndex, pageSize, out totalRecords);    
            }
            else
            {
                return khoiLopDA.GetListKhoiLop(tenKhoiLop, pageIndex, pageSize, out totalRecords);
            }
        }

        public bool CanDeleteKhoiLop(int maKhoiLop)
        {
            return khoiLopDA.CanDeleteKhoiLop(maKhoiLop);
        }        

        public bool KhoiLopExists(string tenKhoiLop)
        {
            return khoiLopDA.KhoiLopExists(tenKhoiLop);
        }

        public bool KhoiLopExists(int maKhoiLop, string tenKhoiLop)
        {
            return khoiLopDA.KhoiLopExists(maKhoiLop, tenKhoiLop);
        }
    }
}
