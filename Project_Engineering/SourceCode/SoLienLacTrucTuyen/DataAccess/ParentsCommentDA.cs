using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class ParentsCommentDA : BaseDA
    {
        public ParentsCommentDA(School school)
            : base(school)
        {

        }

        public List<GopY_YKien> GetParentsComments(CauHinh_NamHoc year, DateTime beginDate, DateTime endDate, 
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<GopY_YKien> parentsComments = new List<GopY_YKien>();

            IQueryable<GopY_YKien> iqParentsComments = from cmt in db.GopY_YKiens
                                                       where cmt.HocSinh_HocSinhLopHoc.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                                                       && cmt.Ngay >= beginDate && cmt.Ngay <= endDate
                                                       select cmt;

            totalRecords = iqParentsComments.Count();
            if (totalRecords != 0)
            {
                parentsComments = iqParentsComments.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return parentsComments;
        }
    }
}
