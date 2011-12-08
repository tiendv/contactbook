using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.BusinessEntity;
using SoLienLacTrucTuyen_WebRole.Modules;

namespace SoLienLacTrucTuyen_WebRole
{    
    public partial class Site : System.Web.UI.MasterPage
    {
        #region Fields
        private string userName;
        public string UserName
        {
            get { return userName; }
        }

        private MenuBL menuBL = new MenuBL();
        private string MenuCap0HienHanh;
        private Dictionary<MyMenuItem, List<MyMenuItem>> dicMenuItem = new Dictionary<MyMenuItem,List<MyMenuItem>>();
        private List<MyMenuItem> listLevel0MenuItems = new List<MyMenuItem>();
        #endregion

        #region Properties
        public string UserNameSession
        {
            get
            {
                if (Session["username"] != null)
                {
                    return Session["username"].ToString();
                }
                return "Anonymous";
            }
        }

        public string CurrentUserName { get; set; }
        
        public string PageUrl { get; set; }
        
        public Guid UserRole { get; set; }

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
            if (Page.User.Identity.Name != "")
            {
                userName = Page.User.Identity.Name.Split('_')[1];
            }
            
            SetLevel0MenuItems();
            SetLevel1MenuItems();
            if(PageUrl != null)
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
                        if (MenuCap0HienHanh == HlkLevel0MenuItem.NavigateUrl)
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
                dicMenuItem = menuBL.BuildMenuTree(UserRole, PageUrl);
                listLevel0MenuItems.Clear();
                foreach (KeyValuePair<SoLienLacTrucTuyen.BusinessEntity.MyMenuItem, List<SoLienLacTrucTuyen.BusinessEntity.MyMenuItem>>
                    pair in dicMenuItem)
                {
                    SoLienLacTrucTuyen.BusinessEntity.MyMenuItem menuItemCap0 = pair.Key;
                    listLevel0MenuItems.Add(menuItemCap0);

                    if (menuItemCap0.HienHanh)
                    {
                        MenuCap0HienHanh = menuItemCap0.Url;
                    }
                }

                RptLevel0MenuItem.DataSource = listLevel0MenuItems;
                RptLevel0MenuItem.DataBind();
            }
        }

        private void SetLevel1MenuItems()
        {
            foreach (KeyValuePair<SoLienLacTrucTuyen.BusinessEntity.MyMenuItem, 
                     List<SoLienLacTrucTuyen.BusinessEntity.MyMenuItem>>
                pair in dicMenuItem)
            {
                if (pair.Key.HienHanh)
                {
                    List<SoLienLacTrucTuyen.BusinessEntity.MyMenuItem> listLevel1MenuItems = pair.Value;
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