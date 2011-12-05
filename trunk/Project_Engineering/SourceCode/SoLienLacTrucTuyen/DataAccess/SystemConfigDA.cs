using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class SystemConfigDA : BaseDA
    {
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
            List<Configuration_Year> lYears = new List<Configuration_Year>();

            IQueryable<Configuration_Year> iqYear = from y in db.Configuration_Years
                                                    select y;
            if (iqYear.Count() != 0)
            {
                lYears = iqYear.OrderBy(year => year.YearName).ToList();
            }

            return lYears;
        }

        public Configuration_Year GetCurrentYear()
        {
            IQueryable<Configuration_Year> iqYear = from sysConfig in db.Configuration_Configurations
                                                    join year in db.Configuration_Years
                                                        on sysConfig.CurrentYear equals year.YearId
                                                    select year;
            return iqYear.First();
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
    }
}
