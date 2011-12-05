using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.BusinessEntity
{
    public class TabularTeachingPeriod
    {
        private int teachingPeriodId;
        public int TeachingPeriodId
        {
            get
            {
                return teachingPeriodId;
            }
            set
            {
                teachingPeriodId = value;
            }
        }

        public string TeachingPeriodName { get; set; }
        public string StringTiet { get; set; }

        public int SessionId { get; set; }
        public string SessionName { get; set; }

        public int TeachingPeriodOrder { get; set; }

        private DateTime beginTime;
        public DateTime BeginTime
        {
            get
            {
                return beginTime;
            }
            set
            {
                beginTime = value;
                StringBeginTime = beginTime.ToShortTimeString();
            }
        }
        public string StringBeginTime { get; set; }

        private DateTime endTime;
        public DateTime EndTime
        {
            get
            {
                return endTime;
            }
            set
            {
                endTime = value;
                StringEndTime = endTime.ToShortTimeString();
            }
        }
        public string StringEndTime { get; set; }
    }
}
