﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;
using System.Web.Security;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class RoleDA : BaseDA
    {
        public RoleDA(School school)
            : base(school)
        {

        }

        public void DeleteRole(string roleName)
        {
            aspnet_Role deletingRole = (from role in db.aspnet_Roles
                                        where role.RoleName == roleName
                                        select role).First();

            // delete RoleDetail of deleting role
            UserManagement_RoleDetail roleDetail;
            roleDetail = (from roleDetails in db.UserManagement_RoleDetails
                          where roleDetails.RoleId == deletingRole.RoleId
                          select roleDetails).First();
            db.UserManagement_RoleDetails.DeleteOnSubmit(roleDetail);
            db.SubmitChanges();

            // delete all Authorizations of deleting role
            IQueryable<UserManagement_Authorization> iqAuthorization;
            iqAuthorization = from authorization in db.UserManagement_Authorizations
                              where authorization.RoleId == deletingRole.RoleId
                              select authorization;
            foreach (UserManagement_Authorization authorization in iqAuthorization)
            {
                db.UserManagement_Authorizations.DeleteOnSubmit(authorization);
            }
            db.SubmitChanges();

            // delete all Users of deleting role
            IQueryable<aspnet_User> iqUser;
            iqUser = from usersInRole in db.aspnet_UsersInRoles
                     join user in db.aspnet_Users on usersInRole.UserId equals user.UserId
                     where usersInRole.RoleId == deletingRole.RoleId
                     select user;
            foreach (aspnet_User user in iqUser)
            {
                Membership.DeleteUser(user.UserName, true);
            }

            // delete role
            db.aspnet_Roles.DeleteOnSubmit(deletingRole);
            db.SubmitChanges();
        }

        public void UpdateRole(string oldRoleName, string newRoleName, string description)
        {
            aspnet_Role role = (from rl in db.aspnet_Roles
                                where rl.RoleName == oldRoleName
                                select rl).First();
            role.RoleName = newRoleName;
            Guid roleId = role.RoleId;
            db.SubmitChanges();

            role.UserManagement_RoleDetail.Description = description;
            role.UserManagement_RoleDetail.DisplayedName = newRoleName.Split('_')[1];
            db.SubmitChanges();
        }

        public void CreateRoleDetail(string roleName, string description)
        {
            Guid roleId = (from role in db.aspnet_Roles
                           where role.RoleName == roleName
                           select role.RoleId).First();

            // create new RoleDetail of Role
            UserManagement_RoleDetail roleDetail = new UserManagement_RoleDetail
            {
                RoleId = roleId,
                Description = description,
                Expired = false,
                CanBeDeleted = true,
                Actived = true,
                SchoolId = school.SchoolId,
                DisplayedName = roleName.Split('_')[1]
            };
            db.UserManagement_RoleDetails.InsertOnSubmit(roleDetail);
            db.SubmitChanges();

            // create default Authorization of Role
            UserManagement_Authorization authorization = new UserManagement_Authorization
            {
                RoleId = roleId,
                AuthorizedPagePathId = 1 // Home page
            };
            db.UserManagement_Authorizations.InsertOnSubmit(authorization);
            db.SubmitChanges();
        }

        public void UpdateRoleDetail(string roleName, string description, bool expired, bool canBeDeleted, bool actived)
        {
            // get RoleDetail
            UserManagement_RoleDetail roleDetail;
            roleDetail = (from roleDetails in db.UserManagement_RoleDetails
                          join role in db.aspnet_Roles on roleDetails.RoleId equals role.RoleId
                          where role.RoleName == roleName
                          select roleDetails).First();

            // change RoleDetail of properties
            roleDetail.Expired = expired;
            roleDetail.CanBeDeleted = canBeDeleted;
            roleDetail.Actived = actived;

            db.SubmitChanges();
        }

        public bool RoleExists(string roleName)
        {
            IQueryable<aspnet_Role> iqRole = from role in db.aspnet_Roles
                                             where role.RoleName == roleName
                                             && role.UserManagement_RoleDetail.SchoolId == school.SchoolId
                                             select role;
            if (iqRole.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsDeletableRole(string roleName)
        {
            aspnet_Role role = (from rl in db.aspnet_Roles
                                where rl.RoleName == roleName
                                select rl).First();

            if (role.UserManagement_RoleDetail.CanBeDeleted)
            {
                if (role.aspnet_UsersInRoles.Count == 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsExpirable(Guid roleId)
        {
            bool bExpired = (from role in db.aspnet_Roles
                             join roleDetail in db.UserManagement_RoleDetails
                                on role.RoleId equals roleDetail.RoleId
                             where role.RoleId == roleId
                             select roleDetail.Expired).First();
            return bExpired;
        }

        public aspnet_Role GetRole(string roleName)
        {
            aspnet_Role role = null;

            IQueryable<aspnet_Role> iqRole = from rl in db.aspnet_Roles
                                             where rl.RoleName == roleName
                                             && rl.UserManagement_RoleDetail.SchoolId == school.SchoolId
                                             select rl;
            if (iqRole.Count() != 0)
            {
                role = iqRole.First();
            }

            return role;
        }

        public aspnet_Role GetRole(Guid roleId)
        {
            aspnet_Role role = null;

            IQueryable<aspnet_Role> iqRole = from rl in db.aspnet_Roles
                                             where rl.RoleId == roleId
                                             && rl.UserManagement_RoleDetail.SchoolId == school.SchoolId
                                             select rl;
            if (iqRole.Count() != 0)
            {
                role = iqRole.First();
            }

            return role;
        }

        public aspnet_Role GetAncestorRole(aspnet_Role descendantRole)
        {
            aspnet_Role role = null;
            IQueryable<aspnet_Role> iqRole = from ancestorRole in db.aspnet_Roles
                                             where ancestorRole.RoleId == descendantRole.UserManagement_RoleDetail.ParentRoleId
                                             select ancestorRole;
            if (iqRole.Count() != 0)
            {
                role = iqRole.First();
            }

            return role;
        }

        public List<aspnet_Role> GetRoles(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<aspnet_Role> roles = new List<aspnet_Role>();

            IQueryable<aspnet_Role> iqRoles = from role in db.aspnet_Roles
                                              where role.UserManagement_RoleDetail.ParentRoleId == null
                                              && role.UserManagement_RoleDetail.SchoolId == school.SchoolId
                                              select role;
            totalRecords = iqRoles.Count();
            if (totalRecords != 0)
            {
                roles = iqRoles.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize)
                    .OrderBy(role => role.RoleName).ToList();
            }

            return roles;
        }

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
            List<aspnet_Role> roles = new List<aspnet_Role>();

            IQueryable<aspnet_Role> iqRole;
            if (parentRoleOnly)
            {
                iqRole = from role in db.aspnet_Roles
                         where role.UserManagement_RoleDetail.ParentRoleId == null
                         select role;
            }
            else
            {
                iqRole = from role in db.aspnet_Roles
                         select role;
            }

            if (iqRole.Count() != 0)
            {
                roles = iqRole.OrderBy(role => role.RoleName).ToList();
            }

            return roles;
        }

        public List<aspnet_Role> GetRolesForAddingUser()
        {
            List<aspnet_Role> roles = new List<aspnet_Role>();

            Guid formerTeacherRoleId = (from param in db.System_Parameters select param.RoleGVCNId).First();
            Guid subjectTeacherRoleId = (from param in db.System_Parameters select param.RoleGVBMId).First();

            IQueryable<aspnet_Role> iqRole;
            iqRole = from role in db.aspnet_Roles
                     join roleDetail in db.UserManagement_RoleDetails
                         on role.RoleId equals roleDetail.RoleId
                     where roleDetail.ParentRoleId == null
                        && role.RoleId != formerTeacherRoleId
                        && role.RoleId != subjectTeacherRoleId
                     select role;

            if (iqRole.Count() != 0)
            {
                roles = iqRole.OrderBy(role => role.RoleName).ToList();
            }

            return roles;
        }

        public bool ValidateAuthorization(Guid role, string pageUrl)
        {
            IQueryable<int> authorizationIds = from authorizedPage in db.UserManagement_AuthorizedPages
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

        public bool ValidateAuthorization(List<aspnet_Role> roles, string pageUrl)
        {
            foreach (aspnet_Role role in roles)
            {
                IQueryable<int> authorizationIds;
                authorizationIds = from authorizedPage in db.UserManagement_AuthorizedPages
                                   join pagePath in db.UserManagement_PagePaths
                                       on authorizedPage.PagePathId equals pagePath.PagePathId
                                   join authorization in db.UserManagement_Authorizations
                                       on authorizedPage.AuthorizedPageId equals authorization.AuthorizedPagePathId
                                   where pagePath.PhysicalPath == pageUrl
                                       && authorization.RoleId == role.RoleId
                                   select authorization.AuthorizationId;
                if (authorizationIds.Count() != 0)
                {
                    return true;
                }
            }

            return false;
        }

        public void AddUserToRole(string userName, aspnet_Role role)
        {   
            aspnet_User user = (new UserDA(school)).GetUser(userName);

            aspnet_UsersInRole usersInRole = new aspnet_UsersInRole
            {
                RoleId = role.RoleId,
                UserId = user.UserId
            };
            db.aspnet_UsersInRoles.InsertOnSubmit(usersInRole);
            db.SubmitChanges();
        }

        public void AddUserToRoleTeacher(string teacherCode)
        {
            Guid userId = (from user in db.aspnet_Users
                           where user.UserName == teacherCode
                           select user.UserId).First();

            // Xác định xem giáo viên này có chủ nhiệm lớp nào không?
            IQueryable<aspnet_User> iqGiaoVien;
            iqGiaoVien = from giaoVien in db.aspnet_Users
                         join GVNCN in db.LopHoc_GVCNs on giaoVien.UserId equals GVNCN.TeacherId
                         where giaoVien.UserName == teacherCode
                         select giaoVien;
            if (iqGiaoVien.Count() != 0)
            {
                Guid formerTeacherRoleId = (from param in db.System_Parameters select param.RoleGVCNId).First();
                db.aspnet_UsersInRoles.InsertOnSubmit(new aspnet_UsersInRole
                {
                    RoleId = formerTeacherRoleId,
                    UserId = userId
                });
            }

            // Xác định xem giáo viên này có day lớp nào không?
            iqGiaoVien = from giaoVien in db.aspnet_Users
                         join tkb in db.LopHoc_MonHocTKBs on giaoVien.UserId equals tkb.TeacherId
                         where giaoVien.UserName == teacherCode
                         select giaoVien;

            if (iqGiaoVien.Count() != 0)
            {
                Guid subjectTeacherRoleId = (from param in db.System_Parameters select param.RoleGVBMId).First();
                db.aspnet_UsersInRoles.InsertOnSubmit(new aspnet_UsersInRole
                {
                    RoleId = subjectTeacherRoleId,
                    UserId = userId
                });
            }

            db.SubmitChanges();
        }

        public List<UserManagement_Function> GetListRoleParentsBasedFunctions()
        {
            List<UserManagement_Function> lstRoleBasedFunctions = new List<UserManagement_Function>();

            IQueryable<UserManagement_Function> functions;
            functions = from function in db.UserManagement_Functions
                        where function.FunctionFlag == FunctionFlag.GetName(typeof(FunctionFlag), FunctionFlag.PARENTSONLY)
                        select function;
            if (functions.Count() != 0)
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
                            if (!lstFunctions.Contains(lstRoleBasedFunctions[i]))
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

        public Guid GetRoleAdminId()
        {
            Guid adminRoleId = (from param in db.System_Parameters
                                select param.AdminRoleId).First();
            return adminRoleId;
        }

        public bool IsRoleParents(aspnet_Role role)
        {
            IQueryable<aspnet_Role> iqRoleParents;
            iqRoleParents = from rl in db.aspnet_Roles
                            join param in db.System_Parameters on role.RoleId equals param.ParentsRoleId
                            where rl.RoleId == role.RoleId
                            select rl;

            if (iqRoleParents.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsRoleTeachers(aspnet_Role role)
        {
            IQueryable<aspnet_Role> iqRoleParents;
            iqRoleParents = from rl in db.aspnet_Roles
                            join param in db.System_Parameters on role.RoleId equals param.RoleTeacher
                            where rl.RoleId == role.RoleId
                            select rl;

            if (iqRoleParents.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsRoleParentsChildRole(Guid roleId)
        {
            aspnet_Role roleParents = (from role in db.aspnet_Roles
                                       join param in db.System_Parameters on role.RoleId equals param.ParentsRoleId
                                       select role).First();

            if (roleParents.RoleId == roleId)
            {
                return true;
            }
            else
            {
                IQueryable<aspnet_Role> iqRole;
                iqRole = from role in db.aspnet_Roles
                         join roleDetail in db.UserManagement_RoleDetails on role.RoleId equals roleDetail.RoleId
                         where (role.RoleId == roleId) && (roleDetail.ParentRoleId == roleParents.RoleId)
                         select role;
                if (iqRole.Count() != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsRoleFormerTeacher(string roleName)
        {
            IQueryable<aspnet_Role> iqFormerTeacherRole;
            iqFormerTeacherRole = from role in db.aspnet_Roles
                                  join param in db.System_Parameters on role.RoleId equals param.RoleGVCNId
                                  where role.RoleName == roleName
                                  select role;

            if (iqFormerTeacherRole.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsRoleSubjectTeacher(string roleName)
        {
            IQueryable<aspnet_Role> iqSbjTeacherRole;
            iqSbjTeacherRole = from role in db.aspnet_Roles
                               join param in db.System_Parameters on role.RoleId equals param.RoleGVBMId
                               where role.RoleName == roleName
                               select role;

            if (iqSbjTeacherRole.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsRoleTeacher(aspnet_Role role)
        {
            IQueryable<aspnet_Role> iqRoleParents;
            iqRoleParents = from rl in db.aspnet_Roles
                            join param in db.System_Parameters on role.RoleId equals param.RoleTeacher
                            where role.RoleId == role.RoleId
                            select role;

            if (iqRoleParents.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get roles those do not have child role
        /// </summary>
        /// <returns></returns>
        public List<aspnet_Role> GetAuthorizedRoles()
        {
            List<aspnet_Role> roles = new List<aspnet_Role>();
            IQueryable<aspnet_Role> iqRoles;

            // ascentor RoleId
            List<Guid> ascentorRoleIds;
            IQueryable<Guid> iqAscentorRoleId = from role in db.aspnet_Roles
                                                where role.UserManagement_RoleDetail.ParentRoleId != null
                                                && role.UserManagement_RoleDetail.SchoolId == school.SchoolId
                                                select (Guid)role.UserManagement_RoleDetail.ParentRoleId;
            if (iqAscentorRoleId.Count() != 0)
            {
                ascentorRoleIds = iqAscentorRoleId.ToList();
                iqRoles = from role in db.aspnet_Roles
                          where ascentorRoleIds.Contains(role.RoleId) == false
                          && role.UserManagement_RoleDetail.SchoolId == school.SchoolId
                          select role;
            }
            else
            {
                iqRoles = from role in db.aspnet_Roles
                          where role.UserManagement_RoleDetail.SchoolId == school.SchoolId
                          select role;
            }

            if (iqRoles.Count() != 0)
            {
                roles = iqRoles.ToList();
            }

            return roles;
        }
    }
}