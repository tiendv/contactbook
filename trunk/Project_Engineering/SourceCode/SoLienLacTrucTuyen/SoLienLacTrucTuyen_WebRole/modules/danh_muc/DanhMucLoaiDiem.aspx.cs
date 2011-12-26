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
                isSearch = false;
                PagerMain.CurrentIndex = 1;
                BindRptLoaiDiem();
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

        public void BindRptLoaiDiem()
        {
            string MarkTypeName = TxtSearchLoaiDiem.Text.Trim();
            double dTotalRecords = 0;
            
            List<Category_MarkType> lstLoaiDiem = loaiDiemBL.GetListMarkTypes(
                MarkTypeName,
                PagerMain.CurrentIndex, PagerMain.PageSize, out dTotalRecords);
            PagerMain.ItemCount = dTotalRecords;

            // Decrease page current index when delete
            if (lstLoaiDiem.Count == 0 && dTotalRecords != 0)
            {
                PagerMain.CurrentIndex--;
                BindRptLoaiDiem();
                return;
            }

            bool bDisplayData = (lstLoaiDiem.Count != 0) ? true : false;
            PnlPopupConfirmDelete.Visible = bDisplayData;
            PnlPopupEdit.Visible = bDisplayData;
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

            RptLoaiDiem.DataSource = lstLoaiDiem;
            RptLoaiDiem.DataBind();
        }

        private bool ValidateForAdd(string MarkTypeName, string maxMarksPerTerm)
        {
            if (MarkTypeName == "")
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
                    if (loaiDiemBL.MarkTypeNameExists(MarkTypeName))
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
                Category_MarkType appliedCalAvgMarkType = loaiDiemBL.GetAppliedCalAvgMarkType();
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

        private bool ValidateForEdit(string editedMarkTypeName, string MarkTypeName, string maxMarksPerTerm)
        {
            // validate page
            if (!Page.IsValid)
            {
                return false;
            }

            // get modalPopupEdit
            ModalPopupExtender modalPopupEdit = new ModalPopupExtender();
            foreach (RepeaterItem rptItem in RptLoaiDiem.Items)
            {
                if (rptItem.ItemType == ListItemType.Item || rptItem.ItemType == ListItemType.AlternatingItem)
                {
                    modalPopupEdit = (ModalPopupExtender)rptItem.FindControl("MPEEdit");
                    if (modalPopupEdit.ClientID == HdfRptLoaiDiemMPEEdit.Value)
                    {
                        break;
                    }
                }
            }

            // validate blank
            if (MarkTypeName == "")
            {
                MarkTypeNameRequiredEdit.IsValid = false;
                modalPopupEdit.Show();
                return false;
            }
            else
            {
                if (!Regex.IsMatch(TxtMarkRatioLoaiDiemSua.Text.Trim(), MarkRatioRegExpEdit.ValidationExpression))
                {
                    MarkRatioRegExpEdit.IsValid = false;
                    modalPopupEdit.Show();
                    return false;
                }
                else
                {
                    if (loaiDiemBL.MarkTypeNameExists(editedMarkTypeName, MarkTypeName))
                    {
                        MarkTypeNameValidatorEdit.IsValid = false;
                        modalPopupEdit.Show();
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
                modalPopupEdit.Show();
                return false;
            }

            if (RbtnYesEdit.Checked)
            {
                Category_MarkType appliedCalAvgMarkType = loaiDiemBL.GetAppliedCalAvgMarkType();
                if (appliedCalAvgMarkType != null)
                {
                    if (appliedCalAvgMarkType.MarkTypeName != MarkTypeName)
                    {
                        LblAppCalAvgMarkEdit.Visible = true;
                        modalPopupEdit.Show();
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

            bool bValidInput = ValidateForAdd(MarkTypeName, maxMarksPerTerm);
            if (bValidInput)
            {
                loaiDiemBL.InsertLoaiDiem(MarkTypeName, MarkRatioLoaiDiem,
                    short.Parse(maxMarksPerTerm), calAverageMark);

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
            Category_MarkType markType = new Category_MarkType();
            markType.MarkTypeId = Int32.Parse(this.HdfSltMarkTypeId.Value);

            loaiDiemBL.DeleteMarkType(markType);
            isSearch = false;
            BindRptLoaiDiem();
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            string editedMarkTypeName = this.HdfEditedMarkTypeName.Value;
            string newMarkTypeName = TxtSuaMarkTypeName.Text.Trim();
            double MarkRatio = Double.Parse(TxtMarkRatioLoaiDiemSua.Text.Trim());
            string maxMarksPerTerm = this.TxtMaxMarksPerTermEdit.Text.Trim();
            bool calAverageMark = this.RbtnYesEdit.Checked;

            bool bValidInput = ValidateForEdit(editedMarkTypeName, newMarkTypeName, maxMarksPerTerm);
            if (bValidInput)
            {
                loaiDiemBL.UpdateMarkType(editedMarkTypeName, newMarkTypeName, MarkRatio,
                    short.Parse(maxMarksPerTerm), calAverageMark);
                BindRptLoaiDiem();
            }
        }
        #endregion

        #region Repeater event handlers
        protected void RptLoaiDiem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (accessibilities.Contains(AccessibilityEnum.Modify))
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

            if (accessibilities.Contains(AccessibilityEnum.Delete))
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (e.Item.DataItem != null)
                    {
                        Category_MarkType markType = (Category_MarkType)e.Item.DataItem;
                        if (!loaiDiemBL.IsDeletable(markType.MarkTypeName))
                        {
                            ImageButton btnDeleteItem = (ImageButton)e.Item.FindControl("BtnDeleteItem");
                            btnDeleteItem.ImageUrl = "~/Styles/Images/button_delete_disable.png";
                            btnDeleteItem.Enabled = false;
                        }
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

        protected void RptLoaiDiem_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDeleteItem":
                    {
                        this.LblConfirmDelete.Text = "Bạn có chắc xóa loại điểm <b>" + e.CommandArgument + "</b> này không?";
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        HiddenField hdfRptMarkTypeId = (HiddenField)e.Item.FindControl("HdfRptMarkTypeId");
                        this.HdfSltMarkTypeId.Value = hdfRptMarkTypeId.Value;

                        this.HdfRptLoaiDiemMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        string markTypeName = (string)e.CommandArgument;
                        Category_MarkType markType = loaiDiemBL.GetMarkType(markTypeName);
                        this.HdfSltMarkTypeId.Value = markType.MarkTypeId.ToString();
                        TxtSuaMarkTypeName.Text = markType.MarkTypeName;
                        TxtMarkRatioLoaiDiemSua.Text = markType.MarkRatio.ToString();
                        TxtMaxMarksPerTermEdit.Text = markType.MaxQuantity.ToString();
                        RbtnYesEdit.Checked = markType.IsUsedForCalculatingAvg;
                        RbtnCancelEdit.Checked = !markType.IsUsedForCalculatingAvg;
                        LblAppCalAvgMarkEdit.Visible = false;

                        ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        mPEEdit.Show();

                        this.HdfRptLoaiDiemMPEEdit.Value = mPEEdit.ClientID;
                        this.HdfEditedMarkTypeName.Value = markTypeName;

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
        public void PagerMain_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            this.PagerMain.CurrentIndex = currnetPageIndx;
            BindRptLoaiDiem();
        }
        #endregion  
    }
}