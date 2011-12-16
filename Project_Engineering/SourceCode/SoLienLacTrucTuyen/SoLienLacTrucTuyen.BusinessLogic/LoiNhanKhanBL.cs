using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class LoiNhanKhanBL : BaseBL
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

        public void ConfirmMessage(MessageToParents_Message message)
        {
            loiNhanKhanDA.UpdateMessage(message, true);
        }

        public void UnconfirmMessage(MessageToParents_Message message)
        {
            loiNhanKhanDA.UpdateMessage(message, false);
        }

        public void DeleteLoiNhanKhan(int maLopNhanKhan)
        {
            loiNhanKhanDA.DeleteLoiNhanKhan(maLopNhanKhan);
        }

        public MessageToParents_Message GetLoiNhanKhan(int maLoiNhanKhan)
        {
            return loiNhanKhanDA.GetLoiNhanKhan(maLoiNhanKhan);
        }

        public List<TabularMessage> GetListTabularLoiNhanKhan(int YearId, DateTime tuNgay, DateTime denNgay,
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

        public List<MessageToParents_Message> GetMessages(Configuration_Year year, DateTime tuNgay, DateTime denNgay,
            Student_Student student, bool? confirmed, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            if (confirmed == null)
            {
                return loiNhanKhanDA.GetMessages(year, tuNgay, denNgay,
                    student, pageCurrentIndex, pageSize, out totalRecords);
            }
            else
            {
                return loiNhanKhanDA.GetMessages(year, tuNgay, denNgay,
                    student, (bool)confirmed, pageCurrentIndex, pageSize, out totalRecords);
            }
        }

        public void Confirm(int maLoiNhanKhan)
        {
            loiNhanKhanDA.UpdateLoiNhanKhan(maLoiNhanKhan, true);
        }

        internal void DeleteLoiNhanKhan(Student_Student deletedStudent)
        {
            loiNhanKhanDA.DeleteLoiNhanKhan(deletedStudent);
        }
    }
}
