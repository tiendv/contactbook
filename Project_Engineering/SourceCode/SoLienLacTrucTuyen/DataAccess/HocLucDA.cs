﻿using System;
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
        public List<DanhMuc_HocLuc> GetHocLucs(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<DanhMuc_HocLuc> lConducts = new List<DanhMuc_HocLuc>();

            IQueryable<DanhMuc_HocLuc> iqConduct = from cdt in db.DanhMuc_HocLucs
                                                     where cdt.SchoolId == school.SchoolId
                                                     select cdt;

            return GetHocLucs(ref iqConduct, pageCurrentIndex, pageSize, out totalRecords);
        }
        private List<DanhMuc_HocLuc> GetHocLucs(ref IQueryable<DanhMuc_HocLuc> iqHocLuc, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            totalRecords = iqHocLuc.Count();
            if (totalRecords != 0)
            {
                return iqHocLuc.OrderBy(hocluc => hocluc.MaHocLuc)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<DanhMuc_HocLuc>();
            }
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
        public bool ConductNameExists(string conductName)
        {
            IQueryable<DanhMuc_HocLuc> iqConduct = from cdt in db.DanhMuc_HocLucs
                                                     where cdt.TenHocLuc == conductName
                                                     //&& cdt.SchoolId == school.SchoolId
                                                     select cdt;
            if (iqConduct.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void InsertConduct(DanhMuc_HocLuc conduct)
        {
            conduct.SchoolId = school.SchoolId;
            db.DanhMuc_HocLucs.InsertOnSubmit(conduct);
            db.SubmitChanges();
        }
        public void DeleteConduct(DanhMuc_HocLuc deletedHocLuc)
        {
            DanhMuc_HocLuc conduct = null;

            IQueryable<DanhMuc_HocLuc> iqConduct = from cdt in db.DanhMuc_HocLucs
                                                   where cdt.MaHocLuc == deletedHocLuc.MaHocLuc
                                                     && cdt.SchoolId == school.SchoolId
                                                     select cdt;

            if (iqConduct.Count() != 0)
            {
                conduct = iqConduct.First();
                db.DanhMuc_HocLucs.DeleteOnSubmit(conduct);
                db.SubmitChanges();
            }
        }
        public DanhMuc_HocLuc GetConduct(string conductName)
        {
            DanhMuc_HocLuc conduct = null;

            IQueryable<DanhMuc_HocLuc> iqConduct = from cdt in db.DanhMuc_HocLucs
                                                     where cdt.TenHocLuc == conductName
                                                     && cdt.SchoolId == school.SchoolId
                                                     select cdt;

            if (iqConduct.Count() != 0)
            {
                conduct = iqConduct.First();
            }

            return conduct;
        }
        public DanhMuc_HocLuc GetConduct(int conductName)
        {
            DanhMuc_HocLuc conduct = null;

            IQueryable<DanhMuc_HocLuc> iqConduct = from cdt in db.DanhMuc_HocLucs
                                                   where cdt.MaHocLuc == conductName
                                                   && cdt.SchoolId == school.SchoolId
                                                   select cdt;

            if (iqConduct.Count() != 0)
            {
                conduct = iqConduct.First();
            }

            return conduct;
        }
        public void UpdateConduct(DanhMuc_HocLuc editedConduct)
        {
            DanhMuc_HocLuc conduct = null;

            IQueryable<DanhMuc_HocLuc> iqConduct = from cdt in db.DanhMuc_HocLucs
                                                     where cdt.MaHocLuc == editedConduct.MaHocLuc
                                                     && cdt.SchoolId == school.SchoolId
                                                     select cdt;

            if (iqConduct.Count() != 0)
            {
                conduct = iqConduct.First();
                conduct.TenHocLuc = editedConduct.TenHocLuc;
                conduct.DTBDau = editedConduct.DTBDau;
                conduct.DTBCuoi = editedConduct.DTBCuoi;
                db.SubmitChanges();
            }
        }
        public bool IsDeletable(string tenHocLuc)
        {
            bool bResult = true;
            IQueryable<HocSinh_DanhHieuHocKy> iqTermStudentResult;

            // Kiểm tra có tồn tại Học sinh nào đạt hạnh kiểm chỉ định hay không
            iqTermStudentResult = from termStudentResult in db.HocSinh_DanhHieuHocKies
                                  join hocluc in db.DanhMuc_HocLucs on termStudentResult.MaHocLucHK equals hocluc.MaHocLuc
                                  where hocluc.TenHocLuc == tenHocLuc
                                  select termStudentResult;

            if (iqTermStudentResult.Count() != 0)
            {
                bResult = false;
            }

            return bResult;
        }
    }
}
