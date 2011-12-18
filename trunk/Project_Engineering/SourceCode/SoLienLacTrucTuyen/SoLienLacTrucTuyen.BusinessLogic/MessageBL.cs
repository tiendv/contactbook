using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

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
            messageDA.UpdateMessage(message, true);
        }

        public void UnconfirmMessage(MessageToParents_Message message)
        {
            messageDA.UpdateMessage(message, false);
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
            string studentCode, bool? confirmed, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularMessage> tabularMessages = new List<TabularMessage>();
            TabularMessage tabularMessage = null;
            List<MessageToParents_Message> messages;
            if (CheckUntils.IsAllOrBlank(studentCode))
            {
                if (confirmed == null)
                {
                    messages = messageDA.GetMessages(year, beginDate, endDate, pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    messages = messageDA.GetMessages(year, beginDate, endDate, (bool)confirmed, pageCurrentIndex, pageSize, out totalRecords);
                }
            }
            else
            {
                if (confirmed == null)
                {
                    messages = messageDA.GetMessages(year, beginDate, endDate, studentCode, pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    messages = messageDA.GetMessages(year, beginDate, endDate, studentCode, (bool)confirmed, pageCurrentIndex, pageSize, out totalRecords);
                }
            }

            foreach (MessageToParents_Message message in messages)
            {
                tabularMessage = new TabularMessage();
                tabularMessage.MaHocSinh = message.Student_StudentInClass.StudentId;
                tabularMessage.MaHocSinhHienThi = message.Student_StudentInClass.Student_Student.StudentCode;
                tabularMessage.MaLoiNhanKhan = message.MessageId;
                tabularMessage.Ngay = message.Date;
                tabularMessage.StrNgay = message.Date.ToShortDateString();
                tabularMessage.TenHocSinh = message.Student_StudentInClass.Student_Student.FullName;
                tabularMessage.TieuDe = message.Title;
                tabularMessage.XacNhan = (message.IsConfirmed == true) ? "Có": "Không";

                tabularMessages.Add(tabularMessage);
            }

            return tabularMessages;
        }

        public List<MessageToParents_Message> GetMessages(Configuration_Year year, DateTime beginDate, DateTime endDate,
            Student_Student student, bool? confirmed, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            if (confirmed == null) // get messages regardless their confirmation
            {
                return messageDA.GetMessages(year, beginDate, endDate, student, 
                    pageCurrentIndex, pageSize, out totalRecords);
            }
            else
            {
                return messageDA.GetMessages(year, beginDate, endDate, student, (bool)confirmed, 
                    pageCurrentIndex, pageSize, out totalRecords);
            }
        }

        public int GetNewMessage(Student_Student student)
        {
            StudentBL studentBL = new StudentBL(school);
            Class_Class Class = studentBL.GetLastedClass(student);
            return messageDA.GetNewMessage(student, Class);
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
