using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class TietBL
    {
        private TietDA tietDA;

        public TietBL()
        {
            tietDA = new TietDA();
        }

        public void Delete(int maTiet)
        {
            tietDA.Delete(maTiet);
        }

        public void InsertTiet(string tenTiet, int buoi, string thuTu,
            string strThoiGianBatDau, string strThoiGianKetThuc)
        {
            string[] strThoiGianBatDaus = strThoiGianBatDau.Split(':');
            int iGioBatDau = Int32.Parse(strThoiGianBatDaus[0]);
            int iPhutBatDau = Int32.Parse(strThoiGianBatDaus[1]);
            DateTime dtThoiGianBatDau = new DateTime(2000, 1, 1, iGioBatDau, iPhutBatDau, 0);

            string[] strThoiGianKetThucs = strThoiGianKetThuc.Split(':');
            int iGioKetThuc = Int32.Parse(strThoiGianKetThucs[0]);
            int iPhutKetThuc = Int32.Parse(strThoiGianKetThucs[1]);
            DateTime dtThoiGianKetThuc = new DateTime(2000, 1, 1, iGioKetThuc, iPhutKetThuc, 0);

            int iThuTu = Int32.Parse(thuTu);

            tietDA.InsertTiet(tenTiet, buoi, iThuTu, dtThoiGianBatDau, dtThoiGianKetThuc);
        }

        public void UpdateTiet(int maTiet, string tenTietMoi, int buoi, string thuTu, 
            string strThoiGianBatDau, string strThoiGianKetThuc)
        {
            string[] strThoiGianBatDaus = strThoiGianBatDau.Split(':');
            int iGioBatDau = Int32.Parse(strThoiGianBatDaus[0]);
            int iPhutBatDau = Int32.Parse(strThoiGianBatDaus[1]);
            DateTime dtThoiGianBatDau = new DateTime(2000, 1, 1, iGioBatDau, iPhutBatDau, 0);

            string[] strThoiGianKetThucs = strThoiGianKetThuc.Split(':');
            int iGioKetThuc = Int32.Parse(strThoiGianKetThucs[0]);
            int iPhutKetThuc = Int32.Parse(strThoiGianKetThucs[1]);
            DateTime dtThoiGianKetThuc = new DateTime(2000, 1, 1, iGioKetThuc, iPhutKetThuc, 0);

            int iThuTu = Int32.Parse(thuTu);

            tietDA.UpdateTiet(maTiet, tenTietMoi, buoi, iThuTu, dtThoiGianBatDau, dtThoiGianKetThuc);
        }

        public DanhMuc_Tiet GetTiet(int maTiet)
        {
            return tietDA.GetTiet(maTiet);
        }
        
        public List<TabularTiet> GetTabularTiet(string tenTiet, int maBuoi, 
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularTiet> listTbTiets;
            if ((tenTiet == "") || (string.Compare(tenTiet, "tất cả", 0) == 0))
            {
                if (maBuoi == 0)
                {
                    listTbTiets = tietDA.GetTabularTiet(
                        pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    listTbTiets = tietDA.GetTabularTiet(maBuoi,
                        pageCurrentIndex, pageSize, out totalRecords);
                }                
            }
            else
            {
                if (maBuoi == 0)
                {
                    listTbTiets = tietDA.GetTabularTiet(tenTiet, 
                        pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    listTbTiets = tietDA.GetTabularTiet(tenTiet, maBuoi,
                        pageCurrentIndex, pageSize, out totalRecords);
                }
            }

            return listTbTiets;
        }

        public bool TietCanBeDeleted(int maTiet)
        {
            return tietDA.TietCanBeDeleted(maTiet);
        }
        
        public string GetChiTietTiet(int maTiet)
        {
            return tietDA.GetChiTietTiet(maTiet);
        }

        public CauHinh_Buoi GetBuoi(int maTiet)
        {
            return tietDA.GetBuoi(maTiet);
        }

        public bool TietHocExists(string tenTiet)
        {
            return tietDA.TietHocExists(tenTiet);
        }

        public bool TietHocExists(string tenTietMoi, int maTiet)
        {
            return tietDA.TietHocExists(tenTietMoi, maTiet);
        }

        
    }
}
