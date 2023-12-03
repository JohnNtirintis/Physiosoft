using AutoMapper;
using Physiosoft.Data;

namespace Physiosoft.DAO
{
    public class AppointmentDaoImpl : IAppointmentDAO
    {
        private readonly PhysiosoftDbContext _dbcontext;
        private readonly IMapper _mapper;

        public AppointmentDaoImpl(PhysiosoftDbContext context, IMapper mapper)
        {
            _dbcontext = context;
            _mapper = mapper;
        }

        public bool Delete(int id)
        {
            var appointmentToDelete = _dbcontext.Appointments.Find(id);

            if(appointmentToDelete != null)
            {
                _dbcontext.Appointments.Remove(appointmentToDelete);

                _dbcontext.SaveChanges();
                return true;
            }

            return false;
        }

        public List<Appointment> GetAll()
        {
            var appointments = _dbcontext.Appointments.ToList();
            return _mapper.Map<List<Appointment>>(appointments);    
        }

        public Appointment? GetById(int id)
        {
            var appointmentToGet = _dbcontext.Appointments.Find(id);
            return _mapper.Map<Appointment>(appointmentToGet);
        }

        public void Insert(Appointment appointment)
        {
            var appointmentToInsert = _mapper.Map<Appointment>(appointment);

            if(appointmentToInsert != null )
            {
                _dbcontext.Appointments.Add(appointmentToInsert);
                _dbcontext.SaveChanges();
            }
            
            // Throw exception error if its null
        }

        public Appointment? Update(int id, Appointment appointment)
        {
            var appointmentToUpdate = _dbcontext.Appointments.Find(id);

            if(appointmentToUpdate != null )
            {
                _mapper.Map(appointmentToUpdate, appointment);

                _dbcontext.SaveChanges();
            }

            return _mapper.Map<Appointment>(appointmentToUpdate);
        }
    }
}
