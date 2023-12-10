using Physiosoft.Data;
using Physiosoft.DTO.User;

namespace Physiosoft.Repisotories
{
    public interface IUserRepository
    {
        Task<bool> SignupUserAsync(UserSignupDTO request);
        Task<User?> GetUserAsync(string username);
        Task<User?> UpdateUserAsync(int userId, UserPatchDTO request);
        Task<User?> GetByUsernameAsync(string username);
    }
}
