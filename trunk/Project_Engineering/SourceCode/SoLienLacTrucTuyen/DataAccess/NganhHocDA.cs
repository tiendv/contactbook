using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class FacultyDA : BaseDA
    {
        public FacultyDA()
            : base()
        {
        }

        public void InsertFaculty(Faculty faculty)
        {
            DanhMuc_NganhHoc tbFaculty = new DanhMuc_NganhHoc
            {
                TenNganhHoc = faculty.Name,
                MoTa = faculty.Description
            };

            db.DanhMuc_NganhHocs.InsertOnSubmit(tbFaculty);
            db.SubmitChanges();
        }

        public void UpdateFaculty(DanhMuc_NganhHoc nganhhoc)
        {
            DanhMuc_NganhHoc _nganhhoc = (from n in db.DanhMuc_NganhHocs
                                          where n.MaNganhHoc == nganhhoc.MaNganhHoc
                                          select n).First();
            _nganhhoc.TenNganhHoc = nganhhoc.TenNganhHoc;
            _nganhhoc.MoTa = nganhhoc.MoTa;
            db.SubmitChanges();
        }

        public void DeleteFaculty(int maNganhHoc)
        {
            DanhMuc_NganhHoc nganhhoc = db.DanhMuc_NganhHocs.Where(n => n.MaNganhHoc == maNganhHoc).FirstOrDefault();
            db.DanhMuc_NganhHocs.DeleteOnSubmit(nganhhoc);
            db.SubmitChanges();
        }

        public DanhMuc_NganhHoc GetNganhHoc(int maNganhHoc)
        {
            DanhMuc_NganhHoc nganhhoc = (from n in db.DanhMuc_NganhHocs
                                        where n.MaNganhHoc == maNganhHoc
                                        select n).First();
            return nganhhoc;
        }

        public List<DanhMuc_NganhHoc> GetListNganhHoc()
        {
            IQueryable<DanhMuc_NganhHoc> nganhhoc = from n in db.DanhMuc_NganhHocs
                                                    select n;
            return nganhhoc.ToList();
        }

        public List<DanhMuc_NganhHoc> GetListNganhHoc(int pageCurrentIndex, int pageSize, 
            out double totalRecords)
        {
            IQueryable<DanhMuc_NganhHoc> nganhHocs = from nganh in db.DanhMuc_NganhHocs
                                                     select nganh;
            totalRecords = nganhHocs.Count();
            if (totalRecords != 0)
            {
                return nganhHocs.OrderBy(fac => fac.TenNganhHoc).Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<DanhMuc_NganhHoc>();
            }
        }

        public List<DanhMuc_NganhHoc> GetListNganhHoc(string tenNganhHoc, 
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<DanhMuc_NganhHoc> nganhHocs = from nganh in db.DanhMuc_NganhHocs
                                                     where nganh.TenNganhHoc == tenNganhHoc
                                                     select nganh;
            totalRecords = nganhHocs.Count();
            if (totalRecords != 0)
            {
                return nganhHocs.OrderBy(fac => fac.TenNganhHoc).Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<DanhMuc_NganhHoc>();
            }
        }

        public bool NganhHocExists(int maNganhHoc, string tenNganhHoc)
        {
            IQueryable<DanhMuc_NganhHoc> nganhhoc = from n in db.DanhMuc_NganhHocs
                                                    where n.TenNganhHoc == tenNganhHoc && n.MaNganhHoc != maNganhHoc
                                                    select n;
            if(nganhhoc.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool NganhHocExists(string tenNganhHoc)
        {
            IQueryable<DanhMuc_NganhHoc> nganhHocs = from nganhHoc in db.DanhMuc_NganhHocs
                                                     where nganhHoc.TenNganhHoc == tenNganhHoc
                                                     select nganhHoc;
            if (nganhHocs.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CanDeleteNganhHoc(int maNganhHoc)
        {   
            var lop = from l in db.LopHoc_Lops
                      where l.MaNganhHoc == maNganhHoc
                      select l;
            bool bConstraintToLop = (lop.Count() != 0) ? true : false;

            var mon = from m in db.DanhMuc_MonHocs
                      where m.MaNganhHoc == maNganhHoc
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
    }
}