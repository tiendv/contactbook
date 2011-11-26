using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class RoleBL : BaseBL
    {
        private RoleDA roleDA;

        public RoleBL(School school)
            : base(school)
        {
            roleDA = new RoleDA(school);
        }

        public void UpdateRole(string roleName, string newRoleName, string description)
        {
            roleName = GetActualName(roleName);
            newRoleName = GetActualName(newRoleName);

            roleDA.UpdateRole(roleName, newRoleName, description);
        }

        public void CreateRoleDetail(string roleName, string description)
        {
            roleName = GetActualName(roleName);
            roleDA.CreateRoleDetail(roleName, description);
        }

        public void UpdateRoleDetail(string roleName, string description, bool expired, bool canBeDeleted, bool actived)
        {
            roleName = GetActualName(roleName);
            roleDA.UpdateRoleDetail(roleName, description, expired, canBeDeleted, actived);
        }

        public TabularRole GetTabularRole(Guid roleId)
        {
            return roleDA.GetTabRole(roleId);
        }
        
        public List<TabularRole> GetTabularRoles(string roleName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            if ((roleName == "") || (string.Compare(roleName, "tất cả", true) == 0))
            {
                return roleDA.GetListTbRoles(pageCurrentIndex, pageSize, out totalRecords);
            }
            else
            {
                roleName = GetActualName(roleName);
                return roleDA.GetListTbRoles(roleName, pageCurrentIndex, pageSize, out totalRecords);
            }
        }

        public void DeleteRole(string roleName)
        {
            roleName = GetActualName(roleName);
            roleDA.DeleteRole(roleName);
        }

        public List<AccessibilityEnum> GetAccessibilities(Guid roleId, string pageUrl)
        {
            return roleDA.GetAccessibilities(roleId, pageUrl);
        }

        public List<aspnet_Role> GetRoles()
        {
            bool parentRoleOnly = true;
            return roleDA.GetListRoles(parentRoleOnly);
        }

        public List<aspnet_Role> GetRolesForAddingUser()
        {
            return roleDA.GetRolesForAddingUser();
        }

        public Guid GetRoleADMIN()
        {
            return roleDA.GetRoleAdminId();
        }

        public bool IsRolePARENTS(string roleName)
        {
            return roleDA.IsRoleParents(roleName);
        }

        public bool ValidateAuthorization(Guid role, string pageUrl)
        {
            return roleDA.ValidateAuthorization(role, pageUrl);
        }

        public bool ValidateAuthorization(List<aspnet_Role> roles, string pageUrl)
        {
            return roleDA.ValidateAuthorization(roles, pageUrl);
        }

        public bool RoleExists(string roleName)
        {
            roleName = GetActualName(roleName);
            return roleDA.RoleExists(roleName);
        }

        public bool RoleExists(string oldRoleName, string newRoleName)
        {
            if (oldRoleName == newRoleName)
            {
                return false;
            }
            else
            {
                newRoleName = GetActualName(newRoleName);
                return roleDA.RoleExists(newRoleName);
            }
        }

        public void AddUserToRole(string userName, string roleName)
        {
            if (roleName == "Giáo viên")
            {
                roleDA.AddUserToRoleTeacher(userName);
            }
            else
            {
                roleDA.AddUserToRole(userName, roleName);
            }
        }

        public List<UserManagement_Function> GetListRoleParentsBasedFunctions()
        {
            return roleDA.GetListRoleParentsBasedFunctions();
        }

        public string GetChildRoleParentsByFunctions(List<int> lstFunctions)
        {
            return roleDA.GetChildRoleParentsByFunctions(lstFunctions);
        }

        public bool IsDeletable(string roleName)
        {
            roleName = GetActualName(roleName);
            return roleDA.IsDeletableRole(roleName);
        }

        public bool IsRoleFORMERERTEACHER(string roleName)
        {
            return roleDA.IsRoleFormerTeacher(roleName);
        }

        public bool IsRoleSUBJECTEDTEACHER(string roleName)
        {
            return roleDA.IsRoleSubjectTeacher(roleName);
        }

    }
}
