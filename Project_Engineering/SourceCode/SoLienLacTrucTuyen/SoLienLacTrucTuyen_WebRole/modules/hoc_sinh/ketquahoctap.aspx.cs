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
    public partial class KetQuaHocTapPage : BaseContentPage
    {
        #region Fields
        private StudentBL hocSinhBL;
        private MarkTypeBL loaiDiemBL;
        private StudyingResultBL ketQuaHocTapBL;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (isAccessDenied)
            {
                return;
            }

            hocSinhBL = new StudentBL(UserSchool);
            loaiDiemBL = new MarkTypeBL(UserSchool);
            ketQuaHocTapBL = new StudyingResultBL(UserSchool);

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
                List<CauHinh_NamHoc> lstNamHoc = hocSinhBL.GetYears(student);
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
            List<DanhMuc_LoaiDiem> lstLoaiDiem = loaiDiemBL.GetListMarkTypes();
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

            double totalRecords;
            List<TabularSubjectTermResult> lstTbKetQuaMonHoc;
            lstTbKetQuaMonHoc = ketQuaHocTapBL.GetTabularSubjectTermResults(student, year, term,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out totalRecords);

            bool bDisplayData = (lstTbKetQuaMonHoc.Count != 0) ? true : false;
            RptKetQuaDiem.Visible = bDisplayData;
            MainDataPager.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            RptLoaiDiem.Visible = bDisplayData;
            tdKQHocTapSTT.Visible = bDisplayData;
            tdKQHocTapMonHoc.Visible = bDisplayData;
            tdKQHocTapDTB.Visible = bDisplayData;
            tdEdit.Visible = bDisplayData;

            this.RptKetQuaDiem.DataSource = lstTbKetQuaMonHoc;
            this.RptKetQuaDiem.DataBind();
            MainDataPager.ItemCount = totalRecords;
        }

        private void BindRepeaterDanhHieu()
        {
            HocSinh_ThongTinCaNhan student = new HocSinh_ThongTinCaNhan();
            CauHinh_NamHoc year = new CauHinh_NamHoc();

            student.MaHocSinh = (int)ViewState["MaHocSinh"];
            year.MaNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);

            double totalRecords;
            List<TabularTermStudentResult> lstTbDanhHieu;
            lstTbDanhHieu = ketQuaHocTapBL.GetTabularTermStudentResults(student, year,
                DataPagerDanhHieu.CurrentIndex, DataPagerDanhHieu.PageSize, out totalRecords);

            RptDanhHieu.DataSource = lstTbDanhHieu;
            RptDanhHieu.DataBind();
            DataPagerDanhHieu.ItemCount = totalRecords;

            bool bDisplayed = (lstTbDanhHieu.Count != 0) ? true : false;
            RptDanhHieu.Visible = bDisplayed;
            PnlPopupHanhKiem.Visible = bDisplayed;
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
                    lstStrDiemMonHocLoaiDiem = ketQuaHocTapBL.GetSubjectMarks(termSubjectedMark);
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

        protected void RptDanhHieu_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "OpenPopupHanhKiem":
                    {
                        int maHanhKiem = Int32.Parse(e.CommandArgument.ToString());

                        List<DanhMuc_HanhKiem> lstHanhKiem = (new ConductBL(UserSchool)).GetListConducts(true);

                        foreach (DanhMuc_HanhKiem hanhKiem in lstHanhKiem)
                        {
                            RadioButton rbtn = new RadioButton();
                            rbtn.ID = "PnlPopupHanhKiem_Rbtn" + hanhKiem.MaHanhKiem.ToString();
                            rbtn.Text = hanhKiem.TenHanhKiem;
                            rbtn.GroupName = "GpRbtnHanhKiem";
                            rbtn.Attributes.Add("class", "rbtnHanhKiem");
                            PnlPopupHanhKiem_DivListHanhKiem.Controls.Add(rbtn);
                            HtmlControl hcBreak = new HtmlGenericControl("br");
                            PnlPopupHanhKiem_DivListHanhKiem.Controls.Add(hcBreak);
                        }

                        RadioButton checkedRbtn = (RadioButton)PnlPopupHanhKiem_DivListHanhKiem.FindControl(
                            "PnlPopupHanhKiem_Rbtn" + e.CommandArgument.ToString());
                        checkedRbtn.Checked = true;

                        ModalPopupExtender mPEHanhKiem = (ModalPopupExtender)e.Item.FindControl("MPEHanhKiem");
                        mPEHanhKiem.Show();

                        RptMPEHanhKiem.Value = mPEHanhKiem.ClientID;

                        HiddenField hdfRptMaDanhHieuHSHK = (HiddenField)e.Item.FindControl("HdfRptMaDanhHieuHSHK");
                        HdfMaDanhHieuHSHK.Value = hdfRptMaDanhHieuHSHK.Value;

                        string userName = Session["username"].ToString();
                        Session[userName + "CheckHanhKiem"] = e.CommandArgument.ToString();

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

        protected void popopHanhKiem_Save_Click(object sender, ImageClickEventArgs e)
        {
            HocSinh_DanhHieuHocKy termResult = null;
            DanhMuc_HanhKiem conduct = null;
            if (Session[Session["username"].ToString() + "CheckHanhKiem"] != null)
            {
                termResult.MaDanhHieuHSHK = Int32.Parse(HdfMaDanhHieuHSHK.Value);
                conduct.MaHanhKiem = Int32.Parse(Session[Session["username"].ToString() + "CheckHanhKiem"].ToString());

                ketQuaHocTapBL.UpdateStudentTermResult(termResult, conduct);
                Session.Remove(Session["username"].ToString() + "CheckHanhKiem");
            }

            BindRepeaterDanhHieu();
        }

        protected void BtnBackPrevPage_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/modules/hoc_sinh/danhsachhocsinh.aspx");
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