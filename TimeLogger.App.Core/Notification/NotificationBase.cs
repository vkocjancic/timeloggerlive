using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TimeLogger.App.Core.Notification
{
    public abstract class NotificationBase
    {

        #region Properties

        public string Domain { get; set; }
        public MailAddress From { get; protected set; }
        public string SmtpServer { get; protected set; }
        public int SmtpPort { get; protected set; }
        public string SmtpUsername { get; protected set; }
        public string SmtpPassword { get; protected set; }

        #endregion

        #region Constructors

        public NotificationBase(string server, int port, string username, string password, string domain)
        {
            Domain = domain;
            SmtpPassword = password;
            SmtpPort = port;
            SmtpServer = server;
            SmtpUsername = username;
        }

        #endregion

        #region Abstract methods

        public abstract string GetSubject();
        public abstract string GetBody();

        #endregion

        #region Virtual methods

        public virtual void Send(string recipients)
        {
            using (var mail = new MailMessage())
            using (var client = new SmtpClient(SmtpServer, SmtpPort))
            {
                mail.From = From;
                mail.To.Add(recipients);
                mail.Subject = GetSubject();
                mail.Body = GetBody();
                if (!string.IsNullOrEmpty(SmtpUsername))
                {
                    client.Credentials = new System.Net.NetworkCredential(SmtpUsername, SmtpPassword);
                }
                client.Send(mail);
            }
        }

        #endregion

    }
}
