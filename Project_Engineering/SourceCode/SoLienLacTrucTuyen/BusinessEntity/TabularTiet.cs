using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.BusinessEntity
{
    public class TabularTeachingPeriod
    {
        private int maTiet;
        public int MaTiet 
        {
            get
            {
                return maTiet;
            }
            set
            {
                maTiet = value;
            }
        }

        public string TenTiet { get; set; }
        public string StringTiet { get; set; }

        public int MaBuoi { get; set; }
        public string TenBuoi { get; set; }

        public int ThuTu { get; set; }

        private DateTime thoiGianBatDau;
        public DateTime ThoiGianBatDau 
        {
            get
            {
                return thoiGianBatDau;
            }
            set
            {
                thoiGianBatDau = value;
                StringThoiGianBatDau = thoiGianBatDau.ToShortTimeString();
            }
        }
        public string StringThoiGianBatDau { get; set; }

        private DateTime thoiGianKetThuc;
        public DateTime ThoiGianKetThuc
        {
            get
            {
                return thoiGianKetThuc;
            }
            set
            {
                thoiGianKetThuc = value;
                StringThoiGianKetThuc = thoiGianKetThuc.ToShortTimeString();
            }
        }
        public string StringThoiGianKetThuc { get; set; }
    }
}
