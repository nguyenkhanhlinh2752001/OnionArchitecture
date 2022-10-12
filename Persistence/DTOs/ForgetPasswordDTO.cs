using System.ComponentModel.DataAnnotations;

namespace Persistence.DTOs
{
    public class ForgetPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}