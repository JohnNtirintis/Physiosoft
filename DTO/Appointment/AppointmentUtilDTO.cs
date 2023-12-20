using System.ComponentModel.DataAnnotations;

namespace Physiosoft.DTO.Appointment
{
    public class AppointmentUtilDTO : BaseDTO
    {
        // TODO ADD FLUENT API
        [Required]
        public int PatientID { get; set; }
        public int PhysioID { get; set; }
        [Required]
        public DateTime AppointmentDate { get; set; }
        [Required]
        public int DurationMinutes { get; set; }
        [Required]
        [MaxLength(500, ErrorMessage = "Maximum of 50 characters allowed")]
        public string? status { get; set; }
        [Required]
        public bool AtWorkplace { get; set; }
        [MaxLength(500, ErrorMessage = "Maximum of 500 characters allowed")]
        public string? Notes { get; set; }
        [Required]
        [MaxLength(500, ErrorMessage = "Maximum of 500 characters allowed")]
        public string PatientIssuse { get; set; }
        [Required]
        public bool HasScans { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var now = DateTime.Now;
            var oneYearFromNow = now.AddYears(1);

            if (AppointmentDate < now || AppointmentDate > oneYearFromNow)
            {
                yield return new ValidationResult($"The appointment date must be between now and {oneYearFromNow:yyyy-MM-dd}.", new[] { nameof(AppointmentDate) });
            }

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
