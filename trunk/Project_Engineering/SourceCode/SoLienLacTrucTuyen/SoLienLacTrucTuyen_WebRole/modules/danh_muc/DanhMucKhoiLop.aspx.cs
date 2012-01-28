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
            bool bInfoInUse = false;
            CheckBox ckbxSelect = null;
            HiddenField HdfRptGradeId = null;
            Category_Grade grade = null;

            foreach (RepeaterItem item in RptKhoiLop.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    ckbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (ckbxSelect.Checked)
                    {
                        HdfRptGradeId = (HiddenField)item.FindControl("HdfRptGradeId");
                        grade = new Category_Grade();
                        grade.GradeId = Int32.Parse(HdfRptGradeId.Value);

                        if (gradeBL.IsDeletable(grade))
                        {
                            gradeBL.DeleteGrade(grade);
                        }
                        else
                        {
                            bInfoInUse = true;
                        }
                    }
                }
            }

            isSearch = false;
            BindRptGrades();

            if (bInfoInUse)
            {
                MPEInfoInUse.Show();
            }
        }

        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            HiddenField hdfRptGradeId = null;
            foreach (RepeaterItem item in RptKhoiLop.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox CkbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (CkbxSelect.Checked)
                    {
                        hdfRptGradeId = (HiddenField)item.FindControl("HdfRptGradeId");
                        Category_Grade grade = gradeBL.GetGrade(Int32.Parse(hdfRptGradeId.Value));
                        TxtSuaGradeName.Text = grade.GradeName;
                        TxtOrderEdit.Text = grade.DisplayedOrder.ToString();
                        HdfSeletedGradeName.Value = grade.GradeName;

                        MPEEdit.Show();
                        return;
                    }
                }
            }
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
            BtnAdd.Visible = accessibilities.Contains(AccessibilityEnum.Add);
            PnlPopupAdd.Visible = accessibilities.Contains(AccessibilityEnum.Add);
            BtnEdit.Visible = accessibilities.Contains(AccessibilityEnum.Modify);
            BtnDelete.Visible = accessibilities.Contains(AccessibilityEnum.Delete);
            PnlPopupConfirmDelete.Visible = accessibilities.Contains(AccessibilityEnum.Delete);
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

        private bool ValidateForAdd(string gradeName, string displayOrder)
        {
            if (!Page.IsValid)
            {
                return false;
            }

            if (CheckUntils.IsNullOrBlank(gradeName))
            {
                GradeNameRequiredAdd.IsValid = false;
                MPEAdd.Show();
                return false;
            }
            else
            {
                if (gradeBL.GradeNameExists(gradeName))
                {
                    GradeNameValidatorAdd.IsValid = false;
                    MPEAdd.Show();
                    return false;
                }
                else
                {
                    try
                    {
                        short.Parse(displayOrder);
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
           
            if (CheckUntils.IsNullOrBlank(newGradeName))
            {
                GradeNameRequiredEdit.IsValid = false;
                MPEEdit.Show();
                return false;
            }
            else
            {
                if (gradeBL.GradeNameExists(editedGradeName, newGradeName))
                {
                    GradeNameValidatorEdit.IsValid = false;
                    MPEEdit.Show();
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
                        MPEEdit.Show();
                        return false;
                    }
                }
            }

            return true;
        }
        #endregion

        #region Repeater event handlers
        protected void RptGrades_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                e.Item.FindControl("thSelectAll").Visible = (accessibilities.Contains(AccessibilityEnum.Modify) || accessibilities.Contains(AccessibilityEnum.Delete));
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.FindControl("tdSelect").Visible = (accessibilities.Contains(AccessibilityEnum.Modify) || accessibilities.Contains(AccessibilityEnum.Delete));
            }
        }
        #endregion
    }
}