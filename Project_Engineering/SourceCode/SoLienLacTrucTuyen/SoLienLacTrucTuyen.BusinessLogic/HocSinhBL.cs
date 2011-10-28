using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class HocSinhBL
    {
        private HocSinhDA hocSinhDA;

        public HocSinhBL()
        {
            hocSinhDA = new HocSinhDA();
        }

        #region Insert, Update, Delete
        public void InsertHocSinh(int maLopHoc,
            string maHocSinhHienThi, string tenHocSinh,
            bool gioiTinhHocSinh, DateTime ngaySinhHocSinh, string noiSinhHocSinh,
            string diaChi, string dienThoai,
            string hoTenBo, string ngheNghiepBo, DateTime? ngaySinhBo,
            string hoTenMe, string ngheNghiepMe, DateTime? ngaySinhMe,
            string hoTenNguoiDoDau, string ngheNghiepNguoiDoDau, DateTime? ngaySinhNguoiDoDau)
        {
            hocSinhDA.InsertHocSinh(maLopHoc,
                maHocSinhHienThi, tenHocSinh,
                gioiTinhHocSinh, ngaySinhHocSinh, noiSinhHocSinh,
                diaChi, dienThoai,
                hoTenBo, ngheNghiepBo, ngaySinhBo,
                hoTenMe, ngheNghiepMe, ngaySinhMe,
                hoTenNguoiDoDau, ngheNghiepNguoiDoDau, ngaySinhNguoiDoDau);
        }

        public void UpdateHocSinh(int maHocSinh, int maLopHoc, string maHocSinhHienThi, string tenHocSinh,
            bool gioiTinh, DateTime ngaySinh, string noiSinh, string diaChi, string dienThoai,
            string tenBo, string ngheNghiepBo, DateTime? ngaySinhBo,
            string tenMe, string ngheNghiepMe, DateTime? ngaySinhMe,
            string tenNguoiDoDau, string ngheNghiepNguoiDoDau, DateTime? ngaySinhNguoiDoDau)
        {
            hocSinhDA.UpdateHocSinh(maHocSinh, maLopHoc,
                maHocSinhHienThi, tenHocSinh,
                gioiTinh, ngaySinh, noiSinh,
                diaChi, dienThoai,
                tenBo, ngheNghiepBo, ngaySinhBo,
                tenMe, ngheNghiepMe, ngaySinhMe,
                tenNguoiDoDau, ngheNghiepNguoiDoDau, ngaySinhNguoiDoDau);
        }

        public void DeleteHocSinh(int maHocSinh)
        {
            hocSinhDA.DeleteHocSinh(maHocSinh);
        }
        #endregion

        #region Get Lists
        public HocSinh_ThongTinCaNhan GetThongTinCaNhan(int maHocSinh)
        {
            return hocSinhDA.GetThongTinCaNhan(maHocSinh);
        }

        public List<TabularHocSinhInfo> GetListTabularHocSinhInfo(int maNamHoc, int maNganhHoc, int maKhoiLop,
            int maLopHoc, string tenHocSinh,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            if (maNganhHoc == 0)
            {
                if (maKhoiLop == 0)
                {
                    if (maLopHoc == 0)
                    {
                        if ((string.Compare(tenHocSinh, "tất cả", true) == 0) || (tenHocSinh == ""))
                        {
                            return hocSinhDA.GetListTabularHocSinhInfo(maNamHoc,
                                pageCurrentIndex, pageSize, out totalRecords);
                        }
                        else
                        {
                            return hocSinhDA.GetListTabularHocSinhInfoByHoTen(maNamHoc,
                                tenHocSinh,
                                pageCurrentIndex, pageSize, out totalRecords);
                        }
                    }
                    else
                    {
                        if ((string.Compare(tenHocSinh, "tất cả", true) == 0) || (tenHocSinh == ""))
                        {
                            return hocSinhDA.GetListTabularHocSinhInfoByLop(maNamHoc, maLopHoc,
                                pageCurrentIndex, pageSize, out totalRecords);
                        }
                        else
                        {
                            return hocSinhDA.GetListTabularHocSinhInfoByLopNHoTen(maNamHoc, maLopHoc,
                                tenHocSinh,
                                pageCurrentIndex, pageSize, out totalRecords);
                        }
                    }
                }
                else
                {
                    if (maLopHoc == 0)
                    {
                        if ((string.Compare(tenHocSinh, "tất cả", true) == 0) || (tenHocSinh == ""))
                        {
                            return hocSinhDA.GetListTabularHocSinhInfoByKhoi(maNamHoc, maKhoiLop,
                                pageCurrentIndex, pageSize, out totalRecords);
                        }
                        else
                        {
                            return hocSinhDA.GetListTabularHocSinhInfoByKhoiNHoTen(maNamHoc, maKhoiLop,
                                tenHocSinh, pageCurrentIndex, pageSize, out totalRecords);
                        }
                    }
                    else
                    {
                        if ((string.Compare(tenHocSinh, "tất cả", true) == 0) || (tenHocSinh == ""))
                        {
                            return hocSinhDA.GetListTabularHocSinhInfoByKhoiNLopHoc(maNamHoc, maKhoiLop,
                                maLopHoc, pageCurrentIndex, pageSize, out totalRecords);
                        }
                        else
                        {
                            return hocSinhDA.GetListTabularHocSinhInfoByKhoiNLopHocNHoTen(maNamHoc, maKhoiLop,
                                maLopHoc, tenHocSinh,
                                pageCurrentIndex, pageSize, out totalRecords);
                        }
                    }
                }
            }
            else
            {
                if (maKhoiLop == 0)
                {
                    if (maLopHoc == 0)
                    {
                        if ((string.Compare(tenHocSinh, "tất cả", true) == 0) || (tenHocSinh == ""))
                        {
                            return hocSinhDA.GetListTabularHocSinhInfoByNganh(maNamHoc, maNganhHoc,
                                pageCurrentIndex, pageSize, out totalRecords);
                        }
                        else
                        {
                            return hocSinhDA.GetListTabularHocSinhInfoByNganhNHoTen(maNamHoc, maNganhHoc,
                                tenHocSinh, pageCurrentIndex, pageSize, out totalRecords);
                        }
                    }
                    else
                    {
                        if ((string.Compare(tenHocSinh, "tất cả", true) == 0) || (tenHocSinh == ""))
                        {
                            return hocSinhDA.GetListTabularHocSinhInfoByNganhNLop(maNamHoc, maNganhHoc, maLopHoc,
                                pageCurrentIndex, pageSize, out totalRecords);
                        }
                        else
                        {
                            return hocSinhDA.GetListTabularHocSinhInfoByNganhNLopNHoTen(maNamHoc, maNganhHoc,
                                maLopHoc, tenHocSinh,
                                pageCurrentIndex, pageSize, out totalRecords);
                        }
                    }
                }
                else
                {
                    if (maLopHoc == 0)
                    {
                        if ((string.Compare(tenHocSinh, "tất cả", true) == 0) || (tenHocSinh == ""))
                        {
                            return hocSinhDA.GetListTabularHocSinhInfoByNganhNKhoi(maNamHoc, maNganhHoc, maKhoiLop,
                                pageCurrentIndex, pageSize, out totalRecords);
                        }
                        else
                        {
                            return hocSinhDA.GetListTabularHocSinhInfoByNganhNKhoiNHoTen(maNamHoc, maNganhHoc, maKhoiLop,
                                tenHocSinh, pageCurrentIndex, pageSize, out totalRecords);
                        }
                    }
                    else
                    {
                        if ((string.Compare(tenHocSinh, "tất cả", true) == 0) || (tenHocSinh == ""))
                        {
                            return hocSinhDA.GetListTabularHocSinhInfoByNganhNKhoiNLopHoc(maNamHoc, maNganhHoc, maKhoiLop,
                                maLopHoc,
                                pageCurrentIndex, pageSize, out totalRecords);
                        }
                        else
                        {
                            return hocSinhDA.GetListTabularHocSinhInfoByNganhNKhoiNLopHocNHoTen(maNamHoc, maNganhHoc, maKhoiLop,
                                maLopHoc, tenHocSinh,
                                pageCurrentIndex, pageSize, out totalRecords);
                        }
                    }
                }
            }
        }

        public List<TabularHocSinhInfo> GetListTabularHocSinhInfo(string maHocSinhHienThi,
            out double totalRecords)
        {
            return hocSinhDA.GetListTabularHocSinhInfo(maHocSinhHienThi, out totalRecords);
        }
        #endregion

        public int GetMaHocSinh(string maHocSinhHienThi)
        {
            return hocSinhDA.GetMaHocSinh(maHocSinhHienThi);
        }

        public bool CanDeleteHocSinh(string maHocSinhHienThi)
        {
            return hocSinhDA.CanDeleteHocSinh(maHocSinhHienThi);
        }

        public LopHocInfo GetLopHocInfo(int maNamHoc, int maHocSinh)
        {
            int maLopHoc = hocSinhDA.GetMaLopHoc(maNamHoc, maHocSinh);
            if (maLopHoc != 0)
            {
                return (new LopHocBL()).GetLopHocInfo(maLopHoc);
            }
            else
            {
                return null;
            }
        }

        public int GetMaLopHoc(int maNamHoc, int maHocSinh)
        {
            return hocSinhDA.GetMaLopHoc(maNamHoc, maHocSinh);
        }

        public List<DDLFormatHocSinh> GetListHocSinh(int maNganh, int maKhoi, int maLopHoc)
        {
            if (maLopHoc == 0)
            {
                if (maNganh == 0)
                {
                    if (maKhoi == 0)
                    {
                        return hocSinhDA.GetListHocSinh();
                    }
                    else
                    {
                        return hocSinhDA.GetListHocSinhByKhoi(maKhoi);
                    }
                }
                else
                {
                    if (maKhoi == 0)
                    {
                        return hocSinhDA.GetListHocSinhByNganh(maNganh);
                    }
                    else
                    {
                        return hocSinhDA.GetListHocSinhByNganh(maNganh, maKhoi);
                    }
                }
            }
            else
            {
                return hocSinhDA.GetListHocSinh(maLopHoc);
            }
        }

        public HocSinh_HocSinhLopHoc GetHocSinhLopHoc(int maHocSinhLopHoc)
        {
            return hocSinhDA.GetHocSinhLopHoc(maHocSinhLopHoc);
        }

        public HocSinh_HocSinhLopHoc GetHocSinhLopHoc(int maNamHoc, int maHocSinh)
        {
            return hocSinhDA.GetHocSinhLopHoc(maNamHoc, maHocSinh);
        }

        public List<CauHinh_NamHoc> GetListNamHoc(int maHocSinh)
        {
            return hocSinhDA.GetListNamHoc(maHocSinh);
        }

        public bool MaHocSinhExists(string maHocSinhHienThi)
        {
            return hocSinhDA.MaHocSinhExists(maHocSinhHienThi);
        }

        public LopHoc_Lop GetLopHoc(int maHocSinh, int maNamHoc)
        {
            return hocSinhDA.GetLopHoc(maHocSinh, maNamHoc);
        }

        public int GetCurrentMaLopHoc(int maHocSinh)
        {
            return hocSinhDA.GetCurrentMaLopHoc(maHocSinh);
        }

        public List<TabularHanhKiemHocSinh> GetListHanhKiemHocSinh(int maLopHoc, int maHocKy,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            return hocSinhDA.GetListHanhKiemHocSinh(maLopHoc, maHocKy,
                pageCurrentIndex, pageSize, out totalRecords);
        }

        public void UpdateHanhKiemHocSinh(int maLopHoc, int maHocKy, int maHocSinh, int? maHanhKiem)
        {
            hocSinhDA.UpdateHanhKiemHocSinh(maLopHoc, maHocKy, maHocSinh, maHanhKiem);
        }
    }
}
