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
    public partial class DanhMucMonHoc : BaseContentPage
    {
        #region Fields
        private MonHocBL monHocBL;
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

            monHocBL = new MonHocBL();            

            if (!Page.IsPostBack)
            {
                BindDropDownLists();
                isSearch = false;
                BindRepeater();
            }

            ProcPermissions();
        }
        #endregion

        #region DropDownList event hanlders
        protected void DdlNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropDownListMonHoc();
        }

        protected void DdlKhoiLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropDownListMonHoc();
        }
        #endregion

        #region Repeater event handlers
        protected void RptMonHoc_ItemDataBound(object sender, RepeaterItemEventArgs e)
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

                PnlPopupEdit.Visible = false;
            }

            if (lstAccessibilities.Contains(AccessibilityEnum.Delete))
            {
                if (e.Item.ItemType == ListItemType.Item
                    || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (e.Item.DataItem != null)
                    {
                        MonHocInfo monhoc = (MonHocInfo)e.Item.DataItem;
                        if (!monHocBL.CheckCanDeleteMonHoc(monhoc.MaMonHoc))
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

        protected void RptMonHoc_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDeleteItem":
                    {
                        // Set confirm text and show dialog
                        this.LblConfirmDelete.Text = "Bạn có chắc xóa môn học <b>" + e.CommandArgument + "</b> này không?";
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        // Save current MaMonHoc to global
                        HiddenField hdfRptMaMonHoc = (HiddenField)e.Item.FindControl("HdfRptMaMonHoc");
                        this.HdfMaMonHoc.Value = hdfRptMaMonHoc.Value;

                        // Save modal popup ClientID
                        this.HdfRptMonHocMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        int maMonHoc = Int32.Parse(e.CommandArgument.ToString());

                        DanhMuc_MonHoc monhoc = monHocBL.GetMonHoc(maMonHoc);

                        TxtTenMonHocSua.Text = monhoc.TenMonHoc;
                        LblTenNganhHocSua.Text = (new FacultyBL()).GetNganhHoc(monhoc.MaNganhHoc).TenNganhHoc;
                        HdfMaNganhHocSua.Value = monhoc.MaNganhHoc.ToString();
                        LblTenKhoiLopSua.Text = (new KhoiLopBL()).GetKhoiLop(monhoc.MaKhoiLop).TenKhoiLop;
                        HdfMaKhoiLopSua.Value = monhoc.MaKhoiLop.ToString();
                        TxtHeSoDiemSua.Text = monhoc.HeSoDiem.ToString();

                        ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        mPEEdit.Show();

                        this.HdfMaMonHoc.Value = maMonHoc.ToString();
                        this.HdfRptMonHocMPEEdit.Value = mPEEdit.ClientID;

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
            string tenMonHoc = this.TxtTenMonHocThem.Text.Trim();
            int maNganhHoc = Int32.Parse(this.DdlNganhHocThem.SelectedValue);
            int maKhoiLop = Int32.Parse(this.DdlKhoiLopThem.SelectedValue);
            string heSoDiem = this.TxtHeSoDiemThem.Text.Trim();
            

            bool bValidAdd = ValidateForAdd(tenMonHoc, maNganhHoc, maKhoiLop, 
                heSoDiem);

            if (bValidAdd)
            {
                monHocBL.InsertMonHoc(tenMonHoc, maNganhHoc, maKhoiLop, Double.Parse(heSoDiem));

                MainDataPager.CurrentIndex = 1;
                BindRepeater();
                BindDropDownListMonHoc();

                this.TxtTenMonHocThem.Text = "";
                this.TxtHeSoDiemThem.Text = "";

                if (this.CkbAddAfterSave.Checked)
                {
                    this.MPEAdd.Show();
                }
                else
                {
                    this.DdlNganhHocThem.SelectedIndex = 0;
                    this.DdlKhoiLopThem.SelectedIndex = 0;
                }
            }
        }

        private bool ValidateForAdd(string tenMonHoc, int maNganhHoc, int maKhoiLop,
            string heSoDiem)
        {
            if (!Page.IsValid)
            {
                return false;
            } 
            
            if (tenMonHoc == "")
            {
                TenMonHocRequiredAdd.IsValid = false;
                MPEAdd.Show();
                return false;
            }
            else
            {
                if (monHocBL.MonHocExists(tenMonHoc, maNganhHoc, maKhoiLop))
                {
                    TenMonHocValidatorAdd.IsValid = false;
                    MPEAdd.Show();
                    return false;
                }
            }

            if (!Regex.IsMatch(heSoDiem, HeSoDiemRegExp.ValidationExpression))
            {
                HeSoDiemRegExp.IsValid = false;
                MPEAdd.Show();
                return false;
            }            

            return true;
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender modalPopupEdit = new ModalPopupExtender();
            foreach (RepeaterItem rptItem in RptMonHoc.Items)
            {
                if (rptItem.ItemType == ListItemType.Item || rptItem.ItemType == ListItemType.AlternatingItem)
                {
                    modalPopupEdit = (ModalPopupExtender)rptItem.FindControl("MPEEdit");
                    if (modalPopupEdit.ClientID == HdfRptMonHocMPEEdit.Value)
                    {
                        break;
                    }
                }
            }

            if (!Page.IsValid)
            {
                return;
            }

            int maMonHoc = Int32.Parse(this.HdfMaMonHoc.Value);
            string tenMonHocMoi = this.TxtTenMonHocSua.Text.Trim();
            double heSoDiem = double.Parse(this.TxtHeSoDiemSua.Text.Trim());

            if (tenMonHocMoi == "")
            {
                TenMonHocRequiredEdit.IsValid = false;
                modalPopupEdit.Show();
                return;
            }
            else
            {
                if (monHocBL.MonHocExists(maMonHoc, tenMonHocMoi))
                {
                    TenMonHocValidatorEdit.IsValid = false;
                    modalPopupEdit.Show();
                    return;
                }
            }

            monHocBL.UpdateMonHoc(maMonHoc, tenMonHocMoi, heSoDiem);
            BindRepeater();
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            int maMonHoc = Int32.Parse(this.HdfMaMonHoc.Value);
            monHocBL.DeleteMonHoc(maMonHoc);
            isSearch = false;
            BindRepeater();
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
            double totalRecords;

            int maMonHoc = 0;
            try
            {
                maMonHoc = Int32.Parse(DdlMonHoc.SelectedValue);
            }
            catch (Exception) { }

            List<MonHocInfo> lstMonHocInfo;
            if (maMonHoc == 0) // "Tất cả"
            {
                int maNganhHoc = 0;
                try
                {
                    maNganhHoc = Int32.Parse(DdlNganh.SelectedValue);
                }
                catch (Exception) { }

                int maKhoiLop = 0;
                try
                {
                    maKhoiLop = Int32.Parse(DdlKhoiLop.SelectedValue);
                }
                catch (Exception) { }

                lstMonHocInfo = monHocBL.GetListMonHocInfo(maNganhHoc, maKhoiLop, 
                    MainDataPager.CurrentIndex, MainDataPager.PageSize, out totalRecords);
                MainDataPager.ItemCount = totalRecords;
            }
            else
            {
                lstMonHocInfo = new List<MonHocInfo> { monHocBL.GetMonHocInfo(maMonHoc) };
                MainDataPager.ItemCount = 1;
            }

            // Decrease page current index when delete
            if (lstMonHocInfo.Count == 0 && MainDataPager.ItemCount != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRepeater();
                return;
            }

            bool bDisplayData = (lstMonHocInfo.Count != 0) ? true : false;
            PnlPopupConfirmDelete.Visible = bDisplayData;
            PnlPopupEdit.Visible = bDisplayData;
            RptMonHoc.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin môn học";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy môn học";
                }
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
            }
            else
            {
                MainDataPager.Visible = true;
            }
            RptMonHoc.DataSource = lstMonHocInfo;
            RptMonHoc.DataBind();
        }

        private void BindDropDownLists()
        {
            BindDropDownListNganhHoc();

            BindDropDownListKhoiLop();

            BindDropDownListMonHoc();
        }

        private void BindDropDownListKhoiLop()
        {
            KhoiLopBL KhoiLopBL = new KhoiLopBL();
            List<DanhMuc_KhoiLop> lstKhoiLop = KhoiLopBL.GetListKhoiLop();
            DdlKhoiLop.DataSource = lstKhoiLop;
            DdlKhoiLop.DataValueField = "MaKhoiLop";
            DdlKhoiLop.DataTextField = "TenKhoiLop";
            DdlKhoiLop.DataBind();
            if (lstKhoiLop.Count > 1)
            {
                DdlKhoiLop.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
            DdlKhoiLopThem.DataSource = lstKhoiLop;
            DdlKhoiLopThem.DataValueField = "MaKhoiLop";
            DdlKhoiLopThem.DataTextField = "TenKhoiLop";
            DdlKhoiLopThem.DataBind();
        }

        private void BindDropDownListNganhHoc()
        {
            FacultyBL nganhHocBL = new FacultyBL();
            List<DanhMuc_NganhHoc> lstNganhHoc = nganhHocBL.GetListNganhHoc();
            DdlNganh.DataSource = lstNganhHoc;
            DdlNganh.DataValueField = "MaNganhHoc";
            DdlNganh.DataTextField = "TenNganhHoc";
            DdlNganh.DataBind();
            if (lstNganhHoc.Count > 1)
            {
                DdlNganh.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
            DdlNganhHocThem.DataSource = lstNganhHoc;
            DdlNganhHocThem.DataValueField = "MaNganhHoc";
            DdlNganhHocThem.DataTextField = "TenNganhHoc";
            DdlNganhHocThem.DataBind();
        }

        private void BindDropDownListMonHoc()
        {
            int maNganhHoc = 0;
            try
            {
                maNganhHoc = Int32.Parse(DdlNganh.SelectedValue);
            }
            catch (Exception) { }

            int maKhoiLop = 0;
            try
            {
                maKhoiLop = Int32.Parse(DdlKhoiLop.SelectedValue);
            }
            catch (Exception) { }

            List<DanhMuc_MonHoc> lstLop = GetListMonHoc(maNganhHoc, maKhoiLop);
            DdlMonHoc.DataSource = lstLop;
            DdlMonHoc.DataValueField = "MaMonHoc";
            DdlMonHoc.DataTextField = "TenMonHoc";
            DdlMonHoc.DataBind();

            if (lstLop.Count > 1)
            {
                DdlMonHoc.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }

        private List<DanhMuc_MonHoc> GetListMonHoc(int maNganhHoc, int maKhoiLop)
        {
            List<DanhMuc_MonHoc> lstLop = monHocBL.GetListMonHoc(maNganhHoc, maKhoiLop);
            return lstLop;
        }
        #endregion
    }
}