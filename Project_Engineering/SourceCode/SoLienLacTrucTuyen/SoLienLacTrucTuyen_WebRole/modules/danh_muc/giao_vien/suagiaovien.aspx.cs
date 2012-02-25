using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using System.Web.Security;
using System.IO;
using System.Text;

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

            string strTeacherName = TxtTenGiaoVien.Text.Trim();
            string strDateOfBirth = TxtNgaySinh.Text.Trim();
            DateTime? dtDateOfBirth;
            bool bGender = RbtnNam.Checked;
            string strAddress = TxtDiaChi.Text.Trim();
            string strPhone = TxtDienThoai.Text.Trim();

            if (CheckUntils.IsNullOrBlank(strTeacherName))
            {
                TenGiaoVienRequired.IsValid = false;
                return;
            }

            if (CheckUntils.IsNullOrBlank(strDateOfBirth))
            {
                NgaySinhRequired.IsValid = false;
                return;
            }

            if (CheckUntils.IsNullOrBlank(strAddress))
            {
                DiaChiRequired.IsValid = false;
                return;
            }

            dtDateOfBirth = DateUtils.StringToDateVN(strDateOfBirth);
            if (dtDateOfBirth == null)
            {
                DateOfBirthCustomValidator.IsValid = false;
                return;
            }

            byte[] bPhoto = null;
            if (CheckSessionKey("Photo"))
            {
                bPhoto = (byte[])GetSession("Photo");
            }

            aspnet_Membership editedTeacher = new aspnet_Membership();
            editedTeacher.UserId = teacherBL.GetTeacher(UserId).UserId;
            teacherBL.UpdateTeacher(editedTeacher, strTeacherName, bGender, (DateTime)dtDateOfBirth, strAddress, strPhone, bPhoto);

            BackToPrevPage();
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            BackToPrevPage();
        }

        protected void BtnUpload_Click(object sender, ImageClickEventArgs e)
        {
            if (FileUploadLogo.PostedFile != null)
            {
                //To create a PostedFile
                HttpPostedFile File = FileUploadLogo.PostedFile;

                //Create byte Array with file len
                byte[] Data = new Byte[File.ContentLength];
                //force the control to load data in array
                File.InputStream.Read(Data, 0, File.ContentLength);

                AddSession("Photo", Data);
                string filename = Path.GetFileName(FileUploadLogo.FileName);
                FileUploadLogo.SaveAs(Server.MapPath("~/upload/temp/") + filename);
                filename = "/upload/temp/" + filename;
                ImgPhoto.ImageUrl = filename;
            }
        }
        #endregion

        #region Methods
        protected void DateOfBirthCustomValidator_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            DateTime dtDateOfBirth;
            e.IsValid = DateTime.TryParse(e.Value, out dtDateOfBirth);
        }

        private void FillGiaoVien(Guid teacherId)
        {
            aspnet_User teacher = teacherBL.GetTeacher(teacherId);
            LblUserIdHienThi.Text = teacher.UserName.Split('_')[1];
            TxtTenGiaoVien.Text = teacher.aspnet_Membership.FullName;
            if (teacher.aspnet_Membership.Birthday != null)
            {
                TxtNgaySinh.Text = ((DateTime)teacher.aspnet_Membership.Birthday).ToString(AppConstant.DATEFORMAT_DDMMYYYY);
            }
            if (teacher.aspnet_Membership.Gender != null)
            {
                RbtnNam.Checked = (bool)teacher.aspnet_Membership.Gender;
                RbtnNu.Checked = !(bool)teacher.aspnet_Membership.Gender;
            }

            TxtDiaChi.Text = teacher.aspnet_Membership.Address;
            TxtDienThoai.Text = (teacher.aspnet_Membership.Phone != "") ? teacher.aspnet_Membership.Phone : "(không có)";

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("~/modules/danh_muc/giao_vien/UserPhotoLoadingHandler.ashx");
            stringBuilder.Append("?id=");
            stringBuilder.Append(teacher.UserId);
            ImgPhoto.ImageUrl = stringBuilder.ToString();
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