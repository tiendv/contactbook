using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class StudyingResultBL : BaseBL
    {
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

        //public void InsertDetailedMark(Student_Student student, Class_Class Class, Configuration_Term, Category_Subject subject, Category_MarkType markType, double mark)
        //{
        //    studyingResultDA.InsertDetailedMark(student, Class, term, subject, markType, mark);
        //}

        public void InsertDetailedMark(Student_TermSubjectMark termSubjectedMark, Category_MarkType markType, double mark)
        {
            Student_DetailedTermSubjectMark detailedMark = new Student_DetailedTermSubjectMark();
            detailedMark.TermSubjectMarkId = termSubjectedMark.TermSubjectMarkId;
            detailedMark.MarkType = markType.MarkTypeId;
            detailedMark.MarkValue = mark;

            studyingResultDA.InsertDetailMark(detailedMark);
            studyingResultDA.CalAvgMark(termSubjectedMark);
        }

        /// <summary>
        /// Update student's marks
        /// </summary>
        /// <param name="student"></param>
        /// <param name="Class"></param>
        /// <param name="term"></param>
        /// <param name="subject"></param>
        /// <param name="marks"></param>
        public void UpdateDetailedMark(Student_Student student, Class_Class Class, Configuration_Term term, Category_Subject subject, List<DetailMark> marks)
        {
            List<Category_MarkType> markTypes = new List<Category_MarkType>();
            Category_MarkType markType = null;
            List<int> markTypeIds = new List<int>();

            // Lấy danh sách phân biệt LoaiDiem
            foreach (DetailMark mark in marks)
            {
                markType = new Category_MarkType();
                markType.MarkTypeId = mark.MarkTypeId;
                markTypes.Add(markType);
            }
            markTypes = markTypes.GroupBy(c => c.MarkTypeId).Select(g => g.First()).ToList();

            // Xoa cac diem cua hoc sinh theo MarkTypeId
            studyingResultDA.DeleteDetailedMark(student, Class, term, subject, markTypes);

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

            studyingResultDA.InsertDetailedMark(student, Class, term, subject, marks);
            if (NeedResetAvgMark(student, Class, term, subject))
            {
                studyingResultDA.ResetAvgMark(student, Class, term, subject);
            }
            else
            {
                studyingResultDA.CalAvgMark(student, Class, term, subject);
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

        public void UpdateDetailedMark(Student_DetailedTermSubjectMark editedDetailedMark, double mark)
        {
            editedDetailedMark.MarkValue = mark;
            studyingResultDA.UpdateDetailedMark(editedDetailedMark);
        }

        public void UpdateStudentTermResult(Student_TermLearningResult editedTermResult, Category_Conduct conduct)
        {
            editedTermResult.TermConductId = conduct.ConductId;
            studyingResultDA.UpdateStudentTermResult(editedTermResult);
        }

        public void DeleteDetailedMark(Student_DetailedTermSubjectMark deletedDetailedMark)
        {
            studyingResultDA.DeleteDetailedMark(deletedDetailedMark);
        }

        public Student_DetailedTermSubjectMark GetDetailedMark(int detailedMarkId)
        {
            return studyingResultDA.GetDetailedMark(detailedMarkId);
        }

        public List<TabularSubjectTermResult> GetTabularSubjectTermResults(Student_Student student, Configuration_Year year, Configuration_Term term, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularSubjectTermResult> tabularSubjectTermResults = new List<TabularSubjectTermResult>();
            TabularSubjectTermResult tabularSubjectTermResult = null;
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

        public List<StrDiemMonHocLoaiDiem> GetSubjectMarks(Student_TermSubjectMark termSubjectedMark)
        {
            List<StrDiemMonHocLoaiDiem> lstStringDiemMonHoc = new List<StrDiemMonHocLoaiDiem>();

            List<List<double>> lstDiemMonHoc = GetDoubleSubjectMarks(termSubjectedMark);
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
                lstStringDiemMonHoc.Add(strDiemMonHocLoaiDiem);
            }
            return lstStringDiemMonHoc;
        }

        public Student_Student GetStudent(Student_TermSubjectMark termSubjectedMark)
        {
            Student_TermSubjectMark termSubjMark = studyingResultDA.GetTermSubjectedMark(termSubjectedMark.TermSubjectMarkId);
            return termSubjectedMark.Student_StudentInClass.Student_Student;
        }

        public Configuration_Year GetYear(Student_TermSubjectMark termSubjectedMark)
        {
            Student_TermSubjectMark termSubjMark = studyingResultDA.GetTermSubjectedMark(termSubjectedMark.TermSubjectMarkId);
            return termSubjectedMark.Student_StudentInClass.Class_Class.Configuration_Year;
        }

        public Configuration_Term GetTerm(Student_TermSubjectMark termSubjectedMark)
        {
            Student_TermSubjectMark termSubjMark = studyingResultDA.GetTermSubjectedMark(termSubjectedMark.TermSubjectMarkId);
            return termSubjectedMark.Configuration_Term;
        }

        public Category_Subject GetSubject(Student_TermSubjectMark termSubjectedMark)
        {
            Student_TermSubjectMark termSubjMark = studyingResultDA.GetTermSubjectedMark(termSubjectedMark.TermSubjectMarkId);
            return termSubjectedMark.Category_Subject;
        }

        public double GetAVGMark(Student_TermSubjectMark termSubjectedMark)
        {
            Student_TermSubjectMark termSubjMark = studyingResultDA.GetTermSubjectedMark(termSubjectedMark.TermSubjectMarkId);
            return termSubjectedMark.AverageMark;
        }

        public List<TabularChiTietDiemMonHocLoaiDiem> GetListTabularChiTietDiemMonHocLoaiDiem(Student_TermSubjectMark termSubjectedMark, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularChiTietDiemMonHocLoaiDiem> tbChiTietDiemMonHocLoaiDiems = new List<TabularChiTietDiemMonHocLoaiDiem>();
            List<Student_DetailedTermSubjectMark> detailedMarks = studyingResultDA.GetDetailedMarks(termSubjectedMark, pageCurrentIndex, pageSize, out totalRecords);
            TabularChiTietDiemMonHocLoaiDiem tbChiTietDiemMonHocLoaiDiem = null;
            foreach (Student_DetailedTermSubjectMark detailedMark in detailedMarks)
            {
                tbChiTietDiemMonHocLoaiDiem = new TabularChiTietDiemMonHocLoaiDiem();
                tbChiTietDiemMonHocLoaiDiem.MaChiTietDiem = detailedMark.DetailedTermSubjectMark;
                tbChiTietDiemMonHocLoaiDiem.MarkTypeName = detailedMark.Category_MarkType.MarkTypeName;
                tbChiTietDiemMonHocLoaiDiem.Diem = detailedMark.MarkValue;

                tbChiTietDiemMonHocLoaiDiems.Add(tbChiTietDiemMonHocLoaiDiem);
            }

            return tbChiTietDiemMonHocLoaiDiems;
        }

        public List<TabularTermStudentResult> GetTabularTermStudentResults(Student_Student student, Configuration_Year year, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            LearningAptitudeBL hocLucBL = null;
            ConductBL conductBL = null;
            DanhHieuBL danhHieuBL = null;
            List<Student_TermLearningResult> termResults = null;
            List<TabularTermStudentResult> tbTermStudentResults = new List<TabularTermStudentResult>();
            TabularTermStudentResult tbTermStudentResult = null;
            TabularTermStudentResult tbFinalStudentResult = null;

            termResults = studyingResultDA.GetStudentTermResults(student, year, pageCurrentIndex, pageSize, out totalRecords);
            if (totalRecords != 0)
            {
                hocLucBL = new LearningAptitudeBL(school);
                conductBL = new ConductBL(school);
                danhHieuBL = new DanhHieuBL(school);
                foreach (Student_TermLearningResult termResult in termResults)
                {
                    tbTermStudentResult = new TabularTermStudentResult();
                    tbTermStudentResult.LearningResultIdHSHK = termResult.TermLearningResultId;
                    tbTermStudentResult.DiemTB = (int)termResult.TermAverageMark;
                    tbTermStudentResult.StrDiemTB = (termResult.TermAverageMark != -1) ? (termResult.TermAverageMark.ToString()) : "";
                    tbTermStudentResult.TermName = termResult.Configuration_Term.TermName;
                    tbTermStudentResult.ConductId = termResult.TermConductId;
                    int ConductId = (int)tbTermStudentResult.ConductId;
                    tbTermStudentResult.ConductName = (ConductId != -1) ? conductBL.GetConduct(ConductId).ConductName : "";
                    int LearningAptitudeId = (int)termResult.TermLearningAptitudeId;
                    tbTermStudentResult.LearningAptitudeName = (LearningAptitudeId != -1) ? hocLucBL.GetHocLuc(LearningAptitudeId).LearningAptitudeName : "";
                    //tbTermStudentResult.LearningResultName = danhHieuBL.GetLearningResultName(LearningAptitudeId, ConductId);

                    tbTermStudentResults.Add(tbTermStudentResult);
                }

                tbFinalStudentResult = new TabularTermStudentResult();
                tbFinalStudentResult.LearningResultIdHSHK = -1;
                tbFinalStudentResult.TermName = "Cả năm";
                if ((tbTermStudentResults[0].DiemTB != -1) && (tbTermStudentResults[1].DiemTB != -1))
                {
                    tbFinalStudentResult.DiemTB = Math.Round(((tbTermStudentResults[0].DiemTB + (2 * tbTermStudentResults[1].DiemTB)) / 3), 1);
                    tbFinalStudentResult.StrDiemTB = tbFinalStudentResult.DiemTB.ToString();
                }
                else
                {
                    tbFinalStudentResult.DiemTB = -1;
                    tbFinalStudentResult.StrDiemTB = "";
                }

                if (tbTermStudentResults[0].ConductId != -1 && tbTermStudentResults[1].ConductId != -1)
                {
                    tbFinalStudentResult.ConductId = tbTermStudentResults[1].ConductId;
                }
                else
                {
                    tbFinalStudentResult.ConductId = -1;
                }
                int ConductIdCuoiNam = (int)tbFinalStudentResult.ConductId;
                tbFinalStudentResult.ConductName = (ConductIdCuoiNam != -1) ? conductBL.GetConduct(ConductIdCuoiNam).ConductName : "";

                int LearningAptitudeIdCuoiNam;
                if (tbFinalStudentResult.DiemTB != -1)
                {
                    //Category_LearningAptitude hocLuc = hocLucBL.GetHocLuc(tbFinalStudentResult.DiemTB);
                    //LearningAptitudeIdCuoiNam = hocLuc.LearningAptitudeId;
                    //tbFinalStudentResult.LearningAptitudeName = hocLuc.LearningAptitudeName;
                }
                else
                {
                    //LearningAptitudeIdCuoiNam = -1;
                    //tbFinalStudentResult.LearningAptitudeName = "";
                }

                //tbFinalStudentResult.LearningResultName = danhHieuBL.GetLearningResultName(LearningAptitudeIdCuoiNam, ConductIdCuoiNam);

                tbTermStudentResults.Add(tbFinalStudentResult);
            }

            return tbTermStudentResults;
        }

        public List<TabularStudentMark> GetTabularStudentMarks(Class_Class Class, Category_Subject subject, Configuration_Term term, List<Category_MarkType> markTypes, int pageCurrentIndex, int pageSize, out double totalRecord)
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

                markMypedMarks = new List<MarkTypedMark>();
                foreach (Category_MarkType markType in markTypes)
                {
                    List<double> dMarks = new List<double>();
                    detailedMarks = studyingResultDA.GetDetailedMarks(termSubjectMark, markType);
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

        //public List<TabularDiemHocSinh> GetListDiemHocSinh(int ClassId, int SubjectId,
        //    int TermId, int MarkTypeId,
        //    int pageCurrentIndex, int pageSize, out double totalRecord)
        //{
        //    MarkTypeBL loaiDiemBL = new MarkTypeBL();
        //    List<Category_MarkType> lLoaiDiems = loaiDiemBL.GetListLoaiDiem(MarkTypeId);
        //    return kqhtDA.GetListDiemHocSinh(ClassId, SubjectId, TermId,
        //        lLoaiDiems,
        //        pageCurrentIndex, pageSize, out totalRecord);
        //}

        //public bool ValidateMark(string marks, int markTypeCode)
        //{
        //    marks = marks.Trim();
        //    if (marks != "")
        //    {
        //        string[] strMarks = marks.Split(',');
        //        short totalMarkCount = 0;
        //        foreach (string strMark in strMarks)
        //        {
        //            double dMark = -1;
        //            if (double.TryParse(strMark.Trim(), out dMark))
        //            {
        //                if (dMark > 10)
        //                {
        //                    return false;
        //                }
        //                else
        //                {
        //                    totalMarkCount++;
        //                }
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }

        //        MarkTypeBL loaiDiemBL = new MarkTypeBL();
        //        Category_MarkType loaiDiem = loaiDiemBL.GetLoaiDiem(markTypeCode);
        //        if (totalMarkCount > loaiDiem.MaxQuantity)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        public bool ValidateMark(string marks, Category_MarkType markType)
        {
            bool bValid = true;

            marks = marks.Trim();
            if (marks != "") // Only validate when marks has value
            {
                string[] strMarks = marks.Split(',');
                short markCount = 0;

                // loop in each mark
                foreach (string strMark in strMarks)
                {
                    double dMark = 0;
                    if (double.TryParse(strMark.Trim(), out dMark))
                    {
                        if (dMark > 10) // mark over 10
                        {
                            bValid = false;
                            break;
                        }
                        else
                        {
                            markCount++;
                        }
                    }
                    else
                    {
                        // mark is not a double
                        bValid = false;
                        break;
                    }
                }

                if (bValid)
                {
                    MarkTypeBL markTypeBL = new MarkTypeBL(school);
                    markType = markTypeBL.GetMarkType(markType.MarkTypeId);
                    if (markCount > markType.MaxQuantity)
                    {
                        bValid = false;
                    }
                }
            }

            return bValid;
        }

        private List<List<double>> GetDoubleSubjectMarks(Student_TermSubjectMark termSubjectedMark)
        {
            MarkTypeBL markTypeBL = new MarkTypeBL(school);
            List<Category_MarkType> markTypes = markTypeBL.GetListMarkTypes();

            List<List<double>> subjectMarks = new List<List<double>>();
            List<double> innerSubjectMarks = null;
            List<Student_DetailedTermSubjectMark> detailMarks;

            foreach (Category_MarkType loaiDiem in markTypes)
            {
                detailMarks = studyingResultDA.GetDetailedMarks(termSubjectedMark, loaiDiem);
                innerSubjectMarks = new List<double>();
                foreach (Student_DetailedTermSubjectMark detailMark in detailMarks)
                {
                    innerSubjectMarks.Add(detailMark.MarkValue);
                }
                subjectMarks.Add(innerSubjectMarks);
            }

            return subjectMarks;
        }
    }
}
