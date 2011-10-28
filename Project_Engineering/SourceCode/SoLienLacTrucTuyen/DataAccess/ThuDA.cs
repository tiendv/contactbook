using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class ThuDA : BaseDA
    {
        public ThuDA()
            : base()
        {
        }

        public CauHinh_Thu GetThu(int maThu)
        {
            CauHinh_Thu thu = (from nh in db.CauHinh_Thus
                               where nh.MaThu == maThu
                               select nh).First();
            return thu;
        }
    }
}
