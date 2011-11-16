using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class DanhHieuBL: BaseBL
    {
        private DanhHieuDA danhHieuDA;

        public DanhHieuBL(School school)
            : base(school)
        {
            danhHieuDA = new DanhHieuDA(school);
        }

        public void InsertDanhHieu(string tenDanhHieu, Dictionary<int, int> dicHanhKiemNHocLuc)
        {
            danhHieuDA.InsertDanhHieu(tenDanhHieu, dicHanhKiemNHocLuc);
        }

        public void UpdateDanhHieu(int maDanhHieu, string tenDanhHieu)
        {
            danhHieuDA.UpdateDanhHieu(maDanhHieu, tenDanhHieu);
        }

        public void UpdateDanhHieu(int maDanhHieu, Dictionary<int, int> dicHanhKiemNHocLuc)
        {
            danhHieuDA.UpdateDanhHieu(maDanhHieu, dicHanhKiemNHocLuc);
        }

        public void DeleteDanhHieu(int maDanhHieu)
        {
            danhHieuDA.DeleteDanhHieu(maDanhHieu);
        }

        public void DeleteChiTietDanhHieu(int maDanhHieu, int maHocLuc, int maHanhKiem)
        {
            danhHieuDA.DeleteChiTietDanhHieu(maDanhHieu, maHocLuc, maHanhKiem);
        }

        public bool DanhHieuExists(int exceptedMaDanhHieu, string tenDanhHieu)
        {
            return danhHieuDA.DanhHieuExists(exceptedMaDanhHieu, tenDanhHieu);
        }

        public bool DanhHieuExists(string tenDanhHieu)
        {
            return danhHieuDA.DanhHieuExists(tenDanhHieu);
        }

        public List<DanhMuc_DanhHieu> GetListDanhHieus(string tenDanhHieu, 
            int pageCurrentIndex, int pageSize, out double totalRecord)
        {
            if (String.Compare(tenDanhHieu, "tất cả", true) == 0 || tenDanhHieu == "")
            {
                return danhHieuDA.GetListDanhHieus(pageCurrentIndex, pageSize, out totalRecord);
            }
            else
            {
                return danhHieuDA.GetListDanhHieus(tenDanhHieu, pageCurrentIndex, pageSize, out totalRecord);
            }
        }

        public DanhMuc_DanhHieu GetDanhHieu(int maDanhHieu)
        {
            return danhHieuDA.GetDanhHieu(maDanhHieu);
        }

        public bool CanDeleteDanhHieu(int maDanhHieu)
        {
            return danhHieuDA.CanDeleteDanhHieu(maDanhHieu);
        }
    }
}
