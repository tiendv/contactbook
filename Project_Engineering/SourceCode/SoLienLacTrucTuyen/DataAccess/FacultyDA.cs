using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class FacultyDA : BaseDA
    {
        public FacultyDA(School_School school)
            : base(school)
        {
        }

        public void InsertFaculty(Category_Faculty faculty)
        {
            faculty.SchoolId = school.SchoolId;
            db.Category_Faculties.InsertOnSubmit(faculty);
            db.SubmitChanges();
        }

        public void UpdateFaculty(Category_Faculty editedFaculty)
        {
            IQueryable<Category_Faculty> iqFaculty;
            iqFaculty = from fac in db.Category_Faculties
                        where fac.FacultyId == editedFaculty.FacultyId
                        && fac.SchoolId == school.SchoolId
                        select fac;

            if (iqFaculty.Count() != 0)
            {
                Category_Faculty faculty = iqFaculty.First();
                faculty.FacultyName = editedFaculty.FacultyName;
                faculty.Description = editedFaculty.Description;
                db.SubmitChanges();
            }
        }

        public void DeleteFaculty(Category_Faculty deletedFaculty)
        {
            IQueryable<Category_Faculty> iqFaculty;
            iqFaculty = from fac in db.Category_Faculties
                        where fac.FacultyId == deletedFaculty.FacultyId
                        && fac.SchoolId == school.SchoolId
                        select fac;

            if (iqFaculty.Count() != 0)
            {
                Category_Faculty faculty = iqFaculty.First();
                db.Category_Faculties.DeleteOnSubmit(faculty);
                db.SubmitChanges();
            }
        }

        public Category_Faculty GetFaculty(string facultyName)
        {
            Category_Faculty faculty = null;

            IQueryable<Category_Faculty> iqFaculty;
            iqFaculty = from fac in db.Category_Faculties
                        where fac.FacultyName == facultyName && fac.SchoolId == school.SchoolId
                        select fac;

            if (iqFaculty.Count() != 0)
            {
                faculty = iqFaculty.First();
            }

            return faculty;
        }

        public List<Category_Faculty> GetFaculties()
        {
            IQueryable<Category_Faculty> iqFaculty = from faculty in db.Category_Faculties
                                                     where faculty.SchoolId == school.SchoolId
                                                     select faculty;
            if (iqFaculty.Count() != 0)
            {
                return iqFaculty.OrderBy(fac => fac.FacultyName).ToList();
            }
            else
            {
                return new List<Category_Faculty>();
            }
        }

        public List<Category_Faculty> GetFaculties(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Category_Faculty> iqFaculty = from faculty in db.Category_Faculties
                                                     where faculty.SchoolId == school.SchoolId
                                                     select faculty;
            totalRecords = iqFaculty.Count();
            if (totalRecords != 0)
            {
                return iqFaculty.OrderBy(fac => fac.FacultyName)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize)
                    .ToList();
            }
            else
            {
                return new List<Category_Faculty>();
            }
        }

        public bool FacultyExists(string facultyName)
        {
            IQueryable<Category_Faculty> iqFaculty = from faculty in db.Category_Faculties
                                                     where faculty.FacultyName == facultyName
                                                     && faculty.SchoolId == school.SchoolId
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

        public bool IsDeletable(Category_Faculty faculty)
        {
            IQueryable<Class_Class> iqClass = from cls in db.Class_Classes
                                             join fac in db.Category_Faculties on cls.FacultyId equals fac.FacultyId
                                             where fac.FacultyName == faculty.FacultyName
                                             && fac.SchoolId == school.SchoolId
                                             select cls;

            if (iqClass.Count() != 0)
            {
                return false;
            }
            else
            {
                IQueryable<Category_Subject> iqSubject = from subject in db.Category_Subjects
                                                       join fac in db.Category_Faculties on subject.FacultyId equals fac.FacultyId
                                                       where fac.FacultyName == faculty.FacultyName
                                                       && fac.SchoolId == school.SchoolId
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