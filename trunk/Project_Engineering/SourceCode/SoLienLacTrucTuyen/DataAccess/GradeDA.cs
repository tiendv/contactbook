﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
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
                                                  && grad.SchoolId == school.SchoolId
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
                                                  && grad.SchoolId == school.SchoolId
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
                                                  where grad.GradeName == gradeName
                                                  && grad.SchoolId == school.SchoolId
                                                  select grad;
            if (iqGrade.Count() != 0)
            {
                grade = iqGrade.First();
            }

            return grade;
        }

        public List<Category_Grade> GetGrades()
        {
            List<Category_Grade> lGrades = new List<Category_Grade>();

            IQueryable<Category_Grade> iqGrade = from grad in db.Category_Grades
                                                  where grad.SchoolId == school.SchoolId
                                                  select grad;
            if (iqGrade.Count() != 0)
            {
                lGrades = iqGrade.OrderBy(grad => grad.DisplayedOrder).ToList();
            }

            return lGrades;
        }

        public List<Category_Grade> GetGrades(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<Category_Grade> lGrades = new List<Category_Grade>();
            IQueryable<Category_Grade> iqGrade = from grad in db.Category_Grades
                                                  where grad.SchoolId == school.SchoolId
                                                  select grad;
            totalRecords = iqGrade.Count();
            if (totalRecords != 0)
            {
                lGrades = iqGrade.OrderBy(grad => grad.DisplayedOrder)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return lGrades;
        }

        public bool IsDeletable(Category_Grade grade)
        {
            var lop = from l in db.Class_Classes
                      where l.GradeId == grade.GradeId
                      && l.SchoolId == school.SchoolId
                      select l;
            bool bConstraintToLop = (lop.Count() != 0) ? true : false;

            var mon = from m in db.Category_Subjects
                      where m.GradeId == grade.GradeId
                      && m.Category_Grade.SchoolId == school.SchoolId
                      select m;
            bool bConstraintToMon = (mon.Count() != 0) ? true : false;

            if (bConstraintToLop || bConstraintToMon)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool GradeNameExists(string gradeName)
        {
            IQueryable<Category_Grade> iqGrade = from grad in db.Category_Grades
                                                  where grad.GradeName == gradeName
                                                  && grad.SchoolId == school.SchoolId
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
    }
}