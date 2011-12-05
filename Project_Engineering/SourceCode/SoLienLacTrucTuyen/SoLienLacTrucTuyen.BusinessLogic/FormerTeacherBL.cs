using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class FormerTeacherBL: BaseBL
    {
        private FormerTeacherDA formerTeacherDA;

        public FormerTeacherBL(School_School school)
            : base(school)
        {
            formerTeacherDA = new FormerTeacherDA(school);
        }

        public void Insert(Class_Class Class, aspnet_User teacher)
        {
            formerTeacherDA.InsertFormerTeacher(Class, teacher);
        }

        public void Update(int formerTeacherId, aspnet_User teacher)
        {
            formerTeacherDA.UpdateFormerTeacher(formerTeacherId, teacher);
        }

        public void Delete(Class_FormerTeacher frmrTeacher)
        {
            formerTeacherDA.DeleteFormerTeacher(frmrTeacher);
        }

        public Class_FormerTeacher GetFormerTeacher(int formerTeacherId)
        {
            return formerTeacherDA.GetFormerTeacher(formerTeacherId);
        }

        public Class_FormerTeacher GetFormerTeacher(Class_Class Class)
        {
            return formerTeacherDA.GetFormerTeacher(Class);
        }

        private List<Class_FormerTeacher> GetListFormerTeachers(Configuration_Year year, Category_Faculty faculty, Category_Grade grade, Class_Class Class, string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<Class_FormerTeacher> lFormerTeachers = new List<Class_FormerTeacher>();
            if (Class != null)
            {
                if ((teacherName == "") || (string.Compare(teacherName, "tất cả", true) == 0))
                {
                    lFormerTeachers = formerTeacherDA.GetFormerTeachers(Class, pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    lFormerTeachers = formerTeacherDA.GetFormerTeachers(Class, teacherName, pageCurrentIndex, pageSize, out totalRecords);
                }
            }
            else
            {
                if (faculty == null)
                {
                    if (grade == null) // FacultyId == 0 + GradeId == 0
                    {
                        if ((teacherName == "") || (string.Compare(teacherName, "tất cả", true) == 0))
                        {
                            lFormerTeachers = formerTeacherDA.GetFormerTeachers(year, pageCurrentIndex, pageSize, out totalRecords);
                        }
                        else
                        {
                            lFormerTeachers = formerTeacherDA.GetFormerTeachers(year, teacherName, pageCurrentIndex, pageSize, out totalRecords);
                        }
                    }
                    else //FacultyId == 0 + GradeId != 0
                    {
                        if ((teacherName == "") || (string.Compare(teacherName, "tất cả", true) == 0))
                        {
                            lFormerTeachers = formerTeacherDA.GetFormerTeachers(year, grade, pageCurrentIndex, pageSize, out totalRecords);
                        }
                        else
                        {
                            lFormerTeachers = formerTeacherDA.GetFormerTeachers(year, grade, teacherName, pageCurrentIndex, pageSize, out totalRecords);
                        }
                    }
                }
                else //FacultyId != 0
                {
                    if (grade == null) //FacultyId != 0 + GradeId = 0
                    {
                        if ((teacherName == "") || (string.Compare(teacherName, "tất cả", true) == 0))
                        {
                            lFormerTeachers = formerTeacherDA.GetFormerTeachers(year, faculty, pageCurrentIndex, pageSize, out totalRecords);
                        }
                        else
                        {
                            lFormerTeachers = formerTeacherDA.GetFormerTeachers(year, faculty, teacherName, pageCurrentIndex, pageSize, out totalRecords);
                        }

                    }
                    else //FacultyId != 0 + GradeId != 0
                    {
                        if ((teacherName == "") || (string.Compare(teacherName, "tất cả", true) == 0))
                        {
                            lFormerTeachers = formerTeacherDA.GetFormerTeachers(year, faculty, grade, pageCurrentIndex, pageSize, out totalRecords);
                        }
                        else
                        {
                            lFormerTeachers = formerTeacherDA.GetFormerTeachers(year, faculty, grade, teacherName, pageCurrentIndex, pageSize, out totalRecords);
                        }

                    }
                }
            }

            return lFormerTeachers;
        }

        private List<Class_FormerTeacher> GetListFormerTeachersByCode(Configuration_Year year, Category_Faculty faculty, Category_Grade grade, Class_Class Class, string teacherCode, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<Class_FormerTeacher> lFormerTeachers = new List<Class_FormerTeacher>();
            if (Class != null)
            {
                if ((teacherCode == "") || (string.Compare(teacherCode, "tất cả", true) == 0))
                {
                    lFormerTeachers = formerTeacherDA.GetFormerTeachers(Class, pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    lFormerTeachers = formerTeacherDA.GetFormerTeachersByCode(Class, teacherCode, pageCurrentIndex, pageSize, out totalRecords);
                }
            }
            else
            {
                if (faculty == null)
                {
                    if (grade == null) // FacultyId == 0 + GradeId == 0
                    {
                        if ((teacherCode == "") || (string.Compare(teacherCode, "tất cả", true) == 0))
                        {
                            lFormerTeachers = formerTeacherDA.GetFormerTeachers(year, pageCurrentIndex, pageSize, out totalRecords);
                        }
                        else
                        {
                            lFormerTeachers = formerTeacherDA.GetFormerTeachersByCode(year, teacherCode, pageCurrentIndex, pageSize, out totalRecords);
                        }
                    }
                    else //FacultyId == 0 + GradeId != 0
                    {
                        if ((teacherCode == "") || (string.Compare(teacherCode, "tất cả", true) == 0))
                        {
                            lFormerTeachers = formerTeacherDA.GetFormerTeachers(year, grade, pageCurrentIndex, pageSize, out totalRecords);
                        }
                        else
                        {
                            lFormerTeachers = formerTeacherDA.GetFormerTeachersByCode(year, grade, teacherCode, pageCurrentIndex, pageSize, out totalRecords);
                        }
                    }
                }
                else //FacultyId != 0
                {
                    if (grade == null) //FacultyId != 0 + GradeId = 0
                    {
                        if ((teacherCode == "") || (string.Compare(teacherCode, "tất cả", true) == 0))
                        {
                            lFormerTeachers = formerTeacherDA.GetFormerTeachers(year, faculty, pageCurrentIndex, pageSize, out totalRecords);
                        }
                        else
                        {
                            lFormerTeachers = formerTeacherDA.GetFormerTeachersByCode(year, faculty, teacherCode, pageCurrentIndex, pageSize, out totalRecords);
                        }

                    }
                    else //FacultyId != 0 + GradeId != 0
                    {
                        if ((teacherCode == "") || (string.Compare(teacherCode, "tất cả", true) == 0))
                        {
                            lFormerTeachers = formerTeacherDA.GetFormerTeachers(year, faculty, grade, pageCurrentIndex, pageSize, out totalRecords);
                        }
                        else
                        {
                            lFormerTeachers = formerTeacherDA.GetFormerTeachersByCode(year, faculty, grade, teacherCode, pageCurrentIndex, pageSize, out totalRecords);
                        }

                    }
                }
            }

            return lFormerTeachers;
        }

        public List<TabularFormerTeacher> GetListFormerTeachers(Configuration_Year year, Category_Faculty faculty, Category_Grade grade, Class_Class Class, string teacherCode, string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularFormerTeacher> lTbFormerTeachers = new List<TabularFormerTeacher>();
            List<Class_FormerTeacher> lFormerTeachers = new List<Class_FormerTeacher>();
            TeacherBL teacherBL = new TeacherBL(school);
            aspnet_User teacher = null;

            if ((teacherCode != "") && (string.Compare(teacherCode, "tất cả", true) != 0))
            {
                teacher = teacherBL.GetTeacher(teacherCode);
                if (teacher == null)  // teacherCode does not exist
                {
                    totalRecords = 0;
                    return lTbFormerTeachers;
                }
                else
                {
                    if (teacher.aspnet_Membership.FullName != teacherName && (teacherName != "") 
                        && (string.Compare(teacherName, "tất cả", true) != 0))
                    {
                        totalRecords = 0;
                        return lTbFormerTeachers;
                    }
                }
            }

            if ((teacherCode == "") || (string.Compare(teacherCode, "tất cả", true) == 0))
            {
                lFormerTeachers = GetListFormerTeachers(year, faculty, grade, Class, teacherName, pageCurrentIndex, pageSize, out totalRecords);
            }
            else
            {
                teacherCode = GetActualName(teacherCode);
                lFormerTeachers = GetListFormerTeachersByCode(year, faculty, grade, Class, teacherCode, pageCurrentIndex, pageSize, out totalRecords);
            }
            

            foreach (Class_FormerTeacher formerTeacher in lFormerTeachers)
            {
                lTbFormerTeachers.Add(new TabularFormerTeacher
                {
                    MaGVCN = formerTeacher.FormerTeacherId,
                    UserId = formerTeacher.TeacherId,
                    TenGiaoVien = formerTeacher.aspnet_User.aspnet_Membership.FullName,
                    ClassId = formerTeacher.ClassId,
                    ClassName = formerTeacher.Class_Class.ClassName
                });
            }

            return lTbFormerTeachers;
        }

        public bool FormerTeacherExists(aspnet_User teacher)
        {
            return formerTeacherDA.FormerTeacherExists(teacher);
        }
    }
}
