using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using EContactBook.DataAccess;
using SoLienLacTrucTuyen.BusinessLogic;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class SuppliersPage : BaseContentPage
    {
        private bool isSearch;
        private SchoolBL schoolBL;

        protected override void Page_Load(object sender, EventArgs e)
        {
            // Check user's accessibility
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

            schoolBL = new SchoolBL();

            if (!Page.IsPostBack)
            {
                isSearch = false;
                BindRptSchools();
            }
        }

        #region Button click event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            isSearch = true;
            BindRptSchools();
        }

        protected void BtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(AppConstant.PAGEPATH_ADDSCHOOL);
        }

        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void BtnDelete_Click(object sender, ImageClickEventArgs e)
        {
            List<School_School> schools = new List<School_School>();
            School_School school = null;

            foreach (RepeaterItem item in RptSchools.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox CkbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (CkbxSelect.Checked)
                    {
                        HiddenField HdfRptSchoolId = (HiddenField)item.FindControl("HdfRptSchoolId");
                        school = new School_School();
                        school.SchoolId = Int32.Parse(HdfRptSchoolId.Value);
                        schools.Add(school);
                    }
                }
            }

            schoolBL.DeleteSchool(schools);
            BindRptSchools();
        }

        #endregion

        #region Pager event handlers
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            this.MainDataPager.CurrentIndex = Convert.ToInt32(e.CommandArgument);
            BindRptSchools();
        }
        #endregion


        private void BindRptSchools()
        {
            string strSchoolName = TxtSchoolName.Text;
            double dTotalRecords;
            List<School_School> schools = schoolBL.GetSchools(strSchoolName, MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            // Decrease page current index when delete
            if (schools.Count == 0 && dTotalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRptSchools();
                return;
            }

            bool bDisplayData = (schools.Count != 0) ? true : false;
            PnlPopupConfirmDelete.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin ngành học";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy ngành học";
                }

                MainDataPager.CurrentIndex = 1;
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
            }
            else
            {
                MainDataPager.Visible = true;
            }

            RptSchools.DataSource = schools;
            RptSchools.DataBind();
            MainDataPager.ItemCount = dTotalRecords;
        }
    }
}