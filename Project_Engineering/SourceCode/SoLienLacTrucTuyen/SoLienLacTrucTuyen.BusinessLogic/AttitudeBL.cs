using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class AttitudeBL:BaseBL
    {
        private AttitudeDA attitudeDA;

        public AttitudeBL(School school)
            : base(school)
        {
            attitudeDA = new AttitudeDA(school);
        }

        public void InsertThaiDoThamGia(DanhMuc_ThaiDoThamGia newAttitude)
        {
            attitudeDA.InsertAttitude(newAttitude);
        }

        public void UpdateAttitude(DanhMuc_ThaiDoThamGia editedAttitude, string newAttitudeName)
        {
            editedAttitude.TenThaiDoThamGia = newAttitudeName;
            attitudeDA.UpdateAttitude(editedAttitude);
        }

        public void DeleteAttitude(DanhMuc_ThaiDoThamGia deletedAttitude)
        {
            attitudeDA.DeleteAttitude(deletedAttitude);
        }
        
        public DanhMuc_ThaiDoThamGia GetAttitude(int attitudeId)
        {
            return attitudeDA.GetAttitude(attitudeId);
        }

        public List<DanhMuc_ThaiDoThamGia> GetListAttitudes()
        {
            return attitudeDA.GetListAttitudes();
        }
      
        public List<DanhMuc_ThaiDoThamGia> GetListAttitudes(string attitudeName, int pageIndex, int pageSize, out double totalRecords)
        {
            if (String.Compare(attitudeName, "tất cả", true) == 0 || attitudeName == "")
            {
                return attitudeDA.GetListAttitudes(pageIndex, pageSize, out totalRecords);
            }
            else
            {
                return attitudeDA.GetListAttitudes(attitudeName, pageIndex, pageSize, out totalRecords);
            }            
        }

        public bool IsDeletable(DanhMuc_ThaiDoThamGia attitude)
        {
            return attitudeDA.IsDeletable(attitude);
        }        

        public bool AttitudeNameExists(string attitudeName)
        {
            return attitudeDA.AttitudeNameExists(attitudeName);
        }

        public bool AttitudeNameExists(string oldAttitudeName, string newAttitudeName)
        {
            if (oldAttitudeName == newAttitudeName)
            {
                return false;
            }
            else
            {
                return attitudeDA.AttitudeNameExists(newAttitudeName);
            }
        }
    }
}
