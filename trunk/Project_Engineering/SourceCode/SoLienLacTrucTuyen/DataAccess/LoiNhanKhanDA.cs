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
                Title  = tieuDe,
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

        public List<TabularLoiNhanKhan> GetListTabularLoiNhanKhan(int YearId, DateTime tuNgay, DateTime denNgay,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularLoiNhanKhan> loiNhanKhans = (from lnk in db.MessageToParents_Messages
                                                          join hs_lh in db.Student_StudentInClasses on lnk.StudentInClassId equals hs_lh.StudentInClassId
                                                          join lop in db.Class_Classes on hs_lh.ClassId equals lop.ClassId
                                                          join hs in db.Student_Students on hs_lh.StudentId equals hs.StudentId
                                                          where lop.YearId == YearId && lnk.Date >= tuNgay && lnk.Date <= denNgay
                                                          select new TabularLoiNhanKhan
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
                List<TabularLoiNhanKhan> lstLoiNhanKhan = loiNhanKhans.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
                return lstLoiNhanKhan;
            }
            else
            {
                return new List<TabularLoiNhanKhan>();
            }
        }

        public List<TabularLoiNhanKhan> GetListTabularLoiNhanKhan(int YearId, DateTime tuNgay, DateTime denNgay,
            bool xacNhan,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularLoiNhanKhan> loiNhanKhans = (from lnk in db.MessageToParents_Messages
                                                          join hs_lh in db.Student_StudentInClasses on lnk.StudentInClassId equals hs_lh.StudentInClassId
                                                          join lop in db.Class_Classes on hs_lh.ClassId equals lop.ClassId
                                                          join hs in db.Student_Students on hs_lh.StudentId equals hs.StudentId
                                                          where lop.YearId == YearId && lnk.Date >= tuNgay && lnk.Date <= denNgay
                                                             && lnk.IsConfirmed == xacNhan
                                                          select new TabularLoiNhanKhan
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
                List<TabularLoiNhanKhan> lstLoiNhanKhan = loiNhanKhans.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
                return lstLoiNhanKhan;
            }
            else
            {
                return new List<TabularLoiNhanKhan>();
            }
        }        

        public List<TabularLoiNhanKhan> GetListTabularLoiNhanKhan(int YearId, DateTime tuNgay, DateTime denNgay,
            string maHocSinhHienThi, bool xacNhan,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularLoiNhanKhan> loiNhanKhans = (from lnk in db.MessageToParents_Messages
                                                          join hs_lh in db.Student_StudentInClasses on lnk.StudentInClassId equals hs_lh.StudentInClassId
                                                          join lop in db.Class_Classes on hs_lh.ClassId equals lop.ClassId
                                                          join hs in db.Student_Students on hs_lh.StudentId equals hs.StudentId
                                                          where lop.YearId == YearId && lnk.Date >= tuNgay && lnk.Date <= denNgay
                                                             && hs.StudentCode == maHocSinhHienThi && lnk.IsConfirmed == xacNhan
                                                          select new TabularLoiNhanKhan
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
                List<TabularLoiNhanKhan> lstLoiNhanKhan = loiNhanKhans.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
                return lstLoiNhanKhan;
            }
            else
            {
                return new List<TabularLoiNhanKhan>();
            }
        }

        public List<TabularLoiNhanKhan> GetListTabularLoiNhanKhan(int YearId, DateTime tuNgay, DateTime denNgay,
            string maHocSinhHienThi,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<TabularLoiNhanKhan> loiNhanKhans = (from lnk in db.MessageToParents_Messages
                                                          join hs_lh in db.Student_StudentInClasses on lnk.StudentInClassId equals hs_lh.StudentInClassId
                                                          join lop in db.Class_Classes on hs_lh.ClassId equals lop.ClassId
                                                          join hs in db.Student_Students on hs_lh.StudentId equals hs.StudentId
                                                          where lop.YearId == YearId && lnk.Date >= tuNgay && lnk.Date <= denNgay
                                                             && hs.StudentCode == maHocSinhHienThi
                                                          select new TabularLoiNhanKhan
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
                List<TabularLoiNhanKhan> lstLoiNhanKhan = loiNhanKhans.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
                return lstLoiNhanKhan;
            }
            else
            {
                return new List<TabularLoiNhanKhan>();
            }
        }        
    }
}
