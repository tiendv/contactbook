using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.BusinessEntity;

namespace EContactBook.DataAccess
{
    public class TeacherDA : BaseDA
    {
        public TeacherDA(School_School school)
            : base(school)
        { }

        //public void InsertTeacher(aspnet_User teacher)
        //{
        //    db.aspnet_Users.InsertOnSubmit(teacher);
        //    db.SubmitChanges();
        //}

        public void UpdateTeacher(aspnet_Membership teacher, string newTeacherName, bool newGender, DateTime newBirthday, string newAddress, string newPhone, byte[] photo)
        {
            IQueryable<aspnet_Membership> queryTeacher = from t in db.aspnet_Memberships
                                                         where t.UserId == teacher.UserId
                                                         & t.SchoolId == school.SchoolId
                                                         select t;

            if (queryTeacher.Count() != 0)
            {
                teacher = queryTeacher.First();
                teacher.FullName = newTeacherName;
                teacher.Gender = newGender;
                teacher.Birthday = newBirthday;
                teacher.Photo = new System.Data.Linq.Binary(photo);
                teacher.Address = newAddress;
                teacher.Phone = newPhone;

                db.SubmitChanges();
            }
        }

        //public void DeleteTeacher(aspnet_User deletedTeacher)
        //{
        //    IQueryable<aspnet_User> iqTeacher = from tchr in db.aspnet_Users
        //                                             where tchr.UserId == deletedTeacher.UserId
        //                                             && tchr.SchoolId == school.SchoolId
        //                                             select tchr;

        //    if (iqTeacher.Count() != 0)
        //    {
        //        aspnet_User teacher = iqTeacher.First();
        //        db.aspnet_Users.DeleteOnSubmit(teacher);
        //        db.SubmitChanges();
        //    }
        //}

        public aspnet_User GetTeacher(string teacherCode)
        {
            aspnet_User teacher = null;

            IQueryable<aspnet_User> iqTeacher = from tchr in db.aspnet_Users
                                                where tchr.UserName == teacherCode
                                                && tchr.aspnet_Membership.SchoolId == school.SchoolId
                                                select tchr;

            if (iqTeacher.Count() != 0)
            {
                teacher = iqTeacher.First();
            }

            return teacher;
        }

        public aspnet_User GetTeacher(Guid teacherId)
        {
            aspnet_User teacher = null;

            IQueryable<aspnet_User> iqTeacher = from tchr in db.aspnet_Users
                                                where tchr.UserId == teacherId
                                                && tchr.aspnet_Membership.SchoolId == school.SchoolId
                                                select tchr;

            if (iqTeacher.Count() != 0)
            {
                teacher = iqTeacher.First();
            }

            return teacher;
        }

