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
        [Column("firstname")]
        public string Firstname { get; set; } = null!;
        [Column("lastname")]
        public string? Lastname { get; set; }
        [Column("telephone")]
        public string Telephone { get; set; } = null!;
        public ICollection<Appointment> Appointments { get; set; }
    }
}
