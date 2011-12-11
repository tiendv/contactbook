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
    public partial class SuaGiaoVienChuNhiemPage : BaseContentPage
    {
        #region Fields
        private FormerTeacherBL giaoVienChuNhiemBL;
        private TeacherBL teacherBL;
        private int maGVCN;
        private int YearId;
        private Guid UserIdHienHanh;
        private bool isSearch;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (accessDenied)
            {
                return;
            }

            giaoVienChuNhiemBL = new FormerTeacherBL(UserSchool);
            teacherBL = new TeacherBL(UserSchool);

            if (Request.QueryString["id"] != null)
            {
                maGVCN = Int32.Parse(Request.QueryString["id"]);
                ViewState["maGVCN"] = maGVCN;
            }
            else
            {
                maGVCN = (int)ViewState["maGVCN"];
            }

            Class_FormerTeacher giaoVienChuNhiem = giaoVienChuNhiemBL.GetFormerTeacher(maGVCN);
            TabularClass lopHoc = (new ClassBL(UserSchool)).GetTabularClass(giaoVienChuNhiem.Class_Class);
            YearId = lopHoc.YearId;

            if (!Page.IsPostBack)
            {
                LblLopHoc.Text = lopHoc.ClassName;
                UserIdHienHanh = giaoVienChuNhiem.TeacherId;
                LblCurrentGiaoVienChuNhiem.Text = lopHoc.TenGVCN;
                LblTitleTeacherList.Text = string.Format("DANH SÁCH GIÁO VIÊN CHƯA PHÂN CÔNG CHỦ NHIỆM (NĂM HỌC {0})",
                    lopHoc.YearName);
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
                            HiddenField hdfRptUserId = (HiddenField)item.FindControl("HdfRptUserId");
                            Guid UserId = new Guid(hdfRptUserId.Value);
                            aspnet_User teacher = new aspnet_User();
                            teacher.UserId = UserId;
                            giaoVienChuNhiemBL.Update(maGVCN, teacher);
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

        #region DataPager event handlers
        public void DataPager_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currnetPageIndx;
            BindRepeater();
        }
        #endregion

        #region Methods
        private void BindRepeater()
        {
            Configuration_Year year = new Configuration_Year();
            year.YearId = YearId;

            string teacherCode = TxtSearchMaHienThiGiaoVien.Text.Trim();
            string teacherName = TxtSearchTenGiaoVien.Text.Trim();

            double dTotalRecords;
            List<TabularTeacher> lTbTeachers = teacherBL.GetTabularUnformeredTeachers(
                year,
                teacherCode, teacherName,
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            // Decrease page current index when delete
            if (lTbTeachers.Count == 0 && dTotalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRepeater();
                return;
            }

            bool bDisplayData = (lTbTeachers.Count != 0) ? true : false;
            ProcDisplayInfo(bDisplayData);

            RptGiaoVien.DataSource = lTbTeachers;
            RptGiaoVien.DataBind();
            MainDataPager.ItemCount = dTotalRecords;

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
            BtnSave.Enabled = bDisplay;
            BtnSave.ImageUrl = (bDisplay) ? "~/Styles/Images/button_save.png" : "~/Styles/Images/button_save_disable.png";
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