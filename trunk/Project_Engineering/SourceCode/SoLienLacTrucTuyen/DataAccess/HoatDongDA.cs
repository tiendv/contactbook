using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class HoatDongDA : BaseDA
    {
        public HoatDongDA()
            : base()
        {
        }

        #region Insert, Update, Delete
        public void InsertHoatDong(int maHocSinh, int maHocKy, DateTime ngay,
            string tenHoatDong, string moTa, int? maThaiDoThamGia)
        {
            HocSinh_HocSinhLopHoc hocSinhLop = (from hsLop in db.HocSinh_HocSinhLopHocs
                                                where hsLop.MaHocSinh == maHocSinh
                                                select hsLop).OrderByDescending(hsLop => hsLop.MaHocSinhLopHoc).First();

            HocSinh_HoatDong HoatDong = new HocSinh_HoatDong
            {
                MaHocSinhLopHoc = hocSinhLop.MaHocSinhLopHoc,
                MaHocKy = maHocKy,
                Ngay = ngay,
                TieuDe = tenHoatDong,
                NoiDung = moTa,
                MaThaiDoThamGia = maThaiDoThamGia
            };

            db.HocSinh_HoatDongs.InsertOnSubmit(HoatDong);
            db.SubmitChanges();
        }

        public void UpdateHoatDong(int maHoatDong, DateTime ngay,
            string moTa, int? maThaiDoThamGia)
        {
            HocSinh_HoatDong hoatDong = (from hd in db.HocSinh_HoatDongs
                                         where hd.MaHoatDong == maHoatDong
                                         select hd).First();
            hoatDong.Ngay = ngay;
            hoatDong.NoiDung = moTa;
            hoatDong.MaThaiDoThamGia = maThaiDoThamGia;

            db.SubmitChanges();
        }

        public void DeleteHoatDong(int maHoatDong)
        {
            HocSinh_HoatDong hoatDong = (from hd in db.HocSinh_HoatDongs
                                         where hd.MaHoatDong == maHoatDong
                                         select hd).First();

            db.HocSinh_HoatDongs.DeleteOnSubmit(hoatDong);
            db.SubmitChanges();
        }
        #endregion

        #region Get HoatDong
        public HocSinh_HoatDong GetHoatDong(int maHoatDong)
        {
            IQueryable<HocSinh_HoatDong> hoatDongs = from hd in db.HocSinh_HoatDongs
                                                     where hd.MaHoatDong == maHoatDong
                                                     select hd;
            if (hoatDongs.Count() != 0)
            {
                return hoatDongs.First();
            }
            else
            {
                return null;
            }
        }

        public HocSinh_HoatDong GetHoatDong(int maHocSinh, int maNamHoc, int maHocKy,
            DateTime ngay)
        {
            IQueryable<HocSinh_HoatDong> hoatDongs = from hd in db.HocSinh_HoatDongs
                                                     join hocSinhLopHoc in db.HocSinh_HocSinhLopHocs on hd.MaHocSinhLopHoc equals hocSinhLopHoc.MaHocSinhLopHoc
                                                     join lop in db.LopHoc_Lops on hocSinhLopHoc.MaLopHoc equals lop.MaLopHoc
                                                     where hocSinhLopHoc.MaHocSinh == maHocSinh && lop.MaNamHoc == maNamHoc
                                                        && hd.MaHocKy == maHocKy && hd.Ngay == ngay
                                                     select hd;
            if (hoatDongs.Count() != 0)
            {
                return hoatDongs.First();
            }
            else
            {
                return null;
            }
        }

        public HocSinh_HoatDong GetHoatDong(int maHoatDong, int maHocSinh, int maNamHoc, int maHocKy,
            DateTime ngay)
        {
            IQueryable<HocSinh_HoatDong> hoatDongs = from ngayNghi in db.HocSinh_HoatDongs
                                                     join hocSinhLopHoc in db.HocSinh_HocSinhLopHocs on ngayNghi.MaHocSinhLopHoc equals hocSinhLopHoc.MaHocSinhLopHoc
                                                     join lop in db.LopHoc_Lops on hocSinhLopHoc.MaLopHoc equals lop.MaLopHoc
                                                     where hocSinhLopHoc.MaHocSinh == maHocSinh && lop.MaNamHoc == maNamHoc
                                                        && ngayNghi.MaHocKy == maHocKy && ngayNghi.Ngay == ngay
                                                        && ngayNghi.MaHoatDong == maHoatDong
                                                     select ngayNghi;
            if (hoatDongs.Count() != 0)
            {
                return hoatDongs.First();
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Get List TabularHoatDong
        public List<TabularHoatDong> GetListTabularHoatDong(int maHocSinh, int maNamHoc, int maHocKy,
            DateTime tuNgay, DateTime denNgay,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            ThaiDoThamGiaDA thaiDoThamGiaDA = new ThaiDoThamGiaDA();
            
            //IQueryable<TabularHoatDong> tabularHoatDongs = from hoatDong in db.HocSinh_HoatDongs
            //                                               join hsLop in db.HocSinh_HocSinhLopHocs 
            //                                                    on hoatDong.MaHocSinhLopHoc equals hsLop.MaHocSinhLopHoc
            //                                               join lop in db.LopHoc_Lops 
            //                                                    on hsLop.MaLopHoc equals lop.MaLopHoc
            //                                               where hsLop.MaHocSinh == maHocSinh
            //                                                    && hoatDong.MaHocKy == maHocKy
            //                                                    && hoatDong.Ngay >= tuNgay && hoatDong.Ngay <= denNgay
            //                                               select new TabularHoatDong
            //                                               {
            //                                                   MaHoatDong = hoatDong.MaHoatDong,
            //                                                   MaHocSinhLopHoc = hsLop.MaHocSinhLopHoc,
            //                                                   Ngay = hoatDong.Ngay,
            //                                                   TenHoatDong = hoatDong.TieuDe,
            //                                                   StrNgay = hoatDong.Ngay.ToShortDateString(),
            //                                                   ThaiDoThamGia = (hoatDong.MaThaiDoThamGia != null) ? 
            //                                                    thaiDoThamGiaDA.GetThaiDoThamGia((int)hoatDong.MaThaiDoThamGia).TenThaiDoThamGia: "Không xác định"
            //                                               };

            IQueryable<TabularHoatDong> tabularHoatDongs = from hoatDong in db.HocSinh_HoatDongs
                                                           join hsLop in db.HocSinh_HocSinhLopHocs
                                                                on hoatDong.MaHocSinhLopHoc equals hsLop.MaHocSinhLopHoc
                                                           join lop in db.LopHoc_Lops 
                                                                on hsLop.MaLopHoc equals lop.MaLopHoc
                                                           where hsLop.MaHocSinh == maHocSinh 
                                                                && lop.MaNamHoc == maNamHoc && hoatDong.MaHocKy == maHocKy
                                                                && hoatDong.Ngay >= tuNgay && hoatDong.Ngay <= denNgay
                                                           select new TabularHoatDong
                                                           {
                                                               MaHoatDong = hoatDong.MaHoatDong,
                                                               MaHocSinhLopHoc = hsLop.MaHocSinhLopHoc,
                                                               Ngay = hoatDong.Ngay,
                                                               TenHoatDong = hoatDong.TieuDe,
                                                               StrNgay = hoatDong.Ngay.ToShortDateString(),
                                                               MaThaiDoThamGia = hoatDong.MaThaiDoThamGia
                                                           };
            totalRecords = tabularHoatDongs.Count();
            if (totalRecords != 0)
            {
                return tabularHoatDongs.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();                    
            }
            else
            {
                return new List<TabularHoatDong>();
            }
        }
        #endregion

        public bool HoatDongExists(string tieuDe, int maHocSinh, 
            int maLopHoc, int maHocKy, DateTime ngay)
        {
            IQueryable<HocSinh_HoatDong> hoatDongs;
            hoatDongs = from hoatDong in db.HocSinh_HoatDongs
                        join hsLop in db.HocSinh_HocSinhLopHocs 
                            on hoatDong.MaHocSinhLopHoc equals hsLop.MaHocSinhLopHoc
                        join lop in db.LopHoc_Lops 
                            on hsLop.MaLopHoc equals lop.MaLopHoc
                        where hoatDong.TieuDe == tieuDe
                            && lop.MaLopHoc == maLopHoc
                            && hoatDong.MaHocKy == maHocKy 
                            && hoatDong.Ngay == ngay
                        select hoatDong;
            if (hoatDongs.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HoatDongExists(int maHoatDong, string tieuDe, int maHocSinh,
            int maLopHoc, int maHocKy, DateTime ngay)
        {
            IQueryable<HocSinh_HoatDong> hoatDongs;
            hoatDongs = from hoatDong in db.HocSinh_HoatDongs
                        join hsLop in db.HocSinh_HocSinhLopHocs
                            on hoatDong.MaHocSinhLopHoc equals hsLop.MaHocSinhLopHoc
                        join lop in db.LopHoc_Lops
                            on hsLop.MaLopHoc equals lop.MaLopHoc
                        where hoatDong.MaHoatDong != maHoatDong
                            && hoatDong.TieuDe == tieuDe 
                            && lop.MaLopHoc == maLopHoc
                            && hoatDong.MaHocKy == maHocKy
                            && hoatDong.Ngay == ngay
                        select hoatDong;
            if (hoatDongs.Count() != 0)
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
