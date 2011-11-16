using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class FacultyBL: BaseBL
    {
        FacultyDA facultyDA;

        public FacultyBL(School school)
            : base(school)
        {
            facultyDA = new FacultyDA(school);
        }

        public void InsertFaculty(DanhMuc_NganhHoc faculty)
        {
            facultyDA.InsertFaculty(faculty);
        }

        public void UpdateFaculty(string editedFacultyName, string newFacultyName, string newDescription)
        {
            DanhMuc_NganhHoc faculty = GetFaculty(editedFacultyName);
            faculty.TenNganhHoc = newFacultyName;
            faculty.MoTa = newDescription;

            facultyDA.UpdateFaculty(faculty);
        }

        public void DeleteFaculty(DanhMuc_NganhHoc faculty)
        {
            facultyDA.DeleteFaculty(faculty);
        }

        public DanhMuc_NganhHoc GetFaculty(string facultyName)
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

        public List<DanhMuc_NganhHoc> GetFaculties()
        {
            return facultyDA.GetFaculties();
        }

        public List<DanhMuc_NganhHoc> GetFaculties(string facultyName, int pageIndex, int pageSize, out double totalRecords)
        {
            List<DanhMuc_NganhHoc> lFaculties = new List<DanhMuc_NganhHoc>();

            if (String.Compare(facultyName, "tất cả", true) == 0 || facultyName == "")
            {
                lFaculties = facultyDA.GetFaculties(pageIndex, pageSize, out totalRecords);
            }
            else
            {
                DanhMuc_NganhHoc faculty = GetFaculty(facultyName);
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
                return facultyDA.FacultyExists(newFacultyName);
            }
        }

        public bool FacultyExists(string facultyName)
        {
            return facultyDA.FacultyExists(facultyName);
        }

        public bool IsDeletable(DanhMuc_NganhHoc faculty)
        {
            return facultyDA.IsDeletable(faculty);
        }
    }
}
