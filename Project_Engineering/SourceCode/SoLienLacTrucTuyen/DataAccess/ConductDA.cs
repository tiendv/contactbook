using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class ConductDA : BaseDA
    {
        public ConductDA()
            : base()
        {

        }

        public void InsertConduct(DanhMuc_HanhKiem conduct)
        {
            db.DanhMuc_HanhKiems.InsertOnSubmit(conduct);
            db.SubmitChanges();
        }

        public void UpdateConduct(DanhMuc_HanhKiem editedConduct)
        {
            DanhMuc_HanhKiem conduct = null;

            IQueryable<DanhMuc_HanhKiem> iqConduct = from cdt in db.DanhMuc_HanhKiems
                                                     where cdt.MaHanhKiem == editedConduct.MaHanhKiem
                                                     select cdt;

            if (iqConduct.Count() != 0)
            {
                conduct = iqConduct.First();
                conduct.TenHanhKiem = editedConduct.TenHanhKiem;
                db.SubmitChanges();
            }
        }

        public void DeleteConduct(DanhMuc_HanhKiem deletedConduct)
        {
            DanhMuc_HanhKiem conduct = null;

            IQueryable<DanhMuc_HanhKiem> iqConduct = from cdt in db.DanhMuc_HanhKiems
                                                     where cdt.MaHanhKiem == deletedConduct.MaHanhKiem
                                                     select cdt;

            if (iqConduct.Count() != 0)
            {
                conduct = iqConduct.First();
                db.DanhMuc_HanhKiems.DeleteOnSubmit(conduct);
                db.SubmitChanges();
            }
        }

        public DanhMuc_HanhKiem GetConduct(int conductId)
        {
            DanhMuc_HanhKiem conduct = null;

            IQueryable<DanhMuc_HanhKiem> iqConduct = from cdt in db.DanhMuc_HanhKiems
                                                     where cdt.MaHanhKiem == conductId
                                                     select cdt;

            if (iqConduct.Count() != 0)
            {
                conduct = iqConduct.First();
            }

            return conduct;
        }

        public DanhMuc_HanhKiem GetConduct(string conductName)
        {
            DanhMuc_HanhKiem conduct = null;

            IQueryable<DanhMuc_HanhKiem> iqConduct = from cdt in db.DanhMuc_HanhKiems
                                                     where cdt.TenHanhKiem == conductName
                                                     select cdt;

            if (iqConduct.Count() != 0)
            {
                conduct = iqConduct.First();
            }

            return conduct;
        }

        public List<DanhMuc_HanhKiem> GetConducts()
        {
            List<DanhMuc_HanhKiem> lConducts = new List<DanhMuc_HanhKiem>();

            IQueryable<DanhMuc_HanhKiem> iqConduct = from cdt in db.DanhMuc_HanhKiems
                                                     select cdt;

            if (iqConduct.Count() != 0)
            {
                lConducts = iqConduct.OrderBy(cdt => cdt.TenHanhKiem).ToList();
            }

            return lConducts;
        }

        public List<DanhMuc_HanhKiem> GetConducts(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<DanhMuc_HanhKiem> lConducts = new List<DanhMuc_HanhKiem>();

            IQueryable<DanhMuc_HanhKiem> iqConduct = from cdt in db.DanhMuc_HanhKiems
                                                     select cdt;

            return GetConducts(ref iqConduct, pageCurrentIndex, pageSize, out totalRecords);
        }

        private List<DanhMuc_HanhKiem> GetConducts(ref IQueryable<DanhMuc_HanhKiem> iqHanhKiem, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            totalRecords = iqHanhKiem.Count();
            if (totalRecords != 0)
            {
                return iqHanhKiem.OrderBy(hanhkiem => hanhkiem.TenHanhKiem)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<DanhMuc_HanhKiem>();
            }
        }

        public bool IsDeletable(string conductName)
        {
            bool bResult = true;
            IQueryable<HocSinh_DanhHieuHocKy> iqTermStudentResult;

            // Kiểm tra có tồn tại Học sinh nào đạt hạnh kiểm chỉ định hay không
            iqTermStudentResult = from termStudentResult in db.HocSinh_DanhHieuHocKies
                                  join conduct in db.DanhMuc_HanhKiems on termStudentResult.MaHanhKiemHK equals conduct.MaHanhKiem
                                  where conduct.TenHanhKiem == conductName
                                  select termStudentResult;

            if (iqTermStudentResult.Count() != 0)
            {
                bResult = false;
            }

            return bResult;
        }

        public bool ConductNameExists(string conductName)
        {
            IQueryable<DanhMuc_HanhKiem> iqConduct = from cdt in db.DanhMuc_HanhKiems
                                                     where cdt.TenHanhKiem == conductName
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
    }
}
