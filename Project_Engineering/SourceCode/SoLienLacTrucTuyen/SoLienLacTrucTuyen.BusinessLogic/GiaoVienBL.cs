using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class GiaoVienBL
    {
        private TeacherDA giaoVienDA;

        public GiaoVienBL()
        {
            giaoVienDA = new TeacherDA();
        }

        public void InsertGiaoVien(string maHienThi, string hoTen, bool gioiTinh,
            DateTime ngaySinh, string diaChi, string dienThoai)
        {
            LopHoc_GiaoVien giaoVien = new LopHoc_GiaoVien
            {
                MaHienThiGiaoVien = maHienThi,
                HoTen = hoTen,
                GioiTinh = gioiTinh,
                NgaySinh = ngaySinh,
                DiaChi = diaChi,
                DienThoai = dienThoai
            };

            giaoVienDA.InsertTeacher(giaoVien);
        }

        public void DeleteGiaoVien(string teacherCode)
        {
            LopHoc_GiaoVien teacher = GetTeacher(teacherCode);
            giaoVienDA.DeleteTeacher(teacher);
        }

        public LopHoc_GiaoVien GetTeacher(string teacherCode)
        {
            return giaoVienDA.GetTeacher(teacherCode);
        }

        public List<TabularGiaoVien> GetListTabularGiaoViens(string maHienThiGiaoVien, string hoTen,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            if (maHienThiGiaoVien == "")
            {
                if ((hoTen == "") || (string.Compare(hoTen, "tất cả", true) == 0))
                {
                    return giaoVienDA.GetListTabularGiaoViens(
                        pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    return giaoVienDA.GetListTabularGiaoViensByHoTen(hoTen,
                        pageCurrentIndex, pageSize, out totalRecords);
                }
            }
            else
            {
                if ((hoTen == "") || (string.Compare(hoTen, "tất cả", true) == 0))
                {
                    return giaoVienDA.GetListTabularGiaoViensByMaHienThi(maHienThiGiaoVien,
                        pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    return giaoVienDA.GetListTabularGiaoViens(maHienThiGiaoVien, hoTen,
                        pageCurrentIndex, pageSize, out totalRecords);
                }
            }
        }

        public List<TabularGiaoVien> GetListTabularGiaoVienKhongChuNhiems(
            int maNamHoc,
            string maHienThiGiaoVien, string hoTen,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            if (maHienThiGiaoVien == "")
            {
                if ((hoTen == "") || (string.Compare(hoTen, "tất cả", true) == 0))
                {
                    return giaoVienDA.GetListTabularGiaoVienKhongChuNhiems(maNamHoc,
                        pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    return giaoVienDA.GetListTabularGiaoVienKhongChuNhiemsByHoTen(maNamHoc, hoTen,
                        pageCurrentIndex, pageSize, out totalRecords);
                }
            }
            else
            {
                if ((hoTen == "") || (string.Compare(hoTen, "tất cả", true) == 0))
                {
                    return giaoVienDA.GetListTabularGiaoVienKhongChuNhiemsByMaHienThi(maNamHoc, maHienThiGiaoVien,
                        pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    return giaoVienDA.GetListTabularGiaoViens(maNamHoc, maHienThiGiaoVien, hoTen,
                        pageCurrentIndex, pageSize, out totalRecords);
                }
            }
        }

        public bool MaGiaoVienExists(string maGiaoVien)
        {
            return giaoVienDA.TeacherCodeExists(maGiaoVien);
        }

        public bool IsDeletable(string teacherCode)
        {
            LopHoc_GiaoVien teacher = GetTeacher(teacherCode);
            return giaoVienDA.IsDeletable(teacher);
        }

        public LopHoc_GiaoVien GetGiaoVien(int maGiaoVien)
        {
            return giaoVienDA.GetTeacher(maGiaoVien);
        }

        public void UpdateTeacher(LopHoc_GiaoVien editedTeacher,
            string newTeacherName, bool newGender, DateTime newBirthday, string newAddress, string newPhone)
        {
            editedTeacher.HoTen = newTeacherName;
            editedTeacher.GioiTinh = newGender;
            editedTeacher.NgaySinh = newBirthday;
            editedTeacher.DiaChi = newAddress;
            editedTeacher.DienThoai = newPhone;

            giaoVienDA.UpdateTeacher(editedTeacher);
        }

        public List<TabularHoatDongChuNhiem> GetListTbHoatDongChuNhiem(int maGiaoVien,
            int pageSize, int pageCurrentIndex, out double totalRecords)
        {
            return giaoVienDA.GetListTbHoatDongChuNhiem(maGiaoVien, pageSize, pageCurrentIndex, out totalRecords);
        }

        public List<TabularHoatDongGiangDay> GetListTbHoatDongGiangDay(int maGiaoVien,
            int pageSize, int pageCurrentIndex, out double totalRecords)
        {
            return giaoVienDA.GetListTbHoatDongGiangDay(maGiaoVien, pageSize, pageCurrentIndex, out totalRecords);
        }

        public bool IsTeaching(int maGiaoVien, int maHocKy, int maThu, int maTiet)
        {
            return giaoVienDA.IsTeaching(maGiaoVien, maHocKy, maThu, maTiet);
        }
    }
}
