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
            if (str == null || str.Trim() == "")
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
