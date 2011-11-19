using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class TeacherDA : BaseDA
    {
        public TeacherDA(School school)
            : base(school)
        { }

        public void InsertTeacher(DanhMuc_GiaoVien teacher)
        {
            db.DanhMuc_GiaoViens.InsertOnSubmit(teacher);
            db.SubmitChanges();
        }

        public void UpdateTeacher(DanhMuc_GiaoVien editedTeacher)
        {
            IQueryable<DanhMuc_GiaoVien> iqTeacher = from tchr in db.DanhMuc_GiaoViens
                                                    where tchr.MaGiaoVien == editedTeacher.MaGiaoVien
                                                    select tchr;

            if (iqTeacher.Count() != 0)
            {
                DanhMuc_GiaoVien teacher = iqTeacher.First();
                teacher.MaHienThiGiaoVien = editedTeacher.MaHienThiGiaoVien;
                teacher.HoTen = editedTeacher.HoTen;
                teacher.GioiTinh = editedTeacher.GioiTinh;
                teacher.NgaySinh = editedTeacher.NgaySinh;
                teacher.HinhAnh = editedTeacher.HinhAnh;
                teacher.DiaChi = editedTeacher.DiaChi;
                teacher.DienThoai = editedTeacher.DienThoai;

                db.SubmitChanges();
            }
        }

        public void DeleteTeacher(DanhMuc_GiaoVien deletedTeacher)
        {
            IQueryable<DanhMuc_GiaoVien> iqTeacher = from tchr in db.DanhMuc_GiaoViens
                                                    where tchr.MaGiaoVien == deletedTeacher.MaGiaoVien
                                                    select tchr;

            if (iqTeacher.Count() != 0)
            {
                DanhMuc_GiaoVien teacher = iqTeacher.First();
                db.DanhMuc_GiaoViens.DeleteOnSubmit(teacher);
                db.SubmitChanges();
            }
        }

        public DanhMuc_GiaoVien GetTeacher(string teacherCode)
        {
            DanhMuc_GiaoVien teacher = null;

            IQueryable<DanhMuc_GiaoVien> iqTeacher = from tchr in db.DanhMuc_GiaoViens
                                                    where tchr.MaHienThiGiaoVien == teacherCode
                                                    select tchr;

            if (iqTeacher.Count() != 0)
            {
                teacher = iqTeacher.First();
            }

            return teacher;
        }

        public DanhMuc_GiaoVien GetTeacher(int teacherId)
        {
            DanhMuc_GiaoVien teacher = null;

            IQueryable<DanhMuc_GiaoVien> iqTeacher = from tchr in db.DanhMuc_GiaoViens
                                                    where tchr.MaGiaoVien == teacherId
                                                    select tchr;

            if (iqTeacher.Count() != 0)
            {
                teacher = iqTeacher.First();
            }

            return teacher;
        }
        private List<DanhMuc_GiaoVien> GetListTeachers(ref IQueryable<DanhMuc_GiaoVien> iqTeacher, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<DanhMuc_GiaoVien> lTeachers = new List<DanhMuc_GiaoVien>();

            totalRecords = iqTeacher.Count();
            if (totalRecords != 0)
            {
                lTeachers = iqTeacher.OrderBy(giaoVien => giaoVien.MaHienThiGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return lTeachers;
        }

        public List<DanhMuc_GiaoVien> GetListTeachers(string teacherCode, string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<DanhMuc_GiaoVien> iqTeacher = from tchr in db.DanhMuc_GiaoViens
                                                    where tchr.MaHienThiGiaoVien == teacherCode
                                                      && tchr.HoTen == teacherName
                                                    select tchr;

            return GetListTeachers(ref iqTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<DanhMuc_GiaoVien> GetListTeachers(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<DanhMuc_GiaoVien> iqTeacher = from tchr in db.DanhMuc_GiaoViens
                                                    select tchr;

            return GetListTeachers(ref iqTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<DanhMuc_GiaoVien> GetListTeachersByCode(string teacherCode, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<DanhMuc_GiaoVien> iqTeacher = from tchr in db.DanhMuc_GiaoViens
                                                    where tchr.MaHienThiGiaoVien == teacherCode
                                                    select tchr;

            return GetListTeachers(ref iqTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<DanhMuc_GiaoVien> GetListTeachersByName(string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<DanhMuc_GiaoVien> iqTeacher = from tchr in db.DanhMuc_GiaoViens
                                                    where tchr.HoTen == teacherName
                                                    select tchr;

            return GetListTeachers(ref iqTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<DanhMuc_GiaoVien> GetListUnformedTeachersByName(CauHinh_NamHoc year, string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<DanhMuc_GiaoVien> lTeachers = new List<DanhMuc_GiaoVien>();
            IQueryable<DanhMuc_GiaoVien> iqTeacher = from tchr in db.DanhMuc_GiaoViens
                                                    where tchr.HoTen == teacherName
                                                    select tchr;

            if (iqTeacher.Count() != 0)
            {
                lTeachers = iqTeacher.ToList();

                int i = 0;
                while (i < lTeachers.Count)
                {
                    IQueryable<LopHoc_GVCN> iqFormerTeacher;
                    iqFormerTeacher = from fTchr in db.LopHoc_GVCNs
                                      where fTchr.MaGiaoVien == lTeachers[i].MaGiaoVien
                                        && fTchr.DanhMuc_GiaoVien.HoTen == teacherName
                                        && fTchr.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                                      select fTchr;
                    if (iqFormerTeacher.Count() != 0)
                    {
                        lTeachers.Remove(lTeachers[i]);
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            totalRecords = lTeachers.Count;

            return lTeachers;
        }

        public List<DanhMuc_GiaoVien> GetListUnformedTeachers(CauHinh_NamHoc year, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<DanhMuc_GiaoVien> lTeachers = new List<DanhMuc_GiaoVien>();
            IQueryable<DanhMuc_GiaoVien> iqTeacher = from tchr in db.DanhMuc_GiaoViens
                                                    select tchr;

            if (iqTeacher.Count() != 0)
            {
                lTeachers = iqTeacher.ToList();

                int i = 0;
                while (i < lTeachers.Count)
                {
                    IQueryable<LopHoc_GVCN> iqFormerTeacher;
                    iqFormerTeacher = from fTchr in db.LopHoc_GVCNs
                                      where fTchr.MaGiaoVien == lTeachers[i].MaGiaoVien
                                          && fTchr.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                                      select fTchr;
                    if (iqFormerTeacher.Count() != 0)
                    {
                        lTeachers.Remove(lTeachers[i]);
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            totalRecords = lTeachers.Count;

            return lTeachers;
        }

        public List<DanhMuc_GiaoVien> GetListUnformedTeachersByCode(CauHinh_NamHoc year, string teacherCode, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<DanhMuc_GiaoVien> lTeachers = new List<DanhMuc_GiaoVien>();
            IQueryable<DanhMuc_GiaoVien> iqTeacher = from tchr in db.DanhMuc_GiaoViens
                                                    where tchr.MaHienThiGiaoVien == teacherCode
                                                    select tchr;

            if (iqTeacher.Count() != 0)
            {
                lTeachers = iqTeacher.ToList();

                int i = 0;
                while (i < lTeachers.Count)
                {
                    IQueryable<LopHoc_GVCN> iqFormerTeacher;
                    iqFormerTeacher = from fTchr in db.LopHoc_GVCNs
                                      where fTchr.MaGiaoVien == lTeachers[i].MaGiaoVien
                                        && fTchr.DanhMuc_GiaoVien.MaHienThiGiaoVien == teacherCode
                                        && fTchr.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                                      select fTchr;
                    if (iqFormerTeacher.Count() != 0)
                    {
                        lTeachers.Remove(lTeachers[i]);
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            totalRecords = lTeachers.Count;

            return lTeachers;
        }

        public List<DanhMuc_GiaoVien> GetListUnformedTeachers(CauHinh_NamHoc year, string teacherCode, string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<DanhMuc_GiaoVien> lTeachers = new List<DanhMuc_GiaoVien>();
            IQueryable<DanhMuc_GiaoVien> iqTeacher = from tchr in db.DanhMuc_GiaoViens
                                                    where tchr.HoTen == teacherName && tchr.MaHienThiGiaoVien == teacherCode
                                                    select tchr;

            if (iqTeacher.Count() != 0)
            {
                lTeachers = iqTeacher.ToList();

                int i = 0;
                while (i < lTeachers.Count)
                {
                    IQueryable<LopHoc_GVCN> iqFormerTeacher;
                    iqFormerTeacher = from fTchr in db.LopHoc_GVCNs
                                      where fTchr.MaGiaoVien == lTeachers[i].MaGiaoVien
                                        && fTchr.DanhMuc_GiaoVien.MaHienThiGiaoVien == teacherCode
                                        && fTchr.DanhMuc_GiaoVien.HoTen == teacherName
                                        && fTchr.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                                      select fTchr;
                    if (iqFormerTeacher.Count() != 0)
                    {
                        lTeachers.Remove(lTeachers[i]);
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            totalRecords = lTeachers.Count;

            return lTeachers;
        }

        public bool TeacherCodeExists(string teacherCode)
        {
            IQueryable<DanhMuc_GiaoVien> giaoViens;
            giaoViens = from giaoVien in db.DanhMuc_GiaoViens
                        where giaoVien.MaHienThiGiaoVien == teacherCode
                        select giaoVien;

            if (giaoViens.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsTeaching(DanhMuc_GiaoVien teacher, CauHinh_HocKy term, CauHinh_Thu dayInWeek, DanhMuc_Tiet teachingPeriod)
        {
            IQueryable<LopHoc_MonHocTKB> iqThoiKhoaBieu;
            iqThoiKhoaBieu = from tkb in db.LopHoc_MonHocTKBs
                             where tkb.MaGiaoVien == teacher.MaGiaoVien
                                && tkb.MaHocKy == term.MaHocKy
                                && tkb.MaThu == dayInWeek.MaThu
                                && tkb.MaTiet == teachingPeriod.MaTiet
                             select tkb;
            if (iqThoiKhoaBieu.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<LopHoc_GVCN> GetFormering(DanhMuc_GiaoVien teacher, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<LopHoc_GVCN> lFormering = new List<LopHoc_GVCN>();

            IQueryable<LopHoc_GVCN> iqFormering;
            iqFormering = from formering in db.LopHoc_GVCNs
                          where formering.MaGiaoVien == teacher.MaGiaoVien
                          select formering;

            totalRecords = iqFormering.Count();
            if (totalRecords != 0)
            {
                lFormering = iqFormering.OrderByDescending(formering => formering.LopHoc_Lop.MaNamHoc)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return lFormering;
        }

        public List<LopHoc_MonHocTKB> GetTeaching(DanhMuc_GiaoVien teacher, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<LopHoc_MonHocTKB> lShedules = new List<LopHoc_MonHocTKB>();

            IQueryable<LopHoc_MonHocTKB> iqSchedule;
            iqSchedule = from schedule in db.LopHoc_MonHocTKBs
                         where schedule.MaGiaoVien == teacher.MaGiaoVien
                         select schedule;

            totalRecords = iqSchedule.Count();
            if (totalRecords != 0)
            {
                lShedules = iqSchedule.OrderByDescending(schedule => schedule.LopHoc_Lop.CauHinh_NamHoc.TenNamHoc)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).Distinct().ToList();
            }

            return lShedules;
        }
    }
}
