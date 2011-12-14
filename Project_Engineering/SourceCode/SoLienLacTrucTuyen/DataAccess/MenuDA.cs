using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace EContactBook.DataAccess
{
    public class MenuDA : BaseDA
    {
        public MenuDA()
            : base()
        {
        }

        public Dictionary<MyMenuItem, List<MyMenuItem>> BuildMenuTree(aspnet_Role role, string pageUrl)
        {
            FunctionsDA functionsDA = new FunctionsDA();
            Dictionary<MyMenuItem, List<MyMenuItem>> menuTree = new Dictionary<MyMenuItem, List<MyMenuItem>>();
            List<MyMenuItem> roleBasedMenuItems = new List<MyMenuItem>();

            // Get list of FunctionId by given role            
            List <UserManagement_Function> roleBasedFunctions = functionsDA.GetFunctions(role);
            
            return BuildMenuTree(roleBasedFunctions, pageUrl);
        }

        private Dictionary<MyMenuItem, List<MyMenuItem>> BuildMenuTree(List<UserManagement_Function> roleBasedFunctions, string pageUrl)
        {
            Dictionary<MyMenuItem, List<MyMenuItem>> menuTree = new Dictionary<MyMenuItem, List<MyMenuItem>>();
            List<MyMenuItem> roleBasedMenuItems = new List<MyMenuItem>();

            if (roleBasedFunctions.Count != 0)
            {
                foreach (UserManagement_Function function in roleBasedFunctions)
                {
                    // Get list of Menu by given role and function
                    IQueryable<MyMenuItem> iqRoleBasedMenuItems;
                    iqRoleBasedMenuItems = from menu in db.UserManagement_Menus
                                           where menu.FunctionId == function.FunctionId
                                           select new MyMenuItem
                                           {
                                               MaMenu = menu.MenuId,
                                               TieuDe = menu.Title,
                                               Url = menu.UserManagement_PagePath.PhysicalPath,
                                               ParentMenuId = menu.ParentMenuId,
                                               CapDo = menu.MenuLevel,
                                               ThuTuHienThi = menu.DisplayOrder,
                                               HienThi = (bool)menu.IsDisplayed
                                           };
                    if (iqRoleBasedMenuItems.Count() != 0)
                    {
                        roleBasedMenuItems.AddRange(iqRoleBasedMenuItems.ToList());
                    }
                }
            }

            // Sort menu by level and display order
            if (roleBasedMenuItems.Count != 0)
            {
                roleBasedMenuItems = roleBasedMenuItems.OrderBy(menuItem => menuItem.CapDo)
                    .ThenBy(menuItem => menuItem.ThuTuHienThi).ToList();
            }

            // Get list of Level 0 MenuItem
            List<MyMenuItem> level0MenuItems = roleBasedMenuItems.Where(menuItem => menuItem.CapDo == 1).ToList();
            UserManagement_Menu pageUrlBasedMenu = GetPageUrlBasedMenu(pageUrl);
            bool bSetCurrentLevel0MenuItem = false;
            bool bSetCurrentLevel1MenuItem = false;

            foreach (MyMenuItem level0MenuItem in level0MenuItems)
            {
                // Get list of Level 1 MenuItem
                IEnumerable<MyMenuItem> ieLevel1MenuItem = from menuItem in roleBasedMenuItems
                                                           where menuItem.CapDo == 2 && menuItem.ParentMenuId == (int)level0MenuItem.MaMenu
                                                           select menuItem;
                List<MyMenuItem> level1MenuItems = new List<MyMenuItem>();
                if (ieLevel1MenuItem.Count() != 0)
                {
                    level1MenuItems = ieLevel1MenuItem.ToList();
                }

                // Determine current MenuItem
                foreach (MyMenuItem level1MenuItem in level1MenuItems)
                {
                    if ((!bSetCurrentLevel1MenuItem) &&
                        ((level1MenuItem.Url == pageUrl) || (level1MenuItem.MaMenu == pageUrlBasedMenu.MenuId)))
                    {
                        level1MenuItem.HienHanh = true;
                        level0MenuItem.HienHanh = true;
                        bSetCurrentLevel1MenuItem = true;
                    }
                }

                if ((!bSetCurrentLevel0MenuItem)
                    && ((level0MenuItem.Url == pageUrl) || (level0MenuItem.MaMenu == pageUrlBasedMenu.MenuId)))
                {
                    level0MenuItem.HienHanh = true;
                    bSetCurrentLevel0MenuItem = true;
                    if (level1MenuItems.Count == 0)
                    {
                        bSetCurrentLevel1MenuItem = true;
                    }
                }

                int i = 0;
                while (i < level1MenuItems.Count())
                {
                    if (!level1MenuItems[i].HienThi)
                    {
                        level1MenuItems.Remove(level1MenuItems[i]);
                    }
                    else
                    {
                        i++;
                    }
                }

                menuTree.Add(level0MenuItem, level1MenuItems);
            }

            return menuTree;
        }
        
        public Dictionary<MyMenuItem, List<MyMenuItem>> BuildMenuTree(string userName, string pageUrl)
        {
            FunctionsDA functionsDA = new FunctionsDA();
            Dictionary<MyMenuItem, List<MyMenuItem>> menuTree = new Dictionary<MyMenuItem, List<MyMenuItem>>();
            List<MyMenuItem> roleBasedMenuItems = new List<MyMenuItem>();

            // Get list of FunctionId by given role            
            List<UserManagement_Function> roleBasedFunctions = functionsDA.GetFunctions(userName);

            return BuildMenuTree(roleBasedFunctions, pageUrl);
        }

        /// <summary>
        /// Get UserManagement_Menu by pageUrl
        /// </summary>
        /// <param name="pageUrl"></param>
        /// <returns></returns>
        private UserManagement_Menu GetPageUrlBasedMenu(string pageUrl)
        {
            UserManagement_Menu pageUrlBasedMenu = null;
            IQueryable<UserManagement_Function> iqPageUrlBasedFunction = from authorizedPage in db.UserManagement_AuthorizedPages
                                                                         where authorizedPage.UserManagement_PagePath.PhysicalPath == pageUrl
                                                                         select authorizedPage.UserManagement_Function;
            if (iqPageUrlBasedFunction.Count() != 0)
            {
                UserManagement_Function urlBasedFunctionId = iqPageUrlBasedFunction.First();
                IQueryable<UserManagement_Menu> iqPageUrlBasedMenu = from menu in db.UserManagement_Menus
                                                                     where menu.FunctionId == urlBasedFunctionId.FunctionId
                                                                     select menu;
                pageUrlBasedMenu = iqPageUrlBasedMenu.First();
            }

            return pageUrlBasedMenu;
        }

        public UserManagement_Menu GetMenu(int menuId)
        {
            UserManagement_Menu menu = (from menus in db.UserManagement_Menus
                                        where menus.MenuId == menuId
                                        select menus).First();
            return menu;
        }
    }
}
