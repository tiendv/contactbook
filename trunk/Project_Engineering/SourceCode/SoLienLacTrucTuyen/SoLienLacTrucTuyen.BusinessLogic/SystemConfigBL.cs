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
            return sysConfigDA.GetYears();
        }

        public Configuration_Year GetLastedYear()
        {
            return sysConfigDA.GetLastedYear();
        }

        public Configuration_Year GetPreviousYear()
        {
            return sysConfigDA.GetPreviousYear();
        }

        public List<TabularYear> GetYears(string yearName, int pageIndex, int pageSize, out double totalRecords)
        {
            List<TabularYear> tabularYears = new List<TabularYear>();
            TabularYear tabularYear = null;
            List<Configuration_Year> years = new List<Configuration_Year>();

            if (CheckUntils.IsNullOrBlank(yearName))
            {
                years = sysConfigDA.GetYears(pageIndex, pageSize, out totalRecords);
            }
            else
            {
                years = sysConfigDA.GetYears(yearName, pageIndex, pageSize, out totalRecords);
            }

            foreach (Configuration_Year year in years)
            {
                tabularYear = new TabularYear();
                tabularYear.YearId = year.YearId;
                tabularYear.YearName = year.YearName;
                tabularYear.FirstTermBeginDate = year.BeginFirstTermDate;
                tabularYear.FirstTermEndDate = year.EndFirstTermDate;
                tabularYear.SecondTermBeginDate = year.BeginSecondTermDate;
                tabularYear.SecondTermEndDate = year.EndSecondTermDate;
                tabularYear.StringFirstTermBeginDate = DateUtils.DateToString(year.EndSecondTermDate, DateUtils.LOCALE.VIETNAMESE);
                tabularYear.StringFirstTermEndDate = DateUtils.DateToString(year.EndFirstTermDate, DateUtils.LOCALE.VIETNAMESE);
                tabularYear.StringSecondTermBeginDate = DateUtils.DateToString(year.BeginSecondTermDate, DateUtils.LOCALE.VIETNAMESE);
                tabularYear.StringSecondTermEndDate = DateUtils.DateToString(year.EndSecondTermDate, DateUtils.LOCALE.VIETNAMESE);
                
                tabularYears.Add(tabularYear);
            }

            return tabularYears;
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
            Configuration_Term currentTerm = null;
            Configuration_Year currentYear = GetLastedYear();
            if (currentYear != null)
            {
                if (DateTime.Now < currentYear.BeginSecondTermDate)
                {
                    currentTerm = GetTerm(1);
                }
                else
                {
                    currentTerm = GetTerm(2);
                }
            }

            return currentTerm;
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

        public bool IsDeletable(Configuration_Year year)
        {
            return sysConfigDA.IsDeletable(year);
        }

        public void DeleteYear(Configuration_Year year)
        {
            sysConfigDA.DeleteYear(year);
        }

        public void InsertYear(string yearName, DateTime firstTermBeginDate, DateTime firstTermEndDate, DateTime secondTermBeginDate, DateTime secondTermEndDate)
        {
            Configuration_Year year = new Configuration_Year();
            year.YearName = yearName;
            year.BeginFirstTermDate = firstTermBeginDate;
            year.EndFirstTermDate = firstTermEndDate;
            year.BeginSecondTermDate = secondTermBeginDate;
            year.EndSecondTermDate = secondTermEndDate;
            year.SchoolId = school.SchoolId;

            sysConfigDA.InsertYear(year);
        }

        public void UpdateYear(Configuration_Year year, DateTime firstTermBeginDate, DateTime firstTermEndDate, DateTime secondTermBeginDate, DateTime secondTermEndDate)
        {
            sysConfigDA.UpdateYear(year, firstTermBeginDate, firstTermEndDate, secondTermBeginDate, secondTermEndDate);
        }
    }
}
