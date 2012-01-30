using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.BusinessEntity;

namespace EContactBook.DataAccess
{
    public class ScheduleDA : BaseDA
    {
        public ScheduleDA(School_School school)
            : base(school)
        {
        }

        public void InsertSchedule(Class_Schedule schedule)
        {
            schedule.LastUpdate = DateTime.Now;
            db.Class_Schedules.InsertOnSubmit(schedule);
            db.SubmitChanges();
        }

        public void UpdateSchedule(Class_Schedule editedSchedule, Category_Subject subject, aspnet_User teacher)
        {
            Class_Schedule schedule = (from schdl in db.Class_Schedules
                                       where schdl.ScheduleId == editedSchedule.ScheduleId
                                       select schdl).First();
            schedule.LastUpdate = DateTime.Now;
            schedule.SubjectId = subject.SubjectId;
            schedule.TeacherId = teacher.UserId;
            db.SubmitChanges();
        }

        public void DeleteSchedule(Class_Schedule deletedSchedule)
        {
            Class_Schedule schedule = null;
            IQueryable<Class_Schedule> iqSchedule = from schd in db.Class_Schedules
                                                    where schd.ScheduleId == deletedSchedule.ScheduleId
                                                    select schd;
            if (iqSchedule.Count() != 0)
            {
                schedule = iqSchedule.First();
                db.Class_Schedules.DeleteOnSubmit(schedule);
                db.SubmitChanges();
            }
        }

        public Class_Schedule GetSchedule(int scheduleId)
        {
            Class_Schedule schedule = null;

            IQueryable<Class_Schedule> iqSchedule = from schd in db.Class_Schedules
                                                    where schd.ScheduleId == scheduleId
                                                    && schd.aspnet_User.aspnet_Membership.SchoolId == school.SchoolId
                                                    select schd;
            if (iqSchedule.Count() != 0)
            {
                schedule = iqSchedule.First();
            }

            return schedule;
        }

        public Class_Schedule GetSchedule(Class_Class Class, Configuration_Term term, Configuration_DayInWeek dayInweek, Category_TeachingPeriod teachingPeriod)
        {
            Class_Schedule schedule = null;
            IQueryable<Class_Schedule> iqSchedule = from schd in db.Class_Schedules
                                                    where schd.ClassId == Class.ClassId && schd.DayInWeekId == dayInweek.DayInWeekId
                                                    && schd.TermId == term.TermId
                                                    && schd.TeachingPeriodId == teachingPeriod.TeachingPeriodId
                                                    select schd;

            if (iqSchedule.Count() != 0)
            {
                schedule = iqSchedule.First();
            }

            return schedule;
        }

        public List<Class_Schedule> GetSchedules(Class_Class Class, Configuration_Term term, Configuration_DayInWeek dayInweek, Configuration_Session session)
        {
            List<Class_Schedule> schedules = new List<Class_Schedule>();

            IQueryable<Class_Schedule> iqSchedule = from schd in db.Class_Schedules
                                                    where schd.ClassId == Class.ClassId && schd.TermId == term.TermId
                                                    && schd.DayInWeekId == dayInweek.DayInWeekId && schd.SessionId == session.SessionId
                                                    select schd;
            if (iqSchedule.Count() != 0)
            {
                schedules = iqSchedule.OrderBy(teachingPeriod => teachingPeriod.TeachingPeriodId).ToList();
            }

            return schedules;
        }

        public List<Category_Subject> GetScheduledSubjects(Class_Class Class, Configuration_Term term)
        {
            List<Category_Subject> scheduledSubjects = new List<Category_Subject>();

            IQueryable<Category_Subject> iqScheduledSubject;
            iqScheduledSubject = from subj in db.Category_Subjects
                                 join schedule in db.Class_Schedules on subj.SubjectId equals schedule.SubjectId
                                 where schedule.ClassId == Class.ClassId && schedule.TermId == term.TermId
                                 select subj;

            if (iqScheduledSubject.Count() != 0)
            {
                scheduledSubjects = iqScheduledSubject.OrderBy(subj => subj.SubjectName)
                    .GroupBy(c => c.SubjectId).Select(g => g.First()).ToList();
            }

            return scheduledSubjects;
        }

        public bool ScheduleExists(Class_Class Class, Category_Subject subject, Configuration_Term term, Configuration_DayInWeek dayInweek, Configuration_Session session)
        {
            IQueryable<Class_Schedule> iqSchedule = from schd in db.Class_Schedules
                                                    where schd.ClassId == Class.ClassId
                                                    && schd.SubjectId == subject.SubjectId
                                                    && schd.TermId == term.TermId
                                                    && schd.DayInWeekId == dayInweek.DayInWeekId
                                                    && schd.SessionId == session.SessionId
                                                    select schd;

            if (iqSchedule.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ScheduleExists(aspnet_User teacher)
        {
            IQueryable<Class_Schedule> iqSchedule = from schd in db.Class_Schedules
                                                    where schd.TeacherId == teacher.UserId
                                                    select schd;

            if (iqSchedule.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ScheduleExists(Class_Class Class, Category_Subject subject, Configuration_Term term)
        {
            IQueryable<Class_Schedule> iqSchedule = from schedule in db.Class_Schedules
                                                    where schedule.ClassId == Class.ClassId
                                                        && schedule.SubjectId == subject.SubjectId
                                                        && schedule.TermId == term.TermId
                                                    select schedule;
            if (iqSchedule.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ScheduleExists(Category_Subject subject)
        {
            IQueryable<Class_Schedule> iqScheduledSubjects = from scheduledSubject in db.Class_Schedules
                                                             where scheduledSubject.SubjectId == subject.SubjectId
                                                             select scheduledSubject;

            if (iqScheduledSubjects.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ScheduleExists(Category_TeachingPeriod teachingPeriod)
        {
            IQueryable<Class_Schedule> iqScheduledSubjects = from scheduledSubject in db.Class_Schedules
                                                             where scheduledSubject.TeachingPeriodId == teachingPeriod.TeachingPeriodId
                                                             select scheduledSubject;

            if (iqScheduledSubjects.Count() != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public DateTime? GetLastUpdateDate(Class_Class Class)
        {
            DateTime? lastUpdateDate = null; 
            IQueryable<Class_Schedule> querySchedule = from schedule in db.Class_Schedules
                                                       where schedule.ClassId == Class.ClassId
                                                       select schedule;

            if (querySchedule.Count() != 0)
            {
                lastUpdateDate = querySchedule.OrderByDescending(schedule => schedule.LastUpdate).First().LastUpdate;
            }

            return lastUpdateDate;
        }
    }
}
