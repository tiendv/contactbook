using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessEntity;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;
using AjaxControlToolkit;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class LoiNhanKhan_User : System.Web.UI.Page
    {
        private StudentBL hocSinhBL;
        private LoiNhanKhanBL loiNhanKhanBL;
        private int maHocSinh;
        private string maHocSinhHienThi;
        private bool isSearch;

        protected void Page_Load(object sender, EventArgs e)
        {
            Site masterPage = (Site)Page.Master;
            masterPage.UserRole = (new UserBL()).GetRoleId(User.Identity.Name);
            masterPage.PageUrl = Page.Request.Path;
            masterPage.PageTitle = "Lời Nhắn Khẩn";

            hocSinhBL = new StudentBL();
            loiNhanKhanBL = new LoiNhanKhanBL();
            HocSinh_ThongTinCaNhan student = hocSinhBL.GetStudent(User.Identity.Name);
            maHocSinh = student.MaHocSinh;
            isSearch = false;            
            maHocSinhHienThi = masterPage.UserNameSession;

            if (!Page.IsPostBack)
            {
                isSearch = false;
                BindDropDownListNamHoc();
                BindDropDownListXacNhan();
                InitDates();
                BindRepeater();
            }
        }

        #region Methods
        private void BindRepeater()
        {
            int maNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);
            DateTime tuNgay = DateTime.Parse(TxtTuNgay.Text);
            DateTime denNgay = DateTime.Parse(TxtDenNgay.Text);
            int xacNhan = Int32.Parse(DdlXacNhan.SelectedValue);

            double totalRecords;
            List<TabularLoiNhanKhan> lstTabularLoiNhanKhan = loiNhanKhanBL.GetListTabularLoiNhanKhan(
                maNamHoc, tuNgay, denNgay,
                maHocSinhHienThi, xacNhan, 
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out totalRecords);
            
            if (lstTabularLoiNhanKhan.Count == 0 && totalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRepeater();
                return;
            }

            bool bDisplayData = (lstTabularLoiNhanKhan.Count != 0) ? true : false;
            ProcessDislayInfo(bDisplayData);
            RptLoiNhanKhan.DataSource = lstTabularLoiNhanKhan;
            RptLoiNhanKhan.DataBind();
            MainDataPager.ItemCount = totalRecords;
        }

        private void ProcessDislayInfo(bool bDisplayData)
        {
            PnlPopupDetail.Visible = bDisplayData;
            RptLoiNhanKhan.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin lời nhắn khẩn";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy lời nhắn khẩn";
                }
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
            }
            else
            {
                MainDataPager.Visible = true;
            }
        }

        private void BindDropDownListNamHoc()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL();
            List<CauHinh_NamHoc> lstNamHoc = systemConfigBL.GetListYears();
            DdlNamHoc.DataSource = lstNamHoc;
            DdlNamHoc.DataValueField = "MaNamHoc";
            DdlNamHoc.DataTextField = "TenNamHoc";
            DdlNamHoc.DataBind();
            DdlNamHoc.SelectedValue = (new SystemConfigBL()).GetCurrentYear().ToString();
        }

        private void BindDropDownListXacNhan()
        {
            DdlXacNhan.Items.Add(new ListItem("Tất cả", "-1"));
            DdlXacNhan.Items.Add(new ListItem("Có", "1"));
            DdlXacNhan.Items.Add(new ListItem("Không", "0"));            
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
        #endregion

        #region Repeater event handlers
        protected void RptLoiNhanKhan_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //LopHocInfo lopHoc = (LopHocInfo)e.Item.DataItem;
                //if (lopHoc != null)
                //{
                //    int maLopHoc = lopHoc.MaLopHoc;
                //    if (!lopHocBL.CheckCanDeleteLopHoc(maLopHoc))
                //    {
                //        ImageButton btnDeleteItem = (ImageButton)e.Item.FindControl("BtnDeleteItem");
                //        btnDeleteItem.ImageUrl = "~/Styles/Images/button_delete_disable.png";
                //        btnDeleteItem.Enabled = false;
                //    }
                //}
            }
        }

        protected void RptLoiNhanKhan_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDetailItem":
                    {
                        int maLoiNhanKhan = Int32.Parse(e.CommandArgument.ToString());
                        LoiNhanKhan_LoiNhanKhan loiNhanKhan = loiNhanKhanBL.GetLoiNhanKhan(maLoiNhanKhan);

                        LblTieuDe.Text = loiNhanKhan.TieuDe;
                        LblNoiDung.Text = loiNhanKhan.NoiDung;
                        LblNgay.Text = loiNhanKhan.Ngay.ToShortDateString();
                        LblXacNhan.Text = (loiNhanKhan.XacNhan) ? "Có" : "Không";
                        BtnConfirm.Visible = !loiNhanKhan.XacNhan;
                        ModalPopupExtender mPEDetail = (ModalPopupExtender)e.Item.FindControl("MPEDetail");
                        mPEDetail.Show();

                        this.HdfMaLoiNhanKhan.Value = maLoiNhanKhan.ToString();
                        this.HdfRptLoiNhanKhanMPEDetail.Value = mPEDetail.ClientID;

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
            int maLoiNhanKhan = Int32.Parse(this.HdfMaLoiNhanKhan.Value);
            loiNhanKhanBL.Confirm(maLoiNhanKhan);
            BindRepeater();
        }

        #endregion

        #region Pager event handlers
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currentPageIndex;
            //BindRepeaterLopHoc();
        }
        #endregion
    }
}