using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class ClassDA : BaseDA
    {
        public ClassDA()
            : base()
        {
        }

        public void InsertClass(LopHoc_Lop Class)
        {
            db.LopHoc_Lops.InsertOnSubmit(Class);
            db.SubmitChanges();
        }

        public void UpdateClass(LopHoc_Lop editedClass)
        {
            LopHoc_Lop Class = null;
            IQueryable<LopHoc_Lop> iqClass = from cls in db.LopHoc_Lops
                                             where cls.MaLopHoc == editedClass.MaLopHoc
                                             select cls;
            if (iqClass.Count() != 0)
            {
                Class = iqClass.First();
                Class.TenLopHoc = editedClass.TenLopHoc;
                db.SubmitChanges();
            }
        }

        public void IncreaseStudentAmount(LopHoc_Lop editedClass)
        {
            LopHoc_Lop Class = null;
            IQueryable<LopHoc_Lop> iqClass = from cls in db.LopHoc_Lops
                                             where cls.MaLopHoc == editedClass.MaLopHoc
                                             select cls;
            if (iqClass.Count() != 0)
            {
                Class = iqClass.First();
                Class.SiSo++;
                db.SubmitChanges();
            }
        }

        public void DeleteClass(LopHoc_Lop deletedClass)
        {
            LopHoc_Lop Class = null;
            IQueryable<LopHoc_Lop> iqClass = from cls in db.LopHoc_Lops
                                             where cls.MaLopHoc == deletedClass.MaLopHoc
                                             select cls;
            if (iqClass.Count() != 0)
            {
                Class = iqClass.First();
                db.LopHoc_Lops.DeleteOnSubmit(Class);
                db.SubmitChanges();
            }
        }

        public LopHoc_Lop GetClass(int classID)
        {
            LopHoc_Lop Class = null;
            IQueryable<LopHoc_Lop> iqClass = from cls in db.LopHoc_Lops
                                             where cls.MaLopHoc == classID
                                             select cls;
            if (iqClass.Count() != 0)
            {
                Class = iqClass.First();
            }

            return Class;
        }

        public List<LopHoc_Lop> GetClasses(CauHinh_NamHoc year)
        {
            List<LopHoc_Lop> lClasses = new List<LopHoc_Lop>();

            IQueryable<LopHoc_Lop> iqClass = from cls in db.LopHoc_Lops
                                             where cls.MaNamHoc == year.MaNamHoc
                                             select cls;
            if (iqClass.Count() != 0)
            {
                lClasses = iqClass.ToList();
            }

            return lClasses;
        }

        public List<LopHoc_Lop> GetClasses(CauHinh_NamHoc year, DanhMuc_NganhHoc faculty, DanhMuc_KhoiLop grade)
        {
            List<LopHoc_Lop> lClasses = new List<LopHoc_Lop>();

            IQueryable<LopHoc_Lop> iqClass = from cls in db.LopHoc_Lops
                                             where cls.MaNamHoc == year.MaNamHoc
                                             && cls.MaKhoiLop == grade.MaKhoiLop && cls.MaNganhHoc == faculty.MaNganhHoc
                                             select cls;
            if (iqClass.Count() != 0)
            {
                lClasses = iqClass.OrderBy(cls => cls.TenLopHoc).ToList();
            }

            return lClasses;
        }

        public List<LopHoc_Lop> GetClasses(CauHinh_NamHoc year, DanhMuc_NganhHoc faculty)
        {
            List<LopHoc_Lop> lClasses = new List<LopHoc_Lop>();

            IQueryable<LopHoc_Lop> iqClass = from cls in db.LopHoc_Lops
                                             where cls.MaNamHoc == year.MaNamHoc && cls.MaNganhHoc == faculty.MaNganhHoc
                                             select cls;
            if (iqClass.Count() != 0)
            {
                lClasses = iqClass.OrderBy(cls => cls.TenLopHoc).ToList();
            }

            return lClasses;
        }

        public List<LopHoc_Lop> GetClasses(CauHinh_NamHoc year, DanhMuc_KhoiLop grade)
        {
            List<LopHoc_Lop> lClasses = new List<LopHoc_Lop>();

            IQueryable<LopHoc_Lop> iqClass = from cls in db.LopHoc_Lops
                                             where cls.MaNamHoc == year.MaNamHoc
                                             && cls.MaKhoiLop == grade.MaKhoiLop
                                             select cls;
            if (iqClass.Count() != 0)
            {
                lClasses = iqClass.OrderBy(cls => cls.TenLopHoc).ToList();
            }

            return lClasses;
        }

        public List<LopHoc_Lop> GetClasses(CauHinh_NamHoc year, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<LopHoc_Lop> iqClass = from cls in db.LopHoc_Lops
                                             where cls.MaNamHoc == year.MaNamHoc
                                             select cls;

            return GetClasses(ref iqClass, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<LopHoc_Lop> GetClasses(CauHinh_NamHoc year, DanhMuc_NganhHoc faculty, DanhMuc_KhoiLop grade, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<LopHoc_Lop> iqClass = from cls in db.LopHoc_Lops
                                             where cls.MaNamHoc == year.MaNamHoc
                                             && cls.MaKhoiLop == grade.MaKhoiLop && cls.MaNganhHoc == faculty.MaNganhHoc
                                             select cls;

            return GetClasses(ref iqClass, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<LopHoc_Lop> GetClasses(CauHinh_NamHoc year, DanhMuc_NganhHoc faculty, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<LopHoc_Lop> iqClass = from cls in db.LopHoc_Lops
                                             where cls.MaNamHoc == year.MaNamHoc && cls.MaNganhHoc == faculty.MaNganhHoc
                                             select cls;

            return GetClasses(ref iqClass, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<LopHoc_Lop> GetClasses(CauHinh_NamHoc year, DanhMuc_KhoiLop grade, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<LopHoc_Lop> iqClass = from cls in db.LopHoc_Lops
                                             where cls.MaNamHoc == year.MaNamHoc
                                             && cls.MaKhoiLop == grade.MaKhoiLop
                                             select cls;

            return GetClasses(ref iqClass, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<LopHoc_Lop> GetUnformeredClasses(CauHinh_NamHoc year)
        {
            List<LopHoc_Lop> lClasses = GetClasses(year);
            if (lClasses.Count != 0)
            {
                int i = 0;
                while (i < lClasses.Count)
                {
                    IQueryable<LopHoc_GVCN> iqFormerTeacher = from formerTeacher in db.LopHoc_GVCNs
                                                              where formerTeacher.MaLopHoc == lClasses[i].MaLopHoc
                                                              select formerTeacher;
                    if (iqFormerTeacher.Count() != 0)
                    {
                        lClasses.Remove(lClasses[i]);
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            return lClasses;
        }

        public List<LopHoc_Lop> GetUnformeredClasses(CauHinh_NamHoc year, DanhMuc_NganhHoc faculty)
        {
            List<LopHoc_Lop> lClasses = GetClasses(year, faculty);
            if (lClasses.Count != 0)
            {
                int i = 0;
                while (i < lClasses.Count)
                {
                    IQueryable<LopHoc_GVCN> iqFormerTeacher = from formerTeacher in db.LopHoc_GVCNs
                                                              where formerTeacher.MaLopHoc == lClasses[i].MaLopHoc
                                                              select formerTeacher;
                    if (iqFormerTeacher.Count() != 0)
                    {
                        lClasses.Remove(lClasses[i]);
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            return lClasses;
        }

        public List<LopHoc_Lop> GetUnformeredClasses(CauHinh_NamHoc year, DanhMuc_KhoiLop grade)
        {
            List<LopHoc_Lop> lClasses = GetClasses(year, grade);
            if (lClasses.Count != 0)
            {
                int i = 0;
                while (i < lClasses.Count)
                {
                    IQueryable<LopHoc_GVCN> iqFormerTeacher = from formerTeacher in db.LopHoc_GVCNs
                                                              where formerTeacher.MaLopHoc == lClasses[i].MaLopHoc
                                                              select formerTeacher;
                    if (iqFormerTeacher.Count() != 0)
                    {
                        lClasses.Remove(lClasses[i]);
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            return lClasses;
        }

        public List<LopHoc_Lop> GetUnformeredClasses(CauHinh_NamHoc year, DanhMuc_NganhHoc faculty, DanhMuc_KhoiLop grade)
        {
            List<LopHoc_Lop> lClasses = GetClasses(year, faculty, grade);
            if (lClasses.Count != 0)
            {
                int i = 0;
                while (i < lClasses.Count)
                {
                    IQueryable<LopHoc_GVCN> iqFormerTeacher = from formerTeacher in db.LopHoc_GVCNs
                                                              where formerTeacher.MaLopHoc == lClasses[i].MaLopHoc
                                                              select formerTeacher;
                    if (iqFormerTeacher.Count() != 0)
                    {
                        lClasses.Remove(lClasses[i]);
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            return lClasses;
        }

        private List<LopHoc_Lop> GetClasses(ref IQueryable<LopHoc_Lop> iqClass, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<LopHoc_Lop> lClasses = new List<LopHoc_Lop>();
            totalRecords = iqClass.Count();
            if (totalRecords != 0)
            {
                lClasses = iqClass.OrderBy(cls => cls.DanhMuc_NganhHoc.TenNganhHoc)
                    .ThenBy(cls => cls.DanhMuc_KhoiLop.TenKhoiLop).ThenBy(cls => cls.TenLopHoc)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return lClasses;
        }

        public bool ClassNameExists(string className, CauHinh_NamHoc year)
        {
            IQueryable<LopHoc_Lop> iqClass = from cls in db.LopHoc_Lops
                                             where cls.TenLopHoc == className && cls.MaNamHoc == year.MaNamHoc
                                             select cls;
            if (iqClass.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ClassNameExists(string className)
        {
            IQueryable<LopHoc_Lop> iqClass = from cls in db.LopHoc_Lops
                                             where cls.TenLopHoc == className
                                             select cls;
            if (iqClass.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsDeletable(LopHoc_Lop Class)
        {
            IQueryable<LopHoc_MonHocTKB> iqSchedule = from schedule in db.LopHoc_MonHocTKBs
                                                      where schedule.MaLopHoc == Class.MaLopHoc
                                                      select schedule;
            if (iqSchedule.Count() != 0)
            {
                return false;
            }
            else
            {
                IQueryable<LopHoc_ThongBao> iqClassNews = from clsNews in db.LopHoc_ThongBaos
                                                          where clsNews.MaLopHoc == Class.MaLopHoc
                                                          select clsNews;
                if (iqClassNews.Count() != 0)
                {
                    return false;
                }
                else
                {
                    IQueryable<HocSinh_HocSinhLopHoc> iqStudentsInClass = from stdsInCls in db.HocSinh_HocSinhLopHocs
                                                                          where stdsInCls.MaLopHoc == Class.MaLopHoc
                                                                          select stdsInCls;
                    if (iqStudentsInClass.Count() != 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        public bool HasFormerTeacher(LopHoc_Lop Class)
        {
            IQueryable<LopHoc_GVCN> iqFormerTeacher = from formerTeacher in db.LopHoc_GVCNs
                                                      where formerTeacher.MaLopHoc == Class.MaLopHoc
                                                      select formerTeacher;
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
