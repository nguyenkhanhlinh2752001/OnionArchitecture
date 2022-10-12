using System.ComponentModel.DataAnnotations;

namespace Persistence.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string Phone { get; set; }
        public string Address { get; set; }
    }
}