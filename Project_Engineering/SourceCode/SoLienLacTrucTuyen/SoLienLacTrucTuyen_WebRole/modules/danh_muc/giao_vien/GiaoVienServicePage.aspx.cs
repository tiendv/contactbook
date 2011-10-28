using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using SoLienLacTrucTuyen.BusinessLogic;

namespace SoLienLacTrucTuyen_WebRole
{
    public partial class GiaoVienServicePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static bool MaGiaoVienExists(string maGiaoVienHienThi)
        {
            maGiaoVienHienThi = Uri.UnescapeDataString(maGiaoVienHienThi);
            GiaoVienBL giaoVienBL = new GiaoVienBL();
            return giaoVienBL.MaGiaoVienExists(maGiaoVienHienThi);
        }

        //[WebMethod]
        //public static bool ExistGiaoVien(string maGiaoVienHienThi)
        //{
        //    maGiaoVienHienThi = Uri.UnescapeDataString(maGiaoVienHienThi);
        //    GiaoVienBL giaoVienBL = new GiaoVienBL();
        //    return giaoVienBL.MaGiaoVienExists(maGiaoVienHienThi);
        //}
    }
}