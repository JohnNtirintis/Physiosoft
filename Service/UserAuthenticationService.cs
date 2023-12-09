using Physiosoft.DAO;

namespace Physiosoft.Service
{
    public class UserAuthenticationService
    {
        private readonly IUserDAO _userDAO;

        public UserAuthenticationService(IUserDAO userDAO)
        {
            _userDAO = userDAO;
        }

        public async Task<bool> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userDAO.GetUserAsync(username, password);

            // password hash?
            return user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }
    }
}
