using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace EContactBook.DataAccess
{
    public class FormerTeacherDA : BaseDA
    {
        public FormerTeacherDA(School_School school)
            : base(school)
        {

        }

        public void InsertFormerTeacher(Class_Class Class, aspnet_User teacher)
        {
            Class_FormerTeacher formerTeacher = new Class_FormerTeacher
            {
                ClassId = Class.ClassId,
                TeacherId = teacher.UserId
            };

            db.Class_FormerTeachers.InsertOnSubmit(formerTeacher);
            db.SubmitChanges();
        }

        public void UpdateFormerTeacher(int formerTeacherId, aspnet_User teacher)
        {
            IQueryable<Class_FormerTeacher> iqFormerTeacher = from fTchr in db.Class_FormerTeachers
                                                      where fTchr.FormerTeacherId == formerTeacherId
                                                      select fTchr;
            if (iqFormerTeacher.Count() != 0)
            {
                Class_FormerTeacher formerTeacher = iqFormerTeacher.First();
                formerTeacher.TeacherId = teacher.UserId;
                db.SubmitChanges();
            }
        }

        public void DeleteFormerTeacher(Class_FormerTeacher frmrTeacher)
        {
            IQueryable<Class_FormerTeacher> iqFormerTeacher = from fTchr in db.Class_FormerTeachers
                                                      where fTchr.FormerTeacherId == frmrTeacher.FormerTeacherId
                                                      select fTchr;
            if (iqFormerTeacher.Count() != 0)
            {
                Class_FormerTeacher formerTeacher = iqFormerTeacher.First();
                db.Class_FormerTeachers.DeleteOnSubmit(formerTeacher);
                db.SubmitChanges();
            }
        }

        private List<Class_FormerTeacher> GetFormerTeachers(ref IQueryable<Class_FormerTeacher> iqFormerTeacher, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<Class_FormerTeacher> lFormerTeacher = new List<Class_FormerTeacher>();
            totalRecords = iqFormerTeacher.Count();
            if (totalRecords != 0)
            {
                lFormerTeacher = iqFormerTeacher.OrderBy(teacher => teacher.aspnet_User.UserName)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return lFormerTeacher;
        }

        public List<Class_FormerTeacher> GetFormerTeachers(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Class_FormerTeacher> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.Class_FormerTeachers
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Class_FormerTeacher> GetFormerTeachers(Class_Class Class, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Class_FormerTeacher> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.Class_FormerTeachers
                             where formerTeacher.Class_Class.ClassId == Class.ClassId
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Class_FormerTeacher> GetFormerTeachersByCode(Class_Class Class, string teacherCode, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Class_FormerTeacher> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.Class_FormerTeachers
                             where formerTeacher.Class_Class.ClassId == Class.ClassId
                               && formerTeacher.aspnet_User.UserName == teacherCode
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Class_FormerTeacher> GetFormerTeachers(Class_Class Class, string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Class_FormerTeacher> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.Class_FormerTeachers
                             where formerTeacher.Class_Class.ClassId == Class.ClassId
                               && formerTeacher.aspnet_User.aspnet_Membership.FullName == teacherName
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Class_FormerTeacher> GetFormerTeachers(Configuration_Year year, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Class_FormerTeacher> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.Class_FormerTeachers
                             where formerTeacher.Class_Class.YearId == year.YearId
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Class_FormerTeacher> GetFormerTeachers(Configuration_Year year, string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Class_FormerTeacher> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.Class_FormerTeachers
                             where formerTeacher.Class_Class.YearId == year.YearId
                             && formerTeacher.aspnet_User.aspnet_Membership.FullName == teacherName
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Class_FormerTeacher> GetFormerTeachersByCode(Configuration_Year year, string teacherCode, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Class_FormerTeacher> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.Class_FormerTeachers
                             where formerTeacher.Class_Class.YearId == year.YearId
                             && formerTeacher.aspnet_User.UserName == teacherCode
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Class_FormerTeacher> GetFormerTeachers(Configuration_Year year, Category_Faculty faculty, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Class_FormerTeacher> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.Class_FormerTeachers
                             where formerTeacher.Class_Class.YearId == year.YearId
                             && formerTeacher.Class_Class.FacultyId == faculty.FacultyId
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Class_FormerTeacher> GetFormerTeachers(Configuration_Year year, Category_Faculty faculty, string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Class_FormerTeacher> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.Class_FormerTeachers
                             where formerTeacher.Class_Class.YearId == year.YearId
                             && formerTeacher.Class_Class.FacultyId == faculty.FacultyId
                             && formerTeacher.aspnet_User.aspnet_Membership.FullName == teacherName
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Class_FormerTeacher> GetFormerTeachersByCode(Configuration_Year year, Category_Faculty faculty, string teacherCode, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Class_FormerTeacher> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.Class_FormerTeachers
                             where formerTeacher.Class_Class.YearId == year.YearId
                             && formerTeacher.Class_Class.FacultyId == faculty.FacultyId
                             && formerTeacher.aspnet_User.UserName == teacherCode
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Class_FormerTeacher> GetFormerTeachers(Configuration_Year year, Category_Grade grade, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Class_FormerTeacher> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.Class_FormerTeachers
                             where formerTeacher.Class_Class.YearId == year.YearId
                             && formerTeacher.Class_Class.GradeId == grade.GradeId
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Class_FormerTeacher> GetFormerTeachers(Configuration_Year year, Category_Grade grade, string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Class_FormerTeacher> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.Class_FormerTeachers
                             where formerTeacher.Class_Class.YearId == year.YearId
                             && formerTeacher.Class_Class.GradeId == grade.GradeId
                             && formerTeacher.aspnet_User.aspnet_Membership.FullName == teacherName
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Class_FormerTeacher> GetFormerTeachersByCode(Configuration_Year year, Category_Grade grade, string teacherCode, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Class_FormerTeacher> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.Class_FormerTeachers
                             where formerTeacher.Class_Class.YearId == year.YearId
                             && formerTeacher.Class_Class.GradeId == grade.GradeId
                             && formerTeacher.aspnet_User.UserName == teacherCode
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Class_FormerTeacher> GetFormerTeachers(Configuration_Year year, Category_Faculty faculty, Category_Grade grade, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Class_FormerTeacher> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.Class_FormerTeachers
                             where formerTeacher.Class_Class.YearId == year.YearId
                             && formerTeacher.Class_Class.FacultyId == faculty.FacultyId
                             && formerTeacher.Class_Class.GradeId == grade.GradeId
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Class_FormerTeacher> GetFormerTeachers(Configuration_Year year, Category_Faculty faculty, Category_Grade grade, string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Class_FormerTeacher> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.Class_FormerTeachers
                             where formerTeacher.Class_Class.YearId == year.YearId
                             && formerTeacher.Class_Class.FacultyId == faculty.FacultyId
                             && formerTeacher.Class_Class.GradeId == grade.GradeId
                             && formerTeacher.aspnet_User.aspnet_Membership.FullName == teacherName
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Class_FormerTeacher> GetFormerTeachersByCode(Configuration_Year year, Category_Faculty faculty, Category_Grade grade, string teacherCode, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Class_FormerTeacher> iqFomerTeacher;
            iqFomerTeacher = from formerTeacher in db.Class_FormerTeachers
                             where formerTeacher.Class_Class.YearId == year.YearId
                             && formerTeacher.Class_Class.FacultyId == faculty.FacultyId
                             && formerTeacher.Class_Class.GradeId == grade.GradeId
                             && formerTeacher.aspnet_User.UserName == teacherCode
                             select formerTeacher;

            return GetFormerTeachers(ref iqFomerTeacher, pageCurrentIndex, pageSize, out totalRecords);
        }

        public Class_FormerTeacher GetFormerTeacher(int formerTeacherId)
        {
            Class_FormerTeacher giaoVienChuNhiem = (from gvcn in db.Class_FormerTeachers
                                            where gvcn.FormerTeacherId == formerTeacherId
                                            select gvcn).First();
            return giaoVienChuNhiem;
        }

        public Class_FormerTeacher GetFormerTeacher(Class_Class Class)
        {
            Class_FormerTeacher formerTeacher = null;

            IQueryable<Class_FormerTeacher> iqFormerTeacher = from fTchr in db.Class_FormerTeachers
                                                      where fTchr.ClassId == Class.ClassId
                                                      select fTchr;
            if (iqFormerTeacher.Count() != 0)
            {
                formerTeacher = iqFormerTeacher.First();
            }

            return formerTeacher;
        }

        public bool FormerTeacherExists(aspnet_User teacher)
        {
            IQueryable<Class_FormerTeacher> iqFormerTeacher = from fTchr in db.Class_FormerTeachers
                                                      where fTchr.TeacherId == teacher.UserId
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
