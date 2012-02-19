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
    public partial class SuaTietThoiKhoaBieuPage : BaseContentPage
    {
        #region Fields
        ScheduleBL scheduleBL;
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

            // Init variables
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
                    Response.Redirect(AppConstant.PAGEPATH_SCHEDULE);
                }
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnOpenPopupMonHoc_Click(object sender, ImageClickEventArgs e)
        {
            BindRepeaterMonHoc();
            MPEMonHoc.Show();
        }

        protected void BtnSearchMonHoc_Click(object sender, ImageClickEventArgs e)
        {
            DataPageMonHoc.CurrentIndex = 1;
            BindRepeaterMonHoc();
            MPEMonHoc.Show();
        }

        protected void BtnSaveMonHoc_Click(object sender, ImageClickEventArgs e)
        {
            foreach (RepeaterItem item in RptMonHoc.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    Control control = item.FindControl("RBtnSelect");
                    if (control != null)
                    {
                        RadioButton rBtnSelect = (RadioButton)control;
                        if (rBtnSelect.Checked)
                        {
                            HiddenField hdfRptSubjectId = (HiddenField)item.FindControl("HdfRptSubjectId");
                            HdfSubjectId.Value = hdfRptSubjectId.Value;

                            Label lblSubjectName = (Label)item.FindControl("LblSubjectName");
                            LblMonHoc.Text = lblSubjectName.Text;
                        }
                    }
                }
            }
        }

        protected void BtnOpenPopupGiaoVien_Click(object sender, ImageClickEventArgs e)
        {
            BindRepeaterGiaoVien();
            MPEGiaoVien.Show();
        }

        protected void BtnSearchGiaoVien_Click(object sender, ImageClickEventArgs e)
        {
            DataPageGiaoVien.CurrentIndex = 1;
            BindRepeaterGiaoVien();
            MPEGiaoVien.Show();
        }

        protected void BtnSaveGiaoVien_Click(object sender, ImageClickEventArgs e)
        {
            foreach (RepeaterItem item in RptGiaoVien.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    Control control = item.FindControl("RBtnSelect");
                    if (control != null)
                    {
                        RadioButton rBtnSelect = (RadioButton)control;
                        if (rBtnSelect.Checked)
                        {
                            HiddenField hdfRptUserId = (HiddenField)item.FindControl("HdfRptUserId");
                            HdfUserId.Value = hdfRptUserId.Value;

                            Label lblTenGiaoVien = (Label)item.FindControl("LblTenGiaoVien");
                            LblGiaoVien.Text = lblTenGiaoVien.Text;
                        }
                    }
                }
            }
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            Category_Subject subject = null;
            aspnet_User teacher = null;

            if (!ValidateInputs())
            {
                return;
            }

            ScheduleBL scheduleBL = new ScheduleBL(UserSchool);

            Class_Schedule schedule = new Class_Schedule();
            schedule.ScheduleId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_SCHEDULEID];

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

        protected void BtnCancelEdit_Click(object sender, ImageClickEventArgs e)
        {
            BackToPrevPage();
        }
        #endregion

        #region Pager event handlers
        public void DataPagerMonHoc_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            DataPageMonHoc.CurrentIndex = currnetPageIndx;
            BindRepeaterMonHoc();
            MPEMonHoc.Show();
        }

        public void DataPagerGiaoVien_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            DataPageGiaoVien.CurrentIndex = currnetPageIndx;
            BindRepeaterGiaoVien();
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

        private void BindRepeaterMonHoc()
        {
            Category_Faculty faculty = null;
            Category_Grade grade = null;
            string subjectName = TxtMonHoc.Text.Trim();

            if (DdlNganh.SelectedIndex >= 0)
            {
                if (ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTYID] != null)
                {
                    faculty = (new FacultyBL(UserSchool)).GetFaculty((string)ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTYID]);
                }
                else
                {
                    faculty = (new FacultyBL(UserSchool)).GetFaculty(DdlNganh.SelectedValue);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTYID] = DdlNganh.SelectedValue;
                }
            }
            else
            {
                LblSearchResultMonHoc.Visible = true;
                DataPageMonHoc.Visible = false;
                return;
            }

            if (DdlKhoi.SelectedIndex >= 0)
            {
                if (ViewState[AppConstant.VIEWSTATE_SELECTED_GRADEID] != null)
                {
                    grade = (new GradeBL(UserSchool)).GetGrade((string)ViewState[AppConstant.VIEWSTATE_SELECTED_GRADEID]);
                }
                else
                {
                    grade = (new GradeBL(UserSchool)).GetGrade(DdlKhoi.SelectedValue);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_GRADEID] = DdlKhoi.SelectedValue;
                }
            }
            else
            {
                LblSearchResultMonHoc.Visible = true;
                DataPageMonHoc.Visible = false;
                return;
            }

            double dTotalRecords;
            List<TabularSubject> lTabularSubjects = (new SubjectBL(UserSchool)).GetListTabularSubjects(faculty, grade,
                    subjectName,
                    DataPageMonHoc.CurrentIndex, DataPageMonHoc.PageSize, out dTotalRecords);
            DataPageMonHoc.ItemCount = dTotalRecords;

            bool bDisplayData = (lTabularSubjects.Count != 0) ? true : false;
            LblSearchResultMonHoc.Visible = !bDisplayData;
            DataPageMonHoc.Visible = bDisplayData;

            BtnSaveMonHoc.Enabled = bDisplayData;
            BtnSaveMonHoc.ImageUrl = (bDisplayData) ? "~/Styles/buttons/button_save.png" : "~/Styles/buttons/button_save_disable.png";
            RptMonHoc.Visible = bDisplayData;
            RptMonHoc.DataSource = lTabularSubjects;
            RptMonHoc.DataBind();

            if (lTabularSubjects.Count != 0)
            {
                RepeaterItemCollection items = RptMonHoc.Items;
                if (items[0].ItemType == ListItemType.Item
                    || items[0].ItemType == ListItemType.AlternatingItem)
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

        private void BindRepeaterGiaoVien()
        {
            TeacherBL giaoVienBL = new TeacherBL(UserSchool);

            string maHienThiGiaoVien = TxtSearchUserId.Text.Trim();
            string hoTen = TxtSearchTenGiaoVien.Text.Trim();

            double dTotalRecords;
            List<TabularTeacher> lstTbGiaoViens = giaoVienBL.GetTabularTeachers(
                maHienThiGiaoVien, hoTen,
                DataPageGiaoVien.CurrentIndex, DataPageGiaoVien.PageSize, out dTotalRecords);
            DataPageGiaoVien.ItemCount = dTotalRecords;

            bool bDisplayData = (lstTbGiaoViens.Count != 0) ? true : false;
            LblSearchResultGiaoVien.Visible = !bDisplayData;
            DataPageGiaoVien.Visible = bDisplayData;

            BtnSaveGiaoVien.Enabled = bDisplayData;
            BtnSaveGiaoVien.ImageUrl = (bDisplayData) ? "~/Styles/buttons/button_save.png" : "~/Styles/buttons/button_save_disable.png";
            RptGiaoVien.Visible = bDisplayData;
            RptGiaoVien.DataSource = lstTbGiaoViens;
            RptGiaoVien.DataBind();

            if (lstTbGiaoViens.Count != 0)
            {
                RepeaterItemCollection items = RptGiaoVien.Items;
                if (items[0].ItemType == ListItemType.Item
                    || items[0].ItemType == ListItemType.AlternatingItem)
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
                LblGiaoVienError.Visible = true;
            }
            else
            {
                LblGiaoVienError.Visible = false;
            }

            return bValid;
        }

        private bool RetrieveSessions()
        {
            if (CheckSessionKey(AppConstant.SESSION_SCHEDULE))
            {
                TeachingPeriodSchedule schedule = (TeachingPeriodSchedule)GetSession(AppConstant.SESSION_SCHEDULE);
                RemoveSession(AppConstant.SESSION_SCHEDULE);

                ViewState[AppConstant.VIEWSTATE_SELECTED_SCHEDULEID] = schedule.ScheduleId;
                ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID] = schedule.ClassId;
                ViewState[AppConstant.VIEWSTATE_SELECTED_TERMID] = schedule.TermId;
                ViewState[AppConstant.VIEWSTATE_SELECTED_DAYINWEEK] = schedule.DayInWeekId;
                ViewState[AppConstant.VIEWSTATE_SELECTED_TEACHINGPERIOD] = schedule.TeachingPeriodId;
                                
                LblNamHoc.Text = schedule.YearName;
                LblHocKy.Text = schedule.TermName;
                LblThu.Text = schedule.DayInWeekName; 
                LblTiet.Text = schedule.StringDetailTeachingPeriod;
                LblTenLop.Text = schedule.ClassName;
                LblGiaoVien.Text = schedule.TeacherName;
                LblMonHoc.Text = schedule.SubjectName;
                HdfSubjectId.Value = schedule.SubjectId.ToString();
                HdfUserId.Value = schedule.UserId.ToString();               

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

            AddSession(AppConstant.SESSION_SCHEDULE_EVENT_MODIFY, 3);

            Response.Redirect(AppConstant.PAGEPATH_SCHEDULE_AGGRANGE);
        }
        #endregion
    }
}