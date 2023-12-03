using Physiosoft.Data;

namespace Physiosoft.DAO
{
    public interface IAppointmentDAO
    {
        void Insert(Appointment appointment);
        Appointment? Update(int id, Appointment appointment);
        bool Delete(int id);
        Appointment? GetById(int id);
        List<Appointment> GetAll();
    }
}
