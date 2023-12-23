using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Physiosoft.Data
{
    public class Appointment : IValidatableObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("appointment_id")]
        public int AppointmentID { get; set; }
        [Required]
        [Column("patient_id")]
        [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be greater than 0")]
        public int PatientID { get; set; }
        [Column("physio_id")]
        [Range(1, int.MaxValue, ErrorMessage = "Physio ID must be greater than 0")]
        public int? PhysioID { get; set; }
        [Required]
        [Column("appointment_date")]
        public DateTime AppointmentDate { get; set; }
        [Required]
        [Range(1, 120, ErrorMessage = "Duration must be between 1 and 120 minutes")]
        [Column("duration_minutes")]
        public int DurationMinutes { get; set; }
        [Required]
        [Column("appointment_status")]
        [MaxLength(500, ErrorMessage = "Maximum of 50 characters allowed")]
        public string? AppointmentStatus { get; set; }
        [Column("notes")]
        [MaxLength(500, ErrorMessage = "Maximum of 500 characters allowed")]
        public string? Notes { get; set; }
        [Required]
        [Column("patient_issue")]
        [MaxLength(500, ErrorMessage = "Maximum of 500 characters allowed")]
        public string PatientIssuse { get; set; }
        [Required]
        [Column("has_scans")]
        [DefaultValue(false)]
        public bool HasScans { get; set; } = false;
        public virtual Patient Patient { get; set; }
        public virtual Physio Physio { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var now = DateTime.Now;
            var oneYearFromNow = now.AddYears(1);

            if (AppointmentDate < now || AppointmentDate > oneYearFromNow)
            {
                yield return new ValidationResult($"The appointment date must be between now and {oneYearFromNow:yyyy-MM-dd}.", new[] { nameof(AppointmentDate) });
            }

            // Validate time (assuming AppointmentDate includes time)
            if (AppointmentDate.Hour < 9 || AppointmentDate.Hour > 17) // 5 PM is 17 in 24-hour time
            {
                yield return new ValidationResult("The appointment time must be between 9 AM and 5 PM.", new[] { nameof(AppointmentDate) });
            }

            if (AppointmentDate.DayOfWeek == DayOfWeek.Saturday || AppointmentDate.DayOfWeek == DayOfWeek.Sunday)
            {
                yield return new ValidationResult("The appointment date must be on a workday (Monday to Friday).", new[] { nameof(AppointmentDate) });
            }
        }
    }
}
