using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;
using AjaxControlToolkit;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class DanhMucDanhHieuPage : BaseContentPage
    {
        #region Fields
        private DanhHieuBL danhHieuBL = new DanhHieuBL();
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

            if (!Page.IsPostBack)
            {
                isSearch = false;
                MainDataPager.CurrentIndex = 1;
                BindData();
            }

            ProcPermissions();
        }
        #endregion

        #region Button click event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            isSearch = true;
            BindData();
        }

        protected void BtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("themdanhhieu.aspx");
        }

        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {
            string tenDanhHieu = this.TxtTenDanhHieu.Text.Trim();

            if (tenDanhHieu == "")
            {
                TenDanhHieuRequiredAdd.IsValid = false;
                TxtTenDanhHieu.Focus();
                return;
            }
            else
            {
                if (danhHieuBL.DanhHieuExists(tenDanhHieu))
                {
                    TenDanhHieuValidatorAdd.IsValid = false;
                    TxtTenDanhHieu.Focus();
                    return;
                }
            }

            danhHieuBL.InsertDanhHieu(tenDanhHieu, new Dictionary<int, int>());

            MainDataPager.CurrentIndex = 1;
            BindData();

            this.TxtTenDanhHieu.Text = "";

            if (this.CkbAddAfterSave.Checked)
            {
                
            }
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            int maDanhHieu = Int32.Parse(this.HdfMaDanhHieu.Value);
            danhHieuBL.DeleteDanhHieu(maDanhHieu);
            isSearch = false;
            BindData();
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender modalPopupEdit = new ModalPopupExtender();
            foreach (RepeaterItem rptItem in RptDanhHieu.Items)
            {
                if (rptItem.ItemType == ListItemType.Item || rptItem.ItemType == ListItemType.AlternatingItem)
                {
                    modalPopupEdit = (ModalPopupExtender)rptItem.FindControl("MPEEdit");
                    if (modalPopupEdit.ClientID == HdfRptDanhHieuMPEEdit.Value)
                    {
                        break;
                    }
                }
            }

            if (!Page.IsValid)
            {
                return;
            }

            int maDanhHieu = Int32.Parse(this.HdfMaDanhHieu.Value);
            string tenDanhHieu = TxtSuaTenDanhHieu.Text.Trim();

            if (tenDanhHieu == "")
            {
                TenDanhHieuRequiredEdit.IsValid = false;
                modalPopupEdit.Show();
                return;
            }
            else
            {
                if (danhHieuBL.DanhHieuExists(maDanhHieu, tenDanhHieu))
                {
                    TenDanhHieuValidatorEdit.IsValid = false;
                    modalPopupEdit.Show();
                    return;
                }
            }

            danhHieuBL.UpdateDanhHieu(maDanhHieu, tenDanhHieu);
            BindData();
        }
        #endregion

        #region Repeater event handlers
        protected void RptDanhHieu_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
                        DanhMuc_DanhHieu DanhHieu = (DanhMuc_DanhHieu)e.Item.DataItem;
                        if (!danhHieuBL.CanDeleteDanhHieu(DanhHieu.MaDanhHieu))
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

        protected void RptDanhHieu_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDeleteItem":
                    {
                        this.LblConfirmDelete.Text = "Bạn có chắc xóa danh hiệu <b>" + e.CommandArgument + "</b> này không?";
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        HiddenField hdfRptMaDanhHieu = (HiddenField)e.Item.FindControl("HdfRptMaDanhHieu");
                        this.HdfMaDanhHieu.Value = hdfRptMaDanhHieu.Value;

                        this.HdfRptDanhHieuMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        int maDanhHieu = Int32.Parse(e.CommandArgument.ToString());
                        DanhMuc_DanhHieu DanhHieu = danhHieuBL.GetDanhHieu(maDanhHieu);

                        TxtSuaTenDanhHieu.Text = DanhHieu.TenDanhHieu;
                        ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        mPEEdit.Show();

                        this.HdfRptDanhHieuMPEEdit.Value = mPEEdit.ClientID;
                        this.HdfMaDanhHieu.Value = maDanhHieu.ToString();

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
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currnetPageIndx;
            BindData();
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

        public void BindData()
        {
            string tenDanhHieu = TxtSearchDanhHieu.Text.Trim();

            double totalRecord;
            List<DanhMuc_DanhHieu> lstDanhHieu = danhHieuBL.GetListDanhHieus(tenDanhHieu,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out totalRecord);

            // Decrease page current index when delete
            if (lstDanhHieu.Count == 0 && MainDataPager.ItemCount != 0)
            {
                MainDataPager.CurrentIndex--;
                BindData();
                return;
            }

            bool bDisplayData = (lstDanhHieu.Count != 0) ? true : false;
            PnlPopupConfirmDelete.Visible = bDisplayData;
            PnlPopupEdit.Visible = bDisplayData;
            RptDanhHieu.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;
            MainDataPager.Visible = bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin danh hiệu";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy danh hiệu";
                }
            }
            else
            {
                MainDataPager.Visible = true;
            }

            RptDanhHieu.DataSource = lstDanhHieu;
            RptDanhHieu.DataBind();
            MainDataPager.ItemCount = totalRecord;
        }
        #endregion
    }
}