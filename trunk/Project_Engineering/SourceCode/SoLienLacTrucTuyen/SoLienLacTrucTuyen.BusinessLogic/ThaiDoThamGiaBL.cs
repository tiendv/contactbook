using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class ThaiDoThamGiaBL
    {
        private ThaiDoThamGiaDA thaiDoThamGiaDA;

        public ThaiDoThamGiaBL()
        {
            thaiDoThamGiaDA = new ThaiDoThamGiaDA();
        }

        public DanhMuc_ThaiDoThamGia GetThaiDoThamGia(int maThaiDoThamGia)
        {
            return thaiDoThamGiaDA.GetThaiDoThamGia(maThaiDoThamGia);
        }

        public List<DanhMuc_ThaiDoThamGia> GetListThaiDoThamGia()
        {
            return thaiDoThamGiaDA.GetListThaiDoThamGia();
        }

        public int GetThaiDoThamGiaCount()
        {
            return thaiDoThamGiaDA.GetThaiDoThamGiaCount();
        }

        public double GetThaiDoThamGiaCount(string tenThaiDoThamGia)
        {
            return thaiDoThamGiaDA.GetThaiDoThamGiaCount(tenThaiDoThamGia);
        }

        public List<DanhMuc_ThaiDoThamGia> GetListThaiDoThamGia(string tenThaiDoThamGia, 
            int pageIndex, int pageSize, out double totalRecords)
        {
            if (String.Compare(tenThaiDoThamGia, "tất cả", true) == 0 || tenThaiDoThamGia == "")
            {
                return thaiDoThamGiaDA.GetListThaiDoThamGia(tenThaiDoThamGia, pageIndex, pageSize, out totalRecords);
            }
            else
            {
                return thaiDoThamGiaDA.GetListThaiDoThamGia(pageIndex, pageSize, out totalRecords);
            }            
        }

        public bool CheckCanDeleteThaiDoThamGia(int maThaiDoThamGia)
        {
            return thaiDoThamGiaDA.CheckCanDeleteThaiDoThamGia(maThaiDoThamGia);
        }

        public void UpdateThaiDoThamGia(DanhMuc_ThaiDoThamGia ThaiDoThamGia)
        {
            thaiDoThamGiaDA.UpdateThaiDoThamGia(ThaiDoThamGia);
        }

        public void DeleteThaiDoThamGia(int maThaiDoThamGia)
        {
            thaiDoThamGiaDA.DeleteThaiDoThamGia(maThaiDoThamGia);
        }

        public void InsertThaiDoThamGia(DanhMuc_ThaiDoThamGia ThaiDoThamGia)
        {
            thaiDoThamGiaDA.InsertThaiDoThamGia(ThaiDoThamGia);
        }

        public bool CheckExistTenThaiDoThamGia(int maThaiDoThamGia, string tenThaiDoThamGia)
        {
            return thaiDoThamGiaDA.CheckExistTenThaiDoThamGia(maThaiDoThamGia, tenThaiDoThamGia);
        }
    }
}
