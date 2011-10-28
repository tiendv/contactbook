using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class ThongBaoLopDA : BaseDA
    {
        public ThongBaoLopDA()
            : base()
        {
        }

        #region Insert, Update, Delete
        public void InsertThongBaoLop(int maLopHoc, string tieuDe, string noiDung,
            int maHocKy, DateTime ngay, bool apDungHoatDong, bool apDungLoiNhanKhan)
        {
            LopHoc_ThongBao thongBaoLop = new LopHoc_ThongBao
            {
                MaLopHoc = maLopHoc,
                TieuDe = tieuDe,
                NoiDung = noiDung,
                Ngay = ngay
            };

            db.LopHoc_ThongBaos.InsertOnSubmit(thongBaoLop);
            db.SubmitChanges();

            int? lastedMaThongBao = GetLastedMaThongBao();
            if(lastedMaThongBao == null)
            {
                return;
            }

            if (apDungHoatDong || apDungLoiNhanKhan)
            {
                IQueryable<HocSinh_HocSinhLopHoc> hsLops;
                hsLops = from hsLop in db.HocSinh_HocSinhLopHocs
                         where hsLop.MaLopHoc == maLopHoc
                         select hsLop;

                if (hsLops.Count() != 0)
                {
                    foreach (HocSinh_HocSinhLopHoc hsLop in hsLops)
                    {
                        if (apDungHoatDong)
                        {
                            HocSinh_HoatDong hoatDong = new HocSinh_HoatDong
                            {
                                MaHocSinhLopHoc = hsLop.MaHocSinhLopHoc,
                                TieuDe = tieuDe,
                                NoiDung = noiDung,
                                Ngay = ngay,
                                MaThongBaoLop = lastedMaThongBao,
                                MaHocKy = maHocKy
                            };
                            db.HocSinh_HoatDongs.InsertOnSubmit(hoatDong);
                        }

                        if (apDungLoiNhanKhan)
                        {
                            LoiNhanKhan_LoiNhanKhan loiNhanKhan = new LoiNhanKhan_LoiNhanKhan
                            {
                                MaHocSinhLopHoc = hsLop.MaHocSinhLopHoc,
                                TieuDe = tieuDe,
                                NoiDung = noiDung,
                                Ngay = ngay,
                                MaThongBaoLop = lastedMaThongBao,
                                XacNhan = false
                            };
                            db.LoiNhanKhan_LoiNhanKhans.InsertOnSubmit(loiNhanKhan);
                        }
                    }
                    db.SubmitChanges();
                }
            }
        }

        public void UpdateThongBaoLop(int maThongBaoLop, string tieuDe, string noiDung,
            DateTime ngay)
        {
            LopHoc_ThongBao thongBao = (from thBao in db.LopHoc_ThongBaos
                                        where thBao.MaThongBaoLop == maThongBaoLop
                                        select thBao).First();
            thongBao.TieuDe = tieuDe;
            thongBao.NoiDung = noiDung;
            thongBao.Ngay = ngay;

            db.SubmitChanges();
        }

        public void DeleteLopHoc(int maThongBaoLop)
        {
            LopHoc_ThongBao thongBao = (from thBao in db.LopHoc_ThongBaos
                                        where thBao.MaThongBaoLop == maThongBaoLop
                                        select thBao).First();
            db.LopHoc_ThongBaos.DeleteOnSubmit(thongBao);
            db.SubmitChanges();
        }
        #endregion

        public LopHoc_ThongBao GetThongBaoLop(int maThongBao)
        {
            IQueryable<LopHoc_ThongBao> thongBaos = from thBao in db.LopHoc_ThongBaos
                                                    where thBao.MaThongBaoLop == maThongBao
                                                    select thBao;
            if (thongBaos.Count() != 0)
            {
                return thongBaos.First();
            }
            else
            {
                return null;
            }
        }

        public List<TabularThongBaoLop> GetListTabularThongBaoLop(int maNamHoc, 
            DateTime tuNgay, DateTime denNgay,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            //IQueryable<TabularThongBaoLop> thongBaos;
            //thongBaos = (from thBao in db.LopHoc_ThongBaos
            //             join lopHoc in db.LopHoc_Lops 
            //                on thBao.MaLopHoc equals lopHoc.MaLopHoc
            //             where lopHoc.MaNamHoc == maNamHoc 
            //                && thBao.Ngay >= tuNgay 
            //                && thBao.Ngay <= denNgay
            //             select new TabularThongBaoLop
            //             {
            //                 MaThongBaoLop = thBao.MaThongBaoLop,
            //                 Ngay = thBao.Ngay,
            //                 TieuDe = thBao.TieuDe,
            //                 StrNgay = thBao.Ngay.ToShortDateString(),
            //                 TenHocSinh = hocSinh.HoTen,
            //                 XacNhan = (thBao.XacNhan) ? "Có" : "Không"
            //             }).OrderBy(loiNhan => loiNhan.Ngay);

            //totalRecords = thongBaos.Count();
            //if (totalRecords != 0)
            //{
            //    List<TabularThongBaoLop> lstThongBaoLop = thongBaos.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            //    return lstThongBaoLop;
            //}
            //else
            //{
            //    return new List<TabularThongBaoLop>();
            //}

            totalRecords = 0;
            return new List<TabularThongBaoLop>();
        }

        public List<TabularThongBaoLop> GetListTabularThongBaoLop(int maNamHoc, DateTime tuNgay, DateTime denNgay,
            bool xacNhan,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            totalRecords = 0;
            return new List<TabularThongBaoLop>();

            //IQueryable<TabularThongBaoLop> ThongBaoLops = (from lnk in db.LopHoc_ThongBaoLops
            //                                               join hs_lh in db.HocSinh_HocSinhLopHocs on lnk.MaHocSinhLopHoc equals hs_lh.MaHocSinhLopHoc
            //                                               join lop in db.LopHoc_Lops on hs_lh.MaLopHoc equals lop.MaLopHoc
            //                                               join hs in db.HocSinh_ThongTinCaNhans on hs_lh.MaHocSinh equals hs.MaHocSinh
            //                                               where lop.MaNamHoc == maNamHoc && lnk.Ngay >= tuNgay && lnk.Ngay <= denNgay
            //                                                  && lnk.XacNhan == xacNhan
            //                                               select new TabularThongBaoLop
            //                                               {
            //                                                   MaThongBaoLop = lnk.MaThongBaoLop,
            //                                                   MaHocSinh = hs.MaHocSinh,
            //                                                   Ngay = lnk.Ngay,
            //                                                   TieuDe = lnk.TieuDe,
            //                                                   StrNgay = lnk.Ngay.ToShortDateString(),
            //                                                   MaHocSinhHienThi = hs.MaHocSinhHienThi,
            //                                                   TenHocSinh = hs.HoTen,
            //                                                   XacNhan = (lnk.XacNhan) ? "Có" : "Không"
            //                                               }).OrderBy(loiNhan => loiNhan.Ngay);
            //totalRecords = ThongBaoLops.Count();
            //if (totalRecords != 0)
            //{
            //    List<TabularThongBaoLop> lstThongBaoLop = ThongBaoLops.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            //    return lstThongBaoLop;
            //}
            //else
            //{
            //    return new List<TabularThongBaoLop>();
            //}
        }

        public List<TabularThongBaoLop> GetListTabularThongBaoLop(int maNamHoc, DateTime tuNgay, DateTime denNgay,
            string maHocSinhHienThi, bool xacNhan,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            totalRecords = 0;
            return new List<TabularThongBaoLop>();

            //IQueryable<TabularThongBaoLop> ThongBaoLops = (from lnk in db.LopHoc_ThongBaoLops
            //                                               join hs_lh in db.HocSinh_HocSinhLopHocs on lnk.MaHocSinhLopHoc equals hs_lh.MaHocSinhLopHoc
            //                                               join lop in db.LopHoc_Lops on hs_lh.MaLopHoc equals lop.MaLopHoc
            //                                               join hs in db.HocSinh_ThongTinCaNhans on hs_lh.MaHocSinh equals hs.MaHocSinh
            //                                               where lop.MaNamHoc == maNamHoc && lnk.Ngay >= tuNgay && lnk.Ngay <= denNgay
            //                                                  && hs.MaHocSinhHienThi == maHocSinhHienThi && lnk.XacNhan == xacNhan
            //                                               select new TabularThongBaoLop
            //                                               {
            //                                                   MaThongBaoLop = lnk.MaThongBaoLop,
            //                                                   MaHocSinh = hs.MaHocSinh,
            //                                                   Ngay = lnk.Ngay,
            //                                                   TieuDe = lnk.TieuDe,
            //                                                   StrNgay = lnk.Ngay.ToShortDateString(),
            //                                                   MaHocSinhHienThi = hs.MaHocSinhHienThi,
            //                                                   TenHocSinh = hs.HoTen,
            //                                                   XacNhan = (lnk.XacNhan) ? "Có" : "Không"
            //                                               }).OrderBy(loiNhan => loiNhan.Ngay);
            //totalRecords = ThongBaoLops.Count();
            //if (totalRecords != 0)
            //{
            //    List<TabularThongBaoLop> lstThongBaoLop = ThongBaoLops.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            //    return lstThongBaoLop;
            //}
            //else
            //{
            //    return new List<TabularThongBaoLop>();
            //}
        }

        public List<TabularThongBaoLop> GetListTabularThongBaoLop(int maNamHoc, DateTime tuNgay, DateTime denNgay,
            string maHocSinhHienThi,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            totalRecords = 0;
            return new List<TabularThongBaoLop>();

            //IQueryable<TabularThongBaoLop> ThongBaoLops = (from lnk in db.LopHoc_ThongBaoLops
            //                                               join hs_lh in db.HocSinh_HocSinhLopHocs on lnk.MaHocSinhLopHoc equals hs_lh.MaHocSinhLopHoc
            //                                               join lop in db.LopHoc_Lops on hs_lh.MaLopHoc equals lop.MaLopHoc
            //                                               join hs in db.HocSinh_ThongTinCaNhans on hs_lh.MaHocSinh equals hs.MaHocSinh
            //                                               where lop.MaNamHoc == maNamHoc && lnk.Ngay >= tuNgay && lnk.Ngay <= denNgay
            //                                                  && hs.MaHocSinhHienThi == maHocSinhHienThi
            //                                               select new TabularThongBaoLop
            //                                               {
            //                                                   MaThongBaoLop = lnk.MaThongBaoLop,
            //                                                   MaHocSinh = hs.MaHocSinh,
            //                                                   Ngay = lnk.Ngay,
            //                                                   TieuDe = lnk.TieuDe,
            //                                                   StrNgay = lnk.Ngay.ToShortDateString(),
            //                                                   MaHocSinhHienThi = hs.MaHocSinhHienThi,
            //                                                   TenHocSinh = hs.HoTen,
            //                                                   XacNhan = (lnk.XacNhan) ? "Có" : "Không"
            //                                               }).OrderBy(loiNhan => loiNhan.Ngay);
            //totalRecords = ThongBaoLops.Count();
            //if (totalRecords != 0)
            //{
            //    List<TabularThongBaoLop> lstThongBaoLop = ThongBaoLops.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            //    return lstThongBaoLop;
            //}
            //else
            //{
            //    return new List<TabularThongBaoLop>();
            //}
        }

        public void UpdateThongBao(int maThongBaoLop, bool xacNhan)
        {
            //LopHoc_ThongBao thongBao = (from thBao in db.LopHoc_ThongBaos
            //                            where thBao.MaThongBaoLop == maThongBaoLop
            //                            select thBao).First();
            //thongBao.xa = xacNhan;

            //db.SubmitChanges();
        }

        public int? GetLastedMaThongBao()
        {
            IQueryable<LopHoc_ThongBao> thongBaos;
            thongBaos = from thongBao in db.LopHoc_ThongBaos
                        select thongBao;

            if (thongBaos.Count() != 0)
            {
                thongBaos = thongBaos.OrderByDescending(thongBao => thongBao.MaThongBaoLop);
                return thongBaos.First().MaThongBaoLop;
            }
            else
            {
                return null;
            }
        }
    }
}
