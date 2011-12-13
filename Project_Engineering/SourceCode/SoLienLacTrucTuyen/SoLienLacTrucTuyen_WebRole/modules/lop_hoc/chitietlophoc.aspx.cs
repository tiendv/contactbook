using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class DetailedClassPage : BaseContentPage
    {
        #region Fields
        private ClassBL lopHocBL;
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

            lopHocBL = new ClassBL(UserSchool);           

            if (!Page.IsPostBack)
            {
                int? ClassId = GetQueryString();
                if (ClassId != null)
                {
                    Class_Class lophoc = lopHocBL.GetClass((int)ClassId);
                    if (lophoc != null)
                    {                        
                        LblClassNameChiTiet.Text = lophoc.ClassName;
                        LblFacultyNameChiTiet.Text = lophoc.Category_Faculty.FacultyName;
                        LblGradeNameChiTiet.Text = lophoc.Category_Grade.GradeName;
                        LblSiSoChiTiet.Text = lophoc.StudentQuantity.ToString();
                        Class_FormerTeacher formerTeacher = (new FormerTeacherBL(UserSchool)).GetFormerTeacher(lophoc);
                        if(formerTeacher != null)
                        {
                            LblTenGVCNChiTiet.Text = formerTeacher.aspnet_User.aspnet_Membership.FullName;
                        }
                        
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