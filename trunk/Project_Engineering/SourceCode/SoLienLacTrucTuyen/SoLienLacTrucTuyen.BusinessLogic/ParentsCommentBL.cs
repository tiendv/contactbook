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

        public ParentsCommentBL(School school): base(school)
        {
            parentsCommentDA = new ParentsCommentDA(school);
        }

        public List<TabularParentsComment> GetTabularParentsComments(CauHinh_NamHoc year, DateTime beginDate, DateTime endDate,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularParentsComment> tabularParentsComments;
            TabularParentsComment tabularParentsComment = null;
            List<GopY_YKien> parentsComments = parentsCommentDA.GetParentsComments(year, beginDate, endDate, 
                pageCurrentIndex, pageSize, out totalRecords);

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
    }
}
