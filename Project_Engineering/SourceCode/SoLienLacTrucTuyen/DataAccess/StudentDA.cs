using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace EContactBook.DataAccess
{
    public class StudentDA : BaseDA
    {
        public StudentDA(School_School school)
            : base(school)
        {

        }

        #region Student
        public void InsertStudent(Student_Student student)
        {
            db.Student_Students.InsertOnSubmit(student);
            db.SubmitChanges();
        }

        public void UpdateStudent(Student_Student editedStudent)
        {
            Student_Student student = null;
            IQueryable<Student_Student> iqStudent = from std in db.Student_Students
                                                    where std.StudentId == editedStudent.StudentId
                                                    select std;
            if (iqStudent.Count() != 0)
            {
                student = iqStudent.First();
                student.StudentCode = editedStudent.StudentCode;
                student.FullName = editedStudent.FullName;
                student.Gender = editedStudent.Gender;
                student.StudentBirthday = editedStudent.StudentBirthday;
                student.Birthplace = editedStudent.Birthplace;
                student.Address = editedStudent.Address;
                student.ContactPhone = editedStudent.ContactPhone;
                student.FatherName = editedStudent.FatherName;
                student.FatherBirthday = editedStudent.FatherBirthday;
                student.FatherJob = editedStudent.FatherJob;
                student.MotherName = editedStudent.MotherName;
                student.MotherBirthday = editedStudent.MotherBirthday;
                student.MotherJob = editedStudent.MotherJob;

                db.SubmitChanges();
            }
        }

        public void DeleteStudent(Student_Student deletedStudent)
        {
            IQueryable<Student_Student> iqStudent;
            iqStudent = from std in db.Student_Students
                        where std.StudentId == deletedStudent.StudentId
                        select std;
            if (iqStudent.Count() != 0)
            {
                db.Student_Students.DeleteOnSubmit(iqStudent.First());
                db.SubmitChanges();
            }
        }

        public Student_Student GetStudent(string studentCode)
        {
            Student_Student student = null;
            IQueryable<Student_Student> iqStudent = from std in db.Student_Students
                                                    where std.StudentCode == studentCode
                                                    select std;
            if (iqStudent.Count() != 0)
            {
                student = iqStudent.First();
            }

            return student;
        }

        public Student_Student GetStudent(int studentId)
        {
            Student_Student student = null;
            IQueryable<Student_Student> iqStudent = from std in db.Student_Students
                                                    where std.StudentId == studentId
                                                    select std;
            if (iqStudent.Count() != 0)
            {
                student = iqStudent.First();
            }

            return student;
        }

        public bool IsDeletable(string studentCode)
        {
            return true;
        }

        public bool StudentCodeExists(string maHocSinhHienThi)
        {
            IQueryable<Student_Student> hocSinhs = from hocSinh in db.Student_Students
                                                   where hocSinh.StudentCode == maHocSinhHienThi
                                                   select hocSinh;
            if (hocSinhs.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Class_Class GetClass(Configuration_Year year, Student_Student student)
        {
            Class_Class Class = null;

            IQueryable<Class_Class> iqClass = from studentsInClass in db.Student_StudentInClasses
                                              where studentsInClass.StudentId == student.StudentId
                                                && studentsInClass.Class_Class.YearId == year.YearId
                                              select studentsInClass.Class_Class;
            if (iqClass.Count() != 0)
            {
                Class = iqClass.First();
            }

            return Class;
        }

        public Class_Class GetLastedClass(Student_Student student)
        {
            Class_Class Class = null;

            IQueryable<Student_StudentInClass> iqStudentsInClass = from studentsInClass in db.Student_StudentInClasses
                                                                   where studentsInClass.StudentId == student.StudentId
                                                                   select studentsInClass;
            if (iqStudentsInClass.Count() != 0)
            {
                Class = iqStudentsInClass.OrderByDescending(studentsInClass => studentsInClass.StudentInClassId).First().Class_Class;
            }

            return Class;
        }

        public List<Configuration_Year> GetYears(Student_Student student)
        {
            List<Configuration_Year> years = new List<Configuration_Year>();
            IQueryable<Configuration_Year> iqYear = from year in db.Configuration_Years
                                                    join cls in db.Class_Classes
                                                       on year.YearId equals cls.YearId
                                                    join stdInCls in db.Student_StudentInClasses
                                                       on cls.ClassId equals stdInCls.ClassId
                                                    where stdInCls.StudentId == student.StudentId
                                                    select year;
            if (iqYear.Count() != 0)
            {
                years = iqYear.OrderByDescending(year => year.BeginYear).ToList();
            }

            return years;
        }
        #endregion

        #region StudentInClass
        public Student_StudentInClass InsertStudentInClass(Student_Student student, Class_Class Class)
        {
            Student_StudentInClass studentInClass = new Student_StudentInClass();
            studentInClass.ClassId = Class.ClassId;
            studentInClass.StudentId = student.StudentId;
            db.Student_StudentInClasses.InsertOnSubmit(studentInClass);
            db.SubmitChanges();

            return GetLastedStudentInClass();
        }

        public void UpdateStudentInClass(Student_StudentInClass editedStudentInClass, Class_Class Class)
        {
            Student_StudentInClass studentInClass = null;
            IQueryable<Student_StudentInClass> iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                                                                  where stdInCls.StudentInClassId == editedStudentInClass.StudentInClassId
                                                                  select stdInCls;

            if (iqStudentInClass.Count() != 0)
            {
                studentInClass = iqStudentInClass.First();
                studentInClass.ClassId = Class.ClassId;
                db.SubmitChanges();
            }
        }

        public Student_StudentInClass GetLastedStudentInClass()
        {
            Student_StudentInClass lastedStudentInClass = null;

            IQueryable<Student_StudentInClass> iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                                                                  select stdInCls;
            if (iqStudentInClass.Count() != 0)
            {
                lastedStudentInClass = iqStudentInClass.OrderByDescending(stdInCls => stdInCls.StudentInClassId).First();
            }

            return lastedStudentInClass;
        }

        public Student_StudentInClass GetLastedStudentInClass(Student_Student student)
        {
            Student_StudentInClass studentInClass = null;
            IQueryable<Student_StudentInClass> iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                                                                  where stdInCls.StudentId == student.StudentId
                                                                  select stdInCls;

            if (iqStudentInClass.Count() != 0)
            {
                studentInClass = iqStudentInClass.OrderByDescending(stdInCls => stdInCls.StudentInClassId).First();
            }

            return studentInClass;
        }

        public Student_StudentInClass GetStudentInClass(int studentInClassId)
        {
            Student_StudentInClass studentInClass = null;

            IQueryable<Student_StudentInClass> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                               where stdInCls.StudentInClassId == studentInClassId
                               select stdInCls;
            if (iqStudentInClass.Count() != 0)
            {
                studentInClass = iqStudentInClass.First();
            }

            return studentInClass;
        }

        public Student_StudentInClass GetStudentInClass(Student_Student student, Configuration_Year year)
        {
            Student_StudentInClass studentInClass = null;
            IQueryable<Student_StudentInClass> iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                                                                  where stdInCls.StudentId == student.StudentId
                                                                  && stdInCls.Class_Class.YearId == year.YearId
                                                                  select stdInCls;

            if (iqStudentInClass.Count() != 0)
            {
                studentInClass = iqStudentInClass.OrderByDescending(stdInCls => stdInCls.StudentInClassId).First();
            }

            return studentInClass;
        }

        public List<Student_StudentInClass> GetStudentInClasses(Class_Class Class)
        {
            List<Student_StudentInClass> studentInClasses = new List<Student_StudentInClass>();
            IQueryable<Student_StudentInClass> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                               where stdInCls.Class_Class.ClassId == Class.ClassId
                               select stdInCls;
            if (iqStudentInClass.Count() != 0)
            {
                studentInClasses = iqStudentInClass.ToList();
            }

            return studentInClasses;
        }

        public List<Student_StudentInClass> GetStudentInClasses()
        {
            List<Student_StudentInClass> studentInClasses = new List<Student_StudentInClass>();

            // get current year
            Configuration_Year currentYear = (new SystemConfigDA(school)).GetLastedYear();

            IQueryable<Student_StudentInClass> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                               where stdInCls.Class_Class.YearId == currentYear.YearId
                               select stdInCls;

            if (iqStudentInClass.Count() != 0)
            {
                studentInClasses = iqStudentInClass.OrderBy(std => std.Student_Student.StudentCode)
                    .ThenBy(std => std.Student_Student.FullName).ToList();
            }

            return studentInClasses;
        }

        public List<Student_StudentInClass> GetStudentInClasses(Category_Grade grade)
        {
            List<Student_StudentInClass> studentInClasses = new List<Student_StudentInClass>();

            // get current year
            Configuration_Year currentYear = (new SystemConfigDA(school)).GetLastedYear();

            IQueryable<Student_StudentInClass> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                               where stdInCls.Class_Class.YearId == currentYear.YearId
                               && stdInCls.Class_Class.GradeId == grade.GradeId
                               select stdInCls;

            if (iqStudentInClass.Count() != 0)
            {
                studentInClasses = iqStudentInClass.OrderBy(std => std.Student_Student.StudentCode)
                    .ThenBy(std => std.Student_Student.FullName).ToList();
            }

            return studentInClasses;
        }

        public List<Student_StudentInClass> GetStudentInClasses(Category_Faculty faculty)
        {
            List<Student_StudentInClass> studentInClasses = new List<Student_StudentInClass>();

            // get current year
            Configuration_Year currentYear = (new SystemConfigDA(school)).GetLastedYear();

            IQueryable<Student_StudentInClass> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                               where stdInCls.Class_Class.YearId == currentYear.YearId
                               && stdInCls.Class_Class.FacultyId == faculty.FacultyId
                               select stdInCls;

            if (iqStudentInClass.Count() != 0)
            {
                studentInClasses = iqStudentInClass.OrderBy(std => std.Student_Student.StudentCode)
                    .ThenBy(std => std.Student_Student.FullName).ToList();
            }

            return studentInClasses;
        }

        public List<Student_StudentInClass> GetStudentInClasses(Category_Grade grade, Category_Faculty faculty)
        {
            List<Student_StudentInClass> studentInClasses = new List<Student_StudentInClass>();

            // get current year
            Configuration_Year currentYear = (new SystemConfigDA(school)).GetLastedYear();

            IQueryable<Student_StudentInClass> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                               where stdInCls.Class_Class.YearId == currentYear.YearId
                               && stdInCls.Class_Class.FacultyId == faculty.FacultyId
                               && stdInCls.Class_Class.GradeId == grade.GradeId
                               select stdInCls;

            if (iqStudentInClass.Count() != 0)
            {
                studentInClasses = iqStudentInClass.OrderBy(std => std.Student_Student.StudentCode)
                    .ThenBy(std => std.Student_Student.FullName).ToList();
            }

            return studentInClasses;
        }

        public List<Student_StudentInClass> GetStudentInClasses(Class_Class Class, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Student_StudentInClass> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                               where stdInCls.Class_Class.ClassId == Class.ClassId
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Student_StudentInClass> GetStudentInClasses(Class_Class Class, string studentName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Student_StudentInClass> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                               where stdInCls.Class_Class.ClassId == Class.ClassId
                                  && stdInCls.Student_Student.FullName == studentName
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Student_StudentInClass> GetStudentInClasses(Configuration_Year year, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Student_StudentInClass> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                               where stdInCls.Class_Class.YearId == year.YearId
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Student_StudentInClass> GetStudentInClasses(Configuration_Year year, string studentName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Student_StudentInClass> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                               where stdInCls.Class_Class.YearId == year.YearId
                               && stdInCls.Student_Student.FullName == studentName
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Student_StudentInClass> GetStudentInClasses(Configuration_Year year, Category_Grade grade, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Student_StudentInClass> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                               where stdInCls.Class_Class.YearId == year.YearId
                               && stdInCls.Class_Class.GradeId == grade.GradeId
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Student_StudentInClass> GetStudentInClasses(Configuration_Year year, Category_Grade grade, string studentName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Student_StudentInClass> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                               where stdInCls.Class_Class.YearId == year.YearId
                               && stdInCls.Class_Class.GradeId == grade.GradeId
                               && stdInCls.Student_Student.FullName == studentName
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Student_StudentInClass> GetStudentInClasses(Configuration_Year year, Category_Faculty faculty, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Student_StudentInClass> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                               where stdInCls.Class_Class.YearId == year.YearId
                               && stdInCls.Class_Class.FacultyId == faculty.FacultyId
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Student_StudentInClass> GetStudentInClasses(Configuration_Year year, Category_Faculty faculty, string studentName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Student_StudentInClass> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                               where stdInCls.Class_Class.YearId == year.YearId
                               && stdInCls.Class_Class.FacultyId == faculty.FacultyId
                               && stdInCls.Student_Student.FullName == studentName
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Student_StudentInClass> GetStudentInClasses(Configuration_Year year, Category_Faculty faculty, Category_Grade grade, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Student_StudentInClass> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                               where stdInCls.Class_Class.YearId == year.YearId
                               && stdInCls.Class_Class.FacultyId == faculty.FacultyId
                               && stdInCls.Class_Class.GradeId == grade.GradeId
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Student_StudentInClass> GetStudentInClasses(Configuration_Year year, Category_Faculty faculty, Category_Grade grade, string studentName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Student_StudentInClass> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                               where stdInCls.Class_Class.YearId == year.YearId
                               && stdInCls.Class_Class.FacultyId == faculty.FacultyId
                               && stdInCls.Class_Class.GradeId == grade.GradeId
                               && stdInCls.Student_Student.FullName == studentName
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass, pageCurrentIndex, pageSize, out totalRecords);
        }

        private List<Student_StudentInClass> GetStudentInClasses(ref IQueryable<Student_StudentInClass> iqStudentInClass, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<Student_StudentInClass> studentInClasses = new List<Student_StudentInClass>();

            totalRecords = iqStudentInClass.Count();
            if (totalRecords != 0)
            {
                studentInClasses = iqStudentInClass.OrderBy(student => student.Student_Student.StudentCode).Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return studentInClasses;
        }

        public List<Student_StudentInClass> GetStudentInClasses(Configuration_Year year, Category_Faculty faculty, Category_Grade grade, string studentName)
        {
            IQueryable<Student_StudentInClass> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                               where stdInCls.Class_Class.YearId == year.YearId
                               && stdInCls.Class_Class.FacultyId == faculty.FacultyId
                               && stdInCls.Class_Class.GradeId == grade.GradeId
                               && stdInCls.Student_Student.FullName == studentName
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass);
        }

        public List<Student_StudentInClass> GetStudentInClasses(Configuration_Year year)
        {
            IQueryable<Student_StudentInClass> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                               where stdInCls.Class_Class.YearId == year.YearId
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass);
        }

        public List<Student_StudentInClass> GetStudentInClasses(Configuration_Year year, Category_Faculty faculty)
        {
            IQueryable<Student_StudentInClass> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                               where stdInCls.Class_Class.YearId == year.YearId
                               && stdInCls.Class_Class.FacultyId == faculty.FacultyId
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass);
        }

        public List<Student_StudentInClass> GetStudentInClasses(Configuration_Year year, Category_Grade grade)
        {
            IQueryable<Student_StudentInClass> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                               where stdInCls.Class_Class.YearId == year.YearId
                               && stdInCls.Class_Class.GradeId == grade.GradeId
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass);
        }

        public List<Student_StudentInClass> GetStudentInClasses(Configuration_Year year, string studentName)
        {
            IQueryable<Student_StudentInClass> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                               where stdInCls.Class_Class.YearId == year.YearId
                               && stdInCls.Student_Student.FullName == studentName
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass);
        }

        public List<Student_StudentInClass> GetStudentInClasses(Configuration_Year year, Category_Grade grade, string studentName)
        {
            IQueryable<Student_StudentInClass> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                               where stdInCls.Class_Class.YearId == year.YearId
                               && stdInCls.Class_Class.GradeId == grade.GradeId
                               && stdInCls.Student_Student.FullName == studentName
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass);
        }

        public List<Student_StudentInClass> GetStudentInClasses(Configuration_Year year, Category_Faculty faculty, Category_Grade grade)
        {
            IQueryable<Student_StudentInClass> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                               where stdInCls.Class_Class.YearId == year.YearId
                               && stdInCls.Class_Class.FacultyId == faculty.FacultyId
                               && stdInCls.Class_Class.GradeId == grade.GradeId
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass);
        }

        public List<Student_StudentInClass> GetStudentInClasses(Configuration_Year year, Category_Faculty faculty, string studentName)
        {
            IQueryable<Student_StudentInClass> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                               where stdInCls.Class_Class.YearId == year.YearId
                               && stdInCls.Class_Class.FacultyId == faculty.FacultyId
                               && stdInCls.Student_Student.FullName == studentName
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass);
        }

        public List<Student_StudentInClass> GetStudentInClasses(Class_Class Class, string studentName)
        {
            IQueryable<Student_StudentInClass> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                               where stdInCls.Class_Class.ClassId == Class.ClassId
                                  && stdInCls.Student_Student.FullName == studentName
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass);
        }

        private List<Student_StudentInClass> GetStudentInClasses(ref IQueryable<Student_StudentInClass> iqStudentInClass)
        {
            List<Student_StudentInClass> studentInClasses = new List<Student_StudentInClass>();
    
            if (iqStudentInClass.Count() != 0)
            {
                studentInClasses = iqStudentInClass.OrderBy(student => student.Student_Student.StudentCode).ToList();
            }

            return studentInClasses;
        }

        #endregion

        public List<TabularHanhKiemHocSinh> GetListHanhKiemHocSinh(int ClassId, int TermId, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularHanhKiemHocSinh> iqDanhHieuHK;
            iqDanhHieuHK = from danhHieuHK in db.Student_TermLearningResults
                           join hocSinhLop in db.Student_StudentInClasses
                                on danhHieuHK.StudentInClassId equals hocSinhLop.StudentInClassId
                           join hocSinh in db.Student_Students
                                on hocSinhLop.StudentId equals hocSinh.StudentId
                           where hocSinhLop.ClassId == ClassId && danhHieuHK.TermId == TermId
                           select new TabularHanhKiemHocSinh
                           {
                               MaHocSinh = hocSinhLop.StudentId,
                               MaHocSinhHienThi = hocSinh.StudentCode,
                               HoTenHocSinh = hocSinh.FullName,
                               ConductId = danhHieuHK.TermConductId
                           };

            totalRecords = iqDanhHieuHK.Count();
            if (totalRecords != 0)
            {
                return iqDanhHieuHK.OrderBy(hocSinh => hocSinh.MaHocSinhHienThi)
                    .ThenBy(hocSinh => hocSinh.HoTenHocSinh)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularHanhKiemHocSinh>();
            }
        }

        public void UpdateStudenTermConduct(Class_Class Class, Configuration_Term term, Student_Student student, Category_Conduct conduct)
        {
            Student_TermLearningResult studentTermResult = null;
            IQueryable<Student_TermLearningResult> iqStudentTermResult;
            iqStudentTermResult = from stdTermResult in db.Student_TermLearningResults
                                  where stdTermResult.Student_StudentInClass.ClassId == Class.ClassId
                                    && stdTermResult.TermId == term.TermId
                                    && stdTermResult.Student_StudentInClass.StudentId == student.StudentId
                                  select stdTermResult;
            if (iqStudentTermResult.Count() != null)
            {
                studentTermResult = iqStudentTermResult.First();
                studentTermResult.TermConductId = conduct.ConductId;
                db.SubmitChanges();
            }
        }

        public void DeleteStudentInClass(Student_Student deletedStudent)
        {
            IQueryable<Student_StudentInClass> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.Student_StudentInClasses
                               where stdInCls.StudentId == deletedStudent.StudentId
                               select stdInCls;
            if(iqStudentInClass.Count() != 0)
            {
                foreach (Student_StudentInClass studentInClass in iqStudentInClass)
                {
                    db.Student_StudentInClasses.DeleteOnSubmit(studentInClass);
                }

                db.SubmitChanges();
            }
        }
    }
}