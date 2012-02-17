using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.BusinessEntity;
using SoLienLacTrucTuyen_WebRole.Modules;
using EContactBook.DataAccess;

namespace SoLienLacTrucTuyen_WebRole
{
    public partial class Site : System.Web.UI.MasterPage
    {
        #region Fields
        private MenuBL menuBL = new MenuBL();
        private string currentLevel0MenuItem;
        private Dictionary<MyMenuItem, List<MyMenuItem>> dicMenuItem = new Dictionary<MyMenuItem, List<MyMenuItem>>();
        private List<MyMenuItem> level0MenuItems = new List<MyMenuItem>();
        #endregion

        #region Properties
        public School_School UserSchool { get; set; }

        public string UserName { get; set; }

        public aspnet_Role UserRole { get; set; }

        public string PageUrl { get; set; }

        public string PageTitle
        {
            set
            {
                lblTitle.Text = value;
                Page.Title = value;
            }
        }
        #endregion

        #region Page event handlers
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Page.User.Identity.IsAuthenticated)
            {
                try
                {
                    Label lblUserName = (Label)LoginView1.Controls[0].FindControl("LblUserName");
                    lblUserName.Text = string.Format("Xin chào {0}!", Page.User.Identity.Name.Split(AppConstant.UNDERSCORE_CHAR)[1]);
                    if(Session[AppConstant.SCHOOL] != null)
                    {
                        School_School school = (School_School)Session[AppConstant.SCHOOL];
                        if (school.SchoolId != 0)
                        {
                            LblSchoolName.Text = school.SchoolName;
                        }
                    }
                }
                catch (Exception ex) { }
            }

            SetLevel0MenuItems();
            SetLevel1MenuItems();
            if (PageUrl != null)
            {
                PageTitle = (new PagePathBL()).GetPageTitle(PageUrl);
            }
        }

        protected void Page_PreRender(object sender, System.EventArgs e)
        {
            foreach (RepeaterItem item in RptLevel0MenuItem.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    Control control = item.FindControl("HlkLevel0MenuItem");
                    if (control != null)
                    {
                        HyperLink HlkLevel0MenuItem = (HyperLink)control;
                        if (currentLevel0MenuItem == HlkLevel0MenuItem.NavigateUrl)
                        {
                            this.hdfActiveLevel0MenuTitle.Value = HlkLevel0MenuItem.ClientID;
                            HlkLevel0MenuItem.CssClass = "menu_title_hover_parent";
                            break;
                        }
                    }
                }
            }
        }
        #endregion

        #region Private methods
        private void SetLevel0MenuItems()
        {
            if (PageUrl != null)
            {
                dicMenuItem = menuBL.BuildMenuTree(UserSchool, UserName, PageUrl);
                level0MenuItems.Clear();
                foreach (KeyValuePair<EContactBook.BusinessEntity.MyMenuItem, List<EContactBook.BusinessEntity.MyMenuItem>>
                    pair in dicMenuItem)
                {
                    EContactBook.BusinessEntity.MyMenuItem menuItemCap0 = pair.Key;
                    level0MenuItems.Add(menuItemCap0);

                    if (menuItemCap0.HienHanh)
                    {
                        currentLevel0MenuItem = menuItemCap0.Url;
                    }
                }

                RptLevel0MenuItem.DataSource = level0MenuItems;
                RptLevel0MenuItem.DataBind();
            }
        }

        private void SetLevel1MenuItems()
        {
            foreach (KeyValuePair<EContactBook.BusinessEntity.MyMenuItem,
                     List<EContactBook.BusinessEntity.MyMenuItem>>
                pair in dicMenuItem)
            {
                if (pair.Key.HienHanh)
                {
                    List<EContactBook.BusinessEntity.MyMenuItem> listLevel1MenuItems = pair.Value;
                    RptFixedLevel1MenuItems.DataSource = listLevel1MenuItems;
                    RptFixedLevel1MenuItems.DataBind();
                    return;
                }
            }
        }
        #endregion

        #region Public methods
        public void SetPageTitleVisibility(bool visibility)
        {
            this.DivPageTitle.Visible = visibility;
        }
        #endregion
    }
}