using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class TeacherDA : BaseDA
    {
        public TeacherDA()
            : base()
        { }

        public void InsertTeacher(LopHoc_GiaoVien teacher)
        {
            db.LopHoc_GiaoViens.InsertOnSubmit(teacher);
            db.SubmitChanges();
        }

        public void UpdateTeacher(LopHoc_GiaoVien editedTeacher)
        {
            IQueryable<LopHoc_GiaoVien> iqTeacher = from tchr in db.LopHoc_GiaoViens
                                                    where tchr.MaGiaoVien == editedTeacher.MaGiaoVien
                                                    select tchr;

            if (iqTeacher.Count() != 0)
            {
                LopHoc_GiaoVien teacher = iqTeacher.First();
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

        public void DeleteTeacher(LopHoc_GiaoVien deletedTeacher)
        {
            IQueryable<LopHoc_GiaoVien> iqTeacher = from tchr in db.LopHoc_GiaoViens
                                                    where tchr.MaGiaoVien == deletedTeacher.MaGiaoVien
                                                    select tchr;

            if (iqTeacher.Count() != 0)
            {
                LopHoc_GiaoVien teacher = iqTeacher.First();
                db.LopHoc_GiaoViens.DeleteOnSubmit(teacher);
                db.SubmitChanges();
            }
        }

        public LopHoc_GiaoVien GetTeacher(string teacherCode)
        {
            LopHoc_GiaoVien teacher = null;

            IQueryable<LopHoc_GiaoVien> iqTeacher = from tchr in db.LopHoc_GiaoViens
                                                    where tchr.MaHienThiGiaoVien == teacherCode
                                                    select tchr;

            if (iqTeacher.Count() != 0)
            {
                teacher = iqTeacher.First();
            }

            return teacher;
        }

        public LopHoc_GiaoVien GetTeacher(int teacherId)
        {
            LopHoc_GiaoVien teacher = null;

            IQueryable<LopHoc_GiaoVien> iqTeacher = from tchr in db.LopHoc_GiaoViens
                                                    where tchr.MaGiaoVien == teacherId
                                                    select tchr;

            if (iqTeacher.Count() != 0)
            {
                teacher = iqTeacher.First();
            }

            return teacher;
        }
        private List<LopHoc_GiaoVien> GetListTeachers(ref IQueryable<LopHoc_GiaoVien> iqTeacher,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<LopHoc_GiaoVien> lTeachers = new List<LopHoc_GiaoVien>();

            totalRecords = iqTeacher.Count();
            if (totalRecords != 0)
            {
                lTeachers = iqTeacher.OrderBy(giaoVien => giaoVien.MaHienThiGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return lTeachers;
        }

        public List<LopHoc_GiaoVien> GetListTeachers(string teacherCode, string teacherName,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<LopHoc_GiaoVien> iqTeacher = from tchr in db.LopHoc_GiaoViens
                                                    where tchr.MaHienThiGiaoVien == teacherCode
                                                      && tchr.HoTen == teacherName
                                                    select tchr;

            return GetListTeachers(ref iqTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<LopHoc_GiaoVien> GetListTeachers(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<LopHoc_GiaoVien> iqTeacher = from tchr in db.LopHoc_GiaoViens
                                                    select tchr;

            return GetListTeachers(ref iqTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<LopHoc_GiaoVien> GetListTeachersByCode(string teacherCode,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<LopHoc_GiaoVien> iqTeacher = from tchr in db.LopHoc_GiaoViens
                                                    where tchr.MaHienThiGiaoVien == teacherCode
                                                    select tchr;

            return GetListTeachers(ref iqTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<LopHoc_GiaoVien> GetListTeachersByName(string teacherName,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<LopHoc_GiaoVien> iqTeacher = from tchr in db.LopHoc_GiaoViens
                                                    where tchr.HoTen == teacherName
                                                    select tchr;

            return GetListTeachers(ref iqTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<LopHoc_GiaoVien> GetListUnformedTeachersByName(CauHinh_NamHoc year,
            string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<LopHoc_GiaoVien> lTeachers = new List<LopHoc_GiaoVien>();
            IQueryable<LopHoc_GiaoVien> iqTeacher = from tchr in db.LopHoc_GiaoViens
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
                                        && fTchr.LopHoc_GiaoVien.HoTen == teacherName
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

        public List<LopHoc_GiaoVien> GetListUnformedTeachers(CauHinh_NamHoc year,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<LopHoc_GiaoVien> lTeachers = new List<LopHoc_GiaoVien>();
            IQueryable<LopHoc_GiaoVien> iqTeacher = from tchr in db.LopHoc_GiaoViens
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

        public List<LopHoc_GiaoVien> GetListUnformedTeachersByCode(CauHinh_NamHoc year,
            string teacherCode, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<LopHoc_GiaoVien> lTeachers = new List<LopHoc_GiaoVien>();
            IQueryable<LopHoc_GiaoVien> iqTeacher = from tchr in db.LopHoc_GiaoViens
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
                                        && fTchr.LopHoc_GiaoVien.MaHienThiGiaoVien == teacherCode
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

        public List<LopHoc_GiaoVien> GetListUnformedTeachers(CauHinh_NamHoc year,
           string teacherCode, string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<LopHoc_GiaoVien> lTeachers = new List<LopHoc_GiaoVien>();
            IQueryable<LopHoc_GiaoVien> iqTeacher = from tchr in db.LopHoc_GiaoViens
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
                                        && fTchr.LopHoc_GiaoVien.MaHienThiGiaoVien == teacherCode
                                        && fTchr.LopHoc_GiaoVien.HoTen == teacherName
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
            IQueryable<LopHoc_GiaoVien> giaoViens;
            giaoViens = from giaoVien in db.LopHoc_GiaoViens
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

        public bool IsDeletable(LopHoc_GiaoVien teacher)
        {
            return true;
        }
        
        public bool IsTeaching(LopHoc_GiaoVien teacher, CauHinh_HocKy term, CauHinh_Thu dayInWeek, DanhMuc_Tiet teachingPeriod)
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

        public List<LopHoc_GVCN> GetFormering(LopHoc_GiaoVien teacher, int pageCurrentIndex, int pageSize, out double totalRecords)
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

        public List<LopHoc_MonHocTKB> GetTeaching(LopHoc_GiaoVien teacher, int pageCurrentIndex, int pageSize, out double totalRecords)
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
