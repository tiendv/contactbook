using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class AuthorizationBL : BaseBL
    {
        AuthorizationDA authorizationDA;
        RoleDA roleDA;

        public AuthorizationBL(School_School school)
            : base(school)
        {
            roleDA = new RoleDA(school);
            authorizationDA = new AuthorizationDA(school);
        }

        public void InsertAuthorizations(aspnet_Role role)
        {
            FunctionsBL functionsBL = new FunctionsBL();
            List<string> functionFlags = functionsBL.GetFunctionFlags(role);
            List<UserManagement_Function> functions = new List<UserManagement_Function>();
            foreach (string functionFlag in functionFlags)
            {
                functions.AddRange(functionsBL.GetFunctions(functionFlag));
            }

            authorizationDA.InsertAuthorizations(role, functions);
        }

        public void UpdateRoleDetail(string roleName, string description, bool expired, bool IsDeletable, bool actived)
        {
            roleName = GetActualName(roleName);
            roleDA.UpdateRoleDetail(roleName, description, expired, IsDeletable, actived);
        }

        public void DeleteAuthorization(string userName)
        {
            authorizationDA.DeleteAuthorization(userName);
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

        public List<AccessibilityEnum> GetAccessibilities(List<aspnet_Role> roles, string pageUrl)
        {
            return roleDA.GetAccessibilities(roles, pageUrl);
        }

        public List<aspnet_Role> GetRoles(bool ascentorRoleOnly)
        {
            return roleDA.GetListRoles(ascentorRoleOnly);
        }

        public bool IsRoleTeachers(aspnet_Role role)
        {
            return roleDA.IsRoleTeachers(role);
        }

        public bool IsRoleFormerTeacher(string roleName)
        {
            return roleDA.IsRoleFormerTeacher(roleName);
        }

        public bool IsRoleSubjectTeacher(string roleName)
        {
            return roleDA.IsRoleSubjectTeacher(roleName);
        }

        public bool IsRoleParents(aspnet_Role role)
        {
            return roleDA.IsRoleParents(role);
        }

        public bool IsRoleAdmin(aspnet_Role role)
        {
            return roleDA.IsRoleAdmin(role);
        }

        public bool ValidateAuthorization(List<aspnet_Role> roles, string pageUrl)
        {
            return roleDA.ValidateAuthorization(roles, pageUrl);
        }

        public bool ValidateAuthorization(string userName, string pageUrl)
        {
            return roleDA.ValidateAuthorization(userName, pageUrl);
        }

        public void AddUserToRole(string userName, aspnet_Role role)
        {
            roleDA.AddUserToRole(userName, role);
        }

        public List<UserManagement_Function> GetListRoleParentsBasedFunctions()
        {
            return roleDA.GetListRoleParentsBasedFunctions();
        }

        /// <summary>
        /// Get functions supplied to user parents
        /// </summary>
        /// <param name="supliedRoleParentsAuthorizations"></param>
        /// <returns></returns>
        public List<UserManagement_Function> GetSupliedRoleParentsFunctions(List<UserManagement_Authorization> supliedRoleParentsAuthorizations)
        {
            FunctionsBL functionsBL = new FunctionsBL();
            List<UserManagement_Function> supliedRoleParentsFunctions = new List<UserManagement_Function>();
            UserManagement_Function function = null;
            UserManagement_Function homePageFunction = functionsBL.GetHomePageFunction();
            foreach (UserManagement_Authorization authorization in supliedRoleParentsAuthorizations)
            {
                function = authorization.UserManagement_AuthorizedPage.UserManagement_Function;
                if ((!functionsBL.ExistsChildFunction(function)) && (homePageFunction.FunctionId != function.FunctionId))
                {
                    supliedRoleParentsFunctions.Add(function);
                }
            }

            supliedRoleParentsFunctions = supliedRoleParentsFunctions.Distinct().ToList();
            return supliedRoleParentsFunctions;
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
            while (i < pagePages.Count)
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

        public void AddParentsUserRegisteredServices(aspnet_User createdUser, List<UserManagement_Authorization> authorizations, List<ChoseService> choseServices)
        {
            List<UserManagement_RoleParentsAuthorization> roleParentsAuthorizations = new List<UserManagement_RoleParentsAuthorization>();
            UserManagement_RoleParentsAuthorization roleParentsAuthorization = null;
            UserBL userBL = new UserBL(school);
            createdUser = userBL.GetUser(createdUser.UserName);

            foreach (UserManagement_Authorization authorization in authorizations)
            {
                roleParentsAuthorization = new UserManagement_RoleParentsAuthorization();
                roleParentsAuthorization.RoleParentAuthorizationId = authorization.AuthorizationId;
                roleParentsAuthorization.UserParentId = createdUser.UserId;
                roleParentsAuthorization.IsRegistered = authorization.IsActivated; // default
                roleParentsAuthorization.IsActivated = authorization.IsActivated;  // default
                roleParentsAuthorization.GetEmail = false;  // default
                roleParentsAuthorization.GetSMS = false;  // default

                foreach (ChoseService choseService in choseServices)
                {
                    if (choseService.FunctionId == authorization.UserManagement_AuthorizedPage.FunctionId)
                    {
                        roleParentsAuthorization.IsRegistered = choseService.Chose;
                        roleParentsAuthorization.IsActivated = choseService.Chose;
                        roleParentsAuthorization.GetEmail = choseService.GetEmail;
                        roleParentsAuthorization.GetSMS = choseService.GetSMS;
                        break;
                    }
                }

                roleParentsAuthorizations.Add(roleParentsAuthorization);
            }

            authorizationDA.InsertRoleParentsAuthorization(roleParentsAuthorizations);
        }

        public List<TabularAuthorization> GetTabularAuthorizations(aspnet_Role role)
        {
            return authorizationDA.GetTabularAuthorizations(role);
        }

        public void Authorize(aspnet_Role role, List<TabularDetailedAuthorization> detailedAuthorizations)
        {
            List<UserManagement_Function> functions = new List<UserManagement_Function>();
            UserManagement_Function function = null;
            FunctionsBL functionsBL = new FunctionsBL();

            List<UserManagement_AuthorizedPage> authorizedPages = new List<UserManagement_AuthorizedPage>();
            bool bUpdateUserParentsAuthorization = IsRoleParents(role);

            foreach (TabularDetailedAuthorization detailedAuthorization in detailedAuthorizations)
            {
                function = new UserManagement_Function();
                function.FunctionId = detailedAuthorization.FunctionId;

                bool bIncludeParentsFunction = false;
                if (detailedAuthorization.ViewAccessibility || detailedAuthorization.AddAccessibility
                    || detailedAuthorization.ModifyAccessibility || detailedAuthorization.DeleteAccessibility)
                {
                    bIncludeParentsFunction = true;
                }

                authorizationDA.UpdateAuthorization(role, function, AccessibilityEnum.View, detailedAuthorization.ViewAccessibility);
                authorizationDA.UpdateAuthorization(role, function, AccessibilityEnum.Add, detailedAuthorization.AddAccessibility);
                authorizationDA.UpdateAuthorization(role, function, AccessibilityEnum.Modify, detailedAuthorization.ModifyAccessibility);
                authorizationDA.UpdateAuthorization(role, function, AccessibilityEnum.Delete, detailedAuthorization.DeleteAccessibility);
                if (bUpdateUserParentsAuthorization)
                {
                    authorizationDA.UpdateRoleParentsAuthorizations(function, AccessibilityEnum.View, detailedAuthorization.ViewAccessibility);
                    authorizationDA.UpdateRoleParentsAuthorizations(function, AccessibilityEnum.Add, detailedAuthorization.AddAccessibility);
                    authorizationDA.UpdateRoleParentsAuthorizations(function, AccessibilityEnum.Modify, detailedAuthorization.ModifyAccessibility);
                    authorizationDA.UpdateRoleParentsAuthorizations(function, AccessibilityEnum.Delete, detailedAuthorization.DeleteAccessibility);
                }                

                if (bIncludeParentsFunction)
                {
                    UserManagement_Function parentsFunction = functionsBL.GetParentFunction(function);
                    if (parentsFunction != null)
                    {
                        authorizationDA.UpdateAuthorization(role, parentsFunction,
                            AccessibilityEnum.View, detailedAuthorization.ViewAccessibility);
                    }
                }
            }
        }

        public List<UserManagement_Authorization> GetSupliedRoleParentsAuthorizations()
        {
            aspnet_Role role = authorizationDA.GetRoleParents();
            bool bIsActivated = true;
            return authorizationDA.GetAuthorizations(role, bIsActivated);
        }
    }
}
