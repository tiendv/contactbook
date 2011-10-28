using System;
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
    public partial class thongtincanhan : System.Web.UI.Page
    {
        #region Fields
        private HocSinhBL hocSinhBL;
        private List<AccessibilityEnum> lstAccessibilities;
        #endregion

        #region Page event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            UserBL userBL = new UserBL();
            RoleBL roleBL = new RoleBL();
            hocSinhBL = new HocSinhBL();

            string pageUrl = Page.Request.Path;
            Guid role = userBL.GetRoleId(User.Identity.Name);

            if (!roleBL.ValidateAuthorization(role, pageUrl))
            {
                Response.Redirect((string)GetGlobalResourceObject("MainResource", "AccessDeniedPageUrl"));
                return;
            }

            Site masterPage = (Site)Page.Master;
            masterPage.UserRole = role;
            masterPage.PageUrl = pageUrl;

            lstAccessibilities = roleBL.GetAccessibilities(role, pageUrl);

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["hocsinh"] != null)
                {
                    int maHocSinh = Int32.Parse(Request.QueryString["hocsinh"]);
                    ViewState["MaHocSinh"] = maHocSinh;
                    BindDropDownListNamHoc(maHocSinh);
                    FillThongTinCaNhan(maHocSinh);

                    HlkKetQuaHocTap.NavigateUrl = string.Format("ketquahoctap.aspx?hocsinh={0}", maHocSinh);
                    HlkNgayNghiHoc.NavigateUrl = string.Format("ngaynghihoc.aspx?hocsinh={0}", maHocSinh);                    
                    HlkHoatDong.NavigateUrl = String.Format("hoatdong.aspx?hocsinh={0}", maHocSinh);
                }
            }            
        }
        #endregion

        #region Methods
        private void BindDropDownListNamHoc(int maHocSinh)
        {
            NamHocBL namHocBL = new NamHocBL();
            List<CauHinh_NamHoc> lstNamHoc = hocSinhBL.GetListNamHoc(maHocSinh);
            DdlNamHoc.DataSource = lstNamHoc;
            DdlNamHoc.DataValueField = "MaNamHoc";
            DdlNamHoc.DataTextField = "TenNamHoc";
            DdlNamHoc.DataBind();
        }

        private void FillLopHoc()
        {
            int maNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);
            LopHocInfo lopHoc = hocSinhBL.GetLopHocInfo(maNamHoc, (int)ViewState["MaHocSinh"]);
            LblLopHoc.Text = lopHoc.TenLopHoc;
        }

        private void FillThongTinCaNhan(int maHocSinh)
        {
            HocSinh_ThongTinCaNhan thongTinCaNhan = hocSinhBL.GetThongTinCaNhan(maHocSinh);

            this.LblMaHocSinhHienThi.Text = thongTinCaNhan.MaHocSinhHienThi;
            this.LblHoTenHocSinh.Text = thongTinCaNhan.HoTen;
            this.LblGioiTinh.Text = (thongTinCaNhan.GioiTinh == true) ? "Nam" : "Nữ";
            this.LblNgaySinhHocSinh.Text = thongTinCaNhan.NgaySinh.Day.ToString()
                + "/" + thongTinCaNhan.NgaySinh.Month.ToString()
                + "/" + thongTinCaNhan.NgaySinh.Year.ToString();
            this.LblNoiSinh.Text = thongTinCaNhan.NoiSinh;
            this.LblDiaChi.Text = thongTinCaNhan.DiaChi;
            this.LblDienThoai.Text = thongTinCaNhan.DienThoai;
            this.LblHoTenBo.Text = thongTinCaNhan.HoTenBo;
            if (thongTinCaNhan.NgaySinhBo != null)
            {
                DateTime ngaySinhBo = (DateTime)thongTinCaNhan.NgaySinhBo;
                this.LblNgaySinhBo.Text = ngaySinhBo.Day.ToString() + "/" + ngaySinhBo.Month.ToString()
                    + "/" + ngaySinhBo.Year.ToString();
            }
            this.LblNgheNghiepBo.Text = thongTinCaNhan.NgheNghiepBo;
            this.LblHoTenMe.Text = thongTinCaNhan.HoTenMe;
            if (thongTinCaNhan.NgaySinhMe != null)
            {
                DateTime ngaySinhMe = (DateTime)thongTinCaNhan.NgaySinhMe;
                this.LblNgaySinhMe.Text = ngaySinhMe.Day.ToString() + "/" + ngaySinhMe.Month.ToString()
                    + "/" + ngaySinhMe.Year.ToString();
            }
            this.LblNgheNghiepMe.Text = thongTinCaNhan.NgheNghiepMe;
            if (thongTinCaNhan.NgaySinhNguoiDoDau != null)
            {
                DateTime ngaySinhNguoiDoDau = (DateTime)thongTinCaNhan.NgaySinhNguoiDoDau;
                this.LblNgaySinhNguoiDoDau.Text = ngaySinhNguoiDoDau.Day.ToString() + "/" + ngaySinhNguoiDoDau.Month.ToString()
                    + "/" + ngaySinhNguoiDoDau.Year.ToString();
            }
            this.LblNgheNghiepNguoiDoDau.Text = thongTinCaNhan.NgheNghiepNguoiDoDau;

            FillLopHoc();
        }
        #endregion

        #region Button event handlers
        protected void BtnSua_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(string.Format("suahocsinh.aspx?hocsinh={0}", ViewState["MaHocSinh"]));
        }

        protected void BtnBackPrevPage_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("danhsachhocsinh.aspx");
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