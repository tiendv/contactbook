﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.BusinessEntity
{
    public class TabularStudentActivity
    {
        public int MaHoatDong { get; set; }
        public int MaHocSinhLopHoc { get; set; }
        public DateTime Ngay { get; set; }

        public string TenHoatDong { get; set; }
        public string StrNgay { get; set; }

        public int? AttitudeId;
        public string ThaiDoThamGia { get; set; }
    }
}
