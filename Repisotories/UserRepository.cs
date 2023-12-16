using Microsoft.EntityFrameworkCore;
using Physiosoft.Data;
using Physiosoft.DTO.User;
using Physiosoft.Security;

namespace Physiosoft.Repisotories
{
    public class UserRepository(PhysiosoftDbContext context) : BaseRepository<User>(context), IUserRepository
    {
        public async Task<bool> SignupUserAsync(UserSignupDTO request)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Username == request.Username);

            if (existingUser != null) return false;

            var user = new User()
            {
                Username = request.Username!,
                Email = request.Email!,
                Password = EncryptionUtil.Encrypt(request.Password!),
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<User?> GetUserAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
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
