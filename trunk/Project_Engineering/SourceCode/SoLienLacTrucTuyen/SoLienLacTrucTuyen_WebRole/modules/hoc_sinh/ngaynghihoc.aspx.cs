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
                    BindRptStudentAbsents();

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
            BindDDDLTerms();
            BindDDLSessions();
        }

        private void BindDDLYears()
        {
            Student_Student student = new Student_Student();
            student.StudentId = (int)ViewState[AppConstant.VIEWSTATE_STUDENTID];
            List<Configuration_Year> years = studentBL.GetYears(student);
            DdlNamHoc.DataSource = years;
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

        private void BindRptStudentAbsents()
        {
            Student_Student student = null;
            Configuration_Year year = null;
            Configuration_Term term = null;
            double dTotalRecords;
            List<TabularAbsent> tabularAbsents;

            student = new Student_Student();
            student.StudentId = (int)ViewState[AppConstant.VIEWSTATE_STUDENTID];
            year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);
            term = new Configuration_Term();
            term.TermId = Int32.Parse(DdlHocKy.SelectedValue);
            DateTime? dtBeginDate = null;
            if (CheckUntils.IsNullOrBlank(TxtTuNgay.Text))
            {
                BeginDateRequired.IsValid = false;
                return;
            }
            else
            {
                dtBeginDate = DateUtils.StringToDateVN(TxtTuNgay.Text);
                if (dtBeginDate == null)
                {
                    BeginDateValidator.IsValid = false;
                    return;
                }
            }

            DateTime? dtEndDate = null;
            if (CheckUntils.IsNullOrBlank(TxtDenNgay.Text))
            {
                EndDateRequired.IsValid = false;
                return;
            }
            else
            {
                dtEndDate = DateUtils.StringToDateVN(TxtDenNgay.Text);
                if (dtBeginDate == null)
                {
                    EndDateValidator.IsValid = false;
                    return;
                }
            } 

            tabularAbsents = absentBL.GetTabularAbsents(student, year, term, (DateTime)dtBeginDate, (DateTime)dtEndDate,
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
            DateTime? dtDate = null;
            if (strDate == "")
            {
                NgayRequiredAdd.IsValid = false;
                MPEAdd.Show();
                return;
            }
            else
            {
                //if (!Regex.IsMatch(strDate, NgayExpression.ValidationExpression))
                //{
                //    NgayExpression.IsValid = false;
                //    MPEAdd.Show();
                //    return;
                //}

                dtDate = DateUtils.StringToDateVN(TxtNgayThem.Text.Trim());

                if(dtDate == null)
                {
                    DateTimeValidatorAdd.IsValid = false;
                    MPEAdd.Show();
                    return;
                }

                if (absentBL.AbsentExists(null, student, term, (DateTime)dtDate, session))
                {
                    NgayValidatorAdd.IsValid = false;
                    MPEAdd.Show();
                    return;
                }
            }

            absentBL.InsertAbsent(student, term, (DateTime)dtDate, session, xinPhep, lyDo);

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

        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            HiddenField HdfRptMaNgayNghiHoc = null;
            foreach (RepeaterItem item in RptNgayNghi.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox CkbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (CkbxSelect.Checked)
                    {
                        HdfRptMaNgayNghiHoc = (HiddenField)item.FindControl("HdfRptMaNgayNghiHoc");
                        Student_Absent absent = absentBL.GetAbsent(Int32.Parse(HdfRptMaNgayNghiHoc.Value));

                        this.DdlHocKySua.SelectedValue = absent.TermId.ToString();
                        this.TxtNgaySua.Text = absent.Date.ToString(AppConstant.DATEFORMAT_DDMMYYYY);
                        this.DdlBuoiSua.SelectedValue = absent.SessionId.ToString();
                        this.RbtnCoSua.Checked = absent.IsAsked;
                        this.RbtnKhongSua.Checked = !absent.IsAsked;
                        this.TxtLyDoSua.Text = absent.Reason;

                        this.HdfMaNgayNghiHoc.Value = HdfRptMaNgayNghiHoc.Value;                        

                        MPEEdit.Show();
                        return;
                    }
                }
            }
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            Student_Student student = null;
            Student_Absent absent = null;
            Configuration_Term term = null;
            Configuration_Session session = null;

            string strDate = TxtNgaySua.Text.Trim();
            absent = new Student_Absent();
            absent.AbsentId = Int32.Parse(this.HdfMaNgayNghiHoc.Value);
            student = new Student_Student();
            student.StudentId = (int)ViewState[AppConstant.VIEWSTATE_STUDENTID];
            term = new Configuration_Term();
            term.TermId = Int32.Parse(this.DdlHocKySua.SelectedValue);
            if (DdlBuoiSua.SelectedIndex > 0)
            {
                session = new Configuration_Session();
                session.SessionId = Int32.Parse(this.DdlBuoiSua.SelectedValue);
            }
            DateTime? dtDate = null;
            if (strDate == "")
            {
                NgayRequiredEdit.IsValid = false;
                MPEEdit.Show();
                return;
            }
            else
            {
                //if (!Regex.IsMatch(strDate, NgayExpressionEdit.ValidationExpression))
                //{
                //    NgayExpressionEdit.IsValid = false;
                //    MPEEdit.Show();
                //    return;
                //}

                dtDate = DateUtils.StringToDateVN(strDate);
                if(dtDate == null)
                {
                    DateTimeValidatorEdit.IsValid = false;
                    MPEEdit.Show();
                    return;
                }

                if (absentBL.AbsentExists(absent, student, term, (DateTime)dtDate, session))
                {
                    NgayValidatorEdit.IsValid = false;
                    MPEEdit.Show();
                    return;
                }
            }

            bool bIsAsked = this.RbtnCoSua.Checked;
            string strReason = this.TxtLyDoSua.Text;

            absentBL.UpdateAbsent(absent, term, (DateTime)dtDate, session, bIsAsked, strReason);

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
            bool bInfoInUse = false;
            CheckBox ckbxSelect = null;
            HiddenField HdfRptMaNgayNghiHoc = null;

            foreach (RepeaterItem item in RptNgayNghi.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    ckbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (ckbxSelect.Checked)
                    {
                        HdfRptMaNgayNghiHoc = (HiddenField)item.FindControl("HdfRptMaNgayNghiHoc");
                        Student_Absent absent = new Student_Absent();
                        absent.AbsentId = Int32.Parse(HdfRptMaNgayNghiHoc.Value);

                        if (absentBL.IsDeletable(absent))
                        {
                            absentBL.DeleteAbsent(absent);
                        }
                        else
                        {
                            bInfoInUse = true;
                        }
                    }
                }
            }

            isSearch = false;
            BindRptStudentAbsents();

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