using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class SystemConfigDA : BaseDA
    {
        public SystemConfigDA(School school)
            : base(school)
        {
        }

        public CauHinh_NamHoc GetYear(int yearId)
        {
            CauHinh_NamHoc year = null;
            IQueryable<CauHinh_NamHoc> iqYear = from y in db.CauHinh_NamHocs
                                                where y.MaNamHoc == yearId
                                                select y;
            if (iqYear.Count() != 0)
            {
                year = iqYear.First();
            }

            return year;
        }

        public List<CauHinh_NamHoc> GetListYears()
        {
            List<CauHinh_NamHoc> lYears = new List<CauHinh_NamHoc>();

            IQueryable<CauHinh_NamHoc> iqYear = from y in db.CauHinh_NamHocs
                                                select y;
            if (iqYear.Count() != 0)
            {
                lYears = iqYear.OrderBy(year => year.TenNamHoc).ToList();
            }

            return lYears;
        }

        public CauHinh_NamHoc GetCurrentYear()
        {
            IQueryable<CauHinh_NamHoc> iqYear = from sysConfig in db.CauHinh_HeThongs
                                                join year in db.CauHinh_NamHocs
                                                    on sysConfig.MaNamHocHienHanh equals year.MaNamHoc
                                                select year;
            return iqYear.First();
        }

        public void UpdateMaNamHocHienHanh(int maNamHocHienHanh)
        {
            CauHinh_HeThong cauHinhHeThong = (from cauhinh_Hethong in db.CauHinh_HeThongs
                                              select cauhinh_Hethong).First();
            cauHinhHeThong.MaNamHocHienHanh = maNamHocHienHanh;
            db.SubmitChanges();
        }

        public CauHinh_HocKy GetTerm(int termId)
        {
            CauHinh_HocKy term = null;
            IQueryable<CauHinh_HocKy> iqTerm = from t in db.CauHinh_HocKies
                                               where t.MaHocKy == termId
                                               select t;

            if (iqTerm.Count() != 0)
            {
                term = iqTerm.First();
            }

            return term;
        }

        public List<CauHinh_HocKy> GetListTerms()
        {
            List<CauHinh_HocKy> lTerms = new List<CauHinh_HocKy>();

            IQueryable<CauHinh_HocKy> iqTerm = from t in db.CauHinh_HocKies
                                               select t;

            if (iqTerm.Count() != 0)
            {
                lTerms = iqTerm.OrderByDescending(t => t.MaHocKy).ToList();
            }

            return lTerms;
        }

        public CauHinh_HocKy GetCurrentTerm()
        {
            CauHinh_HocKy term = null;

            IQueryable<CauHinh_HocKy> iqTerm = from sysConfig in db.CauHinh_HeThongs
                                               join t in db.CauHinh_HocKies on sysConfig.MaHocKyHienHanh equals t.MaHocKy
                                               select t;
            if (iqTerm.Count() != 0)
            {
                term = iqTerm.First();
            }

            return term;
        }

        public void UpdateMaHocKyHienHanh(int maHocKyHienHanh)
        {
            CauHinh_HeThong cauHinhHeThong = (from cauhinh_Hethong in db.CauHinh_HeThongs
                                              select cauhinh_Hethong).First();
            cauHinhHeThong.MaHocKyHienHanh = maHocKyHienHanh;
            db.SubmitChanges();
        }

        public CauHinh_Thu GetDayInWeek(int dayInWeekId)
        {
            CauHinh_Thu dayInWeek = null;

            IQueryable<CauHinh_Thu> iqDayInWeek = from dIW in db.CauHinh_Thus
                                                  where dIW.MaThu == dayInWeekId
                                                  select dIW;
            if (iqDayInWeek.Count() != 0)
            {
                dayInWeek = iqDayInWeek.First();
            }

            return dayInWeek;
        }

        public List<CauHinh_Thu> GetListDayInWeeks()
        {
            List<CauHinh_Thu> lDayInWeek = new List<CauHinh_Thu>();

            IQueryable<CauHinh_Thu> iqDayInWeek = from dIW in db.CauHinh_Thus
                                                  select dIW;
            if (iqDayInWeek.Count() != 0)
            {
                lDayInWeek = iqDayInWeek.ToList();
            }

            return lDayInWeek;
        }

        public List<CauHinh_Buoi> GetSessions()
        {
            List<CauHinh_Buoi> lSesssions = new List<CauHinh_Buoi>();

            IQueryable<CauHinh_Buoi> iqSessions = from session in db.CauHinh_Buois
                                                  select session;
            if (iqSessions.Count() != 0)
            {
                lSesssions = iqSessions.ToList();
            }

            return lSesssions;
        }

        public CauHinh_Buoi GetSession(int sessionId)
        {
            CauHinh_Buoi sesssion = null;

            IQueryable<CauHinh_Buoi> iqSessions = from session in db.CauHinh_Buois
                                                  where session.MaBuoi == sessionId
                                                  select session;
            if (iqSessions.Count() != 0)
            {
                sesssion = iqSessions.First();
            }

            return sesssion;
        }
    }
}
