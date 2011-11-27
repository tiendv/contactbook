using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class ScheduleBL : BaseBL
    {
        private ScheduleDA scheduleDA;

        public ScheduleBL(School school)
            : base(school)
        {
            scheduleDA = new ScheduleDA(school);
        }

        public void InsertSchedule(LopHoc_Lop Class, DanhMuc_MonHoc subject, aspnet_User teacher, CauHinh_HocKy term, CauHinh_Thu dayInWeek, DanhMuc_Tiet teachingPeriod)
        {
            LopHoc_MonHocTKB schedule = new LopHoc_MonHocTKB();
            schedule.MaLopHoc = Class.MaLopHoc;
            schedule.MaMonHoc = subject.MaMonHoc;
            schedule.TeacherId = teacher.UserId;
            schedule.MaHocKy = term.MaHocKy;
            schedule.MaThu = dayInWeek.MaThu;
            schedule.MaBuoi = teachingPeriod.MaBuoi;
            schedule.MaTiet = teachingPeriod.MaTiet;

            scheduleDA.InsertSchedule(schedule);
        }

        public void UpdateSchedule(LopHoc_MonHocTKB editedSchedule, DanhMuc_MonHoc subject, aspnet_User teacher)
        {
            editedSchedule.MaMonHoc = subject.MaMonHoc;
            editedSchedule.TeacherId = teacher.UserId;
            scheduleDA.UpdateSchedule(editedSchedule, subject, teacher);
        }

        public void DeleteSchedule(LopHoc_MonHocTKB deletedSchedule)
        {
            scheduleDA.DeleteSchedule(deletedSchedule);
        }

        public bool ScheduleExists(LopHoc_Lop Class, DanhMuc_MonHoc subject, CauHinh_HocKy term, CauHinh_Thu dayInweek, CauHinh_Buoi session)
        {
            return scheduleDA.ScheduleExists(Class, subject, term, dayInweek, session);
        }

        public bool ScheduleExists(aspnet_User teacher)
        {
            return scheduleDA.ScheduleExists(teacher);
        }

        public List<LopHoc_MonHocTKB> GetSchedules(LopHoc_Lop Class, CauHinh_HocKy term, CauHinh_Thu dayInweek, CauHinh_Buoi session)
        {
            return scheduleDA.GetSchedules(Class, term, dayInweek, session);
        }

        public List<TeachingPeriodSchedule> GetTeachingPeriodSchedules(LopHoc_Lop Class, CauHinh_HocKy term, CauHinh_Thu dayInweek)
        {
            TeachingPeriodBL teachingPeriodBL = new TeachingPeriodBL(school);
            List<TeachingPeriodSchedule> teachingPeriodSchedules = new List<TeachingPeriodSchedule>();
            TeachingPeriodSchedule teachingPeriodSchedule = null;
            LopHoc_MonHocTKB schedule = null;
            List<DanhMuc_Tiet> teachingPeriods = teachingPeriodBL.GetTeachingPeriods();

            foreach (DanhMuc_Tiet teachingPeriod in teachingPeriods)
            {
                schedule = scheduleDA.GetSchedule(Class, term, dayInweek, teachingPeriod);
                if (schedule != null)
                {
                    teachingPeriodSchedule = GetTeachingPeriodSchedule(schedule);
                }
                else
                {
                    teachingPeriodSchedule = new TeachingPeriodSchedule();
                    teachingPeriodSchedule.MaMonHoc = 0;
                    teachingPeriodSchedule.MaMonHocTKB = 0;
                }

                teachingPeriodSchedule.Tiet = teachingPeriod.MaTiet;
                teachingPeriodSchedule.ChiTietTiet = teachingPeriodBL.GetDetailedTeachingPeriod(teachingPeriod);
                teachingPeriodSchedules.Add(teachingPeriodSchedule);
            }

            return teachingPeriodSchedules;
        }

        public List<TeachingPeriodSchedule> GetTeachingPeriodSchedules(LopHoc_Lop Class, CauHinh_HocKy term, CauHinh_Thu dayInweek, CauHinh_Buoi session)
        {
            TeachingPeriodBL tietBL = new TeachingPeriodBL(school);
            List<TeachingPeriodSchedule> teachingPeriodSchedules = new List<TeachingPeriodSchedule>();
            List<LopHoc_MonHocTKB> schedules = scheduleDA.GetSchedules(Class, term, dayInweek, session);
            TeachingPeriodSchedule teachingPeriodSchedule = null;
            DanhMuc_Tiet teachingPeriod = null;

            foreach (LopHoc_MonHocTKB schedule in schedules)
            {
                teachingPeriodSchedule = GetTeachingPeriodSchedule(schedule);

                teachingPeriod = schedule.DanhMuc_Tiet;
                teachingPeriodSchedule.ChiTietTiet = string.Format("{0}({1}-{2})",
                    teachingPeriod.TenTiet,
                    teachingPeriod.ThoiDiemKetThu.ToShortTimeString(),
                    teachingPeriod.ThoiDiemKetThu.ToShortTimeString());
            }

            return teachingPeriodSchedules;
        }

        public List<SessionedSchedule> GetSessionedSchedules(LopHoc_Lop Class, CauHinh_HocKy term, CauHinh_Thu dayInweek)
        {
            List<SessionedSchedule> sessionedSchedules = new List<SessionedSchedule>();
            List<TeachingPeriodSchedule> teachingPeriodSchedules = new List<TeachingPeriodSchedule>();
            List<LopHoc_MonHocTKB> schedules = new List<LopHoc_MonHocTKB>();
            SessionedSchedule sessionedSchedule = null;
            DanhMuc_Tiet teachingPeriod = null;
            SystemConfigBL systemConfigBL = new SystemConfigBL(school);
            TeachingPeriodBL teachingPeriodBL = new TeachingPeriodBL(school);
            List<CauHinh_Buoi> sessions = systemConfigBL.GetSessions();

            foreach (CauHinh_Buoi session in sessions)
            {
                sessionedSchedule = new SessionedSchedule();
                sessionedSchedule.MaBuoi = session.MaBuoi;
                schedules = GetSchedules(Class, term, dayInweek, session);
                teachingPeriodSchedules = new List<TeachingPeriodSchedule>();
                foreach (LopHoc_MonHocTKB schedule in schedules)
                {
                    TeachingPeriodSchedule teachingPeriodSchedule = new TeachingPeriodSchedule();
                    teachingPeriodSchedule.MaMonHocTKB = schedule.MaMonHocTKB;
                    teachingPeriodSchedule.MaMonHoc = schedule.MaMonHoc;
                    teachingPeriodSchedule.TenMonHoc = schedule.DanhMuc_MonHoc.TenMonHoc;
                    teachingPeriodSchedule.MaGiaoVien = schedule.TeacherId;
                    teachingPeriodSchedule.TenGiaoVien = schedule.aspnet_User.aspnet_Membership.RealName;
                    teachingPeriodSchedule.MaLopHoc = schedule.MaLopHoc;
                    teachingPeriodSchedule.MaHocKy = schedule.MaHocKy;
                    teachingPeriodSchedule.MaThu = schedule.MaThu;
                    teachingPeriodSchedule.Tiet = schedule.MaTiet;
                    teachingPeriod = teachingPeriodBL.GetTeachingPeriod(schedule.MaTiet);
                    teachingPeriodSchedule.ChiTietTiet = teachingPeriodBL.GetDetailedTeachingPeriod(teachingPeriod);

                    teachingPeriodSchedules.Add(teachingPeriodSchedule);
                }
                sessionedSchedule.ListThoiKhoaBieuTheoTiet = teachingPeriodSchedules;
                sessionedSchedules.Add(sessionedSchedule);
            }

            return sessionedSchedules;
        }

        public List<DailySchedule> GetDailySchedules(LopHoc_Lop Class, CauHinh_HocKy term)
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(school);
            List<DailySchedule> dailySchedules = new List<DailySchedule>();
            List<CauHinh_Thu> dayInWeeks = systemConfigBL.GetDayInWeeks();
            DailySchedule dailySchedule = null;

            foreach (CauHinh_Thu dayInWeek in dayInWeeks)
            {
                dailySchedule = new DailySchedule();
                dailySchedule.MaLopHoc = Class.MaLopHoc;
                dailySchedule.MaHocKy = term.MaHocKy;
                dailySchedule.MaThu = dayInWeek.MaThu;
                dailySchedule.TenThu = dayInWeek.TenThu;
                dailySchedule.SessionedSchedules = GetSessionedSchedules(Class, term, dayInWeek);
                dailySchedules.Add(dailySchedule);
            }

            return dailySchedules;
        }

        public List<DanhMuc_MonHoc> GetScheduledSubjects(LopHoc_Lop Class, CauHinh_HocKy term)
        {
            return scheduleDA.GetScheduledSubjects(Class, term);
        }

        public TeachingPeriodSchedule GetTeachingPeriodSchedule(LopHoc_MonHocTKB schedule)
        {
            TeachingPeriodSchedule teachingPeriodSchedule = new TeachingPeriodSchedule();

            teachingPeriodSchedule.MaMonHocTKB = schedule.MaMonHocTKB;
            teachingPeriodSchedule.MaLopHoc = schedule.MaLopHoc;
            teachingPeriodSchedule.MaMonHoc = schedule.MaMonHoc;
            teachingPeriodSchedule.TenMonHoc = schedule.DanhMuc_MonHoc.TenMonHoc;
            teachingPeriodSchedule.MaGiaoVien = schedule.TeacherId;
            teachingPeriodSchedule.TenGiaoVien = schedule.aspnet_User.aspnet_Membership.RealName;
            teachingPeriodSchedule.Tiet = schedule.DanhMuc_Tiet.MaTiet;
            teachingPeriodSchedule.MaThu = schedule.CauHinh_Thu.MaThu;

            return teachingPeriodSchedule;
        }

        public LopHoc_MonHocTKB GetSchedule(int scheduleId)
        {
            return scheduleDA.GetSchedule(scheduleId);
        }
    }
}
