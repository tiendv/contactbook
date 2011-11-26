using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;
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
            if (isAccessDenied)
            {
                return;
            }

            facultyBL = new FacultyBL(UserSchool);

            if (!Page.IsPostBack)
            {
                isSearch = false;
                MainDataPager.CurrentIndex = 1;

                ViewState["SortColumn"] = "TenNganhHoc";
                ViewState["SortOrder"] = "ASC";
                BindData();
            }

            ProcPermissions();
        }
        #endregion

        #region Methods
        private void ProcPermissions()
        {
            if (lstAccessibilities.Contains(AccessibilityEnum.Add))
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
            List<DanhMuc_NganhHoc> faculties = facultyBL.GetFaculties(facultyName, MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);
            
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

            string facultyName = this.TxtTenNganhHoc.Text.Trim();

            if (facultyName == "")
            {
                TenNganhHocRequiredAdd.IsValid = false;
                MPEAdd.Show();
                return;
            }
            else
            {
                if (facultyBL.FacultyExists(facultyName))
                {
                    TenNganhHocValidatorAdd.IsValid = false;
                    MPEAdd.Show();
                    return;
                }
            }

            string description = this.TxtMoTaNganhHoc.Text.Trim();
            DanhMuc_NganhHoc faculty = new DanhMuc_NganhHoc
            {
                TenNganhHoc = facultyName,
                MoTa = description
            };
            facultyBL.InsertFaculty(faculty);

            MainDataPager.CurrentIndex = 1;
            BindData();

            this.TxtTenNganhHoc.Text = "";
            this.TxtMoTaNganhHoc.Text = "";

            if (this.CkbAddAfterSave.Checked)
            {
                this.MPEAdd.Show();
            }
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            string deletedFacultyName = this.HdfDeletedFacultyName.Value;
            DanhMuc_NganhHoc faculty = facultyBL.GetFaculty(deletedFacultyName);
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
            string newFacultyName = this.TxtTenNganhHocEdit.Text.Trim();
            string newDescription = this.TxtSuaMoTaNganhHoc.Text.Trim();

            if (newFacultyName == "")
            {
                TenNganhHocRequiredEdit.IsValid = false;
                modalPopupEdit.Show();
                return;
            }
            else
            {
                if (facultyBL.FacultyExists(editedFacultyName, newFacultyName))
                {
                    TenNganhHocValidatorEdit.IsValid = false;
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
            if (lstAccessibilities.Contains(AccessibilityEnum.Modify))
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

            if (lstAccessibilities.Contains(AccessibilityEnum.Delete))
            {
                if (e.Item.ItemType == ListItemType.Item
                    || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DanhMuc_NganhHoc faculty = (DanhMuc_NganhHoc)e.Item.DataItem;
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

                        HiddenField hdfRptMaNganhHoc = (HiddenField)e.Item.FindControl("HdfRptMaNganhHoc");
                        this.HdfMaNganhHoc.Value = hdfRptMaNganhHoc.Value;
                        this.HdfDeletedFacultyName.Value = (string)e.CommandArgument;
                        this.HdfRptNganhHocMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        string facultyName = (string)e.CommandArgument;

                        DanhMuc_NganhHoc faculty = facultyBL.GetFaculty(facultyName);
                        TxtTenNganhHocEdit.Text = faculty.TenNganhHoc;
                        TxtSuaMoTaNganhHoc.Text = faculty.MoTa;

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