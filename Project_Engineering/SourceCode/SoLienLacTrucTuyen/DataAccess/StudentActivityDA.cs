using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class StudentActivityDA : BaseDA
    {
        public StudentActivityDA()
            : base()
        {
        }

        public void InsertStudentActivity(HocSinh_ThongTinCaNhan student, CauHinh_HocKy term, DateTime date, string title, string content, DanhMuc_ThaiDoThamGia attitude)
        {
            HocSinh_HocSinhLopHoc studentInClass = null;

            IQueryable<HocSinh_HocSinhLopHoc> iqStudentInClass = from stdInCls in db.HocSinh_HocSinhLopHocs
                                                                 where stdInCls.MaHocSinh == student.MaHocSinh
                                                                 select stdInCls;

            if (iqStudentInClass.Count() != 0)
            {
                studentInClass = iqStudentInClass.OrderByDescending(stdInCls => stdInCls.MaHocSinhLopHoc).First();

                HocSinh_HoatDong studentActivity = new HocSinh_HoatDong();
                studentActivity.MaHocSinhLopHoc = studentInClass.MaHocSinhLopHoc;
                studentActivity.MaHocKy = term.MaHocKy;
                studentActivity.Ngay = date;
                studentActivity.TieuDe = title;
                studentActivity.NoiDung = content;
                if (attitude != null)
                {
                    studentActivity.MaThaiDoThamGia = attitude.MaThaiDoThamGia;
                }
                db.HocSinh_HoatDongs.InsertOnSubmit(studentActivity);
                db.SubmitChanges();
            }
        }

        public void UpdateStudentActivity(HocSinh_HoatDong editedStudentActivity)
        {
            HocSinh_HoatDong studentActivity = null;

            IQueryable<HocSinh_HoatDong> iqStudentActivity = from stdAct in db.HocSinh_HoatDongs
                                                             where stdAct.MaHoatDong == editedStudentActivity.MaHoatDong
                                                             select stdAct;

            if (iqStudentActivity.Count() != 0)
            {
                studentActivity = iqStudentActivity.First();
                studentActivity.Ngay = editedStudentActivity.Ngay;
                studentActivity.TieuDe = editedStudentActivity.TieuDe;
                studentActivity.NoiDung = editedStudentActivity.NoiDung;
                if (editedStudentActivity.MaThaiDoThamGia != null)
                {
                    studentActivity.MaThaiDoThamGia = editedStudentActivity.MaThaiDoThamGia;
                }

                db.SubmitChanges();
            }
        }

        public void DeleteStudentActivity(HocSinh_HoatDong deletedStudentActivity)
        {
            HocSinh_HoatDong studentActivity = null;

            IQueryable<HocSinh_HoatDong> iqStudentActivity = from stdAct in db.HocSinh_HoatDongs
                                                             where stdAct.MaHoatDong == deletedStudentActivity.MaHoatDong
                                                             select stdAct;

            if (iqStudentActivity.Count() != 0)
            {
                studentActivity = iqStudentActivity.First();
                db.HocSinh_HoatDongs.DeleteOnSubmit(studentActivity);
                db.SubmitChanges();
            }
        }

        public HocSinh_HoatDong GetStudentActivity(int studentActivityId)
        {
            HocSinh_HoatDong studentActivity = null;

            IQueryable<HocSinh_HoatDong> iqStudentActivity = from stdAct in db.HocSinh_HoatDongs
                                                             where stdAct.MaHoatDong == studentActivityId
                                                             select stdAct;

            if (iqStudentActivity.Count() != 0)
            {
                studentActivity = iqStudentActivity.First();
            }

            return studentActivity;
        }

        public HocSinh_HoatDong GetStudentActivity(HocSinh_ThongTinCaNhan student, CauHinh_NamHoc year, CauHinh_HocKy term, DateTime date)
        {
            HocSinh_HoatDong studentActivity = null;

            IQueryable<HocSinh_HoatDong> iqStudentActivity = from stdAct in db.HocSinh_HoatDongs
                                                             join stdInCls in db.HocSinh_HocSinhLopHocs on stdAct.MaHocSinhLopHoc equals stdInCls.MaHocSinhLopHoc
                                                             where stdInCls.MaHocSinh == student.MaHocSinh
                                                             && stdInCls.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                                                             && stdAct.MaHocKy == term.MaHocKy
                                                             && stdAct.Ngay == date
                                                             select stdAct;

            if (iqStudentActivity.Count() != 0)
            {
                studentActivity = iqStudentActivity.First();
            }

            return studentActivity;
        }

        //public HocSinh_HoatDong GetStudentActivity(int maHoatDong, int maHocSinh, int maNamHoc, int maHocKy, DateTime ngay)
        //{
        //    IQueryable<HocSinh_HoatDong> hoatDongs = from ngayNghi in db.HocSinh_HoatDongs
        //                                             join hocSinhLopHoc in db.HocSinh_HocSinhLopHocs on ngayNghi.MaHocSinhLopHoc equals hocSinhLopHoc.MaHocSinhLopHoc
        //                                             join lop in db.LopHoc_Lops on hocSinhLopHoc.MaLopHoc equals lop.MaLopHoc
        //                                             where hocSinhLopHoc.MaHocSinh == maHocSinh && lop.MaNamHoc == maNamHoc
        //                                                && ngayNghi.MaHocKy == maHocKy && ngayNghi.Ngay == ngay
        //                                                && ngayNghi.MaHoatDong == maHoatDong
        //                                             select ngayNghi;
        //    if (hoatDongs.Count() != 0)
        //    {
        //        return hoatDongs.First();
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        public List<HocSinh_HoatDong> GetListStudentActivities(HocSinh_ThongTinCaNhan student, CauHinh_NamHoc year, CauHinh_HocKy term, DateTime beginDate, DateTime endDate,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<HocSinh_HoatDong> studentActivities = new List<HocSinh_HoatDong>();
            AttitudeDA attitudeDA = new AttitudeDA();

            IQueryable<HocSinh_HoatDong> iqStudentActivity = from stdAct in db.HocSinh_HoatDongs
                                                             join stdInCls in db.HocSinh_HocSinhLopHocs on stdAct.MaHocSinhLopHoc equals stdInCls.MaHocSinhLopHoc
                                                             where stdInCls.MaHocSinh == student.MaHocSinh
                                                             && stdInCls.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                                                             && stdAct.MaHocKy == term.MaHocKy
                                                             && stdAct.Ngay >= beginDate && stdAct.Ngay <= endDate
                                                             select stdAct;
            totalRecords = iqStudentActivity.Count();
            if (totalRecords != 0)
            {
                studentActivities = iqStudentActivity.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return studentActivities;
        }

        public bool StudentActivityNameExists(string title, HocSinh_ThongTinCaNhan student, CauHinh_NamHoc year, CauHinh_HocKy term, DateTime date)
        {
            IQueryable<HocSinh_HoatDong> iqStudentActivity = from stdAct in db.HocSinh_HoatDongs
                                                             join stdInCls in db.HocSinh_HocSinhLopHocs on stdAct.MaHocSinhLopHoc equals stdInCls.MaHocSinhLopHoc
                                                             where stdInCls.MaHocSinh == student.MaHocSinh
                                                             && stdInCls.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                                                             && stdAct.MaHocKy == term.MaHocKy
                                                             && stdAct.Ngay == date
                                                             && stdAct.TieuDe == title
                                                             select stdAct;

            if (iqStudentActivity.Count() != 0)
            {
                return true;
            }

            return false;
        }
    }
}
