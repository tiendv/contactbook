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
                    Student_Student student = (Student_Student)GetSession(AppConstant.SESSION_STUDENT);
                    RemoveSession(AppConstant.SESSION_STUDENT);
                    ViewState[AppConstant.VIEWSTATE_STUDENTID] = student.StudentId;

                    Class_Class studentClass = (Class_Class)GetSession(AppConstant.SESSION_STUDENTCLASS);
                    RemoveSession(AppConstant.SESSION_STUDENTCLASS);
                    ViewState[AppConstant.VIEWSTATE_STUDENTCLASS_ID] = studentClass.ClassId;

                    Configuration_Year year = (Configuration_Year)GetSession(AppConstant.SESSION_SELECTED_YEAR);
                    RemoveSession(AppConstant.SESSION_SELECTED_YEAR);
                    ViewState[AppConstant.VIEWSTATE_STUDENTID] = student.StudentId;

                    Category_Faculty faculty = (Category_Faculty)GetSession(AppConstant.SESSION_SELECTED_FACULTY);
                    RemoveSession(AppConstant.SESSION_SELECTED_FACULTY);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTY] = faculty.FacultyId;

                    Category_Grade grade = (Category_Grade)GetSession(AppConstant.SESSION_SELECTED_GRADE);
                    RemoveSession(AppConstant.SESSION_SELECTED_GRADE);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_GRADE] = grade.GradeId;

                    Class_Class Class = (Class_Class)GetSession(AppConstant.SESSION_SELECTED_CLASS);
                    RemoveSession(AppConstant.SESSION_SELECTED_CLASS);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS] = Class.ClassId;

                    String strStudentName = (string)GetSession(AppConstant.SESSION_SELECTED_STUDENTNAME);
                    RemoveSession(AppConstant.SESSION_SELECTED_STUDENTNAME);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTNAME] = strStudentName;

                    String strStudentCode = (string)GetSession(AppConstant.SESSION_SELECTED_STUDENTCODE);
                    RemoveSession(AppConstant.SESSION_SELECTED_STUDENTCODE);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTCODE] = strStudentCode;

                    ViewState[AppConstant.VIEWSTATE_STUDENTID] = student.StudentId;

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
                Student_Student student = new Student_Student();
                student.StudentId = (int)ViewState[AppConstant.VIEWSTATE_STUDENTID];

                List<Configuration_Year> years = studentBL.GetYears(student);
                DdlNamHoc.DataSource = years;
                DdlNamHoc.DataValueField = "YearId";
                DdlNamHoc.DataTextField = "YearName";
                DdlNamHoc.DataBind();
            }
        }

        private void BindDDLTerms()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<Configuration_Term> terms = systemConfigBL.GetListTerms();
            DdlHocKy.DataSource = terms;
            DdlHocKy.DataValueField = "TermId";
            DdlHocKy.DataTextField = "TermName";
            DdlHocKy.DataBind();
            DdlHocKy.SelectedValue = (new SystemConfigBL(UserSchool)).GetCurrentTerm().ToString();

            DdlHocKyThem.DataSource = terms;
            DdlHocKyThem.DataValueField = "TermId";
            DdlHocKyThem.DataTextField = "TermName";
            DdlHocKyThem.DataBind();
            DdlHocKyThem.SelectedValue = (new SystemConfigBL(UserSchool)).GetCurrentTerm().ToString();
        }

        private void BindDDLAttitudes()
        {
            List<Category_Attitude> attitudes = attitudeBL.GetListAttitudes();
            DdlThaiDoThamGiaThem.DataSource = attitudes;
            DdlThaiDoThamGiaThem.DataValueField = "AttitudeId";
            DdlThaiDoThamGiaThem.DataTextField = "AttitudeName";
            DdlThaiDoThamGiaThem.DataBind();
            DdlThaiDoThamGiaThem.Items.Insert(0, new ListItem("Chưa xác định", "0"));

            DdlThaiDoThamGiaSua.DataSource = attitudes;
            DdlThaiDoThamGiaSua.DataValueField = "AttitudeId";
            DdlThaiDoThamGiaSua.DataTextField = "AttitudeName";
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
            Student_Student student = null;
            Configuration_Year year = null;
            Configuration_Term term = null;
            DateTime dtBeginDate;
            DateTime dtEndDate;
            double dTotalRecords;
            List<TabularStudentActivity> tabularStudentActivities;

            student = new Student_Student();
            student.StudentId = (int)ViewState[AppConstant.VIEWSTATE_STUDENTID];
            if (DdlNamHoc.SelectedIndex >= 0)
            {
                year = new Configuration_Year();
                year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);
            }
            if (DdlHocKy.SelectedIndex >= 0)
            {
                term = new Configuration_Term();
                term.TermId = Int32.Parse(DdlHocKy.SelectedValue);
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
            Student_Student student = null;
            Category_Attitude attitude = null;
            Configuration_Term term = null;
            DateTime date;
            Configuration_Year year = null;

            student = new Student_Student();
            student.StudentId = (int)ViewState[AppConstant.VIEWSTATE_STUDENTID];
            year = systemConfigBL.GetCurrentYear();
            term = new Configuration_Term();
            term.TermId = Int32.Parse(this.DdlHocKyThem.SelectedValue);
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

            string strContent = this.TxtDescriptionThem.Text;
            date = DateTime.Parse(this.TxtNgayThem.Text);
            if (this.DdlThaiDoThamGiaThem.SelectedIndex > 0)
            {
                attitude = new Category_Attitude();
                attitude.AttitudeId = Int32.Parse(this.DdlThaiDoThamGiaThem.SelectedValue);
            }

            studentActivityBL.InsertStudentActivity(student, term, date, tieuDe, strContent, attitude);

            MainDataPager.CurrentIndex = 1;
            BindRptStudentActivities();

            this.DdlHocKyThem.SelectedIndex = 0;
            this.TxtNgayThem.Text = DateTime.Now.ToShortDateString();
            this.TxtTieuDeThem.Text = "";
            this.TxtDescriptionThem.Text = "";
            this.DdlThaiDoThamGiaThem.SelectedIndex = 0;

            if (this.CkbAddAfterSave.Checked)
            {
                this.MPEAdd.Show();
            }
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            Student_Activity studentActivity = null;
            Category_Attitude attitude = null;
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
                    //    (int)ViewState["TermId"], DateTime.Parse(strNgay)))
                    //{
                    //    NgayValidatorEdit.IsValid = false;
                    //    modalPopupEdit.Show();
                    //    return;
                    //}
                }
            }

            studentActivity = new Student_Activity();
            studentActivity.ActivityId = iStudentActivityId;
            DateTime date = DateTime.Parse(this.TxtNgaySua.Text);
            string strContent = this.TxtDescriptionSua.Text;
            if (this.DdlThaiDoThamGiaSua.SelectedIndex > 0)
            {
                attitude = new Category_Attitude();
                attitude.AttitudeId = Int32.Parse(this.DdlThaiDoThamGiaSua.SelectedValue);
            }

            studentActivityBL.UpdateStudentActivity(studentActivity, date, strContent, attitude);

            BindRptStudentActivities();
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            int maHoatDong = Int32.Parse(this.HdfMaHoatDong.Value);
            Student_Activity studentActivity = new Student_Activity();
            studentActivity.ActivityId = maHoatDong;
            studentActivityBL.DeleteStudentActivity(studentActivity);

            isSearch = false;
            BindRptStudentActivities();
        }

        protected void BtnBackPrevPage_Click(object sender, ImageClickEventArgs e)
        {
            Configuration_Year year = new Configuration_Year();
            year.YearId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_STUDENTID].ToString());
            AddSession(AppConstant.SESSION_SELECTED_YEAR, year);

            Category_Faculty faculty = new Category_Faculty();
            faculty.FacultyId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTY].ToString());
            AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);

            Category_Grade grade = new Category_Grade();
            grade.GradeId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_GRADE].ToString());
            AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);

            Class_Class Class = new Class_Class();
            Class.ClassId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS].ToString());
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
                        Student_Activity hoatDong = studentActivityBL.GetStudentActivity(maHoatDong);
                        this.HdfSltActivityName.Value = hoatDong.Title;
                        this.LblTieuDeSua.Text = hoatDong.Title;
                        this.HdfTieuDe.Value = hoatDong.Title;
                        this.TxtDescriptionSua.Text = hoatDong.ActivityContent;
                        ViewState["TermId"] = hoatDong.TermId;
                        this.HdfTermId.Value = hoatDong.TermId.ToString();
                        this.LblHocKySua.Text = hoatDong.Configuration_Term.TermName;
                        this.TxtNgaySua.Text = hoatDong.Date.ToShortDateString();
                        if (hoatDong.AttitudeId == null)
                        {
                            this.DdlThaiDoThamGiaSua.SelectedValue = "0";
                        }
                        else
                        {
                            this.DdlThaiDoThamGiaSua.SelectedValue = hoatDong.AttitudeId.ToString();
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
                        Student_Student student = new Student_Student();
                        student.StudentId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_STUDENTID].ToString());
                        AddSession(AppConstant.SESSION_STUDENT, student);

                        Configuration_Year year = new Configuration_Year();
                        year.YearId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_STUDENTID].ToString());
                        AddSession(AppConstant.SESSION_SELECTED_YEAR, year);

                        Category_Faculty faculty = new Category_Faculty();
                        faculty.FacultyId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTY].ToString());
                        AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);

                        Category_Grade grade = new Category_Grade();
                        grade.GradeId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_GRADE].ToString());
                        AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);

                        Class_Class Class = new Class_Class();
                        Class.ClassId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS].ToString());
                        AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);

                        String strStudentName = ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTNAME].ToString();
                        AddSession(AppConstant.SESSION_SELECTED_STUDENTNAME, strStudentName);

                        String strStudentCode = ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTCODE].ToString();
                        AddSession(AppConstant.SESSION_SELECTED_STUDENTCODE, strStudentCode);

                        Class_Class studentClass = new Class_Class();
                        studentClass.ClassId = (int)ViewState[AppConstant.VIEWSTATE_STUDENTCLASS_ID];
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