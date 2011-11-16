﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class SessionDA : BaseDA
    {
        public SessionDA(School school)
            : base(school)
        {
        }

        public string GetSessionName(int sessionId)
        {
            if (sessionId == 0)
            {
                return "Cả ngày";
            }
            else
            {
                CauHinh_Buoi buoi = (from b in db.CauHinh_Buois
                                     where b.MaBuoi == sessionId
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
