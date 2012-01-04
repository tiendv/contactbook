using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class MessageBL : BaseBL
    {
        private LoiNhanKhanDA messageDA;

        public MessageBL(School_School school)
            : base(school)
        {
            messageDA = new LoiNhanKhanDA(school);
        }

        public void InsertMessage(int maHocSinhLopHoc, string tieuDe, string noiDung, DateTime ngay)
        {
            messageDA.InsertLoiNhanKhan(maHocSinhLopHoc, tieuDe, noiDung, ngay);
        }

        public void UpdateMessage(int maLoiNhanKhan, string tieuDe, string noiDung, DateTime ngay)
        {
            messageDA.UpdateLoiNhanKhan(maLoiNhanKhan, tieuDe, noiDung, ngay);
        }

        public void UpdateMessage(int maLoiNhanKhan, string noiDung, DateTime ngay)
        {
            messageDA.UpdateLoiNhanKhan(maLoiNhanKhan, noiDung, ngay);
        }

        public void ConfirmMessage(MessageToParents_Message message)
        {
            ConfigurationMessageStatus messageStatus = new ConfigurationMessageStatus();
            messageStatus.MessageStatusId = 3; // confirmed
            messageDA.UpdateMessage(message, messageStatus);
        }

        public void DeleteMessage(int maLopNhanKhan)
        {
            messageDA.DeleteLoiNhanKhan(maLopNhanKhan);
        }

        public MessageToParents_Message GetMessage(int maLoiNhanKhan)
        {
            return messageDA.GetLoiNhanKhan(maLoiNhanKhan);
        }

        public List<TabularMessage> GetTabularMessages(Configuration_Year year, DateTime beginDate, DateTime endDate,
            string studentCode, ConfigurationMessageStatus messageStatus, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularMessage> tabularMessages = new List<TabularMessage>();
            TabularMessage tabularMessage = null;
            List<MessageToParents_Message> messages;
            if (CheckUntils.IsAllOrBlank(studentCode))
            {
                if (messageStatus == null)
                {
                    messages = messageDA.GetMessages(year, beginDate, endDate, pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    messages = messageDA.GetMessages(year, beginDate, endDate, messageStatus, pageCurrentIndex, pageSize, out totalRecords);
                }
            }
            else
            {
                if (messageStatus == null)
                {
                    messages = messageDA.GetMessages(year, beginDate, endDate, studentCode, pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    messages = messageDA.GetMessages(year, beginDate, endDate, studentCode, messageStatus, pageCurrentIndex, pageSize, out totalRecords);
                }
            }

            foreach (MessageToParents_Message message in messages)
            {
                tabularMessage = new TabularMessage();
                tabularMessage.StudentId = message.Student_StudentInClass.StudentId;
                tabularMessage.StudentCode = message.Student_StudentInClass.Student_Student.StudentCode;
                tabularMessage.MessageId = message.MessageId;
                tabularMessage.Date = message.Date;
                tabularMessage.StringDate = message.Date.ToShortDateString();
                tabularMessage.StudentName = message.Student_StudentInClass.Student_Student.FullName;
                tabularMessage.Title = message.Title;
                tabularMessage.StringMessageStatus = (message.IsConfirmed == true) ? "Có": "Không";
                tabularMessage.MessageStatusId = (int)message.MessageStatusId;

                tabularMessages.Add(tabularMessage);
            }

            return tabularMessages;
        }

        public List<TabularMessage> GetTabularMessages(Configuration_Year year, DateTime beginDate, DateTime endDate,
            Student_Student student, ConfigurationMessageStatus messageStatus, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularMessage> tabularMessages = new List<TabularMessage>();
            TabularMessage tabularMessage = null;
            List<MessageToParents_Message> messages = new List<MessageToParents_Message>();

            if (messageStatus == null) // get messages regardless their confirmation
            {
               messages = messageDA.GetMessages(year, beginDate, endDate, student, 
                    pageCurrentIndex, pageSize, out totalRecords);
            }
            else
            {
                messages = messageDA.GetMessages(year, beginDate, endDate, student, messageStatus, 
                    pageCurrentIndex, pageSize, out totalRecords);
            }

            string strTime;
            string strDate;
            DateTime dtMsgDate;
            DateTime dtToday;
            string strHour;
            string strMinute;
            foreach (MessageToParents_Message message in messages)
            {
                tabularMessage = new TabularMessage();
                tabularMessage.StudentId = message.Student_StudentInClass.StudentId;
                tabularMessage.StudentCode = message.Student_StudentInClass.Student_Student.StudentCode;
                tabularMessage.MessageId = message.MessageId;
                tabularMessage.Date = message.Date;
                dtMsgDate = message.Date;
                dtToday = DateTime.Now;
                if (dtMsgDate.Day == dtToday.Day && dtMsgDate.Month == dtToday.Month && dtMsgDate.Year == dtToday.Year)
                {
                    strDate = "Hôm nay";
                }
                else
                {
                    strDate = message.Date.ToShortDateString();
                }
                if (dtMsgDate.Hour < 10)
                {
                    strHour = string.Format("0{0}", dtMsgDate.Hour);
                }
                else
                {
                    strHour = string.Format("{0}", dtMsgDate.Hour);
                }
                if (dtMsgDate.Minute < 10)
                {
                    strMinute = string.Format("0{0}", dtMsgDate.Minute);
                }
                else
                {
                    strMinute = string.Format("{0}", dtMsgDate.Minute);
                }
                strTime = string.Format("{0}:{1}", strHour, strMinute);
                tabularMessage.StringDate = string.Format("vào lúc {0}, {1}", strTime, strDate);

                tabularMessage.StudentName = message.Student_StudentInClass.Student_Student.FullName;
                tabularMessage.Title = message.Title;
                tabularMessage.MessageStatusId =(int) message.MessageStatusId;
                tabularMessage.StringMessageStatus = (message.IsConfirmed == true) ? "Có" : "Không";

                tabularMessages.Add(tabularMessage);
            }

            return tabularMessages;
        }

        public int GetNewMessageCount(Student_Student student)
        {
            StudentBL studentBL = new StudentBL(school);
            Class_Class Class = studentBL.GetLastedClass(student);
            return messageDA.GetNewMessageCount(student, Class);
        }

        public int GetUnconfirmedMessageCount(Student_Student student)
        {
            StudentBL studentBL = new StudentBL(school);
            Class_Class Class = studentBL.GetLastedClass(student);
            return messageDA.GetUnconfirmedMessageCount(student, Class);
        }

        internal void DeleteMessage(Student_Student student)
        {
            messageDA.DeleteLoiNhanKhan(student);
        }

        public void MarkMessageAsRead(MessageToParents_Message message)
        {
            messageDA.MarkMessageAsRead(message);
        }
    }
}
