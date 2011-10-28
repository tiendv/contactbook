using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;
using AjaxControlToolkit;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class SuaThoiKhoaBieuPage_orginal : System.Web.UI.Page
    {
        #region Fields
        private int maNamHoc;
        private int maHocKy;
        private int maThu;
        private int maLopHoc;
        private MonHocBL monHocBL;
        private ThoiKhoaBieuBL monHocTKBBL;
        List<ThoiKhoaBieuTheoTiet> lstThoiKhoaBieuTheoTietBuoiSang;
        List<ThoiKhoaBieuTheoTiet> lstThoiKhoaBieuTheoTietBuoiChieu;
        #endregion

        #region Constants
        const string QUERYSTRING_NAMHOC = "NamHoc";
        const string QUERYSTRING_HOCKY = "HocKy";
        const string QUERYSTRING_THU = "Thu";
        const string QUERYSTRING_LOPHOC = "LopHoc";
        const int MABUOISANG = 1;
        const int MABUOICHIEU = 2;
        const string THOIKHOABIEU_PAGE = "thoikhoabieu.aspx";
        #endregion

        #region Page event handler
        protected void Page_Load(object sender, EventArgs e)
        {
            RoleBL roleBL = new RoleBL();
            UserBL userBL = new UserBL();
            monHocBL = new MonHocBL();
            monHocTKBBL = new ThoiKhoaBieuBL();

            string pageUrl = Page.Request.Path;
            Guid role = userBL.GetRoleId(User.Identity.Name);

            if (!roleBL.ValidateAuthorization(role, pageUrl))
            {
                Response.Redirect((string)GetGlobalResourceObject("MainResource", "AccessDeniedPageUrl"));
                return;
            }

            Site masterPage = (Site)Page.Master;
            masterPage.UserRole = role;
            masterPage.PageUrl = pageUrl;

            if (Request.QueryString[QUERYSTRING_NAMHOC] != null 
                && Request.QueryString[QUERYSTRING_HOCKY] != null 
                && Request.QueryString[QUERYSTRING_THU] != null 
                && Request.QueryString[QUERYSTRING_LOPHOC] != null)
            {         
                maNamHoc = Int32.Parse(Request.QueryString[QUERYSTRING_NAMHOC]);
                this.LblTenNamHoc.Text = (new NamHocBL()).GetNamHoc(maNamHoc).TenNamHoc;
                this.HdfMaNamHoc.Value = maNamHoc.ToString();

                maHocKy = Int32.Parse(Request.QueryString[QUERYSTRING_HOCKY]);
                this.LblTenHocKy.Text = (new HocKyBL()).GetHocKy(maHocKy).TenHocKy;
                this.HdfMaHocKy.Value = maHocKy.ToString();

                maThu = Int32.Parse(Request.QueryString[QUERYSTRING_THU]);
                this.LblTenThu.Text = (new ThuBL()).GetThu(maThu).TenThu;
                this.HdfMaThu.Value = maThu.ToString();

                maLopHoc = Int32.Parse(Request.QueryString[QUERYSTRING_LOPHOC]);
                this.LblTenLopHoc.Text = (new LopHocBL()).GetLopHoc(maLopHoc).TenLopHoc;
                this.HdfMaLopHoc.Value = maLopHoc.ToString();

                lstThoiKhoaBieuTheoTietBuoiSang  = monHocTKBBL.GetMonHocTKBInfo(maLopHoc, maNamHoc, maHocKy, maThu, MABUOISANG);
                lstThoiKhoaBieuTheoTietBuoiChieu = monHocTKBBL.GetMonHocTKBInfo(maLopHoc, maNamHoc, maHocKy, maThu, MABUOICHIEU);

                BindRepeater();
            }

            if (!Page.IsPostBack)
            {
                if (Session[Session["username"].ToString() + "LstCheckedMonHocSang"] != null)
                {
                    Session.Remove(Session["username"].ToString() + "LstCheckedMonHocSang");
                }

                if (Session[Session["username"].ToString() + "LstCheckedMonHocChieu"] != null)
                {
                    Session.Remove(Session["username"].ToString() + "LstCheckedMonHocChieu");
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            List<int> lstCheckedMonHocSang = new List<int>();
            if (Session[Session["username"].ToString() + "LstCheckedMonHocSang"] != null)
            {
                lstCheckedMonHocSang = (List<int>)Session[Session["username"].ToString() + "LstCheckedMonHocSang"];
            }
            foreach (RepeaterItem rptItem in RptMonHoc.Items)
            {
                HiddenField hdfRptMaMonHoc = (HiddenField)rptItem.FindControl("HdfRptMaMonHoc");
                int maMonHoc = Int32.Parse(hdfRptMaMonHoc.Value);
                
                CheckBox checkBoxSang = (CheckBox)rptItem.FindControl("CkbxSang");
                if (checkBoxSang.Checked)
                {
                    lstCheckedMonHocSang.Add(maMonHoc);
                }
            }
            Session[Session["username"].ToString() + "LstCheckedMonHocSang"] = lstCheckedMonHocSang;

            List<int> lstCheckedMonHocChieu = new List<int>();
            if (Session[Session["username"].ToString() + "LstCheckedMonHocChieu"] != null)
            {
                lstCheckedMonHocChieu = (List<int>)Session[Session["username"].ToString() + "LstCheckedMonHocChieu"];
            }
            foreach (RepeaterItem rptItem in RptMonHoc.Items)
            {
                HiddenField hdfRptMaMonHoc = (HiddenField)rptItem.FindControl("HdfRptMaMonHoc");
                int maMonHoc = Int32.Parse(hdfRptMaMonHoc.Value);

                CheckBox checkBoxChieu = (CheckBox)rptItem.FindControl("CkbxChieu");
                if (checkBoxChieu.Checked)
                {
                    lstCheckedMonHocChieu.Add(maMonHoc);
                }
            }
            Session[Session["username"].ToString() + "LstCheckedMonHocChieu"] = lstCheckedMonHocChieu;
        }
        #endregion

        #region Repeater event handlers
        protected void RptMonHoc_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item 
                || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.DataItem != null)
                {
                    ThoiKhoaBieuTheoTiet thoiKhoaBieuTheoTiet = (ThoiKhoaBieuTheoTiet)e.Item.DataItem;
                    int maMonHoc = thoiKhoaBieuTheoTiet.MaMonHoc;

                    CheckBox checkBoxSang = (CheckBox)e.Item.FindControl("CkbxSang");
                    checkBoxSang.Checked = monHocTKBBL.ExistMonHocTKBInfo(maNamHoc, maHocKy, maThu, MABUOISANG, 
                        maMonHoc, maLopHoc);

                    CheckBox checkBoxChieu = (CheckBox)e.Item.FindControl("CkbxChieu");
                    checkBoxChieu.Checked = monHocTKBBL.ExistMonHocTKBInfo(maNamHoc, maHocKy, maThu, MABUOICHIEU, 
                        maMonHoc, maLopHoc);
                }
            }
        }
        #endregion

        #region Methods
        private void BindRepeater()
        {
            double totalRecords;
            List<MonHocInfo> lstMonHocInfo = monHocBL.GetListMonHocByLopHoc(maLopHoc, 
                MainDataPager.CurrentIndex, MainDataPager.PageSize, out totalRecords);
            MainDataPager.ItemCount = totalRecords;

            bool bDisplayData = (lstMonHocInfo.Count != 0) ? true : false;
            LblSearchResult.Visible = !bDisplayData;
            MainDataPager.Visible = bDisplayData;

            RptMonHoc.DataSource = lstMonHocInfo;
            RptMonHoc.DataBind();
        }
        #endregion

        #region Button event handlers
        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            List<int> lstCheckedMonHocSang = new List<int>();
            if (Session[Session["username"].ToString() + "LstCheckedMonHocSang"] != null)
            {
                lstCheckedMonHocSang = (List<int>)Session[Session["username"].ToString() + "LstCheckedMonHocSang"];
                Session.Remove(Session["username"].ToString() + "LstCheckedMonHocSang");
            }
            monHocTKBBL.Update(maNamHoc, maHocKy, maThu, 1, lstCheckedMonHocSang, maLopHoc);

            List<int> lstCheckedMonHocChieu = new List<int>();
            if (Session[Session["username"].ToString() + "LstCheckedMonHocChieu"] != null)
            {
                lstCheckedMonHocChieu = (List<int>)Session[Session["username"].ToString() + "LstCheckedMonHocChieu"];
                Session.Remove(Session["username"].ToString() + "LstCheckedMonHocChieu");
            }
            monHocTKBBL.Update(maNamHoc, maHocKy, maThu, 2, lstCheckedMonHocChieu, maLopHoc);

            string strMaNamHoc = this.HdfMaNamHoc.Value;
            string strMaHocKy = this.HdfMaHocKy.Value;
            string strMaLopHoc = this.HdfMaLopHoc.Value;
            string query = "NamHoc=" + strMaNamHoc + "&HocKy=" + maHocKy + "&LopHoc=" + maLopHoc;
            Response.Redirect("/Modules/Lop_Hoc/ThoiKhoaBieu.aspx?" + query);
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            if (Session[Session["username"].ToString() + "LstCheckedMonHocSang"] != null)
            {
                Session.Remove(Session["username"].ToString() + "LstCheckedMonHocSang");
            }

            if (Session[Session["username"].ToString() + "LstCheckedMonHocChieu"] != null)
            {
                Session.Remove(Session["username"].ToString() + "LstCheckedMonHocChieu");
            }

            Response.Redirect(string.Format(THOIKHOABIEU_PAGE + "thoikhoabieu.aspx?NamHoc={0}&HocKy={1}&LopHoc={2}", 
                this.HdfMaNamHoc.Value, this.HdfMaHocKy.Value, this.HdfMaLopHoc.Value));
        }
        #endregion

        #region Pager event handlers
        public void pager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currentPageIndex;
        }
        #endregion        
    }
}