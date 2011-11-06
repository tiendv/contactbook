using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class TeacherDA : BaseDA
    {
        public TeacherDA()
            : base()
        { }

        public void InsertTeacher(LopHoc_GiaoVien teacher)
        {
            db.LopHoc_GiaoViens.InsertOnSubmit(teacher);
            db.SubmitChanges();
        }

        public void UpdateTeacher(LopHoc_GiaoVien editedTeacher)
        {
            IQueryable<LopHoc_GiaoVien> iqTeacher = from tchr in db.LopHoc_GiaoViens
                                                    where tchr.MaGiaoVien == editedTeacher.MaGiaoVien
                                                    select tchr;

            if (iqTeacher.Count() != 0)
            {
                LopHoc_GiaoVien teacher = iqTeacher.First();
                teacher.MaHienThiGiaoVien = editedTeacher.MaHienThiGiaoVien;
                teacher.HoTen = editedTeacher.HoTen;
                teacher.GioiTinh = editedTeacher.GioiTinh;
                teacher.NgaySinh = editedTeacher.NgaySinh;
                teacher.HinhAnh = editedTeacher.HinhAnh;
                teacher.DiaChi = editedTeacher.DiaChi;
                teacher.DienThoai = editedTeacher.DienThoai;

                db.SubmitChanges();
            }
        }

        public void DeleteTeacher(LopHoc_GiaoVien deletedTeacher)
        {
            IQueryable<LopHoc_GiaoVien> iqTeacher = from tchr in db.LopHoc_GiaoViens
                                                    where tchr.MaGiaoVien == deletedTeacher.MaGiaoVien
                                                    select tchr;

            if (iqTeacher.Count() != 0)
            {
                LopHoc_GiaoVien teacher = iqTeacher.First();
                db.LopHoc_GiaoViens.DeleteOnSubmit(teacher);
                db.SubmitChanges();
            }
        }

        public LopHoc_GiaoVien GetTeacher(string teacherCode)
        {
            LopHoc_GiaoVien teacher = null;

            IQueryable<LopHoc_GiaoVien> iqTeacher = from tchr in db.LopHoc_GiaoViens
                                                    where tchr.MaHienThiGiaoVien == teacherCode
                                                    select tchr;

            if (iqTeacher.Count() != 0)
            {
                teacher = iqTeacher.First();
            }

            return teacher;
        }

        public List<TabularGiaoVien> GetListTabularGiaoViens(string maHienThiGiaoVien, string hoTen,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVien> tbGiaoViens = from giaoVien in db.LopHoc_GiaoViens
                                                      where giaoVien.MaHienThiGiaoVien == maHienThiGiaoVien
                                                        && giaoVien.HoTen == hoTen
                                                      select new TabularGiaoVien
                                                      {
                                                          MaGiaoVien = giaoVien.MaGiaoVien,
                                                          MaHienThiGiaoVien = giaoVien.MaHienThiGiaoVien,
                                                          HoTen = giaoVien.HoTen,
                                                          NgaySinh = giaoVien.NgaySinh,
                                                          GioiTinh = giaoVien.GioiTinh
                                                      };
            totalRecords = tbGiaoViens.Count();
            if (totalRecords != 0)
            {
                return tbGiaoViens.OrderBy(giaoVien => giaoVien.MaHienThiGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularGiaoVien>();
            }
        }

        public List<TabularGiaoVien> GetListTabularGiaoViens(
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVien> tbGiaoViens = from giaoVien in db.LopHoc_GiaoViens
                                                      select new TabularGiaoVien
                                                      {
                                                          MaGiaoVien = giaoVien.MaGiaoVien,
                                                          MaHienThiGiaoVien = giaoVien.MaHienThiGiaoVien,
                                                          HoTen = giaoVien.HoTen,
                                                          NgaySinh = giaoVien.NgaySinh,
                                                          GioiTinh = giaoVien.GioiTinh
                                                      };
            totalRecords = tbGiaoViens.Count();
            if (totalRecords != 0)
            {
                return tbGiaoViens.OrderBy(giaoVien => giaoVien.MaHienThiGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularGiaoVien>();
            }
        }

        public List<TabularGiaoVien> GetListTabularGiaoViensByHoTen(string hoTen,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVien> tbGiaoViens = from giaoVien in db.LopHoc_GiaoViens
                                                      where giaoVien.HoTen == hoTen
                                                      select new TabularGiaoVien
                                                      {
                                                          MaGiaoVien = giaoVien.MaGiaoVien,
                                                          MaHienThiGiaoVien = giaoVien.MaHienThiGiaoVien,
                                                          HoTen = giaoVien.HoTen,
                                                          NgaySinh = giaoVien.NgaySinh,
                                                          GioiTinh = giaoVien.GioiTinh
                                                      };
            totalRecords = tbGiaoViens.Count();
            if (totalRecords != 0)
            {
                return tbGiaoViens.OrderBy(giaoVien => giaoVien.MaHienThiGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularGiaoVien>();
            }
        }

        public List<TabularGiaoVien> GetListTabularGiaoViensByMaHienThi(string maHienThiGiaoVien,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVien> tbGiaoViens = from giaoVien in db.LopHoc_GiaoViens
                                                      where giaoVien.MaHienThiGiaoVien == maHienThiGiaoVien
                                                      select new TabularGiaoVien
                                                      {
                                                          MaGiaoVien = giaoVien.MaGiaoVien,
                                                          MaHienThiGiaoVien = giaoVien.MaHienThiGiaoVien,
                                                          HoTen = giaoVien.HoTen,
                                                          NgaySinh = giaoVien.NgaySinh,
                                                          GioiTinh = giaoVien.GioiTinh
                                                      };
            totalRecords = tbGiaoViens.Count();
            if (totalRecords != 0)
            {
                return tbGiaoViens.OrderBy(giaoVien => giaoVien.MaHienThiGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularGiaoVien>();
            }
        }

        public List<TabularGiaoVien> GetListTabularGiaoVienKhongChuNhiems(int maNamHoc,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVien> tbGiaoViens = from giaoVien in db.LopHoc_GiaoViens
                                                      select new TabularGiaoVien
                                                      {
                                                          MaGiaoVien = giaoVien.MaGiaoVien,
                                                          MaHienThiGiaoVien = giaoVien.MaHienThiGiaoVien,
                                                          HoTen = giaoVien.HoTen,
                                                          NgaySinh = giaoVien.NgaySinh,
                                                          GioiTinh = giaoVien.GioiTinh
                                                      };

            if (tbGiaoViens.Count() != 0)
            {
                List<TabularGiaoVien> lstTbGiaoViens = tbGiaoViens.ToList();
                int i = 0;
                while (i < lstTbGiaoViens.Count)
                {
                    IQueryable<LopHoc_GVCN> giaoVienChuNhiems;
                    giaoVienChuNhiems = from gvcn in db.LopHoc_GVCNs
                                        join giaoVien in db.LopHoc_GiaoViens
                                            on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                                        join lop in db.LopHoc_Lops
                                            on gvcn.MaLopHoc equals lop.MaLopHoc
                                        where gvcn.MaGiaoVien == lstTbGiaoViens[i].MaGiaoVien
                                            && lop.MaNamHoc == maNamHoc
                                        select gvcn;
                    if (giaoVienChuNhiems.Count() != 0)
                    {
                        lstTbGiaoViens.Remove(lstTbGiaoViens[i]);
                    }
                    else
                    {
                        i++;
                    }
                }
                totalRecords = lstTbGiaoViens.Count;
                return lstTbGiaoViens.OrderBy(giaoVien => giaoVien.MaHienThiGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                totalRecords = 0;
                return new List<TabularGiaoVien>();
            }
        }

        public List<TabularGiaoVien> GetListTabularGiaoVienKhongChuNhiemsByHoTen(int maNamHoc,
            string hoTen, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVien> tbGiaoViens = from giaoVien in db.LopHoc_GiaoViens
                                                      where giaoVien.HoTen == hoTen
                                                      select new TabularGiaoVien
                                                      {
                                                          MaGiaoVien = giaoVien.MaGiaoVien,
                                                          MaHienThiGiaoVien = giaoVien.MaHienThiGiaoVien,
                                                          HoTen = giaoVien.HoTen,
                                                          NgaySinh = giaoVien.NgaySinh,
                                                          GioiTinh = giaoVien.GioiTinh
                                                      };

            if (tbGiaoViens.Count() != 0)
            {
                List<TabularGiaoVien> lstTbGiaoViens = tbGiaoViens.ToList();
                int i = 0;
                while (i < lstTbGiaoViens.Count)
                {
                    IQueryable<LopHoc_GVCN> giaoVienChuNhiems;
                    giaoVienChuNhiems = from gvcn in db.LopHoc_GVCNs
                                        join giaoVien in db.LopHoc_GiaoViens
                                            on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                                        join lop in db.LopHoc_Lops
                                            on gvcn.MaLopHoc equals lop.MaLopHoc
                                        where gvcn.MaGiaoVien == lstTbGiaoViens[i].MaGiaoVien
                                            && lop.MaNamHoc == maNamHoc
                                        select gvcn;
                    if (giaoVienChuNhiems.Count() != 0)
                    {
                        lstTbGiaoViens.Remove(lstTbGiaoViens[i]);
                    }
                    else
                    {
                        i++;
                    }
                }
                totalRecords = lstTbGiaoViens.Count;
                return lstTbGiaoViens.OrderBy(giaoVien => giaoVien.MaHienThiGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                totalRecords = 0;
                return new List<TabularGiaoVien>();
            }
        }

        public List<TabularGiaoVien> GetListTabularGiaoVienKhongChuNhiemsByMaHienThi(int maNamHoc,
            string maHienThiGiaoVien, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVien> tbGiaoViens = from giaoVien in db.LopHoc_GiaoViens
                                                      where giaoVien.MaHienThiGiaoVien == maHienThiGiaoVien
                                                      select new TabularGiaoVien
                                                      {
                                                          MaGiaoVien = giaoVien.MaGiaoVien,
                                                          MaHienThiGiaoVien = giaoVien.MaHienThiGiaoVien,
                                                          HoTen = giaoVien.HoTen,
                                                          NgaySinh = giaoVien.NgaySinh,
                                                          GioiTinh = giaoVien.GioiTinh
                                                      };

            if (tbGiaoViens.Count() != 0)
            {
                List<TabularGiaoVien> lstTbGiaoViens = tbGiaoViens.ToList();
                int i = 0;
                while (i < lstTbGiaoViens.Count)
                {
                    IQueryable<LopHoc_GVCN> giaoVienChuNhiems;
                    giaoVienChuNhiems = from gvcn in db.LopHoc_GVCNs
                                        join giaoVien in db.LopHoc_GiaoViens
                                            on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                                        join lop in db.LopHoc_Lops
                                            on gvcn.MaLopHoc equals lop.MaLopHoc
                                        where gvcn.MaGiaoVien == lstTbGiaoViens[i].MaGiaoVien
                                            && lop.MaNamHoc == maNamHoc
                                        select gvcn;
                    if (giaoVienChuNhiems.Count() != 0)
                    {
                        lstTbGiaoViens.Remove(lstTbGiaoViens[i]);
                    }
                    else
                    {
                        i++;
                    }
                }
                totalRecords = lstTbGiaoViens.Count;
                return lstTbGiaoViens.OrderBy(giaoVien => giaoVien.MaHienThiGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                totalRecords = 0;
                return new List<TabularGiaoVien>();
            }
        }

        public List<TabularGiaoVien> GetListTabularGiaoViens(int maNamHoc,
            string maHienThiGiaoVien, string hoTen,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVien> tbGiaoViens = from giaoVien in db.LopHoc_GiaoViens
                                                      where giaoVien.MaHienThiGiaoVien == maHienThiGiaoVien
                                                            && giaoVien.HoTen == hoTen
                                                      select new TabularGiaoVien
                                                      {
                                                          MaGiaoVien = giaoVien.MaGiaoVien,
                                                          MaHienThiGiaoVien = giaoVien.MaHienThiGiaoVien,
                                                          HoTen = giaoVien.HoTen,
                                                          NgaySinh = giaoVien.NgaySinh,
                                                          GioiTinh = giaoVien.GioiTinh
                                                      };

            if (tbGiaoViens.Count() != 0)
            {
                List<TabularGiaoVien> lstTbGiaoViens = tbGiaoViens.ToList();
                int i = 0;
                while (i < lstTbGiaoViens.Count)
                {
                    IQueryable<LopHoc_GVCN> giaoVienChuNhiems;
                    giaoVienChuNhiems = from gvcn in db.LopHoc_GVCNs
                                        join giaoVien in db.LopHoc_GiaoViens
                                            on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                                        join lop in db.LopHoc_Lops
                                            on gvcn.MaLopHoc equals lop.MaLopHoc
                                        where gvcn.MaGiaoVien == lstTbGiaoViens[i].MaGiaoVien
                                            && lop.MaNamHoc == maNamHoc
                                        select gvcn;
                    if (giaoVienChuNhiems.Count() != 0)
                    {
                        lstTbGiaoViens.Remove(lstTbGiaoViens[i]);
                    }
                    else
                    {
                        i++;
                    }
                }
                totalRecords = lstTbGiaoViens.Count;
                return lstTbGiaoViens.OrderBy(giaoVien => giaoVien.MaHienThiGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                totalRecords = 0;
                return new List<TabularGiaoVien>();
            }
        }

        public bool TeacherCodeExists(string teacherCode)
        {
            IQueryable<LopHoc_GiaoVien> giaoViens;
            giaoViens = from giaoVien in db.LopHoc_GiaoViens
                        where giaoVien.MaHienThiGiaoVien == teacherCode
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

        public bool IsDeletable(LopHoc_GiaoVien teacher)
        {
            return true;
        }

        public LopHoc_GiaoVien GetTeacher(int maGiaoVien)
        {
            LopHoc_GiaoVien giaoVien = (from gv in db.LopHoc_GiaoViens
                                        where gv.MaGiaoVien == maGiaoVien
                                        select gv).First();
            return giaoVien;
        }

        public List<TabularHoatDongChuNhiem> GetListTbHoatDongChuNhiem(int maGiaoVien,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularHoatDongChuNhiem> tbHdongChNhiems;
            tbHdongChNhiems = from chuNhiem in db.LopHoc_GVCNs
                              join lop in db.LopHoc_Lops
                                on chuNhiem.MaLopHoc equals lop.MaLopHoc
                              join namHoc in db.CauHinh_NamHocs
                                on lop.MaNamHoc equals namHoc.MaNamHoc
                              where chuNhiem.MaGiaoVien == maGiaoVien
                              select new TabularHoatDongChuNhiem
                              {
                                  MaNamHoc = namHoc.MaNamHoc,
                                  TenNamHoc = namHoc.TenNamHoc,
                                  MaLopHoc = lop.MaLopHoc,
                                  TenLopHoc = lop.TenLopHoc
                              };

            totalRecords = tbHdongChNhiems.Count();

            if (totalRecords != 0)
            {
                return tbHdongChNhiems.OrderByDescending(chuNhiem => chuNhiem.TenNamHoc)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularHoatDongChuNhiem>();
            }
        }

        public List<TabularHoatDongGiangDay> GetListTbHoatDongGiangDay(int maGiaoVien,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularHoatDongGiangDay> tbHdongGiangDays;
            tbHdongGiangDays = from monHocTKB in db.LopHoc_MonHocTKBs
                               join lop in db.LopHoc_Lops
                                    on monHocTKB.MaLopHoc equals lop.MaLopHoc
                               join namHoc in db.CauHinh_NamHocs
                                    on lop.MaNamHoc equals namHoc.MaNamHoc
                               join monHoc in db.DanhMuc_MonHocs
                                    on monHocTKB.MaMonHoc equals monHoc.MaMonHoc
                               join hocKy in db.CauHinh_HocKies
                                    on monHocTKB.MaHocKy equals hocKy.MaHocKy
                               where monHocTKB.MaGiaoVien == maGiaoVien
                               select new TabularHoatDongGiangDay
                               {
                                   MaNamHoc = namHoc.MaNamHoc,
                                   TenNamHoc = namHoc.TenNamHoc,
                                   MaHocKy = monHocTKB.MaHocKy,
                                   TenHocKy = hocKy.TenHocKy,
                                   MaLopHoc = lop.MaLopHoc,
                                   TenLopHoc = lop.TenLopHoc,
                                   MaMonHoc = monHocTKB.MaMonHoc,
                                   TenMonHoc = monHoc.TenMonHoc
                               };

            totalRecords = tbHdongGiangDays.Count();

            if (totalRecords != 0)
            {
                return tbHdongGiangDays.OrderByDescending(chuNhiem => chuNhiem.TenNamHoc)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).Distinct().ToList();
            }
            else
            {
                return new List<TabularHoatDongGiangDay>();
            }
        }

        public bool IsTeaching(int maGiaoVien, int maHocKy, int maThu, int maTiet)
        {
            IQueryable<LopHoc_MonHocTKB> iqThoiKhoaBieu;
            iqThoiKhoaBieu = from tkb in db.LopHoc_MonHocTKBs
                             where tkb.MaGiaoVien == maGiaoVien
                                && tkb.MaHocKy == maHocKy
                                && tkb.MaThu == maThu
                                && tkb.MaTiet == maTiet
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
    }
}
