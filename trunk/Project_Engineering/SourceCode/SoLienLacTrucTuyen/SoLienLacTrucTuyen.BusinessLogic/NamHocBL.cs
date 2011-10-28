using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class NamHocBL
    {
        NamHocDA namHocDA;
        public NamHocBL()
        {
            namHocDA = new NamHocDA();
        }

        public CauHinh_NamHoc GetNamHoc(int maNamHoc)
        {
            return namHocDA.GetNamHoc(maNamHoc);
        }

        public List<CauHinh_NamHoc> GetListNamHoc()
        {
            return namHocDA.GetListNamHoc();
        }
    }
}
