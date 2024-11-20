using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Column("username")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [Column("password")]
        public string Password { get; set; }

        [Column("expired_password")]
        public bool ExpiredPassword { get; set; } = false;

    }
}
