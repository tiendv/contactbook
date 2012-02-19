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

        public Dictionary<MyMenuItem, List<MyMenuItem>> GetMenu(School_School school, string userName, string pageUrl)
        {
            AuthorizationBL authorizationBL = new AuthorizationBL(school);
            UserBL userBL = new UserBL(school);
            List<aspnet_Role> roles = userBL.GetRoles(userName);

            if (authorizationBL.IsRoleParents(roles[0]))
            {
                return menuDA.GetMenu(userName, pageUrl);
            }
            else
            {
                return menuDA.GetMenu(roles, pageUrl);
            }            
        }
    }
}
