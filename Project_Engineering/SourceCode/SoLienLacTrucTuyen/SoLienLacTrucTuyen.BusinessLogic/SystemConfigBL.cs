using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class SystemConfigBL : BaseBL
    {
        private SystemConfigDA sysConfigDA;

        public SystemConfigBL()
            : base()
        {
            sysConfigDA = new SystemConfigDA();
        }

        public SystemConfigBL(School_School school)
            : base(school)
        {
            sysConfigDA = new SystemConfigDA(school);
        }

        public Configuration_Year GetYear(int yearId)
        {
            return sysConfigDA.GetYear(yearId);
        }

        public List<Configuration_Year> GetListYears()
        {
            return sysConfigDA.GetListYears();
        }

        public Configuration_Year GetLastedYear()
        {
            return sysConfigDA.GetLastedYear();
        }

        public Configuration_Year GetPreviousYear()
        {
            return sysConfigDA.GetPreviousYear();
        }

        public void UpdateYearIdHienHanh(int YearIdHienHanh)
        {
            sysConfigDA.UpdateYearIdHienHanh(YearIdHienHanh);
        }

        public Configuration_Term GetTerm(int TermId)
        {
            return sysConfigDA.GetTerm(TermId);
        }

        public List<Configuration_Term> GetListTerms()
        {
            return sysConfigDA.GetListTerms();
        }

        public Configuration_Term GetCurrentTerm()
        {
            Configuration_Year currentYear = GetLastedYear();
            if (DateTime.Now < currentYear.BeginSecondTermDate)
            {
                return GetTerm(1);
            }
            else
            {
                return GetTerm(2);
            }
        }

        public void UpdateTermIdHienHanh(int TermIdHienHanh)
        {
            sysConfigDA.UpdateTermIdHienHanh(TermIdHienHanh);
        }

        public List<Month> GetMonths(Configuration_Year year, Configuration_Term term)
        {
            List<Month> months = new List<Month>();
            int iMonth;
            year = GetYear(year.YearId);
            DateTime dtBeginTermDate;
            DateTime dtEndTermDate;

            if (term.TermId == 1)
            {
                dtBeginTermDate = year.BeginFirstTermDate;
                dtEndTermDate = year.EndFirstTermDate;
            }
            else
            {
                dtBeginTermDate = year.BeginSecondTermDate;
                dtEndTermDate = year.EndSecondTermDate;
            }

            Month month = null;
            do
            {
                iMonth = dtBeginTermDate.Month;
                month = new Month();
                month.MonthId = iMonth;
                month.MonthName = Month.GetMonthName(iMonth);
                months.Add(month);

                dtBeginTermDate = dtBeginTermDate.AddMonths(1);
            } while (dtBeginTermDate <= dtEndTermDate);



            return months;
        }

        public List<Week> GetWeeks(Configuration_Year year, Configuration_Term term)
        {
            List<Week> weeks = new List<Week>();
            int index = 1;
            Week week = null;
            year = GetYear(year.YearId);
            DateTime dtBeginTermDate;
            DateTime dtEndTermDate;

            if (term.TermId == 1)
            {
                dtBeginTermDate = year.BeginFirstTermDate;
                dtEndTermDate = year.EndFirstTermDate;
            }
            else
            {
                dtBeginTermDate = year.BeginSecondTermDate;
                dtEndTermDate = year.EndSecondTermDate;
            }

            // in case begin date is Sunday
            if ((int)dtBeginTermDate.DayOfWeek == 0)
            {
                dtBeginTermDate = dtBeginTermDate.AddDays(1);
            }

            week = new Week();
            week.WeekIndex = index;
            week.BeginDate = dtBeginTermDate;
            dtBeginTermDate = dtBeginTermDate.AddDays(5 - week.WeekIndex);
            week.EndDate = dtBeginTermDate;
            weeks.Add(week);

            if (dtBeginTermDate <= dtEndTermDate)
            {
                dtBeginTermDate = dtBeginTermDate.AddDays(2);
                do
                {
                    index++;
                    week = new Week();
                    week.WeekIndex = index;
                    week.BeginDate = dtBeginTermDate;
                    dtBeginTermDate = dtBeginTermDate.AddDays(5);
                    week.EndDate = dtBeginTermDate;
                    weeks.Add(week);
                    dtBeginTermDate = dtBeginTermDate.AddDays(2);
                } while (dtBeginTermDate.AddDays(5) <= dtEndTermDate);

                if (dtBeginTermDate <= dtEndTermDate)
                {
                    index++;
                    week = new Week();
                    week.WeekIndex = index;
                    week.BeginDate = dtBeginTermDate;
                    if ((int)dtEndTermDate.DayOfWeek != 0)
                    {
                        dtBeginTermDate = dtBeginTermDate.AddDays((int)dtEndTermDate.DayOfWeek - 1);
                    }
                    week.EndDate = dtBeginTermDate;
                    weeks.Add(week);
                }
            }

            return weeks;
        }

        public List<Configuration_DayInWeek> GetDayInWeeks()
        {
            return sysConfigDA.GetListDayInWeeks();
        }

        public Configuration_DayInWeek GetDayInWeek(int dayInWeekId)
        {
            return sysConfigDA.GetDayInWeek(dayInWeekId);
        }

        public List<Configuration_Session> GetSessions()
        {
            return sysConfigDA.GetSessions();
        }

        public string GetSessionName(int sessionId)
        {
            Configuration_Session session = GetSession(sessionId);
            if (session != null)
            {
                return session.SessionName;
            }
            else
            {
                return "Cả ngày";
            }
        }

        public Configuration_Session GetSession(int sessionId)
        {
            return sysConfigDA.GetSession(sessionId);
        }

        public List<Configuration_MessageStatus> GetMessageStatuses()
        {
            return sysConfigDA.GetMessageStatuses();
        }

        public List<Configuration_Province> GetProvinces()
        {
            return sysConfigDA.GetProvinces();
        }

        public List<Configuration_District> GetDistricts(Configuration_Province province)
        {
            return sysConfigDA.GetDistricts(province);
        }
    }
}
