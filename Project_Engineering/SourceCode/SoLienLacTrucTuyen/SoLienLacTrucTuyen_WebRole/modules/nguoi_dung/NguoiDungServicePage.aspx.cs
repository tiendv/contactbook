using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using SoLienLacTrucTuyen.BusinessLogic;
using System.Web.Security;

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
        //    HocSinhBL hocSinhBL = new HocSinhBL();
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
        //    NguoiDungBL nguoiDungBL = new NguoiDungBL();
        //    int iMaNguoiDung = Int32.Parse(maNguoiDung);
        //    return nguoiDungBL.ExistNguoiDung(iMaNguoiDung, tenNguoiDung);
        //}

        [WebMethod]
        public static bool RoleExists(string roleName)
        {
            return (new RoleBL()).RoleExists(roleName);
        }

        [WebMethod]
        public static bool RoleExists(string roleName, string newRoleName)
        {
            return (new RoleBL()).RoleExists(roleName, newRoleName);
        }
    }
}