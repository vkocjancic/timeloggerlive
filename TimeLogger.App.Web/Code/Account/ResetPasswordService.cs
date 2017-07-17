using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using TimeLogger.App.Core.Repository;

namespace TimeLogger.App.Web.Code.Account
{
    public static class ResetPasswordService
    {

        #region Public methods

        public static ResetPasswordResponse ResetPassword(string connectionString, ResetPasswordModel model)
        {
            var repository = new PasswordResetRepository(connectionString);
            var requestId = new Guid(model.Id);
            var request = repository.GetById(requestId, DateTime.Now.AddDays(-2));
            if (null == request)
            {
                return new ResetPasswordResponse()
                {
                    Code = System.Net.HttpStatusCode.NotFound,
                    Success = false,
                    ErrorDescription = AppResources.ErrorInvalidRequest
                };
            }
            var user = Membership.GetUser(request.UserId);
            if (!user.ChangePassword("oldPassword", model.Password))   
            {
                return new ResetPasswordResponse()
                {
                    Code = System.Net.HttpStatusCode.NotFound,
                    Success = false,
                    ErrorDescription = AppResources.ErrorChangePassword
                };
            }
            repository.Invalidate(requestId);
            return new ResetPasswordResponse() { Code = System.Net.HttpStatusCode.Created, Success = true };
        }

        public static ResetPasswordResponse ResetPasswordForAuthenticatedUser(string connectionString, ResetPasswordModel model, Guid userId)
        {
            var user = Membership.GetUser(userId);
            if (null == user)
            {
                return new ResetPasswordResponse()
                {
                    Code = System.Net.HttpStatusCode.NotFound,
                    Success = false,
                    ErrorDescription = AppResources.ErrorInvalidRequest
                };
            }
            if (!user.ChangePassword("oldPassword", model.Password))
            {
                return new ResetPasswordResponse()
                {
                    Code = System.Net.HttpStatusCode.NotFound,
                    Success = false,
                    ErrorDescription = AppResources.ErrorChangePassword
                };
            }
            return new ResetPasswordResponse() { Code = System.Net.HttpStatusCode.Created, Success = true };
        }

        #endregion

    }
}