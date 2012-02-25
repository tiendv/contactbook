using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class DateUtils
    {
        public enum LOCALE
        {
            VIETNAMESE
        }

        public static int CompareDateWithoutHMS(DateTime dateTime1, DateTime dateTime2)
        {
            dateTime1 = new DateTime(dateTime1.Year, dateTime1.Month, dateTime1.Day);
            dateTime2 = new DateTime(dateTime2.Year, dateTime2.Month, dateTime2.Day);

            return DateTime.Compare(dateTime1, dateTime2);
        }

        public static string DateToString(DateTime datetime, LOCALE locale)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (LOCALE.VIETNAMESE == locale)
            {
                DateTimeFormatInfo dtfi = CultureInfo.CreateSpecificCulture("vi-Vi").DateTimeFormat;
                dtfi.DateSeparator = "/";
                dtfi.ShortDatePattern = @"dd/MM/yyyy";
                return datetime.ToString("dd/MM/yyyy");
            }

            return stringBuilder.ToString();
        }

        public const string PATTERN_DDMMYYYY = "dd/MM/yyyy";

        public static DateTime? StringToDateVN(string str)
        {
            DateTime datetime = DateTime.Now;

            try
            {
                datetime = DateTime.Parse(str, new CultureInfo("vi-VN"));
            }
            catch (Exception ex)
            {
                return null;
            }

            return datetime;
        }
    }
}
