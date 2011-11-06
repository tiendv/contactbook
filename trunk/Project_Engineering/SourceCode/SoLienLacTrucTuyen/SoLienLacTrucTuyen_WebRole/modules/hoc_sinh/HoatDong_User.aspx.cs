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
    public partial class HoatDong_User : System.Web.UI.Page
    {
        #region Fields
        private HocSinhBL hocSinhBL;
        private HoatDongBL hoatDongBL;

        private int maHocSinh;
        private bool isSearch;
        #endregion

        #region Page event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            Site masterPage = (Site)Page.Master;
            masterPage.UserRole = (new UserBL()).GetRoleId(User.Identity.Name);
            masterPage.PageUrl = Page.Request.Path;
            masterPage.PageTitle = "Hoạt Động";

            hocSinhBL = new HocSinhBL();
            hoatDongBL = new HoatDongBL();
            maHocSinh = hocSinhBL.GetMaHocSinh(masterPage.UserNameSession);

            if (!Page.IsPostBack)
            {
                BindDropDownListNamHoc();
                BindDropDownListHocKy();

                BindDropDownListNamHoc();
                BindDropDownListHocKy();
                InitDates();
                isSearch = false;
                BindRepeater();
            }
        }               
        #endregion

        private void BindDropDownListNamHoc()
        {
            NamHocBL namHocBL = new NamHocBL();
            List<CauHinh_NamHoc> lstNamHoc = namHocBL.GetListNamHoc();
            DdlNamHoc.DataSource = lstNamHoc;
            DdlNamHoc.DataValueField = "MaNamHoc";
            DdlNamHoc.DataTextField = "TenNamHoc";
            DdlNamHoc.DataBind();

            DdlNamHoc.SelectedValue = (new SystemConfigBL()).GetCurrentYear().ToString();
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
            List<TabularHoatDong> lstTabularHoatDong = hoatDongBL.GetListTabularHoatDong(maHocSinh, maNamHoc, maHocKy,
                tuNgay, DenNgay, MainDataPager.CurrentIndex, MainDataPager.PageSize,
                out totalRecords);

            if (totalRecords != 0 && lstTabularHoatDong.Count == 0)
            {
                MainDataPager.CurrentIndex--;
                BindRepeater();
                return;
            }

            bool bDisplayData = (lstTabularHoatDong.Count != 0) ? true : false;
            ProcessDislayInfo(bDisplayData);
            RptHoatDong.DataSource = lstTabularHoatDong;
            RptHoatDong.DataBind();
            MainDataPager.ItemCount = totalRecords;
        }

        private void ProcessDislayInfo(bool bDisplayData)
        {
            PnlPopupDetail.Visible = bDisplayData;
            RptHoatDong.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin hoạt động";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy hoạt động";
                }
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
            }
            else
            {
                MainDataPager.Visible = true;
            }
        }

        protected void RptHoatDong_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Control control = e.Item.FindControl("HdfRptMaHoatDong");
                if (control != null)
                {
                    //int maNgayNghiHoc = Int32.Parse(((HiddenField)control).Value);
                    //if (ngayNghiHocBL.IsXacNhan(maNgayNghiHoc))
                    //{
                    //    ImageButton btnDeleteItem = (ImageButton)e.Item.FindControl("BtnDeleteItem");
                    //    btnDeleteItem.ImageUrl = "~/Styles/Images/button_delete_disable.png";
                    //    btnDeleteItem.Enabled = false;

                    //    ImageButton btnEditItem = (ImageButton)e.Item.FindControl("BtnEditItem");
                    //    btnEditItem.ImageUrl = "~/Styles/Images/button_edit_disable.png";
                    //    btnEditItem.Enabled = false;
                    //}
                }
            }
        }

        protected void RptHoatDong_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDetailItem":
                    {
                        int maHoatDong = Int32.Parse(e.CommandArgument.ToString());
                        HocSinh_HoatDong hoatDong = hoatDongBL.GetHoatDong(maHoatDong);

                        this.LblTieuDe.Text = hoatDong.TieuDe;
                        this.LblMoTa.Text = hoatDong.NoiDung;
                        this.LblHocKy.Text = (new HocKyBL()).GetHocKy(hoatDong.MaHocKy).TenHocKy;
                        this.LblNgay.Text = hoatDong.Ngay.ToShortDateString();
                        this.LblThaiDoThamGia.Text = (hoatDong.MaThaiDoThamGia != null) ?
                            (new ThaiDoThamGiaBL()).GetThaiDoThamGia((int)hoatDong.MaThaiDoThamGia).TenThaiDoThamGia: "Không xác định";

                        ModalPopupExtender mPEDetail = (ModalPopupExtender)e.Item.FindControl("MPEDetail");
                        mPEDetail.Show();

                        this.HdfMaHoatDong.Value = e.CommandArgument.ToString();
                        this.HdfRptHoatDongMPEEdit.Value = mPEDetail.ClientID;

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            MainDataPager.CurrentIndex = currentPageIndex;
            BindRepeater();
        }

        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            isSearch = true;
            BindRepeater();
        }
    }
}