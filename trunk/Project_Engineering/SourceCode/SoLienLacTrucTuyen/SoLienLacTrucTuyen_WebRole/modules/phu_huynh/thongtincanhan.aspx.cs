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

        private void FillClass()
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
            this.LblNgaySinhHocSinh.Text = student.StudentBirthday.ToShortDateString();
            if (student.Birthplace != "")
            {
                this.LblNoiSinh.Text = student.Birthplace;
            }
            else
            {
                this.LblNoiSinh.Text = "(Chưa xác định)";
            }
            this.LblDiaChi.Text = student.Address;
            if (student.ContactPhone != "")
            {
                LblDiaChi.Text = student.ContactPhone;
            }
            else
            {
                LblDienThoai.Text = "(Chưa xác định)";
            }
            if (student.FatherName != "")
            {
                this.LblHoTenBo.Text = student.FatherName;
            }
            else
            {
                this.LblHoTenBo.Text = "(Chưa xác định)";
            }

            if (student.FatherBirthday != null)
            {
                this.LblNgaySinhBo.Text = ((DateTime)student.FatherBirthday).ToShortDateString();
            }
            else
            {
                this.LblNgaySinhBo.Text = "(Chưa xác định)";
            }
            if(student.FatherJob != "")
            {
                this.LblNgheNghiepBo.Text = student.FatherJob;
            }
            else
            {
                this.LblNgheNghiepBo.Text = "(Chưa xác định)";
            }

            if (student.MotherName != "")
            {
                this.LblHoTenMe.Text = student.MotherName;
            }
            else
            {
                this.LblHoTenMe.Text = "(Chưa xác định)";
            }
            if (student.MotherBirthday != null)
            {
                this.LblNgaySinhMe.Text = ((DateTime)student.MotherBirthday).ToShortDateString();
            }
            else
            {
                this.LblNgaySinhMe.Text = "(Chưa xác định)";
            }
            if (student.MotherJob != "")
            {
                this.LblNgheNghiepMe.Text = student.MotherJob;
            }
            else
            {
                this.LblNgheNghiepMe.Text = "(Chưa xác định)";
            }
            if (student.PatronName != "")
            {
                LblHoTenNguoiDoDau.Text = student.PatronName;
            }
            else
            {
                LblHoTenNguoiDoDau.Text = "(Chưa xác định)";    
            }
            if (student.PatronBirthday != null)
            {
                this.LblNgaySinhNguoiDoDau.Text = ((DateTime)student.PatronBirthday).ToShortDateString();
            }
            else
            {
                this.LblNgaySinhNguoiDoDau.Text = "(Chưa xác định)";
            }
            if (student.PatronJob != "")
            {
                this.LblNgheNghiepNguoiDoDau.Text = student.PatronJob;
            }
            else
            {
                this.LblNgheNghiepNguoiDoDau.Text = "(Chưa xác định)";
            }            

            FillClass();
        }
        #endregion

        #region Dropdownlist event handlers
        protected void DdlNamHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillClass();
        }
        #endregion
    }
}