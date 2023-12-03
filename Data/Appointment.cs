namespace Physiosoft.Data
{
    public class Appointment
    {
        public int AppointmentID { get; set; }
        public int PatientID { get; set; }
        public int PhysioID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int DurationMinutes { get; set; }
        public string? AppointmentStatus { get; set; }
        public bool AtWorkplace { get; set; }
        public string? PatientLocation { get; set; }
        public string? Notes { get; set; }
        public string PatientIssuse { get; set; }
        public bool HasScans { get; set; }
    }
}
