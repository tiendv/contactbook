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
    public partial class DetailedTeacherPage : BaseContentPage
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
                if (RetrieveSessions())
                {
                    FillTeacherPersonalInformation();
                    BindRptFormerings();
                    BindRptTeachings();
                    ProcPermissions();
                }
                else
                {
                    Response.Redirect(AppConstant.PAGEPATH_TEACHER_LIST);
                }                
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnSua_Click(object sender, ImageClickEventArgs e)
        {
            aspnet_User teacher = new aspnet_User();
            teacher.UserId = new Guid(ViewState[AppConstant.VIEWSTATE_SELECTED_USERID].ToString());

            AddSession(AppConstant.SESSION_SELECTED_USER, teacher);
            AddSession(AppConstant.SESSION_PREVIOUSPAGE, AppConstant.PAGEPATH_TEACHER_DETAIL);

            Response.Redirect(AppConstant.PAGEPATH_TEACHER_EDIT);
        }

        protected void BtnBackPrevPage_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(AppConstant.PAGEPATH_TEACHER_LIST);
        }
        #endregion

        #region Methods
        private void FillTeacherPersonalInformation()
        {
            aspnet_User teacher = teacherBL.GetTeacher(new Guid(ViewState[AppConstant.VIEWSTATE_SELECTED_USERID].ToString()));
            LblUserIdHienThi.Text = teacher.UserName.Split(AppConstant.UNDERSCORE_CHAR)[1];

            LblTenGiaoVien.Text = teacher.aspnet_Membership.FullName;

            if (teacher.aspnet_Membership.Birthday != null)
            {
                LblNgaySinh.Text = ((DateTime)teacher.aspnet_Membership.Birthday).ToShortDateString();
            }
            else
            {
                LblNgaySinh.Text = "(Chưa xác định)";
            }

            if (teacher.aspnet_Membership.Gender != null)
            {
                LblGioiTinh.Text = (bool)teacher.aspnet_Membership.Gender ? AppConstant.STRING_MALE : AppConstant.STRING_FEMALE;
            }
            else
            {
                LblGioiTinh.Text = "(Chưa xác định)";
            }


            if (CheckUntils.IsNullOrBlank(teacher.aspnet_Membership.Address) == false)
            {
                LblDiaChi.Text = teacher.aspnet_Membership.Address;
            }
            else
            {
                LblDiaChi.Text = "(Chưa xác định)";
            }

            if (CheckUntils.IsNullOrBlank(teacher.aspnet_Membership.Phone) == false)
            {
                LblDienThoai.Text = teacher.aspnet_Membership.Phone;
            }
            else
            {
                LblDienThoai.Text = "(Chưa xác định)";
            }
        }

        private void ProcPermissions()
        {
            BtnSua.Visible = accessibilities.Contains(AccessibilityEnum.Modify);
        }

        private bool RetrieveSessions()
        {
            if (CheckSessionKey(AppConstant.SESSION_SELECTED_USER))
            {
                aspnet_User teacher = (aspnet_User)GetSession(AppConstant.SESSION_SELECTED_USER);
                RemoveSession(AppConstant.SESSION_SELECTED_USER);
                ViewState[AppConstant.VIEWSTATE_SELECTED_USERID] = teacher.UserId; 
                return true;
            }

            return false;
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
            aspnet_User teacher = new aspnet_User();
            Guid UserId = new Guid(ViewState[AppConstant.VIEWSTATE_SELECTED_USERID].ToString());            
            teacher.UserId = UserId;
            double dTotalRecords;
            List<TabularTeaching> tabularTeachings = teacherBL.GetListTeachings(
                teacher, DataPagerGiangDay.CurrentIndex, DataPagerGiangDay.PageSize, out dTotalRecords);

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
            aspnet_User teacher = new aspnet_User();
            teacher.UserId = new Guid(ViewState[AppConstant.VIEWSTATE_SELECTED_USERID].ToString());
            double dTotalRecords;
            List<TabularFormering> tabularFormerings = teacherBL.GetListFormerings(
                teacher, DataPagerChuNhiem.CurrentIndex, DataPagerChuNhiem.PageSize, out dTotalRecords);

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