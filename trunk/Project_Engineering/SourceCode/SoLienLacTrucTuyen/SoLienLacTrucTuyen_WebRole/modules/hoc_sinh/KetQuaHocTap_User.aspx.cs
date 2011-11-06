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
using System.Web.UI.HtmlControls;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class KetQuaHocTap_User : System.Web.UI.Page
    {
        #region Fields
        private HocSinhBL hocSinhBL;
        private MarkTypeBL loaiDiemBL;
        private KetQuaHocTapBL ketQuaHocTapBL;
        private int maHocSinh;
        #endregion

        #region Page event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            Site masterPage = (Site)Page.Master;
            masterPage.UserRole = (new UserBL()).GetRoleId(User.Identity.Name);
            masterPage.PageUrl = Page.Request.Path;
            masterPage.PageTitle = "Kết Quả Học Tập";

            hocSinhBL = new HocSinhBL();
            loaiDiemBL = new MarkTypeBL();
            ketQuaHocTapBL = new KetQuaHocTapBL();
            maHocSinh = hocSinhBL.GetMaHocSinh(masterPage.UserNameSession);

            if (!Page.IsPostBack)
            {
                BindDropDownListNamHoc();
                BindDropDownListHocKy();

                BindRepeaterTenLoaiDiem();
                BindRepeaterKetQuaHocTap();
                BindRepeaterDanhHieu();
            }
        }
        #endregion
        
        private void BindDropDownListNamHoc()
        {
            List<CauHinh_NamHoc> lstNamHoc = hocSinhBL.GetListNamHoc(maHocSinh);
            DdlNamHoc.DataSource = lstNamHoc;
            DdlNamHoc.DataValueField = "MaNamHoc";
            DdlNamHoc.DataTextField = "TenNamHoc";
            DdlNamHoc.DataBind();
        }

        private void BindDropDownListHocKy()
        {
            HocKyBL hocKyBL = new HocKyBL();
            List<CauHinh_HocKy> lstHocKy = hocKyBL.GetListHocKy();
            DdlHocKy.DataSource = lstHocKy;
            DdlHocKy.DataValueField = "MaHocKy";
            DdlHocKy.DataTextField = "TenHocKy";
            DdlHocKy.DataBind();

            DdlHocKy.SelectedValue = (new SystemConfigBL()).GetMaHocKyHienHanh().ToString();
        }

        private void BindRepeaterKetQuaHocTap()
        {
            int maNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);
            int maHocKy = Int32.Parse(DdlHocKy.SelectedValue);

            SubjectBL monHocBL = new SubjectBL();
            ThoiKhoaBieuBL monHocTKBBL = new ThoiKhoaBieuBL();

            double totalRecords;
            List<TabularKetQuaMonHoc> lstTabularKetQuaMonHoc = ketQuaHocTapBL.GetListTabularKetQuaMonHoc(maNamHoc, maHocKy, maHocSinh,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out totalRecords);

            // Decrease page current index when delete
            if (lstTabularKetQuaMonHoc.Count == 0 && totalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRepeaterKetQuaHocTap();
                return;
            }

            bool bDisplayData = (lstTabularKetQuaMonHoc.Count != 0) ? true : false;
            RptKetQuaDiem.Visible = bDisplayData;
            MainDataPager.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            this.RptKetQuaDiem.DataSource = lstTabularKetQuaMonHoc;
            this.RptKetQuaDiem.DataBind();
            MainDataPager.ItemCount = totalRecords;
        }

        private void BindRepeaterTenLoaiDiem()
        {
            List<DanhMuc_LoaiDiem> lstLoaiDiem = loaiDiemBL.GetListMarkTypes();
            this.RptTenLoaiDiem.DataSource = lstLoaiDiem;
            this.RptTenLoaiDiem.DataBind();
        }

        protected void RptKetQuaDiem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Control control = e.Item.FindControl("RptKetQuaDiem_HdfMaDiemMonHK");
                if (control != null)
                {
                    int maDiemMonHK = Int32.Parse(((HiddenField)control).Value);
                    List<StrDiemMonHocLoaiDiem> lstStrDiemMonHocLoaiDiem = ketQuaHocTapBL.GetListStringDiemMonHoc(maDiemMonHK);
                    Repeater rptDiemMonHoc = (Repeater)e.Item.FindControl("RptKetQuaDiem_RptDiemMonHoc");
                    rptDiemMonHoc.DataSource = lstStrDiemMonHocLoaiDiem;
                    rptDiemMonHoc.DataBind();
                }
            }
        }

        protected void RptKetQuaDiem_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            
        }

        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            BindRepeaterKetQuaHocTap();
            BindRepeaterDanhHieu();
        }

        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currentPageIndex;
            BindRepeaterKetQuaHocTap();
        }

        private void BindRepeaterDanhHieu()
        {
            
            int maNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);

            double totalRecords;
            List<TabularTermStudentResult> lstTabularDanhHieuHocSinh = ketQuaHocTapBL.GetListTabularDanhHieuHocSinh(
                maHocSinh, maNamHoc,
                DataPagerDanhHieu.CurrentIndex, DataPagerDanhHieu.PageSize,
                out totalRecords);

            RptDanhHieu.DataSource = lstTabularDanhHieuHocSinh;
            RptDanhHieu.DataBind();
            DataPagerDanhHieu.ItemCount = totalRecords;

        }

        protected void RptDanhHieu_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //Control control = e.Item.FindControl("RptKetQuaDiem_HdfMaDiemMonHK");
                //if (control != null)
                //{
                //    int maDiemMonHK = Int32.Parse(((HiddenField)control).Value);
                //    List<StrDiemMonHocLoaiDiem> lstStrDiemMonHocLoaiDiem = ketQuaHocTapBL.GetListStringDiemMonHoc(maDiemMonHK);
                //    Repeater rptDiemMonHoc = (Repeater)e.Item.FindControl("RptKetQuaDiem_RptDiemMonHoc");
                //    rptDiemMonHoc.DataSource = lstStrDiemMonHocLoaiDiem;
                //    rptDiemMonHoc.DataBind();
                //}
            }
        }
    }
}