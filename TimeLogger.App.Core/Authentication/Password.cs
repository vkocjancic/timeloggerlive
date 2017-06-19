using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TimeLogger.App.Core.Authentication
{
    public class Password
    {

        #region Properties

        public string PasswordHash { get; protected set; }
        public string Salt { get; protected set; }

        public string Separator { get; } = "..";

        #endregion

        #region Constructors

        public Password() { }

        public Password(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }
            if (!password.Contains(Separator))
            {
                throw new ArgumentException("Password does not contain salt information");
            }
            var tokens = password.Split(new string[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
            PasswordHash = tokens[0];
            Salt = tokens[1];
        }

        #endregion

        #region Public methods

        public void Generate(string passwordString, string salt = null)
        {
            Salt = (null == salt) ? CreateSalt(8) : salt;
            var hash = Encoding.UTF8.GetBytes(passwordString + Salt);
            using (var algorithm = SHA512.Create())
            {
                for (var nIteration = 0; nIteration < 10000; nIteration++)
                {
                    hash = algorithm.ComputeHash(hash);
                }
            }
            PasswordHash = Convert.ToBase64String(hash);
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            var password = obj as Password;
            if (null != password)
            {
                return password.PasswordHash == PasswordHash;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"{PasswordHash}..{Salt}";
        }

        #endregion

        #region Private methods

        private string CreateSalt(int size)
        {
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }
        #endregion

    }
}
