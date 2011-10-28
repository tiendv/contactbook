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
    public partial class ChiTietGiaoVienPage : BaseContentPage
    {
        #region Fields
        GiaoVienBL giaoVienBL = new GiaoVienBL();
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (isAccessDenied)
            {
                return;
            }

            if (!Page.IsPostBack)
            {
                ViewState["prevpageid"] = Request.QueryString["prevpageid"];

                string maGiaoVien = Request.QueryString["giaovien"];
                ViewState["magiaovien"] = maGiaoVien;                
                FillGiaoVien(Int32.Parse(maGiaoVien));
                BindDataChuNhiem();
                BindDataGiangDay();
                ProcPermissions();
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnSua_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(string.Format("suagiaovien.aspx?giaovien={0}&prevpageid={1}",
                ViewState["magiaovien"], 4));
        }

        protected void BtnBackPrevPage_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/modules/danh_muc/giao_vien/danhsachgiaovien.aspx");
        }
        #endregion

        #region Methods
        private void FillGiaoVien(int maGiaoVien)
        {
            LopHoc_GiaoVien giaoVien = giaoVienBL.GetGiaoVien(maGiaoVien);
            LblMaGiaoVienHienThi.Text = giaoVien.MaHienThiGiaoVien;
            LblTenGiaoVien.Text = giaoVien.HoTen;
            LblNgaySinh.Text = giaoVien.NgaySinh.ToShortDateString();
            LblGioiTinh.Text = giaoVien.GioiTinh ? "Nam" : "Nữ";
            LblDiaChi.Text = giaoVien.DiaChi;
            LblDienThoai.Text = (giaoVien.DienThoai != "") ? giaoVien.DienThoai : "(không có)";
        }

        private void ProcPermissions()
        {
            if (lstAccessibilities.Contains(AccessibilityEnum.Modify))
            {
                // do something
            }
            else
            {
                BtnSua.Visible = false;
            }
        }
        #endregion

        #region Pager event handlers
        public void DataPagerChuNhiem_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            this.DataPagerChuNhiem.CurrentIndex = currnetPageIndx;
            BindDataChuNhiem();
        }

        public void DataPagerGiangDay_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            this.DataPagerGiangDay.CurrentIndex = currnetPageIndx;
            BindDataGiangDay();
        }

        private void BindDataGiangDay()
        {
            int maGiaoVien = Int32.Parse(ViewState["magiaovien"].ToString());
            double totalRecords;
            List<TabularHoatDongGiangDay> lstTbGiangDays = giaoVienBL.GetListTbHoatDongGiangDay(
                maGiaoVien, DataPagerGiangDay.CurrentIndex, DataPagerGiangDay.PageSize, out totalRecords);

            bool bDisplayData = (lstTbGiangDays.Count != 0) ? true : false;
            RptGiangDay.Visible = bDisplayData;
            DataPagerGiangDay.Visible = bDisplayData;
            LblSearchResultGiangDay.Visible = !bDisplayData;

            RptGiangDay.DataSource = lstTbGiangDays;
            RptGiangDay.DataBind();
            DataPagerGiangDay.ItemCount = totalRecords;
        }

        private void BindDataChuNhiem()
        {
            int maGiaoVien = Int32.Parse(ViewState["magiaovien"].ToString());
            double totalRecords;
            List<TabularHoatDongChuNhiem> lstTbChuNhiems = giaoVienBL.GetListTbHoatDongChuNhiem(
                maGiaoVien, DataPagerChuNhiem.CurrentIndex, DataPagerChuNhiem.PageSize, out totalRecords);

            bool bDisplayData = (lstTbChuNhiems.Count != 0) ? true : false;
            RptChuNhiem.Visible = bDisplayData;
            DataPagerChuNhiem.Visible = bDisplayData;
            LblSearchResultChuNhiem.Visible = !bDisplayData;

            RptChuNhiem.DataSource = lstTbChuNhiems;
            RptChuNhiem.DataBind();
            DataPagerChuNhiem.ItemCount = totalRecords;
        }
        #endregion
    }
}