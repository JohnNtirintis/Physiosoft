using AutoMapper;
using Physiosoft.Data;

namespace Physiosoft.DAO
{
    public class PatientDaoImpl : IPatientDAO
    {

        private readonly PhysiosoftDbContext _dbContext;
        private readonly IMapper _mapper;

        public PatientDaoImpl(PhysiosoftDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public bool Delete(int id)
        {
            var patientToDelete = _dbContext.Patients.Find(id);

            if (patientToDelete != null)
            {
                _dbContext.Patients.Remove(patientToDelete);

                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Patient> GetAll()
        {
            var patients = _dbContext.Patients.ToList();
            return _mapper.Map<List<Patient>>(patients);
        }

        public Patient? GetById(int id)
        {
            var patientToGet = _dbContext.Patients.Find(id);
            return _mapper.Map<Patient>(patientToGet);
        }

        public void Insert(Patient patient)
        {
            var patientToInsert = _mapper.Map<Patient>(patient);

            if(patientToInsert != null)
            {
                _dbContext.Patients.Add(patientToInsert);
                _dbContext.SaveChanges();
            }

            // Throw exception error if its null
        }

        public Patient? Update(int id, Patient patient)
        {
            var patientToUpdate = _dbContext.Physios.Find(id);

            if (patientToUpdate != null)
            {
                _mapper.Map(patient, patientToUpdate);

                _dbContext.SaveChanges();
            }

            return _mapper.Map<Patient>(patientToUpdate);
        }
    }
}
