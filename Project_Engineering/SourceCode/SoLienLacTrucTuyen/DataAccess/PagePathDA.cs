using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class PagePathDA : BaseDA
    {
        public PagePathDA()
            : base()
        {
        }

        public string GetPageTitle(string pageUrl)
        {
            IQueryable<string> pageTitles = from pagePath in db.UserManagement_PagePaths
                                            where pagePath.PhysicalPath == pageUrl
                                            select pagePath.PageTitle;
            if (pageTitles.Count() != 0)
            {
                return pageTitles.First();                     
            }
            else
            {
                return "";
            }
        }
    }
}
