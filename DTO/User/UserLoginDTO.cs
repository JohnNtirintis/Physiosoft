using System.ComponentModel.DataAnnotations;

namespace Physiosoft.DTO.User
{
    public class UserLoginDTO
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public bool IsAdmin { get; set; }
        public bool KeepLoggedIn { get; set; }
    }
}
