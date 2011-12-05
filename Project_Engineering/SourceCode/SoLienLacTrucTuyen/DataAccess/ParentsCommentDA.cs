using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class ParentsCommentDA : BaseDA
    {
        public ParentsCommentDA(School_School school)
            : base(school)
        {

        }

        public void UpdateParentsComments(ParentComment_Comment editedParentsComment, string reply)
        {
            ParentComment_Comment parentsComment = null;

            IQueryable<ParentComment_Comment> iqParentsComments = from cmt in db.ParentComment_Comments
                                                       where cmt.CommentId == editedParentsComment.CommentId
                                                       select cmt;
            if (iqParentsComments.Count() != 0)
            {
                parentsComment = iqParentsComments.First();
                parentsComment.Feedback = reply;
                db.SubmitChanges();
            }
        }

        public List<ParentComment_Comment> GetParentsComments(Configuration_Year year, DateTime beginDate, DateTime endDate, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<ParentComment_Comment> iqParentsComments = from cmt in db.ParentComment_Comments
                                                       where cmt.Student_StudentInClass.Class_Class.YearId == year.YearId
                                                       && cmt.Date >= beginDate && cmt.Date <= endDate
                                                       select cmt;

            return GetParentsComments(ref iqParentsComments, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<ParentComment_Comment> GetParentsComments(Configuration_Year year, Configuration_CommentStatus commentStatus, DateTime beginDate, DateTime endDate, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<ParentComment_Comment> iqParentsComments = from cmt in db.ParentComment_Comments
                                                       where cmt.Student_StudentInClass.Class_Class.YearId == year.YearId
                                                       && cmt.CommentStatusId == commentStatus.CommentStatusId
                                                       && cmt.Date >= beginDate && cmt.Date <= endDate
                                                       select cmt;

            return GetParentsComments(ref iqParentsComments, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Configuration_CommentStatus> GetCommentStatuses()
        {
            List<Configuration_CommentStatus> commentStatuses = new List<Configuration_CommentStatus>();
            IQueryable<Configuration_CommentStatus> iqCommentStatus = from cmtStt in db.Configuration_CommentStatus
                                                                 select cmtStt;
            if (iqCommentStatus.Count() != 0)
            {
                commentStatuses = iqCommentStatus.ToList();
            }

            return commentStatuses;

        }

        private List<ParentComment_Comment> GetParentsComments(ref IQueryable<ParentComment_Comment> iqParentsComment, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<ParentComment_Comment> parentsComments = new List<ParentComment_Comment>();

            totalRecords = iqParentsComment.Count();
            if (totalRecords != 0)
            {
                parentsComments = iqParentsComment.OrderByDescending(cmt => cmt.CommentStatusId)
                    .ThenByDescending(cmt => cmt.Date).Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return parentsComments;
        }

        public ParentComment_Comment GetParentsComments(int commentId)
        {
            ParentComment_Comment parentsComment = null;

            IQueryable<ParentComment_Comment> iqParentsComments = from cmt in db.ParentComment_Comments
                                                       where cmt.CommentId == commentId
                                                       select cmt;
            if (iqParentsComments.Count() != 0)
            {
                parentsComment = iqParentsComments.First();
            }

            return parentsComment;
        }
    }
}
