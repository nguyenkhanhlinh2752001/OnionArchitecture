using System.ComponentModel.DataAnnotations;

namespace Persistence.DTOs
{
    public class AddRoleDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; }
    }
}