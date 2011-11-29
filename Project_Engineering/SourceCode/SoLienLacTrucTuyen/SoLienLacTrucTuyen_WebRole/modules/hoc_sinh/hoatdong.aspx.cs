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
    public partial class StudentActivityPage : BaseContentPage
    {
        #region Fields
        private StudentBL studentBL;
        private SystemConfigBL systemConfigBL;
        private StudentActivityBL studentActivityBL;
        private AttitudeBL attitudeBL;
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
            systemConfigBL = new SystemConfigBL(UserSchool);
            studentActivityBL = new StudentActivityBL(UserSchool);
            attitudeBL = new AttitudeBL(UserSchool);

            if (!Page.IsPostBack)
            {
                if (CheckSessionKey(AppConstant.SESSION_STUDENT))
                {
                    HocSinh_ThongTinCaNhan student = (HocSinh_ThongTinCaNhan)GetSession(AppConstant.SESSION_STUDENT);
                    RemoveSession(AppConstant.SESSION_STUDENT);
                    ViewState[AppConstant.VIEWSTATE_STUDENTID] = student.MaHocSinh;

                    LopHoc_Lop studentClass = (LopHoc_Lop)GetSession(AppConstant.SESSION_STUDENTCLASS);
                    RemoveSession(AppConstant.SESSION_STUDENTCLASS);
                    ViewState[AppConstant.VIEWSTATE_STUDENTCLASS_ID] = studentClass.MaLopHoc;

                    CauHinh_NamHoc year = (CauHinh_NamHoc)GetSession(AppConstant.SESSION_SELECTED_YEAR);
                    RemoveSession(AppConstant.SESSION_SELECTED_YEAR);
                    ViewState[AppConstant.VIEWSTATE_STUDENTID] = student.MaHocSinh;

                    DanhMuc_NganhHoc faculty = (DanhMuc_NganhHoc)GetSession(AppConstant.SESSION_SELECTED_FACULTY);
                    RemoveSession(AppConstant.SESSION_SELECTED_FACULTY);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTY] = faculty.MaNganhHoc;

                    DanhMuc_KhoiLop grade = (DanhMuc_KhoiLop)GetSession(AppConstant.SESSION_SELECTED_GRADE);
                    RemoveSession(AppConstant.SESSION_SELECTED_GRADE);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_GRADE] = grade.MaKhoiLop;

                    LopHoc_Lop Class = (LopHoc_Lop)GetSession(AppConstant.SESSION_SELECTED_CLASS);
                    RemoveSession(AppConstant.SESSION_SELECTED_CLASS);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS] = Class.MaLopHoc;

                    String strStudentName = (string)GetSession(AppConstant.SESSION_SELECTED_STUDENTNAME);
                    RemoveSession(AppConstant.SESSION_SELECTED_STUDENTNAME);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTNAME] = strStudentName;

                    String strStudentCode = (string)GetSession(AppConstant.SESSION_SELECTED_STUDENTCODE);
                    RemoveSession(AppConstant.SESSION_SELECTED_STUDENTCODE);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTCODE] = strStudentCode;

                    ViewState[AppConstant.VIEWSTATE_STUDENTID] = student.MaHocSinh;

                    BindDropDownLists();
                    InitDates();
                    isSearch = false;
                    BindRptStudentActivities();

                    AuthorizationBL authorizationBL = new AuthorizationBL(UserSchool);
                    List<UserManagement_PagePath> pagePages = authorizationBL.GetStudentPages(
                        (new UserBL()).GetRoles(User.Identity.Name));
                    RptStudentFunctions.DataSource = pagePages;
                    RptStudentFunctions.DataBind();
                }
                else
                {
                    Response.Redirect(AppConstant.PAGEPATH_STUDENTS);
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
            BindDDLYears();
            BindDDLTerms();
            BindDDLAttitudes();
        }

        private void BindDDLYears()
        {
            if (ViewState[AppConstant.VIEWSTATE_STUDENTID] != null)
            {
                HocSinh_ThongTinCaNhan student = new HocSinh_ThongTinCaNhan();
                student.MaHocSinh = (int)ViewState[AppConstant.VIEWSTATE_STUDENTID];

                List<CauHinh_NamHoc> years = studentBL.GetYears(student);
                DdlNamHoc.DataSource = years;
                DdlNamHoc.DataValueField = "MaNamHoc";
                DdlNamHoc.DataTextField = "TenNamHoc";
                DdlNamHoc.DataBind();
            }
        }

        private void BindDDLTerms()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<CauHinh_HocKy> terms = systemConfigBL.GetListTerms();
            DdlHocKy.DataSource = terms;
            DdlHocKy.DataValueField = "MaHocKy";
            DdlHocKy.DataTextField = "TenHocKy";
            DdlHocKy.DataBind();
            DdlHocKy.SelectedValue = (new SystemConfigBL(UserSchool)).GetCurrentTerm().ToString();

            DdlHocKyThem.DataSource = terms;
            DdlHocKyThem.DataValueField = "MaHocKy";
            DdlHocKyThem.DataTextField = "TenHocKy";
            DdlHocKyThem.DataBind();
            DdlHocKyThem.SelectedValue = (new SystemConfigBL(UserSchool)).GetCurrentTerm().ToString();
        }

        private void BindDDLAttitudes()
        {
            List<DanhMuc_ThaiDoThamGia> attitudes = attitudeBL.GetListAttitudes();
            DdlThaiDoThamGiaThem.DataSource = attitudes;
            DdlThaiDoThamGiaThem.DataValueField = "MaThaiDoThamGia";
            DdlThaiDoThamGiaThem.DataTextField = "TenThaiDoThamGia";
            DdlThaiDoThamGiaThem.DataBind();
            DdlThaiDoThamGiaThem.Items.Insert(0, new ListItem("Chưa xác định", "0"));

            DdlThaiDoThamGiaSua.DataSource = attitudes;
            DdlThaiDoThamGiaSua.DataValueField = "MaThaiDoThamGia";
            DdlThaiDoThamGiaSua.DataTextField = "TenThaiDoThamGia";
            DdlThaiDoThamGiaSua.DataBind();
            DdlThaiDoThamGiaSua.Items.Insert(0, new ListItem("Chưa xác định", "0"));
        }

        private void InitDates()
        {
            DateTime dtToday = DateTime.Now;
            DateTime dtBeginDateOfMonth = new DateTime(dtToday.Year, dtToday.Month, 1);
            DateTime dtDateOfNextMonth = dtToday.AddMonths(1);
            DateTime dtBeginDateOfNextMonth = new DateTime(dtDateOfNextMonth.Year, dtDateOfNextMonth.Month, 1);
            DateTime dtEndDateOfMonth = dtBeginDateOfNextMonth.AddDays(-1);

            TxtTuNgay.Text = dtBeginDateOfMonth.ToShortDateString();
            TxtDenNgay.Text = dtEndDateOfMonth.ToShortDateString();
            TxtNgayThem.Text = dtToday.ToShortDateString();
            TxtNgaySua.Text = dtToday.ToShortDateString();
        }

        private void BindRptStudentActivities()
        {
            HocSinh_ThongTinCaNhan student = null;
            CauHinh_NamHoc year = null;
            CauHinh_HocKy term = null;
            DateTime dtBeginDate;
            DateTime dtEndDate;
            double dTotalRecords;
            List<TabularStudentActivity> tabularStudentActivities;

            student = new HocSinh_ThongTinCaNhan();
            student.MaHocSinh = (int)ViewState[AppConstant.VIEWSTATE_STUDENTID];
            if (DdlNamHoc.SelectedIndex >= 0)
            {
                year = new CauHinh_NamHoc();
                year.MaNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);
            }
            if (DdlHocKy.SelectedIndex >= 0)
            {
                term = new CauHinh_HocKy();
                term.MaHocKy = Int32.Parse(DdlHocKy.SelectedValue);
            }
            dtBeginDate = DateTime.Parse(TxtTuNgay.Text);
            dtEndDate = DateTime.Parse(TxtDenNgay.Text);

            tabularStudentActivities = studentActivityBL.GetTabularStudentActivities(
                student, year, term, dtBeginDate, dtEndDate,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            if (dTotalRecords != 0 && tabularStudentActivities.Count == 0)
            {
                MainDataPager.CurrentIndex--;
                BindRptStudentActivities();
                return;
            }

            bool bDisplayData = (tabularStudentActivities.Count != 0) ? true : false;
            ProcessDislayInfo(bDisplayData);

            RptHoatDong.DataSource = tabularStudentActivities;
            RptHoatDong.DataBind();
            MainDataPager.ItemCount = dTotalRecords;
        }

        private void ProcessDislayInfo(bool bDisplayData)
        {
            PnlPopupConfirmDelete.Visible = bDisplayData;
            PnlPopupEdit.Visible = bDisplayData;
            RptHoatDong.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin hoạt động";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy hoạt động";
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

        #region DataPager event handlers
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            MainDataPager.CurrentIndex = currentPageIndex;
            BindRptStudentActivities();
        }
        #endregion

        #region Button event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            isSearch = true;
            BindRptStudentActivities();
        }

        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {
            HocSinh_ThongTinCaNhan student = null;
            DanhMuc_ThaiDoThamGia attitude = null;
            CauHinh_HocKy term = null;
            DateTime date;
            CauHinh_NamHoc year = null;

            student = new HocSinh_ThongTinCaNhan();
            student.MaHocSinh = (int)ViewState[AppConstant.VIEWSTATE_STUDENTID];
            year = systemConfigBL.GetCurrentYear();
            term = new CauHinh_HocKy();
            term.MaHocKy = Int32.Parse(this.DdlHocKyThem.SelectedValue);
            string tieuDe = this.TxtTieuDeThem.Text.Trim();
            string strNgay = this.TxtNgayThem.Text.Trim();

            if (tieuDe == "")
            {
                TieuDeRequiredAdd.IsValid = false;
                MPEAdd.Show();
                return;
            }
            else
            {
                if (strNgay == "")
                {
                    NgayRequiredAdd.IsValid = false;
                    MPEAdd.Show();
                    return;
                }
                else
                {
                    if (!Regex.IsMatch(strNgay, NgayExpression.ValidationExpression))
                    {
                        NgayExpression.IsValid = false;
                        MPEAdd.Show();
                        return;
                    }
                    else
                    {
                        try
                        {
                            DateTime.Parse(strNgay);
                        }
                        catch (Exception ex)
                        {
                            DateTimeValidatorAdd.IsValid = false;
                            MPEAdd.Show();
                            return;
                        }

                        if (studentActivityBL.StudentActivityNamExists(tieuDe, student, year, term, DateTime.Parse(strNgay)))
                        {
                            TieuDeValidatorAdd.IsValid = false;
                            MPEAdd.Show();
                            return;
                        }
                    }
                }
            }

            string strContent = this.TxtMoTaThem.Text;
            date = DateTime.Parse(this.TxtNgayThem.Text);
            if (this.DdlThaiDoThamGiaThem.SelectedIndex > 0)
            {
                attitude = new DanhMuc_ThaiDoThamGia();
                attitude.MaThaiDoThamGia = Int32.Parse(this.DdlThaiDoThamGiaThem.SelectedValue);
            }

            studentActivityBL.InsertStudentActivity(student, term, date, tieuDe, strContent, attitude);

            MainDataPager.CurrentIndex = 1;
            BindRptStudentActivities();

            this.DdlHocKyThem.SelectedIndex = 0;
            this.TxtNgayThem.Text = DateTime.Now.ToShortDateString();
            this.TxtTieuDeThem.Text = "";
            this.TxtMoTaThem.Text = "";
            this.DdlThaiDoThamGiaThem.SelectedIndex = 0;

            if (this.CkbAddAfterSave.Checked)
            {
                this.MPEAdd.Show();
            }
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            HocSinh_HoatDong studentActivity = null;
            DanhMuc_ThaiDoThamGia attitude = null;
            ModalPopupExtender modalPopupEdit = new ModalPopupExtender();

            foreach (RepeaterItem rptItem in RptHoatDong.Items)
            {
                if (rptItem.ItemType == ListItemType.Item || rptItem.ItemType == ListItemType.AlternatingItem)
                {
                    modalPopupEdit = (ModalPopupExtender)rptItem.FindControl("MPEEdit");
                    if (modalPopupEdit.ClientID == HdfRptHoatDongMPEEdit.Value)
                    {
                        break;
                    }
                }
            }

            string strOldTitle = this.HdfSltActivityName.Value;
            int iStudentActivityId = Int32.Parse(this.HdfMaHoatDong.Value);
            string strDate = TxtNgaySua.Text.Trim();
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
                else
                {
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

                    //if (hoatDongBL.StudentActivityNamExists(iStudentActivityId, LblTieuDeSua.Text, (int)ViewState["MaHocSinh"],
                    //    (int)ViewState["MaHocKy"], DateTime.Parse(strNgay)))
                    //{
                    //    NgayValidatorEdit.IsValid = false;
                    //    modalPopupEdit.Show();
                    //    return;
                    //}
                }
            }

            studentActivity = new HocSinh_HoatDong();
            studentActivity.MaHoatDong = iStudentActivityId;
            DateTime date = DateTime.Parse(this.TxtNgaySua.Text);
            string strContent = this.TxtMoTaSua.Text;
            if (this.DdlThaiDoThamGiaSua.SelectedIndex > 0)
            {
                attitude = new DanhMuc_ThaiDoThamGia();
                attitude.MaThaiDoThamGia = Int32.Parse(this.DdlThaiDoThamGiaSua.SelectedValue);
            }

            studentActivityBL.UpdateStudentActivity(studentActivity, date, strContent, attitude);

            BindRptStudentActivities();
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            int maHoatDong = Int32.Parse(this.HdfMaHoatDong.Value);
            HocSinh_HoatDong studentActivity = new HocSinh_HoatDong();
            studentActivity.MaHoatDong = maHoatDong;
            studentActivityBL.DeleteStudentActivity(studentActivity);

            isSearch = false;
            BindRptStudentActivities();
        }

        protected void BtnBackPrevPage_Click(object sender, ImageClickEventArgs e)
        {
            CauHinh_NamHoc year = new CauHinh_NamHoc();
            year.MaNamHoc = Int32.Parse(ViewState[AppConstant.VIEWSTATE_STUDENTID].ToString());
            AddSession(AppConstant.SESSION_SELECTED_YEAR, year);

            DanhMuc_NganhHoc faculty = new DanhMuc_NganhHoc();
            faculty.MaNganhHoc = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTY].ToString());
            AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);

            DanhMuc_KhoiLop grade = new DanhMuc_KhoiLop();
            grade.MaKhoiLop = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_GRADE].ToString());
            AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);

            LopHoc_Lop Class = new LopHoc_Lop();
            Class.MaLopHoc = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS].ToString());
            AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);

            String strStudentName = ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTNAME].ToString();
            AddSession(AppConstant.SESSION_SELECTED_STUDENTNAME, strStudentName);

            String strStudentCode = ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTCODE].ToString();
            AddSession(AppConstant.SESSION_SELECTED_STUDENTCODE, strStudentCode);

            Response.Redirect(AppConstant.PAGEPATH_STUDENTS);
        }
        #endregion

        #region Repeater event handlers
        protected void RptHoatDong_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
                if (e.Item.ItemType == ListItemType.Item
                    || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (e.Item.DataItem != null)
                    {
                        Control control = e.Item.FindControl("HdfRptMaHoatDong");
                        if (control != null)
                        {
                            //int maNgayNghiHoc = Int32.Parse(((HiddenField)control).Value);
                            //if (ngayNghiHocBL.IsXacNhan(maNgayNghiHoc))
                            //{
                            //    ImageButton btnDeleteItem = (ImageButton)e.Item.FindControl("BtnDeleteItem");
                            //    btnDeleteItem.ImageUrl = "~/Styles/Images/button_delete_disable.png";
                            //    btnDeleteItem.Enabled = false;

                            //    ImageButton btnEditItem = (ImageButton)e.Item.FindControl("BtnEditItem");
                            //    btnEditItem.ImageUrl = "~/Styles/Images/button_edit_disable.png";
                            //    btnEditItem.Enabled = false;
                            //}
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

        protected void RptHoatDong_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDeleteItem":
                    {
                        LblConfirmDelete.Text = "Bạn có chắc xóa hoạt động này không?";
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        HiddenField hdfRptMaHoatDong = (HiddenField)e.Item.FindControl("HdfRptMaHoatDong");
                        HdfMaHoatDong.Value = hdfRptMaHoatDong.Value;

                        HdfRptHoatDongMPEDelete.Value = mPEDelete.ClientID;
                        break;
                    }
                case "CmdEditItem":
                    {
                        int maHoatDong = Int32.Parse(e.CommandArgument.ToString());
                        HocSinh_HoatDong hoatDong = studentActivityBL.GetStudentActivity(maHoatDong);
                        this.HdfSltActivityName.Value = hoatDong.TieuDe;
                        this.LblTieuDeSua.Text = hoatDong.TieuDe;
                        this.HdfTieuDe.Value = hoatDong.TieuDe;
                        this.TxtMoTaSua.Text = hoatDong.NoiDung;
                        ViewState["MaHocKy"] = hoatDong.MaHocKy;
                        this.HdfMaHocKy.Value = hoatDong.MaHocKy.ToString();
                        this.LblHocKySua.Text = hoatDong.CauHinh_HocKy.TenHocKy;
                        this.TxtNgaySua.Text = hoatDong.Ngay.ToShortDateString();
                        if (hoatDong.MaThaiDoThamGia == null)
                        {
                            this.DdlThaiDoThamGiaSua.SelectedValue = "0";
                        }
                        else
                        {
                            this.DdlThaiDoThamGiaSua.SelectedValue = hoatDong.MaThaiDoThamGia.ToString();
                        }

                        ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        mPEEdit.Show();

                        this.HdfMaHoatDong.Value = e.CommandArgument.ToString();
                        this.HdfRptHoatDongMPEEdit.Value = mPEEdit.ClientID;

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        protected void RptStudentFunctions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdfPhysicalPath = (HiddenField)e.Item.FindControl("HdfPhysicalPath");
                if (hdfPhysicalPath.Value == Request.Path)
                {
                    LinkButton lkBtnStudentPage = (LinkButton)e.Item.FindControl("LkBtnStudentPage");
                    lkBtnStudentPage.Style.Add(HtmlTextWriterStyle.TextDecoration, "none");
                }
            }
        }

        protected void RptStudentFunctions_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Redirect":
                    {
                        HocSinh_ThongTinCaNhan student = new HocSinh_ThongTinCaNhan();
                        student.MaHocSinh = Int32.Parse(ViewState[AppConstant.VIEWSTATE_STUDENTID].ToString());
                        AddSession(AppConstant.SESSION_STUDENT, student);

                        CauHinh_NamHoc year = new CauHinh_NamHoc();
                        year.MaNamHoc = Int32.Parse(ViewState[AppConstant.VIEWSTATE_STUDENTID].ToString());
                        AddSession(AppConstant.SESSION_SELECTED_YEAR, year);

                        DanhMuc_NganhHoc faculty = new DanhMuc_NganhHoc();
                        faculty.MaNganhHoc = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTY].ToString());
                        AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);

                        DanhMuc_KhoiLop grade = new DanhMuc_KhoiLop();
                        grade.MaKhoiLop = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_GRADE].ToString());
                        AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);

                        LopHoc_Lop Class = new LopHoc_Lop();
                        Class.MaLopHoc = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS].ToString());
                        AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);

                        String strStudentName = ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTNAME].ToString();
                        AddSession(AppConstant.SESSION_SELECTED_STUDENTNAME, strStudentName);

                        String strStudentCode = ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTCODE].ToString();
                        AddSession(AppConstant.SESSION_SELECTED_STUDENTCODE, strStudentCode);

                        LopHoc_Lop studentClass = new LopHoc_Lop();
                        studentClass.MaLopHoc = (int)ViewState[AppConstant.VIEWSTATE_STUDENTCLASS_ID];
                        AddSession(AppConstant.SESSION_STUDENTCLASS, studentClass);

                        Response.Redirect((string)e.CommandArgument);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        #endregion
    }
}