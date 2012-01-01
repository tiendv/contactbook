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
            if (accessDenied)
            {
                return;
            }
            
            if (sessionExpired)
            {
                FormsAuthentication.SignOut();
                Response.Redirect(FormsAuthentication.LoginUrl);
            }

            gradeBL = new GradeBL(UserSchool);

            if (!Page.IsPostBack)
            {
                isSearch = false;
                BindRptGrades();
            }

            ProcPermissions();
        }
        #endregion

        #region Button click event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            isSearch = true;
            BindRptGrades();
        }

        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {
            string GradeName = this.TxtGradeName.Text.Trim();
            string thuTuHienThi = this.TxtOrderAdd.Text.Trim();

            // validate input
            bool bValidInput = ValidateForAdd(GradeName, thuTuHienThi);

            if (bValidInput)
            {
                // insert new KhoiLop to DB
                gradeBL.InsertGrade(GradeName, short.Parse(thuTuHienThi));

                // Re-bind Repeater
                MainDataPager.CurrentIndex = 1;
                BindRptGrades();

                // Reset GUI values
                this.TxtGradeName.Text = "";
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
            BindRptGrades();
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            string editedGradeName = this.HdfSeletedGradeName.Value;
            string newGradeName = TxtSuaGradeName.Text.Trim();
            string newDisplayOrder = TxtOrderEdit.Text.Trim();

            bool bValidInput = ValidateForEdit(editedGradeName, newGradeName, newDisplayOrder);
            if (bValidInput)
            {
                short sNewDisplayOrder = short.Parse(newDisplayOrder);
                gradeBL.UpdateGrade(editedGradeName, newGradeName, sNewDisplayOrder);

                BindRptGrades();
            }
        }
        #endregion

        #region Repeater event handlers
        protected void RptKhoiLop_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
                    Category_Grade grade = (Category_Grade)e.Item.DataItem;
                    if (!gradeBL.IsDeletable(grade.GradeName))
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

                        HiddenField hdfRptGradeId = (HiddenField)e.Item.FindControl("HdfRptGradeId");
                        this.HdfGradeId.Value = hdfRptGradeId.Value;
                        this.HdfSeletedGradeName.Value = (string)e.CommandArgument;
                        this.HdfRptKhoiLopMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        this.HdfSeletedGradeName.Value = (string)e.CommandArgument;
                        string gradeName = (string)e.CommandArgument;

                        Category_Grade grade = gradeBL.GetGrade(gradeName);

                        TxtSuaGradeName.Text = grade.GradeName;
                        TxtOrderEdit.Text = grade.DisplayedOrder.ToString();

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
            BindRptGrades();
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

        public void BindRptGrades()
        {
            string strGradeName = TxtSearchKhoiLop.Text.Trim();

            double dTotalRecords;
            List<Category_Grade> grades = gradeBL.GetListGrades(strGradeName, 
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);
            MainDataPager.ItemCount = dTotalRecords;

            // Decrease page current index when delete
            if (grades.Count == 0 && dTotalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRptGrades();
                return;
            }

            bool bDisplayData = (grades.Count != 0) ? true : false;
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

            RptKhoiLop.DataSource = grades;
            RptKhoiLop.DataBind();
        }

        private bool ValidateForAdd(string GradeName, string thuTuHienThi)
        {
            if (!Page.IsValid)
            {
                return false;
            }

            if (GradeName == "")
            {
                GradeNameRequiredAdd.IsValid = false;
                MPEAdd.Show();
                return false;
            }
            else
            {
                if (gradeBL.GradeNameExists(GradeName))
                {
                    GradeNameValidatorAdd.IsValid = false;
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
                GradeNameRequiredEdit.IsValid = false;
                modalPopupEdit.Show();
                return false;
            }
            else
            {
                if (gradeBL.GradeNameExists(editedGradeName, newGradeName))
                {
                    GradeNameValidatorEdit.IsValid = false;
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