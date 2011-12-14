using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class GradeBL : BaseBL
    {
        private GradeDA gradeDA;

        public GradeBL(School_School school)
            : base(school)
        {
            gradeDA = new GradeDA(school);
        }

        public void InsertGrade(string gradeName, short displayOrder)
        {
            Category_Grade grade = new Category_Grade
            {
                GradeName = gradeName,
                DisplayedOrder = displayOrder
            };

            gradeDA.InsertGrade(grade);
        }

        public void UpdateGrade(string editedGradeName, string newGradeName, short newDisplayOrder)
        {
            Category_Grade grade = GetGrade(editedGradeName);
            grade.GradeName = newGradeName;
            grade.DisplayedOrder = newDisplayOrder;
            gradeDA.UpdateGrade(grade);
        }

        public void DeleteGrade(string gradeName)
        {
            Category_Grade grade = GetGrade(gradeName);
            gradeDA.DeleteGrade(grade);
        }

        public Category_Grade GetGrade(string gradeName)
        {
            return gradeDA.GetGrade(gradeName);
        }

        public List<Category_Grade> GetListGrades()
        {
            return gradeDA.GetGrades();
        }

        public List<Category_Grade> GetListGrades(string gradeName, int pageIndex, int pageSize, out double totalRecords)
        {
            List<Category_Grade> lGrades = new List<Category_Grade>();

            if (String.Compare(gradeName, "tất cả", true) == 0 || gradeName == "")
            {
                lGrades = gradeDA.GetGrades(pageIndex, pageSize, out totalRecords);
            }
            else
            {
                Category_Grade grade = GetGrade(gradeName);
                lGrades.Add(grade);
                totalRecords = 1;
            }

            return lGrades;
        }

        public bool IsDeletable(string gradeName)
        {
            Category_Grade grade = GetGrade(gradeName);
            return (!gradeDA.ClassInGradeExists(grade) && !gradeDA.SubjectInGradeExists(grade));
        }

        public bool GradeNameExists(string gradeName)
        {
            return gradeDA.GradeNameExists(gradeName);
        }

        public bool GradeNameExists(string oldGradeName, string newGradeName)
        {
            if (oldGradeName == newGradeName)
            {
                return false;
            }
            else
            {
                return gradeDA.GradeNameExists(newGradeName);
            }
        }

        public Category_Grade GetGrade(int gradeId)
        {
            return gradeDA.GetGrade(gradeId);
        }
    }
}
