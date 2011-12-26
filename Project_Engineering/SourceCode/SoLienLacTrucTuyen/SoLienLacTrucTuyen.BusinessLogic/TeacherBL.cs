using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class TeacherBL : BaseBL
    {
        private TeacherDA teacherDA;

        public TeacherBL(School_School school)
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
            editedTeacher.FullName = newTeacherName;
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
                tbTeacher.UserId = teacher.UserId;
                tbTeacher.MaHienThiGiaoVien = teacher.UserName.Split('_')[1];
                tbTeacher.HoTen = teacher.aspnet_Membership.FullName;
                tbTeacher.NgaySinh = teacher.aspnet_Membership.Birthday;
                tbTeacher.GioiTinh = teacher.aspnet_Membership.Gender;

                lTabularTeachers.Add(tbTeacher);
            }
            return lTabularTeachers;
        }

        public List<TabularTeacher> GetTabularUnformeredTeachers(Configuration_Year year, string teacherCode, string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
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
                tbTeacher.UserId = teacher.UserId;
                tbTeacher.MaHienThiGiaoVien = teacher.UserName.Split('_')[1];
                tbTeacher.HoTen = teacher.aspnet_Membership.FullName;
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

        public bool IsTeaching(aspnet_User teacher, Configuration_Term term, Configuration_DayInWeek dayInWeek, Category_TeachingPeriod teachingPeriod)
        {
            return teacherDA.IsTeaching(teacher, term, dayInWeek, teachingPeriod);
        }

        public List<TabularFormering> GetListFormerings(aspnet_User teacher, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularFormering> lTbFormering = new List<TabularFormering>();
            List<Class_FormerTeacher> lFormering = teacherDA.GetFormering(teacher, pageCurrentIndex, pageSize, out totalRecords);

            foreach (Class_FormerTeacher formering in lFormering)
            {
                TabularFormering tbFormering = new TabularFormering
                              {
                                  YearId = formering.Class_Class.YearId,
                                  YearName = formering.Class_Class.Configuration_Year.YearName,
                                  ClassId = formering.Class_Class.ClassId,
                                  ClassName = formering.Class_Class.ClassName
                              };
                lTbFormering.Add(tbFormering);
            }

            return lTbFormering;
        }

        public List<TabularTeaching> GetListTeachings(aspnet_User teacher, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularTeaching> lTeachings = new List<TabularTeaching>();
            List<Class_Schedule> lShedules = new List<Class_Schedule>();

            lShedules = teacherDA.GetTeaching(teacher, pageCurrentIndex, pageSize, out totalRecords);

            foreach (Class_Schedule schedule in lShedules)
            {
                TabularTeaching tbTeaching = new TabularTeaching
                {
                    YearId = schedule.Class_Class.Configuration_Year.YearId,
                    YearName = schedule.Class_Class.Configuration_Year.YearName,
                    TermId = schedule.Configuration_Term.TermId,
                    TermName = schedule.Configuration_Term.TermName,
                    ClassId = schedule.Class_Class.ClassId,
                    ClassName = schedule.Class_Class.ClassName,
                    SubjectId = schedule.Category_Subject.SubjectId,
                    SubjectName = schedule.Category_Subject.SubjectName
                };

                lTeachings.Add(tbTeaching);
            }

            return lTeachings;
        }

    }
}
