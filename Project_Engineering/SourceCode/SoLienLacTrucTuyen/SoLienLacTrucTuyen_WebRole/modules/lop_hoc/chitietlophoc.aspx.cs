using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class DetailedClassPage : BaseContentPage
    {
        #region Fields
        private ClassBL classBL;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            // Check user's accessibility
            base.Page_Load(sender, e);
            if (accessDenied)
            {
                return;
            }

            if (sessionExpired)
            {
                FormsAuthentication.SignOut();
                Response.Redirect(FormsAuthentication.LoginUrl);
            }

            classBL = new ClassBL(UserSchool);

            if (!Page.IsPostBack)
            {
                if (CheckSessionKey(AppConstant.SESSION_SELECTED_CLASS))
                {
                    Class_Class Class = (Class_Class)GetSession(AppConstant.SESSION_SELECTED_CLASS);
                    Class = classBL.GetClass((int)Class.ClassId);
                    LblClassName.Text = Class.ClassName;
                    LblFacultyName.Text = Class.Category_Faculty.FacultyName;
                    LblGradeName.Text = Class.Category_Grade.GradeName;
                    LblStudentQuantity.Text = Class.StudentQuantity.ToString();
                    Class_FormerTeacher formerTeacher = (new FormerTeacherBL(UserSchool)).GetFormerTeacher(Class);
                    if (formerTeacher != null)
                    {
                        LblFormerTeacherName.Text = formerTeacher.aspnet_User.aspnet_Membership.FullName;
                    }
                }
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void BtnBackPrevPage_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("danhsachlop.aspx");
        }
        #endregion

        private int? GetQueryString()
        {
            if (Request.QueryString["malop"] != null)
            {
                int iClassId;
                bool bParseSuccess = Int32.TryParse(Request.QueryString["malop"], out iClassId);
                if (bParseSuccess == true)
                {
                    return iClassId;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}