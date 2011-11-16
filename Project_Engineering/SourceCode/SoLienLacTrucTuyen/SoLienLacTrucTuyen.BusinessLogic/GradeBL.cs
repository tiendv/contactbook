using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class GradeBL : BaseBL
    {
        private GradeDA gradeDA;

        public GradeBL(School school)
            : base(school)
        {
            gradeDA = new GradeDA(school);
        }

        public void InsertGrade(string gradeName, short displayOrder)
        {
            DanhMuc_KhoiLop grade = new DanhMuc_KhoiLop
            {
                TenKhoiLop = gradeName,
                ThuTuHienThi = displayOrder
            };

            gradeDA.InsertGrade(grade);
        }

        public void UpdateGrade(string editedGradeName, string newGradeName, short newDisplayOrder)
        {
            DanhMuc_KhoiLop grade = GetGrade(editedGradeName);
            grade.TenKhoiLop = newGradeName;
            grade.ThuTuHienThi = newDisplayOrder;
            gradeDA.UpdateGrade(grade);
        }

        public void DeleteGrade(string gradeName)
        {
            DanhMuc_KhoiLop grade = GetGrade(gradeName);
            gradeDA.DeleteGrade(grade);
        }

        public DanhMuc_KhoiLop GetGrade(string gradeName)
        {
            return gradeDA.GetGrade(gradeName);
        }

        public List<DanhMuc_KhoiLop> GetListGrades()
        {
            return gradeDA.GetGrades();
        }

        public List<DanhMuc_KhoiLop> GetListGrades(string gradeName, int pageIndex, int pageSize, out double totalRecords)
        {
            List<DanhMuc_KhoiLop> lGrades = new List<DanhMuc_KhoiLop>();

            if (String.Compare(gradeName, "tất cả", true) == 0 || gradeName == "")
            {
                lGrades = gradeDA.GetGrades(pageIndex, pageSize, out totalRecords);
            }
            else
            {
                DanhMuc_KhoiLop grade = GetGrade(gradeName);
                lGrades.Add(grade);
                totalRecords = 1;
            }

            return lGrades;
        }

        public bool IsDeletable(string gradeName)
        {
            DanhMuc_KhoiLop grade = GetGrade(gradeName);
            return gradeDA.IsDeletable(grade);
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
    }
}
