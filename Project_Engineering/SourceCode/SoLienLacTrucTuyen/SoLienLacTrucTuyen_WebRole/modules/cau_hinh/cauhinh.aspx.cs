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
    public partial class ConfigurationPage : BaseContentPage
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
                FillConfiguration();
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(AppConstant.PAGEPATH_CONFIGURATION_MODIFY);
        }
        #endregion

        #region Methods
        private void FillConfiguration()
        {
            ImgLogo.ImageUrl = string.Format("~/modules/supplier/PhotoLoadingHandler.ashx?id={0}", UserSchool.SchoolId);            
            LblSchoolName.Text = UserSchool.SchoolName;
            LblAddress.Text = UserSchool.Address;
            LblEmail.Text = UserSchool.Email;
            LblPhone.Text = UserSchool.Phone;
            LblDistrict.Text = UserSchool.Configuration_District.DistrictName;
            LblProvince.Text = UserSchool.Configuration_District.Configuration_Province.ProvinceName;
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