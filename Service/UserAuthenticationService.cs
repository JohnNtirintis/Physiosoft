using Physiosoft.DAO;
using Physiosoft.Security;

namespace Physiosoft.Service
{
    public class UserAuthenticationService
    {
        private readonly IUserDAO _userDAO;

        public UserAuthenticationService(IUserDAO userDAO)
        {
            _userDAO = userDAO;
        }

        /*public async Task<bool> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userDAO.GetUserAsync(username, password);
            if (user == null) return false;

            // password hash?
            return user != null && BCrypt.Net.BCrypt.Verify(password, user.Password);
        }*/

        public async Task<bool> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userDAO.GetUserAsync(username);
            if (user == null) return false;

            return EncryptionUtil.IsValidPassword(password, user.Password);
        }
    }
}
