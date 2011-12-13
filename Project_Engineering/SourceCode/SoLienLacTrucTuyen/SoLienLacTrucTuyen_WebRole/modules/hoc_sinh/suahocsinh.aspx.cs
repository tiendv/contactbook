using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using System.IO;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class EditStudentPage : BaseContentPage
    {
        #region Fields
        private StudentBL studentBL;
        private Configuration_Year currentYear;
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
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            currentYear = systemConfigBL.GetCurrentYear();

            if (!Page.IsPostBack)
            {
                BindDropDownLists();

                if (CheckSessionKey(AppConstant.SESSION_STUDENT))
                {
                    Student_Student student = (Student_Student)GetSession(AppConstant.SESSION_STUDENT);
                    RemoveSession(AppConstant.SESSION_STUDENT);
                    ViewState[AppConstant.VIEWSTATE_STUDENTID] = student.StudentId;

                    Class_Class studentClass = (Class_Class)GetSession(AppConstant.SESSION_STUDENTCLASS);
                    RemoveSession(AppConstant.SESSION_STUDENTCLASS);
                    ViewState[AppConstant.VIEWSTATE_STUDENTCLASS_ID] = studentClass.ClassId;

                    Configuration_Year year = (Configuration_Year)GetSession(AppConstant.SESSION_SELECTED_YEAR);
                    RemoveSession(AppConstant.SESSION_SELECTED_YEAR);
                    ViewState[AppConstant.VIEWSTATE_STUDENTID] = student.StudentId;

                    Category_Faculty faculty = (Category_Faculty)GetSession(AppConstant.SESSION_SELECTED_FACULTY);
                    RemoveSession(AppConstant.SESSION_SELECTED_FACULTY);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTY] = faculty.FacultyId;

                    Category_Grade grade = (Category_Grade)GetSession(AppConstant.SESSION_SELECTED_GRADE);
                    RemoveSession(AppConstant.SESSION_SELECTED_GRADE);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_GRADE] = grade.GradeId;

                    Class_Class Class = (Class_Class)GetSession(AppConstant.SESSION_SELECTED_CLASS);
                    RemoveSession(AppConstant.SESSION_SELECTED_CLASS);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS] = Class.ClassId;

                    String strStudentName = (string)GetSession(AppConstant.SESSION_SELECTED_STUDENTNAME);
                    RemoveSession(AppConstant.SESSION_SELECTED_STUDENTNAME);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTNAME] = strStudentName;

                    String strStudentCode = (string)GetSession(AppConstant.SESSION_SELECTED_STUDENTCODE);
                    RemoveSession(AppConstant.SESSION_SELECTED_STUDENTCODE);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTCODE] = strStudentCode;

                    string strPrevPage = (string)GetSession(AppConstant.SESSION_PREV_PAGE);
                    RemoveSession(AppConstant.SESSION_PREV_PAGE);
                    ViewState[AppConstant.VIEWSTATE_PREV_PAGE] = strPrevPage;

                    ViewState[AppConstant.VIEWSTATE_STUDENTID] = student.StudentId;

                    FillStudentPersonalInformation(student);
                }                
            }
        }

        private void FillStudentPersonalInformation(Student_Student student)
        {
            student = studentBL.GetStudent(student.StudentId);
            TxtMaHocSinhHienThi.Text = student.StudentCode;
            HdfOldStudentCode.Value = student.StudentCode;
            TxtTenHocSinh.Text = student.FullName;
            TxtNgaySinhHocSinh.Text = student.StudentBirthday.ToShortDateString();
            RbtnNam.Checked = student.Gender;
            RbtnNu.Checked = !student.Gender;
            TxtNoiSinh.Text = student.Birthplace;
            TxtDiaChi.Text = student.Address;
            TxtDienThoai.Text = student.ContactPhone;
            TxtHoTenBo.Text = student.FatherName;
            if (student.FatherBirthday != null)
            {
                TxtNgaySinhBo.Text = ((DateTime)student.FatherBirthday).ToShortDateString();
            }
            TxtNgheNghiepBo.Text = student.FatherJob;
            TxtHoTenMe.Text = student.MotherName;
            if (student.MotherBirthday != null)
            {
                TxtNgaySinhMe.Text = ((DateTime)student.MotherBirthday).ToShortDateString();
            }
            TxtNgheNghiepMe.Text = student.MotherJob;
            TxtHoTenNguoiDoDau.Text = student.PatronName;
            if (student.PatronBirthday != null)
            {
                TxtNgaySinhNguoiDoDau.Text = ((DateTime)student.PatronBirthday).ToShortDateString();
            }
            TxtNgheNghiepNguoiDoDau.Text = student.PatronJob;

            ClassBL lopHocBL = new ClassBL(UserSchool);
            Class_Class lopHoc = studentBL.GetLastedClass(student);
            LblYear.Text = lopHoc.Configuration_Year.YearName;
            DdlNganh.SelectedValue = lopHoc.FacultyId.ToString();
            DdlKhoiLop.SelectedValue = lopHoc.GradeId.ToString();
            DdlLopHoc.SelectedValue = lopHoc.ClassId.ToString();
            BindDDLClasses();
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
            Student_Student editedStudent = null;
            Class_Class newClass = null;            

            string strNewStudentCode = this.TxtMaHocSinhHienThi.Text.Trim();
            string strNewStudentName = this.TxtTenHocSinh.Text.Trim(); 
            string strBirthday = this.TxtNgaySinhHocSinh.Text.Trim();
            string strNewAddress = this.TxtDiaChi.Text.Trim();
            string strNewFatherName = this.TxtHoTenBo.Text.Trim();
            string strNewMotherName = this.TxtHoTenMe.Text.Trim();
            string tenNguoiDoDau = this.TxtHoTenNguoiDoDau.Text;

            if (strNewStudentCode.Trim() == "")
            {
                MaHocSinhRequired.IsValid = false;
                return;
            }
            else
            {
                if (studentBL.StudentCodeExists(HdfOldStudentCode.Value, strNewStudentCode))
                {
                    MaHocSinhValidator.IsValid = false;
                    return;
                }
            }            
            if (strNewStudentName.Trim() == "")
            {
                TenHocSinhRequired.IsValid = false;
                return;
            }            
            if (strBirthday == "")
            {
                NgaySinhHocSinhRequired.IsValid = false;
                return;
            }            
            if (strNewAddress.Trim() == "")
            {
                DiaChiRequired.IsValid = false;
                return;
            }
            if (strNewFatherName == "" && strNewMotherName == "" && tenNguoiDoDau == "")
            {
                LblErrorPhuHuynh.Style.Add(HtmlTextWriterStyle.Display, AppConstant.CSSSTYLE_DISPLAY_NONE);
                return;
            }
            else
            {
                LblErrorPhuHuynh.Style.Add(HtmlTextWriterStyle.Display, AppConstant.CSSSTYLE_DISPLAY_BLOCK);
            }

            string strNewFatherJob = this.TxtNgheNghiepBo.Text.Trim();
            DateTime? dtNewFatherBirthday = ToDateTime(this.TxtNgaySinhBo.Text.Trim());
            string strNewMotherJob = this.TxtNgheNghiepMe.Text.Trim();
            DateTime? dtNewMotherBirthday = ToDateTime(this.TxtNgaySinhMe.Text.Trim());
            string strNewPatronJob = this.TxtNgheNghiepNguoiDoDau.Text.Trim();
            DateTime? dtNewPatronBirthday = ToDateTime(this.TxtNgaySinhNguoiDoDau.Text.Trim());

            bool bNewStudentGender = this.RbtnNam.Checked;
            DateTime dtBirthday = DateTime.Parse(strBirthday);
            string strNewBirthPlace = this.TxtNoiSinh.Text.Trim();
            string strNewPhone = this.TxtDienThoai.Text.Trim();

            editedStudent = new Student_Student();
            editedStudent.StudentId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_STUDENTID].ToString());
            newClass = new Class_Class();
            newClass.ClassId = Int32.Parse(this.DdlLopHoc.SelectedValue);

            studentBL.UpdateHocSinh(editedStudent, newClass, strNewStudentCode, strNewStudentName, bNewStudentGender, dtBirthday,
                strNewBirthPlace, strNewAddress, strNewPhone, strNewFatherName, strNewFatherJob, dtNewFatherBirthday,
                strNewMotherName, strNewMotherJob, dtNewMotherBirthday, tenNguoiDoDau, strNewPatronJob, dtNewPatronBirthday);

            RedirectPage();
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            RedirectPage();
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
            BindDDLFaculties();
            BindDDLGrades();
            BindDDLClasses();
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
    
        private void BindDDLGrades()
        {
            GradeBL gradeBL = new GradeBL(UserSchool);
            List<Category_Grade> grades = gradeBL.GetListGrades();
            DdlKhoiLop.DataSource = grades;
            DdlKhoiLop.DataValueField = "GradeId";
            DdlKhoiLop.DataTextField = "GradeName";
            DdlKhoiLop.DataBind();
        }

        private void BindDDLClasses()
        {
            // declare variables
            Category_Faculty faculty = null;
            Category_Grade grade = null;
            ClassBL classBL = new ClassBL(UserSchool);           

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

            List<Class_Class> classes = classBL.GetListClasses(currentYear, faculty, grade);
            DdlLopHoc.DataSource = classes;
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

        private void RedirectPage()
        {
            Configuration_Year year = new Configuration_Year();
            year.YearId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_STUDENTID].ToString());
            AddSession(AppConstant.SESSION_SELECTED_YEAR, year);

            Category_Faculty faculty = new Category_Faculty();
            faculty.FacultyId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTY].ToString());
            AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);

            Category_Grade grade = new Category_Grade();
            grade.GradeId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_GRADE].ToString());
            AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);

            Class_Class Class = new Class_Class();
            Class.ClassId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS].ToString());
            AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);

            String strStudentName = ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTNAME].ToString();
            AddSession(AppConstant.SESSION_SELECTED_STUDENTNAME, strStudentName);

            String strStudentCode = ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTCODE].ToString();
            AddSession(AppConstant.SESSION_SELECTED_STUDENTCODE, strStudentCode);

            string strPrevPage = (string)ViewState[AppConstant.VIEWSTATE_PREV_PAGE];

            if (strPrevPage == AppConstant.PAGEPATH_STUDENTINFOR)
            {
                Student_Student student = new Student_Student();
                student.StudentId = (int)ViewState[AppConstant.VIEWSTATE_STUDENTID];
                AddSession(AppConstant.SESSION_STUDENT, student);

                Class_Class studentClass = new Class_Class();
                studentClass.ClassId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_STUDENTCLASS_ID].ToString());
                AddSession(AppConstant.SESSION_STUDENTCLASS, studentClass);

                Response.Redirect(AppConstant.PAGEPATH_STUDENTINFOR);
            }
            else
            {
                Response.Redirect(AppConstant.PAGEPATH_STUDENTS);
            }
        }
        #endregion
    }
}