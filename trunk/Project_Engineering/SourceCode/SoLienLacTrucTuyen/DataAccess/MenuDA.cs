using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class MenuDA : BaseDA
    {
        public MenuDA()
            : base()
        {
        }

        public Dictionary<MyMenuItem, List<MyMenuItem>> BuildMenuTree(Guid roleId, string pageUrl)
        {
            Dictionary<MyMenuItem, List<MyMenuItem>> menuTree =
                new Dictionary<MyMenuItem, List<MyMenuItem>>();

            List<MyMenuItem> lRoleBasedMenuItems = new List<MyMenuItem>();

            // Get list of FunctionId by given role
            IQueryable<int> RoleBasedFunctionIds = from authorizedPage in db.UserManagement_AuthorizedPages
                                                   join authorization in db.UserManagement_Authorizations
                                                    on authorizedPage.AuthorizedPageId equals authorization.AuthorizedPagePathId
                                                   where authorization.RoleId == roleId
                                                   select authorizedPage.FunctionId;
            if (RoleBasedFunctionIds.Count() != 0)
            {
                List<int> lRoleBasedFunctionIds = RoleBasedFunctionIds.Distinct().ToList();
                foreach (int functionId in lRoleBasedFunctionIds)
                {
                    // Get list of MenuId by given role and function
                    IQueryable<MyMenuItem> roleBasedMenuItems;
                    roleBasedMenuItems = from menu in db.UserManagement_Menus
                                         join pagePath in db.UserManagement_PagePaths
                                            on menu.PagePathId equals pagePath.PagePathId
                                         where menu.FunctionId == functionId
                                         select new MyMenuItem
                                         {
                                             MaMenu = menu.MenuId,
                                             TieuDe = menu.Title,
                                             Url = pagePath.PhysicalPath,
                                             ParentMenuId = menu.ParentMenuId,
                                             CapDo = menu.MenuLevel,
                                             ThuTuHienThi = menu.DisplayOrder,
                                             HienThi = (bool)menu.IsDisplayed
                                         };
                    if (roleBasedMenuItems.Count() != 0)
                    {
                        lRoleBasedMenuItems.AddRange(roleBasedMenuItems.ToList());
                    }
                }
            }
            lRoleBasedMenuItems = lRoleBasedMenuItems.OrderBy(menuItem => menuItem.CapDo)
                .ThenBy(menuItem => menuItem.ThuTuHienThi).ToList();

            // Get list of Level 0 MenuItem
            List<MyMenuItem> lLevel0MenuItems = lRoleBasedMenuItems.Where(menuItem => menuItem.CapDo == 1).ToList();
            int UrlBasedMenuId = GetUrlBasedMenuId(pageUrl);
            bool bSetCurrentLevel0MenuItem = false;
            bool bSetCurrentLevel1MenuItem = false;

            foreach (MyMenuItem level0MenuItem in lLevel0MenuItems)
            {
                // Get list of Level 1 MenuItem
                IEnumerable<MyMenuItem> level1MenuItems = from menuItem in lRoleBasedMenuItems
                                                          where menuItem.CapDo == 2
                                                              && menuItem.ParentMenuId == (int)level0MenuItem.MaMenu
                                                          select menuItem;
                List<MyMenuItem> lLevel1MenuItems = new List<MyMenuItem>();
                if (level1MenuItems.Count() != 0)
                {
                    lLevel1MenuItems = level1MenuItems.ToList();
                }

                // Determine current MenuItem
                foreach (MyMenuItem level1MenuItem in lLevel1MenuItems)
                {
                    if ((!bSetCurrentLevel1MenuItem) &&
                        ((level1MenuItem.Url == pageUrl) || (level1MenuItem.MaMenu == UrlBasedMenuId)))
                    {
                        level1MenuItem.HienHanh = true;
                        level0MenuItem.HienHanh = true;
                        bSetCurrentLevel1MenuItem = true;
                    }
                }

                if ((!bSetCurrentLevel0MenuItem)
                    && ((level0MenuItem.Url == pageUrl) || (level0MenuItem.MaMenu == UrlBasedMenuId)))
                {
                    level0MenuItem.HienHanh = true;
                    bSetCurrentLevel0MenuItem = true;
                    if (lLevel1MenuItems.Count == 0)
                    {
                        bSetCurrentLevel1MenuItem = true;
                    }
                }

                int i = 0;
                while (i < lLevel1MenuItems.Count())
                {
                    if (!lLevel1MenuItems[i].HienThi)
                    {
                        lLevel1MenuItems.Remove(lLevel1MenuItems[i]);
                    }
                    else
                    {
                        i++;
                    }
                }

                menuTree.Add(level0MenuItem, lLevel1MenuItems);
            }

            return menuTree;
        }

        // Get MenuId base on Url
        private int GetUrlBasedMenuId(string pageUrl)
        {
            IQueryable<int> UrlBasedFunctionIds = from authorizedPage in db.UserManagement_AuthorizedPages
                                                  join authorization in db.UserManagement_Authorizations
                                                    on authorizedPage.AuthorizedPageId equals authorization.AuthorizedPagePathId
                                                  join pagePath in db.UserManagement_PagePaths
                                                    on authorizedPage.PagePathId equals pagePath.PagePathId
                                                  where pagePath.PhysicalPath == pageUrl
                                                  select authorizedPage.FunctionId;

            int urlBasedFunctionId = UrlBasedFunctionIds.First();
            IQueryable<int> UrlBasedMenuIds = from menu in db.UserManagement_Menus
                                              where menu.FunctionId == urlBasedFunctionId
                                              select menu.MenuId;
            return UrlBasedMenuIds.First();
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
