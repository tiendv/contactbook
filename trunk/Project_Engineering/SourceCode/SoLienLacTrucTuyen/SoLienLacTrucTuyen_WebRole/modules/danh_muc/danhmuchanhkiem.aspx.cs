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
    public partial class DanhMucHanhKiem : System.Web.UI.Page
    {
        #region Fields
        private List<AccessibilityEnum> lstAccessibilities;
        private HanhKiemBL hanhKiemBL = new HanhKiemBL();
        private bool isSearch;
        #endregion

        #region Page event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            UserBL userBL = new UserBL();
            RoleBL roleBL = new RoleBL();

            string pageUrl = Page.Request.Path;
            Guid role = userBL.GetRoleId(User.Identity.Name);

            if (!roleBL.ValidateAuthorization(role, pageUrl))
            {
                Response.Redirect((string)GetGlobalResourceObject("MainResource", "AccessDeniedPageUrl"));
                return;
            }

            Site masterPage = (Site)Page.Master;
            masterPage.UserRole = role;
            masterPage.PageUrl = pageUrl;

            lstAccessibilities = roleBL.GetAccessibilities(role, pageUrl);

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
                if (hanhKiemBL.CheckExistTenHanhKiem(0, tenHanhKiem))
                {
                    TenHanhKiemValidatorAdd.IsValid = false;
                    TxtTenHanhKiem.Focus();
                    MPEAdd.Show();
                    return;
                }
            }

            hanhKiemBL.InsertHanhKiem(new DanhMuc_HanhKiem
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
            hanhKiemBL.DeleteHanhKiem(maHanhKiem);
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

            int maHanhKiem = Int32.Parse(this.HdfMaHanhKiem.Value);
            string tenHanhKiem = TxtSuaTenHanhKiem.Text.Trim();

            if (tenHanhKiem == "")
            {
                TenHanhKiemRequiredEdit.IsValid = false;
                modalPopupEdit.Show();
                return;
            }
            else
            {
                if (hanhKiemBL.CheckExistTenHanhKiem(maHanhKiem, tenHanhKiem))
                {
                    TenHanhKiemValidatorEdit.IsValid = false;
                    modalPopupEdit.Show();
                    return;
                }
            }

            DanhMuc_HanhKiem HanhKiem = new DanhMuc_HanhKiem
            {
                MaHanhKiem = maHanhKiem,
                TenHanhKiem = tenHanhKiem,
            };
            hanhKiemBL.UpdateHanhKiem(HanhKiem);
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
                        DanhMuc_HanhKiem HanhKiem = (DanhMuc_HanhKiem)e.Item.DataItem;
                        if (!hanhKiemBL.CheckCanDeleteHanhKiem(HanhKiem.MaHanhKiem))
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
                        int maHanhKiem = Int32.Parse(e.CommandArgument.ToString());
                        DanhMuc_HanhKiem HanhKiem = hanhKiemBL.GetHanhKiem(maHanhKiem);

                        TxtSuaTenHanhKiem.Text = HanhKiem.TenHanhKiem;
                        ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        mPEEdit.Show();

                        this.HdfRptHanhKiemMPEEdit.Value = mPEEdit.ClientID;
                        this.HdfMaHanhKiem.Value = maHanhKiem.ToString();

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

            List<DanhMuc_HanhKiem> lstHanhKiem;
            if (String.Compare(tenHanhKiem, "tất cả", true) == 0 || tenHanhKiem == "")
            {
                lstHanhKiem = hanhKiemBL.GetListHanhKiem(MainDataPager.CurrentIndex, MainDataPager.PageSize);
                MainDataPager.ItemCount = hanhKiemBL.GetHanhKiemCount();
            }
            else
            {
                lstHanhKiem = hanhKiemBL.GetListHanhKiem(tenHanhKiem, MainDataPager.CurrentIndex, MainDataPager.PageSize);
                MainDataPager.ItemCount = hanhKiemBL.GetHanhKiemCount(tenHanhKiem);
            }

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