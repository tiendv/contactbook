using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class KhoiLopDA : BaseDA
    {
        public KhoiLopDA()
            : base()
        {

        }

        #region Insert, Update, Delete
        public void InsertKhoiLop(DanhMuc_KhoiLop khoiLop)
        {
            db.DanhMuc_KhoiLops.InsertOnSubmit(khoiLop);
            db.SubmitChanges();
        }

        public void UpdateKhoiLop(DanhMuc_KhoiLop khoiLop)
        {
            DanhMuc_KhoiLop _khoiLop = (from n in db.DanhMuc_KhoiLops
                                        where n.MaKhoiLop == khoiLop.MaKhoiLop
                                        select n).First();
            _khoiLop.TenKhoiLop = khoiLop.TenKhoiLop;
            _khoiLop.ThuTuHienThi = khoiLop.ThuTuHienThi;
            db.SubmitChanges();
        }

        public void DeleteKhoiLop(int maKhoiLop)
        {
            DanhMuc_KhoiLop khoiLop = (from n in db.DanhMuc_KhoiLops
                                       where n.MaKhoiLop == maKhoiLop
                                       select n).First();
            db.DanhMuc_KhoiLops.DeleteOnSubmit(khoiLop);
            db.SubmitChanges();
        }
        #endregion

        #region Get KhoiLop/List of KhoiLop
        public DanhMuc_KhoiLop GetKhoiLop(int maKhoiLop)
        {
            IQueryable<DanhMuc_KhoiLop> khoilop = from khoi in db.DanhMuc_KhoiLops
                                                  where khoi.MaKhoiLop == maKhoiLop
                                                  select khoi;
            if (khoilop.Count() != 0)
            {
                return khoilop.First();
            }
            else
            {
                return null;
            }
        }

        public List<DanhMuc_KhoiLop> GetListKhoiLop()
        {
            IQueryable<DanhMuc_KhoiLop> iqKhoiLop = from khoiLop in db.DanhMuc_KhoiLops
                                                    select khoiLop;
            if (iqKhoiLop.Count() != 0)
            {
                return iqKhoiLop.OrderBy(khoi => khoi.ThuTuHienThi).ToList();
            }
            else
            {
                return new List<DanhMuc_KhoiLop>();
            }
        }

        public List<DanhMuc_KhoiLop> GetListKhoiLop(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<DanhMuc_KhoiLop> iqKhoiLop = from khoiLop in db.DanhMuc_KhoiLops
                                                    select khoiLop;
            totalRecords = iqKhoiLop.Count();
            if (totalRecords != 0)
            {
                return iqKhoiLop.OrderBy(khoi => khoi.ThuTuHienThi)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<DanhMuc_KhoiLop>();
            }
        }

        public List<DanhMuc_KhoiLop> GetListKhoiLop(string tenKhoiLop,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<DanhMuc_KhoiLop> iqKhoiLop = from khoiLop in db.DanhMuc_KhoiLops
                                                    where khoiLop.TenKhoiLop == tenKhoiLop
                                                    select khoiLop;
            totalRecords = iqKhoiLop.Count();
            if (totalRecords != 0)
            {
                return iqKhoiLop.OrderBy(khoi => khoi.ThuTuHienThi)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<DanhMuc_KhoiLop>();
            }
        }
        #endregion

        public bool CanDeleteKhoiLop(int maKhoiLop)
        {
            var lop = from l in db.LopHoc_Lops
                      where l.MaKhoiLop == maKhoiLop
                      select l;
            bool bConstraintToLop = (lop.Count() != 0) ? true : false;

            var mon = from m in db.DanhMuc_MonHocs
                      where m.MaKhoiLop == maKhoiLop
                      select m;
            bool bConstraintToMon = (mon.Count() != 0) ? true : false;

            if (bConstraintToLop || bConstraintToMon)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #region Check Existence
        public bool KhoiLopExists(string tenKhoiLop)
        {
            IQueryable<DanhMuc_KhoiLop> khoiLops;
            khoiLops = from khoi in db.DanhMuc_KhoiLops
                       where khoi.TenKhoiLop == tenKhoiLop
                       select khoi;
            if (khoiLops.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool KhoiLopExists(int maKhoiLop, string tenKhoiLop)
        {
            IQueryable<DanhMuc_KhoiLop> khoiLops;
            khoiLops = from khoi in db.DanhMuc_KhoiLops
                       where khoi.TenKhoiLop == tenKhoiLop
                            && khoi.MaKhoiLop != maKhoiLop
                       select khoi;
            if (khoiLops.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
