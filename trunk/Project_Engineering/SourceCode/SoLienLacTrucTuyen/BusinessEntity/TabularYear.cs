﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.BusinessEntity
{
    public class TabularYear
    {
        public int YearId { get; set; }
        public string YearName { get; set; }
        public DateTime FirstTermBeginDate { get; set; }
        public DateTime FirstTermEndDate { get; set; }
        public DateTime SecondTermBeginDate { get; set; }
        public DateTime SecondTermEndDate { get; set; }
    }
}