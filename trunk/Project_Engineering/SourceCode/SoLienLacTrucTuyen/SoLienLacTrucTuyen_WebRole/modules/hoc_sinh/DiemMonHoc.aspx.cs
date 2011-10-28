using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.BusinessEntity;
using SoLienLacTrucTuyen.DataAccess;
using AjaxControlToolkit;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class DiemMonHoc : System.Web.UI.Page
    {
        #region Fields
        private KetQuaHocTapBL ketQuaHocTapBL;
        private int maDiemMonHK;
        #endregion

        #region Page event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ketQuaHocTapBL = new KetQuaHocTapBL();

            Site masterPage = (Site)Page.Master;
            masterPage.UserRole = (new UserBL()).GetRoleId(User.Identity.Name);
            masterPage.PageUrl = Page.Request.Path;
            masterPage.PageTitle = "Điểm Môn Học";

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["DiemMonHK"] != null)
                {
                    maDiemMonHK = Int32.Parse(Request.QueryString["DiemMonHK"]);
                    this.HdfMaDiemMonHK.Value = Request.QueryString["DiemMonHK"];
                    FillThongTinHocSinh(maDiemMonHK);                    
                    BindDropDownList();
                    BindRepeater();
                }

                if (Request.QueryString["STab"] != null)
                {
                    this.HdfSTab.Value = Request.QueryString["STab"];
                }
            }
            else
            {
                maDiemMonHK = Int32.Parse(this.HdfMaDiemMonHK.Value);
            }            
        }

        private void GetDiemTrungBinh(int maDiemMonHK)
        {
            double diemTB = ketQuaHocTapBL.GetDiemTrungBinh(maDiemMonHK);
            if (diemTB != -1)
            {
                this.LblDiemTB.Text = "ĐIỂM TRUNG BÌNH: " + diemTB.ToString();
            }
            else
            {
                this.LblDiemTB.Text = "";
            }
        }
        #endregion

        #region Methods
        private void BindDropDownList()
        {
            List<DanhMuc_LoaiDiem> lstLoaiDiem = (new LoaiDiemBL()).GetListLoaiDiem();
            this.DdlLoaiDiemThem.DataSource = lstLoaiDiem;
            this.DdlLoaiDiemThem.DataTextField = "TenLoaiDiem";
            this.DdlLoaiDiemThem.DataValueField = "MaLoaiDiem";
            this.DdlLoaiDiemThem.DataBind();
        }

        private void BindRepeater()
        {
            double totalRecords;
            maDiemMonHK = Int32.Parse(this.HdfMaDiemMonHK.Value);

            List<TabularChiTietDiemMonHocLoaiDiem> lstChiTietDiem = ketQuaHocTapBL.GetListTabularChiTietDiemMonHocLoaiDiem(maDiemMonHK, 
                this.MainDataPager.CurrentIndex, this.MainDataPager.PageSize, out totalRecords);

            // Decrease page current index when delete
            if (lstChiTietDiem.Count == 0 && totalRecords != 0)
            {
                this.MainDataPager.CurrentIndex--;
                BindRepeater();
                return;
            }

            bool bDisplayData = (lstChiTietDiem.Count != 0) ? true : false;
            ProcessDislayInfo(bDisplayData);

            this.MainDataPager.ItemCount = totalRecords;
            this.RptDiemMonHoc.DataSource = lstChiTietDiem;
            this.RptDiemMonHoc.DataBind();

            GetDiemTrungBinh(maDiemMonHK);
        }

        private void ProcessDislayInfo(bool bDisplayData)
        {
            PnlPopupConfirmDelete.Visible = bDisplayData;
            PnlPopupEdit.Visible = bDisplayData;
            RptDiemMonHoc.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;
            this.MainDataPager.Visible = bDisplayData;
        }

        private void FillThongTinHocSinh(int maDiemMonHK)
        {
            HocSinh_ThongTinCaNhan hocsinh = ketQuaHocTapBL.GetHocSinh(maDiemMonHK);
            this.LblHoVaTen.Text = "Họ và tên: " + hocsinh.HoTen;
            this.LblMaHocSinh.Text = "Mã học sinh: " + hocsinh.MaHocSinhHienThi;

            this.LblNamHoc.Text = "Năm học: " + ketQuaHocTapBL.GetNamHoc(maDiemMonHK).TenNamHoc;
            this.LblHocKy.Text = "Học kỳ: " + ketQuaHocTapBL.GetHocKy(maDiemMonHK).TenHocKy;

            this.LblBangDiemMon.Text = "BẢNG ĐIỂM MÔN " + ketQuaHocTapBL.GetMonHoc(maDiemMonHK).TenMonHoc;
        }
        #endregion

        #region Repeater event handlers
        protected void RptDiemMonHoc_ItemDataBound(object sender, RepeaterItemEventArgs e)
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

        protected void RptDiemMonHoc_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDeleteItem":
                    {
                        //this.LblConfirmDelete.Text = "Bạn có chắc xóa lớp học <b>" + e.CommandArgument + "</b> này không?";
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        HiddenField hdfRptMaLopHoc = (HiddenField)e.Item.FindControl("HdfRptMaChiTietDiem");
                        this.HdfMaChiTietDiem.Value = hdfRptMaLopHoc.Value;

                        this.HdfRptLopHocMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        int maChiTietDiem = Int32.Parse(e.CommandArgument.ToString());
                        HocSinh_ChiTietDiem chiTietDiem = ketQuaHocTapBL.GetChiTietDiem(maChiTietDiem);

                        LoaiDiemBL loaiDiemBL = new LoaiDiemBL();
                        this.LblLoaiDiemSua.Text = loaiDiemBL.GetLoaiDiem(chiTietDiem.MaLoaiDiem).TenLoaiDiem;
                        this.TxtDiemSua.Text = chiTietDiem.Diem.ToString();

                        ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        mPEEdit.Show();

                        this.HdfMaChiTietDiem.Value = maChiTietDiem.ToString();
                        this.HdfRptLopHocMPEEdit.Value = mPEEdit.ClientID;

                        break;
                    }             
                default:
                    {
                        break;
                    }
            }
        }
        #endregion

        #region Pager event handlers
        public void pager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currentPageIndex;
            BindRepeater();
        }
        #endregion

        #region Button event handlers
        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {
            this.MainDataPager.CurrentIndex = 1;

            int maLoaiDiem = Int32.Parse(DdlLoaiDiemThem.SelectedValue);
            double diem = double.Parse(TxtDiemThem.Text);

            ketQuaHocTapBL.InsertChiTietDiem(maDiemMonHK, maLoaiDiem, diem);
            BindRepeater();

            TxtDiemThem.Text = "";
            DdlLoaiDiemThem.SelectedIndex = 0;

            if (this.CkbAddAfterSave.Checked)
            {
                this.MPEAdd.Show();
            }
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            int maChiTietDiem = Int32.Parse(this.HdfMaChiTietDiem.Value);
            double diem = double.Parse(this.TxtDiemSua.Text);
            ketQuaHocTapBL.UpdateChiTietDiem(maChiTietDiem, diem);
            BindRepeater();
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            int maChiTietDiem = Int32.Parse(this.HdfMaChiTietDiem.Value);
            ketQuaHocTapBL.DeleteChiTietDiem(maChiTietDiem);
            BindRepeater();
        }

        protected void BtnBackPrevPage_Click(object sender, ImageClickEventArgs e)
        {
            string query = "HocSinh=" + (ketQuaHocTapBL.GetHocSinh(maDiemMonHK).MaHocSinh).ToString();
            query += "&Tab=" + this.HdfSTab.Value;
            //string saveSearchQuery = "&SNam=" + HdfSearchNamHoc.Value + "&SNganh=" + HdfSearchNganhHoc.Value
            //    + "&SKhoi=" + HdfSearchKhoiLop.Value + "&SLop=" + HdfSearchLopHoc.Value
            //    + "&STen=" + HdfSearchTenHocSinh.Value + "&SMa=" + HdfSearchMaHocSinh.Value;

            
            Response.Redirect("/Modules/Hoc_Sinh/ChiTietHocSinh.aspx?" + query);
        }               
        #endregion
    }
}