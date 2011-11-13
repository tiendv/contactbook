﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class TeacherBL
    {
        private TeacherDA teacherDA;

        public TeacherBL()
        {
            teacherDA = new TeacherDA();
        }

        public void InsertTeacher(string maHienThi, string hoTen, bool gioiTinh,
            DateTime ngaySinh, string diaChi, string dienThoai)
        {
            LopHoc_GiaoVien giaoVien = new LopHoc_GiaoVien
            {
                MaHienThiGiaoVien = maHienThi,
                HoTen = hoTen,
                GioiTinh = gioiTinh,
                NgaySinh = ngaySinh,
                DiaChi = diaChi,
                DienThoai = dienThoai
            };

            teacherDA.InsertTeacher(giaoVien);
        }

        public void DeleteTeacher(string teacherCode)
        {
            LopHoc_GiaoVien teacher = GetTeacher(teacherCode);
            teacherDA.DeleteTeacher(teacher);
        }

        public void DeleteTeacher(LopHoc_GiaoVien teacher)
        {
            teacherDA.DeleteTeacher(teacher);
        }

        public void UpdateTeacher(LopHoc_GiaoVien editedTeacher, string newTeacherName, bool newGender, DateTime newBirthday, string newAddress, string newPhone)
        {
            editedTeacher.HoTen = newTeacherName;
            editedTeacher.GioiTinh = newGender;
            editedTeacher.NgaySinh = newBirthday;
            editedTeacher.DiaChi = newAddress;
            editedTeacher.DienThoai = newPhone;

            teacherDA.UpdateTeacher(editedTeacher);
        }

        public LopHoc_GiaoVien GetTeacher(string teacherCode)
        {
            return teacherDA.GetTeacher(teacherCode);
        }

        public LopHoc_GiaoVien GetTeacher(int teacherId)
        {
            return teacherDA.GetTeacher(teacherId);
        }

        public List<TabularTeacher> GetListTabularTeachers(string teacherCode, string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<LopHoc_GiaoVien> lTeachers = new List<LopHoc_GiaoVien>();
            List<TabularTeacher> lTabularTeachers = new List<TabularTeacher>();

            if ((teacherCode == "") || (string.Compare(teacherCode, "tất cả", true) == 0))
            {
                if ((teacherName == "") || (string.Compare(teacherName, "tất cả", true) == 0))
                {
                    lTeachers = teacherDA.GetListTeachers(
                        pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    lTeachers = teacherDA.GetListTeachersByName(teacherName,
                        pageCurrentIndex, pageSize, out totalRecords);
                }
            }
            else
            {
                if ((teacherName == "") || (string.Compare(teacherName, "tất cả", true) == 0))
                {
                    lTeachers = teacherDA.GetListTeachersByCode(teacherCode,
                        pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    lTeachers = teacherDA.GetListTeachers(teacherCode, teacherName,
                        pageCurrentIndex, pageSize, out totalRecords);
                }
            }

            foreach (LopHoc_GiaoVien teacher in lTeachers)
            {
                TabularTeacher tbTeacher = new TabularTeacher();
                tbTeacher.MaGiaoVien = teacher.MaGiaoVien;
                tbTeacher.MaHienThiGiaoVien = teacher.MaHienThiGiaoVien;
                tbTeacher.HoTen = teacher.HoTen;
                tbTeacher.NgaySinh = teacher.NgaySinh;
                tbTeacher.GioiTinh = teacher.GioiTinh;

                lTabularTeachers.Add(tbTeacher);
            }
            return lTabularTeachers;
        }

        public List<TabularTeacher> GetListTabularUnformeredTeachers(CauHinh_NamHoc year, string teacherCode, string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<LopHoc_GiaoVien> lTeachers = new List<LopHoc_GiaoVien>();
            List<TabularTeacher> lTabularTeachers = new List<TabularTeacher>();

            if ((teacherCode == "") || (string.Compare(teacherCode, "tất cả", true) == 0))
            {
                if ((teacherName == "") || (string.Compare(teacherName, "tất cả", true) == 0))
                {
                    lTeachers = teacherDA.GetListUnformedTeachers(year, pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    lTeachers = teacherDA.GetListUnformedTeachersByName(year, teacherName, pageCurrentIndex, pageSize, out totalRecords);
                }
            }
            else
            {
                if ((teacherName == "") || (string.Compare(teacherName, "tất cả", true) == 0))
                {
                    lTeachers = teacherDA.GetListUnformedTeachersByCode(year, teacherName, pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    lTeachers = teacherDA.GetListUnformedTeachers(year, teacherCode, teacherName, pageCurrentIndex, pageSize, out totalRecords);
                }
            }

            foreach (LopHoc_GiaoVien teacher in lTeachers)
            {
                TabularTeacher tbTeacher = new TabularTeacher();
                tbTeacher.MaGiaoVien = teacher.MaGiaoVien;
                tbTeacher.MaHienThiGiaoVien = teacher.MaHienThiGiaoVien;
                tbTeacher.HoTen = teacher.HoTen;
                tbTeacher.NgaySinh = teacher.NgaySinh;
                tbTeacher.GioiTinh = teacher.GioiTinh;

                lTabularTeachers.Add(tbTeacher);
            }
            return lTabularTeachers;
        }

        public bool TeacherCodeExists(string teacherCode)
        {
            return teacherDA.TeacherCodeExists(teacherCode);
        }

        public bool IsDeletable(string teacherCode)
        {
            LopHoc_GiaoVien teacher = GetTeacher(teacherCode);
            return teacherDA.IsDeletable(teacher);
        }

        public bool IsTeaching(LopHoc_GiaoVien teacher, CauHinh_HocKy term, CauHinh_Thu dayInWeek, DanhMuc_Tiet teachingPeriod)
        {
            return teacherDA.IsTeaching(teacher, term, dayInWeek, teachingPeriod);
        }

        public List<TabularFormering> GetListFormerings(LopHoc_GiaoVien teacher, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularFormering> lTbFormering = new List<TabularFormering>();
            List<LopHoc_GVCN> lFormering = teacherDA.GetFormering(teacher, pageCurrentIndex, pageSize, out totalRecords);

            foreach (LopHoc_GVCN formering in lFormering)
            {
                TabularFormering tbFormering = new TabularFormering
                              {
                                  MaNamHoc = formering.LopHoc_Lop.MaNamHoc,
                                  TenNamHoc = formering.LopHoc_Lop.CauHinh_NamHoc.TenNamHoc,
                                  MaLopHoc = formering.LopHoc_Lop.MaLopHoc,
                                  TenLopHoc = formering.LopHoc_Lop.TenLopHoc
                              };
                lTbFormering.Add(tbFormering);
            }

            return lTbFormering;
        }

        public List<TabularTeaching> GetListTeachings(LopHoc_GiaoVien teacher, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularTeaching> lTeachings = new List<TabularTeaching>();
            List<LopHoc_MonHocTKB> lShedules = new List<LopHoc_MonHocTKB>();

            lShedules = teacherDA.GetTeaching(teacher, pageCurrentIndex, pageSize, out totalRecords);

            foreach (LopHoc_MonHocTKB schedule in lShedules)
            {
                TabularTeaching tbTeaching = new TabularTeaching
                {
                    MaNamHoc = schedule.LopHoc_Lop.CauHinh_NamHoc.MaNamHoc,
                    TenNamHoc = schedule.LopHoc_Lop.CauHinh_NamHoc.TenNamHoc,
                    MaHocKy = schedule.CauHinh_HocKy.MaHocKy,
                    TenHocKy = schedule.CauHinh_HocKy.TenHocKy,
                    MaLopHoc = schedule.LopHoc_Lop.MaLopHoc,
                    TenLopHoc = schedule.LopHoc_Lop.TenLopHoc,
                    MaMonHoc = schedule.DanhMuc_MonHoc.MaMonHoc,
                    TenMonHoc = schedule.DanhMuc_MonHoc.TenMonHoc
                };

                lTeachings.Add(tbTeaching);
            }

            return lTeachings;
        }

    }
}
