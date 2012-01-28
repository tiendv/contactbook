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
using SoLienLacTrucTuyen_WebRole.Modules;
using EContactBook.BusinessEntity;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole
{
    public partial class FacultyCategoryPage : BaseContentPage
    {
        #region Field(s)
        private FacultyBL facultyBL;
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

            facultyBL = new FacultyBL(UserSchool);

            if (!Page.IsPostBack)
            {
                isSearch = false;
                MainDataPager.CurrentIndex = 1;

                ViewState["SortColumn"] = "FacultyName";
                ViewState["SortOrder"] = "ASC";
                BindRptFaculties();
            }

            ProcPermissions();
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

        public void BindRptFaculties()
        {
            string strFacultyName = TxtSearchNganhHoc.Text.Trim();
            double dTotalRecords;
            List<Category_Faculty> faculties = facultyBL.GetFaculties(strFacultyName, MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);
            
            // Decrease page current index when delete
            if (faculties.Count == 0 && dTotalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRptFaculties();
                return;
            }

            bool bDisplayData = (faculties.Count != 0) ? true : false;
            RptFaculties.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin ngành học";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy ngành học";
                }

                MainDataPager.CurrentIndex = 1;
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;

                BtnDelete.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_DELETE_DISABLED;
                BtnDelete.Enabled = false;

                BtnEdit.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_MODIFY_DISABLED;
                BtnEdit.Enabled = false;
            }
            else
            {
                MainDataPager.Visible = true;

                BtnDelete.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_DELETE;
                BtnDelete.Enabled = true;

                BtnEdit.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_MODIFY;
                BtnEdit.Enabled = true;
            }

            RptFaculties.DataSource = faculties;
            RptFaculties.DataBind();
            MainDataPager.ItemCount = dTotalRecords;
        }

        private bool ValidateInputsForAdding()
        {
            if (!Page.IsValid)
            {
                return false;
            }

            string facultyName = this.TxtFacultyName.Text.Trim();

            if (CheckUntils.IsNullOrBlank(facultyName))
            {
                FacultyNameRequiredAdd.IsValid = false;
                MPEAdd.Show();
                return false;
            }
            else
            {
                if (facultyBL.FacultyExists(facultyName))
                {
                    FacultyNameValidatorAdd.IsValid = false;
                    MPEAdd.Show();
                    return false;
                }
            }

            return true;
        }

        private bool ValidateInputsForModifying()
        {
            if (!Page.IsValid)
            {
                return false;
            }

            string strNewFacultyName = this.TxtFacultyNameEdit.Text.Trim();
            string strOldFacultyName = (string)this.HdfEditedFacultyName.Value;

            if (CheckUntils.IsNullOrBlank(strNewFacultyName))
            {
                FacultyNameRequiredEdit.IsValid = false;
                MPEEdit.Show();
                return false;
            }
            else
            {
                if (facultyBL.FacultyExists(strOldFacultyName, strNewFacultyName))
                {
                    FacultyNameValidatorEdit.IsValid = false;
                    MPEEdit.Show();
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region Button click event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            MainDataPager.ItemCount = 0;
            isSearch = true;
            BindRptFaculties();
        }

        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {
            if (!ValidateInputsForAdding())
            {
                return;
            }

            string strFacultyName = this.TxtFacultyName.Text.Trim();
            string strDescription = this.TxtDescriptionNganhHoc.Text.Trim();
            
            facultyBL.InsertFaculty(strFacultyName, strDescription);

            MainDataPager.CurrentIndex = 1;
            BindRptFaculties();

            this.TxtFacultyName.Text = "";
            this.TxtDescriptionNganhHoc.Text = "";

            if (this.CkbAddAfterSave.Checked)
            {
                this.MPEAdd.Show();
            }
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            bool bInfoInUse = false;
            CheckBox ckbxSelect = null;
            HiddenField hdfRptFacultyId = null;
            Category_Faculty faculty = null;

            foreach (RepeaterItem item in RptFaculties.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    ckbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (ckbxSelect.Checked)
                    {
                        hdfRptFacultyId = (HiddenField)item.FindControl("HdfRptFacultyId");
                        faculty = new Category_Faculty();
                        faculty.FacultyId = Int32.Parse(hdfRptFacultyId.Value);

                        if (facultyBL.IsDeletable(faculty))
                        {
                            facultyBL.DeleteFaculty(faculty);
                        }
                        else
                        {
                            bInfoInUse = true;
                        }                       
                    }
                }
            }

            isSearch = false;
            BindRptFaculties();

            if (bInfoInUse)
            {                
                MPEInfoInUse.Show();
            }
        }

        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            HiddenField hdfRptFacultyId = null;
            foreach (RepeaterItem item in RptFaculties.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox CkbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (CkbxSelect.Checked)
                    {
                        hdfRptFacultyId = (HiddenField)item.FindControl("HdfRptFacultyId");
                        Category_Faculty faculty = facultyBL.GetFaculty(Int32.Parse(hdfRptFacultyId.Value));
                        TxtFacultyNameEdit.Text = faculty.FacultyName;
                        TxtSuaDescriptionNganhHoc.Text = faculty.Description;
                        HdfEditedFacultyName.Value = faculty.FacultyName;

                        MPEEdit.Show();

                        return;
                    }
                }
            }
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            if (!ValidateInputsForModifying())
            {
                return;
            }
            
            string strNewFacultyName = (string)this.HdfEditedFacultyName.Value;
            string strOldFacultyName = this.TxtFacultyNameEdit.Text.Trim();
            string strDescription = this.TxtSuaDescriptionNganhHoc.Text.Trim();

            facultyBL.UpdateFaculty(strNewFacultyName, strOldFacultyName, strDescription);
            BindRptFaculties();
        }
        #endregion       

        #region Pager event handlers
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currnetPageIndx;
            BindRptFaculties();
        }
        #endregion

        #region Repeater event handlers
        protected void RptFaculties_ItemDataBound(object sender, RepeaterItemEventArgs e)
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