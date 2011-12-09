﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class ScheduleBL : BaseBL
    {
        private ScheduleDA scheduleDA;

        public ScheduleBL(School_School school)
            : base(school)
        {
            scheduleDA = new ScheduleDA(school);
        }

        public void InsertSchedule(Class_Class Class, Category_Subject subject, aspnet_User teacher, Configuration_Term term, Configuration_DayInWeek dayInWeek, Category_TeachingPeriod teachingPeriod)
        {
            Class_Schedule schedule = new Class_Schedule();
            schedule.ClassId = Class.ClassId;
            schedule.SubjectId = subject.SubjectId;
            schedule.TeacherId = teacher.UserId;
            schedule.TermId = term.TermId;
            schedule.DayInWeekId = dayInWeek.DayInWeekId;
            schedule.SessionId = teachingPeriod.SessionId;
            schedule.TeachingPeriodId = teachingPeriod.TeachingPeriodId;

            scheduleDA.InsertSchedule(schedule);
        }

        public void UpdateSchedule(Class_Schedule editedSchedule, Category_Subject subject, aspnet_User teacher)
        {
            editedSchedule.SubjectId = subject.SubjectId;
            editedSchedule.TeacherId = teacher.UserId;
            scheduleDA.UpdateSchedule(editedSchedule, subject, teacher);
        }

        public void DeleteSchedule(Class_Schedule deletedSchedule)
        {
            scheduleDA.DeleteSchedule(deletedSchedule);
        }

        public bool ScheduleExists(Class_Class Class, Category_Subject subject, Configuration_Term term, Configuration_DayInWeek dayInweek, Configuration_Session session)
        {
            return scheduleDA.ScheduleExists(Class, subject, term, dayInweek, session);
        }

        public bool ScheduleExists(aspnet_User teacher)
        {
            return scheduleDA.ScheduleExists(teacher);
        }

        public List<Class_Schedule> GetSchedules(Class_Class Class, Configuration_Term term, Configuration_DayInWeek dayInweek, Configuration_Session session)
        {
            return scheduleDA.GetSchedules(Class, term, dayInweek, session);
        }

        public List<TeachingPeriodSchedule> GetTeachingPeriodSchedules(Class_Class Class, Configuration_Term term, Configuration_DayInWeek dayInweek)
        {
            TeachingPeriodBL teachingPeriodBL = new TeachingPeriodBL(school);
            List<TeachingPeriodSchedule> teachingPeriodSchedules = new List<TeachingPeriodSchedule>();
            TeachingPeriodSchedule teachingPeriodSchedule = null;
            Class_Schedule schedule = null;
            List<Category_TeachingPeriod> teachingPeriods = teachingPeriodBL.GetTeachingPeriods();

            foreach (Category_TeachingPeriod teachingPeriod in teachingPeriods)
            {
                schedule = scheduleDA.GetSchedule(Class, term, dayInweek, teachingPeriod);
                if (schedule != null)
                {
                    teachingPeriodSchedule = GetTeachingPeriodSchedule(schedule);
                }
                else
                {
                    teachingPeriodSchedule = new TeachingPeriodSchedule();
                    teachingPeriodSchedule.SubjectId = 0;
                    teachingPeriodSchedule.ScheduleId = 0;
                }

                teachingPeriodSchedule.TeachingPeriodId = teachingPeriod.TeachingPeriodId;
                teachingPeriodSchedule.StringDetailTeachingPeriod = teachingPeriodBL.GetDetailedTeachingPeriod(teachingPeriod);
                teachingPeriodSchedules.Add(teachingPeriodSchedule);
            }

            return teachingPeriodSchedules;
        }

        public List<TeachingPeriodSchedule> GetTeachingPeriodSchedules(Class_Class Class, Configuration_Term term, Configuration_DayInWeek dayInweek, Configuration_Session session)
        {
            TeachingPeriodBL tietBL = new TeachingPeriodBL(school);
            List<TeachingPeriodSchedule> teachingPeriodSchedules = new List<TeachingPeriodSchedule>();
            List<Class_Schedule> schedules = scheduleDA.GetSchedules(Class, term, dayInweek, session);
            TeachingPeriodSchedule teachingPeriodSchedule = null;
            Category_TeachingPeriod teachingPeriod = null;

            foreach (Class_Schedule schedule in schedules)
            {
                teachingPeriodSchedule = GetTeachingPeriodSchedule(schedule);

                teachingPeriod = schedule.Category_TeachingPeriod;
                teachingPeriodSchedule.StringDetailTeachingPeriod = string.Format("{0}({1}-{2})",
                    teachingPeriod.TeachingPeriodName,
                    teachingPeriod.EndTime.ToShortTimeString(),
                    teachingPeriod.EndTime.ToShortTimeString());
            }

            return teachingPeriodSchedules;
        }

        public List<SessionedSchedule> GetSessionedSchedules(Class_Class Class, Configuration_Term term, Configuration_DayInWeek dayInweek)
        {
            List<SessionedSchedule> sessionedSchedules = new List<SessionedSchedule>();
            List<TeachingPeriodSchedule> teachingPeriodSchedules = new List<TeachingPeriodSchedule>();
            List<Class_Schedule> schedules = new List<Class_Schedule>();
            SessionedSchedule sessionedSchedule = null;
            Category_TeachingPeriod teachingPeriod = null;
            SystemConfigBL systemConfigBL = new SystemConfigBL(school);
            TeachingPeriodBL teachingPeriodBL = new TeachingPeriodBL(school);
            List<Configuration_Session> sessions = systemConfigBL.GetSessions();

            foreach (Configuration_Session session in sessions)
            {
                sessionedSchedule = new SessionedSchedule();
                sessionedSchedule.SessionId = session.SessionId;
                schedules = GetSchedules(Class, term, dayInweek, session);
                teachingPeriodSchedules = new List<TeachingPeriodSchedule>();
                foreach (Class_Schedule schedule in schedules)
                {
                    TeachingPeriodSchedule teachingPeriodSchedule = new TeachingPeriodSchedule();
                    teachingPeriodSchedule.ScheduleId = schedule.ScheduleId;
                    teachingPeriodSchedule.SubjectId = schedule.SubjectId;
                    teachingPeriodSchedule.SubjectName = schedule.Category_Subject.SubjectName;
                    teachingPeriodSchedule.UserId = schedule.TeacherId;
                    teachingPeriodSchedule.TeacherName = schedule.aspnet_User.aspnet_Membership.FullName;
                    teachingPeriodSchedule.ClassId = schedule.ClassId;
                    teachingPeriodSchedule.TermId = schedule.TermId;
                    teachingPeriodSchedule.DayInWeekId = schedule.DayInWeekId;
                    teachingPeriodSchedule.TeachingPeriodId = schedule.TeachingPeriodId;
                    teachingPeriod = teachingPeriodBL.GetTeachingPeriod(schedule.TeachingPeriodId);
                    teachingPeriodSchedule.StringDetailTeachingPeriod = teachingPeriodBL.GetDetailedTeachingPeriod(teachingPeriod);

                    teachingPeriodSchedules.Add(teachingPeriodSchedule);
                }
                sessionedSchedule.ListThoiKhoaBieuTheoTiet = teachingPeriodSchedules;
                sessionedSchedules.Add(sessionedSchedule);
            }

            return sessionedSchedules;
        }

        public List<DailySchedule> GetDailySchedules(Class_Class Class, Configuration_Term term)
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(school);
            List<DailySchedule> dailySchedules = new List<DailySchedule>();
            List<Configuration_DayInWeek> dayInWeeks = systemConfigBL.GetDayInWeeks();
            DailySchedule dailySchedule = null;

            foreach (Configuration_DayInWeek dayInWeek in dayInWeeks)
            {
                dailySchedule = new DailySchedule();
                dailySchedule.ClassId = Class.ClassId;
                dailySchedule.TermId = term.TermId;
                dailySchedule.DayInWeekId = dayInWeek.DayInWeekId;
                dailySchedule.DayInWeekName = dayInWeek.DayInWeekName;
                dailySchedule.SessionedSchedules = GetSessionedSchedules(Class, term, dayInWeek);
                dailySchedules.Add(dailySchedule);
            }

            return dailySchedules;
        }

        public List<Category_Subject> GetScheduledSubjects(Class_Class Class, Configuration_Term term)
        {
            return scheduleDA.GetScheduledSubjects(Class, term);
        }

        public TeachingPeriodSchedule GetTeachingPeriodSchedule(Class_Schedule schedule)
        {
            TeachingPeriodSchedule teachingPeriodSchedule = new TeachingPeriodSchedule();

            teachingPeriodSchedule.ScheduleId = schedule.ScheduleId;
            teachingPeriodSchedule.ClassId = schedule.ClassId;
            teachingPeriodSchedule.SubjectId = schedule.SubjectId;
            teachingPeriodSchedule.SubjectName = schedule.Category_Subject.SubjectName;
            teachingPeriodSchedule.UserId = schedule.TeacherId;
            teachingPeriodSchedule.TeacherName = schedule.aspnet_User.aspnet_Membership.FullName;
            teachingPeriodSchedule.TeachingPeriodId = schedule.Category_TeachingPeriod.TeachingPeriodId;
            teachingPeriodSchedule.DayInWeekId = schedule.Configuration_DayInWeek.DayInWeekId;

            return teachingPeriodSchedule;
        }

        public Class_Schedule GetSchedule(int scheduleId)
        {
            return scheduleDA.GetSchedule(scheduleId);
        }
    }
}