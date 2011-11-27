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

        public List<TabularUser> GetTabularUsers(aspnet_Role role, string userName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<aspnet_User> users = new List<aspnet_User>();
            List<TabularUser> tabularUsers = new List<TabularUser>();
            TabularUser tabularUser = null;            

            if (role == null)
            {
                if ((userName == "") || (string.Compare(userName, "Tất cả", true) == 0))
                {   
                    users = userDA.GetUsers(pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    userName = GetActualName(userName);
                    users = userDA.GetUsers(userName, pageCurrentIndex, pageSize, out totalRecords);
                }
            }
            else
            {
                if ((userName == "") || (string.Compare(userName, "Tất cả", true) == 0))
                {   
                    users = userDA.GetUsers(role, pageCurrentIndex, pageSize, out totalRecords);
                }
                else
                {
                    userName = GetActualName(userName);
                    users = userDA.GetUsers(role, userName, pageCurrentIndex, pageSize, out totalRecords);
                }
            }

            foreach (aspnet_User user in users)
            {
                tabularUser = ConvertToTabular(user);
                tabularUsers.Add(tabularUser);
            }

            return tabularUsers;
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

        public bool IsDeletable(aspnet_User user)
        {
            return userDA.IsDeletable(user);
        }

        public bool UserInRoleParents(string userName)
        {
            return userDA.UserInRolePARENTS(userName);
        }

        public void UpdateMembership(aspnet_User user, bool isTeacher)
        {
            userDA.UpdateMembership(user, isTeacher);
        }

        private TabularUser ConvertToTabular(aspnet_User user)
        {
            RoleDA roleDA = new RoleDA(school);
            TabularUser tabularUser = new TabularUser();
            List<aspnet_Role> roles = null;
            string strRoleName = "";
            List<string> strRoleNames = new List<string>();
            if (user != null)
            {
                tabularUser.UserId = user.UserId;
                tabularUser.ActualUserName = user.UserName;
                tabularUser.UserName = user.UserName.Split('_')[1];
                roles = userDA.GetRoles(user);
                foreach (aspnet_Role role in roles)
                {
                    if (role.UserManagement_RoleDetail.ParentRoleId != null)
                    {
                        strRoleName = roleDA.GetAncestorRole(role).UserManagement_RoleDetail.DisplayedName;
                    }
                    else
                    {
                        strRoleName = role.UserManagement_RoleDetail.DisplayedName;
                    }

                    if (!strRoleNames.Contains(strRoleName))
                    {
                        strRoleNames.Add(strRoleName);
                    }
                }

                foreach (string roleName in strRoleNames)
                {
                    tabularUser.RoleDisplayedName += roleName + ", ";
                }
                tabularUser.RoleDisplayedName = tabularUser.RoleDisplayedName.Trim().Trim(',');
            }

            return tabularUser;
        }
    }
}
