using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class ParentsCommentBL : BaseBL
    {
        ParentsCommentDA parentsCommentDA;

        public ParentsCommentBL(School school)
            : base(school)
        {
            parentsCommentDA = new ParentsCommentDA(school);
        }

        public void Reply(GopY_YKien parentsComment, string reply)
        {
            parentsCommentDA.UpdateParentsComments(parentsComment, reply);
        }

        public List<TabularParentsComment> GetTabularParentsComments(CauHinh_NamHoc year, CauHinh_TinhTrangYKien commentStatus,
            DateTime beginDate, DateTime endDate, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            // Declare variables
            List<TabularParentsComment> tabularParentsComments;
            TabularParentsComment tabularParentsComment = null;
            List<GopY_YKien> parentsComments = null;

            // get list GopY_YKien
            if (commentStatus != null)
            {
                parentsComments = parentsCommentDA.GetParentsComments(year, commentStatus, beginDate, endDate,
                    pageCurrentIndex, pageSize, out totalRecords);
            }
            else
            {
                parentsComments = parentsCommentDA.GetParentsComments(year, beginDate, endDate,
                    pageCurrentIndex, pageSize, out totalRecords);
            }

            // Convert GopY_YKiens to TabularParentsComments
            tabularParentsComments = new List<TabularParentsComment>();
            foreach (GopY_YKien parentsComment in parentsComments)
            {
                tabularParentsComment = new TabularParentsComment();
                tabularParentsComment.MaYKien = parentsComment.MaYKien;
                tabularParentsComment.TieuDe = parentsComment.TieuDe;
                tabularParentsComment.NoiDung = parentsComment.NoiDung;
                tabularParentsComment.TinhTrangYKien = parentsComment.CauHinh_TinhTrangYKien.TenTinhTrangYKien;
                tabularParentsComment.Ngay = parentsComment.Ngay.ToShortDateString();

                tabularParentsComments.Add(tabularParentsComment);
            }

            return tabularParentsComments;
        }

        public List<CauHinh_TinhTrangYKien> GetCommentStatuses()
        {
            List<CauHinh_TinhTrangYKien> commentStatuses;
            CauHinh_TinhTrangYKien optAll = new CauHinh_TinhTrangYKien();
            optAll.MaTinhTrangYKien = 0;
            optAll.TenTinhTrangYKien = "Tất cả";

            commentStatuses = parentsCommentDA.GetCommentStatuses();
            commentStatuses.Add(optAll);
            return commentStatuses;
        }

        public GopY_YKien GetParentsComments(int commentId)
        {
            return parentsCommentDA.GetParentsComments(commentId);
        }
    }
}
