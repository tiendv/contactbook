using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen_WebRole.Modules;

namespace SoLienLacTrucTuyen_WebRole
{
    /// <summary>
    /// Summary description for HocSinhWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class HocSinhWebService : System.Web.Services.WebService
    {
        [ScriptMethod]
        [WebMethod]
        public static bool ExistMaHocSinhHienThi(string maHocSinh, string maHocSinhHienThi)
        {
            StudentBL hocSinhBL = new StudentBL((School_School)HttpContext.Current.Session[AppConstant.SCHOOL]);
            Student_Student student = hocSinhBL.GetStudent(maHocSinhHienThi);
            if (student != null)
            {
                if (student.StudentId.ToString() != maHocSinh)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            
        }
    }
}
