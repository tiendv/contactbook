using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using EContactBook.BusinessEntity;

namespace EContactBook.DataAccess
{
    public class SubjectDA : BaseDA
    {
        public SubjectDA(School_School school)
            : base(school)
        {
        }

        public void InsertSubject(Category_Subject subject)
        {
            db.Category_Subjects.InsertOnSubmit(subject);
            db.SubmitChanges();
        }

        public void UpdateSubject(Category_Subject editedSubject)
        {
            Category_Subject subject = null;
            IQueryable<Category_Subject> iqSubject = from subj in db.Category_Subjects
                                                     where subj.SubjectId == editedSubject.SubjectId
                                                     select subj;
            if (iqSubject.Count() != 0)
            {
                subject = iqSubject.First();
                subject.SubjectName = editedSubject.SubjectName;
                subject.MarkRatio = editedSubject.MarkRatio;
                db.SubmitChanges();
            }
        }

        public void DeleteSubject(Category_Subject deletedSubject)
        {
            Category_Subject subject = null;
            IQueryable<Category_Subject> iqSubject = from subj in db.Category_Subjects
                                                     where subj.SubjectId == deletedSubject.SubjectId
                                                     select subj;
            if (iqSubject.Count() != 0)
            {
                subject = iqSubject.First();
                db.Category_Subjects.DeleteOnSubmit(subject);
                db.SubmitChanges();
            }
        }

        public Category_Subject GetSubject(int subjectId)
        {
            Category_Subject subject = null;
            IQueryable<Category_Subject> iqSubject = from subj in db.Category_Subjects
                                                     where subj.SubjectId == subjectId
                                                     select subj;

            if (iqSubject.Count() != 0)
            {
                subject = iqSubject.First();
            }

            return subject;
        }

        public Category_Subject GetSubject(string subjectName, string facultyName, string gradeName)
        {
            Category_Subject subject = null;
            IQueryable<Category_Subject> iqSubject = from subj in db.Category_Subjects
                                                     where subj.SubjectName == subjectName
                                                          && subj.Category_Faculty.FacultyName == facultyName
                                                          && subj.Category_Grade.GradeName == gradeName
                                                          && subj.Category_Faculty.SchoolId == school.SchoolId
                                                     select subj;

            if (iqSubject.Count() != 0)
            {
                subject = iqSubject.First();
            }

            return subject;
        }

        public List<Category_Subject> GetListSubjects()
        {
            List<Category_Subject> subjects = new List<Category_Subject>();

            IQueryable<Category_Subject> iqSubject = from subj in db.Category_Subjects
                                                     where subj.Category_Faculty.SchoolId == school.SchoolId
                                                     select subj;
            if (iqSubject.Count() != 0)
            {
                subjects = iqSubject.OrderBy(subj => subj.SubjectName)
                    .ThenBy(subj => subj.Category_Faculty.FacultyName)
                    .ThenBy(subj => subj.Category_Grade.GradeName).ToList();
            }

            return subjects;
        }

        public List<Category_Subject> GetListSubjects(Category_Faculty faculty)
        {
            List<Category_Subject> subjects = new List<Category_Subject>();

            IQueryable<Category_Subject> iqSubject = from subj in db.Category_Subjects
                                                     where subj.FacultyId == faculty.FacultyId
                                                     select subj;
            if (iqSubject.Count() != 0)
            {
                subjects = iqSubject.OrderBy(subj => subj.SubjectName)
                    .ThenBy(subj => subj.Category_Faculty.FacultyName)
                    .ThenBy(subj => subj.Category_Grade.GradeName).ToList();
            }

            return subjects;
        }

        public List<Category_Subject> GetListSubjects(Category_Grade grade)
        {
            List<Category_Subject> subjects = new List<Category_Subject>();

            IQueryable<Category_Subject> iqSubject = from subj in db.Category_Subjects
                                                     where subj.GradeId == grade.GradeId
                                                     select subj;
            if (iqSubject.Count() != 0)
            {
                subjects = iqSubject.OrderBy(subj => subj.SubjectName)
                    .ThenBy(subj => subj.Category_Faculty.FacultyName)
                    .ThenBy(subj => subj.Category_Grade.GradeName).ToList();
            }

            return subjects;
        }

