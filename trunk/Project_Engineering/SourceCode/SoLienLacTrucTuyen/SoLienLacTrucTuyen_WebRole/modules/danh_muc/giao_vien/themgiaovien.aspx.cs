using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class AddingTeacherPage : BaseContentPage
    {
        #region Fields
        TeacherBL teacherBL;
        #endregion

        #region Page event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (isAccessDenied)
            {
                return;
            }

            teacherBL = new TeacherBL(UserSchool);
        }
        #endregion

        #region Button click event handlers
        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            string strTeacherCode = TxtMaGiaoVienHienThi.Text.Trim();
            string strTeacherName = TxtTenGiaoVien.Text.Trim();
            string strBirthday = TxtNgaySinh.Text.Trim();
            DateTime dtBirthday = new DateTime(); 
            bool bGender = RbtnNam.Checked;
            string strAddress = TxtDiaChi.Text.Trim();
            string strPhone = TxtDienThoai.Text.Trim();

            if (strTeacherCode == "")
            {
                MaGiaoVienHienThiRequired.IsValid = false;
                return;
            }
            else
            {
                if(teacherBL.TeacherCodeExists(strTeacherCode))
                {
                    MaGiaoVienValidator.IsValid = false;
                    return;
                }
                else
                {
                    if (strTeacherName == "")
                    {
                        TenGiaoVienRequired.IsValid = false;
                        return;
                    }
                    else
                    {
                        if (strBirthday == "")
                        {
                            
                            MaGiaoVienHienThiRequired.IsValid = false;
                            return;
                        }
                        else
                        {                        
                            if (strAddress == "")
                            {
                                DiaChiRequired.IsValid = false;
                                return;
                            }
                        }
                    }
                }
            }

            dtBirthday = DateTime.Parse(strBirthday);
            teacherBL.InsertTeacher(strTeacherCode, strTeacherName, bGender, dtBirthday, strAddress, strPhone);

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