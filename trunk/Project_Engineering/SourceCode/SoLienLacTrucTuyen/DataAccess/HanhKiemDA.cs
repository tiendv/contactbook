using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class HanhKiemDA : BaseDA
    {
        public HanhKiemDA()
            : base()
        {

        }

        public void InsertHanhKiem(DanhMuc_HanhKiem hanhkiemEn)
        {
            db.DanhMuc_HanhKiems.InsertOnSubmit(hanhkiemEn);
            db.SubmitChanges();
        }

        public void UpdateHanhKiem(DanhMuc_HanhKiem hanhkiemEn)
        {
            DanhMuc_HanhKiem hanhkiem = (from hk in db.DanhMuc_HanhKiems
                                        where hk.MaHanhKiem == hanhkiemEn.MaHanhKiem
                                        select hk).First();
            hanhkiem.TenHanhKiem = hanhkiemEn.TenHanhKiem;
            db.SubmitChanges();
        }

        public void DeleteHanhKiem(int maHanhKiem)
        {
            DanhMuc_HanhKiem hanhkiem = db.DanhMuc_HanhKiems.Where(hk => hk.MaHanhKiem == maHanhKiem).FirstOrDefault();
            db.DanhMuc_HanhKiems.DeleteOnSubmit(hanhkiem);
            db.SubmitChanges();
        }

        public DanhMuc_HanhKiem GetHanhKiem(int maHanhKiem)
        {
            IQueryable<DanhMuc_HanhKiem> hanhKiems = from hk in db.DanhMuc_HanhKiems
                                                     where hk.MaHanhKiem == maHanhKiem
                                                     select hk;
            if (hanhKiems.Count() != 0)
            {
                return hanhKiems.First();
            }
            else
            {
                return null;
            }
        }

        public List<DanhMuc_HanhKiem> GetListHanhKiem()
        {
            IQueryable<DanhMuc_HanhKiem> hanhKiems = from hk in db.DanhMuc_HanhKiems
                                                     select hk;
            return hanhKiems.ToList();
        }

        public List<DanhMuc_HanhKiem> GetListHanhKiem(int pageCurrentIndex, int pageSize)
        {
            IQueryable<DanhMuc_HanhKiem> hanhKiems = from hk in db.DanhMuc_HanhKiems
                                                     select hk;
            hanhKiems = hanhKiems.OrderBy(n => n.TenHanhKiem).Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            return hanhKiems.ToList();
        }

        public List<DanhMuc_HanhKiem> GetListHanhKiem(string tenHanhKiem, int pageCurrentIndex, int pageSize)
        {
            IQueryable<DanhMuc_HanhKiem> hanhKiems = from hk in db.DanhMuc_HanhKiems
                                                     where hk.TenHanhKiem.Contains(tenHanhKiem)
                                                     select hk;
            hanhKiems = hanhKiems.OrderBy(n => n.TenHanhKiem).Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            return hanhKiems.ToList();
        }

        public int GetHanhKiemCount()
        {
            IQueryable<DanhMuc_HanhKiem> hanhKiems = from hk in db.DanhMuc_HanhKiems
                                                     select hk;
            return hanhKiems.Count();
        }

        public double GetHanhKiemCount(string tenHanhKiem)
        {
            IQueryable<DanhMuc_HanhKiem> hanhKiems = from hk in db.DanhMuc_HanhKiems
                                                     where hk.TenHanhKiem.Contains(tenHanhKiem)
                                                     select hk;
            return hanhKiems.Count();
        }

        public bool CheckCanDeleteHanhKiem(int maHanhKiem)
        {
            //var lop = from l in db.LopHoc_Lops
            //          where l.MaHanhKiem == maHanhKiem
            //          select l;
            //bool bConstraintToLop = (lop.Count() != 0) ? true : false;

            //var mon = from m in db.DanhMuc_MonHocs
            //          where m.MaHanhKiem == maHanhKiem
            //          select m;
            //bool bConstraintToMon = (mon.Count() != 0) ? true : false;

            //if (bConstraintToLop || bConstraintToMon)
            //{
            //    return false;
            //}
            //else
            //{
            //    return true;
            //}
            return true;
        }

        public bool CheckExistTenHanhKiem(int maHanhKiem, string tenHanhKiem)
        {
            IQueryable<DanhMuc_HanhKiem> hanhKiems = from hk in db.DanhMuc_HanhKiems
                                                     where hk.TenHanhKiem == tenHanhKiem && hk.MaHanhKiem != maHanhKiem
                                                     select hk;
            if (hanhKiems.Count() != 0)
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
