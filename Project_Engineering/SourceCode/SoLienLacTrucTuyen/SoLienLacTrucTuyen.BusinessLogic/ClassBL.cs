using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class ClassBL : BaseBL
    {
        private ClassDA classDA;

        public ClassBL(School_School school)
            : base(school)
        {
            classDA = new ClassDA(school);
        }

        public void InsertClass(string ClassName, Configuration_Year year, Category_Faculty faculty, Category_Grade grade)
        {
            classDA.InsertClass(new Class_Class()
            {
                ClassName = ClassName,
                FacultyId = faculty.FacultyId,
                GradeId = grade.GradeId,
                YearId = year.YearId,
                SchoolId = school.SchoolId
            });
        }

        public void UpdateClass(Class_Class editedClass, string newClassName)
        {
            editedClass.ClassName = newClassName;
            classDA.UpdateClass(editedClass);
        }

        public void IncreaseStudentAmount(Class_Class Class)
        {
            classDA.IncreaseStudentAmount(Class);
        }

        public void DecreaseStudentAmount(Class_Class Class)
        {
            classDA.DecreaseStudentAmount(Class);
        }

        public void DeleteClass(Class_Class deletedClass)
        {
            classDA.DeleteClass(deletedClass);
        }

        public Class_Class GetClass(int classId)
        {
            return classDA.GetClass(classId);
        }

        public TabularClass GetTabularClass(Class_Class Class)
        {
            Class = classDA.GetClass(Class.ClassId);
            TabularClass tabularClass = new TabularClass();
            FormerTeacherBL formerTeacherBL = new FormerTeacherBL(school);
            Class_FormerTeacher formerTeacher = null;

            tabularClass.ClassId = Class.ClassId;
            tabularClass.ClassName = Class.ClassName;
            tabularClass.FacultyName = Class.Category_Faculty.FacultyName;
            tabularClass.FacultyId = Class.FacultyId;
            tabularClass.GradeId = Class.GradeId;
            tabularClass.GradeName = Class.Category_Grade.GradeName;
            tabularClass.SiSo = Class.StudentQuantity;
            tabularClass.YearId = Class.YearId;
            tabularClass.YearName = Class.Configuration_Year.YearName;
            formerTeacher = formerTeacherBL.GetFormerTeacher(Class);
            if (formerTeacher != null)
            {
                tabularClass.HomeroomTeacherCode = formerTeacher.aspnet_User.UserId;
                tabularClass.TenGVCN = formerTeacher.aspnet_User.aspnet_Membership.FullName;
            }

            return tabularClass;
        }

        //public List<Class_Class> GetClasses(Configuration_Year year, Category_Faculty faculty, Category_Grade grade)
        //{
        //    List<Class_Class> lClasses = new List<Class_Class>();
        //    if (faculty == null)
        //    {
        //        if (grade == null)
        //        {
        //            lClasses = classDA.GetClasses(year);
        //        }
        //        else
        //        {
        //            lClasses = classDA.GetClasses(year, grade);
        //        }
        //    }
        //    else
        //    {
        //        if (grade == null)
        //        {
        //            lClasses = classDA.GetClasses(year, faculty);
        //        }
        //        else
        //        {
        //            lClasses = classDA.GetClasses(year, faculty, grade);
        //        }
        //    }

        //    return lClasses;
        //}

        //public List<Class_Class> GetClasses(aspnet_User user, bool isFormerTeacher, Configuration_Year year, Category_Faculty faculty, Category_Grade grade)
        //{
        //    List<Class_Class> lClasses = new List<Class_Class>();
        //    if (faculty == null)
        //    {
        //        if (grade == null)
        //        {
        //            if (isFormerTeacher)
        //            {
        //                lClasses = classDA.GetClasses(user, year);
        //            }
        //            else
        //            {
        //                lClasses = classDA.GetClasses(year);
        //            }
        //        }
        //        else
        //        {
        //            if (isFormerTeacher)
        //            {
        //                lClasses = classDA.GetClasses(user, year, grade);
        //            }
        //            else
        //            {
        //                lClasses = classDA.GetClasses(year, grade);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (grade == null)
        //        {
        //            if (isFormerTeacher)
        //            {
        //                lClasses = classDA.GetClasses(user, year, faculty);
        //            }
        //            else
        //            {
        //                lClasses = classDA.GetClasses(year, faculty);
        //            }
        //        }
        //        else
        //        {
        //            if (isFormerTeacher)
        //            {
        //                lClasses = classDA.GetClasses(user, year, faculty, grade);
        //            }
        //            else
        //            {
        //                lClasses = classDA.GetClasses(year, faculty, grade);
        //            }
        //        }
        //    }

        //    return lClasses;
        //}

        public List<Class_Class> GetClasses(aspnet_User user, bool isFormerTeacher, bool isSubjectTecher,
            Configuration_Year year, Category_Faculty faculty, Category_Grade grade, Configuration_Term term)
        {
            TeacherBL teacherBL = new TeacherBL(school);
            List<Class_Class> classes = new List<Class_Class>();

            if (term == null)
            {
                term = (new SystemConfigBL(school)).GetCurrentTerm();
            }

            if (faculty == null) // faculty = "All"
            {
                if (grade == null) // grade = "All"
                {
                    if (isFormerTeacher == false && isSubjectTecher == false)
                    {
                        // Get list of class in specified year
                        classes = classDA.GetClasses(year);
                    }
                    else
                    {
                        if (isFormerTeacher)
                        {
                            // Get list of class that teacher formering in specified year
                            classes = classDA.GetClasses(user, year);
                        }

                        if (isSubjectTecher)
                        {
                            classes.AddRange(teacherBL.GetTaughtClasses(user, year, term));
                        }
                    }
                }
                else
                {
                    if (isFormerTeacher == false && isSubjectTecher == false)
                    {
                        // Get list of class in specified year and grade
                        classes = classDA.GetClasses(year, grade);
                    }
                    else
                    {
                        if (isFormerTeacher)
                        {
                            // Get list of class that teacher formering in specified year and grade
                            classes = classDA.GetClasses(user, year, grade);
                        }

                        if (isSubjectTecher)
                        {
                            classes.AddRange(teacherBL.GetTaughtClasses(user, year, term, grade));
                        }
                    }
                }
            }
            else
            {
                if (grade == null) // grade = "All"
                {
                    if (isFormerTeacher == false && isSubjectTecher == false)
                    {
                        // Get list of class in specified year and faculty
                        classes = classDA.GetClasses(year, faculty);
                    }
                    else
                    {
                        if (isFormerTeacher)
                        {
                            // Get list of class that teacher formering in specified year and faculty
                            classes = classDA.GetClasses(user, year, faculty);
                        }

                        if (isSubjectTecher)
                        {
                            classes.AddRange(teacherBL.GetTaughtClasses(user, year, term, faculty));
                        }
                    }
                }
                else
                {
                    if (isFormerTeacher == false && isSubjectTecher == false)
                    {
                        // Get list of class in specified year, grade and faculty
                        classes = classDA.GetClasses(year, faculty, grade);
                    }
                    else
                    {
                        if (isFormerTeacher)
                        {
                            // Get list of class that teacher formering in specified year, grade and faculty
                            classes = classDA.GetClasses(user, year, faculty, grade);
                        }

                        if (isSubjectTecher)
                        {
                            classes.AddRange(teacherBL.GetTaughtClasses(user, year, term, faculty, grade));
                        }
                    }
                }
            }

            classes = classes.GroupBy(c => c.ClassId).Select(g => g.First()).ToList();

            // return result
            return classes;
        }

        public List<TabularClass> GetTabularClasses(Configuration_Year year, Category_Faculty faculty, Category_Grade grade, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularClass> lTabularClasses = new List<TabularClass>();
            List<Class_Class> lClasses = new List<Class_Class>();

            if (faculty == null)
            {
                if (grade == null)
                {
                    lClasses = classDA.GetClasses(year, pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    lClasses = classDA.GetClasses(year, grade, pageCurrentIndex, pageSize, out totalRecords);
                }
            }
            else
            {
                if (grade == null)
                {
                    lClasses = classDA.GetClasses(year, faculty, pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    lClasses = classDA.GetClasses(year, faculty, grade, pageCurrentIndex, pageSize, out totalRecords);
                }
            }

            TabularClass tabularClass = null;
            foreach (Class_Class Class in lClasses)
            {
                tabularClass = ConvertToTabularClass(Class);
                lTabularClasses.Add(tabularClass);
            }

            return lTabularClasses;
        }

        public List<Class_Class> GetUnformeredClasses(Configuration_Year year, Category_Faculty faculty, Category_Grade grade)
        {
            List<Class_Class> lUnformeredClasses = new List<Class_Class>();
            if (faculty == null)
            {
                if (grade == null)
                {
                    lUnformeredClasses = classDA.GetUnformeredClasses(year);
                }
                else
                {
                    lUnformeredClasses = classDA.GetUnformeredClasses(year, faculty);
                }
            }
            else
            {
                if (grade == null)
                {
                    lUnformeredClasses = classDA.GetUnformeredClasses(year, grade);
                }
                else
                {
                    lUnformeredClasses = classDA.GetUnformeredClasses(year, faculty, grade);
                }
            }

            return lUnformeredClasses;

        }

        public bool ClassNameExists(string className, Configuration_Year year)
        {
            return classDA.ClassNameExists(className, year);
        }

        public bool ClassNameExists(string oldClassName, string newClassName, Configuration_Year year)
        {
            if (oldClassName == newClassName)
            {
                return false;
            }
            else
            {
                return classDA.ClassNameExists(newClassName, year);
            }
        }

        public bool IsDeletable(Class_Class Class)
        {
            return classDA.IsDeletable(Class);
        }

        public bool HasFormerTeacher(Class_Class Class)
        {
            return classDA.HasFormerTeacher(Class);
        }

        public TabularClass ConvertToTabularClass(Class_Class Class)
        {
            TabularClass tabularClass = new TabularClass();
            FormerTeacherBL formerTeacherBL = new FormerTeacherBL(school);
            Class_FormerTeacher formerTeacher = null;

            tabularClass.ClassId = Class.ClassId;
            tabularClass.ClassName = Class.ClassName;
            tabularClass.FacultyName = Class.Category_Faculty.FacultyName;
            tabularClass.FacultyId = Class.FacultyId;
            tabularClass.GradeId = Class.GradeId;
            tabularClass.GradeName = Class.Category_Grade.GradeName;
            tabularClass.SiSo = Class.StudentQuantity;
            tabularClass.YearId = Class.YearId;
            tabularClass.YearName = Class.Configuration_Year.YearName;
            formerTeacher = formerTeacherBL.GetFormerTeacher(Class);
            if (formerTeacher != null)
            {
                tabularClass.HomeroomTeacherCode = formerTeacher.aspnet_User.UserId;
                tabularClass.TenGVCN = formerTeacher.aspnet_User.aspnet_Membership.FullName;
            }

            return tabularClass;
        }
    }
}
