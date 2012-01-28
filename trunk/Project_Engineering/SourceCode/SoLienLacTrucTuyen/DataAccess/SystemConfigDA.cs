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

        public void InsertYear(Configuration_Year year)
        {
            db.Configuration_Years.InsertOnSubmit(year);
            db.SubmitChanges();
        }

        public void UpdateYear(Configuration_Year year, DateTime firstTermBeginDate, DateTime firstTermEndDate, DateTime secondTermBeginDate, DateTime secondTermEndDate)
        {
            IQueryable<Configuration_Year> iqYear = from y in db.Configuration_Years
                                                    where y.YearId == year.YearId
                                                    select y;
            if (iqYear.Count() != 0)
            {
                year = iqYear.First();
                year.BeginFirstTermDate = firstTermBeginDate;
                year.EndFirstTermDate = firstTermEndDate;
                year.BeginSecondTermDate = secondTermBeginDate;
                year.EndSecondTermDate = secondTermEndDate;

                db.SubmitChanges();
            }
        }

        public void DeleteYear(Configuration_Year year)
        {
            // Firstly, delete term in year 
            IQueryable<Configuration_TermsInYear> queryTermsInYear = from termInYear in db.Configuration_TermsInYears
                                                                  where termInYear.YearId == year.YearId
                                                                  select termInYear;
            if (queryTermsInYear.Count() != 0)
            {
                foreach(Configuration_TermsInYear termsInYear in queryTermsInYear)
                {
                    db.Configuration_TermsInYears.DeleteOnSubmit(termsInYear);                    
                }
                db.SubmitChanges();
            }

            // Secondly, delete year 
            IQueryable<Configuration_Year> iqYear = from y in db.Configuration_Years
                                                    where y.YearId == year.YearId
                                                    select y;
            if (iqYear.Count() != 0)
            {
                year = iqYear.First();
                db.Configuration_Years.DeleteOnSubmit(year);
                db.SubmitChanges();
            }
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

        public List<Configuration_Year> GetYears()
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

        public Configuration_Year GetPreviousYear()
        {
            Configuration_Year prevYear = null;

            IQueryable<Configuration_Year> iqYear = from year in db.Configuration_Years
                                                    where year.SchoolId == school.SchoolId
                                                    select year;

            if (iqYear.Count() >= 2)
            {
                prevYear = iqYear.OrderByDescending(year => year.YearId).ToList()[1];
            }

            return prevYear;
        }

        public bool IsDeletable(Configuration_Year year)
        {
            IQueryable<Class_Class> iqClass = from Class in db.Class_Classes
                                              where Class.YearId == year.YearId
                                              select Class;
            if (iqClass.Count() != 0)
            {
                return false;
            }

            return true;
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
                lTerms = iqTerm.OrderBy(t => t.TermId).ToList();
            }

            return lTerms;
        }

        public Configuration_Term GetCurrentTerm()
        {
            Configuration_Term term = null;

            IQueryable<Configuration_Term> iqTerm = from termsInYear in db.Configuration_TermsInYears
                                                    where termsInYear.BeginDate <= DateTime.Now && termsInYear.EndDate >= DateTime.Now
                                                    && termsInYear.Configuration_Year.SchoolId == school.SchoolId
                                                    select termsInYear.Configuration_Term;
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

        public List<Configuration_Year> GetYears(string yearName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Configuration_Year> iqYear = from y in db.Configuration_Years
                                                    where y.SchoolId == school.SchoolId && y.YearName == yearName
                                                    select y;

            return GetYears(ref iqYear, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Configuration_Year> GetYears(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Configuration_Year> iqYear = from y in db.Configuration_Years
                                                    where y.SchoolId == school.SchoolId
                                                    select y;

            return GetYears(ref iqYear, pageCurrentIndex, pageSize, out totalRecords);
        }

        private List<Configuration_Year> GetYears(ref IQueryable<Configuration_Year> iqYear, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<Configuration_Year> years = new List<Configuration_Year>();
            totalRecords = iqYear.Count();
            if (totalRecords != 0)
            {
                years = iqYear.OrderBy(year => year.YearId)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return years;
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

        public List<Configuration_MessageStatus> GetMessageStatuses()
        {
            List<Configuration_MessageStatus> messageStatuses = new List<Configuration_MessageStatus>();
            IQueryable<Configuration_MessageStatus> iqMessageStatus = from msgStt in db.Configuration_MessageStatus
                                                                      select msgStt;
            if (iqMessageStatus.Count() != 0)
            {
                messageStatuses = iqMessageStatus.ToList();
            }
            return messageStatuses;
        }

        public List<Configuration_Province> GetProvinces()
        {
            List<Configuration_Province> provinces = new List<Configuration_Province>();
            IQueryable<Configuration_Province> iqProvince = from province in db.Configuration_Provinces
                                                            select province;
            if (iqProvince.Count() != 0)
            {
                provinces = iqProvince.OrderBy(province => province.ProvinceName).ToList();
            }

            return provinces;
        }

        public List<Configuration_District> GetDistricts(Configuration_Province province)
        {
            List<Configuration_District> districts = new List<Configuration_District>();
            IQueryable<Configuration_District> iqDistrict = from district in db.Configuration_Districts
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
