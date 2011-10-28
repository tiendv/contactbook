using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoLienLacTrucTuyen.BusinessEntity
{
    public class MenuTitle
    {
        public string Url { get; set; }
        public string TenChucNang { get; set; }

        public MenuTitle(string url, string tenChucNang)
        {
            this.Url = url;
            this.TenChucNang = tenChucNang;
        }
    }
}