using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;

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
            // Check user's accessibility
            base.Page_Load(sender, e);
            if (accessDenied)
            {
                return;
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
            if(DdlYear.Items.Count == 0)
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
                LblFormerTeacherName.Text = formerTeacher.aspnet_User.aspnet_Membership.FullName;
            }
            else
            {
                LblFormerTeacherName.Text = "(Không có)";
            }
        }
        #endregion
    }
}