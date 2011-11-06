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
    public partial class TinhHinhNghiHoc_User : System.Web.UI.Page
    {
        #region Fields
        private HocSinhBL hocSinhBL;
        private NgayNghiHocBL ngayNghiHocBL;
        private BuoiBL buoiBL;

        private int maHocSinh;
        private bool isSearch;
        #endregion

        #region Page event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            Site masterPage = (Site)Page.Master;
            masterPage.UserRole = (new UserBL()).GetRoleId(User.Identity.Name);
            masterPage.PageUrl = Page.Request.Path;
            masterPage.PageTitle = "Tình Hình Nghỉ Học";

            hocSinhBL = new HocSinhBL();
            ngayNghiHocBL = new NgayNghiHocBL();
            buoiBL = new BuoiBL();

            maHocSinh = hocSinhBL.GetMaHocSinh(masterPage.UserNameSession);

            if (!Page.IsPostBack)
            {
                BindDropDownListNamHoc();
                BindDropDownListHocKy();
                InitDates();
                isSearch = false;
                BindRepeater();
            }
        }
        #endregion

        #region Methods
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

        private void InitDates()
        {
            DateTime today = DateTime.Now;
            DateTime beginDateOfMonth = new DateTime(today.Year, today.Month, 1);
            TxtTuNgay.Text = beginDateOfMonth.ToShortDateString();
            DateTime dateOfNextMonth = today.AddMonths(1);
            DateTime beginDateOfNextMonth = new DateTime(dateOfNextMonth.Year, dateOfNextMonth.Month, 1);
            DateTime endDateOfMonth = beginDateOfNextMonth.AddDays(-1);
            TxtDenNgay.Text = endDateOfMonth.ToShortDateString();
        }

        private void BindRepeater()
        {
            int maNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);
            int maHocKy = Int32.Parse(DdlHocKy.SelectedValue);
            DateTime tuNgay = DateTime.Parse(TxtTuNgay.Text);
            DateTime DenNgay = DateTime.Parse(TxtDenNgay.Text);

            double totalRecords;
            List<TabularDayOff> lstTabularNgayNghiHoc = ngayNghiHocBL.GetListTabularNgayNghiHoc(
                maHocSinh, maNamHoc, maHocKy,
                tuNgay, DenNgay,
                MainDataPager.CurrentIndex, MainDataPager.PageSize,
                out totalRecords);

            if(totalRecords != 0 && lstTabularNgayNghiHoc.Count == 0)
            {
                MainDataPager.CurrentIndex--;
                BindRepeater();
                return;
            }

            bool bDisplayData = (lstTabularNgayNghiHoc.Count != 0) ? true : false;
            ProcessDislayInfo(bDisplayData);
            RptNgayNghi.DataSource = lstTabularNgayNghiHoc;
            RptNgayNghi.DataBind();
            MainDataPager.ItemCount = totalRecords;
        }

        private void ProcessDislayInfo(bool bDisplayData)
        {
            PnlPopupConfirm.Visible = bDisplayData;
            RptNgayNghi.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin ngày nghỉ học";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy ngày nghỉ học";
                }
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
            }
            else
            {
                MainDataPager.Visible = true;
            }
        }
        #endregion

        #region Repeater event handlers
        protected void RptNgayNghi_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Control control = e.Item.FindControl("HdfRptMaNgayNghiHoc");
                if (control != null)
                {
                    int maNgayNghiHoc = Int32.Parse(((HiddenField)control).Value);
                    if (ngayNghiHocBL.Confirmed(maNgayNghiHoc))
                    {
                        ImageButton btnConfirmItem = (ImageButton)e.Item.FindControl("BtnConfirmItem");
                        btnConfirmItem.ImageUrl = "~/Styles/Images/button_confirm_disable.png";
                        btnConfirmItem.Enabled = false;
                    }
                }
            }
        }

        protected void RptNgayNghi_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdConfirmItem":
                    {
                        int maNgayNghiHoc = Int32.Parse(e.CommandArgument.ToString());
                        HocSinh_NgayNghiHoc ngayNghi = ngayNghiHocBL.GetNgayNghiHoc(maNgayNghiHoc);
                        LblConfirm.Text = "Bạn có chắc xác nhận ngày nghỉ <b>" + ngayNghi.Ngay.ToShortDateString() + "</b> này không?";

                        ModalPopupExtender mPEConfirm = (ModalPopupExtender)e.Item.FindControl("MPEConfirm");
                        mPEConfirm.Show();

                        this.HdfMaNgayNghiHoc.Value = e.CommandArgument.ToString();
                        this.HdfRptNgayNghiMPEConfirm.Value = mPEConfirm.ClientID;

                        break;
                    }                
                default:
                    {
                        break;
                    }
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            isSearch = true;
            BindRepeater();
        }        

        protected void BtnConfirm_Click(object sender, ImageClickEventArgs e)
        {
            int maNgayNghiHoc = Int32.Parse(this.HdfMaNgayNghiHoc.Value);

            ngayNghiHocBL.ConfirmNgayNghiHoc(maNgayNghiHoc);

            MainDataPager.CurrentIndex = 1;
            BindRepeater();
        }
        #endregion

        #region Pager event handlers
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            MainDataPager.CurrentIndex = currentPageIndex;
            BindRepeater();
        }
        #endregion
    }
}