using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Physiosoft.Data
{
    public class Appointment
    {
        [Key]
        [Column("appointment_id")]
        public int AppointmentID { get; set; }
        [Column("patient_id")]
        public int PatientID { get; set; }
        [Column("physio_id")]
        public int PhysioID { get; set; }
        [Column("appointment_date")]
        public DateTime AppointmentDate { get; set; }
        [Column("duration_minutes")]
        public int DurationMinutes { get; set; }
        [Column("appointment_status")]
        public string? AppointmentStatus { get; set; }
        [Column("at_workplace")]
        public bool AtWorkplace { get; set; }
        // If AtWorkplace is false, then we can get/pull
        // The patients location via the FK 
        [NotMapped]
        public string? PatientLocation { get; set; }
        [Column("notes")]
        public string? Notes { get; set; }
        [Column("patient_issue")]
        public string PatientIssuse { get; set; }
        [Column("has_scans")]
        public bool HasScans { get; set; }
    }
}
