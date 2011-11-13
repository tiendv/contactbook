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

        public CauHinh_NamHoc GetYear(int yearId)
        {
            return sysConfigDA.GetYear(yearId);
        }

        public List<CauHinh_NamHoc> GetListYears()
        {
            return sysConfigDA.GetListYears();
        }

        public CauHinh_NamHoc GetCurrentYear()
        {
            return sysConfigDA.GetCurrentYear();
        }

        public void UpdateMaNamHocHienHanh(int maNamHocHienHanh)
        {
            sysConfigDA.UpdateMaNamHocHienHanh(maNamHocHienHanh);
        }

        public CauHinh_HocKy GetTerm(int maHocKy)
        {
            return sysConfigDA.GetTerm(maHocKy);
        }

        public List<CauHinh_HocKy> GetListTerms()
        {
            return sysConfigDA.GetListTerms();
        }

        public CauHinh_HocKy GetCurrentTerm()
        {
            return sysConfigDA.GetCurrentTerm();
        }

        public void UpdateMaHocKyHienHanh(int maHocKyHienHanh)
        {
            sysConfigDA.UpdateMaHocKyHienHanh(maHocKyHienHanh);
        }

        public List<CauHinh_Thu> GetDayInWeeks()
        {
            return sysConfigDA.GetListDayInWeeks();
        }

        public CauHinh_Thu GetDayInWeek(int dayInWeekId)
        {
            return sysConfigDA.GetDayInWeek(dayInWeekId);
        }

        public List<CauHinh_Buoi> GetSessions()
        {
            return sysConfigDA.GetSessions();
        }

        public string GetSessionName(int sessionId)
        {
            CauHinh_Buoi session = GetSession(sessionId);
            if (session != null)
            {
                return session.TenBuoi;
            }
            else
            {
                return "Cả ngày";
            }
        }

        public CauHinh_Buoi GetSession(int sessionId)
        {
            return sysConfigDA.GetSession(sessionId);
        }
    }
}
