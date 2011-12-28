using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class UserBL : BaseBL
    {
        private UserDA userDA;

        public UserBL(School_School school)
            : base(school)
        {
            userDA = new UserDA(school);
        }

        public UserBL()
            : base()
        {
            userDA = new UserDA();
        }

        public aspnet_User GetUser(string userName)
        {
            return userDA.GetUser(userName);
        }

        public aspnet_User GetUser(Guid userId)
        {
            return userDA.GetUser(userId);
        }

        public List<TabularUser> GetTabularUsers(aspnet_Role role, string userName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<aspnet_User> users = new List<aspnet_User>();
            List<TabularUser> tabularUsers = new List<TabularUser>();
            TabularUser tabularUser = null;

            if (role == null)
            {
                if (CheckUntils.IsAllOrBlank(userName))
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
                if (CheckUntils.IsAllOrBlank(userName))
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

        public aspnet_Role GetRole(string userName)
        {
            return userDA.GetRole(userName);
        }

        public List<aspnet_Role> GetRoles(string userName)
        {
            return userDA.GetRoles(userName);
        }

        public Guid GetApplicationId(string userName)
        {
            return userDA.GetApplicationId(userName);
        }

        public bool ValidateUser(string newUserName, aspnet_Role selectedRole)
        {
            AuthorizationBL authorizationBL = new AuthorizationBL(school);
            StudentBL studentBL = null;
            if (authorizationBL.IsRoleParents(selectedRole))
            {
                studentBL = new StudentBL(school);
                if (!studentBL.StudentCodeExists(newUserName))
                {
                    return false;
                }
            }

            bool bUserNameExists = UserNameExists(newUserName);

            return (!bUserNameExists);
        }

        private bool UserNameExists(string newUserName)
        {
            return true;
        }

        public bool ValidateUser(string userName)
        {
            aspnet_User user = GetUser(userName);
            if (user.aspnet_Membership.Email == null || user.aspnet_Membership.Email == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool IsDeletable(aspnet_User user)
        {
            return userDA.IsDeletable(user);
        }

        public void UpdateMembership(aspnet_User user, bool isTeacher, string realName, string email)
        {
            userDA.UpdateMembership(user, isTeacher, realName, email);
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
                tabularUser.Email = user.aspnet_Membership.Email;
                if (user.aspnet_Membership.Email != null)
                {
                    tabularUser.Actived = true;
                    tabularUser.StringStatus = "Được kích hoạt";
                }                
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

        public void CreateUserParents(aspnet_User user)
        {
            UpdateMembership(user, false, "(Chưa xác định)", null);
            AuthorizationBL authorizationBL = new AuthorizationBL(school);
            RoleBL roleBL = new RoleBL(school);
            
            authorizationBL.AddUserToRole(user.UserName, roleBL.GetRoleParents());

            List<UserManagement_Authorization> authorizations = authorizationBL.GetSupliedRoleParentsAuthorizations();
            authorizationBL.AddParentsUserRegisteredServices(user, authorizations);
        }
    }
}
