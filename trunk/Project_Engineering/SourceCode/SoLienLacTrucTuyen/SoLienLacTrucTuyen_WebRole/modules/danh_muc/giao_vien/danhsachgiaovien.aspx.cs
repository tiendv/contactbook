using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.BusinessEntity;
using AjaxControlToolkit;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class DanhSachGiaoVien : System.Web.UI.Page
    {
        #region Fields
        private UserBL userBL;
        private RoleBL roleBL;
        private GiaoVienBL giaoVienBL;
        private bool isSearch;
        private List<AccessibilityEnum> lstAccessibilities;
        #endregion

        #region Page event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            userBL = new UserBL();
            roleBL = new RoleBL();
            giaoVienBL = new GiaoVienBL();

            string pageUrl = Page.Request.Path;
            Guid role = userBL.GetRoleId(User.Identity.Name);

            if (!roleBL.ValidateAuthorization(role, pageUrl))
            {
                Response.Redirect((string)GetGlobalResourceObject("MainResource", "AccessDeniedPageUrl"));
                return;
            }

            Site masterPage = (Site)Page.Master;
            masterPage.UserRole = role;
            masterPage.PageUrl = pageUrl;

            lstAccessibilities = roleBL.GetAccessibilities(
               userBL.GetRoleId(User.Identity.Name), Page.Request.Path);

            if (!Page.IsPostBack)
            {
                ProcPermissions();

                isSearch = false;
                MainDataPager.CurrentIndex = 1;

                BindDataRepeater();
            }
        }
        #endregion

        #region Methods
        public void BindDataRepeater()
        {
            string maHienThiGiaoVien = TxtSearchMaHienThiGiaoVien.Text.Trim();
            string hoTen = TxtSearchTenGiaoVien.Text.Trim();

            double totalRecords;
            List<TabularGiaoVien> lstTbGiaoViens = giaoVienBL.GetListTabularGiaoViens(
                maHienThiGiaoVien, hoTen,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out totalRecords);

            // Decrease page current index when delete
            if (lstTbGiaoViens.Count == 0 && totalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindDataRepeater();
                return;
            }

            bool bDisplayData = (lstTbGiaoViens.Count != 0) ? true : false;
            PnlPopupConfirmDelete.Visible = bDisplayData;
            RptGiaoVien.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin giáo viên";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy giáo viên";
                }

                MainDataPager.CurrentIndex = 1;
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
            }
            else
            {
                MainDataPager.Visible = true;
            }

            RptGiaoVien.DataSource = lstTbGiaoViens;
            RptGiaoVien.DataBind();
            MainDataPager.ItemCount = totalRecords;
        }

        private void ProcPermissions()
        {
            if (lstAccessibilities.Contains(AccessibilityEnum.Add))
            {
                BtnAddGiaoVien.Visible = true;
                BtnAddGiaoVien.ImageUrl = "~/Styles/Images/button_add_with_text.png";
            }
            else
            {
                BtnAddGiaoVien.Visible = false;
            }
        }
        #endregion

        #region Button click event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            MainDataPager.ItemCount = 0;
            isSearch = true;
            BindDataRepeater();
        }

        protected void BtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/modules/danh_muc/giao_vien/themgiaovien.aspx");
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            string teacherCode = this.HdfTeacherCode.Value;

            giaoVienBL.DeleteGiaoVien(teacherCode);

            isSearch = false;
            BindDataRepeater();
        }
        #endregion

        #region Repeater event handlers
        protected void RptGiaoVien_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (lstAccessibilities.Contains(AccessibilityEnum.Modify))
            {
                if (e.Item.ItemType == ListItemType.Item
                    || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    TabularGiaoVien giaoVien = (TabularGiaoVien)e.Item.DataItem;
                    int maGiaoVien = giaoVien.MaGiaoVien;
                    ImageButton btnEditItem = (ImageButton)e.Item.FindControl("BtnEditItem");
                    btnEditItem.Attributes.Add("OnClientClick",
                        "return editGiaoVien();");
                }
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
            }

            if (lstAccessibilities.Contains(AccessibilityEnum.Delete))
            {
                if (e.Item.ItemType == ListItemType.Item
                    || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    TabularGiaoVien giaoVien = (TabularGiaoVien)e.Item.DataItem;
                    int maGiaoVien = giaoVien.MaGiaoVien;
                    if (!giaoVienBL.IsDeletable(giaoVien.MaHienThiGiaoVien))
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

            if (e.Item.ItemType == ListItemType.Item
                    || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TabularGiaoVien giaoVien = (TabularGiaoVien)e.Item.DataItem;
                HyperLink hlkMaGiaoVien = (HyperLink)e.Item.FindControl("HlkMaGiaoVien");
                hlkMaGiaoVien.NavigateUrl = "chitietgiaovien.aspx?giaovien=" + giaoVien.MaGiaoVien
                    + "&prevpageid=1";
            }
        }

        protected void RptGiaoVien_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDeleteItem":
                    {
                        string teacherCode = (string)e.CommandArgument;
                        HdfTeacherCode.Value = teacherCode;

                        this.LblConfirmDelete.Text = "Bạn có chắc xóa giáo viên <b>\""
                            + giaoVienBL.GetTeacher(teacherCode).HoTen + "\"</b> này không?";
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        this.HdfMaGiaoVien.Value = e.CommandArgument.ToString();
                        this.HdfRptGiaoVienMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        Response.Redirect("suagiaovien.aspx?giaovien=" + e.CommandArgument + "&prevpageid=1");
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
        public void DataPager_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currnetPageIndx;
            BindDataRepeater();
        }
        #endregion
    }
}