using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Physiosoft.Data
{

    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("patient_id")]
        public int PatientId { get; set; }

        [Column("firstname")]
        public string? Firstname { get; set; }

        [Required]
        [Column("lastname")]
        public string Lastname { get; set; }

        [Required]
        [Column("telephone")]
        public string Telephone { get; set; }

        [Required]
        [Column("address")]
        public string Address { get; set; }

        [Column("vat")]
        public string? Vat { get; set; }

        [Required]
        [Column("ssn")]
        public string Ssn { get; set; }

        [Column("reg_num")]
        public string? RegNum { get; set; }

        [Column("notes")]
        public string? Notes { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("has_reviewed")]
        public bool HasReviewed { get; set; }

        [Column("patient_issue")]
        public string? PatientIssue { get; set; }

        public ICollection<Appointment> Appointments { get; set; }

        public Patient()
        {
            Appointments = new HashSet<Appointment>();
        }
    }
}
    /*public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("patient_id")]
        public int PatientId { get; set; }
        [AllowNull]
        [Column("firstname")]
        public string? Firstname { get; set; }
        [Required]
        [Column("lastname")]
        public string Lastname { get; set; }
        [Required]
        [Column("telephone")]
        public string Telephone { get; set; }
        [Required]
        [Column("address")]
        public string address { get; set; }
        [AllowNull]
        [Column("vat")]
        public string? Vat { get; set; }
        [Required]
        [Column("ssn")]
        public string Ssn { get; set; }
        [AllowNull]
        [Column("reg_num")]
        public string? RegNum { get; set; }
        [AllowNull]
        [Column("notes")]
        public string? Notes { get; set; }
        [AllowNull]
        [Column("email")]
        public string? Email { get; set; }
        [AllowNull]
        [Column("has_reviewed")]
        [DefaultValue(false)]
        public bool? HasReviewd { get; set; } = false;
        [Required]
        [Column("patient_issue")]
        public string? PatientIssue {  get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}*/
