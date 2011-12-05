using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class LoiNhanKhanBL:BaseBL
    {
        private LoiNhanKhanDA loiNhanKhanDA;

        public LoiNhanKhanBL(School_School school)
            : base(school)
        {
            loiNhanKhanDA = new LoiNhanKhanDA(school);
        }

        public void InsertLoiNhanKhan(int maHocSinhLopHoc, string tieuDe, string noiDung, DateTime ngay)
        {
            loiNhanKhanDA.InsertLoiNhanKhan(maHocSinhLopHoc, tieuDe, noiDung, ngay);
        }

        public void UpdateLoiNhanKhan(int maLoiNhanKhan, string tieuDe, string noiDung, DateTime ngay)
        {
            loiNhanKhanDA.UpdateLoiNhanKhan(maLoiNhanKhan, tieuDe, noiDung, ngay);
        }

        public void UpdateLoiNhanKhan(int maLoiNhanKhan, string noiDung, DateTime ngay)
        {
            loiNhanKhanDA.UpdateLoiNhanKhan(maLoiNhanKhan, noiDung, ngay);
        }

        public void DeleteLoiNhanKhan(int maLopNhanKhan)
        {
            loiNhanKhanDA.DeleteLoiNhanKhan(maLopNhanKhan);
        }

        public MessageToParents_Message GetLoiNhanKhan(int maLoiNhanKhan)
        {
            return loiNhanKhanDA.GetLoiNhanKhan(maLoiNhanKhan);
        }

        public List<TabularLoiNhanKhan> GetListTabularLoiNhanKhan(int YearId, DateTime tuNgay, DateTime denNgay,
            string maHocSinhHienThi, int xacNhan, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            if (string.Compare(maHocSinhHienThi, "tất cả", true) == 0 || maHocSinhHienThi == "")
            {
                if (xacNhan == -1)
                {
                    return loiNhanKhanDA.GetListTabularLoiNhanKhan(YearId, tuNgay, denNgay,
                        pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    bool bXacNhan = (xacNhan == 0) ? false : true;
                    return loiNhanKhanDA.GetListTabularLoiNhanKhan(YearId, tuNgay, denNgay,
                        bXacNhan,
                        pageCurrentIndex, pageSize, out totalRecords);
                }
            }
            else
            {
                if (xacNhan == -1)
                {
                    return loiNhanKhanDA.GetListTabularLoiNhanKhan(YearId, tuNgay, denNgay,
                        maHocSinhHienThi,
                        pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    bool bXacNhan = (xacNhan == 0) ? false : true;
                    return loiNhanKhanDA.GetListTabularLoiNhanKhan(YearId, tuNgay, denNgay,
                        maHocSinhHienThi, bXacNhan,
                        pageCurrentIndex, pageSize, out totalRecords);
                }
            }
        }

        public void Confirm(int maLoiNhanKhan)
        {
            loiNhanKhanDA.UpdateLoiNhanKhan(maLoiNhanKhan, true);
        }
    }
}
