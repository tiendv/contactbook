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

        //public void InsertTeacher(aspnet_User teacher)
        //{
        //    db.aspnet_Users.InsertOnSubmit(teacher);
        //    db.SubmitChanges();
        //}

        public void UpdateTeacher(aspnet_Membership editedTeacher)
        {
            IQueryable<aspnet_Membership> iqTeacher = from tchr in db.aspnet_Memberships
                                                      where tchr.UserId == editedTeacher.UserId
                                                      & tchr.SchoolId == school.SchoolId
                                                      select tchr;

            if (iqTeacher.Count() != 0)
            {
                aspnet_Membership teacher = iqTeacher.First();
                teacher.RealName = editedTeacher.RealName;
                teacher.Gender = editedTeacher.Gender;
                teacher.Birthday = editedTeacher.Birthday;
                teacher.Photo = editedTeacher.Photo;
                teacher.Address = editedTeacher.Address;
                teacher.Phone = editedTeacher.Phone;

                db.SubmitChanges();
            }
        }

        //public void DeleteTeacher(aspnet_User deletedTeacher)
        //{
        //    IQueryable<aspnet_User> iqTeacher = from tchr in db.aspnet_Users
        //                                             where tchr.MaGiaoVien == deletedTeacher.MaGiaoVien
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

        public List<aspnet_User> GetListTeachers(string teacherCode, string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<aspnet_User> iqTeacher = from tchr in db.aspnet_Users
                                                where tchr.UserName == teacherCode
                                                  && tchr.aspnet_Membership.RealName == teacherName
                                                  && tchr.aspnet_Membership.SchoolId == school.SchoolId
                                                select tchr;

            return GetListTeachers(ref iqTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<aspnet_User> GetListTeachers(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<aspnet_User> iqTeacher = from tchr in db.aspnet_Users
                                                where tchr.aspnet_Membership.IsTeacher == true
                                                && tchr.aspnet_Membership.SchoolId == school.SchoolId
                                                select tchr;

            return GetListTeachers(ref iqTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<aspnet_User> GetListTeachersByCode(string teacherCode, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<aspnet_User> iqTeacher = from tchr in db.aspnet_Users
                                                where tchr.aspnet_Membership.IsTeacher == true
                                                && tchr.UserName == teacherCode
                                                && tchr.aspnet_Membership.SchoolId == school.SchoolId
                                                select tchr;

            return GetListTeachers(ref iqTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<aspnet_User> GetListTeachersByName(string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<aspnet_User> iqTeacher = from tchr in db.aspnet_Users
                                                where tchr.aspnet_Membership.IsTeacher == true
                                                && tchr.aspnet_Membership.RealName == teacherName
                                                && tchr.aspnet_Membership.SchoolId == school.SchoolId
                                                select tchr;

            return GetListTeachers(ref iqTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<aspnet_User> GetListUnformedTeachers(CauHinh_NamHoc year, string teacherCode, string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<aspnet_User> lTeachers = new List<aspnet_User>();
            IQueryable<aspnet_User> iqTeacher = from tchr in db.aspnet_Users
                                                where tchr.aspnet_Membership.IsTeacher == true
                                                && tchr.aspnet_Membership.RealName == teacherName
                                                && tchr.UserName == teacherCode
                                                && tchr.aspnet_Membership.SchoolId == school.SchoolId
                                                select tchr;

            if (iqTeacher.Count() != 0)
            {
                lTeachers = iqTeacher.ToList();

                int i = 0;
                while (i < lTeachers.Count)
                {
                    IQueryable<LopHoc_GVCN> iqFormerTeacher;
                    iqFormerTeacher = from fTchr in db.LopHoc_GVCNs
                                      where fTchr.TeacherId == lTeachers[i].UserId
                                        && fTchr.aspnet_User.UserName == teacherCode
                                        && fTchr.aspnet_User.aspnet_Membership.RealName == teacherName
                                        && fTchr.LopHoc_Lop.MaNamHoc == year.MaNamHoc
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

        public List<aspnet_User> GetListUnformedTeachers(CauHinh_NamHoc year, int pageCurrentIndex, int pageSize, out double totalRecords)
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
                    IQueryable<LopHoc_GVCN> iqFormerTeacher;
                    iqFormerTeacher = from fTchr in db.LopHoc_GVCNs
                                      where fTchr.TeacherId == lTeachers[i].UserId
                                        && fTchr.LopHoc_Lop.MaNamHoc == year.MaNamHoc
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

        public List<aspnet_User> GetListUnformedTeachersByName(CauHinh_NamHoc year, string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<aspnet_User> lTeachers = new List<aspnet_User>();
            IQueryable<aspnet_User> iqTeacher = from tchr in db.aspnet_Users
                                                where tchr.aspnet_Membership.IsTeacher == true
                                                && tchr.aspnet_Membership.RealName == teacherName
                                                && tchr.aspnet_Membership.SchoolId == school.SchoolId
                                                select tchr;

            if (iqTeacher.Count() != 0)
            {
                lTeachers = iqTeacher.ToList();

                int i = 0;
                while (i < lTeachers.Count)
                {
                    IQueryable<LopHoc_GVCN> iqFormerTeacher;
                    iqFormerTeacher = from fTchr in db.LopHoc_GVCNs
                                      where fTchr.TeacherId == lTeachers[i].UserId
                                        && fTchr.aspnet_User.aspnet_Membership.RealName == teacherName
                                        && fTchr.LopHoc_Lop.MaNamHoc == year.MaNamHoc
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

        public List<aspnet_User> GetListUnformedTeachersByCode(CauHinh_NamHoc year, string teacherCode, int pageCurrentIndex, int pageSize, out double totalRecords)
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
                    IQueryable<LopHoc_GVCN> iqFormerTeacher;
                    iqFormerTeacher = from fTchr in db.LopHoc_GVCNs
                                      where fTchr.TeacherId == lTeachers[i].UserId
                                        && fTchr.aspnet_User.UserName == teacherCode
                                        && fTchr.LopHoc_Lop.MaNamHoc == year.MaNamHoc
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

        public bool IsTeaching(aspnet_User teacher, CauHinh_HocKy term, CauHinh_Thu dayInWeek, DanhMuc_Tiet teachingPeriod)
        {
            IQueryable<LopHoc_MonHocTKB> iqThoiKhoaBieu;
            iqThoiKhoaBieu = from tkb in db.LopHoc_MonHocTKBs
                             where tkb.TeacherId == teacher.UserId
                                && tkb.MaHocKy == term.MaHocKy
                                && tkb.MaThu == dayInWeek.MaThu
                                && tkb.MaTiet == teachingPeriod.MaTiet
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

        public List<LopHoc_GVCN> GetFormering(aspnet_User teacher, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<LopHoc_GVCN> lFormering = new List<LopHoc_GVCN>();

            IQueryable<LopHoc_GVCN> iqFormering;
            iqFormering = from formering in db.LopHoc_GVCNs
                          where formering.TeacherId == teacher.UserId
                          && formering.aspnet_User.aspnet_Membership.SchoolId == school.SchoolId
                          select formering;

            totalRecords = iqFormering.Count();
            if (totalRecords != 0)
            {
                lFormering = iqFormering.OrderByDescending(formering => formering.LopHoc_Lop.MaNamHoc)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return lFormering;
        }

        public List<LopHoc_MonHocTKB> GetTeaching(aspnet_User teacher, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<LopHoc_MonHocTKB> lShedules = new List<LopHoc_MonHocTKB>();

            IQueryable<LopHoc_MonHocTKB> iqSchedule;
            iqSchedule = from schedule in db.LopHoc_MonHocTKBs
                         where schedule.TeacherId == teacher.UserId
                         && schedule.aspnet_User.aspnet_Membership.SchoolId == school.SchoolId
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
