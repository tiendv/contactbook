using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class GVCN_User : System.Web.UI.Page
    {
        private HocSinhBL hocSinhBL;
        private int maHocSinh;

        protected void Page_Load(object sender, EventArgs e)
        {
            Site masterPage = (Site)Page.Master;
            masterPage.UserRole = (new UserBL()).GetRoleId(User.Identity.Name);
            masterPage.PageUrl = Page.Request.Path;
            masterPage.PageTitle = "Giáo Viên Chủ Nhiệm";

            hocSinhBL = new HocSinhBL();
            maHocSinh = hocSinhBL.GetMaHocSinh(masterPage.UserNameSession);

            if (!Page.IsPostBack)
            {
                BindDropDownListNamHoc();
                FillGVCN(maHocSinh);
            }
        }
        
        private void BindDropDownListNamHoc()
        {
            List<CauHinh_NamHoc> lstNamHoc = hocSinhBL.GetListNamHoc(maHocSinh);
            DdlNamHoc.DataSource = lstNamHoc;
            DdlNamHoc.DataValueField = "MaNamHoc";
            DdlNamHoc.DataTextField = "TenNamHoc";
            DdlNamHoc.DataBind();
        }

        private void FillGVCN(int maHocSinh)
        {
            int maNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);
            HocSinh_HocSinhLopHoc hocSinhLopHoc = hocSinhBL.GetHocSinhLopHoc(maNamHoc, maHocSinh);
            if (hocSinhLopHoc != null)
            {
                GVCNInfo gvcn = (new GVCNBL()).GetGVCNInfo(hocSinhLopHoc.MaLopHoc, "");
                LblHoTen.Text = gvcn.TenGVCN;
                LblNgaySinh.Text = gvcn.NgaySinh.ToShortDateString();
                LblGioiTinh.Text = (gvcn.GioiTinh) ? "Nam" : "Nữ";
                LblDiaChi.Text = gvcn.DiaChi;
                LblDienThoai.Text = gvcn.DienThoai;
            }                
        }

        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            FillGVCN(maHocSinh);
        }
    }
}