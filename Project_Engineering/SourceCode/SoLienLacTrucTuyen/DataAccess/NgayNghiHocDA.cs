using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class NgayNghiHocDA : BaseDA
    {
        public NgayNghiHocDA()
            : base()
        {
        }

        #region Insert, Update, Delete Methods
        public void InsertNgayNghiHoc(int maHocSinh, int maLopHoc,
            int maHocKy, DateTime ngay, int maBuoi,
            bool xinPhep, string lyDo)
        {
            HocSinh_HocSinhLopHoc currentHSLop = (from hsLop in db.HocSinh_HocSinhLopHocs
                                                  where hsLop.MaHocSinh == maHocSinh
                                                    && hsLop.MaLopHoc == maLopHoc
                                                  select hsLop).First();

            HocSinh_NgayNghiHoc ngayNghiHoc = new HocSinh_NgayNghiHoc
            {
                MaHocSinhLopHoc = currentHSLop.MaHocSinhLopHoc,
                MaHocKy = maHocKy,
                Ngay = ngay,
                MaBuoi = maBuoi,
                XinPhep = xinPhep,
                LyDo = lyDo
            };

            db.HocSinh_NgayNghiHocs.InsertOnSubmit(ngayNghiHoc);
            db.SubmitChanges();
        }

        public void UpdateNgayNghiHoc(int maNgayNghiHoc,
            int maHocKy, DateTime ngay, int maBuoi, bool xinPhep, string lyDo)
        {
            HocSinh_NgayNghiHoc ngayNghiHoc = (from ngayNghi in db.HocSinh_NgayNghiHocs
                                               where ngayNghi.MaNgayNghiHoc == maNgayNghiHoc
                                               select ngayNghi).First();
            ngayNghiHoc.MaHocKy = maHocKy;
            ngayNghiHoc.Ngay = ngay;
            ngayNghiHoc.MaBuoi = maBuoi;
            ngayNghiHoc.XinPhep = xinPhep;
            ngayNghiHoc.LyDo = lyDo;

            db.SubmitChanges();
        }

        public void UpdateNgayNghiHoc(int maNgayNghiHoc, bool xacNhan)
        {
            HocSinh_NgayNghiHoc ngayNghiHoc = (from ngayNghi in db.HocSinh_NgayNghiHocs
                                               where ngayNghi.MaNgayNghiHoc == maNgayNghiHoc
                                               select ngayNghi).First();
            ngayNghiHoc.XacNhan = xacNhan;
            db.SubmitChanges();
        }

        public void DeleteNgayNghiHoc(int maNgayNghiHoc)
        {
            HocSinh_NgayNghiHoc ngayNghiHoc = (from ngayNghi in db.HocSinh_NgayNghiHocs
                                               where ngayNghi.MaNgayNghiHoc == maNgayNghiHoc
                                               select ngayNghi).First();
            db.HocSinh_NgayNghiHocs.DeleteOnSubmit(ngayNghiHoc);
            db.SubmitChanges();
        }
        #endregion

        #region Get Entity, List
        public HocSinh_NgayNghiHoc GetNgayNghiHoc(int maNgayNghiHoc)
        {
            IQueryable<HocSinh_NgayNghiHoc> ngayNghiHocs = from ngayNghi in db.HocSinh_NgayNghiHocs
                                                           where ngayNghi.MaNgayNghiHoc == maNgayNghiHoc
                                                           select ngayNghi;
            if (ngayNghiHocs.Count() != 0)
            {
                return ngayNghiHocs.First();
            }
            else
            {
                return null;
            }
        }

        public HocSinh_NgayNghiHoc GetNgayNghiHoc(int maHocSinh,
            int maNamHoc, int maHocKy, DateTime ngay)
        {
            IQueryable<HocSinh_NgayNghiHoc> ngayNghiHocs;
            ngayNghiHocs = from ngayNghi in db.HocSinh_NgayNghiHocs
                           join hocSinhLopHoc in db.HocSinh_HocSinhLopHocs
                                on ngayNghi.MaHocSinhLopHoc equals hocSinhLopHoc.MaHocSinhLopHoc
                           join lop in db.LopHoc_Lops
                                on hocSinhLopHoc.MaLopHoc equals lop.MaLopHoc
                           where hocSinhLopHoc.MaHocSinh == maHocSinh
                                && lop.MaNamHoc == maNamHoc
                                && ngayNghi.MaHocKy == maHocKy
                                && ngayNghi.Ngay == ngay
                           select ngayNghi;
            if (ngayNghiHocs.Count() != 0)
            {
                return ngayNghiHocs.First();
            }
            else
            {
                return null;
            }
        }

        public List<TabularDayOff> GetListTabularDayOffs(int maHocSinh,
            int maNamHoc, int maHocKy,
            DateTime tuNgay, DateTime denNgay,
            int pageCurrentIndex, int pageSize,
            out double totalRecords)
        {
            BuoiDA buoiDA = new BuoiDA();
            IQueryable<TabularDayOff> iqTbNgayNghiHoc;

            iqTbNgayNghiHoc = from ngayNghi in db.HocSinh_NgayNghiHocs
                              join hocSinhLopHoc in db.HocSinh_HocSinhLopHocs
                                 on ngayNghi.MaHocSinhLopHoc equals hocSinhLopHoc.MaHocSinhLopHoc
                              join lop in db.LopHoc_Lops
                                 on hocSinhLopHoc.MaLopHoc equals lop.MaLopHoc
                              where hocSinhLopHoc.MaHocSinh == maHocSinh
                                 && lop.MaNamHoc == maNamHoc && ngayNghi.MaHocKy == maHocKy
                                 && ngayNghi.Ngay >= tuNgay && ngayNghi.Ngay <= denNgay
                              select new TabularDayOff
                              {
                                  MaNgayNghiHoc = ngayNghi.MaNgayNghiHoc,
                                  MaHocSinhLopHoc = hocSinhLopHoc.MaHocSinhLopHoc,
                                  Ngay = ngayNghi.Ngay.Day + "/" + ngayNghi.Ngay.Month + "/" + ngayNghi.Ngay.Year,
                                  Buoi = buoiDA.GetSessionName(ngayNghi.MaBuoi),
                                  XinPhep = (ngayNghi.XinPhep) ? "Có" : "Không",
                                  LyDo = ngayNghi.LyDo,
                                  XacNhan = (ngayNghi.XacNhan) ? "Có" : "Không"
                              };

            
            totalRecords = iqTbNgayNghiHoc.Count();
            if (totalRecords != 0)
            {
                return iqTbNgayNghiHoc.Skip((pageCurrentIndex - 1) * pageSize)
                    .Take(pageSize).OrderBy(n => n.Ngay).ToList();
            }
            else
            {
                return new List<TabularDayOff>();
            }
        }
        #endregion

        public HocSinh_NgayNghiHoc GetNgayNghiHoc(int maNgayNghiHoc, int maHocSinh,
            int maNamHoc, int maHocKy, DateTime ngay)
        {
            IQueryable<HocSinh_NgayNghiHoc> ngayNghiHocs;
            ngayNghiHocs = from ngayNghi in db.HocSinh_NgayNghiHocs
                           join hocSinhLopHoc in db.HocSinh_HocSinhLopHocs
                                on ngayNghi.MaHocSinhLopHoc equals hocSinhLopHoc.MaHocSinhLopHoc
                           join lop in db.LopHoc_Lops
                                on hocSinhLopHoc.MaLopHoc equals lop.MaLopHoc
                           where ngayNghi.MaNgayNghiHoc != maNgayNghiHoc
                                && hocSinhLopHoc.MaHocSinh == maHocSinh
                                && lop.MaNamHoc == maNamHoc
                                && ngayNghi.MaHocKy == maHocKy
                                && ngayNghi.Ngay == ngay
                           select ngayNghi;
            if (ngayNghiHocs.Count() != 0)
            {
                return ngayNghiHocs.First();
            }
            else
            {
                return null;
            }
        }

        public bool Confirmed(int maNgayNghiHoc)
        {
            bool confirmed = (from ngayNghiHoc in db.HocSinh_NgayNghiHocs
                              where ngayNghiHoc.MaNgayNghiHoc == maNgayNghiHoc
                              select ngayNghiHoc.XacNhan).First();
            return confirmed;
        }

        public bool NgayNghiHocExists(int maHocSinh, int maLopHoc,
            int maHocKy, DateTime ngay)
        {
            IQueryable<HocSinh_NgayNghiHoc> ngayNghiHocs;
            ngayNghiHocs = from ngayNghi in db.HocSinh_NgayNghiHocs
                           join hocSinhLopHoc in db.HocSinh_HocSinhLopHocs
                                on ngayNghi.MaHocSinhLopHoc equals hocSinhLopHoc.MaHocSinhLopHoc
                           where hocSinhLopHoc.MaHocSinh == maHocSinh
                                && hocSinhLopHoc.MaLopHoc == maLopHoc
                                && ngayNghi.MaHocKy == maHocKy
                                && ngayNghi.Ngay == ngay
                           select ngayNghi;
            if (ngayNghiHocs.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool NgayNghiHocExists(int maHocSinh, int maLopHoc,
            int maHocKy, DateTime ngay, int maBuoi)
        {
            IQueryable<HocSinh_NgayNghiHoc> ngayNghiHocs;
            ngayNghiHocs = from ngayNghi in db.HocSinh_NgayNghiHocs
                           join hocSinhLopHoc in db.HocSinh_HocSinhLopHocs
                                on ngayNghi.MaHocSinhLopHoc equals hocSinhLopHoc.MaHocSinhLopHoc
                           where hocSinhLopHoc.MaHocSinh == maHocSinh
                                && hocSinhLopHoc.MaLopHoc == maLopHoc
                                && ngayNghi.MaHocKy == maHocKy
                                && ngayNghi.Ngay == ngay
                                && ngayNghi.MaBuoi == maBuoi
                           select ngayNghi;
            if (ngayNghiHocs.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool NgayNghiHocExists(int maNgayNghiHoc,
            int maHocSinh, int maLopHoc,
            int maHocKy, DateTime ngay, int maBuoi)
        {
            IQueryable<HocSinh_NgayNghiHoc> ngayNghiHocs;
            ngayNghiHocs = from ngayNghi in db.HocSinh_NgayNghiHocs
                           join hocSinhLopHoc in db.HocSinh_HocSinhLopHocs
                                on ngayNghi.MaHocSinhLopHoc equals hocSinhLopHoc.MaHocSinhLopHoc
                           where ngayNghi.MaNgayNghiHoc != maNgayNghiHoc
                                && hocSinhLopHoc.MaHocSinh == maHocSinh
                                && hocSinhLopHoc.MaLopHoc == maLopHoc
                                && ngayNghi.MaHocKy == maHocKy
                                && ngayNghi.Ngay == ngay
                                && ngayNghi.MaBuoi == maBuoi
                           select ngayNghi;
            if (ngayNghiHocs.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool NgayNghiHocExists(int maNgayNghiHoc,
            int maHocSinh, int maLopHoc,
            int maHocKy, DateTime ngay)
        {
            IQueryable<HocSinh_NgayNghiHoc> ngayNghiHocs;
            ngayNghiHocs = from ngayNghi in db.HocSinh_NgayNghiHocs
                           join hocSinhLopHoc in db.HocSinh_HocSinhLopHocs
                                on ngayNghi.MaHocSinhLopHoc equals hocSinhLopHoc.MaHocSinhLopHoc
                           where ngayNghi.MaNgayNghiHoc != maNgayNghiHoc
                                && hocSinhLopHoc.MaHocSinh == maHocSinh
                                && hocSinhLopHoc.MaLopHoc == maLopHoc
                                && ngayNghi.MaHocKy == maHocKy
                                && ngayNghi.Ngay == ngay
                           select ngayNghi;
            if (ngayNghiHocs.Count() != 0)
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
