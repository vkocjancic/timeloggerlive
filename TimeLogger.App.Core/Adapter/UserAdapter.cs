using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using TimeLogger.App.Core.Business;

namespace TimeLogger.App.Core.Adapter
{
    internal static class UserAdapter
    {

        public static MembershipUser ToMembershipUser(User user)
        {
            return new MembershipUser(
                "TimeLoggerProvider",
                user.Email,
                user.Id,
                user.Email,
                null,
                null,
                user.IsApproved,
                false,
                user.Created,
                DateTime.Now,
                DateTime.Today,
                DateTime.Today,
                DateTime.MinValue);
        }

    }
}
