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
            StudentBL hocSinhBL = new StudentBL((School_School)HttpContext.Current.Session[AppConstant.SCHOOL]);
            Student_Student student = hocSinhBL.GetStudent(maHocSinhHienThi);
            int iMaHocSinh = student.StudentId;
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
        //    int TermId, DateTime ngay, int SessionId)
        //{
        //    bool bResult = false;
        //    AbsentBL ngayNghiHocBL = new AbsentBL(UserSchool);
        //    if (maNgayNghiHoc == 0)
        //    {
        //        bResult = ngayNghiHocBL.AbsentExists(null, maHocSinh, TermId, ngay, SessionId);
        //    }
        //    else
        //    {
        //        bResult = ngayNghiHocBL.AbsentExists(maNgayNghiHoc, maHocSinh, TermId, ngay, SessionId);
        //    }
        //    return bResult;
        //}

        [WebMethod(EnableSession = true)]
        public static void CheckedHanhKiem(string radioButtonName)
        {
            int i = "TbPnlKetQuaHocTap_PnlPopupHanhKiem_Rbtn".Length;
            string ConductId = radioButtonName.Substring(i);

            string userName = HttpContext.Current.Session["username"].ToString();
            HttpContext.Current.Session[userName + "CheckHanhKiem"] = ConductId;
        }

        //[WebMethod]
        //public static bool HoatDongExists(int maHoatDong, string tieuDe, int maHocSinh,
        //    int TermId, DateTime ngay)
        //{
        //    StudentActivityBL hoatDongBL = new StudentActivityBL(UserSchool);
        //    if (maHoatDong == 0)
        //    {
        //        return hoatDongBL.HoatDongExists(null, tieuDe,
        //        maHocSinh, TermId, ngay);
        //    }
        //    else
        //    {
        //        return hoatDongBL.HoatDongExists(maHoatDong, tieuDe,
        //            maHocSinh, TermId, ngay);
        //    }
        //}

        [WebMethod]
        public static bool ValidateMark(string marks, int markTypeId)
        {
            StudyingResultBL kqhtBL = new StudyingResultBL((School_School)HttpContext.Current.Session[AppConstant.SCHOOL]);
            Category_MarkType markType = new Category_MarkType();
            markType.MarkTypeId = markTypeId;
            return kqhtBL.ValidateMark(marks, markType);
        }
    }
}