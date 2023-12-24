using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security;

namespace Physiosoft.Data
{
    public class User
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("username")]
        public string Username { get; set; }
        [Column("password")]
        public string Password { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [DefaultValue(false)]
        [Column("is_admin")]
        public bool IsAdmin { get; set; }
    }
}
