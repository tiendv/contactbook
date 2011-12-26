using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.BusinessEntity;
using EContactBook.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class MenuBL
    {
        private MenuDA menuDA;

        public MenuBL()
            : base()
        {
            menuDA = new MenuDA();
        }

        public Dictionary<MyMenuItem, List<MyMenuItem>> BuildMenuTree(School_School school, string userName, string pageUrl)
        {
            AuthorizationBL authorizationBL = new AuthorizationBL(school);
            UserBL userBL = new UserBL(school);
            aspnet_Role role = userBL.GetRole(userName);
            if (authorizationBL.IsRoleParents(role))
            {
                return menuDA.BuildMenuTree(userName, pageUrl);
            }
            else
            {
                return menuDA.BuildMenuTree(role, pageUrl);
            }            
        }
    }
}
