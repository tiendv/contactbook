using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class ClassBL: BaseBL
    {
        private ClassDA classDA;

        public ClassBL(School school)
            : base(school)
        {
            classDA = new ClassDA(school);
        }
                
        public void InsertClass(string ClassName, CauHinh_NamHoc year, DanhMuc_NganhHoc faculty, DanhMuc_KhoiLop grade)
        {
            classDA.InsertClass(new LopHoc_Lop()
            {
                TenLopHoc = ClassName,
                MaNganhHoc = faculty.MaNganhHoc,
                MaKhoiLop = grade.MaKhoiLop,
                MaNamHoc = year.MaNamHoc
            });
        }

        public void UpdateClass(LopHoc_Lop editedClass, string newClassName)
        {
            editedClass.TenLopHoc = newClassName;
            classDA.UpdateClass(editedClass);
        }

        public void IncreaseStudentAmount(LopHoc_Lop Class)
        {
            classDA.IncreaseStudentAmount(Class);
        }

        public void DeleteClass(LopHoc_Lop deletedClass)
        {
            classDA.DeleteClass(deletedClass);
        }

        public LopHoc_Lop GetClass(int classId)
        {
            return classDA.GetClass(classId);
        }

        public TabularClass GetTabularClass(LopHoc_Lop Class)
        {
            Class = GetClass(Class.MaLopHoc);
            TabularClass tabularClass = new TabularClass();
            FormerTeacherBL formerTeacherBL = new FormerTeacherBL(school);
            LopHoc_GVCN formerTeacher = null;

            tabularClass.MaLopHoc = Class.MaLopHoc;
            tabularClass.TenLopHoc = Class.TenLopHoc;
            tabularClass.TenNganhHoc = Class.DanhMuc_NganhHoc.TenNganhHoc;
            tabularClass.MaNganhHoc = Class.MaNganhHoc;
            tabularClass.MaKhoiLop = Class.MaKhoiLop;
            tabularClass.TenKhoiLop = Class.DanhMuc_KhoiLop.TenKhoiLop;
            tabularClass.SiSo = Class.SiSo;
            tabularClass.MaNamHoc = Class.MaNamHoc;
            tabularClass.TenNamHoc = Class.CauHinh_NamHoc.TenNamHoc;
            formerTeacher = formerTeacherBL.GetFormerTeacher(Class);
            if (formerTeacher != null)
            {
                tabularClass.HomeroomTeacherCode = formerTeacher.aspnet_User.UserId;
                tabularClass.TenGVCN = formerTeacher.aspnet_User.aspnet_Membership.RealName;
            }

            return tabularClass;
        }

        public List<LopHoc_Lop> GetListClasses(CauHinh_NamHoc year, DanhMuc_NganhHoc faculty, DanhMuc_KhoiLop grade)
        {
            List<LopHoc_Lop> lClasses = new List<LopHoc_Lop>();
            if (faculty == null)
            {
                if (grade == null)
                {
                    lClasses = classDA.GetClasses(year);
                }
                else
                {
                    lClasses = classDA.GetClasses(year, grade);
                }
            }
            else
            {
                if (grade == null)
                {
                    lClasses = classDA.GetClasses(year, faculty);
                }
                else
                {
                    lClasses = classDA.GetClasses(year, faculty, grade);
                }
            }

            return lClasses;
        }

        public List<TabularClass> GetTabularClasses(CauHinh_NamHoc year, DanhMuc_NganhHoc faculty, DanhMuc_KhoiLop grade, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularClass> lTabularClasses = new List<TabularClass>();
            List<LopHoc_Lop> lClasses = new List<LopHoc_Lop>();

            if (faculty == null)
            {
                if (grade == null)
                {
                    lClasses = classDA.GetClasses(year, pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    lClasses = classDA.GetClasses(year, grade, pageCurrentIndex, pageSize, out totalRecords);
                }
            }
            else
            {
                if (grade == null)
                {
                    lClasses = classDA.GetClasses(year, faculty, pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    lClasses = classDA.GetClasses(year, faculty, grade, pageCurrentIndex, pageSize, out totalRecords);
                }
            }

            TabularClass tabularClass = new TabularClass();
            foreach (LopHoc_Lop Class in lClasses)
            {
                tabularClass = GetTabularClass(Class);

                lTabularClasses.Add(tabularClass);
            }

            return lTabularClasses;
        }

        public List<LopHoc_Lop> GetUnformeredClasses(CauHinh_NamHoc year, DanhMuc_NganhHoc faculty, DanhMuc_KhoiLop grade)
        {
            List<LopHoc_Lop> lUnformeredClasses = new List<LopHoc_Lop>();
            if (faculty == null)
            {
                if (grade == null)
                {
                    lUnformeredClasses = classDA.GetUnformeredClasses(year);
                }
                else
                {
                    lUnformeredClasses = classDA.GetUnformeredClasses(year, faculty);
                }
            }
            else
            {
                if (grade == null)
                {
                    lUnformeredClasses = classDA.GetUnformeredClasses(year, grade);
                }
                else
                {
                    lUnformeredClasses = classDA.GetUnformeredClasses(year, faculty, grade);
                }
            }

            return lUnformeredClasses;
            
        }        

        public bool ClassNameExists(string className, CauHinh_NamHoc year)
        {
            return classDA.ClassNameExists(className, year);
        }

        public bool ClassNameExists(string oldClassName, string newClassName, CauHinh_NamHoc year)
        {
            if (oldClassName == newClassName)
            {
                return false;
            }
            else
            {
                return classDA.ClassNameExists(newClassName, year);
            }
        }

        public bool IsDeletable(LopHoc_Lop Class)
        {
            return classDA.IsDeletable(Class);
        }

        public bool HasFormerTeacher(LopHoc_Lop Class)
        {
            return classDA.HasFormerTeacher(Class);
        }
    }
}
