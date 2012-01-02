using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.DataAccess
{
    public class SystemConfigDA : BaseDA
    {
        public SystemConfigDA()
            : base()
        {
        }

        public SystemConfigDA(School_School school)
            : base(school)
        {
        }

        public Configuration_Year GetYear(int yearId)
        {
            Configuration_Year year = null;
            IQueryable<Configuration_Year> iqYear = from y in db.Configuration_Years
                                                    where y.YearId == yearId
                                                    select y;
            if (iqYear.Count() != 0)
            {
                year = iqYear.First();
            }

            return year;
        }

        public List<Configuration_Year> GetListYears()
        {
            List<Configuration_Year> years = new List<Configuration_Year>();
            IQueryable<Configuration_Year> iqYear = from year in db.Configuration_Years
                                                    where year.SchoolId == school.SchoolId
                                                    select year;
            if (iqYear.Count() != 0)
            {
                years = iqYear.OrderByDescending(year => year.YearName).ToList();
            }

            return years;
        }

        public Configuration_Year GetLastedYear()
        {
            Configuration_Year lastedYear = null;

            IQueryable<Configuration_Year> iqYear = from year in db.Configuration_Years
                                                    where year.SchoolId == school.SchoolId
                                                    select year;

            if (iqYear.Count() != 0)
            {
                lastedYear = iqYear.OrderByDescending(year => year.YearId).First();
            }

            return lastedYear;
        }

        public void UpdateYearIdHienHanh(int YearIdHienHanh)
        {
            Configuration_Configuration cauHinhHeThong = (from cauhinh_Hethong in db.Configuration_Configurations
                                                          select cauhinh_Hethong).First();
            cauHinhHeThong.CurrentYear = YearIdHienHanh;
            db.SubmitChanges();
        }

        public Configuration_Term GetTerm(int termId)
        {
            Configuration_Term term = null;
            IQueryable<Configuration_Term> iqTerm = from t in db.Configuration_Terms
                                                    where t.TermId == termId
                                                    select t;

            if (iqTerm.Count() != 0)
            {
                term = iqTerm.First();
            }

            return term;
        }

        public List<Configuration_Term> GetListTerms()
        {
            List<Configuration_Term> lTerms = new List<Configuration_Term>();

            IQueryable<Configuration_Term> iqTerm = from t in db.Configuration_Terms
                                                    select t;

            if (iqTerm.Count() != 0)
            {
                lTerms = iqTerm.OrderByDescending(t => t.TermId).ToList();
            }

            return lTerms;
        }

        public Configuration_Term GetCurrentTerm()
        {
            Configuration_Term term = null;

            IQueryable<Configuration_Term> iqTerm = from sysConfig in db.Configuration_Configurations
                                                    join t in db.Configuration_Terms on sysConfig.CurrentTerm equals t.TermId
                                                    select t;
            if (iqTerm.Count() != 0)
            {
                term = iqTerm.First();
            }

            return term;
        }

        public void UpdateTermIdHienHanh(int TermIdHienHanh)
        {
            Configuration_Configuration cauHinhHeThong = (from cauhinh_Hethong in db.Configuration_Configurations
                                                          select cauhinh_Hethong).First();
            cauHinhHeThong.CurrentTerm = TermIdHienHanh;
            db.SubmitChanges();
        }

        public Configuration_DayInWeek GetDayInWeek(int dayInWeekId)
        {
            Configuration_DayInWeek dayInWeek = null;

            IQueryable<Configuration_DayInWeek> iqDayInWeek = from dIW in db.Configuration_DayInWeeks
                                                              where dIW.DayInWeekId == dayInWeekId
                                                              select dIW;
            if (iqDayInWeek.Count() != 0)
            {
                dayInWeek = iqDayInWeek.First();
            }

            return dayInWeek;
        }

        public List<Configuration_DayInWeek> GetListDayInWeeks()
        {
            List<Configuration_DayInWeek> lDayInWeek = new List<Configuration_DayInWeek>();

            IQueryable<Configuration_DayInWeek> iqDayInWeek = from dIW in db.Configuration_DayInWeeks
                                                              select dIW;
            if (iqDayInWeek.Count() != 0)
            {
                lDayInWeek = iqDayInWeek.ToList();
            }

            return lDayInWeek;
        }

        public List<Configuration_Session> GetSessions()
        {
            List<Configuration_Session> lSesssions = new List<Configuration_Session>();

            IQueryable<Configuration_Session> iqSessions = from session in db.Configuration_Sessions
                                                           select session;
            if (iqSessions.Count() != 0)
            {
                lSesssions = iqSessions.ToList();
            }

            return lSesssions;
        }

        public Configuration_Session GetSession(int sessionId)
        {
            Configuration_Session sesssion = null;

            IQueryable<Configuration_Session> iqSessions = from session in db.Configuration_Sessions
                                                           where session.SessionId == sessionId
                                                           select session;
            if (iqSessions.Count() != 0)
            {
                sesssion = iqSessions.First();
            }

            return sesssion;
        }

        public List<ConfigurationMessageStatus> GetMessageStatuses()
        {
            List<ConfigurationMessageStatus> messageStatuses = new List<ConfigurationMessageStatus>();
            IQueryable<ConfigurationMessageStatus> iqMessageStatus = from msgStt in db.ConfigurationMessageStatuses
                                                                     select msgStt;
            if (iqMessageStatus.Count() != 0)
            {
                messageStatuses = iqMessageStatus.ToList();
            }
            return messageStatuses;
        }

        public List<ConfigurationProvince> GetProvinces()
        {
            List<ConfigurationProvince> provinces = new List<ConfigurationProvince>();
            IQueryable<ConfigurationProvince> iqProvince = from province in db.ConfigurationProvinces
                                                           select province;
            if (iqProvince.Count() != 0)
            {
                provinces = iqProvince.OrderBy(province => province.ProvinceName).ToList();
            }

            return provinces;
        }

        public List<ConfigurationDistrict> GetDistricts(ConfigurationProvince province)
        {
            List<ConfigurationDistrict> districts = new List<ConfigurationDistrict>();
            IQueryable<ConfigurationDistrict> iqDistrict = from district in db.ConfigurationDistricts
                                                           where district.ProvinceId == province.ProvinceId
                                                           select district;
            if (iqDistrict.Count() != 0)
            {
                districts = iqDistrict.OrderBy(district => district.DistrictName).ToList();
            }

            return districts;
        }
    }
}
