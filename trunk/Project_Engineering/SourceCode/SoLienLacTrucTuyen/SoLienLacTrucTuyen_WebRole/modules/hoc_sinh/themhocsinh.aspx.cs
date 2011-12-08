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
    public partial class AddStudentPage : BaseContentPage
    {
        #region Fields
        private StudentBL hocSinhBL;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (isAccessDenied)
            {
                return;
            }

            hocSinhBL = new StudentBL(UserSchool);

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

            string maHocSinhHienThi = this.TxtMaHocSinhHienThi.Text.Trim();
            if (maHocSinhHienThi == "")
            {
                MaHocSinhRequired.IsValid = false;
                return;
            }
            else
            {
                if (hocSinhBL.StudentCodeExists(maHocSinhHienThi))
                {
                    MaHocSinhValidator.IsValid = false;
                    return;
                }
            }

            string tenHocSinh = this.TxtTenHocSinh.Text.Trim();
            if (tenHocSinh == "")
            {
                TenHocSinhRequired.IsValid = false;
                return;
            }

            string strNgaySinh = this.TxtNgaySinhHocSinh.Text.Trim();
            if (strNgaySinh == "")
            {
                NgaySinhHocSinhRequired.IsValid = false;
                return;
            }

            string diaChi = this.TxtDiaChi.Text.Trim();
            if (diaChi == "")
            {
                DiaChiRequired.IsValid = false;
                return;
            }

            string tenBo = this.TxtHoTenBo.Text.Trim();
            string tenMe = this.TxtHoTenMe.Text.Trim();
            string tenNguoiDoDau = this.TxtHoTenNguoiDoDau.Text;

            if (tenBo == "" && tenMe == "" && tenNguoiDoDau == "")
            {
                LblErrorPhuHuynh.Style.Add(HtmlTextWriterStyle.Display, "none");
                return;
            }
            else
            {
                LblErrorPhuHuynh.Style.Add(HtmlTextWriterStyle.Display, "block");
            }

            string ngheNghiepBo = this.TxtNgheNghiepBo.Text.Trim();
            DateTime? ngaySinhBo = ToDateTime(this.TxtNgaySinhBo.Text.Trim());
            string ngheNghiepMe = this.TxtNgheNghiepMe.Text.Trim();
            DateTime? ngaySinhMe = ToDateTime(this.TxtNgaySinhMe.Text.Trim());
            string ngheNghiepNguoiDoDau = this.TxtNgheNghiepNguoiDoDau.Text.Trim();
            DateTime? ngaySinhNguoiDoDau = ToDateTime(this.TxtNgaySinhNguoiDoDau.Text.Trim());

            bool gioiTinh = this.RbtnNam.Checked;
            string[] arrNgaySinh = strNgaySinh.Split('/');
            DateTime ngaySinh = new DateTime(Int32.Parse(arrNgaySinh[2]),
                Int32.Parse(arrNgaySinh[1]), Int32.Parse(arrNgaySinh[0]));
            string noiSinh = this.TxtNoiSinh.Text.Trim();
            string dienThoai = this.TxtDienThoai.Text.Trim();

            Class = new Class_Class();
            Class.ClassId = Int32.Parse(this.DdlLopHoc.SelectedValue);

            hocSinhBL.InsertStudent(Class, maHocSinhHienThi, tenHocSinh,
                gioiTinh, ngaySinh, noiSinh, diaChi, dienThoai,
                tenBo, ngheNghiepBo, ngaySinhBo,
                tenMe, ngheNghiepMe, ngaySinhMe,
                tenNguoiDoDau, ngheNghiepNguoiDoDau, ngaySinhNguoiDoDau);

            if (this.CkbAddAfterSave.Checked)
            {
                Response.Redirect(Request.Path);
            }
            else
            {
                Response.Redirect("danhsachhocsinh.aspx");
            }
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("danhsachhocsinh.aspx");
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
            BindDropDownListNamHoc();
            BindDropDownListNganhHoc();
            BindDDLGrades();
            BindDDLClasses();
        }

        private void BindDropDownListNamHoc()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<Configuration_Year> lstNamHoc = systemConfigBL.GetListYears();
            DdlNamHoc.DataSource = lstNamHoc;
            DdlNamHoc.DataValueField = "YearId";
            DdlNamHoc.DataTextField = "YearName";
            DdlNamHoc.DataBind();

            if (DdlNamHoc.Items.Count != 0)
            {
                SystemConfigBL cauHinhBL = new SystemConfigBL(UserSchool);
                DdlNamHoc.SelectedValue = cauHinhBL.GetCurrentYear().ToString();
            }
        }

        private void BindDDLGrades()
        {
            GradeBL grades = new GradeBL(UserSchool);
            List<Category_Grade> lstKhoiLop = grades.GetListGrades();
            DdlKhoiLop.DataSource = lstKhoiLop;
            DdlKhoiLop.DataValueField = "GradeId";
            DdlKhoiLop.DataTextField = "GradeName";
            DdlKhoiLop.DataBind();
        }

        private void BindDropDownListNganhHoc()
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
            int YearId = 0;
            try
            {
                YearId = Int32.Parse(DdlNamHoc.SelectedValue);
            }
            catch (Exception) { }

            int FacultyId = 0;
            try
            {
                FacultyId = Int32.Parse(DdlNganh.SelectedValue);
            }
            catch (Exception) { }

            int GradeId = 0;
            try
            {
                GradeId = Int32.Parse(DdlKhoiLop.SelectedValue);
            }
            catch (Exception) { }

            List<Class_Class> lstLop = GetListLopHoc(FacultyId, GradeId, YearId);
            DdlLopHoc.DataSource = lstLop;
            DdlLopHoc.DataValueField = "ClassId";
            DdlLopHoc.DataTextField = "ClassName";
            DdlLopHoc.DataBind();

        }

        private List<Class_Class> GetListLopHoc(int FacultyId, int GradeId, int YearId)
        {
            Configuration_Year year = null;
            Category_Faculty faculty = null;
            Category_Grade grade = null;
            ClassBL lopHocBL = new ClassBL(UserSchool);
            
            year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);

            try
            {
                if (DdlNganh.SelectedIndex > 0)
                {
                    faculty = new Category_Faculty();
                    faculty.FacultyId = Int32.Parse(DdlNganh.SelectedValue);
                }
            }
            catch (Exception) { }

            try
            {
                if (DdlKhoiLop.SelectedIndex > 0)
                {
                    grade = new Category_Grade();
                    grade.GradeId = Int32.Parse(DdlKhoiLop.SelectedValue);
                }
            }
            catch (Exception) { }

            List<Class_Class> lstLop = lopHocBL.GetListClasses(year, faculty, grade);
            return lstLop;
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