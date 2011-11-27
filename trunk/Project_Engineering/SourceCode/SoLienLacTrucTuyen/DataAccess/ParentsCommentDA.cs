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

        public void UpdateParentsComments(GopY_YKien editedParentsComment, string reply)
        {
            GopY_YKien parentsComment = null;

            IQueryable<GopY_YKien> iqParentsComments = from cmt in db.GopY_YKiens
                                                       where cmt.MaYKien == editedParentsComment.MaYKien
                                                       select cmt;
            if (iqParentsComments.Count() != 0)
            {
                parentsComment = iqParentsComments.First();
                parentsComment.PhanHoi = reply;
                db.SubmitChanges();
            }
        }

        public List<GopY_YKien> GetParentsComments(CauHinh_NamHoc year, DateTime beginDate, DateTime endDate, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<GopY_YKien> iqParentsComments = from cmt in db.GopY_YKiens
                                                       where cmt.HocSinh_HocSinhLopHoc.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                                                       && cmt.Ngay >= beginDate && cmt.Ngay <= endDate
                                                       && cmt.SchoolId == school.SchoolId
                                                       select cmt;

            return GetParentsComments(ref iqParentsComments, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<GopY_YKien> GetParentsComments(CauHinh_NamHoc year, CauHinh_TinhTrangYKien commentStatus, DateTime beginDate, DateTime endDate, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<GopY_YKien> iqParentsComments = from cmt in db.GopY_YKiens
                                                       where cmt.HocSinh_HocSinhLopHoc.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                                                       && cmt.MaTinhTrangYKien == commentStatus.MaTinhTrangYKien
                                                       && cmt.Ngay >= beginDate && cmt.Ngay <= endDate
                                                       && cmt.SchoolId == school.SchoolId
                                                       select cmt;

            return GetParentsComments(ref iqParentsComments, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<CauHinh_TinhTrangYKien> GetCommentStatuses()
        {
            List<CauHinh_TinhTrangYKien> commentStatuses = new List<CauHinh_TinhTrangYKien>();
            IQueryable<CauHinh_TinhTrangYKien> iqCommentStatus = from cmtStt in db.CauHinh_TinhTrangYKiens
                                                                 select cmtStt;
            if (iqCommentStatus.Count() != 0)
            {
                commentStatuses = iqCommentStatus.ToList();
            }

            return commentStatuses;

        }

        private List<GopY_YKien> GetParentsComments(ref IQueryable<GopY_YKien> iqParentsComment, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<GopY_YKien> parentsComments = new List<GopY_YKien>();

            totalRecords = iqParentsComment.Count();
            if (totalRecords != 0)
            {
                parentsComments = iqParentsComment.OrderByDescending(cmt => cmt.MaTinhTrangYKien)
                    .ThenByDescending(cmt => cmt.Ngay).Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return parentsComments;
        }

        public GopY_YKien GetParentsComments(int commentId)
        {
            GopY_YKien parentsComment = null;

            IQueryable<GopY_YKien> iqParentsComments = from cmt in db.GopY_YKiens
                                                       where cmt.MaYKien == commentId
                                                       select cmt;
            if (iqParentsComments.Count() != 0)
            {
                parentsComment = iqParentsComments.First();
            }

            return parentsComment;
        }
    }
}
