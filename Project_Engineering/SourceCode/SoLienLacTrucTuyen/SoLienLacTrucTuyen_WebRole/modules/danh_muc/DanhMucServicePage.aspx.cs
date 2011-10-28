using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;
using SoLienLacTrucTuyen.BusinessLogic;

namespace SoLienLacTrucTuyen_WebRole
{
    public partial class DanhMucServicePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static bool CheckExistTenNganhHoc(int maNganhHoc, string tenNganhHoc)
        {
            tenNganhHoc = Uri.UnescapeDataString(tenNganhHoc);
            FacultyBL nganhhocBL = new FacultyBL();
            return nganhhocBL.NganhHocExists(maNganhHoc, tenNganhHoc);
        }

        [WebMethod]
        public static bool CheckExistTenKhoiLop(int maKhoiLop, string tenKhoiLop)
        {
            tenKhoiLop = Uri.UnescapeDataString(tenKhoiLop);
            KhoiLopBL khoiLopBL = new KhoiLopBL();
            return khoiLopBL.KhoiLopExists(maKhoiLop, tenKhoiLop);
        }

        [WebMethod]
        public static bool CheckExistTenMonHoc(string tenMonHoc, int maNganhHoc, int maKhoiLop)
        {
            tenMonHoc = Uri.UnescapeDataString(tenMonHoc);
            MonHocBL monHocBL = new MonHocBL();
            return monHocBL.MonHocExists(tenMonHoc, maNganhHoc, maKhoiLop);
        }

        [WebMethod]
        public static bool CheckExistTenMonHoc(int maMonHoc, string tenMonHoc, int maNganhHoc, int maKhoiLop)
        {
            tenMonHoc = Uri.UnescapeDataString(tenMonHoc);
            MonHocBL monHocBL = new MonHocBL();
            return monHocBL.MonHocExists(maMonHoc, tenMonHoc, maNganhHoc, maKhoiLop);
        }

        [WebMethod]
        public static bool CheckExistTenLoaiDiem(int maLoaiDiem, string tenLoaiDiem)
        {
            tenLoaiDiem = Uri.UnescapeDataString(tenLoaiDiem);
            LoaiDiemBL loaiDiemBL = new LoaiDiemBL();
            return loaiDiemBL.LoaiDiemExists(maLoaiDiem, tenLoaiDiem);
        }

        [WebMethod]
        public static bool CheckExistTenThaiDoThamGia(int maThaiDoThamGia, string tenThaiDoThamGia)
        {
            tenThaiDoThamGia = Uri.UnescapeDataString(tenThaiDoThamGia);
            ThaiDoThamGiaBL thaiDoThamGiaBL = new ThaiDoThamGiaBL();
            return thaiDoThamGiaBL.CheckExistTenThaiDoThamGia(maThaiDoThamGia, tenThaiDoThamGia);
        }

        [WebMethod]
        public static bool CheckExistTenHanhKiem(int maHanhKiem, string tenHanhKiem)
        {
            tenHanhKiem = Uri.UnescapeDataString(tenHanhKiem);
            HanhKiemBL hanhKiemBL = new HanhKiemBL();
            bool b = hanhKiemBL.CheckExistTenHanhKiem(maHanhKiem, tenHanhKiem);
            return hanhKiemBL.CheckExistTenHanhKiem(maHanhKiem, tenHanhKiem);
        }

        [WebMethod]
        public static bool CheckExistTenDanhHieu(string tenDanhHieu)
        {
            tenDanhHieu = Uri.UnescapeDataString(tenDanhHieu);
            DanhHieuBL danhHieuBL = new DanhHieuBL();
            return danhHieuBL.DanhHieuExists(tenDanhHieu);
        }
    }
}