using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class SubjectBL
    {
        private SubjectDA subjectDA;

        public SubjectBL()
        {
            subjectDA = new SubjectDA();
        }

        public void InsertSubject(string subjectName, DanhMuc_KhoiLop grade, DanhMuc_NganhHoc faculty, double markRatio)
        {
            subjectDA.InsertSubject(new DanhMuc_MonHoc()
            {
                TenMonHoc = subjectName,
                MaNganhHoc = faculty.MaNganhHoc,
                MaKhoiLop = grade.MaKhoiLop,
                HeSoDiem = markRatio
            });
        }

        public void UpdateSubject(DanhMuc_MonHoc editedSubject, string newSubjectName, double newMarkRatio)
        {
            editedSubject.TenMonHoc = newSubjectName;
            editedSubject.HeSoDiem = newMarkRatio;
            subjectDA.UpdateSubject(editedSubject);
        }

        public void DeleteSubject(DanhMuc_MonHoc subject)
        {
            subjectDA.DeleteSubject(subject);
        }

        public DanhMuc_MonHoc GetSubject(string subjectName, string facultyName, string gradeName)
        {
            return subjectDA.GetSubject(subjectName, facultyName, gradeName);
        }

        public List<DanhMuc_MonHoc> GetListSubjects(DanhMuc_NganhHoc faculty, DanhMuc_KhoiLop grade)
        {
            if (faculty == null)
            {
                if (grade == null)
                {
                    return subjectDA.GetListSubjects();
                }
                else
                {
                    return subjectDA.GetListSubjects(grade);
                }
            }
            else
            {
                if (grade == null)
                {
                    return subjectDA.GetListSubjects(faculty);
                }
                else
                {
                    return subjectDA.GetListSubjects(faculty, grade);
                }
            }
        }

        public TabularSubject GetTabularSubject(DanhMuc_MonHoc subject)
        {
            TabularSubject tabularSubject = new TabularSubject();
            tabularSubject.MaMonHoc = subject.MaMonHoc;
            tabularSubject.TenMonHoc = subject.TenMonHoc;
            tabularSubject.HeSoDiem = subject.HeSoDiem;
            tabularSubject.TenNganhHoc = subject.DanhMuc_NganhHoc.TenNganhHoc;
            tabularSubject.TenKhoiLop = subject.DanhMuc_KhoiLop.TenKhoiLop;

            return tabularSubject;
        }

        public List<TabularSubject> GetListTabularSubjects(DanhMuc_NganhHoc faculty, DanhMuc_KhoiLop grade, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularSubject> lTabularSubjects = new List<TabularSubject>();
            List<DanhMuc_MonHoc> lSubjects = new List<DanhMuc_MonHoc>();

            if (faculty == null)
            {
                if (grade == null)
                {
                    lSubjects = subjectDA.GetListSubjects(pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    lSubjects =  subjectDA.GetListSubjects(grade, pageCurrentIndex, pageSize, out totalRecords);
                }
            }
            else
            {
                if (grade == null)
                {
                    lSubjects = subjectDA.GetListSubjects(faculty, pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    lSubjects = subjectDA.GetListSubjects(faculty, grade, pageCurrentIndex, pageSize, out totalRecords);
                }
            }

            if (lSubjects.Count != 0)
            {
                foreach (DanhMuc_MonHoc subject in lSubjects)
                {
                    lTabularSubjects.Add(GetTabularSubject(subject));
                }
            }

            return lTabularSubjects;
        }

        public List<TabularSubject> GetListTabularSubjects(DanhMuc_NganhHoc faculty, DanhMuc_KhoiLop grade, string subjectName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularSubject> lTabularSubjects = new List<TabularSubject>();

            if ((subjectName == "") || (string.Compare(subjectName, "tất cả", true) == 0))
            {
                lTabularSubjects = GetListTabularSubjects(faculty, grade, pageCurrentIndex, pageSize, out totalRecords);
            }
            else
            {
                List<DanhMuc_MonHoc> lSubjects = subjectDA.GetListSubjects(subjectName, pageCurrentIndex, pageSize, out totalRecords);
                if (lSubjects.Count != 0)
                {
                    foreach (DanhMuc_MonHoc subject in lSubjects)
                    {
                        lTabularSubjects.Add(GetTabularSubject(subject));
                    }
                }
            }            

            return lTabularSubjects;
        }

        public List<TabularSubject> GetListTabularSubjects(DanhMuc_NganhHoc faculty, DanhMuc_KhoiLop grade, string subjectName, DanhMuc_MonHoc exceptedSubject, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularSubject> lTabularSubjects = new List<TabularSubject>();
            List<DanhMuc_MonHoc> lSubjects = new List<DanhMuc_MonHoc>();

            if ((subjectName == "") || (string.Compare(subjectName, "tất cả", true) == 0))
            {
                lSubjects = subjectDA.GetListSubjects(faculty, grade, exceptedSubject, pageCurrentIndex, pageSize, out totalRecords);
            }
            else
            {
                lSubjects = subjectDA.GetListSubjects(subjectName, exceptedSubject,
                    pageCurrentIndex, pageSize, out totalRecords);
            }

            if (lSubjects.Count != 0)
            {
                foreach (DanhMuc_MonHoc subject in lSubjects)
                {
                    lTabularSubjects.Add(GetTabularSubject(subject));
                }
            }

            return lTabularSubjects;
        }

        public bool SubjectNameExists(string subjectName, string facultyName, string gradeName)
        {
            return subjectDA.SubjectNameExists(subjectName, facultyName, gradeName);
        }

        public bool SubjectNameExists(DanhMuc_MonHoc editedSubject, string newSubjectName)
        {
            if (editedSubject.TenMonHoc == newSubjectName)
            {
                return false;
            }
            else
            {
                return subjectDA.SubjectNameExists(newSubjectName, 
                    editedSubject.DanhMuc_NganhHoc.TenNganhHoc, 
                    editedSubject.DanhMuc_KhoiLop.TenKhoiLop);
            }
        }

        public bool IsDeletable(DanhMuc_MonHoc subject)
        {
            return subjectDA.IsDeletable(subject);
        }        
    }
}
