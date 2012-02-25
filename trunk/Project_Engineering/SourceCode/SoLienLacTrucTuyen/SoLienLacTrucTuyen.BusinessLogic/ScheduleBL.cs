using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;

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
            /*
             * 1) Thêm thông tin TKB mới
             * 2) Thêm điểm môn học học kì của các học sinh thuộc lớp xếp TKB
             *  2.1) Kiểm tra TKB của môn này trong học kì này của lớp đã tồn tại chưa
             *      2.1.1) Nếu có, thì không xử lí
             *      2.1.2) Nếu chưa, thêm điểm môn học học kì của các học sinh thuộc lớp xếp TKB
             */

            StudyingResultBL studyingResultBL = new StudyingResultBL(school);

            Class_Schedule schedule = new Class_Schedule();
            schedule.ClassId = Class.ClassId;
            schedule.SubjectId = subject.SubjectId;
            schedule.TeacherId = teacher.UserId;
            schedule.TermId = term.TermId;
            schedule.DayInWeekId = dayInWeek.DayInWeekId;
            schedule.SessionId = teachingPeriod.SessionId;
            schedule.TeachingPeriodId = teachingPeriod.TeachingPeriodId;
            schedule.LastUpdate = DateTime.Now;

            bool bInsertStudentTermSubjectMark;
            bInsertStudentTermSubjectMark = ScheduleExists(Class, subject, term);

            // Insert new schedule
            scheduleDA.InsertSchedule(schedule);

            // Insert student's TermSubjectMark
            if (!bInsertStudentTermSubjectMark)
            {
                studyingResultBL.InsertTermSubjectMark(Class, subject, term);
            }

            // insert teacher to SubjectTeacherRole
            RoleBL roleBL = new RoleBL(school);
            roleBL.AddUserToSubjectTeacherRole(teacher);
        }

        public void UpdateSchedule(Class_Schedule schedule, Category_Subject subject, aspnet_User teacher)
        {
            /*
             * 1) Thêm thông tin TKB mới
             * 2) Thêm điểm môn học học kì của các học sinh thuộc lớp xếp TKB
             *  2.1) Kiểm tra TKB của môn này trong học kì này của lớp đã tồn tại chưa
             *      2.1.1) Nếu có, thì không xử lí
             *      2.1.2) Nếu chưa, thêm điểm môn học học kì của các học sinh thuộc lớp xếp TKB
             */

            /*
             * 1) Lưu lại thông tin TKB sẽ bị xóa
             * 2) Xóa thông tin TKB
             * 3) Xóa điểm môn học học kì và chi tiết điểm của các học sinh thuộc lớp xếp TKB
             *  3.1) Kiểm tra TKB của môn này trong học kì này của lớp còn tồn tại không
             *      3.1.1) Nếu còn, thì không xử lí
             *      3.1.2) Nếu ko, xóa điểm môn học học kì của các học sinh thuộc lớp xếp TKB
             */

            Class_Schedule originalSchedule = scheduleDA.GetSchedule(schedule.ScheduleId);

            bool bInsertStudentTermSubjectMark;
            bInsertStudentTermSubjectMark = ScheduleExists(originalSchedule.Class_Class, subject, originalSchedule.Configuration_Term);

            schedule.SubjectId = subject.SubjectId;
            schedule.TeacherId = teacher.UserId;
            schedule.LastUpdate = DateTime.Now;
            scheduleDA.UpdateSchedule(schedule, subject, teacher);

            // Xóa thông tin điểm của học sinh liên quan đến Môn học, Lớp học, Học kì
            StudyingResultBL studyingResultBL = new StudyingResultBL(school);
            if (!ScheduleExists(originalSchedule.Class_Class, originalSchedule.Category_Subject, originalSchedule.Configuration_Term))
            {
                studyingResultBL.DeleteTermSubjectMark(originalSchedule.Class_Class, originalSchedule.Configuration_Term, originalSchedule.Category_Subject);
            }

            // Insert student's TermSubjectMark
            if (!bInsertStudentTermSubjectMark)
            {
                studyingResultBL.InsertTermSubjectMark(originalSchedule.Class_Class, originalSchedule.Category_Subject, originalSchedule.Configuration_Term);
            }
        }

        public void DeleteSchedule(Class_Schedule schedule)
        {
            /*
             * 1) Lưu lại thông tin TKB sẽ bị xóa
             * 2) Xóa thông tin TKB
             * 3) Xóa điểm môn học học kì và chi tiết điểm của các học sinh thuộc lớp xếp TKB
             *  3.1) Kiểm tra TKB của môn này trong học kì này của lớp còn tồn tại không
             *      3.1.1) Nếu còn, thì không xử lí
             *      3.1.2) Nếu ko, xóa điểm môn học học kì của các học sinh thuộc lớp xếp TKB
             */

            schedule = GetSchedule(schedule.ScheduleId);

            // Xóa thời khóa biểu
            scheduleDA.DeleteSchedule(schedule);

            // Xóa thông tin điểm của học sinh liên quan đến Môn học, Lớp học, Học kì
            StudyingResultBL studyingResultBL = new StudyingResultBL(school);
            Class_Class Class = new Class_Class();
            Class.ClassId = schedule.ClassId;
            Category_Subject subject = new Category_Subject();
            subject.SubjectId = schedule.SubjectId;
            Configuration_Term term = new Configuration_Term();
            term.TermId = schedule.TermId;
            if (!ScheduleExists(Class, subject, term))
            {
                studyingResultBL.DeleteTermSubjectMark(Class, term, subject);
            }
        }

        public Class_Schedule GetSchedule(int scheduleId)
        {
            return scheduleDA.GetSchedule(scheduleId);
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

            Class = (new ClassBL(school)).GetClass(Class.ClassId);
            term = (new SystemConfigBL(school)).GetTerm(term.TermId);
            dayInweek = (new SystemConfigBL(school)).GetDayInWeek(dayInweek.DayInWeekId);

            foreach (Category_TeachingPeriod teachingPeriod in teachingPeriods)
            {
                schedule = scheduleDA.GetSchedule(Class, term, dayInweek, teachingPeriod);
                if (schedule != null)
                {
                    teachingPeriodSchedule = ConvertToTeachingPeriodSchedule(schedule);
                }
                else
                {   
                    teachingPeriodSchedule = new TeachingPeriodSchedule();
                    teachingPeriodSchedule.TempScheduleId = string.Format("{0}:{1}", dayInweek.DayInWeekId, teachingPeriod.TeachingPeriodId);
                    teachingPeriodSchedule.SubjectId = 0;
                    teachingPeriodSchedule.SubjectName = "Chưa xác định";
                    teachingPeriodSchedule.ScheduleId = 0;
                    teachingPeriodSchedule.TeacherName = "Chưa xác định";
                }

                teachingPeriodSchedule.ClassId = Class.ClassId;
                teachingPeriodSchedule.ClassName = Class.ClassName;
                teachingPeriodSchedule.YearName = Class.Configuration_Year.YearName;
                teachingPeriodSchedule.TermId = term.TermId;
                teachingPeriodSchedule.TermName = term.TermName;
                teachingPeriodSchedule.DayInWeekId = dayInweek.DayInWeekId;
                teachingPeriodSchedule.DayInWeekName = dayInweek.DayInWeekName; 
                teachingPeriodSchedule.SessionId = teachingPeriod.SessionId;
                teachingPeriodSchedule.SessionName = teachingPeriod.Configuration_Session.SessionName;
                teachingPeriodSchedule.TeachingPeriodId = teachingPeriod.TeachingPeriodId;
                teachingPeriodSchedule.TeachingPeriodName = teachingPeriod.TeachingPeriodName;
                teachingPeriodSchedule.StringDetailTeachingPeriod = teachingPeriodBL.GetDetailedTeachingPeriod(teachingPeriod);
                teachingPeriodSchedules.Add(teachingPeriodSchedule);
            }

            return teachingPeriodSchedules;
        }

        public List<TeachingPeriodSchedule> GetTeachingPeriodSchedules(Class_Class Class, Configuration_Term term)
        {
            TeachingPeriodBL teachingPeriodBL = new TeachingPeriodBL(school);
            List<TeachingPeriodSchedule> teachingPeriodSchedules = new List<TeachingPeriodSchedule>(); // result
            TeachingPeriodSchedule teachingPeriodSchedule = null; // variable for loop
            Class_Schedule schedule = null;

            List<Category_TeachingPeriod> teachingPeriods = teachingPeriodBL.GetTeachingPeriods();
            
            SystemConfigBL systemConfigBL = new SystemConfigBL(school);
            List<Configuration_DayInWeek> dayInWeeks = systemConfigBL.GetDayInWeeks();

            foreach (Category_TeachingPeriod teachingPeriod in teachingPeriods)
            {
                foreach (Configuration_DayInWeek dayInWeek in dayInWeeks)
                {
                    schedule = scheduleDA.GetSchedule(Class, term, dayInWeek, teachingPeriod);
                    if (schedule != null)
                    {
                        teachingPeriodSchedule = ConvertToTeachingPeriodSchedule(schedule);
                    }
                    else
                    {
                        teachingPeriodSchedule = new TeachingPeriodSchedule();
                        teachingPeriodSchedule.SubjectId = 0;
                        teachingPeriodSchedule.SubjectName = "(Nghỉ)";
                        teachingPeriodSchedule.ScheduleId = 0;
                        teachingPeriodSchedule.TeacherName = "Chưa xác định";
                    }

                    teachingPeriodSchedule.ClassId = Class.ClassId;
                    teachingPeriodSchedule.TermId = term.TermId;
                    teachingPeriodSchedule.TeachingPeriodId = teachingPeriod.TeachingPeriodId;
                    teachingPeriodSchedule.SessionId = teachingPeriod.SessionId;
                    teachingPeriodSchedule.SessionName = teachingPeriod.Configuration_Session.SessionName;
                    teachingPeriodSchedule.TeachingPeriodName = teachingPeriod.TeachingPeriodName;
                    teachingPeriodSchedule.StringDetailTeachingPeriod = teachingPeriodBL.GetDetailedTeachingPeriod(teachingPeriod);
                    teachingPeriodSchedule.DayInWeekId = dayInWeek.DayInWeekId;

                    teachingPeriodSchedules.Add(teachingPeriodSchedule);
                }
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
                    teachingPeriodSchedule.OrginalSubjectId = schedule.SubjectId;
                    teachingPeriodSchedule.SubjectId = schedule.SubjectId;
                    teachingPeriodSchedule.SubjectName = schedule.Category_Subject.SubjectName;
                    teachingPeriodSchedule.OrginalUserId = schedule.TeacherId;
                    teachingPeriodSchedule.UserId = schedule.TeacherId;
                    teachingPeriodSchedule.TeacherName = schedule.aspnet_User.aspnet_Membership.FullName;
                    teachingPeriodSchedule.ClassId = schedule.ClassId;
                    teachingPeriodSchedule.TermId = schedule.TermId;
                    teachingPeriodSchedule.DayInWeekId = schedule.DayInWeekId;
                    teachingPeriodSchedule.TeachingPeriodId = schedule.TeachingPeriodId;
                    teachingPeriod = teachingPeriodBL.GetTeachingPeriod(schedule.TeachingPeriodId);
                    teachingPeriodSchedule.StringDetailTeachingPeriod = teachingPeriod.TeachingPeriodName;
                    teachingPeriodSchedule.SessionId = teachingPeriod.SessionId;

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

        private bool ScheduleExists(Class_Class Class, Category_Subject subject, Configuration_Term term)
        {
            return scheduleDA.ScheduleExists(Class, subject, term);
        }

        public bool ScheduleExists(Class_Class Class, Category_Subject subject, Configuration_Term term, Configuration_DayInWeek dayInweek, Configuration_Session session)
        {
            return scheduleDA.ScheduleExists(Class, subject, term, dayInweek, session);
        }

        public bool ScheduleExists(aspnet_User teacher)
        {
            return scheduleDA.ScheduleExists(teacher);
        }

        public bool ScheduleExists(Category_Subject subject)
        {
            return scheduleDA.ScheduleExists(subject);
        }

        public bool ScheduleExists(Category_TeachingPeriod teachingPeriod)
        {
            return scheduleDA.ScheduleExists(teachingPeriod);
        }

        private TeachingPeriodSchedule ConvertToTeachingPeriodSchedule(Class_Schedule schedule)
        {
            TeachingPeriodSchedule teachingPeriodSchedule = new TeachingPeriodSchedule();

            teachingPeriodSchedule.ScheduleId = schedule.ScheduleId;
            teachingPeriodSchedule.ClassId = schedule.ClassId;
            teachingPeriodSchedule.ClassName = schedule.Class_Class.ClassName;
            teachingPeriodSchedule.TermId = schedule.TermId;
            teachingPeriodSchedule.TermName = schedule.Configuration_Term.TermName;
            teachingPeriodSchedule.SubjectId = schedule.SubjectId;
            teachingPeriodSchedule.OrginalSubjectId = schedule.SubjectId;
            teachingPeriodSchedule.SubjectName = schedule.Category_Subject.SubjectName;
            teachingPeriodSchedule.UserId = schedule.TeacherId;
            teachingPeriodSchedule.OrginalUserId = schedule.TeacherId;
            teachingPeriodSchedule.TeacherName = schedule.aspnet_User.aspnet_Membership.FullName;
            teachingPeriodSchedule.TeachingPeriodId = schedule.Category_TeachingPeriod.TeachingPeriodId;
            teachingPeriodSchedule.DayInWeekId = schedule.Configuration_DayInWeek.DayInWeekId;
            teachingPeriodSchedule.DayInWeekName = schedule.Configuration_DayInWeek.DayInWeekName;
            teachingPeriodSchedule.YearName = schedule.Class_Class.Configuration_Year.YearName;
            teachingPeriodSchedule.SessionId = schedule.SessionId;
            return teachingPeriodSchedule;
        }

        /// <summary>
        /// Update schedule of class
        /// </summary>
        /// <param name="weeklySchedule">Weekly schedule that contains both before and after change information</param>
        public void UpdateSchedule(List<List<TeachingPeriodSchedule>> weeklySchedule)
        {
            // Declare variables
            StudyingResultBL studyingResultBL = new StudyingResultBL(school);
            RoleBL roleBL = new RoleBL(school);
            TeacherBL teacherBL = new TeacherBL(school);

            List<int> beforeChangedSubjectIds = new List<int>();
            List<int> afterChangedSubjectIds = new List<int>();
            List<int> deletedScheduledSubjectIds = new List<int>();
            List<int> addedScheduledSubjectIds = new List<int>();
            Class_Schedule schedule = null;
            aspnet_User orginalTeacher = null;
            aspnet_User teacher = null;            
            Category_Subject subject = null;
            
            // Initialize Class
            Class_Class Class = new Class_Class();
            Class.ClassId = weeklySchedule[0][0].ClassId;

            // Initialize term
            Configuration_Term term = new Configuration_Term();
            term.TermId = weeklySchedule[0][0].TermId;

            foreach (List<TeachingPeriodSchedule> dailySchedule in weeklySchedule)
            {
                foreach (TeachingPeriodSchedule teachingPeriodlySchedule in dailySchedule)
                {
                    // teachingPeriodlySchedule is new or unarranged
                    if (teachingPeriodlySchedule.ScheduleId == 0)                    
                    {
                        // teachingPeriodlySchedule is new
                        if (teachingPeriodlySchedule.SubjectId != 0)
                        {
                            // Add new subject id to list
                            afterChangedSubjectIds.Add(teachingPeriodlySchedule.SubjectId);

                            schedule = new Class_Schedule();
                            schedule.ClassId = Class.ClassId;
                            schedule.SubjectId = teachingPeriodlySchedule.SubjectId;
                            schedule.TeacherId = teachingPeriodlySchedule.UserId;
                            schedule.TermId = term.TermId;
                            schedule.DayInWeekId = teachingPeriodlySchedule.DayInWeekId;
                            schedule.SessionId = teachingPeriodlySchedule.SessionId;
                            schedule.TeachingPeriodId = teachingPeriodlySchedule.TeachingPeriodId;

                            // Insert new schedule
                            scheduleDA.InsertSchedule(schedule);

                            // insert teacher to SubjectTeacherRole
                            teacher = new aspnet_User();
                            teacher.UserId = teachingPeriodlySchedule.UserId;
                            roleBL.AddUserToSubjectTeacherRole(teacher);
                        }
                    }
                    else
                    {
                        beforeChangedSubjectIds.Add(teachingPeriodlySchedule.OrginalSubjectId);

                        if (teachingPeriodlySchedule.SubjectId == 0)
                        {
                            schedule = new Class_Schedule();
                            schedule.ScheduleId = teachingPeriodlySchedule.ScheduleId;

                            // Xóa thời khóa biểu
                            scheduleDA.DeleteSchedule(schedule);

                            // remove teacher from SubjectTeacherRole                            
                            orginalTeacher = new aspnet_User();
                            orginalTeacher.UserId = teachingPeriodlySchedule.OrginalUserId;
                            if (teacherBL.IsTeaching(orginalTeacher) == false)
                            {
                                roleBL.RemoveUserFromSubjectTeacherRole(orginalTeacher);
                            }
                        }
                        else
                        {
                            afterChangedSubjectIds.Add(teachingPeriodlySchedule.SubjectId);

                            if ((teachingPeriodlySchedule.OrginalSubjectId != teachingPeriodlySchedule.SubjectId)
                                || (teachingPeriodlySchedule.OrginalUserId != teachingPeriodlySchedule.UserId))
                            {
                                // Modify
                                schedule = new Class_Schedule();
                                schedule.ScheduleId = teachingPeriodlySchedule.ScheduleId;
                                teacher = new aspnet_User();
                                teacher.UserId = teachingPeriodlySchedule.UserId;
                                subject = new Category_Subject();
                                subject.SubjectId = teachingPeriodlySchedule.SubjectId;

                                scheduleDA.UpdateSchedule(schedule, subject, teacher);

                                // insert teacher to SubjectTeacherRole
                                teacher = new aspnet_User();
                                teacher.UserId = teachingPeriodlySchedule.UserId;
                                roleBL.AddUserToSubjectTeacherRole(teacher);

                                // remove teacher from SubjectTeacherRole                            
                                orginalTeacher = new aspnet_User();
                                orginalTeacher.UserId = teachingPeriodlySchedule.OrginalUserId;
                                if (teacherBL.IsTeaching(orginalTeacher) == false)
                                {
                                    roleBL.RemoveUserFromSubjectTeacherRole(orginalTeacher);
                                }
                            }
                        }
                    }
                }
            }

            foreach (int orginalSubjectId in beforeChangedSubjectIds)
            {
                if (afterChangedSubjectIds.Contains(orginalSubjectId) == false)
                {
                    deletedScheduledSubjectIds.Add(orginalSubjectId);
                }
            }

            foreach (int subjectId in afterChangedSubjectIds)
            {
                if (beforeChangedSubjectIds.Contains(subjectId) == false)
                {
                    addedScheduledSubjectIds.Add(subjectId);
                }
            }

            foreach (int deletedScheduledSubjectId in deletedScheduledSubjectIds)
            {
                // Xóa thông tin điểm của học sinh liên quan đến Môn học, Lớp học, Học kì
                subject = new Category_Subject();
                subject.SubjectId = deletedScheduledSubjectId;
                studyingResultBL.DeleteTermSubjectMark(Class, term, subject);
            }

            foreach (int addedScheduledSubjectId in addedScheduledSubjectIds)
            {
                // Thêm thông tin điểm của học sinh liên quan đến Môn học, Lớp học, Học kì
                subject = new Category_Subject();
                subject.SubjectId = addedScheduledSubjectId;
                studyingResultBL.InsertTermSubjectMark(Class, subject, term);
            }
        }

        public bool IsScheduleUpdated(Student_Student student, Configuration_Year lastYear)
        {
            StudentBL studentBL = new StudentBL(school);

            Class_Class Class = studentBL.GetClass(student, lastYear);
            SystemConfigBL systemConfigBL = new SystemConfigBL(school);
            int iLimitDay = 3;
            DateTime? dtLastUpdateDate = scheduleDA.GetLastUpdateDate(Class);
            if (dtLastUpdateDate != null)
            {
                if (DateUtils.CompareDateWithoutHMS(DateTime.Now, ((DateTime)dtLastUpdateDate).AddDays(iLimitDay)) <= 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
