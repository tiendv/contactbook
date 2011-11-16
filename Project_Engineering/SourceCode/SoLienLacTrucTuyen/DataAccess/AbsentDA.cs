using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class AbsentDA : BaseDA
    {
        public AbsentDA(School school)
            : base(school)
        {
        }

        public void InsertAbsent(HocSinh_HocSinhLopHoc studentInClass, CauHinh_HocKy term, DateTime date, CauHinh_Buoi session, bool permission, string reason)
        {
            HocSinh_NgayNghiHoc Absent = new HocSinh_NgayNghiHoc();
            Absent.MaHocSinhLopHoc = studentInClass.MaHocSinhLopHoc;
            Absent.MaHocKy = term.MaHocKy;
            Absent.Ngay = date;
            Absent.MaBuoi = session.MaBuoi;
            Absent.XinPhep = permission;
            Absent.XacNhan = false;
            Absent.LyDo = reason;

            db.HocSinh_NgayNghiHocs.InsertOnSubmit(Absent);
            db.SubmitChanges();
        }

        public void UpdateAbsent(HocSinh_NgayNghiHoc editedAbsent)
        {
            HocSinh_NgayNghiHoc absent = null;
            IQueryable<HocSinh_NgayNghiHoc> iqAbsent = from abs in db.HocSinh_NgayNghiHocs
                                                       where abs.MaNgayNghiHoc == editedAbsent.MaNgayNghiHoc
                                                       select abs;
            if (iqAbsent.Count() != 0)
            {
                absent = iqAbsent.First();
                absent.Ngay = editedAbsent.Ngay;
                absent.MaBuoi = editedAbsent.MaBuoi;
                absent.XinPhep = editedAbsent.XinPhep;
                absent.MaHocKy = editedAbsent.MaHocKy;
                absent.LyDo = editedAbsent.LyDo;
                db.SubmitChanges();
            }
        }

        public void ConfirmAbsent(HocSinh_NgayNghiHoc editedAbsent, bool confirm)
        {
            HocSinh_NgayNghiHoc absent = null;
            IQueryable<HocSinh_NgayNghiHoc> iqAbsent = from abs in db.HocSinh_NgayNghiHocs
                                                       where abs.MaNgayNghiHoc == editedAbsent.MaNgayNghiHoc
                                                       select abs;
            if (iqAbsent.Count() != 0)
            {
                absent = iqAbsent.First();
                absent.XacNhan = confirm;
                db.SubmitChanges();
            }
        }

        public void DeleteAbsent(int absentId)
        {
            HocSinh_NgayNghiHoc absent = null;
            IQueryable<HocSinh_NgayNghiHoc> iqAbsent = from abs in db.HocSinh_NgayNghiHocs
                                                       where abs.MaNgayNghiHoc == absentId
                                                       select abs;
            if (iqAbsent.Count() != 0)
            {
                absent = iqAbsent.First();
                db.HocSinh_NgayNghiHocs.DeleteOnSubmit(absent);
                db.SubmitChanges();
            }
        }

        public HocSinh_NgayNghiHoc GetAbsent(int absentId)
        {
            HocSinh_NgayNghiHoc absent = null;
            IQueryable<HocSinh_NgayNghiHoc> iqAbsent = from abs in db.HocSinh_NgayNghiHocs
                                                       where abs.MaNgayNghiHoc == absentId
                                                       select abs;
            if (iqAbsent.Count() != 0)
            {
                absent = iqAbsent.First();
            }

            return absent;
        }

        public HocSinh_NgayNghiHoc GetAbsent(HocSinh_HocSinhLopHoc studentInClass, CauHinh_HocKy term, DateTime date)
        {
            HocSinh_NgayNghiHoc absent = null;
            IQueryable<HocSinh_NgayNghiHoc> iqAbsent = from abs in db.HocSinh_NgayNghiHocs
                                                       where abs.MaHocSinhLopHoc == studentInClass.MaHocSinhLopHoc
                                                       && abs.MaHocKy == term.MaHocKy
                                                       && abs.Ngay == date
                                                       select abs;
            if (iqAbsent.Count() != 0)
            {
                absent = iqAbsent.First();
            }

            return absent;
        }

        public HocSinh_NgayNghiHoc GetAbsent(HocSinh_NgayNghiHoc exceptedAbsent, HocSinh_HocSinhLopHoc studentInClass, CauHinh_HocKy term, DateTime date)
        {
            HocSinh_NgayNghiHoc absent = null;
            IQueryable<HocSinh_NgayNghiHoc> iqAbsent = from abs in db.HocSinh_NgayNghiHocs
                                                       where abs.MaNgayNghiHoc != exceptedAbsent.MaNgayNghiHoc
                                                       && abs.MaHocSinhLopHoc == studentInClass.MaHocSinhLopHoc
                                                       && abs.MaHocKy == term.MaHocKy
                                                       && abs.Ngay == date
                                                       select abs;
            if (iqAbsent.Count() != 0)
            {
                absent = iqAbsent.First();
            }

            return absent;
        }

        public List<HocSinh_NgayNghiHoc> GetAbsents(HocSinh_HocSinhLopHoc studentInClass, CauHinh_HocKy term, DateTime beginDate, DateTime endDate, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<HocSinh_NgayNghiHoc> absents = new List<HocSinh_NgayNghiHoc>();

            IQueryable<HocSinh_NgayNghiHoc> iqAbsent = from abs in db.HocSinh_NgayNghiHocs
                                                       where abs.MaHocSinhLopHoc == studentInClass.MaHocSinhLopHoc
                                                       && abs.MaHocKy == term.MaHocKy
                                                       && abs.Ngay >= beginDate
                                                       && abs.Ngay <= endDate
                                                       select abs;
            totalRecords = iqAbsent.Count();
            if (totalRecords != 0)
            {
                absents = iqAbsent.Skip((pageCurrentIndex - 1) * pageSize)
                    .Take(pageSize).OrderBy(n => n.Ngay).ToList();
            }

            return absents;
        }

        public bool IsConfirmed(HocSinh_NgayNghiHoc absent)
        {
            bool confirmed = (from abs in db.HocSinh_NgayNghiHocs
                              where abs.MaNgayNghiHoc == abs.MaNgayNghiHoc
                              select abs.XacNhan).First();
            return confirmed;
        }

        public bool AbsentExists(HocSinh_ThongTinCaNhan student, LopHoc_Lop Class, CauHinh_HocKy term, DateTime date)
        {
            IQueryable<HocSinh_NgayNghiHoc> iqAbsent = from abs in db.HocSinh_NgayNghiHocs
                                                       where abs.HocSinh_HocSinhLopHoc.MaHocSinh == student.MaHocSinh
                                                       && abs.HocSinh_HocSinhLopHoc.MaLopHoc == Class.MaLopHoc
                                                       && abs.MaHocKy == term.MaHocKy
                                                       && abs.Ngay == date
                                                       select abs;
            if (iqAbsent.Count() != 0)
            {
                return true;
            }

            return false;
        }

        public bool AbsentExists(HocSinh_ThongTinCaNhan student, LopHoc_Lop Class, CauHinh_HocKy term, DateTime date, CauHinh_Buoi session)
        {
            IQueryable<HocSinh_NgayNghiHoc> iqAbsent = from abs in db.HocSinh_NgayNghiHocs
                                                       where abs.HocSinh_HocSinhLopHoc.MaHocSinh == student.MaHocSinh
                                                       && abs.HocSinh_HocSinhLopHoc.MaLopHoc == Class.MaLopHoc
                                                       && abs.MaHocKy == term.MaHocKy
                                                       && abs.Ngay == date
                                                       && abs.MaBuoi == session.MaBuoi
                                                       select abs;
            if (iqAbsent.Count() != 0)
            {
                return true;
            }

            return false;
        }

        public bool AbsentExists(HocSinh_NgayNghiHoc exceptedAbsent, HocSinh_ThongTinCaNhan student, LopHoc_Lop Class, CauHinh_HocKy term, DateTime date)
        {
            IQueryable<HocSinh_NgayNghiHoc> iqAbsent = from abs in db.HocSinh_NgayNghiHocs
                                                       where abs.MaNgayNghiHoc != exceptedAbsent.MaNgayNghiHoc
                                                       && abs.HocSinh_HocSinhLopHoc.MaHocSinh == student.MaHocSinh
                                                       && abs.HocSinh_HocSinhLopHoc.MaLopHoc == Class.MaLopHoc
                                                       && abs.MaHocKy == term.MaHocKy
                                                       && abs.Ngay == date
                                                       select abs;
            if (iqAbsent.Count() != 0)
            {
                return true;
            }

            return false;
        }

        public bool AbsentExists(HocSinh_NgayNghiHoc exceptedAbsent, HocSinh_ThongTinCaNhan student, LopHoc_Lop Class, CauHinh_HocKy term, DateTime date, CauHinh_Buoi session)
        {
            IQueryable<HocSinh_NgayNghiHoc> iqAbsent = from abs in db.HocSinh_NgayNghiHocs
                                                       where abs.MaNgayNghiHoc != exceptedAbsent.MaNgayNghiHoc
                                                       && abs.HocSinh_HocSinhLopHoc.MaHocSinh == student.MaHocSinh
                                                       && abs.HocSinh_HocSinhLopHoc.MaLopHoc == Class.MaLopHoc
                                                       && abs.MaHocKy == term.MaHocKy
                                                       && abs.Ngay == date
                                                       && abs.MaBuoi == session.MaBuoi
                                                       select abs;
            if (iqAbsent.Count() != 0)
            {
                return true;
            }

            return false;
        }
    }
}
