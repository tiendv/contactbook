using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.BusinessEntity;

namespace EContactBook.DataAccess
{
    public class FacultyDA : BaseDA
    {
        public FacultyDA(School_School school)
            : base(school)
        {
        }

        public void InsertFaculty(Category_Faculty newFaculty)
        {
            newFaculty.SchoolId = school.SchoolId;
            db.Category_Faculties.InsertOnSubmit(newFaculty);
            db.SubmitChanges();
        }

        public void UpdateFaculty(Category_Faculty editedFaculty)
        {
            Category_Faculty faculty = null;

            IQueryable<Category_Faculty> iqFaculty = from fac in db.Category_Faculties
                                                     where fac.FacultyId == editedFaculty.FacultyId
                                                     select fac;

            if (iqFaculty.Count() != 0)
            {
                faculty = iqFaculty.First();
                faculty.FacultyName = editedFaculty.FacultyName;
                faculty.Description = editedFaculty.Description;
                db.SubmitChanges();
            }
        }

        public void DeleteFaculty(Category_Faculty deletedFaculty)
        {
            Category_Faculty faculty = null;

            IQueryable<Category_Faculty> iqFaculty = from fac in db.Category_Faculties
                                                     where fac.FacultyId == deletedFaculty.FacultyId
                                                     select fac;

            if (iqFaculty.Count() != 0)
            {
                faculty = iqFaculty.First();
                db.Category_Faculties.DeleteOnSubmit(faculty);
                db.SubmitChanges();
            }
        }

        public Category_Faculty GetFaculty(string facultyName)
        {
            Category_Faculty faculty = null;

            IQueryable<Category_Faculty> iqFaculty = from fac in db.Category_Faculties
                                                     where fac.FacultyName == facultyName
                                                        && fac.SchoolId == school.SchoolId
                                                     select fac;

            if (iqFaculty.Count() != 0)
            {
                faculty = iqFaculty.First();
            }

            return faculty;
        }

        public List<Category_Faculty> GetFaculties()
        {
            List<Category_Faculty> faculties = new List<Category_Faculty>();

            IQueryable<Category_Faculty> iqFaculty = from faculty in db.Category_Faculties
                                                     where faculty.SchoolId == school.SchoolId
                                                     select faculty;
            if (iqFaculty.Count() != 0)
            {
                faculties = iqFaculty.OrderBy(fac => fac.FacultyName).ToList();
            }

            return faculties;
        }

        public List<Category_Faculty> GetFaculties(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<Category_Faculty> faculties = new List<Category_Faculty>();

            IQueryable<Category_Faculty> iqFaculty = from faculty in db.Category_Faculties
                                                     where faculty.SchoolId == school.SchoolId
                                                     select faculty;
            totalRecords = iqFaculty.Count();
            if (totalRecords != 0)
            {
                faculties = iqFaculty.OrderBy(fac => fac.FacultyName)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return faculties;
        }

        public bool FacultyNameExists(string facultyName)
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

        public bool ClassInFacultyExists(Category_Faculty faculty)
        {
            IQueryable<Class_Class> iqClass = from cls in db.Class_Classes
                                              where cls.Category_Faculty.FacultyName == faculty.FacultyName
                                              && cls.Category_Faculty.SchoolId == school.SchoolId
                                              select cls;

            if (iqClass.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SubjectInFacultyExists(Category_Faculty faculty)
        {
            IQueryable<Category_Subject> iqSubject = from subject in db.Category_Subjects
                                                     where subject.Category_Faculty.FacultyName == faculty.FacultyName
                                                     && subject.Category_Faculty.SchoolId == school.SchoolId
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

        public Category_Faculty GetFaculty(int facultyId)
        {
            Category_Faculty faculty = null;

            IQueryable<Category_Faculty> iqFaculty = from fac in db.Category_Faculties
                                                     where fac.FacultyId == facultyId
                                                     select fac;

            if (iqFaculty.Count() != 0)
            {
                faculty = iqFaculty.First();
            }

            return faculty;
        }
    }
}