using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class LopHocBL
    {
        private LopHocDA lophocDA;

        public LopHocBL()
        {
            lophocDA = new LopHocDA();
        }

        #region Insert, Update, Delete
        public void InsertLopHoc(string tenLopHoc, int maNganhHoc, int maKhoiLop, int maNamHoc)
        {
            lophocDA.InsertLopHoc(new LopHoc_Lop()
            {
                TenLopHoc = tenLopHoc,
                MaNganhHoc = maNganhHoc,
                MaKhoiLop = maKhoiLop,
                MaNamHoc = maNamHoc
            });
        }

        public void UpdateLopHoc(int maLopHoc, string tenLopHoc)
        {
            LopHoc_Lop lop = lophocDA.GetLopHoc(maLopHoc);
            lop.TenLopHoc = tenLopHoc;
            lophocDA.UpdateLopHoc(lop);
        }

        public void DeleteLopHoc(int maLopHoc)
        {
            LopHoc_Lop lop = lophocDA.GetLopHoc(maLopHoc);
            if (lop != null)
            {
                lophocDA.DeleteLopHoc(lop);
            }
        }
        #endregion

        #region Get Entity, List
        public LopHoc_Lop GetLopHoc(int maLopHoc)
        {
            return lophocDA.GetLopHoc(maLopHoc);
        }

        public LopHocInfo GetLopHocInfo(int maLopHoc)
        {
            return lophocDA.GetLopHocInfo(maLopHoc);
        }

        public List<LopHoc_Lop> GetListClasses(int maNganhHoc, DanhMuc_KhoiLop grade, int maNamHoc)
        {
            if (maNganhHoc == 0)
            {
                if (grade == null)
                {
                    return lophocDA.GetListLopHoc(maNamHoc);
                }
                else
                {
                    return lophocDA.GetListLopHocByKhoiLop(grade.MaKhoiLop, maNamHoc);
                }
            }
            else
            {
                if (grade == null)
                {
                    return lophocDA.GetListLopHocByNganhHoc(maNganhHoc, maNamHoc);
                }
                else
                {
                    return lophocDA.GetListLopHoc(maNganhHoc, grade.MaKhoiLop, maNamHoc);
                }
            }
        }

        public List<LopHoc_Lop> GetListLopHoc(int maNganhHoc, int maKhoiLop, int maNamHoc)
        {
            if (maNganhHoc == 0)
            {
                if (maKhoiLop == 0)
                {
                    return lophocDA.GetListLopHoc(maNamHoc);
                }
                else
                {
                    return lophocDA.GetListLopHocByKhoiLop(maKhoiLop, maNamHoc);
                }
            }
            else
            {
                if (maKhoiLop == 0)
                {
                    return lophocDA.GetListLopHocByNganhHoc(maNganhHoc, maNamHoc);
                }
                else
                {
                    return lophocDA.GetListLopHoc(maNganhHoc, maKhoiLop, maNamHoc);
                }
            }
        }

        public List<LopHocInfo> GetListLopHocInfo(int maNganhHoc, int maKhoiLop, int maNamHoc,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            if (maNganhHoc == 0)
            {
                if (maKhoiLop == 0)
                {
                    return lophocDA.GetListLopHocInfo(maNamHoc,
                        pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    return lophocDA.GetListLopHocInfoByKhoiLop(maKhoiLop, maNamHoc,
                        pageCurrentIndex, pageSize, out totalRecords);
                }
            }
            else
            {
                if (maKhoiLop == 0)
                {
                    return lophocDA.GetListLopHocInfoByNganhHoc(maNganhHoc, maNamHoc,
                        pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    return lophocDA.GetListLopHocInfo(maNganhHoc, maKhoiLop, maNamHoc,
                        pageCurrentIndex, pageSize, out totalRecords);
                }
            }
        }

        public List<LopHoc_Lop> GetListLopHocChuaCoGVCN(int maNamHoc, int maNganhHoc, int maKhoiLop)
        {
            if (maNganhHoc == 0)
            {
                if (maKhoiLop == 0)
                {
                    return lophocDA.GetListLopHocChuaCoGVCN(maNamHoc);
                }
                else
                {
                    return lophocDA.GetListLopHocChuaCoGVCNByKhoi(maNamHoc, maKhoiLop);
                }
            }
            else
            {
                if (maKhoiLop == 0)
                {
                    return lophocDA.GetListLopHocChuaCoGVCNByNganh(maNamHoc, maNganhHoc);
                }
                else
                {
                    return lophocDA.GetListLopHocChuaCoGVCN(maNamHoc, maNganhHoc, maKhoiLop);
                }
            }
            
        }
        #endregion

        public bool LopHocExists(string tenLopHoc, int maNamHoc)
        {
            return lophocDA.LopHocExists(tenLopHoc, maNamHoc);
        }

        public bool LopHocExists(int maLopHoc, string differTenLopHoc)
        {
            return lophocDA.LopHocExists(maLopHoc, differTenLopHoc);
        }

        public bool CanDeleteLopHoc(int maLopHoc)
        {
            return lophocDA.CanDeleteLopHoc(maLopHoc);
        }

        public bool HasGiaoVienChuNhiem(int maLopHoc)
        {
            return lophocDA.HasGiaoVienChuNhiem(maLopHoc);
        }
    }
}
