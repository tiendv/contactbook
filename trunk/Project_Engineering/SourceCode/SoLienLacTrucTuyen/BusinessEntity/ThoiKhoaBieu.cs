using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.BusinessEntity
{
    public class DailySchedule
    {
        public int MaLopHoc { get; set; }

        public int MaNamHoc { get; set; }
        public int MaHocKy { get; set; }
        public int MaThu { get; set; }
        public string TenThu { get; set; }

        public List<SessionedSchedule> SessionedSchedules { get; set; }
    }

    public class SessionedSchedule
    {
        public int MaBuoi { get; set; }
        public List<TeachingPeriodSchedule> ListThoiKhoaBieuTheoTiet { get; set; }
    }

    public class TeachingPeriodSchedule
    {
        public int MaMonHocTKB { get; set; }
        public int MaMonHoc { get; set; }        
        public string TenMonHoc { get; set; }
        public Guid MaGiaoVien { get; set; }
        public string TenGiaoVien { get; set; }
        public int Tiet { get; set; }
        public string ChiTietTiet { get; set; }
                
        public int MaLopHoc { get; set; }
        public int MaThu { get; set; }
        public int MaHocKy { get; set; }
    }
}
