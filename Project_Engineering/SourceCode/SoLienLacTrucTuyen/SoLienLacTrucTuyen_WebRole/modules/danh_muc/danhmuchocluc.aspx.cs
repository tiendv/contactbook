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
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class CategoryLearningAptitudePage : BaseContentPage
    {
        #region Fields
        private UserBL userBL;
        private LearningAptitudeBL learningAptitudeBL;
        private bool isSearch;

        protected string btnSaveAddClickEvent = string.Empty;
        protected string btnSaveEditClickEvent = string.Empty;
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

            learningAptitudeBL = new LearningAptitudeBL(UserSchool);

            if (!Page.IsPostBack)
            {
                isSearch = false;
                PagerMain.CurrentIndex = 1;
                BindRptLearningAptitudes();
            }
            ProcPermissions();
        }
        
        protected void Page_PreRender(object sender, EventArgs e)
        {
            RegisterScript();
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

        private void RegisterScript()
        {
            btnSaveAddClickEvent = Page.ClientScript.GetPostBackEventReference(this.BtnSaveAdd, string.Empty);
            btnSaveEditClickEvent = Page.ClientScript.GetPostBackEventReference(this.BtnSaveEdit, string.Empty);
        }

        private void BindRptLearningAptitudes()
        {
            string strLearningAptitudeName = TxtSearchHocLuc.Text.Trim();

            double dTotalRecords;
            List<Category_LearningAptitude> learningAptitudes;
            learningAptitudes = learningAptitudeBL.GetLearningAptitudes(strLearningAptitudeName, PagerMain.CurrentIndex, PagerMain.PageSize, out dTotalRecords);
            PagerMain.ItemCount = dTotalRecords;

            // Decrease page current index when delete
            if (learningAptitudes.Count == 0 && PagerMain.ItemCount != 0)
            {
                PagerMain.CurrentIndex--;
                BindRptLearningAptitudes();
                return;
            }

            bool bDisplayData = (learningAptitudes.Count != 0) ? true : false;
            RptHocLuc.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin học lực";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy học lực";
                }

                PagerMain.CurrentIndex = 1;
                PagerMain.ItemCount = 0;
                PagerMain.Visible = false;
            }
            else
            {
                PagerMain.Visible = true;
            }

            RptHocLuc.DataSource = learningAptitudes;
            RptHocLuc.DataBind();
        }

        private bool ValidateInputsForAdd()
        {
            if (!Page.IsValid)
            {
                return false;
            }

            bool bValid = true;

            string strLearningAptitudeName = this.TxtLearningAptitudeNameThem.Text.Trim();

            if (CheckUntils.IsNullOrBlank(strLearningAptitudeName))
            {
                LearningAptitudeNameRequiredAdd.IsValid = false;
                TxtLearningAptitudeNameThem.Focus();
                MPEAdd.Show();
                bValid = false;
            }
            else
            {
                if (learningAptitudeBL.LearningAptitudeNameExists(strLearningAptitudeName))
                {
                    LearningAptitudeNameValidatorAdd.IsValid = false;
                    TxtLearningAptitudeNameThem.Focus();
                    MPEAdd.Show();
                    bValid = false;
                }
            }

            double dBeginAverageMark = 0;
            bool bBeginAverageMarkValid = double.TryParse(this.TxtDTBTuThem.Text, out dBeginAverageMark);
            if (bBeginAverageMarkValid == false || (bBeginAverageMarkValid && dBeginAverageMark > 10))
            {
                BeginMarkCustomValidatorAdd.IsValid = false;
                this.TxtDTBTuThem.Focus();
                MPEAdd.Show();
                bValid = false;
            }
            else if (learningAptitudeBL.LearningAptitudeMarkExists(null, dBeginAverageMark))
            {
                BeginMarkCustomValidatorAdd.IsValid = false;
                this.TxtDTBTuThem.Focus();
                MPEAdd.Show();
                bValid = false;
            }

            double dEndAverageMark = 0;
            bool bEndAverageMarkValid = double.TryParse(this.TxtDTBDenThem.Text, out dEndAverageMark);
            if (bEndAverageMarkValid == false || (bEndAverageMarkValid && dEndAverageMark > 10))
            {
                EndMarkCustomValidatorAdd.IsValid = false;
                this.TxtDTBDenThem.Focus();
                MPEAdd.Show();
                bValid = false;
            }
            else if (learningAptitudeBL.LearningAptitudeMarkExists(null, dEndAverageMark))
            {
                EndMarkCustomValidatorAdd.IsValid = false;
                this.TxtDTBDenThem.Focus();
                MPEAdd.Show();
                bValid = false;
            }

            // relational checking
            if (bBeginAverageMarkValid && bEndAverageMarkValid)
            {
                if(dEndAverageMark <= dBeginAverageMark)
                {
                    CompareValidatorAdd.IsValid = false;
                    this.TxtDTBDenThem.Focus();
                    MPEAdd.Show();
                    bValid = false;
                }
            }

            return bValid;
        }

        private bool ValidateInputsForModify()
        {
            if (!Page.IsValid)
            {
                return false;
            }

            bool bValid = true;

            string strOldLearningAptitudeName = this.HdfEditedConductName.Value;
            string strNewLearningAptitudeName = this.TxtLearningAptitudeNameSua.Text.Trim();
            Category_LearningAptitude learningAptitude = new Category_LearningAptitude();
            learningAptitude.LearningAptitudeId = Int32.Parse(this.HdfLearningAptitudeId.Value);

            if (CheckUntils.IsNullOrBlank(strNewLearningAptitudeName))
            {
                LearningAptitudeNameRequiredModify.IsValid = false;
                TxtLearningAptitudeNameSua.Focus();
                MPEEdit.Show();
                bValid = false;
            }
            else
            {

                if (learningAptitudeBL.LearningAptitudeNameExists(strOldLearningAptitudeName, strNewLearningAptitudeName))
                {
                    LearningAptitudeNameValidatorModify.IsValid = false;
                    TxtLearningAptitudeNameSua.Focus();
                    MPEEdit.Show();
                    bValid = false;
                }
            }

            double dBeginAverageMark = 0;
            bool bBeginAverageMarkValid = double.TryParse(this.TxtDTBTuSua.Text, out dBeginAverageMark);
            if (bBeginAverageMarkValid == false || (bBeginAverageMarkValid && dBeginAverageMark > 10))
            {
                BeginMarkCustomValidatorModify.IsValid = false;
                this.TxtDTBTuSua.Focus();
                MPEEdit.Show();
                bValid = false;
            }
            else if (learningAptitudeBL.LearningAptitudeMarkExists(learningAptitude, dBeginAverageMark))
            {
                BeginMarkCustomValidatorModify.IsValid = false;
                this.TxtDTBTuSua.Focus();
                MPEEdit.Show();
                bValid = false;
            }

            double dEndAverageMark = 0;
            bool bEndAverageMarkValid = double.TryParse(this.TxtDTBDenSua.Text, out dEndAverageMark);
            if (bEndAverageMarkValid == false || (bEndAverageMarkValid && dEndAverageMark > 10))
            {
                EndMarkCustomValidatorModify.IsValid = false;
                this.TxtDTBDenSua.Focus();
                MPEEdit.Show();
                bValid = false;
            }
            else if (learningAptitudeBL.LearningAptitudeMarkExists(learningAptitude, dEndAverageMark))
            {
                EndMarkCustomValidatorModify.IsValid = false;
                this.TxtDTBDenSua.Focus();
                MPEEdit.Show();
                bValid = false;
            }

            // relational checking
            if (bBeginAverageMarkValid && bEndAverageMarkValid)
            {
                if (dEndAverageMark <= dBeginAverageMark)
                {
                    CompareValidatorModify.IsValid = false;
                    this.TxtDTBDenSua.Focus();
                    MPEEdit.Show();
                    bValid = false;
                }
            }

            return bValid;
        }
        #endregion

        #region Button click event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            PagerMain.CurrentIndex = 1;
            PagerMain.ItemCount = 0;
            isSearch = true;
            BindRptLearningAptitudes();
        }

        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {
            if (ValidateInputsForAdd() == false)
            {
                return;
            }

            string strLearningAptitudeName = this.TxtLearningAptitudeNameThem.Text.Trim();
            double dBeginAverageMark = double.Parse(this.TxtDTBTuThem.Text);
            double dEndAverageMark = double.Parse(this.TxtDTBDenThem.Text);

            learningAptitudeBL.InsertLearningAptitude(strLearningAptitudeName, dBeginAverageMark, dEndAverageMark);

            //PagerMain.CurrentIndex = 1;
            BindRptLearningAptitudes();

            this.TxtLearningAptitudeNameThem.Text = "";

            if (this.CkbAddAfterSave.Checked)
            {
                this.MPEAdd.Show();
            }
        }

        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            HiddenField HdfRptLearningAptitudeId = null;
            foreach (RepeaterItem item in RptHocLuc.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox CkbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (CkbxSelect.Checked)
                    {
                        HdfRptLearningAptitudeId = (HiddenField)item.FindControl("HdfRptLearningAptitudeId");

                        Category_LearningAptitude learningAptitude = learningAptitudeBL.GetLearningAptitude(Int32.Parse(HdfRptLearningAptitudeId.Value));

                        TxtLearningAptitudeNameSua.Text = learningAptitude.LearningAptitudeName;
                        TxtDTBTuSua.Text = learningAptitude.BeginAverageMark.ToString();
                        TxtDTBDenSua.Text = learningAptitude.EndAverageMark.ToString();

                        // save global information
                        this.HdfLearningAptitudeId.Value = learningAptitude.LearningAptitudeId.ToString();
                        this.HdfEditedConductName.Value = learningAptitude.LearningAptitudeName;

                        MPEEdit.Show();
                        return;
                    }
                }
            }
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            bool bInfoInUse = false;
            CheckBox ckbxSelect = null;
            HiddenField HdfRptLearningAptitudeId = null;

            foreach (RepeaterItem item in RptHocLuc.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    ckbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (ckbxSelect.Checked)
                    {
                        HdfRptLearningAptitudeId = (HiddenField)item.FindControl("HdfRptLearningAptitudeId");
                        Category_LearningAptitude learningAptitude = new Category_LearningAptitude();
                        learningAptitude.LearningAptitudeId = Int32.Parse(HdfRptLearningAptitudeId.Value);

                        if (learningAptitudeBL.IsDeletable(learningAptitude))
                        {
                            learningAptitudeBL.DeleteLearningAptitude(learningAptitude);
                        }
                        else
                        {
                            bInfoInUse = true;
                        }
                    }
                }
            }

            isSearch = false;
            BindRptLearningAptitudes();

            if (bInfoInUse)
            {
                MPEInfoInUse.Show();
            }            
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            if (ValidateInputsForModify() == false)
            {
                return;
            }

            int iLearningAptitudeId = Int32.Parse(this.HdfLearningAptitudeId.Value);
            string strLearningAptitudeName = this.TxtLearningAptitudeNameSua.Text.Trim();
            double dBeginAverageMark = double.Parse(this.TxtDTBTuSua.Text);
            double dEndAverageMark = double.Parse(this.TxtDTBDenSua.Text);

            Category_LearningAptitude learningAptitude = new Category_LearningAptitude();
            learningAptitude.LearningAptitudeId = iLearningAptitudeId;
            learningAptitudeBL.UpdateLearningAptitude(learningAptitude, strLearningAptitudeName, dBeginAverageMark, dEndAverageMark);

            BindRptLearningAptitudes();
        }
        #endregion

        #region Repeater event handlers
        protected void RptHocLuc_ItemDataBound(object sender, RepeaterItemEventArgs e)
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

        #region Pager event handlers
        public void PagerMain_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            this.PagerMain.CurrentIndex = currnetPageIndx;
            BindRptLearningAptitudes();
        }
        #endregion     
    }
}