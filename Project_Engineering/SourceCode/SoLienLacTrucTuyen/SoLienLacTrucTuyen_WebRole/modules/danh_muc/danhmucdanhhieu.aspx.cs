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
    public partial class LearningResultPage : BaseContentPage
    {
        #region Fields
        private DanhHieuBL danhHieuBL;
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

            danhHieuBL = new DanhHieuBL(UserSchool);

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
            string LearningResultName = this.TxtLearningResultName.Text.Trim();

            if (LearningResultName == "")
            {
                LearningResultNameRequiredAdd.IsValid = false;
                TxtLearningResultName.Focus();
                return;
            }
            else
            {
                if (danhHieuBL.DanhHieuExists(LearningResultName))
                {
                    LearningResultNameValidatorAdd.IsValid = false;
                    TxtLearningResultName.Focus();
                    return;
                }
            }

            danhHieuBL.InsertDanhHieu(LearningResultName, new Dictionary<int, int>());

            MainDataPager.CurrentIndex = 1;
            BindData();

            this.TxtLearningResultName.Text = "";

            if (this.CkbAddAfterSave.Checked)
            {
                
            }
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            int LearningResultId = Int32.Parse(this.HdfLearningResultId.Value);
            danhHieuBL.DeleteDanhHieu(LearningResultId);
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

            int LearningResultId = Int32.Parse(this.HdfLearningResultId.Value);
            string LearningResultName = TxtSuaLearningResultName.Text.Trim();

            if (LearningResultName == "")
            {
                LearningResultNameRequiredEdit.IsValid = false;
                modalPopupEdit.Show();
                return;
            }
            else
            {
                if (danhHieuBL.DanhHieuExists(LearningResultId, LearningResultName))
                {
                    LearningResultNameValidatorEdit.IsValid = false;
                    modalPopupEdit.Show();
                    return;
                }
            }

            danhHieuBL.UpdateDanhHieu(LearningResultId, LearningResultName);
            BindData();
        }
        #endregion

        #region Repeater event handlers
        protected void RptDanhHieu_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
                        Category_LearningResult DanhHieu = (Category_LearningResult)e.Item.DataItem;
                        if (!danhHieuBL.CanDeleteDanhHieu(DanhHieu.LearningResultId))
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

                        HiddenField hdfRptLearningResultId = (HiddenField)e.Item.FindControl("HdfRptLearningResultId");
                        this.HdfLearningResultId.Value = hdfRptLearningResultId.Value;

                        this.HdfRptDanhHieuMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        int LearningResultId = Int32.Parse(e.CommandArgument.ToString());
                        Category_LearningResult DanhHieu = danhHieuBL.GetDanhHieu(LearningResultId);

                        TxtSuaLearningResultName.Text = DanhHieu.LearningResultName;
                        ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        mPEEdit.Show();

                        this.HdfRptDanhHieuMPEEdit.Value = mPEEdit.ClientID;
                        this.HdfLearningResultId.Value = LearningResultId.ToString();

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
            string LearningResultName = TxtSearchDanhHieu.Text.Trim();

            double totalRecord;
            List<Category_LearningResult> lstDanhHieu = danhHieuBL.GetListDanhHieus(LearningResultName,
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