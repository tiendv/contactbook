using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;
using System.Web.Security;

namespace SoLienLacTrucTuyen.DataAccess
{
    public enum FunctionFlag
    {
        ADMINONLY,
        PARENTSONLY,
        SUBJECTBASETEACHERS,
        MANAGEBASETEACHERS,
        ALLROLES,
        OTHERS,
    }

    public class RoleDA : BaseDA
    {
        public RoleDA()
            : base()
        {

        }

        #region Insert, Delete, Update
        public void DeleteRole(Guid roleId)
        {
            // Xoa UserManagement_Authorization lien quan
            IQueryable<UserManagement_Authorization> authorizations;
            authorizations = from authorization in db.UserManagement_Authorizations
                             where authorization.RoleId == roleId
                             select authorization;
            foreach (UserManagement_Authorization authorization in authorizations)
            {
                db.UserManagement_Authorizations.DeleteOnSubmit(authorization);
            }
            db.SubmitChanges();
            // --Xoa UserManagement_Authorization lien quan

            aspnet_Role deleteRole = (from role in db.aspnet_Roles
                                      where role.RoleId == roleId
                                      select role).First();
            db.aspnet_Roles.DeleteOnSubmit(deleteRole);
            db.SubmitChanges();
        }      

        public void CreateRoleDetail(string roleName, string description)
        {
            Guid roleId = (from role in db.aspnet_Roles
                           where role.RoleName == roleName
                           select role.RoleId).First();

            UserManagement_RoleDetail roleDetail = new UserManagement_RoleDetail
            {
                RoleId = roleId,
                Description = description,
                Expired = false,
                CanBeDeleted = true,
                Actived = true
            };

            db.UserManagement_RoleDetails.InsertOnSubmit(roleDetail);
            db.SubmitChanges();

            db.UserManagement_Authorizations.InsertOnSubmit(
                new UserManagement_Authorization()
                {
                    RoleId = roleId,
                    AuthorizedPagePathId = 1
                });
            db.SubmitChanges();
        }

        public void UpdateRoleDetail(string roleName, 
            string description, bool expired, bool canBeDeleted, bool actived)
        {
            UserManagement_RoleDetail roleDetail;
            roleDetail = (from roleDetails in db.UserManagement_RoleDetails
                         join role in db.aspnet_Roles on roleDetails.RoleId equals role.RoleId
                         where role.RoleName == roleName
                         select roleDetails).First();

            roleDetail.Expired = expired;
            roleDetail.CanBeDeleted = canBeDeleted;
            roleDetail.Actived = actived;

            db.SubmitChanges();
        }

        #endregion
        
        #region Checking methods
        public bool CanDeleteRole(string roleName)
        {
            bool bCanBeDel = (from role in db.aspnet_Roles
                              join roleDetail in db.UserManagement_RoleDetails
                                on role.RoleId equals roleDetail.RoleId
                              where role.RoleName == roleName
                              select roleDetail.CanBeDeleted).First();
            if (bCanBeDel)
            {
                IQueryable<aspnet_UsersInRole> usersInRoles;
                usersInRoles = from usersInRole in db.aspnet_UsersInRoles
                               join role in db.aspnet_Roles on usersInRole.RoleId equals role.RoleId
                               where role.RoleName == roleName
                               select usersInRole;
                if (usersInRoles.Count() != 0)
                {
                    bCanBeDel = false;
                }
            }

            return bCanBeDel;
        }

