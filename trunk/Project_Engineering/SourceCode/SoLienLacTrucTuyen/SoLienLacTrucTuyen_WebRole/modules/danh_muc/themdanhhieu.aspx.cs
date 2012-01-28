using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using AjaxControlToolkit;
using System.Web.Security;
using System.Text;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class LearningResultAddPage : BaseContentPage
    {
        #region Fields
        public class SelectedDetailLearningResult
        {
            public int LearningAptitudeId { get; set; }
            public int ConductId { get; set; }
        }
        List<SelectedDetailLearningResult> selectedDetailLearningResults;

        private LearningResultBL learningResultBL;
        List<Category_LearningAptitude> learningAptitudes;
        List<Category_Conduct> conducts;
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

            learningResultBL = new LearningResultBL(UserSchool);
            LearningAptitudeBL learningAptitudeBL = new LearningAptitudeBL(UserSchool);
            learningAptitudes = learningAptitudeBL.GetLearningAptitudes();
            ConductBL conductBL = new ConductBL(UserSchool);
            conducts = conductBL.GetListConducts(false);

            if (learningAptitudes.Count != 0 && conducts.Count != 0)
            {
                if (!Page.IsPostBack)
                {
                    selectedDetailLearningResults = new List<SelectedDetailLearningResult>();
                    BindRptDetailLearningResult();
                }
            }
            else
            {
                Response.Redirect(AppConstant.PAGEPATH_CATEGORY_LEARNINGRESULT_LIST);
            }
        }
        #endregion

        #region Button click event handlers
        protected void BtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            if (CheckSessionKey(AppConstant.SESSION_SELECTEDDETAILLEARNINGRESULTS))
            {
                selectedDetailLearningResults = (List<SelectedDetailLearningResult>)GetSession(AppConstant.SESSION_SELECTEDDETAILLEARNINGRESULTS);
            }
            else
            {
                selectedDetailLearningResults = new List<SelectedDetailLearningResult>();
            }

            for (int i = 0; i < RptDetailLearningResult.Items.Count; i++)
            {
                if (RptDetailLearningResult.Items[i].ItemType == ListItemType.Item || RptDetailLearningResult.Items[i].ItemType == ListItemType.AlternatingItem)
                {
                    DropDownList DLLearningAptitudes = (DropDownList)RptDetailLearningResult.Items[i].FindControl("DDLLearningAptitudes");
                    if (DLLearningAptitudes.Items.Count != 0)
                    {
                        selectedDetailLearningResults[i].LearningAptitudeId = Int32.Parse(DLLearningAptitudes.SelectedValue);
                    }

                    DropDownList DDLConducts = (DropDownList)RptDetailLearningResult.Items[i].FindControl("DDLConducts");
                    if (DDLConducts.Items.Count != 0)
                    {
                        selectedDetailLearningResults[i].ConductId = Int32.Parse(DDLConducts.SelectedValue);
                    }
                }
            }

            SelectedDetailLearningResult detailLearningResult = new SelectedDetailLearningResult();
            detailLearningResult.LearningAptitudeId = learningAptitudes[0].LearningAptitudeId;
            detailLearningResult.ConductId = conducts[0].ConductId;
            selectedDetailLearningResults.Add(detailLearningResult);

            BindRptDetailLearningResult();
            this.LblError.Text = "";
        }

        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            List<KeyValuePair<Category_LearningAptitude, Category_Conduct>> detailLearningResults = new List<KeyValuePair<Category_LearningAptitude, Category_Conduct>>();

            string strLearningResultName = this.TxtLearningResultName.Text.Trim();

            if (CheckUntils.IsNullOrBlank(strLearningResultName))
            {
                LearningResultNameRequiredAdd.IsValid = false;
                TxtLearningResultName.Focus();
                return;
            }
            else
            {
                if (learningResultBL.LearningResultNameExists(strLearningResultName))
                {
                    LearningResultNameValidatorAdd.IsValid = false;
                    TxtLearningResultName.Focus();
                    return;
                }
            }

            if (CheckSessionKey(AppConstant.SESSION_SELECTEDDETAILLEARNINGRESULTS))
            {
                selectedDetailLearningResults = (List<SelectedDetailLearningResult>)GetSession(AppConstant.SESSION_SELECTEDDETAILLEARNINGRESULTS);
            }
            else
            {
                selectedDetailLearningResults = new List<SelectedDetailLearningResult>();
            }

            Category_Conduct conduct = null;
            Category_LearningAptitude learningAptitude = null;
            List<int> iErrorRowIndexes = new List<int>();
            for (int i = 0; i < RptDetailLearningResult.Items.Count; i++)
            {
                if (RptDetailLearningResult.Items[i].ItemType == ListItemType.Item || RptDetailLearningResult.Items[i].ItemType == ListItemType.AlternatingItem)
                {
                    learningAptitude = new Category_LearningAptitude();
                    conduct = new Category_Conduct();

                    DropDownList DLLearningAptitudes = (DropDownList)RptDetailLearningResult.Items[i].FindControl("DDLLearningAptitudes");
                    if (DLLearningAptitudes.Items.Count != 0)
                    {
                        learningAptitude.LearningAptitudeId = Int32.Parse(DLLearningAptitudes.SelectedValue);
                    }

                    DropDownList DDLConducts = (DropDownList)RptDetailLearningResult.Items[i].FindControl("DDLConducts");
                    if (DDLConducts.Items.Count != 0)
                    {
                        conduct.ConductId = Int32.Parse(DDLConducts.SelectedValue);
                    }

                    if (learningResultBL.DetailLearningResultExists(null, learningAptitude, conduct))
                    {
                        iErrorRowIndexes.Add(i);
                    }
                    else
                    {
                        detailLearningResults.Add(new KeyValuePair<Category_LearningAptitude, Category_Conduct>(learningAptitude, conduct));
                    }
                }
            }

            if (iErrorRowIndexes.Count == 0)
            {
                learningResultBL.InsertLearningResult(strLearningResultName, detailLearningResults);
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("Chi tiết danh hiệu thứ ");
                for (int i = 0; i < iErrorRowIndexes.Count - 1; i++)
                {
                    stringBuilder.Append(i + 1);
                    stringBuilder.Append(", ");
                }
                stringBuilder.Append(iErrorRowIndexes[iErrorRowIndexes.Count - 1] + 1);
                stringBuilder.Append(" đã tồn tại ở danh hiệu khác!");
                LblError.Text = stringBuilder.ToString();

                return;
            }

            if (this.CkbAddAfterSave.Checked)
            {
                this.TxtLearningResultName.Text = "";
                this.LblError.Text = "";
            }
            else
            {
                RedirectToPreviousPage();
            }
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            RedirectToPreviousPage();
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            CheckBox ckbxSelect = null;
            if (CheckSessionKey(AppConstant.SESSION_SELECTEDDETAILLEARNINGRESULTS))
            {
                selectedDetailLearningResults = (List<SelectedDetailLearningResult>)GetSession(AppConstant.SESSION_SELECTEDDETAILLEARNINGRESULTS);
            }
            else
            {
                selectedDetailLearningResults = new List<SelectedDetailLearningResult>();
            }

            int k = 0;
            for (int i = 0; i < RptDetailLearningResult.Items.Count; i++)
            {
                if (RptDetailLearningResult.Items[i].ItemType == ListItemType.Item || RptDetailLearningResult.Items[i].ItemType == ListItemType.AlternatingItem)
                {
                    DropDownList DLLearningAptitudes = (DropDownList)RptDetailLearningResult.Items[i].FindControl("DDLLearningAptitudes");
                    if (DLLearningAptitudes.Items.Count != 0)
                    {
                        selectedDetailLearningResults[i - k].LearningAptitudeId = Int32.Parse(DLLearningAptitudes.SelectedValue);
                    }

                    DropDownList DDLConducts = (DropDownList)RptDetailLearningResult.Items[i].FindControl("DDLConducts");
                    if (DDLConducts.Items.Count != 0)
                    {
                        selectedDetailLearningResults[i - k].ConductId = Int32.Parse(DDLConducts.SelectedValue);
                    }

                    ckbxSelect = (CheckBox)RptDetailLearningResult.Items[i].FindControl("CkbxSelect");
                    if (ckbxSelect.Checked)
                    {
                        selectedDetailLearningResults.RemoveAt(i - k);
                        k++;
                    }
                }
            }

            BindRptDetailLearningResult();
            this.LblError.Text = "";
        }
        #endregion

        #region Methods
        private void BindRptDetailLearningResult()
        {
            RptDetailLearningResult.DataSource = selectedDetailLearningResults;
            RptDetailLearningResult.DataBind();

            bool bDisplayData = (selectedDetailLearningResults.Count != 0);
            RptDetailLearningResult.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            AddSession(AppConstant.SESSION_SELECTEDDETAILLEARNINGRESULTS, selectedDetailLearningResults);

            if (selectedDetailLearningResults.Count != 0)
            {
                BtnSaveAdd.Enabled = true;
                BtnSaveAdd.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SAVE;
            }
            else
            {
                BtnSaveAdd.Enabled = false;
                BtnSaveAdd.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SAVE_DISABLE;
            }
        }

        private void RedirectToPreviousPage()
        {
            Response.Redirect("danhmucdanhhieu.aspx");
        }
        #endregion

        #region Repeater event handlers
        protected void RptDetailLearningResult_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList DLLearningAptitudes = (DropDownList)e.Item.FindControl("DDLLearningAptitudes");
                DLLearningAptitudes.DataSource = learningAptitudes;
                DLLearningAptitudes.DataValueField = "LearningAptitudeId";
                DLLearningAptitudes.DataTextField = "LearningAptitudeName";
                DLLearningAptitudes.DataBind();

                HiddenField HdfLearningAptitudeId = (HiddenField)e.Item.FindControl("HdfLearningAptitudeId");
                if (DLLearningAptitudes.Items.Count != 0)
                {
                    DLLearningAptitudes.SelectedValue = HdfLearningAptitudeId.Value;
                }

                DropDownList DDLConducts = (DropDownList)e.Item.FindControl("DDLConducts");
                DDLConducts.DataSource = conducts;
                DDLConducts.DataValueField = "ConductId";
                DDLConducts.DataTextField = "ConductName";
                DDLConducts.DataBind();

                HiddenField HdfConductId = (HiddenField)e.Item.FindControl("HdfConductId");
                if (DDLConducts.Items.Count != 0)
                {
                    DDLConducts.SelectedValue = HdfConductId.Value;
                }
            }
        }
        #endregion
    }
}