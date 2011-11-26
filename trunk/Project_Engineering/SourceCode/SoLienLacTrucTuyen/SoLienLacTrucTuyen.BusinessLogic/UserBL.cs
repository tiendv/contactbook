using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class UserBL : BaseBL
    {
        private UserDA userDA;

        public UserBL(School school)
            : base(school)
        {
            userDA = new UserDA(school);
        }

        public UserBL()
            : base()
        {
            userDA = new UserDA();
        }

        public Guid GetRoleId(string userName)
        {
            return userDA.GetRoleId(userName);
        }

        public List<aspnet_Role> GetRoles(string userName)
        {
            return userDA.GetRoles(userName);
        }

        public Guid GetApplicationId(string userName)
        {
            return userDA.GetApplicationId(userName);
        }

        public bool ValidateUser(string userName)
        {
            return userDA.ValidateUser(userName);
        }

        public List<TabularUser> GetListTabularUsers(Guid roleId, string userName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularUser> lTabularUsers = new List<TabularUser>();

            if (roleId == new Guid())
            {
                if ((userName == "") || (string.Compare(userName, "Tất cả", true) == 0))
                {
                    lTabularUsers = userDA.GetUsers(
                        pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    lTabularUsers = userDA.GetUsers(userName,
                        pageCurrentIndex, pageSize, out totalRecords);
                }
            }
            else
            {
                if ((userName == "") || (string.Compare(userName, "Tất cả", true) == 0))
                {
                    lTabularUsers = userDA.GetUsers(roleId,
                        pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    lTabularUsers = userDA.GetUsers(roleId, userName,
                        pageCurrentIndex, pageSize, out totalRecords);
                }
            }

            string strActualUserName;
            foreach (TabularUser tbUser in lTabularUsers)
            {
                strActualUserName = tbUser.UserName;
                tbUser.UserName = strActualUserName.Substring(strActualUserName.IndexOf('-') + 1);
            }

            return lTabularUsers;
        }

        public bool IsDeletable(aspnet_User user)
        {
            return userDA.IsDeletable(user);
        }

        public bool UserInRoleParents(string userName)
        {
            return userDA.UserInRolePARENTS(userName);
        }

        public void UpdateMembership(aspnet_User user)
        {
            userDA.UpdateMembership(user);
        }
    }
}
