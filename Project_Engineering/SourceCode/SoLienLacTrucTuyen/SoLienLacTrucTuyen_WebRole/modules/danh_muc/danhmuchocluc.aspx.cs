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
    public partial class DanhMucHocLuc : BaseContentPage
    {
        #region Fields
        private UserBL userBL;
        private HocLucBL hocLucBL;
        private bool isSearch;

        protected string btnSaveAddClickEvent = string.Empty;
        protected string btnSaveEditClickEvent = string.Empty;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (isAccessDenied)
            {
                return;
            }

            hocLucBL = new HocLucBL(UserSchool);

            if (!Page.IsPostBack)
            {
                isSearch = false;
                PagerMain.CurrentIndex = 1;
                BindData();
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
            string tenHocLuc = TxtSearchHocLuc.Text.Trim();

            List<DanhMuc_HocLuc> lstHocLuc;
            if (String.Compare(tenHocLuc, "tất cả", true) == 0 || tenHocLuc == "")
            {
                lstHocLuc = hocLucBL.GetListHocLuc(PagerMain.CurrentIndex, PagerMain.PageSize);
                PagerMain.ItemCount = hocLucBL.GetHocLucCount();
            }
            else
            {
                lstHocLuc = hocLucBL.GetListHocLuc(tenHocLuc, PagerMain.CurrentIndex, PagerMain.PageSize);
                PagerMain.ItemCount = hocLucBL.GetHocLucCount(tenHocLuc);
            }

            // Decrease page current index when delete
            if (lstHocLuc.Count == 0 && PagerMain.ItemCount != 0)
            {
                PagerMain.CurrentIndex--;
                BindData();
                return;
            }

            bool bDisplayData = (lstHocLuc.Count != 0) ? true : false;
            PnlPopupConfirmDelete.Visible = bDisplayData;
            PnlPopupEdit.Visible = bDisplayData;
            RptHocLuc.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin học lực";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy học lực";
                }

                PagerMain.CurrentIndex = 1;
                PagerMain.ItemCount = 0;
                PagerMain.Visible = false;
            }
            else
            {
                PagerMain.Visible = true;
            }

            RptHocLuc.DataSource = lstHocLuc;
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
            PagerMain.CurrentIndex = 1;

            string tenHocLuc = this.TxtTenHocLucThem.Text;
            float dtbDau = float.Parse(this.TxtDTBTuThem.Text);
            float dtbCuoi = float.Parse(this.TxtDTBDenThem.Text);

            hocLucBL.InsertHocLuc(new DanhMuc_HocLuc
            {
                TenHocLuc = tenHocLuc,
                DTBDau = dtbDau,
                DTBCuoi = dtbCuoi
            });

            BindData();

            this.TxtTenHocLucThem.Text = "";
            this.TxtDTBTuThem.Text = "";
            this.TxtDTBDenThem.Text = "";

            if (this.CkbAddAfterSave.Checked)
            {
                this.MPEAdd.Show();
            }
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            int maHocLuc = Int32.Parse(this.HdfMaHocLuc.Value);
            hocLucBL.DeleteHocLuc(maHocLuc);
            isSearch = false;
            BindData();
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            int maHocLuc = Int32.Parse(this.HdfMaHocLuc.Value);
            string tenHocLuc = TxtSuaTenHocLuc.Text;
            float dtbDau = float.Parse(TxtHeSoDiemHocLucSua.Text);
            float dtbCuoi = 0; // float.Parse(TxtHeSoDiemHocLucSua.Text);
            hocLucBL.UpdateHocLuc(maHocLuc, tenHocLuc, dtbDau, dtbCuoi);
            BindData();
        }
        #endregion

        #region Repeater event handlers
        protected void RptHocLuc_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DanhMuc_HocLuc HocLuc = (DanhMuc_HocLuc)e.Item.DataItem;
                if (!hocLucBL.CheckCanDeleteHocLuc(HocLuc.MaHocLuc))
                {
                    ImageButton btnDeleteItem = (ImageButton)e.Item.FindControl("BtnDeleteItem");
                    btnDeleteItem.ImageUrl = "~/Styles/Images/button_delete_disable.png";
                    btnDeleteItem.Enabled = false;
                }
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

                        HiddenField hdfRptMaHocLuc = (HiddenField)e.Item.FindControl("HdfRptMaHocLuc");
                        this.HdfMaHocLuc.Value = hdfRptMaHocLuc.Value;

                        this.HdfRptHocLucMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        int maHocLuc = Int32.Parse(e.CommandArgument.ToString());
                        DanhMuc_HocLuc HocLuc = hocLucBL.GetHocLuc(maHocLuc);

                        TxtSuaTenHocLuc.Text = HocLuc.TenHocLuc;
                        TxtHeSoDiemHocLucSua.Text = HocLuc.DTBDau.ToString();
                        ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        mPEEdit.Show();

                        this.HdfRptHocLucMPEEdit.Value = mPEEdit.ClientID;
                        this.HdfMaHocLuc.Value = maHocLuc.ToString();

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