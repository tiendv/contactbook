using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class FormerTeacherBL
    {
        private FormerTeacherDA formerTeacherDA;

        public FormerTeacherBL()
        {
            formerTeacherDA = new FormerTeacherDA();
        }

        public void Insert(LopHoc_Lop Class, LopHoc_GiaoVien teacher)
        {
            formerTeacherDA.InsertFormerTeacher(Class, teacher);
        }

        public void Update(int formerTeacherId, LopHoc_GiaoVien teacher)
        {
            formerTeacherDA.UpdateFormerTeacher(formerTeacherId, teacher);
        }

        public void Delete(LopHoc_GVCN frmrTeacher)
        {
            formerTeacherDA.DeleteFormerTeacher(frmrTeacher);
        }

        public LopHoc_GVCN GetFormerTeacher(int formerTeacherId)
        {
            return formerTeacherDA.GetFormerTeacher(formerTeacherId);
        }

        public LopHoc_GVCN GetFormerTeacher(LopHoc_Lop Class)
        {
            return formerTeacherDA.GetFormerTeacher(Class);
        }

        private List<LopHoc_GVCN> GetListFormerTeachers(CauHinh_NamHoc year, DanhMuc_NganhHoc faculty, DanhMuc_KhoiLop grade, LopHoc_Lop Class,
            string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<LopHoc_GVCN> lFormerTeachers = new List<LopHoc_GVCN>();
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
                    if (grade == null) // maNganhHoc == 0 + maKhoiLop == 0
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
                    else //maNganhHoc == 0 + maKhoiLop != 0
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
                else //maNganhHoc != 0
                {
                    if (grade == null) //maNganhHoc != 0 + maKhoiLop = 0
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
                    else //maNganhHoc != 0 + maKhoiLop != 0
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

        private List<LopHoc_GVCN> GetListFormerTeachersByCode(CauHinh_NamHoc year, DanhMuc_NganhHoc faculty, DanhMuc_KhoiLop grade, LopHoc_Lop Class,
            string teacherCode, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<LopHoc_GVCN> lFormerTeachers = new List<LopHoc_GVCN>();
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
                    if (grade == null) // maNganhHoc == 0 + maKhoiLop == 0
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
                    else //maNganhHoc == 0 + maKhoiLop != 0
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
                else //maNganhHoc != 0
                {
                    if (grade == null) //maNganhHoc != 0 + maKhoiLop = 0
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
                    else //maNganhHoc != 0 + maKhoiLop != 0
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

        public List<TabularFormerTeacher> GetListFormerTeachers(CauHinh_NamHoc year, DanhMuc_NganhHoc faculty, DanhMuc_KhoiLop grade, LopHoc_Lop Class,
            string teacherCode, string teacherName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularFormerTeacher> lTbFormerTeachers = new List<TabularFormerTeacher>();
            List<LopHoc_GVCN> lFormerTeachers = new List<LopHoc_GVCN>();
            TeacherBL teacherBL = new TeacherBL();
            LopHoc_GiaoVien teacher = null;

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
                    if (teacher.HoTen != teacherName && (teacherName != "") && (string.Compare(teacherName, "tất cả", true) != 0))
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
                lFormerTeachers = GetListFormerTeachersByCode(year, faculty, grade, Class, teacherCode, pageCurrentIndex, pageSize, out totalRecords);
            }
            

            foreach (LopHoc_GVCN formerTeacher in lFormerTeachers)
            {
                lTbFormerTeachers.Add(new TabularFormerTeacher
                {
                    MaGVCN = formerTeacher.MaGVCN,
                    MaGiaoVien = formerTeacher.MaGiaoVien,
                    TenGiaoVien = formerTeacher.LopHoc_GiaoVien.HoTen,
                    MaLopHoc = formerTeacher.MaLopHoc,
                    TenLopHoc = formerTeacher.LopHoc_Lop.TenLopHoc
                });
            }

            return lTbFormerTeachers;
        }

        public bool FormerTeacherExists(LopHoc_GiaoVien teacher)
        {
            return formerTeacherDA.FormerTeacherExists(teacher);
        }
    }
}
