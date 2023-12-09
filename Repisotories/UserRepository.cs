using Microsoft.EntityFrameworkCore;
using Physiosoft.Data;
using Physiosoft.DTO.User;
using Physiosoft.Security;

namespace Physiosoft.Repisotories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(PhysiosoftDbContext context) : base(context)
        {

        }

        public async Task<bool> SignupUserAsync(UserSignupDTO request)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Username == request.Username);

            if (existingUser != null) return false;

            var user = new User()
            {
                Username = request.Username,
                Email = request.Email,
                Password = EncryptionUtil.Encrypt(request.Password!),
            };

            await _context.Users.AddAsync(user);
            return true;

        }

        public async Task<User?> GetUserAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username); // || x.Email == username
            if (user is null) return null;

            if (!EncryptionUtil.IsValidPassword(password, user.Password!)) return null;

            return user;
            ;
        }

        public async Task<User?> UpdateUserAsync(int userId, UserPatchDTO request)
        {
            var user = await _context.Users.Where(x => x.UserId == userId).FirstAsync();

            if (user is null) return null;

            user.Email = request.Email;
            user.Password = request.Password;

            _context.Users.Update(user);
            return user;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.Where(x => x.Username == username).FirstOrDefaultAsync();
        }
    }
}
