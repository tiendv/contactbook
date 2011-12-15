﻿using System;
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
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen_WebRole
{
    public partial class FacultyCategoryPage : BaseContentPage
    {
        #region Fields
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

            facultyBL = new FacultyBL(UserSchool);

            if (!Page.IsPostBack)
            {
                isSearch = false;
                MainDataPager.CurrentIndex = 1;

                ViewState["SortColumn"] = "FacultyName";
                ViewState["SortOrder"] = "ASC";
                BindData();
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
                BtnAdd.ImageUrl = "~/Styles/Images/button_add_with_text.png";
                PnlPopupAdd.Visible = true;
            }
            else
            {
                BtnAdd.Visible = false;
                PnlPopupAdd.Visible = false;
            }
        }

        public void BindData()
        {
            string facultyName = TxtSearchNganhHoc.Text.Trim();
            double dTotalRecords;
            List<Category_Faculty> faculties = facultyBL.GetFaculties(facultyName, MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);
            
            // Decrease page current index when delete
            if (faculties.Count == 0 && dTotalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindData();
                return;
            }

            bool bDisplayData = (faculties.Count != 0) ? true : false;
            PnlPopupConfirmDelete.Visible = bDisplayData;
            PnlPopupEdit.Visible = bDisplayData;
            RptNganhHoc.Visible = bDisplayData;
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
            }
            else
            {
                MainDataPager.Visible = true;
            }

            RptNganhHoc.DataSource = faculties;
            RptNganhHoc.DataBind();
            MainDataPager.ItemCount = dTotalRecords;
        }
        #endregion

        #region Button click event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            MainDataPager.ItemCount = 0;
            isSearch = true;
            BindData();
        }

        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {

            if (!Page.IsValid)
            {
                return;
            }

            string facultyName = this.TxtFacultyName.Text.Trim();

            if (facultyName == "")
            {
                FacultyNameRequiredAdd.IsValid = false;
                MPEAdd.Show();
                return;
            }
            else
            {
                if (facultyBL.FacultyExists(facultyName))
                {
                    FacultyNameValidatorAdd.IsValid = false;
                    MPEAdd.Show();
                    return;
                }
            }

            string description = this.TxtDescriptionNganhHoc.Text.Trim();
            Category_Faculty faculty = new Category_Faculty
            {
                FacultyName = facultyName,
                Description = description
            };
            facultyBL.InsertFaculty(faculty);

            MainDataPager.CurrentIndex = 1;
            BindData();

            this.TxtFacultyName.Text = "";
            this.TxtDescriptionNganhHoc.Text = "";

            if (this.CkbAddAfterSave.Checked)
            {
                this.MPEAdd.Show();
            }
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            string deletedFacultyName = this.HdfDeletedFacultyName.Value;
            Category_Faculty faculty = facultyBL.GetFaculty(deletedFacultyName);
            facultyBL.DeleteFaculty(faculty);
            isSearch = false;
            BindData();
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender modalPopupEdit = new ModalPopupExtender();
            foreach (RepeaterItem rptItem in RptNganhHoc.Items)
            {
                if (rptItem.ItemType == ListItemType.Item || rptItem.ItemType == ListItemType.AlternatingItem)
                {
                    modalPopupEdit = (ModalPopupExtender)rptItem.FindControl("MPEEdit");
                    if (modalPopupEdit.ClientID == HdfRptNganhHocMPEEdit.Value)
                    {
                        break;
                    }
                }
            }

            if (!Page.IsValid)
            {
                return;
            }

            string editedFacultyName = (string)this.HdfEditedFacultyName.Value;
            string newFacultyName = this.TxtFacultyNameEdit.Text.Trim();
            string newDescription = this.TxtSuaDescriptionNganhHoc.Text.Trim();

            if (newFacultyName == "")
            {
                FacultyNameRequiredEdit.IsValid = false;
                modalPopupEdit.Show();
                return;
            }
            else
            {
                if (facultyBL.FacultyExists(editedFacultyName, newFacultyName))
                {
                    FacultyNameValidatorEdit.IsValid = false;
                    modalPopupEdit.Show();
                    return;
                }
            }

            facultyBL.UpdateFaculty(editedFacultyName, newFacultyName, newDescription);
            BindData();
        }
        #endregion

        #region Repeater event handlers
        protected void RptNganhHoc_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (accessibilities.Contains(AccessibilityEnum.Modify))
            {
                // Do nothing
            }
            else
            {
                if (e.Item.ItemType == ListItemType.Header)
                {
                    e.Item.FindControl("thEdit").Visible = false;
                }

                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    e.Item.FindControl("tdEdit").Visible = false;
                }

                PnlPopupEdit.Visible = false;
            }

            if (accessibilities.Contains(AccessibilityEnum.Delete))
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    Category_Faculty faculty = (Category_Faculty)e.Item.DataItem;
                    if (!facultyBL.IsDeletable(faculty))
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

        protected void RptNganhHoc_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDeleteItem":
                    {
                        this.LblConfirmDelete.Text = "Bạn có chắc xóa ngành học <b>" + e.CommandArgument + "</b> này không?";
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        HiddenField hdfRptFacultyId = (HiddenField)e.Item.FindControl("HdfRptFacultyId");
                        this.HdfFacultyId.Value = hdfRptFacultyId.Value;
                        this.HdfDeletedFacultyName.Value = (string)e.CommandArgument;
                        this.HdfRptNganhHocMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        string facultyName = (string)e.CommandArgument;

                        Category_Faculty faculty = facultyBL.GetFaculty(facultyName);
                        TxtFacultyNameEdit.Text = faculty.FacultyName;
                        TxtSuaDescriptionNganhHoc.Text = faculty.Description;

                        ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        mPEEdit.Show();

                        this.HdfRptNganhHocMPEEdit.Value = mPEEdit.ClientID;
                        this.HdfEditedFacultyName.Value = facultyName;

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
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currnetPageIndx;
            BindData();
        }
        #endregion
    }
}