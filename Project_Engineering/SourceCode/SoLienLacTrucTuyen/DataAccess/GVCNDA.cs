using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class GVCNDA : BaseDA
    {
        public GVCNDA()
            : base()
        {
        }

        public void InsertGVCN(LopHoc_GVCN giaoVienChuNhiemEn)
        {
            db.LopHoc_GVCNs.InsertOnSubmit(giaoVienChuNhiemEn);
            db.SubmitChanges();
        }

        public void UpdateGVCN(LopHoc_GVCN giaoVienChuNhiemEn)
        {
            //LopHoc_GVCN giaoVienChuNhiem = GetGVCN(giaoVienChuNhiemEn.MaGVCN);
            //if (giaoVienChuNhiem != null)
            //{
            //    giaoVienChuNhiem.MaHienThiGVCN = giaoVienChuNhiemEn.MaHienThiGVCN;
            //    giaoVienChuNhiem.HoTen = giaoVienChuNhiemEn.HoTen;                
            //    giaoVienChuNhiem.GioiTinh = giaoVienChuNhiemEn.GioiTinh;
            //    giaoVienChuNhiem.NgaySinh = giaoVienChuNhiemEn.NgaySinh;
            //    giaoVienChuNhiem.DiaChi = giaoVienChuNhiemEn.DiaChi;
            //    giaoVienChuNhiem.DienThoai = giaoVienChuNhiemEn.DienThoai;
            //    giaoVienChuNhiem.HinhAnh = giaoVienChuNhiemEn.HinhAnh;

            //    db.SubmitChanges();
            //}
        }

        public void DeleteGVCN(int maGVCN)
        {
            IQueryable<LopHoc_GVCN> giaoVienChuNhiems = from gvcn in db.LopHoc_GVCNs
                                                        where gvcn.MaGVCN == maGVCN
                                                        select gvcn;

            db.LopHoc_GVCNs.DeleteOnSubmit(giaoVienChuNhiems.First());
            db.SubmitChanges();
        }

        public LopHoc_GVCN GetGVCN(int maGVCN)
        {
            IQueryable<LopHoc_GVCN> giaoVienChuNhiems = from gvcn in db.LopHoc_GVCNs
                                                        where gvcn.MaGVCN == maGVCN
                                                        select gvcn;
            if (giaoVienChuNhiems.Count() != 0)
            {
                return giaoVienChuNhiems.First();
            }
            else
            {
                return null;
            }
        }        


        public GVCNInfo GetGVCNInfo(int maLopHoc)
        {
            IQueryable<GVCNInfo> giaoVienChuNhiemInfos = from gvcn in db.LopHoc_GVCNs
                                                         join lophoc in db.LopHoc_Lops 
                                                            on gvcn.MaLopHoc equals lophoc.MaLopHoc
                                                         join giaoVien in db.LopHoc_GiaoViens
                                                            on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                                                         where gvcn.MaLopHoc == maLopHoc
                                                         select new GVCNInfo
                                                         {
                                                             MaGVCN = gvcn.MaGVCN,
                                                             MaGVCNHienThi = giaoVien.MaHienThiGiaoVien,
                                                             TenGVCN = giaoVien.HoTen,
                                                             NgaySinh = giaoVien.NgaySinh,
                                                             GioiTinh = giaoVien.GioiTinh,
                                                             DiaChi = giaoVien.DiaChi,
                                                             DienThoai = giaoVien.DienThoai,
                                                             MaLopHoc = gvcn.MaLopHoc,
                                                             TenLopHoc = lophoc.TenLopHoc
                                                         };

            if (giaoVienChuNhiemInfos.Count() != 0)
            {
                GVCNInfo giaoVienChuNhiemInfo = giaoVienChuNhiemInfos.First();
                return giaoVienChuNhiemInfo;
            }
            else
            {
                return null;
            }
        }

        public List<GVCNInfo> GetListGVCNInfo(int maNamHoc, int pageCurrentIndex, int pageSize)
        {
            IQueryable<GVCNInfo> giaoVienChuNhiemInfos = from gvcn in db.LopHoc_GVCNs
                                                         join lophoc in db.LopHoc_Lops
                                                            on gvcn.MaLopHoc equals lophoc.MaLopHoc
                                                         join giaoVien in db.LopHoc_GiaoViens
                                                            on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                                                         where lophoc.MaNamHoc == maNamHoc
                                                         select new GVCNInfo
                                                         {
                                                             MaGVCN = gvcn.MaGVCN,
                                                             MaGVCNHienThi = giaoVien.MaHienThiGVCN,
                                                             TenGVCN = giaoVien.HoTen,
                                                             NgaySinh = giaoVien.NgaySinh,
                                                             GioiTinh = giaoVien.GioiTinh,
                                                             DiaChi = giaoVien.DiaChi,
                                                             DienThoai = giaoVien.DienThoai,
                                                             MaLopHoc = gvcn.MaLopHoc,
                                                             TenLopHoc = lophoc.TenLopHoc
                                                         };
            if (giaoVienChuNhiemInfos.Count() != 0)
            {
                giaoVienChuNhiemInfos = giaoVienChuNhiemInfos.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }

            return giaoVienChuNhiemInfos.ToList();
        }

        public List<GVCNInfo> GetListGVCNInfoByNganhHoc(int maNamHoc, int maNganhHoc, int pageCurrentIndex, int pageSize)
        {
            IQueryable<GVCNInfo> giaoVienChuNhiemInfos = from gvcn in db.LopHoc_GVCNs
                                                         join lophoc in db.LopHoc_Lops
                                                            on gvcn.MaLopHoc equals lophoc.MaLopHoc
                                                         join giaoVien in db.LopHoc_GiaoViens
                                                            on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                                                         where lophoc.MaNamHoc == maNamHoc 
                                                            && lophoc.MaNganhHoc == maNganhHoc
                                                         select new GVCNInfo
                                                         {
                                                             MaGVCN = gvcn.MaGVCN,
                                                             MaGVCNHienThi = giaoVien.MaHienThiGVCN,
                                                             TenGVCN = giaoVien.HoTen,
                                                             NgaySinh = giaoVien.NgaySinh,
                                                             GioiTinh = giaoVien.GioiTinh,
                                                             DiaChi = giaoVien.DiaChi,
                                                             DienThoai = giaoVien.DienThoai,
                                                             MaLopHoc = gvcn.MaLopHoc,
                                                             TenLopHoc = lophoc.TenLopHoc
                                                         };

            if (giaoVienChuNhiemInfos.Count() != 0)
            {
                giaoVienChuNhiemInfos = giaoVienChuNhiemInfos.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }

            return giaoVienChuNhiemInfos.ToList();
        }

        public List<GVCNInfo> GetListGVCNInfoByKhoiLop(int maNamHoc, int maKhoiLop, int pageCurrentIndex, int pageSize)
        {
            IQueryable<GVCNInfo> giaoVienChuNhiemInfos = from gvcn in db.LopHoc_GVCNs
                                                         join lophoc in db.LopHoc_Lops
                                                            on gvcn.MaLopHoc equals lophoc.MaLopHoc
                                                         join giaoVien in db.LopHoc_GiaoViens
                                                            on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                                                         where lophoc.MaNamHoc == maNamHoc
                                                            && lophoc.MaKhoiLop == maKhoiLop
                                                         select new GVCNInfo
                                                         {
                                                             MaGVCN = gvcn.MaGVCN,
                                                             MaGVCNHienThi = giaoVien.MaHienThiGVCN,
                                                             TenGVCN = giaoVien.HoTen,
                                                             NgaySinh = giaoVien.NgaySinh,
                                                             GioiTinh = giaoVien.GioiTinh,
                                                             DiaChi = giaoVien.DiaChi,
                                                             DienThoai = giaoVien.DienThoai,
                                                             MaLopHoc = gvcn.MaLopHoc,
                                                             TenLopHoc = lophoc.TenLopHoc
                                                         };

            if (giaoVienChuNhiemInfos.Count() != 0)
            {
                giaoVienChuNhiemInfos = giaoVienChuNhiemInfos.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }

            return giaoVienChuNhiemInfos.ToList();
        }

        public List<GVCNInfo> GetListGVCNInfo(int maNamHoc, int maNganhHoc, int maKhoiLop, int pageCurrentIndex, int pageSize)
        {
            IQueryable<GVCNInfo> giaoVienChuNhiemInfos = from gvcn in db.LopHoc_GVCNs
                                                         join lophoc in db.LopHoc_Lops
                                                            on gvcn.MaLopHoc equals lophoc.MaLopHoc
                                                         join giaoVien in db.LopHoc_GiaoViens
                                                            on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                                                         where lophoc.MaNamHoc == maNamHoc
                                                            && lophoc.MaNganhHoc == maNganhHoc
                                                            && lophoc.MaKhoiLop == maKhoiLop
                                                         select new GVCNInfo
                                                         {
                                                             MaGVCN = gvcn.MaGVCN,
                                                             MaGVCNHienThi = giaoVien.MaHienThiGiaoVien,
                                                             TenGVCN = giaoVien.HoTen,
                                                             NgaySinh = giaoVien.NgaySinh,
                                                             GioiTinh = giaoVien.GioiTinh,
                                                             DiaChi = giaoVien.DiaChi,
                                                             DienThoai = giaoVien.DienThoai,
                                                             MaLopHoc = gvcn.MaLopHoc,
                                                             TenLopHoc = lophoc.TenLopHoc
                                                         };

            if (giaoVienChuNhiemInfos.Count() != 0)
            {
                giaoVienChuNhiemInfos = giaoVienChuNhiemInfos.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }

            return giaoVienChuNhiemInfos.ToList();
        }


        public GVCNInfo GetGVCNInfo(int maLopHoc, string tenGVCN)
        {
            IQueryable<GVCNInfo> giaoVienChuNhiemInfos = from giaoVien in db.LopHoc_GVCNs
                                                         join gvcn in db.LopHoc_GVCNs 
                                                            on giaoVien.MaGVCN equals gvcn.MaGVCN
                                                         join lopHoc in db.LopHoc_Lops 
                                                            on gvcn.MaLopHoc equals lopHoc.MaLopHoc
                                                         where gvcn.MaLopHoc == maLopHoc && giaoVien.HoTen == tenGVCN
                                                         select new GVCNInfo
                                                         {
                                                             MaGVCN = giaoVien.MaGVCN,
                                                             MaGVCNHienThi = giaoVien.MaHienThiGVCN,
                                                             TenGVCN = giaoVien.HoTen,
                                                             NgaySinh = giaoVien.NgaySinh,
                                                             GioiTinh = giaoVien.GioiTinh,
                                                             DiaChi = giaoVien.DiaChi,
                                                             DienThoai = giaoVien.DienThoai,
                                                             MaLopHoc = lopHoc.MaLopHoc,
                                                             TenLopHoc = lopHoc.TenLopHoc
                                                         };

            if (giaoVienChuNhiemInfos.Count() != 0)
            {
                GVCNInfo giaoVienChuNhiemInfo = giaoVienChuNhiemInfos.First();
                return giaoVienChuNhiemInfo;
            }
            else
            {
                return null;
            }
        }

        public List<GVCNInfo> GetListGVCNInfo(int maNamHoc, string tenGVCN, int pageCurrentIndex, int pageSize)
        {
            IQueryable<GVCNInfo> giaoVienChuNhiemInfos = from giaoVien in db.LopHoc_GVCNs
                                                         join gvcn in db.LopHoc_GVCNs 
                                                            on giaoVien.MaGVCN equals gvcn.MaGVCN
                                                         join lophoc in db.LopHoc_Lops 
                                                            on gvcn.MaLopHoc equals lophoc.MaLopHoc
                                                         where lophoc.MaNamHoc == maNamHoc && giaoVien.HoTen == tenGVCN
                                                         select new GVCNInfo
                                                         {
                                                             MaGVCN = giaoVien.MaGVCN,
                                                             MaGVCNHienThi = giaoVien.MaHienThiGVCN,
                                                             TenGVCN = giaoVien.HoTen,
                                                             NgaySinh = giaoVien.NgaySinh,
                                                             GioiTinh = giaoVien.GioiTinh,
                                                             DiaChi = giaoVien.DiaChi,
                                                             DienThoai = giaoVien.DienThoai,
                                                             MaLopHoc = lophoc.MaLopHoc,
                                                             TenLopHoc = lophoc.TenLopHoc
                                                         };
            if (giaoVienChuNhiemInfos.Count() != 0)
            {
                giaoVienChuNhiemInfos = giaoVienChuNhiemInfos.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }

            return giaoVienChuNhiemInfos.ToList();
        }

        public List<GVCNInfo> GetListGVCNInfo(int maNamHoc, int maNganhHoc, int maKhoiLop, string tenGVCN, int pageCurrentIndex, int pageSize)
        {
            IQueryable<GVCNInfo> giaoVienChuNhiemInfos = from giaoVien in db.LopHoc_GVCNs
                                                         join gvcn in db.LopHoc_GVCNs 
                                                            on giaoVien.MaGVCN equals gvcn.MaGVCN
                                                         join lophoc in db.LopHoc_Lops 
                                                            on gvcn.MaLopHoc equals lophoc.MaLopHoc
                                                         where lophoc.MaNamHoc == maNamHoc 
                                                            && lophoc.MaNganhHoc == maNganhHoc 
                                                            && lophoc.MaKhoiLop == maKhoiLop 
                                                                && giaoVien.HoTen == tenGVCN
                                                         select new GVCNInfo
                                                         {
                                                             MaGVCN = giaoVien.MaGVCN,
                                                             MaGVCNHienThi = giaoVien.MaHienThiGVCN,
                                                             TenGVCN = giaoVien.HoTen,
                                                             NgaySinh = giaoVien.NgaySinh,
                                                             GioiTinh = giaoVien.GioiTinh,
                                                             DiaChi = giaoVien.DiaChi,
                                                             DienThoai = giaoVien.DienThoai,
                                                             MaLopHoc = gvcn.MaLopHoc,
                                                             TenLopHoc = lophoc.TenLopHoc
                                                         };

            if (giaoVienChuNhiemInfos.Count() != 0)
            {
                giaoVienChuNhiemInfos = giaoVienChuNhiemInfos.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }

            return giaoVienChuNhiemInfos.ToList();
        }

        public List<GVCNInfo> GetListGVCNInfoByNganhHoc(int maNamHoc, int maNganhHoc, string tenGVCN, 
            int pageCurrentIndex, int pageSize)
        {
            IQueryable<GVCNInfo> giaoVienChuNhiemInfos = from giaoVien in db.LopHoc_GVCNs
                                                         join gvcn in db.LopHoc_GVCNs 
                                                            on giaoVien.MaGVCN equals gvcn.MaGVCN
                                                         join lophoc in db.LopHoc_Lops 
                                                            on gvcn.MaLopHoc equals lophoc.MaLopHoc
                                                         where lophoc.MaNamHoc == maNamHoc 
                                                            && lophoc.MaNganhHoc == maNganhHoc 
                                                            && giaoVien.HoTen == tenGVCN
                                                         select new GVCNInfo
                                                         {
                                                             MaGVCN = giaoVien.MaGVCN,
                                                             MaGVCNHienThi = giaoVien.MaHienThiGVCN,
                                                             TenGVCN = giaoVien.HoTen,
                                                             NgaySinh = giaoVien.NgaySinh,
                                                             GioiTinh = giaoVien.GioiTinh,
                                                             DiaChi = giaoVien.DiaChi,
                                                             DienThoai = giaoVien.DienThoai,
                                                             MaLopHoc = lophoc.MaLopHoc,
                                                             TenLopHoc = lophoc.TenLopHoc
                                                         };

            if (giaoVienChuNhiemInfos.Count() != 0)
            {
                giaoVienChuNhiemInfos = giaoVienChuNhiemInfos.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }

            return giaoVienChuNhiemInfos.ToList();
        }

        public List<GVCNInfo> GetListGVCNInfoByKhoiLop(int maNamHoc, int maKhoiLop, string tenGVCN, 
            int pageCurrentIndex, int pageSize)
        {
            IQueryable<GVCNInfo> giaoVienChuNhiemInfos = from giaoVien in db.LopHoc_GVCNs
                                                         join gvcn in db.LopHoc_GVCNs 
                                                            on giaoVien.MaGVCN equals gvcn.MaGVCN
                                                         join lophoc in db.LopHoc_Lops 
                                                            on gvcn.MaLopHoc equals lophoc.MaLopHoc
                                                         where lophoc.MaNamHoc == maNamHoc 
                                                            && lophoc.MaKhoiLop == maKhoiLop 
                                                            && giaoVien.HoTen == tenGVCN
                                                         select new GVCNInfo
                                                         {
                                                             MaGVCN = giaoVien.MaGVCN,
                                                             MaGVCNHienThi = giaoVien.MaHienThiGVCN,
                                                             TenGVCN = giaoVien.HoTen,
                                                             NgaySinh = giaoVien.NgaySinh,
                                                             GioiTinh = giaoVien.GioiTinh,
                                                             DiaChi = giaoVien.DiaChi,
                                                             DienThoai = giaoVien.DienThoai,
                                                             MaLopHoc = lophoc.MaLopHoc,
                                                             TenLopHoc = lophoc.TenLopHoc
                                                         };

            if (giaoVienChuNhiemInfos.Count() != 0)
            {
                giaoVienChuNhiemInfos = giaoVienChuNhiemInfos.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }

            return giaoVienChuNhiemInfos.ToList();
        }


        public double GetGVCNInfoCount(int maNamHoc)
        {
            IQueryable<GVCNInfo> giaoVienChuNhiemInfos = from gvcn in db.LopHoc_GVCNs
                                                         join lophoc in db.LopHoc_Lops
                                                            on gvcn.MaLopHoc equals lophoc.MaLopHoc
                                                         join giaoVien in db.LopHoc_GiaoViens
                                                            on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                                                         where lophoc.MaNamHoc == maNamHoc
                                                         select new GVCNInfo
                                                         {
                                                             MaGVCN = giaoVien.MaGVCN,
                                                             MaGVCNHienThi = giaoVien.MaHienThiGVCN,
                                                             TenGVCN = giaoVien.HoTen,
                                                             NgaySinh = giaoVien.NgaySinh,
                                                             GioiTinh = giaoVien.GioiTinh,
                                                             DiaChi = giaoVien.DiaChi,
                                                             DienThoai = giaoVien.DienThoai,
                                                             MaLopHoc = lophoc.MaLopHoc,
                                                             TenLopHoc = lophoc.TenLopHoc
                                                         };
            return giaoVienChuNhiemInfos.Count();
        }

        public double GetGVCNInfoByNganhHocCount(int maNamHoc, int maNganhHoc)
        {
            IQueryable<GVCNInfo> giaoVienChuNhiemInfos = from giaoVien in db.LopHoc_GVCNs
                                                         join gvcn in db.LopHoc_GVCNs 
                                                            on giaoVien.MaGVCN equals gvcn.MaGVCN
                                                         join lophoc in db.LopHoc_Lops 
                                                            on gvcn.MaLopHoc equals lophoc.MaLopHoc
                                                         where lophoc.MaNamHoc == maNamHoc && lophoc.MaNganhHoc == maNganhHoc
                                                         select new GVCNInfo
                                                         {
                                                             MaGVCN = giaoVien.MaGVCN,
                                                             MaGVCNHienThi = giaoVien.MaHienThiGVCN,
                                                             TenGVCN = giaoVien.HoTen,
                                                             NgaySinh = giaoVien.NgaySinh,
                                                             GioiTinh = giaoVien.GioiTinh,
                                                             DiaChi = giaoVien.DiaChi,
                                                             DienThoai = giaoVien.DienThoai,
                                                             MaLopHoc = lophoc.MaLopHoc,
                                                             TenLopHoc = lophoc.TenLopHoc
                                                         };

            return giaoVienChuNhiemInfos.Count();
        }

        public double GetGVCNInfoByKhoiLopCount(int maNamHoc, int maKhoiLop)
        {
            IQueryable<GVCNInfo> giaoVienChuNhiemInfos = from giaoVien in db.LopHoc_GVCNs
                                                         join gvcn in db.LopHoc_GVCNs
                                                            on giaoVien.MaGVCN equals gvcn.MaGVCN
                                                         join lophoc in db.LopHoc_Lops
                                                            on gvcn.MaLopHoc equals lophoc.MaLopHoc
                                                         where lophoc.MaNamHoc == maNamHoc && lophoc.MaKhoiLop == maKhoiLop
                                                         select new GVCNInfo
                                                         {
                                                             MaGVCN = gvcn.MaGVCN,
                                                             MaGVCNHienThi = giaoVien.MaHienThiGVCN,
                                                             TenGVCN = giaoVien.HoTen,
                                                             NgaySinh = giaoVien.NgaySinh,
                                                             GioiTinh = giaoVien.GioiTinh,
                                                             DiaChi = giaoVien.DiaChi,
                                                             DienThoai = giaoVien.DienThoai,
                                                             MaLopHoc = lophoc.MaLopHoc,
                                                             TenLopHoc = lophoc.TenLopHoc
                                                         };
            
            return giaoVienChuNhiemInfos.Count();
        }

        public double GetGVCNInfoCount(int maNamHoc, int maNganhHoc, int maKhoiLop)
        {
            IQueryable<GVCNInfo> giaoVienChuNhiemInfos = from giaoVien in db.LopHoc_GVCNs
                                                         join gvcn in db.LopHoc_GVCNs
                                                            on giaoVien.MaGVCN equals gvcn.MaGVCN
                                                         join lophoc in db.LopHoc_Lops
                                                            on gvcn.MaLopHoc equals lophoc.MaLopHoc
                                                         where lophoc.MaNamHoc == maNamHoc 
                                                            && lophoc.MaNganhHoc == maNganhHoc && lophoc.MaKhoiLop == maKhoiLop
                                                         select new GVCNInfo
                                                         {
                                                             MaGVCN = gvcn.MaGVCN,
                                                             MaGVCNHienThi = giaoVien.MaHienThiGVCN,
                                                             TenGVCN = giaoVien.HoTen,
                                                             NgaySinh = giaoVien.NgaySinh,
                                                             GioiTinh = giaoVien.GioiTinh,
                                                             DiaChi = giaoVien.DiaChi,
                                                             DienThoai = giaoVien.DienThoai,
                                                             MaLopHoc = gvcn.MaLopHoc,
                                                             TenLopHoc = lophoc.TenLopHoc
                                                         };

            return giaoVienChuNhiemInfos.Count();
        }

        public double GetGVCNInfoCount(int maNamHoc, string tenGVCN)
        {
            IQueryable<GVCNInfo> giaoVienChuNhiemInfos = from giaoVien in db.LopHoc_GVCNs
                                                         join gvcn in db.LopHoc_GVCNs
                                                            on giaoVien.MaGVCN equals gvcn.MaGVCN
                                                         join lophoc in db.LopHoc_Lops
                                                            on gvcn.MaLopHoc equals lophoc.MaLopHoc
                                                         where lophoc.MaNamHoc == maNamHoc && giaoVien.HoTen == tenGVCN
                                                         select new GVCNInfo
                                                         {
                                                             MaGVCN = gvcn.MaGVCN,
                                                             MaGVCNHienThi = giaoVien.MaHienThiGVCN,
                                                             TenGVCN = giaoVien.HoTen,
                                                             NgaySinh = giaoVien.NgaySinh,
                                                             GioiTinh = giaoVien.GioiTinh,
                                                             DiaChi = giaoVien.DiaChi,
                                                             DienThoai = giaoVien.DienThoai,
                                                             MaLopHoc = gvcn.MaLopHoc,
                                                             TenLopHoc = lophoc.TenLopHoc
                                                         };            
            return giaoVienChuNhiemInfos.Count();
        }

        public double GetGVCNInfoCount(int maNamHoc, int maNganhHoc, int maKhoiLop, string tenGVCN)
        {
            IQueryable<GVCNInfo> giaoVienChuNhiemInfos = from giaoVien in db.LopHoc_GVCNs
                                                         join gvcn in db.LopHoc_GVCNs
                                                            on giaoVien.MaGVCN equals gvcn.MaGVCN
                                                         join lophoc in db.LopHoc_Lops
                                                            on gvcn.MaLopHoc equals lophoc.MaLopHoc
                                                         where lophoc.MaNamHoc == maNamHoc && lophoc.MaNganhHoc == maNganhHoc
                                                            && lophoc.MaKhoiLop == maKhoiLop && giaoVien.HoTen == tenGVCN
                                                         select new GVCNInfo
                                                         {
                                                             MaGVCN = gvcn.MaGVCN,
                                                             MaGVCNHienThi = giaoVien.MaHienThiGVCN,
                                                             TenGVCN = giaoVien.HoTen,
                                                             NgaySinh = giaoVien.NgaySinh,
                                                             GioiTinh = giaoVien.GioiTinh,
                                                             DiaChi = giaoVien.DiaChi,
                                                             DienThoai = giaoVien.DienThoai,
                                                             MaLopHoc = gvcn.MaLopHoc,
                                                             TenLopHoc = lophoc.TenLopHoc
                                                         };
            
            return giaoVienChuNhiemInfos.Count();
        }

        public double GetGVCNInfoByNganhHocCount(int maNamHoc, int maNganhHoc, string tenGVCN)
        {
            IQueryable<GVCNInfo> giaoVienChuNhiemInfos = from giaoVien in db.LopHoc_GVCNs
                                                         join gvcn in db.LopHoc_GVCNs
                                                            on giaoVien.MaGVCN equals gvcn.MaGVCN
                                                         join lophoc in db.LopHoc_Lops
                                                            on gvcn.MaLopHoc equals lophoc.MaLopHoc
                                                         where lophoc.MaNamHoc == maNamHoc 
                                                            && lophoc.MaNganhHoc == maNganhHoc 
                                                            && giaoVien.HoTen == tenGVCN
                                                         select new GVCNInfo
                                                         {
                                                             MaGVCN = gvcn.MaGVCN,
                                                             MaGVCNHienThi = giaoVien.MaHienThiGVCN,
                                                             TenGVCN = giaoVien.HoTen,
                                                             NgaySinh = giaoVien.NgaySinh,
                                                             GioiTinh = giaoVien.GioiTinh,
                                                             DiaChi = giaoVien.DiaChi,
                                                             DienThoai = giaoVien.DienThoai,
                                                             MaLopHoc = gvcn.MaLopHoc,
                                                             TenLopHoc = lophoc.TenLopHoc
                                                         };           

            return giaoVienChuNhiemInfos.Count();
        }

        public double GetGVCNInfoByKhoiLopCount(int maNamHoc, int maKhoiLop, string tenGVCN)
        {
            IQueryable<GVCNInfo> giaoVienChuNhiemInfos = from giaoVien in db.LopHoc_GVCNs
                                                         join gvcn in db.LopHoc_GVCNs
                                                            on giaoVien.MaGVCN equals gvcn.MaGVCN
                                                         join lophoc in db.LopHoc_Lops
                                                            on gvcn.MaLopHoc equals lophoc.MaLopHoc
                                                         where lophoc.MaNamHoc == maNamHoc 
                                                            && lophoc.MaKhoiLop == maKhoiLop 
                                                            && giaoVien.HoTen == tenGVCN
                                                         select new GVCNInfo
                                                         {
                                                             MaGVCN = gvcn.MaGVCN,
                                                             MaGVCNHienThi = giaoVien.MaHienThiGVCN,
                                                             TenGVCN = giaoVien.HoTen,
                                                             NgaySinh = giaoVien.NgaySinh,
                                                             GioiTinh = giaoVien.GioiTinh,
                                                             DiaChi = giaoVien.DiaChi,
                                                             DienThoai = giaoVien.DienThoai,
                                                             MaLopHoc = gvcn.MaLopHoc,
                                                             TenLopHoc = lophoc.TenLopHoc
                                                         };
            
            return giaoVienChuNhiemInfos.Count();
        }
    }
}
