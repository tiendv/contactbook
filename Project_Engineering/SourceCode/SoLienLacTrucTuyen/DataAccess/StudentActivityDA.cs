using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class StudentActivityDA : BaseDA
    {
        public StudentActivityDA(School_School school)
            : base(school)
        {
        }

        public void InsertStudentActivity(Student_Student student, Configuration_Term term, DateTime date, string title, string content, Category_Attitude attitude)
        {
            Student_StudentInClass studentInClass = null;

            IQueryable<Student_StudentInClass> iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                                                                 where stdInCls.StudentId == student.StudentId
                                                                 select stdInCls;

            if (iqStudentInClass.Count() != 0)
            {
                studentInClass = iqStudentInClass.OrderByDescending(stdInCls => stdInCls.StudentInClassId).First();

                Student_Activity studentActivity = new Student_Activity();
                studentActivity.StudentInClassId = studentInClass.StudentInClassId;
                studentActivity.TermId = term.TermId;
                studentActivity.Date = date;
                studentActivity.Title = title;
                studentActivity.ActivityContent = content;
                if (attitude != null)
                {
                    studentActivity.AttitudeId = attitude.AttitudeId;
                }
                db.Student_Activities.InsertOnSubmit(studentActivity);
                db.SubmitChanges();
            }
        }

        public void UpdateStudentActivity(Student_Activity editedStudentActivity)
        {
            Student_Activity studentActivity = null;

            IQueryable<Student_Activity> iqStudentActivity = from stdAct in db.Student_Activities
                                                             where stdAct.ActivityId == editedStudentActivity.ActivityId 
                                                             select stdAct;

            if (iqStudentActivity.Count() != 0)
            {
                studentActivity = iqStudentActivity.First();
                studentActivity.Date = editedStudentActivity.Date;
                studentActivity.ActivityContent = editedStudentActivity.ActivityContent;
                studentActivity.AttitudeId = editedStudentActivity.AttitudeId;

                db.SubmitChanges();
            }
        }

        public void DeleteStudentActivity(Student_Activity deletedStudentActivity)
        {
            Student_Activity studentActivity = null;

            IQueryable<Student_Activity> iqStudentActivity = from stdAct in db.Student_Activities
                                                             where stdAct.ActivityId  == deletedStudentActivity.ActivityId 
                                                             select stdAct;

            if (iqStudentActivity.Count() != 0)
            {
                studentActivity = iqStudentActivity.First();
                db.Student_Activities.DeleteOnSubmit(studentActivity);
                db.SubmitChanges();
            }
        }

        public Student_Activity GetStudentActivity(int studentActivityId)
        {
            Student_Activity studentActivity = null;

            IQueryable<Student_Activity> iqStudentActivity = from stdAct in db.Student_Activities
                                                             where stdAct.ActivityId  == studentActivityId
                                                             select stdAct;

            if (iqStudentActivity.Count() != 0)
            {
                studentActivity = iqStudentActivity.First();
            }

            return studentActivity;
        }

        public Student_Activity GetStudentActivity(Student_Student student, Configuration_Year year, Configuration_Term term, DateTime date)
        {
            Student_Activity studentActivity = null;

            IQueryable<Student_Activity> iqStudentActivity = from stdAct in db.Student_Activities
                                                             join stdInCls in db.Student_StudentInClasses on stdAct.StudentInClassId equals stdInCls.StudentInClassId
                                                             where stdInCls.StudentId == student.StudentId
                                                             && stdInCls.Class_Class.YearId == year.YearId
                                                             && stdAct.TermId == term.TermId
                                                             && stdAct.Date == date
                                                             select stdAct;

            if (iqStudentActivity.Count() != 0)
            {
                studentActivity = iqStudentActivity.First();
            }

            return studentActivity;
        }

        //public Student_Activity GetStudentActivity(int ActivityId , int maHocSinh, int YearId, int TermId, DateTime ngay)
        //{
        //    IQueryable<Student_Activity> hoatDongs = from ngayNghi in db.Student_Activities
        //                                             join hocSinhLopHoc in db.Student_StudentInClasses on ngayNghi.StudentInClassId equals hocSinhLopHoc.StudentInClassId
        //                                             join lop in db.Class_Classes on hocSinhLopHoc.ClassId equals lop.ClassId
        //                                             where hocSinhLopHoc.StudentId == maHocSinh && lop.YearId == YearId
        //                                                && ngayNghi.TermId == TermId && ngayNghi.Date == ngay
        //                                                && ngayNghi.ActivityId  == ActivityId 
        //                                             select ngayNghi;
        //    if (hoatDongs.Count() != 0)
        //    {
        //        return hoatDongs.First();
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        public List<Student_Activity> GetStudentActivities(Student_Student student, Configuration_Year year, Configuration_Term term, DateTime beginDate, DateTime endDate,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<Student_Activity> studentActivities = new List<Student_Activity>();

            IQueryable<Student_Activity> iqStudentActivity = from stdAct in db.Student_Activities
                                                             where stdAct.Student_StudentInClass.StudentId == student.StudentId
                                                                && stdAct.Student_StudentInClass.Class_Class.YearId == year.YearId
                                                                && stdAct.TermId == term.TermId && stdAct.Date >= beginDate && stdAct.Date <= endDate
                                                             select stdAct;
            totalRecords = iqStudentActivity.Count();
            if (totalRecords != 0)
            {
                studentActivities = iqStudentActivity.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return studentActivities;
        }

        public bool StudentActivityNameExists(string title, Student_Student student, Configuration_Year year, Configuration_Term term, DateTime date)
        {
            IQueryable<Student_Activity> iqStudentActivity = from stdAct in db.Student_Activities
                                                             join stdInCls in db.Student_StudentInClasses on stdAct.StudentInClassId equals stdInCls.StudentInClassId
                                                             where stdInCls.StudentId == student.StudentId
                                                             && stdInCls.Class_Class.YearId == year.YearId
                                                             && stdAct.TermId == term.TermId
                                                             && stdAct.Date == date
                                                             && stdAct.Title == title
                                                             select stdAct;

            if (iqStudentActivity.Count() != 0)
            {
                return true;
            }

            return false;
        }
    }
}
