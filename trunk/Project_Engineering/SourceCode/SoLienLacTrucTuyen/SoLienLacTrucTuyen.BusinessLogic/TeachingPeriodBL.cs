using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class TeachingPeriodBL
    {
        private TeachingPeriodDA teachingPeriodDA;

        public TeachingPeriodBL()
        {
            teachingPeriodDA = new TeachingPeriodDA();
        }

        public void DeleteTeachingPeriod(DanhMuc_Tiet teachingPeriod)
        {
            teachingPeriodDA.DeleteTeachingPeriod(teachingPeriod);
        }

        public void InsertTeachingPeriod(string teachingPeriodName, CauHinh_Buoi session, string order, string beginTime, string endTime)
        {
            string[] strBeginTimes = beginTime.Split(':');
            int iBeginHour = Int32.Parse(strBeginTimes[0]);
            int iBeginMinute = Int32.Parse(strBeginTimes[1]);
            DateTime dtBeginTime = new DateTime(2000, 1, 1, iBeginHour, iBeginMinute, 0);

            string[] strEndTimes = endTime.Split(':');
            int iEndHour = Int32.Parse(strEndTimes[0]);
            int iEndMinute = Int32.Parse(strEndTimes[1]);
            DateTime dtEndTime = new DateTime(2000, 1, 1, iEndHour, iEndMinute, 0);

            int iOrder = Int32.Parse(order);

            DanhMuc_Tiet teachingPeriod = new DanhMuc_Tiet();
            teachingPeriod.TenTiet = teachingPeriodName;
            teachingPeriod.MaBuoi = session.MaBuoi;
            teachingPeriod.ThoiGianBatDau = dtBeginTime;
            teachingPeriod.ThoiDiemKetThu = dtEndTime;

            teachingPeriodDA.InsertTeachingPeriod(teachingPeriod);
        }

        public void UpdateTiet(DanhMuc_Tiet editedTeachingPeriod, string newTeachingPeriodName, CauHinh_Buoi newSession, string newOrder, string newBeginTime, string newEndTime)
        {
            string[] strBeginTimes = newBeginTime.Split(':');
            int iBeginHour = Int32.Parse(strBeginTimes[0]);
            int iBeginMinute = Int32.Parse(strBeginTimes[1]);
            DateTime dtBeginTime = new DateTime(2000, 1, 1, iBeginHour, iBeginMinute, 0);

            string[] strEndTimes = newEndTime.Split(':');
            int iEndHour = Int32.Parse(strEndTimes[0]);
            int iEndMinute = Int32.Parse(strEndTimes[1]);
            DateTime dtEndTime = new DateTime(2000, 1, 1, iEndHour, iEndMinute, 0);

            int iOrder = Int32.Parse(newOrder);

            DanhMuc_Tiet teachingPeriod = new DanhMuc_Tiet();
            teachingPeriod.TenTiet = newTeachingPeriodName;
            teachingPeriod.MaBuoi = newSession.MaBuoi;
            teachingPeriod.ThoiGianBatDau = dtBeginTime;
            teachingPeriod.ThoiDiemKetThu = dtEndTime;

            teachingPeriodDA.UpdateTeachingPeriod(editedTeachingPeriod);
        }

        public DanhMuc_Tiet GetTeachingPeriod(int teachingPeriodId)
        {
            return teachingPeriodDA.GetTeachingPeriod(teachingPeriodId);
        }
        
        public List<DanhMuc_Tiet> GetTeachingPeriods()
        {
            return teachingPeriodDA.GetTeachingPeriods();
        }

        public List<TabularTeachingPeriod> GetTabularTeachingPeriods(string teachingPeriodName, CauHinh_Buoi session, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<DanhMuc_Tiet> lTeachingPeriods = new List<DanhMuc_Tiet>();
            List<TabularTeachingPeriod> lTbTeachingPeriods = new List<TabularTeachingPeriod>();

            if ((teachingPeriodName == "") || (string.Compare(teachingPeriodName, "tất cả", 0) == 0))
            {
                if (session == null)
                {
                    lTeachingPeriods = teachingPeriodDA.GetTeachingPeriods(pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    lTeachingPeriods = teachingPeriodDA.GetTeachingPeriods(session, pageCurrentIndex, pageSize, out totalRecords);
                }
            }
            else
            {
                if (session == null)
                {
                    lTeachingPeriods = teachingPeriodDA.GetTeachingPeriods(teachingPeriodName, pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    lTeachingPeriods = teachingPeriodDA.GetTeachingPeriods(teachingPeriodName, session, pageCurrentIndex, pageSize, out totalRecords);
                }
            }

            TabularTeachingPeriod tbTeachingPeriod = new TabularTeachingPeriod();
            foreach (DanhMuc_Tiet teachingPeriod in lTeachingPeriods)
            {
                tbTeachingPeriod.MaTiet = teachingPeriod.MaTiet;
                tbTeachingPeriod.TenTiet = teachingPeriod.TenTiet;
                tbTeachingPeriod.MaBuoi = teachingPeriod.CauHinh_Buoi.MaBuoi;
                tbTeachingPeriod.TenBuoi = teachingPeriod.CauHinh_Buoi.TenBuoi;
                tbTeachingPeriod.ThuTu = teachingPeriod.ThuTu;
                tbTeachingPeriod.ThoiGianBatDau = teachingPeriod.ThoiGianBatDau;
                tbTeachingPeriod.StringThoiGianBatDau = teachingPeriod.ThoiGianBatDau.ToShortTimeString();
                tbTeachingPeriod.ThoiGianKetThuc = teachingPeriod.ThoiDiemKetThu;
                tbTeachingPeriod.StringThoiGianKetThuc = teachingPeriod.ThoiDiemKetThu.ToShortTimeString();

                lTbTeachingPeriods.Add(tbTeachingPeriod);
            }

            return lTbTeachingPeriods;
        }

        public bool IsDeletable(DanhMuc_Tiet teachingPeriod)
        {
            return teachingPeriodDA.IsDeletable(teachingPeriod);
        }

        public string GetDetailedTeachingPeriod(DanhMuc_Tiet teachingPeriod)
        {
            string chiTietTiet = string.Format("<b>{0}</b><br/>({1}-{2})",
                    teachingPeriod.TenTiet,
                    teachingPeriod.ThoiDiemKetThu.ToShortTimeString(),
                    teachingPeriod.ThoiDiemKetThu.ToShortTimeString());
            return chiTietTiet;
        }

        public bool TeachingPeriodNameExists(string teachingPeriodName)
        {
            return teachingPeriodDA.TeachingPeriodNameExists(teachingPeriodName);
        }

        public bool TeachingPeriodNameExists(string oldTeachingPeriodName, string newTeachingPeriodName)
        {
            if (oldTeachingPeriodName == newTeachingPeriodName)
            {
                return false;
            }
            else
            {
                return teachingPeriodDA.TeachingPeriodNameExists(newTeachingPeriodName);
            }
        }
    }
}
