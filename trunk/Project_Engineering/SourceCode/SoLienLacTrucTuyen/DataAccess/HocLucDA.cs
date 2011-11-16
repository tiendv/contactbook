using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class HocLucDA : BaseDA
    {
        public HocLucDA(School school)
            : base(school)
        {
        }

        public void InsertHocLuc(DanhMuc_HocLuc hoclucEn)
        {
            db.DanhMuc_HocLucs.InsertOnSubmit(hoclucEn);
            db.SubmitChanges();
        }

        public void UpdateHocLuc(DanhMuc_HocLuc hoclucEn)
        {
            DanhMuc_HocLuc hocluc = GetHocLuc(hoclucEn.MaHocLuc);
            hocluc.TenHocLuc = hocluc.TenHocLuc;
            hocluc.DTBDau = hoclucEn.DTBDau;
            hocluc.DTBCuoi = hoclucEn.DTBCuoi;
            db.SubmitChanges();
        }

        public void DeleteHocLuc(int maHocLuc)
        {
            DanhMuc_HocLuc HocLuc = GetHocLuc(maHocLuc);
            if (HocLuc != null)
            {
                db.DanhMuc_HocLucs.DeleteOnSubmit(HocLuc);
                db.SubmitChanges();
            }
        }

        public DanhMuc_HocLuc GetHocLuc(int maHocLuc)
        {
            IQueryable<DanhMuc_HocLuc> HocLucs = from hl in db.DanhMuc_HocLucs
                                                 where hl.MaHocLuc == maHocLuc
                                                 select hl;
            if (HocLucs.Count() != 0)
            {
                return HocLucs.First();
            }
            else
            {
                return null;
            }
        }

        public List<DanhMuc_HocLuc> GetListHocLuc()
        {
            IQueryable<DanhMuc_HocLuc> iqHocLuc = from hocLuc in db.DanhMuc_HocLucs
                                                  select hocLuc;
            if (iqHocLuc.Count() != 0)
            {
                return iqHocLuc.OrderBy(hocLuc => hocLuc.TenHocLuc).ToList();
            }
            else
            {
                return new List<DanhMuc_HocLuc>();
            }            
        }

        public List<DanhMuc_HocLuc> GetListHocLuc(int pageCurrentIndex, int pageSize)
        {
            IQueryable<DanhMuc_HocLuc> hocLucs = from hl in db.DanhMuc_HocLucs
                                                 select hl;
            hocLucs = hocLucs.OrderBy(hl => hl.TenHocLuc);
            hocLucs = hocLucs.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            return hocLucs.ToList();
        }

        public List<DanhMuc_HocLuc> GetListHocLuc(string tenHocLuc, int pageCurrentIndex, int pageSize)
        {
            IQueryable<DanhMuc_HocLuc> hocLucs = from hl in db.DanhMuc_HocLucs
                                                 where hl.TenHocLuc.Contains(tenHocLuc)
                                                 select hl;
            hocLucs = hocLucs.OrderBy(n => n.TenHocLuc).Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            return hocLucs.ToList();
        }

        public double GetHocLucCount()
        {
            IQueryable<DanhMuc_HocLuc> hocLucs = from hl in db.DanhMuc_HocLucs
                                                 select hl;
            return hocLucs.Count();
        }

        public double GetHocLucCount(string tenHocLuc)
        {
            IQueryable<DanhMuc_HocLuc> hocLucs = from hl in db.DanhMuc_HocLucs
                                                 where hl.TenHocLuc.Contains(tenHocLuc)
                                                 select hl;
            return hocLucs.Count();
        }

        public bool CheckExistTenHocLuc(int maHocLuc, string tenHocLuc)
        {
            IQueryable<DanhMuc_HocLuc> hocLucs = from hl in db.DanhMuc_HocLucs
                                                 where hl.TenHocLuc == tenHocLuc && hl.MaHocLuc != maHocLuc
                                                 select hl;
            if (hocLucs.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckCanDeleteHocLuc(int maHocLuc)
        {
            //IQueryable<HocSinh_ChiTietDiem> chiTietDiem = from ctd in db.HocSinh_ChiTietDiems
            //                                              where ctd.MaHocLuc == maHocLuc
            //                                              select ctd;
            //if (chiTietDiem.Count() != 0)
            //{
            //    return false;
            //}
            //else
            //{
            //    return true;
            //}
            return true;
        }

        public DanhMuc_HocLuc GetHocLuc(double diem)
        {
            IQueryable<DanhMuc_HocLuc> hocLucs = from hocluc in db.DanhMuc_HocLucs
                                                 where hocluc.DTBDau <= diem && hocluc.DTBCuoi >= diem
                                                 select hocluc;
            if (hocLucs.Count() != 0)
            {
                return hocLucs.First();
            }
            else
            {
                return null;
            }
        }
    }
}
