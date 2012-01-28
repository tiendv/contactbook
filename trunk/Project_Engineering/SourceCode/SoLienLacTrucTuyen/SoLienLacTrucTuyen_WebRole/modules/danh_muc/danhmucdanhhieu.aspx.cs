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
    public partial class LearningResultListPage : BaseContentPage
    {
        #region Fields
        private LearningResultBL learningResultBL;
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

            learningResultBL = new LearningResultBL(UserSchool);

            if (!Page.IsPostBack)
            {
                LearningAptitudeBL learningAptitudeBL = new LearningAptitudeBL(UserSchool);
                ConductBL conductBL = new ConductBL(UserSchool);
                if (learningAptitudeBL.GetLearningAptitudes().Count != 0 && conductBL.GetListConducts(false).Count != 0)
                {
                    isSearch = false;
                    MainDataPager.CurrentIndex = 1;
                    BindRptLearningResults();
                }
                else
                {
                    ProccessDisplayGUI(false);
                    BtnAdd.Visible = false;
                }
            }

            ProcPermissions();
        }
        #endregion

        #region Button click event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            isSearch = true;
            BindRptLearningResults();
        }

        protected void BtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("themdanhhieu.aspx");
        }

        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            HiddenField HdfRptLearningResultId = null;
            foreach (RepeaterItem item in RptDanhHieu.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox CkbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (CkbxSelect.Checked)
                    {
                        HdfRptLearningResultId = (HiddenField)item.FindControl("HdfRptLearningResultId");
                        Category_LearningResult learningResult = learningResultBL.GetLearningResult(Int32.Parse(HdfRptLearningResultId.Value));

                        AddSession(AppConstant.SESSION_SELECTED_LEARNINGRESULT, learningResult);
                        Response.Redirect(AppConstant.PAGEPATH_CATEGORY_LEARNINGRESULT_MODIFY);
                        return;
                    }
                }
            }
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            bool bInfoInUse = false;
            CheckBox ckbxSelect = null;
            HiddenField HdfRptLearningResultId = null;

            foreach (RepeaterItem item in RptDanhHieu.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    ckbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (ckbxSelect.Checked)
                    {
                        HdfRptLearningResultId = (HiddenField)item.FindControl("HdfRptLearningResultId");
                        Category_LearningResult learningResult = new Category_LearningResult();
                        learningResult.LearningResultId = Int32.Parse(HdfRptLearningResultId.Value);

                        if (learningResultBL.IsDeletable(learningResult))
                        {
                            learningResultBL.DeleteLearningResult(learningResult);
                        }
                        else
                        {
                            bInfoInUse = true;
                        }
                    }
                }
            }

            isSearch = false;
            BindRptLearningResults();

            if (bInfoInUse)
            {
                MPEInfoInUse.Show();
            }
        }
        #endregion

        #region Repeater event handlers
        protected void RptDanhHieu_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                e.Item.FindControl("thSelectAll").Visible = (accessibilities.Contains(AccessibilityEnum.Modify) || accessibilities.Contains(AccessibilityEnum.Delete));
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.FindControl("tdSelect").Visible = (accessibilities.Contains(AccessibilityEnum.Modify) || accessibilities.Contains(AccessibilityEnum.Delete));

                Repeater rptDetailedLearningResult = (Repeater)e.Item.FindControl("RptDetailedLearningResult");

                HiddenField HdfRptLearningResultId = (HiddenField)e.Item.FindControl("HdfRptLearningResultId");

                List<TabularLearningResult> tabularLearningResults = (List<TabularLearningResult>)RptDanhHieu.DataSource;
                foreach (TabularLearningResult tabularLearningResult in tabularLearningResults)
                {
                    if (tabularLearningResult.LearningResultId.ToString() == HdfRptLearningResultId.Value)
                    {
                        rptDetailedLearningResult.DataSource = tabularLearningResult.DetailLearningResults;
                        rptDetailedLearningResult.DataBind();
                        return;
                    }
                }
            }
        }
        #endregion

        #region Pager event handlers
        public void pager_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currnetPageIndx;
            BindRptLearningResults();
        }
        #endregion

        #region Methods
        private void ProcPermissions()
        {
            BtnAdd.Visible = BtnAdd.Visible && accessibilities.Contains(AccessibilityEnum.Add);
            BtnEdit.Visible = accessibilities.Contains(AccessibilityEnum.Modify);
            BtnDelete.Visible = accessibilities.Contains(AccessibilityEnum.Delete);
            PnlPopupConfirmDelete.Visible = accessibilities.Contains(AccessibilityEnum.Delete);
        }

        private void BindRptLearningResults()
        {
            string strLearningResultName = TxtSearchDanhHieu.Text.Trim();

            double totalRecord;
            List<TabularLearningResult> learningResults = learningResultBL.GetTabularLearningResults(strLearningResultName,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out totalRecord);

            // Decrease page current index when delete
            if (learningResults.Count == 0 && totalRecord != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRptLearningResults();
                return;
            }

            bool bDisplayData = (learningResults.Count != 0) ? true : false;
            ProccessDisplayGUI(bDisplayData);

            RptDanhHieu.DataSource = learningResults;
            RptDanhHieu.DataBind();
            MainDataPager.ItemCount = totalRecord;
        }

        private void ProccessDisplayGUI(bool bDisplayData)
        {
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
        }
        #endregion
    }
}