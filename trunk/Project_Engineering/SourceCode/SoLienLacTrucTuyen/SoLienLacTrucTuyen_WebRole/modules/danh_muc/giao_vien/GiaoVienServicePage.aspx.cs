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
        public static bool UserIdExists(string UserIdHienThi)
        {
            UserIdHienThi = Uri.UnescapeDataString(UserIdHienThi);
            TeacherBL giaoVienBL = new TeacherBL((School_School)HttpContext.Current.Session[AppConstant.SCHOOL]);
            return giaoVienBL.TeacherCodeExists(UserIdHienThi);
        }

        //[WebMethod]
        //public static bool ExistGiaoVien(string UserIdHienThi)
        //{
        //    UserIdHienThi = Uri.UnescapeDataString(UserIdHienThi);
        //    GiaoVienBL giaoVienBL = new GiaoVienBL(UserSchool);
        //    return giaoVienBL.UserIdExists(UserIdHienThi);
        //}
    }
}