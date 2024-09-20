using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string Password2 { get; set; }
    }
}
