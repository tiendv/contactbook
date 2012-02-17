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
    public partial class SchoolDetailPage : BaseContentPage
    {
        #region Field(s)
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
                if (RetrieveSessions() == false)
                {
                    Response.Redirect(AppConstant.PAGEPATH_SCHOOLLIST);
                }

                GetSearchedSessions();
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            
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
                LblSchoolName.Text = school.SchoolName;
                LblAddress.Text = school.Address;
                LblEmail.Text = school.Email;
                LblPhone.Text = school.Phone;
                LblDistrict.Text = school.DistrictId.ToString();
                LblProvince.Text = school.Configuration_District.ProvinceId.ToString();
                ViewState[AppConstant.VIEWSTATE_SCHOOLID] = school.SchoolId;
                ViewState[AppConstant.VIEWSTATE_SCHOOLNAME] = school.SchoolName;
                ViewState[AppConstant.VIEWSTATE_EMAIL] = school.Email;
                
                return true;
            }

            return false;
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