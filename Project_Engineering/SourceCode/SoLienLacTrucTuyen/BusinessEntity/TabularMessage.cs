using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.BusinessEntity
{
    public class TabularMessage
    {
        public int MessageId { get; set; }
        public string Title { get; set; }

        public string MessageContent { get; set; }

        public string Feedback { get; set; } 
        
        public DateTime Date { get; set; }

        public string StringDate { get; set; }

        public int StudentInClassId { get; set; }

        public int StudentId { get; set; }

        public string StudentCode { get; set; }

        public string StudentName { get; set; }

        public int MessageStatusId { get; set; }

        public string StringMessageStatus { get; set; }
    }
}
