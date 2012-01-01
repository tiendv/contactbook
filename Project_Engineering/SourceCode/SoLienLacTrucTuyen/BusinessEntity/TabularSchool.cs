using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.BusinessEntity
{
    public class TabularSchool
    {
        public int SchoolId { get; set; }
        public string SchoolName { get; set; }
        public string DistrictName { get; set; }
        public string ProvinceName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
    }
}
