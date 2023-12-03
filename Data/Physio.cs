using System.ComponentModel.DataAnnotations;

namespace Physiosoft.Data
{
    public class Physio
    {
        [Key]
        public int PhysioId { get; set; }
        public string Firstname { get; set; } = null!;
        public string? Lastname { get; set; }
        public string Telephone { get; set; } = null!;
    }
}
