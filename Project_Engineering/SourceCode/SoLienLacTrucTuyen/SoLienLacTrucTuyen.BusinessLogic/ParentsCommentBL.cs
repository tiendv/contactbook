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

        public ParentsCommentBL(School_School school)
            : base(school)
        {
            parentsCommentDA = new ParentsCommentDA(school);
        }

        public void Reply(ParentComment_Comment parentsComment, string reply)
        {
            parentsCommentDA.UpdateParentsComments(parentsComment, reply);
        }

        public List<TabularParentsComment> GetTabularParentsComments(Configuration_Year year, Configuration_CommentStatus commentStatus,
            DateTime beginDate, DateTime endDate, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            // Declare variables
            List<TabularParentsComment> tabularParentsComments;
            TabularParentsComment tabularParentsComment = null;
            List<ParentComment_Comment> parentsComments = null;

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

            // Convert ParentComment_Comments to TabularParentsComments
            tabularParentsComments = new List<TabularParentsComment>();
            foreach (ParentComment_Comment parentsComment in parentsComments)
            {
                tabularParentsComment = new TabularParentsComment();
                tabularParentsComment.CommentId = parentsComment.CommentId;
                tabularParentsComment.Title = parentsComment.Title;
                tabularParentsComment.Content = parentsComment.CommentContent;
                tabularParentsComment.CommentStatusName = parentsComment.Configuration_CommentStatus.CommentStatusName;
                tabularParentsComment.Date = parentsComment.Date.ToShortDateString();

                tabularParentsComments.Add(tabularParentsComment);
            }

            return tabularParentsComments;
        }

        public List<Configuration_CommentStatus> GetCommentStatuses()
        {
            List<Configuration_CommentStatus> commentStatuses;
            Configuration_CommentStatus optAll = new Configuration_CommentStatus();
            optAll.CommentStatusId = 0;
            optAll.CommentStatusName = "Tất cả";

            commentStatuses = parentsCommentDA.GetCommentStatuses();
            commentStatuses.Add(optAll);
            return commentStatuses;
        }

        public ParentComment_Comment GetParentsComments(int commentId)
        {
            return parentsCommentDA.GetParentsComments(commentId);
        }
    }
}
