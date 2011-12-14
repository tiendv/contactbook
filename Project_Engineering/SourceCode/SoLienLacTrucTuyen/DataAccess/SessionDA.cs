using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.DataAccess
{
    public class SessionDA : BaseDA
    {
        public SessionDA(School_School school)
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
                Configuration_Session buoi = (from b in db.Configuration_Sessions
                                     where b.SessionId == sessionId
                                     select b).First();
                return buoi.SessionName;
            }
        }

        public List<Configuration_Session> GetListSessions()
        {
            List<Configuration_Session> lSesssions = new List<Configuration_Session>();

            IQueryable<Configuration_Session> iqSessions = from session in db.Configuration_Sessions
                                                  select session;
            if (iqSessions.Count() != 0)
            {
                lSesssions = iqSessions.ToList();
            }

            return lSesssions;
        }
    }
}
