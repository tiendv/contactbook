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
    public partial class ChiTietLopHoc : System.Web.UI.Page
    {
        #region Fields
        private LopHocBL lopHocBL;
        #endregion

        #region Page event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            lopHocBL = new LopHocBL();

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
                int? maLopHoc = GetQueryString();
                if (maLopHoc != null)
                {
                    LopHoc_Lop lophoc = lopHocBL.GetLopHoc((int)maLopHoc);
                    if (lophoc != null)
                    {
                        LblTenLopHocChiTiet.Text = lophoc.TenLopHoc;
                        LblTenNganhHocChiTiet.Text = (new FacultyBL()).GetNganhHoc(lophoc.MaNganhHoc).TenNganhHoc;
                        LblTenKhoiLopChiTiet.Text = (new KhoiLopBL()).GetKhoiLop(lophoc.MaKhoiLop).TenKhoiLop;
                        LblSiSoChiTiet.Text = lophoc.SiSo.ToString();
                    }
                }
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            
        }

        protected void BtnBackPrevPage_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("danhsachlop.aspx");
        }
        #endregion

        private int? GetQueryString()
        {
            if (Request.QueryString["malop"] != null)
            {
                int iMaLopHoc;
                bool bParseSuccess = Int32.TryParse(Request.QueryString["malop"], out iMaLopHoc);
                if (bParseSuccess == true)
                {
                    return iMaLopHoc;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}