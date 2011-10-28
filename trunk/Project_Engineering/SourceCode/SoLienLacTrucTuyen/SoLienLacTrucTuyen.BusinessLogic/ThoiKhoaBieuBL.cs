using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class ThoiKhoaBieuBL
    {
        private ThoiKhoaBieuDA thoiKhoaBieuDA;

        public ThoiKhoaBieuBL()
        {
            thoiKhoaBieuDA = new ThoiKhoaBieuDA();
        }

        public void Insert(int maLopHoc, int maMonHoc, int maGiaoVien,
            int maHocKy, int maThu, int maTiet)
        {
            TietBL tietBL = new TietBL();
            CauHinh_Buoi buoi = tietBL.GetBuoi(maTiet);

            LopHoc_MonHocTKB thoiKhoaBieuEntity = new LopHoc_MonHocTKB
            {
                MaLopHoc = maLopHoc,
                MaMonHoc = maMonHoc,
                MaGiaoVien = maGiaoVien,
                MaHocKy = maHocKy,
                MaThu = maThu,
                MaBuoi = buoi.MaBuoi,
                MaTiet = maTiet
            };

            thoiKhoaBieuDA.Insert(thoiKhoaBieuEntity);
        }

        public void Update(int maMonHocTKB, int maMonHoc, int maGiaoVien)
        {
            thoiKhoaBieuDA.Update(maMonHocTKB, maMonHoc, maGiaoVien);
        }

        public void Delete(int maMonHocTKB)
        {
            thoiKhoaBieuDA.Delete(maMonHocTKB);
        }        

        public bool ExistMonHocTKBInfo(int maNamHoc, int maHocKy, int maThu, int maBuoi, int maMonHoc, int maLopHoc)
        {
            return thoiKhoaBieuDA.ThoiKhoaBieuExists(maNamHoc, maHocKy, maThu, maBuoi, maMonHoc, maLopHoc);
        }

        // Get list
        public List<ThoiKhoaBieuTheoTiet> GetMonHocTKBInfo(int maLopHoc, 
            int maNamHoc, int maHocKy, int maThu, int maBuoi)
        {
            List<ThoiKhoaBieuTheoTiet> listThoiKhoaBieuTheoTiet = thoiKhoaBieuDA.GetThoiKhoaBieuTheoBuoi(maLopHoc, 
                maNamHoc, maHocKy, maThu, maBuoi);

            TietBL tietBL = new TietBL();
            foreach (ThoiKhoaBieuTheoTiet thoiKhoaBieuTheoTiet in listThoiKhoaBieuTheoTiet)
            {
                DanhMuc_Tiet tiet = tietBL.GetTiet(thoiKhoaBieuTheoTiet.Tiet);
                thoiKhoaBieuTheoTiet.ChiTietTiet = string.Format("{0}({1}-{2})", 
                    tiet.TenTiet, 
                    tiet.ThoiDiemKetThu.ToShortTimeString(), 
                    tiet.ThoiDiemKetThu.ToShortTimeString());
            }

            return listThoiKhoaBieuTheoTiet;
        }

        public List<ThoiKhoaBieuTheoBuoi> GetMonHocTKBBuoiInfo(int maNamHoc, int maHocKy, int maThu, int maLopHoc)
        {
            return thoiKhoaBieuDA.GetThoiKhoaBieuTheoThu(maNamHoc, maHocKy, maThu, maLopHoc);
        }

        public List<ThoiKhoaBieuTheoThu> GetThoiKhoaBieu(int maNamHoc, int maHocKy, int maLopHoc)
        {
            return thoiKhoaBieuDA.GetThoiKhoaBieu(maNamHoc, maHocKy, maLopHoc);
        }

        public List<ThoiKhoaBieuTheoTiet> GetThoiKhoaBieuTheoThu(int maLopHoc, int maHocKy, int maThu)
        {
            List<ThoiKhoaBieuTheoTiet> listThoiKhoaBieuTheoTiet = thoiKhoaBieuDA.GetThoiKhoaBieuTheoBuoi(maLopHoc,
                maHocKy, maThu);

            TietBL tietBL = new TietBL();
            foreach (ThoiKhoaBieuTheoTiet thoiKhoaBieuTheoTiet in listThoiKhoaBieuTheoTiet)
            {
                DanhMuc_Tiet tiet = tietBL.GetTiet(thoiKhoaBieuTheoTiet.Tiet);
                thoiKhoaBieuTheoTiet.ChiTietTiet = string.Format("{0} ({1}-{2})",
                    tiet.TenTiet,
                    tiet.ThoiDiemKetThu.ToShortTimeString(),
                    tiet.ThoiDiemKetThu.ToShortTimeString());
            }

            return listThoiKhoaBieuTheoTiet;
        }

        public ThoiKhoaBieuTheoTiet GetThoiKhoaBieuTheoTiet(int maMonHocTKB)
        {
            return thoiKhoaBieuDA.GetThoiKhoaBieuTheoTiet(maMonHocTKB);
        }

        public List<DanhMuc_MonHoc> GetListMonHoc(int maLopHoc, int maHocKy)
        {
            return thoiKhoaBieuDA.GetListMonHoc(maLopHoc, maHocKy);
        }
    }
}