        public List<Category_Subject> GetListSubjects(Category_Faculty faculty, Category_Grade grade)
        {
            List<Category_Subject> subjects = new List<Category_Subject>();

            IQueryable<Category_Subject> iqSubject = from subj in db.Category_Subjects
                                                     where subj.FacultyId == faculty.FacultyId && subj.GradeId == grade.GradeId
                                                     select subj;
            if (iqSubject.Count() != 0)
            {
                subjects = iqSubject.OrderBy(subj => subj.SubjectName)
                    .ThenBy(subj => subj.Category_Faculty.FacultyName)
                    .ThenBy(subj => subj.Category_Grade.GradeName).ToList();
            }

            return subjects;
        }        
        
        public List<Category_Subject> GetListSubjects(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Category_Subject> iqSubject = from subj in db.Category_Subjects
                                                     where subj.Category_Faculty.SchoolId == school.SchoolId
                                                     select subj;

            return GetListSubjects(ref iqSubject, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Category_Subject> GetListSubjects(string subjectName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Category_Subject> iqSubject = from subj in db.Category_Subjects
                                                     where subj.SubjectName == subjectName
                                                     && subj.Category_Faculty.SchoolId == school.SchoolId
                                                     select subj;

            return GetListSubjects(ref iqSubject, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Category_Subject> GetListSubjects(Category_Faculty faculty, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Category_Subject> iqSubject = from subj in db.Category_Subjects
                                                     where subj.Category_Faculty.FacultyId == faculty.FacultyId
                                                     select subj;

            return GetListSubjects(ref iqSubject, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Category_Subject> GetListSubjects(Category_Grade grade, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Category_Subject> iqSubject = from subj in db.Category_Subjects
                                                     where subj.Category_Grade.GradeId == grade.GradeId
                                                     select subj;

            return GetListSubjects(ref iqSubject, pageCurrentIndex, pageSize, out totalRecords);
        }
        
        public List<Category_Subject> GetListSubjects(Category_Faculty faculty, Category_Grade grade, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Category_Subject> iqSubject = from subj in db.Category_Subjects
                                                     where subj.Category_Faculty.FacultyId == faculty.FacultyId
                                                     && subj.Category_Grade.GradeId == grade.GradeId
                                                     select subj;

            return GetListSubjects(ref iqSubject, pageCurrentIndex, pageSize, out totalRecords);
        }
        
        public List<Category_Subject> GetListSubjects(string subjectName, Category_Subject exceptedSubject, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Category_Subject> iqSubject = from subj in db.Category_Subjects
                                                     where subj.SubjectName == subjectName && subj.SubjectId != exceptedSubject.SubjectId
                                                     && subj.Category_Faculty.SchoolId == school.SchoolId
                                                     select subj;

            return GetListSubjects(ref iqSubject, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<Category_Subject> GetListSubjects(Category_Faculty faculty, Category_Grade grade, Category_Subject exceptedSubject, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<Category_Subject> iqSubject = from subj in db.Category_Subjects
                                                     where subj.Category_Faculty.FacultyId == faculty.FacultyId
                                                        && subj.Category_Grade.GradeId == grade.GradeId
                                                        && subj.SubjectId != exceptedSubject.SubjectId
                                                     select subj;

            return GetListSubjects(ref iqSubject, pageCurrentIndex, pageSize, out totalRecords);
        }
        
        public bool SubjectNameExists(string subjectName, string facultyName, string gradeName)
        {
            IQueryable<Category_Subject> iqSubject;
            iqSubject = from subject in db.Category_Subjects
                        where subject.SubjectName == subjectName
                          && subject.Category_Faculty.FacultyName == facultyName
                          && subject.Category_Grade.GradeName == gradeName
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

        private List<Category_Subject> GetListSubjects(ref IQueryable<Category_Subject> iqSubject, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<Category_Subject> subjects = new List<Category_Subject>();

            totalRecords = iqSubject.Count();
            if (iqSubject.Count() != 0)
            {
                subjects = iqSubject.OrderBy(subj => subj.Category_Faculty.FacultyName)
                    .ThenBy(subj => subj.Category_Grade.GradeName)
                    .ThenBy(subj => subj.SubjectName)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return subjects;
        }
    }
}
