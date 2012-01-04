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
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class ScheduleArrangementPage : BaseContentPage
    {
        #region Fields
        private ScheduleBL scheduleBL;
        private Class_Class Class;
        private Configuration_Term term;
        private bool dataFromSession;
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
                RemoveSession(AppConstant.SESSION_DAILYSCHEDULES);
                // Get session for the first time
                if (!GetSessions())
                {
                    Response.Redirect(AppConstant.PAGEPATH_SCHEDULE);
                }
            }
            else
            {
                // Get viewstate after the first time
                if (!GetViewStates())
                {
                    Response.Redirect(AppConstant.PAGEPATH_SCHEDULE);
                }
            }

            FillPageTitle();
            BindRptSchedule();
        }
        #endregion

        #region Repeater event handlers
        #endregion

        #region Button event handlers
        protected void BtnSearchMonHoc_Click(object sender, ImageClickEventArgs e)
        {
            DataPageMonHoc.CurrentIndex = 1;
            BindRepeaterMonHoc();
            // MPEMonHoc.Show();
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
                            // HiddenField hdfRptSubjectId = (HiddenField)item.FindControl("HdfRptSubjectId");
                            // HdfSubjectId.Value = hdfRptSubjectId.Value;

                            // Label lblSubjectName = (Label)item.FindControl("LblSubjectName");
                            // LblMonHoc.Text = lblSubjectName.Text;
                        }
                    }
                }
            }
        }

        protected void BtnSearchGiaoVien_Click(object sender, ImageClickEventArgs e)
        {
            DataPageGiaoVien.CurrentIndex = 1;
            BindRepeaterGiaoVien();
            //MPEGiaoVien.Show();
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

                            // Label lblTenGiaoVien = (Label)item.FindControl("LblTenGiaoVien");
                            // LblGiaoVien.Text = lblTenGiaoVien.Text;
                        }
                    }
                }
            }
        }
        #endregion

        #region Methods
        private bool GetSessions()
        {
            if (CheckSessionKey(AppConstant.SESSION_SELECTED_CLASS)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_TERM))
            {
                Class = (Class_Class)GetSession(AppConstant.SESSION_SELECTED_CLASS);
                RemoveSession(AppConstant.SESSION_SELECTED_CLASS);
                Class = (new ClassBL(UserSchool)).GetClass(Class.ClassId);
                ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS] = Class.ClassId;

                term = (Configuration_Term)GetSession(AppConstant.SESSION_SELECTED_TERM);
                RemoveSession(AppConstant.SESSION_SELECTED_TERM);
                term = (new SystemConfigBL(UserSchool)).GetTerm(term.TermId);
                ViewState[AppConstant.VIEWSTATE_SELECTED_TERM] = term.TermId;

                return true;
            }
            else
            {
                return false;
            }
        }

        private bool GetViewStates()
        {
            if (ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS] != null
                && ViewState[AppConstant.VIEWSTATE_SELECTED_TERM] != null)
            {
                Class = (new ClassBL(UserSchool)).GetClass((int)ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS]);
                term = (new SystemConfigBL(UserSchool)).GetTerm((int)ViewState[AppConstant.VIEWSTATE_SELECTED_TERM]);

                return true;
            }
            else
            {
                return false;
            }
        }

        private void FillPageTitle()
        {
            LblTitle.Text = string.Format("THỜI KHÓA BIỂU LỚP {0} ({1} - NĂM HỌC {2})",
                Class.ClassName, term.TermName, Class.Configuration_Year.YearName);
        }

        private void BindRptSchedule()
        {
            List<DailySchedule> dailySchedules = null;
            if (CheckSessionKey(AppConstant.SESSION_DAILYSCHEDULES))
            {
                dailySchedules = (List<DailySchedule>)GetSession(AppConstant.SESSION_DAILYSCHEDULES);
                dataFromSession = true;
                RptDailySchedule.DataSource = dailySchedules;
                RptDailySchedule.DataBind();
            }
            else
            {
                // In case there is no DailySchedule stored in session,
                // Get it from database then store in session
                dailySchedules = scheduleBL.GetDailySchedules(Class, term);
                AddSession(AppConstant.SESSION_DAILYSCHEDULES, dailySchedules);
                dataFromSession = false;
                RptDailySchedule.DataSource = dailySchedules;
                RptDailySchedule.DataBind();
            }
        }

        private void BindRepeaterMonHoc()
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
            List<TabularSubject> lTabularSubjects = (new SubjectBL(UserSchool)).GetListTabularSubjects(faculty, grade,
                    subjectName,
                    DataPageMonHoc.CurrentIndex, DataPageMonHoc.PageSize, out dTotalRecords);
            DataPageMonHoc.ItemCount = dTotalRecords;

            bool bDisplayData = (lTabularSubjects.Count != 0) ? true : false;
            LblSearchResultMonHoc.Visible = !bDisplayData;
            DataPageMonHoc.Visible = bDisplayData;

            BtnSaveMonHoc.Enabled = bDisplayData;
            BtnSaveMonHoc.ImageUrl = (bDisplayData) ? "~/Styles/Images/button_save.png" : "~/Styles/Images/button_save_disable.png";
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
            BtnSaveGiaoVien.ImageUrl = (bDisplayData) ? "~/Styles/Images/button_save.png" : "~/Styles/Images/button_save_disable.png";
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
        #endregion

        #region Repeater event handlers
        protected void RptDailySchedule_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.DataItem != null)
                {
                    List<DailySchedule> sessionedDailySchedules = (List<DailySchedule>)GetSession(AppConstant.SESSION_DAILYSCHEDULES);

                    Repeater rptTeachingPeriodSchedule = (Repeater)e.Item.FindControl("RptTeachingPeriodSchedule");
                    DailySchedule dailySchedule = (DailySchedule)e.Item.DataItem;
                    Configuration_DayInWeek dayInWeek = new Configuration_DayInWeek();
                    dayInWeek.DayInWeekId = dailySchedule.DayInWeekId;

                    List<TeachingPeriodSchedule> teachingPeriodSchedules = null;
                    if (dataFromSession)
                    {
                        teachingPeriodSchedules = dailySchedule.SessionedSchedules[0].ListThoiKhoaBieuTheoTiet;
                    }
                    else
                    {
                        teachingPeriodSchedules = scheduleBL.GetTeachingPeriodSchedules(Class, term, dayInWeek);

                        for (int i = 0; i < sessionedDailySchedules.Count; i++)
                        {
                            if (sessionedDailySchedules[i].DayInWeekId == dailySchedule.DayInWeekId)
                            {
                                // update session
                                sessionedDailySchedules[i].SessionedSchedules[0].ListThoiKhoaBieuTheoTiet = teachingPeriodSchedules;
                                break;
                            }
                        }
                        AddSession(AppConstant.SESSION_DAILYSCHEDULES, sessionedDailySchedules);
                    }

                    rptTeachingPeriodSchedule.DataSource = teachingPeriodSchedules;
                    rptTeachingPeriodSchedule.DataBind();
                }
            }
        }

        protected void RptTeachingPeriodSchedule_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdChooseSubject":
                    {
                        BindRepeaterMonHoc();
                        ModalPopupExtender mPEChooseSubject = (ModalPopupExtender)e.Item.FindControl("MPEChooseSubject");
                        mPEChooseSubject.Show();
                        break;
                    }
                case "CmdChooseTeacher":
                    {
                        break;
                    }
                case "CmdDeleteItem":
                    {
                        //// Set confirm text and show dialog
                        //this.LblConfirmDelete.Text = "Bạn có chắc xóa thông tin này không?";

                        //ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        //mPEDelete.Show();

                        //// Save current ClassId to global
                        //HiddenField hdfSubjectIdTKB = (HiddenField)e.Item.FindControl("HdfSubjectIdTKB");

                        //this.HdfSubjectIdTKB.Value = hdfSubjectIdTKB.Value;

                        //// Save modal popup ClientID
                        //this.HdfRptThoiKhoaBieuMPEDelete.Value = mPEDelete.ClientID;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        #endregion

        #region Pager event handlers
        public void DataPagerMonHoc_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            DataPageMonHoc.CurrentIndex = currnetPageIndx;
            BindRepeaterMonHoc();
            // MPEMonHoc.Show();
        }

        public void DataPagerGiaoVien_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            DataPageGiaoVien.CurrentIndex = currnetPageIndx;
            BindRepeaterGiaoVien();
            // MPEGiaoVien.Show();
        }
        #endregion
    }
}