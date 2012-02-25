using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class StudyingResultBL : BaseBL
    {
        public enum ADDINGMARKERROR
        {
            NOERROR,
            EXCEEDVALUETEN,
            NOTANUMBER,
            EXCEEDQUANTITY
        }

        private StudyingResultDA studyingResultDA;

        public StudyingResultBL(School_School school)
            : base(school)
        {
            studyingResultDA = new StudyingResultDA(school);
        }

        /// <summary>
        /// Insert student's new term studying result
        /// </summary>
        /// <param name="term"></param>
        /// <param name="studentInClass"></param>
        /// <param name="termAvgMark"></param>
        /// <param name="termConduct"></param>
        /// <param name="termStudyingAptitude"></param>
        public void InsertTermStudyingResult(Configuration_Term term, Student_StudentInClass studentInClass, double termAvgMark, Category_Conduct termConduct, Category_LearningAptitude termStudyingAptitude)
        {
            Student_TermLearningResult termStudyingResult = new Student_TermLearningResult
            {
                TermId = term.TermId,
                StudentInClassId = studentInClass.StudentInClassId,
                TermAverageMark = termAvgMark,
                TermConductId = termConduct.ConductId,
                TermLearningAptitudeId = termStudyingAptitude.LearningAptitudeId
            };

            studyingResultDA.InsertTermStudyingResult(termStudyingResult);
        }

        /// <summary>
        /// Add new marks for student
        /// </summary>
        /// <param name="student"></param>
        /// <param name="Class"></param>
        /// <param name="term"></param>
        /// <param name="subject"></param>
        /// <param name="marks"></param>
        public void AddMark(Student_Student student, Class_Class Class, Configuration_Term term, Category_Subject subject, List<MarkValueAndTypePair> marks)
        {
            int i = 0;
            while (i < marks.Count)
            {
                if (marks[i].GiaTri == -1)
                {
                    marks.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            if (marks.Count > 0)
            {
                studyingResultDA.InsertDetailedMark(student, Class, term, subject, marks, DateTime.Now);
                if (NeedResetAvgMark(student, Class, term, subject))
                {
                    studyingResultDA.ResetAvgMark(student, Class, term, subject);
                }
                else
                {
                    studyingResultDA.CalculateTermSubjectAvgMark(student, Class, term, subject);
                }

                studyingResultDA.CalculateTermAvgMark(student, Class, term);
            }
        }

        /// <summary>
        /// Check if need to reset avg mark
        /// </summary>
        /// <param name="student"></param>
        /// <param name="Class"></param>
        /// <param name="term"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        private bool NeedResetAvgMark(Student_Student student, Class_Class Class, Configuration_Term term, Category_Subject subject)
        {
            return studyingResultDA.NeedResetAvgMark(student, Class, term, subject);
        }

        public List<TabularSubjectTermResult> GetTabularSubjectTermResults(Student_Student student, Configuration_Year year, Configuration_Term term, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularSubjectTermResult> tabularSubjectTermResults = new List<TabularSubjectTermResult>();
            TabularSubjectTermResult tabularSubjectTermResult = null; // Use for looping
            List<Category_Subject> scheduledSubjects = studyingResultDA.GetScheduledSubjects(student, year, term);
            Student_TermSubjectMark termSubjectedMark;

            foreach (Category_Subject scheduledSubject in scheduledSubjects)
            {
                termSubjectedMark = studyingResultDA.GetTermSubjectedMark(student, year, term, scheduledSubject);
                if (termSubjectedMark != null)
                {
                    tabularSubjectTermResult = new TabularSubjectTermResult();
                    tabularSubjectTermResult.SubjectName = scheduledSubject.SubjectName;
                    tabularSubjectTermResult.MaDiemMonHK = termSubjectedMark.TermSubjectMarkId;
                    tabularSubjectTermResult.DiemTB = termSubjectedMark.AverageMark;

                    tabularSubjectTermResults.Add(tabularSubjectTermResult);
                }
            }

            totalRecords = tabularSubjectTermResults.Count();
            if (totalRecords != 0)
            {
                tabularSubjectTermResults = tabularSubjectTermResults.OrderBy(ketQua => ketQua.SubjectName)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
                foreach (TabularSubjectTermResult tbSubjectTermResult in tabularSubjectTermResults)
                {
                    tbSubjectTermResult.StrDiemTB = (tbSubjectTermResult.DiemTB != -1) ? tbSubjectTermResult.DiemTB.ToString() : "";
                }
            }

            return tabularSubjectTermResults;
        }

        public List<StrDiemMonHocLoaiDiem> GetApprovedSubjectMarks(Student_TermSubjectMark termSubjectedMark)
        {
            List<StrDiemMonHocLoaiDiem> stringDiemMonHocs = new List<StrDiemMonHocLoaiDiem>();

            List<List<double>> lstDiemMonHoc = GetApprovedDoubleSubjectMarks(termSubjectedMark);
            foreach (List<double> lstChiTietDiemMonHoc in lstDiemMonHoc)
            {
                StrDiemMonHocLoaiDiem strDiemMonHocLoaiDiem = new StrDiemMonHocLoaiDiem();
                strDiemMonHocLoaiDiem.Diems = "";
                foreach (double diem in lstChiTietDiemMonHoc)
                {
                    strDiemMonHocLoaiDiem.Diems += diem + ", ";
                }
                strDiemMonHocLoaiDiem.Diems = strDiemMonHocLoaiDiem.Diems.Trim();
                strDiemMonHocLoaiDiem.Diems = strDiemMonHocLoaiDiem.Diems.TrimEnd(',');
                stringDiemMonHocs.Add(strDiemMonHocLoaiDiem);
            }
            return stringDiemMonHocs;
        }

        public List<TabularTermStudentResult> GetTabularTermStudentResults(Student_Student student, Configuration_Year year,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularTermStudentResult> tabularTermStudentResults = null;
            TabularTermStudentResult tabularTermStudentResult = null; // used in loop
            LearningAptitudeBL learningAptitudeBL = null;
            ConductBL conductBL = null;
            LearningResultBL learningResultBL = null;
            Category_Conduct conduct = null;
            Category_LearningAptitude learningAptitude = null;
            Category_LearningResult learningResult = null;
            List<Student_TermLearningResult> termResults = null;
            TabularTermStudentResult tabularFinalStudentResult = null;

            termResults = studyingResultDA.GetStudentTermResults(student, year, pageCurrentIndex, pageSize, out totalRecords);
            if (totalRecords != 0)
            {
                // init BLs
                learningAptitudeBL = new LearningAptitudeBL(school);
                conductBL = new ConductBL(school);
                learningResultBL = new LearningResultBL(school);

                // init result list
                tabularTermStudentResults = new List<TabularTermStudentResult>();

                foreach (Student_TermLearningResult termResult in termResults)
                {
                    tabularTermStudentResult = new TabularTermStudentResult();
                    // term's name
                    tabularTermStudentResult.TermName = termResult.Configuration_Term.TermName;
                    // average mark
                    tabularTermStudentResult.AverageMark = (double)termResult.TermAverageMark;
                    // average mark in string format
                    tabularTermStudentResult.StringAverageMark = (termResult.TermAverageMark != -1) ? (termResult.TermAverageMark.ToString()) : "(Chưa xác định)";

                    tabularTermStudentResult.LearningAptitudeId = (int)termResult.TermLearningAptitudeId; // default is -1
                    // get name of learning aptitude
                    learningAptitude = learningAptitudeBL.GetLearningAptitude(tabularTermStudentResult.AverageMark);
                    if (learningAptitude != null)
                    {
                        tabularTermStudentResult.LearningAptitudeName = learningAptitude.LearningAptitudeName;
                    }
                    else
                    {
                        tabularTermStudentResult.LearningAptitudeName = "(Chưa xác định)";
                    }

                    tabularTermStudentResult.ConductId = (int)termResult.TermConductId;
                    // get name of conduct
                    conduct = conductBL.GetConduct((int)tabularTermStudentResult.ConductId);
                    if (conduct != null)
                    {
                        tabularTermStudentResult.ConductName = conduct.ConductName;
                    }
                    else
                    {
                        tabularTermStudentResult.ConductName = "(Chưa xác định)";
                    }

                    // get name of learningResult
                    if (conduct != null && learningAptitude != null)
                    {
                        learningResult = learningResultBL.GetLearningResult(conduct, learningAptitude);
                        if (learningResult != null)
                        {
                            tabularTermStudentResult.LearningResultName = learningResult.LearningResultName;
                        }
                        else
                        {
                            tabularTermStudentResult.LearningResultName = "(Chưa xác định)";
                        }
                    }
                    else
                    {
                        tabularTermStudentResult.LearningResultName = "(Chưa xác định)";
                    }

                    tabularTermStudentResults.Add(tabularTermStudentResult);
                }

                // final result
                tabularFinalStudentResult = new TabularTermStudentResult();
                tabularFinalStudentResult.LearningAptitudeId = -1;
                tabularFinalStudentResult.TermName = "Cả năm";
                if ((tabularTermStudentResults[0].AverageMark != -1) && (tabularTermStudentResults[1].AverageMark != -1))
                {
                    tabularFinalStudentResult.AverageMark = Math.Round(((tabularTermStudentResults[0].AverageMark + (2 * tabularTermStudentResults[1].AverageMark)) / 3), 1);
                    tabularFinalStudentResult.StringAverageMark = tabularFinalStudentResult.AverageMark.ToString();
                }
                else
                {
                    tabularFinalStudentResult.AverageMark = -1;
                    tabularFinalStudentResult.StringAverageMark = "(Chưa xác định)";
                }

                // Nếu đã xác định được hạnh kiểm cả 2 học kì
                if (tabularTermStudentResults[0].ConductId != -1 && tabularTermStudentResults[1].ConductId != -1)
                {
                    // hạnh kiểm cuối năm = hạnh kiểm học kì 2
                    tabularFinalStudentResult.ConductId = tabularTermStudentResults[1].ConductId;
                }
                else
                {
                    tabularFinalStudentResult.ConductId = -1;
                }

                conduct = conductBL.GetConduct((int)tabularFinalStudentResult.ConductId);
                if (conduct != null)
                {
                    tabularFinalStudentResult.ConductName = conduct.ConductName;
                }
                else
                {
                    tabularFinalStudentResult.ConductName = "(Chưa xác định)";
                }


                learningAptitude = learningAptitudeBL.GetLearningAptitude(tabularFinalStudentResult.AverageMark);
                if (learningAptitude != null)
                {
                    tabularFinalStudentResult.LearningAptitudeName = learningAptitude.LearningAptitudeName;
                }
                else
                {
                    tabularFinalStudentResult.LearningAptitudeName = "(Chưa xác định)";
                }

                if (conduct != null && learningAptitude != null)
                {
                    learningResult = learningResultBL.GetLearningResult(conduct, learningAptitude);
                    if (learningResult != null)
                    {
                        tabularFinalStudentResult.LearningResultName = learningResult.LearningResultName;
                    }
                    else
                    {
                        tabularFinalStudentResult.LearningResultName = "(Chưa xác định)";
                    }
                }
                else
                {
                    tabularFinalStudentResult.LearningResultName = "(Chưa xác định)";
                }

                tabularTermStudentResults.Add(tabularFinalStudentResult);
            }

            return tabularTermStudentResults;
        }

        public List<TabularStudentRating> GetTabularStudentRating(Student_Student student, Configuration_Year year, Configuration_Term term,
            int pageCurrentIndex, int pageSize, out double totalRecords, out int orderNo)
        {
            List<TabularStudentRating> studentRatings = null;
            TabularStudentRating studentRating = null;

            LearningAptitudeBL learningAptitudeBL = null;
            ConductBL conductBL = null;
            LearningResultBL learningResultBL = null;
            StudentBL studentBL = new StudentBL(school);

            Category_Conduct conduct = null;
            Category_LearningAptitude learningAptitude = null;
            Category_LearningResult learningResult = null;

            List<Student_TermLearningResult> termResults = null;
            TabularTermStudentResult tabularFinalStudentResult = null;
            
            Class_Class Class = studentBL.GetClass(student, year);

            if (term == null) // term is "All"
            {
                termResults = studyingResultDA.GetStudentTermResults(Class, student, pageCurrentIndex, pageSize, out totalRecords, out orderNo);
            }
            else
            {
                termResults = studyingResultDA.GetStudentTermResults(Class, term, student, pageCurrentIndex, pageSize, out totalRecords, out orderNo);
            }

            if (totalRecords != 0)
            {
                learningAptitudeBL = new LearningAptitudeBL(school);
                conductBL = new ConductBL(school);
                learningResultBL = new LearningResultBL(school);
                studentRatings = new List<TabularStudentRating>(); // init result list
                if (term != null)
                {
                    foreach (Student_TermLearningResult termResult in termResults)
                    {
                        studentRating = new TabularStudentRating();
                        studentRating.StudentCode = termResult.Student_StudentInClass.Student_Student.StudentCode;
                        studentRating.StudentFullName = termResult.Student_StudentInClass.Student_Student.FullName;
                        studentRating.TermName = termResult.Configuration_Term.TermName;
                        studentRating.AverageMark = (int)termResult.TermAverageMark;
                        studentRating.StringAverageMark = (termResult.TermAverageMark != -1) ? (termResult.TermAverageMark.ToString()) : "(Chưa xác định)";

                        // Thông tin học lực
                        learningAptitude = learningAptitudeBL.GetLearningAptitude(studentRating.AverageMark);
                        if (learningAptitude != null)
                        {
                            studentRating.LearningAptitudeName = learningAptitude.LearningAptitudeName;
                        }
                        else
                        {
                            studentRating.LearningAptitudeName = "(Chưa xác định)";
                        }
                        
                        // Thông tin hạnh kiểm
                        conduct = conductBL.GetConduct((int)termResult.TermConductId);
                        if (conduct != null)
                        {
                            studentRating.ConductName = conduct.ConductName;
                        }
                        else
                        {
                            studentRating.ConductName = "(Chưa xác định)";
                        }
                        
                        // Thông tin danh hiệu
                        if (conduct != null && learningAptitude != null)
                        {
                            learningResult = learningResultBL.GetLearningResult(conduct, learningAptitude);
                            if (learningResult != null)
                            {
                                studentRating.LearningResultName = learningResult.LearningResultName;
                            }
                            else
                            {
                                studentRating.LearningResultName = "(Chưa xác định)";
                            }
                        }
                        else
                        {
                            studentRating.LearningResultName = "(Chưa xác định)";
                        }
                        studentRatings.Add(studentRating);
                    }

                    return studentRatings;
                }

                for (int i = 0; i < termResults.Count - 1; i++)
                {
                    studentRating = new TabularStudentRating();
                    studentRating.StudentCode = termResults[i].Student_StudentInClass.Student_Student.StudentCode;
                    studentRating.StudentFullName = termResults[i].Student_StudentInClass.Student_Student.FullName;
                    studentRating.TermName = "Cả năm";

                    if ((termResults[i].TermAverageMark != -1) && (termResults[i + 1].TermAverageMark != -1))
                    {
                        studentRating.AverageMark = Math.Round(
                            ((double)termResults[i].TermAverageMark + 2 * (double)termResults[i + 1].TermAverageMark) / 3, 1
                        );
                        studentRating.StringAverageMark = studentRating.AverageMark.ToString();
                    }
                    else
                    {
                        studentRating.StringAverageMark = "(Chưa xác định)";
                    }
                    // Nếu đã xác định được hạnh kiểm cả 2 học kì
                    if (termResults[i].TermConductId != -1 && termResults[i + 1].TermConductId != -1)
                    {
                        // hạnh kiểm cuối năm = hạnh kiểm học kì 2
                        studentRating.ConductId = (int)termResults[i + 1].TermConductId;
                    }
                    else
                    {
                        studentRating.ConductId = -1;
                    }
                    conduct = conductBL.GetConduct((int)studentRating.ConductId);
                    if (conduct != null)
                    {
                        studentRating.ConductName = conduct.ConductName;
                    }
                    else
                    {
                        studentRating.ConductName = "(Chưa xác định)";
                    }
                    learningAptitude = learningAptitudeBL.GetLearningAptitude(studentRating.AverageMark);
                    if (learningAptitude != null)
                    {
                        studentRating.LearningAptitudeName = learningAptitude.LearningAptitudeName;
                    }
                    else
                    {
                        studentRating.LearningAptitudeName = "(Chưa xác định)";
                    }

                    if (conduct != null && learningAptitude != null)
                    {
                        learningResult = learningResultBL.GetLearningResult(conduct, learningAptitude);
                        if (learningResult != null)
                        {
                            studentRating.LearningResultName = learningResult.LearningResultName;
                        }
                        else
                        {
                            studentRating.LearningResultName = "(Chưa xác định)";
                        }
                    }
                    else
                    {
                        studentRating.LearningResultName = "(Chưa xác định)";
                    }

                    studentRatings.Add(studentRating);
                    i++;

                }

                studentRatings = studentRatings.OrderByDescending(result => result.AverageMark).ThenBy(result => result.StudentCode).ToList();
                return studentRatings;
            }

            return studentRatings;
        }

        /// <summary>
        /// Get list of TabularStudentMark
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="subject"></param>
        /// <param name="term"></param>
        /// <param name="markTypes"></param>
        /// <param name="pageCurrentIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public List<TabularStudentMark> GetTabularStudentMarks(Class_Class Class, Category_Subject subject, Configuration_Term term, List<Category_MarkType> markTypes, bool? approvedStatus, int pageCurrentIndex, int pageSize, out double totalRecord)
        {
            Student_Student student = new Student_Student();
            // student

            List<TabularStudentMark> tabularStudentMarks = new List<TabularStudentMark>(); // returned list
            TabularStudentMark tabularStudentMark = null;
            List<Student_TermSubjectMark> termSubjectMarks = null;
            List<MarkTypedMark> markMypedMarks = null;
            List<Student_DetailedTermSubjectMark> detailedMarks = null;
            StringBuilder strB = new StringBuilder();
            string strMarks = "";
            MarkTypeBL markTypeBL = new MarkTypeBL(school);

            termSubjectMarks = studyingResultDA.GetTermSubjectedMarks(Class, subject, term, pageCurrentIndex, pageSize, out totalRecord);

            foreach (Student_TermSubjectMark termSubjectMark in termSubjectMarks)
            {
                tabularStudentMark = new TabularStudentMark();
                tabularStudentMark.MaDiemHK = termSubjectMark.TermSubjectMarkId;
                tabularStudentMark.MaHocSinh = termSubjectMark.Student_StudentInClass.StudentId;
                tabularStudentMark.MaHocSinhHienThi = termSubjectMark.Student_StudentInClass.Student_Student.StudentCode;
                tabularStudentMark.TenHocSinh = termSubjectMark.Student_StudentInClass.Student_Student.FullName;
                tabularStudentMark.DiemTrungBinh = termSubjectMark.AverageMark;
                tabularStudentMark.TermName = termSubjectMark.Configuration_Term.TermName;
                markMypedMarks = new List<MarkTypedMark>();
                foreach (Category_MarkType markType in markTypes)
                {
                    List<double> dMarks = new List<double>();
                    if (approvedStatus == null)
                    {
                        detailedMarks = studyingResultDA.GetDetailedMarksWithAllStatuses(termSubjectMark, markType);
                    }
                    else
                    {
                        detailedMarks = studyingResultDA.GetDetailedMarks(termSubjectMark, markType, (bool)approvedStatus);
                    }


                    strMarks = "";

                    foreach (Student_DetailedTermSubjectMark detailedMark in detailedMarks)
                    {
                        strB.Append(detailedMark.MarkValue.ToString());
                        strB.Append(", ");
                    }

                    strMarks = strB.ToString().Trim().Trim(new char[] { ',' });
                    strB.Clear();

                    MarkTypedMark markTypedMark = new MarkTypedMark();
                    markTypedMark.MarkTypeId = markType.MarkTypeId;
                    markTypedMark.MarkTypeName = markType.MarkTypeName;
                    markTypedMark.StringDiems = strMarks;

                    markMypedMarks.Add(markTypedMark);
                }

                tabularStudentMark.DiemTheoLoaiDiems = markMypedMarks;
                tabularStudentMarks.Add(tabularStudentMark);
            }

            return tabularStudentMarks;
        }

        public List<TabularStudentMark> GetTabularStudentMarks(Class_Class Class, Category_Subject subject, Configuration_Term term, int month, List<Category_MarkType> markTypes, bool? approvedStatus, int pageCurrentIndex, int pageSize, out double totalRecord)
        {
            List<TabularStudentMark> tabularStudentMarks = new List<TabularStudentMark>(); // returned list
            TabularStudentMark tabularStudentMark = null;
            List<Student_TermSubjectMark> termSubjectMarks = null;
            List<MarkTypedMark> markMypedMarks = null;
            List<Student_DetailedTermSubjectMark> detailedMarks = null;
            StringBuilder strB = new StringBuilder();
            string strMarks = "";
            MarkTypeBL markTypeBL = new MarkTypeBL(school);

            termSubjectMarks = studyingResultDA.GetTermSubjectedMarks(Class, subject, term, pageCurrentIndex, pageSize, out totalRecord);

            foreach (Student_TermSubjectMark termSubjectMark in termSubjectMarks)
            {
                tabularStudentMark = new TabularStudentMark();
                tabularStudentMark.MaDiemHK = termSubjectMark.TermSubjectMarkId;
                tabularStudentMark.MaHocSinh = termSubjectMark.Student_StudentInClass.StudentId;
                tabularStudentMark.MaHocSinhHienThi = termSubjectMark.Student_StudentInClass.Student_Student.StudentCode;
                tabularStudentMark.TenHocSinh = termSubjectMark.Student_StudentInClass.Student_Student.FullName;
                tabularStudentMark.DiemTrungBinh = termSubjectMark.AverageMark;
                tabularStudentMark.TermName = termSubjectMark.Configuration_Term.TermName;
                markMypedMarks = new List<MarkTypedMark>();
                foreach (Category_MarkType markType in markTypes)
                {
                    List<double> dMarks = new List<double>();
                    if (approvedStatus == null)
                    {
                        detailedMarks = studyingResultDA.GetDetailedMarksWithAllStatuses(termSubjectMark, markType, month);
                    }
                    else
                    {
                        detailedMarks = studyingResultDA.GetDetailedMarks(termSubjectMark, markType, month, (bool)approvedStatus);
                    }
                    strMarks = "";

                    foreach (Student_DetailedTermSubjectMark detailedMark in detailedMarks)
                    {
                        strB.Append(detailedMark.MarkValue.ToString());
                        strB.Append(", ");
                    }

                    strMarks = strB.ToString().Trim().Trim(new char[] { ',' });
                    strB.Clear();

                    MarkTypedMark markTypedMark = new MarkTypedMark();
                    markTypedMark.MarkTypeId = markType.MarkTypeId;
                    markTypedMark.MarkTypeName = markType.MarkTypeName;
                    markTypedMark.StringDiems = strMarks;

                    markMypedMarks.Add(markTypedMark);
                }

                tabularStudentMark.DiemTheoLoaiDiems = markMypedMarks;
                tabularStudentMarks.Add(tabularStudentMark);
            }

            return tabularStudentMarks;
        }

        public List<ConsideredStudentMark> GetConsideredStudentMarks(Class_Class Class, List<Category_Subject> subjects, Configuration_Term term, Category_MarkType markType, bool? approvedStatus, int pageCurrentIndex, int pageSize, out double totalRecord)
        {
            List<ConsideredStudentMark> consideredStudentMarks = new List<ConsideredStudentMark>(); // returned list
            ConsideredStudentMark consideredStudentMark = null;
            List<Student_DetailedTermSubjectMark> detailedMarks = new List<Student_DetailedTermSubjectMark>();
            StringBuilder strB = new StringBuilder();
            MarkTypeBL markTypeBL = new MarkTypeBL(school);

            consideredStudentMark = new ConsideredStudentMark();

            foreach (Category_Subject subject in subjects)
            {
                if (approvedStatus == null)
                {
                    if (markType == null)
                    {
                        detailedMarks.AddRange(studyingResultDA.GetConsideredDetailedMarks(Class, term, subject));
                    }
                    else
                    {
                        detailedMarks.AddRange(studyingResultDA.GetConsideredDetailedMarks(Class, term, subject, markType));
                    }
                }
                else
                {
                    if (markType == null)
                    {
                        detailedMarks.AddRange(studyingResultDA.GetConsideredDetailedMarks(Class, term, subject, (bool)approvedStatus));
                    }
                    else
                    {
                        detailedMarks.AddRange(studyingResultDA.GetConsideredDetailedMarks(Class, term, subject, markType, (bool)approvedStatus));
                    }
                }
            }

            totalRecord = detailedMarks.Count;
            if (detailedMarks.Count != 0)
            {
                totalRecord = detailedMarks.Count();
                if (totalRecord != 0)
                {
                    detailedMarks = detailedMarks.OrderBy(detail => detail.Student_TermSubjectMark.Student_StudentInClass.Student_Student.StudentCode)
                       .ThenBy(detail => detail.Student_TermSubjectMark.Student_StudentInClass.Student_Student.FullName)
                       .ThenBy(detail => detail.Category_MarkType.MarkTypeId).ThenByDescending(detail => detail.Date)
                       .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
                }

                consideredStudentMark = new ConsideredStudentMark();
                consideredStudentMark.Approved = detailedMarks[0].Approved;
                consideredStudentMark.Date = detailedMarks[0].Date;
                consideredStudentMark.DetailTermSubjectMarkId = detailedMarks[0].DetailedTermSubjectMark;
                consideredStudentMark.MarkTypeId = detailedMarks[0].MarkType;
                consideredStudentMark.MarkTypeName = detailedMarks[0].Category_MarkType.MarkTypeName;
                consideredStudentMark.MarkValue = detailedMarks[0].MarkValue;
                consideredStudentMark.StudentCode = detailedMarks[0].Student_TermSubjectMark.Student_StudentInClass.Student_Student.StudentCode;
                consideredStudentMark.StudentName = detailedMarks[0].Student_TermSubjectMark.Student_StudentInClass.Student_Student.FullName;
                consideredStudentMark.Note = detailedMarks[0].Note;
                consideredStudentMark.SubjectName = detailedMarks[0].Student_TermSubjectMark.Category_Subject.SubjectName;
                consideredStudentMarks.Add(consideredStudentMark);
            }

            for (int i = 1; i < detailedMarks.Count; i++)
            {
                consideredStudentMark = new ConsideredStudentMark();
                consideredStudentMark.Approved = detailedMarks[i].Approved;
                consideredStudentMark.Date = detailedMarks[i].Date;
                consideredStudentMark.DetailTermSubjectMarkId = detailedMarks[i].DetailedTermSubjectMark;
                consideredStudentMark.MarkTypeId = detailedMarks[i].MarkType;
                consideredStudentMark.MarkTypeName = detailedMarks[i].Category_MarkType.MarkTypeName;
                consideredStudentMark.MarkValue = detailedMarks[i].MarkValue;
                consideredStudentMark.StudentCode = detailedMarks[i].Student_TermSubjectMark.Student_StudentInClass.Student_Student.StudentCode;
                consideredStudentMark.StudentName = detailedMarks[i].Student_TermSubjectMark.Student_StudentInClass.Student_Student.FullName;
                consideredStudentMark.SubjectName = detailedMarks[i].Student_TermSubjectMark.Category_Subject.SubjectName;
                //if (consideredStudentMark.StudentCode == consideredStudentMarks[i - 1].StudentCode)
                //{
                //    consideredStudentMark.StudentCode = "";
                //}

                //if (consideredStudentMark.StudentName == consideredStudentMarks[i - 1].StudentName)
                //{
                //    consideredStudentMark.StudentName = "";
                //}
                consideredStudentMark.Note = detailedMarks[i].Note;
                consideredStudentMarks.Add(consideredStudentMark);
            }

            return consideredStudentMarks;
        }

        public List<ConsideredStudentMark> GetConsideredStudentMarks(Class_Class Class, List<Category_Subject> subjects, Configuration_Term term, int month, Category_MarkType markType, bool? approvedStatus, int pageCurrentIndex, int pageSize, out double totalRecord)
        {
            List<ConsideredStudentMark> consideredStudentMarks = new List<ConsideredStudentMark>(); // returned list
            ConsideredStudentMark consideredStudentMark = null;
            List<Student_DetailedTermSubjectMark> detailedMarks = new List<Student_DetailedTermSubjectMark>();
            StringBuilder strB = new StringBuilder();
            MarkTypeBL markTypeBL = new MarkTypeBL(school);

            foreach (Category_Subject subject in subjects)
            {
                if (approvedStatus == null)
                {
                    if (markType == null)
                    {
                        detailedMarks.AddRange(studyingResultDA.GetConsideredDetailedMarks(Class, term, subject, month));
                    }
                    else
                    {
                        detailedMarks.AddRange(studyingResultDA.GetConsideredDetailedMarks(Class, term, subject, markType, month));
                    }
                }
                else
                {
                    if (markType == null)
                    {
                        detailedMarks.AddRange(studyingResultDA.GetConsideredDetailedMarks(Class, term, subject, month, (bool)approvedStatus));
                    }
                    else
                    {
                        detailedMarks.AddRange(studyingResultDA.GetConsideredDetailedMarks(Class, term, subject, markType, month, (bool)approvedStatus));
                    }
                }
            }

            totalRecord = detailedMarks.Count;
            if (detailedMarks.Count != 0)
            {
                totalRecord = detailedMarks.Count();
                if (totalRecord != 0)
                {
                    detailedMarks = detailedMarks.OrderBy(detail => detail.Student_TermSubjectMark.Student_StudentInClass.Student_Student.StudentCode)
                        .ThenBy(detail => detail.Student_TermSubjectMark.Student_StudentInClass.Student_Student.FullName)
                        .ThenBy(detail => detail.Category_MarkType.MarkTypeId).ThenByDescending(detail => detail.Date)
                        .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
                }

                consideredStudentMark = new ConsideredStudentMark();
                consideredStudentMark.Approved = detailedMarks[0].Approved;
                consideredStudentMark.Date = detailedMarks[0].Date;
                consideredStudentMark.DetailTermSubjectMarkId = detailedMarks[0].DetailedTermSubjectMark;
                consideredStudentMark.MarkTypeId = detailedMarks[0].MarkType;
                consideredStudentMark.MarkTypeName = detailedMarks[0].Category_MarkType.MarkTypeName;
                consideredStudentMark.MarkValue = detailedMarks[0].MarkValue;
                consideredStudentMark.StudentCode = detailedMarks[0].Student_TermSubjectMark.Student_StudentInClass.Student_Student.StudentCode;
                consideredStudentMark.StudentName = detailedMarks[0].Student_TermSubjectMark.Student_StudentInClass.Student_Student.FullName;
                consideredStudentMark.Note = detailedMarks[0].Note;
                consideredStudentMark.SubjectName = detailedMarks[0].Student_TermSubjectMark.Category_Subject.SubjectName;
                consideredStudentMarks.Add(consideredStudentMark);
            }

            for (int i = 1; i < detailedMarks.Count; i++)
            {
                consideredStudentMark = new ConsideredStudentMark();
                consideredStudentMark.Approved = detailedMarks[i].Approved;
                consideredStudentMark.Date = detailedMarks[i].Date;
                consideredStudentMark.DetailTermSubjectMarkId = detailedMarks[i].DetailedTermSubjectMark;
                consideredStudentMark.MarkTypeId = detailedMarks[i].MarkType;
                consideredStudentMark.MarkTypeName = detailedMarks[i].Category_MarkType.MarkTypeName;
                consideredStudentMark.MarkValue = detailedMarks[i].MarkValue;
                consideredStudentMark.StudentCode = detailedMarks[i].Student_TermSubjectMark.Student_StudentInClass.Student_Student.StudentCode;
                consideredStudentMark.StudentName = detailedMarks[i].Student_TermSubjectMark.Student_StudentInClass.Student_Student.FullName;
                consideredStudentMark.SubjectName = detailedMarks[i].Student_TermSubjectMark.Category_Subject.SubjectName;
                //if (consideredStudentMark.StudentCode == consideredStudentMarks[i - 1].StudentCode)
                //{
                //    consideredStudentMark.StudentCode = "";
                //}

                //if (consideredStudentMark.StudentName == consideredStudentMarks[i - 1].StudentName)
                //{
                //    consideredStudentMark.StudentName = "";
                //}
                consideredStudentMark.Note = detailedMarks[i].Note;
                consideredStudentMarks.Add(consideredStudentMark);
            }

            return consideredStudentMarks;
        }

        public List<TabularStudentMark> GetTabularStudentMarks(Class_Class Class, Category_Subject subject, Configuration_Term term, DateTime beginDate, DateTime endDate, List<Category_MarkType> markTypes, bool? approved, int pageCurrentIndex, int pageSize, out double totalRecord)
        {
            List<TabularStudentMark> tabularStudentMarks = new List<TabularStudentMark>(); // returned list
            TabularStudentMark tabularStudentMark = null;
            List<Student_TermSubjectMark> termSubjectMarks = null;
            List<MarkTypedMark> markMypedMarks = null;
            List<Student_DetailedTermSubjectMark> detailedMarks = null;
            StringBuilder strB = new StringBuilder();
            string strMarks = "";
            MarkTypeBL markTypeBL = new MarkTypeBL(school);

            termSubjectMarks = studyingResultDA.GetTermSubjectedMarks(Class, subject, term, pageCurrentIndex, pageSize, out totalRecord);

            foreach (Student_TermSubjectMark termSubjectMark in termSubjectMarks)
            {
                tabularStudentMark = new TabularStudentMark();
                tabularStudentMark.MaDiemHK = termSubjectMark.TermSubjectMarkId;
                tabularStudentMark.MaHocSinh = termSubjectMark.Student_StudentInClass.StudentId;
                tabularStudentMark.MaHocSinhHienThi = termSubjectMark.Student_StudentInClass.Student_Student.StudentCode;
                tabularStudentMark.TenHocSinh = termSubjectMark.Student_StudentInClass.Student_Student.FullName;
                tabularStudentMark.DiemTrungBinh = termSubjectMark.AverageMark;
                tabularStudentMark.TermName = termSubjectMark.Configuration_Term.TermName;
                markMypedMarks = new List<MarkTypedMark>();
                foreach (Category_MarkType markType in markTypes)
                {
                    List<double> dMarks = new List<double>();
                    if (approved == null)
                    {
                        detailedMarks = studyingResultDA.GetDetailedMarksWithAllStatuses(termSubjectMark, markType, beginDate, endDate);
                    }
                    else
                    {
                        detailedMarks = studyingResultDA.GetDetailedMarks(termSubjectMark, markType, beginDate, endDate, (bool)approved);
                    }
                    strMarks = "";

                    foreach (Student_DetailedTermSubjectMark detailedMark in detailedMarks)
                    {
                        strB.Append(detailedMark.MarkValue.ToString());
                        strB.Append(", ");
                    }

                    strMarks = strB.ToString().Trim().Trim(new char[] { ',' });
                    strB.Clear();

                    MarkTypedMark markTypedMark = new MarkTypedMark();
                    markTypedMark.MarkTypeId = markType.MarkTypeId;
                    markTypedMark.MarkTypeName = markType.MarkTypeName;
                    markTypedMark.StringDiems = strMarks;

                    markMypedMarks.Add(markTypedMark);
                }

                tabularStudentMark.DiemTheoLoaiDiems = markMypedMarks;
                tabularStudentMarks.Add(tabularStudentMark);
            }

            return tabularStudentMarks;
        }

        public ADDINGMARKERROR ValidateMark(Student_Student student, Configuration_Year year, Configuration_Term term, Category_Subject subject, Category_MarkType markType, string markTexts)
        {
            // declare variables
            double dMark;
            List<Student_DetailedTermSubjectMark> existedDetailedTermSubjectMarks;

            markTexts = markTexts.Trim();

            if (!CheckUntils.IsNullOrBlank(markTexts)) // Only validate when markTexts is not blank
            {
                string[] strMarkElements = markTexts.Split(',');
                short sMarkCount = 0; // count of mark

                // loop in each mark
                foreach (string strMark in strMarkElements)
                {
                    dMark = 0;
                    if (double.TryParse(strMark.Trim(), out dMark))
                    {
                        if (dMark > 10) // mark over 10
                        {
                            return ADDINGMARKERROR.EXCEEDVALUETEN;
                        }
                        else
                        {
                            sMarkCount++;
                        }
                    }
                    else
                    {
                        // mark is not a double
                        return ADDINGMARKERROR.NOTANUMBER;
                    }
                }

                existedDetailedTermSubjectMarks = studyingResultDA.GetDetailedTermSubjectMarks(student, year, term, subject, markType);
                sMarkCount += (short)existedDetailedTermSubjectMarks.Count;

                MarkTypeBL markTypeBL = new MarkTypeBL(school);
                markType = markTypeBL.GetMarkType(markType.MarkTypeId);
                if (sMarkCount > markType.MaxQuantity)
                {
                    return ADDINGMARKERROR.EXCEEDQUANTITY;
                }
            }

            return ADDINGMARKERROR.NOERROR;
        }

        public bool MarkExceedQuantity(Student_Student student, Configuration_Year year, Configuration_Term term, Category_Subject subject, Category_MarkType markType)
        {
            // declare variables
            List<Student_DetailedTermSubjectMark> existedDetailedTermSubjectMarks;

            existedDetailedTermSubjectMarks = studyingResultDA.GetDetailedTermSubjectMarks(student, year, term, subject, markType);

            MarkTypeBL markTypeBL = new MarkTypeBL(school);
            markType = markTypeBL.GetMarkType(markType.MarkTypeId);
            if (existedDetailedTermSubjectMarks.Count == markType.MaxQuantity)
            {
                return true;
            }

            return false;
        }

        private List<List<double>> GetApprovedDoubleSubjectMarks(Student_TermSubjectMark termSubjectedMark)
        {
            termSubjectedMark = studyingResultDA.GetTermSubjectedMark(termSubjectedMark.TermSubjectMarkId);
            MarkTypeBL markTypeBL = new MarkTypeBL(school);
            List<Category_MarkType> markTypes = markTypeBL.GetListMarkTypes(termSubjectedMark.Student_StudentInClass.Class_Class.Category_Grade);

            List<List<double>> subjectMarks = new List<List<double>>();
            List<double> innerSubjectMarks = null;
            List<Student_DetailedTermSubjectMark> detailMarks;

            foreach (Category_MarkType markType in markTypes)
            {
                detailMarks = studyingResultDA.GetDetailedMarks(termSubjectedMark, markType, true);
                innerSubjectMarks = new List<double>();
                foreach (Student_DetailedTermSubjectMark detailMark in detailMarks)
                {
                    innerSubjectMarks.Add(detailMark.MarkValue);
                }
                subjectMarks.Add(innerSubjectMarks);
            }

            return subjectMarks;
        }

        internal void InsertTermSubjectMark(Class_Class Class, Category_Subject subject, Configuration_Term term)
        {
            studyingResultDA.InsertTermSubjectMark(Class, subject, term);
        }

        internal void DeleteTermSubjectMark(Class_Class Class, Configuration_Term term, Category_Subject subject)
        {
            studyingResultDA.DeleteTermSubjectMark(Class, subject, term);
        }

        internal void InsertTermSubjectMark(Student_StudentInClass studentInClass, Configuration_Term term, Category_Subject scheduledSubject)
        {
            Student_TermSubjectMark termSubjectMark = null;
            StudentBL studentBL = new StudentBL(school);

            termSubjectMark = new Student_TermSubjectMark();
            termSubjectMark.StudentInClassId = studentInClass.StudentInClassId;
            termSubjectMark.SubjectId = scheduledSubject.SubjectId;
            termSubjectMark.TermId = term.TermId;
            termSubjectMark.AverageMark = -1; // undefined

            studyingResultDA.InsertTermSubjectMark(termSubjectMark);
        }

        internal bool DetailTermSubjectMarkExists(Category_MarkType markType)
        {
            return studyingResultDA.DetailTermSubjectMarkExists(markType);
        }

        internal bool TermLearningResultExists(Category_LearningAptitude learningAptitude)
        {
            return studyingResultDA.TermLearningResultExists(learningAptitude);
        }

        internal void DeleteStudyingResult(Student_Student deletedStudent)
        {
            studyingResultDA.DeleteStudyingResult(deletedStudent);
        }

        public List<TabularTermSubjectMark> GetTabularTermSubjectMarks(Student_Student student, List<Category_MarkType> markTypes,
            Category_Subject subject, Class_Class Class, Configuration_Term term)
        {
            List<TabularTermSubjectMark> tabularTermSubjectMarks = new List<TabularTermSubjectMark>();
            TabularTermSubjectMark tabularTermSubjectMark = null;
            List<Student_DetailedTermSubjectMark> detailedTermSubjectMarks = new List<Student_DetailedTermSubjectMark>();
            TabularDetailTermSubjectMark tabularDetailTermSubjectMark = null;

            foreach (Category_MarkType markType in markTypes)
            {
                tabularTermSubjectMark = new TabularTermSubjectMark();
                tabularTermSubjectMark.MarkTypeId = markType.MarkTypeId;
                tabularTermSubjectMark.MarkTypeName = markType.MarkTypeName;
                detailedTermSubjectMarks = studyingResultDA.GetDetailedTermSubjectMarks(student, markType, subject, Class, term);
                tabularTermSubjectMark.TabularDetailTermSubjectMarks = new List<TabularDetailTermSubjectMark>();
                foreach (Student_DetailedTermSubjectMark detailedTermSubjectMark in detailedTermSubjectMarks)
                {
                    tabularDetailTermSubjectMark = new TabularDetailTermSubjectMark();
                    tabularDetailTermSubjectMark.DetailTermSubjectMarkId = detailedTermSubjectMark.DetailedTermSubjectMark;
                    tabularDetailTermSubjectMark.Date = detailedTermSubjectMark.Date;
                    tabularDetailTermSubjectMark.MarkValue = detailedTermSubjectMark.MarkValue;
                    tabularTermSubjectMark.TabularDetailTermSubjectMarks.Add(tabularDetailTermSubjectMark);
                }

                tabularTermSubjectMarks.Add(tabularTermSubjectMark);
            }

            return tabularTermSubjectMarks;
        }

        public void UpdateDetailedMarks(Student_Student student, Class_Class Class, Configuration_Term term, Category_Subject subject, List<TabularTermSubjectMark> tabularTermSubjectMarks)
        {
            List<Student_DetailedTermSubjectMark> detailedTermSubjectMarks = new List<Student_DetailedTermSubjectMark>();
            Student_DetailedTermSubjectMark detailedTermSubjectMark = null;
            foreach (TabularTermSubjectMark tabularTermSubjectMark in tabularTermSubjectMarks)
            {
                foreach (TabularDetailTermSubjectMark tabularDetailTermSubjectMark in tabularTermSubjectMark.TabularDetailTermSubjectMarks)
                {
                    detailedTermSubjectMark = new Student_DetailedTermSubjectMark();
                    detailedTermSubjectMark.DetailedTermSubjectMark = tabularDetailTermSubjectMark.DetailTermSubjectMarkId;
                    detailedTermSubjectMark.MarkType = tabularTermSubjectMark.MarkTypeId;
                    detailedTermSubjectMark.MarkValue = tabularDetailTermSubjectMark.MarkValue;

                    if (detailedTermSubjectMark.MarkValue != -1)
                    {
                        // update mark
                        studyingResultDA.UpdateDetailedMark(detailedTermSubjectMark);
                    }
                    else
                    {
                        // delete mark
                        studyingResultDA.DeleteDetailedMark(detailedTermSubjectMark);
                    }

                    // send mail here
                }
            }

            if (NeedResetAvgMark(student, Class, term, subject))
            {
                studyingResultDA.ResetAvgMark(student, Class, term, subject);
            }
            else
            {
                studyingResultDA.CalculateTermSubjectAvgMark(student, Class, term, subject);
            }

            studyingResultDA.CalculateTermAvgMark(student, Class, term);
        }

        public List<Category_Subject> HasNewMarks(Student_Student student)
        {
            int iLimitDays = 3;

            // declare BLs
            StudentBL studentBL = new StudentBL(school);

            // get lasted class that student joins
            Class_Class Class = studentBL.GetLastedClass(student);

            // get list of subjects that have new mark
            return studyingResultDA.GetNewMarkSubjects(student, Class, iLimitDays);
        }

        public List<Category_Subject> HasNewFinalMarks(Student_Student student)
        {
            List<Category_Subject> hasNewFinalMarkSubjects = new List<Category_Subject>();
            int iLimitDays = 3;

            // declare BLs
            StudentBL studentBL = new StudentBL(school);
            MarkTypeBL markTypeBL = new MarkTypeBL(school);

            // get lasted class that student joins
            Class_Class Class = studentBL.GetLastedClass(student);

            // get markType that used to calculate average mark
            Category_MarkType markType = markTypeBL.GetAppliedCalAvgMarkType(Class.Category_Grade);
            if (markType != null)
            {
                // get list of subjects that have new final mark
                hasNewFinalMarkSubjects = studyingResultDA.GetNewMarkSubjects(student, Class, iLimitDays, markType);
            }

            return hasNewFinalMarkSubjects;
        }

        public void UnapproveMark(Student_DetailedTermSubjectMark detailedTermSubjectMark, string note)
        {
            studyingResultDA.ChangeApproveStatusMark(detailedTermSubjectMark, false, note);
        }

        public void ApproveMark(Student_DetailedTermSubjectMark detailedTermSubjectMark, string note)
        {
            studyingResultDA.ChangeApproveStatusMark(detailedTermSubjectMark, true, note);
        }

        public int GetTotalUnapprovedMarks(Class_Class Class)
        {
            return studyingResultDA.GetTotalUnapprovedMarks(Class);
        }
    }
}
