using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class ThemGiaoVienChuNhiemPage : BaseContentPage
    {
        #region Fields
        private TeacherBL teacherBL;
        private bool isSearch;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (isAccessDenied)
            {
                return;
            }

            teacherBL = new TeacherBL();
            
            if (!Page.IsPostBack)
            {
                BindDropDownLists();
                BindRepeater();
            }
        }
        #endregion

        #region Repeater event handlers
        protected void RptGiaoVien_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            
        }

        protected void RptGiaoVien_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {                
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
            MainDataPager.ItemCount = 0;
            isSearch = true;
            BindRepeater();
        }
        
        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
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
                            LopHoc_Lop Class = new LopHoc_Lop();
                            LopHoc_GiaoVien teacher = new LopHoc_GiaoVien();
                            FormerTeacherBL gvcnBL = new FormerTeacherBL();

                            HiddenField hdfRptMaGiaoVien = (HiddenField)item.FindControl("HdfRptMaGiaoVien");
                            int maGiaoVien = Int32.Parse(hdfRptMaGiaoVien.Value);
                            int maLopHoc = Int32.Parse(DdlLopHoc.SelectedValue);
                            
                            Class.MaLopHoc = maLopHoc;                            
                            teacher.MaGiaoVien = maGiaoVien;                            
                            gvcnBL.Insert(Class, teacher);
                            Response.Redirect("giaovienchunhiem.aspx");
                        }
                    }
                }
            }            
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("giaovienchunhiem.aspx");
        }
        #endregion

        #region DropDownList event hanlders
        protected void DdlNamHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropDownListLopHoc();
            BindRepeater();

            LblTitleTeacherList.Text = string.Format("DANH SÁCH GIÁO VIÊN CHƯA PHÂN CÔNG CHỦ NHIỆM (NĂM HỌC {0})", DdlNamHoc.SelectedItem.Text);
        }

        protected void DdlNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropDownListLopHoc();
        }

        protected void DdlKhoiLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropDownListLopHoc();
        }
        #endregion
        
        #region DataPager event handlers
        public void DataPager_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currnetPageIndx;
            BindRepeater();
        }
        #endregion

        #region Methods
        private void BindDropDownLists()
        {
            BindDropDownListNamHoc();
            BindDropDownListNganhHoc();
            BindDropDownListKhoiLop();
            BindDropDownListLopHoc();
        }

        private void BindDropDownListKhoiLop()
        {
            GradeBL khoiLopBL = new GradeBL();
            List<DanhMuc_KhoiLop> lstKhoiLop = khoiLopBL.GetListGrades();
            DdlKhoiLop.DataSource = lstKhoiLop;
            DdlKhoiLop.DataValueField = "MaKhoiLop";
            DdlKhoiLop.DataTextField = "TenKhoiLop";
            DdlKhoiLop.DataBind();
            if (lstKhoiLop.Count > 1)
            {
                DdlKhoiLop.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }

        private void BindDropDownListNganhHoc()
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

        private void BindDropDownListNamHoc()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL();
            List<CauHinh_NamHoc> lstNamHoc = systemConfigBL.GetListYears();
            DdlNamHoc.DataSource = lstNamHoc;
            DdlNamHoc.DataValueField = "MaNamHoc";
            DdlNamHoc.DataTextField = "TenNamHoc";
            DdlNamHoc.DataBind();

            if (DdlNamHoc.Items.Count != 0)
            {
                BindRepeater();
                LblTitleTeacherList.Text = string.Format("DANH SÁCH GIÁO VIÊN CHƯA PHÂN CÔNG CHỦ NHIỆM (NĂM HỌC {0})", DdlNamHoc.SelectedItem.Text);
            }
            else
            {
                ProcDisplayInfo(false);
            }
        }

        private void BindDropDownListLopHoc()
        {
            CauHinh_NamHoc year = null;
            DanhMuc_NganhHoc faculty = null;
            DanhMuc_KhoiLop grade = null;

            bool bEnabled;
            if (DdlNamHoc.Items.Count == 0 || DdlNganh.Items.Count == 0 || DdlKhoiLop.Items.Count == 0)
            {
                bEnabled = false;
            }
            else
            {
                int maNamHoc = Int32.Parse(DdlNamHoc.SelectedValue);
                year.MaNamHoc = maNamHoc;
                int maNganhHoc = Int32.Parse(DdlNganh.SelectedValue);
                faculty.MaNganhHoc = maNganhHoc;
                int maKhoiLop = Int32.Parse(DdlKhoiLop.SelectedValue);
                grade.MaKhoiLop = maKhoiLop;

                List<LopHoc_Lop> lstLop = (new ClassBL()).GetUnformeredClasses(year, faculty, grade);
                DdlLopHoc.DataSource = lstLop;
                DdlLopHoc.DataValueField = "MaLopHoc";
                DdlLopHoc.DataTextField = "TenLopHoc";
                DdlLopHoc.DataBind();

                if (DdlLopHoc.Items.Count == 0)
                {
                    bEnabled = false;
                }
                else
                {
                    bEnabled = true;
                }
            }

            BtnSave.Enabled = bEnabled;
            BtnSave.ImageUrl = (bEnabled) ? "~/Styles/Images/button_save.png" : "~/Styles/Images/button_save_disable.png";
        }

        private void BindRepeater()
        {
            CauHinh_NamHoc year = new CauHinh_NamHoc();
            int yearId = Int32.Parse(DdlNamHoc.SelectedValue);
            year.MaNamHoc = yearId;
            string maHienThiGiaoVien = TxtSearchMaHienThiGiaoVien.Text.Trim();
            string hoTen = TxtSearchTenGiaoVien.Text.Trim();


            double totalRecords;
            List<TabularTeacher> lstTbGiaoViens = teacherBL.GetListTabularUnformeredTeachers(
                year,
                maHienThiGiaoVien, hoTen,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out totalRecords);

            // Decrease page current index when delete
            if (lstTbGiaoViens.Count == 0 && totalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRepeater();
                return;
            }

            bool bDisplayData = (lstTbGiaoViens.Count != 0) ? true : false;
            ProcDisplayInfo(bDisplayData);           

            RptGiaoVien.DataSource = lstTbGiaoViens;
            RptGiaoVien.DataBind();
            MainDataPager.ItemCount = totalRecords;

            if (bDisplayData)
            {
                foreach (RepeaterItem item in RptGiaoVien.Items)
                {
                    if (item.ItemType == ListItemType.Item 
                        || item.ItemType == ListItemType.AlternatingItem)
                    {
                        Control control = item.FindControl("RBtnSelect");
                        if (control != null)
                        {
                            RadioButton rBtnSelect = (RadioButton)control;
                            rBtnSelect.Checked = true;
                            return;
                        }
                    }
                }
            }
        }

        public void ProcDisplayInfo(bool bDisplay)
        {
            RptGiaoVien.Visible = bDisplay;
            LblSearchResult.Visible = !bDisplay;
            bool b = (DdlLopHoc.Items.Count != 0) && bDisplay;
            BtnSave.Enabled = b;
            BtnSave.ImageUrl = (b) ? "~/Styles/Images/button_save.png" : "~/Styles/Images/button_save_disable.png";
            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin giáo viên";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy giáo viên";
                }

                MainDataPager.CurrentIndex = 1;
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
            }
            else
            {
                MainDataPager.Visible = true;
            }
        }
        #endregion
    }
}