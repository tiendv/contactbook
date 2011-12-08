using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.BusinessEntity
{
    public class TabularAbsent
    {
        public int AbsentId { get; set; }
        public int StudentInClassId { get; set; }
        public string Date { get; set; }
        public string Session { get; set; }
        public string IsAsked { get; set; }
        public string Reason { get; set; }
        public string Confirmed { get; set; }
    }
}
