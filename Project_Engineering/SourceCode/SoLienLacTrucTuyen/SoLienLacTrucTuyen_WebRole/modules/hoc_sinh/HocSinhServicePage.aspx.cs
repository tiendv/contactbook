using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen_WebRole.Modules;

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
            StudentBL hocSinhBL = new StudentBL((School)HttpContext.Current.Session[AppConstant.SCHOOL]);
            HocSinh_ThongTinCaNhan student = hocSinhBL.GetStudent(maHocSinhHienThi);
            int iMaHocSinh = student.MaHocSinh;
            if (iMaHocSinh.ToString() != maHocSinh)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //[WebMethod]
        //public static bool NgayNghiHocExists(int maNgayNghiHoc, int maHocSinh,
        //    int maHocKy, DateTime ngay, int maBuoi)
        //{
        //    bool bResult = false;
        //    AbsentBL ngayNghiHocBL = new AbsentBL(UserSchool);
        //    if (maNgayNghiHoc == 0)
        //    {
        //        bResult = ngayNghiHocBL.AbsentExists(null, maHocSinh, maHocKy, ngay, maBuoi);
        //    }
        //    else
        //    {
        //        bResult = ngayNghiHocBL.AbsentExists(maNgayNghiHoc, maHocSinh, maHocKy, ngay, maBuoi);
        //    }
        //    return bResult;
        //}

        [WebMethod(EnableSession = true)]
        public static void CheckedHanhKiem(string radioButtonName)
        {
            int i = "TbPnlKetQuaHocTap_PnlPopupHanhKiem_Rbtn".Length;
            string maHanhKiem = radioButtonName.Substring(i);

            string userName = HttpContext.Current.Session["username"].ToString();
            HttpContext.Current.Session[userName + "CheckHanhKiem"] = maHanhKiem;
        }

        //[WebMethod]
        //public static bool HoatDongExists(int maHoatDong, string tieuDe, int maHocSinh,
        //    int maHocKy, DateTime ngay)
        //{
        //    StudentActivityBL hoatDongBL = new StudentActivityBL(UserSchool);
        //    if (maHoatDong == 0)
        //    {
        //        return hoatDongBL.HoatDongExists(null, tieuDe,
        //        maHocSinh, maHocKy, ngay);
        //    }
        //    else
        //    {
        //        return hoatDongBL.HoatDongExists(maHoatDong, tieuDe,
        //            maHocSinh, maHocKy, ngay);
        //    }
        //}

        [WebMethod]
        public static bool ValidateMark(string marks, int markTypeId)
        {
            StudyingResultBL kqhtBL = new StudyingResultBL((School)HttpContext.Current.Session[AppConstant.SCHOOL]);
            DanhMuc_LoaiDiem markType = new DanhMuc_LoaiDiem();
            markType.MaLoaiDiem = markTypeId;
            return kqhtBL.ValidateMark(marks, markType);
        }
    }
}