using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using AjaxControlToolkit;
using EContactBook.BusinessEntity;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class DanhMucHanhKiem : BaseContentPage
    {
        private const string EDITED_ConductId = "EditedConductId";

        #region Fields
        private ConductBL hanhKiemBL;
        private bool isSearch;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (accessDenied)
            {
                return;
            }

            if (sessionExpired)
            {
                FormsAuthentication.SignOut();
                Response.Redirect(FormsAuthentication.LoginUrl);
            }

            hanhKiemBL = new ConductBL(UserSchool);

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
            string ConductName = this.TxtConductName.Text;

            if (ConductName == "")
            {
                ConductNameRequiredAdd.IsValid = false;
                TxtConductName.Focus();
                MPEAdd.Show();
                return;
            }
            else
            {
                if (hanhKiemBL.ConductNameExists(ConductName))
                {
                    ConductNameValidatorAdd.IsValid = false;
                    TxtConductName.Focus();
                    MPEAdd.Show();
                    return;
                }
            }

            hanhKiemBL.InsertConduct(new Category_Conduct
            {
                ConductName = ConductName
            });

            MainDataPager.CurrentIndex = 1;
            BindData();

            this.TxtConductName.Text = "";

            if (this.CkbAddAfterSave.Checked)
            {
                this.MPEAdd.Show();
            }
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            int ConductId = Int32.Parse(this.HdfConductId.Value);

            Category_Conduct conduct = new Category_Conduct();
            conduct.ConductId = ConductId;

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
            
            string editedConductName = (string)HdfEditedConductName.Value;
            string newConductName = TxtSuaConductName.Text.Trim();

            if (newConductName == "")
            {
                ConductNameRequiredEdit.IsValid = false;
                modalPopupEdit.Show();
                return;
            }
            else
            {
                if (hanhKiemBL.ConductNameExists(editedConductName, newConductName))
                {
                    ConductNameValidatorEdit.IsValid = false;
                    modalPopupEdit.Show();
                    return;
                }
            }

            int editedConductId = Int32.Parse(this.HdfConductId.Value);
            hanhKiemBL.UpdateConduct(editedConductName, newConductName);
            BindData();
        }
        #endregion

        #region Repeater event handlers
        protected void RptHanhKiem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (accessibilities.Contains(AccessibilityEnum.Modify))
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

            if (accessibilities.Contains(AccessibilityEnum.Delete))
            {
                if (e.Item.ItemType == ListItemType.Item
                    || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (e.Item.DataItem != null)
                    {
                        Category_Conduct conduct = (Category_Conduct)e.Item.DataItem;

                        if (!hanhKiemBL.IsDeletable(conduct.ConductName))
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

                        HiddenField hdfRptConductId = (HiddenField)e.Item.FindControl("HdfRptConductId");
                        this.HdfConductId.Value = hdfRptConductId.Value;

                        this.HdfRptHanhKiemMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        string conductName = (string)e.CommandArgument;

                        Category_Conduct conduct = hanhKiemBL.GetConduct(conductName);
                        ViewState[EDITED_ConductId] = conduct.ConductId;

                        TxtSuaConductName.Text = conduct.ConductName;
                        ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        mPEEdit.Show();

                        this.HdfRptHanhKiemMPEEdit.Value = mPEEdit.ClientID;
                        this.HdfConductId.Value = conduct.ConductId.ToString();
                        this.HdfEditedConductName.Value = conduct.ConductName;

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
            if (accessibilities.Contains(AccessibilityEnum.Add))
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
            string strConductName = TxtSearchHanhKiem.Text.Trim();

            double dTotalRecords;
            List<Category_Conduct> conducts;
            conducts = hanhKiemBL.GetListConducts(strConductName, MainDataPager.CurrentIndex, MainDataPager.PageSize, 
                out dTotalRecords);
            MainDataPager.ItemCount = dTotalRecords;

            // Decrease page current index when delete
            if (conducts.Count == 0 && MainDataPager.ItemCount != 0)
            {
                MainDataPager.CurrentIndex--;
                BindData();
                return;
            }

            bool bDisplayData = (conducts.Count != 0) ? true : false;
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

            RptHanhKiem.DataSource = conducts;
            RptHanhKiem.DataBind();
        }
        #endregion
    }
}