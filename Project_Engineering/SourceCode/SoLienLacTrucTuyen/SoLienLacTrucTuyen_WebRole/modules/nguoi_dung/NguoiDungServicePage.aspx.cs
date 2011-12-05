using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using SoLienLacTrucTuyen.BusinessLogic;
using System.Web.Security;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen_WebRole.Modules;

namespace SoLienLacTrucTuyen_WebRole
{
    public partial class NguoiDungServicePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        //[WebMethod]
        //public static bool ExistMaHocSinh(string maHocSinhHienThi)
        //{
        //    HocSinhBL hocSinhBL = new HocSinhBL(UserSchool);
        //    int maHocSinh = hocSinhBL.GetMaHocSinh(maHocSinhHienThi);
        //    if (maHocSinh == 0)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        //[WebMethod]
        //public static bool ExistNguoiDung(string maNguoiDung, string tenNguoiDung)
        //{
        //    NguoiDungBL nguoiDungBL = new NguoiDungBL(UserSchool);
        //    int iMaNguoiDung = Int32.Parse(maNguoiDung);
        //    return nguoiDungBL.ExistNguoiDung(iMaNguoiDung, tenNguoiDung);
        //}

        [WebMethod]
        public static bool RoleExists(string roleName)
        {
            return (new RoleBL((School_School)HttpContext.Current.Session[AppConstant.SCHOOL])).RoleExists(roleName);
        }

        [WebMethod]
        public static bool RoleExists(string roleName, string newRoleName)
        {
            return (new RoleBL((School_School)HttpContext.Current.Session[AppConstant.SCHOOL])).RoleExists(roleName, newRoleName);
        }
    }
}