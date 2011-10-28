using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.BusinessEntity;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class ThemTietThoiKhoaBieuPage : BaseContentPage
    {
        #region Fields
        private ThoiKhoaBieuBL thoiKhoaBieuBL;
        private int maLopHoc;
        private int maHocKy;
        private int maThu;
        private int maTiet;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (isAccessDenied)
            {
                return;
            }

            thoiKhoaBieuBL = new ThoiKhoaBieuBL();
            Dictionary<string, int> dicQueryStrings = GetQueryStrings();
            if (dicQueryStrings != null)
            {
                maLopHoc = dicQueryStrings["MaLop"];
                maHocKy = dicQueryStrings["MaHocKy"];
                maThu = dicQueryStrings["MaThu"];
                maTiet = dicQueryStrings["MaTiet"];
            }

            if (!Page.IsPostBack)
            {                
                if (dicQueryStrings != null)
                {
                    LopHocInfo lopHoc = (new LopHocBL()).GetLopHocInfo(maLopHoc);
                    CauHinh_HocKy hocKy = (new HocKyBL()).GetHocKy(maHocKy);
                    CauHinh_Thu thu = (new ThuBL()).GetThu(maThu);
                    DanhMuc_Tiet tiet = (new TietBL()).GetTiet(maTiet);
                    LblTenLop.Text = lopHoc.TenLopHoc;
                    LblNamHoc.Text = lopHoc.TenNamHoc;
                    LblHocKy.Text = hocKy.TenHocKy;
                    LblThu.Text = thu.TenThu;
                    LblTiet.Text = string.Format("{0} ({1} - {2})",
                        tiet.TenTiet,
                        tiet.ThoiGianBatDau.ToShortTimeString(),
                        tiet.ThoiDiemKetThu.ToShortTimeString());

                    FillDDLNganh();
                    FillDDLKhoi();
                }
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnOpenPopupMonHoc_Click(object sender, ImageClickEventArgs e)
        {
            BindRepeaterMonHoc();
            MPEMonHoc.Show();
        }

        protected void BtnSearchMonHoc_Click(object sender, ImageClickEventArgs e)
        {
            DataPageMonHoc.CurrentIndex = 1;
            BindRepeaterMonHoc();
            MPEMonHoc.Show();
        }

        protected void BtnSaveMonHoc_Click(object sender, ImageClickEventArgs e)
        {
            foreach (RepeaterItem item in RptMonHoc.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    Control control = item.FindControl("RBtnSelect");
                    if (control != null)
                    {
                        RadioButton rBtnSelect = (RadioButton)control;
                        if (rBtnSelect.Checked)
                        {
                            HiddenField hdfRptMaMonHoc = (HiddenField)item.FindControl("HdfRptMaMonHoc");
                            HdfMaMonHoc.Value = hdfRptMaMonHoc.Value;

                            Label lblTenMonHoc = (Label)item.FindControl("LblTenMonHoc");
                            LblMonHoc.Text = lblTenMonHoc.Text;
                            break;
                        }
                    }
                }
            }
        }

        protected void BtnOpenPopupGiaoVien_Click(object sender, ImageClickEventArgs e)
        {
            BindRepeaterGiaoVien();
            MPEGiaoVien.Show();
        }

        protected void BtnSearchGiaoVien_Click(object sender, ImageClickEventArgs e)
        {
            DataPageGiaoVien.CurrentIndex = 1;
            BindRepeaterGiaoVien();
            MPEGiaoVien.Show();
        }

        protected void BtnSaveGiaoVien_Click(object sender, ImageClickEventArgs e)
        {
            foreach (RepeaterItem item in RptGiaoVien.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    Control control = item.FindControl("RBtnSelect");
                    if (control != null)
                    {
                        RadioButton rBtnSelect = (RadioButton)control;
                        if (rBtnSelect.Checked)
                        {
                            HiddenField hdfRptMaGiaoVien = (HiddenField)item.FindControl("HdfRptMaGiaoVien");
                            HdfMaGiaoVien.Value = hdfRptMaGiaoVien.Value;

                            Label lblTenGiaoVien = (Label)item.FindControl("LblTenGiaoVien");
                            LblGiaoVien.Text = lblTenGiaoVien.Text;
                            break;
                        }
                    }
                }
            }
        }

        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }

            ThoiKhoaBieuBL thoiKhoaBieuBL = new ThoiKhoaBieuBL();
            Dictionary<string, int> dicQueryStrings = GetQueryStrings();
            if (dicQueryStrings != null)
            {
                int maLopHoc = dicQueryStrings["MaLop"];
                int maHocKy = dicQueryStrings["MaHocKy"];
                int maThu = dicQueryStrings["MaThu"];
                int maTiet = dicQueryStrings["MaTiet"];
                int maMonHoc = Int32.Parse(HdfMaMonHoc.Value);
                int maGiaoVien = Int32.Parse(HdfMaGiaoVien.Value);

                thoiKhoaBieuBL.Insert(maLopHoc, maMonHoc, maGiaoVien, maHocKy, maThu, maTiet);

                Response.Redirect(string.Format("suathoikhoabieu.aspx?lop={0}&hocky={1}&thu={2}",
                    Request.QueryString["lop"], Request.QueryString["hocky"], Request.QueryString["thu"]));
            }
        }

        protected void BtnCancelAdd_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(string.Format("suathoikhoabieu.aspx?lop={0}&hocky={1}&thu={2}",
                Request.QueryString["lop"], Request.QueryString["hocky"], Request.QueryString["thu"]));
        }
        #endregion

        #region Pager event handlers
        public void DataPagerMonHoc_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            DataPageMonHoc.CurrentIndex = currnetPageIndx;
            BindRepeaterMonHoc();
            MPEMonHoc.Show();
        }

        public void DataPagerGiaoVien_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            DataPageGiaoVien.CurrentIndex = currnetPageIndx;
            BindRepeaterGiaoVien();
            MPEGiaoVien.Show();
        }
        #endregion

        #region Methods
        private Dictionary<string, int> GetQueryStrings()
        {
            Dictionary<string, int> dicQueryStrings = new Dictionary<string, int>();

            if (Request.QueryString["lop"] != null && Request.QueryString["hocky"] != null
                && Request.QueryString["thu"] != null && Request.QueryString["tiet"] != null)
            {
                int maLop;
                if (Int32.TryParse(Request.QueryString["lop"], out maLop))
                {
                    dicQueryStrings.Add("MaLop", maLop);
                }
                else
                {
                    return null;
                }

                int maHocKy;
                if (Int32.TryParse(Request.QueryString["hocky"], out maHocKy))
                {
                    dicQueryStrings.Add("MaHocKy", maHocKy);
                }
                else
                {
                    return null;
                }

                int maThu;
                if (Int32.TryParse(Request.QueryString["thu"], out maThu))
                {
                    dicQueryStrings.Add("MaThu", maThu);
                }
                else
                {
                    return null;
                }

                int maTiet;
                if (Int32.TryParse(Request.QueryString["tiet"], out maTiet))
                {
                    dicQueryStrings.Add("MaTiet", maTiet);
                }
                else
                {
                    return null;
                }
            }

            return dicQueryStrings;
        }

        private void FillDDLKhoi()
        {
            KhoiLopBL KhoiLopBL = new KhoiLopBL();
            List<DanhMuc_KhoiLop> lstKhoiLop = KhoiLopBL.GetListKhoiLop();

            DdlKhoi.DataSource = lstKhoiLop;
            DdlKhoi.DataValueField = "MaKhoiLop";
            DdlKhoi.DataTextField = "TenKhoiLop";
            DdlKhoi.DataBind();
        }

        private void FillDDLNganh()
        {
            FacultyBL nganhHocBL = new FacultyBL();
            List<DanhMuc_NganhHoc> lstNganhs = nganhHocBL.GetListNganhHoc();

            DdlNganh.DataSource = lstNganhs;
            DdlNganh.DataValueField = "MaNganhHoc";
            DdlNganh.DataTextField = "TenNganhHoc";
            DdlNganh.DataBind();
        }

        private void BindRepeaterMonHoc()
        {
            int maNganhHoc = -1;
            try
            {
                maNganhHoc = Int32.Parse(DdlNganh.SelectedValue);
            }
            catch (Exception)
            {
                LblSearchResultMonHoc.Visible = true;
                DataPageMonHoc.Visible = false;
                return;
            }

            int maKhoiLop = -1;
            try
            {
                maKhoiLop = Int32.Parse(DdlKhoi.SelectedValue);
            }
            catch (Exception)
            {
                LblSearchResultMonHoc.Visible = true;
                DataPageMonHoc.Visible = false;
                return;
            }

            string tenMonHoc = TxtMonHoc.Text.Trim();

            double totalRecords;
            List<MonHocInfo> lstMonHocInfo = (new MonHocBL()).GetListMonHocInfo(maNganhHoc, maKhoiLop,
                    tenMonHoc,
                    DataPageMonHoc.CurrentIndex, DataPageMonHoc.PageSize, out totalRecords);
            DataPageMonHoc.ItemCount = totalRecords;

            bool bDisplayData = (lstMonHocInfo.Count != 0) ? true : false;
            LblSearchResultMonHoc.Visible = !bDisplayData;
            DataPageMonHoc.Visible = bDisplayData;

            BtnSaveMonHoc.Enabled = bDisplayData;
            BtnSaveMonHoc.ImageUrl = (bDisplayData) ? "~/Styles/Images/button_save.png" : "~/Styles/Images/button_save_disable.png";
            RptMonHoc.Visible = bDisplayData;
            RptMonHoc.DataSource = lstMonHocInfo;
            RptMonHoc.DataBind();

            if (lstMonHocInfo.Count != 0)
            {
                RepeaterItemCollection items = RptMonHoc.Items;
                if (items[0].ItemType == ListItemType.Item
                    || items[0].ItemType == ListItemType.AlternatingItem)
                {
                    Control control = items[0].FindControl("RBtnSelect");
                    if (control != null)
                    {
                        RadioButton rBtnSelect = (RadioButton)control;
                        rBtnSelect.Checked = true;
                    }
                }
            }
        }

        private void BindRepeaterGiaoVien()
        {
            GiaoVienBL giaoVienBL = new GiaoVienBL();

            string maHienThiGiaoVien = TxtSearchMaGiaoVien.Text.Trim();
            string hoTen = TxtSearchTenGiaoVien.Text.Trim();

            double totalRecords;
            List<TabularGiaoVien> lstTbGiaoViens = giaoVienBL.GetListTabularGiaoViens(
                maHienThiGiaoVien, hoTen,
                DataPageGiaoVien.CurrentIndex, DataPageGiaoVien.PageSize, out totalRecords);
            DataPageGiaoVien.ItemCount = totalRecords;

            bool bDisplayData = (lstTbGiaoViens.Count != 0) ? true : false;
            LblSearchResultGiaoVien.Visible = !bDisplayData;
            DataPageGiaoVien.Visible = bDisplayData;

            BtnSaveGiaoVien.Enabled = bDisplayData;
            BtnSaveGiaoVien.ImageUrl = (bDisplayData) ? "~/Styles/Images/button_save.png" : "~/Styles/Images/button_save_disable.png";
            RptGiaoVien.Visible = bDisplayData;
            RptGiaoVien.DataSource = lstTbGiaoViens;
            RptGiaoVien.DataBind();

            if (lstTbGiaoViens.Count != 0)
            {
                RepeaterItemCollection items = RptGiaoVien.Items;
                if (items[0].ItemType == ListItemType.Item
                    || items[0].ItemType == ListItemType.AlternatingItem)
                {
                    Control control = items[0].FindControl("RBtnSelect");
                    if (control != null)
                    {
                        RadioButton rBtnSelect = (RadioButton)control;
                        rBtnSelect.Checked = true;
                    }
                }
            }
        }

        private bool ValidateInput()
        {
            bool bValid;

            if (HdfMaMonHoc.Value == "0" || HdfMaGiaoVien.Value == "0")
            {
                bValid = false;
            }
            else
            {
                bValid = true;                
            }

            if (HdfMaMonHoc.Value == "0")
            {
                LblMonHocError.Visible = true;
            }
            else
            {
                LblMonHocError.Visible = false;
            }

            if (HdfMaGiaoVien.Value == "0")
            {
                LblGiaoVienError.Text = "Chưa chọn giáo viên";
                LblGiaoVienError.Visible = true;
            }
            else
            {
                LblGiaoVienError.Visible = false;

                if (HdfMaGiaoVien.Value != "0")
                {
                    int maGiaoVien = Int32.Parse(HdfMaGiaoVien.Value);
                    GiaoVienBL giaoVienBL = new GiaoVienBL();
                    if (giaoVienBL.IsTeaching(maGiaoVien, maHocKy, maThu, maTiet))
                    {
                        bValid = false;
                        LblGiaoVienError.Text = "Giáo viên đang được phân công dạy tại cùng thời gian này";
                        LblGiaoVienError.Visible = true;
                    }
                }                
            }

            return bValid;
        }
        #endregion
    }
}