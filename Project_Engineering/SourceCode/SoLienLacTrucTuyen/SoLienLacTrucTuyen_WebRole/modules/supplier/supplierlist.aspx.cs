using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using EContactBook.DataAccess;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.BusinessEntity;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class SuppliersPage : BaseContentPage
    {
        #region Fields
        private bool isSearch;
        private SchoolBL schoolBL;
        #endregion

        #region Page event handlers
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
                BindDropDownLists();

                GetSearchedSessions();
                isSearch = false;
                BindRptSchools();
            }
        }
        #endregion

        #region Button click event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            isSearch = true;
            BindRptSchools();
        }

        protected void BtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            // Lưu điều kiện tìm kiếm vào session trước khi chuyển trang
            SaveSearchedSessions();

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

        #region DropDownList event hanlders
        protected void DdlProvinces_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLDistricts();
        }
        #endregion

        #region Methods
        private void BindRptSchools()
        {
            string strSchoolName = "";
            double dTotalRecords;
            ConfigurationProvince province = null;
            ConfigurationDistrict district = null;
            List<TabularSchool> tabularSchools;

            if (DdlProvinces.SelectedIndex > 0)
            {
                province = new ConfigurationProvince();
                province.ProvinceId = Int32.Parse(DdlProvinces.SelectedValue);
            }
            if (DdlDistricts.SelectedIndex > 0)
            {
                district = new ConfigurationDistrict();
                district.DistrictId = Int32.Parse(DdlDistricts.SelectedValue);
            }
            strSchoolName = TxtSchoolName.Text;

            tabularSchools = schoolBL.GetTabularSchools(province, district, strSchoolName, 
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            // Decrease page current index when delete
            if (tabularSchools.Count == 0 && dTotalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRptSchools();
                return;
            }

            RptSchools.DataSource = tabularSchools;
            RptSchools.DataBind();
            MainDataPager.ItemCount = dTotalRecords;

            bool bDisplayData = (tabularSchools.Count != 0) ? true : false;
            ProcessDislayInfo(bDisplayData);

            // save selections to viewstate
            ViewState[AppConstant.VIEWSTATE_SELECTED_PROVINCEID] = Int32.Parse(DdlProvinces.SelectedValue);
            ViewState[AppConstant.VIEWSTATE_SELECTED_DISTRICTID] = Int32.Parse(DdlDistricts.SelectedValue);
            ViewState[AppConstant.VIEWSTATE_SELECTED_SCHOOLNAME] = TxtSchoolName.Text;
        }

        private void BindDropDownLists()
        {
            BindDDLProvinces();
            BindDDLDistricts();
        }

        private void BindDDLProvinces()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<ConfigurationProvince> provinces = systemConfigBL.GetProvinces();
            DdlProvinces.DataSource = provinces;
            DdlProvinces.DataValueField = "ProvinceId";
            DdlProvinces.DataTextField = "ProvinceName";
            DdlProvinces.DataBind();

            if (DdlProvinces.Items.Count != 0)
            {
                DdlProvinces.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }

        private void BindDDLDistricts()
        {
            if (DdlProvinces.Items.Count != 0)
            {
                ConfigurationProvince province = null;
                if (DdlProvinces.SelectedIndex > 0)
                {
                    province = new ConfigurationProvince();
                    province.ProvinceId = Int32.Parse(DdlProvinces.SelectedValue);

                    SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
                    List<ConfigurationDistrict> districts = systemConfigBL.GetDistricts(province);
                    DdlDistricts.DataSource = districts;
                    DdlDistricts.DataValueField = "DistrictId";
                    DdlDistricts.DataTextField = "DistrictName";
                    DdlDistricts.DataBind();

                    if (DdlDistricts.Items.Count != 0)
                    {
                        DdlDistricts.Items.Insert(0, new ListItem("Tất cả", "0"));
                    }
                }
                else
                {
                    DdlDistricts.Items.Clear();
                    DdlDistricts.Items.Insert(0, new ListItem("Tất cả", "0"));
                }                
            }            
        }

        private void ProcessDislayInfo(bool bDisplayData)
        {
            LblSearchResult.Visible = !bDisplayData;
            RptSchools.Visible = bDisplayData;

            if (LblSearchResult.Visible)
            {
                MainDataPager.Visible = false; 
                
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin trường học";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy trường học";
                }
            }
            else
            {
                MainDataPager.Visible = true;
            }
        }

        private void SaveSearchedSessions()
        {
            AddSession(AppConstant.SESSION_SELECTED_PROVINCE, ViewState[AppConstant.VIEWSTATE_SELECTED_PROVINCEID]);
            AddSession(AppConstant.SESSION_SELECTED_DISTRICT, ViewState[AppConstant.VIEWSTATE_SELECTED_DISTRICTID]);
            AddSession(AppConstant.SESSION_SELECTED_SCHOOLNAME, ViewState[AppConstant.VIEWSTATE_SELECTED_SCHOOLNAME]);
        }

        private void GetSearchedSessions()
        {
            if (CheckSessionKey(AppConstant.SESSION_SELECTED_PROVINCE)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_DISTRICT)
                && CheckSessionKey(AppConstant.SESSION_SELECTED_SCHOOLNAME))
            {
                ViewState[AppConstant.VIEWSTATE_SELECTED_PROVINCEID] = (Int32)GetSession(AppConstant.SESSION_SELECTED_PROVINCE);
                RemoveSession(AppConstant.SESSION_SELECTED_PROVINCE);
                DdlProvinces.SelectedValue = ViewState[AppConstant.VIEWSTATE_SELECTED_PROVINCEID].ToString();

                ViewState[AppConstant.VIEWSTATE_SELECTED_DISTRICTID] = (Int32)GetSession(AppConstant.SESSION_SELECTED_DISTRICT);
                RemoveSession(AppConstant.SESSION_SELECTED_DISTRICT);
                DdlDistricts.SelectedValue = ViewState[AppConstant.VIEWSTATE_SELECTED_DISTRICTID].ToString();

                ViewState[AppConstant.VIEWSTATE_SELECTED_SCHOOLNAME] = (string)GetSession(AppConstant.SESSION_SELECTED_SCHOOLNAME);
                RemoveSession(AppConstant.SESSION_SELECTED_SCHOOLNAME);
                TxtSchoolName.Text = (string)ViewState[AppConstant.VIEWSTATE_SELECTED_SCHOOLNAME];
            }
        }
        #endregion
    }
}