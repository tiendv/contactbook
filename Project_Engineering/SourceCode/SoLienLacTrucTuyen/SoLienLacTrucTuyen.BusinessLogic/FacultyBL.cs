using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class FacultyBL
    {
        FacultyDA nganhhocDA;

        public FacultyBL()
        {
            nganhhocDA = new FacultyDA();
        }

        public void InsertFaculty(Faculty faculty)
        {
            nganhhocDA.InsertFaculty(faculty);
        }

        public void UpdateNganhHoc(DanhMuc_NganhHoc nganhhoc)
        {
            nganhhocDA.UpdateFaculty(nganhhoc);
        }

        public void DeleteNganhHoc(int maNganhHoc)
        {
            nganhhocDA.DeleteFaculty(maNganhHoc);
        }

        public DanhMuc_NganhHoc GetNganhHoc(int maNganhHoc)
        {
            return nganhhocDA.GetNganhHoc(maNganhHoc);
        }

        public List<DanhMuc_NganhHoc> GetListNganhHoc()
        {
            return nganhhocDA.GetListNganhHoc();
        }

        public List<DanhMuc_NganhHoc> GetListNganhHoc(string tenNganhHoc, int pageIndex, int pageSize, out double totalRecords)
        {
            if (String.Compare(tenNganhHoc, "tất cả", true) == 0 || tenNganhHoc == "")
            {
                return nganhhocDA.GetListNganhHoc(pageIndex, pageSize, out totalRecords);
            }
            else
            {
                return nganhhocDA.GetListNganhHoc(tenNganhHoc, pageIndex, pageSize, out totalRecords);
            }
            
        }

        public bool NganhHocExists(int maNganhHoc, string tenNganhHoc)
        {
            return nganhhocDA.NganhHocExists(maNganhHoc, tenNganhHoc);
        }

        public bool NganhHocExists(string tenNganhHoc)
        {
            return nganhhocDA.NganhHocExists(tenNganhHoc);
        }

        public bool CheckCanDeleteNganhHoc(int maNganhHoc)
        {
            return nganhhocDA.CanDeleteNganhHoc(maNganhHoc);
        }
    }
}
