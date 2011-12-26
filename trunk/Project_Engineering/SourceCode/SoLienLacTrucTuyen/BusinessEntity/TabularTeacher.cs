using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.BusinessEntity
{
    public class TabularTeacher
    {
        public Guid UserId { get; set; }

        public string MaHienThiGiaoVien { get; set; }
        public string HoTen { get; set; }

        private bool? gioiTinh;
        public bool? GioiTinh 
        {
            get
            {
                return gioiTinh;
            }
            set
            {
                gioiTinh = value;
                if (gioiTinh != null)
                {
                    StringGioiTinh = ((bool)gioiTinh) ? "Nam" : "Nữ";
                }
            }
        }

        private DateTime? ngaySinh;
        public DateTime? NgaySinh 
        { 
            get
            {
                return ngaySinh;
            } 
            set 
            {
                ngaySinh = value;
                if (ngaySinh != null)
                {
                    DateTime birthday = (DateTime)ngaySinh;
                    StringNgaySinh = String.Format("{0}/{1}/{2}", birthday.Day, birthday.Month, birthday.Year);
                }
            } 
        }

        public string StringGioiTinh { get; set; }
        public string StringNgaySinh { get; set; }
    }

    public class TabularFormering
    {
        public int YearId { get; set; }
        public string YearName { get; set; }

        public int ClassId { get; set; }
        public string ClassName { get; set; }
    }

    public class TabularTeaching
    {
        public int YearId { get; set; }
        public string YearName { get; set; }

        public int TermId { get; set; }
        public string TermName { get; set; }

        public int ClassId { get; set; }
        public string ClassName { get; set; }

        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
    }
}
