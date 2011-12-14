using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.DataAccess
{
    public class GradeDA : BaseDA
    {
        public GradeDA(School_School school)
            : base(school)
        {

        }

        public void InsertGrade(Category_Grade grade)
        {
            grade.SchoolId = school.SchoolId;
            db.Category_Grades.InsertOnSubmit(grade);
            db.SubmitChanges();
        }

        public void UpdateGrade(Category_Grade editedGrade)
        {
            Category_Grade grade = null;
            IQueryable<Category_Grade> iqGrade = from grad in db.Category_Grades
                                                 where grad.GradeId == editedGrade.GradeId
                                                 select grad;
            if (iqGrade.Count() != 0)
            {
                grade = iqGrade.First();
                grade.GradeName = editedGrade.GradeName;
                grade.DisplayedOrder = editedGrade.DisplayedOrder;

                db.SubmitChanges();
            }
        }

        public void DeleteGrade(Category_Grade deletedGrade)
        {
            Category_Grade grade = null;
            IQueryable<Category_Grade> iqGrade = from grad in db.Category_Grades
                                                 where grad.GradeId == deletedGrade.GradeId
                                                 select grad;
            if (iqGrade.Count() != 0)
            {
                grade = iqGrade.First();
                db.Category_Grades.DeleteOnSubmit(grade);
                db.SubmitChanges();
            }
        }

        public Category_Grade GetGrade(string gradeName)
        {
            Category_Grade grade = null;
            IQueryable<Category_Grade> iqGrade = from grad in db.Category_Grades
                                                 where grad.GradeName == gradeName && grad.SchoolId == school.SchoolId
                                                 select grad;
            if (iqGrade.Count() != 0)
            {
                grade = iqGrade.First();
            }

            return grade;
        }

        public List<Category_Grade> GetGrades()
        {
            List<Category_Grade> grades = new List<Category_Grade>();

            IQueryable<Category_Grade> iqGrade = from grad in db.Category_Grades
                                                 where grad.SchoolId == school.SchoolId
                                                 select grad;
            if (iqGrade.Count() != 0)
            {
                grades = iqGrade.OrderBy(grad => grad.DisplayedOrder).ToList();
            }

            return grades;
        }

        public List<Category_Grade> GetGrades(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<Category_Grade> grades = new List<Category_Grade>();
            IQueryable<Category_Grade> iqGrade = from grad in db.Category_Grades
                                                 where grad.SchoolId == school.SchoolId
                                                 select grad;
            totalRecords = iqGrade.Count();
            if (totalRecords != 0)
            {
                grades = iqGrade.OrderBy(grad => grad.DisplayedOrder)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return grades;
        }

        public bool GradeNameExists(string gradeName)
        {
            IQueryable<Category_Grade> iqGrade = from grad in db.Category_Grades
                                                 where grad.GradeName == gradeName && grad.SchoolId == school.SchoolId
                                                 select grad;

            if (iqGrade.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ClassInGradeExists(Category_Grade grade)
        {
            IQueryable<Class_Class> iqClass = from cls in db.Class_Classes
                                              where cls.Category_Grade.GradeId == grade.GradeId
                                              && cls.Category_Grade.SchoolId == school.SchoolId
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

        public bool SubjectInGradeExists(Category_Grade grade)
        {
            IQueryable<Category_Subject> iqSubject = from subject in db.Category_Subjects
                                                     where subject.Category_Grade.GradeId == grade.GradeId
                                                     && subject.Category_Grade.SchoolId == school.SchoolId
                                                     select subject;

            if (iqSubject.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Category_Grade GetGrade(int gradeId)
        {
            Category_Grade grade = null;
            IQueryable<Category_Grade> iqGrade = from grad in db.Category_Grades
                                                 where grad.GradeId == gradeId
                                                 select grad;
            if (iqGrade.Count() != 0)
            {
                grade = iqGrade.First();
            }

            return grade;
        }
    }
}
