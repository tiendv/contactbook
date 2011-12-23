using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessEntity;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class HanhKiemHocSinhPage : BaseContentPage
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
                BindDropDownLists();
                BindRptHanhKiemHocSinh();
            }
        }
        #endregion

        #region DropDownList event hanlders
        protected void DdlNamHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLLopHoc();
        }

        protected void DdlNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLLopHoc();
        }

        protected void DdlKhoiLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLLopHoc();
        }
        #endregion

        #region Methods
        private void BindDropDownLists()
        {
            BindDDLNamHoc();
            BindDDLHocKy();
            BindDDLNganhHoc();
            BindDDLKhoiLop();
            BindDDLLopHoc();
        }

        private void BindDDLNamHoc()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<Configuration_Year> years = systemConfigBL.GetListYears();
            DdlNamHoc.DataSource = years;
            DdlNamHoc.DataValueField = "YearId";
            DdlNamHoc.DataTextField = "YearName";
            DdlNamHoc.DataBind();

            if (DdlNamHoc.Items.Count != 0)
            {
                SystemConfigBL cauHinhBL = new SystemConfigBL(UserSchool);
                DdlNamHoc.SelectedValue = cauHinhBL.GetLastedYear().ToString();
            }
        }

        private void BindDDLHocKy()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<Configuration_Term> lstHocKy = systemConfigBL.GetListTerms();
            DdlHocKy.DataSource = lstHocKy;
            DdlHocKy.DataValueField = "TermId";
            DdlHocKy.DataTextField = "TermName";
            DdlHocKy.DataBind();
        }

        private void BindDDLNganhHoc()
        {
            FacultyBL facultyBL = new FacultyBL(UserSchool);
            List<Category_Faculty> faculties = facultyBL.GetFaculties();
            DdlNganh.DataSource = faculties;
            DdlNganh.DataValueField = "FacultyId";
            DdlNganh.DataTextField = "FacultyName";
            DdlNganh.DataBind();
            if (faculties.Count > 1)
            {
                DdlNganh.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }

        private void BindDDLKhoiLop()
        {
            GradeBL grades = new GradeBL(UserSchool);
            List<Category_Grade> lstKhoiLop = grades.GetListGrades();
            DdlKhoiLop.DataSource = lstKhoiLop;
            DdlKhoiLop.DataValueField = "GradeId";
            DdlKhoiLop.DataTextField = "GradeName";
            DdlKhoiLop.DataBind();
            if (lstKhoiLop.Count > 1)
            {
                DdlKhoiLop.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }

        private void BindDDLLopHoc()
        {
            Configuration_Year year = null;
            Category_Faculty faculty = null;
            Category_Grade grade = null;

            if (DdlNamHoc.Items.Count == 0 || DdlNganh.Items.Count == 0 || DdlKhoiLop.Items.Count == 0)
            {
                //BtnSearch.ImageUrl = "~/Styles/Images/button_search_with_text_disable.png";
                //BtnSearch.Enabled = false;

                //BtnAdd.ImageUrl = "~/Styles/Images/button_add_with_text_disable.png";
                //BtnAdd.Enabled = false;

                //PnlPopupConfirmDelete.Visible = false;
                //RptHocSinh.Visible = false;
                //LblSearchResult.Visible = true;
                //LblSearchResult.Text = "Chưa có thông tin HocSinh";

                //MainDataPager.ItemCount = 0;
                //MainDataPager.Visible = false;

                return;
            }

            year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);

            try
            {
                if (DdlNganh.SelectedIndex > 0)
                {
                    faculty = new Category_Faculty();
                    faculty.FacultyId = Int32.Parse(DdlNganh.SelectedValue);
                }
            }
            catch (Exception) { }

            try
            {
                if (DdlKhoiLop.SelectedIndex > 0)
                {
                    grade = new Category_Grade();
                    grade.GradeId = Int32.Parse(DdlKhoiLop.SelectedValue);
                }
            }
            catch (Exception) { }

            ClassBL lopHocBL = new ClassBL(UserSchool);
            List<Class_Class> lstLop = lopHocBL.GetListClasses(year, faculty, grade);
            DdlLopHoc.DataSource = lstLop;
            DdlLopHoc.DataValueField = "ClassId";
            DdlLopHoc.DataTextField = "ClassName";
            DdlLopHoc.DataBind();
        }

        private void BindRptHanhKiemHocSinh()
        {
            if (DdlLopHoc.Items.Count == 0)
            {
                ProcDisplayGUI(false);
                return;
            }

            int ClassId = Int32.Parse(DdlLopHoc.SelectedValue);
            int TermId = Int32.Parse(DdlHocKy.SelectedValue);
            //int ConductId = Int32.Parse(DdlHanhKiem.SelectedValue);

            double dTotalRecords = 0;
            List<TabularHanhKiemHocSinh> lstTbHanhKiemHocSinh;
            lstTbHanhKiemHocSinh = hocSinhBL.GetListHanhKiemHocSinh(
                ClassId, TermId,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            this.RptHanhKiemHocSinh.DataSource = lstTbHanhKiemHocSinh;
            this.RptHanhKiemHocSinh.DataBind();
            MainDataPager.ItemCount = dTotalRecords;

            bool bDisplayData = (lstTbHanhKiemHocSinh.Count != 0) ? true : false;
            ProcDisplayGUI(bDisplayData);
        }

        private void ProcDisplayGUI(bool bDisplayData)
        {
            RptHanhKiemHocSinh.Visible = bDisplayData;
            MainDataPager.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            tdSTT.Visible = bDisplayData;
            tdMaHocSinh.Visible = bDisplayData;
            tdHoTenHocSinh.Visible = bDisplayData;
            tdDTB.Visible = bDisplayData;

            BtnSave.Visible = bDisplayData;
            BtnCancel.Visible = bDisplayData;
        }
        #endregion

        #region Button event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            BindRptHanhKiemHocSinh();
        }

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
            term.TermId = Int32.Parse(DdlHocKy.SelectedValue);

            Class = new Class_Class();
            Class.ClassId = Int32.Parse(DdlLopHoc.SelectedValue);

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

            BindRptHanhKiemHocSinh();
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
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