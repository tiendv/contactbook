using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class StudentBL
    {
        private StudentDA studentDA;

        public StudentBL()
        {
            studentDA = new StudentDA();
        }

        public void InsertStudent(LopHoc_Lop Class, string studentCode, string studentName,
            bool gender, DateTime studentBithday, string birthplace, string address, string phone,
            string fatherName, string fatherJob, DateTime? fatherBirthday,
            string motherName, string motherJob, DateTime? motherBirthday,
            string patronName, string patronJob, DateTime? patronBirthday)
        {
            ClassBL classBL = new ClassBL();
            SystemConfigBL systemConfigBL = new SystemConfigBL();
            StudyingResultBL studyingResultBL = new StudyingResultBL();
            DanhMuc_HanhKiem conduct = null;
            DanhMuc_HocLuc studyingAptitude = null;

            HocSinh_ThongTinCaNhan student = new HocSinh_ThongTinCaNhan
            {
                MaHocSinhHienThi = studentCode,
                HoTen = studentName,
                GioiTinh = gender,
                NgaySinh = studentBithday,
                NoiSinh = birthplace,
                DiaChi = address,
                DienThoai = phone,
                HoTenBo = fatherName,
                NgheNghiepBo = fatherJob,
                NgaySinhBo = fatherBirthday,
                HoTenMe = motherName,
                NgheNghiepMe = motherJob,
                NgaySinhMe = motherBirthday,
                HoTenNguoiDoDau = patronName,
                NgheNghiepNguoiDoDau = patronJob,
                NgaySinhNguoiDoDau = patronBirthday
            };
            studentDA.InsertStudent(student);

            HocSinh_ThongTinCaNhan insertedStudent = studentDA.GetStudent(student.MaHocSinhHienThi);
            HocSinh_HocSinhLopHoc lastedStudentInClass = studentDA.InsertStudentInClass(insertedStudent, Class);
            classBL.IncreaseStudentAmount(Class);

            List<CauHinh_HocKy> terms = systemConfigBL.GetListTerms();
            foreach (CauHinh_HocKy term in terms)
            {
                conduct = new DanhMuc_HanhKiem();
                conduct.MaHanhKiem = -1;
                studyingAptitude = new DanhMuc_HocLuc();
                studyingAptitude.MaHocLuc = -1;
                studyingResultBL.InsertTermStudyingResult(term, lastedStudentInClass, -1, conduct, studyingAptitude);
            }
        }

        public void UpdateHocSinh(HocSinh_ThongTinCaNhan editedStudent, LopHoc_Lop Class, string studentCode, string studentName,
            bool gender, DateTime studentBithday, string birthplace, string address, string phone,
            string fatherName, string fatherJob, DateTime? fatherBirthday,
            string motherName, string motherJob, DateTime? motherBirthday,
            string patronName, string patronJob, DateTime? patronBirthday)
        {
            editedStudent.MaHocSinhHienThi = studentCode;
            editedStudent.HoTen = studentName;
            editedStudent.GioiTinh = gender;
            editedStudent.NgaySinh = studentBithday;
            editedStudent.NoiSinh = birthplace;
            editedStudent.DiaChi = address;
            editedStudent.DienThoai = phone;
            editedStudent.HoTenBo = fatherName;
            editedStudent.NgheNghiepBo = fatherJob;
            editedStudent.NgaySinhBo = fatherBirthday;
            editedStudent.HoTenMe = motherName;
            editedStudent.NgheNghiepMe = motherJob;
            editedStudent.NgaySinhMe = motherBirthday;
            editedStudent.HoTenNguoiDoDau = patronName;
            editedStudent.NgheNghiepNguoiDoDau = patronJob;
            editedStudent.NgaySinhNguoiDoDau = patronBirthday;
            studentDA.UpdateStudent(editedStudent);

            HocSinh_HocSinhLopHoc studentInClass = studentDA.GetLastedStudentInClass(editedStudent);
            studentDA.UpdateStudentInClass(studentInClass, Class);
        }

        public void DeleteStudent(int studentId)
        {
            studentDA.DeleteStudent(studentId);
        }

        public bool IsDeletable(string studentCode)
        {
            return studentDA.IsDeletable(studentCode);
        }

        public bool StudentCodeExists(string studentCode)
        {
            return studentDA.StudentCodeExists(studentCode);
        }

        public HocSinh_ThongTinCaNhan GetStudent(string studentCode)
        {
            return studentDA.GetStudent(studentCode);
        }        

        public HocSinh_ThongTinCaNhan GetStudent(int studentId)
        {
            return studentDA.GetStudent(studentId);
        }

        public HocSinh_HocSinhLopHoc GetStudentInClass(int studentInClassId)
        {
            return studentDA.GetStudentInClass(studentInClassId);
        }

        public HocSinh_HocSinhLopHoc GetStudentInClass(HocSinh_ThongTinCaNhan student, CauHinh_NamHoc year)
        {
            return studentDA.GetStudentInClass(student, year);
        }

        public List<TabularStudent> GetTabularStudents(CauHinh_NamHoc year, DanhMuc_NganhHoc faculty, DanhMuc_KhoiLop grade,
            LopHoc_Lop Class, string studentCode, string studentName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularStudent> tabularStudents = new List<TabularStudent>();
            List<HocSinh_HocSinhLopHoc> studentInClasses = new List<HocSinh_HocSinhLopHoc>();
            TabularStudent tabularStudent = null;
            bool bStudentNameIsAll = (string.Compare(studentName, "tất cả", true) == 0) || (studentName == "");

            if ((string.Compare(studentCode, "tất cả", true) != 0) && (studentCode != ""))
            {
                HocSinh_ThongTinCaNhan student = studentDA.GetStudent(studentCode);
                if (student != null)
                {
                    studentInClasses.Add(studentDA.GetStudentInClass(student, year));
                }

                totalRecords = studentInClasses.Count;
            }
            else
            {
                if (Class != null)
                {
                    if (bStudentNameIsAll)
                    {
                        studentInClasses = studentDA.GetStudentInClasses(Class, pageCurrentIndex, pageSize, out totalRecords);
                    }
                    else
                    {
                        studentInClasses = studentDA.GetStudentInClasses(Class, studentName, pageCurrentIndex, pageSize, out totalRecords);
                    }
                }
                else
                {
                    if (faculty == null)
                    {
                        if (grade == null)
                        {
                            if (bStudentNameIsAll)
                            {
                                studentInClasses = studentDA.GetStudentInClasses(year, pageCurrentIndex, pageSize, out totalRecords);
                            }
                            else
                            {
                                studentInClasses = studentDA.GetStudentInClasses(year, studentName, pageCurrentIndex, pageSize, out totalRecords);
                            }
                        }
                        else
                        {
                            if (bStudentNameIsAll)
                            {
                                studentInClasses = studentDA.GetStudentInClasses(year, grade, pageCurrentIndex, pageSize, out totalRecords);
                            }
                            else
                            {
                                studentInClasses = studentDA.GetStudentInClasses(year, grade, studentName, pageCurrentIndex, pageSize, out totalRecords);
                            }
                        }
                    }
                    else
                    {
                        if (grade == null)
                        {
                            if (bStudentNameIsAll)
                            {
                                studentInClasses = studentDA.GetStudentInClasses(year, faculty, pageCurrentIndex, pageSize, out totalRecords);
                            }
                            else
                            {
                                studentInClasses = studentDA.GetStudentInClasses(year, faculty, studentName, pageCurrentIndex, pageSize, out totalRecords);
                            }
                        }
                        else
                        {
                            if (bStudentNameIsAll)
                            {
                                studentInClasses = studentDA.GetStudentInClasses(year, faculty, grade, pageCurrentIndex, pageSize, out totalRecords);
                            }
                            else
                            {
                                studentInClasses = studentDA.GetStudentInClasses(year, faculty, grade, studentName, pageCurrentIndex, pageSize, out totalRecords);
                            }
                        }
                    }
                }
            }

            foreach (HocSinh_HocSinhLopHoc studentInClass in studentInClasses)
            {
                tabularStudent = new TabularStudent();
                tabularStudent.MaHocSinh = studentInClass.MaHocSinh;
                tabularStudent.MaHocSinhHienThi = studentInClass.HocSinh_ThongTinCaNhan.MaHocSinhHienThi;
                tabularStudent.TenHocSinh = studentInClass.HocSinh_ThongTinCaNhan.HoTen;
                tabularStudent.TenNganh = studentInClass.LopHoc_Lop.DanhMuc_NganhHoc.TenNganhHoc;
                tabularStudent.TenKhoi = studentInClass.LopHoc_Lop.DanhMuc_KhoiLop.TenKhoiLop;
                tabularStudent.TenLopHoc = studentInClass.LopHoc_Lop.TenLopHoc;
                tabularStudent.MaLopHoc = studentInClass.MaLopHoc;

                tabularStudents.Add(tabularStudent);
            }

            return tabularStudents;
        }        

        public TabularClass GetTabularClass(CauHinh_NamHoc year, HocSinh_ThongTinCaNhan student)
        {
            ClassBL classBL = new ClassBL();
            TabularClass tabularClass = null;
            LopHoc_Lop Class = studentDA.GetClass(year, student);
            if (Class != null)
            {
                tabularClass = classBL.GetTabularClass(Class);
            }

            return tabularClass;
        }

        public List<StudentDropdownListItem> GetStudents(DanhMuc_NganhHoc faculty, DanhMuc_KhoiLop grade, LopHoc_Lop Class)
        {
            List<HocSinh_HocSinhLopHoc> studentInClasses = new List<HocSinh_HocSinhLopHoc>();
            List<StudentDropdownListItem> students = new List<StudentDropdownListItem>();
            StudentDropdownListItem student = null;

            if (Class == null)
            {
                if (faculty == null)
                {
                    if (grade == null)
                    {
                        studentInClasses = studentDA.GetStudentInClasses();
                    }
                    else
                    {
                        studentInClasses = studentDA.GetStudentInClasses(grade);
                    }
                }
                else
                {
                    if (grade == null)
                    {
                        studentInClasses = studentDA.GetStudentInClasses(faculty);
                    }
                    else
                    {
                        studentInClasses = studentDA.GetStudentInClasses(grade, faculty);
                    }
                }
            }
            else
            {
                studentInClasses = studentDA.GetStudentInClasses(Class);
            }

            foreach (HocSinh_HocSinhLopHoc studentInClass in studentInClasses)
            {
                student = new StudentDropdownListItem();
                student.StudentId = studentInClass.MaHocSinh;
                student.StudentCode = studentInClass.HocSinh_ThongTinCaNhan.MaHocSinhHienThi;
                student.StudentName = studentInClass.HocSinh_ThongTinCaNhan.HoTen;
                student.StudentInClassId = studentInClass.MaHocSinhLopHoc;

                students.Add(student);
            }

            return students;
        }        

        public List<CauHinh_NamHoc> GetYears(HocSinh_ThongTinCaNhan student)
        {
            return studentDA.GetYears(student);
        }        

        public LopHoc_Lop GetClass(HocSinh_ThongTinCaNhan student, CauHinh_NamHoc year)
        {
            return studentDA.GetClass(year, student);
        }

        public LopHoc_Lop GetLastedClass(HocSinh_ThongTinCaNhan student)
        {
            return studentDA.GetLastedClass(student);
        }

        public List<TabularHanhKiemHocSinh> GetListHanhKiemHocSinh(int maLopHoc, int maHocKy, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            return studentDA.GetListHanhKiemHocSinh(maLopHoc, maHocKy,
                pageCurrentIndex, pageSize, out totalRecords);
        }

        public void UpdateStudenTermConduct(LopHoc_Lop Class, CauHinh_HocKy term, HocSinh_ThongTinCaNhan student, DanhMuc_HanhKiem conduct)
        {
            studentDA.UpdateStudenTermConduct(Class, term, student, conduct);
        }
    }
}
