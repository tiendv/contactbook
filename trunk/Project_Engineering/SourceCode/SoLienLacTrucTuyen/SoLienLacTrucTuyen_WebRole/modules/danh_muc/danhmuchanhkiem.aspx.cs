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
    public partial class CategoryConduct : BaseContentPage
    {
        #region Fields
        private ConductBL conductBL;
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

            conductBL = new ConductBL(UserSchool);

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
            if (!ValidateInputsForAdd())
            {
                return;
            }

            string strConductName = this.TxtConductName.Text.Trim();

            conductBL.InsertConduct(strConductName);

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
            bool bInfoInUse = false;
            CheckBox ckbxSelect = null;
            HiddenField hdfRptConductId = null;
            Category_Conduct conduct = null;

            foreach (RepeaterItem item in RptHanhKiem.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    ckbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (ckbxSelect.Checked)
                    {
                        hdfRptConductId = (HiddenField)item.FindControl("HdfRptConductId");
                        conduct = new Category_Conduct();
                        conduct.ConductId = Int32.Parse(hdfRptConductId.Value);

                        if (conductBL.IsDeletable(conduct))
                        {
                            conductBL.DeleteConduct(conduct);
                        }
                        else
                        {
                            bInfoInUse = true;
                        }
                    }
                }
            }

            isSearch = false;
            BindData();

            if (bInfoInUse)
            {
                MPEInfoInUse.Show();
            }
        }

        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            HiddenField hdfRptConductId = null;
            foreach (RepeaterItem item in RptHanhKiem.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox CkbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (CkbxSelect.Checked)
                    {
                        hdfRptConductId = (HiddenField)item.FindControl("HdfRptConductId");
                        Category_Conduct conduct = conductBL.GetConduct(Int32.Parse(hdfRptConductId.Value));
                        TxtSuaConductName.Text = conduct.ConductName;
                        this.HdfConductId.Value = conduct.ConductId.ToString();
                        this.HdfEditedConductName.Value = conduct.ConductName;

                        MPEEdit.Show();
                        return;
                    }
                }
            }
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {   
            string editedConductName = (string)HdfEditedConductName.Value;
            string newConductName = TxtSuaConductName.Text.Trim();
            Category_Conduct conduct = new Category_Conduct();
            conduct.ConductId = Int32.Parse(this.HdfConductId.Value);

            conductBL.UpdateConduct(conduct, newConductName);
            BindData();
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
            BtnAdd.Visible = accessibilities.Contains(AccessibilityEnum.Add);
            PnlPopupAdd.Visible = accessibilities.Contains(AccessibilityEnum.Add);
            BtnEdit.Visible = accessibilities.Contains(AccessibilityEnum.Modify);
            BtnDelete.Visible = accessibilities.Contains(AccessibilityEnum.Delete);
            PnlPopupConfirmDelete.Visible = accessibilities.Contains(AccessibilityEnum.Delete);
        }

        public void BindData()
        {
            string strConductName = TxtSearchHanhKiem.Text.Trim();

            double dTotalRecords;
            List<Category_Conduct> conducts;
            conducts = conductBL.GetListConducts(strConductName, MainDataPager.CurrentIndex, MainDataPager.PageSize, 
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

        private bool ValidateInputsForAdd()
        {
            string ConductName = this.TxtConductName.Text.Trim();
            if (CheckUntils.IsNullOrBlank(ConductName))
            {
                ConductNameRequiredAdd.IsValid = false;
                TxtConductName.Focus();
                MPEAdd.Show();
                return false;
            }
            else
            {
                if (conductBL.ConductNameExists(ConductName))
                {
                    ConductNameValidatorAdd.IsValid = false;
                    TxtConductName.Focus();
                    MPEAdd.Show();
                    return false;
                }
            }

            return true;
        }

        private bool ValidateInputsForModify()
        {
            if (!Page.IsValid)
            {
                return false;
            }

            string strOldConductName = this.HdfEditedConductName.Value;
            string strNewConductName = TxtSuaConductName.Text.Trim();

            if (CheckUntils.IsNullOrBlank(strNewConductName))
            {
                ConductNameRequiredEdit.IsValid = false;
                TxtSuaConductName.Focus();
                MPEEdit.Show();
                return false;
            }
            else
            {
                if (conductBL.ConductNameExists(strOldConductName, strNewConductName))
                {
                    ConductNameValidatorEdit.IsValid = false;
                    TxtSuaConductName.Focus();
                    MPEEdit.Show();
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region Repeater event handlers
        protected void RptHanhKiem_ItemDataBound(object sender, RepeaterItemEventArgs e)
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