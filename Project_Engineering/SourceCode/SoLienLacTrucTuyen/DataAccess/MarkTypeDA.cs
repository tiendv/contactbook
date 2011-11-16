using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class MarkTypeDA : BaseDA
    {
        public MarkTypeDA(School school)
            : base(school)
        {
        }

        public void InsertMarkType(DanhMuc_LoaiDiem markType)
        {
            db.DanhMuc_LoaiDiems.InsertOnSubmit(markType);
            db.SubmitChanges();
        }

        public void DeleteMarkType(DanhMuc_LoaiDiem markType)
        {
            IQueryable<DanhMuc_LoaiDiem> iqMarkType = from markTp in db.DanhMuc_LoaiDiems
                                                      where markTp.MaLoaiDiem == markType.MaLoaiDiem
                                                      select markTp;
            if (iqMarkType.Count() != 0)
            {
                db.DanhMuc_LoaiDiems.DeleteOnSubmit(iqMarkType.First());
            }
            db.SubmitChanges();
        }

        public void UpdateMarkType(DanhMuc_LoaiDiem newMarkType)
        {
            IQueryable<DanhMuc_LoaiDiem> iqMarkType = from markTp in db.DanhMuc_LoaiDiems
                                                      where markTp.MaLoaiDiem == newMarkType.MaLoaiDiem
                                                      select markTp;
            if (iqMarkType.Count() != 0)
            {
                DanhMuc_LoaiDiem markType = iqMarkType.First();

                markType.TenLoaiDiem = newMarkType.TenLoaiDiem;
                markType.HeSoDiem = newMarkType.HeSoDiem;
                markType.SoCotToiDa = newMarkType.SoCotToiDa;
                markType.TinhDTB = newMarkType.TinhDTB;
            }

            db.SubmitChanges();
        }

        public DanhMuc_LoaiDiem GetMarkType(int markTypeId)
        {
            DanhMuc_LoaiDiem markType = null;

            IQueryable<DanhMuc_LoaiDiem> iqMarkType = from markTp in db.DanhMuc_LoaiDiems
                                                      where markTp.MaLoaiDiem == markTypeId
                                                      select markTp;
            if (iqMarkType.Count() != 0)
            {
                markType = iqMarkType.First();
            }

            return markType;
        }

        public DanhMuc_LoaiDiem GetMarkType(string markTypeName)
        {
            DanhMuc_LoaiDiem markType = null;

            IQueryable<DanhMuc_LoaiDiem> iqMarkType = from markTp in db.DanhMuc_LoaiDiems
                                                      where markTp.TenLoaiDiem == markTypeName
                                                      select markTp;
            if (iqMarkType.Count() != 0)
            {
                markType = iqMarkType.First();
            }
            
            return markType;
        }

        public List<DanhMuc_LoaiDiem> GetListMarkTypes()
        {
            List<DanhMuc_LoaiDiem> lMarkTypes = new List<DanhMuc_LoaiDiem>();

            IQueryable<DanhMuc_LoaiDiem> iqMarkType = from markTp in db.DanhMuc_LoaiDiems
                                                      select markTp;

            if (iqMarkType.Count() != 0)
            {
                lMarkTypes = iqMarkType.OrderBy(loaiDiem => loaiDiem.HeSoDiem)
                    .ThenBy(loaiDiem => loaiDiem.TenLoaiDiem).ToList();
            }
            
            return lMarkTypes;
        }

        public List<DanhMuc_LoaiDiem> GetListMarkTypes(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<DanhMuc_LoaiDiem> lMarkTypes = new List<DanhMuc_LoaiDiem>();

            IQueryable<DanhMuc_LoaiDiem> iqMarkType = from markTp in db.DanhMuc_LoaiDiems
                                                      select markTp;

            totalRecords = iqMarkType.Count();            
            if(totalRecords != 0)
            {
                lMarkTypes = iqMarkType.OrderBy(loaiDiem => loaiDiem.HeSoDiem)
                    .ThenBy(loaiDiem => loaiDiem.TenLoaiDiem)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            
            return lMarkTypes;
        }

        public bool MarkTypeExists(string markTypeName)
        {
            IQueryable<DanhMuc_LoaiDiem> iqMarkType = from markTp in db.DanhMuc_LoaiDiems
                                                      where markTp.TenLoaiDiem == markTypeName
                                                      select markTp;
            if (iqMarkType.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsDeletable(DanhMuc_LoaiDiem markType)
        {
            IQueryable<HocSinh_ChiTietDiem> iqHocSinhChiTietDiem;
            iqHocSinhChiTietDiem = from hsChiTietDiem in db.HocSinh_ChiTietDiems
                                   where hsChiTietDiem.MaLoaiDiem == markType.MaLoaiDiem
                                   select hsChiTietDiem;

            if (iqHocSinhChiTietDiem.Count() != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public DanhMuc_LoaiDiem GetAppliedCalAvgMarkType()
        {
            DanhMuc_LoaiDiem markType = null;

            IQueryable<DanhMuc_LoaiDiem> iqMarkType = from markTp in db.DanhMuc_LoaiDiems
                                                      where markTp.TinhDTB == true
                                                      select markTp;
            if (iqMarkType.Count() != 0)
            {
                markType = iqMarkType.First();
            }

            return markType;
        }
    }
}
