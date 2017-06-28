using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using TimeLogger.App.Core.Notification;
using TimeLogger.App.Core.Repository;

namespace TimeLogger.App.Web.Code.Notification
{
    public static class NotificationService
    {

        public static void SendPasswordResetEmailTo(string email, SmtpServerSettings settings, string connectionString)
        {
            var guid = Guid.NewGuid();
            var user = Membership.GetUser(email);
            if (null != user)
            {
                Task.Factory.StartNew(() =>
                {
                    var repository = new PasswordResetRepository(connectionString);
                    repository.Create(guid, (Guid)user.ProviderUserKey);
                });
                Task.Factory.StartNew(() =>
                {
                    var notification = new PasswordResetNotification(settings);
                    notification.RequestId = guid;
                    notification.Send(email);
                });
            }
        }

    }
}