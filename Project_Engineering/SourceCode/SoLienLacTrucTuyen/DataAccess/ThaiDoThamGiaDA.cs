using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class ThaiDoThamGiaDA : BaseDA
    {
        public ThaiDoThamGiaDA()
            : base()
        {

        }

        #region Insert, Delete, Update
        public void InsertThaiDoThamGia(DanhMuc_ThaiDoThamGia ThaiDoThamGia)
        {
            db.DanhMuc_ThaiDoThamGias.InsertOnSubmit(ThaiDoThamGia);
            db.SubmitChanges();
        }

        public void UpdateThaiDoThamGia(DanhMuc_ThaiDoThamGia ThaiDoThamGia)
        {
            DanhMuc_ThaiDoThamGia _ThaiDoThamGia = (from n in db.DanhMuc_ThaiDoThamGias
                                                    where n.MaThaiDoThamGia == ThaiDoThamGia.MaThaiDoThamGia
                                                    select n).First();
            _ThaiDoThamGia.TenThaiDoThamGia = ThaiDoThamGia.TenThaiDoThamGia;
            db.SubmitChanges();
        }

        public void DeleteThaiDoThamGia(int maThaiDoThamGia)
        {
            DanhMuc_ThaiDoThamGia ThaiDoThamGia = db.DanhMuc_ThaiDoThamGias.Where(k => k.MaThaiDoThamGia == maThaiDoThamGia).FirstOrDefault();
            db.DanhMuc_ThaiDoThamGias.DeleteOnSubmit(ThaiDoThamGia);
            db.SubmitChanges();
        }
        #endregion

        public List<DanhMuc_ThaiDoThamGia> GetListThaiDoThamGia()
        {
            IQueryable<DanhMuc_ThaiDoThamGia> thaiDoThamGia = from tdtg in db.DanhMuc_ThaiDoThamGias
                                                              select tdtg;
            return thaiDoThamGia.ToList();
        }

        public DanhMuc_ThaiDoThamGia GetThaiDoThamGia(int maThaiDoThamGia)
        {
            IQueryable<DanhMuc_ThaiDoThamGia> thaiDoThamGias = from tdtg in db.DanhMuc_ThaiDoThamGias
                                                               where tdtg.MaThaiDoThamGia == maThaiDoThamGia
                                                               select tdtg;
            if (thaiDoThamGias.Count() != 0)
            {
                return thaiDoThamGias.First();
            }
            else
            {
                return null;
            }
        }

        public List<DanhMuc_ThaiDoThamGia> GetListThaiDoThamGia(int pageCurrentIndex, int pageSize,
            out double totalRecords)
        {
            IQueryable<DanhMuc_ThaiDoThamGia> thaiDoThamGias;

            thaiDoThamGias = from thaiDo in db.DanhMuc_ThaiDoThamGias
                             select thaiDo;

            totalRecords = thaiDoThamGias.Count();
            if (totalRecords != 0)
            {
                return thaiDoThamGias.OrderBy(thaiDo => thaiDo.TenThaiDoThamGia)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<DanhMuc_ThaiDoThamGia>();
            }
        }

        public List<DanhMuc_ThaiDoThamGia> GetListThaiDoThamGia(string tenThaiDoThamGia, int pageCurrentIndex, int pageSize,
            out double totalRecords)
        {
            IQueryable<DanhMuc_ThaiDoThamGia> thaiDoThamGias;
            thaiDoThamGias = from thaiDo in db.DanhMuc_ThaiDoThamGias
                             where thaiDo.TenThaiDoThamGia.Contains(tenThaiDoThamGia)
                             select thaiDo;

            totalRecords = thaiDoThamGias.Count();
            if (totalRecords != 0)
            {
                return thaiDoThamGias.OrderBy(thaiDo => thaiDo.TenThaiDoThamGia)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<DanhMuc_ThaiDoThamGia>();
            }
        }

        public int GetThaiDoThamGiaCount()
        {
            IQueryable<DanhMuc_ThaiDoThamGia> thaiDoThamGias = from k in db.DanhMuc_ThaiDoThamGias
                                                               select k;
            return thaiDoThamGias.Count();
        }

        public double GetThaiDoThamGiaCount(string tenThaiDoThamGia)
        {
            IQueryable<DanhMuc_ThaiDoThamGia> thaiDoThamGias = from tdtg in db.DanhMuc_ThaiDoThamGias
                                                               where tdtg.TenThaiDoThamGia.Contains(tenThaiDoThamGia)
                                                               select tdtg;
            return thaiDoThamGias.Count();
        }

        public bool CheckCanDeleteThaiDoThamGia(int maThaiDoThamGia)
        {
            IQueryable<HocSinh_HoatDong> hocSinhHoatDongs = from hs_hd in db.HocSinh_HoatDongs
                                                            where hs_hd.MaThaiDoThamGia == maThaiDoThamGia
                                                            select hs_hd;
            if (hocSinhHoatDongs.Count() != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CheckExistTenThaiDoThamGia(int maThaiDoThamGia, string tenThaiDoThamGia)
        {
            IQueryable<DanhMuc_ThaiDoThamGia> thaiDoThamGias = from tdtg in db.DanhMuc_ThaiDoThamGias
                                                               where tdtg.TenThaiDoThamGia == tenThaiDoThamGia
                                                                 && tdtg.MaThaiDoThamGia != maThaiDoThamGia
                                                               select tdtg;
            if (thaiDoThamGias.Count() != 0)
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
