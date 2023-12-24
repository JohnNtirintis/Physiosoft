using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Physiosoft.DTO.User
{
    public class UserSignupDTO
    {
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Username should be between 2-50 characters.")]
        public string? Username { get; set; }
        [StringLength(100, ErrorMessage = "Email should not be more than 100 characters")]
        [EmailAddress(ErrorMessage = "Invalid Email address")]
        public string? Email { get; set; }
        [StringLength(32, ErrorMessage = "Password should not be more than 100 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$", ErrorMessage = "Password must contain" +
            " at least 1 lower case and 1 upper case letters, 1 numeric character and 1 special character.")]
        public string? Password { get; set; }
        [Column("is_admin")]
        public bool IsAdmin { get; set; }
    }
}
