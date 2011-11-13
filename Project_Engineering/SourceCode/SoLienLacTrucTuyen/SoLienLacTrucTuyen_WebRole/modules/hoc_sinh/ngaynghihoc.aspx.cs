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
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class NgayNghiHocPage : BaseContentPage
    {
        #region Fields
        private StudentBL hocSinhBL;
        private AbsentBL ngayNghiHocBL;
        private SystemConfigBL systemConfigBL;
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

            hocSinhBL = new StudentBL();
            ngayNghiHocBL = new AbsentBL();
            systemConfigBL = new SystemConfigBL();            

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["hocsinh"] != null)
                {
                    HdfMaHocSinh.Value = Request.QueryString["hocsinh"];
                    int maHocSinh = Int32.Parse(Request.QueryString["hocsinh"]);

                    BindDropDownLists();
                    InitDates();
                    isSearch = false;
                    BindRepeater();

                    HlkThongTinCaNhan.NavigateUrl = String.Format("thongtincanhan.aspx?hocsinh={0}", maHocSinh);
                    HlkKetQuaHocTap.NavigateUrl = String.Format("ketquahoctap.aspx?hocsinh={0}", maHocSinh);
                    HlkHoatDong.NavigateUrl = String.Format("hoatdong.aspx?hocsinh={0}", maHocSinh);
                }
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

        private void BindDropDownLists()
        {
            BindDropDownListNamHoc();
            BindDropDownListHocKy();
            BindDropDownListBuoi();
        }

        private void BindDropDownListNamHoc()
        {
            if (HdfMaHocSinh.Value != "")
            {
                HocSinh_ThongTinCaNhan student = new HocSinh_ThongTinCaNhan();
                student.MaHocSinh = Int32.Parse(HdfMaHocSinh.Value.ToString());
                List<CauHinh_NamHoc> lstNamHoc = hocSinhBL.GetYears(student);
                DdlNamHoc.DataSource = lstNamHoc;
                DdlNamHoc.DataValueField = "MaNamHoc";
                DdlNamHoc.DataTextField = "TenNamHoc";
                DdlNamHoc.DataBind();
            }
        }

        private void BindDropDownListHocKy()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL();
            List<CauHinh_HocKy> lstHocKy = systemConfigBL.GetListTerms();
            DdlHocKy.DataSource = lstHocKy;
            DdlHocKy.DataValueField = "MaHocKy";
            DdlHocKy.DataTextField = "TenHocKy";
            DdlHocKy.DataBind();
            DdlHocKy.SelectedValue = (new SystemConfigBL()).GetCurrentTerm().ToString();

            DdlHocKyThem.DataSource = lstHocKy;
            DdlHocKyThem.DataValueField = "MaHocKy";
            DdlHocKyThem.DataTextField = "TenHocKy";
            DdlHocKyThem.DataBind();
            DdlHocKyThem.SelectedValue = (new SystemConfigBL()).GetCurrentTerm().ToString();

            DdlHocKySua.DataSource = lstHocKy;
            DdlHocKySua.DataValueField = "MaHocKy";
            DdlHocKySua.DataTextField = "TenHocKy";
            DdlHocKySua.DataBind();
            DdlHocKySua.SelectedValue = (new SystemConfigBL()).GetCurrentTerm().ToString();
        }

        private void BindDropDownListBuoi()
        {
            List<CauHinh_Buoi> lstBuoi = systemConfigBL.GetSessions();
            DdlBuoiThem.DataSource = lstBuoi;
            DdlBuoiThem.DataValueField = "MaBuoi";
            DdlBuoiThem.DataTextField = "TenBuoi";
            DdlBuoiThem.DataBind();
            DdlBuoiThem.Items.Insert(0, new ListItem("Cả ngày", "0"));

            DdlBuoiSua.DataSource = lstBuoi;
            DdlBuoiSua.DataValueField = "MaBuoi";
            DdlBuoiSua.DataTextField = "TenBuoi";
            DdlBuoiSua.DataBind();
            DdlBuoiSua.Items.Insert(0, new ListItem("Cả ngày", "0"));
        }

        private void InitDates()
        {
            DateTime today = DateTime.Now;
            DateTime beginDateOfMonth = new DateTime(today.Year, today.Month, 1);
            TxtTuNgay.Text = beginDateOfMonth.ToShortDateString();
            DateTime dateOfNextMonth = today.AddMonths(1);
            DateTime beginDateOfNextMonth = new DateTime(dateOfNextMonth.Year, dateOfNextMonth.Month, 1);
            DateTime endDateOfMonth = beginDateOfNextMonth.AddDays(-1);
            TxtDenNgay.Text = endDateOfMonth.ToShortDateString();

            TxtNgayThem.Text = today.ToShortDateString();

            TxtNgaySua.Text = today.ToShortDateString();
        }

        private void BindRepeater()
        {
            HocSinh_ThongTinCaNhan student = null;
            CauHinh_NamHoc year = null;
            CauHinh_HocKy term = null;
            double totalRecords;
            List<TabularAbsent> tabularAbsents;

            student = new HocSinh_ThongTinCaNhan();
            student.MaHocSinh = Int32.Parse(this.HdfMaHocSinh.Value);
            year = new CauHinh_NamHoc();
            year.MaNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);
            term = new CauHinh_HocKy();
            term.MaHocKy = Int32.Parse(DdlHocKy.SelectedValue);
            DateTime dtBeginDate = DateTime.Parse(TxtTuNgay.Text);
            DateTime dtEndDate = DateTime.Parse(TxtDenNgay.Text);

            tabularAbsents = ngayNghiHocBL.GetTabularAbsents(student, year, term, dtBeginDate, dtEndDate,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out totalRecords);

            if (totalRecords != 0 && tabularAbsents.Count == 0)
            {
                MainDataPager.CurrentIndex--;
                BindRepeater();
                return;
            }

            bool bDisplayData = (tabularAbsents.Count != 0) ? true : false;
            ProcessDislayInfo(bDisplayData);
            RptNgayNghi.DataSource = tabularAbsents;
            RptNgayNghi.DataBind();
            MainDataPager.ItemCount = totalRecords;
        }

        private void ProcessDislayInfo(bool bDisplayData)
        {
            PnlPopupConfirmDelete.Visible = bDisplayData;
            PnlPopupEdit.Visible = bDisplayData;
            RptNgayNghi.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin ngày nghỉ học";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy ngày nghỉ học";
                }
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
            }
            else
            {
                MainDataPager.Visible = true;
            }
        }
        #endregion

        #region Repeater event handlers
        protected void RptNgayNghi_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HocSinh_NgayNghiHoc absent = null;

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
                    if (e.Item.DataItem != null)
                    {
                        Control control = e.Item.FindControl("HdfRptMaNgayNghiHoc");
                        if (control != null)
                        {
                            absent = new HocSinh_NgayNghiHoc();
                            absent.MaNgayNghiHoc = Int32.Parse(((HiddenField)control).Value);
                            if (ngayNghiHocBL.Confirmed(absent))
                            {
                                ImageButton btnDeleteItem = (ImageButton)e.Item.FindControl("BtnDeleteItem");
                                btnDeleteItem.ImageUrl = "~/Styles/Images/button_delete_disable.png";
                                btnDeleteItem.Enabled = false;

                                ImageButton btnEditItem = (ImageButton)e.Item.FindControl("BtnEditItem");
                                btnEditItem.ImageUrl = "~/Styles/Images/button_edit_disable.png";
                                btnEditItem.Enabled = false;
                            }
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

        protected void RptNgayNghi_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDeleteItem":
                    {
                        LblConfirmDelete.Text = "Bạn có chắc xóa ngày nghỉ học này không?";
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        HiddenField hdfRptMaNgayNghiHoc = (HiddenField)e.Item.FindControl("HdfRptMaNgayNghiHoc");
                        HdfMaNgayNghiHoc.Value = hdfRptMaNgayNghiHoc.Value;

                        HdfRptNgayNghiMPEDelete.Value = mPEDelete.ClientID;
                        break;
                    }
                case "CmdEditItem":
                    {
                        int maNgayNghiHoc = Int32.Parse(e.CommandArgument.ToString());
                        HocSinh_NgayNghiHoc ngayNghi = ngayNghiHocBL.GetAbsent(maNgayNghiHoc);

                        this.DdlHocKySua.SelectedValue = ngayNghi.MaHocKy.ToString();
                        this.TxtNgaySua.Text = ngayNghi.Ngay.ToShortDateString();
                        this.DdlBuoiSua.SelectedValue = ngayNghi.MaBuoi.ToString();
                        this.RbtnCoSua.Checked = ngayNghi.XinPhep;
                        this.RbtnKhongSua.Checked = !ngayNghi.XinPhep;
                        this.TxtLyDoSua.Text = ngayNghi.LyDo;

                        ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        mPEEdit.Show();

                        this.HdfMaNgayNghiHoc.Value = e.CommandArgument.ToString();
                        this.HdfRptNgayNghiMPEEdit.Value = mPEEdit.ClientID;

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
            HocSinh_ThongTinCaNhan student = null;
            CauHinh_HocKy term = null;
            CauHinh_Buoi session = null;

            string strDate = TxtNgayThem.Text.Trim();
            student = new HocSinh_ThongTinCaNhan();
            student.MaHocSinh = Int32.Parse(this.HdfMaHocSinh.Value);
            term = new CauHinh_HocKy();
            term.MaHocKy = Int32.Parse(this.DdlHocKyThem.SelectedValue);            
            if (this.DdlBuoiThem.SelectedIndex > 0)
            {
                session = new CauHinh_Buoi();
                session.MaBuoi = Int32.Parse(this.DdlBuoiThem.SelectedValue);
            }
            bool xinPhep = this.RbtnCo.Checked;
            string lyDo = this.TxtLyDoThem.Text.Trim();

            if (strDate == "")
            {
                NgayRequiredAdd.IsValid = false;
                MPEAdd.Show();
                return;
            }
            else
            {
                if (!Regex.IsMatch(strDate, NgayExpression.ValidationExpression))
                {
                    NgayExpression.IsValid = false;
                    MPEAdd.Show();
                    return;
                }

                try
                {
                    DateTime.Parse(TxtNgayThem.Text.Trim());
                }
                catch (Exception ex)
                {
                    DateTimeValidatorAdd.IsValid = false;
                    MPEAdd.Show();
                    return;
                }

                if (ngayNghiHocBL.AbsentExists(null, student, term, DateTime.Parse(this.TxtNgayThem.Text.Trim()), session))
                {
                    NgayValidatorAdd.IsValid = false;
                    MPEAdd.Show();
                    return;
                }
            }

            DateTime ngay = DateTime.Parse(this.TxtNgayThem.Text.Trim());

            ngayNghiHocBL.InsertAbsent(student, term, ngay, session, xinPhep, lyDo);

            MainDataPager.CurrentIndex = 1;
            BindRepeater();

            this.DdlHocKyThem.SelectedValue = (new SystemConfigBL()).GetCurrentTerm().MaHocKy.ToString();
            this.TxtNgayThem.Text = DateTime.Now.ToShortDateString();
            this.DdlBuoiThem.SelectedIndex = 0;
            this.RbtnCo.Checked = true;
            this.TxtLyDoThem.Text = "";

            if (this.CkbAddAfterSave.Checked)
            {
                this.MPEAdd.Show();
            }

            NgayRequiredAdd.IsValid = true;
            NgayValidatorAdd.IsValid = true;
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            HocSinh_ThongTinCaNhan student = null; 
            HocSinh_NgayNghiHoc absent = null;
            CauHinh_HocKy term = null;
            CauHinh_Buoi session = null;
            DateTime date;
            ModalPopupExtender modalPopupEdit = new ModalPopupExtender();
            foreach (RepeaterItem rptItem in RptNgayNghi.Items)
            {
                if (rptItem.ItemType == ListItemType.Item || rptItem.ItemType == ListItemType.AlternatingItem)
                {
                    modalPopupEdit = (ModalPopupExtender)rptItem.FindControl("MPEEdit");
                    if (modalPopupEdit.ClientID == HdfRptNgayNghiMPEEdit.Value)
                    {
                        break;
                    }
                }
            }

            string strDate = TxtNgaySua.Text.Trim();
            absent = new HocSinh_NgayNghiHoc();
            absent.MaNgayNghiHoc = Int32.Parse(this.HdfMaNgayNghiHoc.Value);
            student = new HocSinh_ThongTinCaNhan();
            student.MaHocSinh = Int32.Parse(this.HdfMaHocSinh.Value);
            term = new CauHinh_HocKy();
            term.MaHocKy = Int32.Parse(this.DdlHocKySua.SelectedValue);
            if (DdlBuoiSua.SelectedIndex > 0)
            {
                session = new CauHinh_Buoi();
                session.MaBuoi = Int32.Parse(this.DdlBuoiSua.SelectedValue);
            }

            if (strDate == "")
            {
                NgayRequiredEdit.IsValid = false;
                modalPopupEdit.Show();
                return;
            }
            else
            {
                if (!Regex.IsMatch(strDate, NgayExpressionEdit.ValidationExpression))
                {
                    NgayExpressionEdit.IsValid = false;
                    modalPopupEdit.Show();
                    return;
                }

                try
                {
                    DateTime.Parse(strDate);
                }
                catch (Exception ex)
                {
                    DateTimeValidatorEdit.IsValid = false;
                    modalPopupEdit.Show();
                    return;
                }

                if (ngayNghiHocBL.AbsentExists(absent, student, term, DateTime.Parse(this.TxtNgaySua.Text.Trim()), session))
                {
                    NgayValidatorEdit.IsValid = false;
                    modalPopupEdit.Show();
                    return;
                }
            }

            date = DateTime.Parse(this.TxtNgaySua.Text);
            bool xinPhep = this.RbtnCoSua.Checked;
            string lyDo = this.TxtLyDoSua.Text;

            ngayNghiHocBL.UpdateAbsent(absent, term, date, session, xinPhep, lyDo);

            MainDataPager.CurrentIndex = 1;
            BindRepeater();

            this.DdlHocKySua.SelectedIndex = 0;
            this.TxtNgaySua.Text = DateTime.Now.ToShortDateString();
            this.DdlBuoiSua.SelectedIndex = 0;
            this.RbtnCoSua.Checked = true;
            this.TxtLyDoSua.Text = "";
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            int maNgayNghiHoc = Int32.Parse(this.HdfMaNgayNghiHoc.Value);
            ngayNghiHocBL.DeleteAbsent(maNgayNghiHoc);
            isSearch = false;
            BindRepeater();
        }

        protected void BtnBackPrevPage_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("danhsachhocsinh.aspx");
        }
        #endregion

        #region DataPager event handlers
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            MainDataPager.CurrentIndex = currentPageIndex;
            BindRepeater();
        }
        #endregion
    }
}