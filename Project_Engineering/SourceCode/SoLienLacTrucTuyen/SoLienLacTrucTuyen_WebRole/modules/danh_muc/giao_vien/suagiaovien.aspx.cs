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
            if (accessDenied)
            {
                return;
            }

            giaoVienBL = new TeacherBL(UserSchool);

            if (!Page.IsPostBack)
            {
                ViewState["prevpageid"] = Request.QueryString["prevpageid"];

                string UserId = Request.QueryString["giaovien"];
                ViewState["giaovien"] = UserId;
                FillGiaoVien(new Guid(UserId));
            }
        }
        #endregion

        #region Button click event handlers
        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            string UserId = LblUserIdHienThi.Text.Trim();
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

            aspnet_Membership editedTeacher = new aspnet_Membership();
            editedTeacher.UserId = giaoVienBL.GetTeacher(UserId).UserId;
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
        private void FillGiaoVien(Guid teacherId)
        {
            aspnet_User teacher = giaoVienBL.GetTeacher(teacherId);
            LblUserIdHienThi.Text = teacher.UserName.Split('_')[1];
            TxtTenGiaoVien.Text = teacher.aspnet_Membership.FullName;
            if (teacher.aspnet_Membership.Birthday != null)
            {
                TxtNgaySinh.Text = ((DateTime)teacher.aspnet_Membership.Birthday).ToShortDateString();
            }
            if (teacher.aspnet_Membership.Gender != null)
            {
                RbtnNam.Checked = (bool)teacher.aspnet_Membership.Gender;
                RbtnNu.Checked = !(bool)teacher.aspnet_Membership.Gender;
            }

            TxtDiaChi.Text = teacher.aspnet_Membership.Address;
            TxtDienThoai.Text = (teacher.aspnet_Membership.Phone != "") ? teacher.aspnet_Membership.Phone : "(không có)";
        }
        #endregion
    }
}