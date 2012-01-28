using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.DataAccess
{
    public class ParentsCommentDA : BaseDA
    {
        public ParentsCommentDA(School_School school)
            : base(school)
        {

        }

        public void InsertParentsComment(ParentComment_Comment parentsComment)
        {
            db.ParentComment_Comments.InsertOnSubmit(parentsComment);
            db.SubmitChanges();
        }

        /// <summary>
        /// Update parent's comment
        /// </summary>
        /// <param name="editedParentsComment"></param>
        /// <param name="feedback"></param>
        public void UpdateParentsComments(ParentComment_Comment editedParentsComment, string feedback)
        {
            ParentComment_Comment parentsComment = null;

            IQueryable<ParentComment_Comment> iqParentsComments = from cmt in db.ParentComment_Comments
                                                                  where cmt.CommentId == editedParentsComment.CommentId
                                                                  select cmt;
            if (iqParentsComments.Count() != 0)
            {
                parentsComment = iqParentsComments.First();
                parentsComment.Feedback = feedback;
                parentsComment.CommentStatusId = 2; // Đã phản hồi (PH chưa xem)

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

        public List<ParentComment_Comment> GetFedbackParentsComments(Configuration_Year year, DateTime beginDate, DateTime endDate, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<ParentComment_Comment> iqParentsComments = from cmt in db.ParentComment_Comments
                                                                  where cmt.Student_StudentInClass.Class_Class.YearId == year.YearId
                                                                  && cmt.Date >= beginDate && cmt.Date <= endDate
                                                                  && cmt.CommentStatusId > 1
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

        public List<ParentComment_Comment> GetParentsComments(Student_Student student, Configuration_Year year, DateTime beginDate, DateTime endDate, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<ParentComment_Comment> iqParentsComments = from cmt in db.ParentComment_Comments
                                                                  where cmt.Student_StudentInClass.Class_Class.YearId == year.YearId
                                                                  && cmt.Date >= beginDate && cmt.Date <= endDate
                                                                  && cmt.Student_StudentInClass.StudentId == student.StudentId
                                                                  select cmt;

            return GetParentsComments(ref iqParentsComments, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<ParentComment_Comment> GetParentsComments(Student_Student student, Configuration_Year year, Configuration_CommentStatus commentStatus, DateTime beginDate, DateTime endDate, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<ParentComment_Comment> iqParentsComments = from cmt in db.ParentComment_Comments
                                                                  where cmt.Student_StudentInClass.Class_Class.YearId == year.YearId
                                                                  && cmt.CommentStatusId == commentStatus.CommentStatusId
                                                                  && cmt.Date >= beginDate && cmt.Date <= endDate
                                                                  && cmt.Student_StudentInClass.StudentId == student.StudentId
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
                parentsComments = iqParentsComment.OrderByDescending(cmt => cmt.Date)
                    .ThenBy(cmt => cmt.CommentStatusId).Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
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

        public void DeleteParentsComment(ParentComment_Comment comment)
        {
            ParentComment_Comment parentsComment = null;

            IQueryable<ParentComment_Comment> iqParentsComments = from cmt in db.ParentComment_Comments
                                                                  where cmt.CommentId == comment.CommentId
                                                                  select cmt;
            if (iqParentsComments.Count() != 0)
            {
                parentsComment = iqParentsComments.First();
                db.ParentComment_Comments.DeleteOnSubmit(parentsComment);
                db.SubmitChanges();
            }
        }

        public void UpdateParentsComment(ParentComment_Comment comment, string newContent)
        {
            ParentComment_Comment parentsComment = null;

            IQueryable<ParentComment_Comment> iqParentsComments = from cmt in db.ParentComment_Comments
                                                                  where cmt.CommentId == comment.CommentId
                                                                  select cmt;
            if (iqParentsComments.Count() != 0)
            {
                parentsComment = iqParentsComments.First();
                parentsComment.CommentContent = newContent;
                db.SubmitChanges();
            }
        }

        public void DeleteParentsComment(Student_Student deletedStudent)
        {
            IQueryable<ParentComment_Comment> iqComment;
            iqComment = from comment in db.ParentComment_Comments
                        where comment.Student_StudentInClass.StudentId == deletedStudent.StudentId
                        select comment;
            if (iqComment.Count() != 0)
            {
                foreach (ParentComment_Comment termLearningResult in iqComment)
                {
                    db.ParentComment_Comments.DeleteOnSubmit(termLearningResult);
                }

                db.SubmitChanges();
            }
        }

        public void UpdateParentsComment(ParentComment_Comment comment, Configuration_CommentStatus commentStatus)
        {
            IQueryable<ParentComment_Comment> iqParentsComments = from cmt in db.ParentComment_Comments
                                                                  where cmt.CommentId == comment.CommentId
                                                                  select cmt;
            if (iqParentsComments.Count() != 0)
            {
                comment = iqParentsComments.First();
                comment.CommentStatusId = commentStatus.CommentStatusId;

                db.SubmitChanges();
            }
        }

        public int GetUnConfirmedFedbackComment(Student_Student student, Class_Class Class)
        {
            IQueryable<ParentComment_Comment> iqComments = from comment in db.ParentComment_Comments
                                                           where comment.Student_StudentInClass.StudentId == student.StudentId
                                                           && comment.Student_StudentInClass.Class_Class.YearId == Class.YearId
                                                           && comment.CommentStatusId == 2
                                                           select comment;
            return iqComments.Count();
        }

        public bool IsDeletable(ParentComment_Comment comment)
        {
            IQueryable<ParentComment_Comment> iqComments = from c in db.ParentComment_Comments
                                                           where c.CommentId == comment.CommentId 
                                                            && (c.CommentStatusId == 2 || c.CommentStatusId == 3) // read or replied
                                                           select c;
            if (iqComments.Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetUnfeedbackCommentCount()
        {
            IQueryable<ParentComment_Comment> iqComments = from c in db.ParentComment_Comments
                                                           where c.Student_StudentInClass.Class_Class.SchoolId == school.SchoolId
                                                            && c.CommentStatusId == 1
                                                           select c;
            return iqComments.Count();
        }
    }
}
