using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.BusinessEntity
{
    public class TabularRole
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public string DisplayedName { get; set; }
        public string Description { get; set; }
        public bool Expired { get; set; }
        public bool CanBeDeleted { get; set; }
        public bool Actived { get; set; }
    }
}
