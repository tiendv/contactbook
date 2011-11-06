using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class SystemConfigBL
    {
        private SystemConfigDA sysConfigDA;

        public SystemConfigBL()
        {
            sysConfigDA = new SystemConfigDA();
        }

        public CauHinh_NamHoc GetCurrentYear()
        {
            return sysConfigDA.GetCurrentYear();
        }

        public void UpdateMaNamHocHienHanh(int maNamHocHienHanh)
        {
            sysConfigDA.UpdateMaNamHocHienHanh(maNamHocHienHanh);
        }

        public int GetMaHocKyHienHanh()
        {
            return sysConfigDA.GetMaHocKyHienHanh();
        }

        public void UpdateMaHocKyHienHanh(int maHocKyHienHanh)
        {
            sysConfigDA.UpdateMaHocKyHienHanh(maHocKyHienHanh);
        }

        public List<CauHinh_Thu> GetListThu()
        {
            return sysConfigDA.GetListThu();
        }
    }
}
