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

        public void ActivateUsers(aspnet_User user)
        {
            userDA.ChangeUserActivation(user, true);
        }

        public void ActivateUsers(List<aspnet_User> users)
        {
            if (users.Count != 0)
            {
                userDA.ChangeUserActivation(users, true);
            }
        }

        public void DeactivateUsers(List<aspnet_User> users)
        {
            if (users.Count != 0)
            {
                userDA.ChangeUserActivation(users, false);
            }
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
                tabularUser = ConvertToTabularUser(user);
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
            List<aspnet_Role> roles = userDA.GetRoles(userName);
            List<aspnet_Role> returnedRoles = new List<aspnet_Role>();

            RoleBL roleBL = new RoleBL(school);
            aspnet_Role roleFormerTeacher = roleBL.GetRoleFormerTeacher();
            aspnet_Role roleSubjectTeacher = null;
            bool bFormerOrSubject = false;

            if (roleFormerTeacher != null)
            {
                roleSubjectTeacher = roleBL.GetRoleSubjectTeacher();
                foreach(aspnet_Role role in roles)
                {
                    if (role.RoleId == roleFormerTeacher.RoleId)
                    {
                        returnedRoles.Add(roleFormerTeacher);
                        bFormerOrSubject = true;
                    }

                    if (role.RoleId == roleSubjectTeacher.RoleId)
                    {
                        returnedRoles.Add(roleSubjectTeacher);
                        bFormerOrSubject = true;
                    }
                }
            }

            if (bFormerOrSubject)
            {
                return returnedRoles;
            }
            else
            {
                return roles;
            }            
        }

        public bool ValidateUser(string userName)
        {
            aspnet_User user = GetUser(userName);
            return (bool)user.aspnet_Membership.IsActivated;
        }

        public bool IsDeletable(aspnet_User user)
        {
            return userDA.IsDeletable(user);
        }

        public void UpdateMembership(aspnet_User user, bool isTeacher, string realName, string email)
        {
            bool activated = false;
            bool deletable = true;

            if (!CheckUntils.IsNullOrBlank(email))
            {
                activated = true;
            }

            userDA.UpdateMembership(user, isTeacher, realName, email, activated, deletable);
        }

        public void UpdateMembership(aspnet_User user, bool isTeacher, string realName, string email, bool activated, bool deletable)
        {
            if (!user.UserName.Contains('_'))
            {
                user.UserName = school.SchoolId + "_" + user.UserName;
            }
            userDA.UpdateMembership(user, isTeacher, realName, email, activated, deletable);
        }

        private TabularUser ConvertToTabularUser(aspnet_User user)
        {
            RoleDA roleDA = new RoleDA(school);
            TabularUser tabularUser = new TabularUser();
            List<aspnet_Role> roles = null;
            string strRoleName = "";
            List<string> strRoleNames = new List<string>();
            if (user != null)
            {
                tabularUser.UserId = user.UserId;
                tabularUser.FullName = user.aspnet_Membership.FullName;
                tabularUser.ActualUserName = user.UserName;
                tabularUser.UserName = user.UserName.Split('_')[1];
                tabularUser.Email = user.aspnet_Membership.Email;
                tabularUser.Activated = (bool)user.aspnet_Membership.IsActivated;
                if (user.aspnet_Membership.NotYetActivated != null)
                {
                    tabularUser.NotYetActivated = true;
                }
                else
                {
                    tabularUser.NotYetActivated = false;
                }
                if (user.aspnet_Membership.IsActivated == null)
                {
                    tabularUser.Activated = false;
                }
                else if ((bool)user.aspnet_Membership.IsActivated)
                {
                    tabularUser.Activated = true;
                }
                else
                {
                    tabularUser.Activated = false;
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

        public void CreateUserMasterAdministrator(aspnet_User masterAdministrator)
        {
            AuthorizationBL authorizationBL = new AuthorizationBL(school);
            RoleBL roleBL = new RoleBL(school);
            UserBL userBL = new UserBL(school);

            // add administrator user to administrator role
            authorizationBL.AddUserToRole(masterAdministrator.UserName, roleBL.GetRoleAdministrator());

            // update administrator's membership information
            userBL.UpdateMembership(masterAdministrator, false, masterAdministrator.UserName.Split('_')[1], masterAdministrator.aspnet_Membership.Email, true, false);
        }

        public void CreateUserAdministrator(aspnet_User administrator)
        {
            AuthorizationBL authorizationBL = new AuthorizationBL(school);
            RoleBL roleBL = new RoleBL(school);
            UserBL userBL = new UserBL(school);

            // add administrator user to administrator role
            authorizationBL.AddUserToRole(administrator.UserName, roleBL.GetRoleAdministrator());

            // update administrator's membership information
            userBL.UpdateMembership(administrator, false, administrator.UserName.Split('_')[1], administrator.aspnet_Membership.Email);
        }

        /// <summary>
        /// Create new user of role Parents
        /// </summary>
        /// <param name="user">User will be created</param>
        public void CreateUserParents(aspnet_User user, string email)
        {
            // Update membership information
            UpdateMembership(user, false, STRING_UNDEFINED, email);

            AuthorizationBL authorizationBL = new AuthorizationBL(school);
            RoleBL roleBL = new RoleBL(school);

            // Add user to role "Parents"
            authorizationBL.AddUserToRole(user.UserName, roleBL.GetRoleParents());

            // Get all role "Parents" 's authorizations and add them to new user
            List<UserManagement_Authorization> authorizations = authorizationBL.GetSupliedRoleParentsAuthorizations();
            authorizationBL.AddParentsUserRegisteredServices(user, authorizations);
        }

        internal void DeleteUser(School_School school)
        {
            userDA.DeleteUser(school);
        }

        public bool DuplicateTeacherEmailExist(string email)
        {
            RoleBL roleBL = new RoleBL(school);
            aspnet_Role roleSubjectTeacher = roleBL.GetRoleSubjectTeacher();
            return userDA.DuplicateTeacherEmailExist(email);
        }
    }
}
