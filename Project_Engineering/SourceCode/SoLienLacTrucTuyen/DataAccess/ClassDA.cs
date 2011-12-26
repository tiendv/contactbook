using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using EContactBook.BusinessEntity;

namespace EContactBook.DataAccess
{
    public class ClassDA : BaseDA
    {
        public ClassDA(School_School school)
            : base(school)
        {
        }

        public void InsertClass(Class_Class Class)
        {
            db.Class_Classes.InsertOnSubmit(Class);
            db.SubmitChanges();
        }

        public void UpdateClass(Class_Class editedClass)
        {
            Class_Class Class = null;
            IQueryable<Class_Class> iqClass = from cls in db.Class_Classes
                                              where cls.ClassId == editedClass.ClassId
                                              select cls;
            if (iqClass.Count() != 0)
            {
                Class = iqClass.First();
                Class.ClassName = editedClass.ClassName;
                db.SubmitChanges();
            }
        }

        public void IncreaseStudentAmount(Class_Class editedClass)
        {
            Class_Class Class = null;
            IQueryable<Class_Class> iqClass = from cls in db.Class_Classes
                                              where cls.ClassId == editedClass.ClassId
                                              select cls;
            if (iqClass.Count() != 0)
            {
                Class = iqClass.First();
                Class.StudentQuantity++;
                db.SubmitChanges();
            }
        }

        public void DeleteClass(Class_Class deletedClass)
        {
            Class_Class Class = null;
            IQueryable<Class_Class> iqClass = from cls in db.Class_Classes
                                              where cls.ClassId == deletedClass.ClassId
                                              select cls;
            if (iqClass.Count() != 0)
            {
                Class = iqClass.First();
                db.Class_Classes.DeleteOnSubmit(Class);
                db.SubmitChanges();
            }
        }

        public Class_Class GetClass(int classID)
        {
            Class_Class Class = null;
            IQueryable<Class_Class> iqClass = from cls in db.Class_Classes
                                              where cls.ClassId == classID
                                              select cls;
            if (iqClass.Count() != 0)
            {
                Class = iqClass.First();
            }

            return Class;
        }

        public List<Class_Class> GetClasses(Configuration_Year year)
        {
            List<Class_Class> lClasses = new List<Class_Class>();

            IQueryable<Class_Class> iqClass = from cls in db.Class_Classes
                                              where cls.YearId == year.YearId
                                              select cls;
            if (iqClass.Count() != 0)
            {
                lClasses = iqClass.ToList();
            }

            return lClasses;
        }

        public List<Class_Class> GetClasses(Configuration_Year year, Category_Faculty faculty, Category_Grade grade)
        {
            List<Class_Class> lClasses = new List<Class_Class>();

            IQueryable<Class_Class> iqClass = from cls in db.Class_Classes
                                              where cls.YearId == year.YearId
                                              && cls.GradeId == grade.GradeId && cls.FacultyId == faculty.FacultyId
                                              select cls;
            if (iqClass.Count() != 0)
            {
                lClasses = iqClass.OrderBy(cls => cls.ClassName).ToList();
            }

            return lClasses;
        }

        public List<Class_Class> GetClasses(Configuration_Year year, Category_Faculty faculty)
        {
            List<Class_Class> lClasses = new List<Class_Class>();

            IQueryable<Class_Class> iqClass = from cls in db.Class_Classes
                                              where cls.YearId == year.YearId
                                              && cls.FacultyId == faculty.FacultyId
                                              select cls;

            if (iqClass.Count() != 0)
            {
                lClasses = iqClass.OrderBy(cls => cls.ClassName).ToList();
            }

            return lClasses;
        }

        public List<Class_Class> GetClasses(Configuration_Year year, Category_Grade grade)
        {
            List<Class_Class> lClasses = new List<Class_Class>();

            IQueryable<Class_Class> iqClass = from cls in db.Class_Classes
                                              where cls.YearId == year.YearId
                                              && cls.GradeId == grade.GradeId
                                              select cls;

            if (iqClass.Count() != 0)
            {
                lClasses = iqClass.OrderBy(cls => cls.ClassName).ToList();
            }

            return lClasses;
        }

