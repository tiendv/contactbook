using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;
using AjaxControlToolkit;
using System.Web.UI.HtmlControls;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class StudentStudyingResultPage : BaseContentPage
    {
        #region Fields
        private StudentBL studentBL;
        private MarkTypeBL markTypeBL;
        private StudyingResultBL studyingResultBL;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (isAccessDenied)
            {
                return;
            }

            studentBL = new StudentBL(UserSchool);
            markTypeBL = new MarkTypeBL(UserSchool);
            studyingResultBL = new StudyingResultBL(UserSchool);

            if (!Page.IsPostBack)
            {
                if (CheckSessionKey(AppConstant.SESSION_STUDENT))
                {
                    HocSinh_ThongTinCaNhan student = (HocSinh_ThongTinCaNhan)GetSession(AppConstant.SESSION_STUDENT);
                    RemoveSession(AppConstant.SESSION_STUDENT);
                    ViewState[AppConstant.VIEWSTATE_STUDENTID] = student.MaHocSinh;

                    LopHoc_Lop studentClass = (LopHoc_Lop)GetSession(AppConstant.SESSION_STUDENTCLASS);
                    RemoveSession(AppConstant.SESSION_STUDENTCLASS);
                    ViewState[AppConstant.VIEWSTATE_STUDENTCLASS_ID] = studentClass.MaLopHoc;

                    CauHinh_NamHoc year = (CauHinh_NamHoc)GetSession(AppConstant.SESSION_SELECTED_YEAR);
                    RemoveSession(AppConstant.SESSION_SELECTED_YEAR);
                    ViewState[AppConstant.VIEWSTATE_STUDENTID] = student.MaHocSinh;

                    DanhMuc_NganhHoc faculty = (DanhMuc_NganhHoc)GetSession(AppConstant.SESSION_SELECTED_FACULTY);
                    RemoveSession(AppConstant.SESSION_SELECTED_FACULTY);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTY] = faculty.MaNganhHoc;

                    DanhMuc_KhoiLop grade = (DanhMuc_KhoiLop)GetSession(AppConstant.SESSION_SELECTED_GRADE);
                    RemoveSession(AppConstant.SESSION_SELECTED_GRADE);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_GRADE] = grade.MaKhoiLop;

                    LopHoc_Lop Class = (LopHoc_Lop)GetSession(AppConstant.SESSION_SELECTED_CLASS);
                    RemoveSession(AppConstant.SESSION_SELECTED_CLASS);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS] = Class.MaLopHoc;

                    String strStudentName = (string)GetSession(AppConstant.SESSION_SELECTED_STUDENTNAME);
                    RemoveSession(AppConstant.SESSION_SELECTED_STUDENTNAME);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTNAME] = strStudentName;

                    String strStudentCode = (string)GetSession(AppConstant.SESSION_SELECTED_STUDENTCODE);
                    RemoveSession(AppConstant.SESSION_SELECTED_STUDENTCODE);
                    ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTCODE] = strStudentCode;

                    ViewState[AppConstant.VIEWSTATE_STUDENTID] = student.MaHocSinh;

                    BindDropDownLists();
                    BindRptMarkTypes();
                    BindRptKetQuaDiem();
                    BindRepeaterDanhHieu();

                    AuthorizationBL authorizationBL = new AuthorizationBL(UserSchool);
                    List<UserManagement_PagePath> pagePages = authorizationBL.GetStudentPages(
                        (new UserBL()).GetRoles(User.Identity.Name));
                    RptStudentFunctions.DataSource = pagePages;
                    RptStudentFunctions.DataBind();
                }
                else
                {
                    Response.Redirect(AppConstant.PAGEPATH_STUDENTS);
                }
            }
        }
        #endregion

        #region Methods
        private void BindDropDownLists()
        {
            BindDDLYears();
            BindDDLTerms();
        }

        private void BindDDLYears()
        {
            if (ViewState[AppConstant.VIEWSTATE_STUDENTID] != null)
            {
                HocSinh_ThongTinCaNhan student = new HocSinh_ThongTinCaNhan();
                student.MaHocSinh = (int)ViewState[AppConstant.VIEWSTATE_STUDENTID];
                List<CauHinh_NamHoc> years = studentBL.GetYears(student);
                DdlNamHoc.DataSource = years;
                DdlNamHoc.DataValueField = "MaNamHoc";
                DdlNamHoc.DataTextField = "TenNamHoc";
                DdlNamHoc.DataBind();
            }
        }

        private void BindDDLTerms()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<CauHinh_HocKy> terms = systemConfigBL.GetListTerms();
            DdlHocKy.DataSource = terms;
            DdlHocKy.DataValueField = "MaHocKy";
            DdlHocKy.DataTextField = "TenHocKy";
            DdlHocKy.DataBind();
        }

        private void BindRptMarkTypes()
        {
            List<DanhMuc_LoaiDiem> markTypes = markTypeBL.GetListMarkTypes();
            this.RptLoaiDiem.DataSource = markTypes;
            this.RptLoaiDiem.DataBind();
        }

        private void BindRptKetQuaDiem()
        {
            CauHinh_NamHoc year = new CauHinh_NamHoc();
            HocSinh_ThongTinCaNhan student = new HocSinh_ThongTinCaNhan();
            CauHinh_HocKy term = new CauHinh_HocKy();

            student.MaHocSinh = (int)ViewState[AppConstant.VIEWSTATE_STUDENTID]; ;
            year.MaNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);
            term.MaHocKy = Int32.Parse(DdlHocKy.SelectedValue);

            double dTotalRecords;
            List<TabularSubjectTermResult> lstTbKetQuaMonHoc;
            lstTbKetQuaMonHoc = studyingResultBL.GetTabularSubjectTermResults(student, year, term,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            bool bDisplayData = (lstTbKetQuaMonHoc.Count != 0) ? true : false;
            RptKetQuaDiem.Visible = bDisplayData;
            MainDataPager.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            RptLoaiDiem.Visible = bDisplayData;
            tdKQHocTapSTT.Visible = bDisplayData;
            tdKQHocTapMonHoc.Visible = bDisplayData;
            tdKQHocTapDTB.Visible = bDisplayData;

            this.RptKetQuaDiem.DataSource = lstTbKetQuaMonHoc;
            this.RptKetQuaDiem.DataBind();
            MainDataPager.ItemCount = dTotalRecords;
        }

        private void BindRepeaterDanhHieu()
        {
            HocSinh_ThongTinCaNhan student = new HocSinh_ThongTinCaNhan();
            CauHinh_NamHoc year = new CauHinh_NamHoc();

            student.MaHocSinh = (int)ViewState[AppConstant.VIEWSTATE_STUDENTID];
            year.MaNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);

            double dTotalRecords;
            List<TabularTermStudentResult> lstTbDanhHieu;
            lstTbDanhHieu = studyingResultBL.GetTabularTermStudentResults(student, year,
                DataPagerDanhHieu.CurrentIndex, DataPagerDanhHieu.PageSize, out dTotalRecords);

            RptDanhHieu.DataSource = lstTbDanhHieu;
            RptDanhHieu.DataBind();
            DataPagerDanhHieu.ItemCount = dTotalRecords;

            bool bDisplayed = (lstTbDanhHieu.Count != 0) ? true : false;
            RptDanhHieu.Visible = bDisplayed;
        }
        #endregion

        #region Repeater event handlers
        protected void RptKetQuaDiem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HocSinh_DiemMonHocHocKy termSubjectedMark = null;
            if (e.Item.ItemType == ListItemType.Item
                || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Control control = e.Item.FindControl("HdfMaDiemMonHK");
                if (control != null)
                {
                    termSubjectedMark = new HocSinh_DiemMonHocHocKy();
                    termSubjectedMark.MaDiemMonHK = Int32.Parse(((HiddenField)control).Value);
                    List<StrDiemMonHocLoaiDiem> lstStrDiemMonHocLoaiDiem;
                    lstStrDiemMonHocLoaiDiem = studyingResultBL.GetSubjectMarks(termSubjectedMark);
                    Repeater rptDiemMonHoc = (Repeater)e.Item.FindControl("RptDiemTheoMonHoc");
                    rptDiemMonHoc.DataSource = lstStrDiemMonHocLoaiDiem;
                    rptDiemMonHoc.DataBind();
                }
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
                        HocSinh_ThongTinCaNhan student = new HocSinh_ThongTinCaNhan();
                        student.MaHocSinh = Int32.Parse(ViewState[AppConstant.VIEWSTATE_STUDENTID].ToString());
                        AddSession(AppConstant.SESSION_STUDENT, student);

                        CauHinh_NamHoc year = new CauHinh_NamHoc();
                        year.MaNamHoc = Int32.Parse(ViewState[AppConstant.VIEWSTATE_STUDENTID].ToString());
                        AddSession(AppConstant.SESSION_SELECTED_YEAR, year);

                        DanhMuc_NganhHoc faculty = new DanhMuc_NganhHoc();
                        faculty.MaNganhHoc = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTY].ToString());
                        AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);

                        DanhMuc_KhoiLop grade = new DanhMuc_KhoiLop();
                        grade.MaKhoiLop = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_GRADE].ToString());
                        AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);

                        LopHoc_Lop Class = new LopHoc_Lop();
                        Class.MaLopHoc = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS].ToString());
                        AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);

                        String strStudentName = ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTNAME].ToString();
                        AddSession(AppConstant.SESSION_SELECTED_STUDENTNAME, strStudentName);

                        String strStudentCode = ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTCODE].ToString();
                        AddSession(AppConstant.SESSION_SELECTED_STUDENTCODE, strStudentCode);

                        LopHoc_Lop studentClass = new LopHoc_Lop();
                        studentClass.MaLopHoc = (int)ViewState[AppConstant.VIEWSTATE_STUDENTCLASS_ID];
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
            BindRptKetQuaDiem();
            BindRepeaterDanhHieu();
        }

        protected void BtnBackPrevPage_Click(object sender, ImageClickEventArgs e)
        {
            CauHinh_NamHoc year = new CauHinh_NamHoc();
            year.MaNamHoc = Int32.Parse(ViewState[AppConstant.VIEWSTATE_STUDENTID].ToString());
            AddSession(AppConstant.SESSION_SELECTED_YEAR, year);

            DanhMuc_NganhHoc faculty = new DanhMuc_NganhHoc();
            faculty.MaNganhHoc = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_FACULTY].ToString());
            AddSession(AppConstant.SESSION_SELECTED_FACULTY, faculty);

            DanhMuc_KhoiLop grade = new DanhMuc_KhoiLop();
            grade.MaKhoiLop = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_GRADE].ToString());
            AddSession(AppConstant.SESSION_SELECTED_GRADE, grade);

            LopHoc_Lop Class = new LopHoc_Lop();
            Class.MaLopHoc = Int32.Parse(ViewState[AppConstant.VIEWSTATE_SELECTED_CLASS].ToString());
            AddSession(AppConstant.SESSION_SELECTED_CLASS, Class);

            String strStudentName = ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTNAME].ToString();
            AddSession(AppConstant.SESSION_SELECTED_STUDENTNAME, strStudentName);

            String strStudentCode = ViewState[AppConstant.VIEWSTATE_SELECTED_STUDENTCODE].ToString();
            AddSession(AppConstant.SESSION_SELECTED_STUDENTCODE, strStudentCode);

            Response.Redirect(AppConstant.PAGEPATH_STUDENTS);
        }
        #endregion

        #region DataPager event handlers
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currentPageIndex;
            BindRptKetQuaDiem();
        }
        #endregion
    }
}