        public bool ExistNhomNguoiDung(Guid maNhomNguoiDung, string tenNhomNguoiDung)
        {
            IQueryable<aspnet_Role> roles = from role in db.aspnet_Roles
                                            where role.RoleId == maNhomNguoiDung && role.RoleName == tenNhomNguoiDung
                                            select role;
            if (roles.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CanBeExpired(Guid roleId)
        {
            bool bExpired = (from role in db.aspnet_Roles
                             join roleDetail in db.UserManagement_RoleDetails
                                on role.RoleId equals roleDetail.RoleId
                            where role.RoleId == roleId
                            select roleDetail.Expired).First();
            return bExpired;                 
        }
        #endregion

        #region Get (List)NhomNguoiDung
        public TabularRole GetTbRole(Guid roleId)
        {
            IQueryable<TabularRole> tbRoles = from role in db.aspnet_Roles
                                              join roleDetail in db.UserManagement_RoleDetails
                                                on role.RoleId equals roleDetail.RoleId
                                              where role.RoleId == roleId
                                                && roleDetail.ParentRoleId == null
                                              select new TabularRole
                                              {
                                                  RoleId = role.RoleId,
                                                  RoleName = role.RoleName,
                                                  Description = roleDetail.Description,
                                                  Expired = roleDetail.Expired,
                                                  CanBeDeleted = roleDetail.CanBeDeleted,
                                                  Actived = roleDetail.Actived
                                              };
            if (tbRoles.Count() != 0)
            {
                return tbRoles.First();
            }
            else
            {
                return null;
            }
        }

        public List<TabularRole> GetListTbRoles(
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularRole> lstTbRoles = new List<TabularRole>();

            IQueryable<TabularRole> tbRoles = from role in db.aspnet_Roles
                                              join roleDetail in db.UserManagement_RoleDetails
                                                on role.RoleId equals roleDetail.RoleId
                                              where roleDetail.ParentRoleId == null
                                              select new TabularRole
                                              {
                                                  RoleId = role.RoleId,
                                                  RoleName = role.RoleName,
                                                  Description = roleDetail.Description,
                                                  Expired = roleDetail.Expired,
                                                  CanBeDeleted = roleDetail.CanBeDeleted,
                                                  Actived = roleDetail.Actived
                                              };
            totalRecords = tbRoles.Count();
            if (totalRecords != 0)
            {
                lstTbRoles = tbRoles.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize)
                    .OrderBy(role => role.RoleName).ToList();
            }

            return lstTbRoles;
        }

        public List<TabularRole> GetListTbRoles(string roleName,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularRole> lstTbRoles = new List<TabularRole>();

            IQueryable<TabularRole> tbRoles = from role in db.aspnet_Roles
                                              join roleDetail in db.UserManagement_RoleDetails
                                                on role.RoleId equals roleDetail.RoleId
                                              where role.RoleName == roleName
                                                && roleDetail.ParentRoleId == null
                                              select new TabularRole
                                              {
                                                  RoleId = role.RoleId,
                                                  RoleName = role.RoleName,
                                                  Description = roleDetail.Description,
                                                  Expired = roleDetail.Expired,
                                                  CanBeDeleted = roleDetail.CanBeDeleted,
                                                  Actived = roleDetail.Actived
                                              };
            totalRecords = tbRoles.Count();
            if (totalRecords != 0)
            {
                lstTbRoles = tbRoles.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize)
                    .OrderBy(role => role.RoleName).ToList();
            }

            return lstTbRoles;
        }
        
        #endregion

        public List<AccessibilityEnum> GetAccessibilities(Guid roleId, string pageUrl)
        {
            IQueryable<int> pageUrlBasedFunctionIds;
            pageUrlBasedFunctionIds = from pagePath in db.UserManagement_PagePaths
                                      join authorizedPage in db.UserManagement_AuthorizedPages 
                                        on pagePath.PagePathId equals authorizedPage.PagePathId
                                      where pagePath.PhysicalPath == pageUrl
                                      select authorizedPage.FunctionId;

            List<AccessibilityEnum> lstAccessibilities = new List<AccessibilityEnum>();
            foreach (int pageUrlBasedFunctionId in pageUrlBasedFunctionIds)
            {
                IQueryable<int> accessibilities;
                accessibilities = from authorizedPage in db.UserManagement_AuthorizedPages
                                  join authorization in db.UserManagement_Authorizations
                                    on authorizedPage.AuthorizedPageId equals authorization.AuthorizedPagePathId
                                  where authorization.RoleId == roleId && authorizedPage.FunctionId == pageUrlBasedFunctionId
                                  select authorizedPage.AccessibilityId;

                if (accessibilities.Count() != 0)
                {
                    List<int> lst = accessibilities.Distinct().ToList();
                    foreach (int accessibility in lst)
                    {
                        lstAccessibilities.Add((AccessibilityEnum)accessibility);
                    }
                }
            }

            lstAccessibilities = lstAccessibilities.Distinct().ToList();
            return lstAccessibilities;
        }

