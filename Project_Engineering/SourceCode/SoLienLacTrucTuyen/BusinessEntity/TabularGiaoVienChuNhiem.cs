using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.BusinessEntity
{
    public class TabularFormerTeacher
    {
        public int MaGVCN { get; set; }

        public Guid UserId { get; set; }
        public string TenGiaoVien { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
    }
}
