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

        public void InsertDetailedMark(HocSinh_ThongTinCaNhan student, LopHoc_Lop Class, CauHinh_HocKy term, DanhMuc_MonHoc subject, Dictionary<int, double> dicDetailMarks)
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
                if (dicDetailMarks.Count != 0)
                {
                    foreach (KeyValuePair<int, double> pair in dicDetailMarks)
                    {
                        detailedMark = new HocSinh_ChiTietDiem();
                        detailedMark.MaDiemMonHK = termSubjectedMark.MaDiemMonHK;
                        detailedMark.MaLoaiDiem = pair.Key;
                        detailedMark.Diem = pair.Value;
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
            // Tính điểm trung bình của môn học trong học kỳ
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

        public double CalAvgMark(HocSinh_ThongTinCaNhan student, LopHoc_Lop Class, CauHinh_HocKy term, DanhMuc_MonHoc subject)
        {
            double dAvgMark = -1;
            HocSinh_DiemMonHocHocKy termSubjectedMark = null;
            DanhMuc_LoaiDiem markType = null;

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
                        markType = (from mkType in db.DanhMuc_LoaiDiems
                                    where mkType.MaLoaiDiem == detailedMark.MaLoaiDiem
                                    select mkType).First();
                        dTotalHeSoDiemLoai += markType.HeSoDiem;
                        dTotalMarks += detailedMark.Diem * markType.HeSoDiem;
                    }

                    dAvgMark = dTotalMarks / dTotalHeSoDiemLoai;
                    dAvgMark = Math.Round(dAvgMark, 1, MidpointRounding.AwayFromZero);
                }
            }

            return dAvgMark;
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
                            db.HocSinh_ChiTietDiems.DeleteOnSubmit(detailedMark);
                        }
                        db.SubmitChanges();
                    }

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

        //public void DeleteChiTietDiem(int maHocSinh, int maLopHoc,
        //    int hocKy, int maMonHoc, List<int> lMaLoaiDiems)
        //{
        //    IQueryable<HocSinh_DiemMonHocHocKy> iqDiemMonHK;
        //    iqDiemMonHK = from diemMonHK in db.HocSinh_DiemMonHocHocKies
        //                  join hocSinhLop in db.HocSinh_HocSinhLopHocs
        //                    on diemMonHK.MaHocSinhLopHoc equals hocSinhLop.MaHocSinhLopHoc
        //                  where hocSinhLop.MaHocSinh == maHocSinh
        //                        && hocSinhLop.MaLopHoc == maLopHoc
        //                        && diemMonHK.MaHocKy == hocKy
        //                        && diemMonHK.MaMonHoc == maMonHoc
        //                  select diemMonHK;
        //    if (iqDiemMonHK.Count() != 0)
        //    {
        //        HocSinh_DiemMonHocHocKy diemMonHK = iqDiemMonHK.First();
        //        int maDiemMonHK = diemMonHK.MaDiemMonHK;
        //        LoaiDiemDA loaiDiemDA = new LoaiDiemDA();
        //        foreach (int maLoaiDiem in lMaLoaiDiems)
        //        {
        //            IQueryable<HocSinh_ChiTietDiem> iqChiTietDiem;
        //            iqChiTietDiem = from diem in db.HocSinh_ChiTietDiems
        //                            where diem.MaDiemMonHK == maDiemMonHK
        //                                && diem.MaLoaiDiem == maLoaiDiem
        //                            select diem;
        //            if (iqChiTietDiem.Count() != 0)
        //            {
        //                foreach (HocSinh_ChiTietDiem chiTietDiem in iqChiTietDiem)
        //                {
        //                    db.HocSinh_ChiTietDiems.DeleteOnSubmit(chiTietDiem);
        //                }
        //                db.SubmitChanges();
        //            }

        //            DanhMuc_LoaiDiem loaiDiem = loaiDiemDA.GetMarkType(maLoaiDiem);
        //            if (loaiDiem.TinhDTB)
        //            {
        //                diemMonHK.DiemTB = -1;
        //            }
        //        }
        //    }
        //}



        //public void InsertChiTietDiem(int maHocSinh, int maLopHoc,
        //    int hocKy, int maMonHoc, List<Diem> chiTietDiems)
        //{
        //    IQueryable<HocSinh_DiemMonHocHocKy> iqDiemMonHK;
        //    iqDiemMonHK = from diemMonHK in db.HocSinh_DiemMonHocHocKies
        //                  join hocSinhLop in db.HocSinh_HocSinhLopHocs
        //                    on diemMonHK.MaHocSinhLopHoc equals hocSinhLop.MaHocSinhLopHoc
        //                  where hocSinhLop.MaHocSinh == maHocSinh
        //                    && hocSinhLop.MaLopHoc == maLopHoc
        //                    && diemMonHK.MaHocKy == hocKy
        //                    && diemMonHK.MaMonHoc == maMonHoc
        //                  select diemMonHK;
        //    if (iqDiemMonHK.Count() == 0)
        //    {
        //        return;
        //    }

        //    HocSinh_DiemMonHocHocKy diemMonHocHK = iqDiemMonHK.First();
        //    int maDiemMonHK = diemMonHocHK.MaDiemMonHK;
        //    LoaiDiemDA loaiDiemDA = new LoaiDiemDA();
        //    bool bCalAvgMarkSubject = false;
        //    foreach (Diem diem in chiTietDiems)
        //    {
        //        HocSinh_ChiTietDiem newChiTietDiem = new HocSinh_ChiTietDiem
        //        {
        //            MaDiemMonHK = maDiemMonHK,
        //            MaLoaiDiem = diem.MaLoaiDiem,
        //            Diem = diem.GiaTri
        //        };
        //        db.HocSinh_ChiTietDiems.InsertOnSubmit(newChiTietDiem);

        //        DanhMuc_LoaiDiem loaiDiem = loaiDiemDA.GetMarkType(diem.MaLoaiDiem);
        //        if (loaiDiem.TinhDTB)
        //        {
        //            bCalAvgMarkSubject = true;
        //        }
        //    }
        //    db.SubmitChanges();

        //    if (bCalAvgMarkSubject || (diemMonHocHK.DiemTB != -1))
        //    {
        //        diemMonHocHK.DiemTB = CalAvgMark(maHocSinh, maLopHoc, hocKy, maMonHoc);
        //    }

        //    db.SubmitChanges();
        //}

        //public void UpdateChiTietDiem(int maHocSinh, int maLopHoc,
        //    int hocKy, int maMonHoc, List<Diem> chiTietDiems)
        //{
        //    IQueryable<HocSinh_DiemMonHocHocKy> iqDiemMonHK;
        //    iqDiemMonHK = from diemMonHK in db.HocSinh_DiemMonHocHocKies
        //                  join hocSinhLop in db.HocSinh_HocSinhLopHocs
        //                    on diemMonHK.MaHocSinhLopHoc equals hocSinhLop.MaHocSinhLopHoc
        //                  where hocSinhLop.MaHocSinh == maHocSinh
        //                    && hocSinhLop.MaLopHoc == maLopHoc
        //                    && diemMonHK.MaHocKy == hocKy
        //                    && diemMonHK.MaMonHoc == maMonHoc
        //                  select diemMonHK;

        //    if (iqDiemMonHK.Count() != 0)
        //    {
        //        HocSinh_DiemMonHocHocKy diemMonHocHK = iqDiemMonHK.First();
        //        int maDiemMonHK = diemMonHocHK.MaDiemMonHK;

        //        // Delete existed ChiTietDiems
        //        IQueryable<HocSinh_ChiTietDiem> iqExistedChiTietDiem;
        //        iqExistedChiTietDiem = from chiTietDiem in db.HocSinh_ChiTietDiems
        //                               where chiTietDiem.MaDiemMonHK == maDiemMonHK
        //                               select chiTietDiem;
        //        if (iqExistedChiTietDiem.Count() != 0)
        //        {
        //            foreach (HocSinh_ChiTietDiem existedChiTietDiem in iqExistedChiTietDiem)
        //            {
        //                db.HocSinh_ChiTietDiems.DeleteOnSubmit(existedChiTietDiem);
        //            }
        //            db.SubmitChanges();
        //        }
        //        //-----------------------------

        //        if (chiTietDiems.Count != 0)
        //        {
        //            LoaiDiemDA loaiDiemDA = new LoaiDiemDA();
        //            bool bCalAvgMarkSubject = false;
        //            foreach (Diem diem in chiTietDiems)
        //            {
        //                HocSinh_ChiTietDiem newChiTietDiem = new HocSinh_ChiTietDiem
        //                {
        //                    MaDiemMonHK = maDiemMonHK,
        //                    MaLoaiDiem = diem.MaLoaiDiem,
        //                    Diem = diem.GiaTri
        //                };
        //                db.HocSinh_ChiTietDiems.InsertOnSubmit(newChiTietDiem);

        //                DanhMuc_LoaiDiem loaiDiem = loaiDiemDA.GetMarkType(diem.MaLoaiDiem);
        //                if (loaiDiem.TinhDTB)
        //                {
        //                    bCalAvgMarkSubject = true;
        //                }
        //            }
        //            db.SubmitChanges();

        //            if (bCalAvgMarkSubject)
        //            {
        //                diemMonHocHK.DiemTB = CalAvgMark(maHocSinh, maLopHoc, hocKy, maMonHoc);
        //            }
        //            else
        //            {
        //                diemMonHocHK.DiemTB = -1;
        //            }

        //            db.SubmitChanges();
        //        }
        //    }
        //}

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

            List<double> lstDiemMonHoc = new List<double>();
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

        public List<TabularKetQuaMonHoc> GetListTabularKetQuaMonHoc(int maNamHoc, int maHocKy, int maHocSinh, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            // Get list of MaMonHoc based on schedule
            List<DanhMuc_MonHoc> listMonHoc = new List<DanhMuc_MonHoc>();
            IQueryable<DanhMuc_MonHoc> monHocs;
            monHocs = from monHocTKB in db.LopHoc_MonHocTKBs
                      join lop in db.LopHoc_Lops on monHocTKB.MaLopHoc equals lop.MaLopHoc
                      join monHoc in db.DanhMuc_MonHocs on monHocTKB.MaMonHoc equals monHoc.MaMonHoc
                      join hsLop in db.HocSinh_HocSinhLopHocs on lop.MaLopHoc equals hsLop.MaLopHoc
                      where lop.MaNamHoc == maNamHoc && monHocTKB.MaHocKy == maHocKy
                      && hsLop.MaHocSinh == maHocSinh
                      select monHoc;
            if (monHocs.Count() != 0)
            {
                listMonHoc = monHocs.Distinct().ToList();
            }

            List<TabularKetQuaMonHoc> lstTbKetQuaMonHoc = new List<TabularKetQuaMonHoc>();
            foreach (DanhMuc_MonHoc monHoc in listMonHoc)
            {
                TabularKetQuaMonHoc tbKetQuaMonHoc;
                tbKetQuaMonHoc = (from diemHocKy in db.HocSinh_DiemMonHocHocKies
                                  join hS_Lop in db.HocSinh_HocSinhLopHocs on diemHocKy.MaHocSinhLopHoc equals hS_Lop.MaHocSinhLopHoc
                                  join lop in db.LopHoc_Lops on hS_Lop.MaLopHoc equals lop.MaLopHoc
                                  where hS_Lop.MaHocSinh == maHocSinh
                                     && lop.MaNamHoc == maNamHoc
                                     && diemHocKy.MaHocKy == maHocKy
                                     && diemHocKy.MaMonHoc == monHoc.MaMonHoc
                                  select new TabularKetQuaMonHoc
                                  {
                                      TenMonHoc = monHoc.TenMonHoc,
                                      MaDiemMonHK = diemHocKy.MaDiemMonHK,
                                      DiemTB = diemHocKy.DiemTB
                                  }).First();
                lstTbKetQuaMonHoc.Add(tbKetQuaMonHoc);
            }

            totalRecords = lstTbKetQuaMonHoc.Count();
            if (totalRecords != 0)
            {
                List<TabularKetQuaMonHoc> lstResult = lstTbKetQuaMonHoc.OrderBy(ketQua => ketQua.TenMonHoc)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
                foreach (TabularKetQuaMonHoc kq in lstResult)
                {
                    if (kq.DiemTB != -1)
                    {
                        kq.StrDiemTB = kq.DiemTB.ToString();
                    }
                    else
                    {
                        kq.StrDiemTB = "";
                    }
                }
                return lstResult;
            }
            else
            {
                return new List<TabularKetQuaMonHoc>();
            }
        }

        public List<TabularChiTietDiemMonHocLoaiDiem> GetListTabularChiTietDiemMonHocLoaiDiem(int maDiemMonHK, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            var lstTabularChiTietDiem = from diemMonHocHK in db.HocSinh_DiemMonHocHocKies
                                        join chiTietDiem in db.HocSinh_ChiTietDiems on diemMonHocHK.MaDiemMonHK equals chiTietDiem.MaDiemMonHK
                                        join loaiDiem in db.DanhMuc_LoaiDiems on chiTietDiem.MaLoaiDiem equals loaiDiem.MaLoaiDiem
                                        where diemMonHocHK.MaDiemMonHK == maDiemMonHK
                                        select new TabularChiTietDiemMonHocLoaiDiem
                                        {
                                            MaChiTietDiem = chiTietDiem.MaChiTietDiem,
                                            TenLoaiDiem = loaiDiem.TenLoaiDiem,
                                            Diem = chiTietDiem.Diem
                                        };

            totalRecords = lstTabularChiTietDiem.Count();
            if (lstTabularChiTietDiem.Count() != 0)
            {

                return lstTabularChiTietDiem.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<TabularChiTietDiemMonHocLoaiDiem>();
            }
        }
        
        public List<TabularTermStudentResult> GetListTabularTermStudentResult(int maHocSinh, int maNamHoc, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            SystemConfigDA systemConfigDA = new SystemConfigDA();
            List<TabularTermStudentResult> lstTabularDanhHieuHocSinh = new List<TabularTermStudentResult>();

            IQueryable<HocSinh_DanhHieuHocKy> danhHieuHocKies = from hocSinhLop in db.HocSinh_HocSinhLopHocs
                                                                join lop in db.LopHoc_Lops on hocSinhLop.MaLopHoc equals lop.MaLopHoc
                                                                join danhHieu in db.HocSinh_DanhHieuHocKies on hocSinhLop.MaHocSinhLopHoc equals danhHieu.MaHocSinhLopHoc
                                                                where hocSinhLop.MaHocSinh == maHocSinh && lop.MaNamHoc == maNamHoc
                                                                select danhHieu;

            totalRecords = danhHieuHocKies.Count();
            if (totalRecords == 0)
            {
                return lstTabularDanhHieuHocSinh;
            }

            HocLucDA hocLucDA = new HocLucDA();
            ConductDA hanhKiemDA = new ConductDA();
            DanhHieuDA danhHieuDA = new DanhHieuDA();
            foreach (HocSinh_DanhHieuHocKy danhHieu in danhHieuHocKies)
            {
                TabularTermStudentResult tbTermStudentResult = new TabularTermStudentResult();

                tbTermStudentResult.MaDanhHieuHSHK = danhHieu.MaDanhHieuHSHK;
                tbTermStudentResult.DiemTB = (int)danhHieu.DiemTBHK;
                tbTermStudentResult.StrDiemTB = (danhHieu.DiemTBHK != -1) ? (danhHieu.DiemTBHK.ToString()) : "";
                tbTermStudentResult.TenHocKy = danhHieu.CauHinh_HocKy.TenHocKy;
                tbTermStudentResult.MaHanhKiem = danhHieu.MaHanhKiemHK;
                int maHanhKiem = (int)tbTermStudentResult.MaHanhKiem;
                tbTermStudentResult.TenHanhKiem = (maHanhKiem != -1) ? hanhKiemDA.GetConduct(maHanhKiem).TenHanhKiem : "";

                int maHocLuc = (int)danhHieu.MaHocLucHK;
                tbTermStudentResult.TenHocLuc = (maHocLuc != -1) ? hocLucDA.GetHocLuc(maHocLuc).TenHocLuc : "";

                tbTermStudentResult.TenDanhHieu = danhHieuDA.GetTenDanhHieu(maHocLuc, maHanhKiem);

                lstTabularDanhHieuHocSinh.Add(tbTermStudentResult);
            }

            TabularTermStudentResult danhHieuHocSinhCuoiNam = new TabularTermStudentResult();
            danhHieuHocSinhCuoiNam.MaDanhHieuHSHK = -1;
            danhHieuHocSinhCuoiNam.TenHocKy = "Cả năm";
            if ((lstTabularDanhHieuHocSinh[0].DiemTB != -1) && (lstTabularDanhHieuHocSinh[1].DiemTB != -1))
            {
                danhHieuHocSinhCuoiNam.DiemTB = Math.Round(((lstTabularDanhHieuHocSinh[0].DiemTB + (2 * lstTabularDanhHieuHocSinh[1].DiemTB)) / 3), 1);
                danhHieuHocSinhCuoiNam.StrDiemTB = danhHieuHocSinhCuoiNam.DiemTB.ToString();
            }
            else
            {
                danhHieuHocSinhCuoiNam.DiemTB = -1;
                danhHieuHocSinhCuoiNam.StrDiemTB = "";
            }

            if (lstTabularDanhHieuHocSinh[0].MaHanhKiem != -1 && lstTabularDanhHieuHocSinh[1].MaHanhKiem != -1)
            {
                danhHieuHocSinhCuoiNam.MaHanhKiem = lstTabularDanhHieuHocSinh[1].MaHanhKiem;
            }
            else
            {
                danhHieuHocSinhCuoiNam.MaHanhKiem = -1;
            }
            int maHanhKiemCuoiNam = (int)danhHieuHocSinhCuoiNam.MaHanhKiem;
            danhHieuHocSinhCuoiNam.TenHanhKiem = (maHanhKiemCuoiNam != -1) ? hanhKiemDA.GetConduct(maHanhKiemCuoiNam).TenHanhKiem : "";

            int maHocLucCuoiNam;
            if (danhHieuHocSinhCuoiNam.DiemTB != -1)
            {
                DanhMuc_HocLuc hocLuc = hocLucDA.GetHocLuc(danhHieuHocSinhCuoiNam.DiemTB);
                maHocLucCuoiNam = hocLuc.MaHocLuc;
                danhHieuHocSinhCuoiNam.TenHocLuc = hocLuc.TenHocLuc;
            }
            else
            {
                maHocLucCuoiNam = -1;
                danhHieuHocSinhCuoiNam.TenHocLuc = "";
            }

            danhHieuHocSinhCuoiNam.TenDanhHieu = danhHieuDA.GetTenDanhHieu(maHocLucCuoiNam, maHanhKiemCuoiNam);

            lstTabularDanhHieuHocSinh.Add(danhHieuHocSinhCuoiNam);

            return lstTabularDanhHieuHocSinh;
        }

        public List<TabularStudentMark> GetListDiemHocSinh(LopHoc_Lop Class, DanhMuc_MonHoc subject, CauHinh_HocKy term, List<DanhMuc_LoaiDiem> markTypes, int pageCurrentIndex, int pageSize, out double totalRecord)
        {
            IQueryable<TabularStudentMark> iQDiemHocSinhs;
            iQDiemHocSinhs = from hocSinhLop in db.HocSinh_HocSinhLopHocs
                             join hocSinh in db.HocSinh_ThongTinCaNhans
                                on hocSinhLop.MaHocSinh equals hocSinh.MaHocSinh
                             join diemHocKy in db.HocSinh_DiemMonHocHocKies
                                on hocSinhLop.MaHocSinhLopHoc equals diemHocKy.MaHocSinhLopHoc
                             where hocSinhLop.MaLopHoc == Class.MaLopHoc && diemHocKy.MaMonHoc == subject.MaMonHoc
                             select new TabularStudentMark
                             {
                                 MaHocSinh = hocSinh.MaHocSinh,
                                 MaDiemHK = diemHocKy.MaDiemMonHK,
                                 MaHocSinhHienThi = hocSinh.MaHocSinhHienThi,
                                 TenHocSinh = hocSinh.HoTen,
                                 DiemTrungBinh = diemHocKy.DiemTB
                             };

            totalRecord = iQDiemHocSinhs.Count();
            if (totalRecord != 0)
            {
                // Get List of TabularDiemHocSinh by LopHoc and MonHoc
                List<TabularStudentMark> lstTbDiemHS;
                lstTbDiemHS = iQDiemHocSinhs.OrderBy(diemHS => diemHS.TenHocSinh)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();

                MarkTypeDA loaiDiemDA = new MarkTypeDA();
                foreach (TabularStudentMark tbDiemHS in lstTbDiemHS)
                {
                    tbDiemHS.DiemTheoLoaiDiems = new List<DiemTheoLoaiDiem>();
                    int maDiemMonHK = tbDiemHS.MaDiemHK;
                    foreach (DanhMuc_LoaiDiem loaiDiem in markTypes)
                    {
                        List<double> lstDiem = new List<double>();
                        IQueryable<HocSinh_ChiTietDiem> iQDiems;
                        iQDiems = from diem in db.HocSinh_ChiTietDiems
                                  where diem.MaLoaiDiem == loaiDiem.MaLoaiDiem && diem.MaDiemMonHK == maDiemMonHK
                                  select diem;
                        string strDiems = "";
                        if (iQDiems.Count() != 0)
                        {
                            StringBuilder strB = new StringBuilder();
                            foreach (HocSinh_ChiTietDiem diem in iQDiems)
                            {
                                strB.Append(diem.Diem.ToString());
                                strB.Append(", ");
                            }

                            strDiems = strB.ToString().Trim().Trim(new char[] { ',' });
                        }
                        DiemTheoLoaiDiem diemTheoLoaiDiem = new DiemTheoLoaiDiem
                        {
                            MaLoaiDiem = loaiDiem.MaLoaiDiem,
                            StringDiems = strDiems,
                            TenLoaiDiem = loaiDiem.TenLoaiDiem
                        };
                        tbDiemHS.DiemTheoLoaiDiems.Add(diemTheoLoaiDiem);
                    }
                }
                return lstTbDiemHS;
            }
            else
            {
                return new List<TabularStudentMark>();
            }
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
    }
}
