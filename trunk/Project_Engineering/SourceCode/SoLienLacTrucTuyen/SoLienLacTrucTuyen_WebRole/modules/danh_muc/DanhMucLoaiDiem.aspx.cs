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
using SoLienLacTrucTuyen;
using System.Text.RegularExpressions;
using SoLienLacTrucTuyen.BusinessEntity;

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
            if (isAccessDenied)
            {
                return;
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

        public void BindRptLoaiDiem()
        {
            string tenLoaiDiem = TxtSearchLoaiDiem.Text.Trim();
            double totalRecords = 0;
            
            List<DanhMuc_LoaiDiem> lstLoaiDiem = loaiDiemBL.GetListMarkTypes(
                tenLoaiDiem,
                PagerMain.CurrentIndex, PagerMain.PageSize, out totalRecords);
            PagerMain.ItemCount = totalRecords;

            // Decrease page current index when delete
            if (lstLoaiDiem.Count == 0 && totalRecords != 0)
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

        private bool ValidateForAdd(string tenLoaiDiem, string maxMarksPerTerm)
        {
            if (tenLoaiDiem == "")
            {
                TenLoaiDiemRequiredAdd.IsValid = false;
                TxtTenLoaiDiem.Focus();
                MPEAdd.Show();
                return false;
            }
            else
            {
                if (!Regex.IsMatch(TxtHeSoDiemLoaiDiemAdd.Text.Trim(), HeSoDiemRegExpAdd.ValidationExpression))
                {
                    HeSoDiemRegExpAdd.IsValid = false;
                    MPEAdd.Show();
                    return false;
                }
                else
                {
                    if (loaiDiemBL.MarkTypeNameExists(tenLoaiDiem))
                    {
                        TenLoaiDiemValidatorAdd.IsValid = false;
                        TxtTenLoaiDiem.Focus();
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
                DanhMuc_LoaiDiem appliedCalAvgMarkType = loaiDiemBL.GetAppliedCalAvgMarkType();
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

        private bool ValidateForEdit(string editedMarkTypeName, string tenLoaiDiem, string maxMarksPerTerm)
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
            if (tenLoaiDiem == "")
            {
                TenLoaiDiemRequiredEdit.IsValid = false;
                modalPopupEdit.Show();
                return false;
            }
            else
            {
                if (!Regex.IsMatch(TxtHeSoDiemLoaiDiemSua.Text.Trim(), HeSoDiemRegExpEdit.ValidationExpression))
                {
                    HeSoDiemRegExpEdit.IsValid = false;
                    modalPopupEdit.Show();
                    return false;
                }
                else
                {
                    if (loaiDiemBL.MarkTypeNameExists(editedMarkTypeName, tenLoaiDiem))
                    {
                        TenLoaiDiemValidatorEdit.IsValid = false;
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
                DanhMuc_LoaiDiem appliedCalAvgMarkType = loaiDiemBL.GetAppliedCalAvgMarkType();
                if (appliedCalAvgMarkType != null)
                {
                    if (appliedCalAvgMarkType.TenLoaiDiem != tenLoaiDiem)
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
            string tenLoaiDiem = this.TxtTenLoaiDiem.Text.Trim();
            double HeSoDiemLoaiDiem = Double.Parse(this.TxtHeSoDiemLoaiDiemAdd.Text.Trim());
            HeSoDiemLoaiDiem = Math.Round(HeSoDiemLoaiDiem, 1, MidpointRounding.AwayFromZero);
            string maxMarksPerTerm = this.TxtMaxMarksPerTermAdd.Text.Trim();
            bool calAverageMark = this.RbtnYesAdd.Checked;

            bool bValidInput = ValidateForAdd(tenLoaiDiem, maxMarksPerTerm);
            if (bValidInput)
            {
                loaiDiemBL.InsertLoaiDiem(tenLoaiDiem, HeSoDiemLoaiDiem,
                    short.Parse(maxMarksPerTerm), calAverageMark);

                PagerMain.CurrentIndex = 1;
                BindRptLoaiDiem();

                this.TxtTenLoaiDiem.Text = "";
                this.TxtHeSoDiemLoaiDiemAdd.Text = "";
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
            DanhMuc_LoaiDiem markType = new DanhMuc_LoaiDiem();
            markType.MaLoaiDiem = Int32.Parse(this.HdfSltMarkTypeId.Value);

            loaiDiemBL.DeleteMarkType(markType);
            isSearch = false;
            BindRptLoaiDiem();
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            string editedMarkTypeName = this.HdfEditedMarkTypeName.Value;
            string newMarkTypeName = TxtSuaTenLoaiDiem.Text.Trim();
            double heSoDiem = Double.Parse(TxtHeSoDiemLoaiDiemSua.Text.Trim());
            string maxMarksPerTerm = this.TxtMaxMarksPerTermEdit.Text.Trim();
            bool calAverageMark = this.RbtnYesEdit.Checked;

            bool bValidInput = ValidateForEdit(editedMarkTypeName, newMarkTypeName, maxMarksPerTerm);
            if (bValidInput)
            {
                loaiDiemBL.UpdateMarkType(editedMarkTypeName, newMarkTypeName, heSoDiem,
                    short.Parse(maxMarksPerTerm), calAverageMark);
                BindRptLoaiDiem();
            }
        }
        #endregion

        #region Repeater event handlers
        protected void RptLoaiDiem_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (e.Item.DataItem != null)
                    {
                        DanhMuc_LoaiDiem markType = (DanhMuc_LoaiDiem)e.Item.DataItem;
                        if (!loaiDiemBL.IsDeletable(markType.TenLoaiDiem))
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

                        HiddenField hdfRptMaLoaiDiem = (HiddenField)e.Item.FindControl("HdfRptMaLoaiDiem");
                        this.HdfSltMarkTypeId.Value = hdfRptMaLoaiDiem.Value;

                        this.HdfRptLoaiDiemMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        string markTypeName = (string)e.CommandArgument;
                        DanhMuc_LoaiDiem markType = loaiDiemBL.GetMarkType(markTypeName);
                        this.HdfSltMarkTypeId.Value = markType.MaLoaiDiem.ToString();
                        TxtSuaTenLoaiDiem.Text = markType.TenLoaiDiem;
                        TxtHeSoDiemLoaiDiemSua.Text = markType.HeSoDiem.ToString();
                        TxtMaxMarksPerTermEdit.Text = markType.SoCotToiDa.ToString();
                        RbtnYesEdit.Checked = markType.TinhDTB;
                        RbtnCancelEdit.Checked = !markType.TinhDTB;
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