using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class BuoiBL
    {
        private BuoiDA buoiDA;

        public BuoiBL()
        {
            buoiDA = new BuoiDA();
        }

        public List<CauHinh_Buoi> GetListBuoi()
        {
            return buoiDA.GetListSessions();
        }
    }
}
