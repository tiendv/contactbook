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
    public partial class suagiaovien : BaseContentPage
    {
        #region Fields
        TeacherBL giaoVienBL;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (isAccessDenied)
            {
                return;
            }

            giaoVienBL = new TeacherBL(UserSchool);

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

            LopHoc_GiaoVien editedTeacher = giaoVienBL.GetTeacher(maGiaoVien);
            giaoVienBL.UpdateTeacher(editedTeacher, tenGiaoVien, gioiTinh, ngaySinh, diaChi, dienThoai);

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
        private void FillGiaoVien(int teacherId)
        {
            LopHoc_GiaoVien teacher = giaoVienBL.GetTeacher(teacherId);
            LblMaGiaoVienHienThi.Text = teacher.MaHienThiGiaoVien;
            TxtTenGiaoVien.Text = teacher.HoTen;
            TxtNgaySinh.Text = teacher.NgaySinh.ToShortDateString();
            RbtnNam.Checked = teacher.GioiTinh;
            RbtnNu.Checked = !teacher.GioiTinh;
            TxtDiaChi.Text = teacher.DiaChi;
            TxtDienThoai.Text = teacher.DienThoai;
        }
        #endregion
    }
}