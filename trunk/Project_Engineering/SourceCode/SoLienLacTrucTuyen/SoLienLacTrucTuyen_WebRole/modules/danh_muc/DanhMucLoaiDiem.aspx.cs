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
using SoLienLacTrucTuyen;
using System.Text.RegularExpressions;
using EContactBook.BusinessEntity;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class DanhMucLoaiDiem : BaseContentPage
    {
        #region Fields
        private MarkTypeBL loaiDiemBL;
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

            loaiDiemBL = new MarkTypeBL(UserSchool);

            if (!Page.IsPostBack)
            {
                BindDDLGrades();

                if (DdlGradeSearch.Items.Count != 0)
                {
                    isSearch = false;
                    PagerMain.CurrentIndex = 1;
                    BindRptLoaiDiem();
                }
            }

            ProcPermissions();
        }
        #endregion

        #region Methods
        private void ProcPermissions()
        {
            if (accessibilities.Contains(AccessibilityEnum.Add))
            {
                BtnAdd.Enabled = true;
                BtnAdd.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_ADD;
                PnlPopupAdd.Visible = true;
            }
            else
            {
                BtnAdd.Enabled = false;
                BtnAdd.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_ADD_DISABLE;
                PnlPopupAdd.Visible = false;
            }

            if (accessibilities.Contains(AccessibilityEnum.Modify))
            {
                BtnEdit.Enabled = true;
                BtnEdit.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_MODIFY;
                PnlPopupEdit.Visible = true;
            }
            else
            {
                BtnEdit.Enabled = false;
                BtnEdit.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_MODIFY_DISABLED;
                PnlPopupEdit.Visible = false;
            }

            if (accessibilities.Contains(AccessibilityEnum.Delete))
            {
                BtnDelete.Enabled = true;
                BtnDelete.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_EXPORT;
                PnlPopupConfirmDelete.Visible = true;
            }
            else
            {
                BtnDelete.Enabled = false;
                BtnDelete.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_DELETE_DISABLED;
                PnlPopupConfirmDelete.Visible = false;
            }
        }

        public void BindRptLoaiDiem()
        {
            string MarkTypeName = TxtSearchLoaiDiem.Text.Trim();
            double dTotalRecords = 0;
            Category_Grade grade = new Category_Grade();
            grade.GradeId = Int32.Parse(DdlGradeSearch.SelectedValue);

            List<TabularMarkType> tabularMarkTypes = loaiDiemBL.GetTabularMarkTypes(
                MarkTypeName, grade, PagerMain.CurrentIndex, PagerMain.PageSize, out dTotalRecords);
            PagerMain.ItemCount = dTotalRecords;

            // Decrease page current index when delete
            if (tabularMarkTypes.Count == 0 && dTotalRecords != 0)
            {
                PagerMain.CurrentIndex--;
                BindRptLoaiDiem();
                return;
            }

            bool bDisplayData = (tabularMarkTypes.Count != 0) ? true : false;           
            RptLoaiDiem.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin loại điểm";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy loại điểm";
                }

                PagerMain.CurrentIndex = 1;
                PagerMain.ItemCount = 0;
                PagerMain.Visible = false;
            }
            else
            {
                PagerMain.Visible = true;
            }

            RptLoaiDiem.DataSource = tabularMarkTypes;
            RptLoaiDiem.DataBind();
        }

        private bool ValidateForAdd(Category_Grade grade, string MarkTypeName, string maxMarksPerTerm)
        {
            if (CheckUntils.IsNullOrBlank(MarkTypeName))
            {
                MarkTypeNameRequiredAdd.IsValid = false;
                TxtMarkTypeName.Focus();
                MPEAdd.Show();
                return false;
            }
            else
            {
                if (!Regex.IsMatch(TxtMarkRatioLoaiDiemAdd.Text.Trim(), MarkRatioRegExpAdd.ValidationExpression))
                {
                    MarkRatioRegExpAdd.IsValid = false;
                    MPEAdd.Show();
                    return false;
                }
                else
                {
                    if (loaiDiemBL.MarkTypeNameExists(grade, MarkTypeName))
                    {
                        MarkTypeNameValidatorAdd.IsValid = false;
                        TxtMarkTypeName.Focus();
                        MPEAdd.Show();
                        return false;
                    }
                }
            }

            try
            {
                short.Parse(maxMarksPerTerm);
            }
            catch (Exception)
            {
                MaxMarksPerTermRequiredAdd.IsValid = false;
                MPEAdd.Show();
                return false;
            }


            if (RbtnYesAdd.Checked)
            {
                Category_MarkType appliedCalAvgMarkType = loaiDiemBL.GetAppliedCalAvgMarkType(grade);
                if (appliedCalAvgMarkType != null)
                {
                    LblAppCalAvgMarkAdd.Visible = true;
                    MPEAdd.Show();
                    return false;
                }
            }
            else
            {
                LblAppCalAvgMarkAdd.Visible = false;
            }
            return true;
        }

        private bool ValidateForEdit(Category_Grade grade, string editedMarkTypeName, string markTypeName, string maxMarksPerTerm)
        {
            // validate page
            if (!Page.IsValid)
            {
                return false;
            }

            // validate blank
            if (markTypeName == "")
            {
                MarkTypeNameRequiredEdit.IsValid = false;
                MPEEdit.Show();
                return false;
            }
            else
            {
                if (!Regex.IsMatch(TxtMarkRatioLoaiDiemSua.Text.Trim(), MarkRatioRegExpEdit.ValidationExpression))
                {
                    MarkRatioRegExpEdit.IsValid = false;
                    MPEEdit.Show();
                    return false;
                }
                else
                {
                    if (loaiDiemBL.MarkTypeNameExists(grade, editedMarkTypeName, markTypeName))
                    {
                        MarkTypeNameValidatorEdit.IsValid = false;
                        MPEEdit.Show();
                        return false;
                    }
                }
            }

            try
            {
                short.Parse(maxMarksPerTerm);
            }
            catch (Exception)
            {
                MaxMarksPerTermRequiredEdit.IsValid = false;
                MPEEdit.Show();
                return false;
            }

            if (RbtnYesEdit.Checked)
            {
                Category_MarkType appliedCalAvgMarkType = loaiDiemBL.GetAppliedCalAvgMarkType(grade);
                if (appliedCalAvgMarkType != null)
                {
                    if (appliedCalAvgMarkType.MarkTypeName != editedMarkTypeName)
                    {
                        LblAppCalAvgMarkEdit.Visible = true;
                        MPEEdit.Show();
                        return false;
                    }
                }                
            }
            else
            {
                LblAppCalAvgMarkEdit.Visible = false;
            }
            return true;

        }

        private void BindDDLGrades()
        {
            GradeBL gradeBL = new GradeBL(UserSchool);
            List<Category_Grade> grades = gradeBL.GetListGrades();
            DdlGradeSearch.DataSource = grades;
            DdlGradeSearch.DataValueField = "GradeId";
            DdlGradeSearch.DataTextField = "GradeName";
            DdlGradeSearch.DataBind();

            if (DdlGradeSearch.Items.Count == 0)
            {
                BtnAdd.Enabled = false;
                BtnAdd.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_ADD_DISABLE;
            }
            else
            {
                DdlGradeAdd.DataSource = grades;
                DdlGradeAdd.DataValueField = "GradeId";
                DdlGradeAdd.DataTextField = "GradeName";
                DdlGradeAdd.DataBind();
            }
        }
        #endregion

        #region Button click event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            PagerMain.CurrentIndex = 1;
            PagerMain.ItemCount = 0;
            isSearch = true;
            BindRptLoaiDiem();
        }

        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {
            string MarkTypeName = this.TxtMarkTypeName.Text.Trim();
            double MarkRatioLoaiDiem = Double.Parse(this.TxtMarkRatioLoaiDiemAdd.Text.Trim());
            MarkRatioLoaiDiem = Math.Round(MarkRatioLoaiDiem, 1, MidpointRounding.AwayFromZero);
            string maxMarksPerTerm = this.TxtMaxMarksPerTermAdd.Text.Trim();
            bool calAverageMark = this.RbtnYesAdd.Checked;
            Category_Grade grade = new Category_Grade();
            grade.GradeId = Int32.Parse(DdlGradeAdd.SelectedValue);

            bool bValidInput = ValidateForAdd(grade, MarkTypeName, maxMarksPerTerm);
            if (bValidInput)
            {
                loaiDiemBL.InsertLoaiDiem(MarkTypeName, MarkRatioLoaiDiem,
                    short.Parse(maxMarksPerTerm), calAverageMark, grade);

                PagerMain.CurrentIndex = 1;
                BindRptLoaiDiem();

                this.TxtMarkTypeName.Text = "";
                this.TxtMarkRatioLoaiDiemAdd.Text = "";
                this.TxtMaxMarksPerTermAdd.Text = "";
                this.RbtnYesAdd.Checked = false;
                this.RbtnCancelAdd.Checked = true;

                if (this.CkbAddAfterSave.Checked)
                {
                    this.MPEAdd.Show();
                }
            }
        }        

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            HiddenField HdfRptMarkTypeId = null;
            Category_MarkType markType = null;
            foreach (RepeaterItem item in RptLoaiDiem.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox CkbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (CkbxSelect.Checked)
                    {
                        HdfRptMarkTypeId = (HiddenField)item.FindControl("HdfRptMarkTypeId");
                        markType = loaiDiemBL.GetMarkType(Int32.Parse(HdfRptMarkTypeId.Value));
                        if (loaiDiemBL.IsDeletable(markType))
                        {
                            loaiDiemBL.DeleteMarkType(markType);
                        }
                    }
                }
            }          
            
            isSearch = false;
            BindRptLoaiDiem();
        }

        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            HiddenField HdfRptMarkTypeId = null;
            Category_MarkType markType = null;
            foreach (RepeaterItem item in RptLoaiDiem.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox CkbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (CkbxSelect.Checked)
                    {
                        HdfRptMarkTypeId = (HiddenField)item.FindControl("HdfRptMarkTypeId");
                        markType = loaiDiemBL.GetMarkType(Int32.Parse(HdfRptMarkTypeId.Value));

                        TxtSuaMarkTypeName.Text = markType.MarkTypeName;
                        TxtMarkRatioLoaiDiemSua.Text = markType.MarkRatio.ToString();
                        TxtMaxMarksPerTermEdit.Text = markType.MaxQuantity.ToString();
                        RbtnYesEdit.Checked = markType.IsUsedForCalculatingAvg;
                        RbtnCancelEdit.Checked = !markType.IsUsedForCalculatingAvg;
                        LblGradeName.Text = markType.Category_Grade.GradeName;
                        HdfGradeId.Value = markType.GradeId.ToString();
                        LblAppCalAvgMarkEdit.Visible = false;

                        HdfEditedMarkTypeName.Value = markType.MarkTypeName;
                        MPEEdit.Show();
                        return;
                    }
                }
            }
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            string editedMarkTypeName = this.HdfEditedMarkTypeName.Value;
            string newMarkTypeName = TxtSuaMarkTypeName.Text.Trim();
            double MarkRatio = Double.Parse(TxtMarkRatioLoaiDiemSua.Text.Trim());
            string maxMarksPerTerm = this.TxtMaxMarksPerTermEdit.Text.Trim();
            bool calAverageMark = this.RbtnYesEdit.Checked;
            Category_Grade grade = new Category_Grade();
            grade.GradeId = Int32.Parse(HdfGradeId.Value);

            bool bValidInput = ValidateForEdit(grade, editedMarkTypeName, newMarkTypeName, maxMarksPerTerm);
            
            if (bValidInput)
            {
                loaiDiemBL.UpdateMarkType(grade, editedMarkTypeName, newMarkTypeName, MarkRatio,
                    short.Parse(maxMarksPerTerm), calAverageMark);
                BindRptLoaiDiem();
            }
        }
        #endregion

        #region Repeater event handlers
        protected void RptLoaiDiem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            
        }

        protected void RptLoaiDiem_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
               default:
                    {
                        break;
                    }
            }
        }
        #endregion

        #region Pager event handlers
        public void PagerMain_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            this.PagerMain.CurrentIndex = currnetPageIndx;
            BindRptLoaiDiem();
        }
        #endregion  
    }
}