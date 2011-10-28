using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class PagePathBL
    {
        private PagePathDA pagePathDA;

        public PagePathBL()
        {
            pagePathDA = new PagePathDA();
        }

        public string GetPageTitle(string pageUrl)
        {
            return pagePathDA.GetPageTitle(pageUrl);
        }
    }

}
