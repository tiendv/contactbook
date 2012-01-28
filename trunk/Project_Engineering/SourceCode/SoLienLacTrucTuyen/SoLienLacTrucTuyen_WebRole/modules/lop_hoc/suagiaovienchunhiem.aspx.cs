using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class FormerTeacherModifyPage : BaseContentPage
    {
        #region Fields
        private FormerTeacherBL formerTeacherBL;
        private TeacherBL teacherBL;
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

            if (sessionExpired)
            {
                FormsAuthentication.SignOut();
                Response.Redirect(FormsAuthentication.LoginUrl);
            }

            formerTeacherBL = new FormerTeacherBL(UserSchool);
            teacherBL = new TeacherBL(UserSchool);

            Class_FormerTeacher formerTeacher = null;
            if (CheckSessionKey(AppConstant.SESSION_SELECTED_FORMERTEACHER))
            {
                formerTeacher = (Class_FormerTeacher)GetSession(AppConstant.SESSION_SELECTED_FORMERTEACHER);
                ViewState["maGVCN"] = formerTeacher.FormerTeacherId;
            }
            else
            {
                
                if (ViewState["maGVCN"] != null)
                {
                    formerTeacher = new Class_FormerTeacher();
                    formerTeacher.FormerTeacherId = (int)ViewState["maGVCN"];
                }
                else
                {
                    Response.Redirect(AppConstant.PAGEPATH_FORMERTEACHER_LIST);
                }
            }

            formerTeacher = formerTeacherBL.GetFormerTeacher(formerTeacher.FormerTeacherId);
            TabularClass lopHoc = (new ClassBL(UserSchool)).GetTabularClass(formerTeacher.Class_Class);
            YearId = lopHoc.YearId;

            if (!Page.IsPostBack)
            {
                LblLopHoc.Text = lopHoc.ClassName;
                ViewState["CurrentTeacherId"] = formerTeacher.TeacherId;
                UserIdHienHanh = formerTeacher.TeacherId;
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
            aspnet_User oldTeacher = new aspnet_User();
            oldTeacher.UserId = new Guid(ViewState["CurrentTeacherId"].ToString());

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
                            aspnet_User newTeacher = new aspnet_User();
                            newTeacher.UserId = UserId;
                            formerTeacherBL.Update((int)ViewState["maGVCN"], oldTeacher, newTeacher);
                            Response.Redirect(AppConstant.PAGEPATH_FORMERTEACHER_LIST);
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
            BtnSave.ImageUrl = (bDisplay) ? "~/Styles/buttons/button_save.png" : "~/Styles/buttons/button_save_disable.png";
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