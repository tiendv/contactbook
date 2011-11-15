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
using SoLienLacTrucTuyen;
using System.Text.RegularExpressions;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class DanhMucTietPage : BaseContentPage
    {
        #region Fields
        private TeachingPeriodBL tietBL;
        private bool isSearch;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (isAccessDenied)
            {
                return;
            }

            tietBL = new TeachingPeriodBL();

            if (!Page.IsPostBack)
            {
                BindDropDownLists();
                isSearch = false;
                BindRepeater();
            }

            ProcPermissions();
        }
        #endregion

        #region Repeater event handlers
        protected void RptTietHoc_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (lstAccessibilities.Contains(AccessibilityEnum.Modify))
            {
                // Do something
            }
            else
            {
                if (e.Item.ItemType == ListItemType.Header)
                {
                    e.Item.FindControl("thEdit").Visible = false;
                }

                if (e.Item.ItemType == ListItemType.Item ||
                    e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    e.Item.FindControl("tdEdit").Visible = false;
                }

                //PnlPopupEdit.Visible = false;
            }

            if (lstAccessibilities.Contains(AccessibilityEnum.Delete))
            {
                if (e.Item.ItemType == ListItemType.Item
                    || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (e.Item.DataItem != null)
                    {
                        TabularTeachingPeriod tiet = (TabularTeachingPeriod)e.Item.DataItem;
                        DanhMuc_Tiet teachingPeriodTime = new DanhMuc_Tiet();
                        teachingPeriodTime.MaTiet = tiet.MaTiet;
                        if (!tietBL.IsDeletable(teachingPeriodTime))
                        {
                            ImageButton btnDeleteItem = (ImageButton)e.Item.FindControl("BtnDeleteItem");
                            btnDeleteItem.ImageUrl = "~/Styles/Images/button_delete_disable.png";
                            btnDeleteItem.Enabled = false;
                        }
                    }
                }
            }
            else
            {
                if (e.Item.ItemType == ListItemType.Header)
                {
                    e.Item.FindControl("thDelete").Visible = false;
                }

                if (e.Item.ItemType == ListItemType.Item ||
                    e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    e.Item.FindControl("tdDelete").Visible = false;
                }

                this.PnlPopupConfirmDelete.Visible = false;
            }
        }

        protected void RptTietHoc_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDeleteItem":
                    {
                        // Set confirm text and show dialog
                        this.LblConfirmDelete.Text = string.Format("Bạn có chắc xóa tiết học \"<b>{0}</b>\" này không?", e.CommandArgument);
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        // Save current MaTietHoc to global
                        HiddenField hdfRptMaTietHoc = (HiddenField)e.Item.FindControl("HdfRptMaTietHoc");
                        this.HdfMaTietHoc.Value = hdfRptMaTietHoc.Value;

                        // Save modal popup ClientID
                        this.HdfRptTietHocMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        int maTiet = Int32.Parse(e.CommandArgument.ToString());

                        DanhMuc_Tiet tiet = tietBL.GetTeachingPeriod(maTiet);
                        this.HdfSltTeachingPeriodName.Value = tiet.TenTiet;

                        TxtTenTietHocEdit.Text = tiet.TenTiet;
                        TxtThuTuEdit.Text = tiet.ThuTu.ToString();
                        DdlBuoiEdit.SelectedValue = tiet.MaBuoi.ToString();
                        DateTime dtThoiGianBatDau = tiet.ThoiGianBatDau;
                        TxtThoiGianBatDauEdit.Text = string.Format("{0}:{1}",
                            dtThoiGianBatDau.Hour, dtThoiGianBatDau.Minute);
                        DateTime dtThoiGianKetThuc = tiet.ThoiDiemKetThu;
                        TxtThoiGianKetThucEdit.Text = string.Format("{0}:{1}",
                            dtThoiGianKetThuc.Hour, dtThoiGianKetThuc.Minute);
                        ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        mPEEdit.Show();

                        this.HdfMaTietHoc.Value = maTiet.ToString();
                        this.HdfRptTietHocMPEEdit.Value = mPEEdit.ClientID;

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

        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {
            CauHinh_Buoi session = new CauHinh_Buoi();

            if (!ValidateForAdd())
            {
                return;
            }

            string tenTietHoc = this.TxtTenTietHocThem.Text.Trim();
            string thuTu = this.TxtThuTuAdd.Text.Trim();
            int buoi = Int32.Parse(DdlBuoiAdd.SelectedValue);
            session.MaBuoi = buoi;
            string strThoiGianBatDau = TxtThoiGianBatDauAdd.Text.Trim();
            string strThoiGianKetThuc = TxtThoiGianKetThucAdd.Text.Trim();

            tietBL.InsertTeachingPeriod(tenTietHoc, session, thuTu, strThoiGianBatDau, strThoiGianKetThuc);

            MainDataPager.CurrentIndex = 1;
            BindRepeater();

            TxtTenTietHocThem.Text = "";
            TxtThuTuAdd.Text = "";
            TxtThoiGianBatDauAdd.Text = "";
            TxtThoiGianKetThucAdd.Text = "";

            if (this.CkbAddAfterSave.Checked)
            {
                this.MPEAdd.Show();
            }
            else
            {
                this.DdlBuoiAdd.SelectedIndex = 0;
            }
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            DanhMuc_Tiet teachingPeriod = new DanhMuc_Tiet();
            CauHinh_Buoi session = new CauHinh_Buoi();
            if (!ValidateForEdit())
            {
                return;
            }

            int maTiet = Int32.Parse(this.HdfMaTietHoc.Value);
            string tenTietMoi = this.TxtTenTietHocEdit.Text.Trim();
            int buoi = Int32.Parse(DdlBuoiEdit.SelectedValue);
            session.MaBuoi = buoi;
            string thuTu = TxtThuTuEdit.Text.Trim();
            string strThoiGianBatDau = TxtThoiGianBatDauEdit.Text.Trim();
            string strThoiGianKetThuc = TxtThoiGianKetThucEdit.Text.Trim();
            teachingPeriod.MaTiet = maTiet;
            tietBL.UpdateTiet(teachingPeriod, tenTietMoi, session, thuTu, strThoiGianBatDau, strThoiGianKetThuc);
            BindRepeater();
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            int maTietHoc = Int32.Parse(this.HdfMaTietHoc.Value);
            DanhMuc_Tiet teachingPeriod = new DanhMuc_Tiet();
            teachingPeriod.MaTiet = maTietHoc;
            tietBL.DeleteTeachingPeriod(teachingPeriod);
            isSearch = false;
            BindRepeater();
        }
        #endregion

        #region Pager event handlers
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currentPageIndex;
            BindRepeater();
        }
        #endregion

        #region Methods
        private void ProcPermissions()
        {
            if (lstAccessibilities.Contains(AccessibilityEnum.Add))
            {
                BtnAdd.Enabled = true;
                BtnAdd.ImageUrl = "~/Styles/Images/button_add_with_text.png";
                PnlPopupAdd.Visible = true;
            }
            else
            {
                BtnAdd.Visible = false;
                PnlPopupAdd.Visible = false;
            }
        }

        private void BindRepeater()
        {
            CauHinh_Buoi session = null;
            double totalRecords;
            string tenTiet = TxtSearchTiet.Text.Trim();

            if (DdlBuoi.SelectedIndex != 0)
            {
                session = new CauHinh_Buoi();
                session.MaBuoi = Int32.Parse(DdlBuoi.SelectedValue);
            }

            List<TabularTeachingPeriod> listTbTiets = tietBL.GetTabularTeachingPeriods(tenTiet, session,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out totalRecords);

            // Decrease page current index when delete
            if (listTbTiets.Count == 0 && totalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRepeater();
                return;
            }

            MainDataPager.ItemCount = totalRecords;
            bool bDisplayData = (listTbTiets.Count != 0) ? true : false;
            PnlPopupConfirmDelete.Visible = bDisplayData;
            PnlPopupEdit.Visible = bDisplayData;
            RptTietHoc.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin tiết học";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy tiết học";
                }
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
            }
            else
            {
                MainDataPager.Visible = true;
            }
            RptTietHoc.DataSource = listTbTiets;
            RptTietHoc.DataBind();
        }

        private void BindDropDownLists()
        {
            BindDDLBuoi();
        }

        private void BindDDLBuoi()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL();
            List<CauHinh_Buoi> listBuois = systemConfigBL.GetSessions();
            DdlBuoi.DataSource = listBuois;
            DdlBuoi.DataValueField = "MaBuoi";
            DdlBuoi.DataTextField = "TenBuoi";
            DdlBuoi.DataBind();
            DdlBuoi.Items.Insert(0, new ListItem("Tất cả", "0"));

            DdlBuoiAdd.DataSource = listBuois;
            DdlBuoiAdd.DataValueField = "MaBuoi";
            DdlBuoiAdd.DataTextField = "TenBuoi";
            DdlBuoiAdd.DataBind();

            DdlBuoiEdit.DataSource = listBuois;
            DdlBuoiEdit.DataValueField = "MaBuoi";
            DdlBuoiEdit.DataTextField = "TenBuoi";
            DdlBuoiEdit.DataBind();
        }

        private bool ValidateForAdd()
        {
            if (!Page.IsValid)
            {
                return false;
            }

            string tenTietHoc = this.TxtTenTietHocThem.Text.Trim();
            string thuTu = this.TxtThuTuAdd.Text.Trim();

            if (tenTietHoc == "")
            {
                TenTietHocRequiredAdd.IsValid = false;
                MPEAdd.Show();
                return false;
            }
            else
            {
                if (tietBL.TeachingPeriodNameExists(tenTietHoc))
                {
                    TenTietHocValidatorAdd.IsValid = false;
                    MPEAdd.Show();
                    return false;
                }
            }

            if (!Regex.IsMatch(thuTu, ThuTuRegExp.ValidationExpression))
            {
                ThuTuRegExp.IsValid = false;
                MPEAdd.Show();
                return false;
            }

            return true;
        }

        private bool ValidateForEdit()
        {
            if (!Page.IsValid)
            {
                return false;
            }

            ModalPopupExtender modalPopupEdit = new ModalPopupExtender();
            foreach (RepeaterItem rptItem in RptTietHoc.Items)
            {
                if (rptItem.ItemType == ListItemType.Item || rptItem.ItemType == ListItemType.AlternatingItem)
                {
                    modalPopupEdit = (ModalPopupExtender)rptItem.FindControl("MPEEdit");
                    if (modalPopupEdit.ClientID == HdfRptTietHocMPEEdit.Value)
                    {
                        break;
                    }
                }
            }

            int maTiet = Int32.Parse(this.HdfMaTietHoc.Value);
            string oldTeachingPeriodName = this.HdfSltTeachingPeriodName.Value;
            string tenTietMoi = this.TxtTenTietHocEdit.Text.Trim();
            string thuTu = TxtThuTuEdit.Text.Trim();

            if (tenTietMoi == "")
            {
                TenTietHocRequiredEdit.IsValid = false;
                modalPopupEdit.Show();
                return false;
            }
            else
            {
                if (tietBL.TeachingPeriodNameExists(oldTeachingPeriodName, tenTietMoi))
                {
                    TenTietHocValidatorEdit.IsValid = false;
                    modalPopupEdit.Show();
                    return false;
                }
            }

            if (!Regex.IsMatch(thuTu, ThuTuRegExp.ValidationExpression))
            {
                ThuTuRegExp.IsValid = false;
                modalPopupEdit.Show();
                return false;
            }

            return true;
        }
        #endregion
    }
}