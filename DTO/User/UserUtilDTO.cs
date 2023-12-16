using System.ComponentModel.DataAnnotations;

namespace Physiosoft.DTO.User
{
    public class UserUtilDTO : BaseDTO
    { 
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
