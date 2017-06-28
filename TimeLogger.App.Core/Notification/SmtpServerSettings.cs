using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLogger.App.Core.Notification
{
    public class SmtpServerSettings
    {

        #region Properties

        public string Server { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Domain { get; set; }

        #endregion

        #region Constructors

        public SmtpServerSettings(string server, string port, string username, string password, string domain)
        {
            Server = $"{server}{domain}";
            Port = int.Parse(port);
            Username = $"{username}@{domain}";
            Password = password;
            Domain = domain;
        }

        #endregion

    }
}
