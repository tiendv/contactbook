using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class TeacherBL : BaseBL
    {
        private TeacherDA teacherDA;

        public TeacherBL(School school)
            : base(school)
        {
            teacherDA = new TeacherDA(school);
        }

        //public void InsertTeacher(string teacherCode, string teacherName, bool gender, DateTime birthday, string address, string phone)
        //{
        //    aspnet_User teacher = new aspnet_User
        //    {
        //        MaHienThiGiaoVien = teacherCode,
        //        HoTen = teacherName,
        //        GioiTinh = gender,
        //        NgaySinh = birthday,
        //        DiaChi = address,
        //        DienThoai = phone
        //    };

        //    teacherDA.InsertTeacher(teacher);
        //}

        //public void DeleteTeacher(aspnet_User teacher)
        //{
        //    teacherDA.DeleteTeacher(teacher);
        //}

        public void UpdateTeacher(aspnet_Membership editedTeacher, string newTeacherName, bool newGender, DateTime newBirthday, string newAddress, string newPhone)
        {
            editedTeacher.RealName = newTeacherName;
            editedTeacher.Gender = newGender;
            editedTeacher.Birthday = newBirthday;
            editedTeacher.Address = newAddress;
            editedTeacher.Phone = newPhone;

            teacherDA.UpdateTeacher(editedTeacher);
        }

        public aspnet_User GetTeacher(string teacherCode)
        {
            teacherCode = GetActualName(teacherCode);
            return teacherDA.GetTeacher(teacherCode);
        }

        public aspnet_User GetTeacher(Guid teacherId)
        {
            return teacherDA.GetTeacher(teacherId);
        }

        public List<TabularTeacher> GetTabularTeachers(string teacherCode, string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<aspnet_User> lTeachers = new List<aspnet_User>();
            List<TabularTeacher> lTabularTeachers = new List<TabularTeacher>();

            if ((teacherCode == "") || (string.Compare(teacherCode, "tất cả", true) == 0))
            {
                if ((teacherName == "") || (string.Compare(teacherName, "tất cả", true) == 0))
                {
                    lTeachers = teacherDA.GetListTeachers(pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    lTeachers = teacherDA.GetListTeachersByName(teacherName, pageCurrentIndex, pageSize, out totalRecords);
                }
            }
            else
            {
                teacherCode = GetActualName(teacherCode);

                if ((teacherName == "") || (string.Compare(teacherName, "tất cả", true) == 0))
                {
                    lTeachers = teacherDA.GetListTeachersByCode(teacherCode, pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    
                    lTeachers = teacherDA.GetListTeachers(teacherCode, teacherName, pageCurrentIndex, pageSize, out totalRecords);
                }
            }

            foreach (aspnet_User teacher in lTeachers)
            {
                TabularTeacher tbTeacher = new TabularTeacher();
                tbTeacher.MaGiaoVien = teacher.UserId;
                tbTeacher.MaHienThiGiaoVien = teacher.UserName.Split('_')[1];
                tbTeacher.HoTen = teacher.aspnet_Membership.RealName;
                tbTeacher.NgaySinh = teacher.aspnet_Membership.Birthday;
                tbTeacher.GioiTinh = teacher.aspnet_Membership.Gender;

                lTabularTeachers.Add(tbTeacher);
            }
            return lTabularTeachers;
        }

        public List<TabularTeacher> GetTabularUnformeredTeachers(CauHinh_NamHoc year, string teacherCode, string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<aspnet_User> lTeachers = new List<aspnet_User>();
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
                teacherCode = GetActualName(teacherCode);

                if ((teacherName == "") || (string.Compare(teacherName, "tất cả", true) == 0))
                {
                    lTeachers = teacherDA.GetListUnformedTeachersByCode(year, teacherCode, pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    lTeachers = teacherDA.GetListUnformedTeachers(year, teacherCode, teacherName, pageCurrentIndex, pageSize, out totalRecords);
                }
            }

            foreach (aspnet_User teacher in lTeachers)
            {
                TabularTeacher tbTeacher = new TabularTeacher();
                tbTeacher.MaGiaoVien = teacher.UserId;
                tbTeacher.MaHienThiGiaoVien = teacher.UserName.Split('_')[1];
                tbTeacher.HoTen = teacher.aspnet_Membership.RealName;
                tbTeacher.NgaySinh = teacher.aspnet_Membership.Birthday;
                tbTeacher.GioiTinh = teacher.aspnet_Membership.Gender;

                lTabularTeachers.Add(tbTeacher);
            }
            return lTabularTeachers;
        }

        public bool TeacherCodeExists(string teacherCode)
        {
            return teacherDA.TeacherCodeExists(teacherCode);
        }

        public bool IsDeletable(Guid teacherId)
        {
            ScheduleBL scheduleBL = new ScheduleBL(school);
            FormerTeacherBL formerTeacherBL = new FormerTeacherBL(school);
            bool bDeletable = false;

            aspnet_User teacher = GetTeacher(teacherId);

            if (!scheduleBL.ScheduleExists(teacher))
            {
                if (!formerTeacherBL.FormerTeacherExists(teacher))
                {
                    bDeletable = true;
                }
            }

            return bDeletable;
        }

        public bool IsTeaching(aspnet_User teacher, CauHinh_HocKy term, CauHinh_Thu dayInWeek, DanhMuc_Tiet teachingPeriod)
        {
            return teacherDA.IsTeaching(teacher, term, dayInWeek, teachingPeriod);
        }

        public List<TabularFormering> GetListFormerings(aspnet_User teacher, int pageCurrentIndex, int pageSize, out double totalRecords)
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

        public List<TabularTeaching> GetListTeachings(aspnet_User teacher, int pageCurrentIndex, int pageSize, out double totalRecords)
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
