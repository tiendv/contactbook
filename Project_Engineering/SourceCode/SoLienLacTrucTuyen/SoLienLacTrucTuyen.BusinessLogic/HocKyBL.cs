using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class HocKyBL
    {
        private HocKyDA hocKyDA;

        public HocKyBL()
        {
            hocKyDA = new HocKyDA();
        }

        public CauHinh_HocKy GetHocKy(int maHocKy)
        {
            return hocKyDA.GetHocKy(maHocKy);
        }

        public List<CauHinh_HocKy> GetListHocKy()
        {
            return hocKyDA.GetListHocKy();
        }
    }
}
