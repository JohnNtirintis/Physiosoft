namespace Physiosoft.DTO.Appointment
{
    public class AppointmentUtilDTO : BaseDTO
    {
        // TODO ADD FLUENT API
        public int PatientID { get; set; }
        public int PhysioID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int DurationMinutes { get; set; }
        public string? status { get; set; }
        public bool AtWorkplace { get; set; }
        public string? Notes { get; set; }
        public string PatientIssuse { get; set; }
        public bool HasScans { get; set; }
    }
}
