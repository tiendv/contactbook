using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class LoaiDiemBL
    {
        LoaiDiemDA loaiDiemDA;

        public LoaiDiemBL()
        {
            loaiDiemDA = new LoaiDiemDA();
        }

        public void InsertLoaiDiem(string tenLoaiDiem, double heSoDiem, 
            short maxMarksPerTerm, bool calAverageMark)
        {
            DanhMuc_LoaiDiem loaiDiem = new DanhMuc_LoaiDiem
            {
                TenLoaiDiem = tenLoaiDiem,
                HeSoDiem = heSoDiem,
                SoCotToiDa = maxMarksPerTerm,
                TinhDTB = calAverageMark
            };

            loaiDiemDA.InsertLoaiDiem(loaiDiem);
        }

        public void UpdateLoaiDiem(int maLoaiDiem, 
            string tenLoaiDiem, double heSoDiem,
            short maxMarksPerTerm, bool calAverageMark)
        {
            loaiDiemDA.UpdateLoaiDiem(maLoaiDiem, tenLoaiDiem, heSoDiem, maxMarksPerTerm, calAverageMark);
        }

        public void DeleteLoaiDiem(int maLoaiDiem)
        {
            loaiDiemDA.DeleteLoaiDiem(maLoaiDiem);
        }

        public DanhMuc_LoaiDiem GetLoaiDiem(int maLoaiDiem)
        {
            return loaiDiemDA.GetLoaiDiem(maLoaiDiem);
        }

        // Get list of LoaiDiem
        public List<DanhMuc_LoaiDiem> GetListLoaiDiem()
        {
            return loaiDiemDA.GetListLoaiDiem();
        }

        public List<DanhMuc_LoaiDiem> GetListLoaiDiem(int maLoaiDiem)
        {
            List<DanhMuc_LoaiDiem> lLoaiDiems = new List<DanhMuc_LoaiDiem>();
            if (maLoaiDiem == 0)
            {
                lLoaiDiems = loaiDiemDA.GetListLoaiDiem();
            }
            else
            {
                DanhMuc_LoaiDiem loaiDiem = GetLoaiDiem(maLoaiDiem);
                lLoaiDiems.Add(loaiDiem);
            }            
            return lLoaiDiems;
        }

        public List<DanhMuc_LoaiDiem> GetListLoaiDiem(string tenLoaiDiem, 
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            if (String.Compare(tenLoaiDiem, "tất cả", true) == 0 || tenLoaiDiem == "")
            {
                return loaiDiemDA.GetListLoaiDiem(pageCurrentIndex, pageSize, out totalRecords);
            }
            else
            {
                return loaiDiemDA.GetListLoaiDiem(tenLoaiDiem, pageCurrentIndex, pageSize, out totalRecords);
            }
        }
        // ----------------------

        public bool LoaiDiemExists(int maLoaiDiem, string tenLoaiDiem)
        {
            return loaiDiemDA.LoaiDiemExists(maLoaiDiem, tenLoaiDiem);
        }

        public bool CanDeleteLoaiDiem(int maLoaiDiem)
        {
            return loaiDiemDA.CanDeleteLoaiDiem(maLoaiDiem);
        }

        public bool CalAvgLoaiDiemExists()
        {
            List<DanhMuc_LoaiDiem> lLoaiDiems = GetListLoaiDiem();
            foreach (DanhMuc_LoaiDiem loaiDiem in lLoaiDiems)
            {
                if (loaiDiem.TinhDTB)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CalAvgLoaiDiemExists(int maLoaiDiem)
        {
            List<DanhMuc_LoaiDiem> lLoaiDiems = GetListLoaiDiem();
            foreach (DanhMuc_LoaiDiem loaiDiem in lLoaiDiems)
            {
                if (loaiDiem.MaLoaiDiem != maLoaiDiem && loaiDiem.TinhDTB)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
