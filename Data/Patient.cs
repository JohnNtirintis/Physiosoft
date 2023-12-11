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
        [Column("firstname")]
        public string? Firstname { get; set; }
        [Column("lastname")]
        public string Lastname { get; set; }
        [Column("telephone")]
        public string Telephone { get; set; }
        [Column("address")]
        public string address { get; set; }
        [Column("vat")]
        public string? Vat { get; set; }
        [Column("ssn")]
        public string Ssn { get; set; }
        [Column("reg_num")]
        public string? RegNum { get; set; }
        [Column("notes")]
        public string? Notes { get; set; }
        [Column("email")]
        public string? Email { get; set; }
        [Column("has_reviewed")]
        public bool HasReviewd { get; set; } = false;
        [Column("patient_issue")]
        public string? PatientIssue {  get; set; }
    }
}
