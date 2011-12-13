using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EContactBook.DataAccess;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.BusinessEntity;
using AjaxControlToolkit;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class TeachersPage : BaseContentPage
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
            }
            else
            {
                MainDataPager.Visible = true;
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
                BtnAddGiaoVien.ImageUrl = "~/Styles/Images/button_add_with_text.png";
            }
            else
            {
                BtnAddGiaoVien.Visible = false;
            }
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

            AddSession(AppConstant.SESSION_TEACHERID, strTeacherID);
            AddSession(AppConstant.SESSION_TEACHERNAME, strTeacherName);
            Response.Redirect(AppConstant.PAGEPATH_PRINTTEACHERS);
            #endregion
        }
        protected void BtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/modules/danh_muc/giao_vien/themgiaovien.aspx");
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
            if (accessibilities.Contains(AccessibilityEnum.Modify))
            {
                if (e.Item.ItemType == ListItemType.Item
                    || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    TabularTeacher giaoVien = (TabularTeacher)e.Item.DataItem;
                    Guid UserId = giaoVien.UserId;
                    ImageButton btnEditItem = (ImageButton)e.Item.FindControl("BtnEditItem");
                    btnEditItem.Attributes.Add("OnClientClick",
                        "return editGiaoVien();");
                }
            }
            else
            {
                if (e.Item.ItemType == ListItemType.Header)
                {
                    e.Item.FindControl("thEdit").Visible = false;
                }

                if (e.Item.ItemType == ListItemType.Item ||
                    e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    e.Item.FindControl("tdEdit").Visible = false;
                }
            }

            if (accessibilities.Contains(AccessibilityEnum.Delete))
            {
                if (e.Item.ItemType == ListItemType.Item
                    || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    //TabularTeacher giaoVien = (TabularTeacher)e.Item.DataItem;
                    //if (!teacherBL.IsDeletable(giaoVien.UserId))
                    //{
                    //    ImageButton btnDeleteItem = (ImageButton)e.Item.FindControl("BtnDeleteItem");
                    //    btnDeleteItem.ImageUrl = "~/Styles/Images/button_delete_disable.png";
                    //    btnDeleteItem.Enabled = false;
                    //}
                }
            }
            else
            {
                if (e.Item.ItemType == ListItemType.Header)
                {
                    e.Item.FindControl("thDelete").Visible = false;
                }

                if (e.Item.ItemType == ListItemType.Item ||
                    e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    e.Item.FindControl("tdDelete").Visible = false;
                }

                //this.PnlPopupConfirmDelete.Visible = false;
            }

            if (e.Item.ItemType == ListItemType.Item
                    || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TabularTeacher giaoVien = (TabularTeacher)e.Item.DataItem;
                HyperLink hlkUserId = (HyperLink)e.Item.FindControl("HlkUserId");
                hlkUserId.NavigateUrl = "chitietgiaovien.aspx?giaovien=" + giaoVien.UserId
                    + "&prevpageid=1";
            }
        }

        protected void RptGiaoVien_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDeleteItem":
                    {
                        aspnet_User teacher = null;

                        string strTeacherCode = (string)e.CommandArgument;
                        teacher = teacherBL.GetTeacher(strTeacherCode);
                        HdfTeacherCode.Value = strTeacherCode;

                        //this.LblConfirmDelete.Text = "Bạn có chắc xóa giáo viên <b>\""
                        //    + teacher.aspnet_Membership.FullName + "\"</b> này không?";
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        this.HdfUserId.Value = teacher.UserId.ToString();
                        this.HdfRptGiaoVienMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        Response.Redirect("suagiaovien.aspx?giaovien=" + e.CommandArgument + "&prevpageid=1");
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