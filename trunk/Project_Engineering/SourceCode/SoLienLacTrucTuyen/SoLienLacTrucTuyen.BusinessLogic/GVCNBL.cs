using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class GVCNBL
    {
        private GVCNDA gvcnDA;

        public GVCNBL()
        {
            gvcnDA = new GVCNDA();
        }

        #region Insert, Update, Delete
        public void InsertGVCN(LopHoc_GVCN giaoVienChuNhiemEn)
        {
            gvcnDA.InsertGVCN(giaoVienChuNhiemEn);
        }

        public void UpdateGVCN(LopHoc_GVCN giaoVienChuNhiemEn)
        {
            gvcnDA.UpdateGVCN(giaoVienChuNhiemEn);
        }

        public void DeleteGVCN(int maGVCN)
        {
            gvcnDA.DeleteGVCN(maGVCN);
        }
        #endregion

        #region Entity, List
        public LopHoc_GVCN GetGVCN(int maGVCN)
        {
            return gvcnDA.GetGVCN(maGVCN);
        }

        public GVCNInfo GetGVCNInfo(int maLopHoc, string tenGVCN)
        {
            if ((string.Compare(tenGVCN, "tất cả", true) == 0) || (tenGVCN == ""))
            {
                return gvcnDA.GetGVCNInfo(maLopHoc);
            }
            else
            {
                return gvcnDA.GetGVCNInfo(maLopHoc, tenGVCN);
            }
        }

        public List<GVCNInfo> GetListGVCNInfo(int maNamHoc, int maNganhHoc, int maKhoiLop, string tenGVCN, 
            int pageCurrentIndex, int pageSize)
        {
            if ((string.Compare(tenGVCN, "tất cả", true) == 0) || (tenGVCN == ""))
            {
                if (maKhoiLop == 0)
                {
                    if (maNganhHoc == 0)
                    {
                        return gvcnDA.GetListGVCNInfo(maNamHoc, pageCurrentIndex, pageSize);
                    }
                    else
                    {
                        return gvcnDA.GetListGVCNInfoByNganhHoc(maNamHoc, maNganhHoc, pageCurrentIndex, pageSize);
                    }
                }
                else
                {
                    if (maNganhHoc == 0)
                    {
                        return gvcnDA.GetListGVCNInfoByKhoiLop(maNamHoc, maKhoiLop, pageCurrentIndex, pageSize);
                    }
                    else
                    {
                        return gvcnDA.GetListGVCNInfo(maNamHoc, maNganhHoc, maKhoiLop, pageCurrentIndex, pageSize);
                    }
                }
            }
            else
            {
                if (maKhoiLop == 0)
                {
                    if (maNganhHoc == 0)
                    {
                        return gvcnDA.GetListGVCNInfo(maNamHoc, tenGVCN, pageCurrentIndex, pageSize);
                    }
                    else
                    {
                        return gvcnDA.GetListGVCNInfoByNganhHoc(maNamHoc, maNganhHoc, tenGVCN, pageCurrentIndex, pageSize);
                    }
                }
                else
                {
                    if (maNganhHoc == 0)
                    {
                        return gvcnDA.GetListGVCNInfoByKhoiLop(maNamHoc, maKhoiLop, tenGVCN, pageCurrentIndex, pageSize);
                    }
                    else
                    {
                        return gvcnDA.GetListGVCNInfo(maNamHoc, maNganhHoc, maKhoiLop, tenGVCN, pageCurrentIndex, pageSize);
                    }
                }
            }
        }

        public double GetGVCNInfoCount(int maNamHoc, int maNganhHoc, int maKhoiLop, string tenGVCN)
        {
            if ((string.Compare(tenGVCN, "tất cả", true) == 0) || (tenGVCN == ""))
            {
                if (maKhoiLop == 0)
                {
                    if (maNganhHoc == 0)
                    {
                        return gvcnDA.GetGVCNInfoCount(maNamHoc);
                    }
                    else
                    {
                        return gvcnDA.GetGVCNInfoByNganhHocCount(maNamHoc, maNganhHoc);
                    }
                }
                else
                {
                    if (maNganhHoc == 0)
                    {
                        return gvcnDA.GetGVCNInfoByKhoiLopCount(maNamHoc, maKhoiLop);
                    }
                    else
                    {
                        return gvcnDA.GetGVCNInfoCount(maNamHoc, maNganhHoc, maKhoiLop);
                    }
                }
            }
            else
            {
                if (maKhoiLop == 0)
                {
                    if (maNganhHoc == 0)
                    {
                        return gvcnDA.GetGVCNInfoCount(maNamHoc, tenGVCN);
                    }
                    else
                    {
                        return gvcnDA.GetGVCNInfoByNganhHocCount(maNamHoc, maNganhHoc, tenGVCN);
                    }
                }
                else
                {
                    if (maNganhHoc == 0)
                    {
                        return gvcnDA.GetGVCNInfoByKhoiLopCount(maNamHoc, maKhoiLop, tenGVCN);
                    }
                    else
                    {
                        return gvcnDA.GetGVCNInfoCount(maNamHoc, maNganhHoc, maKhoiLop, tenGVCN);
                    }
                }
            }
        }

        #endregion
    }
}
