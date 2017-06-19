using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace TimeLogger.App.Core.Authentication
{
    public class TimeLoggerRoleProvider : RoleProvider
    {
        #region Properties

        public override string ApplicationName { get; set; }

        #endregion

        #region Methods

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            return new string[] { "Admin", "User" };
        }

        public override string[] GetRolesForUser(string username)
        {
            if ("admin" == username)
            {
                return GetAllRoles().Where(r => r == "Admin").ToArray();
            }
            return GetAllRoles().Where(r => r == "User").ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            return GetRolesForUser(username).Any(r => r.ToLower() == roleName.ToLower());
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            return GetAllRoles().Any(r => r.ToLower() == roleName.ToLower());
        }

        #endregion
    }
}
