using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class themgiaovien : System.Web.UI.Page
    {
        #region Fields
        TeacherBL giaoVienBL = new TeacherBL();
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
        }
        #endregion

        #region Button click event handlers
        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            string maGiaoVien = TxtMaGiaoVienHienThi.Text.Trim();
            string tenGiaoVien = TxtTenGiaoVien.Text.Trim();
            string strNgaySinh = TxtNgaySinh.Text.Trim();
            DateTime ngaySinh = new DateTime(); 
            bool gioiTinh = RbtnNam.Checked;
            string diaChi = TxtDiaChi.Text.Trim();
            string dienThoai = TxtDienThoai.Text.Trim();

            if (maGiaoVien == "")
            {
                MaGiaoVienHienThiRequired.IsValid = false;
                return;
            }
            else
            {
                if(giaoVienBL.TeacherCodeExists(maGiaoVien))
                {
                    MaGiaoVienValidator.IsValid = false;
                    return;
                }
                else
                {
                    if (tenGiaoVien == "")
                    {
                        TenGiaoVienRequired.IsValid = false;
                        return;
                    }
                    else
                    {
                        if (strNgaySinh == "")
                        {
                            
                            MaGiaoVienHienThiRequired.IsValid = false;
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
                }
            }

            ngaySinh = DateTime.Parse(strNgaySinh);
            giaoVienBL.InsertTeacher(maGiaoVien, tenGiaoVien, gioiTinh, ngaySinh, diaChi, dienThoai);

            if (CkbAddAfterSave.Checked)
            {
                TxtMaGiaoVienHienThi.Text = "";
                TxtTenGiaoVien.Text = "";
                TxtNgaySinh.Text = "";
                RbtnNam.Checked = true;
                TxtDiaChi.Text = "";
                TxtDienThoai.Text = "";
            }
            else
            {
                Response.Redirect("/modules/danh_muc/giao_vien/danhsachgiaovien.aspx");
            }
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/modules/danh_muc/giao_vien/danhsachgiaovien.aspx");
        }
        #endregion
    }
}