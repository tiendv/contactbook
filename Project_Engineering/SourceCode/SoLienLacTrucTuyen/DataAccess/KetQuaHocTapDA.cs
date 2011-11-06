using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class KetQuaHocTapDA : BaseDA
    {
        public KetQuaHocTapDA()
            : base()
        {
        }

        #region Chi tiết điểm (theo môn, loại điểm)
        public void InsertChiTietDiem(int maDiemMonHK, int maLoaiDiem, double diem)
        {
            db.HocSinh_ChiTietDiems.InsertOnSubmit(new HocSinh_ChiTietDiem
            {
                MaDiemMonHK = maDiemMonHK,
                MaLoaiDiem = maLoaiDiem,
                Diem = diem
            });
            db.SubmitChanges();

            CalAvgMark(maDiemMonHK);
        }

        public void UpdateChiTietDiem(int maChiTietDiem, double diem)
        {
            HocSinh_ChiTietDiem chiTietDiem = (from ctDiem in db.HocSinh_ChiTietDiems
                                               where ctDiem.MaChiTietDiem == maChiTietDiem
                                               select ctDiem).First();
            chiTietDiem.Diem = diem;
            int maDiemMonHK = chiTietDiem.MaDiemMonHK;
            db.SubmitChanges();

            CalAvgMark(maDiemMonHK);
        }

        public void DeleteChiTietDiem(int maChiTietDiem)
        {
            IQueryable<HocSinh_ChiTietDiem> chiTietDiems = from ctDiem in db.HocSinh_ChiTietDiems
                                                           where ctDiem.MaChiTietDiem == maChiTietDiem
                                                           select ctDiem;
            if (chiTietDiems.Count() != 0)
            {
                HocSinh_ChiTietDiem chiTietDiem = chiTietDiems.First();
                HocSinh_DiemMonHocHocKy diemMonHocHK = (from diemMonHK in db.HocSinh_DiemMonHocHocKies
                                                        where diemMonHK.MaDiemMonHK == chiTietDiem.MaDiemMonHK
                                                        select diemMonHK).First();


                db.HocSinh_ChiTietDiems.DeleteOnSubmit(chiTietDiems.First());
                db.SubmitChanges();

                CalAvgMark(diemMonHocHK.MaDiemMonHK);
            }
        }

        public void DeleteChiTietDiems(int maDiemHK, int maLoaiDiem)
        {
            IQueryable<HocSinh_ChiTietDiem> iQChiTietDiems;
            iQChiTietDiems = from chiTietDiem in db.HocSinh_ChiTietDiems
                             where chiTietDiem.MaDiemMonHK == maDiemHK
                                && chiTietDiem.MaLoaiDiem == maLoaiDiem
                             select chiTietDiem;
            if (iQChiTietDiems.Count() != 0)
            {
                foreach (HocSinh_ChiTietDiem chiTietDiem in iQChiTietDiems)
                {
                    db.HocSinh_ChiTietDiems.DeleteOnSubmit(chiTietDiem);
                }
                db.SubmitChanges();
            }
        }

        public void InsertChiTietDiem(int maHocSinh, int maLopHoc,
            int hocKy, int maMonHoc, int maLoaiDiem, double diem)
        {
            IQueryable<HocSinh_DiemMonHocHocKy> iqDiemMonHK;
            iqDiemMonHK = from diemMonHK in db.HocSinh_DiemMonHocHocKies
                          join hocSinhLop in db.HocSinh_HocSinhLopHocs
                            on diemMonHK.MaHocSinhLopHoc equals hocSinhLop.MaHocSinhLopHoc
                          where hocSinhLop.MaHocSinh == maHocSinh
                            && hocSinhLop.MaLopHoc == maLopHoc
                            && diemMonHK.MaHocKy == hocKy
                            && diemMonHK.MaMonHoc == maMonHoc
                          select diemMonHK;
            if (iqDiemMonHK.Count() != 0)
            {
                int maDiemMonHK = iqDiemMonHK.First().MaDiemMonHK;
                HocSinh_ChiTietDiem chiTietDiem = new HocSinh_ChiTietDiem
                {
                    MaDiemMonHK = maDiemMonHK,
                    MaLoaiDiem = maLoaiDiem,
                    Diem = diem
                };

                db.HocSinh_ChiTietDiems.InsertOnSubmit(chiTietDiem);
                db.SubmitChanges();
            }
        }

        public void InsertChiTietDiem(int maHocSinh, int maLopHoc,
            int hocKy, int maMonHoc, Dictionary<int, double> dicChiTietDiem)
        {
            IQueryable<HocSinh_DiemMonHocHocKy> iqDiemMonHK;
            iqDiemMonHK = from diemMonHK in db.HocSinh_DiemMonHocHocKies
                          join hocSinhLop in db.HocSinh_HocSinhLopHocs
                            on diemMonHK.MaHocSinhLopHoc equals hocSinhLop.MaHocSinhLopHoc
                          where hocSinhLop.MaHocSinh == maHocSinh
                            && hocSinhLop.MaLopHoc == maLopHoc
                            && diemMonHK.MaHocKy == hocKy
                            && diemMonHK.MaMonHoc == maMonHoc
                          select diemMonHK;
            if (iqDiemMonHK.Count() != 0)
            {
                int maDiemMonHK = iqDiemMonHK.First().MaDiemMonHK;
                if (dicChiTietDiem.Count != 0)
                {
                    foreach (KeyValuePair<int, double> pair in dicChiTietDiem)
                    {
                        HocSinh_ChiTietDiem chiTietDiem = new HocSinh_ChiTietDiem
                        {
                            MaDiemMonHK = maDiemMonHK,
                            MaLoaiDiem = pair.Key,
                            Diem = pair.Value
                        };
                        db.HocSinh_ChiTietDiems.InsertOnSubmit(chiTietDiem);
                    }

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

        public void DeleteChiTietDiem(int maHocSinh, int maLopHoc,
            int hocKy, int maMonHoc, List<DanhMuc_LoaiDiem> lMarkTypes)
        {
            IQueryable<HocSinh_DiemMonHocHocKy> iqDiemMonHK;
            iqDiemMonHK = from diemMonHK in db.HocSinh_DiemMonHocHocKies
                          join hocSinhLop in db.HocSinh_HocSinhLopHocs
                            on diemMonHK.MaHocSinhLopHoc equals hocSinhLop.MaHocSinhLopHoc
                          where hocSinhLop.MaHocSinh == maHocSinh
                                && hocSinhLop.MaLopHoc == maLopHoc
                                && diemMonHK.MaHocKy == hocKy
                                && diemMonHK.MaMonHoc == maMonHoc
                          select diemMonHK;
            if (iqDiemMonHK.Count() != 0)
            {
                HocSinh_DiemMonHocHocKy diemMonHK = iqDiemMonHK.First();
                int maDiemMonHK = diemMonHK.MaDiemMonHK;
                MarkTypeDA loaiDiemDA = new MarkTypeDA();
                foreach (DanhMuc_LoaiDiem markType in lMarkTypes)
                {
                    IQueryable<HocSinh_ChiTietDiem> iqChiTietDiem;
                    iqChiTietDiem = from diem in db.HocSinh_ChiTietDiems
                                    where diem.MaDiemMonHK == maDiemMonHK
                                        && diem.MaLoaiDiem == markType.MaLoaiDiem
                                    select diem;
                    if (iqChiTietDiem.Count() != 0)
                    {
                        foreach (HocSinh_ChiTietDiem chiTietDiem in iqChiTietDiem)
                        {
                            db.HocSinh_ChiTietDiems.DeleteOnSubmit(chiTietDiem);
                        }
                        db.SubmitChanges();
                    }

                    if (markType.TinhDTB)
                    {
                        diemMonHK.DiemTB = -1;
                    }
                }
            }
        }

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

        public HocSinh_ChiTietDiem GetChiTietDiem(int maChiTietDiem)
        {
            IQueryable<HocSinh_ChiTietDiem> chiTietDiems = from ctDiem in db.HocSinh_ChiTietDiems
                                                           where ctDiem.MaChiTietDiem == maChiTietDiem
                                                           select ctDiem;
            if (chiTietDiems.Count() != 0)
            {
                return chiTietDiems.First();
            }
            else
            {
                return null;
            }
        }

        public void CalAvgMark(int maDiemMonHK)
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

        public double CalAvgMark(int maHocSinh, int maLopHoc,
            int hocKy, int maMonHoc)
        {
            double avgMark = -1;

            IQueryable<HocSinh_DiemMonHocHocKy> iqDiemMonHK;
            iqDiemMonHK = from diemMonHK in db.HocSinh_DiemMonHocHocKies
                          join hocSinhLop in db.HocSinh_HocSinhLopHocs
                            on diemMonHK.MaHocSinhLopHoc equals hocSinhLop.MaHocSinhLopHoc
                          where hocSinhLop.MaHocSinh == maHocSinh
                            && hocSinhLop.MaLopHoc == maLopHoc
                            && diemMonHK.MaHocKy == hocKy
                            && diemMonHK.MaMonHoc == maMonHoc
                          select diemMonHK;

            if (iqDiemMonHK.Count() != 0)
            {
                int maDiemMonHK = iqDiemMonHK.First().MaDiemMonHK;
                IQueryable<HocSinh_ChiTietDiem> iqChiTietDiem;
                iqChiTietDiem = from chiTietDiem in db.HocSinh_ChiTietDiems
                                where chiTietDiem.MaDiemMonHK == maDiemMonHK
                                select chiTietDiem;
                if (iqChiTietDiem.Count() != 0)
                {
                    double totalHeSoDiemLoai = 0;
                    double totalDiem = 0;
                    foreach (HocSinh_ChiTietDiem chiTietDiem in iqChiTietDiem)
                    {
                        DanhMuc_LoaiDiem loaiDiem = (from loai in db.DanhMuc_LoaiDiems
                                                     where loai.MaLoaiDiem == chiTietDiem.MaLoaiDiem
                                                     select loai).First();
                        totalHeSoDiemLoai += loaiDiem.HeSoDiem;
                        totalDiem += chiTietDiem.Diem * loaiDiem.HeSoDiem;
                    }

                    avgMark = totalDiem / totalHeSoDiemLoai;
                    avgMark = Math.Round(avgMark, 1, MidpointRounding.AwayFromZero);
                }
            }

            return avgMark;
        }
        #endregion

        public List<TabularKetQuaMonHoc> GetListTabularKetQuaMonHoc(
            int maNamHoc, int maHocKy, int maHocSinh,
            int pageCurrentIndex, int pageSize, out double totalRecords)
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

        public List<double> GetListDiemMonHocByLoaiDiem(int maDiemMonHK, int maLoaiDiem)
        {
            List<double> lstDiemMonHoc = new List<double>();
            IQueryable<double> diems = from ctDiem in db.HocSinh_ChiTietDiems
                                       where ctDiem.MaLoaiDiem == maLoaiDiem && ctDiem.MaDiemMonHK == maDiemMonHK
                                       select ctDiem.Diem;
            if (diems.Count() != 0)
            {
                lstDiemMonHoc = diems.ToList();
            }
            return lstDiemMonHoc;
        }

        public List<List<double>> GetListDiemMonHoc(int maDiemMonHK)
        {
            List<List<double>> lstDiemMonHoc = new List<List<double>>();

            MarkTypeDA loaiDiemDA = new MarkTypeDA();
            List<DanhMuc_LoaiDiem> lstLoaiDiem = loaiDiemDA.GetListMarkTypes();
            foreach (DanhMuc_LoaiDiem loaiDiem in lstLoaiDiem)
            {
                List<double> lstChiTietDiemMonHoc = GetListDiemMonHocByLoaiDiem(maDiemMonHK, loaiDiem.MaLoaiDiem);
                lstDiemMonHoc.Add(lstChiTietDiemMonHoc);
            }

            return lstDiemMonHoc;
        }

        public List<StrDiemMonHocLoaiDiem> GetListStringDiemMonHoc(int maDiemMonHK)
        {
            List<StrDiemMonHocLoaiDiem> lstStringDiemMonHoc = new List<StrDiemMonHocLoaiDiem>();

            List<List<double>> lstDiemMonHoc = GetListDiemMonHoc(maDiemMonHK);
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

        public HocSinh_ThongTinCaNhan GetHocSinh(int maDiemMonHK)
        {
            IQueryable<HocSinh_ThongTinCaNhan> hocSinh = from diemMonHK in db.HocSinh_DiemMonHocHocKies
                                                         join hs_lop in db.HocSinh_HocSinhLopHocs on diemMonHK.MaHocSinhLopHoc equals hs_lop.MaHocSinhLopHoc
                                                         join hs in db.HocSinh_ThongTinCaNhans on hs_lop.MaHocSinh equals hs.MaHocSinh
                                                         where diemMonHK.MaDiemMonHK == maDiemMonHK
                                                         select hs;
            if (hocSinh.Count() != 0)
            {
                return hocSinh.First();
            }
            else
            {
                return null;
            }
        }

        public CauHinh_HocKy GetHocKy(int maDiemMonHK)
        {
            IQueryable<CauHinh_HocKy> hocKy = from diemMonHK in db.HocSinh_DiemMonHocHocKies
                                              join hk in db.CauHinh_HocKies on diemMonHK.MaHocKy equals hk.MaHocKy
                                              where diemMonHK.MaDiemMonHK == maDiemMonHK
                                              select hk;
            if (hocKy.Count() != 0)
            {
                return hocKy.First();
            }
            else
            {
                return null;
            }
        }

        public CauHinh_NamHoc GetNamHoc(int maDiemMonHK)
        {
            IQueryable<CauHinh_NamHoc> namHoc = from diemMonHK in db.HocSinh_DiemMonHocHocKies
                                                join hs_lop in db.HocSinh_HocSinhLopHocs on diemMonHK.MaHocSinhLopHoc equals hs_lop.MaHocSinhLopHoc
                                                join lop in db.LopHoc_Lops on hs_lop.MaLopHoc equals lop.MaLopHoc
                                                join nam in db.CauHinh_NamHocs on lop.MaNamHoc equals nam.MaNamHoc
                                                where diemMonHK.MaDiemMonHK == maDiemMonHK
                                                select nam;
            if (namHoc.Count() != 0)
            {
                return namHoc.First();
            }
            else
            {
                return null;
            }
        }

        public DanhMuc_MonHoc GetMonHoc(int maDiemMonHK)
        {
            IQueryable<DanhMuc_MonHoc> monHoc = from diemMonHK in db.HocSinh_DiemMonHocHocKies
                                                join mon in db.DanhMuc_MonHocs on diemMonHK.MaMonHoc equals mon.MaMonHoc
                                                where diemMonHK.MaDiemMonHK == maDiemMonHK
                                                select mon;
            if (monHoc.Count() != 0)
            {
                return monHoc.First();
            }
            else
            {
                return null;
            }
        }

        public List<TabularChiTietDiemMonHocLoaiDiem> GetListTabularChiTietDiemMonHocLoaiDiem(int maDiemMonHK,
            int pageCurrentIndex, int pageSize, out double totalRecords)
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

        public double GetDiemTrungBinh(int maDiemMonHK)
        {
            double diemTB = (from diemMonHK in db.HocSinh_DiemMonHocHocKies
                             where diemMonHK.MaDiemMonHK == maDiemMonHK
                             select diemMonHK.DiemTB).First();
            return diemTB;
        }

        public void DeleteListDiemMonHocHocKy(int maMonHocTKB)
        {
            //IQueryable<HocSinh_DiemMonHocHocKy> diemMonHocHocKies = from diemMonHK in db.HocSinh_DiemMonHocHocKies
            //                                                        where diemMonHK.MaMonHocTKB == maMonHocTKB
            //                                                        select diemMonHK;
            //if (diemMonHocHocKies.Count() != 0)
            //{
            //    foreach (HocSinh_DiemMonHocHocKy diemMonHocHK in diemMonHocHocKies)
            //    {
            //        DeleteListChiTietDiem(diemMonHocHK.MaDiemMonHK);
            //        db.HocSinh_DiemMonHocHocKies.DeleteOnSubmit(diemMonHocHK);
            //        db.SubmitChanges();
            //    }
            //}
        }

        public void DeleteListChiTietDiem(int maDiemHonHocHK)
        {
            IQueryable<HocSinh_ChiTietDiem> chiTietDiems = from chiTietDiem in db.HocSinh_ChiTietDiems
                                                           where chiTietDiem.MaDiemMonHK == maDiemHonHocHK
                                                           select chiTietDiem;
            if (chiTietDiems.Count() != 0)
            {
                foreach (HocSinh_ChiTietDiem chiTietDiem in chiTietDiems)
                {
                    db.HocSinh_ChiTietDiems.DeleteOnSubmit(chiTietDiem);
                    db.SubmitChanges();
                }
            }
        }

        #region Danh hiệu
        public void UpDateDanhHieuHocSinhByHanhKiem(int maDanhHieuHSHK, int maHanhKiem)
        {
            HocSinh_DanhHieuHocKy danhHieuHocKy = (from danhHieu in db.HocSinh_DanhHieuHocKies
                                                   where danhHieu.MaDanhHieuHSHK == maDanhHieuHSHK
                                                   select danhHieu).First();
            danhHieuHocKy.MaHanhKiemHK = maHanhKiem;
            db.SubmitChanges();
        }

        public List<TabularTermStudentResult> GetListTabularTermStudentResult(int maHocSinh, int maNamHoc,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
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
            HocKyDA hocKyDA = new HocKyDA();
            foreach (HocSinh_DanhHieuHocKy danhHieu in danhHieuHocKies)
            {
                TabularTermStudentResult tbTermStudentResult = new TabularTermStudentResult();

                tbTermStudentResult.MaDanhHieuHSHK = danhHieu.MaDanhHieuHSHK;
                tbTermStudentResult.DiemTB = (int)danhHieu.DiemTBHK;
                tbTermStudentResult.StrDiemTB = (danhHieu.DiemTBHK != -1) ? (danhHieu.DiemTBHK.ToString()) : "";
                tbTermStudentResult.TenHocKy = hocKyDA.GetHocKy(danhHieu.MaHocKy).TenHocKy;
                tbTermStudentResult.MaHanhKiem = danhHieu.MaHanhKiemHK;
                int maHanhKiem = (int)tbTermStudentResult.MaHanhKiem;
                //tbTermStudentResult.TenHanhKiem = (maHanhKiem != -1) ? hanhKiemDA.GetConduct(maHanhKiem).TenHanhKiem : "";

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
            //danhHieuHocSinhCuoiNam.TenHanhKiem = (maHanhKiemCuoiNam != -1) ? hanhKiemDA.GetConduct(maHanhKiemCuoiNam).TenHanhKiem : "";

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
        #endregion

        public List<TabularDiemHocSinh> GetListDiemHocSinh(int maLopHoc, int maMonHoc,
            int maHocKy, List<DanhMuc_LoaiDiem> lLoaiDiems,
            int pageCurrentIndex, int pageSize, out double totalRecord)
        {
            IQueryable<TabularDiemHocSinh> iQDiemHocSinhs;
            iQDiemHocSinhs = from hocSinhLop in db.HocSinh_HocSinhLopHocs
                             join hocSinh in db.HocSinh_ThongTinCaNhans
                                on hocSinhLop.MaHocSinh equals hocSinh.MaHocSinh
                             join diemHocKy in db.HocSinh_DiemMonHocHocKies
                                on hocSinhLop.MaHocSinhLopHoc equals diemHocKy.MaHocSinhLopHoc
                             where hocSinhLop.MaLopHoc == maLopHoc && diemHocKy.MaMonHoc == maMonHoc
                             select new TabularDiemHocSinh
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
                List<TabularDiemHocSinh> lstTbDiemHS;
                lstTbDiemHS = iQDiemHocSinhs.OrderBy(diemHS => diemHS.TenHocSinh)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();

                MarkTypeDA loaiDiemDA = new MarkTypeDA();
                foreach (TabularDiemHocSinh tbDiemHS in lstTbDiemHS)
                {
                    tbDiemHS.DiemTheoLoaiDiems = new List<DiemTheoLoaiDiem>();
                    int maDiemMonHK = tbDiemHS.MaDiemHK;
                    foreach (DanhMuc_LoaiDiem loaiDiem in lLoaiDiems)
                    {
                        List<double> lstDiem = new List<double>();
                        IQueryable<HocSinh_ChiTietDiem> iQDiems;
                        iQDiems = from diem in db.HocSinh_ChiTietDiems
                                  where diem.MaLoaiDiem == loaiDiem.MaLoaiDiem
                                    && diem.MaDiemMonHK == maDiemMonHK
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
                            StringDiems = strDiems
                        };
                        tbDiemHS.DiemTheoLoaiDiems.Add(diemTheoLoaiDiem);
                    }
                }
                return lstTbDiemHS;
            }
            else
            {
                return new List<TabularDiemHocSinh>();
            }
        }
    }
}
