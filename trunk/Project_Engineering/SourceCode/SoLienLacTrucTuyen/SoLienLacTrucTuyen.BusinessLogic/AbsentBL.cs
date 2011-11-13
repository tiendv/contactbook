using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class AbsentBL
    {
        private AbsentDA absentDA;

        public AbsentBL()
        {
            absentDA = new AbsentDA();
        }

        public void InsertAbsent(HocSinh_ThongTinCaNhan student, CauHinh_HocKy term, DateTime date, CauHinh_Buoi session, bool permission, string reason)
        {
            StudentBL studentBL = new StudentBL();
            LopHoc_Lop Class = studentBL.GetLastedClass(student);
            HocSinh_HocSinhLopHoc studentInClass = studentBL.GetStudentInClass(student, Class.CauHinh_NamHoc);

            absentDA.InsertAbsent(studentInClass, term, date, session, permission, reason);
        }

        public void UpdateAbsent(HocSinh_NgayNghiHoc editedAbsent, CauHinh_HocKy newTerm, DateTime newDate, CauHinh_Buoi newSession, bool newPermission, string newReason)
        {
            absentDA.UpdateAbsent(editedAbsent);
        }

        public void DeleteAbsent(int maNgayNghiHoc)
        {
            absentDA.DeleteAbsent(maNgayNghiHoc);
        }

        public HocSinh_NgayNghiHoc GetAbsent(int absentId)
        {
            return absentDA.GetAbsent(absentId);
        }

        public HocSinh_NgayNghiHoc GetAbsent(HocSinh_ThongTinCaNhan student, CauHinh_NamHoc year, CauHinh_HocKy term, DateTime date)
        {
            StudentBL studentBL = new StudentBL();
            HocSinh_HocSinhLopHoc studentInClass = studentBL.GetStudentInClass(student, year);

            return absentDA.GetAbsent(studentInClass, term, date);
        }

        public HocSinh_NgayNghiHoc GetAbsent(HocSinh_NgayNghiHoc exceptedAbsent, HocSinh_ThongTinCaNhan student, CauHinh_NamHoc year, CauHinh_HocKy term, DateTime date)
        {
            StudentBL studentBL = new StudentBL();
            HocSinh_HocSinhLopHoc studentInClass = studentBL.GetStudentInClass(student, year);

            return absentDA.GetAbsent(exceptedAbsent, studentInClass, term, date);
        }

        public List<TabularAbsent> GetTabularAbsents(HocSinh_ThongTinCaNhan student, CauHinh_NamHoc year, CauHinh_HocKy term, DateTime beginDate, DateTime endDate, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            StudentBL studentBL = new StudentBL();
            SystemConfigBL systemConfigBL = new SystemConfigBL();
            List<TabularAbsent> tabularAbsents = new List<TabularAbsent>();
            TabularAbsent tabularAbsent = null;
            HocSinh_HocSinhLopHoc studentInClass = studentBL.GetStudentInClass(student, year);
            List<HocSinh_NgayNghiHoc> absents = absentDA.GetAbsents(studentInClass, term, beginDate, endDate, pageCurrentIndex, pageSize, out totalRecords);

            foreach (HocSinh_NgayNghiHoc absent in absents)
            {
                tabularAbsent = new TabularAbsent();
                tabularAbsent.MaHocSinhLopHoc = absent.MaHocSinhLopHoc;
                tabularAbsent.Ngay = absent.Ngay.Day + "/" + absent.Ngay.Month + "/" + absent.Ngay.Year;
                tabularAbsent.Buoi = systemConfigBL.GetSessionName(absent.MaBuoi);
                tabularAbsent.XinPhep = (absent.XinPhep) ? "Có" : "Không";
                tabularAbsent.LyDo = absent.LyDo;
                tabularAbsent.XacNhan = (absent.XacNhan) ? "Có" : "Không";

                tabularAbsents.Add(tabularAbsent);
            }

            return tabularAbsents;
        }
        
        public bool Confirmed(HocSinh_NgayNghiHoc absent)
        {
            return absentDA.IsConfirmed(absent);
        }

        public void ConfirmAbsent(HocSinh_NgayNghiHoc editedAbsent)
        {
            bool xacNhan = true;
            absentDA.ConfirmAbsent(editedAbsent, xacNhan);
        }

        public bool AbsentExists(HocSinh_NgayNghiHoc exceptedAbsent, HocSinh_ThongTinCaNhan student, CauHinh_HocKy term, DateTime date, CauHinh_Buoi session)
        {
            StudentBL studentBL = new StudentBL();
            LopHoc_Lop Class = studentBL.GetLastedClass(student);
            int maLopHoc = Class.MaLopHoc;

            if (exceptedAbsent == null)
            {
                if (session == null)
                {
                    return absentDA.AbsentExists(student, Class, term, date);
                }
                else
                {
                    bool bAllDay = absentDA.AbsentExists(student, Class, term, date, null);
                    if (bAllDay)
                    {
                        return true;
                    }
                    else
                    {
                        return absentDA.AbsentExists(student, Class, term, date, session);
                    }
                }
            }
            else
            {
                if (session == null)
                {
                    return absentDA.AbsentExists(exceptedAbsent, student, Class, term, date);
                }
                else
                {
                    bool bAllDay = absentDA.AbsentExists(exceptedAbsent, student, Class, term, date, null);
                    if (bAllDay)
                    {
                        return true;
                    }
                    else
                    {
                        return absentDA.AbsentExists(exceptedAbsent, student, Class, term, date, session);
                    }
                }
            }
        }
    }
}
