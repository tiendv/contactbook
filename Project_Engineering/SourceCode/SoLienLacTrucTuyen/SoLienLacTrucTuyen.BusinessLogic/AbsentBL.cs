using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class AbsentBL: BaseBL
    {
        private AbsentDA absentDA;

        public AbsentBL(School_School school)
            : base(school)
        {
            absentDA = new AbsentDA(school);
        }

        public void InsertAbsent(Student_Student student, Configuration_Term term, DateTime date, Configuration_Session session, bool permission, string reason)
        {
            StudentBL studentBL = new StudentBL(school);
            Class_Class Class = studentBL.GetLastedClass(student);
            Student_StudentInClass studentInClass = studentBL.GetStudentInClass(student, Class.Configuration_Year);

            absentDA.InsertAbsent(studentInClass, term, date, session, permission, reason);
        }

        public void UpdateAbsent(Student_Absent editedAbsent, Configuration_Term newTerm, DateTime newDate, Configuration_Session newSession, bool newPermission, string newReason)
        {
            editedAbsent.TermId = newTerm.TermId;
            editedAbsent.Date = newDate;
            if (newSession != null)
            {
                editedAbsent.SessionId = newSession.SessionId;
            }            
            editedAbsent.IsAsked = newPermission;
            editedAbsent.Reason = newReason;
            absentDA.UpdateAbsent(editedAbsent);
        }

        public void DeleteAbsent(int maNgayNghiHoc)
        {
            absentDA.DeleteAbsent(maNgayNghiHoc);
        }

        public Student_Absent GetAbsent(int absentId)
        {
            return absentDA.GetAbsent(absentId);
        }

        public Student_Absent GetAbsent(Student_Student student, Configuration_Year year, Configuration_Term term, DateTime date)
        {
            StudentBL studentBL = new StudentBL(school);
            Student_StudentInClass studentInClass = studentBL.GetStudentInClass(student, year);

            return absentDA.GetAbsent(studentInClass, term, date);
        }

        public Student_Absent GetAbsent(Student_Absent exceptedAbsent, Student_Student student, Configuration_Year year, Configuration_Term term, DateTime date)
        {
            StudentBL studentBL = new StudentBL(school);
            Student_StudentInClass studentInClass = studentBL.GetStudentInClass(student, year);

            return absentDA.GetAbsent(exceptedAbsent, studentInClass, term, date);
        }

        public List<TabularAbsent> GetTabularAbsents(Student_Student student, Configuration_Year year, Configuration_Term term, DateTime beginDate, DateTime endDate, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            StudentBL studentBL = new StudentBL(school);
            SystemConfigBL systemConfigBL = new SystemConfigBL(school);
            List<TabularAbsent> tabularAbsents = new List<TabularAbsent>();
            TabularAbsent tabularAbsent = null;
            Student_StudentInClass studentInClass = studentBL.GetStudentInClass(student, year);
            List<Student_Absent> absents = absentDA.GetAbsents(studentInClass, term, beginDate, endDate, pageCurrentIndex, pageSize, out totalRecords);

            foreach (Student_Absent absent in absents)
            {
                tabularAbsent = new TabularAbsent();
                tabularAbsent.AbsentId = absent.AbsentId;
                tabularAbsent.StudentInClassId = absent.StudentInClass;
                tabularAbsent.Date = absent.Date.Day + "/" + absent.Date.Month + "/" + absent.Date.Year;
                tabularAbsent.Session = systemConfigBL.GetSessionName(absent.SessionId);
                tabularAbsent.IsAsked = (absent.IsAsked) ? "Có" : "Không";
                tabularAbsent.Reason = absent.Reason;
                tabularAbsent.Confirmed = (absent.IsConfirmed) ? "Có" : "Không";

                tabularAbsents.Add(tabularAbsent);
            }

            return tabularAbsents;
        }

        public bool Confirmed(Student_Absent absent)
        {
            return absentDA.IsConfirmed(absent);
        }

        public void ConfirmAbsent(Student_Absent absent)
        {
            bool xacNhan = true;
            absentDA.ConfirmAbsent(absent, xacNhan);
        }

        public void UnConfirmAbsent(Student_Absent absent)
        {
            bool xacNhan = false;
            absentDA.ConfirmAbsent(absent, xacNhan);
        }

        public bool AbsentExists(Student_Absent exceptedAbsent, Student_Student student, Configuration_Term term, DateTime date, Configuration_Session session)
        {
            StudentBL studentBL = new StudentBL(school);
            Class_Class Class = studentBL.GetLastedClass(student);
            int ClassId = Class.ClassId;

            if (exceptedAbsent == null) // create new
            {
                if (session == null) // choose all day
                {
                    return absentDA.AbsentExists(student, Class, term, date);
                }
                else
                {
                    bool bAllDay = absentDA.AbsentExists(student, Class, term, date);
                    if (bAllDay)
                    {
                        return true;
                    }
                    else
                    {
                        return absentDA.AbsentExists(student, Class, term, date, session);
                    }
                }
            }
            else // update
            {
                if (session == null)
                {
                    return absentDA.AbsentExists(exceptedAbsent, student, Class, term, date);
                }
                else
                {
                    bool bAllDay = absentDA.AbsentExists(exceptedAbsent, student, Class, term, date);
                    if (bAllDay)
                    {
                        return true;
                    }
                    else
                    {
                        return absentDA.AbsentExists(exceptedAbsent, student, Class, term, date, session);
                    }
                }
            }
        }

        internal void DeleteAbsent(Student_Student deletedStudent)
        {
            absentDA.DeleteAbsent(deletedStudent);
        }
    }
}
