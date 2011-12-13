using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.BusinessEntity;
using EContactBook.DataAccess;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class ThemTietThoiKhoaBieuPage : BaseContentPage
    {
        #region Fields
        private ScheduleBL thoiKhoaBieuBL;
        private int ClassId;
        private int TermId;
        private int DayInWeekId;
        private int TeachingPeriodId;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);
            if (accessDenied)
            {
                return;
            }

            Class_Class Class = null;
            thoiKhoaBieuBL = new ScheduleBL(UserSchool);
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);

            Dictionary<string, int> dicQueryStrings = GetQueryStrings();
            if (dicQueryStrings != null)
            {
                ClassId = dicQueryStrings["MaLop"];
                TermId = dicQueryStrings["TermId"];
                DayInWeekId = dicQueryStrings["DayInWeekId"];
                TeachingPeriodId = dicQueryStrings["TeachingPeriodId"];
            }

            if (!Page.IsPostBack)
            {
                if (dicQueryStrings != null)
                {
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
                            break;
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
                            break;
                        }
                    }
                }
            }
        }

        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {
            Category_Subject subject = null;
            aspnet_User teacher = null;
            Category_TeachingPeriod teachingPeriod = null;
            Class_Class Class = null;
            Configuration_Term term = null;
            Configuration_DayInWeek dayInWeek = null;

            if (!ValidateInput())
            {
                return;
            }

            ScheduleBL thoiKhoaBieuBL = new ScheduleBL(UserSchool);
            Dictionary<string, int> dicQueryStrings = GetQueryStrings();
            if (dicQueryStrings != null)
            {
                Class = new Class_Class();
                Class.ClassId = dicQueryStrings["MaLop"];
                term = new Configuration_Term();
                term.TermId = dicQueryStrings["TermId"];
                dayInWeek = new Configuration_DayInWeek();
                dayInWeek.DayInWeekId = dicQueryStrings["DayInWeekId"];
                teachingPeriod = (new TeachingPeriodBL(UserSchool)).GetTeachingPeriod(dicQueryStrings["TeachingPeriodId"]);
                subject = new Category_Subject();
                subject.SubjectId = Int32.Parse(HdfSubjectId.Value);
                teacher = new aspnet_User();
                teacher.UserId = new Guid(HdfUserId.Value);

                thoiKhoaBieuBL.InsertSchedule(Class, subject, teacher, term, dayInWeek, teachingPeriod);

                Response.Redirect(string.Format("suathoikhoabieu.aspx?lop={0}&hocky={1}&thu={2}",
                    Request.QueryString["lop"], Request.QueryString["hocky"], Request.QueryString["thu"]));
            }
        }

        protected void BtnCancelAdd_Click(object sender, ImageClickEventArgs e)
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

            if (Request.QueryString["lop"] != null && Request.QueryString["hocky"] != null
                && Request.QueryString["thu"] != null && Request.QueryString["tiet"] != null)
            {
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

        private bool ValidateInput()
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
                LblGiaoVienError.Text = "Chưa chọn giáo viên";
                LblGiaoVienError.Visible = true;
            }
            else
            {
                LblGiaoVienError.Visible = false;

                if (HdfUserId.Value != "0")
                {
                    Guid UserId = new Guid(HdfUserId.Value);
                    TeacherBL giaoVienBL = new TeacherBL(UserSchool);
                    aspnet_User teacher = new aspnet_User();
                    teacher.UserId = UserId;
                    Configuration_Term term = new Configuration_Term();
                    term.TermId = TermId;
                    Configuration_DayInWeek dayInWeek = new Configuration_DayInWeek();
                    dayInWeek.DayInWeekId = DayInWeekId;
                    Category_TeachingPeriod teachingPeriod = new Category_TeachingPeriod();
                    teachingPeriod.TeachingPeriodId = TeachingPeriodId;
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
        #endregion
    }
}