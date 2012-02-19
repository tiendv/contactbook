using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.BusinessEntity;

namespace EContactBook.DataAccess
{
    public class StudyingResultDA : BaseDA
    {
        public StudyingResultDA(School_School school)
            : base(school)
        {
        }

        public void InsertTermStudyingResult(Student_TermLearningResult termStudyingResult)
        {
            db.Student_TermLearningResults.InsertOnSubmit(termStudyingResult);
            db.SubmitChanges();
        }

        public void InsertDetailedMark(Student_Student student, Class_Class Class, Configuration_Term term, Category_Subject subject, Category_MarkType markType, double mark)
        {
            Student_TermSubjectMark termSubjectedMark = null;
            Student_DetailedTermSubjectMark detailedMark = null;

            IQueryable<Student_TermSubjectMark> iqTermSubjectedMark;
            iqTermSubjectedMark = from termSubjMark in db.Student_TermSubjectMarks
                                  where termSubjMark.Student_StudentInClass.StudentId == student.StudentId
                                    && termSubjMark.Student_StudentInClass.ClassId == Class.ClassId
                                    && termSubjMark.TermId == term.TermId && termSubjMark.SubjectId == subject.SubjectId
                                  select termSubjMark;

            if (iqTermSubjectedMark.Count() != 0)
            {
                termSubjectedMark = iqTermSubjectedMark.First();
                detailedMark = new Student_DetailedTermSubjectMark();
                detailedMark.TermSubjectMarkId = termSubjectedMark.TermSubjectMarkId;
                detailedMark.MarkType = markType.MarkTypeId;
                detailedMark.MarkValue = mark;

                db.Student_DetailedTermSubjectMarks.InsertOnSubmit(detailedMark);
                db.SubmitChanges();
            }
        }

        /// <summary>
        /// Insert student's mark againts class, term, subject and marks (contains mark type and value)
        /// </summary>
        /// <param name="student"></param>
        /// <param name="Class"></param>
        /// <param name="term"></param>
        /// <param name="subject"></param>
        /// <param name="marks"></param>
        public void InsertDetailedMark(Student_Student student, Class_Class Class, Configuration_Term term, Category_Subject subject, List<MarkValueAndTypePair> marks)
        {
            Student_TermSubjectMark termSubjectedMark = null;
            Student_DetailedTermSubjectMark detailedMark = null;

            IQueryable<Student_TermSubjectMark> iqTermSubjectedMark;
            iqTermSubjectedMark = from termSubjMark in db.Student_TermSubjectMarks
                                  where termSubjMark.Student_StudentInClass.StudentId == student.StudentId
                                    && termSubjMark.Student_StudentInClass.ClassId == Class.ClassId
                                    && termSubjMark.TermId == term.TermId && termSubjMark.SubjectId == subject.SubjectId
                                  select termSubjMark;

            if (iqTermSubjectedMark.Count() != 0)
            {
                termSubjectedMark = iqTermSubjectedMark.First();
                if (marks.Count != 0)
                {
                    foreach (MarkValueAndTypePair mark in marks)
                    {
                        detailedMark = new Student_DetailedTermSubjectMark();
                        detailedMark.TermSubjectMarkId = termSubjectedMark.TermSubjectMarkId;
                        detailedMark.MarkType = mark.MarkTypeId;
                        detailedMark.MarkValue = mark.GiaTri;
                        db.Student_DetailedTermSubjectMarks.InsertOnSubmit(detailedMark);
                    }

                    db.SubmitChanges();
                }
            }
        }

        /// <summary>
        /// Insert student's mark againts class, term, subject and marks (contains mark type and value)
        /// </summary>
        /// <param name="student"></param>
        /// <param name="Class"></param>
        /// <param name="term"></param>
        /// <param name="subject"></param>
        /// <param name="marks"></param>
        public void InsertDetailedMark(Student_Student student, Class_Class Class, Configuration_Term term, Category_Subject subject, List<MarkValueAndTypePair> marks, DateTime date)
        {
            Student_TermSubjectMark termSubjectedMark = null;
            Student_DetailedTermSubjectMark detailedMark = null;

            IQueryable<Student_TermSubjectMark> iqTermSubjectedMark;
            iqTermSubjectedMark = from termSubjMark in db.Student_TermSubjectMarks
                                  where termSubjMark.Student_StudentInClass.StudentId == student.StudentId
                                    && termSubjMark.Student_StudentInClass.ClassId == Class.ClassId
                                    && termSubjMark.TermId == term.TermId && termSubjMark.SubjectId == subject.SubjectId
                                  select termSubjMark;

            if (iqTermSubjectedMark.Count() != 0)
            {
                termSubjectedMark = iqTermSubjectedMark.First();
                if (marks.Count != 0)
                {
                    foreach (MarkValueAndTypePair mark in marks)
                    {
                        detailedMark = new Student_DetailedTermSubjectMark();
                        detailedMark.TermSubjectMarkId = termSubjectedMark.TermSubjectMarkId;
                        detailedMark.MarkType = mark.MarkTypeId;
                        detailedMark.MarkValue = mark.GiaTri;
                        detailedMark.Date = date;
                        db.Student_DetailedTermSubjectMarks.InsertOnSubmit(detailedMark);

                    }
                    db.SubmitChanges();
                }
            }
        }

        public void InsertDetailMark(Student_DetailedTermSubjectMark detailedMark)
        {
            db.Student_DetailedTermSubjectMarks.InsertOnSubmit(detailedMark);
            db.SubmitChanges();
        }

        public void InsertTermSubjectMark(Class_Class Class, Category_Subject subject, Configuration_Term term)
        {
            IQueryable<Student_StudentInClass> iqStudentsInClass = from stdsInCls in db.Student_StudentInClasses
                                                                   where stdsInCls.ClassId == Class.ClassId
                                                                   select stdsInCls;

            Student_TermSubjectMark studentTermSubjectMark = null;
            foreach (Student_StudentInClass studentInClass in iqStudentsInClass)
            {
                studentTermSubjectMark = new Student_TermSubjectMark();
                studentTermSubjectMark.StudentInClassId = studentInClass.StudentInClassId;
                studentTermSubjectMark.SubjectId = subject.SubjectId;
                studentTermSubjectMark.TermId = term.TermId;
                studentTermSubjectMark.AverageMark = -1;

                db.Student_TermSubjectMarks.InsertOnSubmit(studentTermSubjectMark);
            }

            db.SubmitChanges();
        }

        public void InsertTermSubjectMark(Student_TermSubjectMark termSubjectMark)
        {
            db.Student_TermSubjectMarks.InsertOnSubmit(termSubjectMark);
            db.SubmitChanges();
        }

