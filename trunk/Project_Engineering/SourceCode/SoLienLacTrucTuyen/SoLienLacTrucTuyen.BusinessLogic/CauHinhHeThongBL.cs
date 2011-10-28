using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class CauHinhHeThongBL
    {
        private CauHinhHeThongDA cauHinhHeThongDA;

        public CauHinhHeThongBL()
        {
            cauHinhHeThongDA = new CauHinhHeThongDA();
        }

        public int GetMaNamHocHienHanh()
        {
            return cauHinhHeThongDA.GetMaNamHocHienHanh();
        }

        public void UpdateMaNamHocHienHanh(int maNamHocHienHanh)
        {
            cauHinhHeThongDA.UpdateMaNamHocHienHanh(maNamHocHienHanh);
        }

        public int GetMaHocKyHienHanh()
        {
            return cauHinhHeThongDA.GetMaHocKyHienHanh();
        }

        public void UpdateMaHocKyHienHanh(int maHocKyHienHanh)
        {
            cauHinhHeThongDA.UpdateMaHocKyHienHanh(maHocKyHienHanh);
        }

        public List<CauHinh_Thu> GetListThu()
        {
            return cauHinhHeThongDA.GetListThu();
        }
    }
}
