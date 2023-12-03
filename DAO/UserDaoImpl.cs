using AutoMapper;
using Physiosoft.Data;

namespace Physiosoft.DAO
{
    public class UserDaoImpl : IUserDAO
    {
        private readonly PhysiosoftDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserDaoImpl(PhysiosoftDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public bool Delete(int id)
        {
            var userToDelete = _dbContext.Users.Find(id);

            if (userToDelete != null)
            {
                _dbContext.Users.Remove(userToDelete);

                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public List<User> GetAll()
        {
            var users = _dbContext.Users.ToList();
            return _mapper.Map<List<User>>(users);
        }

        public User? GetById(int id)
        {
            var userToGet = _dbContext.Users.Find(id);
            return _mapper.Map<User>(userToGet);
        }

        public void Insert(User user)
        {
            var userToInsert = _mapper.Map<User>(user);

            if (userToInsert != null)
            {
                _dbContext.Users.Add(userToInsert);
                _dbContext.SaveChanges();
            }

            // Throw exception error if its null
        }

        public User? Update(int id, User user)
        {
            var userToUpdate = _dbContext.Users.Find(id);

            if (userToUpdate != null)
            {
                _mapper.Map(user, userToUpdate);

                _dbContext.SaveChanges();
            }

            return _mapper.Map<User>(userToUpdate);
        }
    }
}
