using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.BusinessEntity;
using EContactBook.DataAccess;
using System.Web.Security;
using System.Text;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class AddDetailSchedulePage : BaseContentPage
    {
        #region Fields
        private ScheduleBL scheduleBL;
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

            scheduleBL = new ScheduleBL(UserSchool);

            if (!Page.IsPostBack)
            {
                if (RetrieveSessions())
                {
                    FillDDLFaculties();
                    FillDDLGrades();
                }
                else
                {
                    Response.Redirect(AppConstant.PAGEPATH_SCHEDULE_AGGRANGE);
                }
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnOpenPopupMonHoc_Click(object sender, ImageClickEventArgs e)
        {
            BindRptSubjects();
            MPEMonHoc.Show();
        }

        protected void BtnSearchMonHoc_Click(object sender, ImageClickEventArgs e)
        {
            DataPageMonHoc.CurrentIndex = 1;
            BindRptSubjects();
            MPEMonHoc.Show();
        }

        protected void BtnSaveMonHoc_Click(object sender, ImageClickEventArgs e)
        {
            RadioButton rBtnSelect = null;
            HiddenField hdfRptSubjectId = null;
            Label lblSubjectName = null;

            foreach (RepeaterItem item in RptMonHoc.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    rBtnSelect = (RadioButton)item.FindControl("RBtnSelect");
                    if (rBtnSelect.Checked)
                    {
                        hdfRptSubjectId = (HiddenField)item.FindControl("HdfRptSubjectId");
                        HdfSubjectId.Value = hdfRptSubjectId.Value;

                        lblSubjectName = (Label)item.FindControl("LblSubjectName");
                        LblMonHoc.Text = lblSubjectName.Text;
                        break;
                    }
                }
            }
        }

        protected void BtnOpenPopupGiaoVien_Click(object sender, ImageClickEventArgs e)
        {
            BindRptTeachers();
            MPEGiaoVien.Show();
        }

        protected void BtnSearchGiaoVien_Click(object sender, ImageClickEventArgs e)
        {
            DataPageGiaoVien.CurrentIndex = 1;
            BindRptTeachers();
            MPEGiaoVien.Show();
        }

        protected void BtnSaveGiaoVien_Click(object sender, ImageClickEventArgs e)
        {
            RadioButton rBtnSelect = null;
            HiddenField hdfRptUserId = null;
            Label lblTenGiaoVien = null;

            foreach (RepeaterItem item in RptGiaoVien.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    rBtnSelect = (RadioButton)item.FindControl("RBtnSelect");
                    if (rBtnSelect.Checked)
                    {
                        hdfRptUserId = (HiddenField)item.FindControl("HdfRptUserId");
                        HdfUserId.Value = hdfRptUserId.Value;

                        lblTenGiaoVien = (Label)item.FindControl("LblTenGiaoVien");
                        LblGiaoVien.Text = lblTenGiaoVien.Text;
                        break;
                    }

                }
            }
        }

        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {
            Category_Subject subject = null;
            aspnet_User teacher = null;

            if (!ValidateInputs())
            {
                return;
            }

            ScheduleBL scheduleBL = new ScheduleBL(UserSchool);
            Class_Class Class = new Class_Class();
            Class.ClassId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID];

            Configuration_Term term = new Configuration_Term();
            term.TermId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_TERMID];

            Configuration_DayInWeek dayInWeek = new Configuration_DayInWeek();
            dayInWeek.DayInWeekId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_DAYINWEEK];

            Category_TeachingPeriod teachingPeriod = new Category_TeachingPeriod();
            teachingPeriod.TeachingPeriodId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_TEACHINGPERIOD];

            subject = new Category_Subject();
            subject.SubjectId = Int32.Parse(HdfSubjectId.Value);
            teacher = new aspnet_User();
            teacher.UserId = new Guid(HdfUserId.Value);

            if (CheckSessionKey(AppConstant.SESSION_WEEKLYSCHEDULE))
            {
                List<List<TeachingPeriodSchedule>> listOfTeachingPeriodSchedules = (List<List<TeachingPeriodSchedule>>)GetSession(AppConstant.SESSION_WEEKLYSCHEDULE);
                for (int i = 0; i < listOfTeachingPeriodSchedules.Count; i++)
                {
                    if (listOfTeachingPeriodSchedules[i][0].DayInWeekId == dayInWeek.DayInWeekId)
                    {
                        for (int j = 0; j < listOfTeachingPeriodSchedules[i].Count; j++)
                        {
                            if (listOfTeachingPeriodSchedules[i][j].TeachingPeriodId == teachingPeriod.TeachingPeriodId)
                            {
                                listOfTeachingPeriodSchedules[i][j].SubjectId = subject.SubjectId;
                                listOfTeachingPeriodSchedules[i][j].SubjectName = LblMonHoc.Text;
                                listOfTeachingPeriodSchedules[i][j].UserId = teacher.UserId;
                                listOfTeachingPeriodSchedules[i][j].TeacherName = LblGiaoVien.Text;
                                
                                AddSession(AppConstant.SESSION_WEEKLYSCHEDULE, listOfTeachingPeriodSchedules);

                                BackToPrevPage();
                            }
                        }
                    }
                }
            }
        }

        protected void BtnCancelAdd_Click(object sender, ImageClickEventArgs e)
        {
            BackToPrevPage();
        }
        #endregion

        #region Pager event handlers
        public void DataPagerMonHoc_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            DataPageMonHoc.CurrentIndex = currnetPageIndx;
            BindRptSubjects();
            MPEMonHoc.Show();
        }

        public void DataPagerGiaoVien_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            DataPageGiaoVien.CurrentIndex = currnetPageIndx;
            BindRptTeachers();
            MPEGiaoVien.Show();
        }
        #endregion

        #region Methods
        private void FillDDLGrades()
        {
            GradeBL gradeBL = new GradeBL(UserSchool);
            List<Category_Grade> grades = gradeBL.GetListGrades();

            DdlKhoi.DataSource = grades;
            DdlKhoi.DataValueField = "GradeName";
            DdlKhoi.DataTextField = "GradeName";
            DdlKhoi.DataBind();
        }

        private void FillDDLFaculties()
        {
            FacultyBL facultyBL = new FacultyBL(UserSchool);
            List<Category_Faculty> faculties = facultyBL.GetFaculties();

            DdlNganh.DataSource = faculties;
            DdlNganh.DataValueField = "FacultyName";
            DdlNganh.DataTextField = "FacultyName";
            DdlNganh.DataBind();
        }

        private void BindRptSubjects()
        {
            Category_Faculty faculty = null;
            Category_Grade grade = null;
            string subjectName = TxtMonHoc.Text.Trim();

            if (DdlNganh.SelectedIndex >= 0)
            {
                faculty = (new FacultyBL(UserSchool)).GetFaculty(DdlNganh.SelectedValue);
            }
            else
            {
                LblSearchResultMonHoc.Visible = true;
                DataPageMonHoc.Visible = false;
                return;
            }

            if (DdlKhoi.SelectedIndex >= 0)
            {
                grade = (new GradeBL(UserSchool)).GetGrade(DdlKhoi.SelectedValue);
            }
            else
            {
                LblSearchResultMonHoc.Visible = true;
                DataPageMonHoc.Visible = false;
                return;
            }

            double dTotalRecords;
            List<TabularSubject> tabularSubjects = (new SubjectBL(UserSchool)).GetListTabularSubjects(faculty, grade,
                    subjectName,
                    DataPageMonHoc.CurrentIndex, DataPageMonHoc.PageSize, out dTotalRecords);
            DataPageMonHoc.ItemCount = dTotalRecords;

            bool bDisplayData = (tabularSubjects.Count != 0) ? true : false;
            LblSearchResultMonHoc.Visible = !bDisplayData;
            DataPageMonHoc.Visible = bDisplayData;

            BtnSaveMonHoc.Enabled = bDisplayData;
            BtnSaveMonHoc.ImageUrl = (bDisplayData) ? "~/Styles/buttons/button_save.png" : "~/Styles/buttons/button_save_disable.png";
            RptMonHoc.Visible = bDisplayData;
            RptMonHoc.DataSource = tabularSubjects;
            RptMonHoc.DataBind();

            if (tabularSubjects.Count != 0)
            {
                RepeaterItemCollection items = RptMonHoc.Items;
                if (items[0].ItemType == ListItemType.Item || items[0].ItemType == ListItemType.AlternatingItem)
                {
                    Control control = items[0].FindControl("RBtnSelect");
                    if (control != null)
                    {
                        RadioButton rBtnSelect = (RadioButton)control;
                        rBtnSelect.Checked = true;
                    }
                }
            }
        }

        private void BindRptTeachers()
        {
            TeacherBL teacherBL = new TeacherBL(UserSchool);

            string strTeacherCode = TxtSearchUserId.Text.Trim();
            string strTeacherName = TxtSearchTenGiaoVien.Text.Trim();

            double dTotalRecords;
            List<TabularTeacher> tabularTeachers = teacherBL.GetTabularTeachers(
                strTeacherCode, strTeacherName,
                DataPageGiaoVien.CurrentIndex, DataPageGiaoVien.PageSize, out dTotalRecords);
            DataPageGiaoVien.ItemCount = dTotalRecords;

            bool bDisplayData = (tabularTeachers.Count != 0) ? true : false;
            LblSearchResultGiaoVien.Visible = !bDisplayData;
            DataPageGiaoVien.Visible = bDisplayData;

            BtnSaveGiaoVien.Enabled = bDisplayData;
            BtnSaveGiaoVien.ImageUrl = (bDisplayData) ? "~/Styles/buttons/button_save.png" : "~/Styles/buttons/button_save_disable.png";
            RptGiaoVien.Visible = bDisplayData;
            RptGiaoVien.DataSource = tabularTeachers;
            RptGiaoVien.DataBind();

            if (tabularTeachers.Count != 0)
            {
                RepeaterItemCollection items = RptGiaoVien.Items;
                if (items[0].ItemType == ListItemType.Item || items[0].ItemType == ListItemType.AlternatingItem)
                {
                    Control control = items[0].FindControl("RBtnSelect");
                    if (control != null)
                    {
                        RadioButton rBtnSelect = (RadioButton)control;
                        rBtnSelect.Checked = true;
                    }
                }
            }
        }

        private bool ValidateInputs()
        {
            bool bValid;

            if (HdfSubjectId.Value == AppConstant.STRING_ZERO || HdfUserId.Value == AppConstant.STRING_ZERO)
            {
                bValid = false;
            }
            else
            {
                bValid = true;
            }

            if (HdfSubjectId.Value == AppConstant.STRING_ZERO)
            {
                LblMonHocError.Visible = true;
            }
            else
            {
                LblMonHocError.Visible = false;
            }

            if (HdfUserId.Value == AppConstant.STRING_ZERO)
            {
                LblGiaoVienError.Text = "Chưa chọn giáo viên";
                LblGiaoVienError.Visible = true;
            }
            else
            {
                LblGiaoVienError.Visible = false;

                if (HdfUserId.Value != AppConstant.STRING_ZERO)
                {
                    Guid UserId = new Guid(HdfUserId.Value);
                    TeacherBL giaoVienBL = new TeacherBL(UserSchool);
                    aspnet_User teacher = new aspnet_User();
                    teacher.UserId = UserId;
                    Configuration_Term term = new Configuration_Term();
                    term.TermId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_TERMID];
                    Configuration_DayInWeek dayInWeek = new Configuration_DayInWeek();
                    dayInWeek.DayInWeekId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_DAYINWEEK];
                    Category_TeachingPeriod teachingPeriod = new Category_TeachingPeriod();
                    teachingPeriod.TeachingPeriodId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_TEACHINGPERIOD];
                    if (giaoVienBL.IsTeaching(teacher, term, dayInWeek, teachingPeriod))
                    {
                        bValid = false;
                        LblGiaoVienError.Text = "Giáo viên đang được phân công dạy tại cùng thời gian này";
                        LblGiaoVienError.Visible = true;
                    }
                }
            }

            return bValid;
        }

        private bool RetrieveSessions()
        {
            if (CheckSessionKey(AppConstant.SESSION_SELECTED_CLASS)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_TERM)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_DAYINWEEK)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_TEACHINGPERIOD))
            {
                Class_Class Class = (Class_Class)GetSession(AppConstant.SESSION_SELECTED_CLASS);
                Class = (new ClassBL(UserSchool)).GetClass(Class.ClassId);
                ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID] = Class.ClassId;

                Configuration_Term term = (Configuration_Term)GetSession(AppConstant.SESSION_SELECTED_TERM);
                SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
                term = systemConfigBL.GetTerm(term.TermId);
                ViewState[AppConstant.VIEWSTATE_SELECTED_TERMID] = term.TermId;

                Configuration_DayInWeek dayInWeek = (Configuration_DayInWeek)GetSession(AppConstant.SESSION_SELECTED_DAYINWEEK);
                dayInWeek = systemConfigBL.GetDayInWeek(dayInWeek.DayInWeekId);
                ViewState[AppConstant.VIEWSTATE_SELECTED_DAYINWEEK] = dayInWeek.DayInWeekId;

                Category_TeachingPeriod teachingPeriod = (Category_TeachingPeriod)GetSession(AppConstant.SESSION_SELECTED_TEACHINGPERIOD);
                teachingPeriod = (new TeachingPeriodBL(UserSchool)).GetTeachingPeriod(teachingPeriod.TeachingPeriodId);
                ViewState[AppConstant.VIEWSTATE_SELECTED_TEACHINGPERIOD] = teachingPeriod.TeachingPeriodId;

                LblTenLop.Text = Class.ClassName;
                LblNamHoc.Text = Class.Configuration_Year.YearName;
                LblHocKy.Text = term.TermName;
                LblThu.Text = dayInWeek.DayInWeekName;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("");
                stringBuilder.Append(teachingPeriod.TeachingPeriodName);
                stringBuilder.Append(" (");
                stringBuilder.Append(teachingPeriod.BeginTime.ToShortTimeString());
                stringBuilder.Append(" - ");
                stringBuilder.Append(teachingPeriod.EndTime.ToShortTimeString());
                stringBuilder.Append(")");
                LblTiet.Text = stringBuilder.ToString();

                return true;
            }

            return false;
        }

        private void BackToPrevPage()
        {
            Class_Class Class = new Class_Class();
            Class.ClassId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID];
            AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);

            Configuration_Term term = new Configuration_Term();
            term.TermId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_TERMID];
            AddSession(AppConstant.VIEWSTATE_SELECTED_TERMID, term);

            AddSession(AppConstant.SESSION_SCHEDULE_EVENT_ADD, 2);

            Response.Redirect(AppConstant.PAGEPATH_SCHEDULE_AGGRANGE);
        }
        #endregion
    }
}