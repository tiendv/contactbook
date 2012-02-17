using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using SoLienLacTrucTuyen_WebRole.Modules;

namespace SoLienLacTrucTuyen_WebRole
{
    /// <summary>
    /// Summary description for StudentPhotoLoadingHandler
    /// </summary>
    public class StudentPhotoLoadingHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            int iStudentId = Int32.Parse(context.Request.QueryString["id"]);
            //StudentBL studentBL = new StudentBL((School_School)HttpContext.Current.Session[AppConstant.SCHOOL]);
            StudentBL studentBL = new StudentBL(new School_School());

            Student_Student student = studentBL.GetStudent(iStudentId);

            // Set the content type to the appropriate image type.
            context.Response.ContentType = "image/*";

            // Serve the image.
            if (student != null && student.Photo != null)
            {
                context.Response.BinaryWrite(student.Photo.ToArray());
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}