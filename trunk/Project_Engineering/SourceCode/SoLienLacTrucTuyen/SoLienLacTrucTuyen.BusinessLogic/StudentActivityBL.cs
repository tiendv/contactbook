using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class StudentActivityBL:BaseBL
    {
        private StudentActivityDA studentActivityDA;

        public StudentActivityBL(School_School school) : base(school)
        {
            studentActivityDA = new StudentActivityDA(school);
        }

        public void InsertStudentActivity(Student_Student student, Configuration_Term term, DateTime date, string title, string content, Category_Attitude attitude)
        {
            studentActivityDA.InsertStudentActivity(student, term, date, title, content, attitude);
        }

        public void UpdateStudentActivity(Student_Activity editedStudentActivity, DateTime date, string content, Category_Attitude attitude)
        {
            editedStudentActivity.Date = date;
            editedStudentActivity.ActivityContent = content;
            if (attitude != null)
            {
                editedStudentActivity.AttitudeId = attitude.AttitudeId;
            }
            else
            {
                editedStudentActivity.AttitudeId = null;
            }

            studentActivityDA.UpdateStudentActivity(editedStudentActivity);
        }

        public void DeleteStudentActivity(Student_Activity deletedStudentActivity)
        {
            studentActivityDA.DeleteStudentActivity(deletedStudentActivity);
        }        

        public Student_Activity GetStudentActivity(int studentActivityId)
        {
            return studentActivityDA.GetStudentActivity(studentActivityId);
        }

        public Student_Activity GetStudentActivity(Student_Student student, Configuration_Year year, Configuration_Term term, DateTime date)
        {
            return studentActivityDA.GetStudentActivity(student, year, term, date);
        }

        public List<TabularStudentActivity> GetTabularStudentActivities(Student_Student student, Configuration_Year year, Configuration_Term term, DateTime beginDate, DateTime endDate,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            AttitudeBL attitudeBL = new AttitudeBL(school);
            List<TabularStudentActivity> tabularStudentActivities = new List<TabularStudentActivity>();
            List<Student_Activity> studentActivities = new List<Student_Activity>();
            studentActivities = studentActivityDA.GetStudentActivities(student, year, term, beginDate, endDate, pageCurrentIndex, pageSize, out totalRecords);
            TabularStudentActivity tabularStudentActivity = null;

            foreach (Student_Activity studentActivity in studentActivities)
            {
                tabularStudentActivity = new TabularStudentActivity();
                tabularStudentActivity.MaHoatDong = studentActivity.ActivityId;
                tabularStudentActivity.MaHocSinhLopHoc = studentActivity.Student_StudentInClass.StudentInClassId;
                tabularStudentActivity.TenHoatDong = studentActivity.Title;
                tabularStudentActivity.Ngay = studentActivity.Date;
                tabularStudentActivity.StrNgay = studentActivity.Date.ToShortDateString();
                if (studentActivity.AttitudeId != null)
                {
                    tabularStudentActivity.ThaiDoThamGia = attitudeBL.GetAttitude((int)studentActivity.AttitudeId).AttitudeName;
                }
                else
                {
                    tabularStudentActivity.ThaiDoThamGia = "Không xác định";
                }

                tabularStudentActivities.Add(tabularStudentActivity);
            }
            return tabularStudentActivities;
        }

        public bool StudentActivityNamExists(string title, Student_Student student, Configuration_Year year, Configuration_Term term, DateTime date)
        {
            StudentBL studentBL = new StudentBL(school);
            //int ClassId = studentBL.GetCurrentClassId(student.StudentId);
            //Class_Class Class = new Class_Class();
            //Class.ClassId = ClassId;
            return studentActivityDA.StudentActivityNameExists(title, student, year, term, date);
        }

        public bool StudentActivityNameExists(string oldTitlte, string title, Student_Student student, Configuration_Year year, Configuration_Term term, DateTime date)
        {
            if (oldTitlte == title)
            {
                return false;
            }
            else
            {
                StudentBL studentBL = new StudentBL(school);
                //int ClassId = studentBL.GetCurrentClassId(student.StudentId);
                //Class_Class Class = new Class_Class();
                //Class.ClassId = ClassId;
                return studentActivityDA.StudentActivityNameExists(title, student, year, term, date);
            }
        }

        internal void DeleteStudentActivity(Student_Student deletedStudent)
        {
            studentActivityDA.DeleteStudentActivity(deletedStudent);
        }
    }
}
