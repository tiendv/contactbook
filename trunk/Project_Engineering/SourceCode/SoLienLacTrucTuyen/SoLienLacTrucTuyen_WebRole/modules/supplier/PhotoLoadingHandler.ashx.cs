using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace SoLienLacTrucTuyen_WebRole
{
    /// <summary>
    /// Summary description for PhotoLoadingHandler
    /// </summary>
    public class PhotoLoadingHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            int iSchoolId = Int32.Parse(context.Request.QueryString["id"]);
            SchoolBL schoolBL = new SchoolBL();
            School_School school = schoolBL.GetSchool(iSchoolId);

            // Set the content type to the appropriate image type.
            context.Response.ContentType = "image/*";

            // Serve the image.
            context.Response.BinaryWrite(school.Logo.ToArray());
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