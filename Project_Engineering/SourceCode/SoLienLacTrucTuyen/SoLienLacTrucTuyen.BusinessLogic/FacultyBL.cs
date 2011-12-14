using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class FacultyBL: BaseBL
    {
        FacultyDA facultyDA;

        public FacultyBL(School_School school)
            : base(school)
        {
            facultyDA = new FacultyDA(school);
        }

        public void InsertFaculty(Category_Faculty faculty)
        {
            facultyDA.InsertFaculty(faculty);
        }

        public void UpdateFaculty(string editedFacultyName, string newFacultyName, string newDescription)
        {
            Category_Faculty faculty = GetFaculty(editedFacultyName);
            faculty.FacultyName = newFacultyName;
            faculty.Description = newDescription;

            facultyDA.UpdateFaculty(faculty);
        }

        public void DeleteFaculty(Category_Faculty faculty)
        {
            facultyDA.DeleteFaculty(faculty);
        }

        public Category_Faculty GetFaculty(int facultyId)
        {            
            return facultyDA.GetFaculty(facultyId);
        }

        public Category_Faculty GetFaculty(string facultyName)
        {
            if ((facultyName == "") || (string.Compare(facultyName, "tất cả", true) == 0))
            {
                return null;
            }
            else
            {
                return facultyDA.GetFaculty(facultyName);
            }
        }

        public List<Category_Faculty> GetFaculties()
        {
            return facultyDA.GetFaculties();
        }

        public List<Category_Faculty> GetFaculties(string facultyName, int pageIndex, int pageSize, out double totalRecords)
        {
            List<Category_Faculty> lFaculties = new List<Category_Faculty>();

            if (String.Compare(facultyName, "tất cả", true) == 0 || facultyName == "")
            {
                lFaculties = facultyDA.GetFaculties(pageIndex, pageSize, out totalRecords);
            }
            else
            {
                Category_Faculty faculty = GetFaculty(facultyName);
                lFaculties.Add(faculty);
                totalRecords = 1;
            }

            return lFaculties;            
        }

        public bool FacultyExists(string oldFacultyName, string newFacultyName)
        {
            if (oldFacultyName ==  newFacultyName)
            {
                return false;
            }
            else
            {
                return facultyDA.FacultyNameExists(newFacultyName);
            }
        }

        public bool FacultyExists(string facultyName)
        {
            return facultyDA.FacultyNameExists(facultyName);
        }

        public bool IsDeletable(Category_Faculty faculty)
        {
            return ((!facultyDA.SubjectInFacultyExists(faculty)) && (!facultyDA.ClassInFacultyExists(faculty)));
        }
    }
}