        public void UpdateDetailedMark(Student_DetailedTermSubjectMark editedDetailedMark)
        {
            Student_DetailedTermSubjectMark detailedMark = null;
            IQueryable<Student_DetailedTermSubjectMark> iqDetailedMark = from dtlMark in db.Student_DetailedTermSubjectMarks
                                                                         where dtlMark.DetailedTermSubjectMark == editedDetailedMark.DetailedTermSubjectMark
                                                                         select dtlMark;
            if (iqDetailedMark.Count() != 0)
            {
                detailedMark = iqDetailedMark.First();
                detailedMark.MarkValue = editedDetailedMark.MarkValue;
                db.SubmitChanges();
            }
        }

        public void UpdateStudentTermResult(Student_TermLearningResult editedTermResult)
        {
            Student_TermLearningResult termResult = null;
            IQueryable<Student_TermLearningResult> iqTermResult = from trmRes in db.Student_TermLearningResults
                                                                  where trmRes.TermLearningResultId == termResult.TermLearningResultId
                                                                  select trmRes;
            if (iqTermResult.Count() != 0)
            {
                termResult = iqTermResult.First();
                termResult.TermConductId = editedTermResult.TermConductId;
                db.SubmitChanges();
            }
        }

        /// <summary>
        /// Check whether reset average mark
        /// </summary>
        /// <param name="student"></param>
        /// <param name="Class"></param>
        /// <param name="term"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        public bool NeedResetAvgMark(Student_Student student, Class_Class Class, Configuration_Term term, Category_Subject subject)
        {
            bool bResult = true;
            List<Student_DetailedTermSubjectMark> detailedMarks;
            IQueryable<Student_DetailedTermSubjectMark> queryDetailedMark = from detail in db.Student_DetailedTermSubjectMarks
                                                                            where detail.Student_TermSubjectMark.Student_StudentInClass.StudentId == student.StudentId
                                                                              && detail.Student_TermSubjectMark.Student_StudentInClass.ClassId == Class.ClassId
                                                                              && detail.Student_TermSubjectMark.TermId == term.TermId
                                                                              && detail.Student_TermSubjectMark.SubjectId == subject.SubjectId
                                                                              && detail.Approved == true
                                                                            select detail;

            if (queryDetailedMark.Count() != 0)
            {
                detailedMarks = queryDetailedMark.ToList();
                int length = detailedMarks.Count;
                for (int i = 0; i < length; i++)
                {
                    if (detailedMarks[i].Category_MarkType.IsUsedForCalculatingAvg)
                    {
                        bResult = false;
                        break;
                    }
                }
            }

            return bResult;
        }

        public void CalculateTermSubjectAvgMark(Student_Student student, Class_Class Class, Configuration_Term term, Category_Subject subject)
        {
            double dAvgMark = -1;
            Student_TermSubjectMark termSubjectedMark = null;

            IQueryable<Student_TermSubjectMark> iqTermSubjectedMark;
            iqTermSubjectedMark = from termSubjMark in db.Student_TermSubjectMarks
                                  where termSubjMark.Student_StudentInClass.StudentId == student.StudentId
                                    && termSubjMark.Student_StudentInClass.ClassId == Class.ClassId
                                    && termSubjMark.TermId == term.TermId && termSubjMark.SubjectId == subject.SubjectId
                                  select termSubjMark;

            if (iqTermSubjectedMark.Count() != 0)
            {
                termSubjectedMark = iqTermSubjectedMark.First();
                IQueryable<Student_DetailedTermSubjectMark> iqDetailedMark;
                iqDetailedMark = from dtlMark in db.Student_DetailedTermSubjectMarks
                                 where dtlMark.TermSubjectMarkId == termSubjectedMark.TermSubjectMarkId
                                 select dtlMark;
                if (iqDetailedMark.Count() != 0)
                {
                    double dTotalMarkRatioLoai = 0;
                    double dTotalMarks = 0;
                    foreach (Student_DetailedTermSubjectMark detailedMark in iqDetailedMark)
                    {
                        dTotalMarkRatioLoai += detailedMark.Category_MarkType.MarkRatio;
                        dTotalMarks += detailedMark.MarkValue * detailedMark.Category_MarkType.MarkRatio;
                    }

                    dAvgMark = dTotalMarks / dTotalMarkRatioLoai;
                    dAvgMark = Math.Round(dAvgMark, 1, MidpointRounding.AwayFromZero);
                }
            }

            termSubjectedMark.AverageMark = dAvgMark;
            db.SubmitChanges();
        }

        public void ResetAvgMark(Student_Student student, Class_Class Class, Configuration_Term term, Category_Subject subject)
        {
            Student_TermSubjectMark termSubjectedMark = null;

            IQueryable<Student_TermSubjectMark> iqTermSubjectedMark;
            iqTermSubjectedMark = from termSubjMark in db.Student_TermSubjectMarks
                                  where termSubjMark.Student_StudentInClass.StudentId == student.StudentId
                                    && termSubjMark.Student_StudentInClass.ClassId == Class.ClassId
                                    && termSubjMark.TermId == term.TermId && termSubjMark.SubjectId == subject.SubjectId
                                  select termSubjMark;

            if (iqTermSubjectedMark.Count() != 0)
            {
                termSubjectedMark = iqTermSubjectedMark.First();
                termSubjectedMark.AverageMark = -1;
                db.SubmitChanges();
            }
        }

        public void DeleteDetailedMark(Student_DetailedTermSubjectMark deletedDetailedMark)
        {
            Student_DetailedTermSubjectMark detailedMark = null;
            IQueryable<Student_DetailedTermSubjectMark> iqDetailedMark = from dtlMark in db.Student_DetailedTermSubjectMarks
                                                                         where dtlMark.DetailedTermSubjectMark == deletedDetailedMark.DetailedTermSubjectMark
                                                                         select dtlMark;
            if (iqDetailedMark.Count() != 0)
            {
                detailedMark = iqDetailedMark.First();

                Student_TermSubjectMark termSubjectedMark = null;
                IQueryable<Student_TermSubjectMark> iqTermSubjectedMark;
                iqTermSubjectedMark = from termSubjMark in db.Student_TermSubjectMarks
                                      where termSubjMark.TermSubjectMarkId == deletedDetailedMark.TermSubjectMarkId
                                      select termSubjMark;
                if (iqTermSubjectedMark.Count() != 0)
                {
                    termSubjectedMark = iqTermSubjectedMark.First();
                }

                db.Student_DetailedTermSubjectMarks.DeleteOnSubmit(detailedMark);
                db.SubmitChanges();
            }
        }

