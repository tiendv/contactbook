using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
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
                Date = ngay
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

        public void UpdateMessage(MessageToParents_Message editedMessage, bool confirmed)
        {
            MessageToParents_Message message = null;
            IQueryable<MessageToParents_Message> iqMessage = from msg in db.MessageToParents_Messages
                                                             where msg.MessageId == editedMessage.MessageId
                                                             select msg;
            if (iqMessage.Count() != 0)
            {
                message = iqMessage.First();
                message.IsConfirmed = confirmed;
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

        public void UpdateLoiNhanKhan(int maLoiNhanKhan, bool xacNhan)
        {
            MessageToParents_Message loiNhanKhan = (from lnk in db.MessageToParents_Messages
                                                    where lnk.MessageId == maLoiNhanKhan
                                                    select lnk).First();
            loiNhanKhan.IsConfirmed = xacNhan;

            db.SubmitChanges();
        }

        public void DeleteLoiNhanKhan(int maLoiNhanKhan)
        {
            MessageToParents_Message loiNhanKhan = (from lnk in db.MessageToParents_Messages
                                                    where lnk.MessageId == maLoiNhanKhan
                                                    select lnk).First();
            db.MessageToParents_Messages.DeleteOnSubmit(loiNhanKhan);
            db.SubmitChanges();

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

        public List<TabularMessage> GetListTabularLoiNhanKhan(int YearId, DateTime tuNgay, DateTime denNgay,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularMessage> loiNhanKhans = (from lnk in db.MessageToParents_Messages
                                                       join hs_lh in db.Student_StudentInClasses on lnk.StudentInClassId equals hs_lh.StudentInClassId
                                                       join lop in db.Class_Classes on hs_lh.ClassId equals lop.ClassId
                                                       join hs in db.Student_Students on hs_lh.StudentId equals hs.StudentId
                                                       where lop.YearId == YearId && lnk.Date >= tuNgay && lnk.Date <= denNgay
                                                       select new TabularMessage
                                                       {
                                                           MaLoiNhanKhan = lnk.MessageId,
                                                           MaHocSinh = hs.StudentId,
                                                           Ngay = lnk.Date,
                                                           TieuDe = lnk.Title,
                                                           StrNgay = lnk.Date.ToShortDateString(),
                                                           MaHocSinhHienThi = hs.StudentCode,
                                                           TenHocSinh = hs.FullName,
                                                           XacNhan = (lnk.IsConfirmed) ? "Có" : "Không"
                                                       }).OrderBy(loiNhan => loiNhan.Ngay);
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

        public List<TabularMessage> GetListTabularLoiNhanKhan(int YearId, DateTime tuNgay, DateTime denNgay,
            bool xacNhan,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularMessage> loiNhanKhans = (from lnk in db.MessageToParents_Messages
                                                       join hs_lh in db.Student_StudentInClasses on lnk.StudentInClassId equals hs_lh.StudentInClassId
                                                       join lop in db.Class_Classes on hs_lh.ClassId equals lop.ClassId
                                                       join hs in db.Student_Students on hs_lh.StudentId equals hs.StudentId
                                                       where lop.YearId == YearId && lnk.Date >= tuNgay && lnk.Date <= denNgay
                                                          && lnk.IsConfirmed == xacNhan
                                                       select new TabularMessage
                                                       {
                                                           MaLoiNhanKhan = lnk.MessageId,
                                                           MaHocSinh = hs.StudentId,
                                                           Ngay = lnk.Date,
                                                           TieuDe = lnk.Title,
                                                           StrNgay = lnk.Date.ToShortDateString(),
                                                           MaHocSinhHienThi = hs.StudentCode,
                                                           TenHocSinh = hs.FullName,
                                                           XacNhan = (lnk.IsConfirmed) ? "Có" : "Không"
                                                       }).OrderBy(loiNhan => loiNhan.Ngay);
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

        public List<TabularMessage> GetListTabularLoiNhanKhan(int YearId, DateTime tuNgay, DateTime denNgay,
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
                                                           MaLoiNhanKhan = lnk.MessageId,
                                                           MaHocSinh = hs.StudentId,
                                                           Ngay = lnk.Date,
                                                           TieuDe = lnk.Title,
                                                           StrNgay = lnk.Date.ToShortDateString(),
                                                           MaHocSinhHienThi = hs.StudentCode,
                                                           TenHocSinh = hs.FullName,
                                                           XacNhan = (lnk.IsConfirmed) ? "Có" : "Không"
                                                       }).OrderBy(loiNhan => loiNhan.Ngay);
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

        public List<TabularMessage> GetListTabularLoiNhanKhan(int YearId, DateTime tuNgay, DateTime denNgay,
            string maHocSinhHienThi,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularMessage> loiNhanKhans = (from lnk in db.MessageToParents_Messages
                                                       join hs_lh in db.Student_StudentInClasses on lnk.StudentInClassId equals hs_lh.StudentInClassId
                                                       join lop in db.Class_Classes on hs_lh.ClassId equals lop.ClassId
                                                       join hs in db.Student_Students on hs_lh.StudentId equals hs.StudentId
                                                       where lop.YearId == YearId && lnk.Date >= tuNgay && lnk.Date <= denNgay
                                                          && hs.StudentCode == maHocSinhHienThi
                                                       select new TabularMessage
                                                       {
                                                           MaLoiNhanKhan = lnk.MessageId,
                                                           MaHocSinh = hs.StudentId,
                                                           Ngay = lnk.Date,
                                                           TieuDe = lnk.Title,
                                                           StrNgay = lnk.Date.ToShortDateString(),
                                                           MaHocSinhHienThi = hs.StudentCode,
                                                           TenHocSinh = hs.FullName,
                                                           XacNhan = (lnk.IsConfirmed) ? "Có" : "Không"
                                                       }).OrderBy(loiNhan => loiNhan.Ngay);
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
            Student_Student student, bool confirmed, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<MessageToParents_Message> messages = new List<MessageToParents_Message>();

            IQueryable<MessageToParents_Message> iqMessages = from msg in db.MessageToParents_Messages
                                                            where msg.Student_StudentInClass.StudentId == student.StudentId
                                                             && msg.Student_StudentInClass.Class_Class.YearId == year.YearId
                                                             && msg.Date >= beginDate && msg.Date <= endDate
                                                             && msg.IsConfirmed == confirmed
                                                            select msg;
            totalRecords = iqMessages.Count();
            if (totalRecords != 0)
            {
                messages = iqMessages.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
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
                messages = iqMessages.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return messages;
        }
    }
}
