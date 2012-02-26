using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.BusinessEntity
{
    public class TabularUser
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string ActualUserName { get; set; }
        public string Email { get; set; }
        public bool Activated { get; set; }
        public string StringStatus 
        {
            get
            {
                if (Activated)
                {
                    return "Được kích hoạt";
                }
                else
                {
                    return "Chưa kích hoạt";
                }
            }
        }
        public string RoleName { get; set; }
        public string RoleDisplayedName { get; set; }
        public bool NotYetActivated { get; set; }
    }
}
