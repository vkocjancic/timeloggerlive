using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TimeLogger.App.Core.Common
{
    public class Email
    {

        #region Properties

        public string EmailAddress { get; set; }
        public bool IsValid { get; protected set; }

        #endregion

        #region Constructors

        public Email(string emailAddress)
        {
            EmailAddress = emailAddress;
            IsValid = Validate();
        }

        #endregion

        #region Public methods

        public bool Validate()
        {
            try
            {
                return (!string.IsNullOrWhiteSpace(EmailAddress)) 
                    && (new MailAddress(EmailAddress).Address == EmailAddress);
            }
            catch
            {
                return false;
            }
        }

        #endregion

    }
}