        private List<aspnet_User> GetListTeachers(ref IQueryable<aspnet_User> iqTeacher, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<aspnet_User> lTeachers = new List<aspnet_User>();

            totalRecords = iqTeacher.Count();
            if (totalRecords != 0)
            {
                lTeachers = iqTeacher.OrderBy(giaoVien => giaoVien.UserName)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return lTeachers;
        }

        public List<aspnet_User> GetTeachers(string teacherCode, string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<aspnet_User> iqTeacher = from tchr in db.aspnet_Users
                                                where tchr.UserName == teacherCode
                                                  && tchr.aspnet_Membership.FullName == teacherName
                                                  && tchr.aspnet_Membership.SchoolId == school.SchoolId
                                                select tchr;

            return GetListTeachers(ref iqTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<aspnet_User> GetTeachers(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<aspnet_User> iqTeacher = from tchr in db.aspnet_Users
                                                where tchr.aspnet_Membership.IsTeacher == true
                                                && tchr.aspnet_Membership.SchoolId == school.SchoolId
                                                select tchr;

            return GetListTeachers(ref iqTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<aspnet_User> GetTeachersByCode(string teacherCode, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<aspnet_User> iqTeacher = from tchr in db.aspnet_Users
                                                where tchr.aspnet_Membership.IsTeacher == true
                                                && tchr.UserName == teacherCode
                                                && tchr.aspnet_Membership.SchoolId == school.SchoolId
                                                select tchr;

            return GetListTeachers(ref iqTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<aspnet_User> GetTeachersByName(string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<aspnet_User> iqTeacher = from tchr in db.aspnet_Users
                                                where tchr.aspnet_Membership.IsTeacher == true
                                                && tchr.aspnet_Membership.FullName == teacherName
                                                && tchr.aspnet_Membership.SchoolId == school.SchoolId
                                                select tchr;

            return GetListTeachers(ref iqTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<aspnet_User> GetUnformedTeachers(Configuration_Year year, string teacherCode, string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<aspnet_User> lTeachers = new List<aspnet_User>();
            IQueryable<aspnet_User> iqTeacher = from tchr in db.aspnet_Users
                                                where tchr.aspnet_Membership.IsTeacher == true
                                                && tchr.aspnet_Membership.FullName == teacherName
                                                && tchr.UserName == teacherCode
                                                && tchr.aspnet_Membership.SchoolId == school.SchoolId
                                                select tchr;

            if (iqTeacher.Count() != 0)
            {
                lTeachers = iqTeacher.ToList();

                int i = 0;
                while (i < lTeachers.Count)
                {
                    IQueryable<Class_FormerTeacher> iqFormerTeacher;
                    iqFormerTeacher = from fTchr in db.Class_FormerTeachers
                                      where fTchr.TeacherId == lTeachers[i].UserId
                                        && fTchr.aspnet_User.UserName == teacherCode
                                        && fTchr.aspnet_User.aspnet_Membership.FullName == teacherName
                                        && fTchr.Class_Class.YearId == year.YearId
                                        && fTchr.aspnet_User.aspnet_Membership.SchoolId == school.SchoolId
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

        public List<aspnet_User> GetUnformedTeachers(Configuration_Year year, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<aspnet_User> lTeachers = new List<aspnet_User>();
            IQueryable<aspnet_User> iqTeacher = from tchr in db.aspnet_Users
                                                where tchr.aspnet_Membership.IsTeacher == true
                                                && tchr.aspnet_Membership.SchoolId == school.SchoolId
                                                select tchr;

            if (iqTeacher.Count() != 0)
            {
                lTeachers = iqTeacher.ToList();

                int i = 0;
                while (i < lTeachers.Count)
                {
                    IQueryable<Class_FormerTeacher> iqFormerTeacher;
                    iqFormerTeacher = from fTchr in db.Class_FormerTeachers
                                      where fTchr.TeacherId == lTeachers[i].UserId
                                        && fTchr.Class_Class.YearId == year.YearId
                                        && fTchr.aspnet_User.aspnet_Membership.SchoolId == school.SchoolId
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

        public List<aspnet_User> GetUnformedTeachersByName(Configuration_Year year, string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<aspnet_User> lTeachers = new List<aspnet_User>();
            IQueryable<aspnet_User> iqTeacher = from tchr in db.aspnet_Users
                                                where tchr.aspnet_Membership.IsTeacher == true
                                                && tchr.aspnet_Membership.FullName == teacherName
                                                && tchr.aspnet_Membership.SchoolId == school.SchoolId
                                                select tchr;

            if (iqTeacher.Count() != 0)
            {
                lTeachers = iqTeacher.ToList();

                int i = 0;
                while (i < lTeachers.Count)
                {
                    IQueryable<Class_FormerTeacher> iqFormerTeacher;
                    iqFormerTeacher = from fTchr in db.Class_FormerTeachers
                                      where fTchr.TeacherId == lTeachers[i].UserId
                                        && fTchr.aspnet_User.aspnet_Membership.FullName == teacherName
                                        && fTchr.Class_Class.YearId == year.YearId
                                        && fTchr.aspnet_User.aspnet_Membership.SchoolId == school.SchoolId
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

        public List<aspnet_User> GetUnformedTeachersByCode(Configuration_Year year, string teacherCode, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<aspnet_User> lTeachers = new List<aspnet_User>();
            IQueryable<aspnet_User> iqTeacher = from tchr in db.aspnet_Users
                                                where tchr.aspnet_Membership.IsTeacher == true
                                                && tchr.UserName == teacherCode
                                                && tchr.aspnet_Membership.SchoolId == school.SchoolId
                                                select tchr;

            if (iqTeacher.Count() != 0)
            {
                lTeachers = iqTeacher.ToList();

                int i = 0;
                while (i < lTeachers.Count)
                {
                    IQueryable<Class_FormerTeacher> iqFormerTeacher;
                    iqFormerTeacher = from fTchr in db.Class_FormerTeachers
                                      where fTchr.TeacherId == lTeachers[i].UserId
                                        && fTchr.aspnet_User.UserName == teacherCode
                                        && fTchr.Class_Class.YearId == year.YearId
                                        && fTchr.aspnet_User.aspnet_Membership.SchoolId == school.SchoolId
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
            IQueryable<aspnet_User> giaoViens;
            giaoViens = from giaoVien in db.aspnet_Users
                        where giaoVien.UserName == teacherCode
                        && giaoVien.aspnet_Membership.SchoolId == school.SchoolId
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

        public bool IsTeaching(aspnet_User teacher, Configuration_Term term, Configuration_DayInWeek dayInWeek, Category_TeachingPeriod teachingPeriod)
        {
            IQueryable<Class_Schedule> iqThoiKhoaBieu;
            iqThoiKhoaBieu = from tkb in db.Class_Schedules
                             where tkb.TeacherId == teacher.UserId
                                && tkb.TermId == term.TermId
                                && tkb.DayInWeekId == dayInWeek.DayInWeekId
                                && tkb.SessionId == teachingPeriod.SessionId
                                && tkb.aspnet_User.aspnet_Membership.SchoolId == school.SchoolId
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

        public List<Class_FormerTeacher> GetFormering(aspnet_User teacher, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<Class_FormerTeacher> lFormering = new List<Class_FormerTeacher>();

            IQueryable<Class_FormerTeacher> iqFormering;
            iqFormering = from formering in db.Class_FormerTeachers
                          where formering.TeacherId == teacher.UserId
                          && formering.aspnet_User.aspnet_Membership.SchoolId == school.SchoolId
                          select formering;

            totalRecords = iqFormering.Count();
            if (totalRecords != 0)
            {
                lFormering = iqFormering.OrderByDescending(formering => formering.Class_Class.YearId)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return lFormering;
        }

        public List<Class_Schedule> GetTeaching(aspnet_User teacher, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<Class_Schedule> shedules = new List<Class_Schedule>();

            IQueryable<Class_Schedule> querySchedule;
            querySchedule = from schedule in db.Class_Schedules
                            where schedule.TeacherId == teacher.UserId
                            && schedule.aspnet_User.aspnet_Membership.SchoolId == school.SchoolId
                            select schedule;

            totalRecords = querySchedule.Count();
            if (totalRecords != 0)
            {
                shedules = querySchedule.OrderByDescending(t => t.Class_Class.YearId).ThenByDescending(t => t.TermId)
                    .ThenBy(t => t.DayInWeekId).ThenBy(t => t.TeachingPeriodId)
                    .ThenBy(t => t.Class_Class.ClassName).ThenBy(t => t.Category_Subject.SubjectName)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).Distinct().ToList();
            }

            return shedules;
        }

        public bool IsTeaching(aspnet_User teacher)
        {
            IQueryable<Class_Schedule> querySchedule;
            querySchedule = from schedule in db.Class_Schedules
                            where schedule.TeacherId == teacher.UserId
                            && schedule.aspnet_User.aspnet_Membership.SchoolId == school.SchoolId
                            select schedule;

            if (querySchedule.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<Class_Class> GetTaughtClasses(aspnet_User teacher, Configuration_Year year, Configuration_Term term)
        {
            List<Class_Class> Classes = new List<Class_Class>();

            IQueryable<Class_Class> queryClass;
            queryClass = from schedule in db.Class_Schedules
                         where schedule.TeacherId == teacher.UserId
                         && schedule.Class_Class.YearId == year.YearId && schedule.TermId == term.TermId
                         select schedule.Class_Class;


            if (queryClass.Count() != 0)
            {
                Classes = queryClass.OrderByDescending(Class => Class.ClassName).Distinct().ToList();
            }

            return Classes;
        }

        public List<Class_Class> GetTaughtClasses(aspnet_User teacher, Configuration_Year year, Configuration_Term term, Category_Faculty faculty)
        {
            List<Class_Class> Classes = new List<Class_Class>();

            IQueryable<Class_Class> queryClass;
            queryClass = from schedule in db.Class_Schedules
                         where schedule.TeacherId == teacher.UserId
                         && schedule.Class_Class.YearId == year.YearId && schedule.TermId == term.TermId
                         && schedule.Class_Class.FacultyId == faculty.FacultyId
                         select schedule.Class_Class;

            if (queryClass.Count() != 0)
            {
                Classes = queryClass.OrderByDescending(Class => Class.ClassName).Distinct().ToList();
            }

            return Classes;
        }

        public List<Class_Class> GetTaughtClasses(aspnet_User teacher, Configuration_Year year, Configuration_Term term, Category_Grade grade)
        {
            List<Class_Class> Classes = new List<Class_Class>();

            IQueryable<Class_Class> queryClass;
            queryClass = from schedule in db.Class_Schedules
                         where schedule.TeacherId == teacher.UserId
                         && schedule.Class_Class.YearId == year.YearId && schedule.TermId == term.TermId
                         && schedule.Class_Class.GradeId == grade.GradeId
                         select schedule.Class_Class;

            if (queryClass.Count() != 0)
            {
                Classes = queryClass.OrderByDescending(Class => Class.ClassName).Distinct().ToList();
            }

            return Classes;
        }

        public List<Class_Class> GetTaughtClasses(aspnet_User teacher, Configuration_Year year, Configuration_Term term, Category_Faculty faculty, Category_Grade grade)
        {
            List<Class_Class> Classes = new List<Class_Class>();

            IQueryable<Class_Class> queryClass;
            queryClass = from schedule in db.Class_Schedules
                         where schedule.TeacherId == teacher.UserId
                         && schedule.Class_Class.YearId == year.YearId && schedule.TermId == term.TermId
                         && schedule.Class_Class.FacultyId == faculty.FacultyId && schedule.Class_Class.GradeId == grade.GradeId
                         select schedule.Class_Class;

            if (queryClass.Count() != 0)
            {
                Classes = queryClass.OrderByDescending(Class => Class.ClassName).Distinct().ToList();
            }

            return Classes;
        }
    }
}
