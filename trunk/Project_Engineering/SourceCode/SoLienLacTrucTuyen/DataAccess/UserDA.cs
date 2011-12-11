using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class UserDA : BaseDA
    {
        public UserDA(School_School school)
            : base(school)
        {
        }

        public UserDA()
            : base()
        {
        }

        public aspnet_User GetUser(string userName)
        {
            IQueryable<aspnet_User> iqUser;
            iqUser = from user in db.aspnet_Users
                     where user.UserName == userName
                     select user;
            if (iqUser.Count() != 0)
            {
                return iqUser.First();
            }
            else
            {
                return null;
            }
        }

        public aspnet_Role GetRole(string userName)
        {
            aspnet_Role role = null;
            IQueryable<aspnet_Role> iqRole = from usersInRole in db.aspnet_UsersInRoles
                                             where usersInRole.aspnet_User.UserName == userName
                                             select usersInRole.aspnet_Role;
            if (iqRole.Count() != 0)
            {
                role = iqRole.First();
            }
            return role;
        }

        public Guid GetApplicationId(string userName)
        {
            IQueryable<Guid> appIds = from user in db.aspnet_Users
                                      where user.UserName == userName
                                      select user.ApplicationId;
            return appIds.First();
        }

        public bool ValidateUser(string userName)
        {
            return true;
        }

        public List<aspnet_User> GetUsers(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<aspnet_User> iqUser = from user in db.aspnet_Users
                                             where user.aspnet_Membership.IsDeletable == true
                                             && user.aspnet_Membership.SchoolId == school.SchoolId
                                             select user;

            return GetUsers(ref iqUser, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<aspnet_User> GetUsers(string userName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<aspnet_User> iqUser = from user in db.aspnet_Users
                                             where user.aspnet_Membership.IsDeletable == true
                                             && user.UserName == userName
                                             && user.aspnet_Membership.SchoolId == school.SchoolId
                                             select user;

            return GetUsers(ref iqUser, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<aspnet_User> GetUsers(aspnet_Role role, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<aspnet_User> iqUser = from user in db.aspnet_Users
                                             join usersInRole in db.aspnet_UsersInRoles on user.UserId equals usersInRole.UserId
                                             where user.aspnet_Membership.IsDeletable == true
                                             && (usersInRole.RoleId == role.RoleId || usersInRole.aspnet_Role.UserManagement_RoleDetail.ParentRoleId == role.RoleId)
                                             && user.aspnet_Membership.SchoolId == school.SchoolId
                                             select user;

            return GetUsers(ref iqUser, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<aspnet_User> GetUsers(aspnet_Role role, string userName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<aspnet_User> iqUser = from user in db.aspnet_Users
                                             join usersInRole in db.aspnet_UsersInRoles on user.UserId equals usersInRole.UserId
                                             where user.aspnet_Membership.IsDeletable == true
                                             && (usersInRole.RoleId == role.RoleId || usersInRole.aspnet_Role.UserManagement_RoleDetail.ParentRoleId == role.RoleId)
                                             && user.UserName == userName
                                             && user.aspnet_Membership.SchoolId == school.SchoolId
                                             select user;

            return GetUsers(ref iqUser, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<aspnet_Role> GetRoles(string userName)
        {
            IQueryable<aspnet_Role> iqRole = from usersInRole in db.aspnet_UsersInRoles
                                             join user in db.aspnet_Users on usersInRole.UserId equals user.UserId
                                             join role in db.aspnet_Roles on usersInRole.RoleId equals role.RoleId
                                             where user.UserName == userName
                                             select role;
            return iqRole.ToList();
        }

        public bool IsDeletable(aspnet_User user)
        {
            bool bCanDelete = (from membership in db.aspnet_Memberships
                               where membership.UserId == user.UserId
                               select membership.IsDeletable).First();
            return bCanDelete;
        }

        //public bool UserInRolePARENTS(string userName)
        //{
        //    aspnet_Role roleParent = from rl in db.aspnet_Roles
        //                             where 
        //    Guid roleParentsId = (from param in db.System_Parameters
        //                          select param.ParentsRoleId).First();

        //    IQueryable<Guid> childRoleParentsIds;
        //    childRoleParentsIds = from role in db.aspnet_Roles
        //                          join roleDetail in db.UserManagement_RoleDetails on role.RoleId equals roleDetail.RoleId
        //                          where roleDetail.ParentRoleId == roleParentsId
        //                          select role.RoleId;
        //    foreach (Guid childRoleParentsId in childRoleParentsIds)
        //    {
        //        IQueryable<aspnet_UsersInRole> usersInRoles;
        //        usersInRoles = from usersInRole in db.aspnet_UsersInRoles
        //                       join user in db.aspnet_Users
        //                            on usersInRole.UserId equals user.UserId
        //                       where usersInRole.RoleId == childRoleParentsId
        //                            && user.UserName == userName
        //                       select usersInRole;
        //        if (usersInRoles.Count() != 0)
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        public void UpdateMembership(aspnet_User user, bool isTeacher, string realName, string email)
        {
            IQueryable<aspnet_Membership> iqMembership = from mem in db.aspnet_Memberships
                                                         where mem.aspnet_User.UserName == user.UserName
                                                         select mem;
            if (iqMembership.Count() != 0)
            {
                aspnet_Membership membership = iqMembership.First();
                membership.SchoolId = school.SchoolId;
                membership.IsTeacher = isTeacher;
                membership.FullName = realName;
                membership.Email = email;
                db.SubmitChanges();
            }
        }

        public List<aspnet_Role> GetRoles(aspnet_User user)
        {
            List<aspnet_Role> roles = new List<aspnet_Role>();

            IQueryable<aspnet_Role> iqRole = from usersInRole in db.aspnet_UsersInRoles
                                             where usersInRole.UserId == user.UserId
                                             && usersInRole.aspnet_User.aspnet_Membership.SchoolId == school.SchoolId
                                             select usersInRole.aspnet_Role;
            if (iqRole.Count() != 0)
            {
                roles = iqRole.ToList();
            }

            return roles;
        }

        private List<aspnet_User> GetUsers(ref IQueryable<aspnet_User> iqUser, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<aspnet_User> users = new List<aspnet_User>();
            totalRecords = iqUser.Count();

            if (totalRecords != 0)
            {
                users = iqUser.Distinct().OrderBy(user => user.UserName).Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return users;
        }
    }
}
