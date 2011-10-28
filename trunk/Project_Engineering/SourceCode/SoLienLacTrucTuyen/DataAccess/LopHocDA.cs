using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class LopHocDA : BaseDA
    {
        public LopHocDA()
            : base()
        {
        }

        #region Insert, Delete, Update
        public void InsertLopHoc(LopHoc_Lop lop)
        {
            db.LopHoc_Lops.InsertOnSubmit(lop);
            db.SubmitChanges();
        }

        public void UpdateLopHoc(LopHoc_Lop lop)
        {
            LopHoc_Lop modifyinglophoc = GetLopHoc(lop.MaLopHoc);
            modifyinglophoc.TenLopHoc = lop.TenLopHoc;
            db.SubmitChanges();
        }

        public void DeleteLopHoc(LopHoc_Lop lop)
        {
            IQueryable<LopHoc_GVCN> giaoVienChuNhiems;
            giaoVienChuNhiems = from gvcn in db.LopHoc_GVCNs
                                where gvcn.MaLopHoc == lop.MaLopHoc
                                select gvcn;

            if (giaoVienChuNhiems.Count() != 0)
            {
                db.LopHoc_GVCNs.DeleteOnSubmit(giaoVienChuNhiems.First());
            }

            db.LopHoc_Lops.DeleteOnSubmit(lop);
            db.SubmitChanges();
        }
        #endregion

        #region Get List LopHoc_Lop
        public LopHoc_Lop GetLopHoc(int maLopHoc)
        {
            IQueryable<LopHoc_Lop> lophoc = from l in db.LopHoc_Lops
                                            where l.MaLopHoc == maLopHoc
                                            select l;
            if (lophoc.Count() != 0)
            {
                return lophoc.First();
            }
            else
            {
                return null;
            }
        }

        public List<LopHoc_Lop> GetListLopHoc(int maNamHoc)
        {
            IQueryable<LopHoc_Lop> lops = from l in db.LopHoc_Lops
                                          where l.MaNamHoc == maNamHoc
                                          select l;
            return lops.ToList();
        }

        public List<LopHoc_Lop> GetListLopHoc(int maNganhHoc, int maKhoiLop, int maNamHoc)
        {
            IQueryable<LopHoc_Lop> lophoc = (from l in db.LopHoc_Lops
                                             where l.MaNganhHoc == maNganhHoc && l.MaKhoiLop == maKhoiLop && l.MaNamHoc == maNamHoc
                                             select l).OrderBy(l => l.TenLopHoc);
            return lophoc.ToList();
        }

        public List<LopHoc_Lop> GetListLopHocByNganhHoc(int maNganhHoc, int maNamHoc)
        {
            IQueryable<LopHoc_Lop> lophoc = (from l in db.LopHoc_Lops
                                             where l.MaNganhHoc == maNganhHoc && l.MaNamHoc == maNamHoc
                                             select l).OrderBy(l => l.TenLopHoc);
            return lophoc.ToList();
        }

        public List<LopHoc_Lop> GetListLopHocByKhoiLop(int maKhoiLop, int maNamHoc)
        {
            IQueryable<LopHoc_Lop> lophoc = (from l in db.LopHoc_Lops
                                             where l.MaKhoiLop == maKhoiLop && l.MaNamHoc == maNamHoc
                                             select l).OrderBy(l => l.TenLopHoc);
            return lophoc.ToList();
        }

       
        #endregion

        #region Get List LopHocInfo
        public LopHocInfo GetLopHocInfo(int maLopHoc)
        {
            IQueryable<LopHocInfo> lopHocInfos;
            lopHocInfos = from lop in db.LopHoc_Lops
                          join nganh in db.DanhMuc_NganhHocs 
                            on lop.MaNganhHoc equals nganh.MaNganhHoc
                          join khoi in db.DanhMuc_KhoiLops 
                            on lop.MaKhoiLop equals khoi.MaKhoiLop
                          join namHoc in db.CauHinh_NamHocs 
                            on lop.MaNamHoc equals namHoc.MaNamHoc
                          where lop.MaLopHoc == maLopHoc
                          select new LopHocInfo
                          {
                              MaLopHoc = lop.MaLopHoc,
                              TenLopHoc = lop.TenLopHoc,
                              MaNganhHoc = nganh.MaNganhHoc,
                              TenNganhHoc = nganh.TenNganhHoc,
                              MaKhoiLop = khoi.MaKhoiLop,
                              TenKhoiLop = khoi.TenKhoiLop,
                              SiSo = lop.SiSo,
                              MaNamHoc = lop.MaNamHoc,
                              TenNamHoc = namHoc.TenNamHoc
                          };

            if (lopHocInfos.Count() != 0)
            {
                LopHocInfo lopHocInfo = lopHocInfos.First();
                IQueryable<LopHoc_GiaoVien> giaoVienChuNhiems;
                giaoVienChuNhiems = from giaoVien in db.LopHoc_GiaoViens
                                    join gvcn in db.LopHoc_GVCNs
                                        on giaoVien.MaGiaoVien equals gvcn.MaGiaoVien
                                    where gvcn.MaLopHoc == lopHocInfo.MaLopHoc
                                    select giaoVien;
                if (giaoVienChuNhiems.Count() != 0)
                {
                    LopHoc_GiaoVien homeRoomTecher = giaoVienChuNhiems.First();
                    string tenGVCN = homeRoomTecher.HoTen;
                    lopHocInfo.TenGVCN = tenGVCN;
                    lopHocInfo.HomeroomTeacherCode = homeRoomTecher.MaGiaoVien;
                }

                return lopHocInfo;
            }
            else
            {
                return null;
            }
        }

        public List<LopHocInfo> GetListLopHocInfo(int maNamHoc,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<LopHocInfo> lstLopHocInfo = new List<LopHocInfo>();

            IQueryable<LopHocInfo> lopHocInfos = from lop in db.LopHoc_Lops
                                                 join nganh in db.DanhMuc_NganhHocs
                                                    on lop.MaNganhHoc equals nganh.MaNganhHoc
                                                 join khoi in db.DanhMuc_KhoiLops
                                                    on lop.MaKhoiLop equals khoi.MaKhoiLop
                                                 where lop.MaNamHoc == maNamHoc
                                                 select new LopHocInfo
                                                 {
                                                     MaLopHoc = lop.MaLopHoc,
                                                     TenLopHoc = lop.TenLopHoc,
                                                     TenNganhHoc = nganh.TenNganhHoc,
                                                     TenKhoiLop = khoi.TenKhoiLop,
                                                     SiSo = lop.SiSo
                                                 };
            totalRecords = lopHocInfos.Count();
            if (totalRecords != 0)
            {
                lopHocInfos = lopHocInfos.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize)
                    .OrderBy(lop => lop.TenNganhHoc).ThenBy(lop => lop.TenLopHoc).ThenBy(lop => lop.TenLopHoc);
                lstLopHocInfo = lopHocInfos.ToList();
                foreach (LopHocInfo lopHocInfo in lstLopHocInfo)
                {
                    IQueryable<LopHoc_GiaoVien> giaoVienChuNhiems;
                    giaoVienChuNhiems = from giaoVien in db.LopHoc_GiaoViens
                                        join gvcn in db.LopHoc_GVCNs
                                            on giaoVien.MaGiaoVien equals gvcn.MaGiaoVien
                                        where gvcn.MaLopHoc == lopHocInfo.MaLopHoc
                                        select giaoVien;
                    if (giaoVienChuNhiems.Count() != 0)
                    {
                        LopHoc_GiaoVien homeRoomTecher = giaoVienChuNhiems.First();
                        string tenGVCN = homeRoomTecher.HoTen;
                        lopHocInfo.TenGVCN = tenGVCN;
                        lopHocInfo.HomeroomTeacherCode = homeRoomTecher.MaGiaoVien;
                    }
                }
            }

            return lstLopHocInfo;
        }

        public List<LopHocInfo> GetListLopHocInfo(int maNganhHoc, int maKhoiLop, int maNamHoc,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<LopHocInfo> lopHocInfos = from lop in db.LopHoc_Lops
                                                 join nganh in db.DanhMuc_NganhHocs on lop.MaNganhHoc equals nganh.MaNganhHoc
                                                 join khoi in db.DanhMuc_KhoiLops on lop.MaKhoiLop equals khoi.MaKhoiLop
                                                 where lop.MaNamHoc == maNamHoc && lop.MaNganhHoc == maNganhHoc && lop.MaKhoiLop == maKhoiLop
                                                 select new LopHocInfo
                                                 {
                                                     MaLopHoc = lop.MaLopHoc,
                                                     TenLopHoc = lop.TenLopHoc,
                                                     TenNganhHoc = nganh.TenNganhHoc,
                                                     TenKhoiLop = khoi.TenKhoiLop,
                                                     SiSo = lop.SiSo
                                                 };
            totalRecords = lopHocInfos.Count();
            List<LopHocInfo> lstLopHocInfo = new List<LopHocInfo>();
            if (lopHocInfos.Count() != 0)
            {
                lopHocInfos = lopHocInfos.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
                lopHocInfos = lopHocInfos.OrderBy(lop => lop.TenNganhHoc).ThenBy(lop => lop.TenLopHoc).ThenBy(lop => lop.TenLopHoc);
                lstLopHocInfo = lopHocInfos.ToList();
                foreach (LopHocInfo lopHocInfo in lstLopHocInfo)
                {
                    IQueryable<LopHoc_GiaoVien> giaoVienChuNhiems;
                    giaoVienChuNhiems = from giaoVien in db.LopHoc_GiaoViens
                                        join gvcn in db.LopHoc_GVCNs
                                            on giaoVien.MaGiaoVien equals gvcn.MaGiaoVien
                                        where gvcn.MaLopHoc == lopHocInfo.MaLopHoc
                                        select giaoVien;
                    if (giaoVienChuNhiems.Count() != 0)
                    {
                        LopHoc_GiaoVien homeRoomTecher = giaoVienChuNhiems.First();
                        string tenGVCN = homeRoomTecher.HoTen;
                        lopHocInfo.TenGVCN = tenGVCN;
                        lopHocInfo.HomeroomTeacherCode = homeRoomTecher.MaGiaoVien;
                    }
                }
            }

            return lstLopHocInfo;
        }

        public List<LopHocInfo> GetListLopHocInfoByNganhHoc(int maNganhHoc, int maNamHoc,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<LopHocInfo> lopHocInfos = from lop in db.LopHoc_Lops
                                                 join nganh in db.DanhMuc_NganhHocs on lop.MaNganhHoc equals nganh.MaNganhHoc
                                                 join khoi in db.DanhMuc_KhoiLops on lop.MaKhoiLop equals khoi.MaKhoiLop
                                                 where lop.MaNamHoc == maNamHoc && lop.MaNganhHoc == maNganhHoc
                                                 select new LopHocInfo
                                                 {
                                                     MaLopHoc = lop.MaLopHoc,
                                                     TenLopHoc = lop.TenLopHoc,
                                                     TenNganhHoc = nganh.TenNganhHoc,
                                                     TenKhoiLop = khoi.TenKhoiLop,
                                                     SiSo = lop.SiSo
                                                 };
            totalRecords = lopHocInfos.Count();
            List<LopHocInfo> lstLopHocInfo = new List<LopHocInfo>();
            if (lopHocInfos.Count() != 0)
            {
                lopHocInfos = lopHocInfos.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
                lopHocInfos = lopHocInfos.OrderBy(lop => lop.TenNganhHoc).ThenBy(lop => lop.TenLopHoc).ThenBy(lop => lop.TenLopHoc);
                lstLopHocInfo = lopHocInfos.ToList();
                foreach (LopHocInfo lopHocInfo in lstLopHocInfo)
                {
                    IQueryable<LopHoc_GiaoVien> giaoVienChuNhiems;
                    giaoVienChuNhiems = from giaoVien in db.LopHoc_GiaoViens
                                        join gvcn in db.LopHoc_GVCNs
                                            on giaoVien.MaGiaoVien equals gvcn.MaGiaoVien
                                        where gvcn.MaLopHoc == lopHocInfo.MaLopHoc
                                        select giaoVien;
                    if (giaoVienChuNhiems.Count() != 0)
                    {
                        LopHoc_GiaoVien homeRoomTecher = giaoVienChuNhiems.First();
                        string tenGVCN = homeRoomTecher.HoTen;
                        lopHocInfo.TenGVCN = tenGVCN;
                        lopHocInfo.HomeroomTeacherCode = homeRoomTecher.MaGiaoVien;
                    }
                }
            }

            return lstLopHocInfo;
        }

        public List<LopHocInfo> GetListLopHocInfoByKhoiLop(int maKhoiLop, int maNamHoc,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<LopHocInfo> lopHocInfos = from lop in db.LopHoc_Lops
                                                 join nganh in db.DanhMuc_NganhHocs on lop.MaNganhHoc equals nganh.MaNganhHoc
                                                 join khoi in db.DanhMuc_KhoiLops on lop.MaKhoiLop equals khoi.MaKhoiLop
                                                 where lop.MaNamHoc == maNamHoc && lop.MaLopHoc == maKhoiLop
                                                 select new LopHocInfo
                                                 {
                                                     MaLopHoc = lop.MaLopHoc,
                                                     TenLopHoc = lop.TenLopHoc,
                                                     TenNganhHoc = nganh.TenNganhHoc,
                                                     TenKhoiLop = khoi.TenKhoiLop
                                                 };
            totalRecords = lopHocInfos.Count();
            List<LopHocInfo> lstLopHocInfo = new List<LopHocInfo>();
            if (lopHocInfos.Count() != 0)
            {
                lopHocInfos = lopHocInfos.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
                lopHocInfos = lopHocInfos.OrderBy(lop => lop.TenNganhHoc).ThenBy(lop => lop.TenLopHoc).ThenBy(lop => lop.TenLopHoc);
                lstLopHocInfo = lopHocInfos.ToList();
                foreach (LopHocInfo lopHocInfo in lstLopHocInfo)
                {
                    IQueryable<LopHoc_GiaoVien> giaoVienChuNhiems;
                    giaoVienChuNhiems = from giaoVien in db.LopHoc_GiaoViens
                                        join gvcn in db.LopHoc_GVCNs
                                            on giaoVien.MaGiaoVien equals gvcn.MaGiaoVien
                                        where gvcn.MaLopHoc == lopHocInfo.MaLopHoc
                                        select giaoVien;
                    if (giaoVienChuNhiems.Count() != 0)
                    {
                        LopHoc_GiaoVien homeRoomTecher = giaoVienChuNhiems.First();
                        string tenGVCN = homeRoomTecher.HoTen;
                        lopHocInfo.TenGVCN = tenGVCN;
                        lopHocInfo.HomeroomTeacherCode = homeRoomTecher.MaGiaoVien;
                    }
                }
            }

            return lstLopHocInfo;
        }
        #endregion

        public bool LopHocExists(string tenLopHoc, int maNamHoc)
        {
            IQueryable<LopHoc_Lop> lopHocs = from lop in db.LopHoc_Lops
                                             where lop.TenLopHoc == tenLopHoc
                                                && lop.MaNamHoc == maNamHoc
                                             select lop;
            if (lopHocs.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool LopHocExists(int maLopHoc, string tenLopHoc)
        {
            LopHoc_Lop lopHoc = (from lop in db.LopHoc_Lops
                                 where lop.MaLopHoc == maLopHoc
                                 select lop).First();

            IQueryable<LopHoc_Lop> NamHocBasedLopHocs = from lop in db.LopHoc_Lops
                                                        where lop.MaNamHoc == lopHoc.MaNamHoc
                                                            && lop.MaLopHoc != lopHoc.MaLopHoc
                                                            && lop.TenLopHoc == tenLopHoc
                                                        select lop;

            if (NamHocBasedLopHocs.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CanDeleteLopHoc(int maLopHoc)
        {
            //IQueryable<LopHoc_GVCN> gvcnLopHocs = from gvcn_lh in db.LopHoc_GVCNs
            //                                           where gvcn_lh.MaLopHoc == maLopHoc
            //                                           select gvcn_lh;
            //if (gvcnLopHocs.Count() != 0)
            //{
            //    return false;
            //}
            //else
            //{
            IQueryable<LopHoc_MonHocTKB> monHocTKBLops = from mhtkb_lh in db.LopHoc_MonHocTKBs
                                                         where mhtkb_lh.MaLopHoc == maLopHoc
                                                         select mhtkb_lh;
            if (monHocTKBLops.Count() != 0)
            {
                return false;
            }
            else
            {
                IQueryable<LopHoc_ThongBao> thongBaoLops = from tb_lh in db.LopHoc_ThongBaos
                                                           where tb_lh.MaLopHoc == maLopHoc
                                                           select tb_lh;
                if (thongBaoLops.Count() != 0)
                {
                    return false;
                }
                else
                {
                    IQueryable<HocSinh_HocSinhLopHoc> hocSinhLops = from hs_lh in db.HocSinh_HocSinhLopHocs
                                                                    where hs_lh.MaLopHoc == maLopHoc
                                                                    select hs_lh;
                    if (hocSinhLops.Count() != 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            //}
        }

        public bool HasGiaoVienChuNhiem(int maLopHoc)
        {
            IQueryable<LopHoc_GVCN> gvcns = from gvcn in db.LopHoc_GVCNs
                                            where gvcn.MaLopHoc == maLopHoc
                                            select gvcn;
            if (gvcns.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<LopHoc_Lop> GetListLopHocChuaCoGVCN(int maNamHoc)
        {
            IQueryable<LopHoc_Lop> lopHocs = from lop in db.LopHoc_Lops
                                             where lop.MaNamHoc == maNamHoc
                                             select lop;

            List<LopHoc_Lop> lstLopHocChuaCoGVCNs = new List<LopHoc_Lop>();

            foreach (LopHoc_Lop lop in lopHocs)
            {
                IQueryable<LopHoc_GVCN> giaoVienChuNhiems;
                giaoVienChuNhiems = from gvcn in db.LopHoc_GVCNs
                                    where gvcn.MaLopHoc == lop.MaLopHoc
                                    select gvcn;
                if (giaoVienChuNhiems.Count() == 0)
                {
                    lstLopHocChuaCoGVCNs.Add(new LopHoc_Lop
                    {
                        MaLopHoc = lop.MaLopHoc,
                        TenLopHoc = lop.TenLopHoc,
                        MaNganhHoc = lop.MaNganhHoc,
                        MaKhoiLop = lop.MaKhoiLop,
                        MaNamHoc = lop.MaNamHoc,
                        SiSo = lop.SiSo
                    });
                }
            }
            return lstLopHocChuaCoGVCNs;
        }

        public List<LopHoc_Lop> GetListLopHocChuaCoGVCN(int maNamHoc, int maNganhHoc, int maKhoiLop)
        {
            IQueryable<LopHoc_Lop> lopHocs = from lop in db.LopHoc_Lops
                                             where lop.MaNamHoc == maNamHoc 
                                                && lop.MaNganhHoc == maNganhHoc 
                                                && lop.MaKhoiLop == maKhoiLop
                                             select lop;
            if(lopHocs.Count() != 0)
            {
                List<LopHoc_Lop> lstLopHocs = lopHocs.ToList();
                int i = 0;
                while (i < lstLopHocs.Count)
                {
                    IQueryable<LopHoc_GVCN> giaoVienChuNhiems;
                    giaoVienChuNhiems = from gvcn in db.LopHoc_GVCNs
                                        where gvcn.MaLopHoc == lstLopHocs[i].MaLopHoc
                                        select gvcn;
                    if (giaoVienChuNhiems.Count() != 0)
                    {
                        lstLopHocs.Remove(lstLopHocs[i]);
                    }
                    else
                    {
                        i++;
                    }
                }

                return lstLopHocs;
            }
            else
            {
                return new List<LopHoc_Lop>();
            }
        }

        public List<LopHoc_Lop> GetListLopHocChuaCoGVCNByKhoi(int maNamHoc, int maKhoiLop)
        {
            IQueryable<LopHoc_Lop> lopHocs = from lop in db.LopHoc_Lops
                                             where lop.MaNamHoc == maNamHoc
                                                && lop.MaKhoiLop == maKhoiLop
                                             select lop;
            if (lopHocs.Count() != 0)
            {
                List<LopHoc_Lop> lstLopHocs = lopHocs.ToList();
                int i = 0;
                while (i < lstLopHocs.Count)
                {
                    IQueryable<LopHoc_GVCN> giaoVienChuNhiems;
                    giaoVienChuNhiems = from gvcn in db.LopHoc_GVCNs
                                        where gvcn.MaLopHoc == lstLopHocs[i].MaLopHoc
                                        select gvcn;
                    if (giaoVienChuNhiems.Count() != 0)
                    {
                        lstLopHocs.Remove(lstLopHocs[i]);
                    }
                    else
                    {
                        i++;
                    }
                }

                return lstLopHocs;
            }
            else
            {
                return new List<LopHoc_Lop>();
            }
        }

        public List<LopHoc_Lop> GetListLopHocChuaCoGVCNByNganh(int maNamHoc, int maNganhHoc)
        {
            IQueryable<LopHoc_Lop> lopHocs = from lop in db.LopHoc_Lops
                                             where lop.MaNamHoc == maNamHoc
                                                && lop.MaNganhHoc == maNganhHoc
                                             select lop;
            if (lopHocs.Count() != 0)
            {
                List<LopHoc_Lop> lstLopHocs = lopHocs.ToList();
                int i = 0;
                while (i < lstLopHocs.Count)
                {
                    IQueryable<LopHoc_GVCN> giaoVienChuNhiems;
                    giaoVienChuNhiems = from gvcn in db.LopHoc_GVCNs
                                        where gvcn.MaLopHoc == lstLopHocs[i].MaLopHoc
                                        select gvcn;
                    if (giaoVienChuNhiems.Count() != 0)
                    {
                        lstLopHocs.Remove(lstLopHocs[i]);
                    }
                    else
                    {
                        i++;
                    }
                }

                return lstLopHocs;
            }
            else
            {
                return new List<LopHoc_Lop>();
            }
        }
    }
}
