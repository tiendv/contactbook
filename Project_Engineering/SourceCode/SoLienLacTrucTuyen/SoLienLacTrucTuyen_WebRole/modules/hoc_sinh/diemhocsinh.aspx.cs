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
    public partial class DiemHocSinhPage : BaseContentPage
    {
        #region Fields
        private StudyingResultBL ketQuaHocTapBL;

        public bool ChooseAllMarkType
        {
            get
            {
                if (ViewState["ChooseAllMarkType"] != null)
                {
                    return (bool)ViewState["ChooseAllMarkType"];
                }
                else
                {
                    return true;
                }
            }
            set
            {
                ViewState["ChooseAllMarkType"] = value;
            }
        }
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (isAccessDenied)
            {
                return;
            }

            ketQuaHocTapBL = new StudyingResultBL();

            if (!Page.IsPostBack)
            {
                BindDropDownLists();
                BindRptTenLoaiDiem();
                BindRptDiemHocSinh();
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
            SystemConfigBL systemConfigBL = new SystemConfigBL();
            List<CauHinh_NamHoc> lstNamHoc = systemConfigBL.GetListYears();
            DdlNamHoc.DataSource = lstNamHoc;
            DdlNamHoc.DataValueField = "MaNamHoc";
            DdlNamHoc.DataTextField = "TenNamHoc";
            DdlNamHoc.DataBind();

            if (DdlNamHoc.Items.Count != 0)
            {
                SystemConfigBL cauHinhBL = new SystemConfigBL();
                DdlNamHoc.SelectedValue = cauHinhBL.GetCurrentYear().ToString();
            }
        }

        private void BindDDLHocKy()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL();
            List<CauHinh_HocKy> lstHocKy = systemConfigBL.GetListTerms();
            DdlHocKy.DataSource = lstHocKy;
            DdlHocKy.DataValueField = "MaHocKy";
            DdlHocKy.DataTextField = "TenHocKy";
            DdlHocKy.DataBind();
        }

        private void BindDDLNganhHoc()
        {
            FacultyBL nganhHocBL = new FacultyBL();
            List<DanhMuc_NganhHoc> lstNganhHoc = nganhHocBL.GetFaculties();
            DdlNganh.DataSource = lstNganhHoc;
            DdlNganh.DataValueField = "MaNganhHoc";
            DdlNganh.DataTextField = "TenNganhHoc";
            DdlNganh.DataBind();
            if (lstNganhHoc.Count > 1)
            {
                DdlNganh.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }

        private void BindDDLKhoiLop()
        {
            GradeBL KhoiLopBL = new GradeBL();
            List<DanhMuc_KhoiLop> lstKhoiLop = KhoiLopBL.GetListGrades();
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
            MarkTypeBL loaiDiemBL = new MarkTypeBL();
            List<DanhMuc_LoaiDiem> lstLoaiDiem = loaiDiemBL.GetListMarkTypes();
            DdlLoaiDiem.DataSource = lstLoaiDiem;
            DdlLoaiDiem.DataValueField = "TenLoaiDiem";
            DdlLoaiDiem.DataTextField = "TenLoaiDiem";
            DdlLoaiDiem.DataBind();
            if (lstLoaiDiem.Count > 1)
            {
                DdlLoaiDiem.Items.Insert(0, new ListItem("Tất cả", ""));
                ChooseAllMarkType = true;
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

            ClassBL lopHocBL = new ClassBL();
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
            ScheduleBL thoiKhoaBieuBL = new ScheduleBL();

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

                List<DanhMuc_MonHoc> lstMonHoc = thoiKhoaBieuBL.GetScheduledSubjects(Class, term);
                DdlMonHoc.DataSource = lstMonHoc;
                DdlMonHoc.DataValueField = "MaMonHoc";
                DdlMonHoc.DataTextField = "TenMonHoc";
                DdlMonHoc.DataBind();
            }
        }

        private void BindRptTenLoaiDiem()
        {
            MarkTypeBL loaiDiemBL = new MarkTypeBL();
            List<DanhMuc_LoaiDiem> lstLoaiDiem = new List<DanhMuc_LoaiDiem>();

            if (DdlLoaiDiem.Items.Count != 0)
            {
                if (DdlLoaiDiem.SelectedIndex == 0)
                {
                    lstLoaiDiem = loaiDiemBL.GetListMarkTypes();
                }
                else
                {
                    string markTypeName = DdlLoaiDiem.SelectedValue;
                    lstLoaiDiem.Add(loaiDiemBL.GetMarkType(markTypeName));
                }

                ChooseAllMarkType = false;
            }
            else
            {
                ChooseAllMarkType = true;
            }

            this.RptLoaiDiem.DataSource = lstLoaiDiem;
            this.RptLoaiDiem.DataBind();
        }

        private void BindRptDiemHocSinh()
        {
            LopHoc_Lop Class = null;
            DanhMuc_MonHoc subject = null;
            CauHinh_HocKy term = null;
            MarkTypeBL markTypeBL = new MarkTypeBL();
            List<DanhMuc_LoaiDiem> markTypes = new List<DanhMuc_LoaiDiem>();

            if (DdlLopHoc.Items.Count == 0 || DdlMonHoc.Items.Count == 0
                || DdlLoaiDiem.Items.Count == 0)
            {
                ProcDisplayGUI(false);
                return;
            }

            Class = new LopHoc_Lop();
            Class.MaLopHoc = Int32.Parse(DdlLopHoc.SelectedValue);
            subject = new DanhMuc_MonHoc();
            subject.MaMonHoc = Int32.Parse(DdlMonHoc.SelectedValue);
            term = new CauHinh_HocKy();
            term.MaHocKy = Int32.Parse(DdlHocKy.SelectedValue);
            //int maLoaiDiem = Int32.Parse(DdlLoaiDiem.SelectedValue);

            if (DdlLoaiDiem.SelectedIndex == 0)
            {
                markTypes = markTypeBL.GetListMarkTypes();
            }
            else
            {
                string markTypeName = DdlLoaiDiem.SelectedValue;
                markTypes.Add(markTypeBL.GetMarkType(markTypeName));
            }

            double totalRecords;
            List<TabularStudentMark> lstTbDiemHocSinh;
            lstTbDiemHocSinh = ketQuaHocTapBL.GetListDiemHocSinh(Class, subject, term, markTypes,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out totalRecords);

            this.RptDiemMonHoc.DataSource = lstTbDiemHocSinh;
            this.RptDiemMonHoc.DataBind();
            MainDataPager.ItemCount = totalRecords;

            bool bDisplayData = (lstTbDiemHocSinh.Count != 0) ? true : false;
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
            if (DdlLopHoc.Items.Count != 0)
            {
                if ((DdlLoaiDiem.SelectedValue == "") || (string.Compare(DdlLoaiDiem.SelectedValue, "tất cả", true) == 0))
                {
                    ChooseAllMarkType = true;
                }
                else
                {
                    ChooseAllMarkType = false;
                }
            }

            MainDataPager.CurrentIndex = 1;
            //isSearch = true;
            BindRptTenLoaiDiem();
            BindRptDiemHocSinh();
        }

        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            HocSinh_ThongTinCaNhan student = null;
            LopHoc_Lop Class = null;
            CauHinh_HocKy term = null;
            DanhMuc_MonHoc subject = null;

            term = new CauHinh_HocKy();
            term.MaHocKy = Int32.Parse(DdlHocKy.SelectedValue);
            Class = new LopHoc_Lop();
            Class.MaLopHoc = Int32.Parse(DdlLopHoc.SelectedValue);
            subject = new DanhMuc_MonHoc();
            subject.MaMonHoc = Int32.Parse(DdlMonHoc.SelectedValue);
            Dictionary<int, List<Diem>> dicSavingDiemHocSinh = new Dictionary<int, List<Diem>>();

            foreach (RepeaterItem rptItemDiemMonHoc in RptDiemMonHoc.Items)
            {
                if (rptItemDiemMonHoc.ItemType == ListItemType.Item
                    || rptItemDiemMonHoc.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField hdfMaHocSinh = (HiddenField)rptItemDiemMonHoc.FindControl("HdfMaHocSinh");
                    int maHocSinh = Int32.Parse(hdfMaHocSinh.Value);

                    List<Diem> lDiems = new List<Diem>();
                    Repeater rptDiemTheoLoaiDiem = (Repeater)rptItemDiemMonHoc.FindControl("RptDiemTheoLoaiDiem");
                    foreach (RepeaterItem rptItemDiem in rptDiemTheoLoaiDiem.Items)
                    {
                        if (rptItemDiem.ItemType == ListItemType.Item
                            || rptItemDiem.ItemType == ListItemType.AlternatingItem)
                        {
                            HiddenField hdfMaLoaiDiem = (HiddenField)rptItemDiem.FindControl("HdfMaLoaiDiem");
                            int maLoaiDiem = Int32.Parse(hdfMaLoaiDiem.Value);

                            HiddenField hdfTenLoaiDiem = (HiddenField)rptItemDiem.FindControl("HdfTenLoaiDiem");
                            string markTypeName = hdfTenLoaiDiem.Value;

                            TextBox txtDiems = (TextBox)rptItemDiem.FindControl("TxtDiems");
                            string marks = txtDiems.Text.Trim();
                            if (ketQuaHocTapBL.ValidateMark(marks, markTypeName))
                            {
                                if (txtDiems.Text != "")
                                {
                                    string[] strDiems = txtDiems.Text.Trim().Split(new char[] { ',' });
                                    foreach (string strDiem in strDiems)
                                    {
                                        double diem = double.Parse(strDiem.Trim());
                                        lDiems.Add(new Diem { MaLoaiDiem = maLoaiDiem, GiaTri = diem });
                                    }
                                }
                                else
                                {
                                    lDiems.Add(new Diem { MaLoaiDiem = maLoaiDiem, GiaTri = -1 });
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

                    dicSavingDiemHocSinh.Add(maHocSinh, lDiems);
                }
            }

            foreach (KeyValuePair<int, List<Diem>> pair in dicSavingDiemHocSinh)
            {
                int maHocSinh = pair.Key;
                List<Diem> lDiems = pair.Value;
                ketQuaHocTapBL.UpdateDetailedMark(student, Class, term, subject, lDiems);
            }

            BindRptDiemHocSinh();
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
                TabularStudentMark tbDiemHocSinh = (TabularStudentMark)e.Item.DataItem;
                Repeater rptDiemTheoLoaiDiem = (Repeater)e.Item.FindControl("RptDiemTheoLoaiDiem");
                rptDiemTheoLoaiDiem.DataSource = tbDiemHocSinh.DiemTheoLoaiDiems;
                rptDiemTheoLoaiDiem.DataBind();
            }
        }
        #endregion

        #region Pager event handlers
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currnetPageIndx;
            BindRptDiemHocSinh();
        }
        #endregion
    }
}