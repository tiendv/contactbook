using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.BusinessEntity
{
    public class TabularParentsComment
    {
        public int CommentId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Feedback { get; set; }
        public string CommentStatusName { get; set; }
        public string Date { get; set; }
    }
}
