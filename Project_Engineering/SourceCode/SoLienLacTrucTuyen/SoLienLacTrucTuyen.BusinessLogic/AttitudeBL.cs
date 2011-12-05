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

        public AttitudeBL(School_School school)
            : base(school)
        {
            attitudeDA = new AttitudeDA(school);
        }

        public void InsertThaiDoThamGia(Category_Attitude newAttitude)
        {
            attitudeDA.InsertAttitude(newAttitude);
        }

        public void UpdateAttitude(Category_Attitude editedAttitude, string newAttitudeName)
        {
            editedAttitude.AttitudeName = newAttitudeName;
            attitudeDA.UpdateAttitude(editedAttitude);
        }

        public void DeleteAttitude(Category_Attitude deletedAttitude)
        {
            attitudeDA.DeleteAttitude(deletedAttitude);
        }
        
        public Category_Attitude GetAttitude(int attitudeId)
        {
            return attitudeDA.GetAttitude(attitudeId);
        }

        public List<Category_Attitude> GetListAttitudes()
        {
            return attitudeDA.GetAttitudes();
        }
      
        public List<Category_Attitude> GetAttitudes(string attitudeName, int pageIndex, int pageSize, out double totalRecords)
        {
            if (String.Compare(attitudeName, "tất cả", true) == 0 || attitudeName == "")
            {
                return attitudeDA.GetAttitudes(pageIndex, pageSize, out totalRecords);
            }
            else
            {
                return attitudeDA.GetAttitudes(attitudeName, pageIndex, pageSize, out totalRecords);
            }            
        }

        public bool IsDeletable(Category_Attitude attitude)
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
