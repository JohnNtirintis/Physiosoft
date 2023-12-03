using System.ComponentModel.DataAnnotations;

namespace Physiosoft.Data
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }
        public string? Firstname { get; set; }
        public string Lastname { get; set; }
        public string Telephone { get; set; }
        public string address { get; set; }
        public string? Vat { get; set; }
        public string Ssn { get; set; }
        public string? RegNum { get; set; }
        public string? Notes { get; set; }
        public string? Email { get; set; }
        public bool HasReviewd { get; set; } = false;
        public string? PatientIssue {  get; set; }
    }
}
