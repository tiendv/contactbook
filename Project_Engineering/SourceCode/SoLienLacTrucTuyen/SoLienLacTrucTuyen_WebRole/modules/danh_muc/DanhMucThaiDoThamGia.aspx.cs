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
    public partial class DanhMucThaiDoThamGia : System.Web.UI.Page
    {
        #region Fields
        private ThaiDoThamGiaBL thaiDoThamGiaBL;
        private bool isSearch;

        private List<AccessibilityEnum> lstAccessibilities;
        #endregion

        #region Page event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            RoleBL roleBL = new RoleBL();
            UserBL userBL = new UserBL();
            thaiDoThamGiaBL = new ThaiDoThamGiaBL();

            string pageUrl = Page.Request.Path;
            Guid role = userBL.GetRoleId(User.Identity.Name);

            if (!roleBL.ValidateAuthorization(role, pageUrl))
            {
                Response.Redirect((string)GetGlobalResourceObject("MainResource", "AccessDeniedPageUrl"));
                return;
            }

            Site masterPage = (Site)Page.Master;
            masterPage.UserRole = userBL.GetRoleId(User.Identity.Name);
            masterPage.PageUrl = Page.Request.Path;

            lstAccessibilities = roleBL.GetAccessibilities(role, pageUrl);

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
            string tenThaiDoThamGia = this.TxtTenThaiDoThamGia.Text.Trim();
            if (tenThaiDoThamGia == "")
            {
                TenThaiDoThamGiaRequiredAdd.IsValid = false;
                MPEAdd.Show();
                return;
            }
            else
            {
                if (thaiDoThamGiaBL.CheckExistTenThaiDoThamGia(0, tenThaiDoThamGia))
                {
                    TenThaiDoThamGiaValidatorAdd.IsValid = false;
                    MPEAdd.Show();
                    return;
                }
            }

            thaiDoThamGiaBL.InsertThaiDoThamGia(new DanhMuc_ThaiDoThamGia
            {
                TenThaiDoThamGia = tenThaiDoThamGia
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
            int maThaiDoThamGia = Int32.Parse(this.HdfMaThaiDoThamGia.Value);
            thaiDoThamGiaBL.DeleteThaiDoThamGia(maThaiDoThamGia);
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

            int maThaiDoThamGia = Int32.Parse(this.HdfMaThaiDoThamGia.Value);
            string tenThaiDoThamGia = this.TxtSuaTenThaiDoThamGia.Text.Trim();
            if (tenThaiDoThamGia == "")
            {
                TenThaiDoThamGiaRequiredEdit.IsValid = false;
                modalPopupEdit.Show();
                return;
            }
            else
            {
                if (thaiDoThamGiaBL.CheckExistTenThaiDoThamGia(maThaiDoThamGia, tenThaiDoThamGia))
                {
                    TenThaiDoThamGiaValidatorEdit.IsValid = false;
                    modalPopupEdit.Show();
                    return;
                }
            }

            DanhMuc_ThaiDoThamGia ThaiDoThamGia = new DanhMuc_ThaiDoThamGia
            {
                MaThaiDoThamGia = maThaiDoThamGia,
                TenThaiDoThamGia = TxtSuaTenThaiDoThamGia.Text,
            };
            thaiDoThamGiaBL.UpdateThaiDoThamGia(ThaiDoThamGia);
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
                        DanhMuc_ThaiDoThamGia ThaiDoThamGia = (DanhMuc_ThaiDoThamGia)e.Item.DataItem;
                        if (!thaiDoThamGiaBL.CheckCanDeleteThaiDoThamGia(ThaiDoThamGia.MaThaiDoThamGia))
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
                        int maThaiDoThamGia = Int32.Parse(e.CommandArgument.ToString());
                        DanhMuc_ThaiDoThamGia ThaiDoThamGia = thaiDoThamGiaBL.GetThaiDoThamGia(maThaiDoThamGia);

                        TxtSuaTenThaiDoThamGia.Text = ThaiDoThamGia.TenThaiDoThamGia;
                        ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        mPEEdit.Show();

                        this.HdfRptThaiDoThamGiaMPEEdit.Value = mPEEdit.ClientID;
                        this.HdfMaThaiDoThamGia.Value = maThaiDoThamGia.ToString();

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

            double totalRecords = 0;
            List<DanhMuc_ThaiDoThamGia> lstThaiDoThamGia;
            lstThaiDoThamGia = thaiDoThamGiaBL.GetListThaiDoThamGia(tenThaiDoThamGia,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out totalRecords);

            // Decrease page current index when delete
            if (lstThaiDoThamGia.Count == 0 && MainDataPager.ItemCount != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRepeater();
                return;
            }

            MainDataPager.ItemCount = totalRecords;

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