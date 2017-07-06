using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeLogger.App.Core.Authentication;
using TimeLogger.App.Core.Business;

namespace TimeLogger.App.Core.Repository
{
    public class UserRepository : UserRepositoryBase
    {
        #region Constructors

        public UserRepository(string connectionString) : base(connectionString) { }

        #endregion

        #region Overrides

        public override void ActivateUser(Guid id)
        {
            using(var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute(@"update [USER]
                    set [ACTIVE_YN]='Y'
                    where [USER_ID]=@Id",
                    new { Id = id });
            }
        }

        public override void ChangePassword(User user)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var password = PasswordFactory.CreateFromString(user.Password);
                connection.Open();
                connection.Execute(@"update [USER] 
                    set [PASSWORD] = @Password
                    where [USER_ID] = @Id",
                    new
                    {
                        Id = user.Id,
                        Password = password.ToString()
                    });
            }
        }

        public override User CreateUser(User user)
        {
            user.Id = Guid.NewGuid();
            using (var connection = new SqlConnection(ConnectionString))
            {
                var password = PasswordFactory.CreateFromString(user.Password);
                connection.Open();
                connection.Execute("insert into [USER] (USER_ID, EMAIL, PASSWORD, CREATED, ACTIVE_YN)" +
                    " values (@Id, @Email, @Password, @DateCreated, @Active)",
                    new
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Password = password.ToString(),
                        DateCreated = user.Created,
                        Active = (user.IsApproved) ? 'Y' : 'N'
                    });
            }
            return user;
        }

        public override User GetByEmail(string email)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<User>(
                    @"select USER_ID as Id, EMAIL as Email, PASSWORD as Password, CREATED as Created,
                        case when ACTIVE_YN = 'Y' then 1 else 0 end as IsApproved
                      from [USER]
                      where EMAIL = @EMAIL",
                        new { EMAIL = email }).FirstOrDefault();
            }
        }

        public override User GetById(Guid id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<User>(
                    @"select USER_ID as Id, EMAIL as Email, PASSWORD as Password, CREATED as Created,
                        case when ACTIVE_YN = 'Y' then 1 else 0 end as IsApproved
                      from [USER]
                      where [USER_ID] = @USERID",
                        new { USERID = id }).FirstOrDefault();
            }
        }

        #endregion

    }
}
