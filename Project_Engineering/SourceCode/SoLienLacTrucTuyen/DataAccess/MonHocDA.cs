using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class MonHocDA : BaseDA
    {
        public MonHocDA()
            : base()
        {
        }

        public void InsertMonHoc(DanhMuc_MonHoc monhoc)
        {
            db.DanhMuc_MonHocs.InsertOnSubmit(monhoc);
            db.SubmitChanges();
        }

        public void UpdateMonHoc(int maMonHoc, string tenMonHoc, double heSoDiem)
        {
            DanhMuc_MonHoc modifyingMonHoc = GetMonHoc(maMonHoc);
            modifyingMonHoc.TenMonHoc = tenMonHoc;
            modifyingMonHoc.HeSoDiem = heSoDiem;
            db.SubmitChanges();
        }

        public void DeleteMonHoc(DanhMuc_MonHoc monhoc)
        {
            db.DanhMuc_MonHocs.DeleteOnSubmit(monhoc);
            db.SubmitChanges();
        }

        public DanhMuc_MonHoc GetMonHoc(int maMonHoc)
        {
            IQueryable<DanhMuc_MonHoc> monHocs = from m in db.DanhMuc_MonHocs
                                                 where m.MaMonHoc == maMonHoc
                                                 select m;
            if (monHocs.Count() != 0)
            {
                return monHocs.First();
            }
            else
            {
                return null;
            }
        }

        public List<DanhMuc_MonHoc> GetListMonHoc()
        {
            var monHocs = from m in db.DanhMuc_MonHocs
                          select m;
            return monHocs.ToList();
        }

        public List<DanhMuc_MonHoc> GetListMonHoc(int maNganhHoc, int maKhoiLop)
        {
            IQueryable<DanhMuc_MonHoc> monHocs = from m in db.DanhMuc_MonHocs
                                                 where m.MaNganhHoc == maNganhHoc && m.MaKhoiLop == maKhoiLop
                                                 select m;
            monHocs = monHocs.OrderBy(m => m.TenMonHoc);
            return monHocs.ToList();
        }

        public List<DanhMuc_MonHoc> GetListMonHocByNganhHoc(int maNganhHoc)
        {
            IQueryable<DanhMuc_MonHoc> monHocs = from m in db.DanhMuc_MonHocs
                                                 where m.MaNganhHoc == maNganhHoc
                                                 select m;
            monHocs = monHocs.OrderBy(m => m.TenMonHoc);
            return monHocs.ToList();
        }

        public List<DanhMuc_MonHoc> GetListMonHocByKhoiLop(int maKhoiLop)
        {
            IQueryable<DanhMuc_MonHoc> monHocs = from m in db.DanhMuc_MonHocs
                                                 where m.MaKhoiLop == maKhoiLop
                                                 select m;
            monHocs = monHocs.OrderBy(m => m.TenMonHoc);
            return monHocs.ToList();
        }

        public MonHocInfo GetMonHocInfo(int maMonHoc)
        {
            var monHocInfo = from mon in db.DanhMuc_MonHocs
                             join nganh in db.DanhMuc_NganhHocs on mon.MaNganhHoc equals nganh.MaNganhHoc
                             join khoi in db.DanhMuc_KhoiLops on mon.MaKhoiLop equals khoi.MaKhoiLop
                             where mon.MaMonHoc == maMonHoc
                             select new MonHocInfo
                             {
                                 MaMonHoc = mon.MaMonHoc,
                                 TenMonHoc = mon.TenMonHoc,
                                 TenNganhHoc = nganh.TenNganhHoc,
                                 TenKhoiLop = khoi.TenKhoiLop,
                                 HeSoDiem = mon.HeSoDiem
                             };

            if (monHocInfo.Count() != 0)
            {
                return monHocInfo.First();
            }
            else
            {
                return null;
            }
        }

        public List<MonHocInfo> GetListMonHocInfo(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            var monHocInfo = from monhoc in db.DanhMuc_MonHocs
                             join nganh in db.DanhMuc_NganhHocs on monhoc.MaNganhHoc equals nganh.MaNganhHoc
                             join khoi in db.DanhMuc_KhoiLops on monhoc.MaKhoiLop equals khoi.MaKhoiLop
                             select new MonHocInfo
                             {
                                 MaMonHoc = monhoc.MaMonHoc,
                                 TenMonHoc = monhoc.TenMonHoc,
                                 TenNganhHoc = nganh.TenNganhHoc,
                                 TenKhoiLop = khoi.TenKhoiLop,
                                 HeSoDiem = monhoc.HeSoDiem
                             };
            totalRecords = monHocInfo.Count();
            if (monHocInfo.Count() != 0)
            {
                monHocInfo = monHocInfo.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }
            return monHocInfo.ToList();
        }

        public List<MonHocInfo> GetListMonHocInfo(int maNganhHoc, int maKhoiLop, 
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            var monHocInfo = from monhoc in db.DanhMuc_MonHocs
                             join nganh in db.DanhMuc_NganhHocs on monhoc.MaNganhHoc equals nganh.MaNganhHoc
                             join khoi in db.DanhMuc_KhoiLops on monhoc.MaKhoiLop equals khoi.MaKhoiLop
                             where monhoc.MaNganhHoc == maNganhHoc && monhoc.MaKhoiLop == maKhoiLop
                             select new MonHocInfo
                             {
                                 MaMonHoc = monhoc.MaMonHoc,
                                 TenMonHoc = monhoc.TenMonHoc,
                                 TenNganhHoc = nganh.TenNganhHoc,
                                 TenKhoiLop = khoi.TenKhoiLop,
                                 HeSoDiem = monhoc.HeSoDiem
                             };
            totalRecords = monHocInfo.Count();
            if (monHocInfo.Count() != 0)
            {
                monHocInfo = monHocInfo.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }
            return monHocInfo.ToList();
        }

        public List<MonHocInfo> GetListMonHocInfo(int maNganhHoc, int maKhoiLop,
            int? exceptedMaMonHoc,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            var monHocInfo = from monhoc in db.DanhMuc_MonHocs
                             join nganh in db.DanhMuc_NganhHocs 
                                on monhoc.MaNganhHoc equals nganh.MaNganhHoc
                             join khoi in db.DanhMuc_KhoiLops 
                                on monhoc.MaKhoiLop equals khoi.MaKhoiLop
                             where monhoc.MaNganhHoc == maNganhHoc 
                                && monhoc.MaKhoiLop == maKhoiLop
                                && monhoc.MaMonHoc != (int)exceptedMaMonHoc
                             select new MonHocInfo
                             {
                                 MaMonHoc = monhoc.MaMonHoc,
                                 TenMonHoc = monhoc.TenMonHoc,
                                 TenNganhHoc = nganh.TenNganhHoc,
                                 TenKhoiLop = khoi.TenKhoiLop,
                                 HeSoDiem = monhoc.HeSoDiem
                             };
            totalRecords = monHocInfo.Count();
            if (monHocInfo.Count() != 0)
            {
                monHocInfo = monHocInfo.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }
            return monHocInfo.ToList();
        }

        public List<MonHocInfo> GetListMonHocInfoByNganhHoc(int maNganhHoc, 
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            var monHocInfo = from monhoc in db.DanhMuc_MonHocs
                             join nganh in db.DanhMuc_NganhHocs on monhoc.MaNganhHoc equals nganh.MaNganhHoc
                             join khoi in db.DanhMuc_KhoiLops on monhoc.MaKhoiLop equals khoi.MaKhoiLop
                             where monhoc.MaNganhHoc == maNganhHoc
                             select new MonHocInfo
                             {
                                 MaMonHoc = monhoc.MaMonHoc,
                                 TenMonHoc = monhoc.TenMonHoc,
                                 TenNganhHoc = nganh.TenNganhHoc,
                                 TenKhoiLop = khoi.TenKhoiLop
                             };
            totalRecords = monHocInfo.Count();
            if (monHocInfo.Count() != 0)
            {
                monHocInfo = monHocInfo.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }
            return monHocInfo.ToList();
        }

        public List<MonHocInfo> GetListMonHocInfoByKhoiLop(int maKhoiLop, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            var monHocInfo = from monhoc in db.DanhMuc_MonHocs
                             join nganh in db.DanhMuc_NganhHocs on monhoc.MaNganhHoc equals nganh.MaNganhHoc
                             join khoi in db.DanhMuc_KhoiLops on monhoc.MaKhoiLop equals khoi.MaKhoiLop
                             where monhoc.MaKhoiLop == maKhoiLop
                             select new MonHocInfo
                             {
                                 MaMonHoc = monhoc.MaMonHoc,
                                 TenMonHoc = monhoc.TenMonHoc,
                                 TenNganhHoc = nganh.TenNganhHoc,
                                 TenKhoiLop = khoi.TenKhoiLop,
                                 HeSoDiem = monhoc.HeSoDiem
                             };
            totalRecords = monHocInfo.Count();
            if (monHocInfo.Count() != 0)
            {
                monHocInfo = monHocInfo.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }
            return monHocInfo.ToList();
        }

        public bool MonHocExists(string tenMonHoc, int maNganhHoc, int maKhoiLop)
        {
            IQueryable<DanhMuc_MonHoc> monHocs;
            monHocs = from monHoc in db.DanhMuc_MonHocs
                      where monHoc.TenMonHoc == tenMonHoc
                        && monHoc.MaNganhHoc == maNganhHoc && monHoc.MaKhoiLop == maKhoiLop
                      select monHoc;
            if (monHocs.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool MonHocExists(int maMonHoc, string tenMonHoc, int maNganhHoc, int maKhoiLop)
        {
            IQueryable<DanhMuc_MonHoc> monHocs;
            monHocs = from monHoc in db.DanhMuc_MonHocs
                      where monHoc.TenMonHoc == tenMonHoc && monHoc.MaMonHoc != maMonHoc
                         && monHoc.MaNganhHoc == maNganhHoc && monHoc.MaKhoiLop == maKhoiLop
                      select monHoc;
            if (monHocs.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckCanDeleteMonHoc(int maMonHoc)
        {
            IQueryable<LopHoc_MonHocTKB> monHocTKBs = from monTKB in db.LopHoc_MonHocTKBs
                                                      where monTKB.MaMonHoc == maMonHoc
                                                      select monTKB;

            if (monHocTKBs.Count() != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool MonHocExists(int maMonHoc, string tenMonHocMoi)
        {
            DanhMuc_MonHoc monHoc;
            monHoc = (from mon in db.DanhMuc_MonHocs
                      where mon.MaMonHoc == maMonHoc
                      select mon).First();
            IQueryable<DanhMuc_MonHoc> monHocs = from mon in db.DanhMuc_MonHocs
                                                 where mon.MaNganhHoc == monHoc.MaNganhHoc
                                                    && mon.MaKhoiLop == monHoc.MaKhoiLop
                                                    && mon.TenMonHoc == tenMonHocMoi
                                                    && mon.MaMonHoc != maMonHoc
                                                 select mon;
            if (monHocs.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }        

        public List<MonHocInfo> GetListMonHocInfo(string tenMonHoc, 
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            var monHocInfo = from monhoc in db.DanhMuc_MonHocs
                             join nganh in db.DanhMuc_NganhHocs
                                on monhoc.MaNganhHoc equals nganh.MaNganhHoc
                             join khoi in db.DanhMuc_KhoiLops
                                on monhoc.MaKhoiLop equals khoi.MaKhoiLop
                             where monhoc.TenMonHoc == tenMonHoc
                             select new MonHocInfo
                             {
                                 MaMonHoc = monhoc.MaMonHoc,
                                 TenMonHoc = monhoc.TenMonHoc,
                                 TenNganhHoc = nganh.TenNganhHoc,
                                 TenKhoiLop = khoi.TenKhoiLop,
                                 HeSoDiem = monhoc.HeSoDiem
                             };
            totalRecords = monHocInfo.Count();
            if (monHocInfo.Count() != 0)
            {
                monHocInfo = monHocInfo.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }
            return monHocInfo.ToList();
        }

        public List<MonHocInfo> GetListMonHocInfo(string tenMonHoc,
            int? exceptedMaMonHoc,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            var monHocInfo = from monhoc in db.DanhMuc_MonHocs
                             join nganh in db.DanhMuc_NganhHocs
                                on monhoc.MaNganhHoc equals nganh.MaNganhHoc
                             join khoi in db.DanhMuc_KhoiLops
                                on monhoc.MaKhoiLop equals khoi.MaKhoiLop
                             where monhoc.TenMonHoc == tenMonHoc 
                                && monhoc.MaMonHoc != (int)exceptedMaMonHoc
                             select new MonHocInfo
                             {
                                 MaMonHoc = monhoc.MaMonHoc,
                                 TenMonHoc = monhoc.TenMonHoc,
                                 TenNganhHoc = nganh.TenNganhHoc,
                                 TenKhoiLop = khoi.TenKhoiLop,
                                 HeSoDiem = monhoc.HeSoDiem
                             };
            totalRecords = monHocInfo.Count();
            if (monHocInfo.Count() != 0)
            {
                monHocInfo = monHocInfo.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            }
            return monHocInfo.ToList();
        }
    }
}
