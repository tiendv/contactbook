using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen_WebRole.Modules;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen_WebRole
{
    public partial class DanhMucServicePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static bool FacultyExists(string facultyName)
        {
            facultyName = Uri.UnescapeDataString(facultyName);

            FacultyBL facultyBL = new FacultyBL((School)HttpContext.Current.Session[AppConstant.SCHOOL]);
            return facultyBL.FacultyExists(facultyName);
        }

        [WebMethod]
        public static bool FacultyExists(string oldFacultyName, string newFacultyName)
        {
            oldFacultyName = Uri.UnescapeDataString(oldFacultyName);
            newFacultyName = Uri.UnescapeDataString(newFacultyName);

            FacultyBL facultyBL = new FacultyBL((School)HttpContext.Current.Session[AppConstant.SCHOOL]);
            return facultyBL.FacultyExists(oldFacultyName, newFacultyName);
        }

        //[WebMethod]
        //public static bool CheckExistTenKhoiLop(int maKhoiLop, string tenKhoiLop)
        //{
        //    tenKhoiLop = Uri.UnescapeDataString(tenKhoiLop);
        //    GradeBL grades = new GradeBL((School)HttpContext.Current.Session[AppConstant.SCHOOL]);
        //    return grades.GradeNameExists(maKhoiLop, tenKhoiLop);
        //}

        //[WebMethod]
        //public static bool CheckExistTenMonHoc(string tenMonHoc, int maNganhHoc, int maKhoiLop)
        //{
        //    tenMonHoc = Uri.UnescapeDataString(tenMonHoc);
        //    SubjectBL monHocBL = new SubjectBL((School)HttpContext.Current.Session[AppConstant.SCHOOL]);
        //    return monHocBL.SubjectNameExists(tenMonHoc, maNganhHoc, maKhoiLop);
        //}

        //[WebMethod]
        //public static bool CheckExistTenMonHoc(int maMonHoc, string tenMonHoc, int maNganhHoc, int maKhoiLop)
        //{
        //    tenMonHoc = Uri.UnescapeDataString(tenMonHoc);
        //    SubjectBL monHocBL = new SubjectBL((School)HttpContext.Current.Session[AppConstant.SCHOOL]);
        //    return monHocBL.SubjectNameExists(maMonHoc, tenMonHoc, maNganhHoc, maKhoiLop);
        //}

        [WebMethod]
        public static bool MarkTypeNameExists(string markTypeName)
        {
            MarkTypeBL loaiDiemBL = new MarkTypeBL((School)HttpContext.Current.Session[AppConstant.SCHOOL]);

            markTypeName = Uri.UnescapeDataString(markTypeName);            
            return loaiDiemBL.MarkTypeNameExists(markTypeName);
        }

        [WebMethod]
        public static bool MarkTypeNameExists(string oldMarkTypeName, string newMarkTypeName)
        {
            MarkTypeBL loaiDiemBL = new MarkTypeBL((School)HttpContext.Current.Session[AppConstant.SCHOOL]);

            oldMarkTypeName = Uri.UnescapeDataString(oldMarkTypeName);
            newMarkTypeName = Uri.UnescapeDataString(newMarkTypeName);

            if (oldMarkTypeName == "" || oldMarkTypeName == newMarkTypeName)
            {
                return false;
            }
            else
            {
                return loaiDiemBL.MarkTypeNameExists(newMarkTypeName);
            }
        }

        [WebMethod]
        public static bool AttitudeNameExists(string oldAttitudeName, string newAttitudeName)
        {
            AttitudeBL thaiDoThamGiaBL = new AttitudeBL((School)HttpContext.Current.Session[AppConstant.SCHOOL]);

            oldAttitudeName = Uri.UnescapeDataString(oldAttitudeName);
            newAttitudeName = Uri.UnescapeDataString(newAttitudeName);

            if (oldAttitudeName == "")
            {
                return thaiDoThamGiaBL.AttitudeNameExists(newAttitudeName);
            }
            else
            {
                return thaiDoThamGiaBL.AttitudeNameExists(oldAttitudeName, newAttitudeName);
            }            
        }

        [WebMethod]
        public static bool CheckExistTenHanhKiem(string tenHanhKiem)
        {
            tenHanhKiem = Uri.UnescapeDataString(tenHanhKiem);
            ConductBL hanhKiemBL = new ConductBL((School)HttpContext.Current.Session[AppConstant.SCHOOL]);
            return hanhKiemBL.ConductNameExists(tenHanhKiem);
        }

        [WebMethod]
        public static bool CheckExistTenHanhKiem(string oldTenHanhKiem, string newTenHanhKiem)
        {
            oldTenHanhKiem = Uri.UnescapeDataString(oldTenHanhKiem);
            newTenHanhKiem = Uri.UnescapeDataString(newTenHanhKiem);

            ConductBL hanhKiemBL = new ConductBL((School)HttpContext.Current.Session[AppConstant.SCHOOL]);
            return hanhKiemBL.ConductNameExists(oldTenHanhKiem, newTenHanhKiem);
        }

        [WebMethod]
        public static bool CheckExistTenDanhHieu(string tenDanhHieu)
        {
            tenDanhHieu = Uri.UnescapeDataString(tenDanhHieu);
            DanhHieuBL danhHieuBL = new DanhHieuBL((School)HttpContext.Current.Session[AppConstant.SCHOOL]);
            return danhHieuBL.DanhHieuExists(tenDanhHieu);
        }
    }
}