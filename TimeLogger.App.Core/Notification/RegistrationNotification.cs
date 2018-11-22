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

Welcome to TimeLoggerLive, the application that allows you to get hold of your time yet again. 
In order to start using this app, please visit https://timelogger.eu/app, log in and enjoy your 90-day free trial.

Thank you again for believing in us,
Vladimir Kocjancic,
Creator of TimeLoggerLive
https://timelogger.eu
";
        }

        public override string GetSubject()
        {
            return "TimeLoggerLive: Account registration";
        }

        #endregion

    }
}
