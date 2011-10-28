using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.BusinessEntity
{
    public class TabularUser
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
    }
}
