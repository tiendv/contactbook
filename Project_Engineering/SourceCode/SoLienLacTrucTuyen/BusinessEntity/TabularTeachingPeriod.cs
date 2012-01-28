using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.BusinessEntity
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

        public DateTime BeginTime { get; set; }

        public string StringBeginTime
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(BeginTime.Hour);
                stringBuilder.Append(" giờ ");
                if (BeginTime.Minute != 0)
                {
                    stringBuilder.Append(BeginTime.Minute);
                    stringBuilder.Append(" phút ");
                }
                return stringBuilder.ToString();
            }
        }

        public DateTime EndTime { get; set; }

        public string StringEndTime
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(EndTime.Hour);
                stringBuilder.Append(" giờ ");
                if (EndTime.Minute != 0)
                {
                    stringBuilder.Append(EndTime.Minute);
                    stringBuilder.Append(" phút ");
                }

                return stringBuilder.ToString();
            }
        }
    }
}
