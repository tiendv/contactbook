using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class NgayNghiHocBL
    {
        private NgayNghiHocDA ngayNghiHocDA;

        public NgayNghiHocBL()
        {
            ngayNghiHocDA = new NgayNghiHocDA();
        }

        #region Insert, Update, Delete Methods
        public void InsertNgayNghiHoc(int maHocSinh, int maHocKy,
            DateTime ngay, int maBuoi,
            bool xinPhep, string lyDo)
        {
            HocSinhBL hocSinhBL = new HocSinhBL();
            int maLopHoc = (new HocSinhBL()).GetCurrentMaLopHoc(maHocSinh);
            ngayNghiHocDA.InsertNgayNghiHoc(maHocSinh, maLopHoc,
                maHocKy, ngay, maBuoi, xinPhep, lyDo);
        }

        public void UpdateNgayNghiHoc(int maNgayNghiHoc,
            int maHocKy, DateTime ngay, int maBuoi,
            bool xinPhep, string lyDo)
        {
            ngayNghiHocDA.UpdateNgayNghiHoc(maNgayNghiHoc,
                maHocKy, ngay, maBuoi, xinPhep, lyDo);
        }

        public void DeleteNgayNghiHoc(int maNgayNghiHoc)
        {
            ngayNghiHocDA.DeleteNgayNghiHoc(maNgayNghiHoc);
        }
        #endregion

        #region Get Entity, List
        public HocSinh_NgayNghiHoc GetNgayNghiHoc(int maNgayNghiHoc)
        {
            return ngayNghiHocDA.GetNgayNghiHoc(maNgayNghiHoc);
        }

        public HocSinh_NgayNghiHoc GetNgayNghiHoc(int maHocSinh, int maNamHoc, int maHocKy,
            DateTime ngay)
        {
            return ngayNghiHocDA.GetNgayNghiHoc(maHocSinh, maNamHoc, maHocKy, ngay);
        }

        public List<TabularNgayNghiHoc> GetListTabularNgayNghiHoc(int maHocSinh,
            int maNamHoc, int maHocKy,
            DateTime tuNgay, DateTime denNgay,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            return ngayNghiHocDA.GetListTabularNgayNghiHoc(maHocSinh, maNamHoc, maHocKy,
                tuNgay, denNgay,
                pageCurrentIndex, pageSize, out totalRecords);
        }
        #endregion

        public HocSinh_NgayNghiHoc GetNgayNghiHoc(int maNgayNghiHoc, int maHocSinh, int maNamHoc, int maHocKy,
            DateTime ngay)
        {
            return ngayNghiHocDA.GetNgayNghiHoc(maNgayNghiHoc, maHocSinh, maNamHoc, maHocKy, ngay);
        }

        public bool Confirmed(int maNgayNghiHoc)
        {
            return ngayNghiHocDA.Confirmed(maNgayNghiHoc);
        }

        public void ConfirmNgayNghiHoc(int maNgayNghiHoc)
        {
            bool xacNhan = true;
            ngayNghiHocDA.UpdateNgayNghiHoc(maNgayNghiHoc, xacNhan);
        }

        public bool NgayNghiHocExists(int? maNgayNghiHoc, int maHocSinh,
            int maHocKy, DateTime ngay, int maBuoi)
        {
            int maLopHoc = (new HocSinhBL()).GetCurrentMaLopHoc(maHocSinh);

            if (maNgayNghiHoc == null)
            {
                if (maBuoi == 0)
                {
                    return ngayNghiHocDA.NgayNghiHocExists(maHocSinh, maLopHoc, maHocKy, ngay);
                }
                else
                {
                    bool bAllDay = ngayNghiHocDA.NgayNghiHocExists(maHocSinh, maLopHoc, maHocKy, ngay, 0);
                    if (bAllDay)
                    {
                        return true;
                    }
                    else
                    {
                        return ngayNghiHocDA.NgayNghiHocExists(maHocSinh, maLopHoc, maHocKy, ngay, maBuoi);
                    }
                }
            }
            else
            {
                if (maBuoi == 0)
                {
                    return ngayNghiHocDA.NgayNghiHocExists((int)maNgayNghiHoc, maHocSinh, maLopHoc, maHocKy, ngay);
                }
                else
                {
                    bool bAllDay = ngayNghiHocDA.NgayNghiHocExists((int)maNgayNghiHoc, maHocSinh, maLopHoc, maHocKy, ngay, 0);
                    if (bAllDay)
                    {
                        return true;
                    }
                    else
                    {
                        return ngayNghiHocDA.NgayNghiHocExists((int)maNgayNghiHoc, maHocSinh, maLopHoc, maHocKy, ngay, maBuoi);
                    }
                }
            }
        }
    }
}
