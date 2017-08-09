using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TimeLogger.App.Core.Notification
{
    public class RegistrationNotification : NotificationBase
    {
        
        #region Constructors

        public RegistrationNotification(SmtpServerSettings settings) 
            : base(settings)
        {
        }

        #endregion

        #region Overridden methods

        public override string GetBody()
        {
            return @"
Hello!

Thank you for early-bird registration to TimeLoggerLive. 
The application is currently in public-beta and is expected to be production ready by November 1st, 2017. 
Your early-bird registration guarantess: 
 * lower subscription price for first and following 3 years, 
 * access to all non-production releases,
 * hassle free, any-time money back guarantee,
 * personalized support.

Thank you again for believing in us,
Vladimir Kocjancic,
Creator of TimeLoggerLive
";
        }

        public override string GetSubject()
        {
            return "TimeLoggerLive: Account registration";
        }

        #endregion

    }
}
