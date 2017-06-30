using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLogger.App.Core.Notification
{
    public class PasswordResetNotification : NotificationBase
    {

        #region Properties

        public Guid RequestId { get; set; }

        #endregion

        #region Constructors

        public PasswordResetNotification(SmtpServerSettings settings) : base(settings)
        {
        }

        #endregion

        #region Overridden methods

        public override string GetBody()
        {
            return @"
Hello!

Someone has requested password reset for your account at TimeLoggerLive.
If that someone was you, follow this link: https://" + Domain + @"/App/PasswordReset?id=" + RequestId.ToString() + @"

If this request was not made by you, ignore this message. Password reset request will expire in 2 days.

Thank you again for believing in us,
Vladimir Kocjancic,
Creator of TimeLoggerLive
";
        }

        public override string GetSubject()
        {
            return "TimeLoggerLive: Password reset";
        }

        #endregion

    }
}
