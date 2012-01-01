using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public abstract class BaseContentPage : System.Web.UI.Page
    {
        #region Properties
        public School_School UserSchool
        {
            get
            {
                School_School school = null;
                if (Session[AppConstant.SCHOOL] != null)
                {
                    school = (School_School)Session[AppConstant.SCHOOL];
                }

                return school;
            }

            set
            {
                Session[AppConstant.SCHOOL] = value;
            }
        }
        #endregion

        #region Fields
        protected List<AccessibilityEnum> accessibilities;
        protected bool accessDenied = false;
        protected bool sessionExpired = false;
        #endregion

        #region Page event handlers
        protected virtual void Page_Load(object sender, EventArgs e)
        {
            if(Session[AppConstant.SCHOOL] == null)
            {
                sessionExpired = true;
                return;
            }

            UserSchool = (School_School)Session[AppConstant.SCHOOL];
            UserBL userBL = new UserBL(UserSchool);
            AuthorizationBL authorizationBL = new AuthorizationBL(UserSchool);

            string pageUrl = Page.Request.Path;
            List<aspnet_Role> roles = userBL.GetRoles(User.Identity.Name);

            if (!authorizationBL.ValidateAuthorization(roles, pageUrl))
            {
                Response.Redirect(GetText("AccessDeniedPageUrl"));
                accessDenied = false;
                return;
            }

            Site masterPage = (Site)Page.Master;
            masterPage.UserName = User.Identity.Name;
            masterPage.UserRole = roles[0];
            masterPage.PageUrl = pageUrl;
            masterPage.UserSchool = UserSchool;

            accessibilities = authorizationBL.GetAccessibilities(roles, pageUrl);
        }
        #endregion

        #region Session methods
        protected void AddSession(string key, object value)
        {
            Session.Add(UserSchool.SchoolId + AppConstant.UNDERSCORE + key, value);
        }

        protected void RemoveSession(string key)
        {
            if (CheckSessionKey(key))
            {
                Session.Remove(UserSchool.SchoolId + AppConstant.UNDERSCORE + key);
            }
        }

        protected object GetSession(string key)
        {
            if (CheckSessionKey(key))
            {
                return Session[UserSchool.SchoolId + AppConstant.UNDERSCORE + key];
            }
            else
            {
                return null;
            }
        }

        protected bool CheckSessionKey(string key)
        {
            if (Session[UserSchool.SchoolId + AppConstant.UNDERSCORE + key] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        protected string GetText(string key)
        {
            return (string)GetGlobalResourceObject("MainResource", key);
        }

        //protected void ProcPermissions()
        //{

        //    if (accessibilities.Contains(AccessibilityEnum.Add))
        //    {
        //        Control ControlAdd = Page.FindControl("BtnAdd");
        //        if (ControlAdd != null)
        //        {
        //            ImageButton BtnAdd = (ImageButton)ControlAdd;
        //            BtnAdd.Enabled = true;
        //            BtnAdd.ImageUrl = "~/Styles/Images/button_add_with_text.png";
        //            PnlPopupAdd.Visible = true;
        //        }
        //    }
        //    else
        //    {
        //        BtnAdd.Visible = false;
        //        PnlPopupAdd.Visible = false;
        //    }
        //}
    }
}