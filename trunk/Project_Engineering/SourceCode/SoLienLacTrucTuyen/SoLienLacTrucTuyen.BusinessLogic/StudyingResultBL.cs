using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class StudyingResultBL
    {
        private StudyingResultDA studyingResultDA;

        public StudyingResultBL()
        {
            studyingResultDA = new StudyingResultDA();
        }

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

        public void InsertDetailedMark(HocSinh_ThongTinCaNhan student, LopHoc_Lop Class, CauHinh_HocKy term, DanhMuc_MonHoc subject, DanhMuc_LoaiDiem markType, double mark)
        {
            studyingResultDA.InsertDetailedMark(student, Class, term, subject, markType, mark);
        }

        public void InsertDetailedMark(HocSinh_ThongTinCaNhan student, LopHoc_Lop Class, CauHinh_HocKy term, DanhMuc_MonHoc subject, Dictionary<int, double> dicDetailMarks)
        {
            studyingResultDA.InsertDetailedMark(student, Class, term, subject, dicDetailMarks);
        }

        public void InsertDetailedMark(HocSinh_DiemMonHocHocKy termSubjectedMark, DanhMuc_LoaiDiem markType, double mark)
        {
            HocSinh_ChiTietDiem detailedMark = new HocSinh_ChiTietDiem();
            detailedMark.MaDiemMonHK = termSubjectedMark.MaDiemMonHK;
            detailedMark.MaLoaiDiem = markType.MaLoaiDiem;
            detailedMark.Diem = mark;

            studyingResultDA.InsertDetailMark(detailedMark);
            studyingResultDA.CalAvgMark(termSubjectedMark);
        }

        public void UpdateDetailedMark(HocSinh_ThongTinCaNhan student, LopHoc_Lop Class, CauHinh_HocKy term, DanhMuc_MonHoc subject, List<Diem> marks)
        {   
            List<DanhMuc_LoaiDiem> markTypes = new List<DanhMuc_LoaiDiem>();
            DanhMuc_LoaiDiem markType = null;
            List<int> markTypeIds = new List<int>();
            foreach (Diem mark in marks)
            {
                markTypeIds.Add(mark.MaLoaiDiem);
            }
            markTypeIds = markTypeIds.Distinct().ToList();
            foreach (int markTypeId in markTypeIds)
            {
                markType = new DanhMuc_LoaiDiem();
                markType.MaLoaiDiem = markTypeId;
                markTypes.Add(markType);
            }            
            studyingResultDA.DeleteDetailedMark(student, Class, term, subject, markTypes);

            int i = 0;
            while(i < marks.Count)
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

            Dictionary<int, double> dicMarks = new Dictionary<int, double>();
            foreach (Diem mark in marks)
            {
                dicMarks.Add(mark.MaLoaiDiem, mark.GiaTri);
            }

            studyingResultDA.InsertDetailedMark(student, Class, term, subject, dicMarks);
        }

        public void UpdateDetailedMark(HocSinh_ChiTietDiem editedDetailedMark, double mark)
        {
            editedDetailedMark.Diem = mark;
            studyingResultDA.UpdateDetailedMark(editedDetailedMark);
        }

        public void DeleteDetailedMark(HocSinh_ChiTietDiem deletedDetailedMark)
        {
            studyingResultDA.DeleteDetailedMark(deletedDetailedMark);
        }

        public HocSinh_ChiTietDiem GetDetailedMark(int detailedMarkId)
        {
            return studyingResultDA.GetDetailedMark(detailedMarkId);
        }        

        public List<TabularKetQuaMonHoc> GetListTabularKetQuaMonHoc(int maNamHoc, int maHocKy, int maHocSinh, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            return studyingResultDA.GetListTabularKetQuaMonHoc(maNamHoc, maHocKy, maHocSinh,
                pageCurrentIndex, pageSize, out totalRecords);
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

        public List<TabularChiTietDiemMonHocLoaiDiem> GetListTabularChiTietDiemMonHocLoaiDiem(int maDiemMonHK, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            return studyingResultDA.GetListTabularChiTietDiemMonHocLoaiDiem(maDiemMonHK, 
                pageCurrentIndex, pageSize, out totalRecords);
        }

        public void UpdateStudentTermResult(HocSinh_DanhHieuHocKy editedTermResult, DanhMuc_HanhKiem conduct)
        {
            editedTermResult.MaHanhKiemHK = conduct.MaHanhKiem;
            studyingResultDA.UpdateStudentTermResult(editedTermResult);
        }

        public List<TabularTermStudentResult> GetListTabularDanhHieuHocSinh(int maHocSinh, int maNamHoc, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            return studyingResultDA.GetListTabularTermStudentResult(maHocSinh, maNamHoc,
                pageCurrentIndex, pageSize, out totalRecords);
        }
        
        public List<TabularStudentMark> GetListDiemHocSinh(LopHoc_Lop Class, DanhMuc_MonHoc subject, CauHinh_HocKy term, List<DanhMuc_LoaiDiem> markTypes, int pageCurrentIndex, int pageSize, out double totalRecord)
        {
            return studyingResultDA.GetListDiemHocSinh(Class, subject, term, markTypes, pageCurrentIndex, pageSize, out totalRecord);
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

        public bool ValidateMark(string marks, string markTypeName)
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
                    MarkTypeBL markTypeBL = new MarkTypeBL();
                    DanhMuc_LoaiDiem markType = markTypeBL.GetMarkType(markTypeName);
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
            MarkTypeBL markTypeBL = new MarkTypeBL();
            List<DanhMuc_LoaiDiem> markTypes = markTypeBL.GetListMarkTypes();

            List<List<double>> subjectMarks = new List<List<double>>();
            List<double> innerSubjectMarks = null;
            List<HocSinh_ChiTietDiem> detailMarks;            
            
            foreach (DanhMuc_LoaiDiem loaiDiem in markTypes)
            {
                detailMarks = studyingResultDA.GetDetailedMarks(termSubjectedMark, loaiDiem);
                innerSubjectMarks = new List<double>();
                foreach(HocSinh_ChiTietDiem detailMark in detailMarks)
                {
                    innerSubjectMarks.Add(detailMark.Diem);
                }
                subjectMarks.Add(innerSubjectMarks);
            }

            return subjectMarks;
        }
    }
}
