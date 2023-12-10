using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Physiosoft.Data
{
    public class Physio
    {
        [Key]
        [Column("physio_id")]
        public int PhysioId { get; set; }
        [Column("firstname")]
        public string Firstname { get; set; } = null!;
        [Column("lastname")]
        public string? Lastname { get; set; }
        [Column("telephone")]
        public string Telephone { get; set; } = null!;
    }
}
