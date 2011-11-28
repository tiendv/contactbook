using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.BusinessEntity;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class StudentMarkPage : BaseContentPage
    {
        #region Fields
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

            studyingResultBL = new StudyingResultBL(UserSchool);

            if (!Page.IsPostBack)
            {
                BindDropDownLists();
                BindRptMarkTypes();
                BindRptStudentMarks();
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

        protected void DdlHocKy_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLMonHoc();
        }

        protected void DdlLopHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLMonHoc();
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
            BindDDLMonHoc();
            BindDDLoaiDiem();
        }

        private void BindDDLNamHoc()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<CauHinh_NamHoc> lstNamHoc = systemConfigBL.GetListYears();
            DdlNamHoc.DataSource = lstNamHoc;
            DdlNamHoc.DataValueField = "MaNamHoc";
            DdlNamHoc.DataTextField = "TenNamHoc";
            DdlNamHoc.DataBind();

            if (DdlNamHoc.Items.Count != 0)
            {
                SystemConfigBL cauHinhBL = new SystemConfigBL(UserSchool);
                DdlNamHoc.SelectedValue = cauHinhBL.GetCurrentYear().ToString();
            }
        }

        private void BindDDLHocKy()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<CauHinh_HocKy> lstHocKy = systemConfigBL.GetListTerms();
            DdlHocKy.DataSource = lstHocKy;
            DdlHocKy.DataValueField = "MaHocKy";
            DdlHocKy.DataTextField = "TenHocKy";
            DdlHocKy.DataBind();
        }

        private void BindDDLNganhHoc()
        {
            FacultyBL facultyBL = new FacultyBL(UserSchool);
            List<DanhMuc_NganhHoc> faculties = facultyBL.GetFaculties();
            DdlNganh.DataSource = faculties;
            DdlNganh.DataValueField = "MaNganhHoc";
            DdlNganh.DataTextField = "TenNganhHoc";
            DdlNganh.DataBind();
            if (faculties.Count > 1)
            {
                DdlNganh.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }

        private void BindDDLKhoiLop()
        {
            GradeBL grades = new GradeBL(UserSchool);
            List<DanhMuc_KhoiLop> lstKhoiLop = grades.GetListGrades();
            DdlKhoiLop.DataSource = lstKhoiLop;
            DdlKhoiLop.DataValueField = "MaKhoiLop";
            DdlKhoiLop.DataTextField = "TenKhoiLop";
            DdlKhoiLop.DataBind();
            if (lstKhoiLop.Count > 1)
            {
                DdlKhoiLop.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }

        private void BindDDLoaiDiem()
        {
            MarkTypeBL loaiDiemBL = new MarkTypeBL(UserSchool);
            List<DanhMuc_LoaiDiem> lstLoaiDiem = loaiDiemBL.GetListMarkTypes();
            DdlLoaiDiem.DataSource = lstLoaiDiem;
            DdlLoaiDiem.DataValueField = "TenLoaiDiem";
            DdlLoaiDiem.DataTextField = "TenLoaiDiem";
            DdlLoaiDiem.DataBind();
            if (lstLoaiDiem.Count > 1)
            {
                DdlLoaiDiem.Items.Insert(0, new ListItem("Tất cả", ""));
            }
        }

        private void BindDDLLopHoc()
        {
            CauHinh_NamHoc year = null;
            DanhMuc_NganhHoc faculty = null;
            DanhMuc_KhoiLop grade = null;

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

            year = new CauHinh_NamHoc();
            year.MaNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);

            try
            {
                if (DdlNganh.SelectedIndex > 0)
                {
                    faculty = new DanhMuc_NganhHoc();
                    faculty.MaNganhHoc = Int32.Parse(DdlNganh.SelectedValue);
                }
            }
            catch (Exception) { }

            try
            {
                if (DdlKhoiLop.SelectedIndex > 0)
                {
                    grade = new DanhMuc_KhoiLop();
                    grade.MaKhoiLop = Int32.Parse(DdlKhoiLop.SelectedValue);
                }
            }
            catch (Exception) { }

            ClassBL lopHocBL = new ClassBL(UserSchool);
            List<LopHoc_Lop> lstLop = lopHocBL.GetListClasses(year, faculty, grade);
            DdlLopHoc.DataSource = lstLop;
            DdlLopHoc.DataValueField = "MaLopHoc";
            DdlLopHoc.DataTextField = "TenLopHoc";
            DdlLopHoc.DataBind();
        }

        private void BindDDLMonHoc()
        {
            LopHoc_Lop Class = null;
            CauHinh_HocKy term = null;
            ScheduleBL scheduleBL = new ScheduleBL(UserSchool);

            if (DdlLopHoc.Items.Count == 0)
            {
                DdlMonHoc.DataSource = null;
            }
            else
            {
                Class = new LopHoc_Lop();
                Class.MaLopHoc = Int32.Parse(DdlLopHoc.SelectedValue);
                term = new CauHinh_HocKy();
                term.MaHocKy = Int32.Parse(DdlHocKy.SelectedValue);

                List<DanhMuc_MonHoc> scheduledSubjects = scheduleBL.GetScheduledSubjects(Class, term);
                DdlMonHoc.DataSource = scheduledSubjects;
                DdlMonHoc.DataValueField = "MaMonHoc";
                DdlMonHoc.DataTextField = "TenMonHoc";
                DdlMonHoc.DataBind();
            }
        }

        private void BindRptMarkTypes()
        {
            MarkTypeBL markTypeBL = new MarkTypeBL(UserSchool);
            List<DanhMuc_LoaiDiem> markTypes = new List<DanhMuc_LoaiDiem>();

            if (DdlLoaiDiem.Items.Count != 0)
            {
                if (DdlLoaiDiem.SelectedIndex == 0)
                {
                    markTypes = markTypeBL.GetListMarkTypes();
                }
                else
                {
                    string markTypeName = DdlLoaiDiem.SelectedValue;
                    markTypes.Add(markTypeBL.GetMarkType(markTypeName));
                }
            }

            this.RptLoaiDiem.DataSource = markTypes;
            this.RptLoaiDiem.DataBind();
        }

        private void BindRptStudentMarks()
        {
            // declare variables
            LopHoc_Lop Class = null;
            DanhMuc_MonHoc subject = null;
            CauHinh_HocKy term = null;
            MarkTypeBL markTypeBL = new MarkTypeBL(UserSchool);
            List<DanhMuc_LoaiDiem> markTypes = new List<DanhMuc_LoaiDiem>();            
            List<TabularStudentMark> tabularStudentMarks = new List<TabularStudentMark>();
            double dTotalRecords = 0;

            // case: there is no Class or schedule subject or marktype
            if (DdlLopHoc.Items.Count == 0 || DdlMonHoc.Items.Count == 0 || DdlLoaiDiem.Items.Count == 0)
            {
                // do not display 
                ProcDisplayGUI(false);
                return;
            }

            // init object against user selections
            Class = new LopHoc_Lop();
            Class.MaLopHoc = Int32.Parse(DdlLopHoc.SelectedValue);
            subject = new DanhMuc_MonHoc();
            subject.MaMonHoc = Int32.Parse(DdlMonHoc.SelectedValue);
            term = new CauHinh_HocKy();
            term.MaHocKy = Int32.Parse(DdlHocKy.SelectedValue);
            if (DdlLoaiDiem.SelectedIndex == 0)
            {
                markTypes = markTypeBL.GetListMarkTypes();
            }
            else
            {
                string markTypeName = DdlLoaiDiem.SelectedValue;
                markTypes.Add(markTypeBL.GetMarkType(markTypeName));
            }

            // get student mark information
            tabularStudentMarks = studyingResultBL.GetTabularStudentMarks(Class, subject, term, markTypes,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            // bind to repeater and datapager
            this.RptDiemMonHoc.DataSource = tabularStudentMarks;
            this.RptDiemMonHoc.DataBind();
            MainDataPager.ItemCount = dTotalRecords;

            // display information
            bool bDisplayData = (tabularStudentMarks.Count != 0) ? true : false;
            ProcDisplayGUI(bDisplayData);
        }

        private void ProcDisplayGUI(bool bDisplayData)
        {
            RptDiemMonHoc.Visible = bDisplayData;
            MainDataPager.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            RptLoaiDiem.Visible = bDisplayData;
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
            //isSearch = true;
            BindRptMarkTypes();
            BindRptStudentMarks();
        }

        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            Dictionary<HocSinh_ThongTinCaNhan, List<DetailMark>> dicEnteredStudentMarks = new Dictionary<HocSinh_ThongTinCaNhan, List<DetailMark>>();
            HocSinh_ThongTinCaNhan student = null;
            LopHoc_Lop Class = new LopHoc_Lop();
            CauHinh_HocKy term = new CauHinh_HocKy();
            DanhMuc_MonHoc subject = null;
            DanhMuc_LoaiDiem markType = null;

            term.MaHocKy = Int32.Parse(DdlHocKy.SelectedValue);
            Class.MaLopHoc = Int32.Parse(DdlLopHoc.SelectedValue);
            subject = new DanhMuc_MonHoc();
            subject.MaMonHoc = Int32.Parse(DdlMonHoc.SelectedValue);
            foreach (RepeaterItem rptItemStudentMark in RptDiemMonHoc.Items)
            {
                if (rptItemStudentMark.ItemType == ListItemType.Item || rptItemStudentMark.ItemType == ListItemType.AlternatingItem)
                {
                    List<DetailMark> detailMarks = new List<DetailMark>();
                    Repeater rptDetailMark = (Repeater)rptItemStudentMark.FindControl("RptDiemTheoLoaiDiem");
                    foreach (RepeaterItem rptItemDiem in rptDetailMark.Items)
                    {
                        if (rptItemDiem.ItemType == ListItemType.Item || rptItemDiem.ItemType == ListItemType.AlternatingItem)
                        {
                            HiddenField hdfMaLoaiDiem = (HiddenField)rptItemDiem.FindControl("HdfMaLoaiDiem");
                            markType = new DanhMuc_LoaiDiem();
                            markType.MaLoaiDiem = Int32.Parse(hdfMaLoaiDiem.Value);

                            TextBox txtDiems = (TextBox)rptItemDiem.FindControl("TxtDiems");
                            string marks = txtDiems.Text.Trim();

                            if (studyingResultBL.ValidateMark(marks, markType))
                            {
                                if (txtDiems.Text != "")
                                {
                                    string[] strMarks = txtDiems.Text.Trim().Split(new char[] { ',' });
                                    foreach (string strMark in strMarks)
                                    {
                                        double dMark = double.Parse(strMark.Trim());
                                        detailMarks.Add(new DetailMark
                                        {
                                            MaLoaiDiem = Int32.Parse(hdfMaLoaiDiem.Value),
                                            GiaTri = dMark
                                        });
                                    }
                                }
                                else
                                {
                                    detailMarks.Add(new DetailMark
                                    {
                                        MaLoaiDiem = Int32.Parse(hdfMaLoaiDiem.Value),
                                        GiaTri = -1
                                    });
                                }
                            }
                            else
                            {
                                CustomValidator diemsValidator = (CustomValidator)rptItemDiem.FindControl("DiemsValidator");
                                diemsValidator.IsValid = false;
                                return;
                            }
                        }
                    }

                    student = new HocSinh_ThongTinCaNhan();
                    HiddenField hdfStudentId = (HiddenField)rptItemStudentMark.FindControl("HdfMaHocSinh");
                    student.MaHocSinh = Int32.Parse(hdfStudentId.Value);

                    dicEnteredStudentMarks.Add(student, detailMarks);
                }
            }

            foreach (KeyValuePair<HocSinh_ThongTinCaNhan, List<DetailMark>> pair in dicEnteredStudentMarks)
            {
                studyingResultBL.UpdateDetailedMark(pair.Key, Class, term, subject, pair.Value);
            }

            BindRptStudentMarks();
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
        }
        #endregion

        #region Repeater event handlers
        protected void RptDiemMonHoc_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item
                || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TabularStudentMark tabularStudentMark = (TabularStudentMark)e.Item.DataItem;
                Repeater rptMarkTypeBasedMarks = (Repeater)e.Item.FindControl("RptDiemTheoLoaiDiem");
                rptMarkTypeBasedMarks.DataSource = tabularStudentMark.DiemTheoLoaiDiems;
                rptMarkTypeBasedMarks.DataBind();
            }
        }
        #endregion

        #region Pager event handlers
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currnetPageIndx;
            BindRptStudentMarks();
        }
        #endregion
    }
}