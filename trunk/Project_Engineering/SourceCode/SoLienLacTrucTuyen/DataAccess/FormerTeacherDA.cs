using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class FormerTeacherDA : BaseDA
    {
        public FormerTeacherDA(School school)
            : base(school)
        {

        }

        public void InsertFormerTeacher(LopHoc_Lop Class, LopHoc_GiaoVien teacher)
        {
            LopHoc_GVCN formerTeacher = new LopHoc_GVCN
            {
                MaLopHoc = Class.MaLopHoc,
                MaGiaoVien = teacher.MaGiaoVien
            };

            db.LopHoc_GVCNs.InsertOnSubmit(formerTeacher);
            db.SubmitChanges();
        }

        public void UpdateFormerTeacher(int formerTeacherId, LopHoc_GiaoVien teacher)
        {
            IQueryable<LopHoc_GVCN> iqFormerTeacher = from fTchr in db.LopHoc_GVCNs
                                                      where fTchr.MaGVCN == formerTeacherId
                                                      select fTchr;
            if (iqFormerTeacher.Count() != 0)
            {
                LopHoc_GVCN formerTeacher = iqFormerTeacher.First();
                formerTeacher.MaGiaoVien = teacher.MaGiaoVien;
                db.SubmitChanges();
            }
        }

        public void DeleteFormerTeacher(LopHoc_GVCN frmrTeacher)
        {
            IQueryable<LopHoc_GVCN> iqFormerTeacher = from fTchr in db.LopHoc_GVCNs
                                                      where fTchr.MaGVCN == frmrTeacher.MaGVCN
                                                      select fTchr;
            if (iqFormerTeacher.Count() != 0)
            {
                LopHoc_GVCN formerTeacher = iqFormerTeacher.First();
                db.LopHoc_GVCNs.DeleteOnSubmit(formerTeacher);
                db.SubmitChanges();
            }
        }

        private List<LopHoc_GVCN> GetFormerTeachers(ref IQueryable<LopHoc_GVCN> iqFormerTeacher, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<LopHoc_GVCN> lFormerTeacher = new List<LopHoc_GVCN>();
            totalRecords = iqFormerTeacher.Count();
            if (totalRecords != 0)
            {
                lFormerTeacher = iqFormerTeacher.OrderBy(teacher => teacher.MaGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return lFormerTeacher;
        }

        public List<LopHoc_GVCN> GetFormerTeachers(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<LopHoc_GVCN> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.LopHoc_GVCNs
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<LopHoc_GVCN> GetFormerTeachers(LopHoc_Lop Class, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<LopHoc_GVCN> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.LopHoc_GVCNs
                             where formerTeacher.LopHoc_Lop.MaLopHoc == Class.MaLopHoc
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<LopHoc_GVCN> GetFormerTeachersByCode(LopHoc_Lop Class, string teacherCode, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<LopHoc_GVCN> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.LopHoc_GVCNs
                             where formerTeacher.LopHoc_Lop.MaLopHoc == Class.MaLopHoc
                               && formerTeacher.LopHoc_GiaoVien.MaHienThiGiaoVien == teacherCode
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<LopHoc_GVCN> GetFormerTeachers(LopHoc_Lop Class, string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<LopHoc_GVCN> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.LopHoc_GVCNs
                             where formerTeacher.LopHoc_Lop.MaLopHoc == Class.MaLopHoc
                               && formerTeacher.LopHoc_GiaoVien.HoTen == teacherName
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<LopHoc_GVCN> GetFormerTeachers(CauHinh_NamHoc year, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<LopHoc_GVCN> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.LopHoc_GVCNs
                             where formerTeacher.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<LopHoc_GVCN> GetFormerTeachers(CauHinh_NamHoc year, string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<LopHoc_GVCN> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.LopHoc_GVCNs
                             where formerTeacher.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                             && formerTeacher.LopHoc_GiaoVien.HoTen == teacherName
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<LopHoc_GVCN> GetFormerTeachersByCode(CauHinh_NamHoc year, string teacherCode, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<LopHoc_GVCN> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.LopHoc_GVCNs
                             where formerTeacher.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                             && formerTeacher.LopHoc_GiaoVien.MaHienThiGiaoVien == teacherCode
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<LopHoc_GVCN> GetFormerTeachers(CauHinh_NamHoc year, DanhMuc_NganhHoc faculty, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<LopHoc_GVCN> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.LopHoc_GVCNs
                             where formerTeacher.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                             && formerTeacher.LopHoc_Lop.MaNganhHoc == faculty.MaNganhHoc
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<LopHoc_GVCN> GetFormerTeachers(CauHinh_NamHoc year, DanhMuc_NganhHoc faculty, string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<LopHoc_GVCN> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.LopHoc_GVCNs
                             where formerTeacher.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                             && formerTeacher.LopHoc_Lop.MaNganhHoc == faculty.MaNganhHoc
                             && formerTeacher.LopHoc_GiaoVien.HoTen == teacherName
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<LopHoc_GVCN> GetFormerTeachersByCode(CauHinh_NamHoc year, DanhMuc_NganhHoc faculty, string teacherCode, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<LopHoc_GVCN> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.LopHoc_GVCNs
                             where formerTeacher.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                             && formerTeacher.LopHoc_Lop.MaNganhHoc == faculty.MaNganhHoc
                             && formerTeacher.LopHoc_GiaoVien.MaHienThiGiaoVien == teacherCode
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<LopHoc_GVCN> GetFormerTeachers(CauHinh_NamHoc year, DanhMuc_KhoiLop grade, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<LopHoc_GVCN> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.LopHoc_GVCNs
                             where formerTeacher.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                             && formerTeacher.LopHoc_Lop.MaKhoiLop == grade.MaKhoiLop
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<LopHoc_GVCN> GetFormerTeachers(CauHinh_NamHoc year, DanhMuc_KhoiLop grade, string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<LopHoc_GVCN> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.LopHoc_GVCNs
                             where formerTeacher.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                             && formerTeacher.LopHoc_Lop.MaKhoiLop == grade.MaKhoiLop
                             && formerTeacher.LopHoc_GiaoVien.HoTen == teacherName
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<LopHoc_GVCN> GetFormerTeachersByCode(CauHinh_NamHoc year, DanhMuc_KhoiLop grade, string teacherCode, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<LopHoc_GVCN> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.LopHoc_GVCNs
                             where formerTeacher.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                             && formerTeacher.LopHoc_Lop.MaKhoiLop == grade.MaKhoiLop
                             && formerTeacher.LopHoc_GiaoVien.MaHienThiGiaoVien == teacherCode
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<LopHoc_GVCN> GetFormerTeachers(CauHinh_NamHoc year, DanhMuc_NganhHoc faculty, DanhMuc_KhoiLop grade, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<LopHoc_GVCN> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.LopHoc_GVCNs
                             where formerTeacher.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                             && formerTeacher.LopHoc_Lop.MaNganhHoc == faculty.MaNganhHoc
                             && formerTeacher.LopHoc_Lop.MaKhoiLop == grade.MaKhoiLop
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<LopHoc_GVCN> GetFormerTeachers(CauHinh_NamHoc year, DanhMuc_NganhHoc faculty, DanhMuc_KhoiLop grade, string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<LopHoc_GVCN> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.LopHoc_GVCNs
                             where formerTeacher.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                             && formerTeacher.LopHoc_Lop.MaNganhHoc == faculty.MaNganhHoc
                             && formerTeacher.LopHoc_Lop.MaKhoiLop == grade.MaKhoiLop
                             && formerTeacher.LopHoc_GiaoVien.HoTen == teacherName
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<LopHoc_GVCN> GetFormerTeachersByCode(CauHinh_NamHoc year, DanhMuc_NganhHoc faculty, DanhMuc_KhoiLop grade, string teacherCode, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<LopHoc_GVCN> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.LopHoc_GVCNs
                             where formerTeacher.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                             && formerTeacher.LopHoc_Lop.MaNganhHoc == faculty.MaNganhHoc
                             && formerTeacher.LopHoc_Lop.MaKhoiLop == grade.MaKhoiLop
                             && formerTeacher.LopHoc_GiaoVien.MaHienThiGiaoVien == teacherCode
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public LopHoc_GVCN GetFormerTeacher(int formerTeacherId)
        {
            LopHoc_GVCN giaoVienChuNhiem = (from gvcn in db.LopHoc_GVCNs
                                            where gvcn.MaGVCN == formerTeacherId
                                            select gvcn).First();
            return giaoVienChuNhiem;
        }

        public LopHoc_GVCN GetFormerTeacher(LopHoc_Lop Class)
        {
            LopHoc_GVCN formerTeacher = null;

            IQueryable<LopHoc_GVCN> iqFormerTeacher = from fTchr in db.LopHoc_GVCNs
                                                      where fTchr.MaLopHoc == Class.MaLopHoc
                                                      select fTchr;
            if (iqFormerTeacher.Count() != 0)
            {
                formerTeacher = iqFormerTeacher.First();
            }

            return formerTeacher;
        }


        public bool FormerTeacherExists(LopHoc_GiaoVien teacher)
        {
            IQueryable<LopHoc_GVCN> iqFormerTeacher = from fTchr in db.LopHoc_GVCNs
                                                      where fTchr.MaGiaoVien == teacher.MaGiaoVien
                                                      select fTchr;
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
