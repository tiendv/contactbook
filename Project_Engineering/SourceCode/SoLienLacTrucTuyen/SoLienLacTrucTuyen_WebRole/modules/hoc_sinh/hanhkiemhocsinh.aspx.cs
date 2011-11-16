using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessEntity;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;

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
            if (isAccessDenied)
            {
                return;
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
            FacultyBL nganhHocBL = new FacultyBL(UserSchool);
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
            GradeBL KhoiLopBL = new GradeBL(UserSchool);
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

        private void BindRptHanhKiemHocSinh()
        {
            if (DdlLopHoc.Items.Count == 0)
            {
                ProcDisplayGUI(false);
                return;
            }

            int maLopHoc = Int32.Parse(DdlLopHoc.SelectedValue);
            int maHocKy = Int32.Parse(DdlHocKy.SelectedValue);
            //int maHanhKiem = Int32.Parse(DdlHanhKiem.SelectedValue);

            double totalRecords = 0;
            List<TabularHanhKiemHocSinh> lstTbHanhKiemHocSinh;
            lstTbHanhKiemHocSinh = hocSinhBL.GetListHanhKiemHocSinh(
                maLopHoc, maHocKy,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out totalRecords);

            this.RptHanhKiemHocSinh.DataSource = lstTbHanhKiemHocSinh;
            this.RptHanhKiemHocSinh.DataBind();
            MainDataPager.ItemCount = totalRecords;

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
            LopHoc_Lop Class = null;
            HocSinh_ThongTinCaNhan student = null;
            CauHinh_HocKy term = null;
            DanhMuc_HanhKiem conduct = null;

            Dictionary<int, int?> dicHocSinhHanhKiem = new Dictionary<int, int?>();
            foreach (RepeaterItem rptItem in RptHanhKiemHocSinh.Items)
            {
                HiddenField hdfMaHocSinh = (HiddenField)rptItem.FindControl("HdfMaHocSinh");
                HiddenField hdfMaHanhKiemHocSinh = (HiddenField)rptItem.FindControl("HdfMaHanhKiemHocSinh");
                int? orgMaHanhKiemHocSinh = null;
                try
                {
                    orgMaHanhKiemHocSinh = Int32.Parse(hdfMaHanhKiemHocSinh.Value);
                }
                catch (Exception ex) { }
                Repeater rptHanhKiem = (Repeater)rptItem.FindControl("RptHanhKiem");

                foreach (RepeaterItem item in rptHanhKiem.Items)
                {
                    RadioButton rbtnHanhKiem = (RadioButton)item.FindControl("RbtnHanhKiem");
                    if (rbtnHanhKiem.Checked)
                    {
                        HiddenField selectedHdfMaHanhKiem = (HiddenField)item.FindControl("HdfMaHanhKiem");
                        if (((orgMaHanhKiemHocSinh == null) && (selectedHdfMaHanhKiem.Value != "0"))
                        || ((orgMaHanhKiemHocSinh != null) && (selectedHdfMaHanhKiem.Value != orgMaHanhKiemHocSinh.ToString())))
                        {
                            int? iSelectedMaHanhKiem = null;
                            if (selectedHdfMaHanhKiem.Value != "0")
                            {
                                iSelectedMaHanhKiem = Int32.Parse(selectedHdfMaHanhKiem.Value);
                            }
                            int maHocSinh = Int32.Parse(hdfMaHocSinh.Value);
                            dicHocSinhHanhKiem.Add(maHocSinh, iSelectedMaHanhKiem);
                        }
                    }
                }
            }

            term = new CauHinh_HocKy();
            term.MaHocKy = Int32.Parse(DdlHocKy.SelectedValue);

            Class = new LopHoc_Lop();
            Class.MaLopHoc = Int32.Parse(DdlLopHoc.SelectedValue);

            foreach (KeyValuePair<int, int?> pair in dicHocSinhHanhKiem)
            {
                student = new HocSinh_ThongTinCaNhan();
                student.MaHocSinh = pair.Key;

                if (pair.Value != null)
                {
                    conduct = new DanhMuc_HanhKiem();
                    conduct.MaHanhKiem = (int)pair.Value;
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
        int? maHanhKiem;
        protected void RptHanhKiemHocSinh_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem rptItem = e.Item;
            if (rptItem.ItemType == ListItemType.Item
                || rptItem.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdfMaHanhKiemHocSinh = (HiddenField)rptItem.FindControl("HdfMaHanhKiemHocSinh");
                int? maHanhKiemHocSinh = null;
                try
                {
                    maHanhKiemHocSinh = Int32.Parse(hdfMaHanhKiemHocSinh.Value);
                }
                catch (Exception ex) { }
                maHanhKiem = maHanhKiemHocSinh;

                Repeater RptHanhKiem = (Repeater)e.Item.FindControl("RptHanhKiem");
                List<DanhMuc_HanhKiem> lstHanhKiem = hanhKiemBL.GetListConducts(true);
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
                HiddenField hdfMaHanhKiem = (HiddenField)rptItem.FindControl("HdfMaHanhKiem");
                RadioButton rbtnHanhKiem = (RadioButton)rptItem.FindControl("RbtnHanhKiem");
                if (((maHanhKiem == null) && (hdfMaHanhKiem.Value == "0"))
                    || ((maHanhKiem != null) && (hdfMaHanhKiem.Value == maHanhKiem.ToString())))
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