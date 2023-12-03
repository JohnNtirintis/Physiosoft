using Physiosoft.Data;

namespace Physiosoft.DAO
{
    public interface IPhysioDAO
    {
        void Insert(Physio physio);
        Physio? Update(int id, Physio physio);
        bool Delete(int id);
        Physio? GetById(int id);
        List<Physio> GetAll();
    }
}
