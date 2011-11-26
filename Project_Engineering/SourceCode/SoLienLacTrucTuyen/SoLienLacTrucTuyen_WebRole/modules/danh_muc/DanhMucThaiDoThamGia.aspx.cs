using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;
using AjaxControlToolkit;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class DanhMucThaiDoThamGia : BaseContentPage
    {
        #region Fields
        private AttitudeBL attitudeBL;
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

            attitudeBL = new AttitudeBL(UserSchool);
            if (!Page.IsPostBack)
            {
                isSearch = false;
                MainDataPager.CurrentIndex = 1;
                BindRepeater();
            }

            ProcPermissions();
        }
        #endregion

        #region Button click event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            isSearch = true;
            BindRepeater();
        }

        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {
            string strAttitudeName = this.TxtTenThaiDoThamGia.Text.Trim();
            if (strAttitudeName == "")
            {
                TenThaiDoThamGiaRequiredAdd.IsValid = false;
                MPEAdd.Show();
                return;
            }
            else
            {
                if (attitudeBL.AttitudeNameExists(strAttitudeName))
                {
                    TenThaiDoThamGiaValidatorAdd.IsValid = false;
                    MPEAdd.Show();
                    return;
                }
            }

            attitudeBL.InsertThaiDoThamGia(new DanhMuc_ThaiDoThamGia
            {
                TenThaiDoThamGia = strAttitudeName
            });

            MainDataPager.CurrentIndex = 1;
            BindRepeater();

            this.TxtTenThaiDoThamGia.Text = "";

            if (this.CkbAddAfterSave.Checked)
            {
                this.MPEAdd.Show();
            }
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            int attitudeId = Int32.Parse(this.HdfMaThaiDoThamGia.Value);
            DanhMuc_ThaiDoThamGia attitude = new DanhMuc_ThaiDoThamGia();
            attitude.MaThaiDoThamGia = attitudeId;
            attitudeBL.DeleteAttitude(attitude);
            isSearch = false;
            BindRepeater();
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender modalPopupEdit = new ModalPopupExtender();
            foreach (RepeaterItem rptItem in RptThaiDoThamGia.Items)
            {
                if (rptItem.ItemType == ListItemType.Item || rptItem.ItemType == ListItemType.AlternatingItem)
                {
                    modalPopupEdit = (ModalPopupExtender)rptItem.FindControl("MPEEdit");
                    if (modalPopupEdit.ClientID == HdfRptThaiDoThamGiaMPEEdit.Value)
                    {
                        break;
                    }
                }
            }

            int attitudeId = Int32.Parse(this.HdfMaThaiDoThamGia.Value);
            string oldAttitudeName = this.HdfSltAttitudeName.Value;
            string newAttitudeName = this.TxtSuaTenThaiDoThamGia.Text.Trim();
            if (newAttitudeName == "")
            {
                TenThaiDoThamGiaRequiredEdit.IsValid = false;
                modalPopupEdit.Show();
                return;
            }
            else
            {
                if (attitudeBL.AttitudeNameExists(oldAttitudeName, newAttitudeName))
                {
                    TenThaiDoThamGiaValidatorEdit.IsValid = false;
                    modalPopupEdit.Show();
                    return;
                }
            }

            DanhMuc_ThaiDoThamGia attitude = new DanhMuc_ThaiDoThamGia();
            attitude.MaThaiDoThamGia = attitudeId;
            attitudeBL.UpdateAttitude(attitude, newAttitudeName);
            BindRepeater();
        }
        #endregion

        #region Repeater event handlers
        protected void RptThaiDoThamGia_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
                        DanhMuc_ThaiDoThamGia attitude = (DanhMuc_ThaiDoThamGia)e.Item.DataItem;
                        if (!attitudeBL.IsDeletable(attitude))
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

        protected void RptThaiDoThamGia_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDeleteItem":
                    {
                        this.LblConfirmDelete.Text = "Bạn có chắc xóa thái độ tham gia <b>" + e.CommandArgument + "</b> này không?";
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        HiddenField hdfRptMaThaiDoThamGia = (HiddenField)e.Item.FindControl("HdfRptMaThaiDoThamGia");
                        this.HdfMaThaiDoThamGia.Value = hdfRptMaThaiDoThamGia.Value;

                        this.HdfRptThaiDoThamGiaMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        int attitudeId = Int32.Parse(e.CommandArgument.ToString());
                        DanhMuc_ThaiDoThamGia attitude = attitudeBL.GetAttitude(attitudeId);

                        TxtSuaTenThaiDoThamGia.Text = attitude.TenThaiDoThamGia;
                        ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        mPEEdit.Show();

                        this.HdfRptThaiDoThamGiaMPEEdit.Value = mPEEdit.ClientID;
                        this.HdfMaThaiDoThamGia.Value = attitudeId.ToString();
                        this.HdfSltAttitudeName.Value = attitude.TenThaiDoThamGia;
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

        public void BindRepeater()
        {
            string tenThaiDoThamGia = TxtSearchThaiDoThamGia.Text.Trim();

            double dTotalRecords = 0;
            List<DanhMuc_ThaiDoThamGia> lstThaiDoThamGia;
            lstThaiDoThamGia = attitudeBL.GetListAttitudes(tenThaiDoThamGia,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            // Decrease page current index when delete
            if (lstThaiDoThamGia.Count == 0 && MainDataPager.ItemCount != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRepeater();
                return;
            }

            MainDataPager.ItemCount = dTotalRecords;

            bool bDisplayData = (lstThaiDoThamGia.Count != 0) ? true : false;
            PnlPopupConfirmDelete.Visible = bDisplayData;
            PnlPopupEdit.Visible = bDisplayData;
            RptThaiDoThamGia.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin thái độ tham gia";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy thái độ tham gia";
                }

                MainDataPager.CurrentIndex = 1;
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
            }
            else
            {
                MainDataPager.Visible = true;
            }

            RptThaiDoThamGia.DataSource = lstThaiDoThamGia;
            RptThaiDoThamGia.DataBind();
        }
        #endregion
    }
}