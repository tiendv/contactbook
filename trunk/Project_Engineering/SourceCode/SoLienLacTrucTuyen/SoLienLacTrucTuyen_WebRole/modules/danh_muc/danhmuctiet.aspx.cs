using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;
using AjaxControlToolkit;
using SoLienLacTrucTuyen;
using System.Text.RegularExpressions;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class DanhMucTietPage : BaseContentPage
    {
        #region Fields
        private TeachingPeriodBL tietBL;
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

            tietBL = new TeachingPeriodBL(UserSchool);

            if (!Page.IsPostBack)
            {
                BindDropDownLists();
                isSearch = false;
                BindRepeater();
            }

            ProcPermissions();
        }
        #endregion

        #region Repeater event handlers
        protected void RptTietHoc_ItemDataBound(object sender, RepeaterItemEventArgs e)
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

                //PnlPopupEdit.Visible = false;
            }

            if (accessibilities.Contains(AccessibilityEnum.Delete))
            {
                if (e.Item.ItemType == ListItemType.Item
                    || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (e.Item.DataItem != null)
                    {
                        TabularTeachingPeriod tiet = (TabularTeachingPeriod)e.Item.DataItem;
                        Category_TeachingPeriod teachingPeriodTime = new Category_TeachingPeriod();
                        teachingPeriodTime.TeachingPeriodId = tiet.TeachingPeriodId;
                        if (!tietBL.IsDeletable(teachingPeriodTime))
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

        protected void RptTietHoc_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDeleteItem":
                    {
                        // Set confirm text and show dialog
                        this.LblConfirmDelete.Text = string.Format("Bạn có chắc xóa tiết học \"<b>{0}</b>\" này không?", e.CommandArgument);
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        // Save current TeachingPeriodIdHoc to global
                        HiddenField hdfRptTeachingPeriodIdHoc = (HiddenField)e.Item.FindControl("HdfRptTeachingPeriodIdHoc");
                        this.HdfTeachingPeriodIdHoc.Value = hdfRptTeachingPeriodIdHoc.Value;

                        // Save modal popup ClientID
                        this.HdfRptTietHocMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        int TeachingPeriodId = Int32.Parse(e.CommandArgument.ToString());

                        Category_TeachingPeriod teachingPeriod = tietBL.GetTeachingPeriod(TeachingPeriodId);
                        this.HdfSltTeachingPeriodName.Value = teachingPeriod.TeachingPeriodName;

                        TxtTeachingPeriodNameHocEdit.Text = teachingPeriod.TeachingPeriodName;
                        TxtThuTuEdit.Text = teachingPeriod.TeachingPeriodOrder.ToString();
                        DdlBuoiEdit.SelectedValue = teachingPeriod.SessionId.ToString();
                        DateTime dtThoiGianBatDau = teachingPeriod.BeginTime;
                        TxtThoiGianBatDauEdit.Text = string.Format("{0}:{1}",
                            dtThoiGianBatDau.Hour, dtThoiGianBatDau.Minute);
                        DateTime dtThoiGianKetThuc = teachingPeriod.EndTime;
                        TxtThoiGianKetThucEdit.Text = string.Format("{0}:{1}",
                            dtThoiGianKetThuc.Hour, dtThoiGianKetThuc.Minute);
                        ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        mPEEdit.Show();

                        this.HdfTeachingPeriodIdHoc.Value = TeachingPeriodId.ToString();
                        this.HdfRptTietHocMPEEdit.Value = mPEEdit.ClientID;

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            isSearch = true;
            BindRepeater();
        }

        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {
            Configuration_Session session = new Configuration_Session();

            if (!ValidateForAdd())
            {
                return;
            }

            string TeachingPeriodNameHoc = this.TxtTeachingPeriodNameHocThem.Text.Trim();
            string thuTu = this.TxtThuTuAdd.Text.Trim();
            int buoi = Int32.Parse(DdlBuoiAdd.SelectedValue);
            session.SessionId = buoi;
            string strThoiGianBatDau = TxtThoiGianBatDauAdd.Text.Trim();
            string strThoiGianKetThuc = TxtThoiGianKetThucAdd.Text.Trim();

            tietBL.InsertTeachingPeriod(TeachingPeriodNameHoc, session, thuTu, strThoiGianBatDau, strThoiGianKetThuc);

            MainDataPager.CurrentIndex = 1;
            BindRepeater();

            TxtTeachingPeriodNameHocThem.Text = "";
            TxtThuTuAdd.Text = "";
            TxtThoiGianBatDauAdd.Text = "";
            TxtThoiGianKetThucAdd.Text = "";

            if (this.CkbAddAfterSave.Checked)
            {
                this.MPEAdd.Show();
            }
            else
            {
                this.DdlBuoiAdd.SelectedIndex = 0;
            }
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            Category_TeachingPeriod teachingPeriod = new Category_TeachingPeriod();
            Configuration_Session session = new Configuration_Session();
            if (!ValidateForEdit())
            {
                return;
            }

            int TeachingPeriodId = Int32.Parse(this.HdfTeachingPeriodIdHoc.Value);
            string TeachingPeriodNameMoi = this.TxtTeachingPeriodNameHocEdit.Text.Trim();
            int buoi = Int32.Parse(DdlBuoiEdit.SelectedValue);
            session.SessionId = buoi;
            string thuTu = TxtThuTuEdit.Text.Trim();
            string strThoiGianBatDau = TxtThoiGianBatDauEdit.Text.Trim();
            string strThoiGianKetThuc = TxtThoiGianKetThucEdit.Text.Trim();
            teachingPeriod.TeachingPeriodId = TeachingPeriodId;
            tietBL.UpdateTiet(teachingPeriod, TeachingPeriodNameMoi, session, thuTu, strThoiGianBatDau, strThoiGianKetThuc);
            BindRepeater();
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            int TeachingPeriodIdHoc = Int32.Parse(this.HdfTeachingPeriodIdHoc.Value);
            Category_TeachingPeriod teachingPeriod = new Category_TeachingPeriod();
            teachingPeriod.TeachingPeriodId = TeachingPeriodIdHoc;
            tietBL.DeleteTeachingPeriod(teachingPeriod);
            isSearch = false;
            BindRepeater();
        }
        #endregion

        #region Pager event handlers
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currentPageIndex;
            BindRepeater();
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

        private void BindRepeater()
        {
            Configuration_Session session = null;
            double dTotalRecords;
            string TeachingPeriodName = TxtSearchTiet.Text.Trim();

            if (DdlBuoi.SelectedIndex != 0)
            {
                session = new Configuration_Session();
                session.SessionId = Int32.Parse(DdlBuoi.SelectedValue);
            }

            List<TabularTeachingPeriod> listTbTiets = tietBL.GetTabularTeachingPeriods(TeachingPeriodName, session,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            // Decrease page current index when delete
            if (listTbTiets.Count == 0 && dTotalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRepeater();
                return;
            }

            MainDataPager.ItemCount = dTotalRecords;
            bool bDisplayData = (listTbTiets.Count != 0) ? true : false;
            PnlPopupConfirmDelete.Visible = bDisplayData;
            PnlPopupEdit.Visible = bDisplayData;
            RptTietHoc.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin tiết học";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy tiết học";
                }
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
            }
            else
            {
                MainDataPager.Visible = true;
            }
            RptTietHoc.DataSource = listTbTiets;
            RptTietHoc.DataBind();
        }

        private void BindDropDownLists()
        {
            BindDDLBuoi();
        }

        private void BindDDLBuoi()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<Configuration_Session> listBuois = systemConfigBL.GetSessions();
            DdlBuoi.DataSource = listBuois;
            DdlBuoi.DataValueField = "SessionId";
            DdlBuoi.DataTextField = "SessionName";
            DdlBuoi.DataBind();
            DdlBuoi.Items.Insert(0, new ListItem("Tất cả", "0"));

            DdlBuoiAdd.DataSource = listBuois;
            DdlBuoiAdd.DataValueField = "SessionId";
            DdlBuoiAdd.DataTextField = "SessionName";
            DdlBuoiAdd.DataBind();

            DdlBuoiEdit.DataSource = listBuois;
            DdlBuoiEdit.DataValueField = "SessionId";
            DdlBuoiEdit.DataTextField = "SessionName";
            DdlBuoiEdit.DataBind();
        }

        private bool ValidateForAdd()
        {
            if (!Page.IsValid)
            {
                return false;
            }

            string TeachingPeriodNameHoc = this.TxtTeachingPeriodNameHocThem.Text.Trim();
            string thuTu = this.TxtThuTuAdd.Text.Trim();

            if (TeachingPeriodNameHoc == "")
            {
                TeachingPeriodNameHocRequiredAdd.IsValid = false;
                MPEAdd.Show();
                return false;
            }
            else
            {
                if (tietBL.TeachingPeriodNameExists(TeachingPeriodNameHoc))
                {
                    TeachingPeriodNameHocValidatorAdd.IsValid = false;
                    MPEAdd.Show();
                    return false;
                }
            }

            if (!Regex.IsMatch(thuTu, ThuTuRegExp.ValidationExpression))
            {
                ThuTuRegExp.IsValid = false;
                MPEAdd.Show();
                return false;
            }

            return true;
        }

        private bool ValidateForEdit()
        {
            if (!Page.IsValid)
            {
                return false;
            }

            ModalPopupExtender modalPopupEdit = new ModalPopupExtender();
            foreach (RepeaterItem rptItem in RptTietHoc.Items)
            {
                if (rptItem.ItemType == ListItemType.Item || rptItem.ItemType == ListItemType.AlternatingItem)
                {
                    modalPopupEdit = (ModalPopupExtender)rptItem.FindControl("MPEEdit");
                    if (modalPopupEdit.ClientID == HdfRptTietHocMPEEdit.Value)
                    {
                        break;
                    }
                }
            }

            int TeachingPeriodId = Int32.Parse(this.HdfTeachingPeriodIdHoc.Value);
            string oldTeachingPeriodName = this.HdfSltTeachingPeriodName.Value;
            string TeachingPeriodNameMoi = this.TxtTeachingPeriodNameHocEdit.Text.Trim();
            string thuTu = TxtThuTuEdit.Text.Trim();

            if (TeachingPeriodNameMoi == "")
            {
                TeachingPeriodNameHocRequiredEdit.IsValid = false;
                modalPopupEdit.Show();
                return false;
            }
            else
            {
                if (tietBL.TeachingPeriodNameExists(oldTeachingPeriodName, TeachingPeriodNameMoi))
                {
                    TeachingPeriodNameHocValidatorEdit.IsValid = false;
                    modalPopupEdit.Show();
                    return false;
                }
            }

            if (!Regex.IsMatch(thuTu, ThuTuRegExp.ValidationExpression))
            {
                ThuTuRegExp.IsValid = false;
                modalPopupEdit.Show();
                return false;
            }

            return true;
        }
        #endregion
    }
}