        public List<aspnet_Role> GetListRoles(bool parentRoleOnly)
        {
            List<aspnet_Role> lstRoles = new List<aspnet_Role>();

            IQueryable<aspnet_Role> roles;
            if (parentRoleOnly)
            {
                roles = from role in db.aspnet_Roles
                        join roleDetail in db.UserManagement_RoleDetails
                            on role.RoleId equals roleDetail.RoleId
                        where roleDetail.ParentRoleId == null
                        select role;
            }
            else
            {
                roles = from role in db.aspnet_Roles
                        join roleDetail in db.UserManagement_RoleDetails
                            on role.RoleId equals roleDetail.RoleId
                        select role;
            }

            if (roles.Count() != 0)
            {
                lstRoles = roles.OrderBy(role => role.RoleName).ToList();
            }

            return lstRoles;
        }

        public Guid GetRoleAdmin()
        {
            Guid adminRoleId = (from param in db.System_Parameters
                                select param.AdminRoleId).First();
            return adminRoleId;
        }

        public bool ValidateAuthorization(Guid role, string pageUrl)
        {            
            IQueryable<int> authorizationIds =  from authorizedPage in db.UserManagement_AuthorizedPages
                                                join pagePath in db.UserManagement_PagePaths 
                                                    on authorizedPage.PagePathId equals pagePath.PagePathId
                                                join authorization in db.UserManagement_Authorizations
                                                    on authorizedPage.AuthorizedPageId equals authorization.AuthorizedPagePathId
                                                where pagePath.PhysicalPath == pageUrl
                                                    && authorization.RoleId == role
                                                select authorization.AuthorizationId;
            if (authorizationIds.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DeleteRole(string roleName)
        {
            aspnet_Role deletingRole = (from role in db.aspnet_Roles
                                       where role.RoleName == roleName
                                       select role).First();

            UserManagement_RoleDetail roleDetail;
            roleDetail = (from roleDetails in db.UserManagement_RoleDetails                          
                          where roleDetails.RoleId == deletingRole.RoleId
                          select roleDetails).First();
            db.UserManagement_RoleDetails.DeleteOnSubmit(roleDetail);
            db.SubmitChanges();

            IQueryable<UserManagement_Authorization> authorizations;
            authorizations = from authorization in db.UserManagement_Authorizations
                             where authorization.RoleId == deletingRole.RoleId
                             select authorization;
            foreach (UserManagement_Authorization authorization in authorizations)
            {
                db.UserManagement_Authorizations.DeleteOnSubmit(authorization);
            }
            db.SubmitChanges();

            List<Guid> deleteUserIds = new List<Guid>();
            IQueryable<aspnet_User> users;
            users = from usersInRole in db.aspnet_UsersInRoles
                    join user in db.aspnet_Users 
                        on usersInRole.UserId equals user.UserId
                    where usersInRole.RoleId == deletingRole.RoleId
                    select user;
            foreach (aspnet_User user in users)
            {
                Membership.DeleteUser(user.UserName, true);
            }

            db.aspnet_Roles.DeleteOnSubmit(deletingRole);
            db.SubmitChanges();

        }

        public bool RoleExists(string roleName)
        {
            IQueryable<aspnet_Role> roles = from role in db.aspnet_Roles
                                            where role.RoleName == roleName
                                            select role;
            if (roles.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateRole(string roleName, string newRoleName, string description)
        {
            aspnet_Role updateRole = (from role in db.aspnet_Roles
                                      where role.RoleName == roleName
                                      select role).First();
            updateRole.RoleName = newRoleName;
            Guid roleId = updateRole.RoleId;
            db.SubmitChanges();

            UserManagement_RoleDetail roleDetail;
            roleDetail = (from roleDt in db.UserManagement_RoleDetails
                          where roleDt.RoleId == roleId
                          select roleDt).First();
            roleDetail.Description = description;
            db.SubmitChanges(); 
        }

        public void AddUserToRole(string userName, string roleName)
        {
            Guid userId = (from user in db.aspnet_Users
                          where user.UserName == userName
                          select user.UserId).First();
            
            Guid roleId = (from role in db.aspnet_Roles
                           where role.RoleName == roleName
                           select role.RoleId).First();

            aspnet_UsersInRole usersInRole = new aspnet_UsersInRole
            {
                RoleId = roleId,
                UserId = userId
            };

            db.aspnet_UsersInRoles.InsertOnSubmit(usersInRole);
            db.SubmitChanges();
        }

        public bool IsRoleParents(string roleName)
        {
            Guid roleParentsId = (from param in db.System_Parameters
                                 select param.ParentsRoleId).First();

            string roleParentsName = (from role in db.aspnet_Roles
                                     where role.RoleId == roleParentsId
                                     select role.RoleName).First();
            if (roleParentsName == roleName)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsRelatedRoleParents(Guid roleId)
        {
            Guid roleParentsId = (from param in db.System_Parameters
                                  select param.ParentsRoleId).First();
            if (roleId == roleParentsId)
            {
                return true;
            }
            else
            {
                IQueryable<aspnet_Role> relatedsRoleParents = from role in db.aspnet_Roles
                                                              join roleDetail in db.UserManagement_RoleDetails
                                                                on role.RoleId equals roleDetail.RoleId
                                                              where (role.RoleId == roleId)
                                                                && (roleDetail.ParentRoleId == roleParentsId)
                                                              select role;
                if (relatedsRoleParents.Count() != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<UserManagement_Function> GetListRoleParentsBasedFunctions()
        {
            List<UserManagement_Function> lstRoleBasedFunctions = new List<UserManagement_Function>();            
            
            IQueryable<UserManagement_Function> functions;
            functions = from function in db.UserManagement_Functions
                        where function.FunctionFlag == FunctionFlag.GetName(typeof(FunctionFlag), FunctionFlag.PARENTSONLY)
                        select function;
            if(functions.Count() != 0)
            {
                lstRoleBasedFunctions = functions.ToList();
            }

            return lstRoleBasedFunctions;
        }

        public string GetChildRoleParentsByFunctions(List<int> lstFunctions)
        {
            lstFunctions.Sort();
            Guid roleParentsId = (from param in db.System_Parameters
                                 select param.ParentsRoleId).First();

            IQueryable<aspnet_Role> childRoleParents = from role in db.aspnet_Roles
                                                       join roleDetail in db.UserManagement_RoleDetails on role.RoleId equals roleDetail.RoleId
                                                       where roleDetail.ParentRoleId == roleParentsId
                                                       select role;

            foreach (aspnet_Role child in childRoleParents)
            {
                IQueryable<int> roleBasedFunctions = from authorizedPage in db.UserManagement_AuthorizedPages
                                                                join authorization in db.UserManagement_Authorizations 
                                                                    on authorizedPage.AuthorizedPageId equals authorization.AuthorizedPagePathId
                                                                where authorization.RoleId == child.RoleId
                                                                select authorizedPage.FunctionId;
                if (roleBasedFunctions.Count() != 0)
                {
                    List<int> lstRoleBasedFunctions = roleBasedFunctions.Distinct().ToList();
                    lstRoleBasedFunctions.Sort();

                    if (lstRoleBasedFunctions.Count == lstFunctions.Count)
                    {
                        int i;
                        for (i = 0; i < lstRoleBasedFunctions.Count; i++)
                        {
                            if(!lstFunctions.Contains(lstRoleBasedFunctions[i]))
                            {
                                break;
                            }
                        }
                        if (i == lstRoleBasedFunctions.Count)
                        {
                            return child.RoleName;
                        }
                    }
                }
            }

            return "";                         
        }

        public bool IsRoleGiaoVienChuNhiem(string roleName)
        {
            Guid roleGVCNId = (from param in db.System_Parameters
                              select param.RoleGVCNId).First();
            string roleGVCNName = (from role in db.aspnet_Roles
                                   where role.RoleId == roleGVCNId
                                   select role.RoleName).First();

            if (roleName == roleGVCNName)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsRoleGiaoVienBoMon(string roleName)
        {
            Guid roleGVBMId = (from param in db.System_Parameters
                               select param.RoleGVBMId).First();

            string roleGVBMName = (from role in db.aspnet_Roles
                                   where role.RoleId == roleGVBMId
                                   select role.RoleName).First();

            if (roleName == roleGVBMName)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RoleExists(string roleName, string newRoleName)
        {
            IQueryable<aspnet_Role> roles = from role in db.aspnet_Roles
                                            where role.RoleName != roleName
                                                && role.RoleName == newRoleName
                                            select role;
            if (roles.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
