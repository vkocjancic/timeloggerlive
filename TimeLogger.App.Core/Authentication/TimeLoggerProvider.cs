using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using TimeLogger.App.Core.Adapter;
using TimeLogger.App.Core.Business;
using TimeLogger.App.Core.Common;
using TimeLogger.App.Core.Repository;

namespace TimeLogger.App.Core.Authentication
{
    public class TimeLoggerProvider : MembershipProvider
    {

        #region Properties

        public string ConnectionString { get; protected set; }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool EnablePasswordReset
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        private int minRequiredPasswordLength;
        public override int MinRequiredPasswordLength { get { return minRequiredPasswordLength; } }

        public override int PasswordAttemptWindow
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string PasswordStrengthRegularExpression
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                return false;
            }
        }

        public override bool RequiresUniqueEmail
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region Public methods

        public override void Initialize(string name, NameValueCollection config)
        {
            ConnectionString = ConfigurationManager.ConnectionStrings[config["connectionStringName"]].ConnectionString;
            minRequiredPasswordLength = Convert.ToInt32(config["minRequiredPasswordLength"]);
            base.Initialize(name, config);
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            var repo = new UserRepository(ConnectionString);
            var user = repo.GetByEmail(username);
            if (null == user)
            {
                return false;
            }
            user.Password = newPassword;
            repo.ChangePassword(user);
            return true;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            try
            {
                var mailAddress = new Email(email);
                if (!mailAddress.IsValid)
                {
                    status = MembershipCreateStatus.InvalidEmail;
                    return null;
                }
                var repo = new UserRepository(ConnectionString);
                var user = repo.GetByEmail(mailAddress.EmailAddress);
                if (null != user)
                {
                    status = MembershipCreateStatus.DuplicateUserName;
                    return null;
                }
                if (password.Length < MinRequiredPasswordLength)
                {
                    status = MembershipCreateStatus.InvalidPassword;
                    return null;
                }
                user = repo.CreateUser(new User()
                {
                    Created = DateTime.Now,
                    Email = email,
                    Password = password,
                    IsApproved = isApproved
                });
                status = MembershipCreateStatus.Success;
                return UserAdapter.ToMembershipUser(user);
            }
            catch (Exception ex)
            {
                status = MembershipCreateStatus.ProviderError;
                return null;
            }
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            var repo = new UserRepository(this.ConnectionString);
            var user = repo.GetByEmail(username);
            return (null == user) ? null : UserAdapter.ToMembershipUser(user);
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            var repo = new UserRepository(this.ConnectionString);
            var user = repo.GetById((Guid)providerUserKey);
            return (null == user) ? null : UserAdapter.ToMembershipUser(user);
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            var repo = new UserRepository(ConnectionString);
            var user = repo.GetByEmail(username);
            return ((null != user) && (user.IsPasswordCorrect(password)));
        }

        #endregion

    }

}
