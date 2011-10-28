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
    public partial class LoiNhanKhan : BaseContentPage
    {
        #region Fields
        private bool isSearch;
        private LoiNhanKhanBL loiNhanKhanBL;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (isAccessDenied)
            {
                // User can not access this page
                return;
            }

            loiNhanKhanBL = new LoiNhanKhanBL();
            if (!Page.IsPostBack)
            {
                isSearch = false;
                BindDropDownLists();
                InitDates();

                BindRptLoiNhanKhan();
            }
        }
        #endregion

        #region Methods
        private void BindRptLoiNhanKhan()
        {
            int maNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);
            DateTime tuNgay = DateTime.Parse(TxtTuNgay.Text);
            DateTime denNgay = DateTime.Parse(TxtDenNgay.Text);
            string maHocSinhHienThi = TxtMaHS.Text;
            int xacNhan = Int32.Parse(DdlXacNhan.SelectedValue);

            double totalRecords;
            List<TabularLoiNhanKhan> lstTabularLoiNhanKhan = loiNhanKhanBL.GetListTabularLoiNhanKhan(
                maNamHoc, tuNgay, denNgay,
                maHocSinhHienThi, xacNhan, MainDataPager.CurrentIndex, MainDataPager.PageSize, out totalRecords);

            if (lstTabularLoiNhanKhan.Count == 0 && totalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRptLoiNhanKhan();
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
            PnlPopupConfirmDelete.Visible = bDisplayData;
            PnlPopupEdit.Visible = bDisplayData;
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

        private void BindDropDownLists()
        {
            BindDDLNamHoc();
            BindDDLXacNhan();
            BindDDLNganhHoc();
            BindDDLKhoiLop();
            BindDDLLopHoc();
        }

        private void BindDDLNamHoc()
        {
            NamHocBL namHocBL = new NamHocBL();
            List<CauHinh_NamHoc> lstNamHoc = namHocBL.GetListNamHoc();
            DdlNamHoc.DataSource = lstNamHoc;
            DdlNamHoc.DataValueField = "MaNamHoc";
            DdlNamHoc.DataTextField = "TenNamHoc";
            DdlNamHoc.DataBind();
            DdlNamHoc.SelectedValue = (new CauHinhHeThongBL()).GetMaNamHocHienHanh().ToString();
        }

        private void BindDDLXacNhan()
        {
            DdlXacNhan.Items.Add(new ListItem("Tất cả", "-1"));
            DdlXacNhan.Items.Add(new ListItem("Có", "1"));
            DdlXacNhan.Items.Add(new ListItem("Không", "0"));
        }

        private void BindDDLKhoiLop()
        {
            KhoiLopBL KhoiLopBL = new KhoiLopBL();
            List<DanhMuc_KhoiLop> lstKhoiLop = KhoiLopBL.GetListKhoiLop();
            DdlKhoiLopThem.DataSource = lstKhoiLop;
            DdlKhoiLopThem.DataValueField = "MaKhoiLop";
            DdlKhoiLopThem.DataTextField = "TenKhoiLop";
            DdlKhoiLopThem.DataBind();
            if (lstKhoiLop.Count > 1)
            {
                DdlKhoiLopThem.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }

        private void BindDDLNganhHoc()
        {
            FacultyBL nganhHocBL = new FacultyBL();
            List<DanhMuc_NganhHoc> lstNganhHoc = nganhHocBL.GetListNganhHoc();
            DdlNganhHocThem.DataSource = lstNganhHoc;
            DdlNganhHocThem.DataValueField = "MaNganhHoc";
            DdlNganhHocThem.DataTextField = "TenNganhHoc";
            DdlNganhHocThem.DataBind();
            if (lstNganhHoc.Count > 1)
            {
                DdlNganhHocThem.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }

        private void BindDDLLopHoc()
        {
            int maNamHoc = (new CauHinhHeThongBL()).GetMaNamHocHienHanh();

            int maNganhHoc = 0;
            try
            {
                maNganhHoc = Int32.Parse(DdlNganhHocThem.SelectedValue);
            }
            catch (Exception) { }

            int maKhoiLop = 0;
            try
            {
                maKhoiLop = Int32.Parse(DdlKhoiLopThem.SelectedValue);
            }
            catch (Exception) { }

            LopHocBL lopHocBL = new LopHocBL();
            List<LopHoc_Lop> lstLop = lopHocBL.GetListLopHoc(maNganhHoc, maKhoiLop, maNamHoc);
            DdlLopThem.DataSource = lstLop;
            DdlLopThem.DataValueField = "MaLopHoc";
            DdlLopThem.DataTextField = "TenLopHoc";
            DdlLopThem.DataBind();

            if (lstLop.Count > 1)
            {
                DdlLopThem.Items.Insert(0, new ListItem("Tất cả", "0"));
            }

            BindDDLHocSinh();
        }

        private void BindDDLHocSinh()
        {
            List<DDLFormatHocSinh> lstHocSinh = new List<DDLFormatHocSinh>();
            if (DdlLopThem.Items.Count != 0)
            {
                int maNganh = Int32.Parse(DdlNganhHocThem.SelectedValue);
                int maKhoi = Int32.Parse(DdlKhoiLopThem.SelectedValue);
                int maLopHoc = Int32.Parse(DdlLopThem.SelectedValue);
                lstHocSinh = (new HocSinhBL()).GetListHocSinh(maNganh, maKhoi, maLopHoc);
            }

            DdlHocSinhThem.DataSource = lstHocSinh;
            DdlHocSinhThem.DataTextField = "MaHocSinhHienThi";
            DdlHocSinhThem.DataValueField = "MaHocSinhLopHoc";
            DdlHocSinhThem.DataBind();
            if (DdlHocSinhThem.Items.Count > 1)
            {
                DdlHocSinhThem.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
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

            TxtNgayThem.Text = today.ToShortDateString();
        }
        #endregion

        #region DropDownList event hanlders
        protected void DdlNganhThem_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLLopHoc();
        }

        protected void DdlKhoiLopThem_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLLopHoc();
        }

        protected void DdlLopHocThem_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLHocSinh();
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
                case "CmdDeleteItem":
                    {
                        this.LblConfirmDelete.Text = "Bạn có chắc xóa lời nhắn khẩn <b>" + e.CommandArgument + "</b> này không?";
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        HiddenField hdfRptMaLoiNhanKhan = (HiddenField)e.Item.FindControl("HdfRptMaLoiNhanKhan");
                        this.HdfMaLoiNhanKhan.Value = hdfRptMaLoiNhanKhan.Value;

                        this.HdfRptLoiNhanKhanMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        int maLoiNhanKhan = Int32.Parse(e.CommandArgument.ToString());
                        LoiNhanKhan_LoiNhanKhan loiNhanKhan = loiNhanKhanBL.GetLoiNhanKhan(maLoiNhanKhan);

                        LblTieuDeSua.Text = loiNhanKhan.TieuDe;
                        TxtNoiDungSua.Text = TxtNoiDungSua.Text;
                        TxtNgaySua.Text = loiNhanKhan.Ngay.ToShortDateString();

                        HocSinh_HocSinhLopHoc hocSinhLopHoc = (new HocSinhBL()).GetHocSinhLopHoc(loiNhanKhan.MaHocSinhLopHoc);
                        LblMaHocSinhSua.Text = (new HocSinhBL()).GetThongTinCaNhan(hocSinhLopHoc.MaHocSinh).MaHocSinhHienThi;
                        LopHocInfo lopHoc = (new LopHocBL()).GetLopHocInfo(hocSinhLopHoc.MaLopHoc);
                        LblNganhHocSua.Text = lopHoc.TenNganhHoc;
                        LblKhoiSua.Text = lopHoc.TenKhoiLop;
                        LblLopSua.Text = lopHoc.TenLopHoc;

                        ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        mPEEdit.Show();

                        this.HdfMaLoiNhanKhan.Value = maLoiNhanKhan.ToString();
                        this.HdfRptLoiNhanKhanMPEEdit.Value = mPEEdit.ClientID;

                        break;
                    }
                case "CmdDetailItem":
                    {
                        //int maLopHoc = Int32.Parse(e.CommandArgument.ToString());
                        //LopHoc_Lop lophoc = lopHocBL.GetLopHoc(maLopHoc);

                        //LblTenLopHocChiTiet.Text = lophoc.TenLopHoc;
                        //LblTenNganhHocChiTiet.Text = (new NganhHocBL()).GetNganhHoc(lophoc.MaNganhHoc).TenNganhHoc;
                        //LblTenKhoiLopChiTiet.Text = (new KhoiLopBL()).GetKhoiLop(lophoc.MaKhoiLop).TenKhoiLop;
                        //LblSiSoChiTiet.Text = lophoc.SiSo.ToString();
                        //ModalPopupExtender mPEDetail = (ModalPopupExtender)e.Item.FindControl("MPEDetail");
                        //mPEDetail.Show();

                        //this.HdfMaLopHoc.Value = maLopHoc.ToString();
                        //this.HdfRptLopHocMPEDetail.Value = mPEDetail.ClientID;
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
            BindRptLoiNhanKhan();
        }

        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;

            string tieuDe = TxtTieuDeThem.Text;
            string noiDung = TxtNoiDungThem.Text;
            DateTime ngay = DateTime.Parse(TxtNgayThem.Text);
            int maHocSinhLopHoc = Int32.Parse(DdlHocSinhThem.SelectedValue);
            if (maHocSinhLopHoc == 0)
            {
                for (int i = 1; i < DdlHocSinhThem.Items.Count; i++)
                {
                    loiNhanKhanBL.InsertLoiNhanKhan(Int32.Parse(DdlHocSinhThem.Items[i].Value),
                        tieuDe, noiDung, ngay);
                }
            }
            else
            {
                loiNhanKhanBL.InsertLoiNhanKhan(maHocSinhLopHoc,
                        tieuDe, noiDung, ngay);
            }

            BindRptLoiNhanKhan();

            TxtTieuDeThem.Text = "";
            TxtNoiDungThem.Text = "";
            TxtNgayThem.Text = DateTime.Now.ToShortDateString();

            if (this.CkbAddAfterSave.Checked)
            {
                this.MPEAdd.Show();
            }
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            int maLoiNhanKhan = Int32.Parse(this.HdfMaLoiNhanKhan.Value);
            loiNhanKhanBL.UpdateLoiNhanKhan(maLoiNhanKhan, TxtNoiDungSua.Text, DateTime.Parse(TxtNgaySua.Text));
            BindRptLoiNhanKhan();
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            int maLopNhanKhan = Int32.Parse(this.HdfMaLoiNhanKhan.Value);
            loiNhanKhanBL.DeleteLoiNhanKhan(maLopNhanKhan);
            isSearch = false;
            BindRptLoiNhanKhan();
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