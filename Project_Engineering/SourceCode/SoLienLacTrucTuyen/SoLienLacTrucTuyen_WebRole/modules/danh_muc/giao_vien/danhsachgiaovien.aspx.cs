using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EContactBook.DataAccess;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.BusinessEntity;
using AjaxControlToolkit;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class TeachersPage : BaseContentPage, IPostBackEventHandler
    {
        #region Fields
        private TeacherBL teacherBL;
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

            teacherBL = new TeacherBL(UserSchool);

            if (!Page.IsPostBack)
            {
                isSearch = false;
                MainDataPager.CurrentIndex = 1;

                BindRptTeachers();
            }

            ProcPermissions();
        }
        #endregion

        #region Methods
        public void BindRptTeachers()
        {
            // declare variables
            List<TabularTeacher> tabularTeachers = null;
            string strTeacherCode = TxtSearchMaHienThiGiaoVien.Text.Trim();
            string strTeacherName = TxtSearchTenGiaoVien.Text.Trim();
            double dTotalRecords;

            // get list of teachers
            tabularTeachers = teacherBL.GetTabularTeachers(strTeacherCode, strTeacherName,
                 MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            // decrease page current index when delete
            if (tabularTeachers.Count == 0 && dTotalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRptTeachers();
                return;
            }

            bool bDisplayData = (tabularTeachers.Count != 0) ? true : false;
            //PnlPopupConfirmDelete.Visible = bDisplayData;
            RptGiaoVien.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = string.Format(
                        (string)GetGlobalResourceObject(AppConstant.FILENAME_MAINRESOURCE, AppConstant.RESOURCE_SEARCH_NOINFO),
                        (string)GetGlobalResourceObject(AppConstant.FILENAME_MAINRESOURCE, AppConstant.TEACHER));
                }
                else
                {
                    LblSearchResult.Text = string.Format(
                        (string)GetGlobalResourceObject(AppConstant.FILENAME_MAINRESOURCE, AppConstant.RESOURCE_SEARCH_NOMATCH),
                        (string)GetGlobalResourceObject(AppConstant.FILENAME_MAINRESOURCE, AppConstant.TEACHER));
                }

                MainDataPager.CurrentIndex = 1;
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;

                BtnExport.Enabled = false;
                BtnExport.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_EXPORT_DISABLED;
            }
            else
            {
                MainDataPager.Visible = true;

                BtnExport.Enabled = true;
                BtnExport.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_EXPORT;
            }

            RptGiaoVien.DataSource = tabularTeachers;
            RptGiaoVien.DataBind();
            MainDataPager.ItemCount = dTotalRecords;
        }

        private void ProcPermissions()
        {
            if (accessibilities.Contains(AccessibilityEnum.Add))
            {
                BtnAddGiaoVien.Visible = true;
                BtnAddGiaoVien.ImageUrl = "~/Styles/buttons/button_add.png";
            }
            else
            {
                BtnAddGiaoVien.Visible = false;
            }
        }

        protected void PrePrint()
        {
            #region Add Info 2 Session
            string strTeacherName = this.TxtSearchTenGiaoVien.Text;
            string strTeacherID = this.TxtSearchMaHienThiGiaoVien.Text;

            AddSession(AppConstant.SESSION_PAGEPATH, AppConstant.PAGEPATH_PRINTTEACHERS);
            AddSession(AppConstant.SESSION_TEACHERID, strTeacherID);
            AddSession(AppConstant.SESSION_TEACHERNAME, strTeacherName);
            //Response.Redirect(AppConstant.PAGEPATH_PRINTSTUDENTS);
            #endregion
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            PrePrint();
        }
        #endregion

        #region Button click event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            MainDataPager.ItemCount = 0;
            isSearch = true;
            BindRptTeachers();
        }

        protected void BtnPrint_Click(object sender, ImageClickEventArgs e)
        {
            #region Add Info 2 Session
            string strTeacherName = this.TxtSearchTenGiaoVien.Text;
            string strTeacherID = this.TxtSearchMaHienThiGiaoVien.Text;

            AddSession(AppConstant.SESSION_PAGEPATH, AppConstant.PAGEPATH_PRINTTEACHERS);
            AddSession(AppConstant.SESSION_TEACHERID, strTeacherID);
            AddSession(AppConstant.SESSION_TEACHERNAME, strTeacherName);
            Response.Redirect(AppConstant.PAGEPATH_STUDENT_PRINT);
            #endregion
        }

        protected void BtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/modules/danh_muc/giao_vien/themgiaovien.aspx");
        }

        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            HiddenField HdfRptUserId = null;
            foreach (RepeaterItem item in RptGiaoVien.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox CkbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (CkbxSelect.Checked)
                    {
                        HdfRptUserId = (HiddenField)item.FindControl("HdfRptUserId");
                        aspnet_User teacher = new aspnet_User();
                        teacher.UserId = new Guid(HdfRptUserId.Value);
                        AddSession(AppConstant.SESSION_SELECTED_USER, teacher);
                        AddSession(AppConstant.SESSION_PREVIOUSPAGE, AppConstant.PAGEPATH_TEACHER_LIST);
                        Response.Redirect(AppConstant.PAGEPATH_TEACHER_EDIT);
                        return;
                    }
                }
            }
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            aspnet_User teacher = new aspnet_User();
            teacher.UserId = new Guid(HdfUserId.Value);
            //teacherBL.DeleteTeacher(teacher);

            isSearch = false;
            BindRptTeachers();
        }
        #endregion

        #region Repeater event handlers
        protected void RptGiaoVien_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                e.Item.FindControl("thSelectAll").Visible = (accessibilities.Contains(AccessibilityEnum.Modify) || accessibilities.Contains(AccessibilityEnum.Delete));
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.FindControl("tdSelect").Visible = (accessibilities.Contains(AccessibilityEnum.Modify) || accessibilities.Contains(AccessibilityEnum.Delete));
            }
        }

        protected void RptGiaoVien_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDetailItem":
                    {
                        aspnet_User teacher = new aspnet_User();
                        teacher.UserId = new Guid(e.CommandArgument.ToString());
                        AddSession(AppConstant.SESSION_SELECTED_USER, teacher);
                        
                        Response.Redirect(AppConstant.PAGEPATH_TEACHER_DETAIL);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        #endregion

        #region Pager event handlers
        public void DataPager_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currnetPageIndx;
            BindRptTeachers();
        }
        #endregion
    }
}