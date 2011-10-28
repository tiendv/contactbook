using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SoLienLacTrucTuyen.BusinessEntity;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;
using AjaxControlToolkit;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole
{
    public partial class NguoiDung : System.Web.UI.Page
    {
        #region Fields
        private UserBL userBL;
        private RoleBL nhomNguoiDungBL;
        private bool isSearch;
        private List<AccessibilityEnum> lstFunctionType;
        #endregion        

        #region Page event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            userBL = new UserBL();
            nhomNguoiDungBL = new RoleBL();
            
            string pageUrl = Page.Request.Path;            
            Guid role = userBL.GetRoleId(User.Identity.Name);

            if (!nhomNguoiDungBL.ValidateAuthorization(role, pageUrl))
            {
                Response.Redirect("/Modules/ErrorPage/AccessDenied.aspx");
                return;
            }

            Site masterPage = (Site)Page.Master;
            masterPage.UserRole = role;
            masterPage.PageUrl = pageUrl;
        }        
        #endregion      
    }
}