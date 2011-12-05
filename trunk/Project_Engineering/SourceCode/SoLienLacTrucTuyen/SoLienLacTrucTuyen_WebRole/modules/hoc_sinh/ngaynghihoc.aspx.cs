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
    public partial class StudentAbsentPage : BaseContentPage
    {
        #region Fields
        private StudentBL studentBL;
        private AbsentBL absentBL;
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

            studentBL = new StudentBL(UserSchool);
            absentBL = new AbsentBL(UserSchool);

            systemConfigBL = new SystemConfigBL(UserSchool);

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
                    BindRptStudentAbsents();

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
            BindDDDLTerms();
            BindDDLSessions();
        }

        private void BindDDLYears()
        {
            Student_Student student = new Student_Student();
            student.StudentId = (int)ViewState[AppConstant.VIEWSTATE_STUDENTID];
            List<Configuration_Year> lstNamHoc = studentBL.GetYears(student);
            DdlNamHoc.DataSource = lstNamHoc;
            DdlNamHoc.DataValueField = "YearId";
            DdlNamHoc.DataTextField = "YearName";
            DdlNamHoc.DataBind();
        }

        private void BindDDDLTerms()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<Configuration_Term> lstHocKy = systemConfigBL.GetListTerms();
            DdlHocKy.DataSource = lstHocKy;
            DdlHocKy.DataValueField = "TermId";
            DdlHocKy.DataTextField = "TermName";
            DdlHocKy.DataBind();
            DdlHocKy.SelectedValue = (new SystemConfigBL(UserSchool)).GetCurrentTerm().ToString();

            DdlHocKyThem.DataSource = lstHocKy;
            DdlHocKyThem.DataValueField = "TermId";
            DdlHocKyThem.DataTextField = "TermName";
            DdlHocKyThem.DataBind();
            DdlHocKyThem.SelectedValue = (new SystemConfigBL(UserSchool)).GetCurrentTerm().ToString();

            DdlHocKySua.DataSource = lstHocKy;
            DdlHocKySua.DataValueField = "TermId";
            DdlHocKySua.DataTextField = "TermName";
            DdlHocKySua.DataBind();
            DdlHocKySua.SelectedValue = (new SystemConfigBL(UserSchool)).GetCurrentTerm().ToString();
        }

        private void BindDDLSessions()
        {
            List<Configuration_Session> lstBuoi = systemConfigBL.GetSessions();
            DdlBuoiThem.DataSource = lstBuoi;
            DdlBuoiThem.DataValueField = "SessionId";
            DdlBuoiThem.DataTextField = "SessionName";
            DdlBuoiThem.DataBind();
            DdlBuoiThem.Items.Insert(0, new ListItem("Cả ngày", "0"));

            DdlBuoiSua.DataSource = lstBuoi;
            DdlBuoiSua.DataValueField = "SessionId";
            DdlBuoiSua.DataTextField = "SessionName";
            DdlBuoiSua.DataBind();
            DdlBuoiSua.Items.Insert(0, new ListItem("Cả ngày", "0"));
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

        private void BindRptStudentAbsents()
        {
            Student_Student student = null;
            Configuration_Year year = null;
            Configuration_Term term = null;
            double dTotalRecords;
            List<TabularAbsent> tabularAbsents;

            student = new Student_Student();
            student.StudentId = (int)ViewState[AppConstant.VIEWSTATE_STUDENTID];// Int32.Parse(this.HdfMaHocSinh.Value);
            year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);
            term = new Configuration_Term();
            term.TermId = Int32.Parse(DdlHocKy.SelectedValue);
            DateTime dtBeginDate = DateTime.Parse(TxtTuNgay.Text);
            DateTime dtEndDate = DateTime.Parse(TxtDenNgay.Text);

            tabularAbsents = absentBL.GetTabularAbsents(student, year, term, dtBeginDate, dtEndDate,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            if (dTotalRecords != 0 && tabularAbsents.Count == 0)
            {
                MainDataPager.CurrentIndex--;
                BindRptStudentAbsents();
                return;
            }

            bool bDisplayData = (tabularAbsents.Count != 0) ? true : false;
            ProcessDislayInfo(bDisplayData);
            RptNgayNghi.DataSource = tabularAbsents;
            RptNgayNghi.DataBind();
            MainDataPager.ItemCount = dTotalRecords;
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
            Student_Absent absent = null;

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
                            absent = new Student_Absent();
                            absent.AbsentId = Int32.Parse(((HiddenField)control).Value);
                            if (absentBL.Confirmed(absent))
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
                        Student_Absent ngayNghi = absentBL.GetAbsent(maNgayNghiHoc);

                        this.DdlHocKySua.SelectedValue = ngayNghi.TermId.ToString();
                        this.TxtNgaySua.Text = ngayNghi.Date.ToShortDateString();
                        this.DdlBuoiSua.SelectedValue = ngayNghi.SessionId.ToString();
                        this.RbtnCoSua.Checked = ngayNghi.IsAsked;
                        this.RbtnKhongSua.Checked = !ngayNghi.IsAsked;
                        this.TxtLyDoSua.Text = ngayNghi.Reason;

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

        #region Button event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            isSearch = true;
            BindRptStudentAbsents();
        }

        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {
            Student_Student student = null;
            Configuration_Term term = null;
            Configuration_Session session = new Configuration_Session();

            string strDate = TxtNgayThem.Text.Trim();
            student = new Student_Student();
            student.StudentId = (int)ViewState[AppConstant.VIEWSTATE_STUDENTID];
            term = new Configuration_Term();
            term.TermId = Int32.Parse(this.DdlHocKyThem.SelectedValue);
            if (this.DdlBuoiThem.SelectedIndex >= 0)
            {
                session.SessionId = Int32.Parse(this.DdlBuoiThem.SelectedValue);
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

                if (absentBL.AbsentExists(null, student, term, DateTime.Parse(this.TxtNgayThem.Text.Trim()), session))
                {
                    NgayValidatorAdd.IsValid = false;
                    MPEAdd.Show();
                    return;
                }
            }

            DateTime ngay = DateTime.Parse(this.TxtNgayThem.Text.Trim());

            absentBL.InsertAbsent(student, term, ngay, session, xinPhep, lyDo);

            MainDataPager.CurrentIndex = 1;
            BindRptStudentAbsents();

            this.DdlHocKyThem.SelectedValue = (new SystemConfigBL(UserSchool)).GetCurrentTerm().TermId.ToString();
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
            Student_Student student = null;
            Student_Absent absent = null;
            Configuration_Term term = null;
            Configuration_Session session = null;
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
            absent = new Student_Absent();
            absent.AbsentId = Int32.Parse(this.HdfMaNgayNghiHoc.Value);
            student = new Student_Student();
            student.StudentId = Int32.Parse(this.HdfMaHocSinh.Value);
            term = new Configuration_Term();
            term.TermId = Int32.Parse(this.DdlHocKySua.SelectedValue);
            if (DdlBuoiSua.SelectedIndex > 0)
            {
                session = new Configuration_Session();
                session.SessionId = Int32.Parse(this.DdlBuoiSua.SelectedValue);
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

                if (absentBL.AbsentExists(absent, student, term, DateTime.Parse(this.TxtNgaySua.Text.Trim()), session))
                {
                    NgayValidatorEdit.IsValid = false;
                    modalPopupEdit.Show();
                    return;
                }
            }

            date = DateTime.Parse(this.TxtNgaySua.Text);
            bool xinPhep = this.RbtnCoSua.Checked;
            string lyDo = this.TxtLyDoSua.Text;

            absentBL.UpdateAbsent(absent, term, date, session, xinPhep, lyDo);

            MainDataPager.CurrentIndex = 1;
            BindRptStudentAbsents();

            this.DdlHocKySua.SelectedIndex = 0;
            this.TxtNgaySua.Text = DateTime.Now.ToShortDateString();
            this.DdlBuoiSua.SelectedIndex = 0;
            this.RbtnCoSua.Checked = true;
            this.TxtLyDoSua.Text = "";
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            int maNgayNghiHoc = Int32.Parse(this.HdfMaNgayNghiHoc.Value);
            absentBL.DeleteAbsent(maNgayNghiHoc);
            isSearch = false;
            BindRptStudentAbsents();
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

        #region DataPager event handlers
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            MainDataPager.CurrentIndex = currentPageIndex;
            BindRptStudentAbsents();
        }
        #endregion
    }
}