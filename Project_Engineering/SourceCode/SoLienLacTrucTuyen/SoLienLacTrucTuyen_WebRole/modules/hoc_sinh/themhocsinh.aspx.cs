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
                if (DdlNamHoc.Items.Count == 0 || DdlNganh.Items.Count == 0 || DdlKhoiLop.Items.Count == 0 || DdlKhoiLop.Items.Count == 0)
                {
                    Response.Redirect(AppConstant.PAGEPATH_STUDENT_LIST);
                }
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
            if (CheckUntils.IsNullOrBlank(strStudentCode))
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
            if (CheckUntils.IsNullOrBlank(strStudentName))
            {
                TenHocSinhRequired.IsValid = false;
                return;
            }

            string strDayOfBirth = this.TxtNgaySinhHocSinh.Text.Trim();
            if (CheckUntils.IsNullOrBlank(strDayOfBirth))
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

            if (CheckUntils.IsNullOrBlank(strFatherName) && CheckUntils.IsNullOrBlank(strMotherName) 
                && CheckUntils.IsNullOrBlank(strPatron))
            {
                LblErrorPhuHuynh.Style.Add(HtmlTextWriterStyle.Display, "none");
                return;
            }
            else
            {
                LblErrorPhuHuynh.Style.Add(HtmlTextWriterStyle.Display, "block");
            }

            string strFatherJob = this.TxtNgheNghiepBo.Text.Trim();
            DateTime? dtFatherDateOfBirth = ToDateTime(this.TxtNgaySinhBo.Text.Trim());
            string strMotherJob = this.TxtNgheNghiepMe.Text.Trim();
            DateTime? dtMotherDateOfBirth = ToDateTime(this.TxtNgaySinhMe.Text.Trim());
            string strPatronJob = this.TxtNgheNghiepNguoiDoDau.Text.Trim();
            DateTime? dtPatronDateOfBirth = ToDateTime(this.TxtNgaySinhNguoiDoDau.Text.Trim());

            bool bGender = this.RbtnNam.Checked;
            string[] arrDateOfBirth = strDayOfBirth.Split('/');
            DateTime dtDateOfBirth = new DateTime(Int32.Parse(arrDateOfBirth[2]),
                Int32.Parse(arrDateOfBirth[1]), Int32.Parse(arrDateOfBirth[0]));
            string strBirthPlace = this.TxtNoiSinh.Text.Trim();
            string strPhone = this.TxtDienThoai.Text.Trim();

            byte[] bPhoto = null;
            if (CheckSessionKey("Photo"))
            {
                bPhoto = (byte[])GetSession("Photo");
            }

            Class = new Class_Class();
            Class.ClassId = Int32.Parse(this.DdlLopHoc.SelectedValue);

            studentBL.InsertStudent(Class, strStudentCode, strStudentName,
                bGender, dtDateOfBirth, strBirthPlace, strAddress, strPhone, bPhoto,
                strFatherName, strFatherJob, dtFatherDateOfBirth,
                strMotherName, strMotherJob, dtMotherDateOfBirth,
                strPatron, strPatronJob, dtPatronDateOfBirth);

            // create new user parents
            string strGeneratedPassword = Membership.GeneratePassword(Membership.Provider.MinRequiredPasswordLength,
                Membership.Provider.MinRequiredNonAlphanumericCharacters);
            string strUserName = UserSchool.SchoolId.ToString() + "_PH" + strStudentCode;
            Membership.CreateUser(strUserName, strGeneratedPassword);
            UserBL userBL = new UserBL(UserSchool);
            aspnet_User userParents = new aspnet_User();
            userParents.UserName = strUserName;
            userBL.CreateUserParents(userParents, null);

            if (this.CkbAddAfterSave.Checked)
            {
                Response.Redirect(Request.Path);
            }
            else
            {
                Response.Redirect(AppConstant.PAGEPATH_STUDENT_LIST);
            }
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(AppConstant.PAGEPATH_STUDENT_LIST);
        }

        protected void BtnUpload_Click(object sender, ImageClickEventArgs e)
        {
            if (FileUploadLogo.PostedFile != null)
            {
                //To create a PostedFile
                HttpPostedFile file = FileUploadLogo.PostedFile;

                //Create byte Array with file len
                byte[] data = new Byte[file.ContentLength];

                //force the control to load data in array
                file.InputStream.Read(data, 0, file.ContentLength);

                AddSession("Photo", data);

                string filename = Path.GetFileName(FileUploadLogo.FileName);
                FileUploadLogo.SaveAs(Server.MapPath("~/upload/temp/") + filename);
                filename = "/upload/temp/" + filename;
                ImgHinhAnh.ImageUrl = filename;
            }
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
                DdlNamHoc.SelectedValue = systemConfigBL.GetLastedYear().ToString();
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

            if (DdlNamHoc.Items.Count != 0)
            {
                year = new Configuration_Year();
                year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);                
            }
            else
            {
                BtnSave.Enabled = false;
                BtnSave.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SAVE_DISABLE;
                return;
            }

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

            List<Class_Class> Classes = lopHocBL.GetClasses(LogedInUser, IsFormerTeacher, IsSubjectTeacher, year, faculty, grade, null);
            DdlLopHoc.DataSource = Classes;
            DdlLopHoc.DataValueField = "ClassId";
            DdlLopHoc.DataTextField = "ClassName";
            DdlLopHoc.DataBind();

            if (DdlLopHoc.Items.Count == 0)
            {
                BtnSave.Enabled = false;
                BtnSave.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SAVE_DISABLE;
            }
            else
            {
                BtnSave.Enabled = true;
                BtnSave.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SAVE;
            }
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