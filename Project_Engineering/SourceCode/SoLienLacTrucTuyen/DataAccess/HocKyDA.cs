using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class HocKyDA : BaseDA
    {
        public HocKyDA()
            : base()
        {
        }

        public CauHinh_HocKy GetHocKy(int maHocKy)
        {
            CauHinh_HocKy hocKy = (from hk in db.CauHinh_HocKies
                                   where hk.MaHocKy == maHocKy
                                   select hk).First();
            return hocKy;
        }

        public List<CauHinh_HocKy> GetListHocKy()
        {
            IQueryable<CauHinh_HocKy> hocKies = (from hk in db.CauHinh_HocKies
                                               select hk).OrderByDescending(hk => hk.MaHocKy);
            return hocKies.ToList();
        }
    }
}
