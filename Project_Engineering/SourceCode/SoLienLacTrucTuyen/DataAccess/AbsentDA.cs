using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.BusinessEntity;

namespace EContactBook.DataAccess
{
    public class AbsentDA : BaseDA
    {
        public AbsentDA(School_School school)
            : base(school)
        {
        }

        public void InsertAbsent(Student_StudentInClass studentInClass, Configuration_Term term, DateTime date, Configuration_Session session, bool permission, string reason)
        {
            Student_Absent absent = new Student_Absent();
            absent.StudentInClass = studentInClass.StudentInClassId;
            absent.TermId = term.TermId;
            absent.Date = date;
            absent.SessionId = session.SessionId;
            absent.IsAsked = permission;
            absent.IsConfirmed = false;
            absent.Reason = reason;

            db.Student_Absents.InsertOnSubmit(absent);
            db.SubmitChanges();
        }

        public void UpdateAbsent(Student_Absent editedAbsent)
        {
            Student_Absent absent = null;
            IQueryable<Student_Absent> iqAbsent = from abs in db.Student_Absents
                                                  where abs.AbsentId == editedAbsent.AbsentId
                                                  select abs;
            if (iqAbsent.Count() != 0)
            {
                absent = iqAbsent.First();
                absent.Date = editedAbsent.Date;
                absent.SessionId = editedAbsent.SessionId;
                absent.IsAsked = editedAbsent.IsAsked;
                absent.TermId = editedAbsent.TermId;
                absent.Reason = editedAbsent.Reason;
                db.SubmitChanges();
            }
        }

        public void ConfirmAbsent(Student_Absent editedAbsent, bool confirm)
        {
            Student_Absent absent = null;
            IQueryable<Student_Absent> iqAbsent = from abs in db.Student_Absents
                                                  where abs.AbsentId == editedAbsent.AbsentId
                                                  select abs;
            if (iqAbsent.Count() != 0)
            {
                absent = iqAbsent.First();
                absent.IsConfirmed = confirm;
                db.SubmitChanges();
            }
        }

        public void DeleteAbsent(Student_Absent absent)
        {
            IQueryable<Student_Absent> iqAbsent = from abs in db.Student_Absents
                                                  where abs.AbsentId == absent.AbsentId
                                                  select abs;
            if (iqAbsent.Count() != 0)
            {
                absent = iqAbsent.First();
                db.Student_Absents.DeleteOnSubmit(absent);
                db.SubmitChanges();
            }
        }

        public Student_Absent GetAbsent(int absentId)
        {
            Student_Absent absent = null;
            IQueryable<Student_Absent> iqAbsent = from abs in db.Student_Absents
                                                  where abs.AbsentId == absentId
                                                  select abs;
            if (iqAbsent.Count() != 0)
            {
                absent = iqAbsent.First();
            }

            return absent;
        }

        public Student_Absent GetAbsent(Student_StudentInClass studentInClass, Configuration_Term term, DateTime date)
        {
            Student_Absent absent = null;
            IQueryable<Student_Absent> iqAbsent = from abs in db.Student_Absents
                                                  where abs.StudentInClass == studentInClass.StudentInClassId
                                                  && abs.TermId == term.TermId
                                                  && abs.Date == date
                                                  select abs;
            if (iqAbsent.Count() != 0)
            {
                absent = iqAbsent.First();
            }

            return absent;
        }

        public Student_Absent GetAbsent(Student_Absent exceptedAbsent, Student_StudentInClass studentInClass, Configuration_Term term, DateTime date)
        {
            Student_Absent absent = null;
            IQueryable<Student_Absent> iqAbsent = from abs in db.Student_Absents
                                                  where abs.AbsentId != exceptedAbsent.AbsentId
                                                  && abs.StudentInClass == studentInClass.StudentInClassId
                                                  && abs.TermId == term.TermId
                                                  && abs.Date == date
                                                  select abs;
            if (iqAbsent.Count() != 0)
            {
                absent = iqAbsent.First();
            }

            return absent;
        }

        public List<Student_Absent> GetAbsents(Student_StudentInClass studentInClass, Configuration_Term term, DateTime beginDate, DateTime endDate, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<Student_Absent> absents = new List<Student_Absent>();

            IQueryable<Student_Absent> iqAbsent = from abs in db.Student_Absents
                                                  where abs.StudentInClass == studentInClass.StudentInClassId
                                                  && abs.TermId == term.TermId
                                                  && abs.Date >= beginDate
                                                  && abs.Date <= endDate
                                                  select abs;
            totalRecords = iqAbsent.Count();
            if (totalRecords != 0)
            {
                absents = iqAbsent.OrderBy(n => n.Date).Skip((pageCurrentIndex - 1) * pageSize)
                    .Take(pageSize).ToList();
            }

            return absents;
        }

        public bool IsConfirmed(Student_Absent absent)
        {
            IQueryable<Student_Absent> iqAbsent = from abs in db.Student_Absents
                                                  where abs.AbsentId == absent.AbsentId
                                                  select abs;
            if (iqAbsent.Count() != 0)
            {
                absent = iqAbsent.First();
            }

            return absent.IsConfirmed;
        }

