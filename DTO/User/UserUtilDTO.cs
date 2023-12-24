using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Physiosoft.DTO.User
{
    public class UserUtilDTO : BaseDTO
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("username")]
        public string Username { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [DefaultValue(false)]
        [Column("is_admin")]
        public bool IsAdmin { get; set; }
    }
}
