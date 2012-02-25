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
    public partial class TeacherPersonalInfomationPage : BaseContentPage
    {
        #region Fields
        TeacherBL teacherBL;
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
                FillTeacherPersonalInformation();
                BindRptFormerings();
                BindRptTeachings();
            }
        }
        #endregion

        #region Methods
        private void FillTeacherPersonalInformation()
        {
            LblUserIdHienThi.Text = LogedInUser.UserName.Split(AppConstant.UNDERSCORE_CHAR)[1];

            LblTenGiaoVien.Text = LogedInUser.aspnet_Membership.FullName;

            if (LogedInUser.aspnet_Membership.Birthday != null)
            {
                LblNgaySinh.Text = ((DateTime)LogedInUser.aspnet_Membership.Birthday).ToString(AppConstant.DATEFORMAT_DDMMYYYY);
            }
            else
            {
                LblNgaySinh.Text = "(Chưa xác định)";
            }

            if (LogedInUser.aspnet_Membership.Gender != null)
            {
                LblGioiTinh.Text = (bool)LogedInUser.aspnet_Membership.Gender ? AppConstant.STRING_MALE : AppConstant.STRING_FEMALE;
            }
            else
            {
                LblGioiTinh.Text = "(Chưa xác định)";
            }


            if (CheckUntils.IsNullOrBlank(LogedInUser.aspnet_Membership.Address) == false)
            {
                LblDiaChi.Text = LogedInUser.aspnet_Membership.Address;
            }
            else
            {
                LblDiaChi.Text = "(Chưa xác định)";
            }

            if (CheckUntils.IsNullOrBlank(LogedInUser.aspnet_Membership.Phone) == false)
            {
                LblDienThoai.Text = LogedInUser.aspnet_Membership.Phone;
            }
            else
            {
                LblDienThoai.Text = "(Chưa xác định)";
            }
        }
        #endregion

        #region Pager event handlers
        public void DataPagerChuNhiem_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            this.DataPagerChuNhiem.CurrentIndex = currnetPageIndx;
            BindRptFormerings();
        }

        public void DataPagerGiangDay_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            this.DataPagerGiangDay.CurrentIndex = currnetPageIndx;
            BindRptTeachings();
        }

        private void BindRptTeachings()
        {
            double dTotalRecords;
            List<TabularTeaching> tabularTeachings = teacherBL.GetListTeachings(
                LogedInUser, DataPagerGiangDay.CurrentIndex, DataPagerGiangDay.PageSize, out dTotalRecords);

            bool bDisplayData = (tabularTeachings.Count != 0) ? true : false;
            RptGiangDay.Visible = bDisplayData;
            DataPagerGiangDay.Visible = bDisplayData;
            LblSearchResultGiangDay.Visible = !bDisplayData;

            RptGiangDay.DataSource = tabularTeachings;
            RptGiangDay.DataBind();
            DataPagerGiangDay.ItemCount = dTotalRecords;
        }

        private void BindRptFormerings()
        {
            double dTotalRecords;
            List<TabularFormering> tabularFormerings = teacherBL.GetListFormerings(
                LogedInUser, DataPagerChuNhiem.CurrentIndex, DataPagerChuNhiem.PageSize, out dTotalRecords);

            bool bDisplayData = (tabularFormerings.Count != 0) ? true : false;
            RptChuNhiem.Visible = bDisplayData;
            DataPagerChuNhiem.Visible = bDisplayData;
            LblSearchResultChuNhiem.Visible = !bDisplayData;

            RptChuNhiem.DataSource = tabularFormerings;
            RptChuNhiem.DataBind();
            DataPagerChuNhiem.ItemCount = dTotalRecords;
        }
        #endregion
    }
}