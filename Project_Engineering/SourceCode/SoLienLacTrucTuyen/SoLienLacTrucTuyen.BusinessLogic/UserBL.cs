using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class UserBL
    {
        private UserDA userDA;

        public UserBL()
        {
            userDA = new UserDA();
        }

        public Guid GetRoleId(string userName)
        {
            return userDA.GetRoleId(userName);
        }

        public Guid GetApplicationId(string userName)
        {
            return userDA.GetApplicationId(userName);
        }

        public bool ValidateUser(string userName)
        {
            return userDA.ValidateUser(userName);
        }

        public List<TabularUser> GetListTabularUsers(Guid roleId, string userName, 
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            if (roleId == new Guid())
            {
                if ((userName == "") || (string.Compare(userName, "Tất cả", true) == 0))
                {
                    return userDA.GetListTbUsers(
                        pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    return userDA.GetListTbUsers(userName,
                        pageCurrentIndex, pageSize, out totalRecords);
                }
            }
            else
            {
                if ((userName == "") || (string.Compare(userName, "Tất cả", true) == 0))
                {
                    return userDA.GetListTbUsers(roleId,
                        pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    return userDA.GetListUsers(roleId, userName,
                        pageCurrentIndex, pageSize, out totalRecords);
                }
            }            
        }

        public bool CanDeleteNguoiDung(Guid maNguoiDung)
        {
            return userDA.CanDeleteNguoiDung(maNguoiDung);
        }

        public bool UserInRoleParents(string userName)
        {
            return userDA.UserInRoleParents(userName);
        }
    }
}
