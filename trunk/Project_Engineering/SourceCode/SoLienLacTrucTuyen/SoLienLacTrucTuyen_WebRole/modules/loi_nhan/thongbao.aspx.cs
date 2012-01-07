﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EContactBook.BusinessEntity;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using AjaxControlToolkit;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class LoiNhanKhan : BaseContentPage
    {
        #region Fields
        private bool isSearch;
        private MessageBL loiNhanKhanBL;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (accessDenied)
            {
                // User can not access this page
                return;
            }

            if (sessionExpired)
            {
                FormsAuthentication.SignOut();
                Response.Redirect(FormsAuthentication.LoginUrl);
            }

            loiNhanKhanBL = new MessageBL(UserSchool);
            if (!Page.IsPostBack)
            {
                isSearch = false;
                BindDropDownLists();
                InitDates();
                if (DdlNamHoc.Items.Count != 0)
                {
                    BindRptMessages();
                }
            }
        }
        #endregion

        #region Methods
        private void BindRptMessages()
        {
            Configuration_Year year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);
            DateTime tuNgay = DateTime.Parse(TxtTuNgay.Text);
            DateTime denNgay = DateTime.Parse(TxtDenNgay.Text);
            string strStudentCode = TxtMaHS.Text;
            ConfigurationMessageStatus messageStatus = null;
            if (DdlXacNhan.SelectedIndex > 0)
            {
                messageStatus = new ConfigurationMessageStatus();
                messageStatus.MessageStatusId = Int32.Parse(DdlXacNhan.SelectedValue);
            }

            double dTotalRecords;
            List<TabularMessage> lstTabularLoiNhanKhan = loiNhanKhanBL.GetTabularMessages(
                year, tuNgay, denNgay,
                strStudentCode, messageStatus, MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            if (lstTabularLoiNhanKhan.Count == 0 && dTotalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRptMessages();
                return;
            }

            bool bDisplayData = (lstTabularLoiNhanKhan.Count != 0) ? true : false;
            ProcessDislayInfo(bDisplayData);

            RptLoiNhanKhan.DataSource = lstTabularLoiNhanKhan;
            RptLoiNhanKhan.DataBind();
            MainDataPager.ItemCount = dTotalRecords;
        }

        private void ProcessDislayInfo(bool bDisplayData)
        {
            PnlPopupConfirmDelete.Visible = bDisplayData;
            PnlPopupEdit.Visible = bDisplayData;
            RptLoiNhanKhan.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin lời nhắn khẩn";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy lời nhắn khẩn";
                }
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
            }
            else
            {
                MainDataPager.Visible = true;
            }
        }

        private void BindDropDownLists()
        {
            BindDDLNamHoc();
            BindDDLMessageStatuses();
        }

        private void BindDDLNamHoc()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<Configuration_Year> years = systemConfigBL.GetListYears();
            DdlNamHoc.DataSource = years;
            DdlNamHoc.DataValueField = "YearId";
            DdlNamHoc.DataTextField = "YearName";
            DdlNamHoc.DataBind();

            if (DdlNamHoc.Items.Count != 0)
            {
                BtnSearch.Enabled = true;
                BtnSearch.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SEARCH;

                BtnAdd.Enabled = true;
                BtnAdd.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_ADD;

                DdlNamHoc.SelectedValue = (new SystemConfigBL(UserSchool)).GetLastedYear().ToString();
            }
            else
            {
                BtnSearch.Enabled = false;
                BtnSearch.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SEARCH_DISABLE;

                BtnAdd.Enabled = false;
                BtnAdd.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_ADD_DISABLE;

                ProcessDislayInfo(false);
            }  
        }

        private void BindDDLMessageStatuses()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<ConfigurationMessageStatus> messageStatuses = systemConfigBL.GetMessageStatuses();
            DdlXacNhan.DataSource = messageStatuses;
            DdlXacNhan.DataValueField = "MessageStatusId";
            DdlXacNhan.DataTextField = "MessageStatusName";
            DdlXacNhan.DataBind();

            DdlXacNhan.Items.Insert(0, new ListItem("Tất cả", "0")); 
        }

        private void InitDates()
        {
            DateTime today = DateTime.Now;
            TxtTuNgay.Text = today.AddMonths(-1).ToShortDateString();
            TxtDenNgay.Text = today.AddMonths(1).ToShortDateString();

            // dont remove this code
            //DateTime today = DateTime.Now;
            //DateTime beginDateOfMonth = new DateTime(today.Year, today.Month, 1);
            //TxtTuNgay.Text = beginDateOfMonth.ToShortDateString();
            //DateTime dateOfNextMonth = today.AddMonths(1);
            //DateTime beginDateOfNextMonth = new DateTime(dateOfNextMonth.Year, dateOfNextMonth.Month, 1);
            //DateTime endDateOfMonth = beginDateOfNextMonth.AddDays(-1);
            //TxtDenNgay.Text = endDateOfMonth.ToShortDateString();
        }
        #endregion

        #region Repeater event handlers
        protected void RptLoiNhanKhan_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //LopHocInfo lopHoc = (LopHocInfo)e.Item.DataItem;
                //if (lopHoc != null)
                //{
                //    int ClassId = lopHoc.ClassId;
                //    if (!lopHocBL.CheckCanDeleteLopHoc(ClassId))
                //    {
                //        ImageButton btnDeleteItem = (ImageButton)e.Item.FindControl("BtnDeleteItem");
                //        btnDeleteItem.ImageUrl = "~/Styles/Images/button_delete_disable.png";
                //        btnDeleteItem.Enabled = false;
                //    }
                //}
            }
        }

        protected void RptLoiNhanKhan_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDeleteItem":
                    {
                        this.LblConfirmDelete.Text = "Bạn có chắc xóa lời nhắn khẩn <b>" + e.CommandArgument + "</b> này không?";
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        HiddenField hdfRptMaLoiNhanKhan = (HiddenField)e.Item.FindControl("HdfRptMaLoiNhanKhan");
                        this.HdfMaLoiNhanKhan.Value = hdfRptMaLoiNhanKhan.Value;

                        this.HdfRptLoiNhanKhanMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        int maLoiNhanKhan = Int32.Parse(e.CommandArgument.ToString());
                        MessageToParents_Message loiNhanKhan = loiNhanKhanBL.GetMessage(maLoiNhanKhan);

                        LblTieuDeSua.Text = loiNhanKhan.Title;
                        TxtNoiDungSua.Text = TxtNoiDungSua.Text;
                        TxtNgaySua.Text = loiNhanKhan.Date.ToShortDateString();

                        Student_StudentInClass hocSinhLopHoc = (new StudentBL(UserSchool)).GetStudentInClass(loiNhanKhan.StudentInClassId);
                        LblMaHocSinhSua.Text = (new StudentBL(UserSchool)).GetStudent(hocSinhLopHoc.StudentId).StudentCode;                        
                        LblNganhHocSua.Text = hocSinhLopHoc.Class_Class.Category_Faculty.FacultyName;
                        LblKhoiSua.Text = hocSinhLopHoc.Class_Class.Category_Grade.GradeName;
                        LblLopSua.Text = hocSinhLopHoc.Class_Class.ClassName;

                        ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        mPEEdit.Show();

                        this.HdfMaLoiNhanKhan.Value = maLoiNhanKhan.ToString();
                        this.HdfRptLoiNhanKhanMPEEdit.Value = mPEEdit.ClientID;

                        break;
                    }
                case "CmdDetailItem":
                    {
                        //int ClassId = Int32.Parse(e.CommandArgument.ToString());
                        //Class_Class lophoc = lopHocBL.GetLopHoc(ClassId);

                        //LblClassNameChiTiet.Text = lophoc.ClassName;
                        //LblFacultyNameChiTiet.Text = (new facultyBL(UserSchool)).GetNganhHoc(lophoc.FacultyId).FacultyName;
                        //LblGradeNameChiTiet.Text = (new grades(UserSchool)).GetKhoiLop(lophoc.GradeId).GradeName;
                        //LblSiSoChiTiet.Text = lophoc.SiSo.ToString();
                        //ModalPopupExtender mPEDetail = (ModalPopupExtender)e.Item.FindControl("MPEDetail");
                        //mPEDetail.Show();

                        //this.HdfClassId.Value = ClassId.ToString();
                        //this.HdfRptLopHocMPEDetail.Value = mPEDetail.ClientID;
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
            isSearch = true;
            BindRptMessages();
        }

        protected void BtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(AppConstant.PAGEPATH_ADDMESSAGES);
        }
        
        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            int maLoiNhanKhan = Int32.Parse(this.HdfMaLoiNhanKhan.Value);
            loiNhanKhanBL.UpdateMessage(maLoiNhanKhan, TxtNoiDungSua.Text, DateTime.Parse(TxtNgaySua.Text));
            BindRptMessages();
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            int maLopNhanKhan = Int32.Parse(this.HdfMaLoiNhanKhan.Value);
            loiNhanKhanBL.DeleteMessage(maLopNhanKhan);
            isSearch = false;
            BindRptMessages();
        }
        #endregion

        #region Pager event handlers
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currentPageIndex;
            BindRptMessages();
        }
        #endregion
    }
}