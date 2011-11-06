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
        private KetQuaHocTapBL ketQuaHocTapBL;

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

            ketQuaHocTapBL = new KetQuaHocTapBL();
            
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
            NamHocBL namHocBL = new NamHocBL();
            List<CauHinh_NamHoc> lstNamHoc = namHocBL.GetListNamHoc();
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
            HocKyBL hocKyBL = new HocKyBL();
            List<CauHinh_HocKy> lstHocKy = hocKyBL.GetListHocKy();
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

            int maNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);

            int maNganhHoc = 0;
            try
            {
                maNganhHoc = Int32.Parse(DdlNganh.SelectedValue);
            }
            catch (Exception) { }

            int maKhoiLop = 0;
            try
            {
                maKhoiLop = Int32.Parse(DdlKhoiLop.SelectedValue);
            }
            catch (Exception) { }

            LopHocBL lopHocBL = new LopHocBL();
            List<LopHoc_Lop> lstLop = lopHocBL.GetListLopHoc(maNganhHoc, maKhoiLop, maNamHoc);
            DdlLopHoc.DataSource = lstLop;
            DdlLopHoc.DataValueField = "MaLopHoc";
            DdlLopHoc.DataTextField = "TenLopHoc";
            DdlLopHoc.DataBind();
        }

        private void BindDDLMonHoc()
        {
            if (DdlLopHoc.Items.Count == 0)
            {
                DdlMonHoc.DataSource = null;
            }
            else
            {
                int maLopHoc = Int32.Parse(DdlLopHoc.SelectedValue);
                int maHocKy = Int32.Parse(DdlHocKy.SelectedValue);
                ThoiKhoaBieuBL thoiKhoaBieuBL = new ThoiKhoaBieuBL();
                List<DanhMuc_MonHoc> lstMonHoc = thoiKhoaBieuBL.GetListMonHoc(maLopHoc, maHocKy);
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
            MarkTypeBL markTypeBL = new MarkTypeBL();
            List<DanhMuc_LoaiDiem> markTypes = new List<DanhMuc_LoaiDiem>();

            if (DdlLopHoc.Items.Count == 0 || DdlMonHoc.Items.Count == 0
                || DdlLoaiDiem.Items.Count == 0)
            {
                ProcDisplayGUI(false);
                return;
            }

            int maLopHoc = Int32.Parse(DdlLopHoc.SelectedValue);
            int maMonHoc = Int32.Parse(DdlMonHoc.SelectedValue);
            int maHocKy = Int32.Parse(DdlHocKy.SelectedValue);
            //int maLoaiDiem = Int32.Parse(DdlLoaiDiem.SelectedValue);

            if(DdlLoaiDiem.SelectedIndex == 0)
            {
                markTypes = markTypeBL.GetListMarkTypes();
            }
            else
            {
                string markTypeName = DdlLoaiDiem.SelectedValue;
                markTypes.Add(markTypeBL.GetMarkType(markTypeName));
            }            

            double totalRecords;
            List<TabularDiemHocSinh> lstTbDiemHocSinh;
            lstTbDiemHocSinh = ketQuaHocTapBL.GetListDiemHocSinh(maLopHoc, maMonHoc, maHocKy,
                markTypes,
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
                if (Int32.Parse(DdlLoaiDiem.SelectedValue) == 0)
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
            int maHocKy = Int32.Parse(DdlHocKy.SelectedValue);
            int maLopHoc = Int32.Parse(DdlLopHoc.SelectedValue);
            int maMonHoc = Int32.Parse(DdlMonHoc.SelectedValue);
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

                ketQuaHocTapBL.UpdateChiTietDiem(maHocSinh, maLopHoc, maHocKy, maMonHoc, lDiems);
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
                TabularDiemHocSinh tbDiemHocSinh = (TabularDiemHocSinh)e.Item.DataItem;
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