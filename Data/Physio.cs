using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Physiosoft.Data
{
    public class Physio
    {
        public Physio()
        {
            Appointments = new HashSet<Appointment>();
        }


        [Key]
        [Column("physio_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PhysioId { get; set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name should be between 2-50 characters.")]
        [Column("firstname")]
        public string Firstname { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name should be between 2-50 characters.")]
        [Column("lastname")]
        public string? Lastname { get; set; }
        [Required]
        [Column("telephone")]
        [StringLength(13, MinimumLength = 10, ErrorMessage = "Telephone number should be between 10-13 characters.")]
        public string Telephone { get; set; } = null!;

        public ICollection<Appointment> Appointments { get; set; }
    }
}
