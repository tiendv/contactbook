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
    }
}