        /// <summary>
        /// Delete student's mark against class, term, subject and mark types
        /// </summary>
        /// <param name="student"></param>
        /// <param name="Class"></param>
        /// <param name="term"></param>
        /// <param name="subject"></param>
        /// <param name="markTypes"></param>
        public void DeleteDetailedMark(Student_Student student, Class_Class Class, Configuration_Term term, Category_Subject subject, List<Category_MarkType> markTypes)
        {
            Student_TermSubjectMark termSubjectedMark = null;

            IQueryable<Student_TermSubjectMark> iqTermSubjectedMark;
            iqTermSubjectedMark = from termSubjMark in db.Student_TermSubjectMarks
                                  where termSubjMark.Student_StudentInClass.StudentId == student.StudentId
                                    && termSubjMark.Student_StudentInClass.ClassId == Class.ClassId
                                    && termSubjMark.TermId == term.TermId && termSubjMark.SubjectId == subject.SubjectId
                                  select termSubjMark;

            if (iqTermSubjectedMark.Count() != 0)
            {
                termSubjectedMark = iqTermSubjectedMark.First();
                foreach (Category_MarkType markType in markTypes)
                {
                    IQueryable<Student_DetailedTermSubjectMark> iqDetailedMark;
                    iqDetailedMark = from mark in db.Student_DetailedTermSubjectMarks
                                     where mark.TermSubjectMarkId == termSubjectedMark.TermSubjectMarkId && mark.MarkType == markType.MarkTypeId
                                     select mark;
                    if (iqDetailedMark.Count() != 0)
                    {
                        foreach (Student_DetailedTermSubjectMark detailedMark in iqDetailedMark)
                        {
                            // Xoa diem hoc sinh theo MarkTypeId
                            db.Student_DetailedTermSubjectMarks.DeleteOnSubmit(detailedMark);
                        }
                        db.SubmitChanges();
                    }

                    // re calculate avg mark
                    if (markType.IsUsedForCalculatingAvg)
                    {
                        termSubjectedMark.AverageMark = -1;
                        db.SubmitChanges();
                    }
                }
            }
        }

        public void DeleteTermSubjectMark(Class_Class Class, Configuration_Term term, Category_Subject subject)
        {
            IQueryable<Student_TermSubjectMark> iqTermSubjectedMark;
            iqTermSubjectedMark = from termSubjMark in db.Student_TermSubjectMarks
                                  where termSubjMark.Student_StudentInClass.ClassId == Class.ClassId
                                    && termSubjMark.TermId == term.TermId
                                    && termSubjMark.SubjectId == subject.SubjectId
                                  select termSubjMark;

            if (iqTermSubjectedMark.Count() != 0)
            {
                foreach (Student_TermSubjectMark termSubjectedMark in iqTermSubjectedMark)
                {
                    IQueryable<Student_DetailedTermSubjectMark> iqDetailedMark;
                    iqDetailedMark = from mark in db.Student_DetailedTermSubjectMarks
                                     where mark.TermSubjectMarkId == termSubjectedMark.TermSubjectMarkId
                                     select mark;
                    if (iqDetailedMark.Count() != 0)
                    {
                        foreach (Student_DetailedTermSubjectMark detailedMark in iqDetailedMark)
                        {
                            db.Student_DetailedTermSubjectMarks.DeleteOnSubmit(detailedMark);
                        }
                        db.SubmitChanges();
                    }

                    db.SubmitChanges();
                }
            }
        }

        public void DeleteDetailedMarks(Student_TermSubjectMark termSubjectedMark, Category_MarkType markType)
        {
            IQueryable<Student_DetailedTermSubjectMark> iqDetailedMark = from dtlMark in db.Student_DetailedTermSubjectMarks
                                                                         where dtlMark.TermSubjectMarkId == termSubjectedMark.TermSubjectMarkId
                                                                            && dtlMark.MarkType == markType.MarkTypeId
                                                                         select dtlMark;
            if (iqDetailedMark.Count() != 0)
            {
                foreach (Student_DetailedTermSubjectMark detailedMark in iqDetailedMark)
                {
                    db.Student_DetailedTermSubjectMarks.DeleteOnSubmit(detailedMark);
                }
                db.SubmitChanges();
            }
        }

        public void DeleteDetailedMarks(Student_TermSubjectMark termSubjectedMark)
        {
            List<Student_DetailedTermSubjectMark> detailedMarks;
            IQueryable<Student_DetailedTermSubjectMark> iqDetailedMark;
            iqDetailedMark = from mark in db.Student_DetailedTermSubjectMarks
                             where mark.TermSubjectMarkId == termSubjectedMark.TermSubjectMarkId
                             select mark;
            if (iqDetailedMark.Count() != 0)
            {
                detailedMarks = iqDetailedMark.ToList();

                foreach (Student_DetailedTermSubjectMark detailedMark in detailedMarks)
                {
                    db.Student_DetailedTermSubjectMarks.DeleteOnSubmit(detailedMark);
                }
                db.SubmitChanges();
            }
        }

        public void DeleteTermSubjectMarks(Class_Schedule schedule)
        {
            IQueryable<Student_TermSubjectMark> iqTermSubjectMark = from termSubjectMark in db.Student_TermSubjectMarks
                                                                    join schd in db.Class_Schedules on termSubjectMark.SubjectId equals schedule.SubjectId
                                                                    where termSubjectMark.SubjectId == schedule.SubjectId
                                                                    && schd.ScheduleId == schedule.ScheduleId
                                                                    select termSubjectMark;
            if (iqTermSubjectMark.Count() != 0)
            {
                foreach (Student_TermSubjectMark termSubjectMark in iqTermSubjectMark)
                {
                    DeleteDetailedMarks(termSubjectMark);
                    db.Student_TermSubjectMarks.DeleteOnSubmit(termSubjectMark);
                    db.SubmitChanges();
                }
            }
        }

        /// <summary>
        /// Delete all learing result of specified student
        /// </summary>
        /// <param name="student"></param>
        public void DeleteStudyingResult(Student_Student student)
        {
            IQueryable<Student_DetailedTermSubjectMark> iqDetailedTermSubjectMark;
            iqDetailedTermSubjectMark = from detailedTermSubjectMark in db.Student_DetailedTermSubjectMarks
                                        where detailedTermSubjectMark.Student_TermSubjectMark.Student_StudentInClass.StudentId == student.StudentId
                                        select detailedTermSubjectMark;
            if (iqDetailedTermSubjectMark.Count() != 0)
            {
                foreach (Student_DetailedTermSubjectMark detailedTermSubjectMark in iqDetailedTermSubjectMark)
                {
                    db.Student_DetailedTermSubjectMarks.DeleteOnSubmit(detailedTermSubjectMark);
                }

                db.SubmitChanges();
            }

            IQueryable<Student_TermSubjectMark> iqTermSubjectMark;
            iqTermSubjectMark = from termSubjectMark in db.Student_TermSubjectMarks
                                where termSubjectMark.Student_StudentInClass.StudentId == student.StudentId
                                select termSubjectMark;
            if (iqTermSubjectMark.Count() != 0)
            {
                foreach (Student_TermSubjectMark termSubjectMark in iqTermSubjectMark)
                {
                    db.Student_TermSubjectMarks.DeleteOnSubmit(termSubjectMark);
                }

                db.SubmitChanges();
            }

            IQueryable<Student_TermLearningResult> iqTermLearningResult;
            iqTermLearningResult = from termLearningResult in db.Student_TermLearningResults
                                   where termLearningResult.Student_StudentInClass.StudentId == student.StudentId
                                   select termLearningResult;
            if (iqTermLearningResult.Count() != 0)
            {
                foreach (Student_TermLearningResult termLearningResult in iqTermLearningResult)
                {
                    db.Student_TermLearningResults.DeleteOnSubmit(termLearningResult);
                }

                db.SubmitChanges();
            }
        }

