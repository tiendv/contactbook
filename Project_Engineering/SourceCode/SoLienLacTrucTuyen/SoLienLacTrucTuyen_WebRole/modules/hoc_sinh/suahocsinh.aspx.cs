using System;
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
    public partial class SuaHocSinh : BaseContentPage
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

            if (!Page.IsPostBack)
            {
                BindDropDownLists();
                int maHocSinh = Int32.Parse(Request.QueryString["hocsinh"]);
                int maLopHoc = Int32.Parse(Request.QueryString["lop"]);
                ViewState["MaHocSinh"] = maHocSinh;
                FillHocSinh(maHocSinh, maLopHoc);
            }
        }

        private void FillHocSinh(int maHocSinh, int maLopHoc)
        {
            HocSinh_ThongTinCaNhan thongTinCaNhan = hocSinhBL.GetStudent(maHocSinh);
            TxtMaHocSinhHienThi.Text = thongTinCaNhan.MaHocSinhHienThi;
            TxtTenHocSinh.Text = thongTinCaNhan.HoTen;
            TxtNgaySinhHocSinh.Text = thongTinCaNhan.NgaySinh.ToShortDateString();
            RbtnNam.Checked = thongTinCaNhan.GioiTinh;
            RbtnNu.Checked = !thongTinCaNhan.GioiTinh;
            TxtNoiSinh.Text = thongTinCaNhan.NoiSinh;
            TxtDiaChi.Text = thongTinCaNhan.DiaChi;
            TxtDienThoai.Text = thongTinCaNhan.DienThoai;
            TxtHoTenBo.Text = thongTinCaNhan.HoTenBo;
            if (thongTinCaNhan.NgaySinhBo != null)
            {
                TxtNgaySinhBo.Text = ((DateTime)thongTinCaNhan.NgaySinhBo).ToShortDateString();
            }
            TxtNgheNghiepBo.Text = thongTinCaNhan.NgheNghiepBo;
            TxtHoTenMe.Text = thongTinCaNhan.HoTenMe;
            if (thongTinCaNhan.NgaySinhMe != null)
            {
                TxtNgaySinhMe.Text = ((DateTime)thongTinCaNhan.NgaySinhMe).ToShortDateString();
            }
            TxtNgheNghiepMe.Text = thongTinCaNhan.NgheNghiepMe;
            TxtHoTenNguoiDoDau.Text = thongTinCaNhan.HoTenNguoiDoDau;
            if (thongTinCaNhan.NgaySinhNguoiDoDau != null)
            {
                TxtNgaySinhNguoiDoDau.Text = ((DateTime)thongTinCaNhan.NgaySinhNguoiDoDau).ToShortDateString();
            }
            TxtNgheNghiepNguoiDoDau.Text = thongTinCaNhan.NgheNghiepNguoiDoDau;

            ClassBL lopHocBL = new ClassBL(UserSchool);
            LopHoc_Lop lopHoc = lopHocBL.GetClass(maLopHoc);
            DdlNamHoc.SelectedValue = lopHoc.MaNamHoc.ToString();
            DdlNganh.SelectedValue = lopHoc.MaNganhHoc.ToString();
            DdlKhoiLop.SelectedValue = lopHoc.MaKhoiLop.ToString();
            DdlLopHoc.SelectedValue = lopHoc.MaLopHoc.ToString();
            BindDropDownListLopHoc();
        }
        #endregion

        #region DropDownList event hanlders
        protected void DdlNamHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropDownListLopHoc();
        }

        protected void DdlNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropDownListLopHoc();
        }

        protected void DdlKhoiLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropDownListLopHoc();
        }
        #endregion

        #region Button event handlers
        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            int maLopHoc = Int32.Parse(this.DdlLopHoc.SelectedValue);

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

            //hocSinhBL.InsertHocSinh(maLopHoc, maHocSinhHienThi, tenHocSinh,
            //    gioiTinh, ngaySinh, noiSinh, diaChi, dienThoai,
            //    tenBo, ngheNghiepBo, ngaySinhBo,
            //    tenMe, ngheNghiepMe, ngaySinhMe,
            //    tenNguoiDoDau, ngheNghiepNguoiDoDau, ngaySinhNguoiDoDau);

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
            BindDropDownListKhoiLop();
            BindDropDownListLopHoc();
        }

        private void BindDropDownListNamHoc()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<CauHinh_NamHoc> lstNamHoc = systemConfigBL.GetListYears();
            DdlNamHoc.DataSource = lstNamHoc;
            DdlNamHoc.DataValueField = "MaNamHoc";
            DdlNamHoc.DataTextField = "TenNamHoc";
            DdlNamHoc.DataBind();
        }

        private void BindDropDownListKhoiLop()
        {
            GradeBL KhoiLopBL = new GradeBL(UserSchool);
            List<DanhMuc_KhoiLop> lstKhoiLop = KhoiLopBL.GetListGrades();
            DdlKhoiLop.DataSource = lstKhoiLop;
            DdlKhoiLop.DataValueField = "MaKhoiLop";
            DdlKhoiLop.DataTextField = "TenKhoiLop";
            DdlKhoiLop.DataBind();
        }

        private void BindDropDownListNganhHoc()
        {
            FacultyBL nganhHocBL = new FacultyBL(UserSchool);
            List<DanhMuc_NganhHoc> lstNganhHoc = nganhHocBL.GetFaculties();
            DdlNganh.DataSource = lstNganhHoc;
            DdlNganh.DataValueField = "MaNganhHoc";
            DdlNganh.DataTextField = "TenNganhHoc";
            DdlNganh.DataBind();
        }

        private void BindDropDownListLopHoc()
        {
            int maNamHoc = 0;
            try
            {
                maNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);
            }
            catch (Exception) { }

            int maNganhHoc = 0;
            try
            {
                maNganhHoc = Int32.Parse(DdlNganh.SelectedValue);
            }
            catch (Exception) { }

            int maKhoiLop = 0;
            try
            {
                maKhoiLop = Int32.Parse(DdlKhoiLop.SelectedValue);
            }
            catch (Exception) { }

            List<LopHoc_Lop> lstLop = GetListLopHoc(maNganhHoc, maKhoiLop, maNamHoc);
            DdlLopHoc.DataSource = lstLop;
            DdlLopHoc.DataValueField = "MaLopHoc";
            DdlLopHoc.DataTextField = "TenLopHoc";
            DdlLopHoc.DataBind();
        }

        private List<LopHoc_Lop> GetListLopHoc(int maNganhHoc, int maKhoiLop, int maNamHoc)
        {
            ClassBL lopHocBL = new ClassBL(UserSchool);

            DanhMuc_NganhHoc faculty = new DanhMuc_NganhHoc();
            faculty.MaNganhHoc = maNganhHoc;

            DanhMuc_KhoiLop grade = new DanhMuc_KhoiLop();
            grade.MaKhoiLop = maKhoiLop;

            CauHinh_NamHoc year = new CauHinh_NamHoc();
            year.MaNamHoc = maNamHoc;

            List<LopHoc_Lop> lstLop = lopHocBL.GetListClasses(year, faculty, grade);
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