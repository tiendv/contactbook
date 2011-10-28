using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class HoatDongBL
    {
        private HoatDongDA hoatDongDA;

        public HoatDongBL()
        {
            hoatDongDA = new HoatDongDA();
        }

        #region Insert, Update, Delete
        public void InsertHoatDong(int maHocSinh, int maHocKy, DateTime ngay,
            string tenHoatDong, string moTa, int maThaiDoThamGia)
        {
            if (maThaiDoThamGia == 0)
            {
                hoatDongDA.InsertHoatDong(maHocSinh, maHocKy, ngay, tenHoatDong, moTa, null);
            }
            else
            {
                hoatDongDA.InsertHoatDong(maHocSinh, maHocKy, ngay, tenHoatDong, moTa, maThaiDoThamGia);
            }            
        }

        public void UpdateHoatDong(int maHoatDong, DateTime ngay,
            string moTa, int maThaiDoThamGia)
        {
            if (maThaiDoThamGia == 0)
            {
                hoatDongDA.UpdateHoatDong(maHoatDong, ngay, moTa, null);
            }
            else
            {
                hoatDongDA.UpdateHoatDong(maHoatDong, ngay, moTa, maThaiDoThamGia);
            }
            
        }

        public void DeleteHoatDong(int maHoatDong)
        {
            hoatDongDA.DeleteHoatDong(maHoatDong);
        }
        #endregion

        #region Get HoatDong
        public HocSinh_HoatDong GetHoatDong(int maHoatDong)
        {
            return hoatDongDA.GetHoatDong(maHoatDong);
        }

        public HocSinh_HoatDong GetHoatDong(int maHocSinh, int maNamHoc, int maHocKy,
            DateTime ngay)
        {
            return hoatDongDA.GetHoatDong(maHocSinh, maNamHoc, maHocKy, ngay);
        }

        public HocSinh_HoatDong GetHoatDong(int maHoatDong, int maHocSinh, int maNamHoc, int maHocKy,
            DateTime ngay)
        {
            return hoatDongDA.GetHoatDong(maHoatDong, maHocSinh, maNamHoc, maHocKy, ngay);
        }
        #endregion

        #region Get List TabularHoatDong
        public List<TabularHoatDong> GetListTabularHoatDong(int maHocSinh, int maNamHoc, int maHocKy,
            DateTime tuNgay, DateTime denNgay,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularHoatDong> lstTbHoatDongs = hoatDongDA.GetListTabularHoatDong(maHocSinh, 
                maNamHoc, maHocKy,
                tuNgay, denNgay,
                pageCurrentIndex, pageSize, out totalRecords);

            ThaiDoThamGiaBL thaiDoThamGiaBL = new ThaiDoThamGiaBL();
            foreach (TabularHoatDong hoatDong in lstTbHoatDongs)
            {
                if (hoatDong.MaThaiDoThamGia != null)
                {
                    hoatDong.ThaiDoThamGia = thaiDoThamGiaBL.GetThaiDoThamGia((int)hoatDong.MaThaiDoThamGia).TenThaiDoThamGia;
                }
                else
                {
                    hoatDong.ThaiDoThamGia = "Không xác định";
                }
            }
            return lstTbHoatDongs;
        }
        #endregion

        public bool HoatDongExists(int? maHoatDong, string tieuDe, int maHocSinh,
            int maHocKy, DateTime ngay)
        {
            HocSinhBL hocSinhBL = new HocSinhBL();
            int maLopHoc = hocSinhBL.GetCurrentMaLopHoc(maHocSinh);
            if (maHoatDong == null)
            {
                return hoatDongDA.HoatDongExists(tieuDe, maHocSinh, maLopHoc, maHocKy, ngay);
            }
            else
            {
                return hoatDongDA.HoatDongExists((int)maHoatDong, tieuDe, maHocSinh, maLopHoc, maHocKy, ngay);
            }
        }
    }
}