        public void DeleteTermSubjectMark(Class_Class Class, Category_Subject subject, Configuration_Term term)
        {
            IQueryable<Student_TermSubjectMark> iqStudentTermSubjectMark;
            iqStudentTermSubjectMark = from studTermSubjMark in db.Student_TermSubjectMarks
                                       where studTermSubjMark.TermId == term.TermId
                                         && studTermSubjMark.SubjectId == subject.SubjectId
                                         && studTermSubjMark.Student_StudentInClass.ClassId == Class.ClassId
                                       select studTermSubjMark;
            if (iqStudentTermSubjectMark.Count() != 0)
            {
                foreach (Student_TermSubjectMark studentTermSubjectMark in iqStudentTermSubjectMark)
                {
                    IQueryable<Student_DetailedTermSubjectMark> iqDetailedTermSubjectMark;
                    iqDetailedTermSubjectMark = from detailedTermSubjectMark in db.Student_DetailedTermSubjectMarks
                                                where detailedTermSubjectMark.TermSubjectMarkId == studentTermSubjectMark.TermSubjectMarkId
                                                select detailedTermSubjectMark;
                    if (iqDetailedTermSubjectMark.Count() != 0)
                    {
                        foreach (Student_DetailedTermSubjectMark detailedTermSubjectMark in iqDetailedTermSubjectMark)
                        {
                            // delete student's DetailedTermSubjectMark
                            db.Student_DetailedTermSubjectMarks.DeleteOnSubmit(detailedTermSubjectMark);
                        }
                        db.SubmitChanges();
                    }

                    // delete student's TermSubjectMarks
                    db.Student_TermSubjectMarks.DeleteOnSubmit(studentTermSubjectMark);
                }
                db.SubmitChanges();
            }
        }

        public Student_DetailedTermSubjectMark GetDetailedMark(int detailedMarkId)
        {
            Student_DetailedTermSubjectMark detailedMark = null;
            IQueryable<Student_DetailedTermSubjectMark> iqDetailedMark = from detail in db.Student_DetailedTermSubjectMarks
                                                                         where detail.DetailedTermSubjectMark == detailedMarkId
                                                                            && detail.Approved == true
                                                                         select detail;
            if (iqDetailedMark.Count() != 0)
            {
                detailedMark = iqDetailedMark.First();
            }

            return detailedMark;
        }

        public List<Student_DetailedTermSubjectMark> GetDetailedMarksWithAllStatuses(Student_TermSubjectMark termSubjectedMark, Category_MarkType markType)
        {
            List<Student_DetailedTermSubjectMark> detailMarks = new List<Student_DetailedTermSubjectMark>();
            IQueryable<Student_DetailedTermSubjectMark> iqDetailMark = from dtlMark in db.Student_DetailedTermSubjectMarks
                                                                       where dtlMark.MarkType == markType.MarkTypeId
                                                                           && dtlMark.TermSubjectMarkId == termSubjectedMark.TermSubjectMarkId
                                                                       select dtlMark;
            if (iqDetailMark.Count() != 0)
            {
                detailMarks = iqDetailMark.ToList();
            }

            return detailMarks;
        }

        public List<Student_DetailedTermSubjectMark> GetDetailedMarks(Student_TermSubjectMark termSubjectedMark, Category_MarkType markType, bool approvedStatus)
        {
            List<Student_DetailedTermSubjectMark> detailMarks = new List<Student_DetailedTermSubjectMark>();
            IQueryable<Student_DetailedTermSubjectMark> iqDetailMark = from dtlMark in db.Student_DetailedTermSubjectMarks
                                                                       where dtlMark.MarkType == markType.MarkTypeId
                                                                           && dtlMark.TermSubjectMarkId == termSubjectedMark.TermSubjectMarkId
                                                                           && dtlMark.Approved == approvedStatus
                                                                       select dtlMark;
            if (iqDetailMark.Count() != 0)
            {
                detailMarks = iqDetailMark.ToList();
            }

            return detailMarks;
        }

        public List<Student_DetailedTermSubjectMark> GetDetailedMarksWithAllStatuses(Student_TermSubjectMark termSubjectedMark, Category_MarkType markType, int month)
        {
            List<Student_DetailedTermSubjectMark> detailMarks = new List<Student_DetailedTermSubjectMark>();
            IQueryable<Student_DetailedTermSubjectMark> iqDetailMark = from detail in db.Student_DetailedTermSubjectMarks
                                                                       where detail.MarkType == markType.MarkTypeId
                                                                           && detail.TermSubjectMarkId == termSubjectedMark.TermSubjectMarkId
                                                                           && detail.Date.Month == month
                                                                       select detail;
            if (iqDetailMark.Count() != 0)
            {
                detailMarks = iqDetailMark.ToList();
            }

            return detailMarks;
        }        

        public List<Student_DetailedTermSubjectMark> GetDetailedMarks(Student_TermSubjectMark termSubjectedMark, Category_MarkType markType, int month, bool approvedStatus)
        {
            List<Student_DetailedTermSubjectMark> detailMarks = new List<Student_DetailedTermSubjectMark>();
            IQueryable<Student_DetailedTermSubjectMark> iqDetailMark = from detail in db.Student_DetailedTermSubjectMarks
                                                                       where detail.MarkType == markType.MarkTypeId
                                                                       && detail.TermSubjectMarkId == termSubjectedMark.TermSubjectMarkId
                                                                       && detail.Date.Month == month
                                                                       && detail.Approved == approvedStatus
                                                                       select detail;
            if (iqDetailMark.Count() != 0)
            {
                detailMarks = iqDetailMark.ToList();
            }

            return detailMarks;
        }

        public List<Student_DetailedTermSubjectMark> GetDetailedMarksWithAllStatuses(Student_TermSubjectMark termSubjectedMark, Category_MarkType markType, DateTime beginDate, DateTime endDate)
        {
            List<Student_DetailedTermSubjectMark> detailMarks = new List<Student_DetailedTermSubjectMark>();
            IQueryable<Student_DetailedTermSubjectMark> iqDetailMark = from dtlMark in db.Student_DetailedTermSubjectMarks
                                                                       where dtlMark.MarkType == markType.MarkTypeId
                                                                       && dtlMark.TermSubjectMarkId == termSubjectedMark.TermSubjectMarkId
                                                                       && dtlMark.Date >= beginDate && dtlMark.Date <= endDate
                                                                       select dtlMark;
            if (iqDetailMark.Count() != 0)
            {
                detailMarks = iqDetailMark.ToList();
            }

            return detailMarks;
        }

