using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class ThoiKhoaBieuDA : BaseDA
    {
        public ThoiKhoaBieuDA()
            : base()
        {
        }

        #region Insert, Delete, Upadate
        public void Insert(LopHoc_MonHocTKB thoiKhoaBieuEntity)
        {
            // Insert new MonHocTKB
            db.LopHoc_MonHocTKBs.InsertOnSubmit(thoiKhoaBieuEntity);
            db.SubmitChanges();
            //--------------------

            // Check and insert DiemMonHocHocKy for HocSinh
            int maMonHoc = thoiKhoaBieuEntity.MaMonHoc;
            int maLopHoc = thoiKhoaBieuEntity.MaLopHoc;
            int maHocKy = thoiKhoaBieuEntity.MaHocKy;
            IQueryable<HocSinh_HocSinhLopHoc> hsLops = from hsLop in db.HocSinh_HocSinhLopHocs
                                                       where hsLop.MaLopHoc == maLopHoc
                                                       select hsLop;
            if (hsLops.Count() != 0)
            {
                HocSinh_HocSinhLopHoc firstHsLop = hsLops.First();
                IQueryable<HocSinh_DiemMonHocHocKy> diemMonHKs;
                diemMonHKs = from diemMonHK in db.HocSinh_DiemMonHocHocKies
                             where diemMonHK.MaHocSinhLopHoc == firstHsLop.MaHocSinhLopHoc
                                && diemMonHK.MaMonHoc == maMonHoc 
                             select diemMonHK;
                if (diemMonHKs.Count() == 0)
                {
                    foreach (HocSinh_HocSinhLopHoc hsLop in hsLops)
                    {
                        HocSinh_DiemMonHocHocKy diemHK = new HocSinh_DiemMonHocHocKy
                        {
                            MaHocSinhLopHoc = hsLop.MaHocSinhLopHoc,
                            MaMonHoc = maMonHoc,
                            MaHocKy = maHocKy,
                            DiemTB = -1
                        };
                        db.HocSinh_DiemMonHocHocKies.InsertOnSubmit(diemHK);                        
                    }
                    db.SubmitChanges();
                }
            }
        }

        //public void Update(int maLopHoc, int maNamHoc, int maHocKy, int maThu, int maBuoi,
        //    List<int> listMaMonHocs)
        //{
        //    IQueryable<LopHoc_MonHocTKB> monHocTKBs = from monHocTKB in db.LopHoc_MonHocTKBs
        //                                              join lop in db.LopHoc_Lops
        //                                                on monHocTKB.MaLopHoc equals lop.MaLopHoc
        //                                              where monHocTKB.MaLopHoc == maLopHoc
        //                                                && lop.MaNamHoc == maNamHoc
        //                                                && monHocTKB.MaHocKy == maHocKy
        //                                                && monHocTKB.MaThu == maThu
        //                                                && monHocTKB.MaBuoi == maBuoi
        //                                              select monHocTKB;
        //    if (monHocTKBs.Count() != 0)
        //    {
        //        foreach (LopHoc_MonHocTKB monHocTKB in monHocTKBs)
        //        {
        //            KetQuaHocTapDA ketQuaHocTapDA = new KetQuaHocTapDA();
        //            ketQuaHocTapDA.DeleteListDiemMonHocHocKy(monHocTKB.MaMonHocTKB);

        //            db.LopHoc_MonHocTKBs.DeleteOnSubmit(monHocTKB);
        //        }
        //        db.SubmitChanges();
        //    }

        //    foreach (int maMonHoc in listMaMonHocs)
        //    {
        //        db.LopHoc_MonHocTKBs.InsertOnSubmit(new LopHoc_MonHocTKB
        //        {
        //            MaHocKy = maHocKy,
        //            MaThu = maThu,
        //            MaBuoi = maBuoi,
        //            MaLopHoc = maLopHoc,
        //            MaMonHoc = maMonHoc,
        //            MaTiet = 1
        //        });
        //        db.SubmitChanges();

        //        // Thêm Điểm Môn Học Học Kì cho tất cả Học Sinh (thuộc Lớp chỉ định )
        //        // đối với Môn Học vừa được thêm vào Thời Khóa Biểu
        //        int maMonHocTKB = GetLastedMaMonHocTKB();
        //        HocSinhDA hocSinhDA = new HocSinhDA();
        //        List<HocSinh_HocSinhLopHoc> lstHocSinhLopHoc = hocSinhDA.GetListHocSinhLopHoc(maLopHoc);
        //        foreach (HocSinh_HocSinhLopHoc hocSinhLopHoc in lstHocSinhLopHoc)
        //        {
        //            db.HocSinh_DiemMonHocHocKies.InsertOnSubmit(
        //                new HocSinh_DiemMonHocHocKy
        //                {
        //                    MaHocSinhLopHoc = hocSinhLopHoc.MaHocSinhLopHoc,
        //                    MaMonHocTKB = maMonHocTKB,
        //                    DiemTB = -1
        //                });
        //            db.SubmitChanges();
        //        }
        //    }
        //}

        public void Update(int maMonHocTKB, int maMonHoc, int maGiaoVien)
        {
            IQueryable<LopHoc_MonHocTKB> basedNewMonHocMonHocTKB;
            basedNewMonHocMonHocTKB = from monTKB in db.LopHoc_MonHocTKBs
                                      where monTKB.MaMonHoc == maMonHoc
                                      select monTKB;                        
            bool bAdd = (basedNewMonHocMonHocTKB.Count() != 0) ? false : true;

            LopHoc_MonHocTKB monHocTKB = (from monTKB in db.LopHoc_MonHocTKBs
                                          where monTKB.MaMonHocTKB == maMonHocTKB
                                          select monTKB).First();
            int originalMaMonHoc = monHocTKB.MaMonHoc;
            monHocTKB.MaMonHoc = maMonHoc;
            monHocTKB.MaGiaoVien = maGiaoVien;
            db.SubmitChanges();

            IQueryable<LopHoc_MonHocTKB> basedOriginalMonHocMonHocTKB;
            basedOriginalMonHocMonHocTKB = from monTKB in db.LopHoc_MonHocTKBs
                                           where monTKB.MaMonHoc == originalMaMonHoc
                                           select monTKB;
            bool bRemove = (basedOriginalMonHocMonHocTKB.Count() != 0) ? false : true;

            IQueryable<HocSinh_HocSinhLopHoc> hsLops = from hsLop in db.HocSinh_HocSinhLopHocs
                                                       where hsLop.MaLopHoc == monHocTKB.MaLopHoc
                                                       select hsLop;
            if (bAdd)
            {                
                foreach (HocSinh_HocSinhLopHoc hsLop in hsLops)
                {
                    HocSinh_DiemMonHocHocKy diemHK = new HocSinh_DiemMonHocHocKy
                    {
                        MaHocSinhLopHoc = hsLop.MaHocSinhLopHoc,
                        MaMonHoc = maMonHoc,
                        MaHocKy = monHocTKB.MaHocKy,
                        DiemTB = -1
                    };
                    db.HocSinh_DiemMonHocHocKies.InsertOnSubmit(diemHK);
                } 
                db.SubmitChanges();
            }

            if (bRemove)
            {
                foreach (HocSinh_HocSinhLopHoc hsLop in hsLops)
                {
                    IQueryable<HocSinh_DiemMonHocHocKy> diemMonHKs;
                    diemMonHKs = from diemMonHK in db.HocSinh_DiemMonHocHocKies
                                 where diemMonHK.MaHocSinhLopHoc == hsLop.MaHocSinhLopHoc
                                 && diemMonHK.MaMonHoc == originalMaMonHoc
                                 select diemMonHK;
                    db.HocSinh_DiemMonHocHocKies.DeleteOnSubmit(diemMonHKs.First());
                }
                db.SubmitChanges();
            }            
        }

        public void Delete(int maMonHocTKB)
        {
            LopHoc_MonHocTKB MonHocTKB = (from monHocTKB in db.LopHoc_MonHocTKBs
                                          where monHocTKB.MaMonHocTKB == maMonHocTKB
                                          select monHocTKB).First();

            int maLopHoc = MonHocTKB.MaLopHoc;
            int originalMaMonHoc = MonHocTKB.MaMonHoc;            
            int maHocKy = MonHocTKB.MaHocKy;

            db.LopHoc_MonHocTKBs.DeleteOnSubmit(MonHocTKB);
            db.SubmitChanges();

            IQueryable<int> maMonHocs = from thoiKhoaBieu in db.LopHoc_MonHocTKBs
                                        where thoiKhoaBieu.MaMonHoc == originalMaMonHoc
                                        select thoiKhoaBieu.MaMonHoc;
            if (maMonHocs.Count() == 0)
            {
                IQueryable<HocSinh_DiemMonHocHocKy> diemHocKies;
                diemHocKies = from diemHocKy in db.HocSinh_DiemMonHocHocKies
                              join hsLop in db.HocSinh_HocSinhLopHocs 
                                on diemHocKy.MaHocSinhLopHoc equals hsLop.MaHocSinhLopHoc
                              where diemHocKy.MaHocKy == maHocKy 
                                && diemHocKy.MaMonHoc == originalMaMonHoc
                                && hsLop.MaLopHoc == maLopHoc
                              select diemHocKy;
                if (diemHocKies.Count() != 0)
                {
                    foreach (HocSinh_DiemMonHocHocKy diemHocKy in diemHocKies)
                    {
                        db.HocSinh_DiemMonHocHocKies.DeleteOnSubmit(diemHocKy);
                    }
                    db.SubmitChanges();
                }
            }            
        }

        #endregion

        public bool ThoiKhoaBieuExists(int maLopHoc, int maMonHoc,
            int maNamHoc, int maHocKy, int maThu, int maBuoi)
        {
            IQueryable<LopHoc_MonHocTKB> monHocTKBs;
            monHocTKBs = from monHocTKB in db.LopHoc_MonHocTKBs
                         join lop in db.LopHoc_Lops on monHocTKB.MaLopHoc equals lop.MaLopHoc
                         where monHocTKB.MaLopHoc == maLopHoc
                            && monHocTKB.MaMonHoc == maMonHoc
                            && lop.MaNamHoc == maNamHoc && monHocTKB.MaHocKy == maHocKy
                            && monHocTKB.MaThu == maThu && monHocTKB.MaBuoi == maBuoi
                         select monHocTKB;
            if (monHocTKBs.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region Get List
        public List<ThoiKhoaBieuTheoTiet> GetThoiKhoaBieuTheoBuoi(int maLopHoc,
            int maNamHoc, int maHocKy, int maThu, int maBuoi)
        {
            List<ThoiKhoaBieuTheoTiet> lstThoiKhoaBieuTheoTiet = new List<ThoiKhoaBieuTheoTiet>();

            IQueryable<LopHoc_MonHocTKB> monHocTKBs = from monHocTKB in db.LopHoc_MonHocTKBs
                                                      join lop in db.LopHoc_Lops on monHocTKB.MaLopHoc equals lop.MaLopHoc
                                                      where monHocTKB.MaLopHoc == maLopHoc
                                                        && lop.MaNamHoc == maNamHoc
                                                        && monHocTKB.MaHocKy == maHocKy
                                                        && monHocTKB.MaThu == maThu
                                                        && monHocTKB.MaBuoi == maBuoi
                                                      select monHocTKB;
            if (monHocTKBs.Count() != 0)
            {
                List<LopHoc_MonHocTKB> lstMonHocTKB = monHocTKBs.OrderBy(b => b.MaTiet).ToList();
                SubjectDA monHocDA = new SubjectDA();
                TietDA tietDA = new TietDA();
                TeacherDA giaoVienDA = new TeacherDA();
                foreach (LopHoc_MonHocTKB monHocTKB in lstMonHocTKB)
                {
                    ThoiKhoaBieuTheoTiet tkbTheoTiet = new ThoiKhoaBieuTheoTiet();
                    tkbTheoTiet.Tiet = monHocTKB.MaTiet;
                    tkbTheoTiet.MaMonHoc = monHocTKB.MaMonHoc;
                    tkbTheoTiet.TenMonHoc = monHocTKB.DanhMuc_MonHoc.TenMonHoc;
                    DanhMuc_Tiet tiet = tietDA.GetTiet(tkbTheoTiet.Tiet);
                    tkbTheoTiet.ChiTietTiet = string.Format("<b>{0}</b><br/>({1}-{2})",
                        tiet.TenTiet,
                        tiet.ThoiDiemKetThu.ToShortTimeString(),
                        tiet.ThoiDiemKetThu.ToShortTimeString());
                    tkbTheoTiet.MaGiaoVien = monHocTKB.MaGiaoVien;
                    tkbTheoTiet.TenGiaoVien = giaoVienDA.GetTeacher(tkbTheoTiet.MaGiaoVien).HoTen;
                    lstThoiKhoaBieuTheoTiet.Add(tkbTheoTiet);
                }
            }

            return lstThoiKhoaBieuTheoTiet;
        }

        public List<ThoiKhoaBieuTheoBuoi> GetThoiKhoaBieuTheoThu(int maLopHoc, int maNamHoc, int maHocKy, int maThu)
        {
            List<ThoiKhoaBieuTheoBuoi> lstThoiKhoaBieuTheoBuoi = new List<ThoiKhoaBieuTheoBuoi>();

            List<CauHinh_Buoi> lstBuois = (from buoi in db.CauHinh_Buois
                                           select buoi).OrderBy(b => b.MaBuoi).ToList();
            for (int i = 0; i < lstBuois.Count; i++)
            {
                int maBuoi = lstBuois[i].MaBuoi;
                ThoiKhoaBieuTheoBuoi thoiKhoaBieuTheoBuoi = new ThoiKhoaBieuTheoBuoi
                {
                    MaBuoi = maBuoi,
                    ListThoiKhoaBieuTheoTiet = GetThoiKhoaBieuTheoBuoi(maLopHoc, maNamHoc, maHocKy, maThu, maBuoi)
                };

                lstThoiKhoaBieuTheoBuoi.Add(thoiKhoaBieuTheoBuoi);
            }

            return lstThoiKhoaBieuTheoBuoi;
        }

        public List<ThoiKhoaBieuTheoThu> GetThoiKhoaBieu(int maNamHoc, int maHocKy, int maLopHoc)
        {
            List<ThoiKhoaBieuTheoThu> lstThoiKhoaBieuTheoThu = new List<ThoiKhoaBieuTheoThu>();

            List<CauHinh_Thu> lstThus = (from t in db.CauHinh_Thus
                                         select t).OrderBy(t => t.MaThu).ToList();
            for (int i = 0; i < lstThus.Count(); i++)
            {
                int maThu = lstThus[i].MaThu;
                string tenThu = lstThus[i].TenThu;

                ThoiKhoaBieuTheoThu monHocTKBThuInfo = new ThoiKhoaBieuTheoThu()
                {
                    MaNamHoc = maNamHoc,
                    MaHocKy = maHocKy,
                    MaLopHoc = maLopHoc,
                    MaThu = maThu,
                    TenThu = tenThu,
                    ListThoiKhoaBieuTheoBuoi = GetThoiKhoaBieuTheoThu(maLopHoc, maNamHoc, maHocKy, maThu)
                };
                lstThoiKhoaBieuTheoThu.Add(monHocTKBThuInfo);
            }

            return lstThoiKhoaBieuTheoThu;
        }
        #endregion

        public int GetLastedMaMonHocTKB()
        {
            IQueryable<LopHoc_MonHocTKB> monHocTKBs = from monTKB in db.LopHoc_MonHocTKBs
                                                      select monTKB;
            if (monHocTKBs.Count() != 0)
            {
                return monHocTKBs.OrderByDescending(monTKB => monTKB.MaMonHocTKB).First().MaMonHocTKB;
            }
            else
            {
                return 0;
            }
        }

        public List<ThoiKhoaBieuTheoTiet> GetThoiKhoaBieuTheoBuoi(int maLopHoc,
            int maHocKy, int maThu)
        {
            SubjectDA monHocDA = new SubjectDA();
            TeacherDA giaoVienDA = new TeacherDA();
            List<ThoiKhoaBieuTheoTiet> lstThoiKhoaBieuTheoTiet = new List<ThoiKhoaBieuTheoTiet>();
            List<DanhMuc_Tiet> listTiets = (new TietDA()).GetListTiets();
            foreach (DanhMuc_Tiet tiet in listTiets)
            {
                ThoiKhoaBieuTheoTiet tkbTheoTiet = new ThoiKhoaBieuTheoTiet();
                tkbTheoTiet.Tiet = tiet.MaTiet;
                tkbTheoTiet.ChiTietTiet = string.Format("<b>{0}</b><br/> ({1}-{2})",
                        tiet.TenTiet,
                        tiet.ThoiDiemKetThu.ToShortTimeString(),
                        tiet.ThoiDiemKetThu.ToShortTimeString());

                IQueryable<LopHoc_MonHocTKB> monHocTKBs = from monHocTKB in db.LopHoc_MonHocTKBs
                                                          join lop in db.LopHoc_Lops
                                                            on monHocTKB.MaLopHoc equals lop.MaLopHoc
                                                          where monHocTKB.MaLopHoc == maLopHoc
                                                            && monHocTKB.MaHocKy == maHocKy
                                                            && monHocTKB.MaThu == maThu
                                                            && monHocTKB.MaTiet == tiet.MaTiet
                                                          select monHocTKB;
                if (monHocTKBs.Count() != 0)
                {
                    LopHoc_MonHocTKB monHocTKB = monHocTKBs.First();
                    tkbTheoTiet.MaMonHocTKB = monHocTKB.MaMonHocTKB;
                    tkbTheoTiet.MaMonHoc = monHocTKB.MaMonHoc;
                    tkbTheoTiet.TenMonHoc = monHocTKB.DanhMuc_MonHoc.TenMonHoc;
                    tkbTheoTiet.MaGiaoVien = monHocTKB.MaGiaoVien;
                    tkbTheoTiet.TenGiaoVien = giaoVienDA.GetTeacher(tkbTheoTiet.MaGiaoVien).HoTen;
                }
                else
                {
                    tkbTheoTiet.MaMonHocTKB = 0;
                    tkbTheoTiet.MaMonHoc = 0;
                    tkbTheoTiet.TenMonHoc = "Nghỉ";
                }

                lstThoiKhoaBieuTheoTiet.Add(tkbTheoTiet);
            }

            return lstThoiKhoaBieuTheoTiet;
        }

        public ThoiKhoaBieuTheoTiet GetThoiKhoaBieuTheoTiet(int maMonHocTKB)
        {
            ThoiKhoaBieuTheoTiet thoiKhoaBieuTheoTiet = null;
            LopHoc_MonHocTKB monHocTKB = null;
            IQueryable<LopHoc_MonHocTKB> iqMonHocTKB;

            iqMonHocTKB = from monTKB in db.LopHoc_MonHocTKBs
                          where monTKB.MaMonHocTKB == maMonHocTKB
                          select monTKB;

            thoiKhoaBieuTheoTiet = (from MonTKB in db.LopHoc_MonHocTKBs
                                                         where MonTKB.MaMonHocTKB == maMonHocTKB
                                                         select new ThoiKhoaBieuTheoTiet
                                                         {
                                                             MaMonHocTKB = maMonHocTKB,
                                                             MaLopHoc = MonTKB.MaLopHoc,
                                                             MaMonHoc = MonTKB.MaMonHoc,
                                                             MaGiaoVien = MonTKB.MaGiaoVien,
                                                             Tiet = MonTKB.MaTiet,
                                                             MaThu = MonTKB.MaThu,
                                                             MaHocKy = MonTKB.MaHocKy
                                                         }).First();

            if(iqMonHocTKB.Count() != 0)
            {
                monHocTKB = iqMonHocTKB.First();

                thoiKhoaBieuTheoTiet = new ThoiKhoaBieuTheoTiet();
                thoiKhoaBieuTheoTiet.MaMonHocTKB = monHocTKB.MaMonHocTKB;
                thoiKhoaBieuTheoTiet.MaLopHoc = monHocTKB.MaLopHoc;
                thoiKhoaBieuTheoTiet.MaMonHoc = monHocTKB.MaMonHoc;
                thoiKhoaBieuTheoTiet.TenMonHoc = monHocTKB.DanhMuc_MonHoc.TenMonHoc;
                thoiKhoaBieuTheoTiet.MaGiaoVien = monHocTKB.MaGiaoVien;
                thoiKhoaBieuTheoTiet.TenGiaoVien = monHocTKB.LopHoc_GiaoVien.HoTen;
                thoiKhoaBieuTheoTiet.Tiet = monHocTKB.DanhMuc_Tiet.MaTiet;
                thoiKhoaBieuTheoTiet.MaThu = monHocTKB.CauHinh_Thu.MaThu;
            }

            return thoiKhoaBieuTheoTiet;
        }

        public List<DanhMuc_MonHoc> GetListMonHoc(int maLopHoc, int maHocKy)
        {
            IQueryable <DanhMuc_MonHoc> monHocs = from monHoc in db.DanhMuc_MonHocs
                                                  join tkb in db.LopHoc_MonHocTKBs on monHoc.MaMonHoc equals tkb.MaMonHoc
                                                  where tkb.MaLopHoc == maLopHoc && tkb.MaHocKy == maHocKy
                                                  select monHoc;
            if (monHocs.Count() != 0)
            {
                return monHocs.OrderBy(monHoc => monHoc.TenMonHoc).ToList();
            }
            else
            {
                return new List<DanhMuc_MonHoc>();
            }
        }
    }
}
