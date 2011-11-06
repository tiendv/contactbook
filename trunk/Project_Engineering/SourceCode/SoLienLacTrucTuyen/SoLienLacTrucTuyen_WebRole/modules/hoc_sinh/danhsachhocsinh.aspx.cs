using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;
using AjaxControlToolkit;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class DanhSachHocSinh : System.Web.UI.Page
    {
        #region Fields
        private HocSinhBL hocSinhBL;
        private LopHocBL lopHocBL;

        private bool isSearch;
        private List<AccessibilityEnum> lstAccessibilities;
        #endregion
        
        #region Page event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            RoleBL roleBL = new RoleBL();
            UserBL userBL = new UserBL();
            hocSinhBL = new HocSinhBL();
            lopHocBL = new LopHocBL();

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

            lstAccessibilities = roleBL.GetAccessibilities(role, pageUrl);

            if (!Page.IsPostBack)
            {
                BindDropDownLists();
                this.isSearch = false;

                // Khôi phục lại thông tin khi chuyển sang trang khác rồi trở về trang này
                if (Request.QueryString["SNam"] != null && Request.QueryString["SNganh"] != null
                    && Request.QueryString["SKhoi"] != null && Request.QueryString["SLop"] != null
                    && Request.QueryString["SMa"] != null && Request.QueryString["STen"] != null)
                {
                    HdfSearchNamHoc.Value = Request.QueryString["SNam"];
                    HdfSearchNganhHoc.Value = Request.QueryString["SNganh"];
                    HdfSearchKhoiLop.Value = Request.QueryString["SKhoi"];
                    HdfSearchKhoiLop.Value = Request.QueryString["SLop"];
                    HdfSearchMaHocSinh.Value = Request.QueryString["SMa"];
                    HdfSearchTenHocSinh.Value = Request.QueryString["STen"];
                    DdlNamHoc.SelectedValue = HdfSearchNamHoc.Value;
                    DdlNganh.SelectedValue = HdfSearchNganhHoc.Value;
                    DdlKhoiLop.SelectedValue = HdfSearchKhoiLop.Value;
                    DdlLopHoc.SelectedValue = HdfSearchLopHoc.Value;
                    TxtMaHocSinh.Text = HdfSearchMaHocSinh.Value;
                    TxtTenHocSinh.Text = HdfSearchTenHocSinh.Value;
                    isSearch = true;
                }

                if (DdlLopHoc.Items.Count != 0)
                {
                    BindRepeaterHocSinh();
                }
                else
                {
                    ProcessDislayInfo(false);
                }

                ProcPermissions();
            }
        }
        #endregion

        #region DropDownList event hanlders
        protected void DdlNamHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropDownListLopHoc();
        }

        protected void DdlNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropDownListLopHoc();
        }

        protected void DdlKhoiLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropDownListLopHoc();
        }
        #endregion

        #region Repeater event handlers
        protected void RptHocSinh_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
            }

            if (lstAccessibilities.Contains(AccessibilityEnum.Delete))
            {
                if (e.Item.ItemType == ListItemType.Item
                    || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    Control control = e.Item.FindControl("LbtnMaHocSinhHienThi");
                    if (control != null)
                    {
                        string maHocSinhHienThi = ((LinkButton)control).Text;
                        if (!hocSinhBL.CanDeleteHocSinh(maHocSinhHienThi))
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

        protected void RptHocSinh_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDetailItem":
                    {
                        //string query = "HocSinh=" + e.CommandArgument;
                        //string saveSearchQuery = "&SNam=" + HdfSearchNamHoc.Value + "&SNganh=" + HdfSearchNganhHoc.Value
                        //    + "&SKhoi=" + HdfSearchKhoiLop.Value + "&SLop=" + HdfSearchLopHoc.Value
                        //    + "&STen=" + HdfSearchTenHocSinh.Value + "&SMa=" + HdfSearchMaHocSinh.Value;
                        HiddenField hdfMaLopHoc = (HiddenField)e.Item.FindControl("HdfMaLopHoc");
                        Response.Redirect(String.Format("thongtincanhan.aspx?hocsinh={0}&lop={1}",
                            e.CommandArgument, hdfMaLopHoc.Value));
                        break;
                    }
                case "CmdEditItem":
                    {
                        HiddenField hdfMaLopHoc = (HiddenField)e.Item.FindControl("HdfMaLopHoc");
                        Response.Redirect(String.Format("suahocsinh.aspx?hocsinh={0}&lop={1}", 
                            e.CommandArgument, hdfMaLopHoc.Value));
                        break;
                    }
                case "CmdDeleteItem":
                    {
                        // Set confirm text and show dialog
                        this.LblConfirmDelete.Text = "Bạn có chắc xóa học sinh <b>" + e.CommandArgument + "</b> này không?";
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        // Save current MaHocSinh to global
                        HiddenField hdfRptMaHocSinh = (HiddenField)e.Item.FindControl("HdfRptMaHocSinh");
                        this.HdfMaHocSinh.Value = hdfRptMaHocSinh.Value;

                        // Save modal popup ClientID
                        this.HdfRptHocSinhMPEDelete.Value = mPEDelete.ClientID;

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
            BindRepeaterHocSinh();

            // Save searched info
            this.HdfSearchNamHoc.Value = this.DdlNamHoc.SelectedValue;
            this.HdfSearchKhoiLop.Value = this.DdlKhoiLop.SelectedValue;
            this.HdfSearchNganhHoc.Value = this.DdlNganh.SelectedValue;
            this.HdfSearchLopHoc.Value = this.DdlLopHoc.SelectedValue;
            this.HdfSearchTenHocSinh.Value = this.TxtTenHocSinh.Text;
            this.HdfSearchMaHocSinh.Value = this.TxtMaHocSinh.Text;
        }

        protected void BtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("themhocsinh.aspx");
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            int maHocSinh = Int32.Parse(this.HdfMaHocSinh.Value);
            hocSinhBL.DeleteHocSinh(maHocSinh);

            isSearch = false;
            BindDropDownListLopHoc();
            BindRepeaterHocSinh();
        }
        #endregion

        #region Pager event handlers
        public void pager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currentPageIndex;
            BindRepeaterHocSinh();
        }
        #endregion

        #region Methods
        private void ProcPermissions()
        {
            if (lstAccessibilities.Contains(AccessibilityEnum.Add))
            {
                BtnAdd.Enabled = true;
                BtnAdd.ImageUrl = "~/Styles/Images/button_add_with_text.png";
            }
            else
            {
                BtnAdd.Visible = false;
            }
        }

        private void BindRepeaterHocSinh()
        {
            List<TabularHocSinhInfo> lstTabularHocSinhInfo = new List<TabularHocSinhInfo>();
            double totalRecords;

            string maHocSinhHienThi = this.TxtMaHocSinh.Text;
            if ((string.Compare(maHocSinhHienThi, "tất cả", true) == 0) || (maHocSinhHienThi == ""))
            {
                int maNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);

                int maNganhHoc = 0;
                try
                {
                    maNganhHoc = Int32.Parse(DdlNganh.SelectedValue);
                }
                catch (Exception) { }

                int maKhoiLop = 0;
                try
                {
                    maKhoiLop = Int32.Parse(DdlKhoiLop.SelectedValue);
                }
                catch (Exception) { }

                int maLopHoc = 0;
                try
                {
                    maLopHoc = Int32.Parse(DdlLopHoc.SelectedValue);
                }
                catch (Exception) { }

                string tenHocSinh = this.TxtTenHocSinh.Text;

                lstTabularHocSinhInfo = hocSinhBL.GetListTabularHocSinhInfo(maNamHoc, maNganhHoc, maKhoiLop,
                    maLopHoc, tenHocSinh,
                    MainDataPager.CurrentIndex, MainDataPager.PageSize, out totalRecords);
            }
            else
            {
                lstTabularHocSinhInfo = hocSinhBL.GetListTabularHocSinhInfo(maHocSinhHienThi, out totalRecords);
            }

            MainDataPager.ItemCount = totalRecords;

            // Decrease page current index when delete
            if (lstTabularHocSinhInfo.Count == 0 && MainDataPager.ItemCount != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRepeaterHocSinh();
                return;
            }

            bool bDisplayData = (lstTabularHocSinhInfo.Count != 0) ? true : false;
            ProcessDislayInfo(bDisplayData);
            RptHocSinh.DataSource = lstTabularHocSinhInfo;
            RptHocSinh.DataBind();
        }

        private void ProcessDislayInfo(bool bDisplayData)
        {
            PnlPopupConfirmDelete.Visible = bDisplayData;
            RptHocSinh.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin học sinh";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy học sinh";
                }
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
            }
            else
            {
                MainDataPager.Visible = true;
            }
        }

        private void BindDropDownLists()
        {
            BindDropDownListNamHoc();

            BindDropDownListNganhHoc();

            BindDropDownListKhoiLop();

            BindDropDownListLopHoc();
        }

        private void BindDropDownListNganhHoc()
        {
            FacultyBL nganhHocBL = new FacultyBL();
            List<DanhMuc_NganhHoc> lstNganhHoc = nganhHocBL.GetFaculties();
            DdlNganh.DataSource = lstNganhHoc;
            DdlNganh.DataValueField = "MaNganhHoc";
            DdlNganh.DataTextField = "TenNganhHoc";
            DdlNganh.DataBind();
            if (lstNganhHoc.Count > 1)
            {
                DdlNganh.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }

        private void BindDropDownListKhoiLop()
        {
            GradeBL KhoiLopBL = new GradeBL();
            List<DanhMuc_KhoiLop> lstKhoiLop = KhoiLopBL.GetListGrades();
            DdlKhoiLop.DataSource = lstKhoiLop;
            DdlKhoiLop.DataValueField = "MaKhoiLop";
            DdlKhoiLop.DataTextField = "TenKhoiLop";
            DdlKhoiLop.DataBind();
            if (lstKhoiLop.Count > 1)
            {
                DdlKhoiLop.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }

        private void BindDropDownListNamHoc()
        {
            NamHocBL namHocBL = new NamHocBL();
            List<CauHinh_NamHoc> lstNamHoc = namHocBL.GetListNamHoc();
            DdlNamHoc.DataSource = lstNamHoc;
            DdlNamHoc.DataValueField = "MaNamHoc";
            DdlNamHoc.DataTextField = "TenNamHoc";
            DdlNamHoc.DataBind();

            if (DdlNamHoc.Items.Count != 0)
            {
                SystemConfigBL cauHinhBL = new SystemConfigBL();
                DdlNamHoc.SelectedValue = cauHinhBL.GetCurrentYear().ToString();
            }
        }

        private void BindDropDownListLopHoc()
        {
            if (DdlNamHoc.Items.Count == 0 || DdlNganh.Items.Count == 0 || DdlKhoiLop.Items.Count == 0)
            {
                BtnSearch.ImageUrl = "~/Styles/Images/button_search_with_text_disable.png";
                BtnSearch.Enabled = false;

                BtnAdd.ImageUrl = "~/Styles/Images/button_add_with_text_disable.png";
                BtnAdd.Enabled = false;

                PnlPopupConfirmDelete.Visible = false;
                RptHocSinh.Visible = false;
                LblSearchResult.Visible = true;
                LblSearchResult.Text = "Chưa có thông tin HocSinh";

                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;

                return;
            }

            int maNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);

            int maNganhHoc = 0;
            try
            {
                maNganhHoc = Int32.Parse(DdlNganh.SelectedValue);
            }
            catch (Exception) { }

            int maKhoiLop = 0;
            try
            {
                maKhoiLop = Int32.Parse(DdlKhoiLop.SelectedValue);
            }
            catch (Exception) { }

            List<LopHoc_Lop> lstLop = GetListLopHoc(maNganhHoc, maKhoiLop, maNamHoc);
            DdlLopHoc.DataSource = lstLop;
            DdlLopHoc.DataValueField = "MaLopHoc";
            DdlLopHoc.DataTextField = "TenLopHoc";
            DdlLopHoc.DataBind();
            if (lstLop.Count > 1)
            {
                DdlLopHoc.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }

        private List<LopHoc_Lop> GetListLopHoc(int maNganhHoc, int maKhoiLop, int maNamHoc)
        {
            List<LopHoc_Lop> lstLop = lopHocBL.GetListLopHoc(maNganhHoc, maKhoiLop, maNamHoc);
            return lstLop;
        }
        #endregion
    }
}