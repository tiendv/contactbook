using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class StudyingResultDA : BaseDA
    {
        public StudyingResultDA()
            : base()
        {
        }

        public void InsertTermStudyingResult(HocSinh_DanhHieuHocKy termStudyingResult)
        {
            db.HocSinh_DanhHieuHocKies.InsertOnSubmit(termStudyingResult);
            db.SubmitChanges();
        }

        public void InsertDetailedMark(HocSinh_ThongTinCaNhan student, LopHoc_Lop Class, CauHinh_HocKy term, DanhMuc_MonHoc subject, DanhMuc_LoaiDiem markType, double mark)
        {
            HocSinh_DiemMonHocHocKy termSubjectedMark = null;
            HocSinh_ChiTietDiem detailedMark = null;

            IQueryable<HocSinh_DiemMonHocHocKy> iqTermSubjectedMark;
            iqTermSubjectedMark = from termSubjMark in db.HocSinh_DiemMonHocHocKies
                                  where termSubjMark.HocSinh_HocSinhLopHoc.MaHocSinh == student.MaHocSinh
                                    && termSubjMark.HocSinh_HocSinhLopHoc.MaLopHoc == Class.MaLopHoc
                                    && termSubjMark.MaHocKy == term.MaHocKy && termSubjMark.MaMonHoc == subject.MaMonHoc
                                  select termSubjMark;

            if (iqTermSubjectedMark.Count() != 0)
            {
                termSubjectedMark = iqTermSubjectedMark.First();
                detailedMark = new HocSinh_ChiTietDiem();
                detailedMark.MaDiemMonHK = termSubjectedMark.MaDiemMonHK;
                detailedMark.MaLoaiDiem = markType.MaLoaiDiem;
                detailedMark.Diem = mark;

                db.HocSinh_ChiTietDiems.InsertOnSubmit(detailedMark);
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
        public void InsertDetailedMark(HocSinh_ThongTinCaNhan student, LopHoc_Lop Class, CauHinh_HocKy term, DanhMuc_MonHoc subject, List<DetailMark> marks)
        {
            HocSinh_DiemMonHocHocKy termSubjectedMark = null;
            HocSinh_ChiTietDiem detailedMark = null;
            bool bCalAvg = false;

            IQueryable<HocSinh_DiemMonHocHocKy> iqTermSubjectedMark;
            iqTermSubjectedMark = from termSubjMark in db.HocSinh_DiemMonHocHocKies
                                  where termSubjMark.HocSinh_HocSinhLopHoc.MaHocSinh == student.MaHocSinh
                                    && termSubjMark.HocSinh_HocSinhLopHoc.MaLopHoc == Class.MaLopHoc
                                    && termSubjMark.MaHocKy == term.MaHocKy && termSubjMark.MaMonHoc == subject.MaMonHoc
                                  select termSubjMark;

            if (iqTermSubjectedMark.Count() != 0)
            {
                termSubjectedMark = iqTermSubjectedMark.First();
                if (marks.Count != 0)
                {
                    foreach (DetailMark mark in marks)
                    {
                        detailedMark = new HocSinh_ChiTietDiem();
                        detailedMark.MaDiemMonHK = termSubjectedMark.MaDiemMonHK;
                        detailedMark.MaLoaiDiem = mark.MaLoaiDiem;
                        detailedMark.Diem = mark.GiaTri;
                        db.HocSinh_ChiTietDiems.InsertOnSubmit(detailedMark);

                    }
                    db.SubmitChanges();
                }
            }
        }

        public void InsertDetailMark(HocSinh_ChiTietDiem detailedMark)
        {
            db.HocSinh_ChiTietDiems.InsertOnSubmit(detailedMark);
            db.SubmitChanges();
        }

        public void UpdateDetailedMark(HocSinh_ChiTietDiem editedDetailedMark)
        {
            HocSinh_ChiTietDiem detailedMark = null;
            IQueryable<HocSinh_ChiTietDiem> iqDetailedMark = from dtlMark in db.HocSinh_ChiTietDiems
                                                             where dtlMark.MaChiTietDiem == editedDetailedMark.MaChiTietDiem
                                                             select dtlMark;
            if (iqDetailedMark.Count() != 0)
            {
                detailedMark = iqDetailedMark.First();
                detailedMark.Diem = editedDetailedMark.Diem;
                db.SubmitChanges();

                CalAvgMark(detailedMark.HocSinh_DiemMonHocHocKy);
            }
        }

        public void UpdateStudentTermResult(HocSinh_DanhHieuHocKy editedTermResult)
        {
            HocSinh_DanhHieuHocKy termResult = null;
            IQueryable<HocSinh_DanhHieuHocKy> iqTermResult = from trmRes in db.HocSinh_DanhHieuHocKies
                                                             where trmRes.MaDanhHieuHSHK == termResult.MaDanhHieuHSHK
                                                             select trmRes;
            if (iqTermResult.Count() != 0)
            {
                termResult = iqTermResult.First();
                termResult.MaHanhKiemHK = editedTermResult.MaHanhKiemHK;
                db.SubmitChanges();
            }
        }

        public void CalAvgMark(HocSinh_DiemMonHocHocKy termSubjectedMark)
        {
            //Tính điểm trung bình của môn học trong học kỳ
            // Xác định loại điểm cuối kỳ
            //int maLoaiDiemCuoiKy = (from loaiDiem in db.DanhMuc_LoaiDiems
            //                        where loaiDiem.TenLoaiDiem == "Thi cuối học kỳ"
            //                        select loaiDiem.MaLoaiDiem).First();
            //double diemTBMonHocHocKy = 0;
            //double totalHeSoLoaiDiem = 0;
            //bool bUpdateDiemTBMonHocHocKy = false;
            //IQueryable<HocSinh_ChiTietDiem> chiTietDiems = from ctDiem in db.HocSinh_ChiTietDiems
            //                                               where ctDiem.MaDiemMonHK == maDiemMonHK
            //                                               select ctDiem;
            //LoaiDiemDA loaiDiemDA = new LoaiDiemDA();            
            //foreach (HocSinh_ChiTietDiem ctDiem in chiTietDiems)
            //{   
            //    double heSoLoaiDiem = loaiDiemDA.GetLoaiDiem(ctDiem.MaLoaiDiem).HeSoDiem;
            //    diemTBMonHocHocKy += (ctDiem.Diem * heSoLoaiDiem);
            //    totalHeSoLoaiDiem += heSoLoaiDiem;

            //    // Nếu tồn tại loại điểm cuối kỳ thì sẽ tính điểm tb môn học học kỳ
            //    if (ctDiem.MaLoaiDiem == maLoaiDiemCuoiKy)
            //    {
            //        bUpdateDiemTBMonHocHocKy = true;
            //    }
            //}
            //HocSinh_DiemMonHocHocKy diemMonHocHK = (from diemMonHK in db.HocSinh_DiemMonHocHocKies
            //                                        where diemMonHK.MaDiemMonHK == maDiemMonHK
            //                                        select diemMonHK).First();
            //if (bUpdateDiemTBMonHocHocKy)
            //{   
            //    diemMonHocHK.DiemTB = Math.Round(diemTBMonHocHocKy / totalHeSoLoaiDiem, 1);                
            //}
            //else
            //{
            //    diemMonHocHK.DiemTB = -1;
            //}
            //db.SubmitChanges();

            //// Tính điểm trung bình cả học kỳ (bao gồm tất cả các môn)
            //int maHocSinhLopHoc = diemMonHocHK.MaHocSinhLopHoc;
            //double diemTBHocKy = 0;
            //double totalHeSoMonHoc = 0;
            //bool bUpdateDiemTBHocKy = true;
            //IQueryable<HocSinh_DiemMonHocHocKy> diemMonHocHocKies = from diemMonHK in db.HocSinh_DiemMonHocHocKies
            //                                                        where diemMonHK.MaHocSinhLopHoc == maHocSinhLopHoc
            //                                                        select diemMonHK;
            //foreach (HocSinh_DiemMonHocHocKy diemMonHocHocKy in diemMonHocHocKies)
            //{
            //    if (diemMonHocHocKy.DiemTB == -1) // Chưa tính hết
            //    {
            //        bUpdateDiemTBHocKy = false;
            //        break;
            //    }
            //    else
            //    {
            //        double heSoDiemMon = (from mon in db.DanhMuc_MonHocs
            //                          join monTKB in db.LopHoc_MonHocTKBs on mon.MaMonHoc equals monTKB.MaMonHoc
            //                          where monTKB.MaMonHocTKB == diemMonHocHocKy.MaMonHocTKB
            //                          select mon).First().HeSoDiem;
            //        totalHeSoMonHoc += heSoDiemMon;
            //        diemTBHocKy += heSoDiemMon * diemMonHocHocKy.DiemTB;
            //    }
            //}

            //int maHocKy = (from monTKB in db.LopHoc_MonHocTKBs
            //               where monTKB.MaMonHocTKB == diemMonHocHK.MaMonHocTKB
            //               select monTKB).First().MaHocKy;
            //HocSinh_DanhHieuHocKy danhHieuHocKy = (from danhHieu in db.HocSinh_DanhHieuHocKies
            //                                       where danhHieu.MaHocSinhLopHoc == maHocSinhLopHoc && danhHieu.MaHocKy == maHocKy
            //                                       select danhHieu).First();
            //if (bUpdateDiemTBHocKy)
            //{
            //    danhHieuHocKy.DiemTBHK = Math.Round(diemTBHocKy / totalHeSoMonHoc, 1);
            //    HocLucDA hocLucDA = new HocLucDA();
            //    DanhMuc_HocLuc hocLuc = hocLucDA.GetHocLuc((double)danhHieuHocKy.DiemTBHK);
            //    if (hocLuc != null)
            //    {
            //        danhHieuHocKy.MaHocLucHK = hocLuc.MaHocLuc;
            //    }
            //    else
            //    {
            //        danhHieuHocKy.MaHocLucHK = -1;
            //    }
            //}
            //else
            //{
            //    danhHieuHocKy.DiemTBHK = -1;
            //    danhHieuHocKy.MaHocLucHK = -1;
            //}
            //db.SubmitChanges();

        }

        public bool NeedResetAvgMark(HocSinh_ThongTinCaNhan student, LopHoc_Lop Class, CauHinh_HocKy term, DanhMuc_MonHoc subject)
        {
            bool bResult = true;
            List<HocSinh_ChiTietDiem> detailedMarks;
            IQueryable<HocSinh_ChiTietDiem> iqDetailedMark;
            iqDetailedMark = from dtlMark in db.HocSinh_ChiTietDiems 
                                  where dtlMark.HocSinh_DiemMonHocHocKy.HocSinh_HocSinhLopHoc.MaHocSinh == student.MaHocSinh
                                    && dtlMark.HocSinh_DiemMonHocHocKy.HocSinh_HocSinhLopHoc.MaLopHoc == Class.MaLopHoc
                                    && dtlMark.HocSinh_DiemMonHocHocKy.MaHocKy == term.MaHocKy 
                                    && dtlMark.HocSinh_DiemMonHocHocKy.MaMonHoc == subject.MaMonHoc
                                  select dtlMark;
            
            if (iqDetailedMark.Count() != 0)
            {
                detailedMarks = iqDetailedMark.ToList();
                int length = detailedMarks.Count;
                for (int i = 0; i < length; i++)
                {
                    if (detailedMarks[i].DanhMuc_LoaiDiem.TinhDTB)
                    {
                        bResult = false;
                        break;                        
                    }
                }
            }

            return bResult;
        }

        public void CalAvgMark(HocSinh_ThongTinCaNhan student, LopHoc_Lop Class, CauHinh_HocKy term, DanhMuc_MonHoc subject)
        {
            double dAvgMark = -1;
            HocSinh_DiemMonHocHocKy termSubjectedMark = null;

            IQueryable<HocSinh_DiemMonHocHocKy> iqTermSubjectedMark;
            iqTermSubjectedMark = from termSubjMark in db.HocSinh_DiemMonHocHocKies
                                  where termSubjMark.HocSinh_HocSinhLopHoc.MaHocSinh == student.MaHocSinh
                                    && termSubjMark.HocSinh_HocSinhLopHoc.MaLopHoc == Class.MaLopHoc
                                    && termSubjMark.MaHocKy == term.MaHocKy && termSubjMark.MaMonHoc == subject.MaMonHoc
                                  select termSubjMark;

            if (iqTermSubjectedMark.Count() != 0)
            {
                termSubjectedMark = iqTermSubjectedMark.First();
                IQueryable<HocSinh_ChiTietDiem> iqDetailedMark;
                iqDetailedMark = from dtlMark in db.HocSinh_ChiTietDiems
                                 where dtlMark.MaDiemMonHK == termSubjectedMark.MaDiemMonHK
                                 select dtlMark;
                if (iqDetailedMark.Count() != 0)
                {
                    double dTotalHeSoDiemLoai = 0;
                    double dTotalMarks = 0;
                    foreach (HocSinh_ChiTietDiem detailedMark in iqDetailedMark)
                    {   
                        dTotalHeSoDiemLoai += detailedMark.DanhMuc_LoaiDiem.HeSoDiem;
                        dTotalMarks += detailedMark.Diem * detailedMark.DanhMuc_LoaiDiem.HeSoDiem;
                    }

                    dAvgMark = dTotalMarks / dTotalHeSoDiemLoai;
                    dAvgMark = Math.Round(dAvgMark, 1, MidpointRounding.AwayFromZero);
                }                
            }

            termSubjectedMark.DiemTB = dAvgMark;
            db.SubmitChanges();
        }

        public void ResetAvgMark(HocSinh_ThongTinCaNhan student, LopHoc_Lop Class, CauHinh_HocKy term, DanhMuc_MonHoc subject)
        {
            HocSinh_DiemMonHocHocKy termSubjectedMark = null;

            IQueryable<HocSinh_DiemMonHocHocKy> iqTermSubjectedMark;
            iqTermSubjectedMark = from termSubjMark in db.HocSinh_DiemMonHocHocKies
                                  where termSubjMark.HocSinh_HocSinhLopHoc.MaHocSinh == student.MaHocSinh
                                    && termSubjMark.HocSinh_HocSinhLopHoc.MaLopHoc == Class.MaLopHoc
                                    && termSubjMark.MaHocKy == term.MaHocKy && termSubjMark.MaMonHoc == subject.MaMonHoc
                                  select termSubjMark;

            if (iqTermSubjectedMark.Count() != 0)
            {
                termSubjectedMark = iqTermSubjectedMark.First();
                termSubjectedMark.DiemTB = -1;
                db.SubmitChanges();
            }
        }

        public void DeleteDetailedMark(HocSinh_ChiTietDiem deletedDetailedMark)
        {
            HocSinh_ChiTietDiem detailedMark = null;
            IQueryable<HocSinh_ChiTietDiem> iqDetailedMark = from dtlMark in db.HocSinh_ChiTietDiems
                                                             where dtlMark.MaChiTietDiem == deletedDetailedMark.MaChiTietDiem
                                                             select dtlMark;
            if (iqDetailedMark.Count() != 0)
            {
                detailedMark = iqDetailedMark.First();

                HocSinh_DiemMonHocHocKy termSubjectedMark = null;
                IQueryable<HocSinh_DiemMonHocHocKy> iqTermSubjectedMark;
                iqTermSubjectedMark = from termSubjMark in db.HocSinh_DiemMonHocHocKies
                                      where termSubjMark.MaDiemMonHK == deletedDetailedMark.MaDiemMonHK
                                      select termSubjMark;
                if (iqTermSubjectedMark.Count() != 0)
                {
                    termSubjectedMark = iqTermSubjectedMark.First();
                }

                db.HocSinh_ChiTietDiems.DeleteOnSubmit(detailedMark);
                db.SubmitChanges();

                CalAvgMark(termSubjectedMark);
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
        public void DeleteDetailedMark(HocSinh_ThongTinCaNhan student, LopHoc_Lop Class, CauHinh_HocKy term, DanhMuc_MonHoc subject, List<DanhMuc_LoaiDiem> markTypes)
        {
            HocSinh_DiemMonHocHocKy termSubjectedMark = null;

            IQueryable<HocSinh_DiemMonHocHocKy> iqTermSubjectedMark;
            iqTermSubjectedMark = from termSubjMark in db.HocSinh_DiemMonHocHocKies
                                  where termSubjMark.HocSinh_HocSinhLopHoc.MaHocSinh == student.MaHocSinh
                                    && termSubjMark.HocSinh_HocSinhLopHoc.MaLopHoc == Class.MaLopHoc
                                    && termSubjMark.MaHocKy == term.MaHocKy && termSubjMark.MaMonHoc == subject.MaMonHoc
                                  select termSubjMark;

            if (iqTermSubjectedMark.Count() != 0)
            {
                termSubjectedMark = iqTermSubjectedMark.First();
                foreach (DanhMuc_LoaiDiem markType in markTypes)
                {
                    IQueryable<HocSinh_ChiTietDiem> iqDetailedMark;
                    iqDetailedMark = from mark in db.HocSinh_ChiTietDiems
                                     where mark.MaDiemMonHK == termSubjectedMark.MaDiemMonHK && mark.MaLoaiDiem == markType.MaLoaiDiem
                                     select mark;
                    if (iqDetailedMark.Count() != 0)
                    {
                        foreach (HocSinh_ChiTietDiem detailedMark in iqDetailedMark)
                        {
                            // Xoa diem hoc sinh theo MaLoaiDiem
                            db.HocSinh_ChiTietDiems.DeleteOnSubmit(detailedMark);
                        }
                        db.SubmitChanges();
                    }

                    // re calculate avg mark
                    if (markType.TinhDTB)
                    {
                        termSubjectedMark.DiemTB = -1;
                        db.SubmitChanges();
                    }
                }
            }
        }

        public void DeleteDetailedMarks(HocSinh_DiemMonHocHocKy termSubjectedMark, DanhMuc_LoaiDiem markType)
        {
            IQueryable<HocSinh_ChiTietDiem> iqDetailedMark = from dtlMark in db.HocSinh_ChiTietDiems
                                                             where dtlMark.MaDiemMonHK == termSubjectedMark.MaDiemMonHK
                                                                && dtlMark.MaLoaiDiem == markType.MaLoaiDiem
                                                             select dtlMark;
            if (iqDetailedMark.Count() != 0)
            {
                foreach (HocSinh_ChiTietDiem detailedMark in iqDetailedMark)
                {
                    db.HocSinh_ChiTietDiems.DeleteOnSubmit(detailedMark);
                }
                db.SubmitChanges();
            }
        }

        public void DeleteDetailedMarks(HocSinh_DiemMonHocHocKy termSubjectedMark)
        {
            List<HocSinh_ChiTietDiem> detailedMarks;
            IQueryable<HocSinh_ChiTietDiem> iqDetailedMark;
            iqDetailedMark = from mark in db.HocSinh_ChiTietDiems
                             where mark.MaDiemMonHK == termSubjectedMark.MaDiemMonHK
                             select mark;
            if (iqDetailedMark.Count() != 0)
            {
                detailedMarks = iqDetailedMark.ToList();

                foreach (HocSinh_ChiTietDiem detailedMark in detailedMarks)
                {
                    db.HocSinh_ChiTietDiems.DeleteOnSubmit(detailedMark);
                }
                db.SubmitChanges();
            }
        }

        public void DeleteTermSubjectMarks(LopHoc_MonHocTKB schedule)
        {
            IQueryable<HocSinh_DiemMonHocHocKy> iqTermSubjectMark = from termSubjectMark in db.HocSinh_DiemMonHocHocKies
                                                                    join schd in db.LopHoc_MonHocTKBs on termSubjectMark.MaMonHoc equals schedule.MaMonHoc
                                                                    where termSubjectMark.MaMonHoc == schedule.MaMonHoc
                                                                    && schd.MaMonHocTKB == schedule.MaMonHocTKB
                                                                    select termSubjectMark;
            if (iqTermSubjectMark.Count() != 0)
            {
                foreach (HocSinh_DiemMonHocHocKy termSubjectMark in iqTermSubjectMark)
                {
                    DeleteDetailedMarks(termSubjectMark);
                    db.HocSinh_DiemMonHocHocKies.DeleteOnSubmit(termSubjectMark);
                    db.SubmitChanges();
                }
            }
        }

        public HocSinh_ChiTietDiem GetDetailedMark(int detailedMarkId)
        {
            HocSinh_ChiTietDiem detailedMark = null;
            IQueryable<HocSinh_ChiTietDiem> iqDetailedMark = from dtlMark in db.HocSinh_ChiTietDiems
                                                             where dtlMark.MaChiTietDiem == detailedMarkId
                                                             select dtlMark;
            if (iqDetailedMark.Count() != 0)
            {
                detailedMark = iqDetailedMark.First();
            }

            return detailedMark;
        }

        public List<HocSinh_ChiTietDiem> GetDetailedMarks(HocSinh_DiemMonHocHocKy termSubjectedMark, DanhMuc_LoaiDiem markType)
        {
            List<HocSinh_ChiTietDiem> detailMarks = new List<HocSinh_ChiTietDiem>();
            IQueryable<HocSinh_ChiTietDiem> iqDetailMark = from dtlMark in db.HocSinh_ChiTietDiems
                                                           where dtlMark.MaLoaiDiem == markType.MaLoaiDiem
                                                           && dtlMark.MaDiemMonHK == termSubjectedMark.MaDiemMonHK
                                                           select dtlMark;
            if (iqDetailMark.Count() != 0)
            {
                detailMarks = iqDetailMark.ToList();
            }

            return detailMarks;
        }

        public List<HocSinh_ChiTietDiem> GetDetailedMarks(HocSinh_DiemMonHocHocKy termSubjectedMark, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<HocSinh_ChiTietDiem> detailMarks = new List<HocSinh_ChiTietDiem>();
            IQueryable<HocSinh_ChiTietDiem> iqDetailMark = from dtlMark in db.HocSinh_ChiTietDiems
                                                           where dtlMark.MaDiemMonHK == termSubjectedMark.MaDiemMonHK
                                                           select dtlMark;
            totalRecords = iqDetailMark.Count();
            if (totalRecords != 0)
            {
                detailMarks = iqDetailMark.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return detailMarks;
        }

        public List<DanhMuc_MonHoc> GetScheduledSubjects(HocSinh_ThongTinCaNhan student, CauHinh_NamHoc year, CauHinh_HocKy term)
        {
            // Get list of MaMonHoc based on schedule
            List<DanhMuc_MonHoc> scheduledSubjects = new List<DanhMuc_MonHoc>();
            IQueryable<DanhMuc_MonHoc> iqScheduledSubjects = from schedule in db.LopHoc_MonHocTKBs
                                                             join studentInClass in db.HocSinh_HocSinhLopHocs on schedule.MaLopHoc equals studentInClass.MaLopHoc
                                                             where schedule.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                                                             && schedule.MaHocKy == term.MaHocKy
                                                             && studentInClass.MaHocSinh == student.MaHocSinh
                                                             select schedule.DanhMuc_MonHoc;
            if (iqScheduledSubjects.Count() != 0)
            {
                scheduledSubjects = iqScheduledSubjects.Distinct().ToList();
            }

            return scheduledSubjects;
        }

        public HocSinh_DiemMonHocHocKy GetTermSubjectedMark(int termSubjectedMarkId)
        {
            HocSinh_DiemMonHocHocKy termSubjectedMark = null;

            IQueryable<HocSinh_DiemMonHocHocKy> iqTermSubjectedMark;
            iqTermSubjectedMark = from termSubjMark in db.HocSinh_DiemMonHocHocKies
                                  where termSubjMark.MaDiemMonHK == termSubjectedMarkId
                                  select termSubjMark;

            if (iqTermSubjectedMark.Count() != 0)
            {
                termSubjectedMark = iqTermSubjectedMark.First();
            }

            return termSubjectedMark;
        }

        public HocSinh_DiemMonHocHocKy GetTermSubjectedMark(HocSinh_ThongTinCaNhan student, CauHinh_NamHoc year, CauHinh_HocKy term, DanhMuc_MonHoc subject)
        {
            HocSinh_DiemMonHocHocKy termSubjectMark = null;

            IQueryable<HocSinh_DiemMonHocHocKy> iqTermSubjectMark;
            iqTermSubjectMark = from termSubjMark in db.HocSinh_DiemMonHocHocKies
                                where termSubjMark.HocSinh_HocSinhLopHoc.MaHocSinh == student.MaHocSinh
                                && termSubjMark.HocSinh_HocSinhLopHoc.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                                && termSubjMark.MaMonHoc == subject.MaMonHoc && termSubjMark.MaHocKy == term.MaHocKy
                                select termSubjMark;
            if (iqTermSubjectMark.Count() != 0)
            {
                termSubjectMark = iqTermSubjectMark.First();
            }

            return termSubjectMark;
        }

        public List<HocSinh_DiemMonHocHocKy> GetTermSubjectedMarks(LopHoc_Lop Class, DanhMuc_MonHoc subject, CauHinh_HocKy term, int pageCurrentIndex, int pageSize, out double totalRecord)
        {
            List<HocSinh_DiemMonHocHocKy> termSubjectMarks = new List<HocSinh_DiemMonHocHocKy>();

            IQueryable<HocSinh_DiemMonHocHocKy> iqTermSubjectMark;
            iqTermSubjectMark = from termSubjMark in db.HocSinh_DiemMonHocHocKies
                                where termSubjMark.HocSinh_HocSinhLopHoc.MaLopHoc == Class.MaLopHoc
                                && termSubjMark.MaMonHoc == subject.MaMonHoc && termSubjMark.MaHocKy == term.MaHocKy
                                select termSubjMark;

            totalRecord = iqTermSubjectMark.Count();
            if (totalRecord != 0)
            {
                termSubjectMarks = iqTermSubjectMark.OrderBy(termSubjMark => termSubjMark.HocSinh_HocSinhLopHoc.HocSinh_ThongTinCaNhan.MaHocSinhHienThi)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return termSubjectMarks;
        }        

        public List<HocSinh_DanhHieuHocKy> GetStudentTermResults(HocSinh_ThongTinCaNhan student, CauHinh_NamHoc year, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<HocSinh_DanhHieuHocKy> studentTermResults = new List<HocSinh_DanhHieuHocKy>();
            IQueryable<HocSinh_DanhHieuHocKy> iqStudentTermResult = from danhHieu in db.HocSinh_DanhHieuHocKies
                                                                    where danhHieu.HocSinh_HocSinhLopHoc.MaHocSinh == student.MaHocSinh
                                                                    && danhHieu.HocSinh_HocSinhLopHoc.LopHoc_Lop.MaNamHoc == year.MaNamHoc
                                                                    select danhHieu;

            totalRecords = iqStudentTermResult.Count();
            if (totalRecords != 0)
            {
                studentTermResults = iqStudentTermResult.ToList();
            }

            return studentTermResults;
        }
    }
}
