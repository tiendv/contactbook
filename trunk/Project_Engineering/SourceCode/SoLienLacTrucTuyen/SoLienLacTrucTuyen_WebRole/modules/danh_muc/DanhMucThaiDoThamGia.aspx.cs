using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using AjaxControlToolkit;
using SoLienLacTrucTuyen.BusinessEntity;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class DanhMucThaiDoThamGia : BaseContentPage
    {
        #region Fields
        private AttitudeBL attitudeBL;
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

            attitudeBL = new AttitudeBL(UserSchool);
            if (!Page.IsPostBack)
            {
                isSearch = false;
                MainDataPager.CurrentIndex = 1;
                BindRepeater();
            }

            ProcPermissions();
        }
        #endregion

        #region Button click event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            isSearch = true;
            BindRepeater();
        }

        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {
            string strAttitudeName = this.TxtAttitudeName.Text.Trim();
            if (strAttitudeName == "")
            {
                AttitudeNameRequiredAdd.IsValid = false;
                MPEAdd.Show();
                return;
            }
            else
            {
                if (attitudeBL.AttitudeNameExists(strAttitudeName))
                {
                    AttitudeNameValidatorAdd.IsValid = false;
                    MPEAdd.Show();
                    return;
                }
            }

            attitudeBL.InsertThaiDoThamGia(new Category_Attitude
            {
                AttitudeName = strAttitudeName
            });

            MainDataPager.CurrentIndex = 1;
            BindRepeater();

            this.TxtAttitudeName.Text = "";

            if (this.CkbAddAfterSave.Checked)
            {
                this.MPEAdd.Show();
            }
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            int attitudeId = Int32.Parse(this.HdfAttitudeId.Value);
            Category_Attitude attitude = new Category_Attitude();
            attitude.AttitudeId = attitudeId;
            attitudeBL.DeleteAttitude(attitude);
            isSearch = false;
            BindRepeater();
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender modalPopupEdit = new ModalPopupExtender();
            foreach (RepeaterItem rptItem in RptThaiDoThamGia.Items)
            {
                if (rptItem.ItemType == ListItemType.Item || rptItem.ItemType == ListItemType.AlternatingItem)
                {
                    modalPopupEdit = (ModalPopupExtender)rptItem.FindControl("MPEEdit");
                    if (modalPopupEdit.ClientID == HdfRptThaiDoThamGiaMPEEdit.Value)
                    {
                        break;
                    }
                }
            }

            int attitudeId = Int32.Parse(this.HdfAttitudeId.Value);
            string oldAttitudeName = this.HdfSltAttitudeName.Value;
            string newAttitudeName = this.TxtSuaAttitudeName.Text.Trim();
            if (newAttitudeName == "")
            {
                AttitudeNameRequiredEdit.IsValid = false;
                modalPopupEdit.Show();
                return;
            }
            else
            {
                if (attitudeBL.AttitudeNameExists(oldAttitudeName, newAttitudeName))
                {
                    AttitudeNameValidatorEdit.IsValid = false;
                    modalPopupEdit.Show();
                    return;
                }
            }

            Category_Attitude attitude = new Category_Attitude();
            attitude.AttitudeId = attitudeId;
            attitudeBL.UpdateAttitude(attitude, newAttitudeName);
            BindRepeater();
        }
        #endregion

        #region Repeater event handlers
        protected void RptThaiDoThamGia_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
                if (e.Item.ItemType == ListItemType.Item
                    || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (e.Item.DataItem != null)
                    {
                        Category_Attitude attitude = (Category_Attitude)e.Item.DataItem;
                        if (!attitudeBL.IsDeletable(attitude))
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

        protected void RptThaiDoThamGia_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDeleteItem":
                    {
                        this.LblConfirmDelete.Text = "Bạn có chắc xóa thái độ tham gia <b>" + e.CommandArgument + "</b> này không?";
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        HiddenField hdfRptAttitudeId = (HiddenField)e.Item.FindControl("HdfRptAttitudeId");
                        this.HdfAttitudeId.Value = hdfRptAttitudeId.Value;

                        this.HdfRptThaiDoThamGiaMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        int attitudeId = Int32.Parse(e.CommandArgument.ToString());
                        Category_Attitude attitude = attitudeBL.GetAttitude(attitudeId);

                        TxtSuaAttitudeName.Text = attitude.AttitudeName;
                        ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        mPEEdit.Show();

                        this.HdfRptThaiDoThamGiaMPEEdit.Value = mPEEdit.ClientID;
                        this.HdfAttitudeId.Value = attitudeId.ToString();
                        this.HdfSltAttitudeName.Value = attitude.AttitudeName;
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
        public void pager_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currnetPageIndx;
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

        public void BindRepeater()
        {
            string strAttitudeName = TxtSearchThaiDoThamGia.Text.Trim();

            double dTotalRecords = 0;
            List<Category_Attitude> attitudes;
            attitudes = attitudeBL.GetAttitudes(strAttitudeName,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            // Decrease page current index when delete
            if (attitudes.Count == 0 && dTotalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRepeater();
                return;
            }

            MainDataPager.ItemCount = dTotalRecords;

            bool bDisplayData = (attitudes.Count != 0) ? true : false;
            PnlPopupConfirmDelete.Visible = bDisplayData;
            PnlPopupEdit.Visible = bDisplayData;
            RptThaiDoThamGia.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin thái độ tham gia";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy thái độ tham gia";
                }

                MainDataPager.CurrentIndex = 1;
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
            }
            else
            {
                MainDataPager.Visible = true;
            }

            RptThaiDoThamGia.DataSource = attitudes;
            RptThaiDoThamGia.DataBind();
        }
        #endregion
    }
}