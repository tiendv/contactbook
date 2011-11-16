using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class GiaoVienServicePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static bool MaGiaoVienExists(string maGiaoVienHienThi)
        {
            maGiaoVienHienThi = Uri.UnescapeDataString(maGiaoVienHienThi);
            TeacherBL giaoVienBL = new TeacherBL((School)HttpContext.Current.Session[AppConstant.SCHOOL]);
            return giaoVienBL.TeacherCodeExists(maGiaoVienHienThi);
        }

        //[WebMethod]
        //public static bool ExistGiaoVien(string maGiaoVienHienThi)
        //{
        //    maGiaoVienHienThi = Uri.UnescapeDataString(maGiaoVienHienThi);
        //    GiaoVienBL giaoVienBL = new GiaoVienBL(UserSchool);
        //    return giaoVienBL.MaGiaoVienExists(maGiaoVienHienThi);
        //}
    }
}