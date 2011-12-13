using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;
using AjaxControlToolkit;
using System.Web.UI.HtmlControls;
using SoLienLacTrucTuyen_WebRole.Modules;

namespace SoLienLacTrucTuyen_WebRole.ModuleParents
{
    public partial class StudentPersonalPage : BaseContentPage
    {
        #region Fields
        private StudentBL studentBL;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (accessDenied)
            {
                return;
            }

            studentBL = new StudentBL(UserSchool);

            if (!Page.IsPostBack)
            {
                if (LoggedInStudent != null)
                {
                    BindDDLYears(LoggedInStudent);
                    FillPersonalInformation(LoggedInStudent);
                }
                else
                {
                    Response.Redirect(AppConstant.PAGEPATH_HOMEPAGE);
                }
            }
        }
        #endregion

        #region Methods
        private void BindDDLYears(Student_Student student)
        {
            List<Configuration_Year> years = studentBL.GetYears(student);
            DdlNamHoc.DataSource = years;
            DdlNamHoc.DataValueField = "YearId";
            DdlNamHoc.DataTextField = "YearName";
            DdlNamHoc.DataBind();
        }

        private void FillLopHoc()
        {   
            Configuration_Year year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);

            TabularClass tabularClass = studentBL.GetTabularClass(year, LoggedInStudent);
            LblLopHoc.Text = tabularClass.ClassName;
        }

        private void FillPersonalInformation(Student_Student student)
        {
            student = studentBL.GetStudent(student.StudentId);

            this.LblMaHocSinhHienThi.Text = student.StudentCode;
            this.LblHoTenHocSinh.Text = student.FullName;
            this.LblGioiTinh.Text = (student.Gender == true) ? "Nam" : "Nữ";
            this.LblNgaySinhHocSinh.Text = student.StudentBirthday.Day.ToString()
                + "/" + student.StudentBirthday.Month.ToString()
                + "/" + student.StudentBirthday.Year.ToString();
            this.LblNoiSinh.Text = student.Birthplace;
            this.LblDiaChi.Text = student.Address;
            this.LblDienThoai.Text = student.ContactPhone;
            this.LblHoTenBo.Text = student.FatherName;
            if (student.FatherBirthday != null)
            {
                DateTime ngaySinhBo = (DateTime)student.FatherBirthday;
                this.LblNgaySinhBo.Text = ngaySinhBo.Day.ToString() + "/" + ngaySinhBo.Month.ToString()
                    + "/" + ngaySinhBo.Year.ToString();
            }
            this.LblNgheNghiepBo.Text = student.FatherJob;
            this.LblHoTenMe.Text = student.MotherName;
            if (student.MotherBirthday != null)
            {
                DateTime ngaySinhMe = (DateTime)student.MotherBirthday;
                this.LblNgaySinhMe.Text = ngaySinhMe.Day.ToString() + "/" + ngaySinhMe.Month.ToString()
                    + "/" + ngaySinhMe.Year.ToString();
            }
            this.LblNgheNghiepMe.Text = student.MotherJob;
            if (student.PatronBirthday != null)
            {
                DateTime ngaySinhNguoiDoDau = (DateTime)student.PatronBirthday;
                this.LblNgaySinhNguoiDoDau.Text = ngaySinhNguoiDoDau.Day.ToString() + "/" + ngaySinhNguoiDoDau.Month.ToString()
                    + "/" + ngaySinhNguoiDoDau.Year.ToString();
            }
            this.LblNgheNghiepNguoiDoDau.Text = student.PatronJob;

            FillLopHoc();
        }
        #endregion

        #region Dropdownlist event handlers
        protected void DdlNamHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillLopHoc();
        }
        #endregion
    }
}