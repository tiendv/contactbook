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
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Web.Security;

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
            if (accessDenied)
            {
                return;
            }

            if (sessionExpired)
            {
                FormsAuthentication.SignOut();
                Response.Redirect(FormsAuthentication.LoginUrl);
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
                    ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTYID] = faculty.FacultyId;

                    Category_Grade grade = (Category_Grade)GetSession(AppConstant.SESSION_SELECTED_GRADE);
                    RemoveSession(AppConstant.SESSION_SELECTED_GRADE);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_GRADEID] = grade.GradeId;

                    Class_Class Class = (Class_Class)GetSession(AppConstant.SESSION_SELECTED_CLASS);
                    RemoveSession(AppConstant.SESSION_SELECTED_CLASS);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID] = Class.ClassId;

                    String strStudentName = (string)GetSession(AppConstant.SESSION_SELECTED_STUDENTNAME);
                    RemoveSession(AppConstant.SESSION_SELECTED_STUDENTNAME);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTNAME] = strStudentName;

                    String strStudentCode = (string)GetSession(AppConstant.SESSION_SELECTED_STUDENTCODE);
                    RemoveSession(AppConstant.SESSION_SELECTED_STUDENTCODE);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTCODE] = strStudentCode;

                    ViewState[AppConstant.VIEWSTATE_STUDENTID] = student.StudentId;

                    LblStudentName.Text = student.FullName;
                    LblStudentCode.Text = student.StudentCode;
                    BindDropDownLists();
                    InitDates();
                    isSearch = false;
                    BindRptStudentActivities();

                    AuthorizationBL authorizationBL = new AuthorizationBL(UserSchool);
                    List<UserManagement_PagePath> pagePages = authorizationBL.GetStudentPages(
                         (new UserBL(UserSchool)).GetRoles(User.Identity.Name));
                    RptStudentFunctions.DataSource = pagePages;
                    RptStudentFunctions.DataBind();
                }
                else
                {
                    Response.Redirect(AppConstant.PAGEPATH_STUDENT_LIST);
                }
            }

            ProcPermissions();
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
            DateTime today = DateTime.Now;
            TxtTuNgay.Text = today.AddMonths(-1).ToString(AppConstant.DATEFORMAT_DDMMYYYY);
            TxtDenNgay.Text = today.AddMonths(1).ToString(AppConstant.DATEFORMAT_DDMMYYYY);

            // dont remove this code
            //DateTime today = DateTime.Now;
            //DateTime beginDateOfMonth = new DateTime(today.Year, today.Month, 1);
            //TxtTuNgay.Text = beginDateOfMonth.ToShortDateString();
            //DateTime dateOfNextMonth = today.AddMonths(1);
            //DateTime beginDateOfNextMonth = new DateTime(dateOfNextMonth.Year, dateOfNextMonth.Month, 1);
            //DateTime endDateOfMonth = beginDateOfNextMonth.AddDays(-1);
            //TxtDenNgay.Text = endDateOfMonth.ToShortDateString();
        }

        private void BindRptStudentActivities()
        {
            Student_Student student = null;
            Configuration_Year year = null;
            Configuration_Term term = null;
            DateTime? dtBeginDate;
            DateTime? dtEndDate;
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
            dtBeginDate = DateUtils.StringToDateVN(TxtTuNgay.Text);
            dtEndDate = DateUtils.StringToDateVN(TxtDenNgay.Text);
            if (dtBeginDate == null)
            {
                BeginDateCustom.IsValid = false;
                return;
            }

            if (dtEndDate == null)
            {
                EndDateValidator.IsValid = false;
                return;
            }

            tabularStudentActivities = studentActivityBL.GetTabularStudentActivities(
                student, year, term, (DateTime)dtBeginDate, (DateTime)dtEndDate,
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
            DateTime? date;
            Configuration_Year year = null;

            student = new Student_Student();
            student.StudentId = (int)ViewState[AppConstant.VIEWSTATE_STUDENTID];
            year = systemConfigBL.GetLastedYear();
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
                    date = DateUtils.StringToDateVN(strNgay);
                    if (date == null)
                    {
                        DateTimeValidatorAdd.IsValid = false;
                        MPEAdd.Show();
                        return;
                    }

                    if (studentActivityBL.StudentActivityNamExists(tieuDe, student, year, term, (DateTime)date))
                    {
                        TieuDeValidatorAdd.IsValid = false;
                        MPEAdd.Show();
                        return;
                    }

                }
            }

            string strContent = this.TxtDescriptionThem.Text;
            if (this.DdlThaiDoThamGiaThem.SelectedIndex > 0)
            {
                attitude = new Category_Attitude();
                attitude.AttitudeId = Int32.Parse(this.DdlThaiDoThamGiaThem.SelectedValue);
            }

            studentActivityBL.InsertStudentActivity(student, term, (DateTime)date, tieuDe, strContent, attitude);

            MainDataPager.CurrentIndex = 1;
            BindRptStudentActivities();

            this.DdlHocKyThem.SelectedIndex = 0;
            this.TxtNgayThem.Text = DateTime.Now.ToString(AppConstant.DATEFORMAT_DDMMYYYY);
            this.TxtTieuDeThem.Text = "";
            this.TxtDescriptionThem.Text = "";
            this.DdlThaiDoThamGiaThem.SelectedIndex = 0;

            if (this.CkbAddAfterSave.Checked)
            {
                this.MPEAdd.Show();
            }
        }

        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            HiddenField HdfRptMaHoatDong = null;
            foreach (RepeaterItem item in RptHoatDong.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox CkbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (CkbxSelect.Checked)
                    {
                        HdfRptMaHoatDong = (HiddenField)item.FindControl("HdfRptMaHoatDong");
                        Student_Activity hoatDong = studentActivityBL.GetStudentActivity(Int32.Parse(HdfRptMaHoatDong.Value));
                        this.HdfSltActivityName.Value = hoatDong.Title;
                        this.LblTieuDeSua.Text = hoatDong.Title;
                        this.HdfTieuDe.Value = hoatDong.Title;
                        this.TxtDescriptionSua.Text = hoatDong.ActivityContent;
                        ViewState["TermId"] = hoatDong.TermId;
                        this.HdfTermId.Value = hoatDong.TermId.ToString();
                        this.LblHocKySua.Text = hoatDong.Configuration_Term.TermName;
                        this.TxtNgaySua.Text = hoatDong.Date.ToString(AppConstant.DATEFORMAT_DDMMYYYY);
                        if (hoatDong.AttitudeId == null)
                        {
                            this.DdlThaiDoThamGiaSua.SelectedValue = "0";
                        }
                        else
                        {
                            this.DdlThaiDoThamGiaSua.SelectedValue = hoatDong.AttitudeId.ToString();
                        }

                        this.HdfMaHoatDong.Value = HdfRptMaHoatDong.Value;

                        MPEEdit.Show();
                        return;
                    }
                }
            }
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            Student_Activity studentActivity = null;
            Category_Attitude attitude = null;
            DateTime? date = null;
            string strOldTitle = this.HdfSltActivityName.Value;
            int iStudentActivityId = Int32.Parse(this.HdfMaHoatDong.Value);
            string strDate = TxtNgaySua.Text.Trim();
            if (strDate == "")
            {
                NgayRequiredEdit.IsValid = false;
                MPEEdit.Show();
                return;
            }
            else
            {
                date = DateUtils.StringToDateVN(strDate);
                if (date == null)
                {
                    DateTimeValidatorEdit.IsValid = false;
                    MPEEdit.Show();
                    return;

                }
            }

            studentActivity = new Student_Activity();
            studentActivity.ActivityId = iStudentActivityId;
            string strContent = this.TxtDescriptionSua.Text;
            if (this.DdlThaiDoThamGiaSua.SelectedIndex > 0)
            {
                attitude = new Category_Attitude();
                attitude.AttitudeId = Int32.Parse(this.DdlThaiDoThamGiaSua.SelectedValue);
            }

            studentActivityBL.UpdateStudentActivity(studentActivity, (DateTime)date, strContent, attitude);

            BindRptStudentActivities();
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            bool bInfoInUse = false;
            CheckBox ckbxSelect = null;
            HiddenField HdfRptMaHoatDong = null;

            foreach (RepeaterItem item in RptHoatDong.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    ckbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (ckbxSelect.Checked)
                    {
                        HdfRptMaHoatDong = (HiddenField)item.FindControl("HdfRptMaHoatDong");
                        Student_Activity studentActivity = new Student_Activity();
                        studentActivity.ActivityId = Int32.Parse(this.HdfMaHoatDong.Value); ;
                        studentActivityBL.DeleteStudentActivity(studentActivity);
                        if (studentActivityBL.IsDeletable(studentActivity))
                        {
                            studentActivityBL.DeleteStudentActivity(studentActivity);
                        }
                        else
                        {
                            bInfoInUse = true;
                        }
                    }
                }
            }

            isSearch = false;
            BindRptStudentActivities();

            if (bInfoInUse)
            {
                MPEInfoInUse.Show();
            }
        }

        protected void BtnBackPrevPage_Click(object sender, ImageClickEventArgs e)
        {
            Configuration_Year year = new Configuration_Year();
            year.YearId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_STUDENTID].ToString());
            AddSession(AppConstant.SESSION_SELECTED_YEAR, year);

            Category_Faculty faculty = new Category_Faculty();
            faculty.FacultyId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTYID].ToString());
            AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);

            Category_Grade grade = new Category_Grade();
            grade.GradeId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_GRADEID].ToString());
            AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);

            Class_Class Class = new Class_Class();
            Class.ClassId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID].ToString());
            AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);

            String strStudentName = ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTNAME].ToString();
            AddSession(AppConstant.SESSION_SELECTED_STUDENTNAME, strStudentName);

            String strStudentCode = ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTCODE].ToString();
            AddSession(AppConstant.SESSION_SELECTED_STUDENTCODE, strStudentCode);

            Response.Redirect(AppConstant.PAGEPATH_STUDENT_LIST);
        }
        #endregion

        #region Repeater event handlers
        protected void RptHoatDong_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
                        student.StudentCode = LblStudentCode.Text;
                        student.FullName = LblStudentName.Text;
                        AddSession(AppConstant.SESSION_STUDENT, student);

                        Configuration_Year year = new Configuration_Year();
                        year.YearId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_STUDENTID].ToString());
                        AddSession(AppConstant.SESSION_SELECTED_YEAR, year);

                        Category_Faculty faculty = new Category_Faculty();
                        faculty.FacultyId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTYID].ToString());
                        AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);

                        Category_Grade grade = new Category_Grade();
                        grade.GradeId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_GRADEID].ToString());
                        AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);

                        Class_Class Class = new Class_Class();
                        Class.ClassId = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID].ToString());
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