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
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            Class_Class Class = null;

            if (!Page.IsPostBack)
            {
                Dictionary<string, int> dicQueryStrings = GetQueryStrings();
                if (dicQueryStrings != null)
                {
                    int SubjectIdTKB = dicQueryStrings["SubjectIdTKB"];
                    Class_Schedule schedule = scheduleBL.GetSchedule(SubjectIdTKB);
                    TeachingPeriodSchedule tkbTheoTiet = scheduleBL.GetTeachingPeriodSchedule(schedule);
                    HdfSubjectId.Value = tkbTheoTiet.SubjectId.ToString();
                    LblMonHoc.Text = tkbTheoTiet.SubjectName;
                    HdfUserId.Value = tkbTheoTiet.UserId.ToString();
                    LblGiaoVien.Text = tkbTheoTiet.TeacherName;

                    int ClassId = dicQueryStrings["MaLop"];
                    int TermId = dicQueryStrings["TermId"];
                    int DayInWeekId = dicQueryStrings["DayInWeekId"];
                    int TeachingPeriodId = dicQueryStrings["TeachingPeriodId"];


                    Class = new Class_Class();
                    Class.ClassId = ClassId;
                    TabularClass lopHoc = (new ClassBL(UserSchool)).GetTabularClass(Class);
                    Configuration_Term hocKy = systemConfigBL.GetTerm(TermId);
                    Configuration_DayInWeek dayInWeek = systemConfigBL.GetDayInWeek(DayInWeekId);
                    Category_TeachingPeriod tiet = (new TeachingPeriodBL(UserSchool)).GetTeachingPeriod(TeachingPeriodId);
                    LblTenLop.Text = lopHoc.ClassName;
                    LblNamHoc.Text = lopHoc.YearName;
                    LblHocKy.Text = hocKy.TermName;
                    LblThu.Text = dayInWeek.DayInWeekName;
                    LblTiet.Text = string.Format("{0} ({1} - {2})",
                        tiet.TeachingPeriodName,
                        tiet.BeginTime.ToShortTimeString(),
                        tiet.EndTime.ToShortTimeString());

                    FillDDLNganh();
                    FillDDLKhoi();
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
            ScheduleBL scheduleBL = new ScheduleBL(UserSchool);
            Class_Schedule schedule = null;
            Category_Subject subject = null;
            aspnet_User teacher = null;

            if (!validateInput())
            {
                return;
            }
            
            Dictionary<string, int> dicQueryStrings = GetQueryStrings();
            if (dicQueryStrings != null)
            {
                int maTKBTiet = dicQueryStrings["SubjectIdTKB"];
                int ClassId = dicQueryStrings["MaLop"];
                int TermId = dicQueryStrings["TermId"];
                int DayInWeekId = dicQueryStrings["DayInWeekId"];
                int TeachingPeriodId = dicQueryStrings["TeachingPeriodId"];
                int SubjectId = Int32.Parse(HdfSubjectId.Value);
                Guid UserId = new Guid(HdfUserId.Value);

                schedule = new Class_Schedule();
                schedule.ScheduleId = maTKBTiet;
                subject = new Category_Subject();
                subject.SubjectId = SubjectId;
                teacher = new aspnet_User();
                teacher.UserId = UserId;
                scheduleBL.UpdateSchedule(schedule, subject, teacher);

                Response.Redirect(string.Format("suathoikhoabieu.aspx?lop={0}&hocky={1}&thu={2}",
                    Request.QueryString["lop"], Request.QueryString["hocky"], Request.QueryString["thu"]));
            }
        }

        protected void BtnCancelEdit_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(string.Format("suathoikhoabieu.aspx?lop={0}&hocky={1}&thu={2}",
                Request.QueryString["lop"], Request.QueryString["hocky"], Request.QueryString["thu"]));
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
        private Dictionary<string, int> GetQueryStrings()
        {
            Dictionary<string, int> dicQueryStrings = new Dictionary<string, int>();

            if (Request.QueryString["id"] != null && Request.QueryString["lop"] != null 
                && Request.QueryString["hocky"] != null && Request.QueryString["thu"] != null 
                && Request.QueryString["tiet"] != null)
            {
                int SubjectIdTKB;
                if (Int32.TryParse(Request.QueryString["id"], out SubjectIdTKB))
                {
                    dicQueryStrings.Add("SubjectIdTKB", SubjectIdTKB);
                }
                else
                {
                    return null;
                }

                int maLop;
                if (Int32.TryParse(Request.QueryString["lop"], out maLop))
                {
                    dicQueryStrings.Add("MaLop", maLop);
                }
                else
                {
                    return null;
                }

                int TermId;
                if (Int32.TryParse(Request.QueryString["hocky"], out TermId))
                {
                    dicQueryStrings.Add("TermId", TermId);
                }
                else
                {
                    return null;
                }

                int DayInWeekId;
                if (Int32.TryParse(Request.QueryString["thu"], out DayInWeekId))
                {
                    dicQueryStrings.Add("DayInWeekId", DayInWeekId);
                }
                else
                {
                    return null;
                }

                int TeachingPeriodId;
                if (Int32.TryParse(Request.QueryString["tiet"], out TeachingPeriodId))
                {
                    dicQueryStrings.Add("TeachingPeriodId", TeachingPeriodId);
                }
                else
                {
                    return null;
                }
            }

            return dicQueryStrings;
        }

        private void FillDDLKhoi()
        {
            GradeBL grades = new GradeBL(UserSchool);
            List<Category_Grade> lstKhoiLop = grades.GetListGrades();

            DdlKhoi.DataSource = lstKhoiLop;
            DdlKhoi.DataValueField = "GradeName";
            DdlKhoi.DataTextField = "GradeName";
            DdlKhoi.DataBind();
        }

        private void FillDDLNganh()
        {
            FacultyBL facultyBL = new FacultyBL(UserSchool);
            List<Category_Faculty> lstNganhs = facultyBL.GetFaculties();

            DdlNganh.DataSource = lstNganhs;
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

        private bool validateInput()
        {
            bool bValid;

            if (HdfSubjectId.Value == "0" || HdfUserId.Value == "0")
            {
                bValid = false;
            }
            else
            {
                bValid = true;
            }

            if (HdfSubjectId.Value == "0")
            {
                LblMonHocError.Visible = true;
            }
            else
            {
                LblMonHocError.Visible = false;
            }

            if (HdfUserId.Value == "0")
            {
                LblGiaoVienError.Visible = true;
            }
            else
            {
                LblGiaoVienError.Visible = false;
            }

            return bValid;
        }
        #endregion
    }
}