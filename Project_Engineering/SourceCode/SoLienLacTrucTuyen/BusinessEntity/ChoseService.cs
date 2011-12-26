using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.BusinessEntity
{
    public class ChoseService
    {
        public int FunctionId { get; set; }
        public bool GetEmail { get; set; }
        public bool GetSMS { get; set; }
        public bool Chose { get; set; }
    }
}
