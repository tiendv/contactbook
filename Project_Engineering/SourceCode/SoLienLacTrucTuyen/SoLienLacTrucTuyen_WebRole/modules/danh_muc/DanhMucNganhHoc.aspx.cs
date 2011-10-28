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
using SoLienLacTrucTuyen_WebRole.Modules;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen_WebRole
{
    public partial class DanhMucNganhHoc : System.Web.UI.Page
    {
        #region Fields
        private List<AccessibilityEnum> lstAccessibilities;
        private FacultyBL nganhhocBL = new FacultyBL();
        private bool isSearch;
        #endregion

        #region Page event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            RoleBL roleBL = new RoleBL();
            UserBL userBL = new UserBL();

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

                ViewState["SortColumn"] = "TenNganhHoc";
                ViewState["SortOrder"] = "ASC";
                BindData();
            }

            ProcPermissions();
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
            string tenNganhHoc = TxtSearchNganhHoc.Text.Trim();
            double totalRecords;
            List<DanhMuc_NganhHoc> lstNganhHoc = nganhhocBL.GetListNganhHoc(tenNganhHoc, 
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out totalRecords);
            MainDataPager.ItemCount = totalRecords;

            // Decrease page current index when delete
            if (lstNganhHoc.Count == 0 && MainDataPager.ItemCount != 0)
            {
                MainDataPager.CurrentIndex--;
                BindData();
                return;
            }

            bool bDisplayData = (lstNganhHoc.Count != 0) ? true : false;
            PnlPopupConfirmDelete.Visible = bDisplayData;
            PnlPopupEdit.Visible = bDisplayData;
            RptNganhHoc.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin ngành học";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy ngành học";
                }

                MainDataPager.CurrentIndex = 1;
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
            }
            else
            {
                MainDataPager.Visible = true;
            }

            RptNganhHoc.DataSource = lstNganhHoc;
            RptNganhHoc.DataBind();
        }
        #endregion

        #region Button click event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            MainDataPager.ItemCount = 0;
            isSearch = true;
            BindData();
        }

        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            string tenNganhHoc = this.TxtTenNganhHoc.Text.Trim();

            if (tenNganhHoc == "")
            {
                TenNganhHocRequiredAdd.IsValid = false;
                MPEAdd.Show();
                return;
            }
            else
            {
                if (nganhhocBL.NganhHocExists(tenNganhHoc))
                {
                    TenNganhHocValidatorAdd.IsValid = false;
                    MPEAdd.Show();
                    return;
                }
            }

            string motaNganhHoc = this.TxtMoTaNganhHoc.Text.Trim();
            Faculty faculty = new Faculty(tenNganhHoc, motaNganhHoc);
            nganhhocBL.InsertFaculty(faculty);

            MainDataPager.CurrentIndex = 1;
            BindData();

            this.TxtTenNganhHoc.Text = "";
            this.TxtMoTaNganhHoc.Text = "";

            if (this.CkbAddAfterSave.Checked)
            {
                this.MPEAdd.Show();
            }
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            int maNganhHoc = Int32.Parse(this.HdfMaNganhHoc.Value);
            nganhhocBL.DeleteNganhHoc(maNganhHoc);
            isSearch = false;
            BindData();
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender modalPopupEdit;
            foreach (RepeaterItem rptItem in RptNganhHoc.Items)
            {
                if (rptItem.ItemType == ListItemType.Item || rptItem.ItemType == ListItemType.AlternatingItem)
                {
                    modalPopupEdit = (ModalPopupExtender)rptItem.FindControl("MPEEdit");
                    if (modalPopupEdit.ClientID == HdfRptNganhHocMPEEdit.Value)
                    {
                        break;
                    }
                }
            }

            if (!Page.IsValid)
            {
                return;
            }

            int maNganhHoc = Int32.Parse(this.HdfMaNganhHoc.Value);
            string tenNganhHoc = this.TxtTenNganhHocEdit.Text.Trim();

            if (tenNganhHoc == "")
            {
                TenNganhHocRequiredEdit.IsValid = false;
                modalPopupEdit = (ModalPopupExtender)Page.FindControl(HdfRptNganhHocMPEEdit.Value);
                modalPopupEdit.Show();
                return;
            }
            else
            {
                if (nganhhocBL.NganhHocExists(maNganhHoc, tenNganhHoc))
                {
                    TenNganhHocValidatorEdit.IsValid = false;
                    modalPopupEdit = (ModalPopupExtender)Page.FindControl(HdfRptNganhHocMPEEdit.Value);
                    modalPopupEdit.Show();
                    return;
                }
            }

            nganhhocBL.UpdateNganhHoc(new DanhMuc_NganhHoc()
            {
                MaNganhHoc = maNganhHoc,
                TenNganhHoc = TxtTenNganhHocEdit.Text,
                MoTa = TxtSuaMoTaNganhHoc.Text
            });

            BindData();
        }
        #endregion

        #region Repeater event handlers
        protected void RptNganhHoc_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
                    DanhMuc_NganhHoc nganhhoc = (DanhMuc_NganhHoc)e.Item.DataItem;
                    if (!nganhhocBL.CheckCanDeleteNganhHoc(nganhhoc.MaNganhHoc))
                    {
                        ImageButton btnDeleteItem = (ImageButton)e.Item.FindControl("BtnDeleteItem");
                        btnDeleteItem.ImageUrl = "~/Styles/Images/button_delete_disable.png";
                        btnDeleteItem.Enabled = false;
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

        protected void RptNganhHoc_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDeleteItem":
                    {
                        this.LblConfirmDelete.Text = "Bạn có chắc xóa ngành học <b>" + e.CommandArgument + "</b> này không?";
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        HiddenField hdfRptMaNganhHoc = (HiddenField)e.Item.FindControl("HdfRptMaNganhHoc");
                        this.HdfMaNganhHoc.Value = hdfRptMaNganhHoc.Value;

                        this.HdfRptNganhHocMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        int maNganhHoc = Int32.Parse(e.CommandArgument.ToString());
                        DanhMuc_NganhHoc nganhhoc = nganhhocBL.GetNganhHoc(maNganhHoc);

                        TxtTenNganhHocEdit.Text = nganhhoc.TenNganhHoc;
                        TxtSuaMoTaNganhHoc.Text = nganhhoc.MoTa;
                        ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        mPEEdit.Show();

                        this.HdfRptNganhHocMPEEdit.Value = mPEEdit.ClientID;
                        this.HdfMaNganhHoc.Value = maNganhHoc.ToString();

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
    }
}