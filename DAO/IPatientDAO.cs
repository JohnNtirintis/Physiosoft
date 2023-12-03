using Physiosoft.Data;

namespace Physiosoft.DAO
{
    public interface IPatientDAO
    {
        void Insert(Patient patient);
        Patient? Update(int id, Patient patient);
        bool Delete(int id);
        Patient? GetById(int id);
        List<Patient> GetAll();
    }
}
