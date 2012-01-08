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
    public partial class SuaThoiKhoaBieuPage : BaseContentPage
    {
        #region Fields, Properties
        public int ClassId
        {
            get
            {
                int ClassId;
                if (Request.QueryString["lop"] != null)
                {
                    ClassId = Int32.Parse(Request.QueryString["lop"]);
                    ViewState["ClassId"] = ClassId;
                }
                else
                {
                    if (ViewState["ClassId"] != null)
                    {
                        ClassId = (int)ViewState["ClassId"];
                    }
                    else
                    {
                        ClassId = (int)Session[User.Identity.Name + "_Fr_ThemTiet_To_SuaTKB_MaLop"];
                        Session.Remove(User.Identity.Name + "_Fr_ThemTiet_To_SuaTKB_MaLop");
                    }
                }
                return ClassId;
            }
        }
        public int TermId
        {
            get
            {
                if (Request.QueryString["hocky"] != null)
                {
                    int TermId = Int32.Parse(Request.QueryString["hocky"]);
                    ViewState["TermId"] = TermId;
                    return TermId;
                }
                else
                {
                    if (ViewState["TermId"] != null)
                    {
                        return (int)ViewState["TermId"];
                    }
                    else
                    {
                        int TermId = (int)Session[User.Identity.Name + "_Fr_ThemTiet_To_SuaTKB_TermId"];
                        Session.Remove(User.Identity.Name + "_Fr_ThemTiet_To_SuaTKB_HocKy");
                        return TermId;
                    }
                }
            }
        }
        public int OriginalDayInWeekId
        {
            get
            {
                if (Request.QueryString["thu"] != null)
                {
                    int DayInWeekId = Int32.Parse(Request.QueryString["thu"]);
                    ViewState["DayInWeekId"] = DayInWeekId;
                    return DayInWeekId;
                }
                else
                {
                    if (ViewState["DayInWeekId"] != null)
                    {
                        return (int)ViewState["DayInWeekId"];
                    }
                    else
                    {
                        int DayInWeekId = (int)Session[User.Identity.Name + "_Fr_ThemTiet_To_SuaTKB_DayInWeekId"];
                        Session.Remove(User.Identity.Name + "_Fr_ThemTiet_To_SuaTKB_DayInWeekId");
                        return DayInWeekId;

                    }
                }
            }
        }
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

            if (!Page.IsPostBack)
            {
                FillDDLThu();
                FillTitlePage();

                BindRptThoiKhoaBieu();
            }
        }
        #endregion

        #region Methods
        private void FillTitlePage()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            ClassBL classBL = new ClassBL(UserSchool);
            Class_Class Class = new Class_Class();

            Class.ClassId = ClassId;

            TabularClass lopHoc = classBL.GetTabularClass(Class);
            Configuration_Term hocKy = systemConfigBL.GetTerm(TermId);

            LblTitle.Text = string.Format("THỜI KHÓA BIỂU LỚP {0} ({1} - {2} - NĂM HỌC {3})",
                lopHoc.ClassName, DdlThu.SelectedItem.Text, hocKy.TermName, lopHoc.YearName);
        }

        public void FillDDLThu()
        {
            SystemConfigBL cauHinhBL = new SystemConfigBL(UserSchool);
            List<Configuration_DayInWeek> listThu = cauHinhBL.GetDayInWeeks();
            DdlThu.DataSource = listThu;
            DdlThu.DataValueField = "DayInWeekId";
            DdlThu.DataTextField = "DayInWeekName";
            DdlThu.DataBind();

            //DdlThu.SelectedValue = OriginalDayInWeekId.ToString();
        }

        private void BindRptThoiKhoaBieu()
        {
            Class_Class Class = new Class_Class();
            Class.ClassId = ClassId;
            Configuration_Term term = new Configuration_Term();
            term.TermId = TermId;
            Configuration_DayInWeek dayInWeek = new Configuration_DayInWeek();
            dayInWeek.DayInWeekId = Int32.Parse(DdlThu.SelectedValue);

            List<TeachingPeriodSchedule> lTKBTheoTiets = scheduleBL.GetTeachingPeriodSchedules(Class, term, dayInWeek);
            MainDataPager.ItemCount = lTKBTheoTiets.Count;

            RptThoiKhoaBieu.DataSource = lTKBTheoTiets;
            RptThoiKhoaBieu.DataBind();

            if (lTKBTheoTiets.Count == 0)
            {
                PnlPopupConfirmDelete.Visible = false;
            }
            else
            {
                PnlPopupConfirmDelete.Visible = true;
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            FillTitlePage();
            MainDataPager.CurrentIndex = 1;
            BindRptThoiKhoaBieu();
        }

        protected void BtnBackPrevPage_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/modules/lop_hoc/thoikhoabieu.aspx");
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            Class_Schedule schedule = new Class_Schedule();
            schedule.ScheduleId = Int32.Parse(this.HdfSubjectIdTKB.Value);
            scheduleBL.DeleteSchedule(schedule);
            BindRptThoiKhoaBieu();
        }
        #endregion

        #region Repeater event handlers
        protected void RptThoiKhoaBieu_ItemDataBound(object sender, RepeaterItemEventArgs e)
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

        protected void RptThoiKhoaBieu_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdAddItem":
                    {
                        SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
                        TeachingPeriodBL tietBL = new TeachingPeriodBL(UserSchool);
                        ClassBL classBL = new ClassBL(UserSchool);
                        Class_Class Class = new Class_Class();
                        Class.ClassId = ClassId;

                        int TeachingPeriodId = Int32.Parse(e.CommandArgument.ToString());
                        ViewState["TeachingPeriodId_Add"] = TeachingPeriodId;

                        TabularClass lopHoc = classBL.GetTabularClass(Class);
                        Configuration_Term hocKy = systemConfigBL.GetTerm(TermId);

                        Response.Redirect(string.Format("themtietthoikhoabieu.aspx?lop={0}&hocky={1}&thu={2}&tiet={3}",
                            ClassId, TermId, DdlThu.SelectedValue, TeachingPeriodId));
                        break;
                    }
                case "CmdDeleteItem":
                    {
                        // Set confirm text and show dialog
                        this.LblConfirmDelete.Text = "Bạn có chắc xóa thông tin này không?";

                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        // Save current ClassId to global
                        HiddenField hdfSubjectIdTKB = (HiddenField)e.Item.FindControl("HdfSubjectIdTKB");

                        this.HdfSubjectIdTKB.Value = hdfSubjectIdTKB.Value;

                        // Save modal popup ClientID
                        this.HdfRptThoiKhoaBieuMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        int SubjectIdTKB = Int32.Parse(e.CommandArgument.ToString());
                        Class_Schedule schedule = scheduleBL.GetSchedule(SubjectIdTKB);
                        TeachingPeriodSchedule tKBTheoTiet = scheduleBL.GetTeachingPeriodSchedule(schedule);
                        int TeachingPeriodId = tKBTheoTiet.TeachingPeriodId;
                        int SubjectId = tKBTheoTiet.SubjectId;
                        ViewState["SubjectIdTKB_Edit"] = SubjectIdTKB;
                        ViewState["SubjectId_Edit"] = SubjectId;

                        Response.Redirect(string.Format("suatietthoikhoabieu.aspx?id={0}&lop={1}&hocky={2}&thu={3}&tiet={4}",
                            SubjectIdTKB, ClassId, TermId, DdlThu.SelectedValue, TeachingPeriodId));
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