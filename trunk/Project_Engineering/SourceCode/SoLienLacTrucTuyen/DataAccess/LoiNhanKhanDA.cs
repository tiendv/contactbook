using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class LoiNhanKhanDA : BaseDA
    {
        public LoiNhanKhanDA()
            : base()
        {
        }

        public void InsertLoiNhanKhan(int maHocSinhLopHoc, string tieuDe, string noiDung,
            DateTime ngay)
        {
            LoiNhanKhan_LoiNhanKhan loiNhanKhan = new LoiNhanKhan_LoiNhanKhan
            {
                MaHocSinhLopHoc = maHocSinhLopHoc,
                TieuDe = tieuDe,
                NoiDung = noiDung,
                Ngay = ngay
            };

            db.LoiNhanKhan_LoiNhanKhans.InsertOnSubmit(loiNhanKhan);
            db.SubmitChanges();
        }

        public void UpdateLoiNhanKhan(int maLoiNhanKhan, string tieuDe, string noiDung,
            DateTime ngay)
        {
            LoiNhanKhan_LoiNhanKhan loiNhanKhan = (from lnk in db.LoiNhanKhan_LoiNhanKhans
                                                  where lnk.MaLoiNhanKhan == maLoiNhanKhan
                                                  select lnk).First();
            loiNhanKhan.TieuDe = tieuDe;
            loiNhanKhan.NoiDung = noiDung;
            loiNhanKhan.Ngay = ngay;

            db.SubmitChanges();
        }

        public void UpdateLoiNhanKhan(int maLoiNhanKhan, string noiDung,
            DateTime ngay)
        {
            LoiNhanKhan_LoiNhanKhan loiNhanKhan = (from lnk in db.LoiNhanKhan_LoiNhanKhans
                                                   where lnk.MaLoiNhanKhan == maLoiNhanKhan
                                                   select lnk).First();
            loiNhanKhan.NoiDung = noiDung;
            loiNhanKhan.Ngay = ngay;

            db.SubmitChanges();
        }

        public void UpdateLoiNhanKhan(int maLoiNhanKhan, bool xacNhan)
        {
            LoiNhanKhan_LoiNhanKhan loiNhanKhan = (from lnk in db.LoiNhanKhan_LoiNhanKhans
                                                   where lnk.MaLoiNhanKhan == maLoiNhanKhan
                                                   select lnk).First();
            loiNhanKhan.XacNhan = xacNhan;

            db.SubmitChanges();
        }

        public void DeleteLoiNhanKhan(int maLoiNhanKhan)
        {
            LoiNhanKhan_LoiNhanKhan loiNhanKhan = (from lnk in db.LoiNhanKhan_LoiNhanKhans
                                                   where lnk.MaLoiNhanKhan == maLoiNhanKhan
                                                   select lnk).First();
            db.LoiNhanKhan_LoiNhanKhans.DeleteOnSubmit(loiNhanKhan);
            db.SubmitChanges();

        }

        public LoiNhanKhan_LoiNhanKhan GetLoiNhanKhan(int maLoiNhanKhan)
        {
            IQueryable<LoiNhanKhan_LoiNhanKhan> loiNhanKhans = from lnk in db.LoiNhanKhan_LoiNhanKhans
                                                               where lnk.MaLoiNhanKhan == maLoiNhanKhan
                                                               select lnk;
            if (loiNhanKhans.Count() != 0)
            {
                return loiNhanKhans.First();
            }
            else
            {
                return null;
            }
        }

        public List<TabularLoiNhanKhan> GetListTabularLoiNhanKhan(int maNamHoc, DateTime tuNgay, DateTime denNgay,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularLoiNhanKhan> loiNhanKhans = (from lnk in db.LoiNhanKhan_LoiNhanKhans
                                                          join hs_lh in db.HocSinh_HocSinhLopHocs on lnk.MaHocSinhLopHoc equals hs_lh.MaHocSinhLopHoc
                                                          join lop in db.LopHoc_Lops on hs_lh.MaLopHoc equals lop.MaLopHoc
                                                          join hs in db.HocSinh_ThongTinCaNhans on hs_lh.MaHocSinh equals hs.MaHocSinh
                                                          where lop.MaNamHoc == maNamHoc && lnk.Ngay >= tuNgay && lnk.Ngay <= denNgay
                                                          select new TabularLoiNhanKhan
                                                          {
                                                              MaLoiNhanKhan = lnk.MaLoiNhanKhan,
                                                              MaHocSinh = hs.MaHocSinh,                                                          
                                                              Ngay = lnk.Ngay,
                                                              TieuDe = lnk.TieuDe,
                                                              StrNgay = lnk.Ngay.ToShortDateString(),
                                                              MaHocSinhHienThi = hs.MaHocSinhHienThi,
                                                              TenHocSinh = hs.HoTen,
                                                              XacNhan = (lnk.XacNhan) ? "Có" : "Không"
                                                          }).OrderBy(loiNhan => loiNhan.Ngay);
            totalRecords = loiNhanKhans.Count();
            if (totalRecords != 0)
            {
                List<TabularLoiNhanKhan> lstLoiNhanKhan = loiNhanKhans.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
                return lstLoiNhanKhan;
            }
            else
            {
                return new List<TabularLoiNhanKhan>();
            }
        }

        public List<TabularLoiNhanKhan> GetListTabularLoiNhanKhan(int maNamHoc, DateTime tuNgay, DateTime denNgay,
            bool xacNhan,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularLoiNhanKhan> loiNhanKhans = (from lnk in db.LoiNhanKhan_LoiNhanKhans
                                                          join hs_lh in db.HocSinh_HocSinhLopHocs on lnk.MaHocSinhLopHoc equals hs_lh.MaHocSinhLopHoc
                                                          join lop in db.LopHoc_Lops on hs_lh.MaLopHoc equals lop.MaLopHoc
                                                          join hs in db.HocSinh_ThongTinCaNhans on hs_lh.MaHocSinh equals hs.MaHocSinh
                                                          where lop.MaNamHoc == maNamHoc && lnk.Ngay >= tuNgay && lnk.Ngay <= denNgay
                                                             && lnk.XacNhan == xacNhan
                                                          select new TabularLoiNhanKhan
                                                          {
                                                              MaLoiNhanKhan = lnk.MaLoiNhanKhan,
                                                              MaHocSinh = hs.MaHocSinh,
                                                              Ngay = lnk.Ngay,
                                                              TieuDe = lnk.TieuDe,
                                                              StrNgay = lnk.Ngay.ToShortDateString(),
                                                              MaHocSinhHienThi = hs.MaHocSinhHienThi,
                                                              TenHocSinh = hs.HoTen,
                                                              XacNhan = (lnk.XacNhan) ? "Có" : "Không"
                                                          }).OrderBy(loiNhan => loiNhan.Ngay);
            totalRecords = loiNhanKhans.Count();
            if (totalRecords != 0)
            {
                List<TabularLoiNhanKhan> lstLoiNhanKhan = loiNhanKhans.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
                return lstLoiNhanKhan;
            }
            else
            {
                return new List<TabularLoiNhanKhan>();
            }
        }        

        public List<TabularLoiNhanKhan> GetListTabularLoiNhanKhan(int maNamHoc, DateTime tuNgay, DateTime denNgay,
            string maHocSinhHienThi, bool xacNhan,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularLoiNhanKhan> loiNhanKhans = (from lnk in db.LoiNhanKhan_LoiNhanKhans
                                                          join hs_lh in db.HocSinh_HocSinhLopHocs on lnk.MaHocSinhLopHoc equals hs_lh.MaHocSinhLopHoc
                                                          join lop in db.LopHoc_Lops on hs_lh.MaLopHoc equals lop.MaLopHoc
                                                          join hs in db.HocSinh_ThongTinCaNhans on hs_lh.MaHocSinh equals hs.MaHocSinh
                                                          where lop.MaNamHoc == maNamHoc && lnk.Ngay >= tuNgay && lnk.Ngay <= denNgay
                                                             && hs.MaHocSinhHienThi == maHocSinhHienThi && lnk.XacNhan == xacNhan
                                                          select new TabularLoiNhanKhan
                                                          {
                                                              MaLoiNhanKhan = lnk.MaLoiNhanKhan,
                                                              MaHocSinh = hs.MaHocSinh,
                                                              Ngay = lnk.Ngay,
                                                              TieuDe = lnk.TieuDe,
                                                              StrNgay = lnk.Ngay.ToShortDateString(),
                                                              MaHocSinhHienThi = hs.MaHocSinhHienThi,
                                                              TenHocSinh = hs.HoTen,
                                                              XacNhan = (lnk.XacNhan) ? "Có" : "Không"
                                                          }).OrderBy(loiNhan => loiNhan.Ngay);
            totalRecords = loiNhanKhans.Count();
            if (totalRecords != 0)
            {
                List<TabularLoiNhanKhan> lstLoiNhanKhan = loiNhanKhans.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
                return lstLoiNhanKhan;
            }
            else
            {
                return new List<TabularLoiNhanKhan>();
            }
        }

        public List<TabularLoiNhanKhan> GetListTabularLoiNhanKhan(int maNamHoc, DateTime tuNgay, DateTime denNgay,
            string maHocSinhHienThi,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularLoiNhanKhan> loiNhanKhans = (from lnk in db.LoiNhanKhan_LoiNhanKhans
                                                          join hs_lh in db.HocSinh_HocSinhLopHocs on lnk.MaHocSinhLopHoc equals hs_lh.MaHocSinhLopHoc
                                                          join lop in db.LopHoc_Lops on hs_lh.MaLopHoc equals lop.MaLopHoc
                                                          join hs in db.HocSinh_ThongTinCaNhans on hs_lh.MaHocSinh equals hs.MaHocSinh
                                                          where lop.MaNamHoc == maNamHoc && lnk.Ngay >= tuNgay && lnk.Ngay <= denNgay
                                                             && hs.MaHocSinhHienThi == maHocSinhHienThi
                                                          select new TabularLoiNhanKhan
                                                          {
                                                              MaLoiNhanKhan = lnk.MaLoiNhanKhan,
                                                              MaHocSinh = hs.MaHocSinh,
                                                              Ngay = lnk.Ngay,
                                                              TieuDe = lnk.TieuDe,
                                                              StrNgay = lnk.Ngay.ToShortDateString(),
                                                              MaHocSinhHienThi = hs.MaHocSinhHienThi,
                                                              TenHocSinh = hs.HoTen,
                                                              XacNhan = (lnk.XacNhan) ? "Có" : "Không"
                                                          }).OrderBy(loiNhan => loiNhan.Ngay);
            totalRecords = loiNhanKhans.Count();
            if (totalRecords != 0)
            {
                List<TabularLoiNhanKhan> lstLoiNhanKhan = loiNhanKhans.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
                return lstLoiNhanKhan;
            }
            else
            {
                return new List<TabularLoiNhanKhan>();
            }
        }        
    }
}
