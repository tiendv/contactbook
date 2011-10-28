using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class MonHocBL
    {
        private MonHocDA monHocDA;

        public MonHocBL()
        {
            monHocDA = new MonHocDA();
        }

        #region Insert, Update, Delete
        public void InsertMonHoc(string tenMonHoc, int maNganhHoc, int maKhoiLop,
            double heSoDiem)
        {
            monHocDA.InsertMonHoc(new DanhMuc_MonHoc()
            {
                TenMonHoc = tenMonHoc,
                MaNganhHoc = maNganhHoc,
                MaKhoiLop = maKhoiLop,
                HeSoDiem = heSoDiem                
            });
        }

        public void UpdateMonHoc(int maMonHoc, string tenMonHoc, double heSoDiem)
        {
            monHocDA.UpdateMonHoc(maMonHoc, tenMonHoc, heSoDiem);
        }

        public void DeleteMonHoc(int maMonHoc)
        {
            DanhMuc_MonHoc mon = monHocDA.GetMonHoc(maMonHoc);
            if (mon != null)
            {
                monHocDA.DeleteMonHoc(mon);
            }
        }
        #endregion

        #region Get Entity, List
        public DanhMuc_MonHoc GetMonHoc(int maMonHoc)
        {
            return monHocDA.GetMonHoc(maMonHoc);
        }

        public MonHocInfo GetMonHocInfo(int maMonHoc)
        {
            return monHocDA.GetMonHocInfo(maMonHoc);
        }

        public List<DanhMuc_MonHoc> GetListMonHoc(int maNganhHoc, int maKhoiLop)
        {
            if (maNganhHoc == 0)
            {
                if (maKhoiLop == 0)
                {
                    return monHocDA.GetListMonHoc();
                }
                else
                {
                    return monHocDA.GetListMonHocByKhoiLop(maKhoiLop);
                }
            }
            else
            {
                if (maKhoiLop == 0)
                {
                    return monHocDA.GetListMonHocByNganhHoc(maNganhHoc);
                }
                else
                {
                    return monHocDA.GetListMonHoc(maNganhHoc, maKhoiLop);
                }
            }
        }

        public List<MonHocInfo> GetListMonHocInfo(int maNganhHoc, int maKhoiLop, 
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            if (maNganhHoc == 0)
            {
                if (maKhoiLop == 0)
                {
                    return monHocDA.GetListMonHocInfo(pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    return monHocDA.GetListMonHocInfoByKhoiLop(maKhoiLop, 
                        pageCurrentIndex, pageSize, out totalRecords);
                }
            }
            else
            {
                if (maKhoiLop == 0)
                {
                    return monHocDA.GetListMonHocInfoByNganhHoc(maNganhHoc, 
                        pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    return monHocDA.GetListMonHocInfo(maNganhHoc, maKhoiLop, 
                        pageCurrentIndex, pageSize, out totalRecords);
                }
            }
        }

        public List<MonHocInfo> GetListMonHocInfo(int maNganhHoc, int maKhoiLop,
            string tenMonHoc,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            if ((tenMonHoc == "") || (string.Compare(tenMonHoc, "tất cả", true) == 0))
            {
                return monHocDA.GetListMonHocInfo(maNganhHoc, maKhoiLop,
                    pageCurrentIndex, pageSize, out totalRecords);
            }
            else
            {
                return monHocDA.GetListMonHocInfo(tenMonHoc,
                    pageCurrentIndex, pageSize, out totalRecords);
            }
        }

        public List<MonHocInfo> GetListMonHocInfo(int maNganhHoc, int maKhoiLop,
            string tenMonHoc, int? exceptedMaMonHoc,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            if ((tenMonHoc == "") || (string.Compare(tenMonHoc, "tất cả", true) == 0))
            {
                return monHocDA.GetListMonHocInfo(maNganhHoc, maKhoiLop, exceptedMaMonHoc,
                    pageCurrentIndex, pageSize, out totalRecords);
            }
            else
            {
                return monHocDA.GetListMonHocInfo(tenMonHoc, exceptedMaMonHoc,
                    pageCurrentIndex, pageSize, out totalRecords);
            }
        }
        #endregion

        public bool MonHocExists(string tenMonHoc, int maNganhHoc, int maKhoiLop)
        {
            return monHocDA.MonHocExists(tenMonHoc, maNganhHoc, maKhoiLop);
        }

        public bool MonHocExists(int maMonHoc, string tenMonHocMoi)
        {
            return monHocDA.MonHocExists(maMonHoc, tenMonHocMoi);
        }

        public bool MonHocExists(int maMonHoc, string tenMonHoc, int maNganhHoc, int maKhoiLop)
        {
            return monHocDA.MonHocExists(maMonHoc, tenMonHoc, maNganhHoc, maKhoiLop);
        }

        public bool CheckCanDeleteMonHoc(int maMonHoc)
        {
            return monHocDA.CheckCanDeleteMonHoc(maMonHoc);
        }        
        
        public List<MonHocInfo> GetListMonHocByLopHoc(int maLopHoc, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            LopHoc_Lop lop = (new LopHocBL()).GetLopHoc(maLopHoc);
            return GetListMonHocInfo(lop.MaNganhHoc, lop.MaKhoiLop, pageCurrentIndex, pageSize, out totalRecords);
        }        
        
        public double GetMonHocByLopHocCount(int maLopHoc)
        {
            LopHoc_Lop lop = (new LopHocBL()).GetLopHoc(maLopHoc);
            return GetListMonHoc(lop.MaNganhHoc, lop.MaKhoiLop).Count;
        }
    }
}
