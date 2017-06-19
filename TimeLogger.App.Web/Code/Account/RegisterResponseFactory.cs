using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;

namespace TimeLogger.App.Web.Code.Account
{
    public static class RegisterResponseFactory
    {

        #region Public methods

        public static RegisterResponse CreateFromMembershipStatus(MembershipCreateStatus createStatus)
        {
            var response = new RegisterResponse() { Code = HttpStatusCode.OK, Success = false };
            switch (createStatus)
            {
                case MembershipCreateStatus.Success:
                    response.Success = true;
                    break;
                case MembershipCreateStatus.DuplicateUserName:
                    response.Code = HttpStatusCode.Ambiguous;
                    response.Success = false;
                    response.ErrorDescription = Resources.AppResources.ErrorDuplicateUsername;
                    break;
                case MembershipCreateStatus.DuplicateEmail:
                    response.Code = HttpStatusCode.Ambiguous;
                    response.Success = false;
                    response.ErrorDescription = Resources.AppResources.ErrorDuplicateEmail;
                    break;
                case MembershipCreateStatus.InvalidEmail:
                    response.Code = HttpStatusCode.BadRequest;
                    response.Success = false;
                    response.ErrorDescription = Resources.AppResources.ErrorInvalidEmail;
                    break;
                case MembershipCreateStatus.InvalidAnswer:
                    response.Code = HttpStatusCode.BadRequest;
                    response.Success = false;
                    response.ErrorDescription = Resources.AppResources.ErrorInvalidAnswer;
                    break;
                case MembershipCreateStatus.InvalidPassword:
                    response.Code = HttpStatusCode.BadRequest;
                    response.Success = false;
                    response.ErrorDescription = Resources.AppResources.ErrorInvalidPassword;
                    break;
                default:
                    response.Code = HttpStatusCode.InternalServerError;
                    response.Success = false;
                    response.ErrorDescription = Resources.AppResources.ErrorOther;
                    break;
            }
            return response;
        }

        #endregion

    }
}