using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.BusinessEntity
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
        public enum SCHEDULE_FLAG
        {
            ADD,
            DELETE,
            UPDATE
        }

        public int ScheduleId { get; set; }

        // subject
        public int OrginalSubjectId { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }

        // teacher
        public Guid OrginalUserId { get; set; }
        public Guid UserId { get; set; }
        public string TeacherName { get; set; }

        // teaching period
        public int TeachingPeriodId { get; set; }
        public string StringDetailTeachingPeriod { get; set; }

        // class
        public int ClassId { get; set; }
        public string ClassName { get; set; }

        // day in week
        public int DayInWeekId { get; set; }
        public string DayInWeekName { get; set; }

        // term
        public int TermId { get; set; }
        public string TermName { get; set; }

        public string YearName { get; set; }

        public int SessionId { get; set; }
        

        public TeachingPeriodSchedule()
        {
            UserId = new Guid();
        }
    }
}
