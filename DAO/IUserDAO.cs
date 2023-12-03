using Physiosoft.Data;

namespace Physiosoft.DAO
{
    public interface IUserDAO
    {
        void Insert(User user);
        User? Update(int id, User user);
        bool Delete(int id);
        User? GetById(int id);
        List<User> GetAll();
    }
}
