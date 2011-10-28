using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class UserDA : BaseDA
    {
        public UserDA()
            : base()
        {
        }

        public Guid GetRoleId(string userName)
        {
            IQueryable<Guid> roleIds = from usersInRole in db.aspnet_UsersInRoles
                                       join user in db.aspnet_Users on usersInRole.UserId equals user.UserId
                                       where user.UserName == userName
                                       select usersInRole.RoleId;
            return roleIds.First();
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

        public List<TabularUser> GetListUsers(Guid roleId, string userName, 
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<Guid> relatedRoleIds = new List<Guid>();
            relatedRoleIds.Add(roleId);

            IQueryable<Guid> childRoleIds = from role in db.aspnet_Roles
                                            join roleDetail in db.UserManagement_RoleDetails
                                                on role.RoleId equals roleDetail.RoleId
                                            where roleDetail.ParentRoleId == roleId
                                            select role.RoleId;

            if (childRoleIds.Count() != 0)
            {
                relatedRoleIds.AddRange(childRoleIds.ToList());
            }

            List<TabularUser> lstTbUsers = new List<TabularUser>();

            IQueryable<TabularUser> tbUsers = from user in db.aspnet_Users
                                              join usersInRole in db.aspnet_UsersInRoles
                                                on user.UserId equals usersInRole.UserId
                                              join role in db.aspnet_Roles
                                                on usersInRole.RoleId equals role.RoleId
                                              join membership in db.aspnet_Memberships
                                                on user.UserId equals membership.UserId
                                              where relatedRoleIds.Contains(usersInRole.RoleId) == true
                                                && membership.CanBeDeleted == true
                                                && user.UserName == userName
                                              select new TabularUser
                                              {
                                                  UserId = user.UserId,
                                                  UserName = user.UserName,
                                                  RoleName = role.RoleName
                                              };
            totalRecords = tbUsers.Count();
            if (totalRecords != 0)
            {
                lstTbUsers = tbUsers.OrderBy(user => user.RoleName)
                    .ThenBy(user => user.UserName).Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();

                foreach (TabularUser tbUser in lstTbUsers)
                {
                    IQueryable<Guid> parentRoleIds = from role in db.aspnet_Roles
                                                     join roleDetail in db.UserManagement_RoleDetails
                                                        on role.RoleId equals roleDetail.RoleId
                                                     where role.RoleName == tbUser.RoleName
                                                        && roleDetail.ParentRoleId != null
                                                     select (Guid)roleDetail.ParentRoleId;
                    if (parentRoleIds.Count() != 0)
                    {
                        string parentRoleName = (from role in db.aspnet_Roles
                                                 where role.RoleId == parentRoleIds.First()
                                                 select role.RoleName).First();
                        tbUser.RoleName = parentRoleName;
                    }
                }
            }

            return lstTbUsers;
        }

        public List<TabularUser> GetListTbUsers(Guid roleId, 
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<Guid> relatedRoleIds = new List<Guid>();
            relatedRoleIds.Add(roleId);

            IQueryable<Guid> childRoleIds = from role in db.aspnet_Roles
                                            join roleDetail in db.UserManagement_RoleDetails
                                                on role.RoleId equals roleDetail.RoleId
                                            where roleDetail.ParentRoleId == roleId
                                            select role.RoleId;
            
            if (childRoleIds.Count() != 0)
            {
                relatedRoleIds.AddRange(childRoleIds.ToList());
            }

            List<TabularUser> lstTbUsers = new List<TabularUser>();

            IQueryable<TabularUser> tbUsers = from user in db.aspnet_Users
                                              join usersInRole in db.aspnet_UsersInRoles
                                                on user.UserId equals usersInRole.UserId
                                              join role in db.aspnet_Roles
                                                on usersInRole.RoleId equals role.RoleId
                                              join membership in db.aspnet_Memberships
                                                on user.UserId equals membership.UserId
                                              where relatedRoleIds.Contains(usersInRole.RoleId) == true
                                                && membership.CanBeDeleted == true
                                              select new TabularUser
                                              {
                                                  UserId = user.UserId,
                                                  UserName = user.UserName,
                                                  RoleName = role.RoleName
                                              };
            totalRecords = tbUsers.Count();
            if (totalRecords != 0)
            {
                lstTbUsers = tbUsers.OrderBy(user => user.RoleName)
                    .ThenBy(user => user.UserName).Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();

                foreach (TabularUser tbUser in lstTbUsers)
                {
                    IQueryable<Guid> parentRoleIds = from role in db.aspnet_Roles
                                                     join roleDetail in db.UserManagement_RoleDetails
                                                        on role.RoleId equals roleDetail.RoleId
                                                     where role.RoleName == tbUser.RoleName
                                                        && roleDetail.ParentRoleId != null
                                                     select (Guid)roleDetail.ParentRoleId;
                    if (parentRoleIds.Count() != 0)
                    {
                        string parentRoleName = (from role in db.aspnet_Roles
                                                 where role.RoleId == parentRoleIds.First()
                                                 select role.RoleName).First();
                        tbUser.RoleName = parentRoleName;
                    }
                }
            }

            return lstTbUsers;
        }        

        public List<TabularUser> GetListTbUsers(
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularUser> lstTbUsers = new List<TabularUser>();

            IQueryable<TabularUser> tbUsers = from user in db.aspnet_Users
                                              join usersInRole in db.aspnet_UsersInRoles
                                                on user.UserId equals usersInRole.UserId
                                              join role in db.aspnet_Roles
                                                on usersInRole.RoleId equals role.RoleId
                                              join membership in db.aspnet_Memberships
                                                on user.UserId equals membership.UserId
                                              where membership.CanBeDeleted == true
                                              select new TabularUser
                                              {
                                                  UserId = user.UserId,
                                                  UserName = user.UserName,
                                                  RoleName = role.RoleName
                                              };

            totalRecords = tbUsers.Count();
            if (totalRecords != 0)
            {
                lstTbUsers = tbUsers.OrderBy(user => user.RoleName)
                    .ThenBy(user => user.UserName).Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();

                foreach (TabularUser tbUser in lstTbUsers)
                {
                    IQueryable<Guid> parentRoleIds = from role in db.aspnet_Roles
                                                     join roleDetail in db.UserManagement_RoleDetails 
                                                        on role.RoleId equals roleDetail.RoleId
                                                     where role.RoleName == tbUser.RoleName
                                                        && roleDetail.ParentRoleId != null
                                                     select (Guid)roleDetail.ParentRoleId;
                    if (parentRoleIds.Count() != 0)
                    {
                        string parentRoleName = (from role in db.aspnet_Roles
                                                 where role.RoleId == parentRoleIds.First()
                                                 select role.RoleName).First();
                        tbUser.RoleName = parentRoleName;
                    }
                }
            }

            return lstTbUsers;
        }

        public List<TabularUser> GetListTbUsers(string userName, 
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularUser> lstTbUsers = new List<TabularUser>();

            IQueryable<TabularUser> tbUsers = from user in db.aspnet_Users
                                              join usersInRole in db.aspnet_UsersInRoles
                                                on user.UserId equals usersInRole.UserId
                                              join role in db.aspnet_Roles
                                                on usersInRole.RoleId equals role.RoleId
                                              join membership in db.aspnet_Memberships
                                                on user.UserId equals membership.UserId
                                              where user.UserName == userName 
                                                && membership.CanBeDeleted == true
                                              select new TabularUser
                                              {
                                                  UserId = user.UserId,
                                                  UserName = user.UserName,
                                                  RoleName = role.RoleName
                                              };

            totalRecords = tbUsers.Count();
            if (totalRecords != 0)
            {
                lstTbUsers = tbUsers.OrderBy(user => user.RoleName)
                    .ThenBy(user => user.UserName).Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();

                foreach (TabularUser tbUser in lstTbUsers)
                {
                    IQueryable<Guid> parentRoleIds = from role in db.aspnet_Roles
                                                     join roleDetail in db.UserManagement_RoleDetails
                                                        on role.RoleId equals roleDetail.RoleId
                                                     where role.RoleName == tbUser.RoleName
                                                        && roleDetail.ParentRoleId != null
                                                     select (Guid)roleDetail.ParentRoleId;
                    if (parentRoleIds.Count() != 0)
                    {
                        string parentRoleName = (from role in db.aspnet_Roles
                                                 where role.RoleId == parentRoleIds.First()
                                                 select role.RoleName).First();
                        tbUser.RoleName = parentRoleName;
                    }
                }
            }

            return lstTbUsers;
        }

        public bool CanDeleteNguoiDung(Guid maNguoiDung)
        {
            bool bCanDelete = (from user in db.aspnet_Users
                               join membership in db.aspnet_Memberships
                                    on user.UserId equals membership.UserId
                               where user.UserId == maNguoiDung
                               select membership.CanBeDeleted).First();
            return bCanDelete;
        }

        public bool UserInRoleParents(string userName)
        {
            Guid roleParentsId = (from param in db.System_Parameters
                                 select param.ParentsRoleId).First();

            IQueryable<Guid> childRoleParentsIds;
            childRoleParentsIds = from role in db.aspnet_Roles
                                 join roleDetail in db.UserManagement_RoleDetails on role.RoleId equals roleDetail.RoleId
                                 where roleDetail.ParentRoleId == roleParentsId
                                 select role.RoleId;
            foreach (Guid childRoleParentsId in childRoleParentsIds)
            {
                IQueryable<aspnet_UsersInRole> usersInRoles;
                usersInRoles = from usersInRole in db.aspnet_UsersInRoles
                               join user in db.aspnet_Users
                                    on usersInRole.UserId equals user.UserId
                               where usersInRole.RoleId == childRoleParentsId
                                    && user.UserName == userName
                               select usersInRole;
                if (usersInRoles.Count() != 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
