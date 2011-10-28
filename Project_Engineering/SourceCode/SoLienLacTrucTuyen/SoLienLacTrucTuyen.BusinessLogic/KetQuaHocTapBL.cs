using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class KetQuaHocTapBL
    {
        private KetQuaHocTapDA kqhtDA;

        public KetQuaHocTapBL()
        {
            kqhtDA = new KetQuaHocTapDA();
        }

        #region Chi tiết điểm (theo môn, loại)
        public void InsertChiTietDiem(int maHocSinh, int maLopHoc,
            int hocKy, int maMonHoc,
            int maLoaiDiem, double diem)
        {
            kqhtDA.InsertChiTietDiem(maHocSinh, maLopHoc, hocKy, maMonHoc,
                maLoaiDiem, diem);
        }

        public void InsertChiTietDiem(int maHocSinh, int maLopHoc,
            int hocKy, int maMonHoc, Dictionary<int, double> dicChiTietDiem)
        {

            kqhtDA.InsertChiTietDiem(maHocSinh, maLopHoc, hocKy, maMonHoc,
                dicChiTietDiem);
        }

        public void UpdateChiTietDiem(int maHocSinh, int maLopHoc,
            int hocKy, int maMonHoc, List<Diem> chiTietDiems)
        {
            List<int> lMaLoaiDiems = new List<int>();
            foreach (Diem diem in chiTietDiems)
            {
                lMaLoaiDiems.Add(diem.MaLoaiDiem);
            }
            lMaLoaiDiems = lMaLoaiDiems.Distinct().ToList();
            kqhtDA.DeleteChiTietDiem(maHocSinh, maLopHoc, hocKy, maMonHoc, lMaLoaiDiems);

            int i = 0;
            while(i < chiTietDiems.Count)
            {
                if (chiTietDiems[i].GiaTri == -1)
                {
                    chiTietDiems.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            kqhtDA.InsertChiTietDiem(maHocSinh, maLopHoc, hocKy, maMonHoc,
                chiTietDiems);
        }

        public void InsertChiTietDiem(int maDiemMonHK, int maLoaiDiem, double diem)
        {
            kqhtDA.InsertChiTietDiem(maDiemMonHK, maLoaiDiem, diem);
        }

        public void UpdateChiTietDiem(int maChiTietDiem, double diem)
        {
            kqhtDA.UpdateChiTietDiem(maChiTietDiem, diem);
        }

        public void DeleteChiTietDiem(int maChiTietDiem)
        {
            kqhtDA.DeleteChiTietDiem(maChiTietDiem);
        }

        public HocSinh_ChiTietDiem GetChiTietDiem(int maChiTietDiem)
        {
            return kqhtDA.GetChiTietDiem(maChiTietDiem);
        }        
        #endregion

        public List<TabularKetQuaMonHoc> GetListTabularKetQuaMonHoc(int maNamHoc, int maHocKy, int maHocSinh,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            return kqhtDA.GetListTabularKetQuaMonHoc(maNamHoc, maHocKy, maHocSinh,
                pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<StrDiemMonHocLoaiDiem> GetListStringDiemMonHoc(int maDiemMonHK)
        {
            return kqhtDA.GetListStringDiemMonHoc(maDiemMonHK);
        }

        #region Get infomation by MaDiemMonHK
        public HocSinh_ThongTinCaNhan GetHocSinh(int maDiemMonHK)
        {
            return kqhtDA.GetHocSinh(maDiemMonHK);
        }

        public CauHinh_NamHoc GetNamHoc(int maDiemMonHK)
        {
            return kqhtDA.GetNamHoc(maDiemMonHK);
        }

        public CauHinh_HocKy GetHocKy(int maDiemMonHK)
        {
            return kqhtDA.GetHocKy(maDiemMonHK);
        }        

        public DanhMuc_MonHoc GetMonHoc(int maDiemMonHK)
        {
            return kqhtDA.GetMonHoc(maDiemMonHK);
        }

        public double GetDiemTrungBinh(int maDiemMonHK)
        {
            return kqhtDA.GetDiemTrungBinh(maDiemMonHK);
        }

        public List<TabularChiTietDiemMonHocLoaiDiem> GetListTabularChiTietDiemMonHocLoaiDiem(int maDiemMonHK, 
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            return kqhtDA.GetListTabularChiTietDiemMonHocLoaiDiem(maDiemMonHK, 
                pageCurrentIndex, pageSize, out totalRecords);
        }
        #endregion

        #region Danh hiệu
        public void UpDateDanhHieuHocSinhByHanhKiem(int maDanhHieuHSHK, int maHanhKiem)
        {
            kqhtDA.UpDateDanhHieuHocSinhByHanhKiem(maDanhHieuHSHK, maHanhKiem);
        }

        public List<TabularDanhHieuHocSinh> GetListTabularDanhHieuHocSinh(int maHocSinh, int maNamHoc,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            return kqhtDA.GetListTabularDanhHieuHocSinh(maHocSinh, maNamHoc,
                pageCurrentIndex, pageSize, out totalRecords);
        }
        #endregion

        public List<TabularDiemHocSinh> GetListDiemHocSinh(int maLopHoc, int maMonHoc, 
            int maHocKy, int maLoaiDiem,
            int pageCurrentIndex, int pageSize, out double totalRecord)
        {
            LoaiDiemBL loaiDiemBL = new LoaiDiemBL();
            List<DanhMuc_LoaiDiem> lLoaiDiems = loaiDiemBL.GetListLoaiDiem(maLoaiDiem);
            return kqhtDA.GetListDiemHocSinh(maLopHoc, maMonHoc, maHocKy,
                lLoaiDiems,
                pageCurrentIndex, pageSize, out totalRecord);
        }

        public bool ValidateMark(string marks, int markTypeCode)
        {
            marks = marks.Trim();
            if (marks != "")
            {
                string[] strMarks = marks.Split(',');
                short totalMarkCount = 0;
                foreach (string strMark in strMarks)
                {
                    double dMark = -1;
                    if (double.TryParse(strMark.Trim(), out dMark))
                    {
                        if (dMark > 10)
                        {
                            return false;
                        }
                        else
                        {
                            totalMarkCount++;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

                LoaiDiemBL loaiDiemBL = new LoaiDiemBL();
                DanhMuc_LoaiDiem loaiDiem = loaiDiemBL.GetLoaiDiem(markTypeCode);
                if (totalMarkCount > loaiDiem.SoCotToiDa)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
