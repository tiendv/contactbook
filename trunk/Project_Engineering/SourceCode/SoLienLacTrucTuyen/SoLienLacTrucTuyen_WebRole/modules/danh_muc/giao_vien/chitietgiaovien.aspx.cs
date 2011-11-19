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
        TeacherBL giaoVienBL;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (isAccessDenied)
            {
                return;
            }

            giaoVienBL = new TeacherBL(UserSchool);

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
        private void FillGiaoVien(int teacherId)
        {
            DanhMuc_GiaoVien teacher = giaoVienBL.GetTeacher(teacherId);
            LblMaGiaoVienHienThi.Text = teacher.MaHienThiGiaoVien;
            LblTenGiaoVien.Text = teacher.HoTen;
            LblNgaySinh.Text = teacher.NgaySinh.ToShortDateString();
            LblGioiTinh.Text = teacher.GioiTinh ? "Nam" : "Nữ";
            LblDiaChi.Text = teacher.DiaChi;
            LblDienThoai.Text = (teacher.DienThoai != "") ? teacher.DienThoai : "(không có)";
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
            DanhMuc_GiaoVien teacher = new DanhMuc_GiaoVien();
            int maGiaoVien = Int32.Parse(ViewState["magiaovien"].ToString());            
            teacher.MaGiaoVien = maGiaoVien;
            double totalRecords;
            List<TabularTeaching> lstTbGiangDays = giaoVienBL.GetListTeachings(
                teacher, DataPagerGiangDay.CurrentIndex, DataPagerGiangDay.PageSize, out totalRecords);

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
            DanhMuc_GiaoVien teacher = new DanhMuc_GiaoVien();
            int maGiaoVien = Int32.Parse(ViewState["magiaovien"].ToString());
            teacher.MaGiaoVien = maGiaoVien;
            double totalRecords;
            List<TabularFormering> lstTbChuNhiems = giaoVienBL.GetListFormerings(
                teacher, DataPagerChuNhiem.CurrentIndex, DataPagerChuNhiem.PageSize, out totalRecords);

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