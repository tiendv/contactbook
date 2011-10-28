using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class NamHocDA : BaseDA
    {
        public NamHocDA()
            : base()
        {
        }

        public CauHinh_NamHoc GetNamHoc(int maNamHoc)
        {
            CauHinh_NamHoc namHoc = (from nh in db.CauHinh_NamHocs
                                     where nh.MaNamHoc == maNamHoc
                                     select nh).First();
            return namHoc;
        }

        public List<CauHinh_NamHoc> GetListNamHoc()
        {
            IQueryable<CauHinh_NamHoc> namhoc = (from nh in db.CauHinh_NamHocs
                                                select nh).OrderByDescending(n => n.NamBatDau);
            return namhoc.ToList();
        }
    }
}
