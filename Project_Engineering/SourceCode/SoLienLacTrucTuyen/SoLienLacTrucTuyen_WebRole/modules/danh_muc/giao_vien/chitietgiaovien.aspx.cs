﻿using System;
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
    public partial class DetailedTeacherPage : BaseContentPage
    {
        #region Fields
        TeacherBL teacherBL;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (isAccessDenied)
            {
                return;
            }

            teacherBL = new TeacherBL(UserSchool);

            if (!Page.IsPostBack)
            {
                ViewState["prevpageid"] = Request.QueryString["prevpageid"];

                string maGiaoVien = Request.QueryString["giaovien"];
                ViewState["magiaovien"] = maGiaoVien;                
                FillGiaoVien(new Guid(maGiaoVien));
                BindDataChuNhiem();
                BindDataGiangDay();
                ProcPermissions();
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnSua_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(string.Format("suagiaovien.aspx?giaovien={0}&prevpageid={1}",
                ViewState["magiaovien"], 4));
        }

        protected void BtnBackPrevPage_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/modules/danh_muc/giao_vien/danhsachgiaovien.aspx");
        }
        #endregion

        #region Methods
        private void FillGiaoVien(Guid teacherId)
        {
            aspnet_User teacher = teacherBL.GetTeacher(teacherId);
            LblMaGiaoVienHienThi.Text = teacher.UserName.Split('_')[1];
            LblTenGiaoVien.Text = teacher.aspnet_Membership.RealName;
            if (teacher.aspnet_Membership.Birthday != null)
            {
                LblNgaySinh.Text = ((DateTime)teacher.aspnet_Membership.Birthday).ToShortDateString();
            }
            if (teacher.aspnet_Membership.Gender != null)
            {
                LblGioiTinh.Text = (bool)teacher.aspnet_Membership.Gender ? "Nam" : "Nữ";
            }
            
            LblDiaChi.Text = teacher.aspnet_Membership.Address;
            LblDienThoai.Text = (teacher.aspnet_Membership.Phone != "") ? teacher.aspnet_Membership.Phone : "(không có)";
        }

        private void ProcPermissions()
        {
            if (lstAccessibilities.Contains(AccessibilityEnum.Modify))
            {
                // do something
            }
            else
            {
                BtnSua.Visible = false;
            }
        }
        #endregion

        #region Pager event handlers
        public void DataPagerChuNhiem_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            this.DataPagerChuNhiem.CurrentIndex = currnetPageIndx;
            BindDataChuNhiem();
        }

        public void DataPagerGiangDay_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            this.DataPagerGiangDay.CurrentIndex = currnetPageIndx;
            BindDataGiangDay();
        }

        private void BindDataGiangDay()
        {
            aspnet_User teacher = new aspnet_User();
            Guid maGiaoVien = new Guid(ViewState["magiaovien"].ToString());            
            teacher.UserId = maGiaoVien;
            double dTotalRecords;
            List<TabularTeaching> lstTbGiangDays = teacherBL.GetListTeachings(
                teacher, DataPagerGiangDay.CurrentIndex, DataPagerGiangDay.PageSize, out dTotalRecords);

            bool bDisplayData = (lstTbGiangDays.Count != 0) ? true : false;
            RptGiangDay.Visible = bDisplayData;
            DataPagerGiangDay.Visible = bDisplayData;
            LblSearchResultGiangDay.Visible = !bDisplayData;

            RptGiangDay.DataSource = lstTbGiangDays;
            RptGiangDay.DataBind();
            DataPagerGiangDay.ItemCount = dTotalRecords;
        }

        private void BindDataChuNhiem()
        {
            aspnet_User teacher = new aspnet_User();
            teacher.UserId = new Guid(ViewState["magiaovien"].ToString());
            double dTotalRecords;
            List<TabularFormering> lstTbChuNhiems = teacherBL.GetListFormerings(
                teacher, DataPagerChuNhiem.CurrentIndex, DataPagerChuNhiem.PageSize, out dTotalRecords);

            bool bDisplayData = (lstTbChuNhiems.Count != 0) ? true : false;
            RptChuNhiem.Visible = bDisplayData;
            DataPagerChuNhiem.Visible = bDisplayData;
            LblSearchResultChuNhiem.Visible = !bDisplayData;

            RptChuNhiem.DataSource = lstTbChuNhiems;
            RptChuNhiem.DataBind();
            DataPagerChuNhiem.ItemCount = dTotalRecords;
        }
        #endregion
    }
}