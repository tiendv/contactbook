using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class MenuBL
    {
        private MenuDA menuDA;

        public MenuBL()
        {
            menuDA = new MenuDA();
        }

        public Dictionary<MyMenuItem, List<MyMenuItem>> BuildMenuTree(Guid roleId, string pageUrl)
        {
            return menuDA.BuildMenuTree(roleId, pageUrl);
        }
    }
}
