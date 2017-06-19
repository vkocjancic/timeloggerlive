using TimeLogger.App.Core.Repository;

namespace TimeLogger.App.Core.Business
{

	public static class UserFactory
    {

        #region Public methods

		public static User Get(UserRepositoryBase repository, string email)
        {
            return repository.GetByEmail(email);
        }

        #endregion

    }

}