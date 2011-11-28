﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;
using AjaxControlToolkit;
using System.Web.UI.HtmlControls;

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
            if (isAccessDenied)
            {
                return;
            }

            studentBL = new StudentBL(UserSchool);

            if (!Page.IsPostBack)
            {
                if (CheckSessionKey(AppConstant.SESSION_STUDENT))
                {
                    HocSinh_ThongTinCaNhan student = (HocSinh_ThongTinCaNhan)GetSession(AppConstant.SESSION_STUDENT);
                    RemoveSession(AppConstant.SESSION_STUDENT);
                    ViewState[AppConstant.VIEWSTATE_STUDENTID] = student.MaHocSinh;

                    CauHinh_NamHoc year = (CauHinh_NamHoc)GetSession(AppConstant.SESSION_YEAR);
                    RemoveSession(AppConstant.SESSION_YEAR);
                    ViewState[AppConstant.VIEWSTATE_STUDENTID] = student.MaHocSinh;

                    DanhMuc_NganhHoc faculty = (DanhMuc_NganhHoc)GetSession(AppConstant.SESSION_FACULTY);
                    RemoveSession(AppConstant.SESSION_FACULTY);
                    ViewState[AppConstant.VIEWSTATE_FACULTY] = faculty.MaNganhHoc;

                    DanhMuc_KhoiLop grade = (DanhMuc_KhoiLop)GetSession(AppConstant.SESSION_GRADE);
                    RemoveSession(AppConstant.SESSION_GRADE);
                    ViewState[AppConstant.VIEWSTATE_GRADE] = grade.MaKhoiLop;

                    LopHoc_Lop Class = (LopHoc_Lop)GetSession(AppConstant.SESSION_CLASS);
                    RemoveSession(AppConstant.SESSION_CLASS);
                    ViewState[AppConstant.VIEWSTATE_CLASS] = Class.MaLopHoc;

                    String strStudentName = (string)GetSession(AppConstant.SESSION_STUDENTNAME);
                    RemoveSession(AppConstant.SESSION_STUDENTNAME);
                    ViewState[AppConstant.VIEWSTATE_STUDENTNAME] = strStudentName;

                    String strStudentCode = (string)GetSession(AppConstant.SESSION_STUDENTCODE);
                    RemoveSession(AppConstant.SESSION_STUDENTCODE);
                    ViewState[AppConstant.VIEWSTATE_STUDENTCODE] = strStudentCode;

                    ViewState[AppConstant.VIEWSTATE_STUDENTID] = student.MaHocSinh;
                    BindDDLYears(student);
                    FillPersonalInformation(student);

                    AuthorizationBL authorizationBL = new AuthorizationBL(UserSchool);
                    List<UserManagement_PagePath> pagePages = authorizationBL.GetStudentPages(
                        (new UserBL()).GetRoles(User.Identity.Name));
                    foreach (UserManagement_PagePath pagePage in pagePages)
                    {
                        if (pagePage.PhysicalPath == Request.Path)
                        {
                            pagePage.PhysicalPath = "";
                        }
                        else
                        {
                            pagePage.PhysicalPath = String.Format("{0}?hocsinh={1}", pagePage.PhysicalPath, student.MaHocSinh);
                        }
                    }
                    RptStudentFunctions.DataSource = pagePages;
                    RptStudentFunctions.DataBind();
                }
            }
        }
        #endregion

        #region Methods
        private void BindDDLYears(HocSinh_ThongTinCaNhan student)
        {
            List<CauHinh_NamHoc> years = studentBL.GetYears(student);
            DdlNamHoc.DataSource = years;
            DdlNamHoc.DataValueField = "MaNamHoc";
            DdlNamHoc.DataTextField = "TenNamHoc";
            DdlNamHoc.DataBind();
        }

        private void FillLopHoc()
        {
            HocSinh_ThongTinCaNhan student = new HocSinh_ThongTinCaNhan();
            student.MaHocSinh = (int)ViewState[AppConstant.VIEWSTATE_STUDENTID];
            CauHinh_NamHoc year = new CauHinh_NamHoc();
            year.MaNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);

            TabularClass tabularClass = studentBL.GetTabularClass(year, student);
            LblLopHoc.Text = tabularClass.TenLopHoc;
        }

        private void FillPersonalInformation(HocSinh_ThongTinCaNhan student)
        {
            student = studentBL.GetStudent(student.MaHocSinh);

            this.LblMaHocSinhHienThi.Text = student.MaHocSinhHienThi;
            this.LblHoTenHocSinh.Text = student.HoTen;
            this.LblGioiTinh.Text = (student.GioiTinh == true) ? "Nam" : "Nữ";
            this.LblNgaySinhHocSinh.Text = student.NgaySinh.Day.ToString()
                + "/" + student.NgaySinh.Month.ToString()
                + "/" + student.NgaySinh.Year.ToString();
            this.LblNoiSinh.Text = student.NoiSinh;
            this.LblDiaChi.Text = student.DiaChi;
            this.LblDienThoai.Text = student.DienThoai;
            this.LblHoTenBo.Text = student.HoTenBo;
            if (student.NgaySinhBo != null)
            {
                DateTime ngaySinhBo = (DateTime)student.NgaySinhBo;
                this.LblNgaySinhBo.Text = ngaySinhBo.Day.ToString() + "/" + ngaySinhBo.Month.ToString()
                    + "/" + ngaySinhBo.Year.ToString();
            }
            this.LblNgheNghiepBo.Text = student.NgheNghiepBo;
            this.LblHoTenMe.Text = student.HoTenMe;
            if (student.NgaySinhMe != null)
            {
                DateTime ngaySinhMe = (DateTime)student.NgaySinhMe;
                this.LblNgaySinhMe.Text = ngaySinhMe.Day.ToString() + "/" + ngaySinhMe.Month.ToString()
                    + "/" + ngaySinhMe.Year.ToString();
            }
            this.LblNgheNghiepMe.Text = student.NgheNghiepMe;
            if (student.NgaySinhNguoiDoDau != null)
            {
                DateTime ngaySinhNguoiDoDau = (DateTime)student.NgaySinhNguoiDoDau;
                this.LblNgaySinhNguoiDoDau.Text = ngaySinhNguoiDoDau.Day.ToString() + "/" + ngaySinhNguoiDoDau.Month.ToString()
                    + "/" + ngaySinhNguoiDoDau.Year.ToString();
            }
            this.LblNgheNghiepNguoiDoDau.Text = student.NgheNghiepNguoiDoDau;

            FillLopHoc();
        }
        #endregion

        #region Button event handlers
        protected void BtnSua_Click(object sender, ImageClickEventArgs e)
        {
            // Get seleteced student and set to session
            HocSinh_ThongTinCaNhan student = new HocSinh_ThongTinCaNhan();
            student.MaHocSinh = Int32.Parse(ViewState[AppConstant.VIEWSTATE_STUDENTID].ToString());
            AddSession(AppConstant.SESSION_STUDENT, student);

            //// Get seleteced class and set to session
            //LopHoc_Lop Class = new LopHoc_Lop();
            //Class.MaLopHoc = Int32.Parse(((HiddenField)e.Item.FindControl("HdfMaLopHoc")).Value);
            //AddSession(AppConstant.SESSION_CLASS, Class);

            Response.Redirect(AppConstant.PAGEPATH_STUDENTEDIT);
        }

        protected void BtnBackPrevPage_Click(object sender, ImageClickEventArgs e)
        {
            CauHinh_NamHoc year = new CauHinh_NamHoc();
            year.MaNamHoc = Int32.Parse(ViewState[AppConstant.VIEWSTATE_STUDENTID].ToString());
            AddSession(AppConstant.SESSION_YEAR, year);

            DanhMuc_NganhHoc faculty = new DanhMuc_NganhHoc();
            faculty.MaNganhHoc = Int32.Parse(ViewState[AppConstant.VIEWSTATE_FACULTY].ToString());
            AddSession(AppConstant.SESSION_FACULTY, faculty);

            DanhMuc_KhoiLop grade = new DanhMuc_KhoiLop();
            grade.MaKhoiLop = Int32.Parse(ViewState[AppConstant.VIEWSTATE_GRADE].ToString());
            AddSession(AppConstant.SESSION_GRADE, grade);

            LopHoc_Lop Class = new LopHoc_Lop();
            Class.MaLopHoc = Int32.Parse(ViewState[AppConstant.VIEWSTATE_CLASS].ToString());
            AddSession(AppConstant.SESSION_CLASS, Class);

            String strStudentName = ViewState[AppConstant.VIEWSTATE_STUDENTNAME].ToString();
            AddSession(AppConstant.SESSION_STUDENTNAME, strStudentName);

            String strStudentCode = ViewState[AppConstant.VIEWSTATE_STUDENTCODE].ToString();
            AddSession(AppConstant.SESSION_STUDENTCODE, strStudentCode);

            Response.Redirect(AppConstant.PAGEPATH_STUDENTS);
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