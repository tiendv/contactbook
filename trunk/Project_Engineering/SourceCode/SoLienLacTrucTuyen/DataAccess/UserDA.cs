using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.BusinessEntity;

namespace EContactBook.DataAccess
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

        public aspnet_User GetUser(Guid userId)
        {
            IQueryable<aspnet_User> iqUser;
            iqUser = from user in db.aspnet_Users
                     where user.UserId == userId
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
            bool bDeletable = (from membership in db.aspnet_Memberships
                               where membership.UserId == user.UserId
                               select membership.IsDeletable).First();

            if (bDeletable)
            {
                IQueryable<Class_Schedule> iqSchedule = from schedule in db.Class_Schedules
                                                        where schedule.TeacherId == user.UserId
                                                        select schedule;
                if (iqSchedule.Count() == 0)
                {
                    IQueryable<Class_FormerTeacher> iqFormerTeacher = from f in db.Class_FormerTeachers
                                                                      where f.TeacherId == user.UserId
                                                                      select f;
                    if (iqFormerTeacher.Count() != 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

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
                membership.IsActivated = true;
                db.SubmitChanges();
            }
        }

        public void UpdateMembership(aspnet_User user, bool isTeacher, string realName, string email, bool activated, bool deletable)
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
                membership.IsActivated = activated;
                membership.IsDeletable = deletable;
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

        public void ChangeUserActivation(aspnet_User user, bool activateStatus)
        {
            IQueryable<aspnet_Membership> iqMembership;
            iqMembership = from membership in db.aspnet_Memberships
                           where user.UserId == membership.aspnet_User.UserId
                           select membership;

            if (iqMembership.Count() != 0)
            {
                foreach (aspnet_Membership membership in iqMembership)
                {
                    membership.IsActivated = activateStatus;
                }

                db.SubmitChanges();
            }
        }

        public void ChangeUserActivation(List<aspnet_User> users, bool activateStatus)
        {
            List<Guid> gUserIds = new List<Guid>();
            foreach (aspnet_User user in users)
            {
                gUserIds.Add(user.UserId);
            }

            IQueryable<aspnet_Membership> iqMembership;
            iqMembership = from membership in db.aspnet_Memberships
                           where gUserIds.Contains(membership.aspnet_User.UserId)
                           select membership;

            if (iqMembership.Count() != 0)
            {
                foreach (aspnet_Membership membership in iqMembership)
                {
                    membership.IsActivated = activateStatus;
                }

                db.SubmitChanges();
            }
        }

        public void DeleteUser(School_School school)
        {
            IQueryable<aspnet_Membership> queryMembership = from m in db.aspnet_Memberships
                                                            where m.SchoolId == school.SchoolId
                                                            select m;

            if (queryMembership.Count() != 0)
            {
                db.aspnet_Memberships.DeleteAllOnSubmit(queryMembership);
                db.SubmitChanges();
            }

            string strSchoolId = school.SchoolId.ToString() + "_";
            IQueryable<aspnet_User> queryUser = from user in db.aspnet_Users
                                                where user.UserName.Contains(strSchoolId)
                                                select user;

            if (queryUser.Count() != 0)
            {
                db.aspnet_Users.DeleteAllOnSubmit(queryUser);
                db.SubmitChanges();
            }
        }

        public bool DuplicateTeacherEmailExist(string email)
        {
            IQueryable<aspnet_Membership> queryMembership = from membership in db.aspnet_Memberships
                                                            where membership.Email == email
                                                            && membership.IsTeacher == true
                                                            && membership.SchoolId == school.SchoolId
                                                            select membership;

            if (queryMembership.Count() != 0)
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
