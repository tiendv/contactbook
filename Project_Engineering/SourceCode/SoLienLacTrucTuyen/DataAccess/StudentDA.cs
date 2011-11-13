using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class StudentDA : BaseDA
    {
        public StudentDA()
            : base()
        {

        }

        #region Student
        public void InsertStudent(HocSinh_ThongTinCaNhan student)
        {
            db.HocSinh_ThongTinCaNhans.InsertOnSubmit(student);
            db.SubmitChanges();
        }

        public void UpdateStudent(HocSinh_ThongTinCaNhan editedStudent)
        {
            HocSinh_ThongTinCaNhan student = null;
            IQueryable<HocSinh_ThongTinCaNhan> iqStudent = from std in db.HocSinh_ThongTinCaNhans
                                                           where std.MaHocSinh == editedStudent.MaHocSinh
                                                           select std;
            if (iqStudent.Count() != 0)
            {
                student = iqStudent.First();
                student.MaHocSinhHienThi = editedStudent.MaHocSinhHienThi;
                student.HoTen = editedStudent.HoTen;
                student.GioiTinh = editedStudent.GioiTinh;
                student.NgaySinh = editedStudent.NgaySinh;
                student.NoiSinh = editedStudent.NoiSinh;
                student.DiaChi = editedStudent.DiaChi;
                student.DienThoai = editedStudent.DienThoai;
                student.HoTenBo = editedStudent.HoTenBo;
                student.NgheNghiepBo = editedStudent.NgheNghiepBo;
                student.NgaySinhBo = editedStudent.NgaySinhBo;
                student.HoTenMe = editedStudent.HoTenMe;
                student.NgheNghiepMe = editedStudent.NgheNghiepMe;
                student.NgaySinhMe = editedStudent.NgaySinhMe;

                db.SubmitChanges();
            }
        }

        public void DeleteStudent(int maHocSinh)
        {
            IQueryable<HocSinh_HocSinhLopHoc> hocSinhLops;
            hocSinhLops = from hsLop in db.HocSinh_HocSinhLopHocs
                          where hsLop.MaHocSinh == maHocSinh
                          select hsLop;
            foreach (HocSinh_HocSinhLopHoc hsLop in hocSinhLops)
            {
                IQueryable<HocSinh_DanhHieuHocKy> danhHieuHKs;
                danhHieuHKs = from danhHieuHK in db.HocSinh_DanhHieuHocKies
                              where danhHieuHK.MaHocSinhLopHoc == hsLop.MaHocSinhLopHoc
                              select danhHieuHK;

                foreach (HocSinh_DanhHieuHocKy danhHieuHK in danhHieuHKs)
                {
                    db.HocSinh_DanhHieuHocKies.DeleteOnSubmit(danhHieuHK);
                }
                db.HocSinh_HocSinhLopHocs.DeleteOnSubmit(hsLop);
            }

            HocSinh_ThongTinCaNhan hocSinh;
            hocSinh = (from hS in db.HocSinh_ThongTinCaNhans
                       where hS.MaHocSinh == maHocSinh
                       select hS).First();
            db.HocSinh_ThongTinCaNhans.DeleteOnSubmit(hocSinh);
            db.SubmitChanges();

        }

        public HocSinh_ThongTinCaNhan GetStudent(string studentCode)
        {
            HocSinh_ThongTinCaNhan student = null;
            IQueryable<HocSinh_ThongTinCaNhan> iqStudent = from std in db.HocSinh_ThongTinCaNhans
                                                           where std.MaHocSinhHienThi == studentCode
                                                           select std;
            if (iqStudent.Count() != 0)
            {
                student = iqStudent.First();
            }

            return student;
        }

        public HocSinh_ThongTinCaNhan GetStudent(int studentId)
        {
            HocSinh_ThongTinCaNhan student = null;
            IQueryable<HocSinh_ThongTinCaNhan> iqStudent = from std in db.HocSinh_ThongTinCaNhans
                                                           where std.MaHocSinh == studentId
                                                           select std;
            if (iqStudent.Count() != 0)
            {
                student = iqStudent.First();
            }

            return student;
        }

        public bool IsDeletable(string studentCode)
        {
            return true;
        }

        public bool StudentCodeExists(string maHocSinhHienThi)
        {
            IQueryable<HocSinh_ThongTinCaNhan> hocSinhs = from hocSinh in db.HocSinh_ThongTinCaNhans
                                                          where hocSinh.MaHocSinhHienThi == maHocSinhHienThi
                                                          select hocSinh;
            if (hocSinhs.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public LopHoc_Lop GetClass(CauHinh_NamHoc year, HocSinh_ThongTinCaNhan student)
        {
            LopHoc_Lop Class = null;

            IQueryable<LopHoc_Lop> iqClass = from studentsInClass in db.HocSinh_HocSinhLopHocs
                                             join cls in db.LopHoc_Lops on studentsInClass.MaLopHoc equals cls.MaLopHoc
                                             where studentsInClass.MaHocSinh == student.MaHocSinh
                                               && studentsInClass.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                                             select cls;
            if (iqClass.Count() != 0)
            {
                Class = iqClass.First();
            }

            return Class;
        }

        public LopHoc_Lop GetLastedClass(HocSinh_ThongTinCaNhan student)
        {
            LopHoc_Lop Class = null;

            IQueryable<HocSinh_HocSinhLopHoc> iqStudentsInClass = from studentsInClass in db.HocSinh_HocSinhLopHocs
                                                                  where studentsInClass.MaHocSinh == student.MaHocSinh
                                                                  select studentsInClass;
            if (iqStudentsInClass.Count() != 0)
            {
                Class = iqStudentsInClass.OrderByDescending(studentsInClass => studentsInClass.MaHocSinhLopHoc).First().LopHoc_Lop;
            }

            return Class;
        }

        public List<CauHinh_NamHoc> GetYears(HocSinh_ThongTinCaNhan student)
        {
            List<CauHinh_NamHoc> years = new List<CauHinh_NamHoc>();
            IQueryable<CauHinh_NamHoc> iqYear = from year in db.CauHinh_NamHocs
                                                join cls in db.LopHoc_Lops
                                                   on year.MaNamHoc equals cls.MaNamHoc
                                                join stdInCls in db.HocSinh_HocSinhLopHocs
                                                   on cls.MaLopHoc equals stdInCls.MaLopHoc
                                                where stdInCls.MaHocSinh == student.MaHocSinh
                                                select year;
            if (iqYear.Count() != 0)
            {
                years = iqYear.OrderByDescending(year => year.NamBatDau).ToList();
            }

            return years;
        }
        #endregion

        #region StudentInClass
        public HocSinh_HocSinhLopHoc InsertStudentInClass(HocSinh_ThongTinCaNhan student, LopHoc_Lop Class)
        {
            HocSinh_HocSinhLopHoc studentInClass = new HocSinh_HocSinhLopHoc();
            studentInClass.MaLopHoc = Class.MaLopHoc;
            studentInClass.MaHocSinh = student.MaHocSinh;
            db.SubmitChanges();

            return GetLastedStudentInClass();
        }

        public void UpdateStudentInClass(HocSinh_HocSinhLopHoc editedStudentInClass, LopHoc_Lop Class)
        {
            HocSinh_HocSinhLopHoc studentInClass = null;
            IQueryable<HocSinh_HocSinhLopHoc> iqStudentInClass = from stdInCls in db.HocSinh_HocSinhLopHocs
                                                                 where stdInCls.MaHocSinhLopHoc == editedStudentInClass.MaHocSinhLopHoc
                                                                 select stdInCls;

            if (iqStudentInClass.Count() != 0)
            {
                studentInClass = iqStudentInClass.First();
                studentInClass.MaLopHoc = Class.MaLopHoc;
                db.SubmitChanges();
            }
        }

        public HocSinh_HocSinhLopHoc GetLastedStudentInClass()
        {
            HocSinh_HocSinhLopHoc lastedStudentInClass = null;

            IQueryable<HocSinh_HocSinhLopHoc> iqStudentInClass = from stdInCls in db.HocSinh_HocSinhLopHocs
                                                                 select stdInCls;
            if (iqStudentInClass.Count() != 0)
            {
                lastedStudentInClass = iqStudentInClass.OrderByDescending(stdInCls => stdInCls.MaHocSinhLopHoc).First();
            }

            return lastedStudentInClass;
        }

        public HocSinh_HocSinhLopHoc GetLastedStudentInClass(HocSinh_ThongTinCaNhan student)
        {
            HocSinh_HocSinhLopHoc studentInClass = null;
            IQueryable<HocSinh_HocSinhLopHoc> iqStudentInClass = from stdInCls in db.HocSinh_HocSinhLopHocs
                                                                 where stdInCls.MaHocSinh == student.MaHocSinh
                                                                 select stdInCls;

            if (iqStudentInClass.Count() != 0)
            {
                studentInClass = iqStudentInClass.OrderByDescending(stdInCls => stdInCls.MaHocSinhLopHoc).First();
            }

            return studentInClass;
        }

        public HocSinh_HocSinhLopHoc GetStudentInClass(int studentInClassId)
        {
            HocSinh_HocSinhLopHoc studentInClass = null;

            IQueryable<HocSinh_HocSinhLopHoc> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.HocSinh_HocSinhLopHocs
                               where stdInCls.MaHocSinhLopHoc == studentInClassId
                               select stdInCls;
            if (iqStudentInClass.Count() != 0)
            {
                studentInClass = iqStudentInClass.First();
            }

            return studentInClass;
        }

        public HocSinh_HocSinhLopHoc GetStudentInClass(HocSinh_ThongTinCaNhan student, CauHinh_NamHoc year)
        {
            HocSinh_HocSinhLopHoc studentInClass = null;
            IQueryable<HocSinh_HocSinhLopHoc> iqStudentInClass = from stdInCls in db.HocSinh_HocSinhLopHocs
                                                                 where stdInCls.MaHocSinh == student.MaHocSinh
                                                                 && stdInCls.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                                                                 select stdInCls;

            if (iqStudentInClass.Count() != 0)
            {
                studentInClass = iqStudentInClass.OrderByDescending(stdInCls => stdInCls.MaHocSinhLopHoc).First();
            }

            return studentInClass;
        }

        public List<HocSinh_HocSinhLopHoc> GetStudentInClasses(LopHoc_Lop Class)
        {
            List<HocSinh_HocSinhLopHoc> studentInClasses = new List<HocSinh_HocSinhLopHoc>();
            IQueryable<HocSinh_HocSinhLopHoc> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.HocSinh_HocSinhLopHocs
                               where stdInCls.LopHoc_Lop.MaLopHoc == Class.MaLopHoc
                               select stdInCls;
            if (iqStudentInClass.Count() != 0)
            {
                studentInClasses = iqStudentInClass.ToList();
            }

            return studentInClasses;
        }

        public List<HocSinh_HocSinhLopHoc> GetStudentInClasses()
        {
            List<HocSinh_HocSinhLopHoc> studentInClasses = new List<HocSinh_HocSinhLopHoc>();

            // get current year
            CauHinh_NamHoc currentYear = (new SystemConfigDA()).GetCurrentYear();

            IQueryable<HocSinh_HocSinhLopHoc> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.HocSinh_HocSinhLopHocs
                               where stdInCls.LopHoc_Lop.MaNamHoc == currentYear.MaNamHoc
                               select stdInCls;

            if (iqStudentInClass.Count() != 0)
            {
                studentInClasses = iqStudentInClass.OrderBy(std => std.HocSinh_ThongTinCaNhan.MaHocSinhHienThi)
                    .ThenBy(std => std.HocSinh_ThongTinCaNhan.HoTen).ToList();
            }

            return studentInClasses;
        }

        public List<HocSinh_HocSinhLopHoc> GetStudentInClasses(DanhMuc_KhoiLop grade)
        {
            List<HocSinh_HocSinhLopHoc> studentInClasses = new List<HocSinh_HocSinhLopHoc>();

            // get current year
            CauHinh_NamHoc currentYear = (new SystemConfigDA()).GetCurrentYear();

            IQueryable<HocSinh_HocSinhLopHoc> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.HocSinh_HocSinhLopHocs
                               where stdInCls.LopHoc_Lop.MaNamHoc == currentYear.MaNamHoc
                               && stdInCls.LopHoc_Lop.MaKhoiLop == grade.MaKhoiLop
                               select stdInCls;

            if (iqStudentInClass.Count() != 0)
            {
                studentInClasses = iqStudentInClass.OrderBy(std => std.HocSinh_ThongTinCaNhan.MaHocSinhHienThi)
                    .ThenBy(std => std.HocSinh_ThongTinCaNhan.HoTen).ToList();
            }

            return studentInClasses;
        }

        public List<HocSinh_HocSinhLopHoc> GetStudentInClasses(DanhMuc_NganhHoc faculty)
        {
            List<HocSinh_HocSinhLopHoc> studentInClasses = new List<HocSinh_HocSinhLopHoc>();

            // get current year
            CauHinh_NamHoc currentYear = (new SystemConfigDA()).GetCurrentYear();

            IQueryable<HocSinh_HocSinhLopHoc> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.HocSinh_HocSinhLopHocs
                               where stdInCls.LopHoc_Lop.MaNamHoc == currentYear.MaNamHoc
                               && stdInCls.LopHoc_Lop.MaNganhHoc == faculty.MaNganhHoc
                               select stdInCls;

            if (iqStudentInClass.Count() != 0)
            {
                studentInClasses = iqStudentInClass.OrderBy(std => std.HocSinh_ThongTinCaNhan.MaHocSinhHienThi)
                    .ThenBy(std => std.HocSinh_ThongTinCaNhan.HoTen).ToList();
            }

            return studentInClasses;
        }

        public List<HocSinh_HocSinhLopHoc> GetStudentInClasses(DanhMuc_KhoiLop grade, DanhMuc_NganhHoc faculty)
        {
            List<HocSinh_HocSinhLopHoc> studentInClasses = new List<HocSinh_HocSinhLopHoc>();

            // get current year
            CauHinh_NamHoc currentYear = (new SystemConfigDA()).GetCurrentYear();

            IQueryable<HocSinh_HocSinhLopHoc> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.HocSinh_HocSinhLopHocs
                               where stdInCls.LopHoc_Lop.MaNamHoc == currentYear.MaNamHoc
                               && stdInCls.LopHoc_Lop.MaNganhHoc == faculty.MaNganhHoc
                               && stdInCls.LopHoc_Lop.MaKhoiLop == grade.MaKhoiLop
                               select stdInCls;

            if (iqStudentInClass.Count() != 0)
            {
                studentInClasses = iqStudentInClass.OrderBy(std => std.HocSinh_ThongTinCaNhan.MaHocSinhHienThi)
                    .ThenBy(std => std.HocSinh_ThongTinCaNhan.HoTen).ToList();
            }

            return studentInClasses;
        }

        public List<HocSinh_HocSinhLopHoc> GetStudentInClasses(LopHoc_Lop Class, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<HocSinh_HocSinhLopHoc> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.HocSinh_HocSinhLopHocs
                               where stdInCls.LopHoc_Lop.MaLopHoc == Class.MaLopHoc
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<HocSinh_HocSinhLopHoc> GetStudentInClasses(LopHoc_Lop Class, string studentName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<HocSinh_HocSinhLopHoc> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.HocSinh_HocSinhLopHocs
                               where stdInCls.LopHoc_Lop.MaLopHoc == Class.MaLopHoc
                                  && stdInCls.HocSinh_ThongTinCaNhan.HoTen == studentName
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<HocSinh_HocSinhLopHoc> GetStudentInClasses(CauHinh_NamHoc year, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<HocSinh_HocSinhLopHoc> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.HocSinh_HocSinhLopHocs
                               where stdInCls.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<HocSinh_HocSinhLopHoc> GetStudentInClasses(CauHinh_NamHoc year, string studentName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<HocSinh_HocSinhLopHoc> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.HocSinh_HocSinhLopHocs
                               where stdInCls.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                               && stdInCls.HocSinh_ThongTinCaNhan.HoTen == studentName
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<HocSinh_HocSinhLopHoc> GetStudentInClasses(CauHinh_NamHoc year, DanhMuc_KhoiLop grade, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<HocSinh_HocSinhLopHoc> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.HocSinh_HocSinhLopHocs
                               where stdInCls.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                               && stdInCls.LopHoc_Lop.MaKhoiLop == grade.MaKhoiLop
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<HocSinh_HocSinhLopHoc> GetStudentInClasses(CauHinh_NamHoc year, DanhMuc_KhoiLop grade, string studentName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<HocSinh_HocSinhLopHoc> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.HocSinh_HocSinhLopHocs
                               where stdInCls.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                               && stdInCls.LopHoc_Lop.MaKhoiLop == grade.MaKhoiLop
                               && stdInCls.HocSinh_ThongTinCaNhan.HoTen == studentName
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<HocSinh_HocSinhLopHoc> GetStudentInClasses(CauHinh_NamHoc year, DanhMuc_NganhHoc faculty, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<HocSinh_HocSinhLopHoc> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.HocSinh_HocSinhLopHocs
                               where stdInCls.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                               && stdInCls.LopHoc_Lop.MaNganhHoc == faculty.MaNganhHoc
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<HocSinh_HocSinhLopHoc> GetStudentInClasses(CauHinh_NamHoc year, DanhMuc_NganhHoc faculty, string studentName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<HocSinh_HocSinhLopHoc> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.HocSinh_HocSinhLopHocs
                               where stdInCls.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                               && stdInCls.LopHoc_Lop.MaNganhHoc == faculty.MaNganhHoc
                               && stdInCls.HocSinh_ThongTinCaNhan.HoTen == studentName
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<HocSinh_HocSinhLopHoc> GetStudentInClasses(CauHinh_NamHoc year, DanhMuc_NganhHoc faculty, DanhMuc_KhoiLop grade, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<HocSinh_HocSinhLopHoc> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.HocSinh_HocSinhLopHocs
                               where stdInCls.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                               && stdInCls.LopHoc_Lop.MaNganhHoc == faculty.MaNganhHoc
                               && stdInCls.LopHoc_Lop.MaKhoiLop == grade.MaKhoiLop
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<HocSinh_HocSinhLopHoc> GetStudentInClasses(CauHinh_NamHoc year, DanhMuc_NganhHoc faculty, DanhMuc_KhoiLop grade, string studentName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<HocSinh_HocSinhLopHoc> iqStudentInClass;
            iqStudentInClass = from stdInCls in db.HocSinh_HocSinhLopHocs
                               where stdInCls.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                               && stdInCls.LopHoc_Lop.MaNganhHoc == faculty.MaNganhHoc
                               && stdInCls.LopHoc_Lop.MaKhoiLop == grade.MaKhoiLop
                               && stdInCls.HocSinh_ThongTinCaNhan.HoTen == studentName
                               select stdInCls;

            return GetStudentInClasses(ref iqStudentInClass, pageCurrentIndex, pageSize, out totalRecords);
        }

        private List<HocSinh_HocSinhLopHoc> GetStudentInClasses(ref IQueryable<HocSinh_HocSinhLopHoc> iqStudentInClass, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<HocSinh_HocSinhLopHoc> studentInClasses = new List<HocSinh_HocSinhLopHoc>();

            totalRecords = iqStudentInClass.Count();
            if (totalRecords != 0)
            {
                studentInClasses = iqStudentInClass.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return studentInClasses;
        }
        #endregion

        public List<TabularHanhKiemHocSinh> GetListHanhKiemHocSinh(int maLopHoc, int maHocKy, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularHanhKiemHocSinh> iqDanhHieuHK;
            iqDanhHieuHK = from danhHieuHK in db.HocSinh_DanhHieuHocKies
                           join hocSinhLop in db.HocSinh_HocSinhLopHocs
                                on danhHieuHK.MaHocSinhLopHoc equals hocSinhLop.MaHocSinhLopHoc
                           join hocSinh in db.HocSinh_ThongTinCaNhans
                                on hocSinhLop.MaHocSinh equals hocSinh.MaHocSinh
                           where hocSinhLop.MaLopHoc == maLopHoc && danhHieuHK.MaHocKy == maHocKy
                           select new TabularHanhKiemHocSinh
                           {
                               MaHocSinh = hocSinhLop.MaHocSinh,
                               MaHocSinhHienThi = hocSinh.MaHocSinhHienThi,
                               HoTenHocSinh = hocSinh.HoTen,
                               MaHanhKiem = danhHieuHK.MaHanhKiemHK
                           };

            totalRecords = iqDanhHieuHK.Count();
            if (totalRecords != 0)
            {
                return iqDanhHieuHK.OrderBy(hocSinh => hocSinh.MaHocSinhHienThi)
                    .ThenBy(hocSinh => hocSinh.HoTenHocSinh)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularHanhKiemHocSinh>();
            }
        }

        public void UpdateStudenTermConduct(LopHoc_Lop Class, CauHinh_HocKy term, HocSinh_ThongTinCaNhan student, DanhMuc_HanhKiem conduct)
        {
            HocSinh_DanhHieuHocKy studentTermResult = null;
            IQueryable<HocSinh_DanhHieuHocKy> iqStudentTermResult;
            iqStudentTermResult = from stdTermResult in db.HocSinh_DanhHieuHocKies
                                  where stdTermResult.HocSinh_HocSinhLopHoc.MaLopHoc == Class.MaLopHoc
                                    && stdTermResult.MaHocKy == term.MaHocKy
                                    && stdTermResult.HocSinh_HocSinhLopHoc.MaHocSinh == student.MaHocSinh
                                  select stdTermResult;
            if (iqStudentTermResult.Count() != null)
            {
                studentTermResult = iqStudentTermResult.First();
                studentTermResult.MaHanhKiemHK = conduct.MaHanhKiem;
                db.SubmitChanges();
            }
        }
    }
}