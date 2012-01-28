using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.BusinessEntity;

namespace EContactBook.DataAccess
{
    public class LoiNhanKhanDA : BaseDA
    {
        public LoiNhanKhanDA(School_School school)
            : base(school)
        {
        }

        public void InsertLoiNhanKhan(int maHocSinhLopHoc, string tieuDe, string noiDung,
            DateTime ngay)
        {
            MessageToParents_Message loiNhanKhan = new MessageToParents_Message
            {
                StudentInClassId = maHocSinhLopHoc,
                Title = tieuDe,
                MessageContent = noiDung,
                Date = ngay,
                MessageStatusId = 1
            };

            db.MessageToParents_Messages.InsertOnSubmit(loiNhanKhan);
            db.SubmitChanges();
        }

        public void UpdateLoiNhanKhan(int maLoiNhanKhan, string tieuDe, string noiDung,
            DateTime ngay)
        {
            MessageToParents_Message loiNhanKhan = (from lnk in db.MessageToParents_Messages
                                                    where lnk.MessageId == maLoiNhanKhan
                                                    select lnk).First();
            loiNhanKhan.Title = tieuDe;
            loiNhanKhan.MessageContent = noiDung;
            loiNhanKhan.Date = ngay;

            db.SubmitChanges();
        }

        public void UpdateMessage(MessageToParents_Message editedMessage, Configuration_MessageStatus messageStatus)
        {
            MessageToParents_Message message = null;
            IQueryable<MessageToParents_Message> iqMessage = from msg in db.MessageToParents_Messages
                                                             where msg.MessageId == editedMessage.MessageId
                                                             select msg;
            if (iqMessage.Count() != 0)
            {
                message = iqMessage.First();
                message.MessageStatusId = messageStatus.MessageStatusId;
                message.Feedback = editedMessage.Feedback;
                
                db.SubmitChanges();
            }
        }

        public void UpdateLoiNhanKhan(int maLoiNhanKhan, string noiDung,
            DateTime ngay)
        {
            MessageToParents_Message loiNhanKhan = (from lnk in db.MessageToParents_Messages
                                                    where lnk.MessageId == maLoiNhanKhan
                                                    select lnk).First();
            loiNhanKhan.MessageContent = noiDung;
            loiNhanKhan.Date = ngay;

            db.SubmitChanges();
        }

        public void DeleteMessage(MessageToParents_Message message)
        {
            IQueryable<MessageToParents_Message> iqMessage = from msg in db.MessageToParents_Messages
                                                             where msg.MessageId == message.MessageId
                                                             select msg;
            if (iqMessage.Count() != 0)
            {
                message = iqMessage.First();
                db.MessageToParents_Messages.DeleteOnSubmit(message);
                db.SubmitChanges();
            }
        }

        public MessageToParents_Message GetLoiNhanKhan(int maLoiNhanKhan)
        {
            IQueryable<MessageToParents_Message> loiNhanKhans = from lnk in db.MessageToParents_Messages
                                                                where lnk.MessageId == maLoiNhanKhan
                                                                select lnk;
            if (loiNhanKhans.Count() != 0)
            {
                return loiNhanKhans.First();
            }
            else
            {
                return null;
            }
        }

        public List<TabularMessage> GetTabularMessages(int YearId, DateTime tuNgay, DateTime denNgay,
            string maHocSinhHienThi, bool xacNhan,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularMessage> loiNhanKhans = (from lnk in db.MessageToParents_Messages
                                                       join hs_lh in db.Student_StudentInClasses on lnk.StudentInClassId equals hs_lh.StudentInClassId
                                                       join lop in db.Class_Classes on hs_lh.ClassId equals lop.ClassId
                                                       join hs in db.Student_Students on hs_lh.StudentId equals hs.StudentId
                                                       where lop.YearId == YearId && lnk.Date >= tuNgay && lnk.Date <= denNgay
                                                          && hs.StudentCode == maHocSinhHienThi && lnk.IsConfirmed == xacNhan
                                                       select new TabularMessage
                                                       {
                                                           MessageId = lnk.MessageId,
                                                           StudentId = hs.StudentId,
                                                           Date = lnk.Date,
                                                           Title = lnk.Title,
                                                           StringDate = lnk.Date.ToShortDateString(),
                                                           StudentCode = hs.StudentCode,
                                                           StudentName = hs.FullName,
                                                           StringMessageStatus = (lnk.IsConfirmed) ? "Có" : "Không"
                                                       }).OrderBy(loiNhan => loiNhan.Date);
            totalRecords = loiNhanKhans.Count();
            if (totalRecords != 0)
            {
                List<TabularMessage> lstLoiNhanKhan = loiNhanKhans.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
                return lstLoiNhanKhan;
            }
            else
            {
                return new List<TabularMessage>();
            }
        }

