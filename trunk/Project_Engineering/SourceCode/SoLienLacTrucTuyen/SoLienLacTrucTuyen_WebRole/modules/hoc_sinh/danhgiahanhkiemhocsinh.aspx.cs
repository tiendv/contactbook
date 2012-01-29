using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EContactBook.BusinessEntity;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class StudentConductModifyPage : BaseContentPage
    {
        #region Fields
        private StudyingResultBL ketQuaHocTapBL;
        private ConductBL hanhKiemBL;
        private StudentBL hocSinhBL;
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

            hocSinhBL = new StudentBL(UserSchool);
            ketQuaHocTapBL = new StudyingResultBL(UserSchool);
            hanhKiemBL = new ConductBL(UserSchool);

            if (!Page.IsPostBack)
            {
                if (RetrieveSessions())
                {
                    BindRptHanhKiemHocSinh();
                }
                else
                {
                    Response.Redirect(AppConstant.PAGEPATH_STUDENT_CONDUCT_LIST);
                }
            }
        }
        #endregion

        #region Methods
        private void BindRptHanhKiemHocSinh()
        {
            int ClassId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID];
            int TermId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_TERMID];

            double dTotalRecords = 0;
            List<TabularStudentConduct> lstTbHanhKiemHocSinh;
            lstTbHanhKiemHocSinh = hocSinhBL.GetListHanhKiemHocSinh(ClassId, TermId, MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            this.RptHanhKiemHocSinh.DataSource = lstTbHanhKiemHocSinh;
            this.RptHanhKiemHocSinh.DataBind();
            MainDataPager.ItemCount = dTotalRecords;
        }

        private bool RetrieveSessions()
        {
            if (CheckSessionKey(AppConstant.SESSION_SELECTED_YEAR)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_TERM)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_FACULTY)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_GRADE)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_CLASS))
            {
                Configuration_Year year = (Configuration_Year)GetSession(AppConstant.SESSION_SELECTED_YEAR);
                RemoveSession(AppConstant.SESSION_SELECTED_YEAR);
                ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID] = year.YearId;
                LblYearName.Text = year.YearName;

                Configuration_Term term = (Configuration_Term)GetSession(AppConstant.SESSION_SELECTED_TERM);
                RemoveSession(AppConstant.SESSION_SELECTED_TERM);
                ViewState[AppConstant.VIEWSTATE_SELECTED_TERMID] = term.TermId;
                LblTermName.Text = term.TermName;

                Category_Faculty faculty = (Category_Faculty)GetSession(AppConstant.SESSION_SELECTED_FACULTY);
                RemoveSession(AppConstant.SESSION_SELECTED_FACULTY);
                ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTYID] = faculty.FacultyId;

                Category_Grade grade = (Category_Grade)GetSession(AppConstant.SESSION_SELECTED_GRADE);
                RemoveSession(AppConstant.SESSION_SELECTED_GRADE);
                ViewState[AppConstant.VIEWSTATE_SELECTED_GRADEID] = grade.GradeId;

                Class_Class Class = (Class_Class)GetSession(AppConstant.SESSION_SELECTED_CLASS);
                RemoveSession(AppConstant.SESSION_SELECTED_CLASS);
                ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID] = Class.ClassId;
                LblClassName.Text = Class.ClassName;

                return true;
            }

            return false;
        }

        private void BackToPreviousPage()
        {
            Configuration_Year year = new Configuration_Year();
            year.YearId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID];

            Category_Faculty faculty = new Category_Faculty();
            faculty.FacultyId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTYID];

            Category_Grade grade = new Category_Grade();
            grade.GradeId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_GRADEID];

            Class_Class Class = new Class_Class();
            Class.ClassId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID];

            Configuration_Term term = new Configuration_Term();
            term.TermId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_TERMID];

            AddSession(AppConstant.SESSION_SELECTED_YEAR, year);
            AddSession(AppConstant.SESSION_SELECTED_TERM, term);
            AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);
            AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);
            AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);

            Response.Redirect(AppConstant.PAGEPATH_STUDENT_CONDUCT_LIST);
        }
        #endregion

        #region Button event handlers
        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            Class_Class Class = null;
            Student_Student student = null;
            Configuration_Term term = null;
            Category_Conduct conduct = null;

            Dictionary<int, int?> dicHocSinhHanhKiem = new Dictionary<int, int?>();
            foreach (RepeaterItem rptItem in RptHanhKiemHocSinh.Items)
            {
                HiddenField hdfMaHocSinh = (HiddenField)rptItem.FindControl("HdfMaHocSinh");
                HiddenField hdfConductIdHocSinh = (HiddenField)rptItem.FindControl("HdfConductIdHocSinh");
                int? orgConductIdHocSinh = null;
                try
                {
                    orgConductIdHocSinh = Int32.Parse(hdfConductIdHocSinh.Value);
                }
                catch (Exception ex) { }
                Repeater rptHanhKiem = (Repeater)rptItem.FindControl("RptHanhKiem");

                foreach (RepeaterItem item in rptHanhKiem.Items)
                {
                    RadioButton rbtnHanhKiem = (RadioButton)item.FindControl("RbtnHanhKiem");
                    if (rbtnHanhKiem.Checked)
                    {
                        HiddenField selectedHdfConductId = (HiddenField)item.FindControl("HdfConductId");
                        if (((orgConductIdHocSinh == null) && (selectedHdfConductId.Value != "0"))
                        || ((orgConductIdHocSinh != null) && (selectedHdfConductId.Value != orgConductIdHocSinh.ToString())))
                        {
                            int? iSelectedConductId = null;
                            if (selectedHdfConductId.Value != "0")
                            {
                                iSelectedConductId = Int32.Parse(selectedHdfConductId.Value);
                            }
                            int maHocSinh = Int32.Parse(hdfMaHocSinh.Value);
                            dicHocSinhHanhKiem.Add(maHocSinh, iSelectedConductId);
                        }
                    }
                }
            }

            term = new Configuration_Term();
            term.TermId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_TERMID];

            Class = new Class_Class();
            Class.ClassId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_CLASSID];

            foreach (KeyValuePair<int, int?> pair in dicHocSinhHanhKiem)
            {
                student = new Student_Student();
                student.StudentId = pair.Key;

                if (pair.Value != null)
                {
                    conduct = new Category_Conduct();
                    conduct.ConductId = (int)pair.Value;
                }
                else
                {
                    conduct = null;
                }

                hocSinhBL.UpdateStudenTermConduct(Class, term, student, conduct);
            }

            BackToPreviousPage();
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            BackToPreviousPage();
        }
        #endregion

        #region Repeater event handlers
        int? ConductId;
        protected void RptHanhKiemHocSinh_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem rptItem = e.Item;
            if (rptItem.ItemType == ListItemType.Item
                || rptItem.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdfConductIdHocSinh = (HiddenField)rptItem.FindControl("HdfConductIdHocSinh");
                int? ConductIdHocSinh = null;
                try
                {
                    ConductIdHocSinh = Int32.Parse(hdfConductIdHocSinh.Value);
                }
                catch (Exception ex) { }
                ConductId = ConductIdHocSinh;

                Repeater RptHanhKiem = (Repeater)e.Item.FindControl("RptHanhKiem");
                List<Category_Conduct> lstHanhKiem = hanhKiemBL.GetListConducts(true);
                RptHanhKiem.DataSource = lstHanhKiem;
                RptHanhKiem.DataBind();
            }
        }

        protected void RptHanhKiem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem rptItem = e.Item;
            if (rptItem.ItemType == ListItemType.Item
                || rptItem.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdfConductId = (HiddenField)rptItem.FindControl("HdfConductId");
                RadioButton rbtnHanhKiem = (RadioButton)rptItem.FindControl("RbtnHanhKiem");
                if (((ConductId == null) && (hdfConductId.Value == "0"))
                    || ((ConductId != null) && (hdfConductId.Value == ConductId.ToString())))
                {
                    rbtnHanhKiem.Checked = true;
                }
            }
        }
        #endregion

        #region Pager event handlers
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currnetPageIndx;
            BindRptHanhKiemHocSinh();
        }
        #endregion
    }
}