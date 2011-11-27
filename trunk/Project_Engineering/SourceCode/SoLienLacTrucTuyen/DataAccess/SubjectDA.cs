using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class SubjectDA : BaseDA
    {
        public SubjectDA(School school)
            : base(school)
        {
        }

        public void InsertSubject(DanhMuc_MonHoc subject)
        {
            db.DanhMuc_MonHocs.InsertOnSubmit(subject);
            db.SubmitChanges();
        }

        public void UpdateSubject(DanhMuc_MonHoc editedSubject)
        {
            DanhMuc_MonHoc subject = null;
            IQueryable<DanhMuc_MonHoc> iqSubject = from subj in db.DanhMuc_MonHocs
                                                   where subj.MaMonHoc == editedSubject.MaMonHoc
                                                   && subj.SchoolId == school.SchoolId
                                                   select subj;
            if (iqSubject.Count() != 0)
            {
                subject = iqSubject.First();
                subject.TenMonHoc = editedSubject.TenMonHoc;
                subject.HeSoDiem = editedSubject.HeSoDiem;
                db.SubmitChanges();
            }
        }

        public void DeleteSubject(DanhMuc_MonHoc deletedSubject)
        {
            DanhMuc_MonHoc subject = null;
            IQueryable<DanhMuc_MonHoc> iqSubject = from subj in db.DanhMuc_MonHocs
                                                   where subj.MaMonHoc == deletedSubject.MaMonHoc
                                                   && subj.SchoolId == school.SchoolId
                                                   select subj;
            if (iqSubject.Count() != 0)
            {
                subject = iqSubject.First();
                db.DanhMuc_MonHocs.DeleteOnSubmit(subject);
                db.SubmitChanges();
            }
        }

        public DanhMuc_MonHoc GetSubject(string subjectName, string facultyName, string gradeName)
        {
            DanhMuc_MonHoc subject = null;
            IQueryable<DanhMuc_MonHoc> iqSubject = from subj in db.DanhMuc_MonHocs
                                                   where subj.TenMonHoc == subjectName
                                                        && subj.DanhMuc_NganhHoc.TenNganhHoc == facultyName
                                                        && subj.DanhMuc_KhoiLop.TenKhoiLop == gradeName
                                                        && subj.SchoolId == school.SchoolId
                                                   select subj;

            if (iqSubject.Count() != 0)
            {
                subject = iqSubject.First();
            }

            return subject;
        }

        public List<DanhMuc_MonHoc> GetListSubjects()
        {
            List<DanhMuc_MonHoc> lSubjects = new List<DanhMuc_MonHoc>();

            IQueryable<DanhMuc_MonHoc> iqSubject = from subj in db.DanhMuc_MonHocs
                                                   where subj.SchoolId == school.SchoolId
                                                   select subj;
            if (iqSubject.Count() != 0)
            {
                lSubjects = iqSubject.OrderBy(subj => subj.TenMonHoc)
                    .ThenBy(subj => subj.DanhMuc_NganhHoc.TenNganhHoc)
                    .ThenBy(subj => subj.DanhMuc_KhoiLop.TenKhoiLop).ToList();
            }

            return lSubjects;
        }

        public List<DanhMuc_MonHoc> GetListSubjects(DanhMuc_NganhHoc faculty, DanhMuc_KhoiLop grade)
        {
            List<DanhMuc_MonHoc> lSubjects = new List<DanhMuc_MonHoc>();

            IQueryable<DanhMuc_MonHoc> iqSubject = from subj in db.DanhMuc_MonHocs
                                                   where subj.MaNganhHoc == faculty.MaNganhHoc
                                                   && subj.MaKhoiLop == grade.MaKhoiLop
                                                   && subj.SchoolId == school.SchoolId
                                                   select subj;
            if (iqSubject.Count() != 0)
            {
                lSubjects = iqSubject.OrderBy(subj => subj.TenMonHoc)
                    .ThenBy(subj => subj.DanhMuc_NganhHoc.TenNganhHoc)
                    .ThenBy(subj => subj.DanhMuc_KhoiLop.TenKhoiLop).ToList();
            }

            return lSubjects;
        }

        public List<DanhMuc_MonHoc> GetListSubjects(DanhMuc_NganhHoc faculty)
        {
            List<DanhMuc_MonHoc> lSubjects = new List<DanhMuc_MonHoc>();

            IQueryable<DanhMuc_MonHoc> iqSubject = from subj in db.DanhMuc_MonHocs
                                                   where subj.MaNganhHoc == faculty.MaNganhHoc
                                                   && subj.SchoolId == school.SchoolId
                                                   select subj;
            if (iqSubject.Count() != 0)
            {
                lSubjects = iqSubject.OrderBy(subj => subj.TenMonHoc)
                    .ThenBy(subj => subj.DanhMuc_NganhHoc.TenNganhHoc)
                    .ThenBy(subj => subj.DanhMuc_KhoiLop.TenKhoiLop).ToList();
            }

            return lSubjects;
        }

        public List<DanhMuc_MonHoc> GetListSubjects(DanhMuc_KhoiLop grade)
        {
            List<DanhMuc_MonHoc> lSubjects = new List<DanhMuc_MonHoc>();

            IQueryable<DanhMuc_MonHoc> iqSubject = from subj in db.DanhMuc_MonHocs
                                                   where subj.MaKhoiLop == grade.MaKhoiLop
                                                   && subj.SchoolId == school.SchoolId
                                                   select subj;
            if (iqSubject.Count() != 0)
            {
                lSubjects = iqSubject.OrderBy(subj => subj.TenMonHoc)
                    .ThenBy(subj => subj.DanhMuc_NganhHoc.TenNganhHoc)
                    .ThenBy(subj => subj.DanhMuc_KhoiLop.TenKhoiLop).ToList();
            }

            return lSubjects;
        }

        public List<DanhMuc_MonHoc> GetListSubjects(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<DanhMuc_MonHoc> iqSubject = from subj in db.DanhMuc_MonHocs
                                                   where subj.SchoolId == school.SchoolId
                                                   select subj;

            return GetListSubjects(ref iqSubject, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<DanhMuc_MonHoc> GetListSubjects(DanhMuc_NganhHoc faculty, DanhMuc_KhoiLop grade, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<DanhMuc_MonHoc> iqSubject = from subj in db.DanhMuc_MonHocs
                                                   where subj.DanhMuc_NganhHoc.MaNganhHoc == faculty.MaNganhHoc
                                                   && subj.DanhMuc_KhoiLop.MaKhoiLop == grade.MaKhoiLop
                                                   && subj.SchoolId == school.SchoolId
                                                   select subj;

            return GetListSubjects(ref iqSubject, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<DanhMuc_MonHoc> GetListSubjects(DanhMuc_NganhHoc faculty, DanhMuc_KhoiLop grade, DanhMuc_MonHoc exceptedSubject, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<DanhMuc_MonHoc> iqSubject = from subj in db.DanhMuc_MonHocs
                                                   where subj.DanhMuc_NganhHoc.MaNganhHoc == faculty.MaNganhHoc
                                                   && subj.DanhMuc_KhoiLop.MaKhoiLop == grade.MaKhoiLop
                                                   && subj.MaMonHoc != exceptedSubject.MaMonHoc
                                                   && subj.SchoolId == school.SchoolId
                                                   select subj;

            return GetListSubjects(ref iqSubject, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<DanhMuc_MonHoc> GetListSubjects(DanhMuc_NganhHoc faculty, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<DanhMuc_MonHoc> iqSubject = from subj in db.DanhMuc_MonHocs
                                                   where subj.DanhMuc_NganhHoc.MaNganhHoc == faculty.MaNganhHoc
                                                   && subj.SchoolId == school.SchoolId
                                                   select subj;

            return GetListSubjects(ref iqSubject, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<DanhMuc_MonHoc> GetListSubjects(DanhMuc_KhoiLop grade, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<DanhMuc_MonHoc> iqSubject = from subj in db.DanhMuc_MonHocs
                                                   where subj.DanhMuc_KhoiLop.MaKhoiLop == grade.MaKhoiLop
                                                   && subj.SchoolId == school.SchoolId
                                                   select subj;

            return GetListSubjects(ref iqSubject, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<DanhMuc_MonHoc> GetListSubjects(string subjectName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<DanhMuc_MonHoc> iqSubject = from subj in db.DanhMuc_MonHocs
                                                   where subj.TenMonHoc == subjectName
                                                   && subj.SchoolId == school.SchoolId
                                                   select subj;

            return GetListSubjects(ref iqSubject, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<DanhMuc_MonHoc> GetListSubjects(string subjectName, DanhMuc_MonHoc exceptedSubject, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<DanhMuc_MonHoc> iqSubject = from subj in db.DanhMuc_MonHocs
                                                   where subj.TenMonHoc == subjectName && subj.MaMonHoc != exceptedSubject.MaMonHoc
                                                   && subj.SchoolId == school.SchoolId
                                                   select subj;

            return GetListSubjects(ref iqSubject, pageCurrentIndex, pageSize, out totalRecords);
        }

        public bool SubjectNameExists(string subjectName, string facultyName, string gradeName)
        {
            IQueryable<DanhMuc_MonHoc> iqSubject;
            iqSubject = from subject in db.DanhMuc_MonHocs
                        where subject.TenMonHoc == subjectName
                          && subject.DanhMuc_NganhHoc.TenNganhHoc == facultyName
                          && subject.DanhMuc_KhoiLop.TenKhoiLop == gradeName
                          && subject.SchoolId == school.SchoolId
                        select subject;
            if (iqSubject.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsDeletable(DanhMuc_MonHoc subject)
        {
            IQueryable<LopHoc_MonHocTKB> iqScheduledSubjects = from scheduledSubject in db.LopHoc_MonHocTKBs
                                                               where scheduledSubject.MaMonHoc == subject.MaMonHoc
                                                               && scheduledSubject.aspnet_User.aspnet_Membership.SchoolId == school.SchoolId
                                                               select scheduledSubject;

            if (iqScheduledSubjects.Count() != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private List<DanhMuc_MonHoc> GetListSubjects(ref IQueryable<DanhMuc_MonHoc> iqSubject, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<DanhMuc_MonHoc> lSubjects = new List<DanhMuc_MonHoc>();

            totalRecords = iqSubject.Count();
            if (iqSubject.Count() != 0)
            {
                lSubjects = iqSubject.OrderBy(subj => subj.DanhMuc_NganhHoc.TenNganhHoc)
                    .ThenBy(subj => subj.DanhMuc_KhoiLop.TenKhoiLop)
                    .ThenBy(subj => subj.TenMonHoc)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return lSubjects;
        }
    }
}