        public List<Class_Class> GetClasses(Configuration_Year year, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Class_Class> iqClass = from cls in db.Class_Classes
                                              where cls.YearId == year.YearId
                                              select cls;
            return GetClasses(ref iqClass, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Class_Class> GetClasses(Configuration_Year year, Category_Faculty faculty, Category_Grade grade, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Class_Class> iqClass = from cls in db.Class_Classes
                                              where cls.YearId == year.YearId
                                              && cls.GradeId == grade.GradeId && cls.FacultyId == faculty.FacultyId
                                              select cls;

            return GetClasses(ref iqClass, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Class_Class> GetClasses(Configuration_Year year, Category_Faculty faculty, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Class_Class> iqClass = from cls in db.Class_Classes
                                              where cls.YearId == year.YearId
                                              && cls.FacultyId == faculty.FacultyId
                                              select cls;

            return GetClasses(ref iqClass, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Class_Class> GetClasses(Configuration_Year year, Category_Grade grade, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Class_Class> iqClass = from cls in db.Class_Classes
                                              where cls.YearId == year.YearId
                                              && cls.GradeId == grade.GradeId
                                              select cls;

            return GetClasses(ref iqClass, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Class_Class> GetUnformeredClasses(Configuration_Year year)
        {
            List<Class_Class> lClasses = GetClasses(year);
            if (lClasses.Count != 0)
            {
                int i = 0;
                while (i < lClasses.Count)
                {
                    IQueryable<Class_FormerTeacher> iqFormerTeacher = from formerTeacher in db.Class_FormerTeachers
                                                                      where formerTeacher.ClassId == lClasses[i].ClassId
                                                                      select formerTeacher;
                    if (iqFormerTeacher.Count() != 0)
                    {
                        lClasses.Remove(lClasses[i]);
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            return lClasses;
        }

        public List<Class_Class> GetUnformeredClasses(Configuration_Year year, Category_Faculty faculty)
        {
            List<Class_Class> lClasses = GetClasses(year, faculty);
            if (lClasses.Count != 0)
            {
                int i = 0;
                while (i < lClasses.Count)
                {
                    IQueryable<Class_FormerTeacher> iqFormerTeacher = from formerTeacher in db.Class_FormerTeachers
                                                                      where formerTeacher.ClassId == lClasses[i].ClassId
                                                                      select formerTeacher;
                    if (iqFormerTeacher.Count() != 0)
                    {
                        lClasses.Remove(lClasses[i]);
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            return lClasses;
        }

        public List<Class_Class> GetUnformeredClasses(Configuration_Year year, Category_Grade grade)
        {
            List<Class_Class> lClasses = GetClasses(year, grade);
            if (lClasses.Count != 0)
            {
                int i = 0;
                while (i < lClasses.Count)
                {
                    IQueryable<Class_FormerTeacher> iqFormerTeacher = from formerTeacher in db.Class_FormerTeachers
                                                                      where formerTeacher.ClassId == lClasses[i].ClassId
                                                                      select formerTeacher;
                    if (iqFormerTeacher.Count() != 0)
                    {
                        lClasses.Remove(lClasses[i]);
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            return lClasses;
        }

        public List<Class_Class> GetUnformeredClasses(Configuration_Year year, Category_Faculty faculty, Category_Grade grade)
        {
            List<Class_Class> lClasses = GetClasses(year, faculty, grade);
            if (lClasses.Count != 0)
            {
                int i = 0;
                while (i < lClasses.Count)
                {
                    IQueryable<Class_FormerTeacher> iqFormerTeacher = from formerTeacher in db.Class_FormerTeachers
                                                                      where formerTeacher.ClassId == lClasses[i].ClassId
                                                                      select formerTeacher;
                    if (iqFormerTeacher.Count() != 0)
                    {
                        lClasses.Remove(lClasses[i]);
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            return lClasses;
        }

        private List<Class_Class> GetClasses(ref IQueryable<Class_Class> iqClass, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<Class_Class> lClasses = new List<Class_Class>();
            totalRecords = iqClass.Count();
            if (totalRecords != 0)
            {
                lClasses = iqClass.OrderBy(cls => cls.Category_Faculty.FacultyName)
                    .ThenBy(cls => cls.Category_Grade.GradeName).ThenBy(cls => cls.ClassName)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return lClasses;
        }

        public bool ClassNameExists(string className, Configuration_Year year)
        {
            IQueryable<Class_Class> iqClass = from cls in db.Class_Classes
                                              where cls.ClassName == className && cls.YearId == year.YearId
                                              select cls;
            if (iqClass.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ClassNameExists(string className)
        {
            IQueryable<Class_Class> iqClass = from cls in db.Class_Classes
                                              where cls.ClassName == className
                                              select cls;
            if (iqClass.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsDeletable(Class_Class Class)
        {
            IQueryable<Class_Schedule> iqSchedule = from schedule in db.Class_Schedules
                                                    where schedule.ClassId == Class.ClassId
                                                    select schedule;
            if (iqSchedule.Count() != 0)
            {
                return false;
            }
            else
            {
                IQueryable<Student_StudentInClass> iqStudentsInClass = from stdsInCls in db.Student_StudentInClasses
                                                                       where stdsInCls.ClassId == Class.ClassId
                                                                       select stdsInCls;
                if (iqStudentsInClass.Count() != 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public bool HasFormerTeacher(Class_Class Class)
        {
            IQueryable<Class_FormerTeacher> iqFormerTeacher = from formerTeacher in db.Class_FormerTeachers
                                                              where formerTeacher.ClassId == Class.ClassId
                                                              select formerTeacher;
            if (iqFormerTeacher.Count() != 0)
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
