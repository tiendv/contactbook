using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class DanhHieuDA: BaseDA
    {
        public DanhHieuDA(School school)
            : base(school)
        {
        }        

        public void InsertDanhHieu(string tenDanhHieu, Dictionary<int, int> dicHanhKiemNHocLuc)
        {
            DanhMuc_DanhHieu danhHieu = new DanhMuc_DanhHieu
            {
                TenDanhHieu = tenDanhHieu
            };
            db.DanhMuc_DanhHieus.InsertOnSubmit(danhHieu);
            db.SubmitChanges();

            danhHieu = GetLastedDanhHieu();            
            foreach (KeyValuePair<int, int> pair in dicHanhKiemNHocLuc)
            {
                DanhMuc_ChiTietDanhHieu ctDanhHieu = new DanhMuc_ChiTietDanhHieu
                {
                    MaDanhHieu = danhHieu.MaDanhHieu,
                    MaHocLuc = pair.Key,
                    MaHanhKiem = pair.Value                    
                };
                db.DanhMuc_ChiTietDanhHieus.InsertOnSubmit(ctDanhHieu);                   
            }
            
            db.SubmitChanges();
        }

        public void UpdateDanhHieu(int maDanhHieu, Dictionary<int, int> dicHanhKiemNHocLuc)
        {
            IQueryable<DanhMuc_ChiTietDanhHieu> ctDanhHieus = from ctDHieu in db.DanhMuc_ChiTietDanhHieus
                                                              where ctDHieu.MaDanhHieu == maDanhHieu
                                                              select ctDHieu;
            foreach (DanhMuc_ChiTietDanhHieu ctDanhHieu in ctDanhHieus)
            {
                db.DanhMuc_ChiTietDanhHieus.DeleteOnSubmit(ctDanhHieu);
            }
            db.SubmitChanges();

            foreach (KeyValuePair<int, int> pair in dicHanhKiemNHocLuc)
            {
                DanhMuc_ChiTietDanhHieu ctDanhHieu = new DanhMuc_ChiTietDanhHieu
                {
                    MaDanhHieu = maDanhHieu,
                    MaHocLuc = pair.Key,
                    MaHanhKiem = pair.Value
                };
                db.DanhMuc_ChiTietDanhHieus.InsertOnSubmit(ctDanhHieu);
            }

            db.SubmitChanges();
        }

        public void DeleteDanhHieu(int maDanhHieu)
        {
            DanhMuc_DanhHieu danhHieu = (from dHieu in db.DanhMuc_DanhHieus
                                         where dHieu.MaDanhHieu == maDanhHieu
                                         select dHieu).First();
            db.DanhMuc_DanhHieus.DeleteOnSubmit(danhHieu);
            db.SubmitChanges();
        }

        public void DeleteChiTietDanhHieu(int maDanhHieu, int maHocLuc, int maHanhKiem)
        {
            DanhMuc_ChiTietDanhHieu ctDanhHieu = (from ctDHieu in db.DanhMuc_ChiTietDanhHieus
                                                 where ctDHieu.MaDanhHieu == maDanhHieu
                                                    && ctDHieu.MaHocLuc == maHocLuc
                                                    && ctDHieu.MaHanhKiem == maHanhKiem
                                                 select ctDHieu).First();
            db.DanhMuc_ChiTietDanhHieus.DeleteOnSubmit(ctDanhHieu);
            db.SubmitChanges();
        }

        public DanhMuc_DanhHieu GetLastedDanhHieu()
        {
            IQueryable<DanhMuc_DanhHieu> danhHieus = from danhHieu in db.DanhMuc_DanhHieus
                                                     select danhHieu;
            if (danhHieus.Count() != 0)
            {
                return danhHieus.OrderByDescending(danhHieu => danhHieu.MaDanhHieu).First();
            }
            else
            {
                return null;
            }
        }

        public string GetTenDanhHieu(int maHocLuc, int maHanhKiem)
        {
            IQueryable<DanhMuc_DanhHieu> danhHieus = from ctDanhHieu in db.DanhMuc_ChiTietDanhHieus
                                                     join danhHieu in db.DanhMuc_DanhHieus 
                                                        on ctDanhHieu.MaDanhHieu equals danhHieu.MaDanhHieu
                                                     where ctDanhHieu.MaHocLuc == maHocLuc 
                                                        && ctDanhHieu.MaHanhKiem == maHanhKiem
                                                     select danhHieu;
            if (danhHieus.Count() != 0)
            {
                return danhHieus.First().TenDanhHieu;
            }
            else
            {
                return "";
            }
        }

        public bool DanhHieuExists(int exceptedMaDanhHieu, string tenDanhHieu)
        {
            IQueryable<DanhMuc_DanhHieu> danhHieus = from danhHieu in db.DanhMuc_DanhHieus
                                                     where danhHieu.TenDanhHieu == tenDanhHieu
                                                        && danhHieu.MaDanhHieu != exceptedMaDanhHieu
                                                     select danhHieu;
            if (danhHieus.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DanhHieuExists(string tenDanhHieu)
        {
            IQueryable<DanhMuc_DanhHieu> danhHieus = from danhHieu in db.DanhMuc_DanhHieus
                                                     where danhHieu.TenDanhHieu == tenDanhHieu
                                                     select danhHieu;
            if (danhHieus.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateDanhHieu(int maDanhHieu, string tenDanhHieu)
        {
            IQueryable<DanhMuc_DanhHieu> danhHieus = from danhHieu in db.DanhMuc_DanhHieus
                                                     where danhHieu.MaDanhHieu == maDanhHieu
                                                     select danhHieu;
            danhHieus.First().TenDanhHieu = tenDanhHieu;
            db.SubmitChanges();
        }

        public List<DanhMuc_DanhHieu> GetListDanhHieus(string tenDanhHieu, 
            int pageCurrentIndex, int pageSize, out double totalRecord)
        {
            IQueryable<DanhMuc_DanhHieu> danhHieus = from danhHieu in db.DanhMuc_DanhHieus
                                                     where danhHieu.TenDanhHieu == tenDanhHieu
                                                     select danhHieu;
            totalRecord = danhHieus.Count();
            if (totalRecord != 0)
            {
                return danhHieus.OrderBy(danhHieu => danhHieu.TenDanhHieu)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<DanhMuc_DanhHieu>();
            }
        }

        public List<DanhMuc_DanhHieu> GetListDanhHieus(int pageCurrentIndex, int pageSize, out double totalRecord)
        {
            IQueryable<DanhMuc_DanhHieu> danhHieus = from danhHieu in db.DanhMuc_DanhHieus
                                                     select danhHieu;
            totalRecord = danhHieus.Count();
            if (totalRecord != 0)
            {
                return danhHieus.OrderBy(danhHieu => danhHieu.TenDanhHieu)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<DanhMuc_DanhHieu>();
            }
        }

        public DanhMuc_DanhHieu GetDanhHieu(int maDanhHieu)
        {
            IQueryable<DanhMuc_DanhHieu> danhHieus = from danhHieu in db.DanhMuc_DanhHieus
                                                     where danhHieu.MaDanhHieu == maDanhHieu
                                                     select danhHieu;
            if (danhHieus.Count() != 0)
            {
                return danhHieus.First();
            }
            else
            {
                return null;
            }
        }

        public bool CanDeleteDanhHieu(int maDanhHieu)
        {
            return true;
        }
    }
}
