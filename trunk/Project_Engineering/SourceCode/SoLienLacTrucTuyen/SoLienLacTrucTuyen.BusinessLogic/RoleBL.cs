using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class RoleBL
    {
        private RoleDA roleDA;

        public RoleBL()
        {
            roleDA = new RoleDA();
        }

        public void UpdateRole(string roleName, 
            string newRoleName, string description)
        {
            roleDA.UpdateRole(roleName, newRoleName, description);
        }

        public void CreateRoleDetail(string roleName, 
            string description)
        {
            roleDA.CreateRoleDetail(roleName, description);            
        }

        public void UpdateRoleDetail(string roleName,
            string description, bool expired, bool canBeDeleted, bool actived)
        {
            roleDA.UpdateRoleDetail(roleName,
                description, expired, canBeDeleted, actived);
        }

        public List<TabularRole> GetListTbRoles(string roleName,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            if ((roleName == "") || (string.Compare(roleName, "tất cả", true) == 0))
            {
                return roleDA.GetListTbRoles(pageCurrentIndex, pageSize, out totalRecords);
            }
            else
            {
                return roleDA.GetListTbRoles(roleName, pageCurrentIndex, pageSize, out totalRecords);
            }
            
        }

        public void DeleteRole(string roleName)
        {
            roleDA.DeleteRole(roleName);
        }

        public List<AccessibilityEnum> GetAccessibilities(Guid roleId, string pageUrl)
        {
            return roleDA.GetAccessibilities(roleId, pageUrl);
        }

        public TabularRole GetTbRole(Guid roleId)
        {
            return roleDA.GetTbRole(roleId);
        }        

        public List<aspnet_Role> GetListRoles()
        {
            bool parentRoleOnly = true;
            return roleDA.GetListRoles(parentRoleOnly);
        }

        public Guid GetRoleAdmin()
        {
            return roleDA.GetRoleAdmin();
        }

        public bool IsRoleParents(string roleName)
        {
            return roleDA.IsRoleParents(roleName);
        }
        
        public bool ValidateAuthorization(Guid role, string pageUrl)
        {
            return roleDA.ValidateAuthorization(role, pageUrl);
        }        

        public bool RoleExists(string roleName)
        {
            return roleDA.RoleExists(roleName);
        }

        public bool RoleExists(string roleName, string newRoleName)
        {
            return roleDA.RoleExists(roleName, newRoleName);
        }        

        public void AddUserToRole(string userName, string roleName)
        {
            roleDA.AddUserToRole(userName, roleName);
        }        

        public List<UserManagement_Function> GetListRoleParentsBasedFunctions()
        {
            return roleDA.GetListRoleParentsBasedFunctions();
        }

        public string GetChildRoleParentsByFunctions(List<int> lstFunctions)
        {
            return roleDA.GetChildRoleParentsByFunctions(lstFunctions);
        }

        public bool CanDeleteRole(string roleName)
        {
            return roleDA.CanDeleteRole(roleName);
        }

        public bool IsRoleGiaoVienChuNhiem(string roleName)
        {
            return roleDA.IsRoleGiaoVienChuNhiem(roleName);
        }

        public bool IsRoleGiaoVienBoMon(string roleName)
        {
            return roleDA.IsRoleGiaoVienBoMon(roleName);
        }

    }
}
