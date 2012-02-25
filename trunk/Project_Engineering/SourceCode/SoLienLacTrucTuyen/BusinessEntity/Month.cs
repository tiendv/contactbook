using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.BusinessEntity
{
    public class Month
    {
        public int MonthId { get; set; }
        public string MonthName { get; set; }

        public static string GetMonthName(int monthId)
        {
            string strMonthName="";
            switch (monthId)
            {
                case 1:
                    {
                        strMonthName = "Tháng 1";
                        break;
                    }
                case 2:
                    {
                        strMonthName = "Tháng 2";
                        break;
                    }
                case 3:
                    {
                        strMonthName = "Tháng 3";
                        break;
                    }
                case 4:
                    {
                        strMonthName = "Tháng 4";
                        break;
                    }
                case 5:
                    {
                        strMonthName = "Tháng 5";
                        break;
                    }
                case 6:
                    {
                        strMonthName = "Tháng 6";
                        break;
                    }
                case 7:
                    {
                        strMonthName = "Tháng 7";
                        break;
                    }
                case 8:
                    {
                        strMonthName = "Tháng 8";
                        break;
                    }
                case 9:
                    {
                        strMonthName = "Tháng 9";
                        break;
                    }
                case 10:
                    {
                        strMonthName = "Tháng 10";
                        break;
                    }
                case 11:
                    {
                        strMonthName = "Tháng 11";
                        break;
                    }
                case 12:
                    {
                        strMonthName = "Tháng 12";
                        break;
                    }
                default: break;
            }        

            return strMonthName;
        }
    }

    public class Week
    {
        public string WeekId
        {
            get
            {   
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(BeginDate.ToString("dd/MM/yyyy"));
                stringBuilder.Append(" - ");
                stringBuilder.Append(EndDate.ToString("dd/MM/yyyy"));
                return stringBuilder.ToString();
            }
        }
        public int WeekIndex { get; set; }
        public string WeekName 
        {
            get 
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("Tuần ");
                stringBuilder.Append(WeekIndex);
                stringBuilder.Append(" ("); 
                stringBuilder.Append(BeginDate.ToString("dd/MM/yyyy"));
                stringBuilder.Append(" - ");
                stringBuilder.Append(EndDate.ToString("dd/MM/yyyy"));
                stringBuilder.Append(")");
                return stringBuilder.ToString();
            }
        }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }        
    }
}
