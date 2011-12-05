using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class ScheduleDA : BaseDA
    {
        public ScheduleDA(School_School school)
            : base(school)
        {
        }

        public void InsertSchedule(Class_Schedule schedule)
        {
            // Insert new schedule
            db.Class_Schedules.InsertOnSubmit(schedule);
            db.SubmitChanges();
            //---

            // Check and insert DiemMonHocHocKy for HocSinh
            int iClassId = schedule.ClassId;
            int iSubjectId = schedule.SubjectId;
            int iTermId = schedule.TermId;
            IQueryable<Student_StudentInClass> iqStudentsInClass = from stdsInCls in db.Student_StudentInClasses
                                                                  where stdsInCls.ClassId == iClassId
                                                                  select stdsInCls;
            if (iqStudentsInClass.Count() != 0)
            {
                Student_StudentInClass firstHsLop = iqStudentsInClass.First();
                IQueryable<Student_TermSubjectMark> iqStudentTermSubjectMark;
                iqStudentTermSubjectMark = from stdTermSubjMark in db.Student_TermSubjectMarks
                                           where stdTermSubjMark.StudentInClassId == firstHsLop.StudentInClassId
                                              && stdTermSubjMark.SubjectId == iSubjectId
                                           select stdTermSubjMark;
                if (iqStudentTermSubjectMark.Count() == 0)
                {
                    Student_TermSubjectMark studentTermSubjectMark = null;
                    foreach (Student_StudentInClass studentInClass in iqStudentsInClass)
                    {
                        studentTermSubjectMark = new Student_TermSubjectMark();
                        studentTermSubjectMark.StudentInClassId = studentInClass.StudentInClassId;
                        studentTermSubjectMark.SubjectId = iSubjectId;
                        studentTermSubjectMark.TermId = iTermId;
                        studentTermSubjectMark.AverageMark = -1;

                        db.Student_TermSubjectMarks.InsertOnSubmit(studentTermSubjectMark);
                    }

                    db.SubmitChanges();
                }
            }
        }

        public void UpdateSchedule(Class_Schedule editedSchedule, Category_Subject subject, aspnet_User teacher)
        {
            IQueryable<Class_Schedule> iqNewSubjectedSchedule;
            iqNewSubjectedSchedule = from schdl in db.Class_Schedules
                                     where schdl.SubjectId == subject.SubjectId
                                     select schdl;
            bool bAdd = (iqNewSubjectedSchedule.Count() != 0) ? false : true;

            Class_Schedule schedule = (from schdl in db.Class_Schedules
                                         where schdl.ScheduleId == editedSchedule.ScheduleId
                                         select schdl).First();
            int iOriginalSubjectId = schedule.SubjectId; // store
            schedule.SubjectId = subject.SubjectId;
            schedule.TeacherId = teacher.UserId;
            db.SubmitChanges();

            IQueryable<Class_Schedule> iqOrginalSubjectedSchedule;
            iqOrginalSubjectedSchedule = from schdl in db.Class_Schedules
                                         where schdl.SubjectId == iOriginalSubjectId
                                         select schdl;
            bool bRemove = (iqOrginalSubjectedSchedule.Count() != 0) ? false : true;

            IQueryable<Student_StudentInClass> iqStudentsInClass = from stdInCls in db.Student_StudentInClasses
                                                                  where stdInCls.ClassId == schedule.ClassId
                                                                  select stdInCls;

            Student_TermSubjectMark studentTermSubjectMark = null;
            if (bAdd)
            {
                foreach (Student_StudentInClass studentsInClass in iqStudentsInClass)
                {
                    studentTermSubjectMark = new Student_TermSubjectMark();
                    studentTermSubjectMark.StudentInClassId = studentsInClass.StudentInClassId;
                    studentTermSubjectMark.SubjectId = subject.SubjectId;
                    studentTermSubjectMark.TermId = schedule.TermId;
                    studentTermSubjectMark.AverageMark = -1;
                    db.Student_TermSubjectMarks.InsertOnSubmit(studentTermSubjectMark);
                }
                db.SubmitChanges();
            }

            if (bRemove)
            {
                foreach (Student_StudentInClass studentsInClass in iqStudentsInClass)
                {
                    IQueryable<Student_TermSubjectMark> iqStudentTermSubjectMark;
                    iqStudentTermSubjectMark = from stdTermSubjMark in db.Student_TermSubjectMarks
                                               where stdTermSubjMark.StudentInClassId == studentsInClass.StudentInClassId
                                               && stdTermSubjMark.SubjectId == iOriginalSubjectId
                                               select stdTermSubjMark;
                    if (iqStudentTermSubjectMark.Count() != 0)
                    {
                        studentTermSubjectMark = iqStudentTermSubjectMark.First();
                        db.Student_TermSubjectMarks.DeleteOnSubmit(studentTermSubjectMark);
                    }
                }
                db.SubmitChanges();
            }
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

                int iClasId = schedule.ClassId;
                int iOriginalSubjectId = schedule.SubjectId;
                int iTermId = schedule.TermId;

                db.Class_Schedules.DeleteOnSubmit(schedule);
                db.SubmitChanges();

                IQueryable<Category_Subject> iqScheduledSubjects = from scheduledSubj in db.Class_Schedules
                                                                 where scheduledSubj.SubjectId == iOriginalSubjectId
                                                                 select scheduledSubj.Category_Subject;

                if (iqScheduledSubjects.Count() == 0)
                {
                    IQueryable<Student_TermSubjectMark> iqStudentTermSubjectMark;
                    iqStudentTermSubjectMark = from studTermSubjMark in db.Student_TermSubjectMarks
                                               where studTermSubjMark.TermId == iTermId
                                                 && studTermSubjMark.SubjectId == iOriginalSubjectId
                                                 && studTermSubjMark.Student_StudentInClass.ClassId == iClasId
                                               select studTermSubjMark;
                    if (iqStudentTermSubjectMark.Count() != 0)
                    {
                        foreach (Student_TermSubjectMark studentTermSubjectMark in iqStudentTermSubjectMark)
                        {
                            db.Student_TermSubjectMarks.DeleteOnSubmit(studentTermSubjectMark);
                        }
                        db.SubmitChanges();
                    }
                }
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
                schedules = iqSchedule.OrderBy(teachingPeriod => teachingPeriod.SessionId).ToList();
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
    }
}
