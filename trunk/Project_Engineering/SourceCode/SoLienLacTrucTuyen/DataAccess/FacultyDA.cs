using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class FacultyDA : BaseDA
    {
        public FacultyDA(School school)
            : base(school)
        {
        }

        public void InsertFaculty(DanhMuc_NganhHoc faculty)
        {
            db.DanhMuc_NganhHocs.InsertOnSubmit(faculty);
            db.SubmitChanges();
        }

        public void UpdateFaculty(DanhMuc_NganhHoc editedFaculty)
        {
            IQueryable<DanhMuc_NganhHoc> iqFaculty;
            iqFaculty = from fac in db.DanhMuc_NganhHocs
                        where fac.MaNganhHoc == editedFaculty.MaNganhHoc
                        && fac.SchoolId == school.SchoolId
                        select fac;

            if (iqFaculty.Count() != 0)
            {
                DanhMuc_NganhHoc faculty = iqFaculty.First();
                faculty.TenNganhHoc = editedFaculty.TenNganhHoc;
                faculty.MoTa = editedFaculty.MoTa;
                db.SubmitChanges();
            }
        }

        public void DeleteFaculty(DanhMuc_NganhHoc deletedFaculty)
        {
            IQueryable<DanhMuc_NganhHoc> iqFaculty;
            iqFaculty = from fac in db.DanhMuc_NganhHocs
                        where fac.MaNganhHoc == deletedFaculty.MaNganhHoc
                        select fac;

            if (iqFaculty.Count() != 0)
            {
                DanhMuc_NganhHoc faculty = iqFaculty.First();
                db.DanhMuc_NganhHocs.DeleteOnSubmit(faculty);
                db.SubmitChanges();
            }
        }

        public DanhMuc_NganhHoc GetFaculty(string facultyName)
        {
            DanhMuc_NganhHoc faculty = null;

            IQueryable<DanhMuc_NganhHoc> iqFaculty;
            iqFaculty = from fac in db.DanhMuc_NganhHocs
                        where fac.TenNganhHoc == facultyName && fac.SchoolId == school.SchoolId
                        select fac;

            if (iqFaculty.Count() != 0)
            {
                faculty = iqFaculty.First();
            }

            return faculty;
        }

        public List<DanhMuc_NganhHoc> GetFaculties()
        {
            IQueryable<DanhMuc_NganhHoc> iqFaculties = from faculty in db.DanhMuc_NganhHocs
                                                       select faculty;
            if (iqFaculties.Count() != 0)
            {
                return iqFaculties.OrderBy(fac => fac.TenNganhHoc).ToList();
            }
            else
            {
                return new List<DanhMuc_NganhHoc>();
            }
        }

        public List<DanhMuc_NganhHoc> GetFaculties(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<DanhMuc_NganhHoc> iqFaculty = from faculty in db.DanhMuc_NganhHocs
                                                     where faculty.SchoolId == school.SchoolId
                                                     select faculty;
            totalRecords = iqFaculty.Count();
            if (totalRecords != 0)
            {
                return iqFaculty.OrderBy(fac => fac.TenNganhHoc)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize)
                    .ToList();
            }
            else
            {
                return new List<DanhMuc_NganhHoc>();
            }
        }

        public bool FacultyExists(string facultyName)
        {
            IQueryable<DanhMuc_NganhHoc> iqFaculty = from faculty in db.DanhMuc_NganhHocs
                                                     where faculty.TenNganhHoc == facultyName
                                                     select faculty;
            if (iqFaculty.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsDeletable(DanhMuc_NganhHoc faculty)
        {
            IQueryable<LopHoc_Lop> iqClass = from cls in db.LopHoc_Lops
                                             join fac in db.DanhMuc_NganhHocs on cls.MaNganhHoc equals fac.MaNganhHoc
                                             where fac.TenNganhHoc == faculty.TenNganhHoc
                                             select cls;

            if (iqClass.Count() != 0)
            {
                return false;
            }
            else
            {
                IQueryable<DanhMuc_MonHoc> iqSubject = from subject in db.DanhMuc_MonHocs
                                                       join fac in db.DanhMuc_NganhHocs on subject.MaNganhHoc equals fac.MaNganhHoc
                                                       where fac.TenNganhHoc == faculty.TenNganhHoc
                                                       select subject;

                if (iqSubject.Count() != 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}