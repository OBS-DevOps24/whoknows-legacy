using System.ComponentModel.DataAnnotations;

namespace API.Models.Dtos
{
    public class ChangePasswordDTO
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

    }
}
