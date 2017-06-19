using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLogger.App.Core.Authentication
{
    public static class PasswordFactory
    {

        #region Public methods

        public static Password CreateFromString(string passwordString)
        {
            var password = new Password();
            password.Generate(passwordString);
            return password;
        }

        public static Password CreateFromStringWithSalt(string passwordString, string salt)
        {
            var password = new Password();
            password.Generate(passwordString, salt);
            return password;
        }

        #endregion

    }
}
