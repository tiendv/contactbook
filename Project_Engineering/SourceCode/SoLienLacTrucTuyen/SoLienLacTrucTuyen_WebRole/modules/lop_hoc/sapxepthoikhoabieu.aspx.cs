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
        #region Field(s)
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

            // Init variables            
            scheduleBL = new ScheduleBL(UserSchool);

            TeachingPeriodBL teachingPeriodBL = new TeachingPeriodBL(UserSchool);
            

            if (!Page.IsPostBack)
            {
                if (RetrieveSessions())
                {
                    if (teachingPeriodBL.GetTeachingPeriodCount() == 0)
                    {
                        PnlLinkToCategory.Visible = true;
                        PnlSchedule.Visible = false;
                    }
                    else
                    {
                        FillPageTitle();
                        BindRptSchedule();
                    }
                }
                else
                {
                    Response.Redirect(AppConstant.PAGEPATH_SCHEDULE);
                }
            }
        }
        #endregion

        #region Methods
        private void FillPageTitle()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            ClassBL classBL = new ClassBL(UserSchool);

            Class_Class Class = classBL.GetClass((int)ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID]);
            Configuration_Term term = systemConfigBL.GetTerm((int)ViewState[AppConstant.VIEWSTATE_SELECTED_TERMID]);

            LblTitle.Text = string.Format("THỜI KHÓA BIỂU LỚP {0} ({1} - NĂM HỌC {2})",
                Class.ClassName, term.TermName, Class.Configuration_Year.YearName);
        }

        private void BindRptSchedule()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<Configuration_DayInWeek> dayInWeeks = systemConfigBL.GetDayInWeeks();
            RptSchedule.DataSource = dayInWeeks;
            RptSchedule.DataBind();
        }

        private bool RetrieveSessions()
        {
            if (CheckSessionKey(AppConstant.SESSION_SCHEDULE_EVENT_ADD) == false
                && CheckSessionKey(AppConstant.SESSION_SCHEDULE_EVENT_MODIFY) == false)
            {
                RemoveSession(AppConstant.SESSION_WEEKLYSCHEDULE);
            }
            else
            {
                RemoveSession(AppConstant.SESSION_SCHEDULE_EVENT_ADD);
                RemoveSession(AppConstant.SESSION_SCHEDULE_EVENT_MODIFY);
            }

            if (CheckSessionKey(AppConstant.SESSION_SELECTED_CLASS) && CheckSessionKey(AppConstant.SESSION_SELECTED_TERM))
            {
                Class_Class Class = (Class_Class)GetSession(AppConstant.SESSION_SELECTED_CLASS);
                RemoveSession(AppConstant.SESSION_SELECTED_CLASS);
                ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID] = Class.ClassId;

                Configuration_Term term = (Configuration_Term)GetSession(AppConstant.SESSION_SELECTED_TERM);
                RemoveSession(AppConstant.SESSION_SELECTED_TERM);
                ViewState[AppConstant.VIEWSTATE_SELECTED_TERMID] = term.TermId;

                return true;
            }
            else if (ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID] != null && ViewState[AppConstant.SESSION_SELECTED_TERM] != null)
            {
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
            AddSession(AppConstant.SESSION_SELECTED_TERM, term);
            
            Response.Redirect(AppConstant.PAGEPATH_SCHEDULE);
        }
        #endregion

        #region Button event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            FillPageTitle();
            BindRptSchedule();
        }

        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (CheckSessionKey(AppConstant.SESSION_WEEKLYSCHEDULE))
            {
                List<List<TeachingPeriodSchedule>> weeklySchedule = (List<List<TeachingPeriodSchedule>>)GetSession(AppConstant.SESSION_WEEKLYSCHEDULE);
                scheduleBL.UpdateSchedule(weeklySchedule);
            }
            BackToPrevPage();
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            BackToPrevPage();
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            Class_Schedule schedule = new Class_Schedule();

            if (CheckSessionKey(AppConstant.SESSION_WEEKLYSCHEDULE))
            {
                List<List<TeachingPeriodSchedule>> listOfTeachingPeriodSchedules = (List<List<TeachingPeriodSchedule>>)GetSession(AppConstant.SESSION_WEEKLYSCHEDULE);
                for (int i = 0; i < listOfTeachingPeriodSchedules.Count; i++)
                {
                    for (int j = 0; j < listOfTeachingPeriodSchedules[i].Count; j++)
                    {
                        if (listOfTeachingPeriodSchedules[i][j].ScheduleId == Int32.Parse(this.HdfSelectedScheduleId.Value))
                        {
                            listOfTeachingPeriodSchedules[i][j].SubjectId = 0;
                            listOfTeachingPeriodSchedules[i][j].SubjectName = "Chưa xác định";
                            listOfTeachingPeriodSchedules[i][j].UserId = new Guid();
                            listOfTeachingPeriodSchedules[i][j].TeacherName = "Chưa xác định";

                            AddSession(AppConstant.SESSION_WEEKLYSCHEDULE, listOfTeachingPeriodSchedules);
                            BindRptSchedule();
                        }
                    }
                }
            }
        }
        #endregion

        #region Repeater event handlers
        protected void RptSchedule_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rptDailySchedule = (Repeater)e.Item.FindControl("RptDailySchedule");
                HiddenField hdfDayInWeekId = (HiddenField)e.Item.FindControl("HdfDayInWeekId");
                Class_Class Class = new Class_Class();
                Class.ClassId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID];
                Configuration_Term term = new Configuration_Term();
                term.TermId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_TERMID];
                Configuration_DayInWeek dayInWeek = new Configuration_DayInWeek();
                dayInWeek.DayInWeekId = Int32.Parse(hdfDayInWeekId.Value);
                List<TeachingPeriodSchedule> teachingPeriodSchedules = new List<TeachingPeriodSchedule>();
                List<List<TeachingPeriodSchedule>> weeklySchedule = null;
                if (CheckSessionKey(AppConstant.SESSION_WEEKLYSCHEDULE))
                {
                    weeklySchedule = (List<List<TeachingPeriodSchedule>>)GetSession(AppConstant.SESSION_WEEKLYSCHEDULE);
                    foreach (List<TeachingPeriodSchedule> dailySchedules in weeklySchedule)
                    {
                        if (dailySchedules[0].DayInWeekId == dayInWeek.DayInWeekId)
                        {
                            teachingPeriodSchedules = dailySchedules;
                            rptDailySchedule.DataSource = teachingPeriodSchedules;
                            rptDailySchedule.DataBind();

                            if (teachingPeriodSchedules.Count == 0)
                            {
                                PnlPopupConfirmDelete.Visible = false;
                            }
                            else
                            {
                                PnlPopupConfirmDelete.Visible = true;
                            }

                            AddSession(AppConstant.SESSION_WEEKLYSCHEDULE, weeklySchedule);
                            return;
                        }
                    }
                }

                // if it's the first time page requested, get data from database
                teachingPeriodSchedules = scheduleBL.GetTeachingPeriodSchedules(Class, term, dayInWeek);
                rptDailySchedule.DataSource = teachingPeriodSchedules;
                rptDailySchedule.DataBind();

                if (teachingPeriodSchedules.Count == 0)
                {
                    PnlPopupConfirmDelete.Visible = false;
                }
                else
                {
                    PnlPopupConfirmDelete.Visible = true;
                }

                if (!CheckSessionKey(AppConstant.SESSION_WEEKLYSCHEDULE))
                {
                    weeklySchedule = new List<List<TeachingPeriodSchedule>>();
                }

                weeklySchedule.Add(teachingPeriodSchedules);
                AddSession(AppConstant.SESSION_WEEKLYSCHEDULE, weeklySchedule);
            }
        }

        protected void RptDailySchedule_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item
                || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.DataItem != null)
                {
                    TeachingPeriodSchedule tkbTheoTiet = (TeachingPeriodSchedule)e.Item.DataItem;
                    if (tkbTheoTiet.SubjectId == 0)
                    {
                        ImageButton btnDeleteItem = (ImageButton)e.Item.FindControl("BtnDeleteItem");
                        btnDeleteItem.ImageUrl = "~/Styles/Icons/icon_delete_disabled.png";
                        btnDeleteItem.Enabled = false;

                        ImageButton btnEditItem = (ImageButton)e.Item.FindControl("BtnEditItem");
                        btnEditItem.ImageUrl = "~/Styles/Icons/icon_Edit_disabled.png";
                        btnEditItem.Enabled = false;
                    }
                    else
                    {
                        ImageButton btnAddItem = (ImageButton)e.Item.FindControl("BtnAddItem");
                        btnAddItem.ImageUrl = "~/Styles/Icons/icon_Add_disabled.png";
                        btnAddItem.Enabled = false;
                    }
                }
            }
        }

        protected void RptDailySchedule_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdAddItem":
                    {
                        // Class
                        Class_Class Class = new Class_Class();
                        Class.ClassId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID];

                        // term
                        Configuration_Term term = new Configuration_Term();
                        term.TermId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_TERMID];

                        // teaching Period
                        Category_TeachingPeriod teachingPeriod = new Category_TeachingPeriod();
                        teachingPeriod.TeachingPeriodId = Int32.Parse(e.CommandArgument.ToString());

                        // dayInWeek
                        HiddenField hdfDayInWeekId = (HiddenField)e.Item.FindControl("HdfDayInWeekId");
                        Configuration_DayInWeek dayInWeek = new Configuration_DayInWeek();
                        dayInWeek.DayInWeekId = Int32.Parse(hdfDayInWeekId.Value);

                        AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);
                        AddSession(AppConstant.SESSION_SELECTED_TERM, term);
                        AddSession(AppConstant.SESSION_SELECTED_DAYINWEEK, dayInWeek);
                        AddSession(AppConstant.SESSION_SELECTED_TEACHINGPERIOD, teachingPeriod);

                        Response.Redirect(AppConstant.PAGEPATH_SCHEDULE_ADD);
                        break;
                    }
                case "CmdDeleteItem":
                    {
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        // Save current ClassId to global
                        HdfSelectedScheduleId.Value = ((HiddenField)e.Item.FindControl("HdfRptScheduleId")).Value;

                        // Save modal popup ClientID
                        this.HdfMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        // Class
                        Class_Class Class = new Class_Class();
                        Class.ClassId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID];

                        // term
                        Configuration_Term term = new Configuration_Term();
                        term.TermId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_TERMID];

                        AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);
                        AddSession(AppConstant.SESSION_SELECTED_TERM, term);

                        TeachingPeriodSchedule schedule = null;
                        bool bContinue = true;
                        List<List<TeachingPeriodSchedule>> weeklySchedule = (List<List<TeachingPeriodSchedule>>)GetSession(AppConstant.SESSION_WEEKLYSCHEDULE);
                        foreach (List<TeachingPeriodSchedule> dailySchedule in weeklySchedule)
                        {
                            foreach (TeachingPeriodSchedule teachingPeriodSchedule in dailySchedule)
                            {
                                if (teachingPeriodSchedule.ScheduleId == Int32.Parse(e.CommandArgument.ToString()))
                                {
                                    schedule = teachingPeriodSchedule;
                                    bContinue = false;
                                    break;
                                }
                            }

                            if (bContinue)
                            {
                                break;
                            }
                        }

                        AddSession(AppConstant.SESSION_SCHEDULE, schedule);

                        Response.Redirect(AppConstant.PAGEPATH_SCHEDULE_MODIFY);
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