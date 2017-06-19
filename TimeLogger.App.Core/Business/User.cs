using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeLogger.App.Core.Authentication;

namespace TimeLogger.App.Core.Business
{
    public class User
    {

        #region Properties

        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Created { get; set; }
        public bool IsApproved { get; set; }

        private Password _passwordObject = null;
        public Password PasswordObject
        {
            get
            {
                if (null == _passwordObject)
                {
                    _passwordObject = new Password(this.Password);
                }
                return _passwordObject;
            }
        }

        #endregion

        #region Public methods

        public bool IsPasswordCorrect(string password)
        {
            var passwordToCompare = PasswordFactory.CreateFromStringWithSalt(password, PasswordObject.Salt);
            return PasswordObject.Equals(passwordToCompare);
        }

        #endregion

    }
}
