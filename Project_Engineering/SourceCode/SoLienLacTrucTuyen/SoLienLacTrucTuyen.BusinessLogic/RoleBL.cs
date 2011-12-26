using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class RoleBL : BaseBL
    {
        private RoleDA roleDA;

        public RoleBL(School_School school)
            : base(school)
        {
            roleDA = new RoleDA(school);
        }

        public void CreateRoleDetail(string roleName, string description)
        {
            roleName = GetActualName(roleName);
            roleDA.CreateRoleDetail(roleName, description);
        }

        public void UpdateRole(string roleName, string newRoleName, string description)
        {
            newRoleName = GetActualName(newRoleName);
            roleDA.UpdateRole(roleName, newRoleName, description);
        }

        public void DeleteRole(string roleName)
        {
            roleDA.DeleteRole(roleName);
        }

        public List<TabularRole> GetTabularRoles()
        {
            List<aspnet_Role> roles = roleDA.GetListRoles(true);
            List<TabularRole> tabularRoles = null;
            TabularRole tabularRole = null;
            tabularRoles = new List<TabularRole>();
            foreach (aspnet_Role role in roles)
            {
                tabularRole = ConvertToTabular(role);
                tabularRoles.Add(tabularRole);
            }

            return tabularRoles;
        }

        public aspnet_Role GetRole(string roleName)
        {
            return roleDA.GetRole(roleName);
        }

        public List<TabularRole> GetTabularRoles(string roleName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<aspnet_Role> roles = null;
            List<TabularRole> tabularRoles = null;
            TabularRole tabularRole = null;

            if ((roleName == "") || (string.Compare(roleName, "tất cả", true) == 0))
            {
                roles = roleDA.GetRoles(pageCurrentIndex, pageSize, out totalRecords);
            }
            else
            {
                roles = new List<aspnet_Role>();
                roleName = GetActualName(roleName);
                aspnet_Role role = roleDA.GetRole(roleName);
                if (role != null)
                {
                    roles.Add(role);
                }
                totalRecords = roles.Count;
            }

            tabularRoles = new List<TabularRole>();
            foreach (aspnet_Role role in roles)
            {
                tabularRole = ConvertToTabular(role);
                tabularRoles.Add(tabularRole);
            }

            return tabularRoles;
        }

        public TabularRole GetTabularRole(Guid roleId)
        {
            TabularRole tabularRole = null;
            aspnet_Role role = roleDA.GetRole(roleId);
            if (role != null)
            {
                tabularRole = ConvertToTabular(role);
            }

            return tabularRole;
        }

        public bool RoleExists(string roleName)
        {
            roleName = GetActualName(roleName);
            return roleDA.RoleExists(roleName);
        }

        public bool RoleExists(string oldRoleName, string newRoleName)
        {
            newRoleName = GetActualName(newRoleName);

            if (oldRoleName == newRoleName)
            {
                return false;
            }
            else
            {
                return roleDA.RoleExists(newRoleName);
            }
        }

        public bool IsDeletable(string roleName)
        {
            return roleDA.IsDeletableRole(roleName);
        }

        internal TabularRole ConvertToTabular(aspnet_Role role)
        {
            TabularRole tabularRole = new TabularRole();
            tabularRole.RoleId = role.RoleId;
            tabularRole.RoleName = role.RoleName;
            tabularRole.DisplayedName = role.UserManagement_RoleDetail.DisplayedName;
            tabularRole.Description = role.Description;
            tabularRole.IsDeletable = role.UserManagement_RoleDetail.IsDeletable;

            return tabularRole;
        }

        public aspnet_Role GetRoleAdministrator()
        {
            return roleDA.GetRoleAdmin();
        }

        public aspnet_Role GetRoleParents()
        {
            return roleDA.GetRoleParents();
        }
    }
}

