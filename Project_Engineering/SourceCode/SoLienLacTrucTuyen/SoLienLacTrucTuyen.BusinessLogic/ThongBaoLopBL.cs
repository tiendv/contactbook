//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using SoLienLacTrucTuyen.DataAccess;
//using SoLienLacTrucTuyen.BusinessEntity;

//namespace SoLienLacTrucTuyen.BusinessLogic
//{
//    public class ThongBaoLopBL
//    {
//        private ThongBaoLopDA thongBaoLopDA;

//        public ThongBaoLopBL()
//        {
//            thongBaoLopDA = new ThongBaoLopDA();
//        }

//        #region Insert, Update, Delete
//        public void InsertThongBaoLop(int maLopHoc, string tieuDe, string noiDung,
//            int maHocKy, DateTime ngay, bool apDungHoatDong, bool apDungLoiNhanKhan)
//        {
//            LopHoc_ThongBao thongBaoLop = new LopHoc_ThongBao
//            {
//                MaLopHoc = maLopHoc,
//                TieuDe = tieuDe,
//                NoiDung = noiDung,
//                Ngay = ngay
//            };

//            db.LopHoc_ThongBaos.InsertOnSubmit(thongBaoLop);
//            db.SubmitChanges();

//            int? lastedMaThongBao = GetLastedMaThongBao();
//            if (lastedMaThongBao == null)
//            {
//                return;
//            }

//            if (apDungHoatDong || apDungLoiNhanKhan)
//            {
//                IQueryable<HocSinh_HocSinhLopHoc> hsLops;
//                hsLops = from hsLop in db.HocSinh_HocSinhLopHocs
//                         where hsLop.MaLopHoc == maLopHoc
//                         select hsLop;

//                if (hsLops.Count() != 0)
//                {
//                    foreach (HocSinh_HocSinhLopHoc hsLop in hsLops)
//                    {
//                        if (apDungHoatDong)
//                        {
//                            HocSinh_HoatDong hoatDong = new HocSinh_HoatDong
//                            {
//                                MaHocSinhLopHoc = hsLop.MaHocSinhLopHoc,
//                                TieuDe = tieuDe,
//                                NoiDung = noiDung,
//                                Ngay = ngay,
//                                MaThongBaoLop = lastedMaThongBao,
//                                MaHocKy = maHocKy
//                            };
//                            db.HocSinh_HoatDongs.InsertOnSubmit(hoatDong);
//                        }

//                        if (apDungLoiNhanKhan)
//                        {
//                            LoiNhanKhan_LoiNhanKhan loiNhanKhan = new LoiNhanKhan_LoiNhanKhan
//                            {
//                                MaHocSinhLopHoc = hsLop.MaHocSinhLopHoc,
//                                TieuDe = tieuDe,
//                                NoiDung = noiDung,
//                                Ngay = ngay,
//                                MaThongBaoLop = lastedMaThongBao,
//                                XacNhan = false
//                            };
//                            db.LoiNhanKhan_LoiNhanKhans.InsertOnSubmit(loiNhanKhan);
//                        }
//                    }
//                    db.SubmitChanges();
//                }
//            }
//        }

//        public void UpdateThongBaoLop(int maThongBaoLop, string tieuDe, string noiDung,
//            DateTime ngay)
//        {
//            LopHoc_ThongBao thongBao = (from thBao in db.LopHoc_ThongBaos
//                                        where thBao.MaThongBaoLop == maThongBaoLop
//                                        select thBao).First();
//            thongBao.TieuDe = tieuDe;
//            thongBao.NoiDung = noiDung;
//            thongBao.Ngay = ngay;

//            db.SubmitChanges();
//        }

//        public void DeleteLopHoc(int maThongBaoLop)
//        {
//            LopHoc_ThongBao thongBao = (from thBao in db.LopHoc_ThongBaos
//                                        where thBao.MaThongBaoLop == maThongBaoLop
//                                        select thBao).First();
//            db.LopHoc_ThongBaos.DeleteOnSubmit(thongBao);
//            db.SubmitChanges();
//        }
//        #endregion

//        public LopHoc_ThongBao GetThongBaoLop(int maThongBao)
//        {
//            IQueryable<LopHoc_ThongBao> thongBaos = from thBao in db.LopHoc_ThongBaos
//                                                    where thBao.MaThongBaoLop == maThongBao
//                                                    select thBao;
//            if (thongBaos.Count() != 0)
//            {
//                return thongBaos.First();
//            }
//            else
//            {
//                return null;
//            }
//        }

//        public List<TabularThongBaoLop> GetListTabularThongBaoLop(int maNamHoc,
//            DateTime tuNgay, DateTime denNgay,
//            int pageCurrentIndex, int pageSize, out double totalRecords)
//        {
//            return thongBaoLopDA.GetListTabularThongBaoLop(maNamHoc, tuNgay, denNgay,
//                pageCurrentIndex, pageSize, out totalRecords);
//        }

//        public List<TabularThongBaoLop> GetListTabularThongBaoLop(int maNamHoc, 
//            DateTime tuNgay, DateTime denNgay,
//            bool xacNhan,
//            int pageCurrentIndex, int pageSize, out double totalRecords)
//        {
//            return thongBaoLopDA.GetListTabularThongBaoLop(maNamHoc, tuNgay, denNgay,
//                 pageCurrentIndex, pageSize, out totalRecords);
//        }

//        public List<TabularThongBaoLop> GetListTabularThongBaoLop(int maNamHoc, DateTime tuNgay, DateTime denNgay,
//            string maHocSinhHienThi, bool xacNhan,
//            int pageCurrentIndex, int pageSize, out double totalRecords)
//        {
//            return thongBaoLopDA.GetListTabularThongBaoLop(maNamHoc, tuNgay, denNgay,
//                pageCurrentIndex, pageSize, out totalRecords);
//        }

//        public List<TabularThongBaoLop> GetListTabularThongBaoLop(int maNamHoc, 
//            DateTime tuNgay, DateTime denNgay,
//            string maHocSinhHienThi,
//            int pageCurrentIndex, int pageSize, out double totalRecords)
//        {
//            return thongBaoLopDA.GetListTabularThongBaoLop(maNamHoc, tuNgay, denNgay,
//                pageCurrentIndex, pageSize, out totalRecords);
//        }

//        public void UpdateThongBao(int maThongBaoLop, bool xacNhan)
//        {
//            thongBaoLopDA.UpdateThongBao(maThongBaoLop, xacNhan);
//        }

//        public int? GetLastedMaThongBao()
//        {
//            return thongBaoLopDA.GetLastedMaThongBao();
//        }
//    }
//}
