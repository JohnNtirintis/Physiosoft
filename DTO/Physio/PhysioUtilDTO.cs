using System.ComponentModel.DataAnnotations;

namespace Physiosoft.DTO.Physio
{
    public class PhysioUtilDTO : BaseDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name should be between 2-50 characters.")]
        public string Firstname { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name should be between 2-50 characters.")]
        public string Lastname { get; set; }
        [Required]
        [StringLength(13, MinimumLength = 10, ErrorMessage = "Telephone number should be between 10-13 characters.")]
        public string Telephone { get; set; }
    }
}
