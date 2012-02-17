using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class ModifySchoolPage : BaseContentPage
    {
        #region Field(s)
        public int schoolId;
        SchoolBL schoolBL;
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

            schoolBL = new SchoolBL();

            if (!Page.IsPostBack)
            {
                BindDDLProvinces();
                BindDDLDistricts();
                if (RetrieveSessions() == false)
                {
                    Response.Redirect(AppConstant.PAGEPATH_SCHOOLLIST);
                }

                GetSearchedSessions();
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            SchoolBL schoolBL = null;
            School_School lastedInsertedSchool = null;
            School_School supplier = null;

            if (ValidateInputs())
            {
                Configuration_District district = new Configuration_District();
                district.DistrictId = Int32.Parse(DdlDistricts.SelectedValue);
                string strSchoolName = TxtSchoolName.Text.Trim();
                string strAddress = TxtAddress.Text.Trim();
                string strEmail = TxtEmail.Text.Trim();
                string strPhone = TxtPhone.Text.Trim();
                byte[] bLogo = (byte[]) GetSession("Image");
                string strEmailPassword = Membership.GeneratePassword(Membership.Provider.MinRequiredPasswordLength,
                    Membership.Provider.MinRequiredNonAlphanumericCharacters);

                // insert new school and then return it generated id
                schoolBL = new SchoolBL();
                School_School school = new School_School();
                school.SchoolId = (int)ViewState[AppConstant.VIEWSTATE_SCHOOLID];
                schoolBL.UpdateSchool(school, district, strSchoolName, strAddress, strPhone, strEmail, strEmailPassword, bLogo);

                // send confirmation mail to school
                //supplier = schoolBL.GetSupplier();
                //string strEmailContent = string.Format("Account: {0}, Password: {1}", lastedInsertedSchool.Email, lastedInsertedSchool.Password);
                //MailBL.SendByGmail(supplier.Email, lastedInsertedSchool.Email, "Thông báo tạo thông tin trường thành công",
                //    strEmailContent, supplier.Password);

                // redirect back to previous page
                BackToPrevPage();
            }
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

                AddSession("Image", Data);
                string filename = Path.GetFileName(FileUploadLogo.FileName);
                FileUploadLogo.SaveAs(Server.MapPath("~/upload/temp/") + filename);
                filename = "/upload/temp/" + filename;
                ImgLogo.ImageUrl = filename;
            }
        }
        #endregion

        #region DropDownList event hanlders
        protected void DdlProvinces_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLDistricts();
        }
        #endregion

        #region Methods
        private bool RetrieveSessions()
        {
            if (CheckSessionKey(AppConstant.SESSION_SELECTED_SCHOOL))
            {
                School_School school = (School_School)GetSession(AppConstant.SESSION_SELECTED_SCHOOL);
                ImgLogo.ImageUrl = string.Format("~/modules/supplier/PhotoLoadingHandler.ashx?id={0}", school.SchoolId); 
                RemoveSession(AppConstant.SESSION_SELECTED_SCHOOL);

                school = schoolBL.GetSchool(school.SchoolId);
                schoolId = school.SchoolId;
                TxtSchoolName.Text = school.SchoolName;
                TxtAddress.Text = school.Address;
                TxtEmail.Text = school.Email;
                TxtPhone.Text = school.Phone;
                DdlDistricts.SelectedValue = school.DistrictId.ToString();
                DdlProvinces.SelectedValue = school.Configuration_District.ProvinceId.ToString();
                ViewState[AppConstant.VIEWSTATE_SCHOOLID] = school.SchoolId;
                ViewState[AppConstant.VIEWSTATE_SCHOOLNAME] = school.SchoolName;
                ViewState[AppConstant.VIEWSTATE_EMAIL] = school.Email;
                
                return true;
            }

            return false;
        }

        private bool ValidateInputs()
        {
            if (CheckUntils.IsNullOrBlank(DdlDistricts.Text.Trim()))
            {
                DistrictRequired.IsValid = false;
                return false;
            }

            if (CheckUntils.IsNullOrBlank(TxtSchoolName.Text))
            {
                SchoolNameRequired.IsValid = false;
                return false;
            }

            if (CheckUntils.IsNullOrBlank(TxtAddress.Text))
            {
                AddressRequired.IsValid = false;
                return false;
            }

            if (CheckUntils.IsNullOrBlank(TxtPhone.Text))
            {
                PhoneRequired.IsValid = false;
                return false;
            }

            if (CheckUntils.IsNullOrBlank(TxtEmail.Text))
            {
                EmailRequired.IsValid = false;
                return false;
            }

            if (Regex.IsMatch(FileUploadLogo.FileName, FileUpLoadValidator.ValidationExpression))
            {
                FileUpLoadValidator.IsValid = false;
                return false;
            }

            if (DdlDistricts.Items.Count != 0)
            {
                Configuration_District district = new Configuration_District();
                district.DistrictId = Int32.Parse(DdlDistricts.SelectedValue);
                if (schoolBL.SchoolNameExists(district, (string)ViewState[AppConstant.VIEWSTATE_SCHOOLNAME], TxtSchoolName.Text.Trim()))
                {
                    SchoolNameCustomValidator.IsValid = false;
                    return false;
                }
                else
                {
                    SchoolNameCustomValidator.IsValid = true;
                }
            }

            if (MailBL.CheckEmailExist(TxtEmail.Text))
            {
                EmailCustomValidator.IsValid = true;
            }
            else
            {
                EmailCustomValidator.IsValid = false;
                return false;
            }

            return true;
        }

        protected void SchoolNameCustomValidator_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            if (DdlDistricts.Items.Count != 0)
            {
                Configuration_District district = new Configuration_District();
                district.DistrictId = Int32.Parse(DdlDistricts.SelectedValue);
                if (schoolBL.SchoolNameExists(district, (string)ViewState[AppConstant.VIEWSTATE_SCHOOLNAME], TxtSchoolName.Text.Trim()))
                {
                    e.IsValid = false;
                }
                else
                {
                    e.IsValid = true;
                }
            }
        }

        protected void EmailCustomValidator_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            if (MailBL.CheckEmailExist(TxtEmail.Text))
            {
                e.IsValid = true;
            }
            else
            {
                e.IsValid = false;
            }
        }

        private void BackToPrevPage()
        {
            SaveSearchedSessions();
            Response.Redirect(AppConstant.PAGEPATH_SCHOOLLIST);
        }

        private void SaveSearchedSessions()
        {
            AddSession(AppConstant.SESSION_SELECTED_PROVINCE, ViewState[AppConstant.VIEWSTATE_SELECTED_PROVINCEID]);
            AddSession(AppConstant.SESSION_SELECTED_DISTRICT, ViewState[AppConstant.VIEWSTATE_SELECTED_DISTRICTID]);
            AddSession(AppConstant.SESSION_SELECTED_SCHOOLNAME, ViewState[AppConstant.VIEWSTATE_SEARCHED_SCHOOLNAME]);
        }

        private void GetSearchedSessions()
        {
            if (CheckSessionKey(AppConstant.SESSION_SELECTED_PROVINCE)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_DISTRICT)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_SCHOOLNAME))
            {
                ViewState[AppConstant.VIEWSTATE_SELECTED_PROVINCEID] = (Int32)GetSession(AppConstant.SESSION_SELECTED_PROVINCE);
                RemoveSession(AppConstant.SESSION_SELECTED_PROVINCE);

                ViewState[AppConstant.VIEWSTATE_SELECTED_DISTRICTID] = (Int32)GetSession(AppConstant.SESSION_SELECTED_DISTRICT);
                RemoveSession(AppConstant.SESSION_SELECTED_DISTRICT);

                ViewState[AppConstant.VIEWSTATE_SEARCHED_SCHOOLNAME] = (string)GetSession(AppConstant.SESSION_SELECTED_SCHOOLNAME);
                RemoveSession(AppConstant.SESSION_SELECTED_SCHOOLNAME);
            }
        }

        private void BindDDLProvinces()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<Configuration_Province> provinces = systemConfigBL.GetProvinces();
            DdlProvinces.DataSource = provinces;
            DdlProvinces.DataValueField = "ProvinceId";
            DdlProvinces.DataTextField = "ProvinceName";
            DdlProvinces.DataBind();
        }

        private void BindDDLDistricts()
        {
            if (DdlProvinces.Items.Count != 0)
            {
                Configuration_Province province = new Configuration_Province();
                province.ProvinceId = Int32.Parse(DdlProvinces.SelectedValue);

                SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
                List<Configuration_District> districts = systemConfigBL.GetDistricts(province);
                DdlDistricts.DataSource = districts;
                DdlDistricts.DataValueField = "DistrictId";
                DdlDistricts.DataTextField = "DistrictName";
                DdlDistricts.DataBind();
            }
        }

        private byte[] ImageToByteArray(System.Drawing.Image image)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                return memoryStream.ToArray();
            }
        }
        #endregion
    }
}