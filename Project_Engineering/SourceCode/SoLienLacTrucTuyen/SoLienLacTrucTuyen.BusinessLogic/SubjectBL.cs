using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class SubjectBL : BaseBL
    {
        private SubjectDA subjectDA;

        public SubjectBL(School_School school)
            : base(school)
        {
            subjectDA = new SubjectDA(school);
        }

        public void InsertSubject(string subjectName, Category_Grade grade, Category_Faculty faculty, double markRatio)
        {
            subjectDA.InsertSubject(new Category_Subject()
            {
                SubjectName = subjectName,
                FacultyId = faculty.FacultyId,
                GradeId = grade.GradeId,
                MarkRatio = markRatio
            });
        }

        public void UpdateSubject(Category_Subject editedSubject, string newSubjectName, double newMarkRatio)
        {
            editedSubject.SubjectName = newSubjectName;
            editedSubject.MarkRatio = newMarkRatio;
            subjectDA.UpdateSubject(editedSubject);
        }

        public void DeleteSubject(Category_Subject subject)
        {
            subjectDA.DeleteSubject(subject);
        }

        public Category_Subject GetSubject(int subjectId)
        {
            return subjectDA.GetSubject(subjectId);
        }

        public Category_Subject GetSubject(string subjectName, string facultyName, string gradeName)
        {
            return subjectDA.GetSubject(subjectName, facultyName, gradeName);
        }

        public List<Category_Subject> GetListSubjects(Category_Faculty faculty, Category_Grade grade)
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

        public TabularSubject GetTabularSubject(Category_Subject subject)
        {
            TabularSubject tabularSubject = new TabularSubject();
            tabularSubject.SubjectId = subject.SubjectId;
            tabularSubject.SubjectName = subject.SubjectName;
            tabularSubject.MarkRatio = subject.MarkRatio;
            tabularSubject.FacultyName = subject.Category_Faculty.FacultyName;
            tabularSubject.GradeName = subject.Category_Grade.GradeName;

            return tabularSubject;
        }

        public List<TabularSubject> GetListTabularSubjects(Category_Faculty faculty, Category_Grade grade, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularSubject> lTabularSubjects = new List<TabularSubject>();
            List<Category_Subject> lSubjects = new List<Category_Subject>();

            if (faculty == null)
            {
                if (grade == null)
                {
                    lSubjects = subjectDA.GetListSubjects(pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    lSubjects = subjectDA.GetListSubjects(grade, pageCurrentIndex, pageSize, out totalRecords);
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
                foreach (Category_Subject subject in lSubjects)
                {
                    lTabularSubjects.Add(GetTabularSubject(subject));
                }
            }

            return lTabularSubjects;
        }

        public List<TabularSubject> GetListTabularSubjects(Category_Faculty faculty, Category_Grade grade, string subjectName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularSubject> lTabularSubjects = new List<TabularSubject>();

            if ((subjectName == "") || (string.Compare(subjectName, "tất cả", true) == 0))
            {
                lTabularSubjects = GetListTabularSubjects(faculty, grade, pageCurrentIndex, pageSize, out totalRecords);
            }
            else
            {
                List<Category_Subject> lSubjects = subjectDA.GetListSubjects(subjectName, pageCurrentIndex, pageSize, out totalRecords);
                if (lSubjects.Count != 0)
                {
                    foreach (Category_Subject subject in lSubjects)
                    {
                        lTabularSubjects.Add(GetTabularSubject(subject));
                    }
                }
            }

            return lTabularSubjects;
        }

        public List<TabularSubject> GetListTabularSubjects(Category_Faculty faculty, Category_Grade grade, string subjectName, Category_Subject exceptedSubject, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularSubject> lTabularSubjects = new List<TabularSubject>();
            List<Category_Subject> lSubjects = new List<Category_Subject>();

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
                foreach (Category_Subject subject in lSubjects)
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

        public bool SubjectNameExists(Category_Subject editedSubject, string newSubjectName)
        {
            if (editedSubject.SubjectName == newSubjectName)
            {
                return false;
            }
            else
            {
                return subjectDA.SubjectNameExists(newSubjectName,
                    editedSubject.Category_Faculty.FacultyName,
                    editedSubject.Category_Grade.GradeName);
            }
        }

        public bool IsDeletable(Category_Subject subject)
        {
            ScheduleBL scheduleBL = new ScheduleBL(school);
            return !scheduleBL.ScheduleExists(subject);
        }
    }
}
