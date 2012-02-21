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
    public class UserPhotoLoadingHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            Guid userId = new Guid(context.Request.QueryString["id"]);
            UserBL userBL = new UserBL(new School_School());

            aspnet_User user = userBL.GetUser(userId);

            // Set the content type to the appropriate image type.
            context.Response.ContentType = "image/*";

            // Serve the image.
            if (user != null && user.aspnet_Membership != null && user.aspnet_Membership.Photo != null)
            {
                context.Response.BinaryWrite(user.aspnet_Membership.Photo.ToArray());
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