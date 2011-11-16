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
    public partial class TinhHinhNghiHoc_User : BaseContentPage
    {
        #region Fields
        private StudentBL studentBL;
        private AbsentBL absentBL;
        private SystemConfigBL systemConfigBL;

        private int studentId;
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

            studentBL = new StudentBL(UserSchool);
            if (Session[AppConstant.SCHOOL] != null)
            {
                absentBL = new AbsentBL((School)Session[AppConstant.SCHOOL]);    
            }

            systemConfigBL = new SystemConfigBL(UserSchool);

            //maHocSinh = hocSinhBL.GetMaHocSinh(masterPage.UserNameSession);

            if (!Page.IsPostBack)
            {
                BindDropDownListNamHoc();
                BindDropDownListHocKy();
                InitDates();
                isSearch = false;
                BindRepeater();
            }
        }
        #endregion

        #region Methods
        private void BindDropDownListNamHoc()
        {
            HocSinh_ThongTinCaNhan student = new HocSinh_ThongTinCaNhan();
            student.MaHocSinh = studentId;
            List<CauHinh_NamHoc> lstNamHoc = studentBL.GetYears(student);
            DdlNamHoc.DataSource = lstNamHoc;
            DdlNamHoc.DataValueField = "MaNamHoc";
            DdlNamHoc.DataTextField = "TenNamHoc";
            DdlNamHoc.DataBind();
        }

        private void BindDropDownListHocKy()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<CauHinh_HocKy> lstHocKy = systemConfigBL.GetListTerms();
            DdlHocKy.DataSource = lstHocKy;
            DdlHocKy.DataValueField = "MaHocKy";
            DdlHocKy.DataTextField = "TenHocKy";
            DdlHocKy.DataBind();
            DdlHocKy.SelectedValue = (new SystemConfigBL(UserSchool)).GetCurrentTerm().ToString();
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
        }

        private void BindRepeater()
        {
            HocSinh_ThongTinCaNhan student = null;
            CauHinh_NamHoc year = null;
            CauHinh_HocKy term = null;
            double totalRecords;
            List<TabularAbsent> tabularAbsents;

            student = new HocSinh_ThongTinCaNhan();
            student.MaHocSinh = studentId;
            year = new CauHinh_NamHoc();
            year.MaNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);
            term = new CauHinh_HocKy();
            term.MaHocKy = Int32.Parse(DdlHocKy.SelectedValue);
            DateTime dtBeginDate = DateTime.Parse(TxtTuNgay.Text);
            DateTime dtEndDate = DateTime.Parse(TxtDenNgay.Text);
            
            tabularAbsents = absentBL.GetTabularAbsents(student, year, term, dtBeginDate, dtEndDate,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out totalRecords);

            if(totalRecords != 0 && tabularAbsents.Count == 0)
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
            PnlPopupConfirm.Visible = bDisplayData;
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
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Control control = e.Item.FindControl("HdfRptMaNgayNghiHoc");
                if (control != null)
                {
                    absent = new HocSinh_NgayNghiHoc();
                    absent.MaNgayNghiHoc = Int32.Parse(((HiddenField)control).Value);
                    if (absentBL.Confirmed(absent))
                    {
                        ImageButton btnConfirmItem = (ImageButton)e.Item.FindControl("BtnConfirmItem");
                        btnConfirmItem.ImageUrl = "~/Styles/Images/button_confirm_disable.png";
                        btnConfirmItem.Enabled = false;
                    }
                }
            }
        }

        protected void RptNgayNghi_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdConfirmItem":
                    {
                        int maNgayNghiHoc = Int32.Parse(e.CommandArgument.ToString());
                        HocSinh_NgayNghiHoc ngayNghi = absentBL.GetAbsent(maNgayNghiHoc);
                        LblConfirm.Text = "Bạn có chắc xác nhận ngày nghỉ <b>" + ngayNghi.Ngay.ToShortDateString() + "</b> này không?";

                        ModalPopupExtender mPEConfirm = (ModalPopupExtender)e.Item.FindControl("MPEConfirm");
                        mPEConfirm.Show();

                        this.HdfMaNgayNghiHoc.Value = e.CommandArgument.ToString();
                        this.HdfRptNgayNghiMPEConfirm.Value = mPEConfirm.ClientID;

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

        protected void BtnConfirm_Click(object sender, ImageClickEventArgs e)
        {
            HocSinh_NgayNghiHoc absent = new HocSinh_NgayNghiHoc();
            absent.MaNgayNghiHoc = Int32.Parse(this.HdfMaNgayNghiHoc.Value);

            absentBL.ConfirmAbsent(absent);

            MainDataPager.CurrentIndex = 1;
            BindRepeater();
        }
        #endregion

        #region Pager event handlers
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            MainDataPager.CurrentIndex = currentPageIndex;
            BindRepeater();
        }
        #endregion
    }
}