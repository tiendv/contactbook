using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class TietDA : BaseDA
    {
        public TietDA()
            : base()
        {
        }

        public void Delete(int maTiet)
        {
            IQueryable<DanhMuc_Tiet> iqTiet = from tiet in db.DanhMuc_Tiets
                                              where tiet.MaTiet == maTiet
                                              select tiet;
            if (iqTiet.Count() != 0)
            {
                db.DanhMuc_Tiets.DeleteOnSubmit(iqTiet.First());
                db.SubmitChanges();
            }
        }

        public void InsertTiet(string tenTiet, int buoi, int thuTu,
            DateTime thoiGianBatDau, DateTime thoiGianKetThuc)
        {
            DanhMuc_Tiet tiet = new DanhMuc_Tiet
            {
                TenTiet = tenTiet,
                MaBuoi = buoi,
                ThuTu = thuTu,
                ThoiGianBatDau = thoiGianBatDau,
                ThoiDiemKetThu = thoiGianKetThuc
            };

            db.DanhMuc_Tiets.InsertOnSubmit(tiet);
            db.SubmitChanges();
        }

        public void UpdateTiet(int maTiet, string tenTiet, int buoi, int thuTu, 
            DateTime thoiGianBatDau, DateTime thoiGianKetThuc)
        {
            IQueryable<DanhMuc_Tiet> iqTiet = from tiet in db.DanhMuc_Tiets
                                              where tiet.MaTiet == maTiet
                                              select tiet;
            if (iqTiet.Count() != 0)
            {
                DanhMuc_Tiet tiet = iqTiet.First();
                tiet.TenTiet = tenTiet;
                tiet.MaBuoi = tiet.MaBuoi;
                tiet.ThuTu = thuTu;
                tiet.ThoiGianBatDau = thoiGianBatDau;
                tiet.ThoiDiemKetThu = thoiGianKetThuc;
                db.SubmitChanges();
            }
        }

        public DanhMuc_Tiet GetTiet(int maTiet)
        {
            IQueryable<DanhMuc_Tiet> tiets = from tiet in db.DanhMuc_Tiets
                                             where tiet.MaTiet == maTiet
                                             select tiet;
            if (tiets.Count() != 0)
            {
                return tiets.First();
            }
            else
            {
                return null;
            }
        }

        public List<DanhMuc_Tiet> GetListTiets()
        {
            IQueryable<DanhMuc_Tiet> tiets = from tiet in db.DanhMuc_Tiets
                                             select tiet;
            if (tiets.Count() != 0)
            {
                return tiets.OrderBy(tiet => tiet.ThoiGianBatDau).ThenBy(tiet => tiet.ThuTu).ToList();
            }
            else
            {
                return new List<DanhMuc_Tiet>();
            }
        }

        public List<TabularTiet> GetTabularTiet(
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularTiet> tiets = from tiet in db.DanhMuc_Tiets
                                            join buoi in db.CauHinh_Buois
                                                on tiet.MaBuoi equals buoi.MaBuoi
                                            select new TabularTiet
                                            {
                                                MaTiet = tiet.MaTiet,
                                                TenTiet = tiet.TenTiet,
                                                MaBuoi = tiet.MaBuoi,
                                                TenBuoi = buoi.TenBuoi,
                                                ThuTu = tiet.ThuTu,
                                                ThoiGianBatDau = tiet.ThoiGianBatDau,
                                                ThoiGianKetThuc = tiet.ThoiDiemKetThu
                                            };
            totalRecords = tiets.Count();
            if (totalRecords != 0)
            {
                return tiets.OrderBy(tiet => tiet.ThoiGianBatDau).ThenBy(tiet => tiet.ThuTu)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularTiet>();
            }
        }

        public List<TabularTiet> GetTabularTiet(string tenTiet, int maBuoi, 
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularTiet> tiets = from tiet in db.DanhMuc_Tiets
                                            join buoi in db.CauHinh_Buois 
                                                on tiet.MaBuoi equals buoi.MaBuoi
                                            where tiet.MaBuoi == maBuoi && tiet.TenTiet == tenTiet
                                            select new TabularTiet
                                            {
                                                MaTiet = tiet.MaTiet,
                                                TenTiet = tiet.TenTiet,
                                                MaBuoi = tiet.MaBuoi,
                                                TenBuoi = buoi.TenBuoi,
                                                ThuTu = tiet.ThuTu,
                                                ThoiGianBatDau = tiet.ThoiGianBatDau,
                                                ThoiGianKetThuc = tiet.ThoiDiemKetThu
                                            };
            totalRecords = tiets.Count();
            if (totalRecords != 0)
            {
                return tiets.OrderBy(tiet => tiet.ThoiGianBatDau).ThenBy(tiet => tiet.ThuTu)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularTiet>();
            }                                             
        }
        
        public List<TabularTiet> GetTabularTiet(int maBuoi, 
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularTiet> tiets = from tiet in db.DanhMuc_Tiets
                                            join buoi in db.CauHinh_Buois
                                                on tiet.MaBuoi equals buoi.MaBuoi
                                            where tiet.MaBuoi == maBuoi
                                            select new TabularTiet
                                            {
                                                MaTiet = tiet.MaTiet,
                                                TenTiet = tiet.TenTiet,
                                                MaBuoi = tiet.MaBuoi,
                                                TenBuoi = buoi.TenBuoi,
                                                ThuTu = tiet.ThuTu,
                                                ThoiGianBatDau = tiet.ThoiGianBatDau,
                                                ThoiGianKetThuc = tiet.ThoiDiemKetThu
                                            };
            totalRecords = tiets.Count();
            if (totalRecords != 0)
            {
                return tiets.OrderBy(tiet => tiet.ThoiGianBatDau).ThenBy(tiet => tiet.ThuTu)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularTiet>();
            }
        }

        public List<TabularTiet> GetTabularTiet(string tenTiet, 
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularTiet> tiets = from tiet in db.DanhMuc_Tiets
                                            join buoi in db.CauHinh_Buois
                                                on tiet.MaBuoi equals buoi.MaBuoi
                                            where tiet.TenTiet == tenTiet
                                            select new TabularTiet
                                            {
                                                MaTiet = tiet.MaTiet,
                                                TenTiet = tiet.TenTiet,
                                                MaBuoi = tiet.MaBuoi,
                                                TenBuoi = buoi.TenBuoi,
                                                ThuTu = tiet.ThuTu,
                                                ThoiGianBatDau = tiet.ThoiGianBatDau,
                                                ThoiGianKetThuc = tiet.ThoiDiemKetThu
                                            };
            totalRecords = tiets.Count();
            if (totalRecords != 0)
            {
                return tiets.OrderBy(tiet => tiet.ThoiGianBatDau).ThenBy(tiet => tiet.ThuTu)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularTiet>();
            }
        }

        public bool TietCanBeDeleted(int maTiet)
        {
            IQueryable<LopHoc_MonHocTKB> monHocTKBs = from monHocTKB in db.LopHoc_MonHocTKBs
                                                      where monHocTKB.MaTiet == maTiet
                                                      select monHocTKB;
            if (monHocTKBs.Count() != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }                

        public string GetChiTietTiet(int maTiet)
        {
            IQueryable<DanhMuc_Tiet> tiets = from tiet in db.DanhMuc_Tiets
                                             where tiet.MaTiet == maTiet
                                             select tiet;
            if (tiets.Count() != 0)
            {
                DanhMuc_Tiet tiet = tiets.First();
                string chiTietTiet = string.Format("<b>{0}</b><br/>({1}-{2})",
                        tiet.TenTiet,
                        tiet.ThoiDiemKetThu.ToShortTimeString(),
                        tiet.ThoiDiemKetThu.ToShortTimeString());
                return chiTietTiet;
            }
            else
            {
                return "";
            }
        }        

        public CauHinh_Buoi GetBuoi(int maTiet)
        {
            CauHinh_Buoi buoi = (from b in db.CauHinh_Buois
                                 join tiet in db.DanhMuc_Tiets on b.MaBuoi equals tiet.MaBuoi
                                 where tiet.MaTiet == maTiet
                                 select b).First();
            return buoi;
        }

        public bool TietHocExists(string tenTiet)
        {
            IQueryable<DanhMuc_Tiet> iqTiet = from tiet in db.DanhMuc_Tiets
                                              where tiet.TenTiet == tenTiet
                                              select tiet;
            if (iqTiet.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TietHocExists(string tenTietMoi, int maTiet)
        {
            IQueryable<DanhMuc_Tiet> iqTiet = from tiet in db.DanhMuc_Tiets
                                              where tiet.TenTiet == tenTietMoi && tiet.MaTiet != maTiet
                                              select tiet;
            if (iqTiet.Count() != 0)
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
