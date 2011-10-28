using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class HanhKiemBL
    {
        private HanhKiemDA hanhKiemDA;

        public HanhKiemBL()
        {
            hanhKiemDA = new HanhKiemDA();
        }

        public void InsertHanhKiem(DanhMuc_HanhKiem HanhKiem)
        {
            hanhKiemDA.InsertHanhKiem(HanhKiem);
        }

        public void UpdateHanhKiem(DanhMuc_HanhKiem HanhKiem)
        {
            hanhKiemDA.UpdateHanhKiem(HanhKiem);
        }

        public void DeleteHanhKiem(int maHanhKiem)
        {
            hanhKiemDA.DeleteHanhKiem(maHanhKiem);
        }

        public DanhMuc_HanhKiem GetHanhKiem(int maHanhKiem)
        {
            return hanhKiemDA.GetHanhKiem(maHanhKiem);
        }

        public List<DanhMuc_HanhKiem> GetListHanhKiem()
        {
            return hanhKiemDA.GetListHanhKiem();
        }

        public List<DanhMuc_HanhKiem> GetListHanhKiem(bool hasUndefinedOption)
        {
            List<DanhMuc_HanhKiem> lstHanhKiem = hanhKiemDA.GetListHanhKiem();
            if (hasUndefinedOption)
            {
                lstHanhKiem.Add(new DanhMuc_HanhKiem
                {
                    TenHanhKiem = "Chưa xác định",
                    MaHanhKiem = -1
                });
            }
            return lstHanhKiem;
        }

        public List<DanhMuc_HanhKiem> GetListHanhKiem(int currentIndex, int pageSize)
        {
            return hanhKiemDA.GetListHanhKiem(currentIndex, pageSize);
        }

        public List<DanhMuc_HanhKiem> GetListHanhKiem(string tenHanhKiem, int pageIndex, int pageSize)
        {
            return hanhKiemDA.GetListHanhKiem(tenHanhKiem, pageIndex, pageSize);
        }

        public int GetHanhKiemCount()
        {
            return hanhKiemDA.GetHanhKiemCount();
        }

        public double GetHanhKiemCount(string tenHanhKiem)
        {
            return hanhKiemDA.GetHanhKiemCount(tenHanhKiem);
        }
        
        public bool CheckCanDeleteHanhKiem(int maHanhKiem)
        {
            return hanhKiemDA.CheckCanDeleteHanhKiem(maHanhKiem);
        }

        public bool CheckExistTenHanhKiem(int maHanhKiem, string tenHanhKiem)
        {
            return hanhKiemDA.CheckExistTenHanhKiem(maHanhKiem, tenHanhKiem);
        }
    }
}
