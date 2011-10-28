using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class CauHinhHeThongDA : BaseDA
    {
        public CauHinhHeThongDA()
            : base()
        {
        }

        public int GetMaNamHocHienHanh()
        {
            int maNamHocHienHanh = (from cauhinh_Hethong in db.CauHinh_HeThongs
                                   select cauhinh_Hethong.MaNamHocHienHanh).First();
            return maNamHocHienHanh;
        }

        public void UpdateMaNamHocHienHanh(int maNamHocHienHanh)
        {
            CauHinh_HeThong cauHinhHeThong = (from cauhinh_Hethong in db.CauHinh_HeThongs
                                              select cauhinh_Hethong).First();
            cauHinhHeThong.MaNamHocHienHanh = maNamHocHienHanh;
            db.SubmitChanges();
        }

        public int GetMaHocKyHienHanh()
        {
            int maHocKyHienHanh = (from cauhinh_Hethong in db.CauHinh_HeThongs
                                   select cauhinh_Hethong.MaHocKyHienHanh).First();
            return maHocKyHienHanh;
        }

        public void UpdateMaHocKyHienHanh(int maHocKyHienHanh)
        {
            CauHinh_HeThong cauHinhHeThong = (from cauhinh_Hethong in db.CauHinh_HeThongs
                                              select cauhinh_Hethong).First();
            cauHinhHeThong.MaHocKyHienHanh = maHocKyHienHanh;
            db.SubmitChanges();
        }

        public List<CauHinh_Thu> GetListThu()
        {
            IQueryable<CauHinh_Thu> thus = from thu in db.CauHinh_Thus
                                           select thu;
            if (thus.Count() != 0)
            {
                return thus.OrderBy(thu => thu.MaThu).ToList();
            }
            else
            {
                return new List<CauHinh_Thu>();
            }
        }
    }
}
