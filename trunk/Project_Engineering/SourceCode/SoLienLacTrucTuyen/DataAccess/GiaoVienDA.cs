using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class GiaoVienDA : BaseDA
    {
        public GiaoVienDA()
            : base()
        { }

        public void InsertGiaoVien(LopHoc_GiaoVien giaoVien)
        {
            db.LopHoc_GiaoViens.InsertOnSubmit(giaoVien);
            db.SubmitChanges();
        }

        public void UpdateGiaoVien(string maGiaoVien,
            string tenGiaoVien, bool gioiTinh, DateTime ngaySinh, string diaChi, string dienThoai)
        {
            LopHoc_GiaoVien giaoVien = (from gv in db.LopHoc_GiaoViens
                                        where gv.MaHienThiGiaoVien == maGiaoVien
                                        select gv).First();
            giaoVien.HoTen = tenGiaoVien;
            giaoVien.GioiTinh = gioiTinh;
            giaoVien.NgaySinh = ngaySinh;
            giaoVien.DiaChi = diaChi;
            giaoVien.DienThoai = dienThoai;
            db.SubmitChanges();
        }

        public void DeleteGiaoVien(int maGiaoVien)
        {
            LopHoc_GiaoVien giaoVien = (from gv in db.LopHoc_GiaoViens
                                        where gv.MaGiaoVien == maGiaoVien
                                        select gv).First();
            db.LopHoc_GiaoViens.DeleteOnSubmit(giaoVien);
            db.SubmitChanges();
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

        public bool MaGiaoVienExists(string maGiaoVien)
        {
            IQueryable<LopHoc_GiaoVien> giaoViens;
            giaoViens = from giaoVien in db.LopHoc_GiaoViens
                        where giaoVien.MaHienThiGiaoVien == maGiaoVien
                        select giaoVien;

            if(giaoViens.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CanDeleteGiaoVien(int maGiaoVien)
        {
            return true;
        }

        public LopHoc_GiaoVien GetGiaoVien(int maGiaoVien)
        {
            LopHoc_GiaoVien giaoVien = (from gv in db.LopHoc_GiaoViens
                                        where gv.MaGiaoVien == maGiaoVien
                                        select gv).First();
            return giaoVien;
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
                              select new TabularHoatDongChuNhiem {
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