        public List<MessageToParents_Message> GetMessages(Configuration_Year year, DateTime beginDate, DateTime endDate,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<MessageToParents_Message> iqMessage = from message in db.MessageToParents_Messages
                                                             join studentInClass in db.Student_StudentInClasses on message.StudentInClassId equals studentInClass.StudentInClassId
                                                             join Class in db.Class_Classes on studentInClass.ClassId equals Class.ClassId
                                                             join student in db.Student_Students on studentInClass.StudentId equals student.StudentId
                                                             where Class.YearId == year.YearId && message.Date >= beginDate && message.Date <= endDate
                                                                && Class.SchoolId == school.SchoolId
                                                             select message;
            return GetMessages(iqMessage, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<MessageToParents_Message> GetMessages(Class_Class Class, DateTime beginDate, DateTime endDate,
           int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<MessageToParents_Message> iqMessage = from message in db.MessageToParents_Messages
                                                             join studentInClass in db.Student_StudentInClasses on message.StudentInClassId equals studentInClass.StudentInClassId
                                                             join c in db.Class_Classes on studentInClass.ClassId equals c.ClassId
                                                             join student in db.Student_Students on studentInClass.StudentId equals student.StudentId
                                                             where c.ClassId == Class.ClassId && message.Date >= beginDate && message.Date <= endDate
                                                             select message;
            return GetMessages(iqMessage, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<MessageToParents_Message> GetMessages(Configuration_Year year, DateTime beginDate, DateTime endDate, Configuration_MessageStatus messageStatus,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<MessageToParents_Message> iqMessage = from message in db.MessageToParents_Messages
                                                             join studentInClass in db.Student_StudentInClasses on message.StudentInClassId equals studentInClass.StudentInClassId
                                                             join Class in db.Class_Classes on studentInClass.ClassId equals Class.ClassId
                                                             join student in db.Student_Students on studentInClass.StudentId equals student.StudentId
                                                             where Class.YearId == year.YearId && message.Date >= beginDate && message.Date <= endDate
                                                                && message.MessageStatusId == messageStatus.MessageStatusId && Class.SchoolId == school.SchoolId
                                                             select message;

            return GetMessages(iqMessage, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<MessageToParents_Message> GetMessages(Class_Class Class, DateTime beginDate, DateTime endDate, Configuration_MessageStatus messageStatus,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<MessageToParents_Message> iqMessage = from message in db.MessageToParents_Messages
                                                             join studentInClass in db.Student_StudentInClasses on message.StudentInClassId equals studentInClass.StudentInClassId
                                                             join c in db.Class_Classes on studentInClass.ClassId equals c.ClassId
                                                             join student in db.Student_Students on studentInClass.StudentId equals student.StudentId
                                                             where c.ClassId == Class.ClassId && message.Date >= beginDate && message.Date <= endDate
                                                                && message.MessageStatusId == messageStatus.MessageStatusId
                                                             select message;

            return GetMessages(iqMessage, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<MessageToParents_Message> GetConfirmedMessages(Configuration_Year year, DateTime beginDate, DateTime endDate, 
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<MessageToParents_Message> iqMessage = from message in db.MessageToParents_Messages
                                                             join studentInClass in db.Student_StudentInClasses on message.StudentInClassId equals studentInClass.StudentInClassId
                                                             join Class in db.Class_Classes on studentInClass.ClassId equals Class.ClassId
                                                             join student in db.Student_Students on studentInClass.StudentId equals student.StudentId
                                                             where Class.YearId == year.YearId && message.Date >= beginDate && message.Date <= endDate
                                                                && message.MessageStatusId == 3 && Class.SchoolId == school.SchoolId
                                                             select message;

            return GetMessages(iqMessage, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<MessageToParents_Message> GetConfirmedMessages(Class_Class Class, DateTime beginDate, DateTime endDate,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<MessageToParents_Message> iqMessage = from message in db.MessageToParents_Messages
                                                             join studentInClass in db.Student_StudentInClasses on message.StudentInClassId equals studentInClass.StudentInClassId
                                                             join c in db.Class_Classes on studentInClass.ClassId equals c.ClassId
                                                             join student in db.Student_Students on studentInClass.StudentId equals student.StudentId
                                                             where c.ClassId == Class.ClassId && message.Date >= beginDate && message.Date <= endDate
                                                                && message.MessageStatusId == 3
                                                             select message;

            return GetMessages(iqMessage, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<MessageToParents_Message> GetMessages(Configuration_Year year, DateTime beginDate, DateTime endDate, string studentCode,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<MessageToParents_Message> iqMessage = from message in db.MessageToParents_Messages
                                                             join studentInClass in db.Student_StudentInClasses on message.StudentInClassId equals studentInClass.StudentInClassId
                                                             join Class in db.Class_Classes on studentInClass.ClassId equals Class.ClassId
                                                             join student in db.Student_Students on studentInClass.StudentId equals student.StudentId
                                                             where Class.YearId == year.YearId && message.Date >= beginDate && message.Date <= endDate
                                                                && student.StudentCode == studentCode && Class.SchoolId == school.SchoolId
                                                             select message;
            return GetMessages(iqMessage, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<MessageToParents_Message> GetMessages(Class_Class Class, DateTime beginDate, DateTime endDate, string studentCode,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<MessageToParents_Message> iqMessage = from message in db.MessageToParents_Messages
                                                             join studentInClass in db.Student_StudentInClasses on message.StudentInClassId equals studentInClass.StudentInClassId
                                                             join c in db.Class_Classes on studentInClass.ClassId equals c.ClassId
                                                             join student in db.Student_Students on studentInClass.StudentId equals student.StudentId
                                                             where c.ClassId == Class.ClassId && message.Date >= beginDate && message.Date <= endDate
                                                                && student.StudentCode == studentCode
                                                             select message;
            return GetMessages(iqMessage, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<MessageToParents_Message> GetMessages(Configuration_Year year, DateTime beginDate, DateTime endDate, string studentCode, Configuration_MessageStatus messageStatus,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<MessageToParents_Message> iqMessage = from message in db.MessageToParents_Messages
                                                             join studentInClass in db.Student_StudentInClasses on message.StudentInClassId equals studentInClass.StudentInClassId
                                                             join Class in db.Class_Classes on studentInClass.ClassId equals Class.ClassId
                                                             join student in db.Student_Students on studentInClass.StudentId equals student.StudentId
                                                             where Class.YearId == year.YearId && message.Date >= beginDate && message.Date <= endDate
                                                                && student.StudentCode == studentCode && messageStatus.MessageStatusId == messageStatus.MessageStatusId
                                                                && Class.SchoolId == school.SchoolId
                                                             select message;
            return GetMessages(iqMessage, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<MessageToParents_Message> GetMessages(Class_Class Class, DateTime beginDate, DateTime endDate, string studentCode, Configuration_MessageStatus messageStatus,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<MessageToParents_Message> iqMessage = from message in db.MessageToParents_Messages
                                                             join studentInClass in db.Student_StudentInClasses on message.StudentInClassId equals studentInClass.StudentInClassId
                                                             join c in db.Class_Classes on studentInClass.ClassId equals c.ClassId
                                                             join student in db.Student_Students on studentInClass.StudentId equals student.StudentId
                                                             where c.ClassId == Class.ClassId && message.Date >= beginDate && message.Date <= endDate
                                                                && student.StudentCode == studentCode && messageStatus.MessageStatusId == messageStatus.MessageStatusId
                                                             select message;
            return GetMessages(iqMessage, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<MessageToParents_Message> GetConfirmedMessages(Configuration_Year year, DateTime beginDate, DateTime endDate, string studentCode, Configuration_MessageStatus messageStatus,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<MessageToParents_Message> iqMessage = from message in db.MessageToParents_Messages
                                                             join studentInClass in db.Student_StudentInClasses on message.StudentInClassId equals studentInClass.StudentInClassId
                                                             join Class in db.Class_Classes on studentInClass.ClassId equals Class.ClassId
                                                             join student in db.Student_Students on studentInClass.StudentId equals student.StudentId
                                                             where Class.YearId == year.YearId && message.Date >= beginDate && message.Date <= endDate
                                                                && student.StudentCode == studentCode && messageStatus.MessageStatusId == 3
                                                                && Class.SchoolId == school.SchoolId
                                                             select message;
            return GetMessages(iqMessage, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<MessageToParents_Message> GetConfirmedMessages(Class_Class Class, DateTime beginDate, DateTime endDate, string studentCode, Configuration_MessageStatus messageStatus,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<MessageToParents_Message> iqMessage = from message in db.MessageToParents_Messages
                                                             join studentInClass in db.Student_StudentInClasses on message.StudentInClassId equals studentInClass.StudentInClassId
                                                             join c in db.Class_Classes on studentInClass.ClassId equals c.ClassId
                                                             join student in db.Student_Students on studentInClass.StudentId equals student.StudentId
                                                             where c.ClassId == Class.ClassId && message.Date >= beginDate && message.Date <= endDate
                                                                && student.StudentCode == studentCode && messageStatus.MessageStatusId == 3
                                                             select message;
            return GetMessages(iqMessage, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<MessageToParents_Message> GetMessages(Configuration_Year year, DateTime beginDate, DateTime endDate,
            Student_Student student, Configuration_MessageStatus messageStatus, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<MessageToParents_Message> messages = new List<MessageToParents_Message>();

            IQueryable<MessageToParents_Message> iqMessages = from msg in db.MessageToParents_Messages
                                                              where msg.Student_StudentInClass.StudentId == student.StudentId
                                                               && msg.Student_StudentInClass.Class_Class.YearId == year.YearId
                                                               && msg.Date >= beginDate && msg.Date <= endDate
                                                               && msg.MessageStatusId == messageStatus.MessageStatusId
                                                              select msg;
            totalRecords = iqMessages.Count();
            if (totalRecords != 0)
            {
                messages = iqMessages.OrderByDescending(msg => msg.Date).Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return messages;
        }

        public List<MessageToParents_Message> GetMessages(Configuration_Year year, DateTime beginDate, DateTime endDate,
            Student_Student student, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<MessageToParents_Message> messages = new List<MessageToParents_Message>();

            IQueryable<MessageToParents_Message> iqMessages = from msg in db.MessageToParents_Messages
                                                              where msg.Student_StudentInClass.StudentId == student.StudentId
                                                               && msg.Student_StudentInClass.Class_Class.YearId == year.YearId
                                                               && msg.Date >= beginDate && msg.Date <= endDate
                                                              select msg;
            totalRecords = iqMessages.Count();
            if (totalRecords != 0)
            {
                messages = iqMessages.OrderByDescending(msg => msg.Date).Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return messages;
        }

        public void DeleteLoiNhanKhan(Student_Student deletedStudent)
        {
            IQueryable<MessageToParents_Message> iqMessage;
            iqMessage = from message in db.MessageToParents_Messages
                        where message.Student_StudentInClass.StudentId == deletedStudent.StudentId
                        select message;
            if (iqMessage.Count() != 0)
            {
                foreach (MessageToParents_Message message in iqMessage)
                {
                    db.MessageToParents_Messages.DeleteOnSubmit(message);
                }

                db.SubmitChanges();
            }
        }

        private List<MessageToParents_Message> GetMessages(IQueryable<MessageToParents_Message> iqMessage, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<MessageToParents_Message> messages = new List<MessageToParents_Message>();

            totalRecords = iqMessage.Count();
            if (totalRecords != 0)
            {
                messages = iqMessage.OrderByDescending(msg => msg.Date)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return messages;
        }

        public int GetNewMessageCount(Student_Student student, Class_Class Class)
        {
            List<MessageToParents_Message> messages = new List<MessageToParents_Message>();

            IQueryable<MessageToParents_Message> iqMessages = from msg in db.MessageToParents_Messages
                                                              where msg.Student_StudentInClass.StudentId == student.StudentId
                                                               && msg.Student_StudentInClass.Class_Class.YearId == Class.YearId
                                                               && msg.MessageStatusId == 1
                                                              select msg;
            return iqMessages.Count();
        }

        public int GetUnconfirmedMessageCount(Student_Student student, Class_Class Class)
        {
            IQueryable<MessageToParents_Message> iqMessages = from msg in db.MessageToParents_Messages
                                                              where msg.Student_StudentInClass.StudentId == student.StudentId
                                                               && msg.Student_StudentInClass.Class_Class.YearId == Class.YearId
                                                               && msg.MessageStatusId == 2
                                                              select msg;
            return iqMessages.Count();
        }

        public void MarkMessageAsRead(MessageToParents_Message message)
        {
            IQueryable<MessageToParents_Message> iqMessages = from msg in db.MessageToParents_Messages
                                                              where msg.MessageId == message.MessageId
                                                              select msg;
            if (iqMessages.Count() != 0)
            {
                message = iqMessages.First();
                message.IsRead = true;
                message.MessageStatusId = 2;
                db.SubmitChanges();
            }
        }

        public bool IsDeletable(MessageToParents_Message message)
        {
            IQueryable<MessageToParents_Message> iqMessages = from msg in db.MessageToParents_Messages
                                                              where msg.MessageId == message.MessageId
                                                              && msg.MessageStatusId == 3
                                                              select msg;
            if (iqMessages.Count() != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void UpdateMessage(MessageToParents_Message message, string title, string content)
        {
            IQueryable<MessageToParents_Message> queryMessage = from msg in db.MessageToParents_Messages
                                                                where msg.MessageId == message.MessageId
                                                                select msg;
            if (queryMessage.Count() != 0)
            {
                message = queryMessage.First();
                message.Title = title;
                message.MessageContent = content;
                db.SubmitChanges();
            }
        }
    }
}
