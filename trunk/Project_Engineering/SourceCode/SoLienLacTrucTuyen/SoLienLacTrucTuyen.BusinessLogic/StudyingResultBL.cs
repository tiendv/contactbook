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

        public StudyingResultBL(School school)
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
        public void InsertTermStudyingResult(CauHinh_HocKy term, HocSinh_HocSinhLopHoc studentInClass, double termAvgMark, DanhMuc_HanhKiem termConduct, DanhMuc_HocLuc termStudyingAptitude)
        {
            HocSinh_DanhHieuHocKy termStudyingResult = new HocSinh_DanhHieuHocKy
            {
                MaHocKy = term.MaHocKy,
                MaHocSinhLopHoc = studentInClass.MaHocSinhLopHoc,
                DiemTBHK = termAvgMark,
                MaHanhKiemHK = termConduct.MaHanhKiem,
                MaHocLucHK = termStudyingAptitude.MaHocLuc
            };

            studyingResultDA.InsertTermStudyingResult(termStudyingResult);
        }

        //public void InsertDetailedMark(HocSinh_ThongTinCaNhan student, LopHoc_Lop Class, CauHinh_HocKy term, DanhMuc_MonHoc subject, DanhMuc_LoaiDiem markType, double mark)
        //{
        //    studyingResultDA.InsertDetailedMark(student, Class, term, subject, markType, mark);
        //}

        public void InsertDetailedMark(HocSinh_DiemMonHocHocKy termSubjectedMark, DanhMuc_LoaiDiem markType, double mark)
        {
            HocSinh_ChiTietDiem detailedMark = new HocSinh_ChiTietDiem();
            detailedMark.MaDiemMonHK = termSubjectedMark.MaDiemMonHK;
            detailedMark.MaLoaiDiem = markType.MaLoaiDiem;
            detailedMark.Diem = mark;

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
        public void UpdateDetailedMark(HocSinh_ThongTinCaNhan student, LopHoc_Lop Class, CauHinh_HocKy term, DanhMuc_MonHoc subject, List<DetailMark> marks)
        {
            List<DanhMuc_LoaiDiem> markTypes = new List<DanhMuc_LoaiDiem>();
            DanhMuc_LoaiDiem markType = null;
            List<int> markTypeIds = new List<int>();

            // Lấy danh sách phân biệt LoaiDiem
            foreach (DetailMark mark in marks)
            {
                markType = new DanhMuc_LoaiDiem();
                markType.MaLoaiDiem = mark.MaLoaiDiem;
                markTypes.Add(markType);
            }
            markTypes = markTypes.GroupBy(c => c.MaLoaiDiem).Select(g => g.First()).ToList();

            // Xoa cac diem cua hoc sinh theo MaLoaiDiem
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
        private bool NeedResetAvgMark(HocSinh_ThongTinCaNhan student, LopHoc_Lop Class, CauHinh_HocKy term, DanhMuc_MonHoc subject)
        {
            return studyingResultDA.NeedResetAvgMark(student, Class, term, subject);
        }

        public void UpdateDetailedMark(HocSinh_ChiTietDiem editedDetailedMark, double mark)
        {
            editedDetailedMark.Diem = mark;
            studyingResultDA.UpdateDetailedMark(editedDetailedMark);
        }

        public void UpdateStudentTermResult(HocSinh_DanhHieuHocKy editedTermResult, DanhMuc_HanhKiem conduct)
        {
            editedTermResult.MaHanhKiemHK = conduct.MaHanhKiem;
            studyingResultDA.UpdateStudentTermResult(editedTermResult);
        }

        public void DeleteDetailedMark(HocSinh_ChiTietDiem deletedDetailedMark)
        {
            studyingResultDA.DeleteDetailedMark(deletedDetailedMark);
        }

        public HocSinh_ChiTietDiem GetDetailedMark(int detailedMarkId)
        {
            return studyingResultDA.GetDetailedMark(detailedMarkId);
        }

        public List<TabularSubjectTermResult> GetTabularSubjectTermResults(HocSinh_ThongTinCaNhan student, CauHinh_NamHoc year, CauHinh_HocKy term, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularSubjectTermResult> tabularSubjectTermResults = new List<TabularSubjectTermResult>();
            TabularSubjectTermResult tabularSubjectTermResult = null;
            List<DanhMuc_MonHoc> scheduledSubjects = studyingResultDA.GetScheduledSubjects(student, year, term);
            HocSinh_DiemMonHocHocKy termSubjectedMark;
            foreach (DanhMuc_MonHoc scheduledSubject in scheduledSubjects)
            {
                termSubjectedMark = studyingResultDA.GetTermSubjectedMark(student, year, term, scheduledSubject);
                tabularSubjectTermResult = new TabularSubjectTermResult();
                tabularSubjectTermResult.TenMonHoc = scheduledSubject.TenMonHoc;
                tabularSubjectTermResult.MaDiemMonHK = termSubjectedMark.MaDiemMonHK;
                tabularSubjectTermResult.DiemTB = termSubjectedMark.DiemTB;

                tabularSubjectTermResults.Add(tabularSubjectTermResult);
            }

            totalRecords = tabularSubjectTermResults.Count();
            if (totalRecords != 0)
            {
                tabularSubjectTermResults = tabularSubjectTermResults.OrderBy(ketQua => ketQua.TenMonHoc)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
                foreach (TabularSubjectTermResult tbSubjectTermResult in tabularSubjectTermResults)
                {
                    tbSubjectTermResult.StrDiemTB = (tbSubjectTermResult.DiemTB != -1) ? tbSubjectTermResult.DiemTB.ToString() : "";
                }
            }

            return tabularSubjectTermResults;
        }

        public List<StrDiemMonHocLoaiDiem> GetSubjectMarks(HocSinh_DiemMonHocHocKy termSubjectedMark)
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

        public HocSinh_ThongTinCaNhan GetStudent(HocSinh_DiemMonHocHocKy termSubjectedMark)
        {
            HocSinh_DiemMonHocHocKy termSubjMark = studyingResultDA.GetTermSubjectedMark(termSubjectedMark.MaDiemMonHK);
            return termSubjectedMark.HocSinh_HocSinhLopHoc.HocSinh_ThongTinCaNhan;
        }

        public CauHinh_NamHoc GetYear(HocSinh_DiemMonHocHocKy termSubjectedMark)
        {
            HocSinh_DiemMonHocHocKy termSubjMark = studyingResultDA.GetTermSubjectedMark(termSubjectedMark.MaDiemMonHK);
            return termSubjectedMark.HocSinh_HocSinhLopHoc.LopHoc_Lop.CauHinh_NamHoc;
        }

        public CauHinh_HocKy GetTerm(HocSinh_DiemMonHocHocKy termSubjectedMark)
        {
            HocSinh_DiemMonHocHocKy termSubjMark = studyingResultDA.GetTermSubjectedMark(termSubjectedMark.MaDiemMonHK);
            return termSubjectedMark.CauHinh_HocKy;
        }

        public DanhMuc_MonHoc GetSubject(HocSinh_DiemMonHocHocKy termSubjectedMark)
        {
            HocSinh_DiemMonHocHocKy termSubjMark = studyingResultDA.GetTermSubjectedMark(termSubjectedMark.MaDiemMonHK);
            return termSubjectedMark.DanhMuc_MonHoc;
        }

        public double GetAVGMark(HocSinh_DiemMonHocHocKy termSubjectedMark)
        {
            HocSinh_DiemMonHocHocKy termSubjMark = studyingResultDA.GetTermSubjectedMark(termSubjectedMark.MaDiemMonHK);
            return termSubjectedMark.DiemTB;
        }

        public List<TabularChiTietDiemMonHocLoaiDiem> GetListTabularChiTietDiemMonHocLoaiDiem(HocSinh_DiemMonHocHocKy termSubjectedMark, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularChiTietDiemMonHocLoaiDiem> tbChiTietDiemMonHocLoaiDiems = new List<TabularChiTietDiemMonHocLoaiDiem>();
            List<HocSinh_ChiTietDiem> detailedMarks = studyingResultDA.GetDetailedMarks(termSubjectedMark, pageCurrentIndex, pageSize, out totalRecords);
            TabularChiTietDiemMonHocLoaiDiem tbChiTietDiemMonHocLoaiDiem = null;
            foreach (HocSinh_ChiTietDiem detailedMark in detailedMarks)
            {
                tbChiTietDiemMonHocLoaiDiem = new TabularChiTietDiemMonHocLoaiDiem();
                tbChiTietDiemMonHocLoaiDiem.MaChiTietDiem = detailedMark.MaChiTietDiem;
                tbChiTietDiemMonHocLoaiDiem.TenLoaiDiem = detailedMark.DanhMuc_LoaiDiem.TenLoaiDiem;
                tbChiTietDiemMonHocLoaiDiem.Diem = detailedMark.Diem;

                tbChiTietDiemMonHocLoaiDiems.Add(tbChiTietDiemMonHocLoaiDiem);
            }

            return tbChiTietDiemMonHocLoaiDiems;
        }

        public List<TabularTermStudentResult> GetTabularTermStudentResults(HocSinh_ThongTinCaNhan student, CauHinh_NamHoc year, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            SystemConfigBL systemConfigBL = null;
            HocLucBL hocLucBL = null;
            ConductBL conductBL = null;
            DanhHieuBL danhHieuBL = null;
            List<HocSinh_DanhHieuHocKy> termResults = null;
            List<TabularTermStudentResult> tbTermStudentResults = new List<TabularTermStudentResult>();
            TabularTermStudentResult tbTermStudentResult = null;
            TabularTermStudentResult tbFinalStudentResult = null;

            termResults = studyingResultDA.GetStudentTermResults(student, year, pageCurrentIndex, pageSize, out totalRecords);
            if (totalRecords != 0)
            {
                hocLucBL = new HocLucBL(school);
                conductBL = new ConductBL(school);
                danhHieuBL = new DanhHieuBL(school);
                foreach (HocSinh_DanhHieuHocKy termResult in termResults)
                {
                    tbTermStudentResult = new TabularTermStudentResult();
                    tbTermStudentResult.MaDanhHieuHSHK = termResult.MaDanhHieuHSHK;
                    tbTermStudentResult.DiemTB = (int)termResult.DiemTBHK;
                    tbTermStudentResult.StrDiemTB = (termResult.DiemTBHK != -1) ? (termResult.DiemTBHK.ToString()) : "";
                    tbTermStudentResult.TenHocKy = termResult.CauHinh_HocKy.TenHocKy;
                    tbTermStudentResult.MaHanhKiem = termResult.MaHanhKiemHK;
                    int maHanhKiem = (int)tbTermStudentResult.MaHanhKiem;
                    tbTermStudentResult.TenHanhKiem = (maHanhKiem != -1) ? conductBL.GetConduct(maHanhKiem).TenHanhKiem : "";
                    int maHocLuc = (int)termResult.MaHocLucHK;
                    tbTermStudentResult.TenHocLuc = (maHocLuc != -1) ? hocLucBL.GetHocLuc(maHocLuc).TenHocLuc : "";
                    //tbTermStudentResult.TenDanhHieu = danhHieuBL.GetTenDanhHieu(maHocLuc, maHanhKiem);

                    tbTermStudentResults.Add(tbTermStudentResult);
                }

                tbFinalStudentResult = new TabularTermStudentResult();
                tbFinalStudentResult.MaDanhHieuHSHK = -1;
                tbFinalStudentResult.TenHocKy = "Cả năm";
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

                if (tbTermStudentResults[0].MaHanhKiem != -1 && tbTermStudentResults[1].MaHanhKiem != -1)
                {
                    tbFinalStudentResult.MaHanhKiem = tbTermStudentResults[1].MaHanhKiem;
                }
                else
                {
                    tbFinalStudentResult.MaHanhKiem = -1;
                }
                int maHanhKiemCuoiNam = (int)tbFinalStudentResult.MaHanhKiem;
                tbFinalStudentResult.TenHanhKiem = (maHanhKiemCuoiNam != -1) ? conductBL.GetConduct(maHanhKiemCuoiNam).TenHanhKiem : "";

                int maHocLucCuoiNam;
                if (tbFinalStudentResult.DiemTB != -1)
                {
                    //DanhMuc_HocLuc hocLuc = hocLucBL.GetHocLuc(tbFinalStudentResult.DiemTB);
                    //maHocLucCuoiNam = hocLuc.MaHocLuc;
                    //tbFinalStudentResult.TenHocLuc = hocLuc.TenHocLuc;
                }
                else
                {
                    //maHocLucCuoiNam = -1;
                    //tbFinalStudentResult.TenHocLuc = "";
                }

                //tbFinalStudentResult.TenDanhHieu = danhHieuBL.GetTenDanhHieu(maHocLucCuoiNam, maHanhKiemCuoiNam);

                tbTermStudentResults.Add(tbFinalStudentResult);
            }

            return tbTermStudentResults;
        }

        public List<TabularStudentMark> GetTabularStudentMarks(LopHoc_Lop Class, DanhMuc_MonHoc subject, CauHinh_HocKy term, List<DanhMuc_LoaiDiem> markTypes, int pageCurrentIndex, int pageSize, out double totalRecord)
        {
            List<TabularStudentMark> tabularStudentMarks = new List<TabularStudentMark>(); // returned list
            TabularStudentMark tabularStudentMark = null;            
            List<HocSinh_DiemMonHocHocKy> termSubjectMarks = null;
            List<MarkTypedMark> markMypedMarks = null;
            List<HocSinh_ChiTietDiem> detailedMarks = null;
            StringBuilder strB = new StringBuilder();
            string strMarks = "";
            MarkTypeBL markTypeBL = new MarkTypeBL(school);

            termSubjectMarks = studyingResultDA.GetTermSubjectedMarks(Class, subject, term, pageCurrentIndex, pageSize, out totalRecord);
            foreach (HocSinh_DiemMonHocHocKy termSubjectMark in termSubjectMarks)
            {
                tabularStudentMark = new TabularStudentMark();
                tabularStudentMark.MaDiemHK = termSubjectMark.MaDiemMonHK;
                tabularStudentMark.MaHocSinh = termSubjectMark.HocSinh_HocSinhLopHoc.MaHocSinh;
                tabularStudentMark.MaHocSinhHienThi = termSubjectMark.HocSinh_HocSinhLopHoc.HocSinh_ThongTinCaNhan.MaHocSinhHienThi;
                tabularStudentMark.TenHocSinh = termSubjectMark.HocSinh_HocSinhLopHoc.HocSinh_ThongTinCaNhan.HoTen;
                tabularStudentMark.DiemTrungBinh = termSubjectMark.DiemTB;

                markMypedMarks = new List<MarkTypedMark>();
                foreach (DanhMuc_LoaiDiem markType in markTypes)
                {
                    List<double> dMarks = new List<double>();
                    detailedMarks = studyingResultDA.GetDetailedMarks(termSubjectMark, markType);
                    strMarks = "";

                    foreach (HocSinh_ChiTietDiem detailedMark in detailedMarks)
                    {
                        strB.Append(detailedMark.Diem.ToString());
                        strB.Append(", ");
                    }

                    strMarks = strB.ToString().Trim().Trim(new char[] { ',' });
                    strB.Clear();

                    MarkTypedMark markTypedMark = new MarkTypedMark();
                    markTypedMark.MaLoaiDiem = markType.MaLoaiDiem;
                    markTypedMark.TenLoaiDiem = markType.TenLoaiDiem;
                    markTypedMark.StringDiems = strMarks;

                    markMypedMarks.Add(markTypedMark);
                }

                tabularStudentMark.DiemTheoLoaiDiems = markMypedMarks;
                tabularStudentMarks.Add(tabularStudentMark);
            }

            return tabularStudentMarks;
        }

        //public List<TabularDiemHocSinh> GetListDiemHocSinh(int maLopHoc, int maMonHoc,
        //    int maHocKy, int maLoaiDiem,
        //    int pageCurrentIndex, int pageSize, out double totalRecord)
        //{
        //    MarkTypeBL loaiDiemBL = new MarkTypeBL();
        //    List<DanhMuc_LoaiDiem> lLoaiDiems = loaiDiemBL.GetListLoaiDiem(maLoaiDiem);
        //    return kqhtDA.GetListDiemHocSinh(maLopHoc, maMonHoc, maHocKy,
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
        //        DanhMuc_LoaiDiem loaiDiem = loaiDiemBL.GetLoaiDiem(markTypeCode);
        //        if (totalMarkCount > loaiDiem.SoCotToiDa)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        public bool ValidateMark(string marks, DanhMuc_LoaiDiem markType)
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
                    markType = markTypeBL.GetMarkType(markType.MaLoaiDiem);
                    if (markCount > markType.SoCotToiDa)
                    {
                        bValid = false;
                    }
                }
            }

            return bValid;
        }

        private List<List<double>> GetDoubleSubjectMarks(HocSinh_DiemMonHocHocKy termSubjectedMark)
        {
            MarkTypeBL markTypeBL = new MarkTypeBL(school);
            List<DanhMuc_LoaiDiem> markTypes = markTypeBL.GetListMarkTypes();

            List<List<double>> subjectMarks = new List<List<double>>();
            List<double> innerSubjectMarks = null;
            List<HocSinh_ChiTietDiem> detailMarks;

            foreach (DanhMuc_LoaiDiem loaiDiem in markTypes)
            {
                detailMarks = studyingResultDA.GetDetailedMarks(termSubjectedMark, loaiDiem);
                innerSubjectMarks = new List<double>();
                foreach (HocSinh_ChiTietDiem detailMark in detailMarks)
                {
                    innerSubjectMarks.Add(detailMark.Diem);
                }
                subjectMarks.Add(innerSubjectMarks);
            }

            return subjectMarks;
        }
    }
}
