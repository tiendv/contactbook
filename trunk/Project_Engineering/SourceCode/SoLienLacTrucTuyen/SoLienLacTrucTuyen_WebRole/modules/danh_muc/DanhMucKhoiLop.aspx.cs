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
    public partial class GradeCategoryPage : BaseContentPage
    {
        #region Fields
        private GradeBL gradeBL;
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

            gradeBL = new GradeBL();

            if (!Page.IsPostBack)
            {
                isSearch = false;
                BindRptKhoiLop();
            }

            ProcPermissions();
        }
        #endregion

        #region Button click event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            isSearch = true;
            BindRptKhoiLop();
        }

        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {
            string tenKhoiLop = this.TxtTenKhoiLop.Text.Trim();
            string thuTuHienThi = this.TxtOrderAdd.Text.Trim();

            // validate input
            bool bValidInput = ValidateForAdd(tenKhoiLop, thuTuHienThi);

            if (bValidInput)
            {
                // insert new KhoiLop to DB
                gradeBL.InsertGrade(tenKhoiLop, short.Parse(thuTuHienThi));

                // Re-bind Repeater
                MainDataPager.CurrentIndex = 1;
                BindRptKhoiLop();

                // Reset GUI values
                this.TxtTenKhoiLop.Text = "";
                this.TxtOrderAdd.Text = "";

                // Process continue add KhoiLop
                if (this.CkbAddAfterSave.Checked)
                {
                    this.MPEAdd.Show();
                }
            }
        }        

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            string strGradeName = this.HdfSeletedGradeName.Value;
            gradeBL.DeleteGrade(strGradeName);
            isSearch = false;
            BindRptKhoiLop();
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            string editedGradeName = this.HdfSeletedGradeName.Value;
            string newGradeName = TxtSuaTenKhoiLop.Text.Trim();
            string newDisplayOrder = TxtOrderEdit.Text.Trim();

            bool bValidInput = ValidateForEdit(editedGradeName, newGradeName, newDisplayOrder);
            if (bValidInput)
            {
                short sNewDisplayOrder = short.Parse(newDisplayOrder);
                gradeBL.UpdateGrade(editedGradeName, newGradeName, sNewDisplayOrder);

                BindRptKhoiLop();
            }
        }
        #endregion

        #region Repeater event handlers
        protected void RptKhoiLop_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
                    DanhMuc_KhoiLop grade = (DanhMuc_KhoiLop)e.Item.DataItem;
                    if (!gradeBL.IsDeletable(grade.TenKhoiLop))
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

        protected void RptKhoiLop_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDeleteItem":
                    {
                        this.LblConfirmDelete.Text = "Bạn có chắc xóa khối lớp <b>" + e.CommandArgument + "</b> này không?";
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        HiddenField hdfRptMaKhoiLop = (HiddenField)e.Item.FindControl("HdfRptMaKhoiLop");
                        this.HdfMaKhoiLop.Value = hdfRptMaKhoiLop.Value;
                        this.HdfSeletedGradeName.Value = (string)e.CommandArgument;
                        this.HdfRptKhoiLopMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        this.HdfSeletedGradeName.Value = (string)e.CommandArgument;
                        string gradeName = (string)e.CommandArgument;

                        DanhMuc_KhoiLop grade = gradeBL.GetGrade(gradeName);

                        TxtSuaTenKhoiLop.Text = grade.TenKhoiLop;
                        TxtOrderEdit.Text = grade.ThuTuHienThi.ToString();

                        ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        mPEEdit.Show();

                        this.HdfRptKhoiLopMPEEdit.Value = mPEEdit.ClientID;

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
            BindRptKhoiLop();
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

        public void BindRptKhoiLop()
        {
            string tenKhoiLop = TxtSearchKhoiLop.Text.Trim();

            double totalRecords;
            List<DanhMuc_KhoiLop> lstKhoiLop = gradeBL.GetListGrades(tenKhoiLop, 
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out totalRecords);
            MainDataPager.ItemCount = totalRecords;

            // Decrease page current index when delete
            if (lstKhoiLop.Count == 0 && totalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRptKhoiLop();
                return;
            }

            bool bDisplayData = (lstKhoiLop.Count != 0) ? true : false;
            PnlPopupConfirmDelete.Visible = bDisplayData;
            PnlPopupEdit.Visible = bDisplayData;
            RptKhoiLop.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin khối lớp";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy khối lớp";
                }

                MainDataPager.CurrentIndex = 1;
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
            }
            else
            {
                MainDataPager.Visible = true;
            }

            RptKhoiLop.DataSource = lstKhoiLop;
            RptKhoiLop.DataBind();
        }

        private bool ValidateForAdd(string tenKhoiLop, string thuTuHienThi)
        {
            if (!Page.IsValid)
            {
                return false;
            }

            if (tenKhoiLop == "")
            {
                TenKhoiLopRequiredAdd.IsValid = false;
                MPEAdd.Show();
                return false;
            }
            else
            {
                if (gradeBL.GradeNameExists(tenKhoiLop))
                {
                    TenKhoiLopValidatorAdd.IsValid = false;
                    MPEAdd.Show();
                    return false;
                }
                else
                {
                    try
                    {
                        short.Parse(thuTuHienThi);
                    }
                    catch (Exception)
                    {
                        OrderRequiredEdit.IsValid = false;
                        MPEAdd.Show();
                        return false;
                    }
                }
            }

            return true;
        }

        private bool ValidateForEdit(string editedGradeName, string newGradeName, string newDisplayOrder)
        {
            if (!Page.IsValid)
            {
                return false;
            }

            ModalPopupExtender modalPopupEdit = new ModalPopupExtender();
            foreach (RepeaterItem rptItem in RptKhoiLop.Items)
            {
                if (rptItem.ItemType == ListItemType.Item || rptItem.ItemType == ListItemType.AlternatingItem)
                {
                    modalPopupEdit = (ModalPopupExtender)rptItem.FindControl("MPEEdit");
                    if (modalPopupEdit.ClientID == HdfRptKhoiLopMPEEdit.Value)
                    {
                        break;
                    }
                }
            }


            if (newGradeName == "")
            {
                TenKhoiLopRequiredEdit.IsValid = false;
                modalPopupEdit.Show();
                return false;
            }
            else
            {
                if (gradeBL.GradeNameExists(editedGradeName, newGradeName))
                {
                    TenKhoiLopValidatorEdit.IsValid = false;
                    modalPopupEdit.Show();
                    return false;
                }
                else
                {
                    try
                    {
                        short.Parse(newDisplayOrder);
                    }
                    catch (Exception)
                    {
                        OrderRequiredEdit.IsValid = false;
                        modalPopupEdit.Show();
                        return false;
                    }
                }
            }

            return true;
        }
        #endregion
    }
}