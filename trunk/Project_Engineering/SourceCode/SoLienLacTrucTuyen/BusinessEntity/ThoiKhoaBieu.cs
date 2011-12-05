using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.BusinessEntity
{
    public class DailySchedule
    {
        public int ClassId { get; set; }

        public int YearId { get; set; }
        public int TermId { get; set; }
        public int DayInWeekId { get; set; }
        public string DayInWeekName { get; set; }

        public List<SessionedSchedule> SessionedSchedules { get; set; }
    }

    public class SessionedSchedule
    {
        public int SessionId { get; set; }
        public List<TeachingPeriodSchedule> ListThoiKhoaBieuTheoTiet { get; set; }
    }

    public class TeachingPeriodSchedule
    {
        public int ScheduleId { get; set; }
        public int SubjectId { get; set; }        
        public string SubjectName { get; set; }
        public Guid UserId { get; set; }
        public string TeacherName { get; set; }
        public int TeachingPeriodId { get; set; }
        public string StringDetailTeachingPeriod { get; set; }
                
        public int ClassId { get; set; }
        public int DayInWeekId { get; set; }
        public int TermId { get; set; }
    }
}
