using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class StudentActivityBL:BaseBL
    {
        private StudentActivityDA studentActivityDA;

        public StudentActivityBL(School school) : base(school)
        {
            studentActivityDA = new StudentActivityDA(school);
        }

        public void InsertStudentActivity(HocSinh_ThongTinCaNhan student, CauHinh_HocKy term, DateTime date, string title, string content, DanhMuc_ThaiDoThamGia attitude)
        {
            studentActivityDA.InsertStudentActivity(student, term, date, title, content, attitude);
        }

        public void UpdateStudentActivity(HocSinh_HoatDong editedStudentActivity, DateTime date, string content, DanhMuc_ThaiDoThamGia attitude)
        {
            editedStudentActivity.Ngay = date;
            editedStudentActivity.NoiDung = content;
            if (attitude != null)
            {
                editedStudentActivity.MaThaiDoThamGia = attitude.MaThaiDoThamGia;
            }
            else
            {
                editedStudentActivity.MaThaiDoThamGia = null;
            }

            studentActivityDA.UpdateStudentActivity(editedStudentActivity);
        }

        public void DeleteStudentActivity(HocSinh_HoatDong deletedStudentActivity)
        {
            studentActivityDA.DeleteStudentActivity(deletedStudentActivity);
        }        

        public HocSinh_HoatDong GetStudentActivity(int studentActivityId)
        {
            return studentActivityDA.GetStudentActivity(studentActivityId);
        }

        public HocSinh_HoatDong GetStudentActivity(HocSinh_ThongTinCaNhan student, CauHinh_NamHoc year, CauHinh_HocKy term, DateTime date)
        {
            return studentActivityDA.GetStudentActivity(student, year, term, date);
        }

        public List<TabularStudentActivity> GetTabularStudentActivities(HocSinh_ThongTinCaNhan student, CauHinh_NamHoc year, CauHinh_HocKy term, DateTime beginDate, DateTime endDate,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            AttitudeBL attitudeBL = new AttitudeBL(school);
            List<TabularStudentActivity> tabularStudentActivities = new List<TabularStudentActivity>();
            List<HocSinh_HoatDong> studentActivities = new List<HocSinh_HoatDong>();
            studentActivities = studentActivityDA.GetStudentActivities(student, year, term, beginDate, endDate, pageCurrentIndex, pageSize, out totalRecords);
            TabularStudentActivity tabularStudentActivity = null;

            foreach (HocSinh_HoatDong studentActivity in studentActivities)
            {
                tabularStudentActivity = new TabularStudentActivity();
                tabularStudentActivity.MaHoatDong = studentActivity.MaHoatDong;
                tabularStudentActivity.MaHocSinhLopHoc = studentActivity.HocSinh_HocSinhLopHoc.MaHocSinhLopHoc;
                tabularStudentActivity.TenHoatDong = studentActivity.TieuDe;
                tabularStudentActivity.Ngay = studentActivity.Ngay;
                tabularStudentActivity.StrNgay = studentActivity.Ngay.ToShortDateString();
                if (studentActivity.MaThaiDoThamGia != null)
                {
                    tabularStudentActivity.ThaiDoThamGia = attitudeBL.GetAttitude((int)studentActivity.MaThaiDoThamGia).TenThaiDoThamGia;
                }
                else
                {
                    tabularStudentActivity.ThaiDoThamGia = "Không xác định";
                }

                tabularStudentActivities.Add(tabularStudentActivity);
            }
            return tabularStudentActivities;
        }

        public bool StudentActivityNamExists(string title, HocSinh_ThongTinCaNhan student, CauHinh_NamHoc year, CauHinh_HocKy term, DateTime date)
        {
            StudentBL studentBL = new StudentBL(school);
            //int maLopHoc = studentBL.GetCurrentMaLopHoc(student.MaHocSinh);
            //LopHoc_Lop Class = new LopHoc_Lop();
            //Class.MaLopHoc = maLopHoc;
            return studentActivityDA.StudentActivityNameExists(title, student, year, term, date);
        }

        public bool StudentActivityNamExists(string oldTitlte, string title, HocSinh_ThongTinCaNhan student, CauHinh_NamHoc year, CauHinh_HocKy term, DateTime date)
        {
            if (oldTitlte == title)
            {
                return false;
            }
            else
            {
                StudentBL studentBL = new StudentBL(school);
                //int maLopHoc = studentBL.GetCurrentMaLopHoc(student.MaHocSinh);
                //LopHoc_Lop Class = new LopHoc_Lop();
                //Class.MaLopHoc = maLopHoc;
                return studentActivityDA.StudentActivityNameExists(title, student, year, term, date);
            }
        }
    }
}
