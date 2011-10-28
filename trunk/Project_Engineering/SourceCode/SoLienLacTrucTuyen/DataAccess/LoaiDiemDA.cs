using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class LoaiDiemDA : BaseDA
    {
        public LoaiDiemDA()
            : base()
        {
        }

        #region Insert, Update, Delete
        public void InsertLoaiDiem(DanhMuc_LoaiDiem loaiDiem)
        {
            db.DanhMuc_LoaiDiems.InsertOnSubmit(loaiDiem);
            db.SubmitChanges();
        }

        public void UpdateLoaiDiem(int maLoaiDiem, 
            string tenLoaiDiem, double heSoDiem,
            short maxMarksPerTerm, bool calAverageMark)
        {
            DanhMuc_LoaiDiem loaidiem = (from loai in db.DanhMuc_LoaiDiems
                                        where loai.MaLoaiDiem == maLoaiDiem
                                        select loai).First();
            loaidiem.TenLoaiDiem = tenLoaiDiem;
            loaidiem.HeSoDiem = heSoDiem;
            loaidiem.SoCotToiDa = maxMarksPerTerm;
            loaidiem.TinhDTB = calAverageMark;

            db.SubmitChanges();
        }

        public void DeleteLoaiDiem(int maLoaiDiem)
        {
            DanhMuc_LoaiDiem loaidiem = GetLoaiDiem(maLoaiDiem);
            if (loaidiem != null)
            {
                db.DanhMuc_LoaiDiems.DeleteOnSubmit(loaidiem);
                db.SubmitChanges();
            }
        }
        #endregion

        #region Get Entity, List
        public DanhMuc_LoaiDiem GetLoaiDiem(int maLoaiDiem)
        {
            IQueryable<DanhMuc_LoaiDiem> LoaiDiems = from ld in db.DanhMuc_LoaiDiems
                                                     where ld.MaLoaiDiem == maLoaiDiem
                                                     select ld;
            if (LoaiDiems.Count() != 0)
            {
                return LoaiDiems.First();
            }
            else
            {
                return null;
            }
        }

        public List<DanhMuc_LoaiDiem> GetListLoaiDiem()
        {
            IQueryable<DanhMuc_LoaiDiem> iqLoaiDiem = from loaiDiem in db.DanhMuc_LoaiDiems
                                                      select loaiDiem;
            if (iqLoaiDiem.Count() != 0)
            {
                return iqLoaiDiem.OrderBy(loaiDiem => loaiDiem.HeSoDiem)
                    .ThenBy(loaiDiem => loaiDiem.TenLoaiDiem).ToList();
            }
            else
            {
                return new List<DanhMuc_LoaiDiem>();
            }            
        }

        public List<DanhMuc_LoaiDiem> GetListLoaiDiem(string tenLoaiDiem)
        {
            IQueryable<DanhMuc_LoaiDiem> iqLoaiDiem = from loaiDiem in db.DanhMuc_LoaiDiems
                                                      where loaiDiem.TenLoaiDiem == tenLoaiDiem
                                                      select loaiDiem;
            if (iqLoaiDiem.Count() != 0)
            {
                return iqLoaiDiem.OrderBy(loaiDiem => loaiDiem.HeSoDiem)
                    .ThenBy(loaiDiem => loaiDiem.TenLoaiDiem).ToList();
            }
            else
            {
                return new List<DanhMuc_LoaiDiem>();
            }      
        }

        public List<DanhMuc_LoaiDiem> GetListLoaiDiem(int pageCurrentIndex, int pageSize, 
            out double totalRecords)
        {
            IQueryable<DanhMuc_LoaiDiem> iqLoaiDiem = from loaiDiem in db.DanhMuc_LoaiDiems
                                                      select loaiDiem;

            totalRecords = iqLoaiDiem.Count();            
            if(totalRecords != 0)
            {
                return iqLoaiDiem.OrderBy(loaiDiem => loaiDiem.HeSoDiem)
                    .ThenBy(loaiDiem => loaiDiem.TenLoaiDiem)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<DanhMuc_LoaiDiem>();
            }
        }

        public List<DanhMuc_LoaiDiem> GetListLoaiDiem(string tenLoaiDiem, int pageCurrentIndex, int pageSize, 
            out double totalRecords)
        {
            IQueryable<DanhMuc_LoaiDiem> iqLoaiDiem = from loaiDiem in db.DanhMuc_LoaiDiems
                                                      where loaiDiem.TenLoaiDiem == tenLoaiDiem
                                                      select loaiDiem;

            totalRecords = iqLoaiDiem.Count();
            if (totalRecords != 0)
            {
                return iqLoaiDiem.OrderBy(loaiDiem => loaiDiem.HeSoDiem)
                    .ThenBy(loaiDiem => loaiDiem.TenLoaiDiem)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<DanhMuc_LoaiDiem>();
            }
        }
        #endregion

        public bool LoaiDiemExists(int maLoaiDiem, string tenLoaiDiem)
        {
            IQueryable<DanhMuc_LoaiDiem> loaiDiems = from ld in db.DanhMuc_LoaiDiems
                                                     where ld.TenLoaiDiem == tenLoaiDiem && ld.MaLoaiDiem != maLoaiDiem
                                                     select ld;           
            
            if (loaiDiems.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CanDeleteLoaiDiem(int maLoaiDiem)
        {
            IQueryable<HocSinh_ChiTietDiem> chiTietDiem = from ctd in db.HocSinh_ChiTietDiems
                                                          where ctd.MaLoaiDiem == maLoaiDiem
                                                          select ctd;
            if (chiTietDiem.Count() != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
