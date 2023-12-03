using AutoMapper;
using Physiosoft.Data;
using Physiosoft.DTO.Appointment;
using Physiosoft.DTO.Patient;
using Physiosoft.DTO.Physio;
using Physiosoft.DTO.User;

namespace Physiosoft.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<PhysioUtilDTO, Physio>().ReverseMap();
            CreateMap<PatientUtilDTO, Patient>().ReverseMap();
            CreateMap<AppointmentUtilDTO, Appointment>().ReverseMap();
            CreateMap<UserUtilDTO, User>().ReverseMap();

        }
    }
}