        public bool AbsentExists(Student_Student student, Class_Class Class, Configuration_Term term, DateTime date)
        {
            IQueryable<Student_Absent> iqAbsent = from abs in db.Student_Absents
                                                  where abs.Student_StudentInClass.StudentId == student.StudentId
                                                  && abs.Student_StudentInClass.ClassId == Class.ClassId
                                                  && abs.TermId == term.TermId
                                                  && abs.Date == date
                                                  select abs;
            if (iqAbsent.Count() != 0)
            {
                return true;
            }

            return false;
        }

        public bool AbsentExists(Student_Student student, Class_Class Class, Configuration_Term term, DateTime date, Configuration_Session session)
        {
            IQueryable<Student_Absent> iqAbsent = from abs in db.Student_Absents
                                                  where abs.Student_StudentInClass.StudentId == student.StudentId
                                                  && abs.Student_StudentInClass.ClassId == Class.ClassId
                                                  && abs.TermId == term.TermId
                                                  && abs.Date == date
                                                  && abs.SessionId == session.SessionId
                                                  select abs;
            if (iqAbsent.Count() != 0)
            {
                return true;
            }

            return false;
        }

        public bool AbsentExists(Student_Absent exceptedAbsent, Student_Student student, Class_Class Class, Configuration_Term term, DateTime date)
        {
            IQueryable<Student_Absent> iqAbsent = from abs in db.Student_Absents
                                                  where abs.AbsentId != exceptedAbsent.AbsentId
                                                  && abs.Student_StudentInClass.StudentId == student.StudentId
                                                  && abs.Student_StudentInClass.ClassId == Class.ClassId
                                                  && abs.TermId == term.TermId
                                                  && abs.Date == date
                                                  select abs;
            if (iqAbsent.Count() != 0)
            {
                return true;
            }

            return false;
        }

        public bool AbsentExists(Student_Absent exceptedAbsent, Student_Student student, Class_Class Class, Configuration_Term term, DateTime date, Configuration_Session session)
        {
            IQueryable<Student_Absent> iqAbsent = from abs in db.Student_Absents
                                                  where abs.AbsentId != exceptedAbsent.AbsentId
                                                  && abs.Student_StudentInClass.StudentId == student.StudentId
                                                  && abs.Student_StudentInClass.ClassId == Class.ClassId
                                                  && abs.TermId == term.TermId
                                                  && abs.Date == date
                                                  && abs.SessionId == session.SessionId
                                                  select abs;

            if (iqAbsent.Count() != 0)
            {
                return true;
            }

            return false;
        }

        public void DeleteAbsent(Student_Student deletedStudent)
        {
            IQueryable<Student_Absent> iqAbsent;
            iqAbsent = from absent in db.Student_Absents
                       where absent.Student_StudentInClass.StudentId == deletedStudent.StudentId
                       select absent;
            if (iqAbsent.Count() != 0)
            {
                foreach (Student_Absent absent in iqAbsent)
                {
                    db.Student_Absents.DeleteOnSubmit(absent);
                }

                db.SubmitChanges();
            }
        }

        public int GetUnconfirmAbsentCount(Student_Student student, Class_Class Class)
        {
            List<Student_Absent> absents = new List<Student_Absent>();

            IQueryable<Student_Absent> iqAbsent = from a in db.Student_Absents
                                                  where a.Student_StudentInClass.StudentId == student.StudentId
                                                   && a.Student_StudentInClass.Class_Class.YearId == Class.YearId
                                                   && a.IsConfirmed == false
                                                  select a;
            return iqAbsent.Count();
        }

        public int GetTotalDayOfAbsent(Student_Student student, Class_Class Class, Configuration_Term term)
        {
            IQueryable<Student_Absent> iqAbsent = from a in db.Student_Absents
                                                  where a.Student_StudentInClass.StudentId == student.StudentId
                                                   && a.Student_StudentInClass.ClassId == Class.ClassId
                                                   && a.TermId == term.TermId
                                                  select a;
            return iqAbsent.Count();
        }

        public int GetTotalDayOfAbsent(Student_Student student, Class_Class Class, Configuration_Term term, bool asked)
        {
            IQueryable<Student_Absent> iqAbsent = from a in db.Student_Absents
                                                  where a.Student_StudentInClass.StudentId == student.StudentId
                                                   && a.Student_StudentInClass.ClassId == Class.ClassId
                                                   && a.TermId == term.TermId && a.IsAsked == asked
                                                  select a;
            return iqAbsent.Count();
        }

        public bool IsDeletable(Student_Absent absent)
        {
            IQueryable<Student_Absent> iqAbsent = from abs in db.Student_Absents
                                                  where abs.AbsentId == absent.AbsentId
                                                  && abs.IsConfirmed == true
                                                  select abs;
            if (iqAbsent.Count() != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
