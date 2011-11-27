using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class AuthorizationBL : BaseBL
    {
        AuthorizationDA authorizationDA;
        RoleDA roleDA;
        public AuthorizationBL(School school)
            : base(school)
        {
            roleDA = new RoleDA(school);
            authorizationDA = new AuthorizationDA(school);
        }

        public List<TabularRole> GetAuthorizedRoles()
        {
            RoleBL roleBL = new RoleBL(school);

            List<aspnet_Role> roles = roleDA.GetAuthorizedRoles();
            List<TabularRole> tabularRoles = null;
            TabularRole tabularRole = null;
            tabularRoles = new List<TabularRole>();
            foreach (aspnet_Role role in roles)
            {
                tabularRole = roleBL.ConvertToTabular(role);
                tabularRoles.Add(tabularRole);
            }

            return tabularRoles;
        }

        public void UpdateRoleDetail(string roleName, string description, bool expired, bool canBeDeleted, bool actived)
        {
            roleName = GetActualName(roleName);
            roleDA.UpdateRoleDetail(roleName, description, expired, canBeDeleted, actived);
        }

        public List<AccessibilityEnum> GetAccessibilities(Guid roleId, string pageUrl)
        {
            return roleDA.GetAccessibilities(roleId, pageUrl);
        }

        public List<aspnet_Role> GetRoles(bool ascentorRoleOnly)
        {
            return roleDA.GetListRoles(ascentorRoleOnly);
        }

        public List<aspnet_Role> GetRolesForAddingUser()
        {
            return roleDA.GetRolesForAddingUser();
        }

        public Guid GetRoleADMIN()
        {
            return roleDA.GetRoleAdminId();
        }

        public bool IsRoleTeachers(aspnet_Role role)
        {
            return roleDA.IsRoleTeachers(role);
        }

        public bool IsRoleParents(aspnet_Role role)
        {
            return roleDA.IsRoleParents(role);
        }

        public bool ValidateAuthorization(Guid role, string pageUrl)
        {
            return roleDA.ValidateAuthorization(role, pageUrl);
        }

        public bool ValidateAuthorization(List<aspnet_Role> roles, string pageUrl)
        {
            return roleDA.ValidateAuthorization(roles, pageUrl);
        }

        public void AddUserToRole(string userName, aspnet_Role role)
        {
            roleDA.AddUserToRole(userName, role);
        }

        public List<UserManagement_Function> GetListRoleParentsBasedFunctions()
        {
            return roleDA.GetListRoleParentsBasedFunctions();
        }

        public string GetChildRoleParentsByFunctions(List<int> lstFunctions)
        {
            return roleDA.GetChildRoleParentsByFunctions(lstFunctions);
        }

        public bool IsRoleFORMERERTEACHER(string roleName)
        {
            return roleDA.IsRoleFormerTeacher(roleName);
        }

        public bool IsRoleSUBJECTEDTEACHER(string roleName)
        {
            return roleDA.IsRoleSubjectTeacher(roleName);
        }

        public List<UserManagement_Function> GetStudentFunctions(string functionFlag)
        {
            return authorizationDA.GetStudentFunctions(AuthorizationDA.FUNCTIONFLAG_OTHERS);
        }

        public List<UserManagement_PagePath> GetStudentPages(List<aspnet_Role> roles)
        {
            List<UserManagement_PagePath> pagePages = new List<UserManagement_PagePath>();
            pagePages.Add(new UserManagement_PagePath
            {
                PhysicalPath = "/modules/hoc_sinh/thongtincanhan.aspx",
                PageTitle = "Thông tin cá nhân"                
            });
            pagePages.Add(new UserManagement_PagePath
            {
                PhysicalPath = "/modules/hoc_sinh/ketquahoctap.aspx",
                PageTitle = "Kết quả học tập"
            });
            pagePages.Add(new UserManagement_PagePath
            {
                PhysicalPath = "/modules/hoc_sinh/ngaynghihoc.aspx",
                PageTitle = "Ngày nghỉ học"
            });
            pagePages.Add(new UserManagement_PagePath
            {
                PhysicalPath = "/modules/hoc_sinh/hoatdong.aspx",
                PageTitle = "Hoạt động"
            });

            int i = 0;
            while(i < pagePages.Count)
            {
                if (!ValidateAuthorization(roles, pagePages[i].PhysicalPath))
                {
                    pagePages.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }            

            return pagePages;
        }

        public void AddServicesToParentsUser(aspnet_User createdUser, List<UserManagement_Function> functions)
        {
            authorizationDA.AddServicesToParentsUser(createdUser, functions);
        }
    }
}