        public List<Student_DetailedTermSubjectMark> GetDetailedMarks(Student_TermSubjectMark termSubjectedMark, Category_MarkType markType, DateTime beginDate, DateTime endDate, bool approved)
        {
            List<Student_DetailedTermSubjectMark> detailMarks = new List<Student_DetailedTermSubjectMark>();
            IQueryable<Student_DetailedTermSubjectMark> iqDetailMark = from dtlMark in db.Student_DetailedTermSubjectMarks
                                                                       where dtlMark.MarkType == markType.MarkTypeId
                                                                       && dtlMark.TermSubjectMarkId == termSubjectedMark.TermSubjectMarkId
                                                                       && dtlMark.Date >= beginDate && dtlMark.Date <= endDate
                                                                       && dtlMark.Approved == approved
                                                                       select dtlMark;
            if (iqDetailMark.Count() != 0)
            {
                detailMarks = iqDetailMark.ToList();
            }

            return detailMarks;
        }

        public List<Student_DetailedTermSubjectMark> GetDetailedMarks(Student_TermSubjectMark termSubjectedMark, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<Student_DetailedTermSubjectMark> detailMarks = new List<Student_DetailedTermSubjectMark>();
            IQueryable<Student_DetailedTermSubjectMark> iqDetailMark = from detail in db.Student_DetailedTermSubjectMarks
                                                                       where detail.TermSubjectMarkId == termSubjectedMark.TermSubjectMarkId
                                                                        && detail.Approved == true
                                                                       select detail;
            totalRecords = iqDetailMark.Count();
            if (totalRecords != 0)
            {
                detailMarks = iqDetailMark.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return detailMarks;
        }

        public List<Student_DetailedTermSubjectMark> GetDetailedTermSubjectMarks(Student_Student student, Category_MarkType markType, Category_Subject subject, Class_Class Class, Configuration_Term term)
        {
            List<Student_DetailedTermSubjectMark> detailMarks = new List<Student_DetailedTermSubjectMark>();
            IQueryable<Student_DetailedTermSubjectMark> iqDetailMark = from dtlMark in db.Student_DetailedTermSubjectMarks
                                                                       where dtlMark.Student_TermSubjectMark.Student_StudentInClass.StudentId == student.StudentId
                                                                           && dtlMark.MarkType == markType.MarkTypeId
                                                                           && dtlMark.Student_TermSubjectMark.SubjectId == subject.SubjectId
                                                                           && dtlMark.Student_TermSubjectMark.Student_StudentInClass.ClassId == Class.ClassId
                                                                           && dtlMark.Student_TermSubjectMark.TermId == term.TermId
                                                                           && dtlMark.Approved == true
                                                                       select dtlMark;
            if (iqDetailMark.Count() != 0)
            {
                detailMarks = iqDetailMark.OrderBy(dtlMark => dtlMark.Date).ToList();
            }

            return detailMarks;
        }

        public List<Category_Subject> GetScheduledSubjects(Student_Student student, Configuration_Year year, Configuration_Term term)
        {
            // Get list of SubjectId based on schedule
            List<Category_Subject> scheduledSubjects = new List<Category_Subject>();
            IQueryable<Category_Subject> iqScheduledSubjects = from schedule in db.Class_Schedules
                                                               join studentInClass in db.Student_StudentInClasses
                                                                    on schedule.ClassId equals studentInClass.ClassId
                                                               where schedule.Class_Class.YearId == year.YearId
                                                                   && schedule.TermId == term.TermId
                                                                   && studentInClass.StudentId == student.StudentId
                                                               select schedule.Category_Subject;
            if (iqScheduledSubjects.Count() != 0)
            {
                scheduledSubjects = iqScheduledSubjects.Distinct().ToList();
            }

            return scheduledSubjects;
        }

        public Student_TermSubjectMark GetTermSubjectedMark(int termSubjectedMarkId)
        {
            Student_TermSubjectMark termSubjectedMark = null;

            IQueryable<Student_TermSubjectMark> iqTermSubjectedMark;
            iqTermSubjectedMark = from termSubjMark in db.Student_TermSubjectMarks
                                  where termSubjMark.TermSubjectMarkId == termSubjectedMarkId
                                  select termSubjMark;

            if (iqTermSubjectedMark.Count() != 0)
            {
                termSubjectedMark = iqTermSubjectedMark.First();
            }

            return termSubjectedMark;
        }

        public Student_TermSubjectMark GetTermSubjectedMark(Student_Student student, Configuration_Year year, Configuration_Term term, Category_Subject subject)
        {
            Student_TermSubjectMark termSubjectMark = null;

            IQueryable<Student_TermSubjectMark> iqTermSubjectMark;
            iqTermSubjectMark = from termSubjMark in db.Student_TermSubjectMarks
                                where termSubjMark.Student_StudentInClass.StudentId == student.StudentId
                                    && termSubjMark.Student_StudentInClass.Class_Class.YearId == year.YearId
                                    && termSubjMark.SubjectId == subject.SubjectId && termSubjMark.TermId == term.TermId
                                select termSubjMark;
            if (iqTermSubjectMark.Count() != 0)
            {
                termSubjectMark = iqTermSubjectMark.First();
            }

            return termSubjectMark;
        }

        public List<Student_DetailedTermSubjectMark> GetDetailedTermSubjectMarks(Student_Student student, Configuration_Year year, Configuration_Term term, Category_Subject subject, Category_MarkType markType)
        {
            List<Student_DetailedTermSubjectMark> detailedTermSubjectMarks = new List<Student_DetailedTermSubjectMark>();

            IQueryable<Student_DetailedTermSubjectMark> iqDetailedTermSubjectMark;
            iqDetailedTermSubjectMark = from detailedTermSubjectMark in db.Student_DetailedTermSubjectMarks
                                        where detailedTermSubjectMark.Student_TermSubjectMark.Student_StudentInClass.StudentId == student.StudentId
                                            && detailedTermSubjectMark.Student_TermSubjectMark.Student_StudentInClass.Class_Class.YearId == year.YearId
                                            && detailedTermSubjectMark.Student_TermSubjectMark.SubjectId == subject.SubjectId
                                            && detailedTermSubjectMark.Student_TermSubjectMark.TermId == term.TermId
                                            && detailedTermSubjectMark.MarkType == markType.MarkTypeId
                                            && detailedTermSubjectMark.Approved == true
                                        select detailedTermSubjectMark;

            if (iqDetailedTermSubjectMark.Count() != 0)
            {
                detailedTermSubjectMarks = iqDetailedTermSubjectMark.ToList();
            }

            return detailedTermSubjectMarks;
        }

        public List<Student_TermSubjectMark> GetTermSubjectedMarks(Class_Class Class, Category_Subject subject, int pageCurrentIndex, int pageSize, out double totalRecord)
        {
            List<Student_TermSubjectMark> termSubjectMarks = new List<Student_TermSubjectMark>();

            IQueryable<Student_TermSubjectMark> iqTermSubjectMark;
            iqTermSubjectMark = from termSubjMark in db.Student_TermSubjectMarks
                                join detailTermSubjMark in db.Student_DetailedTermSubjectMarks
                                    on termSubjMark.TermSubjectMarkId equals detailTermSubjMark.TermSubjectMarkId
                                where termSubjMark.Student_StudentInClass.ClassId == Class.ClassId
                                    && termSubjMark.SubjectId == subject.SubjectId
                                    && detailTermSubjMark.Approved == true
                                select termSubjMark;

            totalRecord = iqTermSubjectMark.Count();
            if (totalRecord != 0)
            {
                termSubjectMarks = iqTermSubjectMark.OrderBy(termSubjMark => termSubjMark.Student_StudentInClass.Student_Student.StudentCode)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return termSubjectMarks;
        }

        public List<Student_TermSubjectMark> GetTermSubjectedMarks(Class_Class Class, Category_Subject subject, Configuration_Term term, int pageCurrentIndex, int pageSize, out double totalRecord)
        {
            List<Student_TermSubjectMark> termSubjectMarks = new List<Student_TermSubjectMark>();

            IQueryable<Student_TermSubjectMark> iqTermSubjectMark;
            iqTermSubjectMark = from termSubjMark in db.Student_TermSubjectMarks
                                where termSubjMark.Student_StudentInClass.ClassId == Class.ClassId
                                    && termSubjMark.SubjectId == subject.SubjectId
                                    && termSubjMark.TermId == term.TermId
                                select termSubjMark;

            totalRecord = iqTermSubjectMark.Count();
            if (totalRecord != 0)
            {
                termSubjectMarks = iqTermSubjectMark.OrderBy(termSubjMark => termSubjMark.Student_StudentInClass.Student_Student.StudentCode)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return termSubjectMarks;
        }

        public List<Student_TermSubjectMark> GetTermSubjectedMarks(Class_Class Class, Category_Subject subject, Configuration_Term term, int month, int pageCurrentIndex, int pageSize, out double totalRecord)
        {
            List<Student_TermSubjectMark> termSubjectMarks = new List<Student_TermSubjectMark>();

            IQueryable<Student_TermSubjectMark> iqTermSubjectMark = from termSubjMark in db.Student_TermSubjectMarks
                                                                    join detail in db.Student_DetailedTermSubjectMarks
                                                                        on termSubjMark.TermSubjectMarkId equals detail.TermSubjectMarkId
                                                                    where termSubjMark.Student_StudentInClass.ClassId == Class.ClassId
                                                                        && termSubjMark.SubjectId == subject.SubjectId
                                                                        && detail.Student_TermSubjectMark.TermId == term.TermId && detail.Date.Month == month
                                                                        && detail.Approved == true
                                                                    select termSubjMark;

            totalRecord = iqTermSubjectMark.Count();
            if (totalRecord != 0)
            {
                termSubjectMarks = iqTermSubjectMark.OrderBy(termSubjMark => termSubjMark.Student_StudentInClass.Student_Student.StudentCode)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return termSubjectMarks;
        }

        public List<Student_TermLearningResult> GetStudentTermResults(Student_Student student, Configuration_Year year, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<Student_TermLearningResult> studentTermResults = new List<Student_TermLearningResult>();
            IQueryable<Student_TermLearningResult> iqStudentTermResult = from termLearningResult in db.Student_TermLearningResults
                                                                         where termLearningResult.Student_StudentInClass.StudentId == student.StudentId
                                                                            && termLearningResult.Student_StudentInClass.Class_Class.YearId == year.YearId
                                                                         select termLearningResult;

            totalRecords = iqStudentTermResult.Count();
            if (totalRecords != 0)
            {
                studentTermResults = iqStudentTermResult.ToList();
            }

            return studentTermResults;
        }

        public List<Student_TermLearningResult> GetStudentTermResults(Class_Class Class, Configuration_Term term, Student_Student student, int pageCurrentIndex, int pageSize, out double totalRecords, out int orderNo)
        {
            List<Student_TermLearningResult> studentTermResults = new List<Student_TermLearningResult>();
            IQueryable<Student_TermLearningResult> iqStudentTermResult = from termLearningResult in db.Student_TermLearningResults
                                                                         where termLearningResult.Student_StudentInClass.ClassId == Class.ClassId
                                                                            && termLearningResult.TermId == term.TermId
                                                                         select termLearningResult;

            orderNo = 0;
            totalRecords = iqStudentTermResult.Count();
            if (totalRecords != 0)
            {
                studentTermResults = iqStudentTermResult.OrderByDescending(result => result.TermAverageMark)
                   .ThenBy(result => result.Student_StudentInClass.Student_Student.StudentCode).ToList();
                for (int i = 0; i < studentTermResults.Count; i++)
                {
                    if (studentTermResults[i].Student_StudentInClass.StudentId == student.StudentId)
                    {
                        orderNo = i;
                        break;
                    }
                }
            }

            return studentTermResults;
        }

        public List<Student_TermLearningResult> GetStudentTermResults(Class_Class Class, Student_Student student, int pageCurrentIndex, int pageSize, out double totalRecords, out int orderNo)
        {
            List<Student_TermLearningResult> studentTermResults = new List<Student_TermLearningResult>();
            IQueryable<Student_TermLearningResult> iqStudentTermResult = from termLearningResult in db.Student_TermLearningResults
                                                                         where termLearningResult.Student_StudentInClass.ClassId == Class.ClassId
                                                                         select termLearningResult;

            orderNo = 0;
            totalRecords = iqStudentTermResult.Count();
            if (totalRecords != 0)
            {
                studentTermResults = iqStudentTermResult.OrderByDescending(result => result.Student_StudentInClass.Student_Student.StudentCode)
                   .ThenBy(result => result.TermAverageMark).ToList();
                for (int i = 0; i < studentTermResults.Count; i++)
                {
                    if (studentTermResults[i].Student_StudentInClass.StudentId == student.StudentId)
                    {
                        orderNo = i;
                        break;
                    }
                }
            }

            return studentTermResults;
        }

        public void CalculateTermAvgMark(Student_Student student, Class_Class Class, Configuration_Term term)
        {
            double dTermAvgMark = -1;

            IQueryable<Student_TermSubjectMark> iqTermSubjectedMark;
            iqTermSubjectedMark = from termSubjMark in db.Student_TermSubjectMarks
                                  where termSubjMark.Student_StudentInClass.StudentId == student.StudentId
                                    && termSubjMark.Student_StudentInClass.ClassId == Class.ClassId
                                    && termSubjMark.TermId == term.TermId
                                  select termSubjMark;

            IQueryable<Student_TermLearningResult> iqTermLearningResult = from termLearningResult in db.Student_TermLearningResults
                                                                          where termLearningResult.Student_StudentInClass.StudentId == student.StudentId
                                                                          && termLearningResult.Student_StudentInClass.ClassId == Class.ClassId
                                                                          && termLearningResult.TermId == term.TermId
                                                                          select termLearningResult;

            foreach (Student_TermLearningResult termLearningResult in iqTermLearningResult)
            {
                if (iqTermSubjectedMark.Count() != 0)
                {
                    int iUnCalculateTermSubjectAvg = (from result in iqTermSubjectedMark
                                                      where result.AverageMark < 0
                                                      select result).Count();
                    if (iUnCalculateTermSubjectAvg == 0)
                    {
                        double dTotalSubjectMarkRatio = 0;
                        double dTotalMarks = 0;
                        foreach (Student_TermSubjectMark termSubjectedMark in iqTermSubjectedMark)
                        {
                            dTotalSubjectMarkRatio += termSubjectedMark.Category_Subject.MarkRatio;
                            dTotalMarks += termSubjectedMark.Category_Subject.MarkRatio * termSubjectedMark.AverageMark;
                        }

                        dTermAvgMark = dTotalMarks / dTotalSubjectMarkRatio;
                        dTermAvgMark = Math.Round(dTermAvgMark, 1, MidpointRounding.AwayFromZero);

                        termLearningResult.TermAverageMark = dTermAvgMark;
                    }
                    else
                    {
                        termLearningResult.TermAverageMark = -1;
                    }
                }
                else
                {
                    termLearningResult.TermAverageMark = -1;
                }

                LearningAptitudeDA learningAptitudeDA = new LearningAptitudeDA(school);
                Category_LearningAptitude learningAptitude = learningAptitudeDA.GetLearningAptitude((double)termLearningResult.TermAverageMark);
                if (learningAptitude != null)
                {
                    termLearningResult.TermLearningAptitudeId = learningAptitude.LearningAptitudeId;
                }
                else
                {
                    termLearningResult.TermLearningAptitudeId = -1;
                }
            }


            db.SubmitChanges();
        }

        public bool DetailTermSubjectMarkExists(Category_MarkType markType)
        {
            IQueryable<Student_DetailedTermSubjectMark> iqDetailedTermSubjectMark;
            iqDetailedTermSubjectMark = from detailedTermSubjectMark in db.Student_DetailedTermSubjectMarks
                                        where detailedTermSubjectMark.MarkType == markType.MarkTypeId
                                        && detailedTermSubjectMark.Category_MarkType.Category_Grade.SchoolId == school.SchoolId
                                        && detailedTermSubjectMark.Approved == true
                                        select detailedTermSubjectMark;

            if (iqDetailedTermSubjectMark.Count() != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool TermLearningResultExists(Category_LearningAptitude learningAptitude)
        {
            IQueryable<Student_TermLearningResult> iqTermStudentResult;

            iqTermStudentResult = from termStudentResult in db.Student_TermLearningResults
                                  join la in db.Category_LearningAptitudes on termStudentResult.TermLearningAptitudeId equals la.LearningAptitudeId
                                  where la.LearningAptitudeId == learningAptitude.LearningAptitudeId
                                  select termStudentResult;

            if (iqTermStudentResult.Count() != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<Category_Subject> GetNewMarkSubjects(Student_Student student, Class_Class Class, int limitDays)
        {
            List<Category_Subject> newMarkSubjects = new List<Category_Subject>();
            IQueryable<Category_Subject> iqSubject = from termSubjectMark in db.Student_TermSubjectMarks
                                                     join detail in db.Student_DetailedTermSubjectMarks on termSubjectMark.TermSubjectMarkId equals detail.TermSubjectMarkId
                                                     where termSubjectMark.Student_StudentInClass.StudentId == student.StudentId
                                                     && termSubjectMark.Student_StudentInClass.ClassId == Class.ClassId
                                                     && DateTime.Now < detail.Date.AddDays(limitDays)
                                                     && detail.Approved == true
                                                     select termSubjectMark.Category_Subject;
            if (iqSubject.Count() != 0)
            {
                newMarkSubjects = iqSubject.ToList();
            }
            return newMarkSubjects;
        }

        public List<Category_Subject> GetNewMarkSubjects(Student_Student student, Class_Class Class, int limitDays, Category_MarkType markType)
        {
            List<Category_Subject> newMarkSubjects = new List<Category_Subject>();
            IQueryable<Category_Subject> iqSubject = from termSubjectMark in db.Student_TermSubjectMarks
                                                     join detail in db.Student_DetailedTermSubjectMarks on termSubjectMark.TermSubjectMarkId equals detail.TermSubjectMarkId
                                                     where termSubjectMark.Student_StudentInClass.StudentId == student.StudentId
                                                     && termSubjectMark.Student_StudentInClass.ClassId == Class.ClassId
                                                     && DateTime.Now < detail.Date.AddDays(limitDays)
                                                     && detail.MarkType == markType.MarkTypeId
                                                     && detail.Approved == true
                                                     select termSubjectMark.Category_Subject;
            if (iqSubject.Count() != 0)
            {
                newMarkSubjects = iqSubject.ToList();
            }
            return newMarkSubjects;
        }

        public List<Student_DetailedTermSubjectMark> GetConsideredDetailedMarks(Class_Class Class, Configuration_Term term, Category_Subject subject, int month, bool approved)
        {
            List<Student_DetailedTermSubjectMark> detailMarks = new List<Student_DetailedTermSubjectMark>();
            IQueryable<Student_DetailedTermSubjectMark> iqDetailMark = from detail in db.Student_DetailedTermSubjectMarks
                                                                       where detail.Student_TermSubjectMark.Student_StudentInClass.ClassId == Class.ClassId
                                                                       && detail.Student_TermSubjectMark.SubjectId == subject.SubjectId
                                                                       && detail.Student_TermSubjectMark.TermId == term.TermId
                                                                       && detail.Date.Month == month
                                                                       && detail.Approved == approved
                                                                       select detail;
            return detailMarks;
        }

        public List<Student_DetailedTermSubjectMark> GetConsideredDetailedMarks(Class_Class Class, Configuration_Term term, Category_Subject subject, bool approvedStatus)
        {
            List<Student_DetailedTermSubjectMark> detailMarks = new List<Student_DetailedTermSubjectMark>();
            IQueryable<Student_DetailedTermSubjectMark> iqDetailMark = from detail in db.Student_DetailedTermSubjectMarks
                                                                       where detail.Student_TermSubjectMark.Student_StudentInClass.ClassId == Class.ClassId
                                                                       && detail.Student_TermSubjectMark.SubjectId == subject.SubjectId
                                                                       && detail.Student_TermSubjectMark.TermId == term.TermId
                                                                       && detail.Approved == approvedStatus
                                                                       select detail;
            if (iqDetailMark.Count() != 0)
            {
                detailMarks = iqDetailMark.ToList();
            }

            return detailMarks;
        }

        public List<Student_DetailedTermSubjectMark> GetConsideredDetailedMarks(Class_Class Class, Configuration_Term term, Category_Subject subject, Category_MarkType markType, int month, bool approvedStatus)
        {
            List<Student_DetailedTermSubjectMark> detailMarks = new List<Student_DetailedTermSubjectMark>();
            IQueryable<Student_DetailedTermSubjectMark> iqDetailMark = from detail in db.Student_DetailedTermSubjectMarks
                                                                       where detail.MarkType == markType.MarkTypeId
                                                                       && detail.Student_TermSubjectMark.Student_StudentInClass.ClassId == Class.ClassId
                                                                       && detail.Student_TermSubjectMark.SubjectId == subject.SubjectId
                                                                       && detail.Student_TermSubjectMark.TermId == term.TermId
                                                                       && detail.Date.Month == month
                                                                       && detail.Approved == approvedStatus
                                                                       select detail;
        
            return detailMarks;
        }

        public List<Student_DetailedTermSubjectMark> GetConsideredDetailedMarks(Class_Class Class, Configuration_Term term, Category_Subject subject, Category_MarkType markType, bool approvedStatus)
        {
            List<Student_DetailedTermSubjectMark> detailMarks = new List<Student_DetailedTermSubjectMark>();
            IQueryable<Student_DetailedTermSubjectMark> iqDetailMark = from detail in db.Student_DetailedTermSubjectMarks
                                                                       where detail.MarkType == markType.MarkTypeId
                                                                       && detail.Student_TermSubjectMark.Student_StudentInClass.ClassId == Class.ClassId
                                                                       && detail.Student_TermSubjectMark.SubjectId == subject.SubjectId
                                                                       && detail.Student_TermSubjectMark.TermId == term.TermId
                                                                       && detail.Approved == approvedStatus
                                                                       select detail;
            if (iqDetailMark.Count() != 0)
            {
                detailMarks = iqDetailMark.ToList();
            }

            return detailMarks;
        }

        public List<Student_DetailedTermSubjectMark> GetConsideredDetailedMarks(Class_Class Class, Configuration_Term term, Category_Subject subject)
        {
            List<Student_DetailedTermSubjectMark> detailMarks = new List<Student_DetailedTermSubjectMark>();
            IQueryable<Student_DetailedTermSubjectMark> iqDetailMark = from detail in db.Student_DetailedTermSubjectMarks
                                                                       where detail.Student_TermSubjectMark.Student_StudentInClass.ClassId == Class.ClassId
                                                                           && detail.Student_TermSubjectMark.SubjectId == subject.SubjectId
                                                                           && detail.Student_TermSubjectMark.TermId == term.TermId
                                                                       select detail;            
            if (iqDetailMark.Count() != 0)
            {
                detailMarks = iqDetailMark.ToList();
            }

            return detailMarks;
        }

        public List<Student_DetailedTermSubjectMark> GetConsideredDetailedMarks(Class_Class Class, Configuration_Term term, Category_Subject subject, int month)
        {
            List<Student_DetailedTermSubjectMark> detailMarks = new List<Student_DetailedTermSubjectMark>();
            IQueryable<Student_DetailedTermSubjectMark> iqDetailMark = from detail in db.Student_DetailedTermSubjectMarks
                                                                       where detail.Student_TermSubjectMark.Student_StudentInClass.ClassId == Class.ClassId
                                                                           && detail.Student_TermSubjectMark.SubjectId == subject.SubjectId
                                                                           && detail.Student_TermSubjectMark.TermId == term.TermId
                                                                           && detail.Date.Month == month
                                                                           && detail.Approved == true
                                                                       select detail;
            return detailMarks;
        }

        public List<Student_DetailedTermSubjectMark> GetConsideredDetailedMarks(Class_Class Class, Configuration_Term term, Category_Subject subject, Category_MarkType markType, int month)
        {
            List<Student_DetailedTermSubjectMark> detailMarks = new List<Student_DetailedTermSubjectMark>();
            IQueryable<Student_DetailedTermSubjectMark> iqDetailMark = from detail in db.Student_DetailedTermSubjectMarks
                                                                       where detail.MarkType == markType.MarkTypeId
                                                                           && detail.Student_TermSubjectMark.Student_StudentInClass.ClassId == Class.ClassId
                                                                           && detail.Student_TermSubjectMark.SubjectId == subject.SubjectId
                                                                           && detail.Student_TermSubjectMark.TermId == term.TermId
                                                                           && detail.Date.Month == month
                                                                           && detail.Approved == true
                                                                       select detail;
            
            return detailMarks;
        }

        public List<Student_DetailedTermSubjectMark> GetConsideredDetailedMarks(Class_Class Class, Configuration_Term term, Category_Subject subject, Category_MarkType markType)
        {
            List<Student_DetailedTermSubjectMark> detailMarks = new List<Student_DetailedTermSubjectMark>();
            IQueryable<Student_DetailedTermSubjectMark> iqDetailMark = from detail in db.Student_DetailedTermSubjectMarks
                                                                       where detail.MarkType == markType.MarkTypeId
                                                                           && detail.Student_TermSubjectMark.Student_StudentInClass.ClassId == Class.ClassId
                                                                           && detail.Student_TermSubjectMark.SubjectId == subject.SubjectId
                                                                           && detail.Student_TermSubjectMark.TermId == term.TermId
                                                                           && detail.Approved == true
                                                                       select detail;
            if (iqDetailMark.Count() != 0)
            {
                detailMarks = iqDetailMark.ToList();
            }

            return detailMarks;
        }

        public void ChangeApproveStatusMark(Student_DetailedTermSubjectMark detailedTermSubjectMark, bool approvedStatus, string note)
        {
            IQueryable<Student_DetailedTermSubjectMark> iqDetailMark = from detail in db.Student_DetailedTermSubjectMarks
                                                                       where detail.DetailedTermSubjectMark == detailedTermSubjectMark.DetailedTermSubjectMark
                                                                       select detail;
            if (iqDetailMark.Count() != 0)
            {
                detailedTermSubjectMark = iqDetailMark.First();
                detailedTermSubjectMark.Approved = approvedStatus;
                detailedTermSubjectMark.Note = note;

                db.SubmitChanges();
            }
        }
    }
}
