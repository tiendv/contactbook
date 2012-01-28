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
    public partial class CategoryYearPage : BaseContentPage
    {
        #region Fields
        private SystemConfigBL systemConfigBL;
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

            systemConfigBL = new SystemConfigBL(UserSchool);

            if (!Page.IsPostBack)
            {
                // retrieve saved selection session
                RetrieveSelectionSession();

                isSearch = false;
                MainDataPager.CurrentIndex = 1;
                BindRptYears();
            }            
            ProcPermissions();
        }
        #endregion

        #region Button click event handlers
        protected void BtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            SaveSelectionSessions();
            Response.Redirect(AppConstant.PAGEPATH_CATEGORY_ADDYEAR);
        }

        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            isSearch = true;
            BindRptYears();
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            bool bInfoInUse = false;
            CheckBox ckbxSelect = null;
            HiddenField hdfRptYearId = null;
            Configuration_Year year = null;
            foreach (RepeaterItem item in RptYears.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    ckbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (ckbxSelect.Checked)
                    {
                        hdfRptYearId = (HiddenField)item.FindControl("HdfRptYearId");
                        year = new Configuration_Year();
                        year.YearId = Int32.Parse(hdfRptYearId.Value);

                        if (systemConfigBL.IsDeletable(year))
                        {
                            systemConfigBL.DeleteYear(year);
                        }
                        else
                        {
                            bInfoInUse = true;
                        }
                    }
                }
            }

            isSearch = false;
            BindRptYears();

            if (bInfoInUse)
            {
                MPEInfoInUse.Show();
            }
        }

        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            CheckBox ckbxSelect = null;
            HiddenField hdfRptYearId = null;
            Label lblYearName = null;
            Label lblFirstTermBeginDate = null;
            Label lblFirstTermEndDate = null;
            Label lblSecondTermBeginDate = null;
            Label lblSecondTermEndDate = null;

            Configuration_Year year = null;
            foreach (RepeaterItem item in RptYears.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    ckbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (ckbxSelect.Checked)
                    {
                        hdfRptYearId = (HiddenField)item.FindControl("HdfRptYearId");
                        lblYearName = (Label)item.FindControl("LblYearName");
                        lblFirstTermBeginDate = (Label)item.FindControl("LblFirstTermBeginDate");
                        lblFirstTermEndDate = (Label)item.FindControl("LblFirstTermEndDate");
                        lblSecondTermBeginDate = (Label)item.FindControl("LblSecondTermBeginDate");
                        lblSecondTermEndDate = (Label)item.FindControl("LblSecondTermEndDate");

                        year = new Configuration_Year();
                        year.YearId = Int32.Parse(hdfRptYearId.Value);
                        year.YearName = lblYearName.Text;
                        year.BeginFirstTermDate = DateTime.Parse(lblFirstTermBeginDate.Text);
                        year.EndFirstTermDate = DateTime.Parse(lblFirstTermEndDate.Text);
                        year.BeginSecondTermDate = DateTime.Parse(lblSecondTermBeginDate.Text);
                        year.EndSecondTermDate = DateTime.Parse(lblSecondTermEndDate.Text);
                        
                        SaveSelectionSessions();

                        AddSession(AppConstant.SESSION_SELECTED_YEAR, year);
                        Response.Redirect(AppConstant.PAGEPATH_CATEGORY_MODIFYYEAR);
                    }
                }
            }
        }
        #endregion

        #region Pager event handlers
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currnetPageIndx;
            BindRptYears();
        }
        #endregion

        #region Methods
        private void ProcPermissions()
        {
            BtnAdd.Visible = accessibilities.Contains(AccessibilityEnum.Add);
            BtnEdit.Visible = accessibilities.Contains(AccessibilityEnum.Modify); 
            BtnDelete.Visible = accessibilities.Contains(AccessibilityEnum.Delete);
            PnlPopupConfirmDelete.Visible = accessibilities.Contains(AccessibilityEnum.Delete);
        }

        private void BindRptYears()
        {
            string strYearName = TxtSearchYear.Text.Trim();

            double dTotalRecords;
            List<TabularYear> years;
            years = systemConfigBL.GetYears(strYearName, MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);
            MainDataPager.ItemCount = dTotalRecords;

            // Decrease page current index when delete
            if (years.Count == 0 && MainDataPager.ItemCount != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRptYears();
                return;
            }

            bool bDisplayData = (years.Count != 0) ? true : false;            
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin năm học";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy năm học";
                }

                MainDataPager.CurrentIndex = 1;
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
            }
            else
            {
                MainDataPager.Visible = true;
            }

            RptYears.Visible = bDisplayData;
            RptYears.DataSource = years;
            RptYears.DataBind();

            ViewState[AppConstant.VIEWSTATE_SELECTED_YEARNAME] = TxtSearchYear.Text;
        }

        private void SaveSelectionSessions()
        {
            AddSession(AppConstant.SESSION_SELECTED_YEARNAME, (string)ViewState[AppConstant.VIEWSTATE_SELECTED_YEARNAME]);
        }

        private bool RetrieveSelectionSession()
        {
            if(CheckSessionKey(AppConstant.SESSION_SELECTED_YEARNAME))
            {
                TxtSearchYear.Text = (string)GetSession(AppConstant.SESSION_SELECTED_YEARNAME);
                RemoveSession(AppConstant.SESSION_SELECTED_YEARNAME);
                return true;
            }

            return false;
        }
        #endregion

        #region Repeater event handlers
        protected void RptYears_ItemDataBound(object sender, RepeaterItemEventArgs e)
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