using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class GradeDA : BaseDA
    {
        public GradeDA(School school)
            : base(school)
        {

        }

        public void InsertGrade(DanhMuc_KhoiLop grade)
        {
            db.DanhMuc_KhoiLops.InsertOnSubmit(grade);
            db.SubmitChanges();
        }

        public void UpdateGrade(DanhMuc_KhoiLop editedGrade)
        {
            DanhMuc_KhoiLop grade = null;
            IQueryable<DanhMuc_KhoiLop> iqGrade = from grad in db.DanhMuc_KhoiLops
                                                  where grad.MaKhoiLop == editedGrade.MaKhoiLop
                                                  select grad;
            if (iqGrade.Count() != 0)
            {
                grade = iqGrade.First();
                grade.TenKhoiLop = editedGrade.TenKhoiLop;
                grade.ThuTuHienThi = editedGrade.ThuTuHienThi;

                db.SubmitChanges();
            }
        }

        public void DeleteGrade(DanhMuc_KhoiLop deletedGrade)
        {
            DanhMuc_KhoiLop grade = null;
            IQueryable<DanhMuc_KhoiLop> iqGrade = from grad in db.DanhMuc_KhoiLops
                                                  where grad.MaKhoiLop == deletedGrade.MaKhoiLop
                                                  select grad;
            if (iqGrade.Count() != 0)
            {
                grade = iqGrade.First();
                db.DanhMuc_KhoiLops.DeleteOnSubmit(grade);
                db.SubmitChanges();
            }
        }

        public DanhMuc_KhoiLop GetGrade(string gradeName)
        {
            DanhMuc_KhoiLop grade = null;
            IQueryable<DanhMuc_KhoiLop> iqGrade = from grad in db.DanhMuc_KhoiLops
                                                  where grad.TenKhoiLop == gradeName
                                                  select grad;
            if (iqGrade.Count() != 0)
            {
                grade = iqGrade.First();
            }

            return grade;
        }

        public List<DanhMuc_KhoiLop> GetGrades()
        {
            List<DanhMuc_KhoiLop> lGrades = new List<DanhMuc_KhoiLop>();

            IQueryable<DanhMuc_KhoiLop> iqGrade = from grad in db.DanhMuc_KhoiLops
                                                  select grad;
            if (iqGrade.Count() != 0)
            {
                lGrades = iqGrade.OrderBy(grad => grad.ThuTuHienThi).ToList();
            }

            return lGrades;
        }

        public List<DanhMuc_KhoiLop> GetGrades(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<DanhMuc_KhoiLop> lGrades = new List<DanhMuc_KhoiLop>();
            IQueryable<DanhMuc_KhoiLop> iqGrade = from grad in db.DanhMuc_KhoiLops
                                                  select grad;
            totalRecords = iqGrade.Count();
            if (totalRecords != 0)
            {
                lGrades = iqGrade.OrderBy(grad => grad.ThuTuHienThi)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return lGrades;
        }

        public bool IsDeletable(DanhMuc_KhoiLop grade)
        {
            var lop = from l in db.LopHoc_Lops
                      where l.MaKhoiLop == grade.MaKhoiLop
                      select l;
            bool bConstraintToLop = (lop.Count() != 0) ? true : false;

            var mon = from m in db.DanhMuc_MonHocs
                      where m.MaKhoiLop == grade.MaKhoiLop
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
            IQueryable<DanhMuc_KhoiLop> iqGrade = from grad in db.DanhMuc_KhoiLops
                                                  where grad.TenKhoiLop == gradeName
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
