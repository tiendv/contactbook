using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class BuoiDA : BaseDA
    {
        public BuoiDA()
            : base()
        {
        }

        public string GetTenBuoi(int maBuoi)
        {
            if (maBuoi == 0)
            {
                return "Cả ngày";
            }
            else
            {
                CauHinh_Buoi buoi = (from b in db.CauHinh_Buois
                                     where b.MaBuoi == maBuoi
                                     select b).First();
                return buoi.TenBuoi;
            }
        }

        public List<CauHinh_Buoi> GetListBuoi()
        {
            IQueryable<CauHinh_Buoi> buois = from b in db.CauHinh_Buois
                                             select b;
            if (buois.Count() != 0)
            {
                return buois.ToList();
            }
            else
            {
                return new List<CauHinh_Buoi>();
            }
        }
    }
}
