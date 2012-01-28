using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class TeachingPeriodBL: BaseBL
    {
        private TeachingPeriodDA teachingPeriodDA;

        public TeachingPeriodBL(School_School school): base(school)
        {
            teachingPeriodDA = new TeachingPeriodDA(school);
        }

        public void DeleteTeachingPeriod(Category_TeachingPeriod teachingPeriod)
        {
            teachingPeriodDA.DeleteTeachingPeriod(teachingPeriod);
        }

        public void InsertTeachingPeriod(string teachingPeriodName, Configuration_Session session, string order, string beginTime, string endTime)
        {
            string[] strBeginTimes = beginTime.Split(':');
            int iBeginHour = Int32.Parse(strBeginTimes[0]);
            int iBeginMinute = Int32.Parse(strBeginTimes[1]);
            DateTime dtBeginTime = new DateTime(2000, 1, 1, iBeginHour, iBeginMinute, 0);

            string[] strEndTimes = endTime.Split(':');
            int iEndHour = Int32.Parse(strEndTimes[0]);
            int iEndMinute = Int32.Parse(strEndTimes[1]);
            DateTime dtEndTime = new DateTime(2000, 1, 1, iEndHour, iEndMinute, 0);

            int iOrder = Int32.Parse(order);

            Category_TeachingPeriod teachingPeriod = new Category_TeachingPeriod();
            teachingPeriod.TeachingPeriodName = teachingPeriodName;
            teachingPeriod.SessionId = session.SessionId;
            teachingPeriod.BeginTime = dtBeginTime;
            teachingPeriod.EndTime = dtEndTime;

            teachingPeriodDA.InsertTeachingPeriod(teachingPeriod);
        }

        public void UpdateTiet(Category_TeachingPeriod editedTeachingPeriod, string newTeachingPeriodName, Configuration_Session newSession, string newOrder, string newBeginTime, string newEndTime)
        {
            string[] strBeginTimes = newBeginTime.Split(':');
            int iBeginHour = Int32.Parse(strBeginTimes[0]);
            int iBeginMinute = Int32.Parse(strBeginTimes[1]);
            DateTime dtBeginTime = new DateTime(2000, 1, 1, iBeginHour, iBeginMinute, 0);

            string[] strEndTimes = newEndTime.Split(':');
            int iEndHour = Int32.Parse(strEndTimes[0]);
            int iEndMinute = Int32.Parse(strEndTimes[1]);
            DateTime dtEndTime = new DateTime(2000, 1, 1, iEndHour, iEndMinute, 0);

            int iOrder = Int32.Parse(newOrder);

            editedTeachingPeriod.TeachingPeriodName = newTeachingPeriodName;
            editedTeachingPeriod.SessionId = newSession.SessionId;
            editedTeachingPeriod.BeginTime = dtBeginTime;
            editedTeachingPeriod.EndTime = dtEndTime;

            teachingPeriodDA.UpdateTeachingPeriod(editedTeachingPeriod);
        }

        public Category_TeachingPeriod GetTeachingPeriod(int teachingPeriodId)
        {
            return teachingPeriodDA.GetTeachingPeriod(teachingPeriodId);
        }

        public List<Category_TeachingPeriod> GetTeachingPeriods()
        {
            return teachingPeriodDA.GetTeachingPeriods();
        }

        public List<TabularTeachingPeriod> GetTabularTeachingPeriods(string teachingPeriodName, Configuration_Session session, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<Category_TeachingPeriod> lTeachingPeriods = new List<Category_TeachingPeriod>();
            List<TabularTeachingPeriod> lTbTeachingPeriods = new List<TabularTeachingPeriod>();

            if ((teachingPeriodName == "") || (string.Compare(teachingPeriodName, "tất cả", 0) == 0))
            {
                if (session == null)
                {
                    lTeachingPeriods = teachingPeriodDA.GetTeachingPeriods(pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    lTeachingPeriods = teachingPeriodDA.GetTeachingPeriods(session, pageCurrentIndex, pageSize, out totalRecords);
                }
            }
            else
            {
                if (session == null)
                {
                    lTeachingPeriods = teachingPeriodDA.GetTeachingPeriods(teachingPeriodName, pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    lTeachingPeriods = teachingPeriodDA.GetTeachingPeriods(teachingPeriodName, session, pageCurrentIndex, pageSize, out totalRecords);
                }
            }

            TabularTeachingPeriod tbTeachingPeriod = null;
            foreach (Category_TeachingPeriod teachingPeriod in lTeachingPeriods)
            {
                tbTeachingPeriod = new TabularTeachingPeriod();
                tbTeachingPeriod.TeachingPeriodId = teachingPeriod.TeachingPeriodId;
                tbTeachingPeriod.TeachingPeriodName = teachingPeriod.TeachingPeriodName;
                tbTeachingPeriod.SessionId = teachingPeriod.Configuration_Session.SessionId;
                tbTeachingPeriod.SessionName = teachingPeriod.Configuration_Session.SessionName;
                tbTeachingPeriod.TeachingPeriodOrder = teachingPeriod.TeachingPeriodOrder;
                tbTeachingPeriod.BeginTime = teachingPeriod.BeginTime;
                tbTeachingPeriod.EndTime = teachingPeriod.EndTime;

                lTbTeachingPeriods.Add(tbTeachingPeriod);
            }

            return lTbTeachingPeriods;
        }

        public bool IsDeletable(Category_TeachingPeriod teachingPeriod)
        {
            ScheduleBL scheduleBL = new ScheduleBL(school);
            return scheduleBL.ScheduleExists(teachingPeriod);
        }

        public string GetDetailedTeachingPeriod(Category_TeachingPeriod teachingPeriod)
        {
            string chiTietTiet = string.Format("<b>{0}</b>&nbsp;({1}-{2})",
                    teachingPeriod.TeachingPeriodName,
                    teachingPeriod.BeginTime.ToShortTimeString(),
                    teachingPeriod.EndTime.ToShortTimeString());
            return chiTietTiet;
        }

        public bool TeachingPeriodNameExists(string teachingPeriodName)
        {
            return teachingPeriodDA.TeachingPeriodNameExists(teachingPeriodName);
        }

        public bool TeachingPeriodNameExists(string oldTeachingPeriodName, string newTeachingPeriodName)
        {
            if (oldTeachingPeriodName == newTeachingPeriodName)
            {
                return false;
            }
            else
            {
                return teachingPeriodDA.TeachingPeriodNameExists(newTeachingPeriodName);
            }
        }

        public void SetSession(School_School school)
        {
            this.school = school;
        }

        public int GetTeachingPeriodCount()
        {
            return teachingPeriodDA.GetTeachingPeriodCount();
        }
    }
}
