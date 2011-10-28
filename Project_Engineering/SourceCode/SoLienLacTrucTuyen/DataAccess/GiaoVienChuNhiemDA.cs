using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class GiaoVienChuNhiemDA : BaseDA
    {
        public GiaoVienChuNhiemDA()
            : base()
        {

        }

        #region Insert, Delete, Update
        public void Insert(int maLopHoc, int maGiaoVien)
        {
            LopHoc_GVCN gvcn = new LopHoc_GVCN
            {
                MaLopHoc = maLopHoc,
                MaGiaoVien = maGiaoVien
            };

            db.LopHoc_GVCNs.InsertOnSubmit(gvcn);
            db.SubmitChanges();
        }

        public void Update(int maGVCN, int maGiaoVien)
        {
            LopHoc_GVCN giaoVienChuNhiem = (from gvcn in db.LopHoc_GVCNs
                                            where gvcn.MaGVCN == maGVCN
                                            select gvcn).First();

            giaoVienChuNhiem.MaGiaoVien = maGiaoVien;
            db.SubmitChanges();
        }

        public void Delete(int maGVCN)
        {
            LopHoc_GVCN gvcn = (from gv in db.LopHoc_GVCNs
                                where gv.MaGVCN == maGVCN
                                select gv).First();
            db.LopHoc_GVCNs.DeleteOnSubmit(gvcn);
            db.SubmitChanges();
        }
        #endregion 

        #region Get List of GiaoVienChuNhiem
        public List<TabularGiaoVienChuNhiem> GetListTbGiaoVienChuNhiems(
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVienChuNhiem> TbGVCNs;
            TbGVCNs = from gvcn in db.LopHoc_GVCNs
                      join lopHoc in db.LopHoc_Lops 
                        on gvcn.MaLopHoc equals lopHoc.MaLopHoc
                      join giaoVien in db.LopHoc_GiaoViens 
                      on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                      select new TabularGiaoVienChuNhiem
                      {
                          MaGVCN = gvcn.MaGVCN,
                          MaGiaoVien = gvcn.MaGiaoVien,
                          TenGiaoVien = giaoVien.HoTen,
                          MaLopHoc = gvcn.MaLopHoc,
                          TenLopHoc = lopHoc.TenLopHoc
                      };
            totalRecords = TbGVCNs.Count();
            if (totalRecords != 0)
            {
                return TbGVCNs.OrderBy(giaoVien => giaoVien.MaGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularGiaoVienChuNhiem>();
            }
        }

        public List<TabularGiaoVienChuNhiem> GetListTbGiaoVienChuNhiems(
            int maLopHoc,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVienChuNhiem> TbGVCNs;
            TbGVCNs = from gvcn in db.LopHoc_GVCNs
                      join lopHoc in db.LopHoc_Lops
                        on gvcn.MaLopHoc equals lopHoc.MaLopHoc
                      join giaoVien in db.LopHoc_GiaoViens
                        on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                      where gvcn.MaLopHoc == maLopHoc
                      select new TabularGiaoVienChuNhiem
                      {
                          MaGVCN = gvcn.MaGVCN,
                          MaGiaoVien = gvcn.MaGiaoVien,
                          TenGiaoVien = giaoVien.HoTen,
                          MaLopHoc = gvcn.MaLopHoc,
                          TenLopHoc = lopHoc.TenLopHoc
                      };
            totalRecords = TbGVCNs.Count();
            if (totalRecords != 0)
            {
                return TbGVCNs.OrderBy(giaoVien => giaoVien.MaGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularGiaoVienChuNhiem>();
            }
        }

        public List<TabularGiaoVienChuNhiem> GetListTbGiaoVienChuNhiems(
            int maLopHoc, string maGiaoVien, string tenGiaoVien,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVienChuNhiem> TbGVCNs;
            TbGVCNs = from gvcn in db.LopHoc_GVCNs
                      join lopHoc in db.LopHoc_Lops
                        on gvcn.MaLopHoc equals lopHoc.MaLopHoc
                      join giaoVien in db.LopHoc_GiaoViens
                        on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                      where gvcn.MaLopHoc == maLopHoc && giaoVien.MaHienThiGiaoVien == maGiaoVien && giaoVien.HoTen == tenGiaoVien
                      select new TabularGiaoVienChuNhiem
                      {
                          MaGVCN = gvcn.MaGVCN,
                          MaGiaoVien = gvcn.MaGiaoVien,
                          TenGiaoVien = giaoVien.HoTen,
                          MaLopHoc = gvcn.MaLopHoc,
                          TenLopHoc = lopHoc.TenLopHoc
                      };
            totalRecords = TbGVCNs.Count();
            if (totalRecords != 0)
            {
                return TbGVCNs.OrderBy(giaoVien => giaoVien.MaGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularGiaoVienChuNhiem>();
            }
        }              

        public List<TabularGiaoVienChuNhiem> GetListTbGiaoVienChuNhiemsByTen(
            int maLopHoc, string tenGiaoVien, 
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVienChuNhiem> TbGVCNs;
            TbGVCNs = from gvcn in db.LopHoc_GVCNs
                      join lopHoc in db.LopHoc_Lops
                        on gvcn.MaLopHoc equals lopHoc.MaLopHoc
                      join giaoVien in db.LopHoc_GiaoViens
                        on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                      where gvcn.MaLopHoc == maLopHoc && giaoVien.HoTen == tenGiaoVien
                      select new TabularGiaoVienChuNhiem
                      {
                          MaGVCN = gvcn.MaGVCN,
                          MaGiaoVien = gvcn.MaGiaoVien,
                          TenGiaoVien = giaoVien.HoTen,
                          MaLopHoc = gvcn.MaLopHoc,
                          TenLopHoc = lopHoc.TenLopHoc
                      };
            totalRecords = TbGVCNs.Count();
            if (totalRecords != 0)
            {
                return TbGVCNs.OrderBy(giaoVien => giaoVien.MaGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularGiaoVienChuNhiem>();
            }
        }

        public List<TabularGiaoVienChuNhiem> GetListTbGiaoVienChuNhiemsByMa(
            int maLopHoc, string maGiaoVien,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVienChuNhiem> TbGVCNs;
            TbGVCNs = from gvcn in db.LopHoc_GVCNs
                      join lopHoc in db.LopHoc_Lops
                        on gvcn.MaLopHoc equals lopHoc.MaLopHoc
                      join giaoVien in db.LopHoc_GiaoViens
                        on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                      where gvcn.MaLopHoc == maLopHoc && giaoVien.MaHienThiGiaoVien == maGiaoVien
                      select new TabularGiaoVienChuNhiem
                      {
                          MaGVCN = gvcn.MaGVCN,
                          MaGiaoVien = gvcn.MaGiaoVien,
                          TenGiaoVien = giaoVien.HoTen,
                          MaLopHoc = gvcn.MaLopHoc,
                          TenLopHoc = lopHoc.TenLopHoc
                      };
            totalRecords = TbGVCNs.Count();
            if (totalRecords != 0)
            {
                return TbGVCNs.OrderBy(giaoVien => giaoVien.MaGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularGiaoVienChuNhiem>();
            }
        }

        public List<TabularGiaoVienChuNhiem> GetListTbGiaoVienChuNhiemsByNam(
            int maNamHoc,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVienChuNhiem> TbGVCNs;
            TbGVCNs = from gvcn in db.LopHoc_GVCNs
                      join lopHoc in db.LopHoc_Lops
                        on gvcn.MaLopHoc equals lopHoc.MaLopHoc
                      join giaoVien in db.LopHoc_GiaoViens
                        on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                      where lopHoc.MaNamHoc == maNamHoc
                      select new TabularGiaoVienChuNhiem
                      {
                          MaGVCN = gvcn.MaGVCN,
                          MaGiaoVien = gvcn.MaGiaoVien,
                          TenGiaoVien = giaoVien.HoTen,
                          MaLopHoc = gvcn.MaLopHoc,
                          TenLopHoc = lopHoc.TenLopHoc
                      };
            totalRecords = TbGVCNs.Count();
            if (totalRecords != 0)
            {
                return TbGVCNs.OrderBy(giaoVien => giaoVien.MaGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularGiaoVienChuNhiem>();
            }
        }

        public List<TabularGiaoVienChuNhiem> GetListTbGiaoVienChuNhiemsByNam(
            int maNamHoc, int maNganhHoc, int maKhoiLop,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVienChuNhiem> TbGVCNs;
            TbGVCNs = from gvcn in db.LopHoc_GVCNs
                      join lopHoc in db.LopHoc_Lops
                        on gvcn.MaLopHoc equals lopHoc.MaLopHoc
                      join giaoVien in db.LopHoc_GiaoViens
                        on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                      where lopHoc.MaNamHoc == maNamHoc && lopHoc.MaNganhHoc == maNganhHoc && lopHoc.MaKhoiLop == maKhoiLop
                      select new TabularGiaoVienChuNhiem
                      {
                          MaGVCN = gvcn.MaGVCN,
                          MaGiaoVien = gvcn.MaGiaoVien,
                          TenGiaoVien = giaoVien.HoTen,
                          MaLopHoc = gvcn.MaLopHoc,
                          TenLopHoc = lopHoc.TenLopHoc
                      };
            totalRecords = TbGVCNs.Count();
            if (totalRecords != 0)
            {
                return TbGVCNs.OrderBy(giaoVien => giaoVien.MaGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularGiaoVienChuNhiem>();
            }
        }

        public List<TabularGiaoVienChuNhiem> GetListTbGiaoVienChuNhiemsByNam(
            int maNamHoc, string maGiaoVien, string tenGiaoVien,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVienChuNhiem> TbGVCNs;
            TbGVCNs = from gvcn in db.LopHoc_GVCNs
                      join lopHoc in db.LopHoc_Lops
                        on gvcn.MaLopHoc equals lopHoc.MaLopHoc
                      join giaoVien in db.LopHoc_GiaoViens
                        on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                      where lopHoc.MaNamHoc == maNamHoc && giaoVien.MaHienThiGiaoVien == maGiaoVien && giaoVien.HoTen == tenGiaoVien
                      select new TabularGiaoVienChuNhiem
                      {
                          MaGVCN = gvcn.MaGVCN,
                          MaGiaoVien = gvcn.MaGiaoVien,
                          TenGiaoVien = giaoVien.HoTen,
                          MaLopHoc = gvcn.MaLopHoc,
                          TenLopHoc = lopHoc.TenLopHoc
                      };
            totalRecords = TbGVCNs.Count();
            if (totalRecords != 0)
            {
                return TbGVCNs.OrderBy(giaoVien => giaoVien.MaGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularGiaoVienChuNhiem>();
            }
        }

        public List<TabularGiaoVienChuNhiem> GetListTbGiaoVienChuNhiemsByNam(
            int maNamHoc, int maNganhHoc, int maKhoiLop, string maGiaoVien, string tenGiaoVien,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVienChuNhiem> TbGVCNs;
            TbGVCNs = from gvcn in db.LopHoc_GVCNs
                      join lopHoc in db.LopHoc_Lops
                        on gvcn.MaLopHoc equals lopHoc.MaLopHoc
                      join giaoVien in db.LopHoc_GiaoViens
                        on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                      where lopHoc.MaNamHoc == maNamHoc 
                        && lopHoc.MaNganhHoc == maNganhHoc && lopHoc.MaKhoiLop == maKhoiLop 
                        && giaoVien.HoTen == tenGiaoVien && giaoVien.MaHienThiGiaoVien == maGiaoVien
                      select new TabularGiaoVienChuNhiem
                      {
                          MaGVCN = gvcn.MaGVCN,
                          MaGiaoVien = gvcn.MaGiaoVien,
                          TenGiaoVien = giaoVien.HoTen,
                          MaLopHoc = gvcn.MaLopHoc,
                          TenLopHoc = lopHoc.TenLopHoc
                      };
            totalRecords = TbGVCNs.Count();
            if (totalRecords != 0)
            {
                return TbGVCNs.OrderBy(giaoVien => giaoVien.MaGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularGiaoVienChuNhiem>();
            }
        }        

        public List<TabularGiaoVienChuNhiem> GetListTbGiaoVienChuNhiemsByNamAndNganh(
            int maNamHoc, int maNganhHoc,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVienChuNhiem> TbGVCNs;
            TbGVCNs = from gvcn in db.LopHoc_GVCNs
                      join lopHoc in db.LopHoc_Lops
                        on gvcn.MaLopHoc equals lopHoc.MaLopHoc
                      join giaoVien in db.LopHoc_GiaoViens
                        on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                      where lopHoc.MaNamHoc == maNamHoc
                        && lopHoc.MaNganhHoc == maNganhHoc
                      select new TabularGiaoVienChuNhiem
                      {
                          MaGVCN = gvcn.MaGVCN,
                          MaGiaoVien = gvcn.MaGiaoVien,
                          TenGiaoVien = giaoVien.HoTen,
                          MaLopHoc = gvcn.MaLopHoc,
                          TenLopHoc = lopHoc.TenLopHoc
                      };
            totalRecords = TbGVCNs.Count();
            if (totalRecords != 0)
            {
                return TbGVCNs.OrderBy(giaoVien => giaoVien.MaGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularGiaoVienChuNhiem>();
            }
        }

        public List<TabularGiaoVienChuNhiem> GetListTbGiaoVienChuNhiemsByNamAndNganh(
            int maNamHoc, int maNganhHoc, string maGiaoVien, string tenGiaoVien,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVienChuNhiem> TbGVCNs;
            TbGVCNs = from gvcn in db.LopHoc_GVCNs
                      join lopHoc in db.LopHoc_Lops
                        on gvcn.MaLopHoc equals lopHoc.MaLopHoc
                      join giaoVien in db.LopHoc_GiaoViens
                        on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                      where lopHoc.MaNamHoc == maNamHoc
                        && lopHoc.MaNganhHoc == maNganhHoc
                        && giaoVien.HoTen == tenGiaoVien && giaoVien.MaHienThiGiaoVien == maGiaoVien
                      select new TabularGiaoVienChuNhiem
                      {
                          MaGVCN = gvcn.MaGVCN,
                          MaGiaoVien = gvcn.MaGiaoVien,
                          TenGiaoVien = giaoVien.HoTen,
                          MaLopHoc = gvcn.MaLopHoc,
                          TenLopHoc = lopHoc.TenLopHoc
                      };
            totalRecords = TbGVCNs.Count();
            if (totalRecords != 0)
            {
                return TbGVCNs.OrderBy(giaoVien => giaoVien.MaGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularGiaoVienChuNhiem>();
            }
        }

        public List<TabularGiaoVienChuNhiem> GetListTbGiaoVienChuNhiemsByNamAndTen(
            int maNamHoc, string tenGiaoVien,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVienChuNhiem> TbGVCNs;
            TbGVCNs = from gvcn in db.LopHoc_GVCNs
                      join lopHoc in db.LopHoc_Lops
                        on gvcn.MaLopHoc equals lopHoc.MaLopHoc
                      join giaoVien in db.LopHoc_GiaoViens
                        on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                      where lopHoc.MaNamHoc == maNamHoc 
                        && giaoVien.HoTen == tenGiaoVien
                      select new TabularGiaoVienChuNhiem
                      {
                          MaGVCN = gvcn.MaGVCN,
                          MaGiaoVien = gvcn.MaGiaoVien,
                          TenGiaoVien = giaoVien.HoTen,
                          MaLopHoc = gvcn.MaLopHoc,
                          TenLopHoc = lopHoc.TenLopHoc
                      };
            totalRecords = TbGVCNs.Count();
            if (totalRecords != 0)
            {
                return TbGVCNs.OrderBy(giaoVien => giaoVien.MaGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularGiaoVienChuNhiem>();
            }
        }

        public List<TabularGiaoVienChuNhiem> GetListTbGiaoVienChuNhiemsByNamAndTen(
            int maNamHoc, int maNganhHoc, int maKhoiLop, string tenGiaoVien,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVienChuNhiem> TbGVCNs;
            TbGVCNs = from gvcn in db.LopHoc_GVCNs
                      join lopHoc in db.LopHoc_Lops
                        on gvcn.MaLopHoc equals lopHoc.MaLopHoc
                      join giaoVien in db.LopHoc_GiaoViens
                        on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                      where lopHoc.MaNamHoc == maNamHoc
                        && lopHoc.MaNganhHoc == maNganhHoc && lopHoc.MaKhoiLop == maKhoiLop
                        && giaoVien.HoTen == tenGiaoVien
                      select new TabularGiaoVienChuNhiem
                      {
                          MaGVCN = gvcn.MaGVCN,
                          MaGiaoVien = gvcn.MaGiaoVien,
                          TenGiaoVien = giaoVien.HoTen,
                          MaLopHoc = gvcn.MaLopHoc,
                          TenLopHoc = lopHoc.TenLopHoc
                      };
            totalRecords = TbGVCNs.Count();
            if (totalRecords != 0)
            {
                return TbGVCNs.OrderBy(giaoVien => giaoVien.MaGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularGiaoVienChuNhiem>();
            }
        }

        public List<TabularGiaoVienChuNhiem> GetListTbGiaoVienChuNhiemsByNamAndMa(
            int maNamHoc, string maGiaoVien,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVienChuNhiem> TbGVCNs;
            TbGVCNs = from gvcn in db.LopHoc_GVCNs
                      join lopHoc in db.LopHoc_Lops
                        on gvcn.MaLopHoc equals lopHoc.MaLopHoc
                      join giaoVien in db.LopHoc_GiaoViens
                        on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                      where lopHoc.MaNamHoc == maNamHoc                     
                        && giaoVien.MaHienThiGiaoVien == maGiaoVien
                      select new TabularGiaoVienChuNhiem
                      {
                          MaGVCN = gvcn.MaGVCN,
                          MaGiaoVien = gvcn.MaGiaoVien,
                          TenGiaoVien = giaoVien.HoTen,
                          MaLopHoc = gvcn.MaLopHoc,
                          TenLopHoc = lopHoc.TenLopHoc
                      };
            totalRecords = TbGVCNs.Count();
            if (totalRecords != 0)
            {
                return TbGVCNs.OrderBy(giaoVien => giaoVien.MaGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularGiaoVienChuNhiem>();
            }
        }

        public List<TabularGiaoVienChuNhiem> GetListTbGiaoVienChuNhiemsByNamAndMa(
            int maNamHoc, int maNganhHoc, int maKhoiLop, string maGiaoVien,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVienChuNhiem> TbGVCNs;
            TbGVCNs = from gvcn in db.LopHoc_GVCNs
                      join lopHoc in db.LopHoc_Lops
                        on gvcn.MaLopHoc equals lopHoc.MaLopHoc
                      join giaoVien in db.LopHoc_GiaoViens
                        on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                      where lopHoc.MaNamHoc == maNamHoc
                        && lopHoc.MaNganhHoc == maNganhHoc && lopHoc.MaKhoiLop == maKhoiLop
                        && giaoVien.MaHienThiGiaoVien == maGiaoVien
                      select new TabularGiaoVienChuNhiem
                      {
                          MaGVCN = gvcn.MaGVCN,
                          MaGiaoVien = gvcn.MaGiaoVien,
                          TenGiaoVien = giaoVien.HoTen,
                          MaLopHoc = gvcn.MaLopHoc,
                          TenLopHoc = lopHoc.TenLopHoc
                      };
            totalRecords = TbGVCNs.Count();
            if (totalRecords != 0)
            {
                return TbGVCNs.OrderBy(giaoVien => giaoVien.MaGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularGiaoVienChuNhiem>();
            }
        }

        public List<TabularGiaoVienChuNhiem> GetListTbGiaoVienChuNhiemsByNamAndNganhAndTen(
            int maNamHoc, int maNganhHoc, string tenGiaoVien,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVienChuNhiem> TbGVCNs;
            TbGVCNs = from gvcn in db.LopHoc_GVCNs
                      join lopHoc in db.LopHoc_Lops
                        on gvcn.MaLopHoc equals lopHoc.MaLopHoc
                      join giaoVien in db.LopHoc_GiaoViens
                        on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                      where lopHoc.MaNamHoc == maNamHoc
                        && lopHoc.MaNganhHoc == maNganhHoc
                        && giaoVien.HoTen == tenGiaoVien
                      select new TabularGiaoVienChuNhiem
                      {
                          MaGVCN = gvcn.MaGVCN,
                          MaGiaoVien = gvcn.MaGiaoVien,
                          TenGiaoVien = giaoVien.HoTen,
                          MaLopHoc = gvcn.MaLopHoc,
                          TenLopHoc = lopHoc.TenLopHoc
                      };
            totalRecords = TbGVCNs.Count();
            if (totalRecords != 0)
            {
                return TbGVCNs.OrderBy(giaoVien => giaoVien.MaGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularGiaoVienChuNhiem>();
            }
        }

        public List<TabularGiaoVienChuNhiem> GetListTbGiaoVienChuNhiemsByNamAndNganhAndMa(
            int maNamHoc, int maNganhHoc, string maGiaoVien,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVienChuNhiem> TbGVCNs;
            TbGVCNs = from gvcn in db.LopHoc_GVCNs
                      join lopHoc in db.LopHoc_Lops
                        on gvcn.MaLopHoc equals lopHoc.MaLopHoc
                      join giaoVien in db.LopHoc_GiaoViens
                        on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                      where lopHoc.MaNamHoc == maNamHoc
                        && lopHoc.MaNganhHoc == maNganhHoc
                        && giaoVien.MaHienThiGiaoVien == maGiaoVien
                      select new TabularGiaoVienChuNhiem
                      {
                          MaGVCN = gvcn.MaGVCN,
                          MaGiaoVien = gvcn.MaGiaoVien,
                          TenGiaoVien = giaoVien.HoTen,
                          MaLopHoc = gvcn.MaLopHoc,
                          TenLopHoc = lopHoc.TenLopHoc
                      };
            totalRecords = TbGVCNs.Count();
            if (totalRecords != 0)
            {
                return TbGVCNs.OrderBy(giaoVien => giaoVien.MaGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularGiaoVienChuNhiem>();
            }
        }

        public List<TabularGiaoVienChuNhiem> GetListTbGiaoVienChuNhiemsByNamAndKhoi(
            int maNamHoc, int maKhoiLop,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVienChuNhiem> TbGVCNs;
            TbGVCNs = from gvcn in db.LopHoc_GVCNs
                      join lopHoc in db.LopHoc_Lops
                        on gvcn.MaLopHoc equals lopHoc.MaLopHoc
                      join giaoVien in db.LopHoc_GiaoViens
                        on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                      where lopHoc.MaNamHoc == maNamHoc
                        && lopHoc.MaKhoiLop == maKhoiLop                        
                      select new TabularGiaoVienChuNhiem
                      {
                          MaGVCN = gvcn.MaGVCN,
                          MaGiaoVien = gvcn.MaGiaoVien,
                          TenGiaoVien = giaoVien.HoTen,
                          MaLopHoc = gvcn.MaLopHoc,
                          TenLopHoc = lopHoc.TenLopHoc
                      };
            totalRecords = TbGVCNs.Count();
            if (totalRecords != 0)
            {
                return TbGVCNs.OrderBy(giaoVien => giaoVien.MaGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularGiaoVienChuNhiem>();
            }
        }

        public List<TabularGiaoVienChuNhiem> GetListTbGiaoVienChuNhiemsByNamAndKhoi(
            int maNamHoc, int maKhoiLop, string maGiaoVien, string tenGiaoVien,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVienChuNhiem> TbGVCNs;
            TbGVCNs = from gvcn in db.LopHoc_GVCNs
                      join lopHoc in db.LopHoc_Lops
                        on gvcn.MaLopHoc equals lopHoc.MaLopHoc
                      join giaoVien in db.LopHoc_GiaoViens
                        on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                      where lopHoc.MaNamHoc == maNamHoc
                        && lopHoc.MaKhoiLop == maKhoiLop
                        && giaoVien.HoTen == tenGiaoVien && giaoVien.MaHienThiGiaoVien == maGiaoVien
                      select new TabularGiaoVienChuNhiem
                      {
                          MaGVCN = gvcn.MaGVCN,
                          MaGiaoVien = gvcn.MaGiaoVien,
                          TenGiaoVien = giaoVien.HoTen,
                          MaLopHoc = gvcn.MaLopHoc,
                          TenLopHoc = lopHoc.TenLopHoc
                      };
            totalRecords = TbGVCNs.Count();
            if (totalRecords != 0)
            {
                return TbGVCNs.OrderBy(giaoVien => giaoVien.MaGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularGiaoVienChuNhiem>();
            }
        }

        public List<TabularGiaoVienChuNhiem> GetListTbGiaoVienChuNhiemsByNamAndKhoiAndTen(
            int maNamHoc, int maKhoiLop, string tenGiaoVien,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVienChuNhiem> TbGVCNs;
            TbGVCNs = from gvcn in db.LopHoc_GVCNs
                      join lopHoc in db.LopHoc_Lops
                        on gvcn.MaLopHoc equals lopHoc.MaLopHoc
                      join giaoVien in db.LopHoc_GiaoViens
                        on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                      where lopHoc.MaNamHoc == maNamHoc
                        && lopHoc.MaKhoiLop == maKhoiLop
                        && giaoVien.HoTen == tenGiaoVien
                      select new TabularGiaoVienChuNhiem
                      {
                          MaGVCN = gvcn.MaGVCN,
                          MaGiaoVien = gvcn.MaGiaoVien,
                          TenGiaoVien = giaoVien.HoTen,
                          MaLopHoc = gvcn.MaLopHoc,
                          TenLopHoc = lopHoc.TenLopHoc
                      };
            totalRecords = TbGVCNs.Count();
            if (totalRecords != 0)
            {
                return TbGVCNs.OrderBy(giaoVien => giaoVien.MaGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularGiaoVienChuNhiem>();
            }
        }

        public List<TabularGiaoVienChuNhiem> GetListTbGiaoVienChuNhiemsByNamAndKhoiAndMa(
            int maNamHoc, int maKhoiLop, string maGiaoVien,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularGiaoVienChuNhiem> TbGVCNs;
            TbGVCNs = from gvcn in db.LopHoc_GVCNs
                      join lopHoc in db.LopHoc_Lops
                        on gvcn.MaLopHoc equals lopHoc.MaLopHoc
                      join giaoVien in db.LopHoc_GiaoViens
                        on gvcn.MaGiaoVien equals giaoVien.MaGiaoVien
                      where lopHoc.MaNamHoc == maNamHoc
                        && lopHoc.MaKhoiLop == maKhoiLop
                        && giaoVien.MaHienThiGiaoVien == maGiaoVien
                      select new TabularGiaoVienChuNhiem
                      {
                          MaGVCN = gvcn.MaGVCN,
                          MaGiaoVien = gvcn.MaGiaoVien,
                          TenGiaoVien = giaoVien.HoTen,
                          MaLopHoc = gvcn.MaLopHoc,
                          TenLopHoc = lopHoc.TenLopHoc
                      };
            totalRecords = TbGVCNs.Count();
            if (totalRecords != 0)
            {
                return TbGVCNs.OrderBy(giaoVien => giaoVien.MaGiaoVien)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularGiaoVienChuNhiem>();
            }
        }
        #endregion

        public LopHoc_GVCN GetGiaoVienChuNhiem(int maGVCN)
        {
            LopHoc_GVCN giaoVienChuNhiem = (from gvcn in db.LopHoc_GVCNs
                                           where gvcn.MaGVCN == maGVCN
                                           select gvcn).First();
            return giaoVienChuNhiem;
        }        
    }
}
