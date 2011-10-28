using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class ThuBL
    {
        private ThuDA thuDA;

        public ThuBL()
        {
            thuDA = new ThuDA();
        }

        public CauHinh_Thu GetThu(int maThu)
        {
            return thuDA.GetThu(maThu);
        }
    }
}
