using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;
using AjaxControlToolkit;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class ThoiKhoaBieu_User : System.Web.UI.Page
    {
        #region Fields
        private ThoiKhoaBieuBL monHocTKBBL;
        private HocSinhBL hocSinhBL;
        private int maHocSinh;
        private int maLopHoc;
        #endregion

        #region Page event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            Site masterPage = (Site)Page.Master;
            masterPage.UserRole = (new UserBL()).GetRoleId(User.Identity.Name);
            masterPage.PageUrl = Page.Request.Path;
            masterPage.PageTitle = "Thời Khóa Biểu";

            monHocTKBBL = new ThoiKhoaBieuBL();
            hocSinhBL = new HocSinhBL();
            maHocSinh = hocSinhBL.GetMaHocSinh(masterPage.UserNameSession);
            
            if (!Page.IsPostBack)
            {
                BindDropDownLists();
                BindRepeaterMonHocTKB();
            }

            if (DdlNamHoc.Items.Count != 0)
            {
                maLopHoc = hocSinhBL.GetMaLopHoc(Int32.Parse(DdlNamHoc.Items[0].Value), maHocSinh);
            }
        }
        #endregion

        #region Repeater event handlers
        protected void RptMonHocTKB_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.DataItem != null)
                {
                    ThoiKhoaBieuTheoThu monHocTKBThuInfo = (ThoiKhoaBieuTheoThu)e.Item.DataItem;
                    int maNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);
                    int maHocKy = Int32.Parse(DdlHocKy.SelectedValue);
                    int maThu = monHocTKBThuInfo.MaThu;
                    
                    Label lblNghiSang = (Label)e.Item.FindControl("LblNghiSang");                    
                    if (monHocTKBThuInfo.ListThoiKhoaBieuTheoBuoi[0].ListMonHocTKBBuoi.Count == 0)
                    {
                        lblNghiSang.Visible = true;
                    }
                    else
                    {                        
                        lblNghiSang.Visible = false;
                        List<MonHocInfo> lstMonHocTKBInfo = monHocTKBBL.GetMonHocTKBInfo(maNamHoc, maHocKy, maThu, 1, maLopHoc);
                        Repeater rptMonHocTKBBuoiSang = (Repeater)e.Item.FindControl("RptMonHocTKBBuoiSang");
                        rptMonHocTKBBuoiSang.DataSource = lstMonHocTKBInfo;
                        rptMonHocTKBBuoiSang.DataBind();
                    }

                    Label lblNghiChieu = (Label)e.Item.FindControl("LblNghiChieu");
                    if (monHocTKBThuInfo.ListThoiKhoaBieuTheoBuoi[1].ListMonHocTKBBuoi.Count == 0)
                    {
                        lblNghiChieu.Visible = true;
                    }
                    else
                    {
                        lblNghiChieu.Visible = false;                        
                        List<MonHocInfo> lstMonHocTKBInfo = monHocTKBBL.GetMonHocTKBInfo(maNamHoc, maHocKy, maThu, 2, maLopHoc);
                        Repeater rptMonHocTKBBuoiChieu = (Repeater)e.Item.FindControl("RptMonHocTKBBuoiChieu");
                        rptMonHocTKBBuoiChieu.DataSource = lstMonHocTKBInfo;
                        rptMonHocTKBBuoiChieu.DataBind();
                    }
                }
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            BindRepeaterMonHocTKB();
        }
        #endregion

        #region Methods
        private void BindRepeaterMonHocTKB()
        {
            int maNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);
            int maHocKy = Int32.Parse(DdlHocKy.SelectedValue);

            List<ThoiKhoaBieuTheoThu> lstMonHocTKBThuInfo = monHocTKBBL.GetThoiKhoaBieu(maNamHoc, maHocKy, maLopHoc);
            RptMonHocTKB.DataSource = lstMonHocTKBThuInfo;
            RptMonHocTKB.DataBind();
        }

        private void ProcessDislayInfo(bool bDisplayData)
        {
            
        }

        private void BindDropDownLists()
        {
            BindDropDownListNamHoc();
            BindDropDownListHocKy();
        }

        private void BindDropDownListNamHoc()
        {
            List<CauHinh_NamHoc> lstNamHoc = hocSinhBL.GetListNamHoc(maHocSinh);
            DdlNamHoc.DataSource = lstNamHoc;
            DdlNamHoc.DataValueField = "MaNamHoc";
            DdlNamHoc.DataTextField = "TenNamHoc";
            DdlNamHoc.DataBind();

            if (DdlNamHoc.Items.Count != 0)
            {
                maLopHoc = hocSinhBL.GetMaLopHoc(Int32.Parse(DdlNamHoc.Items[0].Value), maHocSinh);
            }
        }

        private void BindDropDownListHocKy()
        {
            HocKyBL hocKyBL = new HocKyBL();
            List<CauHinh_HocKy> lstHocKy = hocKyBL.GetListHocKy();
            DdlHocKy.DataSource = lstHocKy;
            DdlHocKy.DataValueField = "MaHocKy";
            DdlHocKy.DataTextField = "TenHocKy";
            DdlHocKy.DataBind();

            CauHinhHeThongBL cauHinhHeThongBL = new CauHinhHeThongBL();
            DdlHocKy.SelectedValue = cauHinhHeThongBL.GetMaHocKyHienHanh().ToString();
        }
        #endregion   
    }
}