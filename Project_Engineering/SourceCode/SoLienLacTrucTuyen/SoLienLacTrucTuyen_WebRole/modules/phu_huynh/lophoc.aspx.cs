using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using SoLienLacTrucTuyen_WebRole.Modules;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.ModuleParents
{
    public partial class DetailedClassPage : BaseContentPage
    {
        #region Fields
        private StudentBL studentBL;
        private ClassBL classBL;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
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

            studentBL = new StudentBL(UserSchool);
            classBL = new ClassBL(UserSchool);

            if (!Page.IsPostBack)
            {
                BindDDLYears();
                FillClass();
            }
        }
        #endregion

        #region DropDownList event hanlders
        protected void DdlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillClass();
        }
        #endregion

        #region Methods
        private void BindDDLYears()
        {
            List<Configuration_Year> years = studentBL.GetYears(LoggedInStudent);
            DdlYear.DataSource = years;
            DdlYear.DataValueField = "YearId";
            DdlYear.DataTextField = "YearName";
            DdlYear.DataBind();
        }

        private void FillClass()
        {
            StudentBL studentBL = new StudentBL(UserSchool);
            FormerTeacherBL formerTeacherBL = new FormerTeacherBL(UserSchool);
            Configuration_Year year = null;
            if (DdlYear.Items.Count == 0)
            {
                return;
            }

            year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlYear.SelectedValue);
            Class_Class Class = studentBL.GetClass(LoggedInStudent, year);

            LblClassName.Text = Class.ClassName;
            LblFacultyName.Text = Class.Category_Faculty.FacultyName;
            LblGradeName.Text = Class.Category_Grade.GradeName;
            LblQuantity.Text = Class.StudentQuantity.ToString();
            Class_FormerTeacher formerTeacher = formerTeacherBL.GetFormerTeacher(Class);
            if (formerTeacher != null)
            {
                aspnet_Membership teacher = formerTeacher.aspnet_User.aspnet_Membership;
                LblFormerTeacherName.Text = teacher.FullName;
                LblFormerTeacherCode.Text = teacher.aspnet_User.UserName.Split(AppConstant.UNDERSCORE_CHAR)[1];
                if (teacher.Gender != null)
                {
                    LblFormerTeacherGender.Text = (teacher.Gender == true) ? "Nam" : "Nữ";
                }
                else
                {
                    LblFormerTeacherGender.Text = "(Chưa xác định)";
                }

                if (!CheckUntils.IsNullOrBlank(teacher.Phone))
                {
                    LblFormerTeacherPhone.Text = teacher.Phone;
                }
                else
                {
                    LblFormerTeacherPhone.Text = "(Chưa xác định)";
                }

                if (!CheckUntils.IsNullOrBlank(teacher.Address))
                {
                    LblFormerTeacherAddress.Text = teacher.Address;
                }
                else
                {
                    LblFormerTeacherAddress.Text = "(Chưa xác định)";
                }

                if (teacher.Birthday != null)
                {
                    LblFormerTeacherBirthday.Text = ((DateTime)teacher.Birthday).ToShortDateString();
                }
                else
                {
                    LblFormerTeacherBirthday.Text = "(Chưa xác định)";
                }

                LblFormerTeacher.Text = "";
            }
            else
            {
                LblFormerTeacher.Text = "(Không có)";

                LblFormerTeacherName.Text = "";
                LblFormerTeacherCode.Text = "";
                LblFormerTeacherGender.Text = "";
                LblFormerTeacherPhone.Text = "";                
                LblFormerTeacherAddress.Text = "";                
                LblFormerTeacherBirthday.Text = "";
            }
        }
        #endregion
    }
}