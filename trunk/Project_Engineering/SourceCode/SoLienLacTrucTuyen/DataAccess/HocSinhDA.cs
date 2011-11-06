using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class HocSinhDA : BaseDA
    {
        public HocSinhDA()
            : base()
        {

        }

        public void InsertHocSinh(int maLopHoc, string maHocSinhHienThi, string tenHocSinh,
            bool gioiTinhHocSinh, DateTime ngaySinhHocSinh, string noiSinhHocSinh,
            string diaChi, string dienThoai,
            string hoTenBo, string ngheNghiepBo, DateTime? ngaySinhBo,
            string hoTenMe, string ngheNghiepMe, DateTime? ngaySinhMe,
            string hoTenNguoiDoDau, string ngheNghiepNguoiDoDau, DateTime? ngaySinhNguoiDoDau)
        {
            // Insert HocSinh_ThongTinCaNhan
            HocSinh_ThongTinCaNhan thongTinCaNhan = new HocSinh_ThongTinCaNhan
            {
                MaHocSinhHienThi = maHocSinhHienThi,
                HoTen = tenHocSinh,
                GioiTinh = gioiTinhHocSinh,
                NgaySinh = ngaySinhHocSinh,
                NoiSinh = noiSinhHocSinh,
                DiaChi = diaChi,
                DienThoai = dienThoai,
                HoTenBo = hoTenBo,
                NgheNghiepBo = ngheNghiepBo,
                NgaySinhBo = ngaySinhBo,
                HoTenMe = hoTenMe,
                NgheNghiepMe = ngheNghiepMe,
                NgaySinhMe = ngaySinhMe,
                HoTenNguoiDoDau = hoTenNguoiDoDau,
                NgheNghiepNguoiDoDau = ngheNghiepNguoiDoDau,
                NgaySinhNguoiDoDau = ngaySinhNguoiDoDau
            };

            db.HocSinh_ThongTinCaNhans.InsertOnSubmit(thongTinCaNhan);
            db.SubmitChanges();

            // Insert HocSinh_HocSinhLopHoc
            int maHocSinh = GetMaHocSinh(maHocSinhHienThi);
            db.HocSinh_HocSinhLopHocs.InsertOnSubmit(new HocSinh_HocSinhLopHoc
            {
                MaLopHoc = maLopHoc,
                MaHocSinh = maHocSinh
            });
            db.SubmitChanges();

            // Update LopHoc_Lop
            LopHoc_Lop lopHoc = (from lop in db.LopHoc_Lops
                                 where lop.MaLopHoc == maLopHoc
                                 select lop).First();
            lopHoc.SiSo++;
            db.SubmitChanges();

            // Insert HocSinh_DanhHieuHocKy
            int lastedMaHocSinhLopHoc = GetLastedMaHocSinhLopHoc();
            if (lastedMaHocSinhLopHoc != 0)
            {
                HocKyDA hocKyDA = new HocKyDA();
                List<CauHinh_HocKy> lstHocKy = hocKyDA.GetListHocKy();
                foreach (CauHinh_HocKy hocKy in lstHocKy)
                {
                    HocSinh_DanhHieuHocKy danhHieu = new HocSinh_DanhHieuHocKy
                    {
                        MaHocKy = hocKy.MaHocKy,
                        MaHocSinhLopHoc = lastedMaHocSinhLopHoc,
                        DiemTBHK = -1,
                        MaHanhKiemHK = -1,
                        MaHocLucHK = -1
                    };
                    db.HocSinh_DanhHieuHocKies.InsertOnSubmit(danhHieu);
                    db.SubmitChanges();
                }
            }
        }

        public void DeleteHocSinh(int maHocSinh)
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

        public int GetMaHocSinh(string maHocSinhHienThi)
        {
            IQueryable<HocSinh_ThongTinCaNhan> hocSinhs = from hs in db.HocSinh_ThongTinCaNhans
                                                          where hs.MaHocSinhHienThi == maHocSinhHienThi
                                                          select hs;
            if (hocSinhs.Count() != 0)
            {
                return hocSinhs.First().MaHocSinh;
            }
            else
            {
                return 0;
            }
        }

        public HocSinh_ThongTinCaNhan GetThongTinCaNhan(int maHocSinh)
        {
            IQueryable<HocSinh_ThongTinCaNhan> hocSinhs = from hs in db.HocSinh_ThongTinCaNhans
                                                          where hs.MaHocSinh == maHocSinh
                                                          select hs;
            if (hocSinhs.Count() != 0)
            {
                return hocSinhs.First();
            }
            else
            {
                return null;
            }
        }

        #region Get list TabularHocSinhInfo
        public List<TabularHocSinhInfo> GetListTabularHocSinhInfo(string maHocSinhHienThi,
            out double totalRecords)
        {
            totalRecords = 0;
            IQueryable<TabularHocSinhInfo> tabularHocSinhInfo;
            tabularHocSinhInfo = from hocSinh_Lop in db.HocSinh_HocSinhLopHocs
                                 join lopHoc in db.LopHoc_Lops on hocSinh_Lop.MaLopHoc equals lopHoc.MaLopHoc
                                 join nganh in db.DanhMuc_NganhHocs on lopHoc.MaNganhHoc equals nganh.MaNganhHoc
                                 join khoi in db.DanhMuc_KhoiLops on lopHoc.MaKhoiLop equals khoi.MaKhoiLop
                                 join hocSinh in db.HocSinh_ThongTinCaNhans on hocSinh_Lop.MaHocSinh equals hocSinh.MaHocSinh
                                 where hocSinh.MaHocSinhHienThi == maHocSinhHienThi
                                 select new TabularHocSinhInfo
                                 {
                                     MaHocSinh = hocSinh.MaHocSinh,
                                     MaHocSinhHienThi = hocSinh.MaHocSinhHienThi,
                                     TenHocSinh = hocSinh.HoTen,
                                     TenNganh = nganh.TenNganhHoc,
                                     TenKhoi = khoi.TenKhoiLop,
                                     TenLopHoc = lopHoc.TenLopHoc,
                                     MaLopHoc = lopHoc.MaLopHoc
                                 };

            totalRecords = tabularHocSinhInfo.Count();
            return tabularHocSinhInfo.ToList();
        }
        //--------------------------------------------------------------------------------

        public List<TabularHocSinhInfo> GetListTabularHocSinhInfo(int maNamHoc,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            totalRecords = 0;
            IQueryable<TabularHocSinhInfo> tabularHocSinhInfo;
            tabularHocSinhInfo = from hocSinh_Lop in db.HocSinh_HocSinhLopHocs
                                 join lopHoc in db.LopHoc_Lops on hocSinh_Lop.MaLopHoc equals lopHoc.MaLopHoc
                                 join nganh in db.DanhMuc_NganhHocs on lopHoc.MaNganhHoc equals nganh.MaNganhHoc
                                 join khoi in db.DanhMuc_KhoiLops on lopHoc.MaKhoiLop equals khoi.MaKhoiLop
                                 join hocSinh in db.HocSinh_ThongTinCaNhans on hocSinh_Lop.MaHocSinh equals hocSinh.MaHocSinh
                                 where lopHoc.MaNamHoc == maNamHoc
                                 select new TabularHocSinhInfo
                                 {
                                     MaHocSinh = hocSinh.MaHocSinh,
                                     MaHocSinhHienThi = hocSinh.MaHocSinhHienThi,
                                     TenHocSinh = hocSinh.HoTen,
                                     TenNganh = nganh.TenNganhHoc,
                                     TenKhoi = khoi.TenKhoiLop,
                                     TenLopHoc = lopHoc.TenLopHoc,
                                     MaLopHoc = lopHoc.MaLopHoc
                                 };

            if (tabularHocSinhInfo.Count() != 0)
            {
                totalRecords = tabularHocSinhInfo.Count();
                tabularHocSinhInfo = tabularHocSinhInfo.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }

            return tabularHocSinhInfo.ToList();
        }

        public List<TabularHocSinhInfo> GetListTabularHocSinhInfoByHoTen(int maNamHoc,
            string tenHocSinh,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularHocSinhInfo> tabularHocSinhInfo;
            tabularHocSinhInfo = from hocSinh_Lop in db.HocSinh_HocSinhLopHocs
                                 join lopHoc in db.LopHoc_Lops on hocSinh_Lop.MaLopHoc equals lopHoc.MaLopHoc
                                 join nganh in db.DanhMuc_NganhHocs on lopHoc.MaNganhHoc equals nganh.MaNganhHoc
                                 join khoi in db.DanhMuc_KhoiLops on lopHoc.MaKhoiLop equals khoi.MaKhoiLop
                                 join hocSinh in db.HocSinh_ThongTinCaNhans on hocSinh_Lop.MaHocSinh equals hocSinh.MaHocSinh
                                 where lopHoc.MaNamHoc == maNamHoc
                                    && hocSinh.HoTen == tenHocSinh
                                 select new TabularHocSinhInfo
                                 {
                                     MaHocSinh = hocSinh.MaHocSinh,
                                     MaHocSinhHienThi = hocSinh.MaHocSinhHienThi,
                                     TenHocSinh = hocSinh.HoTen,
                                     TenNganh = nganh.TenNganhHoc,
                                     TenKhoi = khoi.TenKhoiLop,
                                     TenLopHoc = lopHoc.TenLopHoc,
                                     MaLopHoc = lopHoc.MaLopHoc
                                 };
            totalRecords = tabularHocSinhInfo.Count();
            if (tabularHocSinhInfo.Count() != 0)
            {
                tabularHocSinhInfo = tabularHocSinhInfo.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }

            return tabularHocSinhInfo.ToList();
        }

        public List<TabularHocSinhInfo> GetListTabularHocSinhInfoByLop(int maNamHoc,
            int maLopHoc,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularHocSinhInfo> tabularHocSinhInfo;
            tabularHocSinhInfo = from hocSinh_Lop in db.HocSinh_HocSinhLopHocs
                                 join lopHoc in db.LopHoc_Lops on hocSinh_Lop.MaLopHoc equals lopHoc.MaLopHoc
                                 join nganh in db.DanhMuc_NganhHocs on lopHoc.MaNganhHoc equals nganh.MaNganhHoc
                                 join khoi in db.DanhMuc_KhoiLops on lopHoc.MaKhoiLop equals khoi.MaKhoiLop
                                 join hocSinh in db.HocSinh_ThongTinCaNhans on hocSinh_Lop.MaHocSinh equals hocSinh.MaHocSinh
                                 where lopHoc.MaNamHoc == maNamHoc && lopHoc.MaLopHoc == maLopHoc
                                 select new TabularHocSinhInfo
                                 {
                                     MaHocSinh = hocSinh.MaHocSinh,
                                     MaHocSinhHienThi = hocSinh.MaHocSinhHienThi,
                                     TenHocSinh = hocSinh.HoTen,
                                     TenNganh = nganh.TenNganhHoc,
                                     TenKhoi = khoi.TenKhoiLop,
                                     TenLopHoc = lopHoc.TenLopHoc,
                                     MaLopHoc = lopHoc.MaLopHoc
                                 };
            totalRecords = tabularHocSinhInfo.Count();
            if (tabularHocSinhInfo.Count() != 0)
            {
                tabularHocSinhInfo = tabularHocSinhInfo.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }

            return tabularHocSinhInfo.ToList();
        }

        public List<TabularHocSinhInfo> GetListTabularHocSinhInfoByLopNHoTen(int maNamHoc,
            int maLopHoc, string tenHocSinh,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularHocSinhInfo> tabularHocSinhInfo;
            tabularHocSinhInfo = from hocSinh_Lop in db.HocSinh_HocSinhLopHocs
                                 join lopHoc in db.LopHoc_Lops on hocSinh_Lop.MaLopHoc equals lopHoc.MaLopHoc
                                 join nganh in db.DanhMuc_NganhHocs on lopHoc.MaNganhHoc equals nganh.MaNganhHoc
                                 join khoi in db.DanhMuc_KhoiLops on lopHoc.MaKhoiLop equals khoi.MaKhoiLop
                                 join hocSinh in db.HocSinh_ThongTinCaNhans on hocSinh_Lop.MaHocSinh equals hocSinh.MaHocSinh
                                 where lopHoc.MaNamHoc == maNamHoc
                                    && lopHoc.MaLopHoc == maLopHoc && hocSinh.HoTen == tenHocSinh
                                 select new TabularHocSinhInfo
                                 {
                                     MaHocSinh = hocSinh.MaHocSinh,
                                     MaHocSinhHienThi = hocSinh.MaHocSinhHienThi,
                                     TenHocSinh = hocSinh.HoTen,
                                     TenNganh = nganh.TenNganhHoc,
                                     TenKhoi = khoi.TenKhoiLop,
                                     TenLopHoc = lopHoc.TenLopHoc,
                                     MaLopHoc = lopHoc.MaLopHoc
                                 };
            totalRecords = tabularHocSinhInfo.Count();
            if (tabularHocSinhInfo.Count() != 0)
            {
                tabularHocSinhInfo = tabularHocSinhInfo.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }

            return tabularHocSinhInfo.ToList();
        }

        //------------------------------------------------------------------------------------------------------

        public List<TabularHocSinhInfo> GetListTabularHocSinhInfoByKhoi(int maNamHoc, int maKhoiLop,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularHocSinhInfo> tabularHocSinhInfo;
            tabularHocSinhInfo = from hocSinh_Lop in db.HocSinh_HocSinhLopHocs
                                 join lopHoc in db.LopHoc_Lops on hocSinh_Lop.MaLopHoc equals lopHoc.MaLopHoc
                                 join nganh in db.DanhMuc_NganhHocs on lopHoc.MaNganhHoc equals nganh.MaNganhHoc
                                 join khoi in db.DanhMuc_KhoiLops on lopHoc.MaKhoiLop equals khoi.MaKhoiLop
                                 join hocSinh in db.HocSinh_ThongTinCaNhans on hocSinh_Lop.MaHocSinh equals hocSinh.MaHocSinh
                                 where lopHoc.MaNamHoc == maNamHoc && lopHoc.MaKhoiLop == maKhoiLop
                                 select new TabularHocSinhInfo
                                 {
                                     MaHocSinh = hocSinh.MaHocSinh,
                                     MaHocSinhHienThi = hocSinh.MaHocSinhHienThi,
                                     TenHocSinh = hocSinh.HoTen,
                                     TenNganh = nganh.TenNganhHoc,
                                     TenKhoi = khoi.TenKhoiLop,
                                     TenLopHoc = lopHoc.TenLopHoc,
                                     MaLopHoc = lopHoc.MaLopHoc
                                 };
            totalRecords = tabularHocSinhInfo.Count();
            if (tabularHocSinhInfo.Count() != 0)
            {
                tabularHocSinhInfo = tabularHocSinhInfo.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }

            return tabularHocSinhInfo.ToList();
        }

        public List<TabularHocSinhInfo> GetListTabularHocSinhInfoByKhoiNHoTen(int maNamHoc, int maKhoiLop,
            string tenHocSinh,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularHocSinhInfo> tabularHocSinhInfo;
            tabularHocSinhInfo = from hocSinh_Lop in db.HocSinh_HocSinhLopHocs
                                 join lopHoc in db.LopHoc_Lops on hocSinh_Lop.MaLopHoc equals lopHoc.MaLopHoc
                                 join nganh in db.DanhMuc_NganhHocs on lopHoc.MaNganhHoc equals nganh.MaNganhHoc
                                 join khoi in db.DanhMuc_KhoiLops on lopHoc.MaKhoiLop equals khoi.MaKhoiLop
                                 join hocSinh in db.HocSinh_ThongTinCaNhans on hocSinh_Lop.MaHocSinh equals hocSinh.MaHocSinh
                                 where lopHoc.MaNamHoc == maNamHoc && lopHoc.MaKhoiLop == maKhoiLop && hocSinh.HoTen == tenHocSinh
                                 select new TabularHocSinhInfo
                                 {
                                     MaHocSinh = hocSinh.MaHocSinh,
                                     MaHocSinhHienThi = hocSinh.MaHocSinhHienThi,
                                     TenHocSinh = hocSinh.HoTen,
                                     TenNganh = nganh.TenNganhHoc,
                                     TenKhoi = khoi.TenKhoiLop,
                                     TenLopHoc = lopHoc.TenLopHoc,
                                     MaLopHoc = lopHoc.MaLopHoc
                                 };
            totalRecords = tabularHocSinhInfo.Count();
            if (tabularHocSinhInfo.Count() != 0)
            {
                tabularHocSinhInfo = tabularHocSinhInfo.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }

            return tabularHocSinhInfo.ToList();
        }

        public List<TabularHocSinhInfo> GetListTabularHocSinhInfoByKhoiNLopHoc(int maNamHoc, int maKhoiLop,
            int maLopHoc,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularHocSinhInfo> tabularHocSinhInfo;
            tabularHocSinhInfo = from hocSinh_Lop in db.HocSinh_HocSinhLopHocs
                                 join lopHoc in db.LopHoc_Lops on hocSinh_Lop.MaLopHoc equals lopHoc.MaLopHoc
                                 join nganh in db.DanhMuc_NganhHocs on lopHoc.MaNganhHoc equals nganh.MaNganhHoc
                                 join khoi in db.DanhMuc_KhoiLops on lopHoc.MaKhoiLop equals khoi.MaKhoiLop
                                 join hocSinh in db.HocSinh_ThongTinCaNhans on hocSinh_Lop.MaHocSinh equals hocSinh.MaHocSinh
                                 where lopHoc.MaNamHoc == maNamHoc && lopHoc.MaKhoiLop == maKhoiLop && lopHoc.MaLopHoc == maLopHoc
                                 select new TabularHocSinhInfo
                                 {
                                     MaHocSinh = hocSinh.MaHocSinh,
                                     MaHocSinhHienThi = hocSinh.MaHocSinhHienThi,
                                     TenHocSinh = hocSinh.HoTen,
                                     TenNganh = nganh.TenNganhHoc,
                                     TenKhoi = khoi.TenKhoiLop,
                                     TenLopHoc = lopHoc.TenLopHoc,
                                     MaLopHoc = lopHoc.MaLopHoc
                                 };
            totalRecords = tabularHocSinhInfo.Count();
            if (tabularHocSinhInfo.Count() != 0)
            {
                tabularHocSinhInfo = tabularHocSinhInfo.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }

            return tabularHocSinhInfo.ToList();
        }


        public List<TabularHocSinhInfo> GetListTabularHocSinhInfoByKhoiNLopHocNHoTen(int maNamHoc, int maKhoiLop,
            int maLopHoc, string tenHocSinh,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularHocSinhInfo> tabularHocSinhInfo;
            tabularHocSinhInfo = from hocSinh_Lop in db.HocSinh_HocSinhLopHocs
                                 join lopHoc in db.LopHoc_Lops on hocSinh_Lop.MaLopHoc equals lopHoc.MaLopHoc
                                 join nganh in db.DanhMuc_NganhHocs on lopHoc.MaNganhHoc equals nganh.MaNganhHoc
                                 join khoi in db.DanhMuc_KhoiLops on lopHoc.MaKhoiLop equals khoi.MaKhoiLop
                                 join hocSinh in db.HocSinh_ThongTinCaNhans on hocSinh_Lop.MaHocSinh equals hocSinh.MaHocSinh
                                 where lopHoc.MaNamHoc == maNamHoc
                                    && lopHoc.MaKhoiLop == maKhoiLop && lopHoc.MaLopHoc == maLopHoc
                                    && hocSinh.HoTen == tenHocSinh
                                 select new TabularHocSinhInfo
                                 {
                                     MaHocSinh = hocSinh.MaHocSinh,
                                     MaHocSinhHienThi = hocSinh.MaHocSinhHienThi,
                                     TenHocSinh = hocSinh.HoTen,
                                     TenNganh = nganh.TenNganhHoc,
                                     TenKhoi = khoi.TenKhoiLop,
                                     TenLopHoc = lopHoc.TenLopHoc,
                                     MaLopHoc = lopHoc.MaLopHoc
                                 };
            totalRecords = tabularHocSinhInfo.Count();
            if (tabularHocSinhInfo.Count() != 0)
            {
                tabularHocSinhInfo = tabularHocSinhInfo.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }

            return tabularHocSinhInfo.ToList();
        }

        //------------------------------------------------------------------------------------------------------       
        public List<TabularHocSinhInfo> GetListTabularHocSinhInfoByNganh(int maNamHoc,
            int maNganhHoc,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            totalRecords = 0;
            IQueryable<TabularHocSinhInfo> tabularHocSinhInfo;
            tabularHocSinhInfo = from hocSinh_Lop in db.HocSinh_HocSinhLopHocs
                                 join lopHoc in db.LopHoc_Lops on hocSinh_Lop.MaLopHoc equals lopHoc.MaLopHoc
                                 join nganh in db.DanhMuc_NganhHocs on lopHoc.MaNganhHoc equals nganh.MaNganhHoc
                                 join khoi in db.DanhMuc_KhoiLops on lopHoc.MaKhoiLop equals khoi.MaKhoiLop
                                 join hocSinh in db.HocSinh_ThongTinCaNhans on hocSinh_Lop.MaHocSinh equals hocSinh.MaHocSinh
                                 where lopHoc.MaNamHoc == maNamHoc && lopHoc.MaNganhHoc == maNganhHoc
                                 select new TabularHocSinhInfo
                                 {
                                     MaHocSinh = hocSinh.MaHocSinh,
                                     MaHocSinhHienThi = hocSinh.MaHocSinhHienThi,
                                     TenHocSinh = hocSinh.HoTen,
                                     TenNganh = nganh.TenNganhHoc,
                                     TenKhoi = khoi.TenKhoiLop,
                                     TenLopHoc = lopHoc.TenLopHoc,
                                     MaLopHoc = lopHoc.MaLopHoc
                                 };

            if (tabularHocSinhInfo.Count() != 0)
            {
                totalRecords = tabularHocSinhInfo.Count();
                tabularHocSinhInfo = tabularHocSinhInfo.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }

            return tabularHocSinhInfo.ToList();
        }

        public List<TabularHocSinhInfo> GetListTabularHocSinhInfoByNganhNHoTen(int maNamHoc, int maNganhHoc,
            string tenHocSinh,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularHocSinhInfo> tabularHocSinhInfo;
            tabularHocSinhInfo = from hocSinh_Lop in db.HocSinh_HocSinhLopHocs
                                 join lopHoc in db.LopHoc_Lops on hocSinh_Lop.MaLopHoc equals lopHoc.MaLopHoc
                                 join nganh in db.DanhMuc_NganhHocs on lopHoc.MaNganhHoc equals nganh.MaNganhHoc
                                 join khoi in db.DanhMuc_KhoiLops on lopHoc.MaKhoiLop equals khoi.MaKhoiLop
                                 join hocSinh in db.HocSinh_ThongTinCaNhans on hocSinh_Lop.MaHocSinh equals hocSinh.MaHocSinh
                                 where lopHoc.MaNamHoc == maNamHoc && lopHoc.MaNganhHoc == maNganhHoc
                                    && hocSinh.HoTen == tenHocSinh
                                 select new TabularHocSinhInfo
                                 {
                                     MaHocSinh = hocSinh.MaHocSinh,
                                     MaHocSinhHienThi = hocSinh.MaHocSinhHienThi,
                                     TenHocSinh = hocSinh.HoTen,
                                     TenNganh = nganh.TenNganhHoc,
                                     TenKhoi = khoi.TenKhoiLop,
                                     TenLopHoc = lopHoc.TenLopHoc,
                                     MaLopHoc = lopHoc.MaLopHoc
                                 };
            totalRecords = tabularHocSinhInfo.Count();
            if (tabularHocSinhInfo.Count() != 0)
            {
                tabularHocSinhInfo = tabularHocSinhInfo.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }

            return tabularHocSinhInfo.ToList();
        }

        public List<TabularHocSinhInfo> GetListTabularHocSinhInfoByNganhNLop(int maNamHoc, int maNganhHoc,
            int maLopHoc,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularHocSinhInfo> tabularHocSinhInfo;
            tabularHocSinhInfo = from hocSinh_Lop in db.HocSinh_HocSinhLopHocs
                                 join lopHoc in db.LopHoc_Lops on hocSinh_Lop.MaLopHoc equals lopHoc.MaLopHoc
                                 join nganh in db.DanhMuc_NganhHocs on lopHoc.MaNganhHoc equals nganh.MaNganhHoc
                                 join khoi in db.DanhMuc_KhoiLops on lopHoc.MaKhoiLop equals khoi.MaKhoiLop
                                 join hocSinh in db.HocSinh_ThongTinCaNhans on hocSinh_Lop.MaHocSinh equals hocSinh.MaHocSinh
                                 where lopHoc.MaNamHoc == maNamHoc && lopHoc.MaNganhHoc == maNganhHoc
                                    && lopHoc.MaLopHoc == maLopHoc
                                 select new TabularHocSinhInfo
                                 {
                                     MaHocSinh = hocSinh.MaHocSinh,
                                     MaHocSinhHienThi = hocSinh.MaHocSinhHienThi,
                                     TenHocSinh = hocSinh.HoTen,
                                     TenNganh = nganh.TenNganhHoc,
                                     TenKhoi = khoi.TenKhoiLop,
                                     TenLopHoc = lopHoc.TenLopHoc,
                                     MaLopHoc = lopHoc.MaLopHoc
                                 };
            totalRecords = tabularHocSinhInfo.Count();
            if (tabularHocSinhInfo.Count() != 0)
            {
                tabularHocSinhInfo = tabularHocSinhInfo.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }

            return tabularHocSinhInfo.ToList();
        }

        public List<TabularHocSinhInfo> GetListTabularHocSinhInfoByNganhNLopNHoTen(int maNamHoc, int maNganhHoc,
            int maLopHoc, string tenHocSinh,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularHocSinhInfo> tabularHocSinhInfo;
            tabularHocSinhInfo = from hocSinh_Lop in db.HocSinh_HocSinhLopHocs
                                 join lopHoc in db.LopHoc_Lops on hocSinh_Lop.MaLopHoc equals lopHoc.MaLopHoc
                                 join nganh in db.DanhMuc_NganhHocs on lopHoc.MaNganhHoc equals nganh.MaNganhHoc
                                 join khoi in db.DanhMuc_KhoiLops on lopHoc.MaKhoiLop equals khoi.MaKhoiLop
                                 join hocSinh in db.HocSinh_ThongTinCaNhans on hocSinh_Lop.MaHocSinh equals hocSinh.MaHocSinh
                                 where lopHoc.MaNamHoc == maNamHoc && lopHoc.MaNganhHoc == maNganhHoc
                                    && lopHoc.MaLopHoc == maLopHoc && hocSinh.HoTen == tenHocSinh
                                 select new TabularHocSinhInfo
                                 {
                                     MaHocSinh = hocSinh.MaHocSinh,
                                     MaHocSinhHienThi = hocSinh.MaHocSinhHienThi,
                                     TenHocSinh = hocSinh.HoTen,
                                     TenNganh = nganh.TenNganhHoc,
                                     TenKhoi = khoi.TenKhoiLop,
                                     TenLopHoc = lopHoc.TenLopHoc,
                                     MaLopHoc = lopHoc.MaLopHoc
                                 };
            totalRecords = tabularHocSinhInfo.Count();
            if (tabularHocSinhInfo.Count() != 0)
            {
                tabularHocSinhInfo = tabularHocSinhInfo.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }

            return tabularHocSinhInfo.ToList();
        }

        //------------------------------------------------------------------------------------------------------

        public List<TabularHocSinhInfo> GetListTabularHocSinhInfoByNganhNKhoi(int maNamHoc, int maNganhHoc, int maKhoiLop,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularHocSinhInfo> tabularHocSinhInfo;
            tabularHocSinhInfo = from hocSinh_Lop in db.HocSinh_HocSinhLopHocs
                                 join lopHoc in db.LopHoc_Lops on hocSinh_Lop.MaLopHoc equals lopHoc.MaLopHoc
                                 join nganh in db.DanhMuc_NganhHocs on lopHoc.MaNganhHoc equals nganh.MaNganhHoc
                                 join khoi in db.DanhMuc_KhoiLops on lopHoc.MaKhoiLop equals khoi.MaKhoiLop
                                 join hocSinh in db.HocSinh_ThongTinCaNhans on hocSinh_Lop.MaHocSinh equals hocSinh.MaHocSinh
                                 where lopHoc.MaNamHoc == maNamHoc
                                    && lopHoc.MaNganhHoc == maNganhHoc && lopHoc.MaKhoiLop == maKhoiLop
                                 select new TabularHocSinhInfo
                                 {
                                     MaHocSinh = hocSinh.MaHocSinh,
                                     MaHocSinhHienThi = hocSinh.MaHocSinhHienThi,
                                     TenHocSinh = hocSinh.HoTen,
                                     TenNganh = nganh.TenNganhHoc,
                                     TenKhoi = khoi.TenKhoiLop,
                                     TenLopHoc = lopHoc.TenLopHoc,
                                     MaLopHoc = lopHoc.MaLopHoc
                                 };
            totalRecords = tabularHocSinhInfo.Count();
            if (tabularHocSinhInfo.Count() != 0)
            {
                tabularHocSinhInfo = tabularHocSinhInfo.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }

            return tabularHocSinhInfo.ToList();
        }

        public List<TabularHocSinhInfo> GetListTabularHocSinhInfoByNganhNKhoiNHoTen(int maNamHoc, int maNganhHoc, int maKhoiLop,
            string tenHocSinh,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularHocSinhInfo> tabularHocSinhInfo;
            tabularHocSinhInfo = from hocSinh_Lop in db.HocSinh_HocSinhLopHocs
                                 join lopHoc in db.LopHoc_Lops on hocSinh_Lop.MaLopHoc equals lopHoc.MaLopHoc
                                 join nganh in db.DanhMuc_NganhHocs on lopHoc.MaNganhHoc equals nganh.MaNganhHoc
                                 join khoi in db.DanhMuc_KhoiLops on lopHoc.MaKhoiLop equals khoi.MaKhoiLop
                                 join hocSinh in db.HocSinh_ThongTinCaNhans on hocSinh_Lop.MaHocSinh equals hocSinh.MaHocSinh
                                 where lopHoc.MaNamHoc == maNamHoc
                                    && lopHoc.MaNganhHoc == maNganhHoc && lopHoc.MaKhoiLop == maKhoiLop
                                    && hocSinh.HoTen == tenHocSinh
                                 select new TabularHocSinhInfo
                                 {
                                     MaHocSinh = hocSinh.MaHocSinh,
                                     MaHocSinhHienThi = hocSinh.MaHocSinhHienThi,
                                     TenHocSinh = hocSinh.HoTen,
                                     TenNganh = nganh.TenNganhHoc,
                                     TenKhoi = khoi.TenKhoiLop,
                                     TenLopHoc = lopHoc.TenLopHoc,
                                     MaLopHoc = lopHoc.MaLopHoc
                                 };
            totalRecords = tabularHocSinhInfo.Count();
            if (tabularHocSinhInfo.Count() != 0)
            {
                tabularHocSinhInfo = tabularHocSinhInfo.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }

            return tabularHocSinhInfo.ToList();
        }

        public List<TabularHocSinhInfo> GetListTabularHocSinhInfoByNganhNKhoiNLopHoc(int maNamHoc, int maNganhHoc, int maKhoiLop,
            int maLopHoc,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularHocSinhInfo> tabularHocSinhInfo;
            tabularHocSinhInfo = from hocSinh_Lop in db.HocSinh_HocSinhLopHocs
                                 join lopHoc in db.LopHoc_Lops on hocSinh_Lop.MaLopHoc equals lopHoc.MaLopHoc
                                 join nganh in db.DanhMuc_NganhHocs on lopHoc.MaNganhHoc equals nganh.MaNganhHoc
                                 join khoi in db.DanhMuc_KhoiLops on lopHoc.MaKhoiLop equals khoi.MaKhoiLop
                                 join hocSinh in db.HocSinh_ThongTinCaNhans on hocSinh_Lop.MaHocSinh equals hocSinh.MaHocSinh
                                 where lopHoc.MaNamHoc == maNamHoc
                                    && lopHoc.MaNganhHoc == maNganhHoc && lopHoc.MaKhoiLop == maKhoiLop && lopHoc.MaLopHoc == maLopHoc
                                 select new TabularHocSinhInfo
                                 {
                                     MaHocSinh = hocSinh.MaHocSinh,
                                     MaHocSinhHienThi = hocSinh.MaHocSinhHienThi,
                                     TenHocSinh = hocSinh.HoTen,
                                     TenNganh = nganh.TenNganhHoc,
                                     TenKhoi = khoi.TenKhoiLop,
                                     TenLopHoc = lopHoc.TenLopHoc,
                                     MaLopHoc = lopHoc.MaLopHoc
                                 };
            totalRecords = tabularHocSinhInfo.Count();
            if (tabularHocSinhInfo.Count() != 0)
            {
                tabularHocSinhInfo = tabularHocSinhInfo.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }

            return tabularHocSinhInfo.ToList();
        }


        public List<TabularHocSinhInfo> GetListTabularHocSinhInfoByNganhNKhoiNLopHocNHoTen(int maNamHoc, int maNganhHoc, int maKhoiLop,
            int maLopHoc, string tenHocSinh,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularHocSinhInfo> tabularHocSinhInfo;
            tabularHocSinhInfo = from hocSinh_Lop in db.HocSinh_HocSinhLopHocs
                                 join lopHoc in db.LopHoc_Lops on hocSinh_Lop.MaLopHoc equals lopHoc.MaLopHoc
                                 join nganh in db.DanhMuc_NganhHocs on lopHoc.MaNganhHoc equals nganh.MaNganhHoc
                                 join khoi in db.DanhMuc_KhoiLops on lopHoc.MaKhoiLop equals khoi.MaKhoiLop
                                 join hocSinh in db.HocSinh_ThongTinCaNhans on hocSinh_Lop.MaHocSinh equals hocSinh.MaHocSinh
                                 where lopHoc.MaNamHoc == maNamHoc
                                    && lopHoc.MaNganhHoc == maNganhHoc
                                    && lopHoc.MaKhoiLop == maKhoiLop && lopHoc.MaLopHoc == maLopHoc
                                    && hocSinh.HoTen == tenHocSinh
                                 select new TabularHocSinhInfo
                                 {
                                     MaHocSinh = hocSinh.MaHocSinh,
                                     MaHocSinhHienThi = hocSinh.MaHocSinhHienThi,
                                     TenHocSinh = hocSinh.HoTen,
                                     TenNganh = nganh.TenNganhHoc,
                                     TenKhoi = khoi.TenKhoiLop,
                                     TenLopHoc = lopHoc.TenLopHoc,
                                     MaLopHoc = lopHoc.MaLopHoc
                                 };
            totalRecords = tabularHocSinhInfo.Count();
            if (tabularHocSinhInfo.Count() != 0)
            {
                tabularHocSinhInfo = tabularHocSinhInfo.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }

            return tabularHocSinhInfo.ToList();
        }

        // ------------------------------------------------------------------------------------
        #endregion

        public bool CanDeleteHocSinh(string maHocSinhHienThi)
        {
            return true;
        }

        public int GetMaLopHoc(int maNamHoc, int maHocSinh)
        {
            IQueryable<HocSinh_HocSinhLopHoc> hocSinhLopHocs = from hs_lh in db.HocSinh_HocSinhLopHocs
                                                               join lop in db.LopHoc_Lops on hs_lh.MaLopHoc equals lop.MaLopHoc
                                                               where hs_lh.MaHocSinh == maHocSinh && lop.MaNamHoc == maNamHoc
                                                               select hs_lh;
            if (hocSinhLopHocs.Count() != 0)
            {
                return hocSinhLopHocs.First().MaLopHoc;
            }
            else
            {
                return 0;
            }
        }

        public void UpdateHocSinh(int maHocSinh, int maLopHoc,
            string maHocSinhHienThi, string tenHocSinh, bool gioiTinh, DateTime ngaySinh,
            string noiSinh, string diaChi, string dienThoai, string tenBo,
            string ngheNghiepBo, DateTime? ngaySinhBo,
            string tenMe, string ngheNghiepMe, DateTime? ngaySinhMe,
            string tenNguoiDoDau, string ngheNghiepNguoiDoDau, DateTime? ngaySinhNguoiDoDau)
        {
            HocSinh_HocSinhLopHoc hocSinhLopHoc = (from hs_lh in db.HocSinh_HocSinhLopHocs
                                                   where hs_lh.MaHocSinh == maHocSinh
                                                   select hs_lh).First();
            hocSinhLopHoc.MaLopHoc = maLopHoc;
            db.SubmitChanges();

            HocSinh_ThongTinCaNhan thongTinCaNhan = (from hocSinh in db.HocSinh_ThongTinCaNhans
                                                     where hocSinh.MaHocSinh == maHocSinh
                                                     select hocSinh).First();
            thongTinCaNhan.MaHocSinhHienThi = maHocSinhHienThi;
            thongTinCaNhan.HoTen = tenHocSinh;
            thongTinCaNhan.GioiTinh = gioiTinh;
            thongTinCaNhan.NgaySinh = ngaySinh;
            thongTinCaNhan.DiaChi = diaChi;
            thongTinCaNhan.DienThoai = dienThoai;
            thongTinCaNhan.HoTenBo = tenBo;
            thongTinCaNhan.NgaySinhBo = ngaySinhBo;
            thongTinCaNhan.NgheNghiepBo = ngheNghiepBo;
            thongTinCaNhan.HoTenMe = tenMe;
            thongTinCaNhan.NgaySinhMe = ngaySinhMe;
            thongTinCaNhan.NgheNghiepMe = ngheNghiepMe;
            thongTinCaNhan.HoTenNguoiDoDau = tenNguoiDoDau;
            thongTinCaNhan.NgaySinhNguoiDoDau = ngaySinhNguoiDoDau;
            thongTinCaNhan.NgheNghiepNguoiDoDau = ngheNghiepNguoiDoDau;
            db.SubmitChanges();
        }

        public List<HocSinh_HocSinhLopHoc> GetListHocSinhLopHoc(int maLopHoc)
        {
            IQueryable<HocSinh_HocSinhLopHoc> hocSinhLopHocs = from hs_lh in db.HocSinh_HocSinhLopHocs
                                                               where hs_lh.MaLopHoc == maLopHoc
                                                               select hs_lh;
            if (hocSinhLopHocs.Count() != 0)
            {
                return hocSinhLopHocs.ToList();
            }
            else
            {
                return new List<HocSinh_HocSinhLopHoc>();
            }
        }

        #region Get list of StudentDropdownListItems
        public List<StudentDropdownListItem> GetStudents()
        {
            // get current year
            CauHinh_NamHoc currentYear = (new SystemConfigDA()).GetCurrentYear();

            // get lis
            IQueryable<StudentDropdownListItem> iqStudent;
            iqStudent = from student in db.HocSinh_ThongTinCaNhans
                        join studentInClass in db.HocSinh_HocSinhLopHocs
                            on student.MaHocSinh equals studentInClass.MaHocSinh
                        join cls in db.LopHoc_Lops
                            on studentInClass.MaLopHoc equals cls.MaLopHoc
                        where cls.MaNamHoc == currentYear.MaNamHoc
                        select new StudentDropdownListItem
                        {
                            StudentId = studentInClass.MaHocSinh,
                            StudentInClassId = studentInClass.MaHocSinhLopHoc,
                            StudentCode = student.MaHocSinhHienThi,
                            StudentName = student.HoTen
                        };
            if (iqStudent.Count() != 0)
            {
                return iqStudent.OrderBy(std => std.StudentCode)
                    .ThenBy(std => std.StudentName).ToList();
            }
            else
            {
                return new List<StudentDropdownListItem>();
            }
        }

        public List<StudentDropdownListItem> GetStudents(LopHoc_Lop pClass)
        {
            IQueryable<StudentDropdownListItem> iqStudent;
            iqStudent = from student in db.HocSinh_ThongTinCaNhans
                        join studentInClass in db.HocSinh_HocSinhLopHocs
                            on student.MaHocSinh equals studentInClass.MaHocSinh
                        where studentInClass.MaLopHoc == pClass.MaLopHoc
                        select new StudentDropdownListItem
                        {
                            StudentId = studentInClass.MaHocSinh,
                            StudentInClassId = studentInClass.MaHocSinhLopHoc,
                            StudentCode = student.MaHocSinhHienThi,
                            StudentName = student.HoTen
                        };
            if (iqStudent.Count() != 0)
            {
                return iqStudent.OrderBy(std => std.StudentCode)
                    .ThenBy(std => std.StudentName).ToList();
            }
            else
            {
                return new List<StudentDropdownListItem>();
            }
        }

        public List<StudentDropdownListItem> GetStudents(DanhMuc_KhoiLop grade)
        {
            // get current year
            CauHinh_NamHoc currentYear = (new SystemConfigDA()).GetCurrentYear();

            // get lis
            IQueryable<StudentDropdownListItem> iqStudent;
            iqStudent = from student in db.HocSinh_ThongTinCaNhans
                        join studentInClass in db.HocSinh_HocSinhLopHocs
                            on student.MaHocSinh equals studentInClass.MaHocSinh
                        join cls in db.LopHoc_Lops
                            on studentInClass.MaLopHoc equals cls.MaLopHoc
                        where cls.MaNamHoc == currentYear.MaNamHoc
                            && cls.MaKhoiLop == grade.MaKhoiLop
                        select new StudentDropdownListItem
                        {
                            StudentId = studentInClass.MaHocSinh,
                            StudentInClassId = studentInClass.MaHocSinhLopHoc,
                            StudentCode = student.MaHocSinhHienThi,
                            StudentName = student.HoTen
                        };
            if (iqStudent.Count() != 0)
            {
                return iqStudent.OrderBy(std => std.StudentCode)
                    .ThenBy(std => std.StudentName).ToList();
            }
            else
            {
                return new List<StudentDropdownListItem>();
            }
        }

        public List<StudentDropdownListItem> GetStudents(DanhMuc_NganhHoc faculty)
        {
            // get current year
            CauHinh_NamHoc currentYear = (new SystemConfigDA()).GetCurrentYear();

            // get lis
            IQueryable<StudentDropdownListItem> iqStudent;
            iqStudent = from student in db.HocSinh_ThongTinCaNhans
                        join studentInClass in db.HocSinh_HocSinhLopHocs
                            on student.MaHocSinh equals studentInClass.MaHocSinh
                        join cls in db.LopHoc_Lops
                            on studentInClass.MaLopHoc equals cls.MaLopHoc
                        where cls.MaNamHoc == currentYear.MaNamHoc && cls.MaNganhHoc == faculty.MaNganhHoc
                        select new StudentDropdownListItem
                        {
                            StudentId = studentInClass.MaHocSinh,
                            StudentInClassId = studentInClass.MaHocSinhLopHoc,
                            StudentCode = student.MaHocSinhHienThi,
                            StudentName = student.HoTen
                        };
            if (iqStudent.Count() != 0)
            {
                return iqStudent.OrderBy(std => std.StudentCode)
                    .ThenBy(std => std.StudentName).ToList();
            }
            else
            {
                return new List<StudentDropdownListItem>();
            }
        }

        public List<StudentDropdownListItem> GetStudents(DanhMuc_KhoiLop grade, DanhMuc_NganhHoc faculty)
        {
            // get current year
            CauHinh_NamHoc currentYear = (new SystemConfigDA()).GetCurrentYear();

            // get lis
            IQueryable<StudentDropdownListItem> iqStudent;
            iqStudent = from student in db.HocSinh_ThongTinCaNhans
                        join studentInClass in db.HocSinh_HocSinhLopHocs
                            on student.MaHocSinh equals studentInClass.MaHocSinh
                        join cls in db.LopHoc_Lops
                            on studentInClass.MaLopHoc equals cls.MaLopHoc
                        where cls.MaNamHoc == currentYear.MaNamHoc
                            && cls.MaNganhHoc == faculty.MaNganhHoc
                            && cls.MaKhoiLop == grade.MaKhoiLop
                        select new StudentDropdownListItem
                        {
                            StudentId = studentInClass.MaHocSinh,
                            StudentInClassId = studentInClass.MaHocSinhLopHoc,
                            StudentCode = student.MaHocSinhHienThi,
                            StudentName = student.HoTen
                        };
            if (iqStudent.Count() != 0)
            {
                return iqStudent.OrderBy(std => std.StudentCode)
                    .ThenBy(std => std.StudentName).ToList();
            }
            else
            {
                return new List<StudentDropdownListItem>();
            }
        }
        #endregion

        public HocSinh_HocSinhLopHoc GetHocSinhLopHoc(int maHocSinhLopHoc)
        {
            IQueryable<HocSinh_HocSinhLopHoc> hocSinhLopHocs = from hs_lh in db.HocSinh_HocSinhLopHocs
                                                               where hs_lh.MaHocSinhLopHoc == maHocSinhLopHoc
                                                               select hs_lh;
            if (hocSinhLopHocs.Count() != 0)
            {
                return hocSinhLopHocs.First();
            }
            else
            {
                return null;
            }
        }

        public HocSinh_HocSinhLopHoc GetHocSinhLopHoc(int maNamHoc, int maHocSinh)
        {
            IQueryable<HocSinh_HocSinhLopHoc> hocSinhLopHocs = from hs_lh in db.HocSinh_HocSinhLopHocs
                                                               join lop in db.LopHoc_Lops on hs_lh.MaLopHoc equals lop.MaLopHoc
                                                               where hs_lh.MaHocSinh == maHocSinh && lop.MaNamHoc == maNamHoc
                                                               select hs_lh;
            if (hocSinhLopHocs.Count() != 0)
            {
                return hocSinhLopHocs.First();
            }
            else
            {
                return null;
            }
        }

        public List<CauHinh_NamHoc> GetListNamHoc(int maHocSinh)
        {
            IQueryable<CauHinh_NamHoc> namHocs = from nam in db.CauHinh_NamHocs
                                                 join lop in db.LopHoc_Lops
                                                    on nam.MaNamHoc equals lop.MaNamHoc
                                                 join hocSinhLopHoc in db.HocSinh_HocSinhLopHocs
                                                    on lop.MaLopHoc equals hocSinhLopHoc.MaLopHoc
                                                 where hocSinhLopHoc.MaHocSinh == maHocSinh
                                                 select nam;
            if (namHocs.Count() != 0)
            {
                return namHocs.OrderByDescending(nam => nam.NamBatDau).ToList();
            }
            else
            {
                return new List<CauHinh_NamHoc>();
            }
        }

        //
        public int GetLastedMaHocSinhLopHoc()
        {
            IQueryable<HocSinh_HocSinhLopHoc> hocSinhLopHocs = from hs_lh in db.HocSinh_HocSinhLopHocs
                                                               select hs_lh;
            if (hocSinhLopHocs.Count() != 0)
            {
                return hocSinhLopHocs.OrderByDescending(hs_lh => hs_lh.MaHocSinhLopHoc).First().MaHocSinhLopHoc;
            }
            else
            {
                return 0;
            }
        }

        public bool MaHocSinhExists(string maHocSinhHienThi)
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

        public LopHoc_Lop GetLopHoc(int maHocSinh, int maNamHoc)
        {
            LopHoc_Lop lopHoc = (from lop in db.LopHoc_Lops
                                 join hsLop in db.HocSinh_HocSinhLopHocs on lop.MaLopHoc equals hsLop.MaLopHoc
                                 where lop.MaNamHoc == maNamHoc && hsLop.MaHocSinh == maHocSinh
                                 select lop).First();
            return lopHoc;
        }

        public int GetCurrentMaLopHoc(int maHocSinh)
        {
            HocSinh_HocSinhLopHoc hocSinhLop = (from hsLop in db.HocSinh_HocSinhLopHocs
                                                where hsLop.MaHocSinh == maHocSinh
                                                select hsLop).OrderByDescending(hsLop => hsLop.MaHocSinhLopHoc).First();
            return hocSinhLop.MaLopHoc;
        }

        public List<TabularHanhKiemHocSinh> GetListHanhKiemHocSinh(int maLopHoc, int maHocKy,
            int pageCurrentIndex, int pageSize, out double totalRecords)
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

        public void UpdateHanhKiemHocSinh(int maLopHoc, int maHocKy, int maHocSinh, int? maHanhKiem)
        {
            IQueryable<HocSinh_DanhHieuHocKy> iqDanhHieuHocKy;
            iqDanhHieuHocKy = from danhHieuHK in db.HocSinh_DanhHieuHocKies
                              join hocSinhLopHoc in db.HocSinh_HocSinhLopHocs
                                on danhHieuHK.MaHocSinhLopHoc equals hocSinhLopHoc.MaHocSinhLopHoc
                              where hocSinhLopHoc.MaLopHoc == maLopHoc
                                && danhHieuHK.MaHocKy == maHocKy
                                && hocSinhLopHoc.MaHocSinh == maHocSinh
                              select danhHieuHK;
            if (iqDanhHieuHocKy.Count() != null)
            {
                foreach (HocSinh_DanhHieuHocKy danhHieuHocKy in iqDanhHieuHocKy)
                {
                    danhHieuHocKy.MaHanhKiemHK = maHanhKiem;
                }
                db.SubmitChanges();
            }
        }
    }
}