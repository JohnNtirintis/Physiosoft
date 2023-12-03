using AutoMapper;
using Microsoft.Data.SqlClient;
using Physiosoft.Configuration;
using Physiosoft.Data;
using System.Drawing.Text;

namespace Physiosoft.DAO
{
    public class PhysioDaoImpl : IPhysioDAO
    {
        private readonly PhysiosoftDbContext _dbContext;
        private readonly IMapper _mapper;

        public PhysioDaoImpl(PhysiosoftDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public List<Physio> GetAll()
        {
            var physios = _dbContext.Physios.ToList();
            return _mapper.Map<List<Physio>>(physios);
        }

        public Physio? GetById(int id)
        {
            var physioToGet = _dbContext.Physios.Find(id);
            return _mapper.Map<Physio>(physioToGet);
        }

        public void Insert(Physio physio)
        {
            var physioToInsert = _mapper.Map<Physio>(physio);
            _dbContext.Physios.Add(physioToInsert);
            _dbContext.SaveChanges();
        }

        public Physio? Update(int id, Physio physio)
        {

            var physioToUpdate = _dbContext.Physios.Find(id);

            if (physioToUpdate != null)
            {
                _mapper.Map(physio, physioToUpdate);

                _dbContext.SaveChanges();
            }

            return _mapper.Map<Physio>(physioToUpdate);
        }

        public bool Delete(int id)
        {
            var physioToDelete = _dbContext.Physios.Find(id);

            if(physioToDelete != null)
            {
                _dbContext.Physios.Remove(physioToDelete);  
                _dbContext.SaveChanges();
                return true;
            }

            return false;
        }  
    }
}
