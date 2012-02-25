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
        private TeachingPeriodBL teachingPeriodBL;
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

            teachingPeriodBL = new TeachingPeriodBL(UserSchool);

            if (!Page.IsPostBack)
            {
                BindDropDownLists();
                isSearch = false;
                BindRptTeachingPeriod();
            }

            ProcPermissions();
        }
        #endregion

        #region Button event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            isSearch = true;
            BindRptTeachingPeriod();
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

            teachingPeriodBL.InsertTeachingPeriod(TeachingPeriodNameHoc, session, thuTu, strThoiGianBatDau, strThoiGianKetThuc);

            MainDataPager.CurrentIndex = 1;
            BindRptTeachingPeriod();

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

        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            HiddenField HdfRptTeachingPeriodId = null;
            foreach (RepeaterItem item in RptTietHoc.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox CkbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (CkbxSelect.Checked)
                    {
                        HdfRptTeachingPeriodId = (HiddenField)item.FindControl("HdfRptTeachingPeriodId");
                        Category_TeachingPeriod teachingPeriod = teachingPeriodBL.GetTeachingPeriod(Int32.Parse(HdfRptTeachingPeriodId.Value));
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

                        HdfTeachingPeriodId.Value = teachingPeriod.TeachingPeriodId.ToString();
                        MPEEdit.Show();
                        return;
                    }
                }
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

            int TeachingPeriodId = Int32.Parse(this.HdfTeachingPeriodId.Value);
            string TeachingPeriodNameMoi = this.TxtTeachingPeriodNameHocEdit.Text.Trim();
            int buoi = Int32.Parse(DdlBuoiEdit.SelectedValue);
            session.SessionId = buoi;
            string thuTu = TxtThuTuEdit.Text.Trim();
            string strThoiGianBatDau = TxtThoiGianBatDauEdit.Text.Trim();
            string strThoiGianKetThuc = TxtThoiGianKetThucEdit.Text.Trim();
            teachingPeriod.TeachingPeriodId = TeachingPeriodId;
            teachingPeriodBL.UpdateTiet(teachingPeriod, TeachingPeriodNameMoi, session, thuTu, strThoiGianBatDau, strThoiGianKetThuc);
            BindRptTeachingPeriod();
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            bool bInfoInUse = false;
            CheckBox ckbxSelect = null;
            HiddenField HdfRptTeachingPeriodId = null;
            Category_TeachingPeriod teachingPeriod = null;

            foreach (RepeaterItem item in RptTietHoc.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    ckbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (ckbxSelect.Checked)
                    {
                        HdfRptTeachingPeriodId = (HiddenField)item.FindControl("HdfRptTeachingPeriodId");
                        teachingPeriod = new Category_TeachingPeriod();
                        teachingPeriod.TeachingPeriodId = Int32.Parse(HdfRptTeachingPeriodId.Value);

                        if (teachingPeriodBL.IsDeletable(teachingPeriod))
                        {
                            teachingPeriodBL.DeleteTeachingPeriod(teachingPeriod);
                        }
                        else
                        {
                            bInfoInUse = true;
                        }
                    }
                }
            }

            isSearch = false;
            BindRptTeachingPeriod();

            if (bInfoInUse)
            {
                MPEInfoInUse.Show();
            }
        }
        #endregion

        #region Pager event handlers
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currentPageIndex;
            BindRptTeachingPeriod();
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

        private void BindRptTeachingPeriod()
        {
            Configuration_Session session = null;
            double dTotalRecords;
            string strTeachingPeriodName = TxtSearchTiet.Text.Trim();

            if (DdlBuoi.SelectedIndex != 0)
            {
                session = new Configuration_Session();
                session.SessionId = Int32.Parse(DdlBuoi.SelectedValue);
            }

            List<TabularTeachingPeriod> tabularTeachingPeriods = teachingPeriodBL.GetTabularTeachingPeriods(strTeachingPeriodName, session,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            // Decrease page current index when delete
            if (tabularTeachingPeriods.Count == 0 && dTotalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRptTeachingPeriod();
                return;
            }

            MainDataPager.ItemCount = dTotalRecords;
            bool bDisplayData = (tabularTeachingPeriods.Count != 0) ? true : false;
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
            RptTietHoc.DataSource = tabularTeachingPeriods;
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
                MPEAdd.Show();
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
                if (teachingPeriodBL.TeachingPeriodNameExists(TeachingPeriodNameHoc))
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

            int TeachingPeriodId = Int32.Parse(this.HdfTeachingPeriodId.Value);
            string oldTeachingPeriodName = this.HdfSltTeachingPeriodName.Value;
            string TeachingPeriodNameMoi = this.TxtTeachingPeriodNameHocEdit.Text.Trim();
            string thuTu = TxtThuTuEdit.Text.Trim();

            if (TeachingPeriodNameMoi == "")
            {
                TeachingPeriodNameHocRequiredEdit.IsValid = false;
                MPEEdit.Show();
                return false;
            }
            else
            {
                if (teachingPeriodBL.TeachingPeriodNameExists(oldTeachingPeriodName, TeachingPeriodNameMoi))
                {
                    TeachingPeriodNameHocValidatorEdit.IsValid = false;
                    MPEEdit.Show();
                    return false;
                }
            }

            if (!Regex.IsMatch(thuTu, ThuTuRegExp.ValidationExpression))
            {
                ThuTuRegExp.IsValid = false;
                MPEEdit.Show();
                return false;
            }

            return true;
        }

        protected void ValidateBeginTimeAdd(object source, ServerValidateEventArgs args)
        {
            args.IsValid = false;
            return;


            if (DdlBuoi.SelectedIndex >= 0)
            {
                // session is "Morning"
                if (DdlBuoi.SelectedIndex == 0)
                {
                    if (string.Compare("12:00", args.Value) <= 0)
                    {
                        args.IsValid = false;
                        BeginTimeCustomValidatorAdd.ErrorMessage = "Thời gian bắt đầu phải nhỏ hơn 12:00";
                    }
                }
                else // session is "Evening"
                {
                    if (string.Compare(args.Value, "12:00") < 0)
                    {
                        args.IsValid = false;
                        BeginTimeCustomValidatorAdd.ErrorMessage = "Thời gian bắt đầu phải lớn hơn hoặc bằng 12:00";
                    }
                }
            }

            args.IsValid = true;
        }

        protected void ValidateEndTimeAdd(object source, ServerValidateEventArgs args)
        {
            if (DdlBuoi.SelectedIndex >= 0)
            {
                // session is "Morning"
                if (DdlBuoi.SelectedIndex == 0)
                {
                    if (string.Compare("12:00", args.Value) <= 0)
                    {
                        args.IsValid = false;
                        EndTimeCustomValidatorAdd.ErrorMessage = "Thời gian kết thúc phải nhỏ hơn 12:00";
                    }
                }
            }

            args.IsValid = true;
        }

        protected void ValidateBeginTimeModify(object source, ServerValidateEventArgs args)
        {
            if (DdlBuoi.SelectedIndex >= 0)
            {
                // session is "Morning"
                if (DdlBuoi.SelectedIndex == 0)
                {
                    if (string.Compare("12:00", args.Value) <= 0)
                    {
                        args.IsValid = false;
                        BeginTimeCustomValidatorAdd.ErrorMessage = "Thời gian bắt đầu phải nhỏ hơn 12:00";
                    }
                }
                else // session is "Evening"
                {
                    if (string.Compare(args.Value, "12:00") < 0)
                    {
                        args.IsValid = false;
                        BeginTimeCustomValidatorAdd.ErrorMessage = "Thời gian bắt đầu phải lớn hơn hoặc bằng 12:00";
                    }
                }
            }

            args.IsValid = true;
        }

        protected void ValidateEndTimeModify(object source, ServerValidateEventArgs args)
        {
            if (DdlBuoi.SelectedIndex >= 0)
            {
                // session is "Morning"
                if (DdlBuoi.SelectedIndex == 0)
                {
                    if (string.Compare("12:00", args.Value) <= 0)
                    {
                        args.IsValid = false;
                        EndTimeCustomValidatorAdd.ErrorMessage = "Thời gian kết thúc phải nhỏ hơn 12:00";
                    }
                }
            }

            args.IsValid = true;
        }
        #endregion

        #region Repeater event handlers
        protected void RptTietHoc_ItemDataBound(object sender, RepeaterItemEventArgs e)
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