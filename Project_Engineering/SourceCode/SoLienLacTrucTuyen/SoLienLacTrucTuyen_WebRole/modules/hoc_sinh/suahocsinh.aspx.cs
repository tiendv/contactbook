﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;
using System.IO;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class EditStudentPage : BaseContentPage
    {
        #region Fields
        private StudentBL studentBL;
        private CauHinh_NamHoc currentYear;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (isAccessDenied)
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
                    HocSinh_ThongTinCaNhan student = (HocSinh_ThongTinCaNhan)GetSession(AppConstant.SESSION_STUDENT);
                    RemoveSession(AppConstant.SESSION_STUDENT);
                    ViewState[AppConstant.VIEWSTATE_STUDENTID] = student.MaHocSinh;

                    LopHoc_Lop studentClass = (LopHoc_Lop)GetSession(AppConstant.SESSION_STUDENTCLASS);
                    RemoveSession(AppConstant.SESSION_STUDENTCLASS);
                    ViewState[AppConstant.VIEWSTATE_STUDENTCLASS_ID] = studentClass.MaLopHoc;

                    CauHinh_NamHoc year = (CauHinh_NamHoc)GetSession(AppConstant.SESSION_SELECTED_YEAR);
                    RemoveSession(AppConstant.SESSION_SELECTED_YEAR);
                    ViewState[AppConstant.VIEWSTATE_STUDENTID] = student.MaHocSinh;

                    DanhMuc_NganhHoc faculty = (DanhMuc_NganhHoc)GetSession(AppConstant.SESSION_SELECTED_FACULTY);
                    RemoveSession(AppConstant.SESSION_SELECTED_FACULTY);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTY] = faculty.MaNganhHoc;

                    DanhMuc_KhoiLop grade = (DanhMuc_KhoiLop)GetSession(AppConstant.SESSION_SELECTED_GRADE);
                    RemoveSession(AppConstant.SESSION_SELECTED_GRADE);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_GRADE] = grade.MaKhoiLop;

                    LopHoc_Lop Class = (LopHoc_Lop)GetSession(AppConstant.SESSION_SELECTED_CLASS);
                    RemoveSession(AppConstant.SESSION_SELECTED_CLASS);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS] = Class.MaLopHoc;

                    String strStudentName = (string)GetSession(AppConstant.SESSION_SELECTED_STUDENTNAME);
                    RemoveSession(AppConstant.SESSION_SELECTED_STUDENTNAME);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTNAME] = strStudentName;

                    String strStudentCode = (string)GetSession(AppConstant.SESSION_SELECTED_STUDENTCODE);
                    RemoveSession(AppConstant.SESSION_SELECTED_STUDENTCODE);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTCODE] = strStudentCode;

                    string strPrevPage = (string)GetSession(AppConstant.SESSION_PREV_PAGE);
                    RemoveSession(AppConstant.SESSION_PREV_PAGE);
                    ViewState[AppConstant.VIEWSTATE_PREV_PAGE] = strPrevPage;

                    ViewState[AppConstant.VIEWSTATE_STUDENTID] = student.MaHocSinh;

                    FillStudentPersonalInformation(student);
                }                
            }
        }

        private void FillStudentPersonalInformation(HocSinh_ThongTinCaNhan student)
        {
            student = studentBL.GetStudent(student.MaHocSinh);
            TxtMaHocSinhHienThi.Text = student.MaHocSinhHienThi;
            HdfOldStudentCode.Value = student.MaHocSinhHienThi;
            TxtTenHocSinh.Text = student.HoTen;
            TxtNgaySinhHocSinh.Text = student.NgaySinh.ToShortDateString();
            RbtnNam.Checked = student.GioiTinh;
            RbtnNu.Checked = !student.GioiTinh;
            TxtNoiSinh.Text = student.NoiSinh;
            TxtDiaChi.Text = student.DiaChi;
            TxtDienThoai.Text = student.DienThoai;
            TxtHoTenBo.Text = student.HoTenBo;
            if (student.NgaySinhBo != null)
            {
                TxtNgaySinhBo.Text = ((DateTime)student.NgaySinhBo).ToShortDateString();
            }
            TxtNgheNghiepBo.Text = student.NgheNghiepBo;
            TxtHoTenMe.Text = student.HoTenMe;
            if (student.NgaySinhMe != null)
            {
                TxtNgaySinhMe.Text = ((DateTime)student.NgaySinhMe).ToShortDateString();
            }
            TxtNgheNghiepMe.Text = student.NgheNghiepMe;
            TxtHoTenNguoiDoDau.Text = student.HoTenNguoiDoDau;
            if (student.NgaySinhNguoiDoDau != null)
            {
                TxtNgaySinhNguoiDoDau.Text = ((DateTime)student.NgaySinhNguoiDoDau).ToShortDateString();
            }
            TxtNgheNghiepNguoiDoDau.Text = student.NgheNghiepNguoiDoDau;

            ClassBL lopHocBL = new ClassBL(UserSchool);
            LopHoc_Lop lopHoc = studentBL.GetLastedClass(student);
            LblYear.Text = lopHoc.CauHinh_NamHoc.TenNamHoc;
            DdlNganh.SelectedValue = lopHoc.MaNganhHoc.ToString();
            DdlKhoiLop.SelectedValue = lopHoc.MaKhoiLop.ToString();
            DdlLopHoc.SelectedValue = lopHoc.MaLopHoc.ToString();
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
            HocSinh_ThongTinCaNhan editedStudent = null;
            LopHoc_Lop newClass = null;            

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

            editedStudent = new HocSinh_ThongTinCaNhan();
            editedStudent.MaHocSinh = Int32.Parse(ViewState[AppConstant.VIEWSTATE_STUDENTID].ToString());
            newClass = new LopHoc_Lop();
            newClass.MaLopHoc = Int32.Parse(this.DdlLopHoc.SelectedValue);

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
            List<DanhMuc_NganhHoc> faculties = facultyBL.GetFaculties();
            DdlNganh.DataSource = faculties;
            DdlNganh.DataValueField = "MaNganhHoc";
            DdlNganh.DataTextField = "TenNganhHoc";
            DdlNganh.DataBind();
        }
    
        private void BindDDLGrades()
        {
            GradeBL gradeBL = new GradeBL(UserSchool);
            List<DanhMuc_KhoiLop> grades = gradeBL.GetListGrades();
            DdlKhoiLop.DataSource = grades;
            DdlKhoiLop.DataValueField = "MaKhoiLop";
            DdlKhoiLop.DataTextField = "TenKhoiLop";
            DdlKhoiLop.DataBind();
        }

        private void BindDDLClasses()
        {
            // declare variables
            DanhMuc_NganhHoc faculty = null;
            DanhMuc_KhoiLop grade = null;
            ClassBL classBL = new ClassBL(UserSchool);           

            try
            {
                if (DdlNganh.SelectedIndex >= 0)
                {
                    faculty = new DanhMuc_NganhHoc();
                    faculty.MaNganhHoc = Int32.Parse(DdlNganh.SelectedValue);
                }
            }
            catch (Exception) { }
            
            try
            {
                if (DdlKhoiLop.SelectedIndex >= 0)
                {
                    grade = new DanhMuc_KhoiLop();
                    grade.MaKhoiLop = Int32.Parse(DdlKhoiLop.SelectedValue);
                }
            }
            catch (Exception) { }

            List<LopHoc_Lop> classes = classBL.GetListClasses(currentYear, faculty, grade);
            DdlLopHoc.DataSource = classes;
            DdlLopHoc.DataValueField = "MaLopHoc";
            DdlLopHoc.DataTextField = "TenLopHoc";
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
            CauHinh_NamHoc year = new CauHinh_NamHoc();
            year.MaNamHoc = Int32.Parse(ViewState[AppConstant.VIEWSTATE_STUDENTID].ToString());
            AddSession(AppConstant.SESSION_SELECTED_YEAR, year);

            DanhMuc_NganhHoc faculty = new DanhMuc_NganhHoc();
            faculty.MaNganhHoc = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTY].ToString());
            AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);

            DanhMuc_KhoiLop grade = new DanhMuc_KhoiLop();
            grade.MaKhoiLop = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_GRADE].ToString());
            AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);

            LopHoc_Lop Class = new LopHoc_Lop();
            Class.MaLopHoc = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS].ToString());
            AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);

            String strStudentName = ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTNAME].ToString();
            AddSession(AppConstant.SESSION_SELECTED_STUDENTNAME, strStudentName);

            String strStudentCode = ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTCODE].ToString();
            AddSession(AppConstant.SESSION_SELECTED_STUDENTCODE, strStudentCode);

            string strPrevPage = (string)ViewState[AppConstant.VIEWSTATE_PREV_PAGE];

            if (strPrevPage == AppConstant.PAGEPATH_STUDENTINFOR)
            {
                HocSinh_ThongTinCaNhan student = new HocSinh_ThongTinCaNhan();
                student.MaHocSinh = (int)ViewState[AppConstant.VIEWSTATE_STUDENTID];
                AddSession(AppConstant.SESSION_STUDENT, student);

                LopHoc_Lop studentClass = new LopHoc_Lop();
                studentClass.MaLopHoc = Int32.Parse(ViewState[AppConstant.VIEWSTATE_STUDENTCLASS_ID].ToString());
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