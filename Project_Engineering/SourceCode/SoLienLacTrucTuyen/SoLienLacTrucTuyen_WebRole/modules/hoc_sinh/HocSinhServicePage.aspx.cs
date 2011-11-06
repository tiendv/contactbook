using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen_WebRole
{
    public partial class HocSinhServicePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static bool ValidateDateTime(string dateTime)
        {
            bool bValid = true;
            try
            {
                DateTime.Parse(dateTime);
            }
            catch (Exception ex)
            {
                bValid = false;
            }
            return bValid;
        }

        [WebMethod]
        public static bool ExistMaHocSinhHienThi(string maHocSinh, string maHocSinhHienThi)
        {
            HocSinhBL hocSinhBL = new HocSinhBL();

            int iMaHocSinh = hocSinhBL.GetMaHocSinh(maHocSinhHienThi);
            if (iMaHocSinh.ToString() != maHocSinh)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [WebMethod]
        public static bool NgayNghiHocExists(int maNgayNghiHoc, int maHocSinh,
            int maHocKy, DateTime ngay, int maBuoi)
        {
            bool bResult = false;
            NgayNghiHocBL ngayNghiHocBL = new NgayNghiHocBL();
            if (maNgayNghiHoc == 0)
            {
                bResult = ngayNghiHocBL.NgayNghiHocExists(null, maHocSinh, maHocKy, ngay, maBuoi);
            }
            else
            {
                bResult = ngayNghiHocBL.NgayNghiHocExists(maNgayNghiHoc, maHocSinh, maHocKy, ngay, maBuoi);
            }
            return bResult;
        }

        [WebMethod(EnableSession = true)]
        public static void CheckedHanhKiem(string radioButtonName)
        {
            int i = "TbPnlKetQuaHocTap_PnlPopupHanhKiem_Rbtn".Length;
            string maHanhKiem = radioButtonName.Substring(i);

            string userName = HttpContext.Current.Session["username"].ToString();
            HttpContext.Current.Session[userName + "CheckHanhKiem"] = maHanhKiem;
        }

        [WebMethod]
        public static bool HoatDongExists(int maHoatDong, string tieuDe, int maHocSinh,
            int maHocKy, DateTime ngay)
        {
            HoatDongBL hoatDongBL = new HoatDongBL();
            if (maHoatDong == 0)
            {
                return hoatDongBL.HoatDongExists(null, tieuDe,
                maHocSinh, maHocKy, ngay);
            }
            else
            {
                return hoatDongBL.HoatDongExists(maHoatDong, tieuDe,
                    maHocSinh, maHocKy, ngay);
            }
        }

        [WebMethod]
        public static bool ValidateMark(string marks, string markTypeName)
        {
            KetQuaHocTapBL kqhtBL = new KetQuaHocTapBL();
            return kqhtBL.ValidateMark(marks, markTypeName);
        }
    }
}