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
    public partial class DanhMucHanhKiem : BaseContentPage
    {
        private const string EDITED_MAHANHKIEM = "EditedMaHanhKiem";

        #region Fields
        private ConductBL hanhKiemBL = new ConductBL();
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

        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {
            string tenHanhKiem = this.TxtTenHanhKiem.Text;

            if (tenHanhKiem == "")
            {
                TenHanhKiemRequiredAdd.IsValid = false;
                TxtTenHanhKiem.Focus();
                MPEAdd.Show();
                return;
            }
            else
            {
                if (hanhKiemBL.ConductNameExists(tenHanhKiem))
                {
                    TenHanhKiemValidatorAdd.IsValid = false;
                    TxtTenHanhKiem.Focus();
                    MPEAdd.Show();
                    return;
                }
            }

            hanhKiemBL.InsertConduct(new DanhMuc_HanhKiem
            {
                TenHanhKiem = tenHanhKiem
            });

            MainDataPager.CurrentIndex = 1;
            BindData();

            this.TxtTenHanhKiem.Text = "";

            if (this.CkbAddAfterSave.Checked)
            {
                this.MPEAdd.Show();
            }
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            int maHanhKiem = Int32.Parse(this.HdfMaHanhKiem.Value);

            DanhMuc_HanhKiem conduct = new DanhMuc_HanhKiem();
            conduct.MaHanhKiem = maHanhKiem;

            hanhKiemBL.DeleteConduct(conduct);
            isSearch = false;
            BindData();
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender modalPopupEdit = new ModalPopupExtender();
            foreach (RepeaterItem rptItem in RptHanhKiem.Items)
            {
                if (rptItem.ItemType == ListItemType.Item || rptItem.ItemType == ListItemType.AlternatingItem)
                {
                    modalPopupEdit = (ModalPopupExtender)rptItem.FindControl("MPEEdit");
                    if (modalPopupEdit.ClientID == HdfRptHanhKiemMPEEdit.Value)
                    {
                        break;
                    }
                }
            }

            if (!Page.IsValid)
            {
                return;
            }
            
            string editedTenHanhKiem = (string)HdfEditedTenHanhKiem.Value;
            string newTenHanhKiem = TxtSuaTenHanhKiem.Text.Trim();

            if (newTenHanhKiem == "")
            {
                TenHanhKiemRequiredEdit.IsValid = false;
                modalPopupEdit.Show();
                return;
            }
            else
            {
                if (hanhKiemBL.ConductNameExists(editedTenHanhKiem, newTenHanhKiem))
                {
                    TenHanhKiemValidatorEdit.IsValid = false;
                    modalPopupEdit.Show();
                    return;
                }
            }

            int editedMaHanhKiem = Int32.Parse(this.HdfMaHanhKiem.Value);
            hanhKiemBL.UpdateConduct(editedTenHanhKiem, newTenHanhKiem);
            BindData();
        }
        #endregion

        #region Repeater event handlers
        protected void RptHanhKiem_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
                        DanhMuc_HanhKiem conduct = (DanhMuc_HanhKiem)e.Item.DataItem;

                        if (!hanhKiemBL.IsDeletable(conduct.TenHanhKiem))
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

        protected void RptHanhKiem_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDeleteItem":
                    {
                        this.LblConfirmDelete.Text = "Bạn có chắc xóa hạnh kiểm <b>" + e.CommandArgument + "</b> này không?";
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        HiddenField hdfRptMaHanhKiem = (HiddenField)e.Item.FindControl("HdfRptMaHanhKiem");
                        this.HdfMaHanhKiem.Value = hdfRptMaHanhKiem.Value;

                        this.HdfRptHanhKiemMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        string conductName = (string)e.CommandArgument;

                        DanhMuc_HanhKiem hanhKiem = hanhKiemBL.GetConduct(conductName);
                        ViewState[EDITED_MAHANHKIEM] = hanhKiem.MaHanhKiem;

                        TxtSuaTenHanhKiem.Text = hanhKiem.TenHanhKiem;
                        ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        mPEEdit.Show();

                        this.HdfRptHanhKiemMPEEdit.Value = mPEEdit.ClientID;
                        this.HdfMaHanhKiem.Value = conductName.ToString();
                        this.HdfEditedTenHanhKiem.Value = hanhKiem.TenHanhKiem;

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
            string tenHanhKiem = TxtSearchHanhKiem.Text.Trim();

            double totalRecords;
            List<DanhMuc_HanhKiem> lstHanhKiem;
            lstHanhKiem = hanhKiemBL.GetListConducts(tenHanhKiem, MainDataPager.CurrentIndex, MainDataPager.PageSize, 
                out totalRecords);
            MainDataPager.ItemCount = totalRecords;

            // Decrease page current index when delete
            if (lstHanhKiem.Count == 0 && MainDataPager.ItemCount != 0)
            {
                MainDataPager.CurrentIndex--;
                BindData();
                return;
            }

            bool bDisplayData = (lstHanhKiem.Count != 0) ? true : false;
            PnlPopupConfirmDelete.Visible = bDisplayData;
            PnlPopupEdit.Visible = bDisplayData;
            RptHanhKiem.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin hạnh kiểm";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy hạnh kiểm";
                }

                MainDataPager.CurrentIndex = 1;
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
            }
            else
            {
                MainDataPager.Visible = true;
            }

            RptHanhKiem.DataSource = lstHanhKiem;
            RptHanhKiem.DataBind();
        }
        #endregion
    }
}