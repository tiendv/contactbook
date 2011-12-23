using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using System.IO;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class AddStudentPage : BaseContentPage
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

            if (sessionExpired)
            {
                FormsAuthentication.SignOut();
                Response.Redirect(FormsAuthentication.LoginUrl);
            }

            studentBL = new StudentBL(UserSchool);

            if (!Page.IsPostBack)
            {
                BindDropDownLists();
            }
        }
        #endregion

        #region DropDownList event hanlders
        protected void DdlNamHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLClasses();
        }

        protected void DdlNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLClasses();
        }

        protected void DdlKhoiLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLClasses();
        }
        #endregion

        #region Button event handlers
        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            Class_Class Class = null;            

            string strStudentCode = this.TxtMaHocSinhHienThi.Text.Trim();
            if (strStudentCode == "")
            {
                MaHocSinhRequired.IsValid = false;
                return;
            }
            else
            {
                if (studentBL.StudentCodeExists(strStudentCode))
                {
                    MaHocSinhValidator.IsValid = false;
                    return;
                }
            }

            string strStudentName = this.TxtTenHocSinh.Text.Trim();
            if (strStudentName == "")
            {
                TenHocSinhRequired.IsValid = false;
                return;
            }

            string strDayOfBirth = this.TxtNgaySinhHocSinh.Text.Trim();
            if (strDayOfBirth == "")
            {
                NgaySinhHocSinhRequired.IsValid = false;
                return;
            }

            string strAddress = this.TxtDiaChi.Text.Trim();
            if (strAddress == "")
            {
                DiaChiRequired.IsValid = false;
                return;
            }

            string strFatherName = this.TxtHoTenBo.Text.Trim();
            string strMotherName = this.TxtHoTenMe.Text.Trim();
            string strPatron = this.TxtHoTenNguoiDoDau.Text;

            if (strFatherName == "" && strMotherName == "" && strPatron == "")
            {
                LblErrorPhuHuynh.Style.Add(HtmlTextWriterStyle.Display, "none");
                return;
            }
            else
            {
                LblErrorPhuHuynh.Style.Add(HtmlTextWriterStyle.Display, "block");
            }

            string strFatherJob = this.TxtNgheNghiepBo.Text.Trim();
            DateTime? ngaySinhBo = ToDateTime(this.TxtNgaySinhBo.Text.Trim());
            string ngheNghiepMe = this.TxtNgheNghiepMe.Text.Trim();
            DateTime? ngaySinhMe = ToDateTime(this.TxtNgaySinhMe.Text.Trim());
            string ngheNghiepNguoiDoDau = this.TxtNgheNghiepNguoiDoDau.Text.Trim();
            DateTime? ngaySinhNguoiDoDau = ToDateTime(this.TxtNgaySinhNguoiDoDau.Text.Trim());

            bool gioiTinh = this.RbtnNam.Checked;
            string[] arrNgaySinh = strDayOfBirth.Split('/');
            DateTime ngaySinh = new DateTime(Int32.Parse(arrNgaySinh[2]),
                Int32.Parse(arrNgaySinh[1]), Int32.Parse(arrNgaySinh[0]));
            string noiSinh = this.TxtNoiSinh.Text.Trim();
            string dienThoai = this.TxtDienThoai.Text.Trim();

            Class = new Class_Class();
            Class.ClassId = Int32.Parse(this.DdlLopHoc.SelectedValue);

            studentBL.InsertStudent(Class, strStudentCode, strStudentName,
                gioiTinh, ngaySinh, noiSinh, strAddress, dienThoai,
                strFatherName, strFatherJob, ngaySinhBo,
                strMotherName, ngheNghiepMe, ngaySinhMe,
                strPatron, ngheNghiepNguoiDoDau, ngaySinhNguoiDoDau);

            if (this.CkbAddAfterSave.Checked)
            {
                Response.Redirect(Request.Path);
            }
            else
            {
                Response.Redirect(AppConstant.PAGEPATH_STUDENTS);
            }
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(AppConstant.PAGEPATH_STUDENTS);
        }

        protected void BtnDuyetHinhAnh_Click(object sender, ImageClickEventArgs e)
        {
            //if (FileUploadControl.HasFile)
            //{
            //    try
            //    {
            //        if (FileUploadControl.PostedFile.ContentType == "image/jpeg")
            //        {
            //            if (FileUploadControl.PostedFile.ContentLength < 102400)
            //            {
            //                string filename = Path.GetFileName(FileUploadControl.FileName);
            //            }
            //            else
            //            {
            //                //StatusLabel.Text = "Upload status: The file has to be less than 100 kb!";
            //            }
            //        }
            //        else
            //        {
            //            //StatusLabel.Text = "Upload status: Only JPEG files are accepted!";
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        //StatusLabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
            //    }
            //}
        }
        #endregion

        #region Methods
        private void BindDropDownLists()
        {
            BindDDLYears();
            BindDDLFaculties();
            BindDDLGrades();
            BindDDLClasses();
        }

        private void BindDDLYears()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<Configuration_Year> years = systemConfigBL.GetListYears();
            DdlNamHoc.DataSource = years;
            DdlNamHoc.DataValueField = "YearId";
            DdlNamHoc.DataTextField = "YearName";
            DdlNamHoc.DataBind();

            if (DdlNamHoc.Items.Count != 0)
            {
                SystemConfigBL cauHinhBL = new SystemConfigBL(UserSchool);
                DdlNamHoc.SelectedValue = cauHinhBL.GetLastedYear().ToString();
            }
        }

        private void BindDDLGrades()
        {
            GradeBL gradeBL = new GradeBL(UserSchool);
            List<Category_Grade> grades = gradeBL.GetListGrades();
            DdlKhoiLop.DataSource = grades;
            DdlKhoiLop.DataValueField = "GradeId";
            DdlKhoiLop.DataTextField = "GradeName";
            DdlKhoiLop.DataBind();
        }

        private void BindDDLFaculties()
        {
            FacultyBL facultyBL = new FacultyBL(UserSchool);
            List<Category_Faculty> faculties = facultyBL.GetFaculties();
            DdlNganh.DataSource = faculties;
            DdlNganh.DataValueField = "FacultyId";
            DdlNganh.DataTextField = "FacultyName";
            DdlNganh.DataBind();
        }

        private void BindDDLClasses()
        {
            Configuration_Year year = null;
            Category_Faculty faculty = null;
            Category_Grade grade = null;
            ClassBL lopHocBL = new ClassBL(UserSchool);

            year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);

            try
            {
                if (DdlNganh.SelectedIndex >= 0)
                {
                    faculty = new Category_Faculty();
                    faculty.FacultyId = Int32.Parse(DdlNganh.SelectedValue);
                }
            }
            catch (Exception) { }

            try
            {
                if (DdlKhoiLop.SelectedIndex >= 0)
                {
                    grade = new Category_Grade();
                    grade.GradeId = Int32.Parse(DdlKhoiLop.SelectedValue);
                }
            }
            catch (Exception) { }

            List<Class_Class> lstLop = lopHocBL.GetListClasses(year, faculty, grade);
            DdlLopHoc.DataSource = lstLop;
            DdlLopHoc.DataValueField = "ClassId";
            DdlLopHoc.DataTextField = "ClassName";
            DdlLopHoc.DataBind();

        }

        private DateTime? ToDateTime(string str)
        {
            if (str != "")
            {
                string[] arrDateTime = str.Split('/');
                return new DateTime(Int32.Parse(arrDateTime[2]),
                    Int32.Parse(arrDateTime[1]), Int32.Parse(arrDateTime[0]));
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}