using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class CheckUntils
    {
        public static bool IsNullOrBlank(string str)
        {
            if (str == null)
            {
                return true;
            }
            else if (str == string.Empty)
            {
                return true;
            }
            else if (str.Trim() == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsAllOrBlank(string str)
        {
            if (IsNullOrBlank(str))
            {
                return true;
            }
            else if (str.Trim() == "tất cả")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int CompareDateWithoutHMS(DateTime dateTime1, DateTime dateTime2)
        {
            dateTime1 = new DateTime(dateTime1.Year, dateTime1.Month, dateTime1.Day);
            dateTime2 = new DateTime(dateTime2.Year, dateTime2.Month, dateTime2.Day);

            return DateTime.Compare(dateTime1, dateTime2);
        }
    }
}
