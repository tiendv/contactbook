using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class suagiaovien : System.Web.UI.Page
    {
        #region Fields
        GiaoVienBL giaoVienBL = new GiaoVienBL();
        #endregion

        #region Page event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageUrl = Page.Request.Path;
            Guid role = (new UserBL()).GetRoleId(User.Identity.Name);

            if (!(new RoleBL()).ValidateAuthorization(role, pageUrl))
            {
                Response.Redirect((string)GetGlobalResourceObject("MainResource", "AccessDeniedPageUrl"));
                return;
            }

            Site masterPage = (Site)Page.Master;
            masterPage.UserRole = role;
            masterPage.PageUrl = pageUrl;

            if (!Page.IsPostBack)
            {
                ViewState["prevpageid"] = Request.QueryString["prevpageid"];

                string maGiaoVien = Request.QueryString["giaovien"];
                ViewState["giaovien"] = maGiaoVien;                
                FillGiaoVien(Int32.Parse(maGiaoVien));
            }
        }
        #endregion

        #region Button click event handlers
        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            string maGiaoVien = LblMaGiaoVienHienThi.Text.Trim();
            string tenGiaoVien = TxtTenGiaoVien.Text.Trim();
            string strNgaySinh = TxtNgaySinh.Text.Trim();
            DateTime ngaySinh = new DateTime();
            bool gioiTinh = RbtnNam.Checked;
            string diaChi = TxtDiaChi.Text.Trim();
            string dienThoai = TxtDienThoai.Text.Trim();

            if (tenGiaoVien == "")
            {
                TenGiaoVienRequired.IsValid = false;
                return;
            }
            else
            {
                if (strNgaySinh == "")
                {
                    NgaySinhRequired.IsValid = false;
                    return;
                }
                else
                {
                    if (diaChi == "")
                    {
                        DiaChiRequired.IsValid = false;
                        return;
                    }
                }
            }

            ngaySinh = DateTime.Parse(strNgaySinh);
            giaoVienBL.UpdateGiaoVien(maGiaoVien, tenGiaoVien, gioiTinh, ngaySinh, diaChi, dienThoai);

            if ((string)ViewState["prevpageid"] == "1")
            {
                Response.Redirect("/modules/danh_muc/giao_vien/danhsachgiaovien.aspx");
            }
            else
            {
                Response.Redirect("/modules/danh_muc/giao_vien/chitietgiaovien.aspx?giaovien="
                    + ViewState["giaovien"]);
            }
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            if ((string)ViewState["prevpageid"] == "1")
            {
                Response.Redirect("/modules/danh_muc/giao_vien/danhsachgiaovien.aspx");
            }
            else
            {
                Response.Redirect("/modules/danh_muc/giao_vien/chitietgiaovien.aspx?giaovien="
                    + ViewState["giaovien"]);
            }
        }
        #endregion

        #region Methods
        private void FillGiaoVien(int maGiaoVien)
        {
            LopHoc_GiaoVien giaoVien = giaoVienBL.GetGiaoVien(maGiaoVien);
            LblMaGiaoVienHienThi.Text = giaoVien.MaHienThiGiaoVien;
            TxtTenGiaoVien.Text = giaoVien.HoTen;
            TxtNgaySinh.Text = giaoVien.NgaySinh.ToShortDateString();
            RbtnNam.Checked = giaoVien.GioiTinh;
            RbtnNu.Checked = !giaoVien.GioiTinh;
            TxtDiaChi.Text = giaoVien.DiaChi;
            TxtDienThoai.Text = giaoVien.DienThoai;
        }
        #endregion
    }
}