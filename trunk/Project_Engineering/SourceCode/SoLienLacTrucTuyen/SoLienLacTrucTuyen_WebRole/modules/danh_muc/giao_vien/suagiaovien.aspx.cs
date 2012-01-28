using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class suagiaovien : BaseContentPage
    {
        #region Fields
        TeacherBL teacherBL;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (accessDenied)
            {
                return;
            }

            if (sessionExpired)
            {
                FormsAuthentication.SignOut();
                Response.Redirect(FormsAuthentication.LoginUrl);
            }

            teacherBL = new TeacherBL(UserSchool);

            if (!Page.IsPostBack)
            {
                if (RetrieveSessions())
                {
                    FillGiaoVien(new Guid(ViewState[AppConstant.VIEWSTATE_SELECTED_USERID].ToString()));
                }
                else
                {
                    Response.Redirect(AppConstant.PAGEPATH_TEACHER_LIST);
                }
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
            editedTeacher.UserId = teacherBL.GetTeacher(UserId).UserId;
            teacherBL.UpdateTeacher(editedTeacher, tenGiaoVien, gioiTinh, ngaySinh, diaChi, dienThoai);

            BackToPrevPage();            
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            BackToPrevPage();
        }
        #endregion

        #region Methods
        private void FillGiaoVien(Guid teacherId)
        {
            aspnet_User teacher = teacherBL.GetTeacher(teacherId);
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

        private bool RetrieveSessions()
        {
            if (CheckSessionKey(AppConstant.SESSION_SELECTED_USER)
                && CheckSessionKey(AppConstant.SESSION_PREVIOUSPAGE))
            {
                aspnet_User teacher = (aspnet_User)GetSession(AppConstant.SESSION_SELECTED_USER);
                RemoveSession(AppConstant.SESSION_SELECTED_USER);
                ViewState[AppConstant.VIEWSTATE_SELECTED_USERID] = teacher.UserId;

                ViewState[AppConstant.VIEWSTATE_PREV_PAGE] = (string)GetSession(AppConstant.SESSION_PREVIOUSPAGE);
                RemoveSession(AppConstant.SESSION_PREVIOUSPAGE);
                return true;
            }

            return false;
        }

        private void BackToPrevPage()
        {
            String strPrevPage = (string)ViewState[AppConstant.VIEWSTATE_PREV_PAGE];
            if (strPrevPage == AppConstant.PAGEPATH_TEACHER_DETAIL)
            {
                aspnet_User teacher = new aspnet_User();
                teacher.UserId = new Guid(ViewState[AppConstant.VIEWSTATE_SELECTED_USERID].ToString());
                AddSession(AppConstant.SESSION_SELECTED_USER, teacher);
            }

            Response.Redirect(strPrevPage);
        }
        #endregion
    }
}