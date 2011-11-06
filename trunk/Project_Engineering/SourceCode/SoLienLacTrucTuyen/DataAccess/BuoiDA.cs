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

        public string GetSessionName(int sessionName)
        {
            if (sessionName == 0)
            {
                return "Cả ngày";
            }
            else
            {
                CauHinh_Buoi buoi = (from b in db.CauHinh_Buois
                                     where b.MaBuoi == sessionName
                                     select b).First();
                return buoi.TenBuoi;
            }
        }

        public List<CauHinh_Buoi> GetListSessions()
        {
            List<CauHinh_Buoi> lSesssions = new List<CauHinh_Buoi>();

            IQueryable<CauHinh_Buoi> iqSessions = from session in db.CauHinh_Buois
                                                  select session;
            if (iqSessions.Count() != 0)
            {
                lSesssions = iqSessions.ToList();
            }

            return lSesssions;
        }
    }
}
