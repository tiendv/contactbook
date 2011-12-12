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

            learningAptitudeBL = new LearningAptitudeBL(UserSchool);

            if (!Page.IsPostBack)
            {
                isSearch = false;
                PagerMain.CurrentIndex = 1;
                BindData();
            }
            ProcPermissions();
        }

        private void ProcPermissions()
        {
            if (accessibilities.Contains(SoLienLacTrucTuyen.BusinessEntity.AccessibilityEnum.Add))
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

        protected void Page_PreRender(object sender, EventArgs e)
        {
            RegisterScript();
        }
        #endregion

        #region Methods
        private void RegisterScript()
        {
            btnSaveAddClickEvent = Page.ClientScript.GetPostBackEventReference(this.BtnSaveAdd, string.Empty);
            btnSaveEditClickEvent = Page.ClientScript.GetPostBackEventReference(this.BtnSaveEdit, string.Empty);
        }

        public void BindData()
        {
            string strLearningAptitudeName = TxtSearchHocLuc.Text.Trim();

            double dTotalRecords;
            List<Category_LearningAptitude> learningAptitudes;
            learningAptitudes = learningAptitudeBL.GetListHocLuc(strLearningAptitudeName, PagerMain.CurrentIndex, PagerMain.PageSize,
                out dTotalRecords);
            PagerMain.ItemCount = dTotalRecords;

            // Decrease page current index when delete
            if (learningAptitudes.Count == 0 && PagerMain.ItemCount != 0)
            {
                PagerMain.CurrentIndex--;
                BindData();
                return;
            }

            bool bDisplayData = (learningAptitudes.Count != 0) ? true : false;
            PnlPopupConfirmDelete.Visible = bDisplayData;
            PnlPopupEdit.Visible = bDisplayData;
            RptHocLuc.Visible = bDisplayData;
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
        #endregion

        #region Button click event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            PagerMain.CurrentIndex = 1;
            PagerMain.ItemCount = 0;
            isSearch = true;
            BindData();
        }

        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {
            //PagerMain.CurrentIndex = 1;

            //string LearningAptitudeName = this.TxtLearningAptitudeNameThem.Text;
            //float BeginAverageMark = float.Parse(this.TxtDTBTuThem.Text);
            //float EndAverageMark = float.Parse(this.TxtDTBDenThem.Text);

            //hocLucBL.InsertHocLuc(new Category_LearningAptitude
            //{
            //    LearningAptitudeName = LearningAptitudeName,
            //    BeginAverageMark = BeginAverageMark,
            //    EndAverageMark = EndAverageMark
            //});

            //BindData();

            //this.TxtLearningAptitudeNameThem.Text = "";
            //this.TxtDTBTuThem.Text = "";
            //this.TxtDTBDenThem.Text = "";

            //if (this.CkbAddAfterSave.Checked)
            //{
            //    this.MPEAdd.Show();
            //}
            string LearningAptitudeName = this.TxtLearningAptitudeNameThem.Text;

            if (LearningAptitudeName == "")
            {
                LearningAptitudeNameRequiredAdd.IsValid = false;
                TxtLearningAptitudeNameThem.Focus();
                MPEAdd.Show();
                return;
            }
            else
            {
                if (learningAptitudeBL.ConductNameExists(LearningAptitudeName))
                {
                    LearningAptitudeNameValidatorAdd.IsValid = false;
                    TxtLearningAptitudeNameThem.Focus();
                    MPEAdd.Show();
                    return;
                }
            }
            double BeginAverageMark = double.Parse(this.TxtDTBTuThem.Text);
            double EndAverageMark = double.Parse(this.TxtDTBDenThem.Text);
            learningAptitudeBL.InsertConduct(new Category_LearningAptitude
            {
                LearningAptitudeName = LearningAptitudeName,
                BeginAverageMark = BeginAverageMark,
                EndAverageMark = EndAverageMark
            });

            //PagerMain.CurrentIndex = 1;
            BindData();

            this.TxtLearningAptitudeNameThem.Text = "";

            if (this.CkbAddAfterSave.Checked)
            {
                this.MPEAdd.Show();
            }
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            //int LearningAptitudeId = Int32.Parse(this.HdfLearningAptitudeId.Value);
            //hocLucBL.DeleteHocLuc(LearningAptitudeId);
            //isSearch = false;
            //BindData();
            int LearningAptitudeId = Int32.Parse(this.HdfLearningAptitudeId.Value);

            Category_LearningAptitude conduct = new Category_LearningAptitude();
            conduct.LearningAptitudeId = LearningAptitudeId;

            learningAptitudeBL.DeleteConduct(conduct);
            isSearch = false;
            BindData();
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            //int LearningAptitudeId = Int32.Parse(this.HdfLearningAptitudeId.Value);
            //string LearningAptitudeName = TxtSuaLearningAptitudeName.Text;
            //float BeginAverageMark = 0;// float.Parse(TxtMarkRatioHocLucSua.Text);
            //float EndAverageMark = 0; // float.Parse(TxtMarkRatioHocLucSua.Text);
            //hocLucBL.UpdateHocLuc(LearningAptitudeId, LearningAptitudeName, BeginAverageMark, EndAverageMark);
            //BindData();
            ModalPopupExtender modalPopupEdit = new ModalPopupExtender();
            foreach (RepeaterItem rptItem in RptHocLuc.Items)
            {
                if (rptItem.ItemType == ListItemType.Item || rptItem.ItemType == ListItemType.AlternatingItem)
                {
                    modalPopupEdit = (ModalPopupExtender)rptItem.FindControl("MPEEdit");
                    if (modalPopupEdit.ClientID == HdfRptHocLucMPEEdit.Value)
                    {
                        break;
                    }
                }
            }

            if (!Page.IsValid)
            {
                return;
            }

            string editedConductName = (string)HdfEditedConductName.Value;
            string newConductName = TxtSuaLearningAptitudeName.Text.Trim();

            if (newConductName == "")
            {
                LearningAptitudeNameRequiredEdit.IsValid = false;
                modalPopupEdit.Show();
                return;
            }
            //else
            //{
            //    if (hocLucBL.ConductNameExists(editedConductName, newConductName))
            //    {
            //        LearningAptitudeNameValidatorEdit.IsValid = false;
            //        modalPopupEdit.Show();
            //        return;
            //    }
            //}
            double BeginAverageMark = double.Parse(TxtDTBTuSua.Text);
            double EndAverageMark = double.Parse(TxtDTBDenSua.Text);

            int editedConductId = Int32.Parse(this.HdfLearningAptitudeId.Value);
            learningAptitudeBL.UpdateConduct(editedConductId, newConductName, BeginAverageMark, EndAverageMark);
            BindData();
        }
        #endregion

        #region Repeater event handlers
        protected void RptHocLuc_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //{
            //    Category_LearningAptitude HocLuc = (Category_LearningAptitude)e.Item.DataItem;
            //    if (!hocLucBL.CheckCanDeleteHocLuc(HocLuc.LearningAptitudeId))
            //    {
            //        ImageButton btnDeleteItem = (ImageButton)e.Item.FindControl("BtnDeleteItem");
            //        btnDeleteItem.ImageUrl = "~/Styles/Images/button_delete_disable.png";
            //        btnDeleteItem.Enabled = false;
            //    }
            //}
            if (accessibilities.Contains(SoLienLacTrucTuyen.BusinessEntity.AccessibilityEnum.Modify))
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

            if (accessibilities.Contains(SoLienLacTrucTuyen.BusinessEntity.AccessibilityEnum.Delete))
            {
                if (e.Item.ItemType == ListItemType.Item
                    || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (e.Item.DataItem != null)
                    {
                        Category_LearningAptitude conduct = (Category_LearningAptitude)e.Item.DataItem;

                        if (!learningAptitudeBL.IsDeletable(conduct.LearningAptitudeName))
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

        protected void RptHocLuc_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDeleteItem":
                    {
                        this.LblConfirmDelete.Text = "Bạn có chắc xóa học lực <b>" + e.CommandArgument + "</b> này không?";
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        HiddenField hdfRptLearningAptitudeId = (HiddenField)e.Item.FindControl("HdfRptLearningAptitudeId");
                        this.HdfLearningAptitudeId.Value = hdfRptLearningAptitudeId.Value;

                        this.HdfRptHocLucMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        int LearningAptitudeId = Int32.Parse(e.CommandArgument.ToString());
                        Category_LearningAptitude HocLuc = learningAptitudeBL.GetLearningAptitude(LearningAptitudeId);

                        TxtSuaLearningAptitudeName.Text = HocLuc.LearningAptitudeName;
                        //TxtMarkRatioHocLucSua.Text = HocLuc.BeginAverageMark.ToString();
                        TxtDTBTuSua.Text = HocLuc.BeginAverageMark.ToString();
                        TxtDTBDenSua.Text = HocLuc.EndAverageMark.ToString();
                        ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        mPEEdit.Show();

                        this.HdfRptHocLucMPEEdit.Value = mPEEdit.ClientID;
                        this.HdfLearningAptitudeId.Value = LearningAptitudeId.ToString();

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
            BindData();
        }
        #endregion     
    }
}