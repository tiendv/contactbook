using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class ParentsCommentBL : BaseBL
    {
        ParentsCommentDA parentsCommentDA;

        public void InsertParentsComment(Student_Student student, string title, string content)
        {
            Class_Class Class = null;
            Student_StudentInClass studentInClass = null;
            StudentBL studentBL = new StudentBL(school);

            Class = studentBL.GetLastedClass(student);
            studentInClass = studentBL.GetStudentInClass(student, Class.Configuration_Year);
            ParentComment_Comment parentsComment = new ParentComment_Comment();
            parentsComment.Title = title;
            parentsComment.CommentContent = content;
            parentsComment.Date = DateTime.Now;
            parentsComment.CommentStatusId = 1; // unread
            parentsComment.StudentInClassId = studentInClass.StudentInClassId;
            parentsComment.Feedback = "";
            parentsCommentDA.InsertParentsComment(parentsComment);
        }

        public ParentsCommentBL(School_School school)
            : base(school)
        {
            parentsCommentDA = new ParentsCommentDA(school);
        }

        /// <summary>
        /// Feed back parent's comment with feedback content
        /// </summary>
        /// <param name="parentsComment"></param>
        /// <param name="feedback"></param>
        public void Feedback(ParentComment_Comment parentsComment, string feedback)
        {
            parentsCommentDA.UpdateParentsComments(parentsComment, feedback);
        }

        public List<TabularParentsComment> GetTabularParentsComments(Configuration_Year year, Configuration_CommentStatus commentStatus,
            DateTime beginDate, DateTime endDate, bool combineIsConfirmedStatus, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            // Declare variables
            List<TabularParentsComment> tabularParentsComments;
            TabularParentsComment tabularParentsComment = null;
            List<ParentComment_Comment> parentsComments = null;

            // get list GopY_YKien
            if (commentStatus != null)
            {
                if (combineIsConfirmedStatus && commentStatus.CommentStatusId != 1)
                {
                    parentsComments = parentsCommentDA.GetFedbackParentsComments(year, beginDate, endDate,
                        pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    parentsComments = parentsCommentDA.GetParentsComments(year, commentStatus, beginDate, endDate,
                        pageCurrentIndex, pageSize, out totalRecords);
                }
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
                if (combineIsConfirmedStatus && parentsComment.CommentStatusId != 1)
                {
                    tabularParentsComment.CommentStatusName = "Đã phản hồi";
                }
                else
                {
                    tabularParentsComment.CommentStatusName = parentsComment.Configuration_CommentStatus.CommentStatusName;
                }
                tabularParentsComment.Date = parentsComment.Date.ToShortDateString();
                tabularParentsComment.Feedback = parentsComment.Feedback;
                
                tabularParentsComments.Add(tabularParentsComment);
            }

            return tabularParentsComments;
        }

        public List<TabularParentsComment> GetTabularParentsComments(Student_Student student, Configuration_Year year, Configuration_CommentStatus commentStatus,
            DateTime beginDate, DateTime endDate, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            // Declare variables
            List<TabularParentsComment> tabularParentsComments;
            TabularParentsComment tabularParentsComment = null;
            List<ParentComment_Comment> parentsComments = null;

            // get list GopY_YKien
            if (commentStatus != null)
            {
                parentsComments = parentsCommentDA.GetParentsComments(student, year, commentStatus, beginDate, endDate,
                    pageCurrentIndex, pageSize, out totalRecords);
            }
            else
            {
                parentsComments = parentsCommentDA.GetParentsComments(student, year, beginDate, endDate,
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
                tabularParentsComment.Feedback = parentsComment.Feedback;
                tabularParentsComments.Add(tabularParentsComment);
            }

            return tabularParentsComments;
        }

        public List<Configuration_CommentStatus> GetCommentStatuses(bool combineIsConfirmed)
        {
            List<Configuration_CommentStatus> commentStatuses = parentsCommentDA.GetCommentStatuses();
            if (commentStatuses.Count != 0 && combineIsConfirmed)
            {
                commentStatuses = commentStatuses.Take(2).ToList();
                commentStatuses[1].CommentStatusName = "Đã phản hồi";
            }

            return commentStatuses;
        }

        public ParentComment_Comment GetParentsComments(int commentId)
        {
            return parentsCommentDA.GetParentsComments(commentId);
        }

        public void DeleteParentsComment(ParentComment_Comment comment)
        {
            parentsCommentDA.DeleteParentsComment(comment);
        }

        public void UpdateParentsComment(ParentComment_Comment comment, string newContent)
        {
            parentsCommentDA.UpdateParentsComment(comment, newContent);
        }

        internal void DeleteParentsComment(Student_Student deletedStudent)
        {
            parentsCommentDA.DeleteParentsComment(deletedStudent);
        }

        public void UpdateParentsComment(ParentComment_Comment comment, Configuration_CommentStatus commentStatus)
        {
            parentsCommentDA.UpdateParentsComment(comment, commentStatus);
        }

        public int GetUnConfirmedFedbackComment(Student_Student student)
        {
            StudentBL studentBL = new StudentBL(school);
            Class_Class Class = studentBL.GetLastedClass(student);
            return parentsCommentDA.GetUnConfirmedFedbackComment(student, Class);
        }
    }
}
