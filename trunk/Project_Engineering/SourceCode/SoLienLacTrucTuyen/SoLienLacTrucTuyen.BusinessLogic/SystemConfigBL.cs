using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class SystemConfigBL : BaseBL
    {
        private SystemConfigDA sysConfigDA;

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
            return sysConfigDA.GetCurrentTerm();
        }

        public void UpdateTermIdHienHanh(int TermIdHienHanh)
        {
            sysConfigDA.UpdateTermIdHienHanh(TermIdHienHanh);
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

        public List<ConfigurationMessageStatus> GetMessageStatuses()
        {
            return sysConfigDA.GetMessageStatuses();             
        }
        
        public List<ConfigurationProvince> GetProvinces()
        {
            return sysConfigDA.GetProvinces();
        }

        public List<ConfigurationDistrict> GetDistricts(ConfigurationProvince province)
        {
            return sysConfigDA.GetDistricts(province);
        }
    }
}
