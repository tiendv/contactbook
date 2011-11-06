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
    public partial class ChiTietLopHoc : BaseContentPage
    {
        #region Fields
        private LopHocBL lopHocBL;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            // Check user's accessibility
            base.Page_Load(sender, e);
            if (isAccessDenied)
            {
                return;
            }

            lopHocBL = new LopHocBL();           

            if (!Page.IsPostBack)
            {
                int? maLopHoc = GetQueryString();
                if (maLopHoc != null)
                {
                    LopHoc_Lop lophoc = lopHocBL.GetLopHoc((int)maLopHoc);
                    if (lophoc != null)
                    {                        
                        LblTenLopHocChiTiet.Text = lophoc.TenLopHoc;
                        LblTenNganhHocChiTiet.Text = lophoc.DanhMuc_NganhHoc.TenNganhHoc;
                        LblTenKhoiLopChiTiet.Text = lophoc.DanhMuc_KhoiLop.TenKhoiLop;
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