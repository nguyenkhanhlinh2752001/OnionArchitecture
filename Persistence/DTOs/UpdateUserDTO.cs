using System.ComponentModel.DataAnnotations;

namespace Persistence.DTOs
{
    public class UpdateUserDTO
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Username { get; set; }

        public string Phone { get; set; }
        public string Address { get; set; }
    }
}