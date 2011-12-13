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
using EContactBook.DataAccess;

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

            FacultyBL facultyBL = new FacultyBL((School_School)HttpContext.Current.Session[AppConstant.SCHOOL]);
            return facultyBL.FacultyExists(facultyName);
        }

        [WebMethod]
        public static bool FacultyExists(string oldFacultyName, string newFacultyName)
        {
            oldFacultyName = Uri.UnescapeDataString(oldFacultyName);
            newFacultyName = Uri.UnescapeDataString(newFacultyName);

            FacultyBL facultyBL = new FacultyBL((School_School)HttpContext.Current.Session[AppConstant.SCHOOL]);
            return facultyBL.FacultyExists(oldFacultyName, newFacultyName);
        }

        //[WebMethod]
        //public static bool CheckExistGradeName(int GradeId, string GradeName)
        //{
        //    GradeName = Uri.UnescapeDataString(GradeName);
        //    GradeBL grades = new GradeBL((School)HttpContext.Current.Session[AppConstant.SCHOOL]);
        //    return grades.GradeNameExists(GradeId, GradeName);
        //}

        //[WebMethod]
        //public static bool CheckExistSubjectName(string SubjectName, int FacultyId, int GradeId)
        //{
        //    SubjectName = Uri.UnescapeDataString(SubjectName);
        //    SubjectBL monHocBL = new SubjectBL((School)HttpContext.Current.Session[AppConstant.SCHOOL]);
        //    return monHocBL.SubjectNameExists(SubjectName, FacultyId, GradeId);
        //}

        //[WebMethod]
        //public static bool CheckExistSubjectName(int SubjectId, string SubjectName, int FacultyId, int GradeId)
        //{
        //    SubjectName = Uri.UnescapeDataString(SubjectName);
        //    SubjectBL monHocBL = new SubjectBL((School)HttpContext.Current.Session[AppConstant.SCHOOL]);
        //    return monHocBL.SubjectNameExists(SubjectId, SubjectName, FacultyId, GradeId);
        //}

        [WebMethod]
        public static bool MarkTypeNameExists(string markTypeName)
        {
            MarkTypeBL loaiDiemBL = new MarkTypeBL((School_School)HttpContext.Current.Session[AppConstant.SCHOOL]);

            markTypeName = Uri.UnescapeDataString(markTypeName);            
            return loaiDiemBL.MarkTypeNameExists(markTypeName);
        }

        [WebMethod]
        public static bool MarkTypeNameExists(string oldMarkTypeName, string newMarkTypeName)
        {
            MarkTypeBL loaiDiemBL = new MarkTypeBL((School_School)HttpContext.Current.Session[AppConstant.SCHOOL]);

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
            AttitudeBL thaiDoThamGiaBL = new AttitudeBL((School_School)HttpContext.Current.Session[AppConstant.SCHOOL]);

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
        public static bool CheckExistConductName(string ConductName)
        {
            ConductName = Uri.UnescapeDataString(ConductName);
            ConductBL hanhKiemBL = new ConductBL((School_School)HttpContext.Current.Session[AppConstant.SCHOOL]);
            return hanhKiemBL.ConductNameExists(ConductName);
        }

        [WebMethod]
        public static bool CheckExistConductName(string oldConductName, string newConductName)
        {
            oldConductName = Uri.UnescapeDataString(oldConductName);
            newConductName = Uri.UnescapeDataString(newConductName);

            ConductBL hanhKiemBL = new ConductBL((School_School)HttpContext.Current.Session[AppConstant.SCHOOL]);
            return hanhKiemBL.ConductNameExists(oldConductName, newConductName);
        }

        [WebMethod]
        public static bool CheckExistLearningResultName(string LearningResultName)
        {
            LearningResultName = Uri.UnescapeDataString(LearningResultName);
            DanhHieuBL danhHieuBL = new DanhHieuBL((School_School)HttpContext.Current.Session[AppConstant.SCHOOL]);
            return danhHieuBL.DanhHieuExists(LearningResultName);
        }
    }
}