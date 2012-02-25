using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;
using AjaxControlToolkit;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.Text;

namespace SoLienLacTrucTuyen_WebRole.Modules
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

            if (sessionExpired)
            {
                FormsAuthentication.SignOut();
                Response.Redirect(FormsAuthentication.LoginUrl);
            }

            studentBL = new StudentBL(UserSchool);

            if (!Page.IsPostBack)
            {
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
                    ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID] = year.YearId;

                    Category_Faculty faculty = (Category_Faculty)GetSession(AppConstant.SESSION_SELECTED_FACULTY);
                    RemoveSession(AppConstant.SESSION_SELECTED_FACULTY);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTYID] = faculty.FacultyId;

                    Category_Grade grade = (Category_Grade)GetSession(AppConstant.SESSION_SELECTED_GRADE);
                    RemoveSession(AppConstant.SESSION_SELECTED_GRADE);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_GRADEID] = grade.GradeId;

                    Class_Class Class = (Class_Class)GetSession(AppConstant.SESSION_SELECTED_CLASS);
                    RemoveSession(AppConstant.SESSION_SELECTED_CLASS);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID] = Class.ClassId;

                    String strStudentName = (string)GetSession(AppConstant.SESSION_SELECTED_STUDENTNAME);
                    RemoveSession(AppConstant.SESSION_SELECTED_STUDENTNAME);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTNAME] = strStudentName;

                    String strStudentCode = (string)GetSession(AppConstant.SESSION_SELECTED_STUDENTCODE);
                    RemoveSession(AppConstant.SESSION_SELECTED_STUDENTCODE);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTCODE] = strStudentCode;

                    ViewState[AppConstant.VIEWSTATE_STUDENTID] = student.StudentId;

                    BindDDLYears(student);
                    FillPersonalInformation(student);

                    AuthorizationBL authorizationBL = new AuthorizationBL(UserSchool);
                    List<UserManagement_PagePath> pagePages = authorizationBL.GetStudentPages(
                        (new UserBL(UserSchool)).GetRoles(User.Identity.Name));
                    RptStudentFunctions.DataSource = pagePages;
                    RptStudentFunctions.DataBind();
                }
                else
                {
                    Response.Redirect(AppConstant.PAGEPATH_STUDENT_LIST);
                }
            }

            ProcPermissions();
        }
        #endregion

        #region Methods
        private void ProcPermissions()
        {
            BtnEdit.Visible = accessibilities.Contains(AccessibilityEnum.Modify);
        }

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
            Student_Student student = new Student_Student();
            student.StudentId = (int)ViewState[AppConstant.VIEWSTATE_STUDENTID];
            Configuration_Year year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);

            TabularClass tabularClass = studentBL.GetTabularClass(year, student);
            LblLopHoc.Text = tabularClass.ClassName;
        }

        private void FillPersonalInformation(Student_Student student)
        {
            student = studentBL.GetStudent(student.StudentId);
            LblStudentName.Text = student.FullName;
            LblStudentCode.Text = student.StudentCode;
            this.LblMaHocSinhHienThi.Text = student.StudentCode;
            this.LblHoTenHocSinh.Text = student.FullName;
            this.LblGioiTinh.Text = (student.Gender == true) ? "Nam" : "Nữ";
            this.LblNgaySinhHocSinh.Text = student.StudentBirthday.ToString(AppConstant.DATEFORMAT_DDMMYYYY);
            this.LblNoiSinh.Text = student.Birthplace;
            this.LblDiaChi.Text = student.Address;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("~/modules/hoc_sinh/StudentPhotoLoadingHandler.ashx");
            stringBuilder.Append("?id=");
            stringBuilder.Append(student.StudentId);
            ImgPhoto.ImageUrl = stringBuilder.ToString();
            this.LblDienThoai.Text = student.ContactPhone;

            if(CheckUntils.IsNullOrBlank(student.FatherName) == false)
            {
                this.LblHoTenBo.Text = student.FatherName;
            }
            else
            {
                this.LblHoTenBo.Text = AppConstant.STRING_UNDEFINED;
            }

            if (student.FatherBirthday != null)
            {
                this.LblNgaySinhBo.Text = ((DateTime)student.FatherBirthday).ToString(AppConstant.DATEFORMAT_DDMMYYYY);
            }
            else
            {
                this.LblNgaySinhBo.Text = AppConstant.STRING_UNDEFINED;
            }

            if (CheckUntils.IsNullOrBlank(student.FatherJob) == false)
            {
                this.LblNgheNghiepBo.Text = student.FatherJob;
            }
            else
            {
                this.LblNgheNghiepBo.Text = AppConstant.STRING_UNDEFINED;
            }

            if (CheckUntils.IsNullOrBlank(student.MotherName) == false)
            {
                this.LblHoTenMe.Text = student.MotherName;
            }
            else
            {
                this.LblHoTenMe.Text = AppConstant.STRING_UNDEFINED;
            }

            if (student.MotherBirthday != null)
            {
                this.LblNgaySinhMe.Text = ((DateTime)student.MotherBirthday).ToString(AppConstant.DATEFORMAT_DDMMYYYY);
            }
            else
            {
                this.LblNgaySinhMe.Text = AppConstant.STRING_UNDEFINED;
            }

            if (CheckUntils.IsNullOrBlank(student.MotherJob) == false)
            {
                this.LblNgheNghiepMe.Text = student.MotherJob;
            }
            else
            {
                this.LblNgheNghiepMe.Text = AppConstant.STRING_UNDEFINED;
            }

            if (student.PatronBirthday != null)
            {
                DateTime ngaySinhNguoiDoDau = (DateTime)student.PatronBirthday;
                this.LblNgaySinhNguoiDoDau.Text = ngaySinhNguoiDoDau.ToString(AppConstant.DATEFORMAT_DDMMYYYY);
            }
            else
            {
                this.LblNgaySinhNguoiDoDau.Text = AppConstant.STRING_UNDEFINED;
            }

            if (CheckUntils.IsNullOrBlank(student.PatronJob) == false)
            {
                this.LblNgheNghiepNguoiDoDau.Text = student.PatronJob;
            }
            else
            {
                this.LblNgheNghiepNguoiDoDau.Text =  AppConstant.STRING_UNDEFINED;
            }

            if (CheckUntils.IsNullOrBlank(student.PatronName) == false)
            {
                LblHoTenNguoiDoDau.Text = student.PatronName;
            }
            else
            {
                LblHoTenNguoiDoDau.Text = AppConstant.STRING_UNDEFINED;
            }

            FillLopHoc();
        }
        #endregion

        #region Button event handlers
        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            Student_Student student = new Student_Student();
            student.StudentId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_STUDENTID].ToString());
            AddSession(AppConstant.SESSION_STUDENT, student);

            Configuration_Year year = new Configuration_Year();
            year.YearId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_STUDENTID].ToString());
            AddSession(AppConstant.SESSION_SELECTED_YEAR, year);

            Category_Faculty faculty = new Category_Faculty();
            faculty.FacultyId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTYID].ToString());
            AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);

            Category_Grade grade = new Category_Grade();
            grade.GradeId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_GRADEID].ToString());
            AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);

            Class_Class Class = new Class_Class();
            Class.ClassId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID].ToString());
            AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);

            String strStudentName = ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTNAME].ToString();
            AddSession(AppConstant.SESSION_SELECTED_STUDENTNAME, strStudentName);

            String strStudentCode = ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTCODE].ToString();
            AddSession(AppConstant.SESSION_SELECTED_STUDENTCODE, strStudentCode);

            Class_Class studentClass = new Class_Class();
            studentClass.ClassId = (int)ViewState[AppConstant.VIEWSTATE_STUDENTCLASS_ID];
            AddSession(AppConstant.SESSION_STUDENTCLASS, studentClass);

            AddSession(AppConstant.SESSION_PREV_PAGE, Request.Path);

            Response.Redirect(AppConstant.PAGEPATH_STUDENT_EDIT);
        }

        protected void BtnBackPrevPage_Click(object sender, ImageClickEventArgs e)
        {
            Configuration_Year year = new Configuration_Year();
            year.YearId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID].ToString());
            AddSession(AppConstant.SESSION_SELECTED_YEAR, year);

            Category_Faculty faculty = new Category_Faculty();
            faculty.FacultyId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTYID].ToString());
            AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);

            Category_Grade grade = new Category_Grade();
            grade.GradeId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_GRADEID].ToString());
            AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);

            Class_Class Class = new Class_Class();
            Class.ClassId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID].ToString());
            AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);

            String strStudentName = ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTNAME].ToString();
            AddSession(AppConstant.SESSION_SELECTED_STUDENTNAME, strStudentName);

            String strStudentCode = ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTCODE].ToString();
            AddSession(AppConstant.SESSION_SELECTED_STUDENTCODE, strStudentCode);

            Response.Redirect(AppConstant.PAGEPATH_STUDENT_LIST);
        }
        #endregion

        #region Dropdownlist event handlers
        protected void DdlNamHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillLopHoc();
        }
        #endregion

        #region Repeater event handlers
        protected void RptStudentFunctions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdfPhysicalPath = (HiddenField)e.Item.FindControl("HdfPhysicalPath");
                if (hdfPhysicalPath.Value == Request.Path)
                {
                    LinkButton lkBtnStudentPage = (LinkButton)e.Item.FindControl("LkBtnStudentPage");
                    lkBtnStudentPage.Style.Add(HtmlTextWriterStyle.TextDecoration, "none");
                }
            }
        }

        protected void RptStudentFunctions_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Redirect":
                    {
                        Student_Student student = new Student_Student();
                        student.StudentId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_STUDENTID].ToString());
                        student.StudentCode = LblStudentCode.Text;
                        student.FullName = LblStudentName.Text;
                        AddSession(AppConstant.SESSION_STUDENT, student);

                        Configuration_Year year = new Configuration_Year();
                        year.YearId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_STUDENTID].ToString());
                        AddSession(AppConstant.SESSION_SELECTED_YEAR, year);

                        Category_Faculty faculty = new Category_Faculty();
                        faculty.FacultyId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTYID].ToString());
                        AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);

                        Category_Grade grade = new Category_Grade();
                        grade.GradeId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_GRADEID].ToString());
                        AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);

                        Class_Class Class = new Class_Class();
                        Class.ClassId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID].ToString());
                        AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);

                        String strStudentName = ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTNAME].ToString();
                        AddSession(AppConstant.SESSION_SELECTED_STUDENTNAME, strStudentName);

                        String strStudentCode = ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTCODE].ToString();
                        AddSession(AppConstant.SESSION_SELECTED_STUDENTCODE, strStudentCode);

                        Class_Class studentClass = new Class_Class();
                        studentClass.ClassId = (int)ViewState[AppConstant.VIEWSTATE_STUDENTCLASS_ID];
                        AddSession(AppConstant.SESSION_STUDENTCLASS, studentClass);

                        Response.Redirect((string)e.CommandArgument);

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        #endregion
    }
}