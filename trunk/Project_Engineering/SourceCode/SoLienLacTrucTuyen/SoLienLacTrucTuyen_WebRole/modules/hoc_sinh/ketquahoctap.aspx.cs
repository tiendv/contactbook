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
                if (Request.QueryString["hocsinh"] != null)
                {
                    int maHocSinh = Int32.Parse(Request.QueryString["hocsinh"]);
                    ViewState["MaHocSinh"] = maHocSinh;
                    HdfMaHocSinh.Value = Request.QueryString["hocsinh"];

                    BindDropDownLists();
                    BindRptTenLoaiDiem();
                    BindRptKetQuaDiem();
                    BindRepeaterDanhHieu();

                    HlkThongTinCaNhan.NavigateUrl = String.Format("thongtincanhan.aspx?hocsinh={0}", maHocSinh);
                    HlkNgayNghiHoc.NavigateUrl = String.Format("ngaynghihoc.aspx?hocsinh={0}", maHocSinh);
                    HlkHoatDong.NavigateUrl = String.Format("hoatdong.aspx?hocsinh={0}", maHocSinh);
                }
            }
        }
        #endregion

        #region Methods
        private void BindDropDownLists()
        {
            BindDDLNamHoc();
            BindDDLHocKy();
        }

        private void BindDDLNamHoc()
        {
            if (ViewState["MaHocSinh"] != null)
            {
                HocSinh_ThongTinCaNhan student = new HocSinh_ThongTinCaNhan();
                student.MaHocSinh = (int)ViewState["MaHocSinh"];
                List<CauHinh_NamHoc> lstNamHoc = studentBL.GetYears(student);
                DdlNamHoc.DataSource = lstNamHoc;
                DdlNamHoc.DataValueField = "MaNamHoc";
                DdlNamHoc.DataTextField = "TenNamHoc";
                DdlNamHoc.DataBind();
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

        private void BindRptTenLoaiDiem()
        {
            List<DanhMuc_LoaiDiem> lstLoaiDiem = markTypeBL.GetListMarkTypes();
            this.RptLoaiDiem.DataSource = lstLoaiDiem;
            this.RptLoaiDiem.DataBind();
        }

        private void BindRptKetQuaDiem()
        {
            CauHinh_NamHoc year = new CauHinh_NamHoc();
            HocSinh_ThongTinCaNhan student = new HocSinh_ThongTinCaNhan();
            CauHinh_HocKy term = new CauHinh_HocKy();
            
            student.MaHocSinh = (int)ViewState["MaHocSinh"];;
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

            student.MaHocSinh = (int)ViewState["MaHocSinh"];
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

        private void BindRepeaterHanhKiem()
        {

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

        protected void RptKetQuaDiem_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDetailItem":
                    {
                        Response.Redirect("/modules/hoc_sinh/diemmonhoc.aspx");
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        protected void RptDanhHieu_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item
                || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //Control control = e.Item.FindControl("HdfMaDiemMonHK");
                //if (control != null)
                //{
                //    int maDiemMonHK = Int32.Parse(((HiddenField)control).Value);
                //    List<StrDiemMonHocLoaiDiem> lstStrDiemMonHocLoaiDiem = ketQuaHocTapBL.GetListStringDiemMonHoc(maDiemMonHK);
                //    Repeater rptDiemMonHoc = (Repeater)e.Item.FindControl("RptDiemTheoMonHoc");
                //    rptDiemMonHoc.DataSource = lstStrDiemMonHocLoaiDiem;
                //    rptDiemMonHoc.DataBind();
                //}
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
            Response.Redirect(AppConstant.PAGEPATH_USERS);
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