using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Physiosoft.Data
{
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("patient_id")]
        public int PatientId { get; set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name should be between 2-50 characters.")]

        [Column("firstname")]
        public string? Firstname { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name should be between 2-50 characters.")]
        [Column("lastname")]
        public string Lastname { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 10, ErrorMessage = "Telephone number should at max 12 chars/digits.")]
        [Column("telephone")]
        public string Telephone { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Telephone number should at max 12 chars/digits.")]
        [Column("address")]
        public string Address { get; set; }

        [StringLength(9, MinimumLength = 9, ErrorMessage = "Vat should be exactly 9 digits")]
        [Column("vat")]
        public string? Vat { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "SSN should be exactly 11 digits")]
        [Column("ssn")]
        public string Ssn { get; set; }

        [StringLength(11, MinimumLength = 11, ErrorMessage = "RegNum should be exactly 11 digits")]
        [Column("reg_num")]
        public string? RegNum { get; set; }

        [StringLength(500, MinimumLength = 0, ErrorMessage = "Notes should be 0-500 chars")]
        [Column("notes")]
        public string? Notes { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Column("email")]
        public string? Email { get; set; }

        [Column("has_reviewed")]
        public bool HasReviewed { get; set; }

        // TODO: CONFIRM THAT MAX LENGTH IS CORRECT VALUE
        [Required]
        [StringLength(500, MinimumLength = 0, ErrorMessage = "Notes should be 0-500 chars")]
        [Column("patient_issue")]
        public string? PatientIssue { get; set; }

        public ICollection<Appointment> Appointments { get; set; }

        public Patient()
        {
            Appointments = new HashSet<Appointment>();
        }
    }
}